using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyPlugIn {
  public partial class MyPlugInSettingsForm:Form {
    public MyPlugInSettingsForm() {
      InitializeComponent();
    }

    private void _btnOK_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void _btnCancel_Click(object sender, EventArgs e) {
      this.Close();
    }
  }
}