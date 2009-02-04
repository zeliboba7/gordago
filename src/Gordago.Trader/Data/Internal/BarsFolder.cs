/**
* @version $Id: BarsFolder.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  class BarsFolder {

    private readonly DirectoryInfo _dir;
    private static readonly string BARS_DIR = "timeframe";

    private readonly Dictionary<FileKey, BarsFileData> _files = new Dictionary<FileKey, BarsFileData>();

    public BarsFolder(DirectoryInfo dirTicksFiles) {
      _dir = new DirectoryInfo(dirTicksFiles.FullName + "\\" + BARS_DIR);
      if (!_dir.Exists)
        _dir.Create();

      FileInfo[] files = _dir.GetFiles("*.gtf");
      foreach (FileInfo file in files) {
        BarsFileData barsFileData = new BarsFileData(file);
        _files.Add(new FileKey(file), barsFileData);
      }
    }

    #region public DirectoryInfo Directory
    public DirectoryInfo Directory {
      get { return this._dir; }
    }
    #endregion

    #region public BarsFileData[] Pop(string symbolName)
    public BarsFileData[] Pop(string symbolName) {
      List<BarsFileData> list = new List<BarsFileData>();

      SymbolKey akey = new SymbolKey(symbolName);

      foreach (BarsFileData barsFileData in _files.Values) {
        SymbolKey bkey = new SymbolKey(barsFileData.SymbolName);
        if (akey.Equals(bkey)) {
          list.Add(barsFileData);
        }
      }

      foreach (BarsFileData barsFileData in list) {
        _files.Remove(new FileKey(barsFileData.File));
      }
      return list.ToArray();
    }
    #endregion

    #region public void DeleteEmpty()
    public void DeleteEmpty() {
      foreach (BarsFileData barsFileData in _files.Values) {
        FileInfo file = barsFileData.File;
        barsFileData.CloseStream();
        file.Delete();
      }
      _files.Clear();
    }
    #endregion
  }
}
