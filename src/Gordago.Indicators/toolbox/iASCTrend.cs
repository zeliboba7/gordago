using System;
using System.Drawing;
using System.Collections;

namespace Gordago.Analysis.Toolbox {
	public class iASCTrend : Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Signal indicators";
			this.Name = "ASCTrend";
			this.ShortName = "ASCTrend";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;
			this.SetCustomDrawingIndicator(typeof (iCrossCustomDrawing));
			
			RegFunction("ASCTrend");
		}
	}

	[Function("ASCTrend")]
	public class ASCTrend: Function{

		private Function _funcWPR;

		protected override void Initialize() {
			_funcWPR = this.GetFunction("WPR");

			ParameterVector pvopen = new ParameterVector("Open", "Open");
			pvopen.Visible = false;
			RegParameter(pvopen);

			ParameterVector pvlow = new ParameterVector("Low", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);

			ParameterVector pvhigh = new ParameterVector("High", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);

			ParameterVector pvclose = new ParameterVector("Close", "Close");
			pvclose.Visible = false;
			RegParameter(pvclose);
		}

		private class ASCTrendVector: Vector{
			public float Value10 = 10;
			public float X1=70;
			public float X2=30;
			public int Up=0, Dn=0;
			public float[] Table_value= new float[0];
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector open = (IVector)parameters[0];
			IVector low = (IVector)parameters[1];
			IVector high = (IVector)parameters[2];
			IVector close = (IVector)parameters[3];

			float point = analyzer.Point;

			if (result == null)
				result = new ASCTrendVector();

			ASCTrendVector asctv = result as ASCTrendVector;

			if (result.Count == close.Count)
				result.RemoveLastValue();

      if (asctv.Table_value.Length < close.Count) {
				ArrayList al = new ArrayList(asctv.Table_value);
        al.AddRange(new float[close.Count - asctv.Table_value.Length]);
				asctv.Table_value = (float[])al.ToArray(typeof(float));
			}


      for (int i = result.Count; i < close.Count; i++) {
				if (i < 9){
					result.Add(float.NaN);
				}else{
					float Range=0;
					float AvgRange=0;
					for (int ii = i - 9; ii <= i; ii++) {
						AvgRange=AvgRange+Math.Abs(high[ii]-low[ii]);
					}
					Range=AvgRange/10;
      
					int counter=i;
					int TrueCount=0;
					for (int ii = i; ii >= i-8; ii--){
						if (Math.Abs(open[ii]-close[ii-1]) >= Range * 2f ){
							TrueCount++;
							break;
						}
						counter++;
					}

					float mro1 = TrueCount>=1 ? counter : -1;
            
					counter=i;
					TrueCount=0;
					for (int ii = i; ii >= i-6; ii--){
						if(Math.Abs(close[ii-3]-close[ii]) >= Range * 4.6F) {
							TrueCount++;
							break;
						}
						counter++;
					}
      
					float mro2 = TrueCount>=1 ? counter : -1;
        
					float value11 = mro1 > -1 ? 3 : asctv.Value10;

					value11 = mro2 >-1 ? 4 : asctv.Value10;
				
					IVector WPRResult = analyzer.Compute(_funcWPR, low, high, close, (int)value11);
            
					float value2 = 100-Math.Abs(WPRResult[i]);

					asctv.Table_value[i] = value2;

					float val1=0;
					float val2=0;
					float value3=0;

					if (value2 < asctv.X2 ) {  
						int i1=1;
						while (asctv.Table_value[i-i1] >= asctv.X2 && asctv.Table_value[i-i1] <= asctv.X1) {
							i1++;
						}
						if (asctv.Table_value[i-i1] > asctv.X1) {
							value3 = high[i] + Range * 0.5F;
							val1=value3;
						}
					}
      
					if ( value2 > asctv.X1) {
						int i1=1;
						while (asctv.Table_value[i-i1] >= asctv.X2 && asctv.Table_value[i-i1] <= asctv.X1) {
							i1++;
						}
            
						if (asctv.Table_value[i-i1] < asctv.X2) {
							value3 = low[i] - Range * 0.5F;
							val2=value3;
						} 
					}

					if (val2 != 0 && asctv.Up == 0) {     
						result.Add(val2 - point);
						asctv.Up = 1;
						asctv.Dn = 0;
					}else if (val1 !=0 && asctv.Dn == 0) {
						result.Add(val1 + point);
						asctv.Dn = 1;
						asctv.Up = 0;
					}else{
						result.Add(float.NaN);
					}
				}
			}
			return result;
		}
	}
}
