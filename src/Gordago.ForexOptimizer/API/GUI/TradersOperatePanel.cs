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
using Cursit.Table;
using Gordago.Analysis.Chart;
using Language;
using System.Collections.Generic;
#endregion

namespace Gordago.API {
  public class TradersOperatePanel : UserControl, IBrokerEvents {
    public event EventHandler StatusChanged;
    private const int ADDED_POINT = 30;

    private System.Windows.Forms.NumericUpDown _nudLotsCloseTrade;
    private System.Windows.Forms.Button _btnCloseTrade;
    private System.Windows.Forms.Label _lblLotsCloseTrade;
    private System.Windows.Forms.Button _btnModifyTrade;
    private System.ComponentModel.Container components = null;
    private System.Windows.Forms.NumericUpDown _nudSleppage;

    private Gordago.API.CheckNudPoint _cnpStopModify;
    private Gordago.API.CheckNudPoint _cnpLimitModify;
    private System.Windows.Forms.Label _lblSlipPage;
    private System.Windows.Forms.Label _lblslp;
    private System.Windows.Forms.Label _lbltpp;
    private Cursit.TabControlExt _tbcMain;
    private TabPage _tbpClose;
    private TabPage _tbpModify;
    private TabPage _tbpOpen;
    private Gordago.Analysis.Chart.ExtButton _btnMinMax;

    private Color _borderColor;
    private Pen _borderPen;
    private string _text;
    private Font _font;

    private ITrade _trade = null;
    private ComboBox _cmbSymbols;
    private Gordago.API.OperatePanel.HOCPActionButton _btnBuy;
    private Gordago.API.OperatePanel.HOCPActionButton _btnSell;
    private Cursit.LabelExt _lblOpenInfo;
    private CheckBox _chkOneClick;
    private CheckBox _chkStop;
    private NumericUpDown _nudStopOpen;
    private NumericUpDown _nudLimitOpen;
    private CheckBox _chkLimit;
    private NumericUpDown _nudSlippageOpen;
    private Label _lblSlipPageOpen;
    private NumericUpDown _nudLotsOpen;
    private Label _lblLotsOperate;
    private Cursit.PanelExt panelExt1;
    private ITrade _savedTrade = null;
    private IOnlineRate _savedOnlineRate = null;
    private TradeCalculator _sellCalculator, _buyCalculator;
    private Cursit.LabelExt _lblModifyInfo;
    private Cursit.LabelExt _lblCloseInfo;

    public TradersOperatePanel() {
      InitializeComponent();

      this.BorderColor = Color.FromArgb(172, 168, 153);
      this.Text = "Trade Manager";
      this.FontVerticalText = new Font("Microsoft Sans Serif", 7);

      try {
        this._cnpStopModify.Caption = BrokerCommandManager.LNG_TBL_STOP;
        this._cnpLimitModify.Caption = BrokerCommandManager.LNG_TBL_LIMIT;
        this._lblSlipPage.Text = BrokerCommandManager.LNG_TBL_SLIPPAGE;
        this._lblLotsCloseTrade.Text = BrokerCommandManager.LNG_TBL_LOTS;
        this._btnCloseTrade.Text = BrokerCommandManager.LNG_BTN_CLOSE;
        this._btnModifyTrade.Text = BrokerCommandManager.LNG_BTN_MODIFY;
        this._btnSell.Text = Dictionary.GetString(33, 1, "Продать");
        this._btnBuy.Text = Dictionary.GetString(33, 2, "Купить");
        this._lblLotsOperate.Text = Dictionary.GetString(33, 3, "Объем");
        this._chkOneClick.Text = Dictionary.GetString(33, 4, "Один клик");
        this._lblSlipPage.Text = Dictionary.GetString(33, 6, "Отклонение");
        this._chkStop.Text = Dictionary.GetString(33, 8, "Стоп");
        this._chkLimit.Text = Dictionary.GetString(33, 9, "Лимит");
        this._lblSlipPage.Text = 
          this._lblSlipPageOpen.Text = Dictionary.GetString(33, 6, "Допуск");
        this.Trade = null;
      } catch { }

      this._lblCloseInfo.Text = "";
      this._lblOpenInfo.Text = "";
      this._lblModifyInfo.Text = "";

      _sellCalculator = new TradeCalculator(TradeType.Sell);
      _buyCalculator = new TradeCalculator(TradeType.Buy);
      this.UpdateOpenTab();
    }

    #region public bool Maximized
    public bool Maximized {
      get { return this._btnMinMax.Checked; }
      set { this._btnMinMax.Checked = value; }
    }
    #endregion

    #region public Font FontVerticalText
    public Font FontVerticalText {
      get { return this._font; }
      set { this._font = value; }
    }
    #endregion

    #region public new string Text
    public new string Text {
      get { return this._text; }
      set { this._text = value; }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set {
        _borderColor = value;
        _borderPen = new Pen(value);
        this.Invalidate();
      }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public ITrade Trade
    public ITrade Trade {
      get { return this._trade; }
      set {
        bool evt = this._trade != value;
        this._trade = value;
        this.OnTradeChanged();
      }
    }
    #endregion

    #region OpenTab

    #region private void _cmbSymbols_SelectedIndexChanged(object sender, EventArgs e)
    private void _cmbSymbols_SelectedIndexChanged(object sender, EventArgs e) {
      this.UpdateOpenTab();
    }
    #endregion

    #region private void InitOpenTab()
    private void InitOpenTab() {

      if (_tbcMain.SelectedTab != this._tbpOpen || BCM.ConnectionStatus != BrokerConnectionStatus.Online)
        return;

      _cmbSymbols.Items.Clear();

      TableControl symbolsTable = GordagoMain.MainForm.SymbolsPanel.Table;
      if (symbolsTable.Rows.Count != this._cmbSymbols.Items.Count) {
        this._cmbSymbols.Items.Clear();
        for (int i = 0; i < symbolsTable.Rows.Count; i++) {
          TableRow row = symbolsTable.Rows[i];
          _cmbSymbols.Items.Add(row.AdditionObject);
        }
      }
      if (GordagoMain.MainForm.ActiveMdiChild is ChartForm) {
        ChartForm chartForm = GordagoMain.MainForm.ActiveMdiChild as ChartForm;
        this._cmbSymbols.SelectedItem = chartForm.Symbol;
      }
    }
    #endregion

    #region private void UpdateOpenTab()
    private void UpdateOpenTab() {

      if (this._cmbSymbols.SelectedItem == null) {
        if (GordagoMain.MainForm.ActiveMdiChild is ChartForm)
          _cmbSymbols.SelectedItem = (GordagoMain.MainForm.ActiveMdiChild as ChartForm).Symbol;
      }

      ISymbol symbol = this._cmbSymbols.SelectedItem as ISymbol;
      this._btnSell.Symbol = symbol;
      this._btnBuy.Symbol = symbol;
      IOnlineRate onlineRate = null;

      if (this.BCM.ConnectionStatus == BrokerConnectionStatus.Online && symbol != null) {
        onlineRate = this.BCM.Broker.OnlineRates.GetOnlineRate(symbol.Name);
      }

      if (_savedOnlineRate != onlineRate || !this.Visible) {
        _savedOnlineRate = onlineRate;

        _chkOneClick.Enabled =
          _chkStop.Checked =
          _chkStop.Enabled =
          _chkLimit.Checked =
          _chkLimit.Enabled =
          _nudLotsOpen.Enabled =
          _nudSlippageOpen.Enabled = onlineRate != null;
      }

      if (onlineRate == null) {
        _btnSell.Price = 0;
        _btnBuy.Price = 0;

      } else {
        _btnSell.Price = onlineRate.SellRate;
        _btnBuy.Price = onlineRate.BuyRate;
        _sellCalculator.SetValues(onlineRate, _chkStop.Checked ? (int)_nudStopOpen.Value : 0, _chkLimit.Checked ? (int)_nudLimitOpen.Value : 0);
        _buyCalculator.SetValues(onlineRate, _chkStop.Checked ? (int)_nudStopOpen.Value : 0, _chkLimit.Checked ? (int)_nudLimitOpen.Value : 0);
      }
    }
    #endregion

    #region public void MainFormMdiChildActivate()
    public void MainFormMdiChildActivate() {

      if (GordagoMain.MainForm.ActiveMdiChild is ChartForm) {
        _cmbSymbols.SelectedItem = (GordagoMain.MainForm.ActiveMdiChild as ChartForm).Symbol;
      }

      this.UpdateOpenTab();
    }
    #endregion

    #region private void ViewPreview(TradeType tradeType)
    private void ViewPreview(TradeType tradeType) {
      string s = "";
      if (this.BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
        s = (tradeType == TradeType.Sell ? _btnSell.Text : _btnBuy.Text) + "\n" +
          BrokerCommandManager.LNG_HO_RATE + ": {0}\n" +
          BrokerCommandManager.LNG_HO_LOTS + ": {1}";

        TradeCalculator calc = tradeType == TradeType.Sell ? _sellCalculator : _buyCalculator;
        s = string.Format(s, calc.Rate, (int)_nudLotsOpen.Value);

        if (!float.IsNaN(calc.StopRate)) {
          s += "\n" + BrokerCommandManager.LNG_HO_STOP + ": " + SymbolManager.ConvertToCurrencyString(calc.StopRate, calc.DecimalDigits);
        }
        if (!float.IsNaN(calc.LimitRate)) {
          s += "\n" + BrokerCommandManager.LNG_HO_LIMIT + ": " + SymbolManager.ConvertToCurrencyString(calc.LimitRate, calc.DecimalDigits);
        }
      }
      _lblOpenInfo.Text = s;
    }
    #endregion

    #region private void _btnSell_MouseEnter(object sender, EventArgs e)
    private void _btnSell_MouseEnter(object sender, EventArgs e) {
      this.UpdateOpenTab();
      this.ViewPreview(TradeType.Sell);
    }
    #endregion

    #region private void _btnSell_MouseMove(object sender, MouseEventArgs e)
    private void _btnSell_MouseMove(object sender, MouseEventArgs e) {
      this.ViewPreview(TradeType.Sell);
    }
    #endregion

    #region private void _btnSell_MouseLeave(object sender, EventArgs e)
    private void _btnSell_MouseLeave(object sender, EventArgs e) {
      _lblOpenInfo.Text = "";
    }
    #endregion

    #region private void _btnBuy_MouseEnter(object sender, EventArgs e)
    private void _btnBuy_MouseEnter(object sender, EventArgs e) {
      this.UpdateOpenTab();
      this.ViewPreview(TradeType.Buy);
    }
    #endregion

    #region private void _btnBuy_MouseLeave(object sender, EventArgs e)
    private void _btnBuy_MouseLeave(object sender, EventArgs e) {
      _lblOpenInfo.Text = "";
    }
    #endregion

    #region private void _btnBuy_MouseMove(object sender, MouseEventArgs e)
    private void _btnBuy_MouseMove(object sender, MouseEventArgs e) {
      this.ViewPreview(TradeType.Buy);
    }
    #endregion

    #region private void _btnSell_Click(object sender, EventArgs e)
    private void _btnSell_Click(object sender, EventArgs e) {
      SendOrderFromHandOperate(TradeType.Sell);
    }
    #endregion

    #region private void _btnBuy_Click(object sender, EventArgs e)
    private void _btnBuy_Click(object sender, EventArgs e) {
      SendOrderFromHandOperate(TradeType.Buy);
    }
    #endregion

    #region private void SendOrderFromHandOperate(TradeType buysell)
    private void SendOrderFromHandOperate(TradeType tradeType) {

      if (!this.BCM.Busy || this.BCM.ConnectionStatus != BrokerConnectionStatus.Online) return;

      if (!this._chkOneClick.Checked) {
        string text = _lblOpenInfo.Text;

        HOCPDialog hocpd = new HOCPDialog(text);
        if (hocpd.ShowDialog() != DialogResult.OK)
          return;
      }

      TradeCalculator calc = tradeType == TradeType.Sell ? _sellCalculator : _buyCalculator;

      int lots = Convert.ToInt32(_nudLotsOpen.Value);
      int slippage = Convert.ToInt32(_nudSlippageOpen.Value);

      BrokerCommandTradeOpen command =
        new BrokerCommandTradeOpen(GordagoMain.MainForm.DefaultAccountId,
              calc.OnlineRate, tradeType, lots, slippage, calc.StopRate, calc.LimitRate, "");
      BCM.ExecuteCommand(command);
    }
    #endregion

    #region private void _chkStop_CheckedChanged(object sender, EventArgs e)
    private void _chkStop_CheckedChanged(object sender, EventArgs e) {
      this._nudStopOpen.Enabled = this._chkStop.Checked;
      this.UpdateOpenTab();
    }
    #endregion

    #region private void _chkLimit_CheckedChanged(object sender, EventArgs e)
    private void _chkLimit_CheckedChanged(object sender, EventArgs e) {
      this._nudLimitOpen.Enabled = this._chkLimit.Checked;
      this.UpdateOpenTab();
    }
    #endregion
    #endregion

    //private void UpdateModifyPanelStatus() {
    //  bool modify = _trade != null;

    //  if (_lblModifyInfo.Enabled == modify)
    //    return;
    //  _lblModifyInfo.Enabled =
    //    _cnpStopModify.Enabled =
    //    _cnpLimitModify.Enabled =
    //    this._btnModifyTrade.Enabled = modify;

    //  if (_savedTrade != _trade && _trade != null) {

          //    else {
          //  this._cnpLimitModify.Value = this.GetMiminumLimitPrice(_trade.TradeType);
          //  _cnpLimitModify.Value += (_trade.TradeType == TradeType.Sell ? -ADDED_POINT : ADDED_POINT) * _trade.OnlineRate.Symbol.Point;
          //}

    //    //if (_cnpStopModify.Checked) {
    //    //  if (_trade.StopOrder != null) {
    //    //    this._cnpStopModify.Value = _trade.StopOrder.Rate;
    //    //  } else {
    //    //    this._cnpStopModify.Value = this.GetMinimumStopPrice(_trade.TradeType);
    //    //    _cnpStopModify.Value += (_trade.TradeType == TradeType.Sell ? ADDED_POINT : -ADDED_POINT) * _trade.OnlineRate.Symbol.Point;
    //    //  }
    //    //} else {
    //    //  this._cnpStopModify.Value = 0;
    //    //}

    //  }
    //}

    private bool _isTradeChanged = false;

    #region public void OnTradeChanged()
    public void OnTradeChanged() {

      if (_isTradeChanged)
        return;
      _isTradeChanged = true;

      /* 
       * Статусы обновления трейда:
       * новый
       * обновлен
       * удален
       */

      bool enabled = _trade != null;

      if (this._btnCloseTrade.Enabled != enabled) {
        this._btnCloseTrade.Enabled =
        this._btnModifyTrade.Enabled =
        this._lblLotsCloseTrade.Enabled =
        this._lblSlipPage.Enabled =
        this._nudLotsCloseTrade.Enabled =
        this._nudSleppage.Enabled =
        this._cnpStopModify.Enabled =
        this._cnpLimitModify.Enabled = 
        this._lblslp.Enabled = 
        this._lbltpp.Enabled = 
          enabled;
      }

      bool ismodify = false;
      bool errorsl = false, errortp = false;

      /* Удален */
      if (_trade == null) {

        this._nudLotsCloseTrade.Value = 1;
        this._cnpStopModify.Checked = this._cnpLimitModify.Checked = false;
        this._cnpStopModify.Value = this._cnpLimitModify.Value = 0;
        _lblModifyInfo.Text = _lblCloseInfo.Text = "";
        string ptxt = "=0";
        this._lblslp.Text = ptxt;
        this._lbltpp.Text = ptxt;

      } else {

        if (_savedTrade != _trade) {      /* Новый */
          int lots = _trade.Lots;
          this._nudLotsCloseTrade.Maximum = lots;
          this._nudLotsCloseTrade.Value = lots;

          this._cnpLimitModify.DecimalDigits = this._cnpStopModify.DecimalDigits = _trade.OnlineRate.Symbol.DecimalDigits;

          float max = _trade.OnlineRate.SellRate * 2;

          this._cnpStopModify.Maximum = this._cnpLimitModify.Maximum = max;

          this._cnpStopModify.Checked = _trade.StopOrder != null;
          this._cnpLimitModify.Checked = _trade.LimitOrder != null;

          this._cnpStopModify.Value = _trade.StopOrder != null ? _trade.StopOrder.Rate : 0;
          this._cnpLimitModify.Value = _trade.LimitOrder != null ? _trade.LimitOrder.Rate : 0;
        }

        float minsl = this.GetMinimumStopPrice(_trade.TradeType);
        bool flagsl = _trade.StopOrder != null;
        if (flagsl != _cnpStopModify.Checked)
          ismodify = true;

        float stoprate = flagsl ? _trade.StopOrder.Rate : 0;
        if (flagsl && _cnpStopModify.Checked && _cnpStopModify.Value != stoprate)
          ismodify = true;

        if (_cnpStopModify.Checked) {
          errorsl = (_trade.TradeType == TradeType.Sell) ? (_cnpStopModify.Value <= minsl) : (_cnpStopModify.Value >= minsl);
        }

        float mintp = this.GetMiminumLimitPrice(_trade.TradeType);
        bool flagtp = _trade.LimitOrder != null;
        float limitrate = flagtp ? _trade.LimitOrder.Rate : 0;
        if (flagtp != _cnpLimitModify.Checked)
          ismodify = true;
        if (flagtp && _cnpLimitModify.Checked && _cnpLimitModify.Value != limitrate)
          ismodify = true;

        if (_cnpLimitModify.Checked)
          errortp = (_trade.TradeType == TradeType.Sell) ? (_cnpLimitModify.Value >= mintp) : (_cnpLimitModify.Value <= mintp);

        string ptxt = "=0";

        if (_cnpStopModify.Checked) {
          int p = this.GetStopPoint(_cnpStopModify.Value);
          this._lblslp.Text = "=" + p.ToString();
        } else {
          this._lblslp.Text = ptxt;
        }

        if (_cnpLimitModify.Checked) {
          this._lbltpp.Text = "=" + this.GetLimitPoint(this._cnpLimitModify.Value).ToString();
        } else {
          this._lbltpp.Text = ptxt;
        }

        int decdig = _trade.OnlineRate.Symbol.DecimalDigits;

        List<string> sa = new List<string>();

        sa.Add((_trade.TradeType == TradeType.Sell ? "Sell" : "Buy") + " #" + _trade.TradeId + "  " +
          "Lots " + _trade.Lots.ToString() + "  " + "Profit " + SymbolManager.ConvertToCurrencyString(_trade.NetPL));

        if (_trade.StopOrder != null)
          sa.Add(BrokerCommandManager.LNG_TBL_STOP + " " + SymbolManager.ConvertToCurrencyString(_trade.StopOrder.Rate, decdig));

        if (_trade.LimitOrder != null)
          sa.Add(BrokerCommandManager.LNG_TBL_LIMIT + " " + SymbolManager.ConvertToCurrencyString(_trade.LimitOrder.Rate));

        sa.Add(_trade.OpenTime.ToShortDateString() + " " + _trade.OpenTime.ToShortTimeString());
        _lblModifyInfo.Text =
          _lblCloseInfo.Text = string.Join("\n", sa.ToArray());
      }

      if (errorsl || errortp || !BCM.Busy)
        this._btnModifyTrade.Enabled = false;
      else
        this._btnModifyTrade.Enabled = ismodify;

      _savedTrade = _trade;
      _isTradeChanged = false;
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
    private void InitializeComponent() {
      this._tbcMain = new Cursit.TabControlExt();
      this._tbpClose = new System.Windows.Forms.TabPage();
      this._lblCloseInfo = new Cursit.LabelExt();
      this._btnCloseTrade = new System.Windows.Forms.Button();
      this._nudLotsCloseTrade = new System.Windows.Forms.NumericUpDown();
      this._lblSlipPage = new System.Windows.Forms.Label();
      this._nudSleppage = new System.Windows.Forms.NumericUpDown();
      this._lblLotsCloseTrade = new System.Windows.Forms.Label();
      this._tbpModify = new System.Windows.Forms.TabPage();
      this._lblModifyInfo = new Cursit.LabelExt();
      this._btnModifyTrade = new System.Windows.Forms.Button();
      this._lblslp = new System.Windows.Forms.Label();
      this._lbltpp = new System.Windows.Forms.Label();
      this._cnpLimitModify = new Gordago.API.CheckNudPoint();
      this._cnpStopModify = new Gordago.API.CheckNudPoint();
      this._tbpOpen = new System.Windows.Forms.TabPage();
      this.panelExt1 = new Cursit.PanelExt();
      this._chkStop = new System.Windows.Forms.CheckBox();
      this._nudStopOpen = new System.Windows.Forms.NumericUpDown();
      this._lblLotsOperate = new System.Windows.Forms.Label();
      this._nudLimitOpen = new System.Windows.Forms.NumericUpDown();
      this._nudLotsOpen = new System.Windows.Forms.NumericUpDown();
      this._chkLimit = new System.Windows.Forms.CheckBox();
      this._lblSlipPageOpen = new System.Windows.Forms.Label();
      this._nudSlippageOpen = new System.Windows.Forms.NumericUpDown();
      this._chkOneClick = new System.Windows.Forms.CheckBox();
      this._lblOpenInfo = new Cursit.LabelExt();
      this._btnBuy = new Gordago.API.OperatePanel.HOCPActionButton();
      this._btnSell = new Gordago.API.OperatePanel.HOCPActionButton();
      this._cmbSymbols = new System.Windows.Forms.ComboBox();
      this._btnMinMax = new Gordago.Analysis.Chart.ExtButton();
      this._tbcMain.SuspendLayout();
      this._tbpClose.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsCloseTrade)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudSleppage)).BeginInit();
      this._tbpModify.SuspendLayout();
      this._tbpOpen.SuspendLayout();
      this.panelExt1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudStopOpen)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimitOpen)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsOpen)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudSlippageOpen)).BeginInit();
      this.SuspendLayout();
      // 
      // _tbcMain
      // 
      this._tbcMain.Alignment = System.Windows.Forms.TabAlignment.Left;
      this._tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbcMain.BackColorTabPages = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
      this._tbcMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tbcMain.BorderVisible = true;
      this._tbcMain.Controls.Add(this._tbpClose);
      this._tbcMain.Controls.Add(this._tbpModify);
      this._tbcMain.Controls.Add(this._tbpOpen);
      this._tbcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this._tbcMain.Location = new System.Drawing.Point(14, 0);
      this._tbcMain.Multiline = true;
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(286, 150);
      this._tbcMain.TabIndex = 25;
      this._tbcMain.SelectedIndexChanged += new System.EventHandler(this._tbcMain_SelectedIndexChanged);
      // 
      // _tbpClose
      // 
      this._tbpClose.BackColor = System.Drawing.Color.White;
      this._tbpClose.Controls.Add(this._lblCloseInfo);
      this._tbpClose.Controls.Add(this._btnCloseTrade);
      this._tbpClose.Controls.Add(this._nudLotsCloseTrade);
      this._tbpClose.Controls.Add(this._lblSlipPage);
      this._tbpClose.Controls.Add(this._nudSleppage);
      this._tbpClose.Controls.Add(this._lblLotsCloseTrade);
      this._tbpClose.Location = new System.Drawing.Point(23, 4);
      this._tbpClose.Name = "_tbpClose";
      this._tbpClose.Padding = new System.Windows.Forms.Padding(3);
      this._tbpClose.Size = new System.Drawing.Size(259, 142);
      this._tbpClose.TabIndex = 0;
      this._tbpClose.Text = "Close";
      // 
      // _lblCloseInfo
      // 
      this._lblCloseInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblCloseInfo.BackColor = System.Drawing.Color.White;
      this._lblCloseInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._lblCloseInfo.BorderVisible = true;
      this._lblCloseInfo.Location = new System.Drawing.Point(4, 3);
      this._lblCloseInfo.Name = "_lblCloseInfo";
      this._lblCloseInfo.Size = new System.Drawing.Size(251, 81);
      this._lblCloseInfo.TabIndex = 14;
      this._lblCloseInfo.Text = "Text";
      // 
      // _btnCloseTrade
      // 
      this._btnCloseTrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCloseTrade.Location = new System.Drawing.Point(3, 115);
      this._btnCloseTrade.Name = "_btnCloseTrade";
      this._btnCloseTrade.Size = new System.Drawing.Size(253, 24);
      this._btnCloseTrade.TabIndex = 4;
      this._btnCloseTrade.Text = "Close";
      this._btnCloseTrade.UseVisualStyleBackColor = false;
      this._btnCloseTrade.Click += new System.EventHandler(this._btnCloseTrade_Click);
      // 
      // _nudLotsCloseTrade
      // 
      this._nudLotsCloseTrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._nudLotsCloseTrade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLotsCloseTrade.Location = new System.Drawing.Point(42, 90);
      this._nudLotsCloseTrade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLotsCloseTrade.Name = "_nudLotsCloseTrade";
      this._nudLotsCloseTrade.Size = new System.Drawing.Size(48, 20);
      this._nudLotsCloseTrade.TabIndex = 12;
      this._nudLotsCloseTrade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // _lblSlipPage
      // 
      this._lblSlipPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._lblSlipPage.AutoSize = true;
      this._lblSlipPage.Location = new System.Drawing.Point(138, 92);
      this._lblSlipPage.Name = "_lblSlipPage";
      this._lblSlipPage.Size = new System.Drawing.Size(48, 13);
      this._lblSlipPage.TabIndex = 13;
      this._lblSlipPage.Text = "Slippage";
      // 
      // _nudSleppage
      // 
      this._nudSleppage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._nudSleppage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudSleppage.Location = new System.Drawing.Point(207, 90);
      this._nudSleppage.Name = "_nudSleppage";
      this._nudSleppage.Size = new System.Drawing.Size(48, 20);
      this._nudSleppage.TabIndex = 12;
      // 
      // _lblLotsCloseTrade
      // 
      this._lblLotsCloseTrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblLotsCloseTrade.AutoSize = true;
      this._lblLotsCloseTrade.Location = new System.Drawing.Point(9, 92);
      this._lblLotsCloseTrade.Name = "_lblLotsCloseTrade";
      this._lblLotsCloseTrade.Size = new System.Drawing.Size(27, 13);
      this._lblLotsCloseTrade.TabIndex = 13;
      this._lblLotsCloseTrade.Text = "Lots";
      // 
      // _tbpModify
      // 
      this._tbpModify.BackColor = System.Drawing.Color.White;
      this._tbpModify.Controls.Add(this._lblModifyInfo);
      this._tbpModify.Controls.Add(this._btnModifyTrade);
      this._tbpModify.Controls.Add(this._lblslp);
      this._tbpModify.Controls.Add(this._lbltpp);
      this._tbpModify.Controls.Add(this._cnpLimitModify);
      this._tbpModify.Controls.Add(this._cnpStopModify);
      this._tbpModify.Location = new System.Drawing.Point(23, 4);
      this._tbpModify.Name = "_tbpModify";
      this._tbpModify.Padding = new System.Windows.Forms.Padding(3);
      this._tbpModify.Size = new System.Drawing.Size(259, 142);
      this._tbpModify.TabIndex = 1;
      this._tbpModify.Text = "Modify";
      // 
      // _lblModifyInfo
      // 
      this._lblModifyInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblModifyInfo.BackColor = System.Drawing.Color.White;
      this._lblModifyInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._lblModifyInfo.BorderVisible = true;
      this._lblModifyInfo.Location = new System.Drawing.Point(3, 3);
      this._lblModifyInfo.Name = "_lblModifyInfo";
      this._lblModifyInfo.Size = new System.Drawing.Size(253, 64);
      this._lblModifyInfo.TabIndex = 21;
      this._lblModifyInfo.Text = "Text";
      // 
      // _btnModifyTrade
      // 
      this._btnModifyTrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._btnModifyTrade.Location = new System.Drawing.Point(3, 115);
      this._btnModifyTrade.Name = "_btnModifyTrade";
      this._btnModifyTrade.Size = new System.Drawing.Size(253, 24);
      this._btnModifyTrade.TabIndex = 4;
      this._btnModifyTrade.Text = "Modify";
      this._btnModifyTrade.UseVisualStyleBackColor = false;
      this._btnModifyTrade.Click += new System.EventHandler(this._btnModifyTrade_Click);
      // 
      // _lblslp
      // 
      this._lblslp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblslp.AutoSize = true;
      this._lblslp.Location = new System.Drawing.Point(71, 97);
      this._lblslp.Name = "_lblslp";
      this._lblslp.Size = new System.Drawing.Size(19, 13);
      this._lblslp.TabIndex = 20;
      this._lblslp.Text = "=0";
      this._lblslp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _lbltpp
      // 
      this._lbltpp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._lbltpp.AutoSize = true;
      this._lbltpp.Location = new System.Drawing.Point(186, 97);
      this._lbltpp.Name = "_lbltpp";
      this._lbltpp.Size = new System.Drawing.Size(19, 13);
      this._lbltpp.TabIndex = 20;
      this._lbltpp.Text = "=0";
      this._lbltpp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _cnpLimitModify
      // 
      this._cnpLimitModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._cnpLimitModify.Caption = "Limit";
      this._cnpLimitModify.Checked = false;
      this._cnpLimitModify.DecimalDigits = 0;
      this._cnpLimitModify.Location = new System.Drawing.Point(120, 73);
      this._cnpLimitModify.Maximum = 100F;
      this._cnpLimitModify.Minimum = 0F;
      this._cnpLimitModify.Name = "_cnpLimitModify";
      this._cnpLimitModify.Size = new System.Drawing.Size(64, 40);
      this._cnpLimitModify.TabIndex = 18;
      this._cnpLimitModify.Value = float.NaN;
      this._cnpLimitModify.ValueChanged += new System.EventHandler(this._cnpTP_ValueChanged);
      this._cnpLimitModify.CheckedChanged += new System.EventHandler(this._cnpTP_CheckedChanged);
      // 
      // _cnpStopModify
      // 
      this._cnpStopModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._cnpStopModify.Caption = "Stop";
      this._cnpStopModify.Checked = false;
      this._cnpStopModify.DecimalDigits = 0;
      this._cnpStopModify.Location = new System.Drawing.Point(6, 73);
      this._cnpStopModify.Maximum = 100F;
      this._cnpStopModify.Minimum = 0F;
      this._cnpStopModify.Name = "_cnpStopModify";
      this._cnpStopModify.Size = new System.Drawing.Size(64, 40);
      this._cnpStopModify.TabIndex = 18;
      this._cnpStopModify.Value = float.NaN;
      this._cnpStopModify.ValueChanged += new System.EventHandler(this._cnpSL_ValueChanged);
      this._cnpStopModify.CheckedChanged += new System.EventHandler(this._cnpSL_CheckedChanged);
      // 
      // _tbpOpen
      // 
      this._tbpOpen.BackColor = System.Drawing.Color.White;
      this._tbpOpen.Controls.Add(this.panelExt1);
      this._tbpOpen.Controls.Add(this._chkOneClick);
      this._tbpOpen.Controls.Add(this._lblOpenInfo);
      this._tbpOpen.Controls.Add(this._btnBuy);
      this._tbpOpen.Controls.Add(this._btnSell);
      this._tbpOpen.Controls.Add(this._cmbSymbols);
      this._tbpOpen.Location = new System.Drawing.Point(23, 4);
      this._tbpOpen.Margin = new System.Windows.Forms.Padding(0);
      this._tbpOpen.Name = "_tbpOpen";
      this._tbpOpen.Size = new System.Drawing.Size(259, 142);
      this._tbpOpen.TabIndex = 2;
      this._tbpOpen.Text = "Open";
      // 
      // panelExt1
      // 
      this.panelExt1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelExt1.BackColor = System.Drawing.Color.White;
      this.panelExt1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this.panelExt1.BorderVisible = true;
      this.panelExt1.Controls.Add(this._chkStop);
      this.panelExt1.Controls.Add(this._nudStopOpen);
      this.panelExt1.Controls.Add(this._lblLotsOperate);
      this.panelExt1.Controls.Add(this._nudLimitOpen);
      this.panelExt1.Controls.Add(this._nudLotsOpen);
      this.panelExt1.Controls.Add(this._chkLimit);
      this.panelExt1.Controls.Add(this._lblSlipPageOpen);
      this.panelExt1.Controls.Add(this._nudSlippageOpen);
      this.panelExt1.Location = new System.Drawing.Point(3, 88);
      this.panelExt1.Name = "panelExt1";
      this.panelExt1.Size = new System.Drawing.Size(253, 54);
      this.panelExt1.TabIndex = 40;
      // 
      // _chkStop
      // 
      this._chkStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._chkStop.Checked = true;
      this._chkStop.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkStop.Location = new System.Drawing.Point(130, 6);
      this._chkStop.Name = "_chkStop";
      this._chkStop.Size = new System.Drawing.Size(62, 20);
      this._chkStop.TabIndex = 42;
      this._chkStop.Text = "Stop";
      this._chkStop.CheckedChanged += new System.EventHandler(this._chkStop_CheckedChanged);
      // 
      // _nudStopOpen
      // 
      this._nudStopOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._nudStopOpen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudStopOpen.Enabled = false;
      this._nudStopOpen.Location = new System.Drawing.Point(200, 5);
      this._nudStopOpen.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this._nudStopOpen.Name = "_nudStopOpen";
      this._nudStopOpen.Size = new System.Drawing.Size(49, 20);
      this._nudStopOpen.TabIndex = 43;
      this._nudStopOpen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudStopOpen.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      // 
      // _lblLotsOperate
      // 
      this._lblLotsOperate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblLotsOperate.AutoSize = true;
      this._lblLotsOperate.Location = new System.Drawing.Point(3, 7);
      this._lblLotsOperate.Name = "_lblLotsOperate";
      this._lblLotsOperate.Size = new System.Drawing.Size(27, 13);
      this._lblLotsOperate.TabIndex = 38;
      this._lblLotsOperate.Text = "Lots";
      // 
      // _nudLimitOpen
      // 
      this._nudLimitOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._nudLimitOpen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLimitOpen.Enabled = false;
      this._nudLimitOpen.Location = new System.Drawing.Point(200, 28);
      this._nudLimitOpen.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this._nudLimitOpen.Name = "_nudLimitOpen";
      this._nudLimitOpen.Size = new System.Drawing.Size(49, 20);
      this._nudLimitOpen.TabIndex = 44;
      this._nudLimitOpen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudLimitOpen.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      // 
      // _nudLotsOpen
      // 
      this._nudLotsOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._nudLotsOpen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLotsOpen.Location = new System.Drawing.Point(66, 5);
      this._nudLotsOpen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLotsOpen.Name = "_nudLotsOpen";
      this._nudLotsOpen.Size = new System.Drawing.Size(48, 20);
      this._nudLotsOpen.TabIndex = 37;
      this._nudLotsOpen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudLotsOpen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // _chkLimit
      // 
      this._chkLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._chkLimit.Checked = true;
      this._chkLimit.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkLimit.Location = new System.Drawing.Point(130, 28);
      this._chkLimit.Name = "_chkLimit";
      this._chkLimit.Size = new System.Drawing.Size(62, 20);
      this._chkLimit.TabIndex = 41;
      this._chkLimit.Text = "Limit";
      this._chkLimit.CheckedChanged += new System.EventHandler(this._chkLimit_CheckedChanged);
      // 
      // _lblSlipPageOpen
      // 
      this._lblSlipPageOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblSlipPageOpen.AutoSize = true;
      this._lblSlipPageOpen.Location = new System.Drawing.Point(3, 30);
      this._lblSlipPageOpen.Name = "_lblSlipPageOpen";
      this._lblSlipPageOpen.Size = new System.Drawing.Size(48, 13);
      this._lblSlipPageOpen.TabIndex = 40;
      this._lblSlipPageOpen.Text = "Slippage";
      // 
      // _nudSlippageOpen
      // 
      this._nudSlippageOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._nudSlippageOpen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudSlippageOpen.Location = new System.Drawing.Point(66, 28);
      this._nudSlippageOpen.Name = "_nudSlippageOpen";
      this._nudSlippageOpen.Size = new System.Drawing.Size(48, 20);
      this._nudSlippageOpen.TabIndex = 39;
      this._nudSlippageOpen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // _chkOneClick
      // 
      this._chkOneClick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._chkOneClick.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._chkOneClick.Checked = true;
      this._chkOneClick.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkOneClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkOneClick.Location = new System.Drawing.Point(3, 69);
      this._chkOneClick.Name = "_chkOneClick";
      this._chkOneClick.Size = new System.Drawing.Size(153, 17);
      this._chkOneClick.TabIndex = 38;
      this._chkOneClick.Text = "OnClick";
      this._chkOneClick.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _lblOpenInfo
      // 
      this._lblOpenInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblOpenInfo.BackColor = System.Drawing.Color.White;
      this._lblOpenInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this._lblOpenInfo.BorderVisible = true;
      this._lblOpenInfo.Location = new System.Drawing.Point(162, 24);
      this._lblOpenInfo.Name = "_lblOpenInfo";
      this._lblOpenInfo.Size = new System.Drawing.Size(94, 62);
      this._lblOpenInfo.TabIndex = 37;
      this._lblOpenInfo.Text = "Text";
      // 
      // _btnBuy
      // 
      this._btnBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._btnBuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(255)))), ((int)(((byte)(216)))));
      this._btnBuy.BigFont = new System.Drawing.Font("Microsoft Sans Serif", 19.09998F, System.Drawing.FontStyle.Bold);
      this._btnBuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this._btnBuy.Cursor = System.Windows.Forms.Cursors.Hand;
      this._btnBuy.Location = new System.Drawing.Point(81, 0);
      this._btnBuy.Margin = new System.Windows.Forms.Padding(0);
      this._btnBuy.Name = "_btnBuy";
      this._btnBuy.Price = 0.0001F;
      this._btnBuy.Size = new System.Drawing.Size(78, 66);
      this._btnBuy.SmallFont = new System.Drawing.Font("Microsoft Sans Serif", 10.79999F, System.Drawing.FontStyle.Bold);
      this._btnBuy.Symbol = null;
      this._btnBuy.TabIndex = 8;
      this._btnBuy.UseToolTip = false;
      this._btnBuy.Click += new System.EventHandler(this._btnBuy_Click);
      this._btnBuy.MouseMove += new System.Windows.Forms.MouseEventHandler(this._btnBuy_MouseMove);
      this._btnBuy.MouseEnter += new System.EventHandler(this._btnBuy_MouseEnter);
      this._btnBuy.MouseLeave += new System.EventHandler(this._btnBuy_MouseLeave);
      // 
      // _btnSell
      // 
      this._btnSell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._btnSell.BackColor = System.Drawing.Color.White;
      this._btnSell.BigFont = new System.Drawing.Font("Microsoft Sans Serif", 19.09998F, System.Drawing.FontStyle.Bold);
      this._btnSell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this._btnSell.Cursor = System.Windows.Forms.Cursors.Hand;
      this._btnSell.Location = new System.Drawing.Point(3, 0);
      this._btnSell.Margin = new System.Windows.Forms.Padding(0);
      this._btnSell.Name = "_btnSell";
      this._btnSell.Price = 0.0001F;
      this._btnSell.Size = new System.Drawing.Size(78, 66);
      this._btnSell.SmallFont = new System.Drawing.Font("Microsoft Sans Serif", 10.69999F, System.Drawing.FontStyle.Bold);
      this._btnSell.Symbol = null;
      this._btnSell.TabIndex = 7;
      this._btnSell.UseToolTip = false;
      this._btnSell.Click += new System.EventHandler(this._btnSell_Click);
      this._btnSell.MouseMove += new System.Windows.Forms.MouseEventHandler(this._btnSell_MouseMove);
      this._btnSell.MouseEnter += new System.EventHandler(this._btnSell_MouseEnter);
      this._btnSell.MouseLeave += new System.EventHandler(this._btnSell_MouseLeave);
      // 
      // _cmbSymbols
      // 
      this._cmbSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbSymbols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbSymbols.FormattingEnabled = true;
      this._cmbSymbols.Location = new System.Drawing.Point(162, 0);
      this._cmbSymbols.Name = "_cmbSymbols";
      this._cmbSymbols.Size = new System.Drawing.Size(94, 21);
      this._cmbSymbols.TabIndex = 6;
      this._cmbSymbols.SelectedIndexChanged += new System.EventHandler(this._cmbSymbols_SelectedIndexChanged);
      // 
      // _btnMinMax
      // 
      this._btnMinMax.Alignment = Gordago.Analysis.Chart.ExtButton.ExtButtonAlignment.Left;
      this._btnMinMax.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._btnMinMax.BorderVisible = true;
      this._btnMinMax.Checked = true;
      this._btnMinMax.ExtButtonType = Gordago.Analysis.Chart.ExtButtonType.Hide;
      this._btnMinMax.Location = new System.Drawing.Point(1, 1);
      this._btnMinMax.Name = "_btnMinMax";
      this._btnMinMax.Size = new System.Drawing.Size(12, 12);
      this._btnMinMax.TabIndex = 26;
      this._btnMinMax.CheckedChanged += new System.EventHandler(this._btnMinMax_CheckedChanged);
      // 
      // TradersOperatePanel
      // 
      this.Controls.Add(this._btnMinMax);
      this.Controls.Add(this._tbcMain);
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "TradersOperatePanel";
      this.Size = new System.Drawing.Size(300, 150);
      this._tbcMain.ResumeLayout(false);
      this._tbpClose.ResumeLayout(false);
      this._tbpClose.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsCloseTrade)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudSleppage)).EndInit();
      this._tbpModify.ResumeLayout(false);
      this._tbpModify.PerformLayout();
      this._tbpOpen.ResumeLayout(false);
      this.panelExt1.ResumeLayout(false);
      this.panelExt1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudStopOpen)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimitOpen)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsOpen)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudSlippageOpen)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

    #region private int GetStopPoint(float price)
    private int GetStopPoint(float price) {
      ISymbol symbol = _trade.OnlineRate.Symbol;
      float rate = _trade.OpenRate;
      if (_trade.TradeType == TradeType.Buy) {
        return Broker.CalculatePoint(_trade.OnlineRate.SellRate, price, symbol.DecimalDigits, false);
      } else {
        return Broker.CalculatePoint(price, _trade.OnlineRate.BuyRate, symbol.DecimalDigits, false);
      }
    }
    #endregion

    #region private int GetLimitPoint(float price)
    private int GetLimitPoint(float price) {
      ISymbol symbol = _trade.OnlineRate.Symbol;

      if (_trade.TradeType == TradeType.Buy) {
        return Broker.CalculatePoint(price, _trade.OnlineRate.SellRate, symbol.DecimalDigits, false);
      } else {
        return Broker.CalculatePoint(_trade.OnlineRate.BuyRate, price, symbol.DecimalDigits, false);
      }
    }
    #endregion

    #region private void _btnCloseTrade_Click(object sender, System.EventArgs e)
    private void _btnCloseTrade_Click(object sender, System.EventArgs e) {
      if (_trade == null)
        return;
      int lots = Convert.ToInt32(this._nudLotsCloseTrade.Value);
      int slippage = Convert.ToInt32(this._nudSleppage.Value);
      //      API.APITrader.Log.Add(String.Format("_api.CloseTrade(trade.TradeId={0}, lots={1})", _trade.TradeId, lots));
      BrokerCommandTradeClose command = new BrokerCommandTradeClose(_trade, lots, slippage, "");
      BCM.ExecuteCommand(command);
    }
    #endregion

    #region private void _btnModifyTrade_Click(object sender, System.EventArgs e)
    private void _btnModifyTrade_Click(object sender, System.EventArgs e) {
      if (_trade == null)
        return;
      BrokerCommandTradeModify command = new BrokerCommandTradeModify(_trade, _cnpStopModify.Value, _cnpLimitModify.Value);
      BCM.ExecuteCommand(command);
    }
    #endregion

    #region private void _cnpSL_CheckedChanged(object sender, System.EventArgs e)
    private void _cnpSL_CheckedChanged(object sender, System.EventArgs e) {
      if (_cnpStopModify.Checked) {
        if (_trade.StopOrder != null) {
          this._cnpStopModify.Value = _trade.StopOrder.Rate;
        } else {
          this._cnpStopModify.Value = this.GetMinimumStopPrice(_trade.TradeType);
          _cnpStopModify.Value += (_trade.TradeType == TradeType.Sell ? ADDED_POINT : -ADDED_POINT) * _trade.OnlineRate.Symbol.Point;
        }
      } else {
        this._cnpStopModify.Value = 0;
      }

      this.OnTradeChanged();
    }
    #endregion

    #region private void _cnpTP_CheckedChanged(object sender, System.EventArgs e)
    private void _cnpTP_CheckedChanged(object sender, System.EventArgs e) {
      if (_cnpLimitModify.Checked) {
        if (_trade.LimitOrder != null) {
          this._cnpLimitModify.Value = _trade.LimitOrder.Rate;
        } else {
          this._cnpLimitModify.Value = this.GetMiminumLimitPrice(_trade.TradeType);
          _cnpLimitModify.Value += (_trade.TradeType == TradeType.Sell ? -ADDED_POINT : ADDED_POINT) * _trade.OnlineRate.Symbol.Point;
        }
      } else {
        this._cnpLimitModify.Value = 0;
      }

      this.OnTradeChanged();
    }
    #endregion

    #region private float GetMinimumStopPrice(TradeType tradeType)
    private float GetMinimumStopPrice(TradeType tradeType) {
      float minprice;
      if (tradeType == TradeType.Sell) {
        minprice = _trade.OnlineRate.BuyRate;
      } else {
        minprice = _trade.OnlineRate.SellRate;
      }
      return minprice;
    }
    #endregion

    #region private float GetMiminumLimitPrice(TradeType buysell)
    private float GetMiminumLimitPrice(TradeType buysell) {
      float minprice;
      if (buysell == TradeType.Sell) {
        minprice = _trade.OnlineRate.BuyRate;
      } else {
        minprice = _trade.OnlineRate.SellRate;
      }
      return minprice;
    }
    #endregion

    #region private void _cnpSL_ValueChanged(object sender, System.EventArgs e)
    private void _cnpSL_ValueChanged(object sender, System.EventArgs e) {
      this.OnTradeChanged();
    }
    #endregion

    #region private void _cnpTP_ValueChanged(object sender, System.EventArgs e)
    private void _cnpTP_ValueChanged(object sender, System.EventArgs e) {
      this.OnTradeChanged();
    }
    #endregion

    #region private void _cnpTS_CheckedChanged(object sender, System.EventArgs e)
    private void _cnpTS_CheckedChanged(object sender, System.EventArgs e) {
      //			if (_cnpTS.Checked){
      //				if (_trade.StopOrder != null){
      //					this._cnpSL.Value = _trade.StopOrder.Rate;
      //				}else{
      //					this._cnpSL.Value = this.GetMinimumStopPrice(_trade.TradeType);
      //					_cnpSL.Value += (_trade.TradeType == TradeType.Sell ? ADDED_POINT : -ADDED_POINT) * _trade.OnlineRate.Symbol.Point;
      //				}
      //			}else{
      //				this._cnpSL.Value = 0;
      //			}
      //			this.RefreshPanel();
    }
    #endregion

    #region private void _cnpTS_ValueChanged(object sender, System.EventArgs e)
    private void _cnpTS_ValueChanged(object sender, System.EventArgs e) {
      this.OnTradeChanged();
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      Graphics g = e.Graphics;
      Color c = this.Focused ? Color.FromArgb(53, 116, 215) : Color.FromArgb(216, 212, 202);
      g.Clear(c);
      _btnMinMax.BackColor = c;

      this.DrawVerticalString(g, this.Text, 1, 18, 14, this.Height - 30);

      g.DrawRectangle(_borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }
    #endregion

    #region private void _btnMinMax_CheckedChanged(object sender, EventArgs e)
    private void _btnMinMax_CheckedChanged(object sender, EventArgs e) {
      if (!this._btnMinMax.Checked) {
        this._tbcMain.Visible = false;
        this.Width = 15;
      } else {
        this._tbcMain.Visible = true;
        this.Width = 300;
      }
      this.Refresh();
      if (this.StatusChanged != null)
        this.StatusChanged(this, new EventArgs());
    }
    #endregion

    #region private void DrawVerticalString(Graphics g, string s, int x, int y, int width, int height)
    private void DrawVerticalString(Graphics g, string s, int x, int y, int width, int height) {
      Bitmap bitmap = new Bitmap(width, height);
      Graphics gbm = Graphics.FromImage(bitmap);


      RectangleF rect = new RectangleF(-height, 0, height, width);
      gbm.RotateTransform(-90, System.Drawing.Drawing2D.MatrixOrder.Append);

      using (StringFormat sformat = new StringFormat()) {
        sformat.Alignment = StringAlignment.Center;
        sformat.LineAlignment = StringAlignment.Center;
        gbm.DrawString(s, this.FontVerticalText, new SolidBrush(this.ForeColor), rect, sformat);
      }
      gbm.Flush();
      g.DrawImageUnscaled(bitmap, x, y);
    }
    #endregion

    #region private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e)
    private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e) {
      this.InitOpenTab();
    }
    #endregion

    #region IBrokerEvents Members

    #region public void BrokerConnectionStatusChanged(BrokerConnectionStatus status)
    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      this.InitOpenTab();
      this.UpdateOpenTab();
    }
    #endregion

    #region public void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
    }
    #endregion

    #region public void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
    }
    #endregion

    #region public void BrokerTradesChanged(BrokerTradesEventArgs be)
    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
    }
    #endregion

    #region public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      if (be.OnlineRate.Symbol == (this._cmbSymbols.SelectedItem as ISymbol))
        this.UpdateOpenTab();

      if (_trade != null && _trade.OnlineRate == be.OnlineRate)
          this.OnTradeChanged();
    }
    #endregion

    #region public void BrokerCommandStarting(BrokerCommand command)
    public void BrokerCommandStarting(BrokerCommand command) {
      this.Trade = null;
    }
    #endregion

    #region public void BrokerCommandStopping(BrokerCommand command, BrokerResult result)
    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      this.OnTradeChanged();
    }
    #endregion
    #endregion


  }
}
