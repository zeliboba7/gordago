/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region Using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
#endregion
using Gordago.API;

namespace Gordago.API {
	public class ConnectSettingForm : System.Windows.Forms.Form {

		#region Private property

		private System.Windows.Forms.Button _btnOK;
		private System.Windows.Forms.Button _btnCancel;
		private Gordago.API.HTTPSettingPanel _httpProxySetting;
		private System.ComponentModel.Container components = null;
		#endregion

		public ConnectSettingForm() {
			InitializeComponent();
		}

		public HTTPSettingPanel HTTPSettingPanel{
			get{return this._httpProxySetting;}
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
      this._btnOK = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this._httpProxySetting = new Gordago.API.HTTPSettingPanel();
      this.SuspendLayout();
      // 
      // _btnOK
      // 
      this._btnOK.Location = new System.Drawing.Point(96, 198);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 4;
      this._btnOK.Text = "OK";
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(196, 198);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 5;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // _httpProxySetting
      // 
      this._httpProxySetting.Location = new System.Drawing.Point(0, 0);
      this._httpProxySetting.Name = "_httpProxySetting";
      this._httpProxySetting.ProxyFromIE = false;
      this._httpProxySetting.ProxyPass = "";
      this._httpProxySetting.ProxyPort = 0;
      this._httpProxySetting.ProxyServer = "";
      this._httpProxySetting.ProxyUser = "";
      this._httpProxySetting.Size = new System.Drawing.Size(360, 191);
      this._httpProxySetting.TabIndex = 6;
      this._httpProxySetting.UseProxy = false;
      // 
      // ConnectSettingForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(362, 228);
      this.Controls.Add(this._httpProxySetting);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ConnectSettingForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Settings";
      this.ResumeLayout(false);

		}
		#endregion

		#region private void _btnOK_Click(object sender, System.EventArgs e) 
		private void _btnOK_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		#endregion

		#region private void _btnCancel_Click(object sender, System.EventArgs e) 
		private void _btnCancel_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion


  }
}
