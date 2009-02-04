using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gordago.PlugIn.MetaQuotesHistory {
  public partial class ConvertToTicksForm:Form {
    private MQHBarReader[] _mqhBarReaders;
    

    public ConvertToTicksForm() {
      InitializeComponent();

      string[] files = Directory.GetFiles(MQHPlugIn.MQHistoryDir, "*.hst");
      _lstMQHFiles.Items.Clear();
      List<MQHBarReader> list = new List<MQHBarReader>();
      for(int i=0;i<files.Length;i++){
        string file = files[i];
        try {
          MQHBarReader barReader = new MQHBarReader(file, null);
          list.Add(barReader);
          ListViewItem lvi = new ListViewItem(((int)(i+1)).ToString());
          lvi.SubItems.Add(GetFileName(file));
          lvi.SubItems.Add(barReader.SymbolName);
          lvi.SubItems.Add(GetTimeFrameName(barReader.TimeFrameSecond));
          lvi.SubItems.Add(barReader.TimeFrom.ToString());
          lvi.SubItems.Add(barReader.TimeTo.ToString());
          _lstMQHFiles.Items.Add(lvi);
        } catch { }
      }
      _mqhBarReaders = list.ToArray();
    }

    private void _btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

    public static string GetFileName(string path) {
      string[] sa = path.Split(new char[] { '\\' });
      return sa[sa.Length - 1];
    }

    public static string GetTimeFrameName(int second) {
      for(int i = 0; i < MQHPlugIn.MQH_TIMEFRAMES.Length; i++) {
        if(second == MQHPlugIn.MQH_TIMEFRAMES[i].Second)
          return MQHPlugIn.MQH_TIMEFRAMES[i].Name;
      }
      return "Er";
    }

    private void _lstMQHFiles_SelectedIndexChanged(object sender, EventArgs e) {
      if(this._lstMQHFiles.SelectedItems.Count != 1) {
        _btnConvert.Enabled = false;
        return;
      }
      _btnConvert.Enabled = true;

    }

    private void SetEnableStatus(bool enable) {
      this._btnClose.Enabled =
        this._lstMQHFiles.Enabled =
        this._btnConvert.Enabled = enable;
    }

    private void _btnConvert_Click(object sender, EventArgs e) {
      if(this._lstMQHFiles.SelectedItems.Count != 1) {
        _btnConvert.Enabled = false;
        return;
      }

      this.SetEnableStatus(false);

      ListViewItem lvi = _lstMQHFiles.SelectedItems[0];
      int index = Convert.ToInt32(lvi.Text) - 1;
      MQHBarReader barReader = _mqhBarReaders[index];

      string dir = Application.StartupPath + "\\history";
      MQHConverter converter = new MQHConverter(barReader, dir);
      converter.ProgressChanged += new EventHandler<MQHConverterEventArgs>(this.converter_ProgressChanged);
      converter.Start();
    }

    private void converter_ProgressChanged(object sender, MQHConverterEventArgs e) {
      if(!this.InvokeRequired) {
        if(e.Current == e.Total) {
          this._pgsConvert.Value = 0;
          this.SetEnableStatus(true);
          MessageBox.Show(MQHPlugIn.LanguageDictionary.ConvertComplete, "MetaQuotesHistory");
        } else {
          this._pgsConvert.Maximum = e.Total;
          this._pgsConvert.Value = e.Current;
        }
      } else {
        this.Invoke(new EventHandler<MQHConverterEventArgs>(this.converter_ProgressChanged), sender, e);
      }
    }
  }
}