using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gordago.PlugIn.ImpExpHistory {
  partial class ImportForm : Form {
    public ImportForm() {
      InitializeComponent();
    }

    private void _btnFile_Click(object sender, EventArgs e) {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Multiselect = false;
      if (ofd.ShowDialog() != DialogResult.OK)
        return;
      _txtFile.Text = ofd.FileName;
    }

    private void _txtFile_TextChanged(object sender, EventArgs e) {
      
    }
  }
}