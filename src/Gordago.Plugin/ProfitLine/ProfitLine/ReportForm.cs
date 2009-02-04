using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gordago.Analysis {
  public partial class ReportForm:Form {
    
    public ReportForm() {
      InitializeComponent();
    }

    public void AddRow(string name, string value) {
      ListViewItem lvi = new ListViewItem(name);
      lvi.SubItems.Add(value);
      this._lstReport.Items.Add(lvi);
    }

    public void AddRow(string name, object value) {
      this.AddRow(name, value.ToString());
    }

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion
  }
}