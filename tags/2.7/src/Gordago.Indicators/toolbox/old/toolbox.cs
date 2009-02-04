using System;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	#region public class RawRVI : Function 
	[Function("rawrvi",typeof(IVector),typeof(IVector),typeof(IVector),typeof(IVector))]
	public class RawRVI : Function {
		private Function _sub, _div;

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			if ( _sub == null || _div == null ) {
				_sub = analyzer["sub"];
				_div = analyzer["div"];
			}

			IVector open = (IVector)parameters[0];
			IVector low = (IVector)parameters[1];
			IVector high = (IVector)parameters[2];
			IVector close = (IVector)parameters[3];

			IVector resSub1 = analyzer.Compute(_sub,close,open);
			IVector resSub2 = analyzer.Compute(_sub,high,low);

			result = analyzer.Compute(_div,resSub1,resSub2);

			return result;
		}
	}
	#endregion

	#region public class BaseRVI : Function 
	[Function("baservi",typeof(IVector),typeof(IVector),typeof(IVector),typeof(IVector),typeof(int))]
	public class BaseRVI : Function {
		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			if ( _ma == null || _rawrvi == null ) {
				_ma = analyzer["ma"];
				_rawrvi = analyzer["rawrvi"];
			}
			IVector open = (IVector)parameters[0];
			IVector low = (IVector)parameters[1];
			IVector high = (IVector)parameters[2];
			IVector close = (IVector)parameters[3];
			int period = (int)parameters[4];
			
			IVector resRawRVI = analyzer.Compute(_rawrvi,open,low,high,close);
			result = analyzer.Compute(_ma, period, 1, resRawRVI);

			return result;
		}
		
		private Function _ma, _rawrvi;
	}
	#endregion

	#region public class SignalRVI : Function 
	[Function("signalrvi",typeof(IVector),typeof(IVector),typeof(IVector),typeof(IVector),typeof(int))]
	public class SignalRVI : Function {
		private Function _ma, baservi;
		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			if ( _ma == null || baservi == null ) {
				_ma = analyzer["ma"];
				baservi = analyzer["baservi"];
			}
			return analyzer.Compute(_ma,4,2,analyzer.Compute(baservi,parameters[0],parameters[1],parameters[2],parameters[3],parameters[4]));
		}
	}
	#endregion
}
