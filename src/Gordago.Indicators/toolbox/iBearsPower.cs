using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iBearsPower: Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Bears Power";
			this.ShortName = "Bears";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("BearsPower");
			fs.SetDrawPen(Color.DodgerBlue);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);
		}
	}
	[Function("BearsPower")]
	public class BearsPower : Function {
		private Function _funcSub, _funcMA;

		protected override void Initialize() {
			SetShortName("Bears");
			_funcMA = GetFunction("MA");
			_funcSub = GetFunction("sub");

			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			RegParameter(_funcMA);
      RegParameter(new ParameterColor("ColorBearsPower", new string[] { "Color", "Цвет" }, Color.DodgerBlue));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector mares = analyzer.Compute(_funcMA,parameters[1],parameters[2],parameters[3]);
			return analyzer.Compute(_funcSub,parameters[0],mares);
		}
		
	}
}
