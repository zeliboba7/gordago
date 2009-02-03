/**
* @version $Id: VersionInfo.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;
  using System.IO;

  public class VersionInfo {

    public static readonly string EXT = "luvs";

    private readonly FilesModify _modify = new FilesModify();
    private TMFileSystem _fileSystem;
    private readonly int _number;
    private readonly Project _project;

    private string _infoHtml;
    private bool _isSaved = true;

    public VersionInfo(Project project, int version, TMFileSystem fileSystem) {
      _project = project;
      _number = version;
      _fileSystem = fileSystem;
    }

    public VersionInfo(Project project, FileInfo file) {
      _project = project;
      _fileSystem = new TMFileSystem();
      EasyProperties ps = new EasyProperties();
      ps.Load(file);

      _number = ps.GetValue<int>("Version", 1);
      _modify.FilesUpdated.AddRange(ps["Update"].GetValue<string[]>("Files", new string[] { }));
      _modify.FilesRemoved.AddRange(ps["Remove"].GetValue<string[]>("Files", new string[] { }));
      _modify.FilesAdded.AddRange(ps["Add"].GetValue<string[]>("Files", new string[] { }));
      _infoHtml = ps["UserInfo"].GetValue("Html", "");
      Project.LoadFileSystem(ps["FileSystem"], _fileSystem);
    }

    #region public bool IsSaved
    public bool IsSaved {
      get { return _isSaved; }
      set { _isSaved = value; }
    }
    #endregion

    #region public FilesModify Modify
    public FilesModify Modify {
      get { return _modify; }
    }
    #endregion

    #region public string InfoHtml
    public string InfoHtml {
      get { return _infoHtml; }
      set { 
        _infoHtml = value;
        _isSaved = false;
        _project.ChangeStatusDataChanged();
      }
    }
    #endregion

    #region public int Number
    public int Number {
      get { return _number; }
    }
    #endregion

    #region public TMFileSystem FileSystem
    public TMFileSystem FileSystem {
      get { return _fileSystem; }
      set { _fileSystem = value; }
    }
    #endregion

    #region public void Save(FileInfo file)
    public void Save(FileInfo file) {
      EasyProperties ps = new EasyProperties();

      ps.SetValue<int>("Version", _number);
      ps["Update"].SetValue<string[]>("Files", _modify.FilesUpdated.ToArray());
      ps["Remove"].SetValue<string[]>("Files", _modify.FilesRemoved.ToArray());
      ps["Add"].SetValue<string[]>("Files", _modify.FilesAdded.ToArray());
      ps["UserInfo"].SetValue<string>("Html", _infoHtml);
      Project.SaveFileSystem(ps["FileSystem"], this.FileSystem);
      ps.Save(file);
    }
    #endregion
  }
}
