/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using Gordago.Analysis.Chart;
using System;
using System.Windows.Forms;
using Gordago.Strategy;
using Gordago.API;
using Gordago.Stock;
using Cursit.Applic.AConfig;
using Gordago.WebUpdate;
using System.Drawing;
using Language;
using Gordago.Stock.Loader;
using System.Collections.Generic;
using Cursit.Utils;
using System.Reflection;
using Gordago.PlugIn;
using Gordago.Analysis;
using System.IO;

using Gordago.Windows.Forms;
using Cursit.Table;
using Gordago.API.VirtualForex;
using System.Diagnostics;
using Cursit;
#endregion

namespace Gordago {
  partial class MainForm : Form, IMainForm, IBrokerEvents, IBrokerSymbolsEvents {

    public event EventHandler ChartCreating;
    public event EventHandler ChartActivate;

    private StrategyManager _sewfs;
    private ViewOpenWindows _owsPanel;

    private int _cst;
    private string _captiontext;
    private bool _isfirststart = true;
    private string[] _laststrategyfiles;
    private bool _statusttpanel = true;

    private ConfigValue _cfgval;

    private string _prodVersion;

    private List<PlugInModule> _plugins;
    private List<Type> _brokers;

    private ChartObjectManager _chartObjectManger;
    private ChartPanelManager _chartPanelManager;
    private MdiClient _mdiClient;

    private BrokerCommandManager _brokerCommandManager;
    private string _defaultAccountId = "";

    private BrokerConnectionStatus _savedConnStatus;

#if DEMO
    private bool _isVirtualBroker = false;
#endif
    
    private ToolStripItemPanel _tsiTerminal, _tsiSymbols, _tsiExplorer, _tsiTester;

    private DockManager _dockManager;

    public MainForm() {
      GordagoMain._mainform = this;
      _brokerCommandManager = new BrokerCommandManager(GordagoMain.SymbolEngine);

      this.ControlAdded += new ControlEventHandler(this.Controls_Added);

      this._owsPanel = new ViewOpenWindows();
      _owsPanel.Dock = DockStyle.Bottom;
      this.Controls.Add(_owsPanel);

      InitializeComponent();

      this.SuspendLayout();
      this._tspBottom.SuspendLayout();
      this._tspTop.SuspendLayout();
      this._tspLeft.SuspendLayout();
      this._tspRight.SuspendLayout();

      _tsiTester = new ToolStripItemPanel();
      _tsiExplorer = new ToolStripItemPanel();
      _tsiSymbols = new ToolStripItemPanel();
      _tsiTerminal = new ToolStripItemPanel();

      _tsiTerminal.Name = "_tsiTerminal";
      _tsiTerminal.Item = new Terminal();
      _tspBottom.Controls.Add(_tsiTerminal);

      _tsiTester.Name = "_tsiTester";
      _tsiTester.Item = new TesterPanel();
      _tspBottom.Controls.Add(_tsiTester);

      _tsiExplorer.Name = "_tsiExplorer";
      _tsiExplorer.Item = new ExplorerPanel();
      _tspRight.Controls.Add(_tsiExplorer);

      _tsiSymbols.Name = "_tsiSymbols";
      _tsiSymbols.Item = new SymbolsPanel();
      _tspLeft.Controls.Add(_tsiSymbols);

      _tsiTerminal.VisibleChanged += new EventHandler(_tsiTerminal_VisibleChanged);
      _tsiExplorer.VisibleChanged += new EventHandler(_tsiExplorer_VisibleChanged);
      _tsiTester.VisibleChanged += new EventHandler(_tsiTester_VisibleChanged);
      _tsiSymbols.VisibleChanged += new EventHandler(_tsiSymbols_VisibleChanged);

      this.ResumeLayout(false);
      this._tspBottom.ResumeLayout(false);
      this._tspTop.ResumeLayout(false);
      this._tspLeft.ResumeLayout(false);
      this._tspRight.ResumeLayout(false);

      #region over
      this._sewfs = new StrategyManager();
      UpdateLastOpenStrategyFileInMenu();
      SetLanguage(Config.Users["Language", "English"]);

      _cfgval = Config.Users["Form"]["MainForm"];

      this.StatusTTPanel = _cfgval["TTStatus", true];

      _cst = _cfgval["cst", 0] + 1;
      _cfgval["cst"].SetValue(_cst);

      string[] psa = Application.ProductVersion.Split('.');
      _prodVersion = psa[0] + "." + psa[1] + "." + psa[2];
      _captiontext = this.Text = "Gordago Forex Optimizer TT " + _prodVersion;

      this.Text = _captiontext + " [Offline]";
      #endregion

      this.ChartSetMenuEnabled(false);
      this._tsslblTerminalCommand.Text = "";

      this.InitializeMenuTimeFrame();
      this.InitializeChartObjectManager();
      this.LoadBrokers();
      this.LoadPlugIns();

      this.Tester.UpdateSymbolsList();

      _dockManager = new DockManager(this);
      _dockManager.RegisterItemPanel(_tsiSymbols);
      _dockManager.RegisterItemPanel(_tsiTester);
      _dockManager.RegisterItemPanel(_tsiExplorer);
      _dockManager.RegisterItemPanel(_tsiTerminal);
      MainFormManager.Load(_dockManager);
      _dockManager.Refresh();

      _tspBottom.Controls.Clear();
      _tspBottom.Join(_tsiTester, 0);
      _tspBottom.Join(_tsiTerminal, 1);

      string gsofile = Application.StartupPath + "\\gso.xml";
      if (!File.Exists(gsofile)) {
        this.WindowState = FormWindowState.Maximized;

        ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol("EURUSD");
        if (symbol != null) {
          this.ChartShowNewForm(symbol);
        }
      }

      this.InitializeDictionary();
    }

    #region private void AllSuspendLayout()
    private void AllSuspendLayout() {
      //this.SuspendLayout();
      //this._tspBottom.SuspendLayout();
      //this._tspTop.SuspendLayout();
      //this._tspLeft.SuspendLayout();
      //this._tspRight.SuspendLayout();
    }
    #endregion

    #region private void AllResumeLayout()
    private void AllResumeLayout() {
      //this.ResumeLayout(false);
      //this._tspBottom.ResumeLayout(false);
      //this._tspTop.ResumeLayout(false);
      //this._tspLeft.ResumeLayout(false);
      //this._tspRight.ResumeLayout(false);
    }
    #endregion

    #region private void InitializeChartObjectManager()
    private void InitializeChartObjectManager() {
      _chartObjectManger = new ChartObjectManager();
      _chartObjectManger.TypeRegister += new EventHandler<TypeEventArgs>(this._chartObjectManager_Register);

      _chartObjectManger.Register(typeof(ChartObjectCrosshair));
      _chartObjectManger.Register(typeof(ChartObjectVerticalLine));
      _chartObjectManger.Register(typeof(ChartObjectHorizontalLine));
      _chartObjectManger.Register(typeof(ChartObjectTrendLine));
      _chartObjectManger.Register(typeof(ChartObjectCycleLines));
      _chartObjectManger.Register(typeof(ChartObjectFiboFan));
      _chartObjectManger.Register(typeof(ChartObjectFiboRetracement));
      _chartObjectManger.Register(typeof(ChartObjectFiboTimeZones));
      _chartObjectManger.Register(typeof(ChartObjectFiboExpansion));
      _chartObjectManger.Register(typeof(ChartObjectFiboArcs));

      _chartObjectManger.Register(typeof(ChartObjectLabel));
      _chartObjectManger.Register(typeof(ChartObjectRectagle));

      _chartPanelManager = new ChartPanelManager();

      _chartPanelManager.Register(typeof(HandOperateChartPanel));
      _chartPanelManager.Register(typeof(SymbolsChartPanel));
      _chartPanelManager.Register(typeof(TickChartPanel));
    }
    #endregion

    #region private void InitializeMenuTimeFrame()
    private void InitializeMenuTimeFrame() {
      for (int i = 0; i < TimeFrameManager.TimeFrames.Count; i++) {
        TimeFrame tf = TimeFrameManager.TimeFrames[i];
        ToolStripMenuItem mnitf = new ToolStripMenuItem();
        mnitf.Name = "_mniCTF_" + tf.Second.ToString();
        mnitf.ToolTipText = mnitf.Text = string.Format("{0} ({1} second)", tf.Name, tf.Second);
        mnitf.Click += new EventHandler(this._mniCTF_Click);
        _mniCTimeFrame.DropDownItems.Add(mnitf);

        ToolStripMenuItem mnbtf = new ToolStripMenuItem();
        mnbtf.Name = "_mnbCTF_" + tf.Second.ToString();
        mnbtf.ToolTipText = mnbtf.Text = string.Format("{0} ({1} second)", tf.Name, tf.Second);
        mnbtf.Click += new EventHandler(this._mniCTF_Click);
        _mnbCTimeFrameList.DropDownItems.Add(mnbtf);

        ToolStripButton tfbtn = new ToolStripButton();
        tfbtn.Name = "_mnbbtnCTF_" + tf.Second.ToString();
        tfbtn.Text = tf.Name;
        tfbtn.ToolTipText = string.Format("{0} ({1} second)", tf.Name, tf.Second);
        tfbtn.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tfbtn.Click += new EventHandler(this._mniCTF_Click);
        _tsTimeFrame.Items.Add(tfbtn);
      }
    }
    #endregion

    #region private void InitializeDictionary()
    private void InitializeDictionary() {

      _mniHHelp.Text = GordagoMain.Lang == "rus" ? "Помощь..." : "Help...";

      _mniFile.Text = Dictionary.GetString(2, 1);
      _mniFileNew.Text = Dictionary.GetString(2, 2);
      _mnbFileNew.ToolTipText = Dictionary.GetString(2, 2);
      _mniFileOpen.Text = Dictionary.GetString(2, 3);
      _mnbFileOpen.ToolTipText = Dictionary.GetString(2, 3);
      _mniFileSave.Text = Dictionary.GetString(2, 4);
      _mnbFileSave.ToolTipText = Dictionary.GetString(2, 4);
      _mniFileSaveAs.Text = Dictionary.GetString(2, 5);
      _mniFileClose.Text = Dictionary.GetString(2, 6);
      _mniExit.Text = Dictionary.GetString(2, 7);

      _mniConnect.Text =
        _mnbConnect.Text = _mnbConnect.ToolTipText = Language.Dictionary.GetString(31, 9, "Connect ...");

      _mniDisconnect.Text =
        _mnbDisconnect.Text = _mnbDisconnect.ToolTipText = Language.Dictionary.GetString(31, 10, "Disconnect");

      _mniFileExportMQL.Text = Dictionary.GetString(2, 9);

      _mniView.Text = Dictionary.GetString(3, 1);
      
      _tsiSymbols.Text = 
        _mniVSymbols.Text = 
        _mnbVSymbols.ToolTipText = Dictionary.GetString(3, 2);

      _tsiExplorer.Text =
        _mniVTools.Text = 
        _mnbVTools.ToolTipText = Dictionary.GetString(3, 3);
      
        _mniVControlPanel.Text = 
        _mnbVControlPanel.ToolTipText = Dictionary.GetString(3, 5);

      _tsiTerminal.Text = 
        _mniVTerminal.Text = 
        _mnbVTerminal.Text = 
        _mnbVTerminal.ToolTipText = Dictionary.GetString(3, 10, "Terminal");

      
      _tsiTester.Text  =
        _mniVControlPanel.Text = 
        _mnbVControlPanel.Text = 
        _mnbVControlPanel.ToolTipText = Dictionary.GetString(3, 11, "Tester");

      _mniChart.Text = Language.Dictionary.GetString(32, 1, "Chart");

      _mniCCandle.Text =
        _mnbCCandle.Text = _mnbCCandle.ToolTipText = Language.Dictionary.GetString(32, 2, "Candlestick");

      _mniCBar.Text =
        _mnbCBar.Text = _mnbCBar.ToolTipText = Language.Dictionary.GetString(32, 3, "Bar Chart");

      _mniCLine.Text =
        _mnbCLine.Text = _mnbCLine.ToolTipText = Language.Dictionary.GetString(32, 4, "Line Chart");

      _mniCAutoScroll.Text =
        _mnbCAutoScroll.Text = _mnbCAutoScroll.ToolTipText = Language.Dictionary.GetString(32, 5, "Auto Scroll");

      _mniCChartShift.Text =
        _mnbCChartShift.Text = _mnbCChartShift.ToolTipText = Language.Dictionary.GetString(32, 6, "Chart Shift");

      _mniCGrid.Text = Language.Dictionary.GetString(32, 7, "Grid");
      _mniCPeriodSeparators.Text = Dictionary.GetString(32, 34, "Period Separators");

      _mniCZoomIn.Text =
        _mnbCZoomIn.Text = _mnbCZoomIn.ToolTipText = Language.Dictionary.GetString(32, 8, "Zoom In");

      _mniCZoomOut.Text =
        _mnbCZoomOut.Text = _mnbCZoomOut.ToolTipText = Language.Dictionary.GetString(32, 9, "Zoom Out");

      _mniCSaveAsReport.Text = Dictionary.GetString(32, 28, "Save As Report...");
      _mniCSaveAsPicture.Text = Dictionary.GetString(32, 29, "Сохранить как рисунок...");

      _mniCTimeFrame.Text =
        _mnbCTimeFrameList.Text = _mnbCTimeFrameList.ToolTipText = Language.Dictionary.GetString(32, 10, "Time Frame");

      _mniService.Text = Dictionary.GetString(4, 1);
      _mniSUpdate.Text = Dictionary.GetString(4, 2);
      _mniSDownloadHistory.Text = Dictionary.GetString(7, 19);
      _mniSSettings.Text = Dictionary.GetString(26, 1);

      _mniWindow.Text = Dictionary.GetString(5, 1);
      _mniWCascade.Text = Dictionary.GetString(5, 2);
      _mniWHoriz.Text = Dictionary.GetString(5, 3);
      _mniWVert.Text = Dictionary.GetString(5, 4);


      _mniHelp.Text = Dictionary.GetString(6, 1);
      _mniHAbout.Text = Dictionary.GetString(6, 2);
      _mniHReg.Text = Dictionary.GetString(6, 3);
      _mniHAskAQuestion.Text = Dictionary.GetString(6, 4, "Ask a Question");


      _mniVToolbars.Text = Dictionary.GetString(3, 16, "Toolbars");
      _mniVTStandart.Text =
        _mncnStandart.Text = Dictionary.GetString(3, 12, "Standard");
      _mniVTCharts.Text =
        _mncnCharts.Text = Dictionary.GetString(3, 13, "Charts");
      _mniVTLineStudies.Text =
        _mncnLineStudies.Text = Dictionary.GetString(3, 14, "Line Studies");
      _mniVTTimeFrames.Text =
        _mncnTimeFrame.Text = Dictionary.GetString(3, 15, "Periodicity");

      _mnbCODefault.ToolTipText = Dictionary.GetString(32, 14, "Cursor");

      _mniTemplate.Text = Dictionary.GetString(32, 22, "Шаблон");
      _mniTSaveTemplate.Text = Dictionary.GetString(32, 23, "Сохранить шаблон ...");
      _mniTLoadTemplate.Text = Dictionary.GetString(32, 24, "Загрузить шаблон ...");
      _mniTRemoveTemplate.Text = Dictionary.GetString(32, 25, "Удалить шаблон");

      _mniVSPause.Text = _mnbVSPause.Text = _mnbVSPause.ToolTipText = Dictionary.GetString(35, 2, "Пауза");
      _mniVSSpeed.Text = _mnbVSSped.Text = _mnbVSSped.ToolTipText = Dictionary.GetString(35, 3, "Скорость");
      _mniBroker.Text = Dictionary.GetString(35, 1, "Сервер");
    }
    #endregion

    #region public string DefaultAccountId
    public string DefaultAccountId {
      get { return this._defaultAccountId; }
      set { this._defaultAccountId = value; }
    }
    #endregion

    #region public BrokerCommandManager BCM
    public BrokerCommandManager BCM {
      get { return this._brokerCommandManager; }
    }
    #endregion

    #region internal string ProductVersionSocr
    internal string ProductVersionSocr {
      get { return this._prodVersion; }
    }
    #endregion

    #region internal int CountStartApp
    internal int CountStartApp {
      get { return this._cst; }
    }
    #endregion

    #region internal StrategyManager StrategyManager
    internal StrategyManager StrategyManager {
      get { return this._sewfs; }
    }
    #endregion

    #region public ChartObjectManager ChartObjectManager
    public ChartObjectManager ChartObjectManager {
      get { return this._chartObjectManger; }
    }
    #endregion

    #region public ChartPanelManager ChartPanelManager
    public ChartPanelManager ChartPanelManager {
      get { return _chartPanelManager; }
    }
    #endregion

    #region public SymbolsPanel SymbolsPanel
    public SymbolsPanel SymbolsPanel {
      get { return _tsiSymbols.Item as SymbolsPanel; }
    }
    #endregion

    #region public TesterPanel Tester
    public TesterPanel Tester {
      get { return this._tsiTester.Item as TesterPanel; }
    }
    #endregion

    #region public TerminalPanel Terminal
    public Terminal Terminal {
      get { return this._tsiTerminal.Item as Terminal; }
    }
    #endregion

    #region public ExplorerPanel Explorer
    public ExplorerPanel Explorer {
      get { return this._tsiExplorer.Item as ExplorerPanel; }
    }
    #endregion

    #region public bool StatusTTPanel
    public bool StatusTTPanel{
      get { return _statusttpanel; }
      set {
        _statusttpanel = value;
        _cfgval["TTStatus"].SetValue(value);
      }
    }
    #endregion

    #region public bool ViewTerminal
    public bool ViewTerminal {
      get { return this._tsiTerminal.Visible; }
      set {
        this.AllSuspendLayout();
        bool evt = this._tsiTerminal.Visible != value;

        this._tsiTerminal.Visible = value;
        _mniVTerminal.Checked = value;
        _mnbVTerminal.Checked = value;
        if (evt)
          this.UpdateTTPanels();
        this.AllResumeLayout();
      }
    }
    #endregion

    #region public bool ViewTester
    public bool ViewTester {
      get { return this._tsiTester.Visible; }
      set {
        bool evt = this._tsiTerminal.Visible != value;

        this.AllSuspendLayout();
        this._tsiTester.Visible = value;
        _mniVControlPanel.Checked = value;
        _mnbVControlPanel.Checked = value;
        if (evt)
          this.UpdateTTPanels();
        this.AllResumeLayout();
      }
    }
    #endregion

    #region private void UpdateTTPanels()
    private void UpdateTTPanels() {
      if (this._tsiTerminal.Visible && this._tsiTester.Visible) {
        _tspBottom.Controls.Clear();
        _tspBottom.Join(_tsiTester, 0);
        _tspBottom.Join(_tsiTerminal, 1);
      }
    }
    #endregion

    #region public bool ViewIIBoxPanel
    public bool ViewIIBoxPanel {
      get { return this._tsiExplorer.Visible; }
      set {
        this.AllSuspendLayout();
        this._tsiExplorer.Visible = value;
        _mniVTools.Checked = value;
        _mnbVTools.Checked = value;
        this.AllResumeLayout();
      }
    }
    #endregion

    #region public bool ViewSymbolPanel
    public bool ViewSymbolPanel {
      get { return this._tsiSymbols.Visible; }
      set {
        this.AllSuspendLayout();
        this._tsiSymbols.Visible = value;
        this._mniVSymbols.Checked = value;
        this._mnbVSymbols.Checked = value;
        this.AllResumeLayout();
      }
    }
    #endregion

    #region public bool ViewStatusBar
    public bool ViewStatusBar {
      get { return this._statusstrip.Visible; }
      set {
        _statusstrip.Visible = value;
        _mniVStatusBar.Checked = value;
        _cfgval["ViewStatusBar"].SetValue(value);
      }
    }
    #endregion

    #region private void UpdateToolsPanelView()
    private void UpdateToolsPanelView() {
      this._mncnStandart.Checked = 
        this._mniVTStandart.Checked = this._tsStandart.Visible;

      this._mncnCharts.Checked =
        this._mniVTCharts.Checked = this._tsCharts.Visible;

      this._mncnTimeFrame.Checked = 
        this._mniVTTimeFrames.Checked = this._tsTimeFrame.Visible;

      _mncnLineStudies.Checked =
        this._mniVTLineStudies.Checked = this._tsChartObject.Visible;

      _mncnServer.Checked = _mniVTBroker.Checked = this._tsVirtualServer.Visible;
    }
    #endregion

    #region private void AutoCheckUpdateProccess()
    private void AutoCheckUpdateProccess() {
      UpdateForm uf = new UpdateForm();
      uf.CheckAutoUpdate(new UpdateEngineCheckUpdateResult(this.UE_CheckUpdateResult));
    }
    #endregion

    #region private void UE_CheckUpdateResult(UpdateEngineCheckUpdateType cutype)
    private void UE_CheckUpdateResult(UpdateEngineCheckUpdateType cutype) {
      if(GordagoMain.IsCloseProgram) return;
      try {
        if(!this.InvokeRequired) {
          if(cutype == UpdateEngineCheckUpdateType.Yes) {
            UpdateForm.IsNewUpdate = true;
            this.UpdateApplic();
          }
        } else {
          this.Invoke(new UpdateEngineCheckUpdateResult(this.UE_CheckUpdateResult), new object[] { cutype });
        }
      } catch { }
    }
    #endregion

    #region private void SetLanguage(string lang)
    private void SetLanguage(string lang) {
      Config.Users["Language"].SetValue(lang);
      switch(lang) {
        case "Russian":
          _mniVLEnglish.Checked = false;
          _mniVLRussian.Checked = true;
          break;
        case "English":
          _mniVLEnglish.Checked = true;
          _mniVLRussian.Checked = false;
          break;
      }
    }
    #endregion

    #region public void ShowDownloadHistory (Symbol symbol)
    public void ShowDownloadHistory (ISymbol symbol) {
      string sname = symbol == null ? "" : symbol.Name;
      ServerArhiveForm safrm = new ServerArhiveForm(sname);
      safrm.ShowDialog();
    }
    #endregion

    #region internal ProxySetting LoadProxySetting()
    internal ProxySetting LoadProxySetting() {
      ProxySetting ps = new ProxySetting();

      ps.ProxySettingFromIE = Config.Users["API"]["ProxyFromIE", true];
      ps.ProxyEnable = Config.Users["API"]["ProxyEnable", false];
      ps.ProxyServer = Config.Users["API"]["ProxyServer", ""];
      ps.ProxyPort = Config.Users["API"]["ProxyPort", 0];
      ps.ProxyUserName = Config.Users["API"]["ProxyUserName", ""];
      if(ps.ProxyEnable || ps.ProxyUserName.Length > 0) {
        byte[] proxypasskey = Config.Users["API"]["ProxyKey", new byte[] { }];
        ps.ProxyPass = Cursit.Utils.Password.CreatePass(ps.ProxyUserName, proxypasskey);
      }
      return ps;
    }
    #endregion

    #region internal void SaveProxySetting(ProxySetting proxy)
    internal void SaveProxySetting(ProxySetting proxy) {
      Config.Users["API"]["ProxyFromIE"].SetValue(proxy.ProxySettingFromIE);
      Config.Users["API"]["ProxyEnable"].SetValue(proxy.ProxyEnable);
      Config.Users["API"]["ProxyServer"].SetValue(proxy.ProxyServer);
      Config.Users["API"]["ProxyPort"].SetValue(proxy.ProxyPort);
      Config.Users["API"]["ProxyUserName"].SetValue(proxy.ProxyUserName);

      byte[] proxypasskey = Cursit.Utils.Password.CreateEncrypted(proxy.ProxyUserName, proxy.ProxyPass);
      Config.Users["API"]["ProxyKey"].SetValue(proxypasskey);
    }
    #endregion

    #region private void UpdateApplic() - Обновление программы
    /// <summary>
    /// Обновление программы
    /// </summary>
    private void UpdateApplic() {
      if(GordagoMain.UpdateEngine != null) {
        MessageBox.Show(Dictionary.GetString(19, 8), GordagoMain.MessageCaption);
        return;
      }

      UpdateForm uf = new UpdateForm();
      uf.ShowInTaskbar = false;
      uf.Icon = this.Icon;
      uf.ShowDialog();

      ConfigValue cfgval = Config.Users["Update"];
      cfgval["Commercial"].SetValue(uf.Commercy);
      cfgval["IsSaveLP"].SetValue(uf.IsSavePass);
      cfgval["CheckIsStart"].SetValue(uf.CheckUpdateIsStartProg);

      if(uf.IsSavePass) {
        cfgval["Login"].SetValue(uf.Login);
        cfgval["Profile"].SetValue(Password.CreateEncrypted(uf.Login, uf.Password));
      } else {
        cfgval["Login"].SetValue("");
        cfgval["Profile"].SetValue(System.Text.Encoding.Default.GetBytes("empty profyle"));
      }
    }
    #endregion

    #region public DialogResult ShowRegPage()
    public DialogResult ShowRegPage() {
      GetFullVersionForm form = new GetFullVersionForm();
      return form.ShowDialog();
    }
    #endregion

    #region private void _tsStandart_VisibleChanged(object sender, EventArgs e)
    private void _tsStandart_VisibleChanged(object sender, EventArgs e) {
      UpdateToolsPanelView();
    }
    #endregion

    #region private void _tsTimeFrame_VisibleChanged(object sender, EventArgs e)
    private void _tsTimeFrame_VisibleChanged(object sender, EventArgs e) {
      UpdateToolsPanelView();
    }
    #endregion

    #region private void _tsCharts_VisibleChanged(object sender, EventArgs e)
    private void _tsCharts_VisibleChanged(object sender, EventArgs e) {
      UpdateToolsPanelView();
    }
    #endregion

    #region private void _tsChartObject_VisibleChanged(object sender, EventArgs e)
    private void _tsChartObject_VisibleChanged(object sender, EventArgs e) {
      UpdateToolsPanelView();
    }
    #endregion

    #region private void LoadBrokers()
    private void LoadBrokers() {
      _brokers = new List<Type>();

      string dir = Application.StartupPath + "\\brokers";
      FileEngine.CheckDir(dir + "\\tmp.tmp");

      string[] files = Directory.GetFiles(dir, "*.dll");
      foreach(string file in files){
        string fn = FileEngine.GetFileNameFromPath(file);
        switch (fn) {
          case "libeay32.dll":
          case "ssleay32.dll":
          case "IFXMarketsLib.dll":
            break;
          default:
            try {
              Type[] types = Assembly.LoadFile(file).GetTypes();
              foreach (Type type in types) {
                if (type.BaseType == typeof(Broker) && type.IsPublic) {
                  _brokers.Add(type);
                }
              }
            } catch { }
            break;
        }
      }
    }
    #endregion

    #region private void LoadPlugIns()
    private void LoadPlugIns() {
      string dir = Application.StartupPath + "\\plugin";
      Cursit.Utils.FileEngine.CheckDir(dir + "\\tmp.tmp");
      string[] fplugins = System.IO.Directory.GetFiles(dir, "*.dll");
      List<ToolStripItem> items = new List<ToolStripItem>();
      _plugins = new List<PlugInModule>();
      foreach(string file in fplugins) {
        try {
          Type[] types = Assembly.LoadFile(file).GetTypes();
          foreach(Type type in types) {
            if(type.BaseType == typeof(PlugInModule) && type.IsPublic) {
              PlugInModule plugin = Activator.CreateInstance(type) as PlugInModule;
              IPlugIn iplugin = plugin as IPlugIn;
              if(iplugin != null && plugin.OnLoad(this)) {
                _plugins.Add(plugin);
                if(iplugin.MenuItem != null) {
                  items.Add(iplugin.MenuItem);
                }
                if(iplugin.Toolbar != null) {
                  //this._tspTop.Controls.Add(iplugin.Toolbar);
                  iplugin.Toolbar.Location = new System.Drawing.Point(3, 150);
                  //iplugin.Toolbar.Visible = true;
                }
              }
            }
          } 

        } catch {
          MessageBox.Show("Can not load plugin:\n" + file);
        }
      }
      //iplugin.Toolbar.Visible = true;

      if(items.Count > 0) {
        int bindex = 3;
        int k = 0;
        for(int i = 0; i < items.Count; i++) {
          _mniService.DropDownItems.Insert(bindex+i, items[i]);
          k = i+1;
        }
        _mniService.DropDownItems.Insert(bindex + k, new ToolStripSeparator());
      }
      this.RefreshViBroker();
    }
    #endregion

    #region private void OnMdiChildActivateMethod()
    private void OnMdiChildActivateMethod() {
      this.Explorer.ClearIndicatorProperty();
      this.Explorer.UnSelectAllRowInViewIndicators();

      bool chartenabled = false;
      IChartManager chartManager = null;
      if(this.ActiveMdiChild is ChartForm) {
        chartenabled = true;
        chartManager = (this.ActiveMdiChild as ChartForm).ChartManager;
      } 

      this.ChartSetMenuEnabled(chartenabled);

      this.Tester.UpdateStrategyList();
      this.Tester.SelectStrategy(this.ActiveMdiChild as IStrategyForm);

      if(this.ChartActivate != null) {
        this.ChartActivate(chartManager, new EventArgs());
      }
    }
    #endregion

    #region public bool GetSymbolIsHide(Symbol symbol)
    public bool GetSymbolIsHide(ISymbol symbol) {
      return Config.Users["Terminal"][symbol.Name]["Hide", false];
    }
    #endregion

    #region public void SetSymbolHide(Symbol symbol, bool ishide)
    public void SetSymbolHide(ISymbol symbol, bool ishide) {
      Config.Users["Terminal"][symbol.Name]["Hide"].SetValue(ishide);
      this.SymbolsPanel.UpdateSymbolTable();
      this.Tester.UpdateSymbolsList();
    }
    #endregion

    #region public string Lang
    public string Lang {
      get { return GordagoMain.Lang; }
    }
    #endregion

    #region public IChartManager[] GetCharts()
    public IChartManager[] GetCharts() {
      List<IChartManager> list = new List<IChartManager>();
      foreach(Form form in this.MdiChildren) {
        if(form is ChartForm) {
          list.Add((form as ChartForm).ChartManager);
        }
      }
      return list.ToArray();
    }
    #endregion

    #region public ISymbolList Symbols
    public ISymbolList Symbols {
      get { return GordagoMain.SymbolEngine; }
    }
    #endregion

    #region public IndicatorManager IndicatorManager
    public IndicatorManager IndicatorManager {
      get { return GordagoMain.IndicatorManager; }
    }
    #endregion

    #region public IChartManager ActiveChart
    public IChartManager ActiveChart {
      get {
        if(this.ActiveMdiChild != null && this.ActiveMdiChild is ChartForm) {
          return (this.ActiveMdiChild as ChartForm).ChartManager;
        }
        return null;
      }
    }
    #endregion

    #region public TimeFrameManager TimeFrameManager
    public TimeFrameManager TimeFrameManager {
      get { return TimeFrameManager.TimeFrames; }
    }
    #endregion

    #region private void _chartObjectManager_Register(object sender, TypeEventArgs e)
    private void _chartObjectManager_Register(object sender, TypeEventArgs e) {
      ChartObject chartObject = _chartObjectManger.Create(e.Type.FullName, "temp");
      if(chartObject == null) return;

      ToolStripButton tsb = new ToolStripButton();
      tsb.Name = GetChartObjectBTNNameFromTypeName(e.Type.FullName);
      tsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      if(chartObject.Image != null) {
        tsb.Image = chartObject.Image;
      }
      tsb.ImageTransparentColor = System.Drawing.Color.Magenta;
      tsb.Size = new System.Drawing.Size(23, 22);
      tsb.Text = "";
      tsb.ToolTipText = chartObject.TypeName;

      tsb.Click += new System.EventHandler(this._mnbChartObject_Click);
      _tsChartObject.Items.Add(tsb);
    }
    #endregion

    #region private string GetChartObjectTypeNameFromBTNName(string tsButtonName)
    private string GetChartObjectTypeNameFromBTNName(string tsButtonName) {
      return tsButtonName.Replace("mnbCO_", "").Replace("_", ".");
    }
    #endregion

    #region private string GetChartObjectBTNNameFromTypeName(string typename)
    private string GetChartObjectBTNNameFromTypeName(string typename) {
      return "mnbCO_" + typename.Replace(".", "_");
    }
    #endregion

    #region private void _mnbChartObject_Click(object sender, EventArgs e)
    private void _mnbChartObject_Click(object sender, EventArgs e) {
      string typename = GetChartObjectTypeNameFromBTNName((sender as ToolStripButton).Name);
      ChartObject chartObject = _chartObjectManger.Create(typename);
      ChartSetMouseType(chartObject);
    }
    #endregion

    #region protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
      if ((Control.ModifierKeys & Keys.Control) == Keys.Control) {
        switch (keyData) {
          case Keys.N | Keys.Control:
            this.CreateNewStrategy();
            return true;
          case Keys.O | Keys.Control:
            _sewfs.OpenFromFileDialog();
            return true;
        }
      }
      if (this.ActiveMdiChild == null) {
        if (this.ActiveMdiChild is ChartForm) {
          ChartForm chf = this.ActiveMdiChild as ChartForm;
          //					switch (keyData){
          //						case Keys.Add:
          //							chf.ScaleType++;
          //							return true;
          //						case Keys.Subtract:
          //							chf.ScaleType--;
          //							return true;
          //						case Keys.End:
          //							chf.Position = chf.Bars.Size;
          //							return true;
          //						case Keys.Home:
          //							chf.Position = 0;
          //							return true;
          //						case Keys.Left:
          //							chf.Position--;
          //							return true;
          //						case Keys.Right:
          //							chf.Position++;
          //							return true;
          //						case Keys.PageUp:
          //							chf.Position -= chf.CountViewBar;
          //							return true;
          //						case Keys.PageDown:
          //							chf.Position += chf.CountViewBar;
          //							return true;
          //					}
        } else if (this.ActiveMdiChild is EditorForm) {
          EditorForm wf = this.ActiveMdiChild as EditorForm;
          switch (keyData) {
            case Keys.F5:
              //							this._seTesterPanel.Start();
              return true;
          }
          if ((Control.ModifierKeys & Keys.Control) == Keys.Control) {
            switch (keyData) {
              case Keys.S | Keys.Control:
                this.SaveStrategy();
                return true;
            }
          }
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }
    #endregion

    #region protected override void OnActivated(EventArgs e)
    protected override void OnActivated(EventArgs e) {
      base.OnActivated(e);

      if (!this._isfirststart)
        return;

      this._isfirststart = false;
      this.Visible = true;
      if (GSOStart.LForm != null) {
        System.Threading.Thread.Sleep(1000);
        GSOStart.LForm.Close();
        GSOStart.LForm.Dispose();
        GSOStart.LForm = null;

        if (this.MdiChildren.Length > 0) {
          if (this.MdiChildren[this.MdiChildren.Length - 1].WindowState == FormWindowState.Maximized) {
            this.MdiChildren[this.MdiChildren.Length - 1].WindowState = FormWindowState.Normal;
            this.MdiChildren[this.MdiChildren.Length - 1].WindowState = FormWindowState.Maximized;
          }
        }
      }
      if (Config.Users["Update"]["CheckIsStart", true]) {
        System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(this.AutoCheckUpdateProccess));
        t.IsBackground = true;
        t.Start();
      }
      this.OnMdiChildActivateMethod();
      if (ConnectForm.GetConfigShowAtStartup()) {
        this.ShowConnectForm();
      }
    }
    #endregion

    #region protected override void OnMdiChildActivate(EventArgs e)
    protected override void OnMdiChildActivate(EventArgs e) {
      base.OnMdiChildActivate(e);
      this.OnMdiChildActivateMethod();
      this._owsPanel.ChildFormActivate(this.ActiveMdiChild);
      this.Terminal.MainFormMdiChildActivate();
      
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      if (disposing) {
        foreach (Form form in this.MdiChildren) {
          if (form is ChartForm) {
            ChartForm cform = form as ChartForm;
            cform.DestroyFigures();
          }
        }

        for (int i = 0; i < _plugins.Count; i++) {
          _plugins[i].OnDestroy();
        }

        MainFormManager.Save(_dockManager);

        //bool ws = this.WindowState == FormWindowState.Maximized;

        //if (!ws)
        //  Config.Users["Form"]["MainForm"]["WindowSize"].SetValue(this.Size);

        //Config.Users["Form"]["MainForm"]["WindowState"].SetValue(ws);

        //ToolStripManager.SaveSettings(this, "Default");
//        _workoffline = true;
//        _api.Dispose();
        BCM.Stop();
      }
      base.Dispose(disposing);
    }
    #endregion

    #region protected override void WndProc(ref Message m)
    protected override void WndProc(ref Message m) {
      if (m.Msg == Cursit.WMsg.WM_CLOSE) {
        if (!CloseAllStrategy())
          return;
        GordagoMain.IsCloseProgram = true;
      }
      base.WndProc(ref m);
    }
    #endregion

    #region private void Controls_Added(object sender, ControlEventArgs ce)
    private void Controls_Added(object sender, ControlEventArgs ce) {
      if (ce.Control is MdiClient) {
        _mdiClient = ce.Control as MdiClient;
        _mdiClient.ControlAdded += new ControlEventHandler(this.MDIClient_ControlAdded);
        _mdiClient.ControlRemoved += new ControlEventHandler(this.MDIClient_ControlRemoved);
      }
    }
    #endregion

    #region private void MDIClient_ControlAdded(object sender, ControlEventArgs ce)
    private void MDIClient_ControlAdded(object sender, ControlEventArgs ce) {
      _owsPanel.ChildFormAdd(ce.Control as Form);
    }
    #endregion

    #region private void MDIClient_ControlRemoved(object sender, ControlEventArgs ce)
    private void MDIClient_ControlRemoved(object sender, ControlEventArgs ce) {
      _owsPanel.ChildFormRemove(ce.Control as Form);
    }
    #endregion

    #region public void UpdateStatusString()
    public void UpdateStatusString() {
      string ss = "";
      switch (this.BCM.ConnectionStatus) {
        case BrokerConnectionStatus.Offline:
          ss = "Offline";
          break;
        case BrokerConnectionStatus.LoadingData:
          ss = "Loading data";
          break;
        case BrokerConnectionStatus.WaitingForConnection:
          ss = "Waiting for connection";
          break;
        case BrokerConnectionStatus.Online:
          ss = "Online";
          break;
      }

      this.Text = String.Format("{0} [{2}] {1}", _captiontext, this.DefaultAccountId, ss);
    }
    #endregion

    #region private void ShowConnectForm()
    private void ShowConnectForm() {
      ConnectForm cform = new ConnectForm();
      DialogResult dr = cform.ShowDialog();

      Config.Users["API"]["IsVS"].SetValue(cform.IsVirtualServer);
      Config.Users["API"]["ShowConnAtStart"].SetValue(cform.IsShowAsStartup);
      Config.Users["API"]["VSUseAllSmb"].SetValue(cform.UseAllSymbols);
      Config.Users["API"]["VSTime"].SetValue(cform.VSStartTime);

      if (dr != DialogResult.OK)
        return;

      BrokerProxyInfo proxy = null;

      if (cform.ProxySetting.ProxyEnable) {
        proxy = new BrokerProxyInfo(cform.ProxySetting.ProxyServer,
          cform.ProxySetting.ProxyPort,
          cform.ProxySetting.ProxyUserName,
          cform.ProxySetting.ProxyPass);
      }

      BrokerCommandLogon bcmdLogon = new BrokerCommandLogon(cform.UserName, cform.UserPass, proxy, cform.IsDemo);

      BrokerConnectionStatusChanged(BrokerConnectionStatus.WaitingForConnection);

      if (cform.IsVirtualServer) {
#if DEMO
        _isVirtualBroker = true;
#endif

        _brokerCommandManager.Start(typeof(Gordago.API.VirtualForex.VirtualBroker), bcmdLogon,
          cform.VSSettings, cform.VSStartTime, -1, false, cform.UseAllSymbols);
        VirtualBroker vbroker = _brokerCommandManager.Broker as VirtualBroker;

      } else {
#if DEMO
        _isVirtualBroker = false;
#endif
        if (_brokers.Count == 0) {
          MessageBox.Show("Broker not found!", GordagoMain.MessageCaption);
          BrokerConnectionStatusChanged(BrokerConnectionStatus.Offline);
          return;
        }
        Type brokerType = _brokers[0];
        _brokerCommandManager.Start(brokerType, bcmdLogon);
      }
      this.Refresh();
    }
    #endregion

    #region IBrokerEvents Members

    private delegate void BrokerHandler<T>(T e);

    #region public void BrokerConnectionStatusChanged(BrokerConnectionStatus status)
    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      if (GordagoMain.IsCloseProgram) return;
      if (!this.InvokeRequired) {

        this._mniConnect.Enabled = this._mnbConnect.Enabled =
          this._mniSDownloadHistory.Enabled = status == BrokerConnectionStatus.Offline;

        this._mniDisconnect.Enabled =
          this._mnbDisconnect.Enabled = status == BrokerConnectionStatus.Online;

        _defaultAccountId = "";
        switch (status) {
          case BrokerConnectionStatus.Offline:
            if (_savedConnStatus == BrokerConnectionStatus.Online) {
              //this.ShowConnectForm();
            }
            this._tsslblConnectStatus.Text = "Offline";

#if DEMO
            if (_isVirtualBroker) {
              if (API.VirtualForex.VirtualBroker.IsDemoLimit) {
                if (DialogResult.OK != MessageBox.Show(Language.Dictionary.GetString(1, 10), GordagoMain.MessageCaption, MessageBoxButtons.OKCancel)) {
                  GordagoMain.MainForm.ShowRegPage();
                }
              }
            }
#endif
            break;
          case BrokerConnectionStatus.LoadingData:
            this._tsslblConnectStatus.Text = "Loading data...";
            break;
          case BrokerConnectionStatus.WaitingForConnection:
            this._tsslblConnectStatus.Text = "Waiting for connection...";
            break;
          case BrokerConnectionStatus.Online:
            string defaccoutid = Config.Users["Terminal"]["AccoutId", ""];
            if (this.BCM.Broker.Accounts.Count == 1) {
              defaccoutid = this.BCM.Broker.Accounts[0].AccountId;
            } else {
              bool lfind = false;
              for (int j = 0; j < this.BCM.Broker.Accounts.Count; j++) {
                IAccount account = this.BCM.Broker.Accounts[j];
                if (account.AccountId == defaccoutid) {
                  lfind = true;
                  break;
                }
              }
              if (!lfind) {
                defaccoutid = this.BCM.Broker.Accounts[0].AccountId;
              }
            }
            _defaultAccountId = defaccoutid;
            Config.Users["Terminal"]["AccoutId"].SetValue(defaccoutid);
            this._tsslblConnectStatus.Text = "Online";
            break;
        }
        this.RefreshViBroker();

        this.SymbolsPanel.BrokerConnectionStatusChanged(status);
        this.Terminal.BrokerConnectionStatusChanged(status);
        this.Tester.UpdateSymbolsList();

        foreach (Form form in this.MdiChildren)
          if (form is ChartForm) (form as ChartForm).BrokerConnectionStatusChanged(status);

        this.UpdateStatusString();
        _savedConnStatus = status;
      } else {
        this.Invoke(new BCMConnectionStatusHandler(this.BrokerConnectionStatusChanged), new object[] { status });
      }
    }
    #endregion

    #region public void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      if (!this.InvokeRequired) {
        this.Terminal.BrokerAccountsChanged(be);
        foreach (Form form in this.MdiChildren)
          if (form is ChartForm)
            (form as ChartForm).BrokerAccountsChanged(be);
      } else {
        if (!this.Disposing)
          this.Invoke(new BCMAccountsHandler(this.BrokerAccountsChanged), new object[] { be });
      }
    }
    #endregion

    #region public void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      if (!this.InvokeRequired) {
        
        this.Terminal.BrokerOrdersChanged(be);
        foreach (Form form in this.MdiChildren)
          if (form is ChartForm) (form as ChartForm).BrokerOrdersChanged(be);
      } else {
        if (!this.Disposing)
          this.Invoke(new BCMOrdersHandler(this.BrokerOrdersChanged), new object[] { be });
      }
    }
    #endregion

    #region public void BrokerTradesChanged(BrokerTradesEventArgs be)
    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      if (!this.InvokeRequired) {
        this.Terminal.BrokerTradesChanged(be);
        foreach (Form form in this.MdiChildren)
          if (form is ChartForm) (form as ChartForm).BrokerTradesChanged(be);
      } else {
        if (!this.Disposing)
          this.Invoke(new BCMTradesHandler(this.BrokerTradesChanged), new object[] { be });
      }
    }
    #endregion

    #region public void BrokerCommandStarting(BrokerCommand command)
    public void BrokerCommandStarting(BrokerCommand command) {
      if (GordagoMain.IsCloseProgram) return;
      if (!this.InvokeRequired) {
        this.Terminal.BrokerCommandStarting(command);
        foreach (Form form in this.MdiChildren)
          if (form is ChartForm) (form as ChartForm).BrokerCommandStarting(command);
        string txtcmd = "";

        if (command is BrokerCommandLogoff) {
          txtcmd = "Logoff";
        } else if (command is  BrokerCommandLogon) {
          txtcmd = "Logon";
        } else if (command is BrokerCommandTradeOpen) {
          txtcmd = "Trade Open";
        } else if (command is BrokerCommandTradeModify) {
          txtcmd = "Trade Modify";
        } else if (command is BrokerCommandTradeClose) {
          txtcmd = "Trade Close";
        } else if (command is BrokerCommandEntryOrderCreate) {
          txtcmd = "Pending Order Create";
        } else if (command is BrokerCommandEntryOrderModify) {
          txtcmd = "Pending Order Modify";
        } else if (command is BrokerCommandEntryOrderDelete) {
          txtcmd = "Pending Order Delete";
        }
        this._tsslblTerminalCommand.Text = txtcmd;
      } else {
        if (!this.Disposing)
          this.Invoke(new BCMCommandStarting(this.BrokerCommandStarting), new object[] { command });
      }
    }
    #endregion

    private long _savedUpdateOnlineRateTime = DateTime.Now.AddMinutes(-1).Ticks;
    private BCMOnlineRatesHandler _brokerOnlineRateHandler;

    #region public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {

      if (GordagoMain.IsCloseProgram)
        return;

      try {
        if (!this.InvokeRequired) {
          if (DateTime.Now.Ticks - _savedUpdateOnlineRateTime < 5000000L && this.BCM.Broker is VirtualBroker) 
            return;

          for (int i = 0; i < this.MdiChildren.Length; i++) {
            if (this.MdiChildren[i] is ChartForm) {
              (this.MdiChildren[i] as ChartForm).BrokerOnlineRatesChanged(be);
            }
          }
          this.SymbolsPanel.BrokerOnlineRatesChanged(be);
          this.Terminal.BrokerOnlineRatesChanged(be);
          _savedUpdateOnlineRateTime = DateTime.Now.Ticks;
        } else {
          if (_brokerOnlineRateHandler == null)
            _brokerOnlineRateHandler = new BCMOnlineRatesHandler(this.BrokerOnlineRatesChanged);
          if (!this.Disposing)
            this.Invoke(_brokerOnlineRateHandler, be);
        }
      } catch { }
    }
    #endregion

    #region public void BrokerJournalRecordAdded(object sender, BrokerJornalEventArgs e)
    public void BrokerJournalRecordAdded(object sender, BrokerJornalEventArgs e) {
      if (GordagoMain.IsCloseProgram)
        return;
      try {
        if (!this.InvokeRequired) {
          this.Terminal.BrokerJournalRecordAdded(e);
        } else {
        if (!this.Disposing)
          this.Invoke(new EventHandler<BrokerJornalEventArgs>(this.BrokerJournalRecordAdded), new object[] { sender, e});
        }
      } catch { }
    }
    #endregion

    #region public void BrokerInitializeResult(object sender, EventArgs e)
    public void BrokerInitializeResult(object sender, EventArgs e) {
      if (!this.InvokeRequired) {
        if (sender != null)
          return;

        MessageBox.Show("The Broker library Error", GordagoMain.MessageCaption);
        BrokerConnectionStatusChanged(BrokerConnectionStatus.Offline);
      } else {
        if (!this.Disposing)
          this.Invoke(new EventHandler(this.BrokerInitializeResult), new object[] { sender, e });
      }
    }
    #endregion

    #region public void BrokerCommandStopping(BrokerCommand command, BrokerResult result)
    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      if (GordagoMain.IsCloseProgram) return;
      if (!this.InvokeRequired) {
        if (command is BrokerCommandLogon) {
          if (result.Error != null) {
            MessageBox.Show(result.Error.Message, GordagoMain.MessageCaption);
            this.ShowConnectForm();
          } else {
            BrokerCommandLogon cmdLogon = command as BrokerCommandLogon;
            if (cmdLogon.Login != "") {
              Config.Users["API"]["Demo"].SetValue(cmdLogon.Demo);
              Config.Users["API"]["Login"].SetValue(cmdLogon.Login);
              byte[] key = Cursit.Utils.Password.CreateEncrypted(cmdLogon.Login, cmdLogon.Password);
              Config.Users["API"]["Key"].SetValue(key);
            }
          }
        }
        this._tsslblTerminalCommand.Text = "";
        this.Terminal.BrokerCommandStopping(command, result);
        foreach (Form form in this.MdiChildren)
          if (form is ChartForm) (form as ChartForm).BrokerCommandStopping(command, result);
      } else {
        if (!this.Disposing)
          this.Invoke(new BCMCommandStopping(this.BrokerCommandStopping),
          new object[] { command, result});
      }
    }
    #endregion

    #region public void BrokerUpdateSymbolStarting(UpdateSymbolEventArgs se)
    public void BrokerUpdateSymbolStarting(UpdateSymbolEventArgs se) {
      if (GordagoMain.IsCloseProgram) return;
      try {
        if (!this.InvokeRequired) {
          foreach (Form form in this.MdiChildren)
            if (form is ChartForm) (form as ChartForm).BrokerUpdateSymbolStarting(se);
        } else {
          if (!this.Disposing)
            this.Invoke(new BrokerHandler<UpdateSymbolEventArgs>(this.BrokerUpdateSymbolStarting), new object[] {se});
        }
      } catch { }
    }
    #endregion

    #region public void BrokerUpdateSymbolStopping(UpdateSymbolEventArgs se)
    public void BrokerUpdateSymbolStopping(UpdateSymbolEventArgs se) {
      if (GordagoMain.IsCloseProgram) return;
      try {
        if (!this.InvokeRequired) {
          foreach (Form form in this.MdiChildren)
            if (form is ChartForm) (form as ChartForm).BrokerUpdateSymbolStopping(se);
        } else {
          if (!this.Disposing)
            this.Invoke(new BrokerHandler<UpdateSymbolEventArgs>(this.BrokerUpdateSymbolStopping), new object[] { se });
        }
      } catch { }
    }
    #endregion

    #region public void BrokerUpdateSymbolDownloadPart(UpdateSymbolEventArgs se)
    public void BrokerUpdateSymbolDownloadPart(UpdateSymbolEventArgs se) {
      if (GordagoMain.IsCloseProgram) return;
      try {
        if (!this.InvokeRequired) {
          foreach (Form form in this.MdiChildren)
            if (form is ChartForm) (form as ChartForm).BrokerUpdateSymbolDownloadPart(se);
        } else {
          if (!this.Disposing)
            this.Invoke(new BrokerHandler<UpdateSymbolEventArgs>(this.BrokerUpdateSymbolDownloadPart), new object[] { se });
        }
      } catch { }
    }
    #endregion

    #endregion

    #region private void SetViBrokerSpeed(int speed)
    private void SetViBrokerSpeed(int speed) {
      if (BCM.Broker == null)
        return;
      if (!(BCM.Broker is API.VirtualForex.VirtualBroker))
        return;
      API.VirtualForex.VirtualBroker broker = BCM.Broker as API.VirtualForex.VirtualBroker;
      broker.Speed = speed;
      this.RefreshViBroker();
    }
    #endregion

    #region private void SetViBrokerPause()
    private void SetViBrokerPause() {
      if (BCM.Broker == null)
        return;
      if (!(BCM.Broker is API.VirtualForex.VirtualBroker))
        return;
      API.VirtualForex.VirtualBroker broker = BCM.Broker as API.VirtualForex.VirtualBroker;
      broker.Pause = !broker.Pause;
      this.RefreshViBroker();
    }
    #endregion

    #region private void RefreshViBroker()
    private void RefreshViBroker() {
      bool enable = true;
      if (BCM.Broker == null)
        enable = false;
      if (!(BCM.Broker is API.VirtualForex.VirtualBroker))
        enable = false;

      if (BCM.ConnectionStatus != BrokerConnectionStatus.Online)
        enable = false;

      this._mniBroker.Visible = enable;
      this.SetMenuToolbarEnable(this._tsVirtualServer, enable);
      //this._tsVirtualServer.Enabled = enable;

      if (!enable) {
        return;
      }
      API.VirtualForex.VirtualBroker broker = BCM.Broker as API.VirtualForex.VirtualBroker;

      _mnbVSPause.Checked = _mniVSPause.Checked = broker.Pause;

      _mniVSS100x.Checked =
        _mniVSS10x.Checked =
        _mniVSS1x.Checked =
        _mniVSS2x.Checked =
        _mniVSS5x.Checked = false;
      _mnbVSS100x.Checked =
        _mnbVSS10x.Checked =
        _mnbVSS1x.Checked =
        _mnbVSS2x.Checked =
        _mnbVSS5x.Checked = false;

      switch (broker.Speed) { 
        case 1000:
          _mniVSS1x.Checked = true;
          _mnbVSS1x.Checked = true;
          break;
        case 100:
          _mniVSS10x.Checked = true;
          _mnbVSS10x.Checked = true;
          break;
        case 10:
          _mniVSS100x.Checked = true;
          _mnbVSS100x.Checked = true;
          break;
        case 500:
          _mniVSS2x.Checked = true;
          _mnbVSS2x.Checked = true;
          break;
        case 200:
          _mniVSS5x.Checked = true;
          _mnbVSS5x.Checked = true;
          break;
      }
    }
    #endregion

    #region private void _mnbVSS1x_Click(object sender, EventArgs e)
    private void _mnbVSS1x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(1000);
    }
    #endregion

    #region private void _mnbVSS10x_Click(object sender, EventArgs e)
    private void _mnbVSS10x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(100);
    }
    #endregion

    #region private void _mnbVSS100x_Click(object sender, EventArgs e)
    private void _mnbVSS100x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(10);
    }
    #endregion

    #region private void _mniVSS1x_Click(object sender, EventArgs e)
    private void _mniVSS1x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(1000);
    }
    #endregion

    #region private void _mniVSS2x_Click(object sender, EventArgs e)
    private void _mniVSS2x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(500);
    }
    #endregion

    #region private void _mniVSS5x_Click(object sender, EventArgs e)
    private void _mniVSS5x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(200);
    }
    #endregion

    #region private void _mniVSS10x_Click(object sender, EventArgs e)
    private void _mniVSS10x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(100);
    }
    #endregion

    #region private void _mniVSS100x_Click(object sender, EventArgs e)
    private void _mniVSS100x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(10);
    }
    #endregion

    #region private void _mnbVSS2x_Click(object sender, EventArgs e)
    private void _mnbVSS2x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(500);
    }
    #endregion

    #region private void _mnbVSS5x_Click(object sender, EventArgs e)
    private void _mnbVSS5x_Click(object sender, EventArgs e) {
      this.SetViBrokerSpeed(200);
    }
    #endregion

    #region private void _mnbVSPause_Click(object sender, EventArgs e)
    private void _mnbVSPause_Click(object sender, EventArgs e) {
      this.SetViBrokerPause();
    }
    #endregion

    #region private void _mniVSPause_Click(object sender, EventArgs e)
    private void _mniVSPause_Click(object sender, EventArgs e) {
      this.SetViBrokerPause();
    }
    #endregion

    #region class ToolStripMenuItemTableColumn : ToolStripMenuItem
    class ToolStripMenuItemTableColumn : ToolStripMenuItem {
      private TableColumn _tableColumn;

      public ToolStripMenuItemTableColumn(TableColumn column):base(column.Caption) {
        this.Name = column.Name;
        this._tableColumn = column;
      }

      public TableColumn TableColumn {
        get { return this._tableColumn; }
      }
    }
    #endregion

    #region public void SetContextMenuOnTable(TableControl table)
    public void SetContextMenuOnTable(TableControl table) {
      if (table.ContextMenuStrip == null) 
        table.ContextMenuStrip = new ContextMenuStrip();

      ContextMenuStrip cmnu = table.ContextMenuStrip;

      cmnu.Opened += new EventHandler(this.CMnuTable_Opened);
      if (cmnu.Items.Count > 0) {
        cmnu.Items.Add(new ToolStripSeparator());
      }
      ToolStripMenuItem mni = new ToolStripMenuItem(Dictionary.GetString(18, 9, "Скрыть"));
      mni.Name = "mniHideColumn";
      cmnu.Items.Add(mni);
      
      mni = new ToolStripMenuItem(Dictionary.GetString(18, 10, "Показать"));
      mni.Name = "mniShowColumn";
      cmnu.Items.Add(mni);

      mni = new ToolStripMenuItem(Dictionary.GetString(2, 5, "Save as..."));
      mni.Click += new EventHandler(this.CMnuTable_mniSaveAs);
      cmnu.Items.Add(mni);
    }
    #endregion

    #region private void CMnuTable_Opened(object sender, EventArgs e)
    private void CMnuTable_Opened(object sender, EventArgs e) {
      ContextMenuStrip cmnu = sender as ContextMenuStrip;
      TableControl table = cmnu.SourceControl as TableControl;

      ToolStripMenuItem mniH = null, mniS = null ;

      for (int i = 0; i < cmnu.Items.Count; i++) {
        ToolStripMenuItem mni = cmnu.Items[i] as ToolStripMenuItem;
        if (mni != null && mni.Name == "mniHideColumn") {
          mniH = mni;
        } else if (mni != null && mni.Name == "mniShowColumn") {
          mniS = mni;
        }
      }
      mniH.DropDownItems.Clear();
      mniS.DropDownItems.Clear();
      
      for (int i = 0; i < table.Columns.Count; i++) {
        TableColumn col = table.Columns[i];
        if (col.Visible) {
          ToolStripMenuItem mniHI = new ToolStripMenuItemTableColumn(col);
          mniHI.Click += new EventHandler(CMnuTableHide_Click);
          mniH.DropDownItems.Add(mniHI);
        } else {
          ToolStripMenuItem mniSI = new ToolStripMenuItemTableColumn(col);
          mniSI.Click += new EventHandler(CMnuTableShow_Click);
          mniS.DropDownItems.Add(mniSI);
        }
      }
      mniH.Visible = mniH.DropDownItems.Count > 0;
      mniS.Visible = mniS.DropDownItems.Count > 0;
    }
    #endregion

    #region private TableColumn GetColumnFromTable(object sender)
    private TableColumn GetColumnFromTable(object sender) {
      ToolStripMenuItemTableColumn mni = sender as ToolStripMenuItemTableColumn;
      return mni.TableColumn;
    }
    #endregion

    #region private void CMnuTableHide_Click(object sender, EventArgs e)
    private void CMnuTableHide_Click(object sender, EventArgs e) {
      TableColumn column = this.GetColumnFromTable(sender);
      column.Visible = false;
    }
    #endregion

    #region private void CMnuTableShow_Click(object sender, EventArgs e)
    private void CMnuTableShow_Click(object sender, EventArgs e) {
      TableColumn column = this.GetColumnFromTable(sender);
      column.Visible = true;
    }
    #endregion

    #region private void CMnuTable_mniSaveAs(object sender, EventArgs e)
    private void CMnuTable_mniSaveAs(object sender, EventArgs e) {
      ToolStripMenuItem mni = sender as ToolStripMenuItem;

      ContextMenuStrip cmnu = mni.Owner as ContextMenuStrip;
      
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "CSV (*.csv)|*.csv";

      string reportpath = Application.StartupPath + "\\reports";
      string path = Config.Users["PathReport", reportpath];
      sfd.InitialDirectory = path;
      sfd.FileName = path + "\\table.csv";
      if (sfd.ShowDialog() != DialogResult.OK)
        return;

      Config.Users["PathReport"].SetValue(Cursit.Utils.FileEngine.GetDirectory(sfd.FileName));

      TableControl table = cmnu.SourceControl as TableControl;
      table.SaveCSV(sfd.FileName);
    }
    #endregion

    #region private void _mniHAskAQuestion_Click(object sender, EventArgs e)
    private void _mniHAskAQuestion_Click(object sender, EventArgs e) {
      string link = "";
      if (GordagoMain.Lang == "rus") {
        link = "http://forum.gordago.ru";
      } else {
        link = "http://forum.gordago.com";
      }

      this.Cursor = Cursors.WaitCursor;
      try {
        ProcessStartInfo psi = new ProcessStartInfo("iexplore", link);
        psi.WorkingDirectory = "C:\\";
        Process.Start(psi);
      } catch { }
      this.Cursor = Cursors.Default;
    }
    #endregion

    #region private void MainForm_Load(object sender, EventArgs e)
    private void MainForm_Load(object sender, EventArgs e) {

    }
    #endregion

    #region private void _mnbVTerminal_Click(object sender, EventArgs e)
    private void _mnbVTerminal_Click(object sender, EventArgs e) {
      this.ViewTerminal = !this.ViewTerminal;
    }
    #endregion

    #region private void _mniVTerminal_Click(object sender, EventArgs e)
    private void _mniVTerminal_Click(object sender, EventArgs e) {
      this.ViewTerminal = !this.ViewTerminal;
    }
    #endregion

    #region private void _tsiExplorer_VisibleChanged(object sender, EventArgs e)
    private void _tsiExplorer_VisibleChanged(object sender, EventArgs e) {
      if (!this.Visible) return;
      this.ViewIIBoxPanel = this.ViewIIBoxPanel;
    }
    #endregion

    #region private void _tsiSymbols_VisibleChanged(object sender, EventArgs e)
    private void _tsiSymbols_VisibleChanged(object sender, EventArgs e) {
      if (!this.Visible) return;
      this.ViewSymbolPanel = this.ViewSymbolPanel;
    }
    #endregion

    #region private void _tsiTerminal_VisibleChanged(object sender, EventArgs e)
    private void _tsiTerminal_VisibleChanged(object sender, EventArgs e) {
      if (!this.Visible) return;
      this.ViewTerminal = this.ViewTerminal;
    }
    #endregion

    #region private void _tsiTester_VisibleChanged(object sender, EventArgs e)
    private void _tsiTester_VisibleChanged(object sender, EventArgs e) {
      if (!this.Visible) return;
      this.ViewTester = this.ViewTester;
    }
    #endregion

    private void _mniHHelp_Click(object sender, EventArgs e) {
      string link = "";
      if (GordagoMain.Lang == "rus") {
        FileInfo file = new FileInfo(Application.StartupPath + "\\help\\help_ru.chm");
        if (file.Exists)
          Help.ShowHelp(this, file.FullName);
      } else {
        link = "http://gordago.com/forex_platform.html";
        this.Cursor = Cursors.WaitCursor;
        try {
          ProcessStartInfo psi = new ProcessStartInfo("iexplore", link);
          psi.WorkingDirectory = "C:\\";
          Process.Start(psi);
        } catch { }
        this.Cursor = Cursors.Default;
      }
    }
  }
}