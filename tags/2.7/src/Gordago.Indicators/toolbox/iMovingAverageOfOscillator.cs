using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iMovingAverageOfOscillator:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Moving Average of Oscillator";
			this.ShortName = "MAO";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("MAO");
			fs.SetDrawPen(Color.ForestGreen);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);
		}
	}
	[Function("MAO")]
	public class MAO : Function {
		private Function _funcBaseMACD, _funcSub,_funcSignalMACD;
		protected override void Initialize() {
			_funcBaseMACD = GetFunction("BaseMACD");
			_funcSignalMACD = GetFunction("SignalMACD");
			_funcSub = GetFunction("sub");
			RegParameter(new ParameterInteger("PeriodFastMA", new string[]{"Fast MA", "Быстрая МА"}, 12, 1, 10000));
			RegParameter(new ParameterInteger("PeriodSlowMA", new string[]{"Slow MA", "Медленная МА"}, 26, 1, 10000));
			RegParameter(new ParameterInteger("PeriodSmoothMA", new string[]{"Smooth", "Сглаживание"}, 9, 1, 10000));
			RegParameter(Function.CreateDefParam_MAType(1));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      RegParameter(new ParameterColor("ColorMAO", new string[] { "Color", "Цвет" }, Color.ForestGreen));
    }


		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {

			IVector resbmacd = analyzer.Compute(_funcBaseMACD,parameters[0],parameters[1],parameters[3],parameters[4]);
			IVector ressmacd = analyzer.Compute(_funcSignalMACD,parameters[0],parameters[1],parameters[3],parameters[4],parameters[2]);
			
			result = analyzer.Compute(_funcSub,resbmacd,ressmacd);
			return result;
		}
	}
}
