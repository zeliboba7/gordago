/**
* @version $Id: iADX.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.ComponentModel;
  using System.Drawing;

  public class iADX : Indicator {

    private iBars _ibars;
    private FxPSDI _psdi;
    private MSDI _msdi;
    private FxPDI _pdi;
    private FxMDI _mdi;
    private DX _dx;
    private FxADX _adx;

    private int _period;

    [Input("iBars", "Period")]
    public iADX(iBars ibars, int period) {
      _ibars = ibars;
      _period = period;

      _psdi = new FxPSDI(ibars);
      _pdi = new FxPDI(_psdi, period);

      _msdi = new MSDI(ibars);
      _mdi = new FxMDI(_msdi, period);

      _dx = new DX(_pdi, _mdi);
      _adx = new FxADX(_dx, period);
    }

    #region public int Period
    [Parameter("Period"), DefaultValue(13)]
    public int Period {
      get { return this._period; }
    }
    #endregion

    #region public iBars iBars
    [Parameter("iBars")]
    public iBars iBars {
      get { return _ibars; }
    }
    #endregion

    #region public FxPDI PDI
    [Function("PDI"), FunctionColor("BurlyWood")]
    public FxPDI PDI {
      get { return _pdi; }
    }
    #endregion

    #region public FxADX ADX
    [Function("ADX"), FunctionColor("Green")]
    public FxADX ADX {
      get { return _adx; }
    }
    #endregion

    #region public FxMDI MDI
    [Function("MDI"), FunctionColor("RosyBrown")]
    public FxMDI MDI {
      get { return this._mdi; }
    }
    #endregion

    #region internal class FxPSDI : Function
    internal class FxPSDI : Function {

      private iBars _iBars;

      public FxPSDI(iBars ibars) : base(ibars) {
        _iBars = ibars;
      }

      protected override void OnCompute() {
        for (int i = this.Count; i < this.InData.Count; i++) {
          if (i > 0 && _iBars.High.Items[i] > _iBars.High.Items[i - 1]) {
            float tr = Math.Max(Math.Max(_iBars.High.Items[i] - _iBars.Low.Items[i],
              _iBars.High.Items[i] - _iBars.Close.Items[i - 1]),
              _iBars.Close.Items[i - 1] - _iBars.Low.Items[i]);
            this.Add((_iBars.High.Items[i] - _iBars.High.Items[i - 1]) / tr * 100);
          } else
            this.Add(0);
        }
      }
    }
    #endregion

    #region class MSDI : Function
    internal class MSDI : Function {

      private iBars _iBars;
      public MSDI(iBars ibars)
        : base(ibars) {
        _iBars = ibars;
      }

      protected override void OnCompute() {
        for (int i = this.Count; i < this.InData.Count; i++) {
          if (i > 0 && _iBars.Low.Items[i] < _iBars.Low.Items[i - 1]) {
            float tr = Math.Max(
              Math.Max(_iBars.High.Items[i] - _iBars.Low.Items[i], _iBars.High.Items[i] - _iBars.Close.Items[i - 1]),
              _iBars.Close.Items[i - 1] - _iBars.Low.Items[i]);
            this.Add((_iBars.Low.Items[i - 1] - _iBars.Low.Items[i]) / tr * 100);
          } else {
            this.Add(0);
          }
        }
      }
    }
    #endregion

    #region public class FxPDI : MovingAverage
    public class FxPDI : FxMA {

      internal FxPDI(FxPSDI psdi, int period)
        : base(psdi, period, FxMAMethod.Exponential) {
      }

      public FxPDI(iBars ibars, int period)
        : base(new FxPSDI(ibars), period, FxMAMethod.Exponential) {
      }
    }
    #endregion

    #region public class FxMDI : MovingAverage
    public class FxMDI : FxMA {
      internal FxMDI(MSDI msdi, int period)
        : base(msdi, period, FxMAMethod.Exponential) {
      }
      public FxMDI(iBars ibars, int period)
        : base(new MSDI(ibars), period, FxMAMethod.Exponential) {
      }
    }
    #endregion

    #region internal class DX:Function
    internal class DX : Function {

      private FxPDI _pdi;
      private FxMDI _mdi;

      public DX(FxPDI pdi, FxMDI mdi)
        : base(pdi) {
        _pdi = pdi;
        _mdi = mdi;
      }

      protected override void OnCompute() {

        for (int i = this.Count; i < this.InData.Count; i++) {
          this.Add(Math.Abs(_pdi.Items[i] - _mdi.Items[i]) /
                   Math.Abs(_pdi.Items[i] + _mdi.Items[i]) * 100);
        }
      }
    }
    #endregion

    #region public class FxADX : MovingAverage
    public class FxADX : FxMA {
      internal FxADX(DX dx, int period)
        : base(dx, period, FxMAMethod.Exponential) {
      }
    }
    #endregion
  }
}
