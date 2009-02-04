/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Windows.Forms;

namespace Gordago.Strategy {
	public class TextBoxObjElementCtrl: TextBoxObjElement {
		private int _lrout;
		public event EventHandler Resize;

		public TextBoxObjElementCtrl(Control ctrl):base(ctrl) {
			this._lrout = 3;
			ctrl.Resize += new EventHandler(this.Ctrl_ReSize);
			this.SetSize();
		}

		public Control Element{
			get{return (Control)base.Elmnt;}
		}

		#region private void Ctrl_ReSize(object sender, EventArgs e)
		private void Ctrl_ReSize(object sender, EventArgs e){
			this.SetSize();
			if (this.Resize != null)
				this.Resize(this, new EventArgs());
		}
		#endregion

		#region private void SetSize()
		/// <summary>
		/// Установка размеров элемента
		/// </summary>
		private void SetSize(){
			this.Width = this.Element.Width+_lrout*2;
			this.Height = this.Element.Height;
		}
		#endregion

		#region public override int Left 
		public override int Left {
			get {
				return base.Left;
			}
			set {
				base.Left = value;
				this.Element.Left = value+this._lrout+2;
			}
		}
		#endregion

		#region protected override void DrawElement(PaintEventArgs e)
		protected override void DrawElement(PaintEventArgs e) {	}
		#endregion
	}
}
