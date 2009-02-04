using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	
	[Function("LowestIndex")]
	public class LowestIndex : Function {
		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 10, 1, 100000));
			this.RegParameter(Function.CreateDefParam_ApplyTo("Low"));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector vector = (IVector)parameters[1];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<vector.Count;i++){
				int mperiod = Math.Min(i, period);
				
				float min = vector[i];
				int minindex = i;
				for (int ii=i; ii > i-mperiod; ii--){
					if (vector[ii] < min){
						min = vector[ii];
						minindex = ii;
					}
				}
				result.Add(minindex);
			}
			return result;
		}
	}

	public class iLowest: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "Lowest";
			this.ShortName = "Lowest";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;
			
			RegFunction("Lowest").SetDrawPen(Color.DarkBlue);
		}
	}

	[Function("Lowest")]
	public class Lowest : Function {
		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 10, 1, 100000));
			this.RegParameter(Function.CreateDefParam_ApplyTo("Low"));
      RegParameter(new ParameterColor("ColorLowest", new string[] { "Color", "Цвет" }, Color.DarkBlue));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector vector = (IVector)parameters[1];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<vector.Count;i++){
				int mperiod = period;
				if (period > i) mperiod = i;

				float minvalue ;
				minvalue = vector[i];
				for (int ii=i; ii > i-mperiod; ii--)
					minvalue = Math.Min(vector[ii], minvalue);
				result.Add(minvalue);
			}
			return result;
		}
	}

	[Function("HighestIndex")]
	public class HighestIndex : Function {
		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 10, 1, 100000));
			this.RegParameter(Function.CreateDefParam_ApplyTo("High"));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector vector = (IVector)parameters[1];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<vector.Count;i++){
				int mperiod = Math.Min(i, period);
				
				float max = vector[i];
				int maxindex = i;
				for (int ii=i; ii > i-mperiod; ii--){
					if (vector[ii] > max){
						max = vector[ii];
						maxindex = ii;
					}
				}
				result.Add(maxindex);
			}
			return result;
		}
	}

	public class iHighest: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "Highest";
			this.ShortName = "Highest";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;
			
			RegFunction("Highest").SetDrawPen(Color.DarkRed);
		}
	}

	[Function("Highest")]
	public class Highest : Function {
		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 10, 1, 100000));
			this.RegParameter(Function.CreateDefParam_ApplyTo("High"));
      RegParameter(new ParameterColor("ColorHighest", new string[] { "Color", "Цвет" }, Color.DarkRed));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector vector = (IVector)parameters[1];

			if (result == null)
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<vector.Count;i++){
				int mperiod = period;
				if (period > i) mperiod = i;

				float maxvalue = vector[i];
				for (int ii=i; ii > i-mperiod; ii--)
					maxvalue = Math.Max(vector[ii], maxvalue);
				result.Add(maxvalue);
			}
			return result;
		}
	}

}

