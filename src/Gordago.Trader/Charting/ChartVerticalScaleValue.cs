/**
* @version $Id: ChartVerticalScaleValue.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Trader.Charting {
  public class ChartVerticalScaleValue {
    private float _value;
    private string _name;

    public ChartVerticalScaleValue(float value, string name) {
      _value = value;
      _name = name;
    }

    #region public float Value
    /// <summary>
    /// Value
    /// </summary>
    public float Value {
      get { return this._value; }
      set { this._value = value; }
    }
    #endregion

    #region public string Name
    /// <summary>
    /// Name
    /// </summary>
    public string Name {
      get { return this._name; }
      set { this._name = value; }
    }
    #endregion
  }
}
