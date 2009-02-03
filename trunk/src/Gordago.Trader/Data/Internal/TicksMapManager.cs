/**
* @version $Id: TicksMapManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  class TicksMapManager {

    private readonly TicksManager _owner;
    private TicksFileMapData _history, _cache;
    private readonly object _locked = new object();
    private int _sessionId = 0;

    public TicksMapManager(TicksManager owner) {
      _owner = owner;
    }

    #region public TicksManager Owner
    public TicksManager Owner {
      get { return this._owner; }
    }
    #endregion

    #region public TicksFileMap History
    public TicksFileMapData History {
      get { return this._history; }
      set {
        if (_history != null)
          throw (new Exception("The file of history is initialized"));
        this._history = value; 
      }
    }
    #endregion

    #region public TicksFileMap Cache
    public TicksFileMapData Cache {
      get { return this._cache; }
      set {
        if (_cache != null)
          throw (new Exception("The file of cache is initialized"));

        this._cache = value; 
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

    #region public bool IsHistoryCaching
    public bool IsHistoryCaching {
      get {
        return this.Owner.History.Count == this.HistoryCountTick;
      }
    }
    #endregion

    #region public bool IsCacheCaching
    public bool IsCacheCaching {
      get { return this.Owner.Cache.Count == this.CacheCountTick; }
    }
    #endregion

    #region public void SetNewData(bool isHistory, TicksFileMap newTicksFileMap)
    public void SetNewData(bool isHistory, TicksFileMapData newTicksFileMap) {
      lock (_locked) {
        TicksFileMapData tfm = isHistory ? this.History : this.Cache;
        if (tfm != null) {
          tfm.CloseStream();
          tfm.File.Delete();
        }

        string fn = newTicksFileMap.File.FullName;
        fn = fn.Substring(0, fn.Length - 4);
        newTicksFileMap.File.MoveTo(fn);

        if (isHistory) {
          _history = new TicksFileMapData(new FileInfo(fn));
        } else {
          _cache = new TicksFileMapData(new FileInfo(fn));
        }
        _sessionId++;
      }
    }
    #endregion

    //public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick) {

    //  fromdtm = new DateTime(fromdtm.Year, fromdtm.Month, fromdtm.Day, 0, 0, 0);
    //  todtm = new DateTime(todtm.Year, todtm.Month, todtm.Day, 0, 0, 0, 0);

    //  int counttick = 0;
    //  bool begin = false;

    //  for (int i = 0; i < this.MapHistory.Count; i++) {

    //    DateTime dtm = new DateTime(this.MapHistory[i].Time);
    //    dtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0, 0, 0, 0);

    //    if (!begin && dtm == fromdtm)
    //      begin = true;
    //    if (begin && dtm > todtm)
    //      break;
    //    if (begin)
    //      counttick += this.MapHistory[i].CountTick;
    //  }

    //  begin = false;

    //  for (int i = 0; i < this.MapCache.Count; i++) {

    //    DateTime dtm = new DateTime(this.MapCache[i].Time);
    //    dtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0, 0, 0, 0);

    //    if (!begin && dtm == fromdtm)
    //      begin = true;
    //    if (begin && dtm > todtm)
    //      break;
    //    if (begin)
    //      counttick += this.MapCache[i].CountTick;
    //  }
    //  if (counttick == 0) return false;

    //  return cnttick == counttick;
    //}
  }
}
