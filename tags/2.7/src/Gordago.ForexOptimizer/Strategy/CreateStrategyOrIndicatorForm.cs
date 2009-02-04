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

namespace Gordago.Strategy {
  public partial class CreateStrategyOrIndicatorForm : Form {
    public CreateStrategyOrIndicatorForm() {
      InitializeComponent();
      this._lstType.SelectedIndex = 0;
    }

    public int SelectedIndex {
      get { return this._lstType.SelectedIndex; }
    }

    private void _btnOK_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

  }
}