/**
* @version $Id: BarsFileDataCacher.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  
  class BarsFileDataCacher {

    private BarsFileData _currentBarsFileData;
    private BarsFileData _newBarsFileData;
    private int _countTicks = 0;
    private readonly TimeFrame _timeFrame;
    private readonly TicksManager _ticksManager;
    private readonly DirectoryInfo _directory;
    private readonly BarManager _bm;
    private readonly string _symbolName;
    private readonly int _digits;
    
    private int _countBar = 0;
    private Bar _bar;
    private int _tfsec;
    private bool _isHistory;

    public BarsFileDataCacher(TicksManager ticksManager, BarManager bm,
                              bool isHistory,
                              BarsFileData currentBFD, 
                              TimeFrame timeFrame) {
      _bm = bm;
      _ticksManager = ticksManager;
      _currentBarsFileData = currentBFD;
      _timeFrame = timeFrame;
      _symbolName = ticksManager.Name;
      _digits = ticksManager.Digits;
      _tfsec = timeFrame.Second;
      _isHistory = isHistory;
      _directory = isHistory ? ticksManager.DirectoryBarsHistory : ticksManager.DirectoryBarsCache;

      string fn = string.Format(@"{0}\{1}{2}.gtf.tmp", _directory, _symbolName, _timeFrame.Second);
      FileInfo file = new FileInfo(fn);
      if (file.Exists)
        file.Delete();
      _newBarsFileData = new BarsFileData(file);
    }

    #region public void Add(Tick tick)
    public void Add(Tick tick) {
      _countTicks++;
      bool flagnewbar = false;
      long time = 0;

      if (_countBar == 0) {
        flagnewbar = true;
      } else {
        if (this._timeFrame.Calculator != null) {
          flagnewbar = this._timeFrame.Calculator.CheckNewBar(_bar, new DateTime(tick.Time));
        } else {
          long sec1 = tick.Time / 10000000L / _tfsec;
          long sec2 = _bar.Time.Ticks / 10000000L / _tfsec;
          flagnewbar = sec1 - sec2 > 0;
        }
      }

      if (flagnewbar)
        time = this._timeFrame.Calculator != null ? this._timeFrame.Calculator.GetRoundTime(new DateTime(tick.Time)).Ticks : this.GetRoundTime(tick.Time);

      if (flagnewbar) {
        this.SaveCurrentBar();

        Bar bar = new Bar(tick.Price, tick.Price, tick.Price, tick.Price, 1, time);
        _bar = bar;
         _countBar++;
      } else {
        float price = tick.Price;
        _bar.Close = price;
        _bar.Volume++;

        if (_bar.Low > price)
          _bar.Low = price;
        if (_bar.High < price)
          _bar.High = price;
      }
    }
    #endregion

    #region private void SaveCurrentBar()
    private void SaveCurrentBar() {
      if (_countBar == 0)
        return;
      this._newBarsFileData.Write(_bar, _countBar - 1);
    }
    #endregion

    #region private long GetRoundTime(long time)
    private long GetRoundTime(long time) {
      long nt = (time / 10000000L) / _tfsec;
      return (nt * _tfsec) * 10000000L;
    }
    #endregion

    #region public void Complete()
    public void Complete() {
      this.SaveCurrentBar();
      _newBarsFileData.SymbolName = _ticksManager.Name;
      _newBarsFileData.Digits = _ticksManager.Digits;
      _newBarsFileData.CountTicks = _countTicks;
      _newBarsFileData.TimeFrameSecond = _timeFrame.Second;
      _newBarsFileData.SaveFileInfo();
      _newBarsFileData.CloseStream();

      _bm.SetNewData(_isHistory, _newBarsFileData);
    }
    #endregion

    #region public void Abort()
    public void Abort() {
      _newBarsFileData.CloseStream();
      _newBarsFileData.File.Delete();
    }
    #endregion
  }
}
