using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iZigZag: Indicator {
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Signal indicators";
			this.Name = "ZigZag";
			this.ShortName = "ZigZag";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;
			// this.SetCustomDrawingIndicator(typeof (iCrossCustomDrawing));
			
			RegFunction("ZigZag");
		}
	}

	public class ZigZag: Function{
		Function _funcLowest, _funcHighest;
		protected override void Initialize() {

			_funcLowest = this.GetFunction("Lowest");
			_funcHighest = this.GetFunction("Highest");

			this.RegParameter(new ParameterInteger("Depth", 12, 1, 10000));
			this.RegParameter(new ParameterInteger("Deviation", 5, 1, 10000));
			this.RegParameter(new ParameterInteger("Backstep", 3, 1, 10000));

			ParameterVector pvlow = new ParameterVector("Low", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);

			ParameterVector pvhigh = new ParameterVector("High", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
		}

		private class ZigZagVector: Vector{
			public float LastLow = 0;
			public float LastHigh = 0;
			public Vector ResultLow;
			public Vector ResultHigh;
			public ZigZagVector(){
				ResultLow = new Vector();
				ResultHigh = new Vector();
			}
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int depth = (int)parameters[0];
			int deviation = (int)parameters[1];
			int backstep = (int)parameters[2];
			IVector low = (IVector)parameters[3];
			IVector high = (IVector)parameters[4];

			if (result == null)
				result = new ZigZagVector();

			ZigZagVector zzresult = result as ZigZagVector;

      if (result.Count == low.Count) {
				zzresult.RemoveLastValue();
				zzresult.ResultLow.RemoveLastValue();
				zzresult.ResultHigh.RemoveLastValue();
			}

			IVector lowest = analyzer.Compute(_funcLowest, depth, low);
			IVector highest = analyzer.Compute(_funcHighest, depth, high);

			float point = analyzer.Point;
			int beginindex = result.Count;

      for (int i = beginindex; i < low.Count; i++) {
				if (i < depth){
					zzresult.Add(float.NaN);
					zzresult.ResultLow.Add(float.NaN);
					zzresult.ResultHigh.Add(float.NaN);
				}else{
					float val = lowest[i];
					zzresult.ResultLow.Add(val);
					if(val == zzresult.LastLow) {
						val = 0;
					} else { 
						zzresult.LastLow = val; 
						if(low[i] - val > deviation * point) 
							val = 0;
						else {
							for(int back=1; back <= backstep; back++) {
								float res = zzresult.ResultLow[i-back];
								if(!float.IsNaN(res) && res > val) 
									zzresult.ResultLow[i-back] = float.NaN; 
							}
						}
					}
					zzresult.ResultLow[i] = val;

					val = highest[i];
					zzresult.ResultHigh.Add(val);
					
					if(val == zzresult.LastHigh) 
						val = 0;
					else {
						zzresult.LastHigh = val;
						if(val-high[i] > deviation * point) 
							val = 0;
						else {
							for(int back = 1; back <= backstep; back++) {
								float res = zzresult.ResultHigh[i-back];
								if(res != 0 && res < val) 
									zzresult.ResultHigh[i-back] = 0; 
							} 
						}
					}
					zzresult.ResultHigh[i] = val;
				}
				zzresult.Add(float.NaN);
			}

			zzresult.LastHigh=-1; int lasthighpos=-1;
			zzresult.LastLow=-1;  int lastlowpos=-1;

      for (int i = beginindex; i < low.Count; i++) {
				float curlow = zzresult.ResultLow[i];
				float curhigh = zzresult.ResultHigh[i];

				if(curhigh > 0) {
					if(zzresult.LastHigh > 0) {
						if(zzresult.LastHigh < curhigh) 
							zzresult.ResultHigh[lasthighpos] = 0;
						else 
							zzresult.ResultHigh[i] = 0;
					}
					//---
					if(zzresult.LastHigh < curhigh || zzresult.LastHigh < 0) {
						zzresult.LastHigh = curhigh;
						lasthighpos=i;
					}
					zzresult.LastLow = -1;
				}

				if( curlow != 0) {
					if(zzresult.LastLow > 0) {
						if(zzresult.LastLow > curlow) 
							zzresult.ResultLow[lastlowpos] = 0;
						else 
							zzresult.ResultLow[i]=0;
					}

					if(curlow < zzresult.LastLow || zzresult.LastLow < 0) {
						zzresult.LastLow = curlow;
						lastlowpos=i;
					} 
					zzresult.LastHigh = -1;
				}
			}

      for (int i = beginindex; i < low.Count; i++) {
				if (i < depth)
					zzresult.ResultLow[i] =0;
				else {
					float res = zzresult.ResultHigh[i];
					if(res != 0) 
						zzresult.ResultLow[i] = res;
				}
				if (zzresult.ResultHigh[i] > 0)
					result[i] = zzresult.ResultHigh[i];
				if (zzresult.ResultLow[i] > 0)
					result[i] = zzresult.ResultLow[i];

			}
			return zzresult;
		}
	}
}
