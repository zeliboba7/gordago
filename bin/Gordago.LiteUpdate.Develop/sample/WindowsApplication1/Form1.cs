using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1 {
  public partial class Form1 : Form {

    private Gordago.LiteUpdate.UpdateManager _updateManager;
    
    public Form1() {
      InitializeComponent();

      _updateManager = new Gordago.LiteUpdate.UpdateManager();

      /* Подпись на события UpdateManager */
      /* Стоп проверки обновления */
      _updateManager.StopCheckForUpdates += new EventHandler(_updateManager_StopCheckForUpdates);
    }

    private void _updateManager_StopCheckForUpdates(object sender, EventArgs e) {
      if (this.InvokeRequired) {
        this.Invoke(new EventHandler(_updateManager_StopCheckForUpdates), sender, e);
        return;
      }
      this.button1.Enabled =
        this.button2.Enabled =
        this.button3.Enabled = true;
      this.Cursor = Cursors.Default;
      this.label1.Text = "";

      if (!_updateManager.IsOldVersion)
        return;

      Gordago.LiteUpdate.UpdateForm form = 
        new Gordago.LiteUpdate.UpdateForm(_updateManager);
      form.ShowDialog();
    }

    #region private void button3_Click(object sender, EventArgs e)
    private void button3_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion

    #region private void button1_Click(object sender, EventArgs e)
    private void button1_Click(object sender, EventArgs e) {
      this.button1.Enabled =
        this.button2.Enabled =
        this.button3.Enabled = false;
      this.Cursor = Cursors.WaitCursor;
      this.label1.Text = "Check For Updates";

      /* Запуск проверки обновления */
      this._updateManager.CheckForUpdates();
    }
    #endregion

    #region private void button2_Click(object sender, EventArgs e)
    private void button2_Click(object sender, EventArgs e) {
      ProxySettingsForm form = new ProxySettingsForm();
      form.ShowDialog();
    }
    #endregion
  }
}