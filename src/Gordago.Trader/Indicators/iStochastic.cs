/**
* @version $Id: iStochastic.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.ComponentModel;

  public class iStochastic : Indicator {

    private FxK _fxK;
    private FxD _fxD;

    private iBars _ibars;
    private int _period;
    private int _delayK;
    private int _delayD;

    [Input("iBars", "Period", "DelayK", "DelayD")]
    public iStochastic(iBars ibars, int period, int delayK, int delayD) {
      _fxK = new FxK(ibars, period, delayK);
      _fxD = new FxD(_fxK, delayD);
      _ibars = ibars;
      _period = period;
      _delayK = delayK;
      _delayD = delayD;
    }

    #region public iBars iBars
    [Parameter("iBars")]
    public iBars iBars {
      get { return _ibars; }
    }
    #endregion

    #region public int Period
    [Parameter("Period"), DefaultValue(5)]
    public int Period {
      get {
        return _period;
      }
    }
    #endregion

    #region public int DelayK
    [Parameter("DelayK"), DefaultValue(3)]
    public int DelayK {
      get {
        return _delayK;
      }
    }
    #endregion

    #region public int DelayD
    [Parameter("DelayD"), DefaultValue(3)]
    public int DelayD {
      get {
        return _delayD;
      }
    }
    #endregion

    #region public FxK K
    [Function("K"), FunctionColor("ForestGreen")]
    public FxK K {
      get { return _fxK; }
    }
    #endregion

    #region public FxD D
    [Function("D"), FunctionColor("Blue")]
    public FxD D {
      get { return _fxD; }
    }
    #endregion

    #region public class FxK : Function
    public class FxK : Function {

      private iBars _iBars;
      private int _period;
      private int _delayK;


      public FxK(iBars ibars, int period, int delayK):base(ibars) {
        _iBars = ibars;
        _period = period;
        _delayK = delayK;
      }

      protected override void OnCompute() {
        int thisCount = this.Count;
        int inDataCount = this.InData.Count;

        for (int i = thisCount + 1; i < inDataCount + 1; i++) {
          if (i < _period +_delayK) {
            this.Add();
          } else {
            float a = 0, b = 0;
            for (int k = 0; k < _delayK; k++) {
              float h = float.MinValue;
              float l = float.MaxValue;
              for (int j = i - k - _period; j < i - k; j++) {
                if (h < _iBars.High.Items[j])
                  h = _iBars.High.Items[j];
                if (l > _iBars.Low.Items[j])
                  l = _iBars.Low.Items[j];
              }
              a += _iBars.Close.Items[i - k - 1] - l;
              b += h - l;
            }
            this.Add(100f * a / b);
          }
        }
      }
    }
    #endregion

    #region public class FxD : FxMA
    public class FxD : FxMA {
      
      public FxD(FxK fxK, int delayD):base(fxK, delayD, FxMAMethod.Exponential) {
      }
    }
    #endregion
  }
}
