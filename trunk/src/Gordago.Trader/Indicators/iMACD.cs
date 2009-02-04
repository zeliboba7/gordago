/**
* @version $Id: iMACD.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.ComponentModel;

  public class iMACD: Indicator {

    private FxBase _macdBase;
    private FxSignal _macdSignal;

    [Input("InData", "FastEMA", "SlowEMA", "SignalSMA")]
    public iMACD(IFunction inData, int fast, int slow, int signalSMA) {
      _macdBase = new FxBase(inData, fast, slow);
      _macdSignal = new FxSignal(_macdBase, signalSMA);
    }

    #region public IFunction InData
    [ParameterFrom(typeof(Function), "InData", 0)]
    public IFunction InData {
      get { return _macdBase.InData; }
    }
    #endregion

    #region public int FastEMA
    [Parameter("FastEMA", 1), DefaultValue(12)]
    public int FastEMA {
      get { return this._macdBase.FastEMA; }
    }
    #endregion

    #region public int SlowEMA
    [Parameter("SlowEMA", 2), DefaultValue(26)]
    public int SlowEMA {
      get { return _macdBase.SlowEMA; }
    }
    #endregion

    #region public int SignalSMA
    [Parameter("SignalSMA", 1), DefaultValue(12)]
    public int SignalSMA {
      get { return _macdSignal.Period; }
    }
    #endregion

    #region public FxBase Base
    [Function("Base"), FunctionColor("Blue"), FunctionStyle( FunctionStyle.Histogram)]
    public FxBase Base {
      get { return _macdBase; }
    }
    #endregion

    #region public FxSignal Signal
    [Function("Signal"), FunctionColor("Red")]
    public FxSignal Signal {
      get { return _macdSignal; }
    }
    #endregion

    #region public class FxBase : Function
    public class FxBase : Function {

      private int _fastEMA = 12;
      private int _slowEMA = 26;

      private FxMA _maFast;
      private FxMA _maSlow;

      public FxBase(IFunction data, int fastEMA, int slowEMA):base(data) {
        _fastEMA = fastEMA;
        _slowEMA = slowEMA;
        _maFast = new FxMA(data, fastEMA, FxMAMethod.Exponential);
        _maSlow = new FxMA(data, slowEMA, FxMAMethod.Exponential);
      }

      #region public int FastEMA 
      public int FastEMA {
        get { return _fastEMA; }
      }
      #endregion

      #region public int SlowEMA
      public int SlowEMA {
        get { return _slowEMA; }
      }
      #endregion

      #region protected override void OnCompute()
      protected override void OnCompute() {
        Function.Subtraction(_maFast, _maSlow, this);
      }
      #endregion
    }
    #endregion

    #region public class FxSignal : FxMA
    public class FxSignal : FxMA {

      public FxSignal(FxBase macdBase, int period) : base(macdBase, period, FxMAMethod.Simple) {
      }
    }
    #endregion
  }
}
