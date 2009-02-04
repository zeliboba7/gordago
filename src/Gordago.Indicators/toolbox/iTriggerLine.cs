using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iTriggerLine: Indicator {

		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Signal indicators";
			this.Name = "TriggerLine";
			this.ShortName = "TriggerLine";
			this.SetImage("i.gif");
			this.IsSeparateWindow = true;
			this.SetCustomDrawingIndicator(typeof (iTriggerLineCustomDrawing));

			RegFunction("TriggerLineSell").SetDrawPen(new Pen(Color.Red,2));
			RegFunction("TriggerLineBuy").SetDrawPen(new Pen(Color.RoyalBlue,2));
		}
	}

	#region public class TriggerLine:Function
	[Function("TriggerLine")]
	public class TriggerLine:Function{

		public class TriggerVector: Vector{
			public IVector WT_Vector;
			public IVector LSMA_MA_Vector;

			public TriggerVector(){
				WT_Vector = new Vector();
				LSMA_MA_Vector = new Vector();
			}
		}

		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("RPeriod", 30, 1, 100000));
			this.RegParameter(new ParameterInteger("LSMA_Period", 5, 1, 100000));
			this.RegParameter(Function.CreateDefParam_ApplyTo("Close"));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			float lsmaperiod = (int)parameters[1];
			IVector vector = (IVector)parameters[2];

			if (result == null)
				result = new TriggerVector();

			TriggerVector tresult = result as TriggerVector;

			if (tresult.Count == vector.Count){
				tresult.RemoveLastValue();
				tresult.WT_Vector.RemoveLastValue();
				tresult.LSMA_MA_Vector.RemoveLastValue();
			}

			for(int shift = result.Count; shift < vector.Count; shift++){
				tresult.Add(float.NaN);
				tresult.WT_Vector.Add(float.NaN);
				tresult.LSMA_MA_Vector.Add(float.NaN);

				float koef = ((float)period + 1)/3f;
				if (shift >= period){
					float sum = 0;
					for(int i=0; i < period; i++)
						sum += ((float)(period-i) - koef) * vector[shift-i];
      
					tresult.WT_Vector[shift] = sum*6 / (period * (period+1));
					tresult.LSMA_MA_Vector[shift] = tresult.WT_Vector[shift-1] + 
						(tresult.WT_Vector[shift]-tresult.WT_Vector[shift-1])* 2 / (lsmaperiod+1);

					tresult[shift] = (tresult.WT_Vector[shift]+tresult.LSMA_MA_Vector[shift])/2;
				}
			}
			return tresult;
		}
	}
	#endregion

	#region public class TriggerLineSell: Function
	[Function("TriggerLineSell")]
	public class TriggerLineSell: Function{
		private Function _functrigger;

		protected override void Initialize() {
			_functrigger = this.GetFunction("TriggerLine");
			this.RegParameter(_functrigger);
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			TriggerLine.TriggerVector tresult = analyzer.Compute(_functrigger, parameters[0], parameters[1], parameters[2]) as TriggerLine.TriggerVector;

			if (result == null)
				result = new Vector();

			if (result.Count == tresult.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<tresult.Count;i++){
				result.Add(float.NaN);

				if (tresult.WT_Vector[i] < tresult.LSMA_MA_Vector[i]){
					result[i] = tresult[i];
				}else if (tresult.WT_Vector[i] == tresult.LSMA_MA_Vector[i]){
//					if (i>0 && !float.IsNaN(result[i-1]))
//						result[i] = tresult[i];
				}

			}
			return result;	
		}
	}
	#endregion

	#region public class TriggerLineBuy: Function
	[Function("TriggerLineBuy")]
	public class TriggerLineBuy: Function{
		private Function _functrigger;

		protected override void Initialize() {
			_functrigger = this.GetFunction("TriggerLine");
			this.RegParameter(_functrigger);
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			TriggerLine.TriggerVector tresult = analyzer.Compute(_functrigger, parameters[0], parameters[1], parameters[2]) as TriggerLine.TriggerVector;

			if (result == null)
				result = new Vector();

			if (result.Count == tresult.Count)
				result.RemoveLastValue();

			for (int i=result.Count;i<tresult.Count;i++){
				result.Add(float.NaN);
				if (tresult.WT_Vector[i] > tresult.LSMA_MA_Vector[i]){
					result[i] = tresult[i];
				}else if (tresult.WT_Vector[i] == tresult.LSMA_MA_Vector[i]){
//					if (i>0 && !float.IsNaN(result[i-1]))
//						result[i] = tresult[i];
				}

			}
			return result;
		}

	}
	#endregion

	#region public class iTriggerLineCustomDrawing: DrawingIndicator
	public class iTriggerLineCustomDrawing: DrawingIndicator{
		
		private Pen _penSell;
		private Brush _brushSell;
		private Pen _selectpenSell;

		private Pen _penBuy;
		private Brush _brushBuy;
		private Pen _selectpenBuy;

		private int _pw = 4, _ph = 4;

		public iTriggerLineCustomDrawing(){ 
			_penSell = new Pen(Color.Red, 1);
			_selectpenSell = new Pen(Color.Red, 2);
			_brushSell = new SolidBrush(Color.FromArgb(50, Color.Red));

			_penBuy = new Pen(Color.Blue, 1);
			_selectpenBuy = new Pen(Color.Blue, 2);
			_brushBuy = new SolidBrush(Color.FromArgb(50, Color.Blue));
		}

    protected override void PaintElement(Graphics g, int funcIndex, int vectorIndex, int x) { 
			Pen pen = _penBuy, selpen = _selectpenBuy;
			Brush brush = _brushBuy;
			float val = this.Vectors[1][vectorIndex];

			if (!float.IsNaN(this.Vectors[0][vectorIndex])){
				pen = _penSell;
				selpen = _selectpenSell;
				brush = _brushSell;
				val = this.Vectors[0][vectorIndex];
			}

			if (!float.IsNaN(val)){
        int y = this.ChartBox.GetY(val);
				switch (this.Status){
					case DrawingIndicatorStatus.Default:
						g.FillEllipse(brush, x-_pw/2, y-_ph/2, _pw, _ph);
						g.DrawArc(pen, x-_pw/2, y-_ph/2, _pw, _ph, 0, 360);
						break;
					case DrawingIndicatorStatus.MouseSelected:
						g.FillEllipse(brush, x-_pw/2, y-_ph/2, _pw, _ph);
						g.DrawArc(selpen, x-_pw/2, y-_ph/2, _pw, _ph, 0, 360);
						break;
					case DrawingIndicatorStatus.UserSelected:
						int w = 2;
						g.DrawLine(pen, x-w, y-w, x+w, y+w);
						g.DrawLine(pen, x-w, y+w, x+w, y-w);

						g.FillEllipse(brush, x-_pw/2, y-_ph/2, _pw, _ph);
						g.DrawArc(selpen, x-_pw/2, y-_ph/2, _pw, _ph, 0, 360);
						break;
				}
			}
		}
	}
	#endregion
}
