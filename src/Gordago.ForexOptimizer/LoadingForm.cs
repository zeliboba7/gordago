/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
using System.Windows.Forms;

namespace Gordago {
	public class LoadingForm : System.Windows.Forms.Form {
		private System.ComponentModel.Container components = null;
		System.Drawing.Image _img;

		public LoadingForm() {
			string path = System.Windows.Forms.Application.StartupPath;
			try{
				_img = Image.FromFile(path + @"\resources\loading.gif");
			}catch{}
			InitializeComponent();

			this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
		}


		#region protected override void Dispose( bool disposing )
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
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
			// LoadingForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(460, 180);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "LoadingForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "LoadingForm";
			this.TopMost = true;

		}
		#endregion
		
		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint (e);
			if (_img != null){
				e.Graphics.DrawImage(_img, 0,0,_img.Width, _img.Height);
			}
		}

	}
}
