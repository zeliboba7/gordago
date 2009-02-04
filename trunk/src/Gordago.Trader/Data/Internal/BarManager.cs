/**
* @version $Id: BarManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  using Gordago.Core;

  class BarManager: IBarsData, ISession {

    private readonly TimeFrame _timeFrame;

    private BarsFileData _history;
    private BarsFileData _cache;
    
    private readonly List<Bar> _online = new List<Bar>();
    private readonly object _locked = new object();

    private int _count;
    private int _countHistory = 0, _countCache = 0;
    private readonly BarsManager _owner;
    private readonly SessionSource _session;
    private readonly SessionDest _savedSession;
    private string _traceMessage = "";

    public BarManager(BarsManager owner, TimeFrame timeFrame) {
      _session = owner.Session;
      _savedSession = new SessionDest(_session);
      _owner = owner;
      _timeFrame = timeFrame;
    }

    #region internal string TraceMessage
    internal string TraceMessage {
      get { return this._traceMessage; }
    }
    #endregion

    #region public SessionSource Session
    public SessionSource Session {
      get { return _session; }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get { return this[this.Count - 1]; }
    }
    #endregion

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return this._timeFrame; }
    }
    #endregion

    #region public BarsFileData History
    public BarsFileData History {
      get {
        return _history;
      }
      set {
        if (_history != null)
          throw (new Exception("The file of history is initialized"));
        this._history = value;
      }
    }
    #endregion

    #region public BarsFileData Cache
    public BarsFileData Cache {
      get { return this._cache; }
      set {
        if (_cache != null)
          throw (new Exception("The file of cache is initialized"));

        this._cache = value;
      }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        lock (_locked) {
          this.CheckSessionId();
          return _count + _online.Count;
        }
      }
    }
    #endregion

    #region private int HistoryCountTick
    private int HistoryCountTick {
      get {
        if (_history == null)
          return 0;
        return _history.CountTicks;
      }
    }
    #endregion

    #region private int CacheCountTick
    private int CacheCountTick {
      get {
        if (_cache == null)
          return 0;
        return _cache.CountTicks;
      }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
         lock (_locked) {
          this.CheckSessionId();
          if (index >= 0 && index < _countHistory && _countHistory > 0) {
            return _history.Read(index);
          } else if (index >= _countHistory && index < (_countHistory + _countCache) && _countCache > 0) {
            return _cache.Read(index - _countHistory);
          } else if (index >= (_countHistory + _countCache) && index < (_countHistory + _countCache + _online.Count) && _online.Count > 0) {
            return _online[index - _countHistory - _countCache];
          } else
            throw (new ArgumentOutOfRangeException("index"));
        }
      }
    }
    #endregion

    #region public BarsManager Owner
    public BarsManager Owner {
      get { return this._owner; }
    }
    #endregion

    #region public bool IsHistoryCaching
    public bool IsHistoryCaching {
      get {
        bool result = this.Owner.Owner.History.Count == this.HistoryCountTick;
        _traceMessage = "";
        if (!result) {
          _traceMessage = string.Format("TicksCnt={0} != BTicksCnt={1}", this.Owner.Owner.History.Count, this.HistoryCountTick);
        }
        return result;
      }
    }
    #endregion

    #region public bool IsCacheCaching
    public bool IsCacheCaching {
      get {
        bool result = this.Owner.Owner.Cache.Count == this.CacheCountTick;
        _traceMessage = "";
        if (!result) {
          _traceMessage = string.Format("TicksCnt={0} != BTicksCnt={1}", this.Owner.Owner.Cache.Count, this.CacheCountTick);
        }
        return result;
      }
    }
    #endregion

    #region private void CheckSessionId()
    private void CheckSessionId() {
      if (_savedSession.CheckSession() == 0)
        return;
      _count = _countHistory = _countCache = 0;
      if (_history != null)
        _countHistory = _history.Count;

      if (_cache != null)
        _count = _cache.Count;

      _count = _countHistory + _countCache;
      _savedSession.Complete();
    }
    #endregion

    #region public void SetNewData(bool isHistory, BarsFileData newBarsFileData)
    public void SetNewData(bool isHistory, BarsFileData newBarsFileData) {
      lock (_locked) {
        BarsFileData bfd = isHistory ? this.History : this.Cache;
        if (bfd != null) {
          bfd.CloseStream();
          bfd.File.Delete();
        }

        string fn = newBarsFileData.File.FullName;
        fn = fn.Substring(0, fn.Length - 4);
        newBarsFileData.File.MoveTo(fn);

        if (isHistory) {
          _history = new BarsFileData(new FileInfo(fn));
          _session.IncrementsLevel1();
        } else {
          _cache = new BarsFileData(new FileInfo(fn));
          _session.IncrementsLevel2();
        }
      }
    }
    #endregion
  }
}
