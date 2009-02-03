/**
* @version $Id: Symbol.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;

  class Symbol : ISymbol {

    private readonly TicksManager _ticksManager;
    private readonly string _name;
    private readonly int _digits;
    private readonly float _point;

    public Symbol(TicksManager ticksManager) {
      _ticksManager = ticksManager;
      _name = ticksManager.Name;
      _digits = ticksManager.Digits;
      _point = Convert.ToSingle(Math.Pow(0.1, _digits));
    }

    #region public string Name
    public string Name {
      get { return this._name; }
    }
    #endregion

    #region public int Digits
    public int Digits {
      get { return _digits; }
    }
    #endregion

    #region public TicksManager TicksManager
    public TicksManager TicksManager {
      get { return this._ticksManager; }
    }
    #endregion

    #region public ITickCollection Ticks
    public ITickCollection Ticks {
      get { return this.TicksManager; }
    }
    #endregion

    #region public float Point
    public float Point {
      get { return _point; }
    }
    #endregion
  }
}
