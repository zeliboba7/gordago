/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  public class TestHistoryInfo {

    private ISymbol _symbol;
    private int _tickCount;
    private long _firstTimeTick = 0, _endTimeTick = 0;
    private float _minRate, _maxRate;

    public TestHistoryInfo(ISymbol symbol) {
      _symbol = symbol;
    }

    public ISymbol Symbol {
      get { return this._symbol; }
    }

    #region public int CountTick
    public int CountTick {
      get { return this._tickCount; }
    }
    #endregion

    #region public DateTime TimeFromTick
    public DateTime TimeFromTick {
      get { return new DateTime(_firstTimeTick); }
    }
    #endregion

    #region public DateTime TimeToTick
    public DateTime TimeToTick {
      get { return new DateTime(_endTimeTick); }
    }
    #endregion

    #region public float MinRate
    public float MinRate {
      get { return this._minRate; }
    }
    #endregion

    #region public float MaxRate
    public float MaxRate {
      get { return this._maxRate; }
    }
    #endregion

    #region public void AddTick(Tick tick)
    public void AddTick(Tick tick) {
      if(_tickCount == 0) {
        _firstTimeTick = tick.Time;
        _minRate = _maxRate = tick.Price;
      }
      _endTimeTick = tick.Time;
      _minRate = Math.Min(_minRate, tick.Price);
      _maxRate = Math.Max(_maxRate, tick.Price);
      _tickCount++;
    }
    #endregion
  }
}
