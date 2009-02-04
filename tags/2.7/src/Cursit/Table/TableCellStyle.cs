/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Cursit.Table {
	/// <summary>
	/// Стиль отображение строки или ячейки
	/// </summary>
	public class TableCellStyle {
		private Color _backcolor;
		private Color _selectcolor;
		private Color _forecolor;
		private Brush _forebrush;
		private Font _font;
		private Color _gridlinecolor;
		private Pen _gridlinepen;
		private ContentAlignment _textalignment;
		private StringFormat _stringformat;

		public TableCellStyle() {
			this.BackColor = Color.White;
			this.SelectColor = Color.BlueViolet;
			this.ForeColor = Color.Black;
			this.GridLineColor = Color.Gainsboro;
			this.TextAlignment = ContentAlignment.MiddleLeft;
      this.Font = new Font("Microsoft Sans Serif", 8);
    }

		#region public Color BackColor
		public Color BackColor{
			get{return this._backcolor;}
			set{this._backcolor = value;}
		}
		#endregion

		#region public Color SelectColor
		public Color SelectColor{
			get{return this._selectcolor;}
			set{this._selectcolor = value;}
		}
		#endregion

		#region public Color ForeColor
		public Color ForeColor{
			get{return this._forecolor;}
			set{this._forecolor = value;
				_forebrush = new SolidBrush(value);
			}
		}
		#endregion

		#region internal Brush ForeBrush
		internal Brush ForeBrush{
			get{return this._forebrush;}
		}
		#endregion

		#region public Font Font
		public Font Font{
			get{return this._font;}
			set{this._font = value;}
		}
		#endregion

		#region public ContentAlignment TextAlignment
		public ContentAlignment TextAlignment{
			get{return this._textalignment;}
			set{
				this._textalignment = value;
				StringAlignment[] sals = TableControl.ConvertContentAlignmentToSAlignment(value);
				if (_stringformat == null)
					_stringformat = new StringFormat();

				_stringformat.LineAlignment = sals[0];
				_stringformat.Alignment = sals[1];
				_stringformat.FormatFlags = StringFormatFlags.NoWrap;
			}
		}
		#endregion

		#region internal StringFormat StringFormat
		internal StringFormat StringFormat{
			get{return this._stringformat;}
		}
		#endregion

		#region public Color GridLineColor
		public Color GridLineColor{
			get{return this._gridlinecolor;}
			set{
				this._gridlinecolor = value;
				_gridlinepen = new Pen(value);
			}
		}
		#endregion

		#region internal Pen GridLinePen
		internal Pen GridLinePen{
			get{return this._gridlinepen;}
		}
		#endregion

		public TableCellStyle Clone(){
			TableCellStyle tcs = new TableCellStyle();
			tcs.BackColor = this.BackColor;
			tcs.Font = this.Font;
			tcs.ForeColor = this.ForeColor;
			tcs.GridLineColor = this.GridLineColor;
			tcs.SelectColor = this.SelectColor;
			tcs.TextAlignment = this.TextAlignment;
			return tcs;
		}
	}
}
