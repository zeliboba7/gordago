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
using System.Windows.Forms;

using Language;
using Cursit.Applic.AConfig;
using Gordago.Analysis.Chart;
#endregion

namespace Gordago.GConfig {
	public class CFIGraph : Gordago.GConfig.ConfigFormItem {

		#region private property

    private Gordago.Analysis.Chart.ChartManager _chartmanager;
		private System.ComponentModel.IContainer components = null;

		private DateTime _dtm = DateTime.Now;
		private System.Windows.Forms.ComboBox _cmbstyles;
		private ISymbol _symbol;
    private PropertyGrid _property;
    private Button _btnEdit;
    private Button _btnSave;
    private Button _btnCancel;
		#endregion
    private ChartStyle _editCS;

		public CFIGraph() {
			InitializeComponent();
			this.Text = Dictionary.GetString(29,17,"Chart");

      _symbol = GordagoMain.SymbolEngine.GetSymbol("EURUSD");

      if(_symbol == null) {
        for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
          ISymbol symbol = GordagoMain.SymbolEngine[i];
          if (symbol.Ticks.Count > 100) {
            _symbol = symbol;
            break;
          }
        }
      }
      TimeFrame tf = TimeFrameManager.TimeFrames.GetTimeFrame(60);
			if (tf == null)
        tf = TimeFrameManager.TimeFrames[TimeFrameManager.TimeFrames.Count - 1];

			this._chartmanager.SetTraderComponent(GordagoMain.MainForm.ChartPanelManager, GordagoMain.MainForm.ChartObjectManager, _symbol, GordagoMain.IndicatorManager, tf);

			Font fontdesc = new Font(this._chartmanager.Style.ScaleFont.FontFamily, 8);

			this._chartmanager.ChartBoxes[0].Figures.Add(new ChartFigureBar("Bars"));
			ChartFigureText cftext = new ChartFigureText("Description", "EURUSD", fontdesc, this._chartmanager.Style.ScaleForeColor, 3, 3);
      this._chartmanager.ChartBoxes[0].Figures.Add(cftext);
      this.UpdateList();
		}

    private void UpdateList() {
      this._cmbstyles.Items.Clear();
      this._cmbstyles.Items.AddRange(GordagoMain.ChartStyleManager.GetStyles());
      this._cmbstyles.SelectedItem = GordagoMain.ChartStyleManager.GetDefaultStyle();
    }

		#region Designer generated code
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent() {
      this._chartmanager = new Gordago.Analysis.Chart.ChartManager();
      this._cmbstyles = new System.Windows.Forms.ComboBox();
      this._property = new System.Windows.Forms.PropertyGrid();
      this._btnEdit = new System.Windows.Forms.Button();
      this._btnSave = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _chartmanager
      // 
      this._chartmanager.AutoDeleteEmptyChartBox = true;
      this._chartmanager.Location = new System.Drawing.Point(0, 0);
      this._chartmanager.Name = "_chartmanager";
      this._chartmanager.Size = new System.Drawing.Size(426, 209);
      this._chartmanager.TabIndex = 0;
      // 
      // _cmbstyles
      // 
      this._cmbstyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbstyles.Location = new System.Drawing.Point(3, 213);
      this._cmbstyles.Name = "_cmbstyles";
      this._cmbstyles.Size = new System.Drawing.Size(192, 21);
      this._cmbstyles.TabIndex = 2;
      this._cmbstyles.SelectedIndexChanged += new System.EventHandler(this._cmbstyles_SelectedIndexChanged);
      // 
      // _property
      // 
      this._property.HelpVisible = false;
      this._property.Location = new System.Drawing.Point(0, 238);
      this._property.Name = "_property";
      this._property.Size = new System.Drawing.Size(426, 172);
      this._property.TabIndex = 3;
      this._property.ToolbarVisible = false;
      // 
      // _btnEdit
      // 
      this._btnEdit.Location = new System.Drawing.Point(201, 213);
      this._btnEdit.Name = "_btnEdit";
      this._btnEdit.Size = new System.Drawing.Size(64, 23);
      this._btnEdit.TabIndex = 4;
      this._btnEdit.Text = "Edit";
      this._btnEdit.UseVisualStyleBackColor = true;
      this._btnEdit.Click += new System.EventHandler(this._btnEdit_Click);
      // 
      // _btnSave
      // 
      this._btnSave.Enabled = false;
      this._btnSave.Location = new System.Drawing.Point(271, 213);
      this._btnSave.Name = "_btnSave";
      this._btnSave.Size = new System.Drawing.Size(68, 23);
      this._btnSave.TabIndex = 4;
      this._btnSave.Text = "Save";
      this._btnSave.UseVisualStyleBackColor = true;
      this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.Enabled = false;
      this._btnCancel.Location = new System.Drawing.Point(345, 213);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(68, 23);
      this._btnCancel.TabIndex = 4;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // CFIGraph
      // 
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnSave);
      this.Controls.Add(this._btnEdit);
      this.Controls.Add(this._property);
      this.Controls.Add(this._cmbstyles);
      this.Controls.Add(this._chartmanager);
      this.Name = "CFIGraph";
      this.ResumeLayout(false);

		}
		#endregion

    #region public override void SaveConfig()
    public override void SaveConfig() {
			base.SaveConfig ();
    }
    #endregion

    #region private void _cmbstyles_SelectedIndexChanged(object sender, System.EventArgs e)
    private void _cmbstyles_SelectedIndexChanged(object sender, System.EventArgs e) {
      this._property.SelectedObject = null;
			if (this._cmbstyles.SelectedItem == null)
				return;
			ChartStyle cs = this._cmbstyles.SelectedItem as ChartStyle;
			GordagoMain.ChartStyleManager.SetDefaultStyle(cs);
			this._chartmanager.SetStyle(cs);
      foreach(Form form in GordagoMain.MainForm.MdiChildren) {
				if (form is ChartForm){
					(form as ChartForm).RefreshStyle();
				}
			}
			this.Invalidate();
    }
    #endregion

    #region private void UpdateStyle()
    private void UpdateStyle() {
      ChartStyle cs = this._cmbstyles.SelectedItem as ChartStyle;
      GordagoMain.ChartStyleManager.SetDefaultStyle(cs);
      this._chartmanager.SetStyle(cs);
      foreach(Form form in GordagoMain.MainForm.MdiChildren) {
        if(form is ChartForm) {
          (form as ChartForm).RefreshStyle();
        }
      }
      this.Invalidate();
    }
    #endregion

    private void _btnEdit_Click(object sender, EventArgs e) {
      ChartStyle cs = this._cmbstyles.SelectedItem as ChartStyle;
      if(cs == null) return;
      _editCS = cs.Clone();
      _property.SelectedObject = _editCS;
      this.SetEditStatus(true);
      _editCS.StyleChangedEvent += new EventHandler(this.ChartStyle_StyleChanged);
    }

    #region private void SetEditStatus(bool isedit)
    private void SetEditStatus(bool isedit) {
      this._cmbstyles.Enabled =
        this._btnEdit.Enabled = 
        this.ConfigForm.ButtonOK.Enabled = 
        this.ConfigForm.ConfigList.Enabled = !isedit;

      this._btnSave.Enabled = this._btnCancel.Enabled = isedit;
      if (!isedit)
        _property.SelectedObject = null;
    }
    #endregion

    private void _btnSave_Click(object sender, EventArgs e) {
      GordagoMain.ChartStyleManager.Delete(_editCS.Name);
      GordagoMain.ChartStyleManager.Save(_editCS);
      GordagoMain.ChartStyleManager.Add(_editCS);
      GordagoMain.ChartStyleManager.SetDefaultStyle(_editCS);

      this.SetEditStatus(false);
      _editCS.StyleChangedEvent -= new EventHandler(this.ChartStyle_StyleChanged);
      UpdateList();
    }
    
    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      _editCS.StyleChangedEvent -= new EventHandler(this.ChartStyle_StyleChanged);
      this.SetEditStatus(false);
    }
    #endregion

    #region private void ChartStyle_StyleChanged(object sender, EventArgs e)
    private void ChartStyle_StyleChanged(object sender, EventArgs e) {
      this._chartmanager.SetStyle(_editCS);
    }
    #endregion
  }
}


