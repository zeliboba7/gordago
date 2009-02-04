using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iParabolicSARCustomDrawing: DrawingIndicator{
		
		private Pen _pen;
		private Brush _brush;
		private Pen _selectpen;
		private int _pw = 4, _ph = 4;
		public iParabolicSARCustomDrawing(){ 
			_pen = new Pen(Color.Blue);
			_selectpen = new Pen(Color.Blue, 2);

			_brush = new SolidBrush(Color.FromArgb(50, Color.Blue));
		}

		protected override void PaintElement(Graphics g, int funcIndex, int vectorIndex, int x) { 
			foreach (IVector vector in this.Vectors){
				float val = vector[vectorIndex];
				if (!float.IsNaN(val)){
					int y = this.ChartBox.GetY(val);
					switch (this.Status){
						case DrawingIndicatorStatus.Default:
							g.FillEllipse(_brush, x-_pw/2, y-_ph/2, _pw, _ph);
							g.DrawArc(_pen, x-_pw/2, y-_ph/2, _pw, _ph, 0, 360);
							break;
						case DrawingIndicatorStatus.MouseSelected:
							g.FillEllipse(_brush, x-_pw/2, y-_ph/2, _pw, _ph);
							g.DrawArc(_selectpen, x-_pw/2, y-_ph/2, _pw, _ph, 0, 360);
							break;
						case DrawingIndicatorStatus.UserSelected:
							int w = 2;
							g.DrawLine(_pen, x-w, y-w, x+w, y+w);
							g.DrawLine(_pen, x-w, y+w, x+w, y-w);

							g.FillEllipse(_brush, x-_pw/2, y-_ph/2, _pw, _ph);
							g.DrawArc(_selectpen, x-_pw/2, y-_ph/2, _pw, _ph, 0, 360);
							break;
					}
				}
			}
		}
	}

	public class iParabolicSAR: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Absolute;
			this.Name = "Parabolic SAR";
			this.ShortName = "SAR";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("ParabolicSAR");
			this.SetCustomDrawingIndicator(typeof(iParabolicSARCustomDrawing));
		}
	}

	[Function("ParabolicSAR")]
	public class ParabolicSAR : Function {

		#region public class SarVector : Vector 
		public class SarVector : Vector {
			public SarVector() {
			}
			
			public SarVector(int capacity) : 
				base(capacity) {
			}
			
			public float Step {
				get {return step;}
				set {step = value;}
			}
			
			public float High {
				get {return high;}
				set {high = value;}
			}
			
			public float Low {
				get {return low;}
				set {low = value;}
			}
			
			public float PrevStep {
				get {return prevStep;}
				set {prevStep = value;}
			}
			
			public float PrevHigh {
				get {return prevHigh;}
				set {prevHigh = value;}
			}
			
			public float PrevLow {
				get {return prevLow;}
				set {prevLow = value;}
			}
			
			public bool IsLong {
				get {return isLong;}
				set {isLong = value;}
			}
			
			public int Start {
				get {return start;}
				set {start = value;}
			}
			
			public bool First {
				get {return first;}
				set {first = value;}
			}
			
			public bool PrevState {
				get {return prevState;}
				set {prevState = value;}
			}
			
			private float step;
			private float high;
			private float low;
			private float prevStep;
			private float prevHigh;
			private float prevLow;
			private bool isLong;
			private bool prevState;
			private int start = 1;
			private bool first = true;
		}
		#endregion

		protected override void Initialize() {
			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			RegParameter(new ParameterFloat("Step", new string[]{"Step", "Шаг"}, 0.001F, 0.001F, 1, 0.001F, 3));
			RegParameter(new ParameterFloat("Max", new string[]{"Max", "Максимум"}, 0.02F, 0.01F, 1, 0.01F, 2));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector low = (IVector)parameters[0];
			IVector high = (IVector) parameters[1];
			float param1 = (float)parameters[2];
			float param2 = (float)parameters[3];

			SarVector sv = result as SarVector;
			if (sv == null) {
				sv = new SarVector();
				sv.Step = param1;
				sv.First = false;
				result = sv;
			}

			if (result.Count == high.Count)
				result.RemoveLastValue();

			for (int j = sv.Count; j < high.Count; j++){
				if (j < 2){
					result.Add(float.NaN);
				}else{
					if (float.IsNaN(result.Current)){
						if ((high[j] > high[(j - 1)]) && (low[j] > low[(j - 1)])) {
							sv.IsLong = true;
							sv.Step = param1;
							sv.High = high[j];
							sv.Low = low[j - 1];
							sv.First = true;
							result.Add((low[j - 1]));
						}else if (high[j] < high[j - 1] && low[j] < low[j - 1]){
							sv.IsLong = false;
							sv.Step = param1;
							sv.High = high[(j - 1)];
							sv.Low = low[j];
							sv.First = true;
							result.Add(((float) high[(j - 1)]));
						}else{
							sv.Start++;
							result.Add(float.NaN);
						}
					} else {
						sv.First = false;
						sv.PrevHigh = sv.High;
						sv.PrevLow = sv.Low;
						sv.PrevStep = sv.Step;
						sv.PrevState = sv.IsLong;
					
						if (sv.High < high[j])
							sv.High = high[j];
						if (sv.Low > low[j])
							sv.Low = low[j];

						if (sv.IsLong) {
							if ((high[j] > high[(j - 1)]) && (low[j] > low[(j - 1)]))
								if (sv.Step < param2)
									sv.Step = sv.Step + param1;
								else sv.Step = param2;

							float sar = (sv.Step * (sv.High - sv.Current)) + sv.Current;
							if (sar > low[j]) {
								sv.IsLong = false;
								sv.Step = param1;
								result.Add(sv.High);
								sv.High = high[j];
								sv.Low = low[j];
								sv.First = true;
							}else
								result.Add(sar);
						} else {
							if ((low[j] < low[(j - 1)]) && (high[j] < high[(j - 1)]))
								if (sv.Step < param2)
									sv.Step = sv.Step + param1;
								else sv.Step = param2;
							float sar = (sv.Step * (sv.Low - sv.Current)) + sv.Current;
							if (sar < high[j]) {
								sv.IsLong = true;
								sv.Step = param1;
								result.Add(sv.Low);
								sv.Low = low[j];
								sv.High = high[j];
								sv.First = true;
							} else 
								result.Add(sar);
						}
					}
				}
			}
			return result;
		}
	}
}
