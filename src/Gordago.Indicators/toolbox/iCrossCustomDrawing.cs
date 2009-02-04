using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iCrossCustomDrawing: DrawingIndicator {
		
		private Pen _penup, _pendown;
		private Brush _brushup, _brushdown;
		private Pen _selpenup, _selpendown;
		private int _pw = 10, _ph = 10;
		public iCrossCustomDrawing(){ 
			Color cup = Color.FromArgb(239, 30, 202);
			Color cdown = Color.FromArgb(27, 81, 235);
			_penup = new Pen(cup, 2);
			_pendown = new Pen(cdown, 2);

			_selpenup = new Pen(cup, 4);
			_selpendown = new Pen(cdown, 4);

			_brushup = new SolidBrush(Color.FromArgb(50, cup));
			_brushdown = new SolidBrush(Color.FromArgb(50, cdown));
		}

    protected override void PaintElement(Graphics g, int funcIndex, int vectorIndex, int x) { 
			foreach (IVector vector in this.Vectors){
				float val = vector[vectorIndex];
				float high = this.ChartBox.ChartManager.Bars[vectorIndex].High;

				if (!float.IsNaN(val)){

          int y = this.ChartBox.GetY(val);
					Brush brush;
					Pen pen, selpen;
					if (val > high){
						brush = _brushup;
						pen = _penup;
						selpen = _selpenup;
					}else{
						brush = _brushdown;
						pen = _pendown;
						selpen = _selpendown;
					}

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
	}
}
