using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestStrategy {
  public partial class StatisticsForm:Form {

    public StatisticsForm() {
      InitializeComponent();
    }

    public void AddRow(string name, string value) {
      ListViewItem lvi = new ListViewItem(name);
      lvi.SubItems.Add(value);
      _lstView.Items.Add(lvi);
    }
  }
}