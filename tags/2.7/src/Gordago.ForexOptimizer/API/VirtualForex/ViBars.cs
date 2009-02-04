/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {

  class ViBars:IBarList {

    private IBarList _bars;
    private DateTime _startTime;
    private int _count = 0;
    private Bar _lastBar = new Bar();
    private ViTickList _ticks;

    private int _savedCountTicks;
    private int _deltaIndex = 0;

    public ViBars(VirtualBroker broker, ViTickList ticks, IBarList bars, DateTime startTime) {
      _ticks = ticks;
      _bars = bars;
      _startTime = startTime;

      if (broker.Settings.UseTimeFrame != null) {
        int barIndex = _bars.GetBarIndex(_startTime);
        int startTickIndex = 0;

        if (barIndex > _bars.Count)
          barIndex = _bars.Count - 1;
        if (barIndex < 0)
          barIndex = 0;

        _count = barIndex + 1;
        _lastBar = _bars[barIndex];

      } else {
        int barIndex = _bars.GetBarIndex(_startTime);
        int startTickIndex = 0;

        if (barIndex > 0 && barIndex < _bars.Count) {
          _count = barIndex;
          Bar bar = _bars[barIndex];
          ITickManager tm = ticks.FiTicks as ITickManager;
          startTickIndex = tm.GetPosition(bar.Time);

        } else if (barIndex > 0 && barIndex >= _bars.Count) {
          _count = _bars.Count;
          startTickIndex = ticks.FiTicks.Count;
        }

        for (int i = startTickIndex; i < ticks.Count; i++) {
          this.AddTick(ticks[i]);
        }
      }

      if (broker.FromTime > 0) {
        _deltaIndex = _bars.GetBarIndex(new DateTime(broker.FromTime));
        _deltaIndex = Math.Max(Math.Min(_deltaIndex, _count - 1), 0);
        if (_deltaIndex > 1000) {
          _deltaIndex = _deltaIndex - 1000;
        }
      }

      _savedCountTicks = ticks.Count;
    }

    #region public int Count
    public int Count {
      get {
        return _count-_deltaIndex; 
      }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
        index = index + _deltaIndex;

        if (index == _count - 1)
          return _lastBar;
        else if (index>=_count)
          throw(new Exception("Index out of range"));
        return _bars[index];
      }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get { return this[this.Count-1]; }
    }
    #endregion

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return _bars.TimeFrame; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { return this[0].Time; }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { return this.Current.Time; }
    }
    #endregion

    #region public int GetBarIndex(DateTime time)
    public int GetBarIndex(DateTime time) {
      return _bars.GetBarIndex(time)-_deltaIndex;
    }
    #endregion

    #region private DateTime GetRoundTime(DateTime time)
    private DateTime GetRoundTime(DateTime time) {
      int second = this.TimeFrame.Second;
      long nt = (time.Ticks / 10000000L) / second;
      return new DateTime((nt * second) * 10000000L);
    }
    #endregion

    #region public void AddTick(Tick tick)
    public void AddTick(Tick tick) {
      bool newbar = false;
      DateTime time = DateTime.Now;

      if (!(_lastBar.Close > 0)) {
        if (_bars.Count > 0) {
          _lastBar = _bars[0];
          _lastBar.Time = _lastBar.Time.AddDays(-10000);
        }
      }

      if (this.TimeFrame.Calculator != null) {
        newbar = this.TimeFrame.Calculator.CheckNewBar(this._lastBar, new DateTime(tick.Time));
      } else {
        long sec1 = tick.Time / 10000000L / this.TimeFrame.Second;
        long sec2 = _lastBar.Time.Ticks / 10000000L / this.TimeFrame.Second;
        newbar = sec1 - sec2 > 0;
      }

      if (newbar) {
        /* проверка */
        //if (_count > 3) {

        //  Bar barReal = _bars[_count - 1];
        //  // Bar barFix = this[_count - 2];
        //  bool check = Check(barReal, _lastBar);
        //  if (!check)
        //    System.Diagnostics.Debug.WriteLine("Error");
        //}
        //if (this.TimeFrame.Second == 3600) {
        //  System.Diagnostics.Debug.WriteLine(_count + " Real " + _bars[_count-1].ToString(true));
        //  System.Diagnostics.Debug.WriteLine(_count + " Virt " + _lastBar.ToString(true));
        //}
        time = this.TimeFrame.Calculator != null ? this.TimeFrame.Calculator.GetRoundTime(new DateTime(tick.Time)) : this.GetRoundTime(new DateTime(tick.Time));
        _lastBar = new Bar(tick.Price, tick.Price, tick.Price, tick.Price, 1, time);
        _count++;
      } else {
        _lastBar.Volume += 1;
        if (_lastBar.Low > tick.Price)
          _lastBar.Low = tick.Price;
        if (_lastBar.High < tick.Price)
          _lastBar.High = tick.Price;
        _lastBar.Close = tick.Price;
      }
      
      /* проверка */
      //if (_count < 3) return;

      //Bar barReal = _bars[_count - 2];
      //Bar barFix = this[_count - 2];
      //bool check = Check(barReal, barFix);
    }
    #endregion

    #region private bool Check(Bar barReal, Bar barVirt)
    private bool Check(Bar barReal, Bar barVirt) {
      if (barReal.Close != barVirt.Close ||
        barReal.Open != barVirt.Open ||
        barReal.High != barVirt.High ||
        barReal.Low != barVirt.Low) {
        return false;
      }
      return true;
    }
    #endregion

#if DEBUG
    //public int RealIndex(int index) {
    //  return _bars.RealIndex(index + _deltaIndex);
    //}
#endif
  }
}
