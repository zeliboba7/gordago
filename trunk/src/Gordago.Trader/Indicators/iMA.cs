/**
* @version $Id: iMA.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.ComponentModel;
  using System.Drawing;

  public class iMA : Indicator {

    private FxMA _fxMA;
    private int _period = 13;
    private FxMAMethod _maMethod = FxMAMethod.Exponential;

    [Input("InData", "Period", "Method")]
    public iMA(IFunction inData, int period, FxMAMethod method) {
      _fxMA = new FxMA(inData, period, method);
    }

    #region public IFunction InData
    [ParameterFrom(typeof(Function), "InData", 0)]
    public IFunction InData {
      get { return _fxMA.InData; }
    }
    #endregion

    #region public int Period
    [ParameterFrom(typeof(FxMA), "Period", 1)]
    public int Period {
      get { return _period; }
    }
    #endregion

    #region public FxMAMethod Method 
    [ParameterFrom(typeof(FxMA), "Method", 2)]
    public FxMAMethod Method {
      get { return _maMethod; }
    }
    #endregion

    #region public FxMA MA 
    [Function("MA")]
    [FunctionStyle(FunctionStyle.Line), FunctionColor("Blue")]
    public FxMA MA {
      get { return _fxMA; }
    }
    #endregion
  }

  public class FxMA : Function {

    private int _period = 13;
    private FxMAMethod _method = FxMAMethod.Exponential;

    private double _smma = 0, _smmaLast = 0;
    private int _savedLastIndex;

    public FxMA(IFunction data, int period, FxMAMethod method)
      : base(data) {
      this._period = period;
      this._method = method;
    }

    #region public int Period
    [Parameter("Period", 1)]
    [DefaultValue(13)]
    public int Period {
      get { return _period; }
    }
    #endregion

    #region public MovingAverageMethod Method
    [Parameter("Method", 2)]
    [DefaultValue(FxMAMethod.Exponential)]
    public FxMAMethod Method {
      get { return _method; }
    }
    #endregion

    #region protected override void OnCompute()
    protected override void OnCompute() {
      switch (Method) {
        case FxMAMethod.Simple:
          this.SMA();
          break;
        case FxMAMethod.Exponential:
          this.EMA();
          break;
        case FxMAMethod.LinearWeighted:
          this.LWMA();
          break;
        case FxMAMethod.Smoothed:
          this.SMMA();
          break;
      }
    }
    #endregion

    #region protected override void OnRemoveLastValue()
    protected override void OnRemoveLastValue() {
      _smma = _smmaLast;
      base.OnRemoveLastValue();
    }
    #endregion

    #region private void SMA()
    private void SMA() {
      int count1 = this.Count + 1;
      int count2 = this.InData.Count + 1;

      for (int i = count1; i < count2; i++) {
        if (i < Period) {
          this.Add();
        } else {
          if (_savedLastIndex < i) {
            _smmaLast = _smma;
            _savedLastIndex = i;
          }
          if (i == Period || float.IsNaN(this[0])) {
            _smma = 0;
            for (int j = i - Period; j < i; j++)
              _smma += this.InData.Items[j];
          } else {
            _smma -= this.InData.Items[i - Period - 1];
            _smma += this.InData.Items[i - 1];
          }
          this.Add(Convert.ToSingle(_smma / Period));
#if DEBUGg
          double sma = 0;
          for (int j = i - Period; j < i; j++)
            sma += this.InData.Items[j];
          if (_smma != sma && !double.IsNaN(sma)) {
            throw (new Exception("Error"));
          }
#endif
        }
      }
    }
    #endregion

    #region private void EMA()
    private void EMA() {

      float k = 2f / (Period + 1f);
      int count1 = this.Count;
      int count2 = this.InData.Count;

      for (int i = count1; i < count2; i++) {
        if (i < Period) {
          this.Add();
        } else {
          float lastValue;
          if (i == Period || float.IsNaN(this[0]))
            lastValue = this.InData.Items[i];
          else
            lastValue = this[0];

          this.Add(this.InData.Items[i] * k + lastValue * (1 - k));
        }
      }
    }
    #endregion

    #region private void LWMA()
    private void LWMA() {
      float sw = 0.5f * Period * (Period + 1);
      for (int i = this.Count + 1; i < this.InData.Count + 1; i++) {
        if (i < Period) {
          this.Add();
        } else {
          float wma = 0;
          for (int j = i - Period, c = 1; j < i; j++, c++)
            wma += this.InData.Items[j] * c;
          this.Add(wma / sw);
        }
      }
    }
    #endregion

    #region private void SMMA()
    private void SMMA() {
      for (int i = this.Count + 1; i < this.InData.Count + 1; i++) {
        if (i < Period) {
          this.Add();
        } else {

          if (_savedLastIndex < i) {
            _smmaLast = _smma;
            _savedLastIndex = i;
          }

          if (i == Period || float.IsNaN(this[0])) {
            _smma = 0;
            for (int j = i - Period; j < i; j++)
              _smma += this.InData.Items[j];
          } else {
            _smma = this[0] * (Period - 1) + this.InData.Items[i - 1];
          }
          this.Add(Convert.ToSingle(_smma / Period));
        }
      }
    }
    #endregion
  }
}
