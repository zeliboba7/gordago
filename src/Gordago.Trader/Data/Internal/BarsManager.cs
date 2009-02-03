/**
* @version $Id: BarsManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Diagnostics;
  using Gordago.Core;

  class BarsManager : IEnumerable<BarManager>, IBarsDataCollection, ISession {

    private readonly Dictionary<int, BarManager> _bars = new Dictionary<int, BarManager>();
    private readonly TicksManager _owner;
    private readonly SessionSource _session;

    public BarsManager(TicksManager owner) {
      _owner = owner;
      _session = owner.Session;
      foreach (TimeFrame timeFrame in TimeFrameManager.TimeFrames) {
        BarManager barManager = new BarManager(this, timeFrame);
        _bars.Add(timeFrame.Second, barManager);
      }
    }

    #region public SessionSource Session
    public SessionSource Session {
      get { return _session; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return this._bars.Count; }
    }
    #endregion

    #region public IBarsData this[TimeFrame tf]
    public IBarsData this[TimeFrame tf] {
      get {
        return this[tf.Second];
      }
    }
    #endregion

    #region public IBarsData this[int period]
    public IBarsData this[int period] {
      get {
        BarManager bars = null;
        _bars.TryGetValue(period, out bars);
        return bars;
      }
    }
    #endregion

    #region public TicksManager Owner
    public TicksManager Owner {
      get { return this._owner; }
    }
    #endregion

    #region public BarManager[] GetBadDataBarManager()
    public BarManager[] GetBadDataBarManager() {
      List<BarManager> bms = new List<BarManager>();

      foreach (BarManager bm in _bars.Values) {
        bool hst = bm.IsHistoryCaching;
        string mhst = bm.TraceMessage;

        bool cache = bm.IsCacheCaching;
        string mcache = bm.TraceMessage;

        if (!hst || !cache) {
          bms.Add(bm);
          Trace.WriteLine(string.Format(
            "File of bars data is bad: Symbol={0}, TF={1}, history={2}({3}), cache={4}({5})",
            this._owner.Name, 
            bm.TimeFrame, 
            !hst, mhst,
            !cache, mcache));
        }
      }
      return bms.ToArray();
    }
    #endregion

    #region public IEnumerator<BarManager> GetEnumerator()
    public IEnumerator<BarManager> GetEnumerator() {
      return this._bars.Values.GetEnumerator();
    }
    #endregion

    #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return null;
    }
    #endregion

  }
}
