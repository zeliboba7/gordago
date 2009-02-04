using System;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid{
		/// <summary>
		/// Summary description for TextBoxViewer.
		/// </summary>
		internal class TextBoxViewer: TextBox {
				public TextBoxViewer():base() {
						this.BorderStyle = BorderStyle.None;
						this.Cursor = Cursors.Arrow;
						this.Multiline = false;
				}
				protected override void OnGotFocus(EventArgs e) {
						base.OnGotFocus (e);
						Cursor = Cursors.IBeam;
						//this.SelectAll();
						this.SelectionStart = 0;
						this.SelectionLength = this.Text.Length;
				}

				protected override void OnLostFocus(EventArgs e) {
						base.OnLostFocus (e);
						Cursor = Cursors.Arrow;
						SelectionStart = 0;
				}

		}
}
