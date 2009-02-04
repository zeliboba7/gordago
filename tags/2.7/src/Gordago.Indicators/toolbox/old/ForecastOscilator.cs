using System;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	#region ForecastOscillatorOsc
	[Function("ForecastOscillatorOsc",typeof(int),typeof(int),typeof(int),typeof(IVector))]
	public class ForecastOscillatorOsc: Function {

		public override IVector Compute(Analyzer	analyzer,object[]	parameters,IVector	result) {
			int	regress	= (int)parameters[0];
			int	t3 = (int)parameters[1];
			float b = (float)parameters[2];

			IVector	Close	= (IVector)parameters[3];
			int	size	= Close.Count;

			if(size < regress)
				return null;

			if(result == null){
				result	= new Vector();
				result.ArrayShift = regress-1;
			}
			int rsize = result.Count;
			float current = 0;

			for (int pos = rsize+regress-1; pos < size; pos++){

				int length;
				float b2,b3,c1,c2,c3,c4,w1,w2,n,WT,sum,tmp,tmp2;

				b2=b*b; 
				b3=b2*b; 
				c1=-b3; 
				c2=(3*(b2+b3)); 
				c3=-3*(2*b2+b+b3); 
				c4=(1+3*b+b3+3*b2); 
				n=t3; 

				if (n<1) n=1; 
				n = 1 + 0.5F*(n-1); 
				w1 = 2 / (n + 1); 
				w2 = 1 - w1; 

				length=regress; 
				sum = 0; 
				for (int i = length; i > 0; i--) {
					tmp = length+1;
					tmp = tmp/3;
					tmp2 = i;
					tmp = tmp2 - tmp;
					sum = sum + tmp*Close[pos-length+i]; 
				}
				tmp = length;
				WT = sum*6/(tmp*(tmp+1)); 

				current = (Close[pos]-WT)/WT*100; 
				result.Add(current);
			}
			result.Current = current;
			return result;
		}
	}
	#endregion

	#region ForecastOscillatorOsct3
	[Function("ForecastOscillatorOsct3",typeof(int),typeof(int),typeof(int),typeof(IVector))]
	public class ForecastOscillatorOsct3: Function {

		public override IVector Compute(Analyzer	analyzer,object[]	parameters,IVector result) {
			int		regress	= (int)parameters[0];
			int		t3		= (int)parameters[1];
			float	b		= (float)parameters[2];

			IVector	Close	= (IVector)parameters[3];
			int	size	= Close.Count;

			if(size < regress)
				return null;

			if(result == null){
				result	= new Vector();
				result.ArrayShift = regress-1;
			}

			int rsize = result.Count;
			float current = 0;
			for (int pos = rsize+regress-1; pos < size; pos++){

				int length;
				float b2,b3,c1,c2,c3,c4,w1,w2,n,WT,forecastosc,sum,
					e1	= 0,
					e2	= 0,
					e3	= 0,
					e4	= 0,	
					e5	= 0,	
					e6	= 0,
					tmp,tmp2;

				b2=b*b; 
				b3=b2*b; 
				c1=-b3; 
				c2=(3*(b2+b3)); 
				c3=-3*(2*b2+b+b3); 
				c4=(1+3*b+b3+3*b2); 
				n=t3; 

				if (n<1) n=1; 
				n = 1 + 0.5F*(n-1); 
				w1 = 2 / (n + 1); 
				w2 = 1 - w1; 

				length=regress; 
				sum = 0; 
				for (int i = length; i > 0; i--) {
					tmp = length+1;
					tmp = tmp/3;
					tmp2 = i;
					tmp = tmp2 - tmp;
					sum = sum + tmp*Close[pos-length+i]; 
				}
				tmp = length;
				WT = sum*6/(tmp*(tmp+1)); 

				forecastosc=(Close[pos]-WT)/WT*100; 

				e1 = w1*forecastosc + w2*e1; 
				e2 = w1*e1 + w2*e2; 
				e3 = w1*e2 + w2*e3; 
				e4 = w1*e3 + w2*e4; 
				e5 = w1*e4 + w2*e5; 
				e6 = w1*e5 + w2*e6; 

				current = c1*e6 + c2*e5 + c3*e4 + c4*e3; 

				result.Add(current);
			}
			return result;
		}
	}
	#endregion

	#region ForecastOscillatorHiSig
	[Function("ForecastOscillatorHiSig",typeof(int),typeof(int),typeof(int),typeof(IVector))]
	public class ForecastOscillatorHiSig: Function {

		private Function osc_func	= null;
		private Function osct3_func	= null;

		public override IVector Compute(Analyzer	analyzer,object[]	parameters,IVector result) {
			int		regress	= (int)parameters[0];
			int		t3		= (int)parameters[1];
			float	b		= (float)parameters[2];

			IVector	Close	= (IVector)parameters[3];
			int	size	= Close.Count;

			if (size < regress)
				return null;

			if(result == null){
				result	= new Vector();
				result.ArrayShift = regress-1;
			}

			if(osc_func == null || osct3_func == null) {
				osc_func	= analyzer["ForecastOscillatorOsc"];
				osct3_func	= analyzer["ForecastOscillatorOsct3"];
				if(osc_func == null || osct3_func == null)
					return null;
			}

			IVector osc	= analyzer.Compute(osc_func, regress, t3, b, Close);
			IVector osct3= analyzer.Compute(osct3_func,regress,t3,b,Close);

			if(osc == null || osct3 == null)
				return null;

			if(osc.Count < 4)
				return null;

			int rsize = result.Count+3;
			float current = 0;

			for (int pos = rsize+regress-1; pos < size; pos++){
				int npos = pos-regress+1; 

				if(osc[npos-1] > osct3[npos-2] && osc[npos-2] <= osct3[npos-3] && osct3[npos-1]<0)
					current = osct3[npos] - 0.05F;
				else
					current = 0;	
				result.Add(current);
			}
			result.Current = 0;
			return result;
		}

	}
	#endregion

	#region ForecastOscillatorLoSig
	[Function("ForecastOscillatorLoSig",typeof(int),typeof(int),typeof(int),typeof(IVector))]
	public class ForecastOscillatorLoSig: Function {
		private Function osc_func	= null;
		private Function osct3_func	= null;

		public override IVector Compute(Analyzer	analyzer,object[]	parameters,IVector result) {
			int		regress	= (int)parameters[0];
			int		t3		= (int)parameters[1];
			float	b		= (float)parameters[2];

			IVector	Close	= (IVector)parameters[3];
			int	size	= Close.Count;

			if (size < regress)
				return null;

			if(result == null){
				result	= new Vector();
				result.ArrayShift = regress-1;
			}

			if(osc_func == null || osct3_func == null) {
				osc_func	= analyzer["ForecastOscillatorOsc"];
				osct3_func	= analyzer["ForecastOscillatorOsct3"];
				if(osc_func == null || osct3_func == null)
					return null;
			}

			IVector osc	= analyzer.Compute(osc_func,regress,t3, b,Close);
			IVector osct3= analyzer.Compute(osct3_func,regress,t3, b,Close);

			if(osc == null || osct3 == null)
				return null;

			if(osc.Count < 4)
				return null;

			int rsize = result.Count+3;
			float current = 0;
			for (int pos = rsize+regress-1; pos < size; pos++){
				int npos = pos-regress+1; 

				if(osc[npos-1] < osct3[npos-2] && osc[npos-2] >= osct3[npos-3] && osct3[npos-1]>0)
					current = Convert.ToSingle(osct3[npos] + 0.05);
				else
					current = 0;
				result.Add(current);
			}
			return result;

		}
	}
	#endregion
}
