/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {

  class TestSymbol: ISymbol {
    
    private ISymbol _symbol;
    private TestTickManager _ticks;
    private float _bid, _ask;
    private DateTime _time;

    public TestSymbol(ISymbol symbol, int indexfrom) {
      _symbol = symbol;
      _ticks = new TestTickManager(_symbol.Ticks, indexfrom);
    }
    
    #region public string Name
    public string Name {
      get { return _symbol.Name; }
    }
    #endregion

    #region public float Point
    public float Point {
      get { return _symbol.Point; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return _symbol.DecimalDigits; }
    }
    #endregion

    #region public SymbolProperty Property
    public SymbolProperty Property {
      get { return _property; }
      set { this._property = value; }
    }
    #endregion

    #region public ITickList Ticks
    public ITickList Ticks {
      get { return _ticks; }
    }
    #endregion

    #region public float Bid
    public float Bid {
      get { return this._bid; }
    }
    #endregion
    
    #region public float Ask
    public float Ask {
      get { return this._ask; }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get { return this._time; }
    }
    #endregion

    #region public Symbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
    }
    #endregion

    #region internal void SetTick(float bid, float ask, DateTime time)
    internal void SetTick(float bid, float ask, DateTime time) {
      _bid = bid;
      _ask = ask;
      _time = time;
    }
    #endregion
  }
}
