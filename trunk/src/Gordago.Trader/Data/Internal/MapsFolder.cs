/**
* @version $Id: MapsFolder.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  class MapsFolder {
    private readonly DirectoryInfo _dir;
    private static readonly string MAPS_DIR = "map";

    private readonly Dictionary<FileKey, TicksFileMapData> _files = new Dictionary<FileKey, TicksFileMapData>();

    public MapsFolder(DirectoryInfo dirTicksFiles) {
      _dir = new DirectoryInfo(dirTicksFiles.FullName + "\\" + MAPS_DIR);
      if (!_dir.Exists)
        _dir.Create();

      FileInfo[] files = _dir.GetFiles("*.gmp");
      foreach (FileInfo file in files) {
        TicksFileMapData ticksFileMapData = new TicksFileMapData(file);
        _files.Add(new FileKey(file), ticksFileMapData);
      }
    }

    #region public DirectoryInfo Directory
    public DirectoryInfo Directory {
      get { return this._dir; }
    }
    #endregion

    #region public TicksFileMapData[] Pop(string symbolName)
    public TicksFileMapData[] Pop(string symbolName) {
      List<TicksFileMapData> list = new List<TicksFileMapData>();

      SymbolKey akey = new SymbolKey(symbolName);

      foreach (TicksFileMapData fileData in _files.Values) {
        SymbolKey bkey = new SymbolKey(fileData.SymbolName);
        if (akey.Equals(bkey)) {
          list.Add(fileData);
        }
      }

      foreach (TicksFileMapData fileData in list) {
        _files.Remove(new FileKey(fileData.File));
      }
      return list.ToArray();
    }
    #endregion
  }
}
