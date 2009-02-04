using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iJMA: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Absolute scaled";
			this.Name = "JMA";
			this.ShortName = "JMA";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("JMA").SetDrawPen(Color.Green);
		}
	}

	public class JMAVector: Vector{
		public int LimitValue, StartValue;
		public double[] List = new double[128];
		public double[] Ring1 = new double[128];
		public double[] Ring2 = new double[11];
		public double[] Buffer = new double[62];
		public bool InitFlag = true;
		public double LogParam, SqrtParam, LengthDivider;
		public double CycleDelta = 0;
		public double ParamA, ParamB;

		public IVector C0Buffer;
		public IVector A8Buffer;
		public IVector C8Buffer;
		public double PhaseParam;

		public int CounterA = 0, CounterB = 0, CycleLimit = 0;

		public double HighDValue = 0, LowDValue = 0;

		#region public JMAVector(int length, int phase)
		public JMAVector(int length, int phase){
			C0Buffer = new Vector();
			A8Buffer = new Vector();
			C8Buffer = new Vector();
			LimitValue = 63; 
			StartValue = 64;

			for (int i = 0; i <= LimitValue; i++) 
				List [i] = -1000000; 
			for (int i = StartValue; i <= 127; i++)   
				List [i] = 1000000; 

			double lengthParam;

			if (length < 1) lengthParam = 0.0000000001f;
			else lengthParam = (length - 1) / 2;

			if (phase < -100) PhaseParam = 0.5;
			else if (phase > 100) PhaseParam = 2.5;
			else PhaseParam = phase / 100 + 1.5;

			LogParam = Math.Log (Math.Sqrt (Convert.ToDouble(lengthParam))) / Math.Log (2);

			if (LogParam + 2 < 0) LogParam = 0;
			else LogParam = LogParam + 2; 

			SqrtParam     = Convert.ToSingle(Math.Sqrt(Convert.ToDouble(lengthParam)) * Convert.ToDouble(LogParam));
			lengthParam   = lengthParam * 0.9; 
			LengthDivider = lengthParam / (lengthParam + 2);
		}
		#endregion
	}

	[Function("JMA")]
	public class JMA : Function {
		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("Length", 14, 0, 10000));
			this.RegParameter(new ParameterInteger("Phase", 0, -100000, 100000));
			this.RegParameter(Function.CreateDefParam_ApplyTo("Close"));
		}
		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int length = (int)parameters[0];
			int phase = (int)parameters[1];
			IVector vector = (IVector)parameters[2];

			if (result == null)
				result = new JMAVector(length, phase);

			JMAVector jma = result as JMAVector;

			if (jma.Count == vector.Count){
				jma.RemoveLastValue();
				jma.A8Buffer.RemoveLastValue();
				jma.C0Buffer.RemoveLastValue();
				jma.C8Buffer.RemoveLastValue();
			}

			for (int i=jma.Count;i<vector.Count;i++){
				jma.Add(float.NaN);
				jma.A8Buffer.Add(float.NaN);
				jma.C0Buffer.Add(float.NaN);
				jma.C8Buffer.Add(float.NaN);
				
				double series = vector[i];
				double JMAValue, JMATempValue;
				int s58, s60, s40 = 0, s38 = 0, s68;

				if (i < 61)
					jma.Buffer[i] = series; 
  
				if (i > 30) {
					int highLimit;
					if (jma.InitFlag) { 
						jma.InitFlag = false;
             
						int diffFlag = 0; 
						for (int ii = 1; ii <= 29; ii++) { 
							if (jma.Buffer [ii] != jma.Buffer [ii + 1]) 
								diffFlag = 1;
						}  
						highLimit = diffFlag * 30;
             
						if (highLimit == 0) jma.ParamB = series;
						else jma.ParamB = jma.Buffer[1];
             
						jma.ParamA = jma.ParamB; 
						if (highLimit > 29) highLimit = 29; 
					} else 
						highLimit = 0;

					int loopCriteria =0;
					for (int ii = highLimit; ii >= 0; ii--) { 
						double sValue;
						if (ii == 0) sValue = series; else sValue = jma.Buffer [31 - ii]; 
        
						double absValue;
						if (Math.Abs (sValue - jma.ParamA) > Math.Abs (sValue - jma.ParamB)) 
							absValue = Math.Abs(sValue - jma.ParamA); 
						else 
							absValue = Math.Abs(sValue - jma.ParamB); 
               
						double dValue = absValue + 0.0000000001;
    
						if (jma.CounterA <= 1) jma.CounterA = 127; else jma.CounterA--; 
						if (jma.CounterB <= 1) jma.CounterB = 10;  else jma.CounterB--; 
						if (jma.CycleLimit < 128) jma.CycleLimit++; 
						jma.CycleDelta += (dValue - jma.Ring2 [jma.CounterB]); 
						jma.Ring2[jma.CounterB] = dValue; 

						if (jma.CycleLimit > 10) 
							jma.HighDValue = jma.CycleDelta / 10; 
						else 
							jma.HighDValue = jma.CycleDelta / jma.CycleLimit; 
               
						if (jma.CycleLimit > 127) { 
							dValue = jma.Ring1 [jma.CounterA]; 
							jma.Ring1 [jma.CounterA] = jma.HighDValue; 
							s68 = s58 = 64; 
							while (s68 > 1) { 
								if (jma.List [s58] < dValue) { 
									s68 = s68 / 2; 
									s58 += s68; 
								} else if (jma.List [s58] <= dValue){ 
									s68 = 1; 
								} else { 
									s68 = s68 / 2;
									s58 -= s68; 
								}
							} 
						} else {
							jma.Ring1 [jma.CounterA] = jma.HighDValue; 
							if (jma.LimitValue + jma.StartValue > 127) {
								jma.StartValue--; 
								s58 = jma.StartValue; 
							} else {
								jma.LimitValue++; 
								s58 = jma.LimitValue; 
							}
							if (jma.LimitValue > 96) s38 = 96; 
							else s38 = jma.LimitValue; 
							if (jma.StartValue < 32) s40 = 32; 
							else s40 = jma.StartValue; 
						}

						s68 = 64; 
						s60 = s68; 
						while (s68 > 1) {
							if (jma.List [s60] >= jma.HighDValue) {
								if (jma.List [s60-1] <= jma.HighDValue) {
									s68 = 1; 
								} else {
									s68 = s68 / 2; 
									s60 -= s68; 
								}
							} else {
								s68 = s68 / 2; 
								s60 += s68; 
							}
							if (s60 == 127 && jma.HighDValue > jma.List[127]) 
								s60 = 128; 
						}
						if (jma.CycleLimit > 127) {
							if (s58 >= s60) {
								if (s38 + 1 > s60 && s40 - 1 < s60){
									jma.LowDValue += jma.HighDValue; 
								} else if (s40 > s60 &&  s40 - 1 < s58){
									jma.LowDValue += jma.List [s40 - 1]; 
								}
							} else if (s40 >= s60) {                       
								if (s38 + 1 < s60 && s38 + 1 > s58) 
									jma.LowDValue += jma.List[s38 + 1]; 
							} else if (s38 + 2 > s60){
								jma.LowDValue += jma.HighDValue; 
							} else if (s38 + 1 < s60 && s38 + 1 > s58) 
								jma.LowDValue += jma.List[s38 + 1]; 
            
							if (s58 > s60) {
								if (((s40 - 1) < s58) && ((s38 + 1) > s58)) 
									jma.LowDValue -= jma.List [s58]; 
								else if ((s38 < s58) && ((s38 + 1) > s60)) 
									jma.LowDValue -= jma.List[s38]; 
							} else {
								if (((s38 + 1) > s58) && ((s40 - 1) < s58)) 
									jma.LowDValue -= jma.List [s58]; 
								else if ((s40 > s58) && (s40 < s60)) 
									jma.LowDValue -= jma.List [s40]; 
							}
						}
						if (s58 <= s60) {
							if (s58 >= s60) jma.List[s60] = jma.HighDValue; 
							else {
								for (int j = s58 + 1; j <= (s60 - 1); j++) {
									jma.List [j-1] = jma.List[j]; 
								}
								jma.List [s60 - 1] = jma.HighDValue; 
							}
						} else {
							for (int j = s58 - 1; j >= s60; j--) {
								jma.List [j+1] = jma.List [j]; 
							}
							jma.List [s60] = jma.HighDValue; 
						}
            
						if (jma.CycleLimit <= 127) {
							jma.LowDValue = 0;  
							for (int j = s40; j <= s38; j++) {
								jma.LowDValue += jma.List[j]; 
							}
						}
						//----                
						if (loopCriteria + 1 > 31) loopCriteria = 31; else loopCriteria++; 
						
						double sqrtDivider = jma.SqrtParam / (jma.SqrtParam + 1);
               
						if (loopCriteria <= 30) {
							if (sValue - jma.ParamA > 0) jma.ParamA = sValue; else jma.ParamA = sValue - (sValue - jma.ParamA) * sqrtDivider; 
							if (sValue - jma.ParamB < 0) jma.ParamB = sValue; else jma.ParamB = sValue - (sValue - jma.ParamB) * sqrtDivider; 
							JMATempValue = series;
                 
							if (loopCriteria == 30) { 
								jma.C0Buffer[i] = series;
								int intPart;
								if (Math.Ceiling(jma.SqrtParam) >= 1) intPart = Convert.ToInt32(Math.Ceiling(jma.SqrtParam)); 
								else intPart = 1; 
								int leftInt = IntPortion (intPart); 
								if (Math.Floor(jma.SqrtParam) >= 1) intPart = Convert.ToInt32(Math.Floor(jma.SqrtParam)); 
								else intPart = 1; 
								int rightPart = IntPortion (intPart);
                     
								if (leftInt == rightPart) dValue = 1; 
								else dValue = (jma.SqrtParam - rightPart) / (leftInt - rightPart);
                 
								int upShift, dnShift;
								if (rightPart <= 29) 
									upShift = rightPart; else upShift = 29; 
								if (leftInt <= 29) 
									 dnShift = leftInt; else dnShift = 29; 
								jma.A8Buffer[i] = (series - jma.Buffer[i-upShift]) * (1 - dValue) / rightPart + (series - jma.Buffer[i-dnShift]) * dValue / leftInt;
							}
						} else {
							float powerValue;
                  
							dValue = lowDValue / (s38 - s40 + 1);
							if (0.5F <= jma.LogParam - 2F) powerValue = jma.LogParam - 2F;
							else powerValue = 0.5F;
               
							if (jma.LogParam >= Math.Pow(absValue/dValue, powerValue)) 
								dValue = Math.Pow (absValue/dValue, powerValue); 
							else dValue = jma.LogParam; 
							if (dValue < 1) dValue = 1;
                    
							powerValue = Convert.ToSingle(Math.Pow (sqrtDivider, Math.Sqrt (dValue))); 
							if (sValue - jma.ParamA > 0) jma.ParamA = sValue; 
							else jma.ParamA = sValue - (sValue - jma.ParamA) * powerValue; 
							if (sValue - jma.ParamB < 0) 
								jma.ParamB = sValue; 
							else jma.ParamB = sValue - (sValue - jma.ParamB) * powerValue; 
						}
					}

					if (loopCriteria > 30) {
						JMATempValue = jma[i-1];
						float powerValue	= Convert.ToSingle(Math.Pow (jma.LengthDivider, dValue));
						float squareValue	= Convert.ToSingle(Math.Pow (powerValue, 2));
                         
						jma.C0Buffer[i] = (1 - powerValue) * series + powerValue * jma.C0Buffer [i-1];
						jma.C8Buffer[i] = (series - jma.C0Buffer [i]) * (1 - jma.LengthDivider) + jma.LengthDivider * jma.C8Buffer [i-1];
            
						jma.A8Buffer[i] = (jma.PhaseParam * jma.C8Buffer [i] + jma.C0Buffer[i] - JMATempValue) * 
							(powerValue * (-2F) + squareValue + 1) + squareValue * jma.A8Buffer [i-1];  
						JMATempValue += jma.A8Buffer [i]; 
					}
					JMAValue = JMATempValue;
				}
      
				if (i <= 30) 
					JMAValue = float.NaN;
      
				jma[i] = JMAValue;
			}

			return result;
		}
		private int IntPortion (float param) {
			if (param > 0) return Convert.ToInt32(Math.Floor (param));
			if (param < 0) return Convert.ToInt32(Math.Ceiling (param));
			return 0;
		}

	}
}
