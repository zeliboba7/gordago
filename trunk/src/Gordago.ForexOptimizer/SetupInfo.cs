/**
* @version $Id: SetupInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Reflection;
  using Gordago.Core;

  class SetupInfo {
    private readonly DirectoryInfo _appRootDirectory;
    private readonly DirectoryInfo _binDirectory;
    private readonly DirectoryInfo _languagesDirectory;
    private readonly DirectoryInfo _optionsDirectory;
    private readonly FileInfo _appConfigFile;
    private readonly DirectoryInfo _dataDirectory;
    private readonly DirectoryInfo _templateDirectory;
    private readonly DirectoryInfo _updateDirectory;
    private readonly DirectoryInfo _historyDirectory;

    private readonly DirectoryInfo _indicatorsDirectory;
    private readonly DirectoryInfo _strategyDirectory;
    private readonly DirectoryInfo _instrumentsDirectory;

    public SetupInfo() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      _binDirectory = new DirectoryInfo(new FileInfo(assembly.Location).Directory.FullName);

      AppStructure appStructure = new AppStructure(_binDirectory);
      _appRootDirectory = appStructure.AppRootDirectory;
      _languagesDirectory = appStructure.LanguagesDirectory;
      _languagesDirectory.Create();
      _optionsDirectory = appStructure.OptionsDirectory;
      _optionsDirectory.Create();

      _appConfigFile = new FileInfo(Path.Combine(appStructure.OptionsDirectory.FullName, Path.GetFileNameWithoutExtension(assembly.Location) + ".xml"));

      _dataDirectory = new DirectoryInfo(Path.Combine(_appRootDirectory.FullName, "data"));
      _templateDirectory = new DirectoryInfo(Path.Combine(_dataDirectory.FullName, "template"));
      _updateDirectory = new DirectoryInfo(Path.Combine(_dataDirectory.FullName, "update"));
      _historyDirectory = new DirectoryInfo(Path.Combine(_dataDirectory.FullName, "history"));
      _instrumentsDirectory = new DirectoryInfo(Path.Combine(_appRootDirectory.FullName, "Instruments"));
      _indicatorsDirectory = new DirectoryInfo(Path.Combine(_instrumentsDirectory.FullName, "Indicators"));
      _strategyDirectory = new DirectoryInfo(Path.Combine(_instrumentsDirectory.FullName, "Strategy"));

      _historyDirectory.Create();
      _dataDirectory.Create();
      _templateDirectory.Create();
      _updateDirectory.Create();
      _strategyDirectory.Create();
      _indicatorsDirectory.Create();
    }

    #region public FileInfo AppConfigFile
    public FileInfo AppConfigFile {
      get { return _appConfigFile; }
    }
    #endregion

    #region public DirectoryInfo AppRootDirectory
    public DirectoryInfo AppRootDirectory {
      get { return _appRootDirectory; }
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
    }
    #endregion

    #region public DirectoryInfo DataDirectory
    public DirectoryInfo DataDirectory {
      get { return _dataDirectory; }
    }
    #endregion

    #region public DirectoryInfo TemplateDirectory
    public DirectoryInfo TemplateDirectory {
      get { return _templateDirectory; }
    }
    #endregion

    #region public DirectoryInfo UpdateDirectory
    public DirectoryInfo UpdateDirectory {
      get { return _updateDirectory; }
    }
    #endregion

    #region public DirectoryInfo HistoryDirectory
    public DirectoryInfo HistoryDirectory {
      get { return _historyDirectory; }
    }
    #endregion

    #region public DirectoryInfo InstrumentsDirectory
    public DirectoryInfo InstrumentsDirectory {
      get { return _instrumentsDirectory; }
    }
    #endregion

    #region public DirectoryInfo IndicatorsDirectory
    public DirectoryInfo IndicatorsDirectory {
      get { return _indicatorsDirectory; }
    }
    #endregion

    #region public DirectoryInfo StrategyDirectory
    public DirectoryInfo StrategyDirectory {
      get { return _strategyDirectory; }
    }
    #endregion
  }
}
