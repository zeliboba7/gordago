using System;
using System.Reflection;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	

	[Function("weekofday",typeof(int),typeof(int),typeof(int))]
	public class WeekOfDay: Function {
		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			if (result == null)
				result = new Vector();

			return null;
		}

	}


	[Function("weektime",typeof(int),typeof(int),typeof(int))]
	public class WeekTime: Function{
		public class WTVector: Vector{
			private bool _flag;
			public bool Flag{
				get{return this._flag;}
				set{this._flag = value;}
			}
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			WTVector wt = result as WTVector;

			if ( wt == null ) {
				wt = new WTVector();
				wt.Flag = false;
				wt.Add(0); 
				result = wt;
			}
			int dayofweek = (int)parameters[0];
			int hour = (int)parameters[1];
			int minute = (int)parameters[2];

			DateTime dt = new DateTime(analyzer.Time * 10000000);
			result.Current = 0;
			if (!wt.Flag){
				if ((int)(dt.DayOfWeek) == dayofweek && dt.Hour == hour && dt.Minute >= minute){
					result.Current = 1;
					wt.Flag = true;
				}
			}else{
				if ((int)(dt.DayOfWeek) <= dayofweek && dt.Hour <= hour && dt.Minute <= minute){
					wt.Flag = false;
				}
			}
			return result;
		}
	}
}
