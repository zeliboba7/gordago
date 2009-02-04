/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Gordago.Analysis.Chart {
	public class ChartFigureProgressBar: ChartFigure {
		#region private property
		private int _total=100, _current=0;

		private Color _bordercolor, _backcolor, _progresscolor;
		private Brush _progressbrush, _backbrush;
		private Pen _borderpen;

		private int _pwidth, _pheight;
		#endregion

		#region public ChartFigureProgressBar(string name):base(name) 
		public ChartFigureProgressBar(string name):base(name, false) {
			this.ProgressColor = Color.Blue;
			this.BorderColor = Color.Black;
			this.BackColor = Color.Blue;
		}
		#endregion

		#region public int Total
		public int Total{
			get{return this._total;}
			set{this._total = value;}
		}
		#endregion

		#region public int Current
		public int Current{
			get{return this._current;}
			set{this._current = value;}
		}
		#endregion

		#region public Color ProgressColor
		public Color ProgressColor{
			get{return this._progresscolor;}
			set{
				this._progresscolor = value;
				_progressbrush = new SolidBrush(value);
			}
		}
		#endregion

		#region public Color BorderColor
		public Color BorderColor{
			get{return this._bordercolor;}
			set{
				this._bordercolor = value;
				this._borderpen = new Pen(value);
			}
		}
		#endregion

		#region public Color BackColor
		public Color BackColor{
			get{return this._backcolor;}
			set{
				this._backcolor = value;
				this._backbrush = new SolidBrush(value);
			}
		}
		#endregion

    protected override void OnCalculateScale() {
    }

    protected override void OnPaint(Graphics g) {
      _pheight = this.ChartBox.Height / 20;
      _pwidth = this.ChartBox.Width;
      int w = 1;
      if(this.Total > 0)
        w = (this.ChartBox.Width * this.Current) / this.Total;
      int y1 = this.ChartBox.Height - _pheight;
      g.FillRectangle(this._backbrush, 0, y1, _pwidth, _pheight);
      g.FillRectangle(this._progressbrush, 0, y1, w, _pheight);
      g.DrawRectangle(this._borderpen, 0, y1, _pwidth, _pheight);
    }
		
	}
}
