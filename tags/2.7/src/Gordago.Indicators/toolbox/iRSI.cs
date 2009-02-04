using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iRSI: Indicator{

		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "RSI";
			this.ShortName = "RSI";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("RSI");
			fs.SetDrawPen(Color.Blue);
		}
	}
	[Function("RSI")]
	public class RSI : Function {

		#region private class RsiVector : Vector
		private class RsiVector : Vector {
			private float _v1, _v2, _pv1, _pv2;
			

			public RsiVector(int capacity) : base(capacity) {
			}
			
			#region public float V1 
			public float V1 {
				get {return _v1;}
				set {_v1 = value;}
			}
			#endregion
			
			#region public float V2 
			public float V2 {
				get { return _v2; }
				set { _v2 = value;}
			}
			#endregion

			#region public float PV1 
			public float PV1 {
				get {return _pv1;}
				set {_pv1 = value;}
			}
			#endregion
			
			#region public float PV2 
			public float PV2 {
				get {return _pv2;}
				set {_pv2 = value;}
			}
			#endregion
		}
		#endregion

		protected override void Initialize() {
			RegParameter(new ParameterInteger("Period", new string[]{"Period", "Период"}, 13, 1, 10000));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      RegParameter(new ParameterColor("ColorRSI", new string[] { "Color", "Цвет" }, Color.Blue));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			IVector vector = parameters[1] as IVector;
			
			if ( result == null )
				result = new RsiVector(0);

			RsiVector rsiResult = result as RsiVector;

			if (result.Count == vector.Count){
				rsiResult.RemoveLastValue();
				rsiResult.V1 = rsiResult.PV1;
				rsiResult.V2 = rsiResult.PV2;
			}
			
			for ( int i = result.Count; i < vector.Count; i++ ) {
				if (i < period ){
					rsiResult.Add(float.NaN);
				}else{

					float h = 0, l = 0;
					if ( i == period ) {
						for ( int j = i - period + 1; j < i + 1; j++ ) {
							if ( vector[j] > vector[j - 1] )
								h += vector[j] - vector[j - 1];
							else
								l += vector[j - 1] - vector[j];
						}
						h = h / period; l = l / period;
						if ( l == 0 )
							rsiResult.Add(100);
						else
							rsiResult.Add( 100f - 100f / (1 + h / l));
						rsiResult.V1 = h; rsiResult.V2 = l;
					} else {
						rsiResult.PV1 = rsiResult.V1;
						rsiResult.PV2 = rsiResult.V2;
						
						h = rsiResult.V1 * (period - 1);
						l = rsiResult.V2 * (period - 1);
						if ( vector[i] > vector[i - 1] )
							h += vector[i] - vector[i - 1];
						else
							l += vector[i - 1] - vector[i];
						h = h / period; l = l / period;
						if ( l == 0 )
							rsiResult.Add(100);
						else
							rsiResult.Add( 100f - 100f / (1 + h / l));
						rsiResult.V1 = h; rsiResult.V2 = l;
					//	System.Diagnostics.Debug.WriteLine(result.Current.ToString());
					}
				}
			}
			return result;
		}
	}
}
