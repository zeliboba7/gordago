using System;
using System.Reflection;

namespace Gordago.StockOptimizer2.Toolbox {
	[Function("parabolicsarold",typeof(IVector),typeof(IVector),typeof(float),typeof(float))]
	public class ParabolicSARold : Function {

		#region public class SarVector : Vector 
		public class SarVector : Vector {
			public SarVector() {
			}
			public SarVector(int capacity) : base(capacity) {
			}
			
			public float Step {
				get {
					return step;
				}
				set {
					step = value;
				}
			}
			
			public float High {
				get {
					return high;
				}
				set {
					high = value;
				}
			}
			
			public float Low {
				get {
					return low;
				}
				set {
					low = value;
				}
			}
			
			public bool IsLong {
				get {
					return isLong;
				}
				set {
					isLong = value;
				}
			}
			
			public int Start {
				get {
					return start;
				}
				set {
					start = value;
				}
			}

			public bool First {
				get {
					return first;
				}
				set {
					first = value;
				}
			}
			
			private float step = 0,high,low;
			private bool isLong;
			private int start = 1;
			private bool first = true;
		}
		#endregion
		
		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector low = (IVector)parameters[0];
			IVector high = (IVector)parameters[1];
			float param1 = (float)parameters[2];
			float param2 = (float)parameters[3];
			
			int size = Math.Min(low.Count,high.Count);
			
			if ( size < 2 )
				return null;
			
			SarVector sv = result as SarVector;
			
			if ( sv == null ) {
				sv = new SarVector(Math.Min(low.Capacity,high.Capacity));
				sv.Step = param1;
				sv.First = false;
				result = sv;
			}
			
			for ( int i = sv.Count + sv.Start; i < size; i++ ) {
				if ( result.Count == 0 ) {
					if ( high[i] > high[i - 1] && low[i] > low[i - 1] ) {
						sv.IsLong = true;
						sv.Step = param1;
						sv.High = high[i];
						sv.Low = low[i - 1];
						sv.First = true;
						result.Add(low[i - 1]);
					} else if ( high[i] < high[i - 1] && low[i] < low[i - 1] ) {
						sv.IsLong = false;
						sv.Step = param1;
						sv.High = high[i - 1];
						sv.Low = low[i];
						sv.First = true;
						result.Add(high[i - 1]);
					} else
						sv.Start++;
				} else {
					sv.First = false;
					if ( sv.High < high[i] )
						sv.High = high[i];
					if ( sv.Low > low[i] )
						sv.Low = low[i];
					
					if ( sv.IsLong ) {
						if ( high[i] > high[i - 1] && low[i] > low[i - 1] ) {
							if ( sv.Step < param2 )
								sv.Step += param1;
							else
								sv.Step = param2;
						}
						
						float sar = sv.Step * (sv.High - sv.Current) + sv.Current;
						if ( sar > low[i] ) {
							sv.IsLong = false;
							sv.Step = param1;
							result.Add(sv.High);
							sv.High = high[i];
							
							sv.Low = low[i];
							sv.First = true;
						} else
							result.Add(sar);
					} else {
						if ( low[i] < low[i - 1] && high[i] < high[i - 1] ) {
							if ( sv.Step < param2 )
								sv.Step += param1;
							else
								sv.Step = param2;
						}
						float sar = sv.Step * (sv.Low - sv.Current) + sv.Current ;
						if ( sar < high[i] ) {
							sv.IsLong = true;
							sv.Step = param1;
							result.Add(sv.Low);
							sv.Low = low[i];
							
							sv.High = high[i];
							sv.First = true;
						} else
							result.Add(sar);
					}
				}
			}

			if (!sv.First) {
				if (result.Count == 1) {
					if ( high.Current > high[high.Count - 2] && low.Current > low[low.Count - 2] ) {
						sv.IsLong = true;
						sv.Step = param1;
						sv.High = high.Current;
						sv.Low = low[low.Count - 2];
						result.Current = low[low.Count - 2];
						sv.First = true;
					} else if ( high.Current < high[high.Count - 2] && low.Current < low[low.Count - 2] ) {
						sv.IsLong = false;
						sv.Step = param1;
						sv.High = high[high.Count - 2];
						sv.Low = low.Current;
						result.Current = high[high.Count - 2];
						sv.First = true;
					}
				}
				else {
					if ( sv.High < high.Current )
						sv.High = high.Current;
					if ( sv.Low > low.Current )
						sv.Low = low.Current;
					
					if ( sv.IsLong ) {
						/*if ( high.Current > high[high.Count - 2] && low.Current > low[low.Count - 2] ) {
							if ( sv.Step < param2 )
								sv.Step += param1;
							else
								sv.Step = param2;
						}*/
						
						float sar = sv.Step * (sv.High - sv[sv.Count - 2]) + sv[sv.Count - 2];
						if ( sar > low.Current ) {
							sv.IsLong = false;
							sv.Step = param1;
							result.Current = sv.High;
							sv.High = high.Current;
							sv.Low = low.Current;
							sv.First = true;
						} else
							result.Current = sar;
					} else {
						/*if ( low.Current < low[low.Count - 2] && high.Current < high[high.Count - 2] ) {
							if ( sv.Step < param2 )
								sv.Step += param1;
							else
								sv.Step = param2;
						}*/
						float sar = sv.Step * (sv.Low - sv[sv.Count - 2]) + sv[sv.Count - 2];
						if ( sar < high.Current ) {
							sv.IsLong = true;
							sv.Step = param1;
							result.Current = sv.Low;
							sv.Low = low.Current;
							sv.High = high.Current;
							sv.First = true;
						} else
							result.Current = sar;
					}
				}
			}
			return result;
		}
	}
}
