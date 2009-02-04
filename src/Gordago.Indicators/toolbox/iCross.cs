using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iCross: Indicator {
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Signal indicators";
			this.Name = "Cross";
			this.ShortName = "Cross";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;
			this.SetCustomDrawingIndicator(typeof (iCrossCustomDrawing));
			
			RegFunction("Cross");
		}
	}

	[Function("Cross")]
	public class Cross: Function{
		Function _funcMA;
		protected override void Initialize() {
			_funcMA = this.GetFunction("MA");
			this.RegParameter(new ParameterInteger("SlowPeriod", 20, 1, 100000));
			this.RegParameter(new ParameterInteger("FastPeriod", 5, 1, 100000));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));

			ParameterVector pvhigh = new ParameterVector("High", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);

			ParameterVector pvlow = new ParameterVector("Low", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
      RegParameter(new ParameterColor("ColorCross", new string[] { "Color", "÷‚ÂÚ" }, Color.Brown));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int slowPeriod = (int)parameters[0];
			int fastPeriod = (int)parameters[1];
			IVector vector = (IVector)parameters[2];
			IVector high = (IVector)parameters[3];
			IVector low = (IVector)parameters[4];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			IVector maSlow = analyzer.Compute(_funcMA, slowPeriod, 1, vector);
			IVector maFast = analyzer.Compute(_funcMA, fastPeriod, 1, vector);

			for (int i=result.Count;i<vector.Count;i++){
				if (i == 0){
					result.Add(float.NaN);
				}else{
					float maSPrevious = maSlow[i];
					float maSCurrent = maSlow[i-1];
					float maFPrevious = maFast[i];
					float maFCurrent= maFast[i-1];

					if (maFPrevious < maSPrevious && maFCurrent > maSCurrent)
						result.Add( high[i] + 5*analyzer.Point);
					else if (maFPrevious > maSPrevious && maFCurrent < maSCurrent ) 
						result.Add( low[i] - 5*analyzer.Point);
					else 
						result.Add(float.NaN);
				}
			}
			return result;
		}
	}
}
