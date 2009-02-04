using System;
using System.Drawing;
using System.Windows.Forms;

using Cursit.Applic.AGUI.Primitive;

namespace Cursit.Applic.APropGrid {

	/// <summary>
	/// Базовый класс редактора свойства в PropGridRow
	/// </summary>
	public class PropGridViewerPrimitive: Control{

		internal Control _button;
		private Control _vViewer;
		private EventArgs _eventArgs;
		internal PropGridValue _pgValue;

		private Rectangle _emptyRect;

		public PropGridViewerPrimitive(PropGridValue pgValue) {
			this._pgValue = pgValue;
			this._eventArgs = new EventArgs();
			this.Recalcul();
		}

		public PropGridValue PropGridValue{
			get{return this._pgValue;}
			set{this._pgValue = value;}
		}

		protected void SetVButton(Control vButton){
			this._button = vButton;
			this.Controls.Add(_button);
		}

		protected void SetVViewer(Control vViewer){
			this._vViewer = vViewer;
			this._vViewer.Click += new EventHandler(this.VViewer_Click);
			this._vViewer.LostFocus += new EventHandler(this.VViewer_LostFocus); 
			RecalculViwer();
			this.Controls.Add(vViewer);
		}

		/// <summary>
		/// Свободная от кнопки область, для прорисовки оставшихся компонентов
		/// </summary>
		public Rectangle EmptyRect{
			get{return this._emptyRect;}
		}

		private void Recalcul() {
			int w = this.Width;
			int h = this.Height;
			if (this._button != null) {
				int wb = _button.Width;
				this._emptyRect = new Rectangle(0,0, w-wb, h);
				//_button.Height = this.Height-2;
				_button.Location = new Point(w-wb, 0);
			}else
				this._emptyRect = new Rectangle(0,0,w,h);
		}

		private void RecalculViwer(){
			if (this._vViewer == null) return;
			int wb = this._button != null ? _button.Width : 0;

			if (_vViewer is NumericUpDown)
				this._vViewer.Bounds = new Rectangle(1,0, this.Width-wb, this.Height-1);
			else
				this._vViewer.Bounds = new Rectangle(1,0, this.Width-wb-5, this.Height-1);
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize (e);
			this.Recalcul();
			RecalculViwer();
			this.Invalidate();
		}

		private void VViewer_LostFocus(object sender, System.EventArgs e){
			this.UnSelectRow();
		}
				
		private void VViewer_Click(object sender, System.EventArgs e){
			this.OnClick(this._eventArgs);
			this.SelectRow();
		}


		public virtual void SelectRow(){
			this._vViewer.Focus();
		}

		public virtual void UnSelectRow(){
		}
	}
}
