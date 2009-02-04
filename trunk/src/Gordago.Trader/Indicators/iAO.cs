/**
* @version $Id: iAO.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  /// <summary>
  /// Awesome Oscillator
  /// </summary>
  public class iAO:Indicator {

    private readonly FxAO _fxAO;
    private readonly IFunction _inData;
    private readonly int _fast, _slow;

    public iAO(IFunction inData, int fast, int slow) {
      _inData = inData;
      _fast = fast;
      _slow = slow;
      _fxAO = new FxAO(inData, fast, slow);
    }


    [Parameter("InData", DefaultType = typeof(iBars.FxMedian))]
    public IFunction InData {
      get { return _inData; }
    }

    public int Fast {
      get { return _fast; }
    }

    public int Slow {
      get { return _slow; }
    }

    public FxAO AO {
      get { return _fxAO; }
    }

    public class FxAO:Function {

      private readonly IFunction _inData;
      private readonly int _fast;
      private readonly int _slow;

      private readonly FxMA _fxMAFast, _fxMASlow;

      public FxAO(IFunction inData, int fast, int slow):base(inData) {
        _fast = fast;
        _slow = slow;
        _fxMAFast = new FxMA(inData, fast, FxMAMethod.Exponential);
        _fxMASlow = new FxMA(inData, slow, FxMAMethod.Exponential);
      }


      protected override void OnCompute() {
        Function.Subtraction(_fxMAFast, _fxMASlow, this);
      }
    }
  }
}
