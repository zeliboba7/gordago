/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  
  public enum BarsTypeValue {
    Open,
    Low, 
    High, 
    Close, 
    Volume,
    Time
  }

  public class BarsVector : BaseVector, IVector {

    private IBarList _bars;
    private int _tfsecond;
    private BarsTypeValue _barsType;

    public BarsVector(IBarList bars, int tfsecond, BarsTypeValue barsType) {
      _bars = bars;
      _barsType = barsType;
      _tfsecond = tfsecond;
    }

    #region public IBars Bars
    public IBarList Bars {
      get { return this._bars; }
    }
    #endregion

    #region public int TFSecond
    public int TFSecond {
      get { return this._tfsecond; }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        if (this.HideMode)
          return this.HideCount;
        return _bars.Count;
      }
    }
    #endregion

    #region public float Current
    public float Current {
      get { 
        switch(_barsType){
          case BarsTypeValue.Close:
            return _bars.Current.Close;
          case BarsTypeValue.High:
            return _bars.Current.High;
          case BarsTypeValue.Low:
            return _bars.Current.Low;
          case BarsTypeValue.Open:
            return _bars.Current.Open;
          case BarsTypeValue.Volume:
            return _bars.Current.Volume;
        }
        return float.NaN;
      }
    }
    #endregion

    #region public float this[int index]
    public float this[int index] {
      get {
        switch (_barsType) {
          case BarsTypeValue.Close:
            return _bars[index].Close;
          case BarsTypeValue.High:
            return _bars[index].High;
          case BarsTypeValue.Low:
            return _bars[index].Low;
          case BarsTypeValue.Open:
            return _bars[index].Open;
          case BarsTypeValue.Volume:
            return _bars[index].Volume;
        }
        return float.NaN;
      }
      set { }
    }
    #endregion

    public void Add(float value) { }
    public void Clear() { }
    public void RemoveLastValue() { }
  }
}
