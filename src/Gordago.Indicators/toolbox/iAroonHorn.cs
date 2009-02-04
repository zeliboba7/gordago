using System;
using System.Drawing;

using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iAroonHorn : Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Aroon Horn";
			this.ShortName = "AroonHorn";
			this.SetImage("i.gif");

			RegFunction("AroonHornUp").SetDrawPen(Color.DarkBlue);
			RegFunction("AroonHornDown").SetDrawPen(Color.DarkGreen);
		}
	}
	[Function("AroonHornUp")]
	public class AroonHornUp: Function {
		private Function _ah;

		protected override void Initialize() {
			_ah = GetFunction("__aroonhorn");
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 10, 1, 10000));
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
      RegParameter(new ParameterColor("ColorAHUp", new string[] { "Color", "Цвет" }, Color.DarkBlue));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector high = (IVector)parameters[1];

			return analyzer.Compute(_ah, period, high, 0);
		}
	}

	[Function("AroonHornDown")]
	public class AroonHornDown: Function {
		private Function _ah;

		protected override void Initialize() {
			_ah = GetFunction("__aroonhorn");
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 10, 1, 10000));

			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
      RegParameter(new ParameterColor("ColorAHDown", new string[] { "Color", "Цвет" }, Color.DarkGreen));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector low = (IVector)parameters[1];


			return analyzer.Compute(_ah, period, low, 1);
		}
	}

	[Function("__AroonHorn")]
	public class __AroonHorn: Function {
		protected override void Initialize() {}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {

			int period = (int)parameters[0];
			IVector vector = (IVector)parameters[1];
			int numline = (int)parameters[2];
			
			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i = result.Count; i < vector.Count; i++){
				if (i<period){
					result.Add(float.NaN);
				}else{
					float temp = vector[i];
					float pos = 0;  
					for (int j = i; j >=i-period+1; j--){
						if (numline == 0){
							if (vector[j] > temp) pos = i-j;
						
							temp = Math.Max(vector[j], temp);
						}else{
							if (vector[j] < temp) pos = i-j;
						
							temp = Math.Min(vector[j], temp);
						}
					}
					float d = pos / period;

					result.Add(100 - (d)*100);
				}
			}
			return result;
		}
	}

}
