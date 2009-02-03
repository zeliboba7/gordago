/**
* @version $Id: Setup.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.IOTicks
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Reflection;

  class Setup {
    private DirectoryInfo _applicationDirectory;
    private DirectoryInfo _importGainCapitalDirectory;
    private DirectoryInfo _historyDirectory;
    private DirectoryInfo _serverDirectory;

    public Setup() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      _applicationDirectory = new DirectoryInfo(new FileInfo(assembly.Location).Directory.FullName);
      
      _importGainCapitalDirectory = new DirectoryInfo(
        Path.Combine(_applicationDirectory.FullName, "import\\GainCapital"));
      _importGainCapitalDirectory.Create();

      _historyDirectory = new DirectoryInfo(
        Path.Combine(_applicationDirectory.FullName, "data\\history"));
      _historyDirectory.Create();

      _serverDirectory = new DirectoryInfo(
        Path.Combine(_applicationDirectory.FullName, "data\\server"));
      _serverDirectory.Create();
    }

    #region public DirectoryInfo ApplicationDirectory
    public DirectoryInfo ApplicationDirectory {
      get { return _applicationDirectory; }
    }
    #endregion

    #region public DirectoryInfo ImportGainCapitalDirectory
    public DirectoryInfo ImportGainCapitalDirectory {
      get { return _importGainCapitalDirectory; }
    }
    #endregion

    #region public DirectoryInfo HistoryDirectory
    public DirectoryInfo HistoryDirectory {
      get { return _historyDirectory; }
    }
    #endregion

    #region public DirectoryInfo ServerDirectory
    public DirectoryInfo ServerDirectory {
      get { return _serverDirectory; }
    }
    #endregion
  }
}
