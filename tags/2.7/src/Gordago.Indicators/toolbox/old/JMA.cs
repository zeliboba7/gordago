using System;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	
	[Function ("JMA",typeof(IVector))]
	public class JMA : Function {
	
		#region JMAVector 
		public class JMAVector : Vector {

			private int    limitValue, startValue, loopParam, loopCriteria;
			private int    cycleLimit, highLimit, counterA, counterB;

			private bool   initFlag;
			
			private double cycleDelta,  paramA, paramB;
			private double phaseParam, logParam, _JMAValue, sValue, lengthDivider,sqrtParam;

			private int s58, s60, s40, s38, s68;

			public double[] List, Ring1, Ring2, Buffer;

			public double[] JMAValueBuffer = new double[1000000];
			public double[] fC0Buffer = new double[1000000];
			public double[] fA8Buffer = new double[1000000];
			public double[] fC8Buffer = new double[1000000];

			public JMAVector():base() {
			}
			public JMAVector(int capacity) : base(capacity) {
			}
			
			#region public int LimitValue
			public int LimitValue {
				get {return limitValue;}
				set {limitValue = value;}
			}
			#endregion
			
			#region public int StartValue
			public int StartValue {
				get {
					return startValue;
				}
				set {
					startValue = value;
				}
			}
			#endregion

			#region public int LoopParam 
			public int LoopParam {
				get {return loopParam;}
				set {loopParam = value;}
			}
			#endregion

			#region public int LoopCriteria 
			public int LoopCriteria {
				get {return loopCriteria;}
				set {loopCriteria = value;}
			}
			#endregion

			#region public int CycleLimit 
			public int CycleLimit {
				get {return cycleLimit;}
				set {cycleLimit = value;}
			}
			#endregion

			#region public int HighLimit 
			public int HighLimit {
				get {
					return highLimit;
				}
				set {
					highLimit = value;
				}
			}
			#endregion

			#region public int CounterA 
			public int CounterA {
				get {
					return counterA;
				}
				set {
					counterA = value;
				}
			}
			#endregion

			#region public int CounterB 
			public int CounterB {
				get {
					return counterB;
				}
				set {
					counterB = value;
				}
			}
			#endregion

			#region public double CycleDelta 
			public double CycleDelta {
				get {
					return cycleDelta;
				}
				set {
					cycleDelta = value;
				}
			}
			#endregion
			
			#region public double ParamA 
			public double ParamA {
				get {
					return paramA;
				}
				set {
					paramA = value;
				}
			}
			#endregion

			#region public double ParamB 
			public double ParamB {
				get {
					return paramB;
				}
				set {
					paramB = value;
				}
			}
			#endregion

			#region public double PhaseParam 
			public double PhaseParam {
				get {
					return phaseParam;
				}
				set {
					phaseParam = value;
				}
			}
			#endregion

			#region public double LogParam
			public double LogParam {
				get {
					return logParam;
				}
				set {
					logParam = value;
				}
			}
			#endregion

			#region public double JMAValue 
			public double JMAValue {
				get {
					return _JMAValue;
				}
				set {
					_JMAValue = value;
				}
			}
			#endregion

			#region public double LengthDivider 
			public double LengthDivider {
				get {
					return lengthDivider;
				}
				set {
					lengthDivider= value;
				}
			}
			#endregion

			#region public double SqrtParam 
			public double SqrtParam {
				get {
					return sqrtParam;
				}
				set {
					sqrtParam = value;
				}
			}
			#endregion

			#region public double SValue 
			public double SValue {
				get {
					return sValue;
				}
				set {
					sValue = value;
				}
			}
			#endregion

			#region public bool InitFlag 
			public bool InitFlag {
				get {
					return initFlag;
				}
				set {
					initFlag = value;
				}
			}
			#endregion
			
			#region public int S58 
			public int S58 {
				get {return s58;}
				set {s58 = value;}
			}
			#endregion
			
			#region public int S60 
			public int S60 {
				get {
					return s60;
				}
				set {
					s60 = value;
				}
			}
			#endregion

			#region public int S40 
			public int S40 {
				get {
					return s40;
				}
				set {
					s40 = value;
				}
			}
			#endregion

			#region public int S38 
			public int S38 {
				get {
					return s38;
				}
				set {
					s38 = value;
				}
			}
			#endregion

			#region public int S68 
			public int S68 {
				get {
					return s68;
				}
				set {
					s68 = value;
				}
			}
			#endregion

		}
		#endregion

		#region IntPortion (double param) 
		int IntPortion (double param) {
			if (param > 0) return ((int)Math.Floor (param));
			if (param < 0) return ((int)Math.Ceiling (param));
			return 0;
		}
		#endregion

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			double highDValue = 0, absValue = 0,  sqrtParam = 0;
			double JMATempValue = 0, sqrtDivider = 0,JMAValue = 0;
			double powerValue = 0, squareValue, dValue = 0;
			
			double series;

			IVector vector = parameters[0] as IVector;
			int lenght = (int)parameters[1];
			int phase  = (int)parameters[2];

			JMAVector jma = result as JMAVector;

			#region if (jma == null) {...}
			if (jma == null) {
				jma = new JMAVector();
				double   lengthParam;

				jma.List = new double[128];
				jma.Ring1 = new double[128];
				jma.Ring2 = new double[11];
				jma.Buffer = new double[62];

				jma.LimitValue = 63; 
				jma.StartValue = 64;

				for (int i = 0; i <= jma.LimitValue; i++) jma.List [i] = -1000000; 
				for (int i = jma.StartValue; i <= 127; i++)   jma.List [i] = 1000000; 

				jma.InitFlag  = true;
				if (lenght < 1.0000000002) lengthParam = 0.0000000001;
				else lengthParam = (lenght - 1) / 2.0;

				if (phase < -100) jma.PhaseParam = 0.5;
				else if (phase > 100) jma.PhaseParam = 2.5;
				else jma.PhaseParam = phase / 100.0 + 1.5;
				
				jma.LogParam = Math.Log (Math.Sqrt (lengthParam)) / Math.Log (2.0);

				if (jma.LogParam + 2.0 < 0) jma.LogParam = 0;
				else jma.LogParam = jma.LogParam + 2.0; 
				//----
				jma.SqrtParam     = Math.Sqrt(lengthParam) * jma.LogParam; 
				lengthParam   = lengthParam * 0.9; 
				jma.LengthDivider = lengthParam / (lengthParam + 2.0);
			}
			#endregion

			for (int i = jma.Count; i < vector.Count; i++) {
				if (i<32){
					jma.Add(float.NaN);
				} else {
					series = vector [i];
 
					if (jma.LoopParam < 61) { 
						jma.LoopParam++; 
						jma.Buffer [jma.LoopParam] = series; 
					} 
					if (jma.LoopParam > 30) {
						if (jma.InitFlag) { 
							jma.InitFlag = false;
             
							int diffFlag = 0; 
							for (int ii = 1; ii <= 29; ii++)  
								if (jma.Buffer [ii + 1] != jma.Buffer [ii])
									diffFlag = 1;
							  
							jma.HighLimit = diffFlag * 30;
             
							if (jma.HighLimit == 0) jma.ParamB = series;
							else jma.ParamB = jma.Buffer[1];
             
							jma.ParamA = jma.ParamB; 
							if (jma.HighLimit > 29) jma.HighLimit = 29; 
						} else {
							jma.HighLimit = 0;
						}
						double lowDValue = 0;

						for (int ii = 0; ii <= jma.HighLimit; ii++) { 
							if (ii == 0) jma.SValue = series; 
							else jma.SValue = jma.Buffer [ii]; 
	    
							if (Math.Abs (jma.SValue - jma.ParamA) > Math.Abs (jma.SValue - jma.ParamB)) 
								absValue = Math.Abs(jma.SValue - jma.ParamA); 
							else absValue = Math.Abs(jma.SValue - jma.ParamB); 
			   
							dValue = absValue + 0.0000000001;
	
							if (jma.CounterA <= 1) jma.CounterA = 127; else jma.CounterA--; 
							if (jma.CounterB <= 1) jma.CounterB = 10;  else jma.CounterB--; 
							
							if (jma.CycleLimit < 128)
								jma.CycleLimit++; 
							
							jma.CycleDelta += (dValue - jma.Ring2 [jma.CounterB]); 
							jma.Ring2 [jma.CounterB] = dValue; 
			   
							if (jma.CycleLimit > 10) 
								highDValue = jma.CycleDelta / 10.0; 
							else 
								highDValue = jma.CycleDelta / jma.CycleLimit; 
			   
							#region if (jma.CycleLimit > 127) {...}
							if (jma.CycleLimit > 127) { 
								dValue = jma.Ring1 [jma.CounterA]; 
								jma.Ring1 [jma.CounterA] = highDValue; 
								jma.S68 = 64; jma.S58 = jma.S68; 
								while (jma.S68 > 1) { 
									if (jma.List [jma.S58] < dValue) { 
										jma.S68 = (int)Math.Floor(jma.S68 / 2.0); 
										jma.S58 += jma.S68; 
									} 
									else 
										if (jma.List [jma.S58] <= dValue) { 
										jma.S68 = 1; 
									} 
									else { 
										jma.S68 = (int)Math.Floor(jma.S68 / 2.0); 
										jma.S58 -= jma.S68; 
									}
								} 
							} 
								#endregion
							#region else {..}
							else {
								jma.Ring1 [jma.CounterA] = highDValue; 
								if ((jma.LimitValue + jma.StartValue) > 127) {
									jma.StartValue--; 
									jma.S58 = jma.StartValue; 
								} 
								else {
									jma.LimitValue++; 
									jma.S58 = jma.LimitValue; 
								}
								if (jma.LimitValue > 96) jma.S38 = 96; 
								else jma.S38 = jma.LimitValue; 
								if (jma.StartValue < 32) jma.S40 = 32; 
								else jma.S40 = jma.StartValue; 
							}
							#endregion

							//----		      
							jma.S60 = jma.S68 = 64; 
							#region while (jma.S68 > 1) {...}
							while (jma.S68 > 1) {
								if (jma.List [jma.S60] >= highDValue) {
									if (jma.List [jma.S60 - 1] <= highDValue) {
										jma.S68 = 1; 
									}
									else {
										jma.S68 = (int)Math.Floor(jma.S68 / 2.0); 
										jma.S60 -= jma.S68; 
									}
								}
								else {
									jma.S68 = (int)Math.Floor(jma.S68 / 2.0); 
									jma.S60 += jma.S68; 
								}
								if ((jma.S60 == 127) && (highDValue > jma.List[127])) jma.S60 = 128; 
							}
							#endregion

							#region if (jma.CycleLimit > 127) {...}
							if (jma.CycleLimit > 127) {
								if (jma.S58 >= jma.S60) {
									if (((jma.S38 + 1) > jma.S60) && ((jma.S40 - 1) < jma.S60)) 
										lowDValue += highDValue; 
									else if ((jma.S40 > jma.S60) && ((jma.S40 - 1) < jma.S58)) 
										lowDValue += jma.List [jma.S40 - 1]; 
								}
								else if (jma.S40 >= jma.S60) {					   
									if (((jma.S38 + 1) < jma.S60) && ((jma.S38 + 1) > jma.S58)) 
										lowDValue += jma.List[jma.S38 + 1]; 
								}
								else if ((jma.S38 + 2) > jma.S60) 
									lowDValue += highDValue; 
								else if (((jma.S38 + 1) < jma.S60) && ((jma.S38 + 1) > jma.S58)) 
									lowDValue += jma.List[jma.S38 + 1]; 
			
								if (jma.S58 > jma.S60) {
									if (((jma.S40 - 1) < jma.S58) && ((jma.S38 + 1) > jma.S58)) 
										lowDValue -= jma.List [jma.S58]; 
									else if ((jma.S38 < jma.S58) && ((jma.S38 + 1) > jma.S60)) 
										lowDValue -= jma.List[jma.S38]; 
								}
								else {
									if (((jma.S38 + 1) > jma.S58) && ((jma.S40 - 1) < jma.S58)) 
										lowDValue -= jma.List [jma.S58]; 
									else if ((jma.S40 > jma.S58) && (jma.S40 < jma.S60)) 
										lowDValue -= jma.List [jma.S40]; 
								}
							}
							#endregion

							if (jma.S58 <= jma.S60) {
								if (jma.S58 >= jma.S60) jma.List[jma.S60] = highDValue; else {
									for (int j = jma.S58 + 1; j <= (jma.S60 - 1); j++) {
										jma.List [j - 1] = jma.List[j]; 
									}
									jma.List [jma.S60 - 1] = highDValue; 
								}
							} 
							else {
								for (int j = jma.S58 - 1; j >= jma.S60; j--) {
									jma.List [j + 1] = jma.List [j]; 
								}
								jma.List [jma.S60] = highDValue; 
							}
			
							if (jma.CycleLimit <= 127) {
								lowDValue = 0;  
								for (int j = jma.S40; j <= jma.S38; j++) 
									lowDValue += jma.List[j]; 
							}

							if ((jma.LoopCriteria + 1) > 31) jma.LoopCriteria = 31; else jma.LoopCriteria++; 
							sqrtDivider = sqrtParam / (sqrtParam + 1.0);
			   
							if (jma.LoopCriteria <= 30) {
								if (jma.SValue - jma.ParamA > 0) 
									jma.ParamA = jma.SValue; 
								else 
									jma.ParamA = jma.SValue - (jma.SValue - jma.ParamA) * sqrtDivider; 
								if (jma.SValue - jma.ParamB < 0) 
									jma.ParamB = jma.SValue; 
								else jma.ParamB = jma.SValue - (jma.SValue - jma.ParamB) * sqrtDivider; 
								JMATempValue = series;
				 
								if (jma.LoopCriteria == 30) { 
									jma.fC0Buffer [i] = series;
									int intPart;
				      
									if (Math.Ceiling(sqrtParam) >= 1) intPart = (int)Math.Ceiling(sqrtParam); else intPart = 1; 
									int leftInt = IntPortion (intPart); 
									if (Math.Floor(sqrtParam) >= 1) intPart = (int)Math.Floor(sqrtParam); else intPart = 1; 
									int rightPart = IntPortion (intPart);
				     
									if (leftInt == rightPart) dValue = 1.0; 
									else 
										dValue = (sqrtParam - rightPart) / (leftInt - rightPart);
									int upShift = 0;
									int dnShift = 0;

									if (rightPart <= 29) 
										upShift = rightPart; 
									else 
										upShift = 29; 
									if (leftInt <= 29) 
										dnShift = leftInt; 
									else dnShift = 29; 
									jma.fA8Buffer [i] = (series - jma.Buffer [jma.LoopParam - upShift]) * (1 - dValue) / rightPart + (series - jma.Buffer[jma.LoopParam - dnShift]) * dValue / leftInt;
								}
							} 
							else {
	      
								dValue = lowDValue / (jma.S38 - jma.S40 + 1);
								if (0.5 <= jma.LogParam - 2.0) powerValue = jma.LogParam - 2.0;
								else powerValue = 0.5;
               
								if (jma.LogParam >= Math.Pow(absValue/dValue, powerValue)) 
									dValue = Math.Pow (absValue/dValue, powerValue); 
								else 
									dValue = jma.LogParam; 
								if (dValue < 1) 
									dValue = 1;
				    
								powerValue = Math.Pow (sqrtDivider, Math.Sqrt (dValue)); 
								if (jma.SValue - jma.ParamA > 0) jma.ParamA = jma.SValue; else jma.ParamA = jma.SValue - (jma.SValue - jma.ParamA) * powerValue; 
								if (jma.SValue - jma.ParamB < 0) jma.ParamB = jma.SValue; else jma.ParamB = jma.SValue - (jma.SValue - jma.ParamB) * powerValue; 
							}
						}

						if (jma.LoopCriteria > 30) {
							JMATempValue = jma [i - 1];
							powerValue   = Math.Pow (jma.LengthDivider, dValue);
							squareValue  = Math.Pow (powerValue, 2);
                         
							jma.fC0Buffer [i] = (1 - powerValue) * series + powerValue * jma.fC0Buffer [i - 1];
							jma.fC8Buffer [i] = (series - jma.fC0Buffer [i]) * (1 - jma.LengthDivider) + jma.LengthDivider * jma.fC8Buffer [i - 1];
            
							jma.fA8Buffer [i] = (jma.PhaseParam * jma.fC8Buffer [i] + jma.fC0Buffer [i] - JMATempValue) * 
								(powerValue * (-2.0) + squareValue + 1) + squareValue * jma.fA8Buffer [i - 1];  
							JMATempValue += jma.fA8Buffer [i]; 
						}
						JMAValue = JMATempValue;
					}
      
					if (jma.LoopParam <= 30) 
						JMAValue = 0;
      
					jma.JMAValueBuffer [i] = JMAValue;
					jma.Add(Convert.ToSingle(JMAValue));
				}
			} 
			return jma;
		}
	}

}
