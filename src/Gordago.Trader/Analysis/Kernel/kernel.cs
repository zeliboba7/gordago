/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Gordago.Analysis.Kernel {

  #region internal class Add : Function
  [Function("add")]
  internal class Add : Function {

    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector1 = parameters[0] as IVector;
      IVector vector2 = parameters[1] as IVector;

      if (result == null)
        result = new Vector();

      if (result.Count == vector1.Count)
        result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++)
        result.Add(vector1[i] + vector2[i]);

      return result;
    }
  }
  #endregion

  #region internal class Subtract : Function
  [Function("sub")]
  internal class Subtract : Function {
    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector1 = parameters[0] as IVector;
      IVector vector2 = parameters[1] as IVector;

      if (result == null) 
        result = new Vector();

      if (result.Count == vector1.Count) result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++)
        result.Add(vector1[i] - vector2[i]);

      return result;
    }
  }
  #endregion

  #region internal class Multiply : Function
  [Function("mul")]
  internal class Multiply : Function {
    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector1 = parameters[0] as IVector;
      IVector vector2 = parameters[1] as IVector;

      if (result == null) result = new Vector();

      if (result.Count == vector1.Count) result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++)
        result.Add(vector1[i] * vector2[i]);

      return result;
    }
  }
  #endregion

  #region internal class ScalarMultiply : Function
  [Function("smul")]
  internal class ScalarMultiply : Function {

    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector = parameters[0] as IVector;
      float scalar = (float)parameters[1];

      if (result == null) result = new Vector();
      if (result.Count == vector.Count) result.RemoveLastValue();

      for (int i = result.Count; i < vector.Count; i++)
        result.Add(vector[i] * scalar);
      return result;
    }
  }
  #endregion

  #region internal class Divide : Function
  [Function("div")]
  internal class Divide : Function {

    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector1 = parameters[0] as IVector;
      IVector vector2 = parameters[1] as IVector;

      int size = Math.Min(vector1.Count, vector2.Count);

      if (result == null) result = new Vector();
      if (result.Count == vector1.Count) result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++) {
        if (vector2[i] == 0)
          result.Add(float.NaN);
        else
          result.Add(vector1[i] / vector2[i]);

      }
      return result;
    }
  }
  #endregion

  #region internal class Absolute : Function
  [Function("abs")]
  internal class Absolute : Function {
    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector = parameters[0] as IVector;

      if (result == null)
        result = new Vector();
      if (result.Count == vector.Count)
        result.RemoveLastValue();

      for (int i = result.Count; i < vector.Count; i++)
        result.Add(Math.Abs(vector[i]));
      return result;
    }
  }
  #endregion

  #region internal class Open : Function
  [Function("Open")]
  internal class Open : Function {

    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorOpen", new string[] { "Color", "Цвет" }, Color.Goldenrod));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVector(analyzer.GetBars(second), second, BarsTypeValue.Open);
      return result;
    }
  }
  #endregion

  #region internal class Low : Function
  [Function("Low")]
  internal class Low : Function {
    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorLow", new string[] { "Color", "Цвет" }, Color.Gainsboro));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVector(analyzer.GetBars(second), second, BarsTypeValue.Low);

      return result;
    }
  }
  #endregion

  #region internal class Volume : Function
  [Function("Volume")]
  internal class Volume : Function {

    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorVolume", new string[] { "Color", "Цвет" }, Color.Violet));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVector(analyzer.GetBars(second), second, BarsTypeValue.Volume);

      //IBarList bars = (result as BarsVector).Bars;

      //if (bars.Count == result.Count) {
      //  result.Current = bars.Current.Volume;
      //} else
      //  for (int i = result.Count; i < bars.Count; i++)
      //    result.Add(bars[i].Volume);

      return result;
    }
  }
  #endregion

  #region internal class Time : Function
  [Function("Time")]
  internal class Time : Function {

    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null) 
        result = new BarsVector(analyzer.GetBars(second), second, BarsTypeValue.Time);

      //if (bars.Count == result.Count) {
      //  result.Current = float.NaN;
      //} else
      //  for (int i = result.Count; i < bars.Count; i++)
      //    result.Add(float.NaN);

      return result;
    }
  }
  #endregion

  #region internal class SwitchFunction : Function
  [Function("switch")]
  internal class SwitchFunction : Function {
    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      return (IVector)parameters[1 + (int)parameters[0]];
    }
  }
  #endregion

#if ASDF
	[Function("primary")]
	internal class PrimaryFunction : Function {
		protected override void Initialize() {}

		public override Vector Compute(Analyzer analyzer, object[] parameters, Vector result) {
			if ( open == null || low == null || high == null || close == null || volume == null || median == null || typical == null || weighted == null ) {
				open = analyzer["open"];
				low = analyzer["low"];
				high = analyzer["high"];
				close = analyzer["close"];
				volume = analyzer["volume"];
				median = analyzer["median"];
				typical = analyzer["typical"];
				weighted = analyzer["weighted"];
			}

			switch ( (int)parameters[1] ) {
				case 0:
					return analyzer.Compute(open,parameters[0]);
				case 1:
					return analyzer.Compute(low,parameters[0]);
				case 2:
					return analyzer.Compute(high,parameters[0]);
				case 3:
					return analyzer.Compute(close,parameters[0]);
				case 4:
					return analyzer.Compute(volume,parameters[0]);
				case 5:
					return analyzer.Compute(median,parameters[0]);
				case 6:
					return analyzer.Compute(typical,parameters[0]);
				case 7:
					return analyzer.Compute(weighted,parameters[0]);
			}
			return (Vector)parameters[1 + (int)parameters[0]];
		}

		private Function open,low,high,close,volume,median,typical,weighted;
	}
#endif

  #region internal class Number : Function
  [Function("Number")]
  internal class Number : Function {

    protected override void Initialize() {
      RegParameter(new ParameterInteger("__NumberValue", "Value", 0, -10000000, +10000000));
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorNumber", new string[] { "Color", "Цвет" }, Color.Linen));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      if (result == null)
        result = new Vector();

      int second = (int)parameters[1];

      IBarList bars = analyzer.GetBars(second);

      for (int i = result.Count; i < bars.Count; i++)
        result.Add((int)parameters[0]);

      return result;
    }
  }
  #endregion

  #region internal class RoundFunction : Function
  [Function("round")]
  internal class RoundFunction : Function {

    protected override void Initialize() { }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector vector = (IVector)parameters[0];

      double accuracy = Math.Pow(10, (int)parameters[1]);

      if (result == null)
        result = new Vector();
      if (result.Count == vector.Count)
        result.RemoveLastValue();

      for (int i = result.Count; i < vector.Count; i++)
        result.Add((float)(Math.Round(vector[i] * accuracy) / accuracy));

      return result;
    }
  }
  #endregion
}
