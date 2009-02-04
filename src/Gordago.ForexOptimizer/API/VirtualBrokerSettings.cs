/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cursit.Applic.AConfig;
using Gordago.API.VirtualForex;
using Language;

namespace Gordago.API {
  partial class VirtualBrokerSettingsForm : Form {

    public VirtualBrokerSettingsForm() {
      InitializeComponent();
      this.Text = Dictionary.GetString(31, 7, "Настройки");
      this._btnCancel.Text = Dictionary.GetString(33, 11, "Cancel");

    }

    #region public VSSettingsPanel Settings
    public VSSettingsPanel Settings {
      get { return _settingsPanel; }
    }
    #endregion

    #region private void _btnOk_Click(object sender, EventArgs e)
    private void _btnOk_Click(object sender, EventArgs e) {
      _settingsPanel.Save();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion
  }
}