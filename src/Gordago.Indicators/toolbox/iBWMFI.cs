using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iBWMFI :Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.Name = "Bill Williams Market Facilitation Index";
			this.GroupName = GroupNames.Relative;
			this.ShortName = "BWMFI";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("BWMFI");
			fs.SetDrawPen(Color.Indigo);
		}
	}

	[Function("BWMFI")]
	public class BWMFI : Function {
		private Function _funcSub, _funcDiv, _funcSmul;
		protected override void Initialize() {
			_funcSub = this.GetFunction("sub");
			_funcDiv = this.GetFunction("div");
			_funcSmul = this.GetFunction("smul");

			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			ParameterVector pvvolume = new ParameterVector("VectorVolume", "Volume");
			pvvolume.Visible = false;
			RegParameter(pvvolume);

      RegParameter(new ParameterColor("ColorBWMFI", new string[] { "Color", "Цвет" }, Color.Indigo));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			return analyzer.Compute(_funcDiv,analyzer.Compute(_funcSmul,analyzer.Compute(_funcSub,parameters[1],parameters[0]),10000f),parameters[2]);
		}
	}

}
