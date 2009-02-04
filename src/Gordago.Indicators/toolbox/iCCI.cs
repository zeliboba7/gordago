using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iCCI: Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.Name = "Commodity Channel Index";
			this.GroupName = GroupNames.Relative;
			this.ShortName = "CCI";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("cci");
			fs.SetDrawPen(Color.Brown);
		}
	}

	[Function("CCI")]
	public class CCI : Function {
		protected override void Initialize() {
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 13, 1, 10000));
			RegParameter(Function.CreateDefParam_ApplyTo("Typical"));
      RegParameter(new ParameterColor("ColorCCI", new string[] { "Color", "Цвет" }, Color.Brown));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			IVector vector1 = parameters[1] as IVector;
			
			if ( result == null )
				result = new Vector();

			if (result.Count == vector1.Count)
				result.RemoveLastValue();

			for ( int i = result.Count+1; i < vector1.Count + 1; i++ ) {
				if (i < period){
					result.Add(float.NaN);
				}else{

					double sma = 0;
					for ( int k = i - period; k < i; k++ )
						sma += vector1[k];
					sma /= period; 

					double md = 0;
					for ( int j = i - period; j < i; j++ )
						md += Math.Abs(vector1[j] - sma);
					md /= period;
					if ( md == 0 )
						result.Add(0);
					else
						result.Add( (float)((vector1[i - 1] - sma) / ( 0.015 * md)) );
				}
			}
			return result;
		}
	}
}
