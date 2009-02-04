using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iDeMarker: Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "DeMarker";
			this.ShortName = "DeMarker";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("DeMarker");
			fs.SetDrawPen(Color.Brown);
		}
	}
	[Function("DeMax")]
	public class DeMax : Function {
		protected override void Initialize() { }


		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector1 = parameters[0] as IVector;
			
			if ( result == null )
				result = new Vector();
			
			if (result.Count == vector1.Count)
				result.RemoveLastValue();

			for ( int i = result.Count; i < vector1.Count; i++ ) {
				if (i == 0)
					result.Add(float.NaN);
				else if ( vector1[i] > vector1[i - 1] )
					result.Add(vector1[i] - vector1[i - 1]);
				else
					result.Add(0);
			}
			return result;
		}
	}

	[Function("DeMin")]
	public class DeMin : Function {
		protected override void Initialize() {}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector2 = parameters[0] as IVector;
			
			if ( result == null )
				result = new Vector();
			if (result.Count == vector2.Count)
				result.RemoveLastValue();

			for ( int i = result.Count; i < vector2.Count; i++ ) {
				if (i == 0)
					result.Add(float.NaN);
				else if ( vector2[i] < vector2[i - 1] )
					result.Add(vector2[i - 1] - vector2[i]);
				else
					result.Add(0);
				
			}
			return result;
		}
	}

	[Function("DeMarker")]
	public class DeMarker : Function {
		private Function _funcDiv, _funcMA, _funcAdd,_funcDeMax, _funcDeMin;
		protected override void Initialize() {
			_funcDiv = GetFunction("div");
			_funcMA = GetFunction("MA");
			_funcAdd = GetFunction("add");
			_funcDeMax = GetFunction("DeMax");
			_funcDeMin = GetFunction("DeMin");

			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);

			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 13, 1, 10000));
			RegParameter(Function.CreateDefParam_MAType(1));
      RegParameter(new ParameterColor("ColorDeMarker", new string[] { "Color", "Цвет" }, Color.Brown));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[2];
			result = analyzer.Compute(_funcDiv,analyzer.Compute(_funcMA,parameters[2],parameters[3],analyzer.Compute(_funcDeMax,parameters[1])),analyzer.Compute(_funcAdd,analyzer.Compute(_funcMA,parameters[2],parameters[3],analyzer.Compute(_funcDeMax,parameters[1])),analyzer.Compute(_funcMA,parameters[2],parameters[3],analyzer.Compute(_funcDeMin,parameters[0]))));
			return result;
		}
	}

}
