/**
* @version $Id: UpdateDetailsManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  #region class MoveFile
  class MoveFile {
    private readonly FileInfo _file;
    private readonly DirectoryInfo _moveToDirectory;

    public MoveFile(FileInfo file, DirectoryInfo todir) {
      _file = file;
      _moveToDirectory = todir;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public DirectoryInfo ToDir
    public DirectoryInfo ToDir {
      get { return _moveToDirectory; }
    }
    #endregion
  }
  #endregion

  class UpdateDetailsManager {
    
    private readonly UpdateManager _updateManager;
    private readonly List<string> _userInfo = new List<string>();
    private readonly WebReader _webReader;

    private readonly List<LUFileInfo> _filesUpdate = new List<LUFileInfo>();
    private FileInfo _updateFilesFile;
    private UpdateFileInfoManager _newUFI;
    private readonly List<MoveFile> _moveFiles = new List<MoveFile>();

    #region public static string TEMPLATE_BODY
    public static string TEMPLATE_BODY =
      "<html><head><title>Detailed Info</title></head><body><h1>Detailed Info</h1>{0}</body></html>";
    #endregion

    #region public static string TEMPLATE_INFO_SIZE
    public static string TEMPLATE_INFO_SIZE = "" +
      "<p>Download Size = {0}</p>";
    #endregion

    #region public static string TEMPLATE_VERSION
    public static string TEMPLATE_VERSION = "" +
      "<p>================ Update Version {0} ================<br>" +
      "{1}</p>";
    #endregion

    #region public static string TEMPLATE_VERSION_CURRENT
    public static string TEMPLATE_VERSION_CURRENT = "" +
      "<p>================ <font color=red>Update Version {0}</font> ================<br>" +
      "{1}</p>";
    #endregion

    public UpdateDetailsManager(UpdateManager updateManager) {
      _updateManager = updateManager;
      _webReader = updateManager.WebReader;
      this.CreateDetails();
    }

    #region private void CreateDetails()
    private void CreateDetails() {
      DirectoryInfo dir = Configure.VersionUserInfoDirectory;

      for (int i = 1; i <= _updateManager.CurrentVersionNumber; i++) {

        string templateVersion = i == _updateManager.CurrentVersionNumber ? TEMPLATE_VERSION_CURRENT : TEMPLATE_VERSION;
        string userInfoHtml = "";
        FileInfo userInfoFile = new FileInfo(Path.Combine(Configure.VersionUserInfoDirectory.FullName, string.Format(Configure.VersionUserInfoFileName, i)));

        if (userInfoFile.Exists)
          userInfoHtml = File.ReadAllText(userInfoFile.FullName);

        _userInfo.Add(string.Format(templateVersion, i, userInfoHtml));
      }

      WebReader webReader = _updateManager.WebReader;
      UpdateManagerError error = this.DownloaFiles();
      if (error != UpdateManagerError.None)
        throw (new WebReaderException(error, ""));

      _newUFI = new UpdateFileInfoManager(_updateFilesFile);
      _moveFiles.Add(new MoveFile(_updateFilesFile, Configure.UpdateDirectory));

      UpdateFileInfo[] ufis = this.GetUpdateFiles();
      int downloadSize = 0;
      foreach (UpdateFileInfo ufi in ufis) {
        if (ufi.Action != UpdateFileInfoAction.Deleted)
          downloadSize += ufi.CompressSize;
      }
      
      _userInfo.Add(string.Format(TEMPLATE_INFO_SIZE, ConvertToSize(downloadSize)));
    }
    #endregion

    #region public void UpdateComplete()
    public void UpdateComplete() {
      foreach (MoveFile mf in _moveFiles) {
        if (!mf.File.Exists)
          continue;
        FileInfo destFile = new FileInfo(Path.Combine(mf.ToDir.FullName, mf.File.Name));
        if (destFile.Exists)
          destFile.Delete();
        File.Move(mf.File.FullName, destFile.FullName);
      }
    }
    #endregion

    #region internal static string ConvertToSize(int size)
    internal static string ConvertToSize(int size) {
      double dsize = size;
      string ssize = string.Format("{0}kb", ((double)(dsize / 1024)).ToString("N"));
      return ssize;
    }
    #endregion

    #region private UpdateManagerError DownloaFiles()
    private UpdateManagerError DownloaFiles() {
      UpdateManagerError error = UpdateManagerError.None;
      DirectoryInfo downloadsDir = Configure.DownloadsDirectory;
      downloadsDir.Create();

      for (int i = _updateManager.CurrentVersionNumber + 1; i <= _updateManager.NewVersionNumber; i++) {
        string fileUserInfoPath = Configure.VersionUserInfoDirectoryName + "\\" + string.Format(Configure.VersionUserInfoFileName, i);

        FileInfo fileUserInfo = new FileInfo(Path.Combine(downloadsDir.FullName, fileUserInfoPath));

        string urlUserInfo = Configure.CreateScriptDownloadFile(fileUserInfoPath);
        error = _webReader.DownloadFileMethod(urlUserInfo, fileUserInfo);
        if (error != UpdateManagerError.None)
          return error;

        _userInfo.Add(string.Format(TEMPLATE_VERSION, i, File.ReadAllText(fileUserInfo.FullName)));
        _moveFiles.Add(new MoveFile(fileUserInfo, Configure.VersionUserInfoDirectory));
      }

      string filePath = Configure.UpdateFileListName;
      _updateFilesFile = new FileInfo(Path.Combine(downloadsDir.FullName, filePath));
      string url = Configure.CreateScriptDownloadFile(filePath);
      error = _webReader.DownloadFileMethod(url, _updateFilesFile);

      return error;
    }
    #endregion

    #region public string GetHtmlReport()
    public string GetHtmlReport() {
      string info = "";
      for (int i = _userInfo.Count - 1; i >= 0; i--) {
        info += _userInfo[i];
      }
      return string.Format(TEMPLATE_BODY, info);
    }
    #endregion

    #region public UpdateFileInfo[] GetUpdateFiles()
    public UpdateFileInfo[] GetUpdateFiles() {
      List<UpdateFileInfo> list = new List<UpdateFileInfo>();

      foreach (UpdateFileInfo ufi in _newUFI) {
        UpdateFileInfo cufi = _updateManager.CurrentUFI.Find(ufi);
        if (cufi == null) {
          list.Add(ufi);
          continue;
        }
        if (ufi.Action == UpdateFileInfoAction.Deleted && cufi.Action != UpdateFileInfoAction.Deleted) {
          list.Add(ufi);
          continue;
        }

        if (ufi.Version == cufi.Version)
          continue;
        if (ufi.Version > cufi.Version)
          list.Add(ufi);
      }
      return list.ToArray();
    }
    #endregion
  }
}
