/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Gordago.WebUpdate;
using Language;
#endregion

namespace Gordago.API {

	public class HTTPSettingPanel : System.Windows.Forms.UserControl {
		private System.Windows.Forms.GroupBox _gboxMain;
		private System.Windows.Forms.CheckBox _chkUserProxy;
		private System.Windows.Forms.TextBox _txtProxyServer;
		private System.Windows.Forms.Label _lblServername;
		private System.Windows.Forms.Label _lblserverport;
		private System.Windows.Forms.TextBox _txtProxyPort;
		private System.Windows.Forms.Label _lblusername;
		private System.Windows.Forms.TextBox _txtProxyUser;
		private System.Windows.Forms.Label _lblpassword;
		private System.Windows.Forms.TextBox _txtProxyPass;
		private System.Windows.Forms.CheckBox _chkGetFromIE;

		private System.ComponentModel.Container components = null;

		#region public HTTPSettingPanel() 
		public HTTPSettingPanel() {
			InitializeComponent();
			try{
				this._gboxMain.Text = Dictionary.GetString(29,19,"HTTP Proxy Settings");
				this._chkGetFromIE.Text = Dictionary.GetString(29,20,"Get settings from Internet Explorer");
				this._chkUserProxy.Text = Dictionary.GetString(29,21,"To establish settings manually");
				this._lblServername.Text = Dictionary.GetString(29,22,"Server:");
				this._lblserverport.Text = Dictionary.GetString(29,23,"Port:");
				this._lblusername.Text = Dictionary.GetString(29,24,"User Name:");
				this._lblpassword.Text = Dictionary.GetString(29,25,"Password:");
			}catch{}
		}
		#endregion

		#region public string ProxyServer
		public string ProxyServer{
			get{return this._txtProxyServer.Text;}
			set{this._txtProxyServer.Text = value;}
		}
		#endregion

		#region public int ProxyPort
		public int ProxyPort{
			get{
				int port = 0;
				try{
					port = Convert.ToInt32(this._txtProxyPort.Text);
				}catch{}
				return port;
			}
			set{
				this._txtProxyPort.Text = Convert.ToString(value);
			}
		}
		#endregion

		#region public string ProxyUser
		public string ProxyUser{
			get{return this._txtProxyUser.Text;}
			set{this._txtProxyUser.Text = value;}
		}
		#endregion

		#region public string ProxyPass
		public string ProxyPass{
			get{return this._txtProxyPass.Text;}
			set{this._txtProxyPass.Text = value;}
		}
		#endregion

		#region public bool UseProxy
		public bool UseProxy{
			get{return this._chkUserProxy.Checked;}
			set{
				this._chkUserProxy.Checked = value;
				this.RefreshStatus();
			}
		}
		#endregion

		#region public bool ProxyFromIE
		public bool ProxyFromIE{
			get{return this._chkGetFromIE.Checked;}
			set{this._chkGetFromIE.Checked = value;}
		}
		#endregion

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
      this._gboxMain = new System.Windows.Forms.GroupBox();
      this._chkGetFromIE = new System.Windows.Forms.CheckBox();
      this._chkUserProxy = new System.Windows.Forms.CheckBox();
      this._txtProxyServer = new System.Windows.Forms.TextBox();
      this._lblServername = new System.Windows.Forms.Label();
      this._lblserverport = new System.Windows.Forms.Label();
      this._txtProxyPort = new System.Windows.Forms.TextBox();
      this._lblusername = new System.Windows.Forms.Label();
      this._txtProxyUser = new System.Windows.Forms.TextBox();
      this._lblpassword = new System.Windows.Forms.Label();
      this._txtProxyPass = new System.Windows.Forms.TextBox();
      this._gboxMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // _gboxMain
      // 
      this._gboxMain.Controls.Add(this._chkGetFromIE);
      this._gboxMain.Controls.Add(this._chkUserProxy);
      this._gboxMain.Controls.Add(this._txtProxyServer);
      this._gboxMain.Controls.Add(this._lblServername);
      this._gboxMain.Controls.Add(this._lblserverport);
      this._gboxMain.Controls.Add(this._txtProxyPort);
      this._gboxMain.Controls.Add(this._lblusername);
      this._gboxMain.Controls.Add(this._txtProxyUser);
      this._gboxMain.Controls.Add(this._lblpassword);
      this._gboxMain.Controls.Add(this._txtProxyPass);
      this._gboxMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this._gboxMain.Location = new System.Drawing.Point(0, 0);
      this._gboxMain.Name = "_gboxMain";
      this._gboxMain.Size = new System.Drawing.Size(344, 191);
      this._gboxMain.TabIndex = 7;
      this._gboxMain.TabStop = false;
      this._gboxMain.Text = "HTTP Proxy settings";
      // 
      // _chkGetFromIE
      // 
      this._chkGetFromIE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._chkGetFromIE.Location = new System.Drawing.Point(320, 19);
      this._chkGetFromIE.Name = "_chkGetFromIE";
      this._chkGetFromIE.Size = new System.Drawing.Size(320, 24);
      this._chkGetFromIE.TabIndex = 4;
      this._chkGetFromIE.Text = "Get settings from Internet Explorer";
      this._chkGetFromIE.Visible = false;
      this._chkGetFromIE.CheckedChanged += new System.EventHandler(this._chkGetFromIE_CheckedChanged);
      // 
      // _chkUserProxy
      // 
      this._chkUserProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._chkUserProxy.Location = new System.Drawing.Point(16, 20);
      this._chkUserProxy.Name = "_chkUserProxy";
      this._chkUserProxy.Size = new System.Drawing.Size(320, 24);
      this._chkUserProxy.TabIndex = 0;
      this._chkUserProxy.Text = "To establish settings manually";
      this._chkUserProxy.CheckedChanged += new System.EventHandler(this._chkUserProxy_CheckedChanged);
      // 
      // _txtProxyServer
      // 
      this._txtProxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtProxyServer.Location = new System.Drawing.Point(8, 70);
      this._txtProxyServer.Name = "_txtProxyServer";
      this._txtProxyServer.Size = new System.Drawing.Size(264, 20);
      this._txtProxyServer.TabIndex = 3;
      // 
      // _lblServername
      // 
      this._lblServername.AutoSize = true;
      this._lblServername.Location = new System.Drawing.Point(8, 52);
      this._lblServername.Name = "_lblServername";
      this._lblServername.Size = new System.Drawing.Size(41, 13);
      this._lblServername.TabIndex = 2;
      this._lblServername.Text = "Server:";
      // 
      // _lblserverport
      // 
      this._lblserverport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._lblserverport.AutoSize = true;
      this._lblserverport.Location = new System.Drawing.Point(272, 52);
      this._lblserverport.Name = "_lblserverport";
      this._lblserverport.Size = new System.Drawing.Size(29, 13);
      this._lblserverport.TabIndex = 2;
      this._lblserverport.Text = "Port:";
      // 
      // _txtProxyPort
      // 
      this._txtProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._txtProxyPort.Location = new System.Drawing.Point(272, 70);
      this._txtProxyPort.Name = "_txtProxyPort";
      this._txtProxyPort.Size = new System.Drawing.Size(64, 20);
      this._txtProxyPort.TabIndex = 3;
      // 
      // _lblusername
      // 
      this._lblusername.AutoSize = true;
      this._lblusername.Location = new System.Drawing.Point(8, 94);
      this._lblusername.Name = "_lblusername";
      this._lblusername.Size = new System.Drawing.Size(63, 13);
      this._lblusername.TabIndex = 2;
      this._lblusername.Text = "User Name:";
      // 
      // _txtProxyUser
      // 
      this._txtProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtProxyUser.Location = new System.Drawing.Point(8, 113);
      this._txtProxyUser.Name = "_txtProxyUser";
      this._txtProxyUser.Size = new System.Drawing.Size(328, 20);
      this._txtProxyUser.TabIndex = 3;
      // 
      // _lblpassword
      // 
      this._lblpassword.AutoSize = true;
      this._lblpassword.Location = new System.Drawing.Point(8, 137);
      this._lblpassword.Name = "_lblpassword";
      this._lblpassword.Size = new System.Drawing.Size(56, 13);
      this._lblpassword.TabIndex = 2;
      this._lblpassword.Text = "Password:";
      // 
      // _txtProxyPass
      // 
      this._txtProxyPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtProxyPass.Location = new System.Drawing.Point(8, 156);
      this._txtProxyPass.Name = "_txtProxyPass";
      this._txtProxyPass.PasswordChar = '*';
      this._txtProxyPass.Size = new System.Drawing.Size(328, 20);
      this._txtProxyPass.TabIndex = 3;
      // 
      // HTTPSettingPanel
      // 
      this.Controls.Add(this._gboxMain);
      this.Name = "HTTPSettingPanel";
      this.Size = new System.Drawing.Size(344, 191);
      this._gboxMain.ResumeLayout(false);
      this._gboxMain.PerformLayout();
      this.ResumeLayout(false);

		}
		#endregion
		
		#region private void RefreshStatus()
		private void RefreshStatus(){
			bool check = this._chkUserProxy.Checked;
			this._lblpassword.Enabled = 
				this._lblServername.Enabled = 
				this._lblserverport.Enabled = 
				this._lblusername.Enabled = 
				this._txtProxyPass.Enabled = 
				this._txtProxyPort.Enabled =
				this._txtProxyServer.Enabled = 
				this._txtProxyUser.Enabled = check;
		}
		#endregion

		#region private void _chkUserProxy_CheckedChanged(object sender, System.EventArgs e)
		private void _chkUserProxy_CheckedChanged(object sender, System.EventArgs e) {
			if (this._chkUserProxy.Checked)
				this._chkGetFromIE.Checked = false;
			this.RefreshStatus();
		}
		#endregion

    #region public void SetProxy(ProxySetting proxy)
    public void SetProxy(ProxySetting proxy){
			this.UseProxy = proxy.ProxyEnable;
			this.ProxyServer = proxy.ProxyServer;
			this.ProxyPort = proxy.ProxyPort;
			this.ProxyUser = proxy.ProxyUserName;
			this.ProxyPass = proxy.ProxyPass;
			this.ProxyFromIE = proxy.ProxySettingFromIE;
    }
    #endregion

    #region public ProxySetting GetProxy()
    public ProxySetting GetProxy(){
			ProxySetting proxy = new ProxySetting();
			proxy.ProxyEnable = this.UseProxy;
			proxy.ProxyServer = this.ProxyServer ;
			proxy.ProxyPort = this.ProxyPort;
			proxy.ProxyUserName = this.ProxyUser;
			proxy.ProxyPass = this.ProxyPass;
			proxy.ProxySettingFromIE = this.ProxyFromIE;
			return proxy;
    }
    #endregion

    #region private void _chkGetFromIE_CheckedChanged(object sender, System.EventArgs e)
    private void _chkGetFromIE_CheckedChanged(object sender, System.EventArgs e) {
			if (this._chkGetFromIE.Checked)
				this._chkUserProxy.Checked = false;
			this.RefreshStatus();
    }
    #endregion

  }
}
