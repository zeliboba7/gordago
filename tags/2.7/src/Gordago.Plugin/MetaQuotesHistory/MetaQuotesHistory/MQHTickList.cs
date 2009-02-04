using System;
using System.Collections.Generic;
using System.Text;
using Gordago;

namespace Gordago.PlugIn.MetaQuotesHistory {
  class ddd : ITickList, ITickManager {
    #region ITickList Members

    public void Add(Tick tick) {
      throw new Exception("The method or operation is not implemented.");
    }

    public IBarList[] BarLists {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public int Count {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public Tick Current {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public IBarList GetBarList(int second) {
      throw new Exception("The method or operation is not implemented.");
    }

    public DateTime TimeFrom {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public DateTime TimeTo {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public Tick this[int index] {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    #endregion

    #region ITickManager Members

    public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void DataCaching() {
      throw new Exception("The method or operation is not implemented.");
    }

    public event TickManagerEventHandler DataCachingChanged;

    public void DataCachingMethod() {
      throw new Exception("The method or operation is not implemented.");
    }

    public int GetPosition(DateTime fdtm) {
      throw new Exception("The method or operation is not implemented.");
    }

    public int GetPositionFromMap(DateTime fdtm) {
      throw new Exception("The method or operation is not implemented.");
    }

    public TickFileInfo InfoCache {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public TickFileInfo InfoHistory {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public bool IsDataCaching {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public TickManagerStatus Status {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public void Update(TickFileInfo tfi, bool isCacheBuffer) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void Update(TickFileInfo tfi) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void Update(TickCollection ticks, bool isCacheBuffer) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void Update(TickCollection ticks) {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }

  class MQHTickList:ITickList, ITickManager {

    public event TickManagerEventHandler DataCachingChanged;
    private ITickList _gTicks;
    private MQHBarList[] _mtbars;

    public MQHTickList(ITickList gTicks) {
      _gTicks = gTicks;

      _mtbars = new MQHBarList[gTicks.BarLists.Length];
      for(int i = 0; i < _mtbars.Length; i++) {
        _mtbars[i] = new MQHBarList(_gTicks.BarLists[i]);
      }

      //(gTicks as ITickManager).DataCachingProccess += new TickManagerProccessHandler(this._gTicks_DataCachingProccess);
      //(gTicks as ITickManager).DataCachingStopping += new TickManagerHandler(_gTicks_DataCachingStopping);
      (gTicks as ITickManager).DataCachingChanged += new TickManagerEventHandler(MQHTickList_DataCachingChanged);
    }

    #region public void Add(Tick tick)
    public void Add(Tick tick) {
      _gTicks.Add(tick);
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _gTicks.Count; }
    }
    #endregion

    #region public Tick this[int index]
    public Tick this[int index] {
      get { return _gTicks[index]; }
    }
    #endregion

    #region public Tick Current
    public Tick Current {
      get { return _gTicks.Current; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { return _gTicks.TimeFrom; }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { return _gTicks.TimeTo; }
    }
    #endregion

    #region public IBarList[] BarLists
    public IBarList[] BarLists {
      get { return _mtbars; }
    }
    #endregion

    #region public IBarList GetBarList(int second)
    public IBarList GetBarList(int second) {
      for(int i = 0; i < _mtbars.Length; i++) {
        if(_mtbars[i].TimeFrame.Second == second) {
          return _mtbars[i];
        }
      }
      return null;
    }
    #endregion

    #region private void _gTicks_DataCachingProccess(int current, int total)
    private void _gTicks_DataCachingProccess(int current, int total) {
      //if(this.DataCachingProccess != null)
      //  this.DataCachingProccess(current, total);
    }
    #endregion

    #region public void _gTicks_DataCachingStopping()
    public void _gTicks_DataCachingStopping() {
      //if(this.DataCachingStopping != null)
      //  this.DataCachingStopping();
      for(int i = 0; i < _mtbars.Length; i++) {
        _mtbars[i].Refresh();
      }
    }
    #endregion

    private void MQHTickList_DataCachingChanged(object sender, TickManagerEventArgs tme) {
// if (tme.Status == TickManagerStatus.
    }


    #region ITickManager Members
    // public event Gordago.TickManagerHandler DataCachingStopping;

    // public event Gordago.TickManagerProccessHandler DataCachingProccess;

    #region public TickManagerStatus Status
    public TickManagerStatus Status {
      get { return (_gTicks as ITickManager).Status; }
    }
    #endregion

    #region public int GetPosition(DateTime fdtm)
    public int GetPosition(DateTime fdtm) {
      return (_gTicks as ITickManager).GetPosition(fdtm);
    }
    #endregion

    public int GetPositionFromMap(DateTime fdtm) {
      return (_gTicks as ITickManager).GetPositionFromMap(fdtm);
    }


    #region public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick)
    public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick) {
      return (_gTicks as ITickManager).CheckPeriodInMap(fromdtm, todtm, cnttick);
    }
    #endregion

    //#region public bool UseDataCachingEvents
    //public bool UseDataCachingEvents {
    //  get {
    //    return (_gTicks as ITickManager).us.UseDataCachingEvents;
    //  }
    //  set {
    //    (_gTicks as ITickManager).UseDataCachingEvents = value;
    //  }
    //}
    //#endregion

    #region public void Update(TickCollection ticks)
    public void Update(TickCollection ticks) {
      (_gTicks as ITickManager).Update(ticks);
    }
    #endregion

    #region public void Update(TickCollection ticks, bool isCacheBuffer)
    public void Update(TickCollection ticks, bool isCacheBuffer) {
      (_gTicks as ITickManager).Update(ticks, isCacheBuffer);
    }
    #endregion

    #region public void Update(TickFileInfo tfi)
    public void Update(TickFileInfo tfi) {
      (_gTicks as ITickManager).Update(tfi);
    }
    #endregion

    #region public void Update(TickFileInfo tfi, bool isCacheBuffer)
    public void Update(TickFileInfo tfi, bool isCacheBuffer) {
      (_gTicks as ITickManager).Update(tfi, isCacheBuffer);
    }
    #endregion

    #endregion

    public TickFileInfo InfoCache {
      get { return (_gTicks as ITickManager).InfoCache; }
    }

    public TickFileInfo InfoHistory {
      get { return (_gTicks as ITickManager).InfoHistory; }
    }

    public bool IsDataCaching {
      get { return (_gTicks as ITickManager).IsDataCaching;  }
    }

    #region ITickList Members
    /*
    void ITickList.Add(Tick tick) {
      throw new Exception("The method or operation is not implemented.");
    }

    IBarList[] ITickList.BarLists {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    int ITickList.Count {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    Tick ITickList.Current {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    IBarList ITickList.GetBarList(int second) {
      throw new Exception("The method or operation is not implemented.");
    }

    DateTime ITickList.TimeFrom {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    DateTime ITickList.TimeTo {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    Tick ITickList.this[int index] {
      get { throw new Exception("The method or operation is not implemented."); }
    }
    /**/
    #endregion

    public void DataCaching() {
      (_gTicks as ITickManager).DataCaching();
    }

    public void DataCachingMethod() {
      (_gTicks as ITickManager).DataCachingMethod();
    }

    //bool ITickManager.DataCaching() {
    //  return (_gTicks as ITickManager).DataCaching();
    //}

    //#region public void DataCachingMethod()
    //public void DataCachingMethod() {
    //  (_gTicks as ITickManager).DataCachingMethod();
    //}
    //#endregion

  }
}
