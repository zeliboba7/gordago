#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cursit.Applic.AConfig;
using Language;
#endregion

namespace Gordago {
  public partial class ReportBrowserForm : Form {

    private string _filename;
    private string _savePrefix;

    public ReportBrowserForm(string tempFileName, string savePrefix) {
      _filename = tempFileName;
      _savePrefix = savePrefix;

      InitializeComponent();
      this._webBrowser.Navigate(tempFileName);

      try {
        this.Text = Dictionary.GetString(34, 1, "Отчет");
        this._btnSaveAs.Text = Dictionary.GetString(34, 2, "Сохранить как...");
        this._btnClose.Text = Dictionary.GetString(34, 3, "Закрыть");
      } catch { }
    }

    #region private void _btnClose_Click(object sender, EventArgs e)
    private void _btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion

    #region private void _btnSaveAs_Click(object sender, EventArgs e)
    private void _btnSaveAs_Click(object sender, EventArgs e) {
      string reportpath = Application.StartupPath + "\\reports";
      Cursit.Utils.FileEngine.CheckDir(reportpath + "\\txt.txt");

      string filename = _savePrefix + "_" +
        DateTime.Now.Year.ToString()+"_" +
        DateTime.Now.Month.ToString() + "_" +
        DateTime.Now.Day.ToString() + "_" +
        DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" +
        DateTime.Now.Second.ToString() + ".html";

      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "Web pages (*.html)|*.html";

      string path = Config.Users["PathReport", reportpath];
      sfd.InitialDirectory = path;
      sfd.FileName = path + "\\" + filename;
      if (sfd.ShowDialog() != DialogResult.OK)
        return;

      Config.Users["PathReport"].SetValue(Cursit.Utils.FileEngine.GetDirectory(sfd.FileName));

      try {
        if (System.IO.File.Exists(sfd.FileName))
          System.IO.File.Delete(sfd.FileName);

        System.IO.File.Copy(_filename, sfd.FileName);
      } catch { }
    }
    #endregion

    #region protected override void OnClosed(EventArgs e)
    protected override void OnClosed(EventArgs e) {
      base.OnClosed(e);
    }
    #endregion
  }
}