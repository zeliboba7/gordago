namespace Gordago.StockOptimizer2.Toolbox {
	using System;
	using System.Drawing;
	using Gordago.Analysis;

	public class iAcceleratorOscillator:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Accelerator Oscillator";
			this.ShortName = "AC";
			this.SetImage("i.gif");
      this.DecimalDigits = 6;

			FunctionStyle fs = RegFunction("AC");
			fs.SetDrawPen(Color.Blue);
		}
	}
	
	[Function("AC")]
	public class AC : Function {
		private Function _funcMA, _funcSub, _funcAO;

		protected override void Initialize() {
			_funcMA = this.GetFunction("MA");
			_funcSub= this.GetFunction("sub");
			_funcAO = this.GetFunction("AO");

			RegParameter(new ParameterInteger("PeriodFast", new string[]{"Fast period", "Быстрый период"}, 5, 1, 10000));
			RegParameter(new ParameterInteger("PeriodSlow", new string[]{"Slow period", "Медленный период"}, 34, 1, 10000));
			RegParameter(new ParameterInteger("PeriodSmooth", new string[]{"Smooth period", "Период сглаживания"}, 5, 1, 10000));
			RegParameter(Function.CreateDefParam_MAType(0));
			RegParameter(Function.CreateDefParam_ApplyTo("Median"));
      RegParameter(new ParameterColor("ColorAC", new string[] { "Color", "Цвет" }, Color.Blue));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {

			int fastperiod = (int)parameters[0];
			int slowperiod = (int)parameters[1];
			int smoothperiod = (int)parameters[2];

			IVector aores = analyzer.Compute(_funcAO,fastperiod,slowperiod,parameters[3],parameters[4]);
			IVector mares = analyzer.Compute(_funcMA,smoothperiod,parameters[3],aores);

			result = analyzer.Compute(_funcSub, aores, mares);
			return result;
		}

	}
}
