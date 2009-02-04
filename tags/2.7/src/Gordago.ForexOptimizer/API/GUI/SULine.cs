/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Gordago.Analysis;
using Gordago.Strategy.FIndicator;
using Cursit.Applic.AConfig;
using Gordago.API;
using System.Threading;
#endregion

namespace Gordago.Strategy {

  delegate void SULineHandler(SULine suline);

  class SULine : UserControl, IBrokerEvents {

    #region private enum Status
    private enum Status {
      Stopping,
      Starting
    }
    #endregion

    #region private property
    public event SULineHandler ParametersChanged;

    private System.Windows.Forms.Label _lblnum;
    private System.Windows.Forms.ComboBox _cmbsymbol;
    private System.Windows.Forms.ComboBox _cmbstrategy;
    private System.Windows.Forms.Button _btnStart;
    private System.ComponentModel.Container components = null;

    private int _numline;

    private Status _status;
    private ISymbol _symbol;
    #endregion

    private string _lngstart, _lngstop, _lngdownstart, _lngdown;
    private SULines _sulines;

    private int _strategyId;

    private CompileDllData _cmpldata;
    
    public SULine() {
      InitializeComponent();
      this._btnStart.Enabled = false;

      try {
        _lngstart = Language.Dictionary.GetString(28, 15, "Start");
        _lngstop = Language.Dictionary.GetString(28, 16, "Stop");
        _lngdown = Language.Dictionary.GetString(28, 17, "Downloading history...");
        _lngdownstart = Language.Dictionary.GetString(28, 18, "Start history downloading...");
      } catch { }
      this.SetStatus(Status.Stopping);
    }

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public int NumberLine
    public int NumberLine {
      get { return this._numline; }
      set {
        this._numline = value;
        this._lblnum.Text = value.ToString();
      }
    }
    #endregion

    #region public string FileName
    public string FileName {
      get {
        if (this._cmbstrategy.SelectedItem == null)
          return "";
        return (this._cmbstrategy.SelectedItem as SUStrategyFileList).FileName;
      }
    }
    #endregion

    #region public SUStrategyFileList SUStrategyFileList
    public SUStrategyFileList SUStrategyFileList {
      get {
        if (this._cmbstrategy.SelectedItem == null)
          return null;
        return this._cmbstrategy.SelectedItem as SUStrategyFileList;
      }
      set {
        _blockchangeevent = true;
        this._cmbstrategy.SelectedItem = value;
        _blockchangeevent = false;
        UpdateStartButton();
      }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get {
        if (this._cmbsymbol.SelectedItem == null)
          return "";
        return (string)(this._cmbsymbol.SelectedItem);
      }
      set {
        _blockchangeevent = true;
        if (value == "")
          this._cmbsymbol.SelectedItem = null;
        else
          this._cmbsymbol.SelectedItem = value;
        _blockchangeevent = false;
        UpdateStartButton();
      }
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose(bool disposing) {
      if (disposing) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._lblnum = new System.Windows.Forms.Label();
      this._cmbsymbol = new System.Windows.Forms.ComboBox();
      this._cmbstrategy = new System.Windows.Forms.ComboBox();
      this._btnStart = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _lblnum
      // 
      this._lblnum.AutoSize = true;
      this._lblnum.Location = new System.Drawing.Point(-16, 5);
      this._lblnum.Name = "_lblnum";
      this._lblnum.Size = new System.Drawing.Size(13, 13);
      this._lblnum.TabIndex = 0;
      this._lblnum.Text = "1";
      // 
      // _cmbsymbol
      // 
      this._cmbsymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbsymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbsymbol.Location = new System.Drawing.Point(305, 2);
      this._cmbsymbol.Name = "_cmbsymbol";
      this._cmbsymbol.Size = new System.Drawing.Size(72, 21);
      this._cmbsymbol.TabIndex = 1;
      this._cmbsymbol.SelectedIndexChanged += new System.EventHandler(this._cmbsymbol_SelectedIndexChanged);
      // 
      // _cmbstrategy
      // 
      this._cmbstrategy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbstrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbstrategy.Location = new System.Drawing.Point(3, 2);
      this._cmbstrategy.Name = "_cmbstrategy";
      this._cmbstrategy.Size = new System.Drawing.Size(299, 21);
      this._cmbstrategy.TabIndex = 1;
      this._cmbstrategy.SelectedIndexChanged += new System.EventHandler(this._cmbstrategy_SelectedIndexChanged);
      // 
      // _btnStart
      // 
      this._btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnStart.Location = new System.Drawing.Point(383, 2);
      this._btnStart.Margin = new System.Windows.Forms.Padding(0);
      this._btnStart.Name = "_btnStart";
      this._btnStart.Size = new System.Drawing.Size(72, 20);
      this._btnStart.TabIndex = 2;
      this._btnStart.Text = "Start";
      this._btnStart.UseVisualStyleBackColor = false;
      this._btnStart.Click += new System.EventHandler(this._btnStart_Click);
      // 
      // SULine
      // 
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this._cmbstrategy);
      this.Controls.Add(this._lblnum);
      this.Controls.Add(this._cmbsymbol);
      this.Controls.Add(this._btnStart);
      this.Name = "SULine";
      this.Size = new System.Drawing.Size(455, 24);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    #region protected void OnParametersChanged()
    protected void OnParametersChanged() {
      if (this.ParametersChanged == null)
        return;

      if (!this.UpdateStartButton())
        return;

      this.ParametersChanged(this);
    }
    #endregion

    #region private bool UpdateStartButton()
    private bool UpdateStartButton() {
      bool flag;
      if (this.FileName == "" || this.SymbolName == "") {
        flag = false;
      } else {
        flag = true;
      }
      this._btnStart.Enabled = flag;
      return flag;
    }
    #endregion

    #region public void SetInitComponenet(SULines sulines)
    public void SetInitComponenet(SULines sulines) {
      _sulines = sulines;
    }
    #endregion

    #region public void SynchronizedSymbolList()
    /// <summary>
    /// Занесение в списки символы
    /// </summary>
    public void SynchronizedSymbolList() {
      string sname = this.SymbolName;
      this._cmbsymbol.Items.Clear();
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        this._cmbsymbol.Items.Add(symbol.Name);
      }
      if (sname != "")
        this.SymbolName = sname;
    }
    #endregion

    #region public void UpdateSUStrategyFileList(SUStrategyFileList[] sufiles)

    bool _blockchangeevent;

    public void UpdateSUStrategyFileList(SUStrategyFileList[] sufiles) {
      _blockchangeevent = true;

      SUStrategyFileList sfs = null;
      if (this._cmbstrategy.SelectedItem != null)
        sfs = (SUStrategyFileList)this._cmbstrategy.SelectedItem;

      this._cmbstrategy.Items.Clear();
      bool issel = false;
      foreach (SUStrategyFileList sf in sufiles) {
        this._cmbstrategy.Items.Add(sf);
        if (sfs != null) {
          if (sf == sfs)
            issel = true;
        }
      }
      if (issel)
        this._cmbstrategy.SelectedItem = sfs;
      else
        this._cmbsymbol.SelectedItem = null;
      _blockchangeevent = false;
    }
    #endregion

    #region private void _cmbstrategy_SelectedIndexChanged(object sender, System.EventArgs e)
    private void _cmbstrategy_SelectedIndexChanged(object sender, System.EventArgs e) {
      if (_blockchangeevent)
        return;
      this.OnParametersChanged();
    }
    #endregion

    #region private void _cmbsymbol_SelectedIndexChanged(object sender, System.EventArgs e)
    private void _cmbsymbol_SelectedIndexChanged(object sender, System.EventArgs e) {
      if (_blockchangeevent)
        return;
      this.OnParametersChanged();
    }
    #endregion

    #region private void SetStatus(Status status)
    private void SetStatus(Status status) {
      _status = status;

      this._cmbsymbol.Enabled =
         this._cmbstrategy.Enabled =
         status == Status.Stopping;

      this._btnStart.Text = status == Status.Stopping ? _lngstart : _lngstop;
    }
    #endregion

    #region private void _btnStart_Click(object sender, System.EventArgs e)
    private void _btnStart_Click(object sender, System.EventArgs e) {
      if (_status == Status.Stopping) {
        this.Start();
      } else {
        this.Stop();
      }
    }
    #endregion

    #region private IStrategyForm GetOpenStrategyForm()
    private IStrategyForm GetOpenStrategyForm() {
      if (!(GordagoMain.MainForm.ActiveMdiChild is IStrategyForm))
        return null;

      return GordagoMain.MainForm.ActiveMdiChild as IStrategyForm;
    }
    #endregion

    #region private bool Start()
    private bool Start() {
      string filename = this.FileName;
      string sname = this.SymbolName;
      if (filename == "" || sname == "" || !System.IO.File.Exists(filename)) return false;

      EditorForm wfm = (EditorForm)GordagoMain.MainForm.StrategyManager.LoadStrategyForm(filename);

      if (wfm == null)
        return false;

      _cmpldata = wfm.Compile("");
      wfm.Close();

      if (_cmpldata == null) {
        MessageBox.Show("Error in strategy", GordagoMain.MessageCaption);
        return false;
      }

      _symbol = GordagoMain.SymbolEngine.GetSymbol(sname);
      if (_symbol == null)
        return false;

      (this._cmpldata.Strategy as VisualStrategy).Compile(_cmpldata.Variables, _symbol);

      if (BCM.GetSymbolUpdateStatus(_symbol) == SymbolUpdateStatus.Hole) {
        BCM.UpdateSymbolHole(_symbol);
      }
      
      Thread.Sleep(10);

      if (!this._sulines.Start(_cmpldata, _symbol)) {
        MessageBox.Show("Strategy can not be loaded", GordagoMain.MessageCaption);
        return false;
      }

      this.SetStatus(Status.Starting);

      return true;
    }
    #endregion

    #region private void Stop()
    private void Stop() {
      this._sulines.Stop(_cmpldata);
      this.SetStatus(Status.Stopping);
    }
    #endregion

    #region IBrokerEvents Members

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      this.SynchronizedSymbolList();
    }

    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerCommandStarting(BrokerCommand command) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
