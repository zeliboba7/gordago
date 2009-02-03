/**
* @version $Id: SetupInfo.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Reflection;
  using Gordago.Core;

  class SetupInfo {

    private DirectoryInfo _appDirectory;
    private DirectoryInfo _binDirectory;

    private DirectoryInfo _dataDirectory;
    private DirectoryInfo _optionsDirectory;
    private DirectoryInfo _templateDirectory;
    private DirectoryInfo _updateDirectory;
    private DirectoryInfo _languagesDirectory;

    private FileInfo _appConfigFile;

    public SetupInfo() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      _binDirectory = new DirectoryInfo(new FileInfo(assembly.Location).Directory.FullName);

      AppStructure appStructure = new AppStructure(_binDirectory);

      _appDirectory = appStructure.AppRootDirectory;
      _languagesDirectory = appStructure.LanguagesDirectory;
      _optionsDirectory = appStructure.OptionsDirectory;

      _appConfigFile = new FileInfo(Path.Combine(appStructure.OptionsDirectory.FullName,  Path.GetFileNameWithoutExtension(assembly.Location) + ".xml"));

      _dataDirectory = new DirectoryInfo(Path.Combine(_appDirectory.FullName, "data"));
      _templateDirectory = new DirectoryInfo(Path.Combine(_dataDirectory.FullName, "template"));
      _updateDirectory = new DirectoryInfo(Path.Combine(_dataDirectory.FullName, "update"));

      _dataDirectory.Create();
      _templateDirectory.Create();
      _optionsDirectory.Create();
      _updateDirectory.Create();
    }

    #region public FileInfo AppConfigFile
    public FileInfo AppConfigFile {
      get { return _appConfigFile; }
    }
    #endregion

    #region public DirectoryInfo AppDirectory
    public DirectoryInfo AppDirectory {
      get { return _appDirectory; }
    }
    #endregion

    #region public DirectoryInfo BinDirectory
    public DirectoryInfo BinDirectory {
      get { return _binDirectory; }
      set { _binDirectory = value; }
    }
    #endregion

    #region public DirectoryInfo ProjectsDirectory
    public DirectoryInfo ProjectsDirectory {
      get {
        string def = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Gordago LiteUpdate Develop\\Projects");
        string dirFullName = Global.Properties["Project"].GetValue<string>("Directory", "");
        if (dirFullName == "" || !Directory.Exists(dirFullName)) {
          dirFullName = def;
        }
        DirectoryInfo dir = new DirectoryInfo(dirFullName);
        dir.Create();
        return dir;
      }
      set {
        Global.Properties["Project"].SetValue<string>("Directory", value.FullName);
      }
    }
    #endregion

    #region public DirectoryInfo DataDirectory
    public DirectoryInfo DataDirectory {
      get { return _dataDirectory; }
      set { _dataDirectory = value; }
    }
    #endregion

    #region public DirectoryInfo TemplateDirectory
    public DirectoryInfo TemplateDirectory {
      get { return _templateDirectory; }
    }
    #endregion

    #region public DirectoryInfo OptionsDirectory
    public DirectoryInfo OptionsDirectory {
      get { return _optionsDirectory; }
      set { _optionsDirectory = value; }
    }
    #endregion

    #region public DirectoryInfo UpdateDirectory
    public DirectoryInfo UpdateDirectory {
      get { return _updateDirectory; }
      set { _updateDirectory = value; }
    }
    #endregion

    #region public DirectoryInfo LanguagesDirectory
    public DirectoryInfo LanguagesDirectory {
      get { return _languagesDirectory; }
    }
    #endregion
  }
}
