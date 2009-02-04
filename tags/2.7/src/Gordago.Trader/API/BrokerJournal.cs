/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  #region public abstract class BrokerJournalRecord
  public abstract class BrokerJournalRecord {
    private DateTime _serverTime;

    internal BrokerJournalRecord(DateTime serverTime) {
      _serverTime = serverTime;
    }
    #region public DateTime Time
    public DateTime Time {
      get { return _serverTime; }
    }
    #endregion

  }
  #endregion

  #region public class BJRComment : BrokerJournalRecord
  public class BJRComment : BrokerJournalRecord {
    private string _comment;

    internal BJRComment(string comment, DateTime serverTime) : base(serverTime) {
      _comment = comment;
    }

    #region public string Comment
    public string Comment {
      get { return _comment; }
    }
    #endregion
  }
  #endregion

  #region public class BJRAccount:BrokerJournalRecord
  public class BJRAccount:BrokerJournalRecord {
    private string _accountId;
    private float _balance;
    private bool _active;
    private int _lots;
    private float _commision;
    private float _equity, _netpl, _premium;
    private float _usedMargin, _usableMargin;
    private string _moneyOwner;

    internal BJRAccount(IAccount account, DateTime serverTime):base(serverTime) {
      if (account == null)
        return;
      _balance = account.Balance;
      _lots = account.Lots;
      _active = account.Active;
      _accountId = account.AccountId;
      _commision = account.Commission;
      _equity = account.Equity;
      _moneyOwner = account.MoneyOwner;
      _netpl = account.NetPL;
      _premium = account.Premium;
      _usableMargin = account.UsableMargin;
      _usedMargin = account.UsedMargin;
    }

    #region public float Balance
    public float Balance {
      get { return this._balance; }
    }
    #endregion

    #region public bool Active
    public bool Active {
      get { return _active; }
    }
    #endregion

    #region public string AccountId
    public string AccountId {
      get { return _accountId; }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get { return _commision; }
    }
    #endregion

    #region public float Equity
    public float Equity {
      get { return _equity; }
    }
    #endregion

    #region public float UsedMargin
    public float UsedMargin {
      get { return _usedMargin; }
    }
    #endregion

    #region public float UsableMargin
    public float UsableMargin {
      get { return _usableMargin; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get { return _netpl; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return _lots; }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get { return _premium; }
    }
    #endregion

    #region public float Commision
    public float Commision {
      get { return _commision; }
    }
    #endregion

    #region public string MoneyOwner
    public string MoneyOwner {
      get { return _moneyOwner; }
    }
    #endregion
  }
  #endregion

  #region public class BJRConnectionStatus : BrokerJournalRecord
  public class BJRConnectionStatus : BJRAccount {
    private BrokerConnectionStatus _status;

    internal BJRConnectionStatus(IAccount account, DateTime serverTime, BrokerConnectionStatus status) : base(account, serverTime) {
      _status = status;
    }

    #region public BrokerConnectionStatus ConnectionStatus
    public BrokerConnectionStatus ConnectionStatus {
      get { return this._status; }
    }
    #endregion
  }
  #endregion

  #region public class BJRTrade : BrokerJournalRecord
  public class BJRTrade : BJRAccount {

    private string _tradeId, _parentOrderId;
    private int _lots;
    private float _amount;
    private float _openRate, _closeRate, _stopRate, _limitRate;
    private TradeType _tradeType;

    private BrokerMessageType _mtype;
    private string _symbolName;
    private int _symbolDecimalDigits;
    private float _symbolSellRate, _symbolBuyRate;
    private float _netPL;
    private float _fee;

    internal BJRTrade(DateTime serverTime, ITrade trade, BrokerMessageType mtype) : base(trade.Account, serverTime) {
      _parentOrderId = trade.ParentOrderId;
      _mtype = mtype;
      _tradeId = trade.TradeId;
      _tradeType = trade.TradeType;
      _symbolName = trade.OnlineRate.Symbol.Name;
      _symbolSellRate = trade.OnlineRate.SellRate;
      _symbolBuyRate = trade.OnlineRate.BuyRate;
      _netPL = trade.NetPL;
      _amount = trade.Lots * trade.OnlineRate.LotSize;
      _symbolDecimalDigits = trade.OnlineRate.Symbol.DecimalDigits;

      _openRate = trade.OpenRate;
      _closeRate = trade.CloseRate;

      if (trade.StopOrder != null) _stopRate = trade.StopOrder.Rate;
      if (trade.LimitOrder != null) _limitRate = trade.LimitOrder.Rate;
      _lots = trade.Lots;
      _fee = trade.Fee;
    }

    #region public string ParentOrderId
    public string ParentOrderId {
      get { return this._parentOrderId; }
    }
    #endregion

    #region public float Amount
    public float Amount {
      get { return this._amount; }
    }
    #endregion

    #region public string TradeId
    public string TradeId {
      get { return this._tradeId; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get { return this._netPL; }
    }
    #endregion

    #region public float OpenRate
    public float OpenRate{
      get { return this._openRate; }
    }
    #endregion

    #region public float CloseRate
    public float CloseRate {
      get { return this._closeRate; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return this._tradeType; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return this._stopRate; }
    }
    #endregion

    #region public BrokerMessageType MessageType
    public BrokerMessageType MessageType {
      get { return this._mtype; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return this._symbolName; }
    }
    #endregion

    #region public int SymbolDecimalDigits
    public int SymbolDecimalDigits {
      get { return this._symbolDecimalDigits; }
    }
    #endregion

    #region public float SymbolSellRate
    public float SymbolSellRate {
      get { return this._symbolSellRate; }
    }
    #endregion

    #region public float SymbolBuyRate
    public float SymbolBuyRate {
      get { return this._symbolBuyRate; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return this._lots; }
    }
    #endregion

    #region public float Fee
    public float Fee {
      get { return this._fee; }
    }
    #endregion
  }
  #endregion

  #region public class BJROrder : BrokerJournalRecord 
  public class BJROrder : BJRAccount {

    private string _orderId, _tradeId;
    private int _lots;
    private float _rate, _stopRate, _limitRate;
    private TradeType _tradeType;

    private BrokerMessageType _mtype;
    private string _symbolName;
    private float _symbolSellRate, _symbolBuyRate;
    private OrderType _orderType;

    private int _amount;
    private int _symbolDecimalDigits;

    internal BJROrder(DateTime serverTime, IOrder order, BrokerMessageType mtype) : base(order.Account, serverTime) {
      _tradeId = order.TradeId;
      _mtype = mtype;
      _orderId = order.OrderId;
      _tradeType = order.TradeType;
      _symbolName = order.OnlineRate.Symbol.Name;
      _symbolSellRate = order.OnlineRate.SellRate;
      _symbolBuyRate = order.OnlineRate.BuyRate;
      _rate = order.Rate;
      _orderType = order.OrderType;

      if (order.StopOrder != null) _stopRate = order.StopOrder.Rate;
      if (order.LimitOrder != null) _limitRate = order.LimitOrder.Rate;
      _lots = order.Lots;
      _amount = order.Lots * order.OnlineRate.LotSize;
      _symbolDecimalDigits = order.OnlineRate.Symbol.DecimalDigits;
    }

    #region public string TradeId
    public string TradeId {
      get { return _tradeId; }
    }
    #endregion

    #region public string OrderId
    public string OrderId {
      get { return this._orderId; }
    }
    #endregion

    #region public int Amount
    public int Amount {
      get { return this._amount; }
    }
    #endregion

    #region public OrderType OrderType
    public OrderType OrderType {
      get { return this._orderType; }
    }
    #endregion

    #region public float Rate
    public float Rate {
      get { return this._rate; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return this._tradeType; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return this._stopRate; }
    }
    #endregion

    #region public BrokerMessageType MessageType
    public BrokerMessageType MessageType {
      get { return this._mtype; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return this._symbolName; }
    }
    #endregion

    #region public int SymbolDecimalDigits
    public int SymbolDecimalDigits {
      get { return this._symbolDecimalDigits; }
    }
    #endregion

    #region public float SymbolSellRate
    public float SymbolSellRate {
      get { return this._symbolSellRate; }
    }
    #endregion

    #region public float SymbolBuyRate
    public float SymbolBuyRate {
      get { return this._symbolBuyRate; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return this._lots; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerJornalEventArgs : EventArgs
  public class BrokerJornalEventArgs : EventArgs {
    private BrokerJournalRecord _record;

    public BrokerJornalEventArgs(BrokerJournalRecord record) {
      _record = record;
    }
    #region public BrokerJournalRecord Record
    public BrokerJournalRecord Record {
      get { return _record; }
    }
    #endregion
  }
  #endregion

  public class BrokerJournal {

    public event EventHandler<BrokerJornalEventArgs> RecordAdded;
    private IBroker _broker;

    private List<BrokerJournalRecord> _records;

    internal BrokerJournal(Broker broker) {
      _broker = broker as IBroker;
      _records = new List<BrokerJournalRecord>();
    }

    #region public int Count
    public int Count {
      get { return this._records.Count; }
    }
    #endregion

    #region public BrokerJournalRecord this[int index]
    public BrokerJournalRecord this[int index] {
      get { return _records[index]; }
    }
    #endregion

    #region 
    private void OnRecordAdded(BrokerJournalRecord record) {
      _records.Add(record);
      if (this.RecordAdded != null) {
        BrokerJornalEventArgs e = new BrokerJornalEventArgs(record);
        this.RecordAdded(this, e);
      }
    }
    #endregion

    #region internal void BrokerConnectionStatusChanged()
    internal void BrokerConnectionStatusChanged() {
      if (_broker.ConnectionStatus == BrokerConnectionStatus.Online) {
        for (int i = 0; i < _broker.Accounts.Count; i++)
          OnRecordAdded(new BJRConnectionStatus(_broker.Accounts[i], _broker.Time, _broker.ConnectionStatus));
      } else if (_broker.ConnectionStatus == BrokerConnectionStatus.Offline){
        OnRecordAdded(new BJRConnectionStatus(null, _broker.Time, _broker.ConnectionStatus));
      }
    }
    #endregion

    #region internal void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    internal void BrokerAccountsChanged(BrokerAccountsEventArgs be){
    }
    #endregion

    #region internal void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    internal void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      this.OnRecordAdded(new BJROrder(_broker.Time, be.Order, be.MessageType));
    }
    #endregion

    #region internal void BrokerTradesChanged(BrokerTradesEventArgs be)
    internal void BrokerTradesChanged(BrokerTradesEventArgs be) {
      this.OnRecordAdded(new BJRTrade(_broker.Time, be.Trade, be.MessageType));
    }
    #endregion

    #region internal void BrokerComment(string comment)
    internal void BrokerComment(string comment) {
      OnRecordAdded(new BJRComment(comment, _broker.Time));
    }
    #endregion

    #region internal void BrokerMarginCallCreated(BrokerMarginCallEventArgs be)
    internal void BrokerMarginCallCreated(BrokerMarginCallEventArgs be) {
    }
    #endregion
  }
}

