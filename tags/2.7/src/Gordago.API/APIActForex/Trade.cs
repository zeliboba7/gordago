using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;
using Gordago;

namespace IFXMarkets {

  class Trade:ITrade {

    private IFXMarketsBroker _broker;
    private Account _account; 
    private OnlineRate _onlineRate;

    private Order _limitorder, _stoporder;
    private int _savedSessionId;

    // private FxTrade _fxTrade;
    private int _lots, _netPLPoint, _delimiter;
    
    private string _tradeId, _parentOrderId;
    private float _openRate, _closeRate, _netPL;
    private DateTime _openTime;
    private float _commission, _premium, _fee;

    private TradeType _tradeType;
    private string _comment = "";
    private DateTime _closeTime;
    private int _amount;

    public Trade(IFXMarketsBroker broker, FxTrade fxtrade) {
      // _fxTrade = fxtrade;
      _broker = broker;
      _account = (Account)broker.Accounts.GetAccount(fxtrade.AccountId);
      if (_account == null)
        throw(new Exception("Account is null"));
      _onlineRate = (_broker.OnlineRates as OnlineRateList).GetOnlineRateFromPairId(fxtrade.PairId);
      _delimiter = SymbolManager.GetDelimiter(_onlineRate.Symbol.DecimalDigits);

      this.Update(fxtrade);
    }

    #region public string TradeId
    public string TradeId {
      get {
        return _tradeId;
      }
    }
    #endregion
    
    #region public float OpenRate
    public float OpenRate {
      get {
        return _openRate;
      }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get {
        return _premium;
      }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get {
        return _commission;
      }
    }
    #endregion

    #region public DateTime OpenTime
    public DateTime OpenTime {
      get {
        return _openTime;
      }
    }
    #endregion

    #region public float Fee
    public float Fee {
      get {
        return _fee;
      }
    }
    #endregion

    #region public float CloseRate
    public float CloseRate {
      get {
        CalculateValue();
        return _closeRate; 
      }
    }
    #endregion

    #region public DateTime CloseTime
    public DateTime CloseTime {
      get {

        return _broker.GetGmtTime(_closeTime);
      }
    }
    #endregion

    #region public string ParentOrderId
    public string ParentOrderId {
      get {
        return _parentOrderId;
      }
    }
    #endregion

    #region public IOrder LimitOrder
    public IOrder LimitOrder {
      get { 
        return this._limitorder; 
      }
    }
    #endregion

    #region public IOrder StopOrder
    public IOrder StopOrder {
      get { return this._stoporder; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get {
        this.CalculateValue();
        return _lots; 
      }
    }
    #endregion

    #region public int GetPipsProfitOnline()
    public int GetPipsProfitOnline() {
      float price = this.CloseRate;
      int pips = 0;
      if (this.TradeType == TradeType.Sell) {
        pips = IFXMarketsBroker.CalculatePoint(this.OpenRate, price, this.OnlineRate.Symbol.DecimalDigits, false);
      } else {
        pips = IFXMarketsBroker.CalculatePoint(price, this.OpenRate, this.OnlineRate.Symbol.DecimalDigits, false);
      }
      return pips;
    }
    #endregion

    #region public IAccount Account
    public IAccount Account {
      get { return _account; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _tradeType; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get {
        this.CalculateValue();
        return _netPL; 
      }
    }
    #endregion

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { 
        return _onlineRate; 
      }
    }
    #endregion

    #region public int NetPLPoint
    public int NetPLPoint {
      get { 
        this.CalculateValue();
        return _netPLPoint;
      }
    }
    #endregion

    #region public void Update(FxTrade fxTrade)
    public void Update(FxTrade fxTrade) {

      _tradeId = fxTrade.TradeId;
      _amount = fxTrade.Amount;_closeTime = fxTrade.CloseTime;
      _openRate = Convert.ToSingle(fxTrade.OpenRate);
      _openTime = _broker.GetGmtTime(fxTrade.OpenTime);
      _parentOrderId = fxTrade.ParentOrderId;
      _tradeType = IFXMarketsBroker.GetTradeType(fxTrade.BuySell);

      _premium = Convert.ToSingle(fxTrade.Premium);
      _commission = Convert.ToSingle(fxTrade.Commission);
      _fee = Convert.ToSingle(fxTrade.Fee);

      System.Diagnostics.Debug.WriteLine(
        string.Format("{0}, {1}, {2}, {3}, {4}, {5}", _tradeId, _onlineRate.Symbol.Name, _amount, _openRate, _openTime, fxTrade.NetPL));

      Order oldStopOrder = _stoporder;
      Order oldLimitOrder = _limitorder;

      _limitorder = null;
      _stoporder = null;
      for (int i = 0; i < _broker.Orders.Count; i++) {
        Order order = _broker.Orders[i] as Order;
        if (order.TradeId == this.TradeId) {
          if (order.OrderType == OrderType.Limit) {
            _limitorder = order;
          } else if (order.OrderType == OrderType.Stop) {
            _stoporder = order;
          }
        }
      }

      this.CalculateValue();
      if (_limitorder != oldLimitOrder || _stoporder != oldStopOrder) {
        _broker.OnTradeUpdateEvents(this);
      }
    }
    #endregion

    private void CalculateValue() {
      if (_savedSessionId == _onlineRate.SessionId)
        return;

      _savedSessionId = _onlineRate.SessionId;

      _closeRate = _tradeType == TradeType.Sell ? _onlineRate.BuyRate : _onlineRate.SellRate;
      _lots = _amount / _onlineRate.LotSize;

      //_netPL = Convert.ToSingle(_fxTrade.NetPL);
      //_netPL = _tradeType == TradeType.Sell ? _openRate - _closeRate : _closeRate - _openRate;
      //_netPLPoint = Convert.ToInt32(netPL * _delimiter);


      float netPL = _tradeType == TradeType.Sell ? _openRate - _closeRate : _closeRate - _openRate;
      _netPLPoint = Convert.ToInt32(netPL * _delimiter);
      int lotSize = this._onlineRate.LotSize;
      int over = 10000 / _delimiter;
      _netPL = (float)Math.Round(Convert.ToDecimal(netPL * lotSize * _lots * this._onlineRate.PipCost / over), 2);
    }

    #region public string Comment
    public string Comment {
      get { return _comment; }
    }
    #endregion
  }
}
