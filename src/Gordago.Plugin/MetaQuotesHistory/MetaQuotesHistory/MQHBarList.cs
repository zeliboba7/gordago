using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.MetaQuotesHistory {
  class MQHBarList:IBarList {

    private IBarList _gBars;
    private MQHBarReader _mqhBarReader;

    /// <summary>
    /// Кол-во баров, ограниченное наложение истории Gordago
    /// </summary>
    private int _limitMQHBarCount;

    public MQHBarList(IBarList gBars) {
      _gBars = gBars;
    }

    #region public int Count
    public int Count {
      get { return _limitMQHBarCount + _gBars.Count; }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
        if(index < _limitMQHBarCount)
          return _mqhBarReader[index];
        return _gBars[index - _limitMQHBarCount];
      }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get { return this[this.Count - 1]; }
    }
    #endregion

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return _gBars.TimeFrame; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get {
        if(_limitMQHBarCount > 0)
          return _mqhBarReader.TimeFrom;
        return _gBars.TimeFrom;
      }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get {
        return _gBars.TimeTo;
      }
    }
    #endregion

    #region public int GetBarIndex(DateTime time)
    public int GetBarIndex(DateTime time) {
      if(_limitMQHBarCount > 0 && time >= _mqhBarReader.TimeFrom && time <= _mqhBarReader.TimeTo) {
        return _mqhBarReader.GetBarIndex(time);
      }
      return _gBars.GetBarIndex(time);
    }
    #endregion

    #region public void SetBarReader(MQHBarReader mqhBarReader)
    public void SetBarReader(MQHBarReader mqhBarReader) {
      _mqhBarReader = mqhBarReader;
      this.Refresh();
    }
    #endregion

    #region public void Refresh()
    public void Refresh() {
      if(_mqhBarReader == null) {
        _limitMQHBarCount = 0;
        return;
      }
      _limitMQHBarCount = _mqhBarReader.Count;

      if(_gBars.TimeFrom < _mqhBarReader.TimeFrom) {
        _limitMQHBarCount = 0;
      } else if(_gBars.TimeFrom > _mqhBarReader.TimeFrom && _gBars.TimeFrom < _mqhBarReader.TimeTo) {
        _limitMQHBarCount = _mqhBarReader.GetBarIndex(_mqhBarReader.TimeFrom);
      }
    }
    #endregion
  }
}
