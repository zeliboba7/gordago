using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iATR : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Average True Range";
			this.ShortName = "ATR";
			this.SetImage("i.gif");

			FunctionStyle fsma = RegFunction("ATR");
			fsma.SetDrawPen(Color.Gold);
		}
	}

	[Function("TR")]
	public class TR : Function {
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
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector1 = (IVector)parameters[0];
			IVector vector2 = (IVector)parameters[1];
			IVector vector3 = (IVector)parameters[2]; 

			if ( result == null )
				result = new Vector();
			if (result.Count == vector1.Count)
				result.RemoveLastValue();

			for ( int i = result.Count; i < vector1.Count; i++ ) {
				if ( result.Count == 0 ) {
					result.Add(vector2[i] - vector1[i]);
				} else {
					float temp2 = float.MinValue;
					float temp = vector2[i] - vector1[i];;
					if ( temp > temp2 )
						temp2 = temp;
					temp = vector2[i] - vector3[i - 1];
					if ( temp > temp2 )
						temp2 = temp;
					temp = vector3[i - 1] - vector1[i];
					if ( temp > temp2 )
						temp2 = temp;
					result.Add(temp2); 
				}
			}
			return result;
		}
	}

	[Function("ATR")]
	public class ATR : Function {
		private Function _funcTR, _funcMA;

		protected override void Initialize() {
			_funcTR = GetFunction("TR"); 
			_funcMA = GetFunction("MA");
			this.RegParameter(_funcTR);
			this.RegParameter(_funcMA.CloneParameter("Period"));
      RegParameter(new ParameterColor("ColorATR", new string[] { "Color", "Цвет" }, Color.Gold));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			return analyzer.Compute(_funcMA,parameters[3],0,analyzer.Compute(_funcTR,parameters[0],parameters[1],parameters[2]));
		}

	}
}
