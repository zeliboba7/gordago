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
using Language;

namespace Gordago.API {
  public partial class HOCPDialog : Form {
    
    public HOCPDialog(string text) {
      InitializeComponent();
      _lblText.Text = text;
      try {
        this._btnYes.Text = Dictionary.GetString(33, 10, "OK");
        this._btnNo.Text = Dictionary.GetString(33, 11, "Cancel");
      } catch { }
    }

    #region private void _btnYes_Click(object sender, EventArgs e)
    private void _btnYes_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _btnNo_Click(object sender, EventArgs e)
    private void _btnNo_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion
  }
}