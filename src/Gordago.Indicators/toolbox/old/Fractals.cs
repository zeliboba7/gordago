using System;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	[Function("FractalDown",typeof(IVector),typeof(IVector))]
	public class FractalDown: Function {
		public override IVector Compute(Analyzer	analyzer, object[]	parameters, IVector result) {
			IVector low = (IVector)parameters[0];

			if(result == null)
				result	= new Vector();
			if (result.Count == low.Count)
				result.RemoveLastValue();

			for (int i = result.Count; i < low.Count; i++){
				if (i >= 5 && low[i-3] < low[i-4] && low[i-4] < low[i-5] && 
					low[i-3] < low[i-2] && low[i-2] < low[i-1]){
					result.Add(low[i-3]);
				} else 
					result.Add(float.NaN);
				
			}
			return result;
		}
	}

	[Function("FractalUp",typeof(IVector),typeof(IVector))]
	public class FractalUp: Function {
		public override IVector Compute(Analyzer	analyzer, object[]	parameters, IVector result) {
			IVector high = (IVector)parameters[0];

			if(result == null)
				result	= new Vector();
			if (result.Count == high.Count)
				result.RemoveLastValue();

			for (int i = result.Count; i < high.Count; i++){
				if (i >= 5 && 
					high[i-3] > high[i-4] && high[i-4] > high[i-5] && 
					high[i-3] > high[i-2] && high[i-2] > high[i-1])
					result.Add(high[i-3]);
				else 
					result.Add(float.NaN);
			}
			return result;
		}
	}
}
