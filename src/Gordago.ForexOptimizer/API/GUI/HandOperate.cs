/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Cursit.Applic.AConfig;
using Language;
#endregion

namespace Gordago.API {
  public partial class HandOperate:UserControl, IBrokerEvents {

    private ISymbol _symbol;
    private IOnlineRate _onlineRate;

    public HandOperate() {
      InitializeComponent();
      try {
        this._btnSellOperate.Text = Dictionary.GetString(33, 1, "Продать");
        this._btnBuyOperate.Text = Dictionary.GetString(33, 2, "Купить");
        this._lblLotsOperate.Text = Dictionary.GetString(33, 3, "Объем");
        this._lblSlipPage.Text = Dictionary.GetString(33, 6, "Отклонение");
        this._chkPO.Text = Dictionary.GetString(33, 7, "Отложенный");
        this._chkStop.Text = Dictionary.GetString(33, 8, "Стоп");
        this._chkLimit.Text = Dictionary.GetString(33, 9, "Лимит");

      } catch { }
    }

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public void SetSymbol(ISymbol symbol)
    public void SetSymbol(ISymbol symbol) {
      IOnlineRate oldpair = _onlineRate;
      if(oldpair != null) {
        this.SaveConfig(oldpair);
      }
      _symbol = symbol;
      bool online = BCM.ConnectionStatus == BrokerConnectionStatus.Online;
      if(online && symbol != null) {
        _onlineRate = BCM.Broker.OnlineRates.GetOnlineRate(symbol.Name);
      } else {
        _onlineRate = null;
      }
      if(_onlineRate != null && _onlineRate != oldpair) {
        this.LoadConfig(_onlineRate);
      }
      RefreshInfoPanels();
      RefreshSymbolLabel();
    }
    #endregion

    #region private void SaveConfig(IOnlineRate onlineRate)
    private void SaveConfig(IOnlineRate onlineRate) {
      if(onlineRate == null) return;

      ConfigValue cfgval = Config.Users["Terminal"][onlineRate.Symbol.Name];
      cfgval["Lots"].SetValue(Convert.ToInt32(_nudLots.Value));
      cfgval["Slippage"].SetValue(Convert.ToInt32(_nudSlipPage.Value));

      cfgval["UseStop"].SetValue(_chkStop.Checked);
      cfgval["Stop"].SetValue(Convert.ToInt32(_nudStop.Value));
      cfgval["UseLimit"].SetValue(_chkLimit.Checked);
      cfgval["Limit"].SetValue(Convert.ToInt32(_nudLimit.Value));

      cfgval["UsePO"].SetValue(false);
    }
    #endregion

    #region private void LoadConfig(IOnlineRate onlineRate)
    private void LoadConfig(IOnlineRate onlineRate) {

      if(onlineRate == null) return;

      ConfigValue cfgval = Config.Users["Terminal"][onlineRate.Symbol.Name];
      _nudLots.Value = cfgval["Lots", 1];
      _nudSlipPage.Value = cfgval["Slippage", 0];
      _chkStop.Checked = cfgval["UseStop", false];
      _nudStop.Value = cfgval["Stop", 20];
      _chkLimit.Checked = cfgval["UseLimit", false];
      _nudLimit.Value = cfgval["Limit", 50];

      _chkPO.Checked = cfgval["UsePO", false];
    }
    #endregion

    #region private void RefreshInfoPanels()
    private void RefreshInfoPanels() {
      if(_onlineRate == null) {
        _tprevSell.Clear();
        _tprevBuy.Clear();
        return;
      }
      _tprevSell.SetTrade(_onlineRate, Convert.ToInt32(_nudLots.Value),
        _chkPO.Checked, Convert.ToSingle(_nudPO.Value),
        _chkStop.Checked, Convert.ToInt32(_nudStop.Value),
        _chkLimit.Checked, Convert.ToInt32(_nudLimit.Value));

      _tprevBuy.SetTrade(_onlineRate, Convert.ToInt32(_nudLots.Value),
        _chkPO.Checked, Convert.ToSingle(_nudPO.Value),
        _chkStop.Checked, Convert.ToInt32(_nudStop.Value),
        _chkLimit.Checked, Convert.ToInt32(_nudLimit.Value));
    }
    #endregion

    #region private void RefreshSymbolLabel()
    private void RefreshSymbolLabel() {

      if(_onlineRate == null) return;

      this._lblSymbolOperate.Text = _onlineRate.Symbol.Name;
      this._lblBidOperate.Text = SymbolManager.ConvertToCurrencyString(_onlineRate.SellRate, _symbol.DecimalDigits);
      this._lblAskOperate.Text = SymbolManager.ConvertToCurrencyString(_onlineRate.BuyRate, _symbol.DecimalDigits);

      if(_onlineRate.SellRate > _onlineRate.LastSellRate) {
        this._lblSymbolOperate.ForeColor =
         this._lblBidOperate.ForeColor = this._lblAskOperate.ForeColor = Color.Blue;
      } else if (_onlineRate.SellRate < _onlineRate.LastSellRate) {
        this._lblSymbolOperate.ForeColor =
          this._lblBidOperate.ForeColor = this._lblAskOperate.ForeColor = Color.Red;
      } else {
        this._lblSymbolOperate.ForeColor =
          this._lblBidOperate.ForeColor = this._lblAskOperate.ForeColor = Color.Black;
      }
    }
    #endregion

    #region private void SendOrderFromHandOperate(TradeType buysell)
    private void SendOrderFromHandOperate(TradeType buysell) {

      if(_onlineRate == null) return;

//      if(!API.CheckAccountTrade()) return;

      int lots = Convert.ToInt32(_nudLots.Value);
      int slippage = Convert.ToInt32(_nudSlipPage.Value);

      float stop = buysell == TradeType.Sell ? _tprevSell.PriceStop : _tprevBuy.PriceStop;
      float limit = buysell == TradeType.Sell ? _tprevSell.PriceLimit : _tprevBuy.PriceLimit;

      BrokerCommand command = null;
      if(this._chkPO.Checked) {
        bool error = buysell == TradeType.Sell ? _tprevSell.OrderStopLimitError : _tprevBuy.OrderStopLimitError;
        if (error) return;
        OrderType stoplimit = buysell == TradeType.Sell ? _tprevSell.OrderType : _tprevBuy.OrderType;

        float orderprice = Convert.ToSingle(this._nudPO.Value);
        command = new BrokerCommandEntryOrderCreate(GordagoMain.MainForm.DefaultAccountId, _onlineRate, stoplimit, buysell, lots, orderprice, stop, limit, "");
      } else {
        command = new BrokerCommandTradeOpen(GordagoMain.MainForm.DefaultAccountId, _onlineRate, buysell, lots,slippage, stop, limit, "");
      }
      BCM.ExecuteCommand(command);
    }
    #endregion

    #region private void _nudLots_ValueChanged(object sender, EventArgs e)
    private void _nudLots_ValueChanged(object sender, EventArgs e) {
      this.RefreshInfoPanels();
    }
    #endregion

    #region private void _chkPO_CheckedChanged(object sender, EventArgs e)
    private void _chkPO_CheckedChanged(object sender, EventArgs e) {
      
      if(_chkPO.Checked && _onlineRate != null) {
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

    #region IBrokerEvents Members

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) { }

    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) { }

    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) { }

    public void BrokerTradesChanged(BrokerTradesEventArgs be) { }

    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      this.RefreshSymbolLabel();
    }

    public void BrokerCommandStarting(BrokerCommand command) {
    }

    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
    }

    #endregion
  }
}
