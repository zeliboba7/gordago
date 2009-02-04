#region using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Cursit.Applic.APropGrid;
using Cursit.Dock;
#endregion
using System.Drawing.Drawing2D;

namespace TestCursitLibrary {
	public class MainForm : System.Windows.Forms.Form {
		private System.ComponentModel.Container components = null;

		private DockBox _dockbox;
		
		public MainForm() {
			InitializeComponent();
			this.AllowDrop = true;

			this._dockbox = new DockBox();
			this._dockbox.Text = "Вот такой крутой компонент";
			this.Controls.Add(this._dockbox);

		}

		#region protected override void Dispose( bool disposing )
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(528, 373);
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.ForeColor = System.Drawing.Color.Salmon;
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Тестирование Cursit";

		}
		#endregion

		[STAThread]
		static void Main() {
			Application.Run(new MainForm());
		}

		protected override void OnDragEnter(DragEventArgs drgevent) {
			if (drgevent.Data.GetDataPresent(typeof(DockBox))){
				drgevent.Effect = DragDropEffects.Move;
			}
		}

	}
}
