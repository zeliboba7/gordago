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
using System.Diagnostics;

namespace Gordago.WebUpdate {
  public partial class GetFullVersionForm:Form {

    public GetFullVersionForm() {
      InitializeComponent();
      if(GordagoMain.Lang == "rus") {
        this._rbtnRussian.Checked = true;
      }

      this.Text = GordagoMain.MessageCaption;

      this.RefreshLang();
    }

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {
      string link = "";
      if(_rbtnRussian.Checked) {
        link = "http://www.gordago.ru/?act=reg";
      } else {
        link = "http://www.gordago.com/?act=reg";
      }
      link += "&vers=" + Application.ProductVersion +
          "&cst=" + GordagoMain.MainForm.CountStartApp.ToString();

      this.Cursor = Cursors.WaitCursor;
      try {
        ProcessStartInfo psi = new ProcessStartInfo("iexplore", link);
        psi.WorkingDirectory = "C:\\";
        Process.Start(psi);
      } catch { }
      this.Cursor = Cursors.Default;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _rbtnEnglish_CheckedChanged(object sender, EventArgs e)
    private void _rbtnEnglish_CheckedChanged(object sender, EventArgs e) {
      if(_rbtnEnglish.Checked)
        _rbtnRussian.Checked = false;

      this.RefreshLang();
    }
    #endregion

    #region private void _rbtnRussian_CheckedChanged(object sender, EventArgs e)
    private void _rbtnRussian_CheckedChanged(object sender, EventArgs e) {
      if(_rbtnRussian.Checked)
        _rbtnEnglish.Checked = false;
      this.RefreshLang();
    }
    #endregion

    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion

    #region private void RefreshLang()
    private void RefreshLang() {
      if(_rbtnRussian.Checked) {
        _lblCaption.Text = "Получить полную версию";
        _lngChoiseLang.Text = "Выберите язык:";
        _btnOK.Text = "Дальше >";
        _btnCancel.Text = "Отмена";
      } else {
        _lblCaption.Text = "To get the full version";
        _lngChoiseLang.Text = "Choose Language:";
        _btnOK.Text = "Next >";
        _btnCancel.Text = "Cancel";
      }
    }
    #endregion
  }
}