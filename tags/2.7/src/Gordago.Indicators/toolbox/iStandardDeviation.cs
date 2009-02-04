using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	public class iStandardDeviation: Indicator{

		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Standard Deviation";
			this.ShortName = "StdDev";
			this.SetImage("i.gif");


			FunctionStyle fsStdDev = RegFunction("StdDev");
			fsStdDev.SetDrawPen(Color.GreenYellow);
		}
	}

	[Function("StdDev")]
	public class StdDev : Function {

		protected override void Initialize() {
			this.RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			this.RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 13, 1, 10000));
      RegParameter(new ParameterColor("ColorStdDev", new string[] { "Color", "Цвет" }, Color.GreenYellow));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector vector = parameters[0] as IVector;
			int period = (int)parameters[1];
			
			if ( period == 1 )
				return vector;
			
			if ( result == null )
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			float sma = 0, sd = 0;
			for ( int i = result.Count+1; i < vector.Count + 1; i++ ) {
				if (i < period){
					result.Add(float.NaN);
				} else {
					sma = 0;
					for ( int j = i - period; j < i; j++ )
						sma += vector[j];
					sma /= period;
					sd=0;
					for ( int j = i - period; j < i; j++ )
						sd += (sma - vector[j]) * (sma - vector[j]);
					result.Add((float)Math.Sqrt(sd / period));
				}
			}
			return result;
		}
	}
}
