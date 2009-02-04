/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Gordago.Strategy {
	/// <summary>
	/// Символ в текстовом редакторе
	/// </summary>
	public class TextBoxObjElementChar: TextBoxObjElement {

		private Font _font;
		private Color _foreColor;
		private Brush _foreBrush;

		public TextBoxObjElementChar(char ch):base(ch) {
			this.Font = new Font("Microsoft Sans Serif", 12);
			this.OverInit();
		}

		public TextBoxObjElementChar(char ch, Font font):base(ch) {
			this.Font = font;
			this.OverInit();
		}

		private void OverInit(){
			this.ForeColor = Color.Black;
		}

		#region Public propertyes
		public char Element {
			get {
				return (char)base.Elmnt;
			}
		}

		public Font Font{
			get{return _font;}
			set{
				this._font = value;
				Graphics g = Graphics.FromImage(new Bitmap(10,10));

				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				StringFormat m_sf = new StringFormat(StringFormat.GenericTypographic);
				m_sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces; 
				SizeF sz = g.MeasureString(new string(this.Element,1), _font, 1000, m_sf);
				this.Width = (int)Math.Round(sz.Width);
				this.Height = (int)Math.Round(sz.Height);
			}
		}
		public Color ForeColor{
			get{return this._foreColor;}
			set{
				this._foreColor = value;
				this._foreBrush = new SolidBrush(value);
			}
		}
		#endregion

		protected override void DrawElement(PaintEventArgs e) {
			string s = new string(this.Element, 1);
			e.Graphics.DrawString(s, this.Font, this._foreBrush, this.Left, this.Top);
		}
	}
}
