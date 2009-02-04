/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cursit.Applic.AGUI.Primitive {
	/// <summary>
	/// Базовый класс кнопки
	/// </summary>
	public class AButtonPrimitive: Control{

		private Color _backColor;
		private Color _backColorSelect;
		private Color _borderColor;
		private Pen _penBorder;
		private Brush _fontBrush;
		private Rectangle _rectDrwString;

		private StringFormat _stringFormat;

		/// <summary>
		/// true - компонент в данный момент находится под мышью
		/// </summary>
		private bool _isSelect = false;

		public AButtonPrimitive() {
			this.Size = new Size(40,14);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint, true);
			base.BackColor = _backColor = Color.LightGray;
						
			_backColorSelect = Color.WhiteSmoke;
			_borderColor = Color.Black;
			_penBorder = new Pen(_borderColor);
			_fontBrush = new SolidBrush(this.ForeColor);
			this._stringFormat = new StringFormat();
			this._stringFormat.Alignment = StringAlignment.Center;
			this._stringFormat.LineAlignment = StringAlignment.Center;
			//this.SetRectDrawString();
		}

//		private void SetRectDrawString(){
//		}

		public Color BackColorUnSelect{
			get{return this._backColor;}
			set{this.BackColor = this._backColor = value;}
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize (e);
			this._rectDrwString = new Rectangle(0,0, this.Width, this.Height-2);
		}

		#region public override Color ForeColor 
		public override Color ForeColor {
			get {
				return base.ForeColor;
			}
			set {
				if (this.ForeColor == value) return;

				base.ForeColor = value;
				this._fontBrush = new SolidBrush(value);
			}
		}
		#endregion

		#region public Color BackColorSelect
		public Color BackColorSelect{
			get{return this._backColorSelect;}
			set{
				this._backColorSelect = value;
			}
		}
		#endregion

		#region public Color BorderColor
		public Color BorderColor{
			get{return this._borderColor;}
			set{
				if (this._borderColor == value) return;
				this._borderColor = value;
				_penBorder = new Pen(value);
			}
		}
		#endregion

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint (e);
			Graphics g = e.Graphics;
			g.DrawRectangle(this._penBorder, 0 , 0, this.Width-1, this.Height-1);
			g.DrawString(Text, Font, this._fontBrush, _rectDrwString, this._stringFormat);
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave (e);
			this.BackColor = this._backColor;
			this._isSelect = false;
		}

		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter (e);
			if (!this._isSelect){
				this.BackColor = this._backColorSelect;
				this._isSelect = true;
			}
		}
		protected override void OnMouseHover(EventArgs e) {
			base.OnMouseHover (e);
		}
	}
}
