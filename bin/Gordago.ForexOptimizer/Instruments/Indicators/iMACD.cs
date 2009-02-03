namespace Gordago.Trader.Indicators { 
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

    [ParameterFrom(typeof(Function), "InData", 0)]
    public IFunction InData {
      get { return _macdBase.InData; }
    }

    [Parameter("FastEMA", 1), DefaultValue(12)]
    public int FastEMA {
      get { return this._macdBase.FastEMA; }
    }

    [Parameter("SlowEMA", 2), DefaultValue(26)]
    public int SlowEMA {
      get { return _macdBase.SlowEMA; }
    }

    [Parameter("SignalSMA", 1), DefaultValue(12)]
    public int SignalSMA {
      get { return _macdSignal.Period; }
    }

    [Function("Base"), FunctionColor("Blue"), FunctionStyle( FunctionStyle.Histogram)]
    public FxBase Base {
      get { return _macdBase; }
    }

    [Function("Signal"), FunctionColor("Red")]
    public FxSignal Signal {
      get { return _macdSignal; }
    }

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

      public int FastEMA {
        get { return _fastEMA; }
      }

      public int SlowEMA {
        get { return _slowEMA; }
      }

      protected override void OnCompute() {
        Function.Subtraction(_maFast, _maSlow, this);
      }
    }

    public class FxSignal : FxMA {

      public FxSignal(FxBase macdBase, int period) : base(macdBase, period, FxMAMethod.Simple) {
      }
    }
  }
}
