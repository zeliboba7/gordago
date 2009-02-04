using System;
using Gordago.Analysis;

#if DEBUGd
namespace Gordago.StockOptimizer2.Toolbox {
	
	public class iTemp: Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = "";
			this.Name = "Temp";
			this.ShortName = "Temp";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("Temp");
		}
	}

	[Function("Temp")]
	public class Temp: Function{
		protected override void Initialize() {

		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector vector = parameters[0] as IVector;

			if (result == null)
				result = new Vector();


			if (result.Count == vector.Count)
				result.RemoveLastValue();
			
			for ( int i = result.Count; i < vector.Count; i++ ) {
				result.Add(float.NaN);
			}
			return result;
		}


	}
}
#endif