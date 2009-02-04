/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {

  class Trade:ITrade {

    private Account _account;
    private OnlineRate _onlineRate;
    private float _openRate, _closeRate;
    private DateTime _openTime, _closeTime;
    private string _tradeId, _parentOrderId;
    private TradeType _tradeType;
    private int _lots;
    private Order _stopOrder, _limitOrder;
    private float _commision, _fee, _premium;
    private VirtualBroker _broker;
    private bool _isclose, _savedIsClose = false;
    private float _drowDown, _growUp;

    private int _delimiter;

    private long _savedSessionId = 0;
    private int _netPLPoint;
    private float _netPL;
    private string _comment;

    public Trade(VirtualBroker broker, Account account, OnlineRate onlineRate, string tradeId, TradeType tradeType, int lots, DateTime opentTime, string comment) {
      _comment = comment;
      _broker = broker;
      _account = account;
      _onlineRate = onlineRate;
      _tradeId = tradeId;
      _tradeType = tradeType;
      _lots = lots;

      if (tradeType == TradeType.Sell) {
        _openRate = onlineRate.SellRate;
        _drowDown = float.MinValue;
        _growUp = float.MaxValue;
      } else {
        _openRate = onlineRate.BuyRate;
        _drowDown = float.MaxValue;
        _growUp = float.MinValue;
      }

      _openTime = opentTime;
      _delimiter = SymbolManager.GetDelimiter(onlineRate.Symbol.DecimalDigits);
      this.CalculateValues();
    }

    #region public IAccount Account
    public IAccount Account {
      get { return _account; }
    }
    #endregion

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return _onlineRate; }
    }
    #endregion

    #region public string TradeId
    public string TradeId {
      get { return _tradeId;}
    }
    #endregion

    #region public string ParentOrderId
    public string ParentOrderId {
      get { return _parentOrderId; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return _lots; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get {return _tradeType; }
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

    #region public float CloseRate
    public float CloseRate {
      get {
        this.CalculateValues();
        return _closeRate;
      }
    }
    #endregion

    #region public DateTime CloseTime
    public DateTime CloseTime {
      get { return _closeTime; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get {
        this.CalculateValues();
        return _netPL;
      }
    }
    #endregion

    #region public int NetPLPoint
    public int NetPLPoint {
      get {
        this.CalculateValues();
        return _netPLPoint;
      }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get { return _commision; }
    }
    #endregion

    #region public float Fee
    public float Fee {
      get { return _fee; }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get { return _premium; }
    }
    #endregion

    #region public IOrder StopOrder
    public IOrder StopOrder {
      get { return _stopOrder; }
    }
    #endregion

    #region public IOrder LimitOrder
    public IOrder LimitOrder {
      get { return _limitOrder; }
    }
    #endregion

    #region public float DrawDown
    /// <summary>
    /// ÷ена максимальной просадки до закрытие позиции
    /// </summary>
    public float DrawDown {
      get { return this._drowDown; }
    }
    #endregion

    #region public float GrowUp
    /// <summary>
    /// ÷ена максимальной прибыли до закрытие позиции
    /// </summary>
    public float GrowUp {
      get { return this._growUp; }
    }
    #endregion

    #region public void Close(DateTime closeTime)
    public void Close(DateTime closeTime) {
      _closeTime = closeTime;
      _isclose = true;
      CalculateValues();
    }
    #endregion

    #region public void SetOpenRate(float openRate) {
    public void SetOpenRate(float openRate) {
      _openRate = openRate;
    }
    #endregion

    #region public void SetStopOrder(Order stopOrder)
    public void SetStopOrder(Order stopOrder) {
      _stopOrder = stopOrder;
    }
    #endregion

    #region public void SetLimitOrder(Order limitOrder)
    public void SetLimitOrder(Order limitOrder) {
      _limitOrder = limitOrder;
    }
    #endregion

    #region public void SetLots(int lots)
    public void SetLots(int lots) {
      _lots = lots;
    }
    #endregion

    #region public void SetParentOrderId(string parentOrderId)
    public void SetParentOrderId(string parentOrderId) {
      _parentOrderId = parentOrderId;
    }
    #endregion

    #region public void SubLots(int lots)
    public void SubLots(int lots) {
      _lots = _lots - lots;
    }
    #endregion

    #region public void CalculateValues()
    public void CalculateValues(){
      if (_savedIsClose)
        return;
      if (_broker.SessionId == _savedSessionId)
        return;

      _savedIsClose = _isclose;
      _savedSessionId = _broker.SessionId;

      _closeRate = _tradeType == TradeType.Sell ? _onlineRate.BuyRate : _onlineRate.SellRate;

      float netPL = _tradeType == TradeType.Sell ? _openRate - _closeRate : _closeRate - _openRate;
      _netPLPoint = Convert.ToInt32(netPL * _delimiter);
      int lotSize = this._onlineRate.LotSize;
      int over = 10000 / _delimiter;
      _netPL = (float)Math.Round(Convert.ToDecimal(netPL * lotSize * _lots * this._onlineRate.PipCost / over),2);

      if (_tradeType == TradeType.Sell) {
        _growUp = Math.Min(_growUp, _closeRate);
        _drowDown = Math.Max(_drowDown, _closeRate);
      } else {
        _growUp = Math.Max(_growUp, _closeRate);
        _drowDown = Math.Min(_drowDown, _closeRate);
      }
    }
    #endregion

    #region public string Comment
    public string Comment {
      get { return _comment; }
    }
    #endregion
  }
}
