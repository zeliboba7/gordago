using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iMoneyFlowIndex : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Money Flow Index";
			this.ShortName = "MFI";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("MFI");
			fs.SetDrawPen(Color.Chocolate);
		}
	}
	[Function("MFI")]
	public class MFI : Function {

		protected override void Initialize() {
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 13, 1, 10000));
			RegParameter(Function.CreateDefParam_ApplyTo("Typical"));
			ParameterVector pvvolume = new ParameterVector("VectorVolume", "Volume");
			pvvolume.Visible = false;
			RegParameter(pvvolume);
      RegParameter(new ParameterColor("ColorMFI", new string[] { "Color", "Цвет" }, Color.Chocolate));
    }
		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			IVector vector = parameters[1] as IVector;
			IVector volume = parameters[2] as IVector;
			
			if ( result == null )
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();
			
			float pmf,nmf;
			for ( int i = result.Count+1; i < vector.Count+1; i++ ) {
				if (i < period+1){
					result.Add(float.NaN);
				}else{
					pmf = 0; nmf = 0;
					for ( int j = i - period; j < i; j++ ) {
						if ( vector[j] > vector[j - 1] )
							pmf += vector[j] * volume[j];
						else if ( vector[j] < vector[j - 1] )
							nmf += vector[j] * volume[j];
					}
					if ( nmf == 0 )
						result.Add(100);
					else if ( pmf == 0 )
						result.Add(0);
					else
						result.Add(100 - 100f / (1 + pmf / nmf));
				}
			}
			return result;
		}
	}
}
