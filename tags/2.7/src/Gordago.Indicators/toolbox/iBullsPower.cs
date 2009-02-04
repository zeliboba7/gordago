using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iBullsPower:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "BullsPower";
			this.ShortName = "Bulls";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("BullsPower");
			fs.SetDrawPen(Color.DodgerBlue);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);
		}

	}

	[Function("BullsPower")]
	public class BullsPower : Function {
		private Function _funcSub, _funcMA;
		protected override void Initialize() {
			SetShortName("Bulls");
			_funcMA = GetFunction("MA");
			_funcSub = GetFunction("sub");

			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			RegParameter(_funcMA);
      RegParameter(new ParameterColor("ColorBulls", new string[] { "Color", "Цвет" }, Color.DodgerBlue));
		}


		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector mares = analyzer.Compute(_funcMA,parameters[1],parameters[2],parameters[3]);
			return analyzer.Compute(_funcSub,parameters[0],mares);
		}
	}

}
