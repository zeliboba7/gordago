namespace Gordago.Analysis.Chart {
  partial class ChartForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if(disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartForm));
      this._tooltip = new System.Windows.Forms.ToolTip(this.components);
      this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._mniCChartPanels = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCActiveCharts = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCSymbolsPanel = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCTicksChart = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCTrade = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this._mniCTimeFrame = new System.Windows.Forms.ToolStripMenuItem();
      this._mniTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this._mniTSaveTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this._mniTLoadTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this._mniTRemoveTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this._mniTSeparator = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this._mniCCandle = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCBar = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCLine = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this._mniCGrid = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCPeriodSeparators = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this._mniCZoomIn = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCZoomOut = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this._mniCSaveAsPicture = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCSaveReport = new System.Windows.Forms.ToolStripMenuItem();
      this._mnsepCRemIndic = new System.Windows.Forms.ToolStripSeparator();
      this._mniCRemoveIndicator = new System.Windows.Forms.ToolStripMenuItem();
      this._mniCRemoveAllIndicators = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this._mniCRemoveAllObjects = new System.Windows.Forms.ToolStripMenuItem();
      this._chartmanager = new Gordago.Analysis.Chart.ChartManager();
      this._contextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // _contextMenu
      // 
      this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniCChartPanels,
            this.toolStripSeparator6,
            this._mniCTimeFrame,
            this._mniTemplate,
            this.toolStripSeparator1,
            this._mniCCandle,
            this._mniCBar,
            this._mniCLine,
            this.toolStripSeparator2,
            this._mniCGrid,
            this._mniCPeriodSeparators,
            this.toolStripSeparator3,
            this._mniCZoomIn,
            this._mniCZoomOut,
            this.toolStripSeparator5,
            this._mniCSaveAsPicture,
            this._mniCSaveReport,
            this._mnsepCRemIndic,
            this._mniCRemoveIndicator,
            this._mniCRemoveAllIndicators,
            this.toolStripSeparator4,
            this._mniCRemoveAllObjects});
      this._contextMenu.Name = "_contextMenu";
      this._contextMenu.Size = new System.Drawing.Size(186, 376);
      this._contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenu_Opening);
      // 
      // _mniCChartPanels
      // 
      this._mniCChartPanels.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniCActiveCharts,
            this._mniCSymbolsPanel,
            this._mniCTicksChart,
            this._mniCTrade});
      this._mniCChartPanels.Name = "_mniCChartPanels";
      this._mniCChartPanels.Size = new System.Drawing.Size(185, 22);
      this._mniCChartPanels.Text = "Panels";
      // 
      // _mniCActiveCharts
      // 
      this._mniCActiveCharts.Name = "_mniCActiveCharts";
      this._mniCActiveCharts.Size = new System.Drawing.Size(150, 22);
      this._mniCActiveCharts.Text = "Active Charts";
      this._mniCActiveCharts.Visible = false;
      // 
      // _mniCSymbolsPanel
      // 
      this._mniCSymbolsPanel.Name = "_mniCSymbolsPanel";
      this._mniCSymbolsPanel.Size = new System.Drawing.Size(150, 22);
      this._mniCSymbolsPanel.Text = "Symbols";
      this._mniCSymbolsPanel.Click += new System.EventHandler(this._mniCSymbolsPanel_Click);
      // 
      // _mniCTicksChart
      // 
      this._mniCTicksChart.Name = "_mniCTicksChart";
      this._mniCTicksChart.Size = new System.Drawing.Size(150, 22);
      this._mniCTicksChart.Text = "Tick Chart";
      this._mniCTicksChart.Click += new System.EventHandler(this._mniCTicksChart_Click);
      // 
      // _mniCTrade
      // 
      this._mniCTrade.Name = "_mniCTrade";
      this._mniCTrade.Size = new System.Drawing.Size(150, 22);
      this._mniCTrade.Text = "Trade";
      this._mniCTrade.Click += new System.EventHandler(this._mniCTrade_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCTimeFrame
      // 
      this._mniCTimeFrame.Image = global::Gordago.Properties.Resources.m_ctimeframe;
      this._mniCTimeFrame.Name = "_mniCTimeFrame";
      this._mniCTimeFrame.Size = new System.Drawing.Size(185, 22);
      this._mniCTimeFrame.Text = "Time Frame";
      // 
      // _mniTemplate
      // 
      this._mniTemplate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniTSaveTemplate,
            this._mniTLoadTemplate,
            this._mniTRemoveTemplate,
            this._mniTSeparator});
      this._mniTemplate.Name = "_mniTemplate";
      this._mniTemplate.Size = new System.Drawing.Size(185, 22);
      this._mniTemplate.Text = "Template";
      // 
      // _mniTSaveTemplate
      // 
      this._mniTSaveTemplate.Name = "_mniTSaveTemplate";
      this._mniTSaveTemplate.Size = new System.Drawing.Size(169, 22);
      this._mniTSaveTemplate.Text = "Save template ...";
      this._mniTSaveTemplate.Click += new System.EventHandler(this._mniTSaveTemplate_Click);
      // 
      // _mniTLoadTemplate
      // 
      this._mniTLoadTemplate.Name = "_mniTLoadTemplate";
      this._mniTLoadTemplate.Size = new System.Drawing.Size(169, 22);
      this._mniTLoadTemplate.Text = "Load template ...";
      this._mniTLoadTemplate.Click += new System.EventHandler(this._mniTLoadTemplate_Click);
      // 
      // _mniTRemoveTemplate
      // 
      this._mniTRemoveTemplate.Name = "_mniTRemoveTemplate";
      this._mniTRemoveTemplate.Size = new System.Drawing.Size(169, 22);
      this._mniTRemoveTemplate.Text = "Remove template";
      // 
      // _mniTSeparator
      // 
      this._mniTSeparator.Name = "_mniTSeparator";
      this._mniTSeparator.Size = new System.Drawing.Size(166, 6);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCCandle
      // 
      this._mniCCandle.Image = global::Gordago.Properties.Resources.m_ccandle;
      this._mniCCandle.Name = "_mniCCandle";
      this._mniCCandle.Size = new System.Drawing.Size(185, 22);
      this._mniCCandle.Text = "Candlesticks";
      this._mniCCandle.Click += new System.EventHandler(this._mniCCandle_Click);
      // 
      // _mniCBar
      // 
      this._mniCBar.Image = global::Gordago.Properties.Resources.m_cbar;
      this._mniCBar.Name = "_mniCBar";
      this._mniCBar.Size = new System.Drawing.Size(185, 22);
      this._mniCBar.Text = "Bar Chart";
      this._mniCBar.Click += new System.EventHandler(this._mniCBar_Click);
      // 
      // _mniCLine
      // 
      this._mniCLine.Image = global::Gordago.Properties.Resources.m_cline;
      this._mniCLine.Name = "_mniCLine";
      this._mniCLine.Size = new System.Drawing.Size(185, 22);
      this._mniCLine.Text = "Line Chart";
      this._mniCLine.Click += new System.EventHandler(this._mniCLine_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCGrid
      // 
      this._mniCGrid.Image = global::Gordago.Properties.Resources.m_cgrid;
      this._mniCGrid.Name = "_mniCGrid";
      this._mniCGrid.Size = new System.Drawing.Size(185, 22);
      this._mniCGrid.Text = "Grid";
      this._mniCGrid.Click += new System.EventHandler(this._mniCGrid_Click);
      // 
      // _mniCPeriodSeparators
      // 
      this._mniCPeriodSeparators.Name = "_mniCPeriodSeparators";
      this._mniCPeriodSeparators.Size = new System.Drawing.Size(185, 22);
      this._mniCPeriodSeparators.Text = "Period Separators";
      this._mniCPeriodSeparators.Click += new System.EventHandler(this._mniCPeriodSeparators_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCZoomIn
      // 
      this._mniCZoomIn.Image = global::Gordago.Properties.Resources.m_czoomin;
      this._mniCZoomIn.Name = "_mniCZoomIn";
      this._mniCZoomIn.Size = new System.Drawing.Size(185, 22);
      this._mniCZoomIn.Text = "Zoom In";
      this._mniCZoomIn.Click += new System.EventHandler(this._mniCZoomIn_Click);
      // 
      // _mniCZoomOut
      // 
      this._mniCZoomOut.Image = global::Gordago.Properties.Resources.m_czoomout;
      this._mniCZoomOut.Name = "_mniCZoomOut";
      this._mniCZoomOut.Size = new System.Drawing.Size(185, 22);
      this._mniCZoomOut.Text = "Zoom Out";
      this._mniCZoomOut.Click += new System.EventHandler(this._mniCZoomOut_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCSaveAsPicture
      // 
      this._mniCSaveAsPicture.Name = "_mniCSaveAsPicture";
      this._mniCSaveAsPicture.Size = new System.Drawing.Size(185, 22);
      this._mniCSaveAsPicture.Text = "Save As Picture..";
      this._mniCSaveAsPicture.Click += new System.EventHandler(this._mniC_Click);
      // 
      // _mniCSaveReport
      // 
      this._mniCSaveReport.Name = "_mniCSaveReport";
      this._mniCSaveReport.Size = new System.Drawing.Size(185, 22);
      this._mniCSaveReport.Text = "Save As Report...";
      this._mniCSaveReport.Click += new System.EventHandler(this._mniCSaveReport_Click);
      // 
      // _mnsepCRemIndic
      // 
      this._mnsepCRemIndic.Name = "_mnsepCRemIndic";
      this._mnsepCRemIndic.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCRemoveIndicator
      // 
      this._mniCRemoveIndicator.Name = "_mniCRemoveIndicator";
      this._mniCRemoveIndicator.Size = new System.Drawing.Size(185, 22);
      this._mniCRemoveIndicator.Text = "Remove Indicator";
      this._mniCRemoveIndicator.Click += new System.EventHandler(this._mniCRemoveIndicator_Click);
      // 
      // _mniCRemoveAllIndicators
      // 
      this._mniCRemoveAllIndicators.Name = "_mniCRemoveAllIndicators";
      this._mniCRemoveAllIndicators.Size = new System.Drawing.Size(185, 22);
      this._mniCRemoveAllIndicators.Text = "Remove All Indicatos";
      this._mniCRemoveAllIndicators.Click += new System.EventHandler(this._mniCRemoveAllIndicators_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(182, 6);
      // 
      // _mniCRemoveAllObjects
      // 
      this._mniCRemoveAllObjects.Name = "_mniCRemoveAllObjects";
      this._mniCRemoveAllObjects.Size = new System.Drawing.Size(185, 22);
      this._mniCRemoveAllObjects.Text = "Remove All Objects";
      this._mniCRemoveAllObjects.Click += new System.EventHandler(this._mniCRemoveAllObjects_Click);
      // 
      // _chartmanager
      // 
      this._chartmanager.AllowDrop = true;
      this._chartmanager.AutoDeleteEmptyChartBox = true;
      this._chartmanager.ContextMenuStrip = this._contextMenu;
      this._chartmanager.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chartmanager.Location = new System.Drawing.Point(0, 0);
      this._chartmanager.Name = "_chartmanager";
      this._chartmanager.SelectedFigure = null;
      this._chartmanager.Size = new System.Drawing.Size(373, 300);
      this._chartmanager.TabIndex = 0;
      this._chartmanager.TimeFrameChanged += new System.EventHandler(this._chartManager_TimeFrameChanged);
      this._chartmanager.DragDrop += new System.Windows.Forms.DragEventHandler(this._chartManager_DragDrop);
      this._chartmanager.MouseTypeChanged += new System.EventHandler(this._chartmanager_MouseTypeChanged);
      this._chartmanager.SelectedFigureChanged += new System.EventHandler(this._chartManager_SelectedFigureChanged);
      this._chartmanager.SelectedFigureMouseChanged += new System.EventHandler(this._chartManager_SelectedFigureMouseChanged);
      this._chartmanager.ZoomChanged += new System.EventHandler(this._chartManager_ZoomChanged);
      this._chartmanager.PeriodSeparatorsChanged += new System.EventHandler(this._chartmanager_PeriodSeparatorsChanged);
      this._chartmanager.ChartShiftChanged += new System.EventHandler(this._chartmanager_ChartShiftChanged);
      this._chartmanager.BarTypeChanged += new System.EventHandler(this._chartManager_BarTypeChanged);
      this._chartmanager.GridVisibleChanged += new System.EventHandler(this._chartManager_GridVisibleChanged);
      this._chartmanager.PositionChanged += new System.EventHandler(this._chartManager_PositionChanged);
      this._chartmanager.DragOver += new System.Windows.Forms.DragEventHandler(this._chartManager_DragOver);
      this._chartmanager.Click += new System.EventHandler(this._chartmanager_Click);
      // 
      // ChartForm
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(373, 300);
      this.Controls.Add(this._chartmanager);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ChartForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "ChartForm";
      this._contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private ChartManager _chartmanager;
    private System.Windows.Forms.ToolTip _tooltip;
    private System.Windows.Forms.ContextMenuStrip _contextMenu;
    private System.Windows.Forms.ToolStripMenuItem _mniCTimeFrame;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem _mniCCandle;
    private System.Windows.Forms.ToolStripMenuItem _mniCBar;
    private System.Windows.Forms.ToolStripMenuItem _mniCLine;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem _mniCGrid;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem _mniCZoomIn;
    private System.Windows.Forms.ToolStripMenuItem _mniCZoomOut;
    private System.Windows.Forms.ToolStripSeparator _mnsepCRemIndic;
    private System.Windows.Forms.ToolStripMenuItem _mniCRemoveIndicator;
    private System.Windows.Forms.ToolStripMenuItem _mniCRemoveAllIndicators;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem _mniCRemoveAllObjects;
    private System.Windows.Forms.ToolStripMenuItem _mniTemplate;
    private System.Windows.Forms.ToolStripMenuItem _mniTSaveTemplate;
    private System.Windows.Forms.ToolStripMenuItem _mniTLoadTemplate;
    private System.Windows.Forms.ToolStripMenuItem _mniTRemoveTemplate;
    private System.Windows.Forms.ToolStripSeparator _mniTSeparator;
    private System.Windows.Forms.ToolStripMenuItem _mniCSaveReport;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem _mniCSaveAsPicture;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem _mniCChartPanels;
    private System.Windows.Forms.ToolStripMenuItem _mniCTrade;
    private System.Windows.Forms.ToolStripMenuItem _mniCActiveCharts;
    private System.Windows.Forms.ToolStripMenuItem _mniCSymbolsPanel;
    private System.Windows.Forms.ToolStripMenuItem _mniCTicksChart;
    private System.Windows.Forms.ToolStripMenuItem _mniCPeriodSeparators;
  }
}