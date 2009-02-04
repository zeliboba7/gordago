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
using Cursit.Utils;
using Cursit.Applic.AConfig;
using System.Collections;
using System.IO;
using Gordago.Strategy.FIndicator;

namespace Gordago.Strategy {
  partial class CustomIndicatorForm : Form, IIndicatorBoxCollection {


    public static readonly string EXT_CUSTOM_INDICATOR = "gci";
    private string _fileName;

    private bool _isSave = false;

    public CustomIndicatorForm() {
      InitializeComponent();
      this.FileName = this.GetDefaultFileName();
      this._tbcMain.TabPages.Clear();
      this.CreateFunction();
    }

    #region public string FileName
    public string FileName {
      get { return this._fileName; }
      set { 
        this._fileName = value;
        this._txtName.Text = this.IndicatorName;
        this.IsSave = this.IsSave;
      }
    }
    #endregion

    #region public string IndicatorName
    public string IndicatorName {
      get {
        string iname = FileEngine.GetFileNameFromPath(_fileName);
        iname = iname.Substring(0, iname.Length - 4);
        return iname;
      }
    }
    #endregion

    #region public bool IsSave
    public bool IsSave {
      get { return this._isSave; }
      set {
        _isSave = value;
        this.Text = FileEngine.GetFileNameFromPath(_fileName);
        if (!_isSave)
          this.Text += "*";
      }
    }
    #endregion

    #region public void CreateFunction()
    public void CreateFunction() {

      TabPage tabPage = new TabPage();

      _tbcMain.SuspendLayout();
      tabPage.SuspendLayout();

      int cnt = _tbcMain.TabPages.Count + 1;

      CustomFuntionControl ctrl = new CustomFuntionControl(this);
      ctrl.FunctionName = CustomFuntionControl.DEF_FUNC_NAME + cnt.ToString();
      ctrl.Bilder.SetCurstomIndicatorForm(this);

      ctrl.Dock = DockStyle.Fill;
      tabPage.Controls.Add(ctrl);
      tabPage.Text = ctrl.FunctionName;

      _tbcMain.TabPages.Add(tabPage);

      tabPage.ResumeLayout(false);
      _tbcMain.ResumeLayout(false);
      this.IsSave = false;
      this.UpdateFunctionNames();
    }
    #endregion

    #region public void RemoveFunction(int index)
    public void RemoveFunction(int index) {
      if (index >= this._tbcMain.TabPages.Count || index < 0)
        return;

      this._tbcMain.TabPages.RemoveAt(index);
      this.IsSave = false;
    }
    #endregion

    #region public int Count
    public int Count {
      get { return this._tbcMain.TabPages.Count; }
    }
    #endregion

    #region public CustomFuntionControl this[int index]
    public CustomFuntionControl this[int index]{
      get {
        TabPage page = this._tbcMain.TabPages[index];
        return page.Controls[0] as CustomFuntionControl; 
      }
    }
    #endregion

    #region public string GetDefaultFileName()
    public string GetDefaultFileName() {
      string defname = "CustomIndicator";
      string defpath = Application.StartupPath + "\\indicators";

      FileEngine.CheckDir(defpath + "\\txt.txt");

      string[] files = Directory.GetFiles(defpath, "*." + EXT_CUSTOM_INDICATOR);

      /* так же необxодимо просмотреть все открытые окна */
      ArrayList al = new ArrayList(files);
      foreach (Form frm in GordagoMain.MainForm.MdiChildren) {
        if (frm is CustomIndicatorForm) {
          CustomIndicatorForm cifm = frm as CustomIndicatorForm;
          if (cifm != this)
            al.Add(cifm.FileName);
        }
      }
      files = new string[al.Count];
      al.CopyTo(files);
      
      string fname = "";
      for (int i = 1; i < 10000; i++) {
        string retfn = defname + i.ToString() + "." + EXT_CUSTOM_INDICATOR;
        string cncfn = defpath + "\\" + retfn;
        bool isfind = false;
        foreach (string filename in files) {
          if (filename == cncfn) {
            isfind = true;
          }
        }
        if (!isfind) {
          fname = cncfn;
          break;
        }
      }
      return fname;
    }
    #endregion

    #region private void _btnClose_Click(object sender, EventArgs e)
    private void _btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion

    #region private void _btnAdd_Click(object sender, EventArgs e)
    private void _btnAdd_Click(object sender, EventArgs e) {
      this.CreateFunction();
    }
    #endregion

    #region private void _btnDel_Click(object sender, EventArgs e)
    private void _btnDel_Click(object sender, EventArgs e) {
      if (this._tbcMain.TabPages.Count <= 1) 
        return;
      this.RemoveFunction(this._tbcMain.SelectedIndex);
    }
    #endregion

    #region private void _txtName_TextChanged(object sender, EventArgs e)
    private void _txtName_TextChanged(object sender, EventArgs e) {
      this.UpdateFunctionNames();
    }
    #endregion

    #region private void UpdateFunctionNames()
    private void UpdateFunctionNames() {
      for (int i = 0; i < this.Count; i++) {
        CustomFuntionControl ctrl = this[i];
        ctrl.UpdateName();
      }
    }
    #endregion

    #region public IndicFunctionBox[] GetIndicFunctionBoxCollection()
    public IndicFunctionBox[] GetIndicFunctionBoxCollection() {
      List<IndicFunctionBox> list = new List<IndicFunctionBox>();
      for (int i = 0; i < this.Count; i++) {
        TextBoxObjElement[] els = this[i].Bilder.TextBoxObject.Elements;
        for (int e = 0; e < els.Length; e++) {
          if (els[e] is TextBoxObjElementCtrl) {
            TextBoxObjElementCtrl ctrl = els[e] as TextBoxObjElementCtrl;
            IndicFunctionBox indfbox = ctrl.Element as IndicFunctionBox;
            if (indfbox != null)
              list.Add(indfbox);
          }
        }
      }
      return list.ToArray();
    }
    #endregion

    #region public void SelectIndicFunctionBox(IndicFunctionBox indicFuncBox)
    public void SelectIndicFunctionBox(IndicFunctionBox indicFuncBox) {
      IndicFunctionBox[] list = this.GetIndicFunctionBoxCollection();
      for (int i = 0; i < list.Length; i++) {
        if (list[i] == indicFuncBox)
          list[i].IsSelect = true;
        else
          list[i].IsSelect = false;
      }
    }
    #endregion
  }
}