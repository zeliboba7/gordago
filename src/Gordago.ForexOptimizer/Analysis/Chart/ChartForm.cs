/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Gordago.Strategy.FIndicator;
using Cursit.Applic.AConfig;
using Gordago.API;
using Gordago.Strategy;
using Language;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using Cursit.Utils;
#endregion

namespace Gordago.Analysis.Chart {
  partial class ChartForm:Form, IBrokerEvents, IBrokerSymbolsEvents {

    private long _savedUpdateOnlineRateTimeForChart = DateTime.Now.AddMinutes(-1).Ticks;
    private long _savedUpdateOnlineRateTimeForAll = DateTime.Now.AddMinutes(-1).Ticks;

    private ISymbol _symbol;
    private Font _fontdesc;

    private bool _autoscroll = true;

    private const string FN_ProgressBar = "__FigurePgsBarProgressBar";
    private const string FN_Description = "__FigureTxtDescription";
    private const string FN_PleaseWait = "__FigureTxtPlaseWait";
    private const string FN_HistoryDownload = "__FigureStartHistoryDownload";
    private const string FN_Trades = "__FigureTrade";
    private const string FN_Orders = "__FigureOrder";
    private const string FN_CloseTrades = "__FigureCloseTrades";
    private const string FN_CloseTradesTester = "__FigureCloseTradesTester";
    private const string FN_Bars = "__FigureBars";

    private int _tmpposition;

    private ChartTemplates _templates;

    private TickManagerStatus _currentTickManagerStatus = TickManagerStatus.Default;

    private ChartFigureProgressBar _progressBar = null;

    private DateTime _savedTMProcessTime = DateTime.Now;

    #region public ChartsForm(Symbol symbol)
    public ChartForm(ISymbol symbol) {

      _templates = new ChartTemplates(this);

      InitializeComponent();
      this.StartPosition = FormStartPosition.Manual;

      this._chartmanager.ChartPanels.PanelAdded += new EventHandler<ChartPanelEventArgs>(this._chartPanels_PanelAdded);

      _symbol = symbol;
      ITickManager tmanager = this._symbol.Ticks as ITickManager;
      tmanager.DataCachingChanged += new TickManagerEventHandler(tmanager_DataCachingChanged);

      _fontdesc = new Font(this._chartmanager.Style.ScaleFont.FontFamily, 7);

      TimeFrame inittf = TimeFrameManager.TimeFrames.GetTimeFrame(3600);
      if(inittf == null) {
        int avTF = TimeFrameManager.TimeFrames.Count / 2;
        inittf = TimeFrameManager.TimeFrames[avTF];
      }
      this._chartmanager.SetTraderComponent(GordagoMain.MainForm.ChartPanelManager, GordagoMain.MainForm.ChartObjectManager, this._symbol, GordagoMain.IndicatorManager, inittf);
      this.UpdateText();
      this.SetChartDescription(0, this._symbol.Name);

      ChartFigure figure = _chartmanager.ChartBoxes[0].Figures.GetFigure(FN_Bars);
      if(figure == null) {
        figure = new ChartFigureBar(FN_Bars);
        _chartmanager.ChartBoxes[0].Figures.Insert(figure);
      }
      figure.Visible = false;

      #region Initialize Menu Time Frame
      for(int i = 0; i < TimeFrameManager.TimeFrames.Count; i++) {
        TimeFrame tf = TimeFrameManager.TimeFrames[i];
        ToolStripMenuItem mnitf = new ToolStripMenuItem();
        mnitf.Name = "_mniCTF_" + tf.Second.ToString();
        mnitf.ToolTipText = mnitf.Text = string.Format("{0} ({1} second)", tf.Name, tf.Second);
        mnitf.Click += new EventHandler(this._mniCTF_Click);
        _mniCTimeFrame.DropDownItems.Add(mnitf);
      }
      #endregion

      _mniCCandle.Text = Dictionary.GetString(32, 2, "Candlestick");
      _mniCBar.Text = Dictionary.GetString(32, 3, "Bar Chart");
      _mniCLine.Text = Dictionary.GetString(32, 4, "Line Chart");
      _mniCGrid.Text = Dictionary.GetString(32, 7, "Grid");
      _mniCPeriodSeparators.Text = Dictionary.GetString(32, 34, "Period Separators");
      _mniCZoomIn.Text = Dictionary.GetString(32, 8, "Zoom In");
      _mniCZoomOut.Text = Dictionary.GetString(32, 9, "Zoom Out");
      _mniCSaveAsPicture.Text = Dictionary.GetString(32, 29, "Сохранить как рисунок...");
      _mniCSaveReport.Text = Dictionary.GetString(32, 28, "Save As Report...");
      _mniCTimeFrame.Text = Dictionary.GetString(32, 10, "Time Frame");
      _mniCRemoveAllObjects.Text = Dictionary.GetString(32, 13, "Remove all objects");

      _mniTemplate.Text = Dictionary.GetString(32, 22, "Шаблон");
      _mniTSaveTemplate.Text = Dictionary.GetString(32, 23, "Сохранить шаблон ...");
      _mniTLoadTemplate.Text = Dictionary.GetString(32, 24, "Загрузить шаблон ...");
      _mniTRemoveTemplate.Text = Dictionary.GetString(32, 25, "Удалить шаблон");

      _mniCChartPanels.Text = Dictionary.GetString(32, 31, "Panels");
      _mniCTicksChart.Text = Dictionary.GetString(32, 30, "Tick Chart");
      _mniCSymbolsPanel.Text = Dictionary.GetString(32, 32, "Symbols");
      _mniCTrade.Text = Dictionary.GetString(32, 33, "Trade");

      this.UpdateContextMenu();

      MainForm mf = GordagoMain.MainForm;
      if(mf.MdiChildren.Length > 0) 
        this.WindowState = mf.MdiChildren[mf.MdiChildren.Length - 1].WindowState;

      GordagoMain.MainForm.OnChartCreating(this.ChartManager);

      if ((this._symbol.Ticks as ITickManager).Status == TickManagerStatus.Default) {
        if ((this._symbol.Ticks as ITickManager).IsDataCaching) {
          /* Данные закешированны */
          InitializeBars();
        } else {
          (this._symbol.Ticks as ITickManager).DataCaching();
        }
      } else {
        tmanager_DataCachingChanged((this._symbol.Ticks as ITickManager), new TickManagerEventArgs((this._symbol.Ticks as ITickManager).Status, 0, 0));
      }
      this.RefreshStyle();

      this.BCM.UpdateSymbolHole(_symbol);
    }

    #endregion

    #region private bool IsVirtualBroker
    private bool IsVirtualBroker {
      get {
        return this.BCM.Broker is API.VirtualForex.VirtualBroker;
      }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public ChartTemplates Templates
    public ChartTemplates Templates {
      get { return this._templates; }
    }
    #endregion

    #region public void DestroyFigures()
    public void DestroyFigures() {
      IChartBox[] boxes = this.ChartManager.ChartBoxes;

      for(int i = 0; i < boxes.Length; i++) {
        boxes[i].Figures.Destroy();
      }
    }
    #endregion

    #region internal int TmpPostion
    internal int TmpPostion {
      get { return this._tmpposition; }
      set { this._tmpposition = value; }
    }
    #endregion

    #region public bool AutoScroll
    public bool ChartAutoScroll {
      get { return this._autoscroll; }
      set {
        this._autoscroll = value;
        UpdateMainFormComponents();
      }
    }
    #endregion

    #region public Symbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
    }
    #endregion

    #region public ChartManager ChartManager
    public ChartManager ChartManager {
      get { return this._chartmanager; }
    }
    #endregion

    #region protected override void OnActivated(EventArgs e)
    protected override void OnActivated(EventArgs e) {
      this.UpdateMainFormComponents();
      this._chartmanager.Focus();
    }
    #endregion

    #region protected override void OnLostFocus(EventArgs e)
    protected override void OnLostFocus(EventArgs e) {
      this.Invalidate();
      base.OnLostFocus(e);
    }
    #endregion

    #region private void InitializeBars()
    private void InitializeBars() {
      ChartFigure figure = _chartmanager.ChartBoxes[0].Figures.GetFigure(FN_Bars);
      figure.Visible = true;

      this.BrokerConnectionStatusChanged(this.BCM.ConnectionStatus);
      this.BCM.UpdateSymbolHole(this._symbol);
      this.RefreshStyle();
      this.UpdateMainFormComponents();
      this.UpdateSymbolInfo();
      this._chartmanager.Focus();
    }
    #endregion

    #region private void UpdateText()
    private void UpdateText() {
      string doptxt = "";
      if (this._chartmanager.Bars != null)
        doptxt = ", " + this._chartmanager.Bars.TimeFrame.Name;
      this.Text = _symbol.Name + doptxt;
    }
    #endregion

    #region private void UpdateMainFormComponents()
    private void UpdateMainFormComponents() {
      try {
        if(this._chartmanager.Bars != null && GordagoMain.MainForm.ActiveMdiChild == this) {
          GordagoMain.MainForm.ChartSetTimeFrameView(this._chartmanager.Bars.TimeFrame.Second);
          GordagoMain.MainForm.ChartSetZoomView();
          GordagoMain.MainForm.ChartSetGridVisibleView();
          GordagoMain.MainForm.ChartSetPeriodSeparatorsView();
          GordagoMain.MainForm.ChartSetBarTypeView();
          GordagoMain.MainForm.ChartSetChartShiftView();
          GordagoMain.MainForm.ChartSetAutoScrollView();
        }
        GordagoMain.MainForm.ChartSetMouseTypeView();
      } catch { }
    }
    #endregion

    #region private void UpdateCloseTrades()
    private void UpdateCloseTrades() {
      bool deleteFigure = false;

      if (BCM.ConnectionStatus != BrokerConnectionStatus.Online) {
        deleteFigure = true;
      } else {
        if (BCM.Broker.ClosedTrades.Count == 0)
          deleteFigure = true;
      }
      ChartFigureCloseTrades figure = this._chartmanager.ChartBoxes[0].Figures.GetFigure(FN_CloseTrades) as ChartFigureCloseTrades;

      if (figure == null && !deleteFigure) {
        this._chartmanager.ChartBoxes[0].Figures.Add(new ChartFigureCloseTrades(FN_CloseTrades, BCM.Broker.ClosedTrades));
      }

      if (figure != null && deleteFigure) {
        this._chartmanager.ChartBoxes[0].Figures.Remove(figure);
      }
      this.Invalidate();
    }
    #endregion

    #region private void UpdateTrades()
    private void UpdateTrades() {

      List<ITrade> trades = new List<ITrade>();
      if (BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
        for (int i = 0; i < BCM.Broker.Trades.Count;i++ ) {
          ITrade trade = BCM.Broker.Trades[i];
          if (trade.OnlineRate.Symbol.Name == this._symbol.Name) {
            trades.Add(trade);
          }
        }
      }
      
      if(trades.Count == 0) {
        this._chartmanager.ChartBoxes[0].Figures.RemoveAt(FN_Trades);
      } else {
        ChartFigureTrades figure = this._chartmanager.ChartBoxes[0].Figures.GetFigure(FN_Trades) as ChartFigureTrades;
        if(figure == null) {
          this._chartmanager.ChartBoxes[0].Figures.Add(new ChartFigureTrades(FN_Trades, trades.ToArray()));
        } else {
          figure.SetTrades(trades.ToArray());
        }
      }
      this.Invalidate();
    }
    #endregion

    #region private void UpdateOrders()
    private void UpdateOrders() {
      List<IOrder> orders = new List<IOrder>();
      if (BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
        for (int i = 0; i < BCM.Broker.Orders.Count; i++) {
          IOrder order = BCM.Broker.Orders[i];
          if (order.OnlineRate.Symbol.Name == this._symbol.Name) {
            orders.Add(order);
          }
        }
      }

      if(orders.Count == 0) {
        this._chartmanager.ChartBoxes[0].Figures.RemoveAt(FN_Orders);
      } else {
        ChartFigureOrders figure = this._chartmanager.ChartBoxes[0].Figures.GetFigure(FN_Orders) as ChartFigureOrders;
        if(figure == null) {
          this._chartmanager.ChartBoxes[0].Figures.Add(new ChartFigureOrders(FN_Orders, orders.ToArray()));
        } else {
          figure.SetOrders(orders.ToArray());
        }
      }
      this.Invalidate();
    }
    #endregion

    #region private void DeleteAllIndicator()
    private void DeleteAllIndicator() {
      bool del = false;
      for (int i = 0; i < this._chartmanager.ChartBoxes.Length; i++) {
        for (int index = 0;i < this._chartmanager.ChartBoxes.Length && index < this.ChartManager.ChartBoxes[i].Figures.Count; index++) {
          ChartFigure figure = this.ChartManager.ChartBoxes[i].Figures[index];

          if (figure is ChartFigureIndicator) {
            this.ChartManager.ChartBoxes[i].Figures.Remove(figure);
            del = true;
          }
        }
      }
      if (del)
        DeleteAllIndicator();
      this.Invalidate();
    }
    #endregion

    #region private void SetChartDescription(int numchart, string desc)
    private void SetChartDescription(int numchart, string desc) {
      ChartFigureText cftext = (ChartFigureText)this._chartmanager.ChartBoxes[numchart].Figures.GetFigure(FN_Description);
      if(cftext == null) {
        cftext = new ChartFigureText(FN_Description, desc, this._chartmanager.Style.ScaleFont, this._chartmanager.Style.ScaleForeColor, 3, 3);
        this._chartmanager.ChartBoxes[numchart].Figures.Add(cftext);
      } else {
        cftext.Text = desc;
      }
      this.Invalidate();
    }
    #endregion

    #region private void UpdateSymbolInfo()
    private void UpdateSymbolInfo() {
      if (this.ChartManager.Bars == null)
        return;

      Bar bar = this.ChartManager.Bars.Current;
      int decDig = this.Symbol.DecimalDigits;

      string desc = this.Symbol.Name +
        ", " + this.ChartManager.Bars.TimeFrame.Name;
      if (bar.Open > 0) {
        desc += ", " +
          SymbolManager.ConvertToCurrencyString(bar.Open, decDig) + ", " +
          SymbolManager.ConvertToCurrencyString(bar.High, decDig) + ", " +
          SymbolManager.ConvertToCurrencyString(bar.Low, decDig) + ", " +
          SymbolManager.ConvertToCurrencyString(bar.Close, decDig);
      }
        
      this.SetChartDescription(0, desc);
    }
    #endregion

    #region private void SetBidAskLine(float bid, float ask)
    private void SetBidAskLine(float bid, float ask) {
      ChartFigureBidAsk figure = this._chartmanager.ChartBoxes[0].Figures.GetFigure(ChartFigureBidAsk.FigureName) as ChartFigureBidAsk;
      if (float.IsNaN(bid) || bid == 0) {
        _chartmanager.ChartBoxes[0].Figures.Remove(figure);
      } else {
        if (figure == null) {
          figure = new ChartFigureBidAsk();
          this._chartmanager.ChartBoxes[0].Figures.Add(figure);
        }
        figure.SetBidAsk(bid, ask);
      }
      this.UpdateSymbolInfo();
      this.Invalidate();
    }
    #endregion

    #region private void UpdateContextMenu()
    private void UpdateContextMenu() {
      foreach(ToolStripMenuItem mnitf in _mniCTimeFrame.DropDownItems) {
        mnitf.Checked = GordagoMain.MainForm.ChartCheckTimeFrameNameFromItem(mnitf.Name, this._chartmanager.Bars.TimeFrame.Second);
      }

      _mniCBar.Checked = _mniCCandle.Checked = _mniCLine.Checked = false;
      switch(_chartmanager.BarType) {
        case ChartFigureBarType.Bar:
          _mniCBar.Checked = true;
          break;
        case ChartFigureBarType.CandleStick:
          _mniCCandle.Checked = true;
          break;
        case ChartFigureBarType.Line:
          _mniCLine.Checked = true;
          break;
      }

      this._mniCZoomIn.Enabled = ChartManager.IsZoomIn();
      this._mniCZoomOut.Enabled = ChartManager.IsZoomOut();
      this._mniCGrid.Checked = ChartManager.GridVisible;
      this._mniCPeriodSeparators.Checked = ChartManager.PeriodSeparators;
    }
    #endregion

    #region public void AddIndicator(int numChartBox, string name, Indicator indicator, Parameter[] parameters, int shift)
    public void AddIndicator(int numChartBox, string name, Indicator indicator, Parameter[] parameters, int shift) {
      ChartFigureIndicator cfindic = new ChartFigureIndicator(name, indicator, parameters);
      cfindic.Shift = shift;

      if (numChartBox > 0)
        SetChartDescription(numChartBox, indicator.Name);

      _chartmanager.ChartBoxes[numChartBox].Figures.Add(cfindic);
      this._chartmanager.Invalidate();
    }
    #endregion

    #region ChartManager Events

    #region private void _chartManager_BarTypeChanged(object sender, EventArgs e)
    private void _chartManager_BarTypeChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
      this.UpdateContextMenu();
    }
    #endregion

    #region private void _chartmanager_ChartShiftChanged(object sender, EventArgs e)
    private void _chartmanager_ChartShiftChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
    }
    #endregion

    #region private void _chartManager_DragDrop(object sender, DragEventArgs e)
    private void _chartManager_DragDrop(object sender, DragEventArgs drgevent) {
      if(drgevent.Data.GetDataPresent(typeof(Gordago.Strategy.FIndicator.IndicatorGUI))) {
        Gordago.Strategy.FIndicator.IndicatorGUI indic = (Gordago.Strategy.FIndicator.IndicatorGUI)drgevent.Data.GetData(typeof(Gordago.Strategy.FIndicator.IndicatorGUI));
        Random rnd = new Random();
        string name = "__indicator_" + indic.Indicator.Name + rnd.Next(1, 1000);
        int numchartbox = _chartmanager.GetIndexChartBox(this.PointToClient(new Point(drgevent.X, drgevent.Y)));

        if(indic.Indicator.IsSeparateWindow) {
          if(numchartbox >= this._chartmanager.ChartBoxes.Length) {
            numchartbox = _chartmanager.CreateChartBox();
          } else if(numchartbox > 0) {
            bool lfind = false;
            ChartFigure[] figures = this.ChartManager.ChartBoxes[numchartbox].Figures.GetFigures(typeof(ChartFigureIndicator));
            int cnt = 0;
            foreach(ChartFigure figure in figures) {
              if(figure is ChartFigureIndicator) {
                cnt++;
                ChartFigureIndicator cfindicf = figure as ChartFigureIndicator;
                if(cfindicf.Indicator.Name == indic.Indicator.Name) {
                  lfind = true;
                }
              }
            }
            if(!lfind && cnt > 0)
              numchartbox = _chartmanager.CreateChartBox();
          } else
            numchartbox = _chartmanager.CreateChartBox();
        }

        this.AddIndicator(numchartbox, name, indic.Indicator, indic.GetParameters(), indic.GetShift());

        _chartmanager.Focus();
        GordagoMain.MainForm.Explorer.ClearIndicatorProperty();
      }
    }
    #endregion

    #region private void _chartManager_DragOver(object sender, DragEventArgs e)
    private void _chartManager_DragOver(object sender, DragEventArgs drgevent) {
      if(drgevent.Data.GetDataPresent(typeof(Gordago.Strategy.FIndicator.IndicatorGUI))) {
        drgevent.Effect = DragDropEffects.Move;
      } else
        drgevent.Effect = DragDropEffects.None;
    }
    #endregion

    #region private void _chartManager_GridVisibleChanged(object sender, EventArgs e)
    private void _chartManager_GridVisibleChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
      this.UpdateContextMenu();
    }
    #endregion

    #region private void _chartmanager_PeriodSeparatorsChanged(object sender, EventArgs e)
    private void _chartmanager_PeriodSeparatorsChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
      this.UpdateContextMenu();
    }
    #endregion

    #region private void _chartManager_PositionChanged(object sender, EventArgs e)
    private void _chartManager_PositionChanged(object sender, EventArgs e) {

    }
    #endregion

    #region private void _chartManager_SelectedFigureChanged(object sender, EventArgs e)
    private void _chartManager_SelectedFigureChanged(object sender, EventArgs e) {
      if(this._chartmanager.SelectedFigure == null) {
        GordagoMain.MainForm.Explorer.ClearIndicatorProperty();
        return;
      }
      if(this._chartmanager.SelectedFigure is ChartFigureIndicator ||
        this._chartmanager.SelectedFigure is ChartObject) {

        this.RefreshPropertyPanel();
      }
    }
    #endregion

    #region private void RefreshPropertyPanel()
    private void RefreshPropertyPanel() {
      if(this._chartmanager.SelectedFigure == null) {
        GordagoMain.MainForm.Explorer.ClearIndicatorProperty();
        return;
      }
      if(this._chartmanager.SelectedFigure is ChartFigureIndicator) {
        ChartFigureIndicator cfindic = this._chartmanager.SelectedFigure as ChartFigureIndicator;
        IndicatorGUI indgui = new IndicatorGUI(cfindic.Indicator, cfindic.Parameters, this);
        GordagoMain.MainForm.Explorer.ViewIndicatorProperty(indgui, cfindic.Parameters, cfindic);
      } else {
        GordagoMain.MainForm.Explorer.ViewChartObjectProperty(this._chartmanager.SelectedFigure as ChartObject, this);
      }
    }
    #endregion

    #region private void _chartManager_SelectedFigureMouseChanged(object sender, EventArgs e)
    private void _chartManager_SelectedFigureMouseChanged(object sender, EventArgs e) {
      if(this._chartmanager.SelectedFigureMouse == null) {
        this._tooltip.RemoveAll();
        return;
      }
      string tt = this._chartmanager.SelectedFigureMouse.ToolTipText;
      this._tooltip.SetToolTip(this._chartmanager, tt);
    }
    #endregion

    #region private void _chartManager_TimeFrameChanged(object sender, EventArgs e)
    private void _chartManager_TimeFrameChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
      this.UpdateContextMenu();
      this.UpdateText();
      //if(this.ChartAutoScroll)
      //  this.ChartManager.SetPositionToEnd();
    }
    #endregion

    #region private void _chartManager_ZoomChanged(object sender, EventArgs e)
    private void _chartManager_ZoomChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
      this.UpdateContextMenu();
      if(this.ChartAutoScroll)
        this.ChartManager.SetPositionToEnd();
    }
    #endregion

    #region private void _chartmanager_MouseTypeChanged(object sender, EventArgs e)
    private void _chartmanager_MouseTypeChanged(object sender, EventArgs e) {
      this.UpdateMainFormComponents();
    }
    #endregion

    #region private void _chartmanager_Click(object sender, EventArgs e)
    private void _chartmanager_Click(object sender, EventArgs e) {
      GordagoMain.MainForm.Explorer.ViewChartPanelProperty(null, this);
    }
    #endregion

    #endregion

    #region public void RefreshStyle()
    public void RefreshStyle() {
      ChartStyle cs = GordagoMain.ChartStyleManager.GetDefaultStyle();
      this._chartmanager.SetStyle(cs);
      this.BackColor = cs.BackColor;
      int cntbox = this._chartmanager.ChartBoxes.Length;
      for(int i = 0; i < cntbox; i++) {
        for (int index=0;index<_chartmanager.ChartBoxes[i].Figures.Count;index++){
          ChartFigure figure = _chartmanager.ChartBoxes[i].Figures[index];
          if(figure is ChartFigureProgressBar) {
            ChartFigureProgressBar pgsfigure = figure as ChartFigureProgressBar;
            pgsfigure.BackColor = cs.BackColor;
            pgsfigure.BorderColor = cs.BorderColor;
            pgsfigure.ProgressColor = cs.BarColor;
          } else if(figure is ChartFigureText) {
            ChartFigureText txtfigure = figure as ChartFigureText;
            txtfigure.ForeColor = cs.ScaleForeColor;
          }
        }
      }
      this.Invalidate();
    }
    #endregion

    #region public void SetStrategyReport(TestReport report)
    public void SetStrategyReport(TestReport report) {
      ChartFigureCloseTrades figure = this._chartmanager.ChartBoxes[0].Figures.GetFigure(FN_CloseTradesTester) as ChartFigureCloseTrades;
      if(figure == null) {
        figure = new ChartFigureCloseTrades(FN_CloseTradesTester, report.ClosedTrades);
        this._chartmanager.ChartBoxes[0].Figures.Add(figure);
      } else {
        figure.SetCloseTrades(report.ClosedTrades);
      }
      if (report.ClosedTrades.Count > 0) {
        //int barIndex = this.ChartManager.Bars.GetBarIndex(report.ClosedTrades[0].OpenTime);
        this.ChartManager.SetPosition(report.ClosedTrades[0].OpenTime);
      }
      this.Refresh();
    }
    #endregion

    #region Context Menu Events

    private ChartFigureIndicator _selectInMenuIndicator = null;

    #region private void _contextMenu_Opening(object sender, CancelEventArgs e)
    private void _contextMenu_Opening(object sender, CancelEventArgs e) {
      bool isind = false, isobject = false;
      
      this._mnsepCRemIndic.Visible = 
        this._mniCRemoveAllIndicators.Visible = 
        this._mniCRemoveIndicator.Visible = 
        this._mniCRemoveAllObjects.Visible = 
        false;

      ChartFigureIndicator indic = null;
      if(this._chartmanager.SelectedFigureMouse != null &&
        this._chartmanager.SelectedFigureMouse is ChartFigureIndicator) {
        indic = this.ChartManager.SelectedFigureMouse as ChartFigureIndicator;
      } else if(this._chartmanager.SelectedFigure != null &&
        this._chartmanager.SelectedFigure is ChartFigureIndicator) {
        indic = this.ChartManager.SelectedFigure as ChartFigureIndicator;
      }
      
      if(indic != null) {
        this._mniCRemoveIndicator.Text = Language.Dictionary.GetString(32, 11, "Remove Indicator:") + " " + indic.Indicator.Name;
        this._mniCRemoveIndicator.Visible = true;
      }
      _selectInMenuIndicator =  indic;

      this._mniCRemoveAllIndicators.Text = Language.Dictionary.GetString(32, 12, "Remove all indicators");

      for(int i = 0; i < this._chartmanager.ChartBoxes.Length; i++) {
        for(int index = 0; index < _chartmanager.ChartBoxes[i].Figures.Count; index++) {
          ChartFigure figure = _chartmanager.ChartBoxes[i].Figures[index];
          if(figure is ChartFigureIndicator) {
            isind = true;
          } else if(figure is ChartObject) {
            isobject = true;
          }
        }
      }

      if(isind) {
        this._mniCRemoveAllIndicators.Visible =
          this._mnsepCRemIndic.Visible = true;
      }

      if(isobject) {
        this._mniCRemoveAllObjects.Visible = true;
      }
      _templates.Refresh(this._mniTemplate);
    }
    #endregion

    #region private void _mniCTF_Click(object sender, EventArgs e)
    private void _mniCTF_Click(object sender, EventArgs e) {
      GordagoMain.MainForm._mniCTF_Click(sender, e);
    }
    #endregion

    #region private void _mniCCandle_Click(object sender, EventArgs e)
    private void _mniCCandle_Click(object sender, EventArgs e) {
      this._chartmanager.SetBarType(ChartFigureBarType.CandleStick);
    }
    #endregion

    #region private void _mniCBar_Click(object sender, EventArgs e)
    private void _mniCBar_Click(object sender, EventArgs e) {
      this._chartmanager.SetBarType(ChartFigureBarType.Bar);
    }
    #endregion

    #region private void _mniCLine_Click(object sender, EventArgs e)
    private void _mniCLine_Click(object sender, EventArgs e) {
      this._chartmanager.SetBarType(ChartFigureBarType.Line);
    }
    #endregion

    #region private void _mniCGrid_Click(object sender, EventArgs e)
    private void _mniCGrid_Click(object sender, EventArgs e) {
      this._chartmanager.SetGridVisible(!this._chartmanager.GridVisible);
    }
    #endregion

    #region private void _mniCPeriodSeparators_Click(object sender, EventArgs e)
    private void _mniCPeriodSeparators_Click(object sender, EventArgs e) {
      this.ChartManager.SetPeriodSeparators(!this._chartmanager.PeriodSeparators);
    }
    #endregion

    #region private void _mniCZoomIn_Click(object sender, EventArgs e)
    private void _mniCZoomIn_Click(object sender, EventArgs e) {
      this._chartmanager.SetZoomIn();
    }
    #endregion

    #region private void _mniCZoomOut_Click(object sender, EventArgs e)
    private void _mniCZoomOut_Click(object sender, EventArgs e) {
      this._chartmanager.SetZoomOut();
    }
    #endregion

    #region private void _mniCRemoveIndicator_Click(object sender, EventArgs e)
    private void _mniCRemoveIndicator_Click(object sender, EventArgs e) {
      if (_selectInMenuIndicator != null) {
        _selectInMenuIndicator.ChartBox.Figures.Remove(_selectInMenuIndicator);
      }
    }
    #endregion

    #region private void _mniCRemoveAllIndicators_Click(object sender, EventArgs e)
    private void _mniCRemoveAllIndicators_Click(object sender, EventArgs e) {
      this.DeleteAllIndicator();
    }
    #endregion

    #region private void _mniCRemoveAllObjects_Click(object sender, EventArgs e)
    private void _mniCRemoveAllObjects_Click(object sender, EventArgs e) {
      this.DeleteAllObject();
    }
    #endregion
    #endregion

    #region private void DeleteAllObject()
    private void DeleteAllObject() {
      for(int i = 0; i < this._chartmanager.ChartBoxes.Length; i++) {
        for(int index = 0; index < _chartmanager.ChartBoxes[i].Figures.Count; index++) {
          ChartFigure figure = _chartmanager.ChartBoxes[i].Figures[index];
          if(figure is ChartObject) {
            _chartmanager.ChartBoxes[i].Figures.Remove(figure);
          }
        }
      }
      this.Invalidate();
    }
    #endregion

    #region private void _mniTSaveTemplate_Click(object sender, EventArgs e)
    private void _mniTSaveTemplate_Click(object sender, EventArgs e) {
      _templates.Save();
    }
    #endregion

    #region private void _mniTLoadTemplate_Click(object sender, EventArgs e)
    private void _mniTLoadTemplate_Click(object sender, EventArgs e) {
      this._templates.Load();
    }
    #endregion

    #region private void _mniCSaveReport_Click(object sender, EventArgs e)
    private void _mniCSaveReport_Click(object sender, EventArgs e) {
      this.SaveAsReport();
    }
    #endregion

    #region private void _mniC_Click(object sender, EventArgs e)
    private void _mniC_Click(object sender, EventArgs e) {
      SaveAsPicture();
    }
    #endregion

    #region public void SaveAsPicture()
    public void SaveAsPicture() {
      string reportpath = Application.StartupPath + "\\reports";
      Cursit.Utils.FileEngine.CheckDir(reportpath + "\\txt.txt");

      string filename = "Chart_" + this.Symbol.Name + "_" +
        DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" +
        DateTime.Now.Day.ToString() + "_" +
        DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" +
        DateTime.Now.Second.ToString();
        //+ ".gif";

      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "GIF (*.gif)|*.gif|PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp|TIFF (*.tiff)|*.tiff";

      string path = Config.Users["PathReport", reportpath];
      sfd.InitialDirectory = path;
      sfd.FileName = path + "\\" + filename;
      if (sfd.ShowDialog() != DialogResult.OK)
        return;

      Config.Users["PathReport"].SetValue(Cursit.Utils.FileEngine.GetDirectory(sfd.FileName));

      string fn = sfd.FileName;
      try {
        if (File.Exists(fn))
          File.Delete(fn);

        
        string ext = FileEngine .GetFileExt(fn).ToUpper();
        ImageFormat imgFormat = ImageFormat.Gif;
        switch (ext) {
          case "JPG":
            imgFormat = ImageFormat.Jpeg;
            break;
          case "BMP":
            imgFormat = ImageFormat.Bmp;
            break;
          case "PNG":
            imgFormat = ImageFormat.Png;
            break;
          case "TIFF":
            imgFormat = ImageFormat.Tiff;
            break;
        }

        this.ChartManager.SaveBitmap(sfd.FileName, imgFormat);
      } catch { }
    }
    #endregion

    #region public void ClearTemplate()
    public void ClearTemplate() {
      for (int i = this._chartmanager.ChartBoxes.Length - 1; i > 0; i--) {
        this._chartmanager.DeleteChartBox(i);
      }
      for (int i = 0; i < this._chartmanager.ChartBoxes[0].Figures.Count; i++) {
        ChartFigure figure = this._chartmanager.ChartBoxes[0].Figures[i];
        if (figure is ChartFigureIndicator || figure is ChartObject) {
          this._chartmanager.ChartBoxes[0].Figures.Remove(figure);
          i--;
        }
      }
    }
    #endregion

    #region public void SaveAsReport()
    public void SaveAsReport() {
      string temppath = Application.StartupPath + "\\temp";
      Cursit.Utils.FileEngine.CheckDir(temppath + "\\txt.txt");

      string prefix = "Chart_" + this.Symbol.Name;

      string tempFileName = temppath + "\\" + prefix + "_" +
        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
        DateTime.Now.Second.ToString() + ".html";

      this.ChartManager.SaveValuesReport(tempFileName);

      ReportBrowserForm rbf = new ReportBrowserForm(tempFileName, prefix);
      rbf.ShowDialog();

      try {
        if (System.IO.File.Exists(tempFileName))
          System.IO.File.Delete(tempFileName);
      } catch { }
    }
    #endregion

    #region private void _mniCTrade_Click(object sender, EventArgs e)
    private void _mniCTrade_Click(object sender, EventArgs e) {
      this._mniCTrade.Checked = !this._mniCTrade.Checked;
      this.UpdateTradePanel();
    }
    #endregion

    #region private void _mniCSymbolsPanel_Click(object sender, EventArgs e)
    private void _mniCSymbolsPanel_Click(object sender, EventArgs e) {
      this._mniCSymbolsPanel.Checked = !this._mniCSymbolsPanel.Checked;
      this.UpdateSymbolsPanel();
    }
    #endregion

    #region private void _mniCTicksChart_Click(object sender, EventArgs e)
    private void _mniCTicksChart_Click(object sender, EventArgs e) {
      _mniCTicksChart.Checked = !this._mniCTicksChart.Checked;
      this.UpdateTicksChartPanel();
    }
    #endregion

    #region private void UpdateTradePanel()
    private void UpdateTradePanel() {
      HandOperateChartPanel hocp = this.ChartManager.ChartPanels[typeof(HandOperateChartPanel)] as HandOperateChartPanel;
      if (this._mniCTrade.Checked) {
        if (hocp == null) {
          hocp = new HandOperateChartPanel();
          this.ChartManager.ChartPanels.Add(hocp);
        } 
      } else  {
        if (hocp != null) {
          hocp.Close();
          hocp = null;
        }
      }
      _mniCTrade.Checked = hocp != null;
    }
    #endregion

    #region private void UpdateSymbolsPanel()
    private void UpdateSymbolsPanel() {
      SymbolsChartPanel scp = this.ChartManager.ChartPanels[typeof(SymbolsChartPanel)] as SymbolsChartPanel;
      if (this._mniCSymbolsPanel.Checked){
        if (scp == null) {
          scp = new SymbolsChartPanel();
          this.ChartManager.ChartPanels.Add(scp);
        }
      }else {
        if (scp != null) {
          scp.Close();
          scp = null;
        }
      }
      _mniCSymbolsPanel.Checked = scp != null;
    }
    #endregion

    #region private void UpdateTicksChartPanel()
    private void UpdateTicksChartPanel() {
      TickChartPanel tcp = this.ChartManager.ChartPanels[typeof(TickChartPanel)] as TickChartPanel;
      if (this._mniCTicksChart.Checked) {
        if (tcp == null) {
          tcp = new TickChartPanel();
          tcp.Symbol = this.Symbol;
          tcp.ChartTicksManager.Style = this.ChartManager.Style;
          this.ChartManager.ChartPanels.Add(tcp);
        }
      } else {
        if (tcp != null) {
          tcp.Close();
          tcp = null;
        }
      }
      _mniCTicksChart.Checked = tcp != null;
    }
    #endregion

    #region private void _handPanel_PanelClosed(object sender, EventArgs e)
    private void _handPanel_PanelClosed(object sender, EventArgs e) {
      _mniCTrade.Checked = false;
    }
    #endregion

    #region private void SymbolsChartPanel_PanelClosed(object sender, EventArgs e)
    private void SymbolsChartPanel_PanelClosed(object sender, EventArgs e) {
      _mniCSymbolsPanel.Checked = false;
    }
    #endregion

    #region private void TicksChartPanel_PanelClosed(object sender, EventArgs e)
    private void TicksChartPanel_PanelClosed(object sender, EventArgs e) {
      _mniCTicksChart.Checked = false;
    }
    #endregion

    #region private void _chartPanels_PanelAdded(object sender, ChartPanelEventArgs cpe)
    private void _chartPanels_PanelAdded(object sender, ChartPanelEventArgs cpe) {
      if (cpe.ChartPanel is HandOperateChartPanel) {
        _mniCTrade.Checked = true;
        HandOperateChartPanel hocp = cpe.ChartPanel as HandOperateChartPanel;
        hocp.SetSymbol(_symbol);
        hocp.PanelClosed += new EventHandler(this._handPanel_PanelClosed);
      } else if (cpe.ChartPanel is SymbolsChartPanel) {
        _mniCSymbolsPanel.Checked = true;
        SymbolsChartPanel scp = cpe.ChartPanel as SymbolsChartPanel;
        scp.PanelClosed += new EventHandler(this.SymbolsChartPanel_PanelClosed);
      } else if (cpe.ChartPanel is TickChartPanel) {
        _mniCTicksChart.Checked = true;
        TickChartPanel tcp = cpe.ChartPanel as TickChartPanel;
        tcp.Symbol = this.Symbol;
        tcp.ChartTicksManager.Style = this.ChartManager.Style;
        tcp.PanelClosed += new EventHandler(this.TicksChartPanel_PanelClosed);
      }
      cpe.ChartPanel.PropertyView += new EventHandler(this.ChartPanel_PropertyView);
    }
    #endregion

    #region private void ChartPanel_PropertyView(object sender, EventArgs e)
    private void ChartPanel_PropertyView(object sender, EventArgs e) {
      GordagoMain.MainForm.Explorer.ViewChartPanelProperty(sender as ChartPanel, this);
    }
    #endregion

    #region internal void LoadSettingsCompleate()
    internal void LoadSettingsCompleate() {
      this.UpdateTradePanel();
      this.UpdateSymbolsPanel();
    }
    #endregion

    #region IBrokerEvents Members

    #region public void BrokerConnectionStatusChanged(BrokerConnectionStatus status)
    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      switch (status) {
        case BrokerConnectionStatus.Online:
          this.ChartManager.Analyzer.Cache.Clear();
          IOnlineRate onlineRate = BCM.Broker.OnlineRates.GetOnlineRate(_symbol.Name);
          this.SetBidAskLine(onlineRate.SellRate, onlineRate.BuyRate);
          this.UpdateTrades();
          this.UpdateOrders();
          break;
        case BrokerConnectionStatus.Offline:
        case BrokerConnectionStatus.LoadingData:
        case BrokerConnectionStatus.WaitingForConnection:
          this.SetBidAskLine(float.NaN, float.NaN);
          break;
      }
      HandOperateChartPanel hocp = this.ChartManager.ChartPanels[typeof(HandOperateChartPanel)] as HandOperateChartPanel;
      if (hocp != null)
        hocp.BrokerConnectionStatusChanged(status);

      SymbolsChartPanel scp = this.ChartManager.ChartPanels[typeof(SymbolsChartPanel)] as SymbolsChartPanel;
      if (scp != null) {
        scp.BrokerConnectionStatusChanged(status);
      }

      UpdateCloseTrades();

      if (_chartmanager.Bars != null)
        this._chartmanager.SetTimeFrame(_chartmanager.Bars.TimeFrame);
      this.Invalidate();
    }
    #endregion

    #region public void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {

    }
    #endregion

    #region public void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      if (be.Order.OnlineRate.Symbol != this._symbol)
        return;
      this.UpdateOrders();
    }
    #endregion

    #region public void BrokerTradesChanged(BrokerTradesEventArgs be)
    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      if (be.Trade.OnlineRate.Symbol != this._symbol)
        return;
      this.UpdateTrades();
      this.UpdateOrders();
      this.UpdateCloseTrades();
    }
    #endregion

    #region public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {

      if (this.IsVirtualBroker) {
        if (DateTime.Now.Ticks - _savedUpdateOnlineRateTimeForChart < 1000000L)
          return;

        _savedUpdateOnlineRateTimeForChart = DateTime.Now.Ticks;
      }

      bool thisSymbol = false;
      if (be.OnlineRate.Symbol.Name == this._symbol.Name) {
        this.SetBidAskLine(be.OnlineRate.SellRate, be.OnlineRate.BuyRate);
        if (this.ChartAutoScroll)
          this.ChartManager.SetPositionToEnd();
        thisSymbol = true;
      }

      if (this.IsVirtualBroker) {
        if (DateTime.Now.Ticks - _savedUpdateOnlineRateTimeForAll < 5000000L)
          return;
        _savedUpdateOnlineRateTimeForAll = DateTime.Now.Ticks;
      }

      for (int i = 0; i < this.ChartManager.ChartPanels.Count; i++) {
        if (this.ChartManager.ChartPanels[i] is HandOperateChartPanel && thisSymbol) {
          HandOperateChartPanel hocp = this.ChartManager.ChartPanels[i] as HandOperateChartPanel;
          hocp.BrokerOnlineRatesChanged(be);
        } else if (this.ChartManager.ChartPanels[i] is SymbolsChartPanel) {
          SymbolsChartPanel scp = this.ChartManager.ChartPanels[i] as SymbolsChartPanel;
          scp.BrokerOnlineRatesChanged(be);
        } else if (this.ChartManager.ChartPanels[i] is TickChartPanel && thisSymbol) {
          TickChartPanel tcp = this.ChartManager.ChartPanels[i] as TickChartPanel;
          tcp.ChartTicksManager.Invalidate();
        }
      }
    }
    #endregion

    #region public void BrokerCommandStarting(BrokerCommand command)
    public void BrokerCommandStarting(BrokerCommand command) {
      for (int i = 0; i < this.ChartManager.ChartPanels.Count; i++) {
        if (this.ChartManager.ChartPanels[i] is HandOperateChartPanel) {
          HandOperateChartPanel hocp = this.ChartManager.ChartPanels[i] as HandOperateChartPanel;
          hocp.BrokerCommandStarting(command);
        }
      }
    }
    #endregion

    #region public void BrokerCommandStopping(BrokerCommand command, BrokerResult result)
    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      for (int i = 0; i < this.ChartManager.ChartPanels.Count; i++) {
        if (this.ChartManager.ChartPanels[i] is HandOperateChartPanel) {
          HandOperateChartPanel hocp = this.ChartManager.ChartPanels[i] as HandOperateChartPanel;
          hocp.BrokerCommandStopping(command, result);
        }
      }
    }
    #endregion
    #endregion

    #region IBrokerSymbolsEvents Members

    public void BrokerUpdateSymbolStarting(UpdateSymbolEventArgs se) {
    }

    public void BrokerUpdateSymbolStopping(UpdateSymbolEventArgs se) {
    }

    public void BrokerUpdateSymbolDownloadPart(UpdateSymbolEventArgs se) {
      if (this._symbol != se.Symbol)
        return;

      if (this.ChartAutoScroll)
        this.ChartManager.SetPositionToEnd();
    }

    #endregion

    #region private void CreateTMProcess()
    private void CreateTMProcess() {
      if (_progressBar != null)
        return;

      _progressBar = new ChartFigureProgressBar(FN_ProgressBar);
      _progressBar.Visible = false;
      this._chartmanager.ChartBoxes[0].Figures.Add(_progressBar);

      Font bfont = new Font(this._chartmanager.Style.ScaleFont.FontFamily, 20, FontStyle.Bold);
      ChartFigureText bcftext = new ChartFigureText(FN_PleaseWait,
        Dictionary.GetString(32, 21, "Пожалуйста, подождите...\nИдет кэширование данных"),
        bfont, this._chartmanager.Style.ScaleForeColor, 0, 0);
      bcftext.TextAlignment = ContentAlignment.MiddleCenter;
      this._chartmanager.ChartBoxes[0].Figures.Add(bcftext);
      this.RefreshStyle();
    }
    #endregion

    #region private void UpdateTMProccess(int current, int total)
    private void UpdateTMProccess(int current, int total) {
      this.CreateTMProcess();

      if (DateTime.Now.Ticks - _savedTMProcessTime.Ticks < 5000000L) 
        return;

      _savedTMProcessTime = DateTime.Now;

      _progressBar.Visible = true;
      _progressBar.Total = total;
      _progressBar.Current = current;
      this._chartmanager.Invalidate();
    }
    #endregion

    #region private void DeleteTMProccess()
    private void DeleteTMProccess() {
      if (_progressBar == null)
        return;
      _progressBar = null;
      this._chartmanager.ChartBoxes[0].Figures.RemoveAt(FN_ProgressBar);
      this._chartmanager.ChartBoxes[0].Figures.RemoveAt(FN_PleaseWait);
      if (this.TmpPostion != 0) {
        this.ChartManager.SetPosition(this.TmpPostion);
        this.TmpPostion = 0;
      } else {
        this.ChartManager.SetPositionToEnd();
      }
      this.InitializeBars();
    }
    #endregion

    #region private void tmanager_DataCachingChanged(object sender, TickManagerEventArgs tme)
    private void tmanager_DataCachingChanged(object sender, TickManagerEventArgs tme) {
      if (this.InvokeRequired) {
        this.Invoke(new TickManagerEventHandler(this.tmanager_DataCachingChanged), sender, tme);
      } else {
        switch (tme.Status) {
          case TickManagerStatus.DataCachingStarting:
            this.CreateTMProcess();
            break;
          case TickManagerStatus.DataCaching:
            this.UpdateTMProccess(tme.Current, tme.Total);
            break;
          case TickManagerStatus.Default:
            this.DeleteTMProccess();
            break;
        }
      }
    }
    #endregion
  }
}