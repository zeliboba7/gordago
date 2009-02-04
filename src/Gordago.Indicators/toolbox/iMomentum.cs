using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iMomentum : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Momentum";
			this.ShortName = "Momentum";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("Momentum");
			fs.SetDrawPen(Color.Firebrick);
		}
	}
	[Function("Momentum")]
	public class Momentum : Function {
		
		protected override void Initialize() {
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 13, 1, 10000));
      RegParameter(new ParameterColor("ColorMomentum", new string[] { "Color", "Цвет" }, Color.Firebrick));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector = parameters[0] as IVector;
			int period = (int)parameters[1];
			
			if ( result == null )
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();
			
			for ( int i = result.Count; i < vector.Count; i++ ) {
				if (i<period)
					result.Add(float.NaN);
				else
					result.Add(vector[i] / vector[i - period] * 100);
			}
			return result;
		}
	}
}
