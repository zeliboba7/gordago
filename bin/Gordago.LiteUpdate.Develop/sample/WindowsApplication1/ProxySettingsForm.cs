using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gordago.LiteUpdate;

namespace WindowsApplication1 {
  public partial class ProxySettingsForm : Form {
    public ProxySettingsForm() {
      InitializeComponent();
      this._chkEnableProxy.Enabled = false;
    }

    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);

      ProxySettings proxy = Gordago.LiteUpdate.Configure.Proxy;
      this._chkEnableProxy.Checked = proxy.Enable;
      this._txtServer.Text = proxy.Server;
      _txtPort.Text = proxy.Port.ToString();
      _txtUser.Text = proxy.UserName;
      _txtPassword.Text = proxy.UserPassword;

    }

    private void button1_Click(object sender, EventArgs e) {
      ProxySettings proxy = Gordago.LiteUpdate.Configure.Proxy;
      proxy.Enable = _chkEnableProxy.Checked;
      proxy.Server = _txtServer.Text;
      proxy.Port = Convert.ToInt32(_txtPort.Text);
      proxy.UserName = _txtUser.Text;
      proxy.UserPassword = _txtPassword.Text;
      this.Close();
    }

    private void button2_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void _chkEnableProxy_CheckedChanged(object sender, EventArgs e) {
      this.groupBox1.Enabled = this._chkEnableProxy.Checked;
    }
  }
}