/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {
  class ClosedTrade : IClosedTrade {

    private string _accountId;
    private string _symbolName;
    private float _openRate, _closeRate;
    private DateTime _openTime, _closeTime;
    private string _tradeId, _parentOrderId;
    private TradeType _tradeType;
    private int _amount;
    private float _stopOrderRate, _limitOrderRate;
    private float _commision, _fee, _premium;
    private float _drowDown, _growUp;
    private int _delimiter;

    private int _netPLPoint;
    private float _netPL;
    private string _openComment, _closeComment;

    private ISymbol _symbol;

    public ClosedTrade(Trade trade, string closeComment) {
      _openComment = trade.Comment;
      _closeComment = closeComment;
      _symbolName = trade.OnlineRate.Symbol.Name;
      _symbol = trade.OnlineRate.Symbol;
      _accountId = trade.Account.AccountId;
      _parentOrderId = trade.ParentOrderId;
      _tradeId = trade.TradeId;
      _tradeType = trade.TradeType;
      _amount = trade.Lots * trade.OnlineRate.LotSize;

      _openRate = trade.OpenRate;
      _openTime = trade.OpenTime;
      _closeRate = trade.CloseRate;
      _closeTime = trade.CloseTime;
      _commision = trade.Commission;
      _fee = trade.Fee;
      _premium = trade.Premium;

      _netPL = trade.NetPL;
      _netPLPoint = trade.NetPLPoint;

      _growUp = trade.GrowUp;
      _drowDown = trade.DrawDown;

      _delimiter = SymbolManager.GetDelimiter(trade.OnlineRate.Symbol.DecimalDigits);

//       System.Diagnostics.Debug.WriteLine(string.Format("{0}, {1}, {2}, {3}", _openRate, _closeRate, _growUp, _drowDown));

      if (trade.StopOrder != null)
        _stopOrderRate = trade.StopOrder.Rate;

      if (trade.LimitOrder != null)
        _limitOrderRate = trade.LimitOrder.Rate;
    }

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
    }
    #endregion

    #region public string AccountId
    public string AccountId {
      get { return this._accountId; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return this._symbolName; }
    }
    #endregion

    #region public string TradeId
    public string TradeId {
      get { return _tradeId; }
    }
    #endregion

    #region public string ParentOrderId
    public string ParentOrderId {
      get { return _parentOrderId; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _tradeType; }
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
      get { return _closeRate; }
    }
    #endregion

    #region public DateTime CloseTime
    public DateTime CloseTime {
      get { return _closeTime; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get { return _netPL; }
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

    #region public int NetPLPoint
    public int NetPLPoint {
      get {
        return _netPLPoint;
      }
    }
    #endregion

    #region public int GetDrawDownPoint()
    public int GetDrawDownPoint() {
      return -Math.Abs(Convert.ToInt32((this.OpenRate - this.DrawDown) * Gordago.SymbolManager.GetDelimiter(this.Symbol.DecimalDigits)));
    }
    #endregion

    #region public int GetGrowUpPoint()
    public int GetGrowUpPoint() {
      return Math.Abs(Convert.ToInt32((this.OpenRate - this.GrowUp) * Gordago.SymbolManager.GetDelimiter(this.Symbol.DecimalDigits)));
    }
    #endregion

    #region public float StopOrderRate
    public float StopOrderRate {
      get { return _stopOrderRate; }
    }
    #endregion 

    #region public float LimitOrderRate
    public float LimitOrderRate {
      get { return _limitOrderRate; }
    }
    #endregion

    #region public int Amount
    public int Amount {
      get { return _amount; }
    }
    #endregion

    #region public string OpenComment
    public string OpenComment {
      get {
        return _openComment;
      }
    }
    #endregion

    #region public string CloseComment
    public string CloseComment {
      get { return _closeComment; }
    }
    #endregion
  }

  #region class ClosedTradeList:IClosedTradeList
  class ClosedTradeList:IClosedTradeList {

    private List<IClosedTrade> _trades;

    public ClosedTradeList() {
      _trades = new List<IClosedTrade>();
    }

    #region public int Count
    public int Count {
      get { return _trades.Count; }
    }
    #endregion

    #region public IClosedTrade this[int index]
    public IClosedTrade this[int index] {
      get { return _trades[index]; }
    }
    #endregion

    #region public IClosedTrade GetClosedTrade(string closeTradeId)
    public IClosedTrade GetClosedTrade(string closeTradeId) {
      for (int i = 0; i < _trades.Count; i++) {
        if (_trades[i].TradeId == closeTradeId)
          return _trades[i];
      }
      return null;
    }
    #endregion 

    #region public void Add(ClosedTrade closeTrade)
    public void Add(ClosedTrade closeTrade) {
      _trades.Add(closeTrade);
    }
    #endregion
  }
  #endregion
}
