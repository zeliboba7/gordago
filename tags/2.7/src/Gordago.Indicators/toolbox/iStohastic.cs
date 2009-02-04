using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iStochastic : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Stochastic";
			this.ShortName = "Stochastic";
			this.SetImage("i.gif");

			RegFunction("StochK").SetDrawPen(Color.ForestGreen);
			RegFunction("StochD").SetDrawPen(Color.GreenYellow);
		}
	}

	[Function("StochK")]
	public class K : Function {
		protected override void Initialize() {
			this.SetShortName("StochK");
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
			RegParameter(Function.CreateDefParam_MAType(1));
			RegParameter(new ParameterInteger("DelayK", new string[]{"Delay K", "Замедление K"}, 3, 1, 10000));
      RegParameter(new ParameterColor("ColorStochK", new string[] { "Color", "Цвет" }, Color.ForestGreen));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector low = parameters[0] as IVector;
			IVector high = parameters[1] as IVector;
			IVector close = parameters[2] as IVector;
			int period = (int)parameters[3];
			int delayK = (int)parameters[5];
			
			if ( result == null )
				result = new Vector();
      if (result.Count == low.Count)
				result.RemoveLastValue();

      for (int i = result.Count + 1; i < high.Count + 1; i++) {
				if (i < period + delayK){
					result.Add(float.NaN);
				}else{
					float a = 0, b = 0;
					for ( int k = 0; k < delayK; k++ ) {
						float h = float.MinValue;
						float l = float.MaxValue;
						for ( int j = i - k - period; j < i - k; j++ ) {
							if ( h < high[j] )
								h = high[j];
							if ( l > low[j] )
								l = low[j];
						}
						a += close[i - k - 1] - l;
						b += h - l;
					}
					result.Add(100f * a / b);
				}
			}

			return result;
		}
	}

	[Function("StochD")]
	public class D : Function {
				
		private Function _funcStochK, _funcMA;

		protected override void Initialize() {
			_funcStochK = GetFunction("StochK");
			_funcMA = GetFunction("MA");
			this.RegParameter(_funcStochK);
			RegParameter(new ParameterInteger("DelayD", new string[]{"Delay D", "Замедление D"}, 3, 1, 10000));
      RegParameter(new ParameterColor("ColorStochD", new string[] { "Color", "Цвет" }, Color.GreenYellow));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[6];
			IVector resk = analyzer.Compute(_funcStochK,parameters[0],parameters[1],parameters[2],parameters[3],parameters[4],parameters[5]);

			result = analyzer.Compute(_funcMA,period,parameters[4],resk);
			return result;
		}
	}
}
