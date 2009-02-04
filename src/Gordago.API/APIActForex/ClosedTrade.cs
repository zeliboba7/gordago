using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;
using System.IO;
using System.Xml;
using Gordago;

namespace IFXMarkets {

  #region class ClosedTrade:IClosedTrade
  class ClosedTrade:IClosedTrade {

    private string _accountId, _tradeId, _parentOrderId;
    private float _openRate, _closeRate, _stopRate, _limitRate;
    private DateTime _openTime, _closeTime;
    private float _commission, _fee, _netPL, _premium;

    private int _amount;
    private string _symbolName;
    private string _openComment = "", _closeComment = "";

    private TradeType _tradeType;

    public ClosedTrade(IAccount account, XmlNodeManager nodeM) {
      _accountId = account.AccountId;
      this.Load(nodeM);
    }

    public ClosedTrade(IFXMarketsBroker broker, FxTrade fxTrade, string comment) {
      _closeComment = _openComment = "";
      
      _accountId = fxTrade.AccountId;
      _tradeId = fxTrade.TradeId;
      _parentOrderId = fxTrade.ParentOrderId;

      _openRate = Convert.ToSingle( fxTrade.OpenRate);
      _closeRate = Convert.ToSingle( fxTrade.CloseRate);
      _stopRate = Convert.ToSingle(fxTrade.StopRate );
      _limitRate = Convert.ToSingle(fxTrade.LimitRate);

      _openTime = broker.GetGmtTime(fxTrade.OpenTime);
      _closeTime = broker.GetGmtTime(fxTrade.CloseTime);

      _commission = Convert.ToSingle(fxTrade.Commission);
      _fee = Convert.ToSingle(fxTrade.Fee);

      OnlineRate onlineRate = (broker.OnlineRates as OnlineRateList).GetOnlineRateFromPairId(fxTrade.PairId);
      if (onlineRate != null) {
        _symbolName = onlineRate.Symbol.Name; 
      }
      _amount = fxTrade.Amount;
      _netPL = Convert.ToSingle(fxTrade.NetPL);

      _premium = Convert.ToSingle(fxTrade.Premium);
//      _tradeType = IFXMarketsBroker.GetTradeType(fxTrade.BuySell == FxBuySell.bs_Sell ? FxBuySell.bs_Buy : FxBuySell.bs_Sell);
      _tradeType = IFXMarketsBroker.GetTradeType(fxTrade.BuySell);
    }

    #region public string AccountId
    public string AccountId {
      get { return _accountId; }
    }
    #endregion

    #region public float CloseRate
    public float CloseRate {
      get { return _closeRate; }
    }
    #endregion

    #region public DateTime CloseTime
    public DateTime CloseTime {
      get { return _closeTime; }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get { return _commission; }
    }
    #endregion

    # region public float Fee
    public float Fee {
      get { return _fee; }
    }
    #endregion

    #region public float LimitOrderRate
    public float LimitOrderRate {
      get { return _limitRate; }
    }
    #endregion

    #region public int Amount
    public int Amount {
      get { return this._amount; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get { return _netPL; }
    }
    #endregion

    #region public float OpenRate
    public float OpenRate {
      get { return _openRate; }
    }
    #endregion

    #region public DateTime OpenTime
    public DateTime OpenTime {
      get { return _openTime; }
    }
    #endregion

    #region public string ParentOrderId
    public string ParentOrderId {
      get { return _parentOrderId; }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get { return _premium; }
    }
    #endregion

    #region public float StopOrderRate
    public float StopOrderRate {
      get { return _stopRate; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return _symbolName; }
    }
    #endregion

    #region public string TradeId
    public string TradeId {
      get { return _tradeId; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _tradeType; }
    }
    #endregion

    #region public void Save(XmlNodeManager nodeM)
    public void Save(XmlNodeManager nodeM) {
      nodeM.SetAttribute("TradeId", this.TradeId);
      nodeM.SetAttribute("Amount", this.Amount);
      nodeM.SetAttribute("CRate", this.CloseRate);
      nodeM.SetAttribute("CTime", this.CloseTime);
      nodeM.SetAttribute("Commission", this.Commission);
      nodeM.SetAttribute("Fee", this.Fee);
      nodeM.SetAttribute("LimitRate", this.LimitOrderRate);
      nodeM.SetAttribute("StopRate", this.StopOrderRate);
      nodeM.SetAttribute("NetPL", this.NetPL);
      nodeM.SetAttribute("ORate", this.OpenRate);
      nodeM.SetAttribute("OTime", this.OpenTime);
      nodeM.SetAttribute("POrderId", this.ParentOrderId);
      nodeM.SetAttribute("Premium", this.Premium);
      nodeM.SetAttribute("Symbol", this.SymbolName);
      nodeM.SetAttribute("Type", this.TradeType == TradeType.Buy ? "Buy" : "Sell");
    }
    #endregion

    #region public void Load(XmlNodeManager nodeM)
    public void Load(XmlNodeManager nodeM) {
      _tradeId = nodeM.GetAttributeString("TradeId", "");

      _amount = nodeM.GetAttributeInt32("Amount", 0);
      _closeRate = nodeM.GetAttributeFloat("CRate", 0);
      _closeTime = nodeM.GetAttributeDateTime("CTime", DateTime.Now);
      _commission = nodeM.GetAttributeFloat("Commission", 0);
      _fee = nodeM.GetAttributeFloat("Fee", 0);
      _limitRate = nodeM.GetAttributeFloat("LimitRate", 0);
      _netPL = nodeM.GetAttributeFloat("NetPL", 0);
      _openRate = nodeM.GetAttributeFloat("ORate", 0);
      _openTime = nodeM.GetAttributeDateTime("OTime", DateTime.Now);

      _parentOrderId = nodeM.GetAttributeString("POrderId", "");
      _premium = nodeM.GetAttributeFloat("Premium", 0);
      _stopRate = nodeM.GetAttributeFloat("StopRate", 0);
      _symbolName = nodeM.GetAttributeString("Symbol", "");
      string type = nodeM.GetAttributeString("Type", "Sell");

      if (type == "Sell")
        _tradeType = TradeType.Sell;
      else
        _tradeType = TradeType.Buy;
    }
    #endregion

    #region public string CloseComment
    public string CloseComment {
      get {
        return _closeComment;
      }
    }
    #endregion

    #region public string OpenComment
    public string OpenComment {
      get {
        return _openComment;
      }
    }
    #endregion
  }
  #endregion

}
