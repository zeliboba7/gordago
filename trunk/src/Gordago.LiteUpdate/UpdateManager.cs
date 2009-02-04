/**
* @version $Id: UpdateManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading;
  using System.IO;
  using Gordago.LiteUpdate;
  using System.Windows.Forms;
  using ICSharpCode.SharpZipLib.Zip;

  #region public class UpdateManagerProcessEventArgs : EventArgs
  public class UpdateManagerProcessEventArgs : EventArgs {

    private int _total;
    private int _current;

    public UpdateManagerProcessEventArgs(int total, int current)
      : base() {
      _total = total;
      _current = current;
    }

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

  public delegate void UpdateManagerProcessEventHandler(object sender, UpdateManagerProcessEventArgs e);

  #region public class UnZipFileEventArgs : EventArgs
  public class UnZipFileEventArgs : EventArgs {
    private readonly FileInfo _file;

    public UnZipFileEventArgs(FileInfo file)
      : base() {
      _file = file;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion
  }
  #endregion

  public delegate void UnZipFileEventHandler(object sender, UnZipFileEventArgs e);

  #region public class UnZipProgressEventArgs : UpdateManagerProcessEventArgs
  public class UnZipProgressEventArgs : UpdateManagerProcessEventArgs {
    private readonly FileInfo _file;

    public UnZipProgressEventArgs(FileInfo file, int total, int current)
      : base(total, current) {
      _file = file;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion
  }
  #endregion

  public delegate void UnZipProgressEventHandler(object sender, UnZipProgressEventArgs e);

  public class UpdateManager {
    
    public event EventHandler StartCheckForUpdates;
    public event EventHandler StopCheckForUpdates;
    public event EventHandler StartGetUpdateDetails;
    public event EventHandler StopGetUpdateDetails;
    public event EventHandler StartUpdate;
    public event EventHandler StopUpdate;

    public event UnZipFileEventHandler StartUnZipFile;
    public event UnZipFileEventHandler StopUnZipFile;
    public event UnZipProgressEventHandler UnZipFileProgress;

    private bool _isOldVersion = false;
    private int _currentVersionNumber = 1;
    private int _newVersionNumber = 1;
    private UpdateManagerError _error;
    private string _errorMessage;
    private bool _checkForUpdateProcess = false;
    private string _htmlDetailsInfo = "";
    private readonly WebReader _webReader = new WebReader();
    private UpdateFileInfoManager _currentUFI;

    private UpdateDetailsManager _udm;
    private DateTime _lastGetUDMTime = DateTime.Now.AddDays(-1);
    private bool _isDownloadUpdateFiles = false;
    private bool _abort = false;
    private bool _isUpdateComplete = false;
    private bool _isLoad = false;

    public UpdateManager() { }

    #region public bool IsUpdateComplete
    public bool IsUpdateComplete {
      get { return _isUpdateComplete; }
    }
    #endregion

    #region public bool IsDownloadUpdateFiles
    public bool IsDownloadUpdateFiles {
      get { return _isDownloadUpdateFiles; }
    }
    #endregion

    #region internal UpdateFileInfoManager CurrentUFI
    internal UpdateFileInfoManager CurrentUFI {
      get { return _currentUFI; }
    }
    #endregion

    #region public WebReader WebReader
    public WebReader WebReader {
      get { return _webReader; }
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

    #region public bool IsOldVersion
    public bool IsOldVersion {
      get { return _isOldVersion; }
    }
    #endregion

    #region public bool IsCheckForUpdateProcess
    public bool IsCheckForUpdateProcess {
      get { return _checkForUpdateProcess; }
    }
    #endregion

    #region public int CurrentVersionNumber
    public int CurrentVersionNumber {
      get { return _currentVersionNumber; }
    }
    #endregion

    #region public int NewVersionNumber
    public int NewVersionNumber {
      get { return _newVersionNumber; }
    }
    #endregion

    #region public string HtmlDetailsInfo
    public string HtmlDetailsInfo {
      get { return _htmlDetailsInfo; }
    }
    #endregion

    #region public void Load()
    public void Load() {
      if (_isLoad)
        return;
      _isLoad = true;
      Configure.UpdateDirectory.Create();

      FileInfo file = new FileInfo(Path.Combine(Configure.UpdateDirectory.FullName, Configure.VersionNumberFileName));
      if (!file.Exists)
        return;
      string version = File.ReadAllText(file.FullName);
      _currentVersionNumber = Convert.ToInt32(version);
      _currentUFI = new UpdateFileInfoManager(new FileInfo(Path.Combine(Configure.UpdateDirectory.FullName, Configure.UpdateFileListName)));

      try {
        if (Configure.DownloadsDirectory.Exists) {
          DateTime dirCreated = Configure.DownloadsDirectory.CreationTime;
          if (dirCreated.AddHours(1).Ticks < DateTime.Now.Ticks) {
            try {
              Configure.DownloadsDirectory.Delete(true);
            } catch { }
          }
        }
      } catch { }
      Configure.DownloadsDirectory.Create();
    }
    #endregion

    #region public void CheckForUpdates()
    public void CheckForUpdates() {
      Thread th = new Thread(new ThreadStart(this.CheckForUpdatesMethod));
      th.IsBackground = true;
      th.Name = "CheckForUpdates";
      th.Start();
    }
    #endregion

    #region public void CheckForUpdatesMethod()
    public void CheckForUpdatesMethod() {
      this.Load();
      _error = UpdateManagerError.None;
      _checkForUpdateProcess = true;
      this.OnStartCheckForUpdates(EventArgs.Empty);
      try {
        if (!_isOldVersion) {
          string url = Configure.CreateScriptGetVersionUrl();
          string page = WebReader.ReadPage(url);

          if (page == "") {
            _error = UpdateManagerError.ConnectionError;
          } else {
            string[] sa = page.Split('\n');
            if (sa.Length != 2 || sa[0] != "ok") {
              _error = UpdateManagerError.ServerError;
            } else {
              _newVersionNumber = Convert.ToInt32(sa[1]);

              if (_newVersionNumber < 1) {
                _error = UpdateManagerError.ServerError;
              } else {
                if (_newVersionNumber > _currentVersionNumber)
                  _isOldVersion = true;
              }
            }
          }
        }
      } finally {
        _checkForUpdateProcess = false;
        this.OnStopCheckForUpdates(EventArgs.Empty);
      }
    }
    #endregion

    #region protected virtual void OnStartCheckForUpdates(EventArgs e)
    protected virtual void OnStartCheckForUpdates(EventArgs e) {
      if (StartCheckForUpdates != null)
        StartCheckForUpdates(this, e);
    }
    #endregion

    #region protected virtual void OnStopCheckForUpdates(EventArgs e)
    protected virtual void OnStopCheckForUpdates(EventArgs e) {
      if (this.StopCheckForUpdates != null)
        this.StopCheckForUpdates(this, e);

    }
    #endregion

    #region protected virtual void OnStartGetUpdateDetails(EventArgs e)
    protected virtual void OnStartGetUpdateDetails(EventArgs e) {
      if (StartGetUpdateDetails != null)
        this.StartGetUpdateDetails(this, e);
    }
    #endregion

    #region protected virtual void OnStopGetUpdateDetails(EventArgs e)
    protected virtual void OnStopGetUpdateDetails(EventArgs e) {
      if (this.StopGetUpdateDetails != null)
        this.StopGetUpdateDetails(this, e);
    }
    #endregion

    #region protected virtual void OnStartUpdate(EventArgs e)
    protected virtual void OnStartUpdate(EventArgs e) {
      if (StartUpdate != null)
        StartUpdate(this, e);
    }
    #endregion

    #region protected virtual void OnStopUpdate(EventArgs e)
    protected virtual void OnStopUpdate(EventArgs e) {
      if (StopUpdate != null)
        this.StopUpdate(this, e);
    }
    #endregion

    #region public void GetUpdateDetails()
    public void GetUpdateDetails() {
      Thread th = new Thread(new ThreadStart(this.GetUpdateDetailsMethod));
      th.IsBackground = true;
      th.Name = "GetUpdateDetails";
      th.Start();
    }
    #endregion

    #region public void GetUpdateDetailsMethod()
    public void GetUpdateDetailsMethod() {
      this.Load();
      _error = UpdateManagerError.None;
      this.OnStartGetUpdateDetails(EventArgs.Empty);
      try {
        this.GetUpdateDetailsProcess();
      } catch (WebReaderException we) {
        _error = we.Error;
        _errorMessage = we.Message;
      } catch (Exception e) {
        _error = UpdateManagerError.Unknow;
        _errorMessage = e.Message;
      } finally {
        this.OnStopGetUpdateDetails(EventArgs.Empty);
      }
    }
    #endregion

    #region private void GetUpdateDetailsProcess()
    private void GetUpdateDetailsProcess() {
      if (_udm != null && _lastGetUDMTime.AddHours(1).Ticks > DateTime.Now.Ticks)
        return;

      UpdateDetailsManager udm = new UpdateDetailsManager(this);
      _htmlDetailsInfo = udm.GetHtmlReport();
      _udm = udm;
    }
    #endregion

    #region public void Update()
    public void Update() {
      Thread th = new Thread(new ThreadStart(this.UpdateMethod));
      th.IsBackground = true;
      th.Name = "Update";
      th.Start();
    }
    #endregion

    #region public void UpdateMethod()
    public void UpdateMethod() {
      _error = UpdateManagerError.None;
      this.OnStartUpdate(EventArgs.Empty);
      try {
        this.UpdateProcess();
      } catch (WebReaderException we) {
        _error = we.Error;
        _errorMessage = we.Message;
      } catch (Exception e) {
        _error = UpdateManagerError.Unknow;
        _errorMessage = e.Message;
      } finally {
        this.OnStopUpdate(EventArgs.Empty);
      }
    }
    #endregion

    #region private void UpdateProcess()
    private void UpdateProcess() {
      this.GetUpdateDetailsProcess();
      UpdateFileInfo[] ufis = _udm.GetUpdateFiles();
      DirectoryInfo dir = new DirectoryInfo(Path.Combine(Configure.DownloadsDirectory.FullName, "files"));

      foreach (UpdateFileInfo ufi in ufis) {
        string zipFile = ufi.FileInfo + ".zip";
        FileInfo file = new FileInfo(Path.Combine(dir.FullName, zipFile));
        file.Directory.Create();

        string url = Configure.CreateScriptDownloadFile(zipFile);

        UpdateManagerError error = _webReader.DownloadFileMethod(url, file);

        if (error != UpdateManagerError.None)
          throw (new WebReaderException(error, ""));
      }
      _isDownloadUpdateFiles = true;

      DirectoryInfo newReleaseDirectory = new DirectoryInfo(Path.Combine(Configure.UpdateDirectory.FullName, "newrelease"));
      try {
        if (newReleaseDirectory.Exists)
          newReleaseDirectory.Delete(true);
      } catch { }
      newReleaseDirectory.Create();

      Exception exError = null;

      _abort = false;
      foreach (UpdateFileInfo ufi in ufis) {
        if (ufi.Action == UpdateFileInfoAction.Deleted)
          continue;

        FileInfo zipFile = new FileInfo(Path.Combine(dir.FullName, ufi.FileInfo + ".zip"));
        FileInfo destFile = new FileInfo(Path.Combine(newReleaseDirectory.FullName, ufi.FileInfo));

        this.OnStartUnZipFile(new UnZipFileEventArgs(zipFile));
        try {
          this.UnZipFile(zipFile, destFile);
        } catch (Exception e) {
          exError = e;
        }
        this.OnStopUnZipFile(new UnZipFileEventArgs(zipFile));
        if (exError != null)
          break;
      }
      if (exError != null)
        throw (exError);


      DirectoryInfo prevRelDir = Configure.PrevReleaseDirectory;
      try{
        if (prevRelDir.Exists)
          prevRelDir.Delete(true);
      }catch{}

      /* Move old file */
      foreach (UpdateFileInfo ufi in ufis) {
        FileInfo sourceFile = new FileInfo(LURootFolder.Convert(ufi.FileInfo));
        FileInfo destFile = new FileInfo(Path.Combine(prevRelDir.FullName, ufi.FileInfo));

        if (ufi.Action == UpdateFileInfoAction.Deleted) 
          destFile = new FileInfo(destFile.FullName + ".upddeleted");

        destFile.Directory.Create();

        File.Move(sourceFile.FullName, destFile.FullName);
      }
      
      /* Move new File*/
      foreach (UpdateFileInfo ufi in ufis) {
        if (ufi.Action == UpdateFileInfoAction.Deleted)
          return;

        FileInfo sourceFile = new FileInfo(Path.Combine(newReleaseDirectory.FullName, ufi.FileInfo));
        FileInfo destFile = new FileInfo(LURootFolder.Convert(ufi.FileInfo));
        File.Move(sourceFile.FullName, destFile.FullName);
      }

      try {
        newReleaseDirectory.Delete(true);
      } catch { }

      _udm.UpdateComplete();

      FileInfo fileVersionNumber = new FileInfo(Path.Combine(Configure.UpdateDirectory.FullName, Configure.VersionNumberFileName));
      if (fileVersionNumber.Exists)
        fileVersionNumber.Delete();
      File.WriteAllText(fileVersionNumber.FullName, _newVersionNumber.ToString());

      try {
        Configure.DownloadsDirectory.Delete(true);
      } catch { }
      
      _isUpdateComplete = true;
    }
    #endregion

    #region public void Abort()
    public void Abort() {
      _abort = true;
    }
    #endregion

    #region protected virtual void OnStartUnZipFile(UnZipFileEventArgs e)
    protected virtual void OnStartUnZipFile(UnZipFileEventArgs e) {
      if (this.StartUnZipFile != null)
        this.StartUnZipFile(this, e);
    }
    #endregion

    #region protected virtual void OnStopUnZipFile(UnZipFileEventArgs e)
    protected virtual void OnStopUnZipFile(UnZipFileEventArgs e) {
      if (this.StopUnZipFile != null)
        this.StopUnZipFile(this, e);
    }
    #endregion

    #region protected virtual void OnUnZipFileProgress(UnZipProgressEventArgs e)
    protected virtual void OnUnZipFileProgress(UnZipProgressEventArgs e) {
      if (this.UnZipFileProgress != null)
        this.UnZipFileProgress(this, e);
    }
    #endregion

    #region private void UnZipFile(FileInfo zipFile, FileInfo destFile)
    private void UnZipFile(FileInfo zipFile, FileInfo destFile) {
      using (ZipInputStream s = new ZipInputStream(zipFile.OpenRead())) {

        ZipEntry theEntry = s.GetNextEntry();

        string fileName = Path.GetFileName(theEntry.Name);

        if (!fileName.Equals(destFile.Name))
          throw(new Exception("Bad zip file"));

        if (destFile.Exists)
          destFile.Delete();
        
        long time = DateTime.Now.Ticks;
        destFile.Directory.Create();

        using (FileStream streamWriter = destFile.Create()) {

          int size = 2048;
          byte[] data = new byte[2048];
          int current = 0;
          while (true) {
            if (_abort)
              throw (new WebReaderException(UpdateManagerError.UserAbort, "Abort"));

            size = s.Read(data, 0, data.Length);
            if (size > 0) {
              streamWriter.Write(data, 0, size);
              current += size;

              if (DateTime.Now.Ticks - time > 10000000L) {
                this.OnUnZipFileProgress(new UnZipProgressEventArgs(destFile, (int)theEntry.Size, current));
                time = DateTime.Now.Ticks;
              }

            } else {
              break;
            }
          }
        }
        destFile.LastWriteTime = destFile.LastAccessTime =  destFile.CreationTime = theEntry.DateTime;
      }
    }
    #endregion
  }
}
