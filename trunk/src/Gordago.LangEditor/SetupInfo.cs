/**
* @version $Id: SetupInfo.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Reflection;
  using System.IO;
  using Gordago.Core;

  class SetupInfo {
    private DirectoryInfo _binDirectory;
    private DirectoryInfo _languagesDirectory;
    private DirectoryInfo _optionsDirectory;
    private FileInfo _appConfigFile;

    public SetupInfo() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      _binDirectory = new DirectoryInfo(new FileInfo(assembly.Location).Directory.FullName);

      AppStructure appStructure = new AppStructure(_binDirectory);
      _languagesDirectory = appStructure.LanguagesDirectory;
      _languagesDirectory.Create();
      _optionsDirectory = appStructure.OptionsDirectory;
      _optionsDirectory.Create();

      _appConfigFile = new FileInfo(Path.Combine(appStructure.OptionsDirectory.FullName, Path.GetFileNameWithoutExtension(assembly.Location) + ".xml"));
    }

    #region public FileInfo AppConfigFile
    public FileInfo AppConfigFile {
      get { return _appConfigFile; }
    }
    #endregion

    #region public DirectoryInfo BinDirectory
    public DirectoryInfo BinDirectory {
      get { return _binDirectory; }
    }
    #endregion

    #region public DirectoryInfo LanguagesDirectory
    public DirectoryInfo LanguagesDirectory {
      get { return _languagesDirectory; }
    }
    #endregion

    #region public DirectoryInfo OptionsDirectory
    public DirectoryInfo OptionsDirectory {
      get { return _optionsDirectory; }
      set { _optionsDirectory = value; }
    }
    #endregion

  }
}
