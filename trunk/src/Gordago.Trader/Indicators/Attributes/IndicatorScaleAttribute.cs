/**
* @version $Id: IndicatorScaleAttribute.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class IndicatorScaleAttribute {

    private float _minValue = float.NaN;
    private float _maxValue = float.NaN;

    private int _digits = -1;

    public IndicatorScaleAttribute() {}

    #region public IndicatorScaleAttribute(int digits, float minValue, float maxValue)
    public IndicatorScaleAttribute(int digits, float minValue, float maxValue) {
      _digits = digits;
      _minValue = minValue;
      _maxValue = maxValue;
    }
    #endregion

    #region public int Digits
    /// <summary>
    /// If -1, then digits from Symbol
    /// </summary>
    public int Digits {
      get { return this._digits; }
      set { 
        this._digits = Math.Max(value, -1); 
      }
    }
    #endregion

    #region public float MinimumValue
    public float MinimumValue{
      get { return _minValue; }
      set { _minValue = value; }
    }
    #endregion

    #region public float MaximumValue
    public float MaximumValue {
      get { return _maxValue; }
      set { this._maxValue = value; }
    }
    #endregion
  }
}
