using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iTimeCustomDrawing: DrawingIndicator{
		
		private Pen _pen;
		private Brush _brush;
		private Pen _selectpen;
		private int _pw = 8, _ph = 8;
		public iTimeCustomDrawing(){ 
			_pen = new Pen(Color.Red, 2);
			_selectpen = new Pen(Color.Red, 4);

			_brush = new SolidBrush(Color.FromArgb(50, Color.Red));
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
}
