using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iLinearRegression : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Absolute;
			this.Name = "Linear Regression";
			this.ShortName = "LineReg";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("linereg").SetDrawPen(Color.Red);
		}
	}
	[Function("LineReg")]
	public class LineReg : Function {
		protected override void Initialize() {
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 20, 1, 10000));
      RegParameter(new ParameterColor("Color", new string[] { "Color", "Цвет" }, Color.Red));
    }
		
		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector close = parameters[0] as IVector;
			int period = (int)parameters[1];

			if (result == null) 
				result = new Vector();
			if (result.Count == close.Count)
				result.RemoveLastValue();

			for (int i = result.Count; i < close.Count; i++) {
				if (i<period)
					result.Add(float.NaN);
				else{
					float sum = 0;
					for (int j = period; j>=1; j--) {
						sum = sum + (j-(period+1)/3)*close[i-period+j];
					}
					result.Add(sum*6/(period*(period+1)));
				}
			}
			return result;
		}
	}
}
