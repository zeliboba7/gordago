/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Gordago.Analysis.Kernel {
  #region class BarsVectorExt : IVector
  class BarsVectorExt : BaseVector, IVector {
    
    public enum BarTypeValueExt {
      Median,
      Typecal, //(bars[i].High + bars[i].Low + bars[i].Close) / 3
      Weighted //(bars[i].High + bars[i].Low + bars[i].Close * 2F) / 4
    }

    private BarTypeValueExt _extType;
    private BarsVector _high, _low, _close;
    private IBarList _bars;

    public BarsVectorExt(int tfsecond, Analyzer analyzer, BarTypeValueExt extType) {
      _bars = analyzer.GetBars(tfsecond);
      _extType = extType;
      _high = new BarsVector(_bars, tfsecond, BarsTypeValue.High);
      _low = new BarsVector(_bars, tfsecond, BarsTypeValue.Low);
      _close = new BarsVector(_bars, tfsecond, BarsTypeValue.Close);
    }

    #region public int Count
    public int Count {
      get { return _bars.Count; }
    }
    #endregion

    #region public float Current 
    public float Current {
      get {
        return this[this.Count-1];
      }
    }
    #endregion

    #region public float this[int index]
    public float this[int index] {
      get {
        switch (_extType) {
          case BarTypeValueExt.Median:
            return _high[index] + _low[index];
          case BarTypeValueExt.Typecal:
            return (_high[index] + _low[index] + _close[index]) / 3;
          case BarTypeValueExt.Weighted:
            return (_high[index] + _low[index] + _close[index]*2F) / 4;
        }
        return float.NaN;
      }
      set { }
    }
    #endregion

    #region over
    public void Add(float value) { }
    public void Clear() { }
    public void RemoveLastValue() { }
    #endregion
  }
  #endregion

  #region internal class Median : Function
  [Function("Median")]
  internal class Median : Function {
    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorMedian", new string[] { "Color", "Цвет" }, Color.OrangeRed));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVectorExt(second, analyzer, BarsVectorExt.BarTypeValueExt.Median);
      
      return result;

      //IBarList bars = (result as BarsVector).Bars;
      //if (bars.Count == result.Count)
      //  result.RemoveLastValue();
      //for (int i = result.Count; i < bars.Count; i++)
      //  result.Add((bars[i].High + bars[i].Low) / 2);
      //return result;
    }
  }
  #endregion

  #region internal class Typical : Function
  [Function("Typical")]
  internal class Typical : Function {
    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorTypical", new string[] { "Color", "Цвет" }, Color.Salmon));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVectorExt(second, analyzer, BarsVectorExt.BarTypeValueExt.Typecal);

      //IBarList bars = (result as BarsVector).Bars;

      //if (bars.Count == result.Count)
      //  result.RemoveLastValue();

      //for (int i = result.Count; i < bars.Count; i++)
      //  result.Add((bars[i].High + bars[i].Low + bars[i].Close) / 3);

      return result;
    }
  }
  #endregion

  #region internal class Weighted : Function
  [Function("Weighted")]
  internal class Weighted : Function {
    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorWeighted", new string[] { "Color", "Цвет" }, Color.Snow));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVectorExt(second, analyzer, BarsVectorExt.BarTypeValueExt.Weighted);

      //IBarList bars = (result as BarsVector).Bars;

      //if (bars.Count == result.Count)
      //  result.RemoveLastValue();

      //for (int i = result.Count; i < bars.Count; i++)
      //  result.Add((bars[i].High + bars[i].Low + bars[i].Close * 2F) / 4);

      return result;
    }
  }
  #endregion
}