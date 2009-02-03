/**
* @version $Id: WebReader.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Net;
  using System.IO;
  using System.Threading;

  public delegate void WebReaderEventHandler(object sender, WebReaderEventArgs e);
  public delegate void WebReaderProcessEventHandler(object sender, WebReaderProcessEventArgs e);

  #region public class WebReaderEventArgs : EventArgs
  public class WebReaderEventArgs : EventArgs {
    private readonly FileInfo _file;
    private readonly int _size;
    private readonly UpdateManagerError _error;
    private readonly string _errorMessage;

    public WebReaderEventArgs(FileInfo file, int size, UpdateManagerError error, string errorMessage) {
      _file = file;
      _size = size;
      _error = error;
      _errorMessage = errorMessage;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion 

    #region public int Size
    public int Size {
      get { return _size; }
    }
    #endregion

    #region public UpdateManagerError Error
    public UpdateManagerError Error {
      get { return _error; }
    }
    #endregion

    #region public string ErrorMessage
    public string ErrorMessage {
      get { return _errorMessage; }
    }
    #endregion
  }
  #endregion

  #region public class WebReaderProcessEventArgs : EventArgs
  public class WebReaderProcessEventArgs : EventArgs {

    private readonly FileInfo _file;
    private readonly int _total;
    private readonly int _current;

    public WebReaderProcessEventArgs(FileInfo file, int total, int current) {
      _file = file;
      _total = total;
      _current = current;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public int Total
    public int Total {
      get { return _total; }
    }
    #endregion

    #region public int Current
    public int Current {
      get { return _current; }
    }
    #endregion
  }
  #endregion
  
  #region class WebReaderException : Exception
  class WebReaderException : Exception {
    private UpdateManagerError _error;

    public WebReaderException(UpdateManagerError error, string message)
      : base(message) {
      _error = error;
    }

    public UpdateManagerError Error {
      get { return _error; }
    }
  }
  #endregion

  public class WebReader {

    private bool _AbortFlag = false;

    public event WebReaderEventHandler StartDownloadFile;
    public event WebReaderEventHandler StopDownloadFile;
    public event WebReaderProcessEventHandler DownloadFileProcess;

    public void DownloadFile(string url, FileInfo toFile) {
      Thread th = new Thread(new ParameterizedThreadStart(DownloadFileMethod));
      th.IsBackground = true;
      th.Priority = ThreadPriority.Lowest;
      th.Start(new object[] { url, toFile});
    }

    #region private bool AbortFlag
    private bool AbortFlag {
      get { return _AbortFlag; }
      set {
        _AbortFlag = value;
      }
    }
    #endregion

    #region protected virtual void OnStartDownloadFile(WebReaderEventArgs e)
    protected virtual void OnStartDownloadFile(WebReaderEventArgs e) {
      if (this.StartDownloadFile != null)
        StartDownloadFile(this, e);
    }
    #endregion

    #region protected virtual void OnStopDownloadFile(WebReaderEventArgs e)
    protected virtual void OnStopDownloadFile(WebReaderEventArgs e) {
      if (this.StopDownloadFile != null)
        this.StopDownloadFile(this, e);
    }
    #endregion

    #region protected virtual void OnDownloadFileProcess(WebReaderProcessEventArgs e)
    protected virtual void OnDownloadFileProcess(WebReaderProcessEventArgs e) {
      if (DownloadFileProcess != null)
        DownloadFileProcess(this, e);
    }
    #endregion

    #region public void Abort()
    public void Abort() {
      this.AbortFlag = true;
    }
    #endregion

    #region public void DownloadFileMethod(object param)
    /// <summary>
    /// Download file.
    /// Sample: http://localhost/liteupdate/update.php?go=down&p=LiteUpdateDevelop&f=mydir/myfile.txt
    /// </summary>
    /// <param name="param"></param>
    private void DownloadFileMethod(object param) {
      object[] objs = (object[])param;
      string url = (string)objs[0];
      FileInfo file = (FileInfo)objs[1];
      this.DownloadFileMethod(url, file);
    }
    #endregion

    #region public UpdateManagerError DownloadFileMethod(string url, FileInfo file)
    public UpdateManagerError DownloadFileMethod(string url, FileInfo file) {
      string errorMessage = "";
      UpdateManagerError error = UpdateManagerError.None;
      int size = 0;
      string urlSize = url.Replace("?go=down", "?go=size");
      string page = ReadPage(urlSize);

      OnStartDownloadFile(new WebReaderEventArgs(file, size, error, errorMessage));

      try {
        if (page == "") {
          error = UpdateManagerError.ConnectionError;
        } else {
          string[] sa = page.Split('\n');
          if (sa.Length != 2 || sa[0] != "ok") {
            if (sa.Length >= 2)
              errorMessage = sa[1];

            error = UpdateManagerError.ServerError;
          } else {
            size = Convert.ToInt32(sa[1]);
          }
        }
        if (error != UpdateManagerError.None)
          throw (new WebReaderException(error, errorMessage));

        this.DownloadFile(url, file, size);

      } catch (WebReaderException we) {
        error = we.Error;
        errorMessage = we.Message;

      } catch (Exception e) {
        error = UpdateManagerError.Unknow;
        errorMessage = e.Message;

      } finally {
        OnStopDownloadFile(new WebReaderEventArgs(file, size, error, errorMessage));
      }
      return error;
    }
    #endregion

    #region private void DownloadFile(string url, FileInfo file, int size)
    private void DownloadFile(string url, FileInfo file, int size) {
      AbortFlag = false;

      if (file.Exists)
        if (file.Length == size)
          return;
        else
          file.Delete();
      if (!file.Directory.Exists)
        file.Directory.Create();

      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
      request.Headers.Add("Cache-Control", "no-cache");
      request.Credentials = CredentialCache.DefaultCredentials;

      if (Configure.Proxy.Enable) {
        WebProxy proxyObject = Configure.Proxy.GetWebProxy();
        if (proxyObject != null)
          request.Proxy = proxyObject;
      }

      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      Stream strm = response.GetResponseStream();

      FileStream fst = new FileStream(file.FullName, FileMode.CreateNew);
      StreamWriter sw = new StreamWriter(fst);

      long time = DateTime.Now.Ticks;
      int countRead;
      int downloadSize = 0;
      bool userAbort = false;
      do {
        byte[] buf = new byte[2 * 1024];
        countRead = strm.Read(buf, 0, 2 * 1024);
        fst.Write(buf, 0, countRead);
        downloadSize += countRead;

        #region if (DateTime.Now.Ticks - time > 10000000L) {...}
        if (DateTime.Now.Ticks - time > 10000000L) {
          this.OnDownloadFileProcess(new WebReaderProcessEventArgs(file, size, downloadSize));
          time = DateTime.Now.Ticks;
        }
        #endregion
        
#if DEBUG
        // Thread.Sleep(500);
#endif
        userAbort = AbortFlag;
        if (userAbort) {
          break;
        }
      } while (countRead > 0);

      fst.Flush();
      fst.Close();
      response.Close();
      file.Refresh();
      AbortFlag = false;

      if (userAbort) {
        file.Delete();
        throw (new WebReaderException(UpdateManagerError.UserAbort, "Abort"));
      }

      if (file.Length != size) {
        file.Delete();
        throw(new WebReaderException( UpdateManagerError.ServerError, "It is impossible to download a file"));
      }
    }
    #endregion

    #region public static string ReadPage(string url)
    public static string ReadPage(string url) {
      try {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.Headers.Add("Cache-Control", "no-cache");
        request.Credentials = CredentialCache.DefaultCredentials;

        if (Configure.Proxy.Enable) {
          WebProxy proxyObject = Configure.Proxy.GetWebProxy();
          if (proxyObject != null)
            request.Proxy = proxyObject;
        }

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream strm = response.GetResponseStream();

        string strpage = "";

        int numReaded = 1;
        while (numReaded > 0) {
          byte[] buf = new byte[4 * 1024];
          numReaded = strm.Read(buf, 0, 4 * 1024);

          for (int i = 0; i < numReaded; i++) {
            strpage += new string(Convert.ToChar(buf[i]), 1);
          }
        }
        response.Close();
        return strpage;
      } catch (Exception) { }
      return "";
    }
    #endregion
  }
}
