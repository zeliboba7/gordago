using System;
using System.Drawing;

namespace Gordago.Analysis.CustomIndicators {
	public class iT3_Trix:Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Relative scaled";
			this.Name = "T3_Trix";
			this.ShortName = "T3_Trix";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("Trix");
			fs.SetDrawPen(Color.DeepSkyBlue);
			
			FunctionStyle fsb = RegFunction("TrixB");
			fs.SetDrawPen(Color.Blue);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);
		}
	}

	[Function("TrixB")]
	public class TrixB: Function{
		private Function _functTR_Trix;

		protected override void Initialize() {
			_functTR_Trix  = this.GetFunction("Trix");
			this.RegParameter(_functTR_Trix);
			RegParameter(new ParameterInteger("BT3Period", 10, 1, 10000));
      RegParameter(new ParameterColor("ColorTrixB", new string[] { "Color", "÷‚ÂÚ" }, Color.Blue));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {

			int at3_period = (int)parameters[0];
			float hot = (float)parameters[1];
			IVector vector = (IVector)parameters[2];
			int bt3_period = (int)parameters[3];

			return analyzer.Compute(_functTR_Trix, bt3_period, hot, vector);
		}
	}

	#region public class iT3_TrixVector: Vector 
	public class iT3_TrixVector: Vector {

		public float d1x = 0, d2x = 0, d3x = 0, d4x = 0, d5x = 0, d6x = 0;
		public float 	e1x=0, e2x=0, e3x=0, e4x=0, e5x=0 , e6x=0;
		public float A_w1=0,A_w2=0;
		public float A_t3_1=0;


		public iT3_TrixVector(int period) {
			A_w1 = 2 / (1 + 0.5f*(period-1) + 1);
			A_w2 = 1 - A_w1;
		}
	}
	#endregion

	[Function("Trix")]
	public class Trix: Function{
		protected override void Initialize() {
			RegParameter(new ParameterInteger("AT3Period", 18, 1, 10000));
			RegParameter(new ParameterFloat("Hot", 0.7f));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      RegParameter(new ParameterColor("ColorTrix", new string[] { "Color", "÷‚ÂÚ" }, Color.DeepSkyBlue));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {

			int period = (int)parameters[0];
			float hot = (float)parameters[1];
			IVector vector = (IVector)parameters[2];

			float  c1=0,c2=0,c3=0,c4=0;
			float b2=0,b3=0;

			b2=hot*hot; b3=b2*hot;
			c1=-b3; c2=(3*(b2+b3));
			c3=-3*(2*b2+hot+b3);
			c4=(1+3*hot+b3+3*b2);
 
			if ( result == null )
				result = new iT3_TrixVector(period);
			
			iT3_TrixVector t3rst = result as iT3_TrixVector;
			
			if (result.Count == vector.Count) result.RemoveLastValue();

			for ( int i = result.Count; i < vector.Count; i++ ){
				float d1 = t3rst.A_w1 * vector[i] + t3rst.A_w2*t3rst.d1x;
				float d2 = t3rst.A_w1*d1 + t3rst.A_w2*t3rst.d2x;
				float d3 = t3rst.A_w1*d2 + t3rst.A_w2*t3rst.d3x;
				float d4 = t3rst.A_w1*d3 + t3rst.A_w2*t3rst.d4x;
				float d5 = t3rst.A_w1*d4 + t3rst.A_w2*t3rst.d5x;
				float d6 = t3rst.A_w1*d5 + t3rst.A_w2*t3rst.d6x;

				float A_t3 = c1*d6 + c2*d5 + c3*d4 + c4*d3;

				t3rst.d1x=d1; t3rst.d2x=d2;  t3rst.d3x=d3;  t3rst.d4x=d4;  t3rst.d5x=d5;  t3rst.d6x=d6;
				if (i == 0){
					t3rst.Add(float.NaN);
				}else{
					t3rst.Add((A_t3-t3rst.A_t3_1)/t3rst.A_t3_1);
				}
				t3rst.A_t3_1 = A_t3;
			}
			return t3rst;
		}
	}
}
