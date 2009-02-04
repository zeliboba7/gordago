using System;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	[Function("HMA",typeof(IVector),typeof(int))]
	public class HMA: Function {
		
		private Function _funcma;
		private Function _funcsmul;
		private Function _funcsub;

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector vector = (IVector)parameters[0];
			
			int period = (int)parameters[1];

			if (_funcma == null || _funcsmul == null || _funcsub == null){
				_funcma = analyzer["ma"];
				_funcsmul = analyzer["smul"];
				_funcsub = analyzer["sub"];
			}

			int period1 = Convert.ToInt32(Math.Floor(period/2));
			
			IVector resma1 = analyzer.Compute(_funcma, period1, 2, vector);
			IVector resma2 = analyzer.Compute(_funcma, period,  2, vector);
			IVector ressmul = analyzer.Compute(_funcsmul, resma1, (float)2);
			IVector ressub = analyzer.Compute(_funcsub, ressmul, resma2);
			
			int period2 = Convert.ToInt32(Math.Floor(Math.Sqrt(period)));

			result = analyzer.Compute(_funcma, period2, 2, ressub);
			return result;
		}
	}
}
