/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using Cursit.Applic.AConfig;
using Cursit.UnRarLib;

using Gordago.Docs;
using Gordago.WebUpdate;

#endregion

namespace Gordago.Stock.Loader {
	public delegate void DAHHandler(DAHCommnad cmd);
	public delegate void DAHDownPartHandler(string message, int total, int current);

	public class DownloaderArhiveHistory {

		public event DAHHandler StartingCommnadEvent;
		public event DAHHandler StoppingCommnadEvent;
		public event DAHDownPartHandler DownloadedPartEvent;

		private string _script, _dwndir, _tmpdwndir;
		private DocUpdates _du;
		private DAHError _lasterror;
		private DownloadFile[] _dfs;
		private DownloadFile _dfcurrent;
		private string _currentfile;
		private bool _flagabort;
		private DownloadFileEngine _dfe;
		private Unrar _unrar;
    private string _symbolName;

    #region public DownloaderArhiveHistory(string symbolName)
    public DownloaderArhiveHistory(string symbolName) {
      _symbolName = symbolName;
			_dfe = new DownloadFileEngine();
			_dfe.DownPartEvent += new EventHandler(this.DownloadedFilePage);
			_dfe.Proxy = GordagoMain.MainForm.LoadProxySetting();

			_dwndir = System.Windows.Forms.Application.StartupPath + "\\download";
			_tmpdwndir = System.Windows.Forms.Application.StartupPath + "\\download\\temp";
			Cursit.Utils.FileEngine.CheckDir(_tmpdwndir);
			_unrar = new Unrar();
			_unrar.ExtractionProgress += new ExtractionProgressHandler(unrar_ExtractionProgress);


#if DEBUG
      string host = "localhost";
#else 
      string host = "gordago.com";
//			this._script = "http://gordago.com/download.php?act=down&whois=file&type=2.3&fn=";
#endif
      this._script = "http://"+host+"/download.php?act=down&whois=file&type=2.3&fn=";
    }
		#endregion

		#region public DAHError LastError
		public DAHError LastError{
			get{return this._lasterror;}
			set{this._lasterror = value;}
		}
		#endregion

		#region public DocUpdates DocUpdates
		public DocUpdates DocUpdates{
			get{return this._du;}
		}
		#endregion

		#region protected virtual void OnStartingCommnad(DAHCommnad cmd)
		protected virtual void OnStartingCommnad(DAHCommnad cmd){
			if (this.StartingCommnadEvent != null)
				this.StartingCommnadEvent(cmd);
		}
		#endregion

		#region protected virtual void OnStoppingCommand(DAHCommnad cmd)
		protected virtual void OnStoppingCommand(DAHCommnad cmd){
			if (this.StoppingCommnadEvent != null)
				this.StoppingCommnadEvent(cmd);
		}
		#endregion

		#region protected virtual void OnDownloadedPart(string message, int total, int current)
		protected virtual void OnDownloadedPart(string message, int total, int current){
			if (this.DownloadedPartEvent != null)
				this.DownloadedPartEvent(message, total, current);
		}
		#endregion

		#region public void GetInfoServer()
		public void GetInfoServer(){
			Thread th = new Thread(new ThreadStart(this.GetInfoServerMethod));
      th.IsBackground = true;
			th.Start();
		}
		#endregion

		#region private void GetInfoServerMethod()
		private void GetInfoServerMethod(){
			OnStartingCommnad(DAHCommnad.DownloadHistoryInfo);
			this._lasterror = DAHError.OK;
			string file = _dwndir + "\\descript.uht";
			string script = this._script + "descript.uht";

			try{
				DownloadFileEngine dfe = new DownloadFileEngine();
				dfe.Proxy = GordagoMain.MainForm.LoadProxySetting();
				bool dres = dfe.DownloadFile(script, file, _dwndir);
				if (!dres){
					this._lasterror = DAHError.ConnectError;
				}else{
					_du = new DocUpdates(file.Replace("\\descript.uht",""));
				}
			}catch{
				this._lasterror = DAHError.ConnectError;
			}

			OnStoppingCommand(DAHCommnad.DownloadHistoryInfo);
		}
		#endregion

		#region public void DownloadFiles(DownloadFile[] dfs)
		public void DownloadFiles(DownloadFile[] dfs){
			_dfs = dfs;

			OnStartingCommnad(DAHCommnad.DownloadFiles);
			Thread th = new Thread(new ThreadStart(this.DownloadFilesMethod));
      th.IsBackground = true;
			th.Start();
		}
		#endregion

		#region private void DownloadFilesMethod()
		private void DownloadFilesMethod(){
			_flagabort = false;
			this._lasterror = DAHError.OK;
			
			foreach (DownloadFile df in _dfs){
				if (_flagabort)
					break;
				string filename = df.ShortFileName;
				_dfcurrent = df;
				df.UrlFile = _script + filename;
				df.DestFile = _dwndir + "\\" + filename;

				this.OnDownloadedPart("Please wait...", 100, 0);
				this._currentfile = df.UrlFile;
				bool filedown = false;
				if (System.IO.File.Exists(df.DestFile)){
					long fsize = Cursit.Utils.FileEngine.GetSizeFile(df.DestFile);
					if (fsize == df.Size){
						filedown = true;
					}
				}
				if (!filedown){
					if (!_dfe.DownloadFile(df.UrlFile, df.DestFile, _tmpdwndir)){
						this._lasterror = DAHError.ConnectError;
						break;
					}
				}
			}
			OnStoppingCommand(DAHCommnad.DownloadFiles);
			if (this._lasterror == DAHError.OK)
				this.StartExtract(_dfs);
		}
		#endregion

		#region public void Abort()
		public void Abort(){
			this._flagabort = true;
		}
		#endregion

		#region private void DownloadedFilePage(object sender, EventArgs e)
		private void DownloadedFilePage(object sender, EventArgs e){
			DownloadFileEngine dfe = sender as DownloadFileEngine;
			OnDownloadedPart(_dfcurrent.ShortFileName, dfe.Lenght, dfe.Position);
		}
		#endregion

		#region public void StartExtract(DownloadFile[] dfs)
		public void StartExtract(DownloadFile[] dfs){
			OnStartingCommnad(DAHCommnad.UnPack);
			this._lasterror = DAHError.OK;
			foreach (DownloadFile df in dfs)
				this.ExtractFile(df.DestFile);
			OnStoppingCommand(DAHCommnad.UnPack);
			if (this.LastError == DAHError.OK)
				this.UpdateTickHistory();
		}
		#endregion

		#region private void ExtractFile(string filename)
		private void ExtractFile(string filename){
			try {
				string directory = Path.GetDirectoryName(filename);

				_unrar.DestinationPath=directory;
				_unrar.Open(filename, Unrar.OpenMode.Extract);

				while(_unrar.ReadHeader()) {
					if (_flagabort)
						break;
					
					OnDownloadedPart(_unrar.CurrentFile.FileName, 100, 0);
					_unrar.Extract();
				}
			} catch(Exception) {
				OnDownloadedPart("Erron in unrar", 100, 0);
				this._lasterror = DAHError.UnRarError;
			} finally {
				OnDownloadedPart("Ready", 0,0);
				if(_unrar!=null)
					_unrar.Close();
			}
		}
		#endregion

		#region private void unrar_ExtractionProgress(object sender, ExtractionProgressEventArgs e) 
		private void unrar_ExtractionProgress(object sender, ExtractionProgressEventArgs e) {
			OnDownloadedPart(e.FileName, 100, (int)e.PercentComplete);
		}
		#endregion

		#region private void UpdateTickHistory()
		private void UpdateTickHistory(){
			OnStartingCommnad(DAHCommnad.Update);
			this.UpdateTickHistoryMethod();
			OnStoppingCommand(DAHCommnad.Update);
		}
		#endregion

		#region private void UpdateTickHistoryMethod()
		private void UpdateTickHistoryMethod(){
			string[] files = Directory.GetFiles(_dwndir, "*.gtk");
			int current = 0;
			int total = files.Length;

			ISymbol[] dsymbols = new ISymbol[0];

			foreach (string file in files){
				current++;
				Cursit.Utils.FileEngine.DisplayFile df = new Cursit.Utils.FileEngine.DisplayFile(file);
				
				OnDownloadedPart("Update from " + df.ToString(), total, current);
				try{
					TickFileInfo tfi = new TickFileInfo(file);
					ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(tfi.SymbolName);
					if (symbol == null){
            symbol = GordagoMain.SymbolEngine.Add(tfi.SymbolName, tfi.DecimalDigits);
					}

					(symbol.Ticks as ITickManager).Update(tfi, false);

					bool find = false;
					foreach(ISymbol dsymbol in dsymbols){
						if (dsymbol == symbol){
							find = true;
							break;
						}
					}

					if (!find){
						ArrayList al = new ArrayList(dsymbols);
						al.Add(symbol);
						dsymbols = (ISymbol[])al.ToArray(typeof(ISymbol));
					}

					System.IO.File.Delete(file);
					string gmpfile = file.Replace(".gtk", ".gmp");
					if (System.IO.File.Exists(gmpfile))
						File.Delete(gmpfile);
				}catch{}
			}

			foreach (ISymbol dsymbol in dsymbols){
// 				TickManagerProcessHandler tmproc = new TickManagerProcessHandler(this.TickManager_DataCachingProccess);
        TickManagerEventHandler tmproc = new TickManagerEventHandler(this.TickManager_DataCachingChanged);
        ITickManager tmanager = dsymbol.Ticks as ITickManager;

				tmanager.DataCachingChanged += tmproc;
        tmanager.DataCachingMethod();
        tmanager.DataCachingChanged -= tmproc;
			}
      
			GordagoMain.MainForm.SymbolsPanel.UpdateSymbolTable();
      GordagoMain.MainForm.Tester.UpdateSymbolsList();

			OnDownloadedPart("Complete", 100, 100);
		}
		#endregion

    //#region private void TickManager_DataCachingProccess(int current, int total)
    //private void TickManager_DataCachingProccess(int current, int total){
    //  this.OnDownloadedPart("Data Caching", total, current);
    //}
    //#endregion

    #region private void TickManager_DataCachingChanged(object sender, TickManagerEventArgs tme)
    private void TickManager_DataCachingChanged(object sender, TickManagerEventArgs tme) {
      if (tme.Status == TickManagerStatus.DataCaching) {
        this.OnDownloadedPart("Data Caching", tme.Total, tme.Current);
      }
    }
    #endregion
  }

	#region public enum DAHCommnad
	public enum DAHCommnad{
		DownloadHistoryInfo,
		DownloadFiles,
		UnPack,
		Update
	}
	#endregion

	#region public enum DAHError
	public enum DAHError{
		OK,
		ConnectError,
		UnRarError
	}
	#endregion
}
