/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region Using
using System;
using System.IO;
using System.Net;
using System.Collections;

using Cursit.Text;
using Cursit.Utils;
#endregion
using System.Windows.Forms;

namespace Gordago.WebUpdate {

  #region internal class UEEventArgs : EventArgs
  internal class UEEventArgs : EventArgs {
    public string CurrentDownloadFile;
    
    public UEEventArgs(string curdf) {
      CurrentDownloadFile = curdf;
    }
  }
  #endregion

  public delegate void UpdateEngineStartingDownloadFileHandler(string filename);
  public delegate void UpdateEnginePartDownloadFileHandler(string filename, int current, int total);
  public delegate void UpdateEngineStoppingDownloadFileHandler(string filename, bool isok);
  public delegate void UpdateEngineCheckUpdateResult(UpdateEngineCheckUpdateType result);

  public class UpdateEngine {

		public static bool IsUpdateComplete = false;

    public event UpdateEnginePartDownloadFileHandler PartDownloadFileEvent;
    public event UpdateEngineStartingDownloadFileHandler StartingDownloadFileEvent;
    public event UpdateEngineStoppingDownloadFileHandler StoppingDownloadFileEVent;
    public event UpdateEngineCheckUpdateResult CheckUpdateResultEvent;

		private UpdateSynchronizeListFile _uslf;

		string _tempdir;
		string _curlstFileName;

		private string _sessionId;
    private string _downfilescript;
    private bool _abort;

		private ProxySetting _proxy;

#if DEMO
		public static bool IsDemo = true;
#else
    public static bool IsDemo = false;
#endif

#if DEBUG
    private const string SCRIPT_STRING = "http://localhost/download.php?act=";
#else
    private const string SCRIPT_STRING = "http://www.gordago.com/download.php?act=";
#endif

    internal const string TEMP_DIR_DOWNLOAD = "temp_upd";
    internal const string TEMP_DIR_OLD_FILES = "temp_old";
    internal const string UPDATE_LIST_FILENAME = "gordago.lst";

		#region public ProxySetting Proxy
		public ProxySetting Proxy{
			get{return this._proxy;}
			set{this._proxy = value;}
		}
		#endregion

    #region public UpdateEngine(ProxySetting proxy)
    public UpdateEngine(ProxySetting proxy) {
      _proxy = proxy;
      _curlstFileName = Application.StartupPath + "\\" + UPDATE_LIST_FILENAME;
    }
    #endregion

    #region public void Abort()
    public void Abort() {
      _abort = true;
    }
    #endregion

    #region public UpdateEngineCheckUpdateType CheckUpdate(string login, string password, bool isautocheck)
    public UpdateEngineCheckUpdateType CheckUpdate(string login, string password, bool isautocheck) {
      UpdateEngineCheckUpdateType uecut = this.CheckUpdateMethod(login, password, isautocheck);
      if(GordagoMain.IsCloseProgram) return UpdateEngineCheckUpdateType.None;
      if(this.CheckUpdateResultEvent != null)
        this.CheckUpdateResultEvent(uecut);
      return uecut;
    }
    #endregion

    #region private UpdateEngineCheckUpdateType CheckUpdateMethod(string login, string password, bool isautocheck)
    private UpdateEngineCheckUpdateType CheckUpdateMethod(string login, string password, bool isautocheck) {

      string script = SCRIPT_STRING +
        "autorize&login=" + login +
        "&pass=" + password +
        "&vers=" + Application.ProductVersion +
        "&lng=" + GordagoMain.Lang +
        "&cst=" + GordagoMain.MainForm.CountStartApp.ToString() +
        "&type=2.7";

      bool isfull = false;
#if !DEMO
      isfull = true;
#endif

      if(isautocheck) {
        script += "&autocheck=yes" + 
          "&full=" + (isfull ? "1" : "0");
      }

      DownloadFileEngine dfe = new DownloadFileEngine();
      dfe.Proxy = _proxy;

      string str = dfe.DownloadPage(script);

      if(str == string.Empty)  // ошибка получение страницы авторизации
        return UpdateEngineCheckUpdateType.ErrorOnServer;

      string[] ssa = str.Split(new char[] { '\n' });
      if(ssa.Length != 2)  // ошибка на стороне сервера
        return UpdateEngineCheckUpdateType.ErrorOnServer;

      string[] cfg = ssa[0].Split(new char[] { '|' });

      if(cfg.Length != 2)
        return UpdateEngineCheckUpdateType.ErrorOnServer;

      if(cfg[0] != "0")  // ошибка в авторизации
        return UpdateEngineCheckUpdateType.ErrorLoginOrPassword;

      Update.IsUpdateFromDemo = false;
      if(cfg[1] == "1" && IsDemo) { // это демо версия, и есть доступ на комерческий продукт
        Update.IsUpdateFromDemo = true;
      }

      if(cfg[1] == "0" && !IsDemo)  // это реал версия, и попытка обновиться до ДЕМО
        return UpdateEngineCheckUpdateType.UpdateRealToDemo;

      if(ssa[1].Length < 2)  // неудалось установить сессию
        return UpdateEngineCheckUpdateType.ErrorCreatedSession;

      this._sessionId = ssa[1];
      _downfilescript = SCRIPT_STRING + "down&" + this._sessionId + "&fn=";

      string urlFileName = _downfilescript + "gordago.lst";
      _tempdir = Application.StartupPath + "\\" + TEMP_DIR_DOWNLOAD;
      string distFileName = _tempdir + "\\gordago.lst";

      if(!this.DownloadFile(urlFileName, distFileName, _tempdir)) {
        FileEngine.DeleteDir(_tempdir);
        return UpdateEngineCheckUpdateType.ErrorDownloadListFile;
      }

      _uslf = new UpdateSynchronizeListFile(distFileName, _curlstFileName);

      if(_uslf.FilesDownload.Length > 0)
        return UpdateEngineCheckUpdateType.Yes;

      return UpdateEngineCheckUpdateType.None;
    }
    #endregion

    #region public UpdateSynchronizeListFile USListFile
    public UpdateSynchronizeListFile USListFile{
			get{return this._uslf;}
		}
		#endregion 

    #region public bool DownloadUpdateFiles()
    public bool DownloadUpdateFiles(){

      if(_uslf == null)
        throw new ArgumentNullException();

			foreach (string f in _uslf.FilesDownload){
				string downf = _downfilescript + f.Replace("\\", "/");
				string distf = _tempdir + "\\" + f;
        
				if (f.IndexOf("\\")>-1) // необходимо создать подкаталоги
					FileEngine.CheckDir(distf);

        if(!DownloadFile(downf, distf, _tempdir))
          return false;
			}
			return true;
    }
    #endregion

    #region public bool ApplyUpdate() - Применение обновления
    /// <summary>
		/// Применение обновления
		/// </summary>
		public bool ApplyUpdate(){
      if(_uslf == null) return false;


      ArrayList als = new ArrayList();
      foreach(string file in _uslf.FilesDownload) {
        als.Add(file);
      }
      als.Add("gordago.lst");
      string[] files = (string[])als.ToArray(typeof(string));
			
			// формирование списка старых файлов, для перемещния в папку для последующего обновления
			string olddir = System.Windows.Forms.Application.StartupPath + "\\" + UpdateEngine.TEMP_DIR_OLD_FILES;
			int nins = olddir.Split(new char[]{'\\'}).Length-1;
			foreach (string nfile in files){
        
        //string fold = nfile.Replace("\\" + UpdateEngine.TEMP_DIR_DOWNLOAD, "");
        string fold = Application.StartupPath + "\\" + nfile;
				string fileDist = "";

				if (File.Exists(fold)){
					string[] el = fold.Split(new char[]{'\\'});
					ArrayList al = new ArrayList(el);
          al.Insert(nins, UpdateEngine.TEMP_DIR_OLD_FILES);
					StringCreater crt = new StringCreater();
					crt.AppendStringRange((string[])al.ToArray(typeof(string)));
					fileDist = crt.GetString("\\");
					FileEngine.CheckDir(fileDist);
					File.Move(fold, fileDist);
				}
				// перемещение новых файлов
				FileEngine.CheckDir(fold);
        string newfile = Application.StartupPath + "\\" + UpdateEngine.TEMP_DIR_DOWNLOAD + "\\" + nfile;
				File.Move(newfile, fold);
			}

      FileEngine.DeleteDir(Application.StartupPath + "\\" + UpdateEngine.TEMP_DIR_DOWNLOAD);
			return true;
		}
		#endregion

    #region private string DownloadPage(string urlpage)
    private string DownloadPage(string urlpage) {
      System.Uri uri = new Uri(urlpage);
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlpage);
      request.Headers.Add("Cache-Control", "no-cache");
      request.Credentials = CredentialCache.DefaultCredentials;

      if(this.Proxy != null) {
        WebProxy proxyObject = this.Proxy.GetWebProxy();
        if(proxyObject != null)
          request.Proxy = proxyObject;
      }
      try {
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream strm = response.GetResponseStream();

        string strpage = "";

        int numReaded = 1;
        while(numReaded > 0) {
          byte[] buf = new byte[4 * 1024];
          numReaded = strm.Read(buf, 0, 4 * 1024);

          for(int i = 0; i < numReaded; i++) {
            strpage += new string(Convert.ToChar(buf[i]), 1);
          }
        }
        response.Close();
        return strpage;
      } catch {
        return "";
      }
    }
    #endregion

    #region private bool DownloadFile(string urlFileName, string distFileName, string downloadDir)
    private bool DownloadFile(string urlFileName, string distFileName, string downloadDir) {
      if(this.StartingDownloadFileEvent != null)
        this.StartingDownloadFileEvent(distFileName);

      bool iserror = this.DownloadFileMethod(urlFileName, distFileName, downloadDir);

      if(this.StoppingDownloadFileEVent != null)
        this.StoppingDownloadFileEVent(distFileName, iserror);
      
      return iserror;
    }
    #endregion

    #region private bool DownloadFileMethod(string urlFileName, string distFileName, string downloadDir)
    private bool DownloadFileMethod(string urlFileName, string distFileName, string downloadDir) {
      
      string urlsize = urlFileName.Replace("?act=down", "?act=size");
      string sizePage = DownloadPage(urlsize);
      int size = 0;
      try {
        size = Convert.ToInt32(sizePage);
      } catch(Exception) {
        return false;
      }

      Uri uri = new Uri(urlFileName);
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlFileName);
      request.Headers.Add("Cache-Control", "no-cache");
      request.Credentials = CredentialCache.DefaultCredentials;

      if(_proxy != null) {
        WebProxy proxyObject = _proxy.GetWebProxy();
        if(proxyObject != null)
          request.Proxy = proxyObject;
      }

      if(!Directory.Exists(downloadDir))
        Directory.CreateDirectory(downloadDir);

      if(File.Exists(distFileName)) 
        File.Delete(distFileName);

      try {
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream strm = response.GetResponseStream();

        FileStream fst = new FileStream(distFileName, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fst);

        EventArgs eargs = new EventArgs();
        int numReaded = 1, posToWrite = 0;
        while(numReaded > 0) {

          byte[] buf = new byte[4 * 1024];
          numReaded = strm.Read(buf, 0, 4 * 1024);
          fst.Write(buf, 0, numReaded);
          posToWrite += numReaded;

          if(this.PartDownloadFileEvent != null) {
            this.PartDownloadFileEvent(distFileName, posToWrite, size);
          }
          if(_abort || GordagoMain.IsCloseProgram)
            break;
        }
        int sz = (int)fst.Position;
        fst.Flush();
        fst.Close();
        response.Close();
        if(sz != size || _abort) {
          return false;
        }
        return true;
      } catch {
        return false;
      }
    }
    #endregion
  }
}
