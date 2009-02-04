/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {

  class ViTickList:ITickList, ITickManager {

    private ITickList _ticks;
    // private DateTime _currentTime;
    private int _currentIndex;
    private List<ViBars> _barsList;

    private OnlineRate _onlineRate;
    private long _lastUpdateTime = DateTime.Now.Ticks;

    private IBarList _movedBars = null;
    private VirtualBroker _broker;
    private long _movedBarsTimeStep = 0;

    public ViTickList(VirtualBroker broker, ITickList ticks, DateTime startTime) {
      _broker = broker;
      _ticks = ticks;

      if (broker.Settings.UseTimeFrame != null) {
        int second = broker.Settings.UseTimeFrame.Second;
        _movedBarsTimeStep = second * 10000000L;
        _movedBars = ticks.GetBarList(second);
        _currentIndex = Math.Max(_movedBars.GetBarIndex(startTime), 0);
      } else {
        _currentIndex = (_ticks as ITickManager).GetPosition(broker.Time);
      }

      IBarList[] barsList = ticks.BarLists;
      _barsList = new List<ViBars>();
    }

    #region public IBarList MovedBars
    public IBarList MovedBars {
      get { return _movedBars; }
    }
    #endregion

    #region public ITickList FiTicks
    public ITickList FiTicks {
      get { return _ticks; }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        if (_movedBars != null)
          return Math.Max(Math.Min(_currentIndex, _movedBars.Count), 0);
        return Math.Max(Math.Min(_currentIndex, _ticks.Count), 0);
      }
    }
    #endregion

    #region public bool EndOf
    public bool EndOf {
      get { 
        if (_movedBars != null)
          return this._currentIndex >= _movedBars.Count; 
        return this._currentIndex >= _ticks.Count; 
      }
    }
    #endregion

    #region public Tick this[int index]
    public Tick this[int index] {
      get {
        if (_movedBars != null)
          return new Tick(_movedBars[index].Time.Ticks, _movedBars[index].Close);

        return _ticks[index]; 
      }
    }
    #endregion

    #region public Tick Current
    public Tick Current {
      get {
        return this[this.Count - 1];
      }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { 
        //if (_movedBars != null)
        return _ticks.TimeFrom; 
      }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { return new DateTime(this.Current.Time); }
    }
    #endregion

    #region public IBarList[] BarLists
    public IBarList[] BarLists {
      get { return _barsList.ToArray(); }
    }
    #endregion

    #region public void SetOnlineRate(OnlineRate onlineRate)
    public void SetOnlineRate(OnlineRate onlineRate) {
      _onlineRate = onlineRate;
    }
    #endregion

    #region public void MoveNext(DateTime time)
    /// <summary>
    /// Новое время, сдвиг
    /// </summary>
    /// <param name="time"></param>
    public void MoveNext(VirtualBroker broker, DateTime time) {

      if (_movedBars != null) {

        #region init
        int barsCount = _movedBars.Count;
        if (barsCount == 0)
          return;

        if (_currentIndex < 0) {
          if (time.Ticks > _movedBars[0].Time.Ticks)
            _currentIndex = 0;
          else
            return;
        }
        #endregion

        for (int i = _currentIndex; i < barsCount; i++) {
          Bar bar = _movedBars[i];
          Tick tick = new Tick(bar.Time.Ticks + _movedBarsTimeStep - 9000000L, bar.Close);
          if (tick.Time >= time.Ticks) {
            break;
          } else {
            _onlineRate.SetRate(tick);
            _currentIndex++;
            for (int j = 0; j < _barsList.Count; j++)
              _barsList[j].AddTick(tick);

            broker.OnlineRateChanged(_onlineRate);

            if (broker.Strategy == null) broker.OnlineRateChangedEvent(_onlineRate);
            else broker.Strategy.OnOnlineRateChanged(_onlineRate);
          }
        }
      } else {

        #region init
        int ticksCount = _ticks.Count;
        if (ticksCount == 0)
          return;

        if (_currentIndex < 0) {
          if (time.Ticks > _ticks[0].Time)
            _currentIndex = 0;
          else
            return;
        }
        #endregion

        for (int i = _currentIndex; i < ticksCount; i++) {
          Tick tick = _ticks[i];
          if (tick.Time > time.Ticks) {
            break;
          } else {
            _onlineRate.SetRate(tick);
            _currentIndex++;
            for (int j = 0; j < _barsList.Count; j++)
              _barsList[j].AddTick(tick);

            broker.OnlineRateChanged(_onlineRate);

            if (broker.Strategy == null) broker.OnlineRateChangedEvent(_onlineRate);
            else broker.Strategy.OnOnlineRateChanged(_onlineRate);
          }
        }
      }
    }
    #endregion

    #region public IBarList GetBarList(int second)
    public IBarList GetBarList(int second) {
      for (int i = 0; i < _barsList.Count; i++) {
        if (_barsList[i].TimeFrame.Second == second)
          return _barsList[i];
      }

      IBarList bars = _ticks.GetBarList(second);
      if (bars == null) return null;

      ViBars viBars = new ViBars(_broker, this, bars, _broker.Time);
      _barsList.Add(viBars);
      return viBars;
    }
    #endregion

    #region public void Add(Tick tick)
    public void Add(Tick tick) {
      throw new Exception("The method or operation is not implemented.");
    }
    #endregion

    #region public TickManagerStatus Status
    public TickManagerStatus Status {
      get { return TickManagerStatus.Default; }
    }
    #endregion

    //#region public bool UseDataCachingEvents
    //public bool UseDataCachingEvents {
    //  get {
    //    return (_ticks as ITickManager).UseDataCachingEvents;
    //  }
    //  set {
    //    (_ticks as ITickManager).UseDataCachingEvents = value;
    //  }
    //}
    //#endregion

    #region public TickFileInfo InfoHistory
    public TickFileInfo InfoHistory {
      get { return (_ticks as ITickManager).InfoHistory; }
    }
    #endregion

    #region public TickFileInfo InfoCache
    public TickFileInfo InfoCache {
      get { return (_ticks as ITickManager).InfoCache; }
    }
    #endregion

    #region public int GetPositionFromMap(DateTime fdtm)
    public int GetPositionFromMap(DateTime fdtm) {
      return (_ticks as ITickManager).GetPositionFromMap(fdtm);
    }
    #endregion

    #region public int GetPosition(DateTime fdtm)
    public int GetPosition(DateTime fdtm) {
      return (_ticks as ITickManager).GetPosition(fdtm);
    }
    #endregion

    #region Empty Method
    #region public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick)
    public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick) {
      // return (_ticks as ITickManager).CheckPeriodInMap(fromdtm, todtm, cnttick);
      return true;
    }
    #endregion

    #region public void DataCachingMethod()
    public void DataCachingMethod() {
      // (_ticks as ITickManager).DataCachingMethod();
    }
    #endregion

    #region public void Update(TickCollection ticks)
    public void Update(TickCollection ticks) {

    }
    #endregion

    #region public void Update(TickCollection ticks, bool isCacheBuffer)
    public void Update(TickCollection ticks, bool isCacheBuffer) {

    }
    #endregion

    #region public void Update(TickFileInfo tfi)
    public void Update(TickFileInfo tfi) {

    }
    #endregion

    #region public void Update(TickFileInfo tfi, bool isCacheBuffer)
    public void Update(TickFileInfo tfi, bool isCacheBuffer) {

    }
    #endregion

    #endregion


    public event TickManagerEventHandler DataCachingChanged;

    public bool IsDataCaching {
      get { return true; }
    }

    public void DataCaching() {
    }
  }
}
