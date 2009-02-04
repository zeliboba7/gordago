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
using Gordago.Analysis.Chart;
using Language;

namespace Gordago.API {
  public partial class HandOperateChartPanel : ChartPanel, IBrokerEvents{

    private IOnlineRate _onlineRate;
    private ISymbol _symbol;

    private BrokerConnectionStatus _savedAPIStatus = BrokerConnectionStatus.WaitingForConnection;

    public HandOperateChartPanel() {
      
      InitializeComponent();
      try {
        this._btnSell.Text = Dictionary.GetString(33, 1, "Продать");
        this._btnBuy.Text = Dictionary.GetString(33, 2, "Купить");
        this._lblLotsOperate.Text = Dictionary.GetString(33, 3, "Объем");
        this._chkOneClick.Text = Dictionary.GetString(33, 4, "Один клик");
        this._chkExtended.Text = Dictionary.GetString(33, 5, "Все");
        this._lblSlipPage.Text = Dictionary.GetString(33, 6, "Отклонение");
        this._chkPO.Text = Dictionary.GetString(33, 7, "Отложенный");
        this._chkStop.Text = Dictionary.GetString(33, 8, "Стоп");
        this._chkLimit.Text = Dictionary.GetString(33, 9, "Лимит");
      } catch { }

      this.Properties = new HOCPProperties(this);
    }

    #region public bool OneClick
    public bool OneClick {
      get { return this._chkOneClick.Checked; }
      set { this._chkOneClick.Checked = value; }
    }
    #endregion

    #region public bool public bool AllProperty
    public bool AllProperty {
      get { return this._chkExtended.Checked; }
      set { 
        this._chkExtended.Checked = value;
        this.CheckSize();
      }
    }
    #endregion

    #region public Color BidAskForeColor
    public Color BidAskForeColor {
      get { return this._lblask.ForeColor; }
      set { 
        this._lblask.ForeColor = this._lblBid.ForeColor = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Color BidAskBackColor
    public Color BidAskBackColor {
      get { return this._lblask.BackColor; }
      set { 
        this._lblBid.BackColor = this._lblask.BackColor = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Color SellForeColor
    public Color SellForeColor {
      get { return this._btnSell.ForeColor; }
      set { this._btnSell.ForeColor = value; }
    }
    #endregion

    #region public Color SellBackColor
    public Color SellBackColor {
      get { return this._btnSell.BackColor; }
      set { this._btnSell.BackColor = value; }
    }
    #endregion

    #region public Color SellBorderColor
    public Color SellBorderColor {
      get { return this._btnSell.BorderColor; }
      set { this._btnSell.BorderColor = value; }
    }
    #endregion

    #region public Color BuyForeColor
    public Color BuyForeColor {
      get { return this._btnBuy.ForeColor; }
      set { this._btnBuy.ForeColor = value; }
    }
    #endregion

    #region public Color BuyBackColor
    public Color BuyBackColor {
      get { return this._btnBuy.BackColor; }
      set { this._btnBuy.BackColor = value; }
    }
    #endregion

    #region public Color BuyBorderColor
    public Color BuyBorderColor {
      get { return this._btnBuy.BorderColor; }
      set { this._btnBuy.BorderColor = value; }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public void SetSymbol(ISymbol symbol)
    public void SetSymbol(ISymbol symbol) {
      _symbol = symbol;
      _btnSell.Symbol = symbol;
      _btnBuy.Symbol = symbol;
      this.Text = symbol.Name;
      this.UpdateStatus();
    }
    #endregion

    #region private void _nudLots_ValueChanged(object sender, EventArgs e)
    private void _nudLots_ValueChanged(object sender, EventArgs e) {
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _chkPO_CheckedChanged(object sender, EventArgs e)
    private void _chkPO_CheckedChanged(object sender, EventArgs e) {

      if (_chkPO.Checked && _onlineRate != null) {
        this._nudPO.DecimalPlaces = _onlineRate.Symbol.DecimalDigits;
        this._nudPO.Minimum = 0;
        this._nudPO.Maximum = Convert.ToDecimal(_onlineRate.SellRate * 100);
        this._nudPO.Value = Convert.ToDecimal(_onlineRate.SellRate);
        decimal point = (decimal)1 / (decimal)SymbolManager.GetDelimiter(_onlineRate.Symbol.DecimalDigits);
        this._nudPO.Increment = point;

      } else {
        this._nudPO.Value = 0;
      }
      _nudSlipPage.Enabled =
        _lblSlipPage.Enabled = !_chkPO.Checked;
      _nudPO.Enabled = _chkPO.Checked;

      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _nudPO_ValueChanged(object sender, EventArgs e)
    private void _nudPO_ValueChanged(object sender, EventArgs e) {
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _chkStop_CheckedChanged(object sender, EventArgs e)
    private void _chkStop_CheckedChanged(object sender, EventArgs e) {
      _nudStop.Enabled = _chkStop.Checked && _onlineRate != null;
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _nudStop_ValueChanged(object sender, EventArgs e)
    private void _nudStop_ValueChanged(object sender, EventArgs e) {
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _chkLimit_CheckedChanged(object sender, EventArgs e)
    private void _chkLimit_CheckedChanged(object sender, EventArgs e) {
      this._nudLimit.Enabled = this._chkLimit.Checked && _onlineRate != null;
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _nudLimit_ValueChanged(object sender, EventArgs e)
    private void _nudLimit_ValueChanged(object sender, EventArgs e) {
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _chkExtended_CheckedChanged(object sender, EventArgs e)
    private void _chkExtended_CheckedChanged(object sender, EventArgs e) {
      this.CheckSize();
    }
    #endregion

    #region private void CheckSize()
    private void CheckSize() {
      bool resize = this._gboxPO.Enabled != this._chkExtended.Checked;

      this._gboxPO.Visible =
        this._gboxPO.Enabled =
        this._gboxSL.Visible =
        this._gboxSlippage.Visible = this._chkExtended.Checked;

      if (resize) {
        int dh = 113;
        this.SuspendLayout();
        if (this._chkExtended.Checked) {
          this.Height = this.Height + dh;
        } else {
          this.Height = this.Height - dh;
        }
        this.ResumeLayout(false);
      }
      if (!this._chkExtended.Checked)
        this._chkPO.Checked = this._chkStop.Checked = this._chkLimit.Checked = false;
    }
    #endregion

    #region private void RefreshInfoPanels()
    private void RefreshInfoPanels() {
      if (_onlineRate == null) {
        _tprevSell.Clear();
        _tprevBuy.Clear();
        this._btnSell.SetToolTipText("Offline");
        this._btnBuy.SetToolTipText("Offline");
        this._btnSell.Price = this._btnBuy.Price = 0;
        return;
      }
      _btnSell.Price = _onlineRate.SellRate;
      _btnBuy.Price = _onlineRate.BuyRate;

      _tprevSell.SetTrade(_onlineRate, Convert.ToInt32(_nudLots.Value),
        _chkPO.Checked, Convert.ToSingle(_nudPO.Value),
        _chkStop.Checked, Convert.ToInt32(_nudStop.Value),
        _chkLimit.Checked, Convert.ToInt32(_nudLimit.Value));

      _tprevBuy.SetTrade(_onlineRate, Convert.ToInt32(_nudLots.Value),
        _chkPO.Checked, Convert.ToSingle(_nudPO.Value),
        _chkStop.Checked, Convert.ToInt32(_nudStop.Value),
        _chkLimit.Checked, Convert.ToInt32(_nudLimit.Value));

      this._btnSell.Price = _onlineRate.SellRate;
      this._btnBuy.Price = _onlineRate.BuyRate;

      string strSell = Dictionary.GetString(33, 1, "Продать") + "\n" + _tprevSell.Text;
      this._btnSell.SetToolTipText(strSell);

      string strBuy = Dictionary.GetString(33, 2, "Купить") + "\n" + _tprevBuy.Text;
      this._btnBuy.SetToolTipText(strBuy);
    }
    #endregion

    #region private void UpdateStatus()
    /// <summary>
    /// Главный метод проверки статуса. Вызывается при изменение цены, статуса коннекта
    /// </summary>
    private void UpdateStatus() {
      if (_savedAPIStatus != this.BCM.ConnectionStatus) {
        if (BCM.ConnectionStatus != BrokerConnectionStatus.Online) {
          _onlineRate = null;
        } else {
          if (_symbol != null)
            _onlineRate = BCM.Broker.OnlineRates.GetOnlineRate(_symbol.Name);
        }
        _savedAPIStatus = BCM.ConnectionStatus;
      }

      this._lblWait.Visible = !BCM.Busy;
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _btnBuyOperate_Click(object sender, EventArgs e)
    private void _btnBuyOperate_Click(object sender, EventArgs e) {
      this.SendOrderFromHandOperate(TradeType.Buy);
    }
    #endregion

    #region private void _btnSellOperate_Click(object sender, EventArgs e)
    private void _btnSellOperate_Click(object sender, EventArgs e) {
      this.SendOrderFromHandOperate(TradeType.Sell);
    }
    #endregion

    #region private void SendOrderFromHandOperate(TradeType buysell)
    private void SendOrderFromHandOperate(TradeType buysell) {

      if (!this.BCM.Busy) return;

      if (!this._chkOneClick.Checked) {
        string strSell = Dictionary.GetString(33, 1, "Продать") + "\n" + _tprevSell.Text;
        string strBuy = Dictionary.GetString(33, 2, "Купить") + "\n" + _tprevBuy.Text;

        string text = buysell == TradeType.Sell ? strSell : strBuy;

        HOCPDialog hocpd = new HOCPDialog(text);
        if (hocpd.ShowDialog() != DialogResult.OK)
          return;
      }
      int lots = Convert.ToInt32(_nudLots.Value);
      int slippage = Convert.ToInt32(_nudSlipPage.Value);

      float stop = buysell == TradeType.Sell ? _tprevSell.PriceStop : _tprevBuy.PriceStop;
      float limit = buysell == TradeType.Sell ? _tprevSell.PriceLimit : _tprevBuy.PriceLimit;

      if (this._chkPO.Checked) {
        bool error = buysell == TradeType.Sell ? _tprevSell.OrderStopLimitError : _tprevBuy.OrderStopLimitError;
        if (error) return;

        OrderType stoplimit = buysell == TradeType.Sell ? _tprevSell.OrderType : _tprevBuy.OrderType;

        float orderprice = Convert.ToSingle(this._nudPO.Value);
        BrokerCommandEntryOrderCreate command =
          new BrokerCommandEntryOrderCreate(GordagoMain.MainForm.DefaultAccountId, _onlineRate, stoplimit, buysell, lots, orderprice, stop, limit, "");
        BCM.ExecuteCommand(command);
      } else {
        BrokerCommandTradeOpen command = 
          new BrokerCommandTradeOpen(GordagoMain.MainForm.DefaultAccountId, _onlineRate, buysell, lots, slippage, stop, limit, "");
        BCM.ExecuteCommand(command);
      }
    }
    #endregion

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      this.UpdateStatus();
    }

    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      
    }

    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      
    }

    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      
    }

    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      if (this._symbol != be.OnlineRate.Symbol)
        return;

      UpdateStatus();
    }

    public void BrokerCommandStarting(BrokerCommand command) {
      this.UpdateStatus();
    }

    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      this.UpdateStatus();
    }
  }
}
