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

namespace Gordago {
  public partial class ProgramModeForm : Form {
    public ProgramModeForm() {
      InitializeComponent();
    }

    private void _btnVServer_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void _btnStandart_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

  }
}