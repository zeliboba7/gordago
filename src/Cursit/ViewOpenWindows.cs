/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Cursit {
  public partial class ViewOpenWindows : UserControl {
    
    public ViewOpenWindows() {
      InitializeComponent();
      this._tbcMain.TabPages.Clear();
      _tbcMain.SelectedIndexChanged += new EventHandler(this._tbcMain_SelectedIndexChanged);
      this.CheckVisible();
    }

    #region public TabControl TabControl
    public TabControl TabControl {
      get { return this._tbcMain; }
    }
    #endregion

    #region private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e)
    private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e) {
      if (this._tbcMain.SelectedTab == null)
        return;

      VOTabPage tpb = this._tbcMain.SelectedTab as VOTabPage;
      tpb.Form.Activate();
      //GordagoMain.MainForm.ActiveMdiChild
      
    }
    #endregion

    #region public void ChildFormAdd(Form form)
    public void ChildFormAdd(Form form) {
      VOTabPage votb = new VOTabPage(form);
      
      this._tbcMain.TabPages.Add(votb);
      this._tbcMain.SelectedTab = votb;
      this.CheckVisible();
    }
    #endregion

    #region public void ChildFormRemove(Form form)
    public void ChildFormRemove(Form form) {
      for (int i = 0; i < this._tbcMain.TabPages.Count; i++) {
        VOTabPage tbp = this._tbcMain.TabPages[i] as VOTabPage;
        if (tbp.Form == form) {
          _tbcMain.TabPages.Remove(tbp);
          break;
        }
      }
      this.CheckVisible();
    }
    #endregion

    #region private VOTabPage GetTabPage(Form form)
    private VOTabPage GetTabPage(Form form) {
      for (int i = 0; i < this._tbcMain.TabPages.Count; i++) {
        VOTabPage tbp = this._tbcMain.TabPages[i] as VOTabPage;
        if (tbp.Form == form) {
          return tbp;
        }
      }
      return null;
    }
    #endregion

    #region public void ChildFormActivate(Form form)
    public void ChildFormActivate(Form form) {
      VOTabPage tbp = this.GetTabPage(form);
      if (tbp == null)
        return;
      this._tbcMain.SelectedTab = tbp;
    }
    #endregion

    #region private void CheckVisible()
    private void CheckVisible() {
      this.Visible = _tbcMain.TabCount>0;
    }
    #endregion
  }

  #region class VOTabPage : TabPage 
  class VOTabPage : TabPage {
    
    private Form _form;

    public VOTabPage(Form form) {
      _form = form;
      this.Text = form.Text;
      form.TextChanged += new EventHandler(this.Form_TextChanged);
    }

    #region public Form Form
    public Form Form {
      get { return this._form; }
    }
    #endregion

    #region private void Form_TextChanged(object sender, EventArgs e)
    private void Form_TextChanged(object sender, EventArgs e) {
      if (!this.InvokeRequired) {
        this.Text = _form.Text;
      } else {
        this.Invoke(new EventHandler(this.Form_TextChanged), sender, e);
      }
    }
    #endregion
  }
  #endregion
}
