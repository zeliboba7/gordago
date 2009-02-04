using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iGatorOscillator : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Gator Oscillator";
			this.ShortName = "Gator";
			this.SetImage("i.gif");

			FunctionStyle fsBlue = RegFunction("ReGator");
			fsBlue.SetDrawPen(Color.Blue);
			fsBlue.SetDrawStyle(FunctionDrawStyle.Histogram);

			FunctionStyle fsgreen = RegFunction("Gator");
			fsgreen.SetDrawPen(Color.Green);
			fsgreen.SetDrawStyle(FunctionDrawStyle.Histogram);
		}
	}

	[Function("ReGator")]
	public class ReGator : Function {
		private Function _funcMA;

		protected override void Initialize() {
			SetShortName("Top Gator");
			_funcMA = GetFunction("ma");

			RegParameter(new ParameterInteger("RedLinePeriod", new string[]{"Red Line Period (\"Teeth\"", "ѕериод красной линии (\"«убы\")"}, 8, 1, 10000));
			RegParameter(new ParameterInteger("RedLineShift", new string[]{"Red Line Shift (\"Teeth\"", "—двиг красной линии (\"«убы\")"}, 5, -10000, 10000));
			RegParameter(Function.CreateDefParam_MAType(3));
			RegParameter(Function.CreateDefParam_ApplyTo("Median"));
			RegParameter(new ParameterInteger("BlueLinePeriod", new string[]{"Blue Line Period (\"Jaw\"", "ѕериод синей линии (\"„елюсти\")"}, 13, 1, 10000));
			RegParameter(new ParameterInteger("BlueLineShift", new string[]{"Blue Line Shift (\"Jaw\"", "—двиг синей линии (\"„елюсти\")"}, 8, -10000, 10000));
      RegParameter(new ParameterColor("ColorReGator", new string[] { "Color", "÷вет" }, Color.Blue));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters, IVector result) {
			int shift1 = (int)parameters[5];
			int shift2 = (int)parameters[1];
			int period1 = (int)parameters[4];
			int period2 = (int)parameters[0];
			
			
			IVector vector1 = analyzer.Compute(_funcMA,period1,parameters[2],parameters[3]);
			IVector vector2 = analyzer.Compute(_funcMA,period2,parameters[2],parameters[3]);

      int size = Math.Max(vector1.Count - shift1, vector2.Count - shift2);
			
			if ( result == null ) 
				result = new Vector();
      if (result.Count == vector1.Count)
				result.RemoveLastValue();

			int period = Math.Max(shift1, shift2);

      for (int i = result.Count; i < vector1.Count; i++) {
				if (i<period)
					result.Add(float.NaN);
				else
					result.Add( Math.Abs(vector1[i-shift1] - vector2[i - shift2]));
			}
			return result;
		}
	}

	[Function("Gator")]
	public class Gator : Function {
		private Function _funcMA;

		protected override void Initialize() {
			SetShortName("Bot.Gator");
			_funcMA = GetFunction("ma");

			RegParameter(new ParameterInteger("RedLinePeriod", new string[]{"Red Line Period (\"Teeth\"", "ѕериод красной линии (\"«убы\")"}, 8, 1, 10000));
			RegParameter(new ParameterInteger("RedLineShift", new string[]{"Red Line Shift (\"Teeth\"", "—двиг красной линии (\"«убы\")"}, 5, -10000, 10000));
			RegParameter(Function.CreateDefParam_MAType(3));
			RegParameter(Function.CreateDefParam_ApplyTo("Median"));
			RegParameter(new ParameterInteger("GreenLinePeriod", new string[]{"Green Line Period (\"Lips\"", "ѕериод зеленой линии (\"√убы\")"}, 5, 1, 10000));
			RegParameter(new ParameterInteger("GreenLineShift", new string[]{"Green Line Shift (\"Lips\"", "—двиг зеленой линии (\"√убы\")"}, 3, -10000, 10000));
      RegParameter(new ParameterColor("ColorGator", new string[] { "Color", "÷вет" }, Color.Green));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters, IVector result) {
			int shift1 = (int)parameters[1];
			int shift2 = (int)parameters[5];
			int period1 = (int)parameters[0];
			int period2 = (int)parameters[4];
			
			IVector vector1 = analyzer.Compute(_funcMA,period1,parameters[2],parameters[3]);
			IVector vector2 = analyzer.Compute(_funcMA,period2,parameters[2],parameters[3]);
			
			if ( result == null ) 
				result = new Vector();
			if (result.Count == vector1.Count)
				result.RemoveLastValue();

			int period = Math.Max(shift1, shift2);

			for ( int i = result.Count; i < vector1.Count; i++ ) {
				if (i < period)
					result.Add(float.NaN);
				else
					result.Add( -Math.Abs(vector1[i - shift1] - vector2[i - shift2]));
			}
			return result;
		}
	}
}
