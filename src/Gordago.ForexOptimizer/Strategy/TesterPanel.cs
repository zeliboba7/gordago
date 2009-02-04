/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region Using
using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Threading;

using Gordago.Stock;
using Gordago.Strategy.FIndicator;
using Gordago.Strategy.IO;
using Gordago.Analysis;

using Cursit.Utils;
using Cursit.Applic.AConfig;
using Language;

using Cursit.Table;
using Gordago.Analysis.Chart;
using Gordago.API;
using Gordago.API.VirtualForex;
using System.Collections.Generic;
#endregion

namespace Gordago.Strategy {

	class TesterPanel : UserControl {

    #region class StrategyListItem
    class StrategyListItem {

      private string _name;
      private IStrategyForm _form;
      private TestReport _report;

      public StrategyListItem(string name, IStrategyForm form) {
        _name = name;
        _form = form;
      }

      #region public string Name
      public string Name {
        get { return this._name; }
      }
      #endregion

      #region public IStrategyForm Form
      public IStrategyForm Form {
        get { return this._form; }
      }
      #endregion

      #region public TestReport Report
      public TestReport Report {
        get { return this._report; }
        set { this._report = value; }
      }
      #endregion

      #region public override string ToString()
      public override string ToString() {
        return _name;
      }
      #endregion
    }
    #endregion

    delegate void UpdateSymbolListHandler();

    private System.ComponentModel.Container components = null;
		private Label _lblBegin, _lblEnd, _lblHistory;
    private Label _lblTotalProccess;
    private DateTimePicker _dtpBegin, _dtpEnd;
    private ProgressBar _pgsMain;
    private ProgressBar _pgsChild;
		private Button _btnStart;
    private Button _btnStop;
    private Button _btnReport;
    private Button _btnChart;
    private Button _btnNewStrategy;

    private Optimizer _optimizer;

    private StrategyListItem _currentTestedStrategy;
		private long _tbegin, _tend;

    private ISymbol _symbol;

    private ConfigValue _cfgval;
    private TableLayoutPanel _tbllayoutSSButton;
    private Panel _pnlBtnReport;
    private TableLayoutPanel _tbllayoutReports;
    private Panel _pnlProgress;
    private Panel _pnlProperty;
    private Label _lblSymbol;
    private TableLayoutPanel _tbllayoutDTM;
    private TableLayoutPanel tableLayoutPanel3;
    private ComboBox _cmbSymbols;
    private Button _btnSettings;
    private bool _isStart;
    private ComboBox _cmbStrategy;
    private Label label1;
    private VSSettingsPanel _settings;

		public TesterPanel() {
			InitializeComponent();
      
      try {
        _cfgval = Config.Users["Tester"];

        this.SetStyle(ControlStyles.DoubleBuffer, true);
        this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

        this._btnStart.Image = GordagoImages.Images.GetImage("start", "tester");

        this._btnStart.ImageAlign = ContentAlignment.MiddleLeft;
        this._btnStart.TextAlign = ContentAlignment.MiddleRight;

        this._btnStop.Image = GordagoImages.Images.GetImage("stop", "tester");
        this._btnStop.ImageAlign = ContentAlignment.MiddleLeft;
        this._btnStop.TextAlign = ContentAlignment.MiddleRight;

        _lblSymbol.Text = Dictionary.GetString(7, 2);
        _lblBegin.Text = Dictionary.GetString(7, 3);
        _lblEnd.Text = Dictionary.GetString(7, 4);

        _btnStop.Text = Dictionary.GetString(7, 8);
        _btnStart.Text = Dictionary.GetString(7, 7);
        _lblHistory.Text = Dictionary.GetString(7, 10);
        _lblTotalProccess.Text = Dictionary.GetString(7, 11);
        _btnReport.Text = Dictionary.GetString(7, 13);
        _btnChart.Text = Dictionary.GetString(7, 14);
        _btnNewStrategy.Text = Dictionary.GetString(7, 15);
        this._btnSettings.Text = Dictionary.GetString(31, 7, "Настройки");
        
        this.SetSymbol(null);
      } catch { }
      _settings = new VSSettingsPanel();
    }

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this._pgsMain.Width = this._pnlProgress.Width - this._pgsMain.Location.X - 5;
      this._pgsChild.Width = this._pnlProgress.Width - this._pgsChild.Location.X - 5;
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
      this.SaveConfigValue();
      base.Dispose(disposing);
		}
		#endregion

		#region private void InitializeComponent()
		private void InitializeComponent() {
      this._btnStart = new System.Windows.Forms.Button();
      this._btnStop = new System.Windows.Forms.Button();
      this._lblHistory = new System.Windows.Forms.Label();
      this._pgsMain = new System.Windows.Forms.ProgressBar();
      this._pgsChild = new System.Windows.Forms.ProgressBar();
      this._lblTotalProccess = new System.Windows.Forms.Label();
      this._cmbSymbols = new System.Windows.Forms.ComboBox();
      this._dtpBegin = new System.Windows.Forms.DateTimePicker();
      this._lblBegin = new System.Windows.Forms.Label();
      this._lblEnd = new System.Windows.Forms.Label();
      this._dtpEnd = new System.Windows.Forms.DateTimePicker();
      this._btnReport = new System.Windows.Forms.Button();
      this._btnChart = new System.Windows.Forms.Button();
      this._btnNewStrategy = new System.Windows.Forms.Button();
      this._tbllayoutReports = new System.Windows.Forms.TableLayoutPanel();
      this._tbllayoutSSButton = new System.Windows.Forms.TableLayoutPanel();
      this._pnlBtnReport = new System.Windows.Forms.Panel();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this._pnlProgress = new System.Windows.Forms.Panel();
      this._pnlProperty = new System.Windows.Forms.Panel();
      this._btnSettings = new System.Windows.Forms.Button();
      this._tbllayoutDTM = new System.Windows.Forms.TableLayoutPanel();
      this._cmbStrategy = new System.Windows.Forms.ComboBox();
      this._lblSymbol = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this._tbllayoutReports.SuspendLayout();
      this._tbllayoutSSButton.SuspendLayout();
      this._pnlBtnReport.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this._pnlProgress.SuspendLayout();
      this._pnlProperty.SuspendLayout();
      this._tbllayoutDTM.SuspendLayout();
      this.SuspendLayout();
      // 
      // _btnStart
      // 
      this._btnStart.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnStart.Location = new System.Drawing.Point(3, 3);
      this._btnStart.Name = "_btnStart";
      this._btnStart.Size = new System.Drawing.Size(63, 27);
      this._btnStart.TabIndex = 1;
      this._btnStart.Text = "Старт";
      this._btnStart.UseVisualStyleBackColor = false;
      this._btnStart.Click += new System.EventHandler(this._btnStart_Click);
      // 
      // _btnStop
      // 
      this._btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnStop.Enabled = false;
      this._btnStop.Location = new System.Drawing.Point(72, 3);
      this._btnStop.Name = "_btnStop";
      this._btnStop.Size = new System.Drawing.Size(64, 27);
      this._btnStop.TabIndex = 1;
      this._btnStop.Text = "Стоп";
      this._btnStop.UseVisualStyleBackColor = false;
      this._btnStop.Click += new System.EventHandler(this._btnStop_Click);
      // 
      // _lblHistory
      // 
      this._lblHistory.AutoSize = true;
      this._lblHistory.Location = new System.Drawing.Point(2, -1);
      this._lblHistory.Name = "_lblHistory";
      this._lblHistory.Size = new System.Drawing.Size(50, 13);
      this._lblHistory.TabIndex = 1;
      this._lblHistory.Text = "История";
      // 
      // _pgsMain
      // 
      this._pgsMain.Location = new System.Drawing.Point(98, 2);
      this._pgsMain.Name = "_pgsMain";
      this._pgsMain.Size = new System.Drawing.Size(178, 10);
      this._pgsMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this._pgsMain.TabIndex = 0;
      // 
      // _pgsChild
      // 
      this._pgsChild.Location = new System.Drawing.Point(98, 18);
      this._pgsChild.Name = "_pgsChild";
      this._pgsChild.Size = new System.Drawing.Size(178, 10);
      this._pgsChild.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this._pgsChild.TabIndex = 0;
      // 
      // _lblTotalProccess
      // 
      this._lblTotalProccess.AutoSize = true;
      this._lblTotalProccess.Location = new System.Drawing.Point(2, 15);
      this._lblTotalProccess.Name = "_lblTotalProccess";
      this._lblTotalProccess.Size = new System.Drawing.Size(70, 13);
      this._lblTotalProccess.TabIndex = 1;
      this._lblTotalProccess.Text = "Комбинации";
      // 
      // _cmbSymbols
      // 
      this._cmbSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
      this._cmbSymbols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbSymbols.Location = new System.Drawing.Point(153, 17);
      this._cmbSymbols.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
      this._cmbSymbols.Name = "_cmbSymbols";
      this._cmbSymbols.Size = new System.Drawing.Size(138, 21);
      this._cmbSymbols.TabIndex = 2;
      this._cmbSymbols.SelectedIndexChanged += new System.EventHandler(this._cmbSymbols_SelectedIndexChanged);
      // 
      // _dtpBegin
      // 
      this._dtpBegin.Checked = false;
      this._dtpBegin.Dock = System.Windows.Forms.DockStyle.Fill;
      this._dtpBegin.Location = new System.Drawing.Point(297, 17);
      this._dtpBegin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
      this._dtpBegin.Name = "_dtpBegin";
      this._dtpBegin.ShowCheckBox = true;
      this._dtpBegin.Size = new System.Drawing.Size(129, 20);
      this._dtpBegin.TabIndex = 1;
      this._dtpBegin.ValueChanged += new System.EventHandler(this._dtpBegin_ValueChanged);
      // 
      // _lblBegin
      // 
      this._lblBegin.AutoSize = true;
      this._lblBegin.Location = new System.Drawing.Point(297, 0);
      this._lblBegin.Name = "_lblBegin";
      this._lblBegin.Size = new System.Drawing.Size(23, 13);
      this._lblBegin.TabIndex = 0;
      this._lblBegin.Text = "От:";
      // 
      // _lblEnd
      // 
      this._lblEnd.AutoSize = true;
      this._lblEnd.Location = new System.Drawing.Point(432, 0);
      this._lblEnd.Name = "_lblEnd";
      this._lblEnd.Size = new System.Drawing.Size(25, 13);
      this._lblEnd.TabIndex = 0;
      this._lblEnd.Text = "До:";
      // 
      // _dtpEnd
      // 
      this._dtpEnd.Checked = false;
      this._dtpEnd.Dock = System.Windows.Forms.DockStyle.Fill;
      this._dtpEnd.Location = new System.Drawing.Point(432, 17);
      this._dtpEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
      this._dtpEnd.Name = "_dtpEnd";
      this._dtpEnd.ShowCheckBox = true;
      this._dtpEnd.Size = new System.Drawing.Size(124, 20);
      this._dtpEnd.TabIndex = 1;
      // 
      // _btnReport
      // 
      this._btnReport.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnReport.Location = new System.Drawing.Point(3, 3);
      this._btnReport.Name = "_btnReport";
      this._btnReport.Size = new System.Drawing.Size(72, 27);
      this._btnReport.TabIndex = 0;
      this._btnReport.Text = "Отчет";
      this._btnReport.UseVisualStyleBackColor = false;
      this._btnReport.Click += new System.EventHandler(this._btnReport_Click);
      // 
      // _btnChart
      // 
      this._btnChart.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnChart.Location = new System.Drawing.Point(81, 3);
      this._btnChart.Name = "_btnChart";
      this._btnChart.Size = new System.Drawing.Size(72, 27);
      this._btnChart.TabIndex = 0;
      this._btnChart.Text = "Сделки";
      this._btnChart.UseVisualStyleBackColor = false;
      this._btnChart.Click += new System.EventHandler(this._btnChart_Click);
      // 
      // _btnNewStrategy
      // 
      this._btnNewStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnNewStrategy.Location = new System.Drawing.Point(159, 3);
      this._btnNewStrategy.Name = "_btnNewStrategy";
      this._btnNewStrategy.Size = new System.Drawing.Size(73, 27);
      this._btnNewStrategy.TabIndex = 0;
      this._btnNewStrategy.Text = "Стратегия";
      this._btnNewStrategy.UseVisualStyleBackColor = false;
      this._btnNewStrategy.Click += new System.EventHandler(this._btnNewStrategy_Click);
      // 
      // _tbllayoutReports
      // 
      this._tbllayoutReports.ColumnCount = 3;
      this._tbllayoutReports.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllayoutReports.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllayoutReports.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllayoutReports.Controls.Add(this._btnNewStrategy, 2, 0);
      this._tbllayoutReports.Controls.Add(this._btnChart, 1, 0);
      this._tbllayoutReports.Controls.Add(this._btnReport, 0, 0);
      this._tbllayoutReports.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tbllayoutReports.Location = new System.Drawing.Point(436, 3);
      this._tbllayoutReports.Name = "_tbllayoutReports";
      this._tbllayoutReports.RowCount = 1;
      this._tbllayoutReports.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._tbllayoutReports.Size = new System.Drawing.Size(235, 33);
      this._tbllayoutReports.TabIndex = 0;
      // 
      // _tbllayoutSSButton
      // 
      this._tbllayoutSSButton.ColumnCount = 2;
      this._tbllayoutSSButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.2924F));
      this._tbllayoutSSButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.7076F));
      this._tbllayoutSSButton.Controls.Add(this._btnStop, 1, 0);
      this._tbllayoutSSButton.Controls.Add(this._btnStart, 0, 0);
      this._tbllayoutSSButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tbllayoutSSButton.Location = new System.Drawing.Point(291, 3);
      this._tbllayoutSSButton.Name = "_tbllayoutSSButton";
      this._tbllayoutSSButton.RowCount = 1;
      this._tbllayoutSSButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tbllayoutSSButton.Size = new System.Drawing.Size(139, 33);
      this._tbllayoutSSButton.TabIndex = 0;
      // 
      // _pnlBtnReport
      // 
      this._pnlBtnReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pnlBtnReport.BackColor = System.Drawing.Color.White;
      this._pnlBtnReport.Controls.Add(this.tableLayoutPanel3);
      this._pnlBtnReport.Location = new System.Drawing.Point(3, 50);
      this._pnlBtnReport.Margin = new System.Windows.Forms.Padding(0);
      this._pnlBtnReport.Name = "_pnlBtnReport";
      this._pnlBtnReport.Size = new System.Drawing.Size(674, 39);
      this._pnlBtnReport.TabIndex = 1;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
      this.tableLayoutPanel3.ColumnCount = 3;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
      this.tableLayoutPanel3.Controls.Add(this._tbllayoutReports, 2, 0);
      this.tableLayoutPanel3.Controls.Add(this._pnlProgress, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this._tbllayoutSSButton, 1, 0);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(674, 39);
      this.tableLayoutPanel3.TabIndex = 5;
      // 
      // _pnlProgress
      // 
      this._pnlProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pnlProgress.Controls.Add(this._pgsMain);
      this._pnlProgress.Controls.Add(this._lblHistory);
      this._pnlProgress.Controls.Add(this._lblTotalProccess);
      this._pnlProgress.Controls.Add(this._pgsChild);
      this._pnlProgress.Location = new System.Drawing.Point(3, 3);
      this._pnlProgress.Name = "_pnlProgress";
      this._pnlProgress.Size = new System.Drawing.Size(282, 31);
      this._pnlProgress.TabIndex = 2;
      // 
      // _pnlProperty
      // 
      this._pnlProperty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pnlProperty.BackColor = System.Drawing.Color.White;
      this._pnlProperty.Controls.Add(this._btnSettings);
      this._pnlProperty.Controls.Add(this._tbllayoutDTM);
      this._pnlProperty.Location = new System.Drawing.Point(3, 3);
      this._pnlProperty.Name = "_pnlProperty";
      this._pnlProperty.Size = new System.Drawing.Size(674, 42);
      this._pnlProperty.TabIndex = 4;
      this._pnlProperty.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlProperty_Paint);
      // 
      // _btnSettings
      // 
      this._btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnSettings.Location = new System.Drawing.Point(565, 9);
      this._btnSettings.Name = "_btnSettings";
      this._btnSettings.Size = new System.Drawing.Size(106, 25);
      this._btnSettings.TabIndex = 6;
      this._btnSettings.Text = "Settings";
      this._btnSettings.UseVisualStyleBackColor = false;
      this._btnSettings.Click += new System.EventHandler(this._btnSettings_Click);
      // 
      // _tbllayoutDTM
      // 
      this._tbllayoutDTM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbllayoutDTM.ColumnCount = 4;
      this._tbllayoutDTM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this._tbllayoutDTM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.39095F));
      this._tbllayoutDTM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.12757F));
      this._tbllayoutDTM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.48148F));
      this._tbllayoutDTM.Controls.Add(this._cmbStrategy, 0, 1);
      this._tbllayoutDTM.Controls.Add(this._lblEnd, 3, 0);
      this._tbllayoutDTM.Controls.Add(this._cmbSymbols, 1, 1);
      this._tbllayoutDTM.Controls.Add(this._dtpEnd, 3, 1);
      this._tbllayoutDTM.Controls.Add(this._lblSymbol, 1, 0);
      this._tbllayoutDTM.Controls.Add(this._lblBegin, 2, 0);
      this._tbllayoutDTM.Controls.Add(this._dtpBegin, 2, 1);
      this._tbllayoutDTM.Controls.Add(this.label1, 0, 0);
      this._tbllayoutDTM.Location = new System.Drawing.Point(0, 0);
      this._tbllayoutDTM.Name = "_tbllayoutDTM";
      this._tbllayoutDTM.RowCount = 2;
      this._tbllayoutDTM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._tbllayoutDTM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
      this._tbllayoutDTM.Size = new System.Drawing.Size(559, 40);
      this._tbllayoutDTM.TabIndex = 5;
      // 
      // _cmbStrategy
      // 
      this._cmbStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
      this._cmbStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbStrategy.Location = new System.Drawing.Point(3, 17);
      this._cmbStrategy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
      this._cmbStrategy.Name = "_cmbStrategy";
      this._cmbStrategy.Size = new System.Drawing.Size(144, 21);
      this._cmbStrategy.TabIndex = 4;
      // 
      // _lblSymbol
      // 
      this._lblSymbol.AutoSize = true;
      this._lblSymbol.Location = new System.Drawing.Point(153, 0);
      this._lblSymbol.Name = "_lblSymbol";
      this._lblSymbol.Size = new System.Drawing.Size(41, 13);
      this._lblSymbol.TabIndex = 3;
      this._lblSymbol.Text = "Symbol";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(46, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Strategy";
      // 
      // TesterPanel
      // 
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this._pnlBtnReport);
      this.Controls.Add(this._pnlProperty);
      this.MinimumSize = new System.Drawing.Size(400, 94);
      this.Name = "TesterPanel";
      this.Size = new System.Drawing.Size(682, 94);
      this._tbllayoutReports.ResumeLayout(false);
      this._tbllayoutSSButton.ResumeLayout(false);
      this._pnlBtnReport.ResumeLayout(false);
      this.tableLayoutPanel3.ResumeLayout(false);
      this._pnlProgress.ResumeLayout(false);
      this._pnlProgress.PerformLayout();
      this._pnlProperty.ResumeLayout(false);
      this._tbllayoutDTM.ResumeLayout(false);
      this._tbllayoutDTM.PerformLayout();
      this.ResumeLayout(false);

		}
		#endregion

    #region internal static string ConvertDateTimeToUnix(DateTime dt)
    internal static string ConvertDateTimeToUnix(DateTime dt) {
      UInt32 retval = (UInt32)((dt.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks) / 10000000L);
      
      return Convert.ToString(retval);
    }
    #endregion

    #region internal static DateTime ConvertUnixToDateTime(string sctm)
    internal static DateTime ConvertUnixToDateTime(string sctm) {
      if(sctm == "") return DateTime.Now;
      try {
        UInt32 ctm = Convert.ToUInt32(sctm);
        return new DateTime((long)(ctm * 10000000L) + new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks);
      } catch { }
      return DateTime.Now;
    }
    #endregion

    #region private void SaveConfigValue()
    private void SaveConfigValue() {
      if(_symbol == null) return;

      _cfgval["Symbol"].SetValue(_symbol.Name);
      
      _cfgval[_symbol.Name]["BDateUse"].SetValue(_dtpBegin.Checked);

      string bval = _dtpBegin.Checked ? ConvertDateTimeToUnix(_dtpBegin.Value) : "0";
      _cfgval[_symbol.Name]["BDate"].SetValue(bval);
      
      _cfgval[_symbol.Name]["EDateUse"].SetValue(_dtpEnd.Checked);

      string eval = _dtpEnd.Checked ? ConvertDateTimeToUnix(_dtpEnd.Value) : "0";
      _cfgval[_symbol.Name]["EDate"].SetValue(eval);
    }
    #endregion

    #region private void SetSymbol(Symbol symbol)
    private void SetSymbol(ISymbol symbol){

      if(_symbol != symbol)
        this.SaveConfigValue();


			_symbol = symbol;
			
			if (_symbol == null) return;

    if (_symbol.Ticks.Count == 0)
      return;

      DateTime mindt = new DateTime(Math.Max(_symbol.Ticks.TimeFrom.Ticks, (new DateTime(1753, 1, 2)).Ticks));
      DateTime maxdt = new DateTime(Math.Max(_symbol.Ticks.TimeTo.Ticks, (new DateTime(1753, 1, 3)).Ticks));

      this._dtpBegin.MinDate = this._dtpEnd.MinDate = new DateTime(1753, 1, 1);
      this._dtpBegin.MinDate = this._dtpEnd.MinDate = mindt;

      this._dtpBegin.MaxDate = this._dtpEnd.MaxDate = new DateTime(9998, 12, 31);
      this._dtpBegin.MaxDate = this._dtpEnd.MaxDate = maxdt;

			this._dtpBegin.Value = mindt;
			this._dtpEnd.Value = maxdt;

      _dtpBegin.Checked = _cfgval[_symbol.Name]["BDateUse", false];
      if(_dtpBegin.Checked) {
        long ldtm = ConvertUnixToDateTime(_cfgval[_symbol.Name]["BDate", "0"]).Ticks;
        if(ldtm >= mindt.Ticks && ldtm <= maxdt.Ticks)
          _dtpBegin.Value = new DateTime(ldtm);
        else
          _dtpBegin.Checked = false;
      } else {
        this._dtpBegin.Value = mindt;
      }

      _dtpEnd.Checked = _cfgval[_symbol.Name]["EDateUse", false];
      if(_dtpEnd.Checked) {
        DateTime dtm = ConvertUnixToDateTime(_cfgval[_symbol.Name]["EDate", "0"]);
        if(dtm >= mindt && dtm <= maxdt)
          _dtpEnd.Value = dtm;
        else
          _dtpEnd.Checked = false;
      } else {
        this._dtpEnd.Value = maxdt;
      }
      this._dtpBegin.Refresh();
    }
		#endregion

    #region private StrategyListItem SelectStrategy
    private StrategyListItem SelectedStrategy {
      get {
        return _cmbStrategy.SelectedItem as StrategyListItem;
      }
    }
    #endregion

    #region public void UpdateStrategyList()
    public void UpdateStrategyList() {
      if (_isStart) return;

      List<StrategyListItem> list = new List<StrategyListItem>();
      foreach (object o in this._cmbStrategy.Items) 
        list.Add(o as StrategyListItem);
      
      StrategyListItem selectItem = null, currentSelectItem = this.SelectedStrategy;

      _cmbStrategy.Items.Clear();

      foreach (Form form in GordagoMain.MainForm.MdiChildren) {
        if (form is IStrategyForm) {
          IStrategyForm sform = form as IStrategyForm;
          foreach (string sname in sform.StrategyNames) {
            StrategyListItem sli = new StrategyListItem(sname, sform);

            for (int i = 0; i < list.Count; i++) {
              StrategyListItem oldItem = list[i];
              if (sli.Name == oldItem.Name) {
                sli.Report = oldItem.Report;
                break;
              }
            }
            _cmbStrategy.Items.Add(sli);

            if (currentSelectItem != null) {
              if (sli.Name == currentSelectItem.Name)
                selectItem = sli;
            }
          }
        }
      }
      if (selectItem != null)
        _cmbStrategy.SelectedItem = selectItem;
      else {
        if (_cmbStrategy.Items.Count>0)
          _cmbStrategy.SelectedIndex = 0;
      }
    }
    #endregion

    #region internal void SelectStrategy(IStrategyForm sform)
    internal void SelectStrategy(IStrategyForm sform) {
      if (sform != null){
        foreach (object o in this._cmbStrategy.Items) {
          StrategyListItem sli = o as StrategyListItem;
          if (sli.Form == sform) {
            this._cmbStrategy.SelectedItem = sli;
            break;
          }
        }
      }
    }
    #endregion

    #region private void _btnStart_Click(object sender, System.EventArgs e)
    private void _btnStart_Click(object sender, System.EventArgs e) {
      if(_optimizer == null) {
        if(this._symbol == null) return;
        StrategyListItem strategyItem = this.SelectedStrategy;

        if(strategyItem == null) {
          MessageBox.Show("It is necessary to open strategy", GordagoMain.MessageCaption);
          return;
        }

        IStrategyForm sform = strategyItem.Form;
        CompileDllData cmpldata = sform.Compile(strategyItem.Name);

        if(cmpldata == null) {
          MessageBox.Show("Error in strategy", GordagoMain.MessageCaption);
          return;
        }
#if DEMO
        if (cmpldata.Strategy is VisualStrategy) {

          if (DialogResult.OK != MessageBox.Show(Language.Dictionary.GetString(1, 4), GordagoMain.MessageCaption, MessageBoxButtons.OKCancel)) {
            if (GordagoMain.MainForm.ShowRegPage() == DialogResult.OK)
              return;
          }
        }
#endif

        DateTime dtpb = this._dtpBegin.Value;
        dtpb = new DateTime(dtpb.Year, dtpb.Month, dtpb.Day, 0, 0, 0, 0);

        DateTime dtpe = this._dtpEnd.Value;
        dtpe = new DateTime(dtpe.Year, dtpe.Month, dtpe.Day, 23, 59, 59, 0);


        _tbegin = this._dtpBegin.Checked ? dtpb.Ticks : -1;
        _tend = this._dtpEnd.Checked ? dtpe.Ticks : -1;

        _currentTestedStrategy = strategyItem;

        _optimizer = new Optimizer(_symbol, cmpldata.Strategy, cmpldata.Variables, _tbegin, _tend, 0, 0, 0, _settings);
        _optimizer.ProgressChangedEvent += new OptimizerCallbackHandler(this.OptCallback);
        _optimizer.ProcessStatusChangedEvent += new OptimizerHandler(OptimizerProcessStatusChanged);

        Thread th = new Thread(new ThreadStart(this.StartProccess));
        th.IsBackground = true;
        th.Name = "Tester";
        th.Priority = ThreadPriority.Lowest;
        th.Start();
      } else if(_optimizer.ProcessStatus == OptimizerProcessStatus.Starting) {
        if(_optimizer != null)
          _optimizer.Pause();
      } else {
        if(_optimizer != null)
          _optimizer.Resume();
      }
    }
		#endregion

		#region private void OptimizerProcessStatusChanged()
		private void OptimizerProcessStatusChanged(){
      if(!this.InvokeRequired) {
        if(_optimizer == null)
          return;
        OptimizerProcessStatus status = _optimizer.ProcessStatus;

        string lngPause = Dictionary.GetString(7,16);
        string lngStart = Dictionary.GetString(7,7);
        string lngStop = Dictionary.GetString(7,8);
        string lngNextGo = Dictionary.GetString(7,17);

        if(status == OptimizerProcessStatus.Starting) {

          this._btnStart.Text = lngPause;
          this._btnStart.Image = GordagoImages.Images.GetImage("pause", "tester");
          this._btnStop.Enabled = true;
          this._btnStart.Text = lngPause;
          this._btnStart.Image = GordagoImages.Images.GetImage("pause", "tester");

        } else {
          this._btnStart.Text = lngNextGo;
          this._btnStart.Image = GordagoImages.Images.GetImage("start", "tester");
          this._btnStart.Enabled = true;

          if(status == OptimizerProcessStatus.Stopping) {
            this._btnStart.Text = lngStart;
            this._btnStop.Enabled = false;
            this._pgsChild.Value = 0;
            this._pgsMain.Value = 0;
          } else {
            this._btnStop.Enabled = true;
          }
        }
      } else {
        this.Invoke(new OptimizerHandler(this.OptimizerProcessStatusChanged));
      }
		}
		#endregion

    #region private void StartProccess()
    private void StartProccess() {
      _isStart = true;
      this.SetStartStatus();

      _optimizer.Start();
      if(GordagoMain.IsCloseProgram) 
        return;

      _currentTestedStrategy.Report = _optimizer.Correlation;
      _isStart = false;

      this.SetStartStatus();
      
      _optimizer = null;
      this.RefreshReportButtons();
      this.ShowReport();
    }
    #endregion

    #region private void SetStartStatus()
    private void SetStartStatus() {
      if(!this.InvokeRequired) {

        _cmbSymbols.Enabled =
          _dtpBegin.Enabled =
          _dtpEnd.Enabled = 
          _cmbStrategy.Enabled =  _btnSettings.Enabled = !_isStart;


        if (this.SelectedStrategy != null) {
          this.SelectedStrategy.Form.SetTestStatus(_isStart);
          if (_currentTestedStrategy != null && !_isStart) {
            this.SelectedStrategy.Report = _currentTestedStrategy.Report;
            _currentTestedStrategy = null;
          }
        }

      } else {
        this.Invoke(new OptimizerHandler(this.SetStartStatus));
      }
    }
    #endregion

    #region private void OptCallback(int a, int b)
    private void OptCallback(int a, int b) {
      if(!this.InvokeRequired) {
        this._pgsChild.Value = a > 100 ? 100 : a;
        this._pgsMain.Value = b > 100 ? 100 : b;
      } else {
        this.Invoke(new OptimizerCallbackHandler(this.OptCallback), new object[] { a, b });
      }
		}
		#endregion

		#region private void _btnReport_Click(object sender, System.EventArgs e)
		private void _btnReport_Click(object sender, System.EventArgs e) {
			this.ShowReport();
		}
		#endregion

    #region private void ShowReport()
    private void ShowReport() {
      if (!this.InvokeRequired) {

        if (this.SelectedStrategy == null)
          return;

        if (SelectedStrategy.Report == null)
          return;

        TestReportForm rform = new TestReportForm();
        rform.SetTestServer(SelectedStrategy.Report, SelectedStrategy.Form.FileName);
        rform.Show();
      } else {
        this.Invoke(new OptimizerHandler(this.ShowReport));
      }
    }
    #endregion

    #region private void _btnChart_Click(object sender, System.EventArgs e)
    private void _btnChart_Click(object sender, System.EventArgs e) {
      if (this.SelectedStrategy == null) return;
      if (this.SelectedStrategy.Report == null) return;

      ChartForm chform = null;
      ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(this.SelectedStrategy.Report.SymbolName);

      if (GordagoMain.MainForm.ActiveMdiChild is ChartForm) {
        chform = GordagoMain.MainForm.ActiveMdiChild as ChartForm;
        if (chform.Symbol.Name != symbol.Name)
          chform = null;
      } 
      if (chform == null){
        chform = GordagoMain.MainForm.ChartShowNewForm(symbol);
      }

      chform.SetStrategyReport(SelectedStrategy.Report);
    }
		#endregion

		#region private void _btnStop_Click(object sender, System.EventArgs e)
		private void _btnStop_Click(object sender, System.EventArgs e) {
			this._btnStop.Enabled = false;
			this._btnStart.Enabled = false;
      if(_optimizer != null)
        _optimizer.Stop();
		}
		#endregion

		#region private void _btnNewStrategy_Click(object sender, System.EventArgs e) 
		private void _btnNewStrategy_Click(object sender, System.EventArgs e) {
#if DEMO
			if (GordagoMain.CountEvalutionDay < 0){
				MessageBox.Show(Dictionary.GetString(13,3), GordagoMain.MessageCaption);
				return;
			}
#endif


      EditorForm sform = this.SelectedStrategy.Form as EditorForm;
      if (this.SelectedStrategy == null || sform == null) {
        MessageBox.Show("It is necessary to open strategy", GordagoMain.MessageCaption);
        return;
      }

      if (SelectedStrategy.Report == null) {
				MessageBox.Show(Dictionary.GetString(1,7,"Данную стратегию необходимо протестировать"), GordagoMain.MessageCaption);
				return;
			}
      
      EditorForm wf = EditorForm.CloneStrategyToOptimized(sform, SelectedStrategy.Report);
			if (wf == null) return;
      wf.MdiParent = GordagoMain.MainForm;
			wf.Show();
		}
		#endregion

    #region public void UpdateSymbolsList()
    public void UpdateSymbolsList(){
      if(!this.InvokeRequired) {
        this._cmbSymbols.Items.Clear();
        for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
          ISymbol symbol = GordagoMain.SymbolEngine[i];
          if (!GordagoMain.MainForm.GetSymbolIsHide(symbol))
            this._cmbSymbols.Items.Add(symbol);
        }
        this.SetSymbol(null);

        string sname = _cfgval["Symbol", "EURUSD"];
        ISymbol smb = GordagoMain.SymbolEngine.GetSymbol(sname);
        if(smb != null)
          this._cmbSymbols.SelectedItem = smb;
      } else {
        this.Invoke(new UpdateSymbolListHandler(this.UpdateSymbolsList));
      }
		}
		#endregion

		#region private void _cmbSymbols_SelectedIndexChanged(object sender, System.EventArgs e) 
		private void _cmbSymbols_SelectedIndexChanged(object sender, System.EventArgs e) {
			ISymbol symbol = this._cmbSymbols.SelectedItem as ISymbol;
			this.SetSymbol(symbol);
      if(symbol == null) return;
		}
		#endregion

    #region private void _nudSpred_ValueChanged(object sender, EventArgs e)
    private void _nudSpred_ValueChanged(object sender, EventArgs e) {
      if(this._symbol == null) return;
    }
    #endregion

    #region private void _nudMinTF_ValueChanged(object sender, EventArgs e)
    private void _nudMinTF_ValueChanged(object sender, EventArgs e) {
      if(_symbol == null) return;
    }
    #endregion

    #region private void _dtpBegin_ValueChanged(object sender, EventArgs e)
    private void _dtpBegin_ValueChanged(object sender, EventArgs e) {
      if(_symbol == null) return;
    }
    #endregion

    #region private void RefreshReportButtons()
    private void RefreshReportButtons() {
      if(!this.InvokeRequired) {
        bool enabled = false;
        if (SelectedStrategy != null)
          if (SelectedStrategy.Report != null)
            enabled = true;

        this._btnChart.Enabled =
          this._btnReport.Enabled =
          this._btnNewStrategy.Enabled = enabled;

      } else {
        this.Invoke(new OptimizerHandler(this.RefreshReportButtons));
      }
    }
    #endregion

    #region private void _btnSettings_Click(object sender, EventArgs e)
    private void _btnSettings_Click(object sender, EventArgs e) {
      VirtualBrokerSettingsForm frm = new VirtualBrokerSettingsForm();
      frm.ShowDialog();

      _settings = frm.Settings; ;
    }
    #endregion

    #region private void _cmbStrategy_SelectedIndexChanged(object sender, EventArgs e)
    private void _cmbStrategy_SelectedIndexChanged(object sender, EventArgs e) {
      this.RefreshReportButtons();
    }
    #endregion

    private void _pnlProperty_Paint(object sender, PaintEventArgs e) {

    }
  }
}


