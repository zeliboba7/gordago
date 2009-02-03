/**
* @version $Id: Project.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Xml;
  using System.Windows.Forms;
  using System.ComponentModel;
  using Gordago.Core;

  #region public class VersionInfoEventArgs : EventArgs
  public class VersionInfoEventArgs : EventArgs {

    private readonly VersionInfo _version;

    public VersionInfoEventArgs(VersionInfo version)
      : base() {
      _version = version;
    }

    #region public VersionInfo Version
    public VersionInfo Version {
      get { return _version; }
    }
    #endregion
  }
  #endregion

  public delegate void VersionInfoEventHandler(object sender, VersionInfoEventArgs e);

  public class Project {

    public event EventHandler DataChanged;
    public event VersionInfoEventHandler VersionChanged;

    public const string EXT = "luprj";
    public readonly string SUBDIR_VERSIONS = "versions";

    private readonly TMFileSystem _fileSystem = new TMFileSystem();

    private readonly Dictionary<int, VersionInfo> _loadedVersions = new Dictionary<int, VersionInfo>();

    private FileInfo _projectFile;

    private bool _isSaved = false;
    
    private string _guid;

    private int _version;

    private DirectoryInfo _appDirectory;
    private DirectoryInfo _appUpdateDirectory;

    #region private Project()
    private Project() {
      _guid = Guid.NewGuid().ToString();
      _fileSystem.DataChanged += new EventHandler(_fileSystem_DataChanged);
    }
    #endregion

    #region public Project(FileInfo projectFile, DirectoryInfo luAppDirectory):this()
    /// <summary>
    /// Create new project
    /// </summary>
    /// <param name="projectFile"></param>
    /// <param name="luAppDirectory"></param>
    public Project(FileInfo projectFile, DirectoryInfo luAppDirectory):this() {
      _projectFile = projectFile;

      TMRootFolder appRootFolder = new TMRootFolder(LURootFolderType.Application);
      _fileSystem.Add(appRootFolder);
      this.JoinDirectory(appRootFolder, luAppDirectory);

      _appDirectory = luAppDirectory;
      _appUpdateDirectory = new DirectoryInfo(Path.Combine(luAppDirectory.FullName, "Update"));

      VersionInfo version = this.CreateNewVersion();
      this.ApplyVersion(version, true);
      this.Save();
    }
    #endregion

    #region public Project(FileInfo projectFile)
    /// <summary>
    /// Load project
    /// </summary>
    /// <param name="projectFile"></param>
    public Project(FileInfo projectFile)
      : this() {
      _projectFile = projectFile;
      _isSaved = true;
      this.Load();
    }
    #endregion

    #region public DirectoryInfo AppDirectory
    public DirectoryInfo AppDirectory {
      get { return _appDirectory; }
      set { _appDirectory = value; }
    }
    #endregion

    #region public DirectoryInfo AppUpdateDirectory
    public DirectoryInfo AppUpdateDirectory {
      get { return _appUpdateDirectory; }
      set {
        _appUpdateDirectory = value;
        this.OnDataChanged();
      }
    }
    #endregion

    #region public int Version
    public int Version {
      get { return _version; }
    }
    #endregion

    #region public string GUID
    public string GUID {
      get { return _guid; }
    }
    #endregion

    #region public string Text
    [Browsable(false)]
    public string Text {
      get {
        return _projectFile.Name + (this.IsSaved ? "":"*");
      }
    }
    #endregion

    #region public FileInfo File
    [Browsable(false)]
    public FileInfo File {
      get { return _projectFile; }
    }
    #endregion

    #region public string Name
    [Browsable(false)]
    public string Name {
      get { 
        return Path.GetFileNameWithoutExtension(_projectFile.Name); 
      }
    }
    #endregion

    #region public TMFileSystem FileSystem
    [Browsable(false)]
    public TMFileSystem FileSystem {
      get { return _fileSystem; }
    }
    #endregion

    #region public bool IsSaved
    [Browsable(false)]
    public bool IsSaved {
      get { return _isSaved; }
    }
    #endregion

    #region private void JoinDirectory(TMFolder folder, DirectoryInfo dir)
    private void JoinDirectory(TMFolder folder, DirectoryInfo dir) {
      foreach (DirectoryInfo d in dir.GetDirectories()) {
        TMFolder childTMFolder = new TMFolder(d.Name, folder);
        folder.Folders.Add(childTMFolder);
        this.JoinDirectory(childTMFolder, d);
      }
      foreach (FileInfo file in dir.GetFiles()) {
        TMFile tmFile = new TMFile(file, folder);
        tmFile.ModifyTime = file.LastWriteTime;
        tmFile.Length = file.Length;
        folder.Files.Add(tmFile);
      }

    }
    #endregion

    #region private FileInfo GetSaveFile(FileInfo file)
    private FileInfo GetSaveFile(FileInfo file) {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.FileName = file.FullName;
      sfd.InitialDirectory = file.Directory.FullName;
      sfd.Filter = string.Format("Lite Update Files (*.{0})|*.{0}", EXT);
      if (sfd.ShowDialog() != DialogResult.OK)
        return null;
      
      file = new FileInfo(sfd.FileName);
      return file;
    }
    #endregion

    #region public static FileInfo GetOpenFile()
    public static FileInfo GetOpenFile() {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = string.Format("Lite Update Files (*.{0})|*.{0}", EXT);
      ofd.InitialDirectory = Global.Properties["Project"].GetValue<string>("Directory", Global.Setup.ProjectsDirectory.FullName);
      ofd.Multiselect = false;
      if (ofd.ShowDialog() != DialogResult.OK)
        return null;
      FileInfo file = new FileInfo(ofd.FileName);
      Global.Properties["Project"].SetValue<string>("Directory", file.Directory.FullName);
      return file;
    }
    #endregion

    #region public void SaveAs()
    public void SaveAs() {
      FileInfo file = this.GetSaveFile(_projectFile);
      if (file == null)
        return ;
      _isSaved = false;
      _projectFile = file;
      this.Save();
    }
    #endregion

    #region private static void SaveFolder(EasyPropertiesNode node, TMFolder folder)
    private static void SaveFolder(EasyPropertiesNode node, TMFolder folder) {
      foreach (TMFolder tmFolder in folder.Folders) {
        EasyPropertiesNode childNode = node[tmFolder.Name];
        SaveFolder(childNode, tmFolder);
      }

      node.SetValue<int>("Count", folder.Files.Count);
      for (int i = 0; i < folder.Files.Count; i++) {
        TMFile tmFile = folder.Files[i];
        string pname = "File" + i.ToString();
        string[] sa = new string[] { };
        node.SetValue<int>(pname + "_version", tmFile.Version);
        node.SetValue<string>(pname + "_fullname", tmFile.File.FullName);
        node.SetValue<DateTime>(pname + "_time", tmFile.ModifyTime);
        node.SetValue<long>(pname + "_length", tmFile.Length);
      }
    }
    #endregion

    #region public static void SaveFileSystem(EasyPropertiesNode ps, TMFileSystem fileSystem)
    public static void SaveFileSystem(EasyPropertiesNode ps, TMFileSystem fileSystem) {
      foreach (TMRootFolder rootFolder in fileSystem) {
        EasyPropertiesNode node = ps[rootFolder.RootFolderType.ToString()];
        SaveFolder(node, rootFolder);
      }
    }
    #endregion

    #region public bool Save()
    public bool Save() {
      EasyProperties ps = new EasyProperties();

      ps.SetValue<string>("GUID", _guid);
      ps.SetValue<string>("AppDir", _appDirectory.FullName);
      ps.SetValue<string>("AppUpdateDir", _appUpdateDirectory.FullName);

      SaveFileSystem(ps["FileSystem"], _fileSystem);

      ps.SetValue<int>("Version", this.Version);

      foreach (VersionInfo version in _loadedVersions.Values) {
        if (!version.IsSaved) {
          version.Save(this.GetVersionFileInfo(version.Number));
        }
      }

      this._isSaved = true;
      ps.Save(_projectFile);

      return true;
    }
    #endregion

    #region public static void LoadFileSystem(EasyPropertiesNode pnode, TMFileSystem fileSystem)
    public static void LoadFileSystem(EasyPropertiesNode pnode, TMFileSystem fileSystem) {
      foreach (EasyPropertiesNode node in pnode.GetChildProperties()) {
        TMRootFolder rootFolder = new TMRootFolder((LURootFolderType)Enum.Parse(typeof(LURootFolderType), node.Name));
        LoadFolders(node, rootFolder);
        LoadFiles(node, rootFolder);
        fileSystem.Add(rootFolder);
      }
    }
    #endregion

    #region public void Load()
    public void Load() {
      EasyProperties ps = new EasyProperties();
      ps.Load(_projectFile);
      _guid = ps.GetValue<string>("GUID", Guid.NewGuid().ToString());
      _appDirectory = new DirectoryInfo(ps.GetValue<string>("AppDir", ""));
      _appUpdateDirectory = new DirectoryInfo(ps.GetValue<string>("AppUpdateDir", ""));

      LoadFileSystem(ps["FileSystem"], _fileSystem);

      _version = ps.GetValue<int>("Version", 0);

    }
    #endregion

    #region private static void LoadFolders(EasyPropertiesNode node, TMFolder folder)
    private static void LoadFolders(EasyPropertiesNode node, TMFolder folder) {
      EasyPropertiesNode[] childProps = node.GetChildProperties();
      foreach (EasyPropertiesNode childProp in childProps) {
        TMFolder tmFolder = new TMFolder(childProp.Name, folder);
        LoadFolders(childProp, tmFolder);
        LoadFiles(childProp, tmFolder);
        folder.Folders.Add(tmFolder);
      }
    }
    #endregion

    #region private static void LoadFiles(EasyPropertiesNode node, TMFolder folder)
    private static void LoadFiles(EasyPropertiesNode node, TMFolder folder) {
      int count = node.GetValue<int>("Count", 0);
      for (int i = 0; i < count; i++) {
        string pname = "File" + i.ToString();
        string fileName = node.GetValue<string>(pname + "_fullname", "");

        TMFile tmFile = new TMFile(new FileInfo(fileName), folder);
        tmFile.Version = node.GetValue<int>(pname + "_version", 1); 
        tmFile.ModifyTime = node.GetValue<DateTime>(pname + "_time", DateTime.Now);
        tmFile.Length = node.GetValue<long>(pname + "_length", 0);
        
        folder.Files.Add(tmFile);
      }
    }
    #endregion

    #region private void _fileSystem_DataChanged(object sender, EventArgs e)
    private void _fileSystem_DataChanged(object sender, EventArgs e) {
      this.OnDataChanged();
    }
    #endregion

    #region internal void ChangeStatusDataChanged()
    internal void ChangeStatusDataChanged() {
      this.OnDataChanged();
    }
    #endregion

    #region protected virtual void OnDataChanged()
    protected virtual void OnDataChanged() {
      _isSaved = false;
      if (this.DataChanged != null)
        this.DataChanged(this, EventArgs.Empty);
    }
    #endregion

    #region private FileInfo GetVersionFileInfo(int number)
    private FileInfo GetVersionFileInfo(int number) {
      DirectoryInfo dir = new DirectoryInfo(Path.Combine(_projectFile.Directory.FullName, SUBDIR_VERSIONS));

      FileInfo file = new FileInfo(Path.Combine(dir.FullName, string.Format("{0}.{1}", number, VersionInfo.EXT)));

      return file;
    }
    #endregion

    #region public VersionInfo OpenVersion(int number)
    public VersionInfo OpenVersion(int number) {
      if(number > _version)
        throw (new ArgumentException("Number of the version should be less or is equal to the current version", "number"));

      VersionInfo info = null;
      _loadedVersions.TryGetValue(number, out info);
      if (info != null)
        return info;
      FileInfo file = this.GetVersionFileInfo(number);
      if (!file.Exists)
        throw(new FileNotFoundException(string.Format("Version {0} - file not found", number), file.FullName));

      VersionInfo version = new VersionInfo(this, file);
      _loadedVersions.Add(number, version);
      return version;
    }
    #endregion

    #region protected virtual void OnVersionChanged(VersionInfoEventArgs e)
    protected virtual void OnVersionChanged(VersionInfoEventArgs e) {
      if (this.VersionChanged != null)
        this.VersionChanged(this, e);
    }
    #endregion

    #region public VersionInfo CreateNewVersion()
    public VersionInfo CreateNewVersion() {
      VersionInfo lastVersion;
      if (_version == 0)
        lastVersion = new VersionInfo(this, 0, new TMFileSystem());
      else
        lastVersion = this.OpenVersion(_version);
      TMFile[] lastFiles = lastVersion.FileSystem.Clone().GetFiles();
      TMFile[] newFiles = this.FileSystem.Clone().GetFiles();

      VersionInfo newVersion = new VersionInfo(this, lastVersion.Number + 1, this.FileSystem.Clone());

      List<string> filesAdded = new List<string>();
      List<string> filesRemoved = new List<string>();
      List<string> filesUpdated = new List<string>();

      foreach (TMFile newFile in newFiles) {
        bool find = false;

        foreach (TMFile lastFile in lastFiles) {
          if (newFile.FullName.ToUpper() == lastFile.FullName.ToUpper()) {
            find = true;

            if (newFile.File.LastWriteTime.Ticks / 10000000L > lastFile.ModifyTime.Ticks / 10000000L ||
              newFile.Length != lastFile.Length)
              filesUpdated.Add(newFile.FullName);
            break;
          }
        }
        if (!find)
          filesAdded.Add(newFile.FullName);
      }

      foreach (TMFile lastFile in lastFiles) {
        bool find = false;
        foreach (TMFile newFile in newFiles) {
          if (newFile.FullName.ToUpper() == lastFile.FullName.ToUpper()) {
            find = true;
            break;
          }
        }
        if (!find)
          filesRemoved.Add(lastFile.FullName);
      }

      if (filesAdded.Count == 0 && filesRemoved.Count == 0 && filesUpdated.Count == 0)
        return null;

      newVersion.Modify.FilesAdded.AddRange(filesAdded);
      newVersion.Modify.FilesRemoved.AddRange(filesRemoved);
      newVersion.Modify.FilesUpdated.AddRange(filesUpdated);

      return newVersion;
    }
    #endregion

    #region private void UpdateFileSystemCurrentVersion()
    private void UpdateFileSystemCurrentVersion() {
      VersionInfo version = this.OpenVersion(_version);
      TMFile[] files = version.FileSystem.GetFiles();
      foreach (TMFile tmFile in files) {
        tmFile.Update();
      }
    }
    #endregion

    #region public void ApplyVersion(VersionInfo version, bool isNew)
    public void ApplyVersion(VersionInfo version, bool isNew) {

      TMFile[] files = this.FileSystem.GetFiles();

      foreach (TMFile tmFile in files) {

        bool find = false;
        foreach (string sFile in version.Modify.FilesUpdated) {
          if (tmFile.FullName == sFile) {
            find = true;
            break;
          }
        }
        if (find && isNew)
          tmFile.Version++;

        tmFile.Update();
      }

      if (isNew) {
        FileInfo file = this.GetVersionFileInfo(version.Number);
        version.Save(file);
        _version = version.Number;
        _loadedVersions.Add(version.Number, version);
      } else {
        FileInfo file = this.GetVersionFileInfo(_version);
        VersionInfo currentVersion = this.OpenVersion(_version);
        currentVersion.FileSystem = this.FileSystem.Clone();
        version = currentVersion;
        currentVersion.Save(file);
      }

      this.UpdateFileSystemCurrentVersion();
      if (isNew)
        this.OnVersionChanged(new VersionInfoEventArgs(version));
    }
    #endregion

    #region public void ApplyChangesInFileSystem()
    public void ApplyChangesInFileSystem() {
      TMFile[] files = this.FileSystem.GetFiles();

      foreach (TMFile tmFile in files) {
        tmFile.Update();
      }
      this.UpdateFileSystemCurrentVersion();
    }
    #endregion
  }
}
