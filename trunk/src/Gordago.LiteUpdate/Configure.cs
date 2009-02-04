/**
* @version $Id: Configure.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Reflection;

  public static class Configure {

    private static string _UpdateScriptUrl = "http://localhost";
    private static string _ScriptFileName = "update.php";

    private static string _UpdateFileListName = "lu_files.txt";
    private static string _VersionUserInfoFileName = "lu_userinfo_{0}.html";
    private static string _VersionUserInfoDirectoryName = "versions";
    private static string _VersionNumberFileName = "lu_version.txt";

    private static DirectoryInfo _ApplicationDirectory;
    private static DirectoryInfo _UpdateDirectory;

    private static string _ProductId = "";
    private static string _ProductName = "";

    private static ProxySettings _proxy = new ProxySettings();

    static Configure() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      _ApplicationDirectory = new DirectoryInfo(new FileInfo(assembly.Location).Directory.FullName);
      _UpdateDirectory = new DirectoryInfo(Path.Combine(_ApplicationDirectory.FullName, "Update"));

      /*
      object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
      if (attributes.Length != 0) 
        _ProductName = ((AssemblyProductAttribute)attributes[0]).Product;
      /**/
    }

    #region public static DirectoryInfo ApplicationDirectory
    public static DirectoryInfo ApplicationDirectory {
      get { return _ApplicationDirectory; }
      set { _ApplicationDirectory = value; }
    }
    #endregion

    #region public static DirectoryInfo UpdateDirectory
    public static DirectoryInfo UpdateDirectory {
      get { return _UpdateDirectory; }
      set { _UpdateDirectory = value; }
    }
    #endregion

    #region public static DirectoryInfo DownloadsDirectory
    public static DirectoryInfo DownloadsDirectory {
      get {
        return new DirectoryInfo(Path.Combine(_UpdateDirectory.FullName, "downloads"));  
      }
    }
    #endregion

    #region public static DirectoryInfo PrevReleaseDirectory
    public static DirectoryInfo PrevReleaseDirectory {
      get {
        return new DirectoryInfo(Path.Combine(_UpdateDirectory.FullName, "prevrelease")); 
      }
    }
    #endregion

    #region public static string ProductId
    public static string ProductId {
      get { 
        return _ProductId; 
      }
      set {
        _ProductId = value;
      }
    }
    #endregion

    #region public static string ProductName
    public static string ProductName {
      set {
        _ProductName = value;
      }
      get {
        if (_ProductName == "")
          return _ProductId;
        return _ProductName; 
      }
    }
    #endregion

    #region public static ProxySettings Proxy
    public static ProxySettings Proxy {
      get { return _proxy; }
      set { _proxy = value; }
    }
    #endregion

    #region public static string UpdateUrl
    public static string UpdateUrl {
      get { return _UpdateScriptUrl; }
      set { _UpdateScriptUrl = value; }
    }
    #endregion

    #region public static string UpdateFileListName
    public static string UpdateFileListName {
      get { return _UpdateFileListName; }
      set { _UpdateFileListName = value; }
    }
    #endregion

    #region public static string VersionUserInfoDirectoryName
    public static string VersionUserInfoDirectoryName {
      get { 
        return _VersionUserInfoDirectoryName; 
      }
      set { _VersionUserInfoDirectoryName = value; }
    }
    #endregion

    #region public static DirectoryInfo VersionUserInfoDirectory
    public static DirectoryInfo VersionUserInfoDirectory {
      get { return new DirectoryInfo( Path.Combine(_UpdateDirectory.FullName, _VersionUserInfoDirectoryName)); }
    }
    #endregion

    #region public static string VersionUserInfoFileName
    public static string VersionUserInfoFileName {
      get { return _VersionUserInfoFileName; }
      set { _VersionUserInfoFileName = value; }
    }
    #endregion

    #region public static string VersionNumberFileName
    /// <summary>
    /// lu_version.txt
    /// </summary>
    public static string VersionNumberFileName {
      get { return _VersionNumberFileName; }
      set { _VersionNumberFileName = value; }
    }
    #endregion

    #region internal static string CreateScriptGetVersionUrl()
    internal static string CreateScriptGetVersionUrl() {
      return string.Format("{0}/{1}?go=check&p={2}", _UpdateScriptUrl, _ScriptFileName, _ProductId);
    }
    #endregion

    #region internal static string CreateScriptDownloadFile(string filename)
    internal static string CreateScriptDownloadFile(string filename) {
      return string.Format("{0}/{1}?go=down&p={2}&f={3}", _UpdateScriptUrl, _ScriptFileName, _ProductId, filename.Replace("\\", "/"));
    }
    #endregion
  }
}

