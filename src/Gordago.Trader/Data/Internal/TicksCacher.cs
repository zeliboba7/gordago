/**
* @version $Id: TicksCacher.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Gordago.Trader.Data {
  class TicksCacher {

    private static readonly string BARS_DIR = "timeframe";

    private readonly TicksFileData _ticksFileData;
    private readonly object _locked = new object();

    private readonly List<BarsFileData> _listBarsFileData = new List<BarsFileData>();
    private readonly DirectoryInfo _dirBars;

    public TicksCacher(TicksFileData ticksFileData, BarsFileData[] barsFiles) {
      _listBarsFileData = new List<BarsFileData>(barsFiles);
      _ticksFileData = ticksFileData;
      string dir = _ticksFileData.File.Directory.FullName + "\\" + BARS_DIR;
      _dirBars = new DirectoryInfo(dir);
      if (!_dirBars.Exists)
        _dirBars.Create();
    }

    #region public TicksFileData TicksFileData
    public TicksFileData TicksFileData {
      get { return _ticksFileData; }
    }
    #endregion
  }
}
