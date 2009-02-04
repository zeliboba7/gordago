/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Gordago.Analysis.Chart {

	public class ChartFigureText: ChartFigure {

		private string _text = "";
		private int _x, _y, _twidth = 0, _theigh = 0;
		private Font _font;
		private Color _forecolor;
		private Brush _forebrush;
		private ContentAlignment _textalignment;
		private StringFormat _stringformat;
		

		public ChartFigureText(string name, string text, Font font, Color color, int x, int y):base(name, false){
			_text = text;
			_x = x;
			_y = y;
			this.Font = font;
			this.ForeColor = color;
			this.TextAlignment = ContentAlignment.TopLeft;
		}

		public ChartFigureText(string name, string text, Font font, Color color, int x, int y, int width, int height): this(name, text, font, color, x, y){
			this._twidth = width;
			this._theigh = height;
		}

		#region public string Text
		public string Text{
			get{return this._text;}
			set{this._text = value;}
		}
		#endregion

		#region public int X
		public int X{
			get{return this._x;}
			set{this._x = value;}
		}
		#endregion

		#region public int Y
		public int Y{
			get{return this._y;}
			set{this._y = value;}
		}
		#endregion

		#region public Font Font
		public Font Font{
			get{return this._font;}
			set{this._font = value;}
		}
		#endregion

		#region public Color ForeColor
		public Color ForeColor{
			get{return this._forecolor;}
			set{
				this._forecolor = value;
				this._forebrush = new SolidBrush(value);
			}
		}
		#endregion

		#region public ContentAlignment TextAlignment
		public ContentAlignment TextAlignment{
			get{return this._textalignment;}
			set{
				this._textalignment = value;
				StringAlignment[] sals = ConvertContentAlignmentToSAlignment(value);
				if (_stringformat == null)
					_stringformat = new StringFormat();

				_stringformat.LineAlignment = sals[0];
				_stringformat.Alignment = sals[1];
				_stringformat.FormatFlags = StringFormatFlags.NoWrap;
			}
		}
		#endregion

		#region public int TWidth
		public int TWidth{
			get{return this._twidth;}
			set{this._twidth = value;}
		}
		#endregion

		#region public int THeigh
		public int THeigh{
			get{return this._theigh;}
			set{this._theigh = value;}
		}
		#endregion

		#region internal static StringAlignment[] ConvertContentAlignmentToSAlignment(ContentAlignment contentalignment)
		internal static StringAlignment[] ConvertContentAlignmentToSAlignment(ContentAlignment contentalignment){
			StringAlignment hal = StringAlignment.Far, val = StringAlignment.Far;

			switch(contentalignment){
				case ContentAlignment.BottomCenter:
					hal = StringAlignment.Center;
					val = StringAlignment.Far;
					break;
				case ContentAlignment.BottomLeft:
					hal = StringAlignment.Near;
					val = StringAlignment.Far;
					break;
				case ContentAlignment.BottomRight:
					hal = StringAlignment.Far;
					val = StringAlignment.Far;
					break;
				case ContentAlignment.MiddleCenter:
					hal = StringAlignment.Center;
					val = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleLeft:
					hal = StringAlignment.Near;
					val = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleRight:
					hal = StringAlignment.Far;
					val = StringAlignment.Center;
					break;
				case ContentAlignment.TopCenter:
					hal = StringAlignment.Center;
					val = StringAlignment.Near;
					break;
				case ContentAlignment.TopLeft:
					hal = StringAlignment.Near;
					val = StringAlignment.Near;
					break;
				case ContentAlignment.TopRight:
					hal = StringAlignment.Far;
					val = StringAlignment.Near;
					break;
			}
			return new StringAlignment[]{val, hal};
		}
		#endregion

    protected internal override void OnPaint(Graphics g) {
			RectangleF rect;
			if (this._twidth == 0 || this._theigh == 0)
        rect = new RectangleF(X, Y, this.ChartBox.Width - X, this.ChartBox.Height - Y);
			else
				rect = new RectangleF(X, Y, _twidth, _theigh);
			g.DrawString(this.Text, this.Font, this._forebrush, rect, this._stringformat);
		}
	}
}
