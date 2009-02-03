/**
* @version $Id: SetupInfo.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.AppStructureEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Reflection;

  class SetupInfo {
    private readonly DirectoryInfo _binDirectory;
    private readonly FileInfo _appConfigFile;

    public SetupInfo() {
      Assembly assembly = Assembly.GetExecutingAssembly();
      _binDirectory = new DirectoryInfo(new FileInfo(assembly.Location).Directory.FullName);

      _appConfigFile = new FileInfo(assembly.Location + ".xml");
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
  }
}
