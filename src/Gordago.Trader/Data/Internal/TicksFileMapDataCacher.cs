/**
* @version $Id: TicksFileMapDataCacher.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  class TicksFileMapDataCacher {

    private const int TIME_FRAME_SECOND = 86400;

    private TicksFileMapData _currentBFD, _newBFD;
    private int _countTicks = 0;
    private readonly TicksManager _ticksManager;
    private readonly DirectoryInfo _directory;

    private readonly string _symbolName;
    
    private TicksMapItem _mapItem;
    private bool _isHistory;
    private int _countMapItem = 0;

    public TicksFileMapDataCacher(TicksManager ticksManager,
                              bool isHistory,
                              TicksFileMapData currentBFD) {
      _ticksManager = ticksManager;
      _currentBFD = currentBFD;
      _symbolName = ticksManager.Name;
      _isHistory = isHistory;
      _directory = isHistory ? ticksManager.DirectoryMapsHistory : ticksManager.DirectoryMapsCache;

      string fn = string.Format(@"{0}\{1}.gmp.tmp", _directory, _symbolName);
      FileInfo file = new FileInfo(fn);
      if (file.Exists)
        file.Delete();
      _newBFD = new TicksFileMapData(file);
    }

    #region public void Add(Tick tick)
    public void Add(Tick tick) {
      _countTicks++;
      bool flagNewMapItem = false;

      if (_countMapItem == 0) {
        flagNewMapItem = true;
      } else {
        long sec1 = tick.Time / 10000000L;
        long sec2 = _mapItem.Time / 10000000L;
        flagNewMapItem = sec1 / TIME_FRAME_SECOND - sec2 / TIME_FRAME_SECOND > 0;
      }

      if (flagNewMapItem) {
        this.SaveCurrentBar();

        _mapItem = new TicksMapItem(tick.Time, 1);
         _countMapItem++;
      } else {
        _mapItem.CountTick++;
      }
    }
    #endregion

    #region private void SaveCurrentBar()
    private void SaveCurrentBar() {
      if (_countMapItem == 0)
        return;
      this._newBFD.Write(_mapItem, _countMapItem - 1);
    }
    #endregion

    #region public void Complete()
    public void Complete() {
      this.SaveCurrentBar();
      _newBFD.SymbolName = _ticksManager.Name;
      _newBFD.CountTicks = _countTicks;
      _newBFD.SaveFileInfo();
      _newBFD.CloseStream();

      this._ticksManager.Map.SetNewData(_isHistory, _newBFD);
    }
    #endregion

    #region public void Abort()
    public void Abort() {
      _newBFD.CloseStream();
      _newBFD.File.Delete();
    }
    #endregion
  }
}
