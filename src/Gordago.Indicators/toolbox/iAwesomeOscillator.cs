using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iAwesomeOscillator:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Awesome Oscillator";
			this.ShortName = "AO";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("AO");
			fs.SetDrawPen(Color.Blue);
		}
	}

	[Function("AO")]
	public class AO : Function {
		private Function _funcSub, _funcMA;

		protected override void Initialize() {
			_funcMA = this.GetFunction("MA");
			_funcSub = this.GetFunction("sub");

			RegParameter(new ParameterInteger("PeriodFastMA", new string[]{"Fast MA", "Быстрая МА"}, 5, 1, 10000));
			RegParameter(new ParameterInteger("PeriodSlowMA", new string[]{"Slow MA", "Медленная МА"}, 34, 1, 10000));
			RegParameter(Function.CreateDefParam_MAType(1));
			RegParameter(Function.CreateDefParam_ApplyTo("Median"));
      RegParameter(new ParameterColor("ColorAO", new string[] { "Color", "Цвет" }, Color.Blue));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period1 = (int)parameters[0];
			int period2 = (int)parameters[1];

			result = analyzer.Compute(_funcSub,analyzer.Compute(_funcMA,period1,parameters[2],parameters[3]),analyzer.Compute(_funcMA,period2,parameters[2],parameters[3]));
			return result;
		}
	}

}
