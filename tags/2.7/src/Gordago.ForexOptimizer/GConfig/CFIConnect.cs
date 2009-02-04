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

using Language;
using Cursit.Applic.AConfig;
#endregion

using Gordago.API;
using Gordago.WebUpdate;

namespace Gordago.GConfig {
	public class CFIConnect : Gordago.GConfig.ConfigFormItem {

		#region private property

		private System.ComponentModel.Container components = null;
		#endregion

		private Gordago.API.HTTPSettingPanel _httpProxySet;

		public CFIConnect() {
			InitializeComponent();
			this.Text = Dictionary.GetString(29,18,"Proxy server");
      ProxySetting proxy = GordagoMain.MainForm.LoadProxySetting();
			this._httpProxySet.SetProxy(proxy);
		}

		#region Component Designer generated code
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._httpProxySet = new Gordago.API.HTTPSettingPanel();
			this.SuspendLayout();
			// 
			// _httpProxySet
			// 
			this._httpProxySet.Location = new System.Drawing.Point(8, 0);
			this._httpProxySet.Name = "_httpProxySet";
			this._httpProxySet.ProxyFromIE = false;
			this._httpProxySet.ProxyPass = "";
			this._httpProxySet.ProxyPort = 0;
			this._httpProxySet.ProxyServer = "";
			this._httpProxySet.ProxyUser = "";
			this._httpProxySet.Size = new System.Drawing.Size(408, 224);
			this._httpProxySet.TabIndex = 1;
			this._httpProxySet.UseProxy = false;
			// 
			// CFIConnect
			// 
			this.Controls.Add(this._httpProxySet);
			this.Name = "CFIConnect";
			this.ResumeLayout(false);

		}
		#endregion

		#region public override void SaveConfig()
		public override void SaveConfig() {
      GordagoMain.MainForm.SaveProxySetting(_httpProxySet.GetProxy());
		}
		#endregion
	}
}
