/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.IO;
using System.Net;
using System.Collections;

using Cursit.Text;
using Cursit.Utils;
#endregion

namespace Gordago.WebUpdate {

	#region public class DownloadFile
	public class DownloadFile {
		private string _urlfile;
		private string _destfile;
		private string _shortfilename;

		private int _size;

		public DownloadFile() {
		}

		public string UrlFile{
			get{return this._urlfile;}
			set{this._urlfile = value;}
		}
		public string DestFile{
			get{return this._destfile;}
			set{this._destfile = value;}
		}
		public int Size{
			get{return this._size;}
			set{this._size = value;}
		}
		public string ShortFileName{
			get{return this._shortfilename;}
			set{this._shortfilename = value;}
		}
	}
	#endregion

	/// <summary>
	/// Закачка файла
	/// </summary>
	public class DownloadFileEngine {

		#region private property
		public event EventHandler DownPartEvent;
		private int _currentPos;
		private int _lenght;
		private string _filedown;
		#endregion

		private ProxySetting _proxy;

		public DownloadFileEngine() {
		}

		#region public ProxySetting Proxy
		public ProxySetting Proxy{
			get{return this._proxy;}
			set{this._proxy = value;}
		}
		#endregion

		#region public string FileDown
		public string FileDown{
			get{return this._filedown;}
		}
		#endregion
		
		#region public int Position
		public int Position{
			get{return this._currentPos;}
		}
		#endregion

		#region public int Lenght
		public int Lenght{
			get{return this._lenght;}
		}
		#endregion

		#region public string DownloadPage(string urlpage)
		public string DownloadPage(string urlpage){
			try{
				System.Uri uri = new Uri(urlpage);
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlpage);
				request.Headers.Add("Cache-Control", "no-cache");
				request.Credentials = CredentialCache.DefaultCredentials;
	
				if (this.Proxy != null){
          WebProxy proxyObject = this.Proxy.GetWebProxy();
					if (proxyObject != null)
						request.Proxy = proxyObject;
				}
				
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream strm = response.GetResponseStream();

				string strpage = "";

				int numReaded = 1;
				while (numReaded > 0) {
					byte[] buf = new byte [4*1024];
					numReaded = strm.Read(buf, 0, 4*1024);

					for (int i=0;i<numReaded;i++){
						strpage += new string(Convert.ToChar(buf[i]), 1);
					}
				}
				response.Close();
				return strpage;
			}catch(Exception){}
			return string.Empty;
		}
		#endregion

		#region public bool DownloadFile(string urlFileName, string distFileName, string downloadDir)
		public bool DownloadFile(string urlFileName, string distFileName, string downloadDir){
			try{
				this._filedown = urlFileName;
				string urlsize = urlFileName.Replace("?act=down", "?act=size");
				string sizePage = DownloadPage(urlsize);
				int size = 0;
				try{
					size = Convert.ToInt32(sizePage);
				}catch(Exception){
					return false;
				}
				this._lenght = size;

				System.Uri uri = new Uri(urlFileName);
				System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlFileName);
				request.Headers.Add("Cache-Control", "no-cache");
				request.Credentials = CredentialCache.DefaultCredentials;
				
				if (this.Proxy != null){
					WebProxy proxyObject = this.Proxy.GetWebProxy();
					if (proxyObject != null)
						request.Proxy = proxyObject;
				}

				if (!Directory.Exists(downloadDir))
					Directory.CreateDirectory(downloadDir);

				if (File.Exists(distFileName)){
					File.Delete(distFileName);
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream strm = response.GetResponseStream();

				FileStream fst = new FileStream(distFileName, FileMode.CreateNew);
				StreamWriter sw = new StreamWriter(fst);

				EventArgs eargs = new EventArgs();
				int numReaded = 1, posToWrite = 0;
				while (numReaded > 0) {

					byte[] buf = new byte [4*1024];
					numReaded = strm.Read(buf, 0, 4*1024);
					fst.Write(buf, 0, numReaded);
					posToWrite += numReaded;
					this._currentPos = posToWrite;
					if (this.DownPartEvent != null)
						this.DownPartEvent(this, eargs);
          if(GordagoMain.IsCloseProgram)
            break;
				}
				int sz = (int)fst.Position;
				fst.Flush();
				fst.Close();
				response.Close();
				if (sz != size){
					return false;
				}
				return true;
			}catch(Exception){}
			return false;
		}
		#endregion
	}
}
