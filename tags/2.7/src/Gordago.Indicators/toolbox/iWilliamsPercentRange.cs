using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iWilliamsPercentRange : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Williams Percent Range";
			this.ShortName = "WPR";
			this.SetImage("i.gif");

			RegFunction("WPR").SetDrawPen(Color.ForestGreen);
		}
	}

	[Function("WPR")]
	public class WPR : Function {
		protected override void Initialize() {
			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			ParameterVector pvclose = new ParameterVector("VectorClose", "Close");
			pvclose.Visible = false;
			RegParameter(pvclose);
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 5, 1, 10000));
      RegParameter(new ParameterColor("ColoriWPR", new string[] { "Color", "Цвет" }, Color.ForestGreen));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector low = parameters[0] as IVector; 
			IVector high = parameters[1] as IVector;
			IVector close = parameters[2] as IVector;
			int period = (int)parameters[3];
			
			if ( result == null )
				result = new Vector();
			if (result.Count == close.Count)
				result.RemoveLastValue();

			for ( int i = result.Count + 1; i < close.Count + 1; i++ ) {
				if (i<period){
					result.Add(float.NaN);
				}else{
					float h = float.MinValue;
					float l = float.MaxValue;
					for ( int j = i - period; j < i; j++ ) {
						if ( h < high[j] )
							h = high[j];
						if ( l > low[j] )
							l = low[j];
					}
					result.Add( -100f * (h - close[i - 1]) / (h - l) );
				}
			}
			return result;
		}
	}
}
