/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {

  class ServerError : IServerError {
    private ServerErrorType _et;

    public ServerError(ServerErrorType et) {
      _et = et;
    }

    public ServerErrorType ErrorType {
      get { return _et; }
    }

    public object Detail {
      get { throw new Exception("The method or operation is not implemented."); }
    }
  }

  #region class TestTrade:ITrade
  class TestTrade:ITrade {

    private IAccount _account;
    private ISymbol _symbol;
    private string _tradeId;
    private string _parentOrderId;
    private int _lots;
    private TradeType _ttype;
    private float _openRate, _closeRate;
    private DateTime _openTime, _closeTime;
    private IOrder _stopOrder, _limitOrder;
    private bool _isClose = false;
    private TradeCloseType _tclosetype;
    private float _drowDown, _growUp;
    private int _delimiter;


    internal TestTrade(IAccount account, ISymbol symbol, string tradeId, TradeType ttype, int lots, float openRate, DateTime openTime) {
      _account = account;
      _symbol = symbol;
      _tradeId = tradeId;
      _ttype = ttype;
      _lots = lots;
      _openRate = openRate;
      _openTime = openTime;
      _tclosetype = TradeCloseType.Custom;

      /* РёР_РёС┼РёР°Р>РёР·Р°С┼РёС_ С┼РчР_Р_Р№ Р·Р°РєС_С<С'РёС_ */
      _drowDown = _growUp = ttype == TradeType.Sell ? symbol.Ask : symbol.Bid;
      _delimiter = Gordago.SymbolManager.GetDelimiter(symbol.DecimalDigits);
    }

    #region public IAccount Account
    public IAccount Account {
      get { return _account; }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion

    #region public string TradeId
    public string TradeId {
      get { return _tradeId; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _ttype; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return _lots; }
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
      get {
        TestSymbol tsymbol = _symbol as TestSymbol;
        return this.NetPLPoint * _lots * tsymbol.Property.PipCost;
      }
    }
    #endregion

    #region public int NetPLPoint
    public int NetPLPoint {
      get {
        float netpl = 0;
        TestSymbol tsymbol = _symbol as TestSymbol;
        if(this.IsClose) {
          netpl = _ttype == TradeType.Sell ? OpenRate - CloseRate : CloseRate - OpenRate;
        } else {
          netpl = _ttype == TradeType.Sell ? OpenRate - tsymbol.Bid : tsymbol.Bid - OpenRate;
        }
        int pips = Convert.ToInt32(netpl * _delimiter);

        return pips;
      }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get { return float.NaN; }
    }
    #endregion

    #region public float Fee
    public float Fee {
      get { return 0; }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get { return 0; }
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

    #region internal bool IsClose
    internal bool IsClose {
      get { return this._isClose; }
    }
    #endregion

    #region internal float DrawDown
    /// <summary>
    /// Р│РчР_Р° Р_Р°РєС_РёР_Р°Р>С_Р_Р_Р№ РїС_Р_С_Р°Р_РєРё Р_Р_ Р·Р°РєС_С<С'РёРч РїР_Р·РёС┼РёРё
    /// </summary>
    internal float DrawDown {
      get { return this._drowDown; }
    }
    #endregion

    #region internal float GrowUp
    /// <summary>
    /// Р│РчР_Р° Р_Р°РєС_РёР_Р°Р>С_Р_Р_Р№ РїС_РёР+С<Р>Рё Р_Р_ Р·Р°РєС_С<С'РёРч РїР_Р·РёС┼РёРё
    /// </summary>
    internal float GrowUp {
      get { return this._growUp; }
    }
    #endregion

    #region internal void SetStopOrder(Order stopOrder)
    internal void SetStopOrder(TestOrder stopOrder) {
      _stopOrder = stopOrder;
    }
    #endregion
    #region internal void SetLimitOrder(Order limitOrder)
    internal void SetLimitOrder(TestOrder limitOrder) {
      _limitOrder = limitOrder;
    }
    #endregion
    #region internal void SubLots(int lots)
    internal void SubLots(int lots) {
      _lots = _lots - lots;
    }
    #endregion

    #region internal void Close(float closeRate, DateTime closeTime, TradeCloseType tcloseType)
    internal void Close(float closeRate, DateTime closeTime, TradeCloseType tcloseType) {
      _closeRate = closeRate;
      _closeTime = closeTime;
      _tclosetype = tcloseType;
      _isClose = true;
    }
    #endregion

    #region internal TradeCloseType CloseType
    internal TradeCloseType CloseType {
      get { return _tclosetype; }
    }
    #endregion

    #region internal void UpdateStatistic()
    internal void UpdateStatistic() {
      float closeRate = this.TradeType == TradeType.Sell ? this.Symbol.Ask : this.Symbol.Bid; ;
      if (this.TradeType == TradeType.Sell){
        _drowDown = Math.Max(_drowDown, closeRate);
        _growUp = Math.Min(_growUp, closeRate);
      }else{
        _drowDown = Math.Min(_drowDown, closeRate);
        _growUp = Math.Max(_growUp, closeRate);
      }
    }
    #endregion

    #region internal int GetDrawDownPoint()
    internal int GetDrawDownPoint() {
      return -Math.Abs(Convert.ToInt32((this.OpenRate - this.DrawDown) * Gordago.SymbolManager.GetDelimiter(this.Symbol.DecimalDigits)));
    }
    #endregion

    #region internal int GetGrowUpPoint()
    internal int GetGrowUpPoint() {
      return Math.Abs(Convert.ToInt32((this.OpenRate - this.GrowUp) * Gordago.SymbolManager.GetDelimiter(this.Symbol.DecimalDigits)));
    }
    #endregion

    public string ParentOrderId {
      get { return _parentOrderId; }
    }

    internal void SetParentOrderId(string parentOrderId) {
      _parentOrderId = parentOrderId;
    }
  }
  #endregion

  #region class TestOrder:IOrder
  class TestOrder:IOrder {

    private IAccount _account;
    private ISymbol _symbol;
    private string _orderId, _parentOrderId, _tradeId;
    private TradeType _ttype;
    private OrderType _otype;
    private float _rate;
    private DateTime _time;
    private TestOrder _stopOrder, _limitOrder;
    private int _lots;

    public TestOrder(IAccount account, ISymbol symbol, string orderId, TradeType ttype, OrderType otype, int lots, float rate, DateTime time) {
      _account = account;
      _symbol = symbol;
      _orderId = orderId;
      _ttype = ttype;
      _otype = otype;
      _lots = lots;
      _rate = rate;
      _time = time;
    }
    #region public IAccount Account
    public IAccount Account {
      get { return this._account; }
    }
    #endregion
    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion
    #region public string OrderId
    public string OrderId {
      get { return _orderId; }
    }
    #endregion
    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _ttype; }
    }
    #endregion
    #region public OrderType OrderType
    public OrderType OrderType {
      get { return _otype;}
    }
    #endregion
    #region public int Lots
    public int Lots {
      get { return _lots; }
    }
    #endregion
    #region public float Rate
    public float Rate {
      get { return _rate; }
    }
    #endregion
    #region public DateTime Time
    public DateTime Time {
      get { return _time; }
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
    #region public string ParentOrderId
    public string ParentOrderId {
      get { return _parentOrderId; }
    }
    #endregion
    #region public string TradeId
    public string TradeId {
      get { return _tradeId; }
    }
    #endregion

    #region internal void SetStopOrder(Order stopOrder)
    internal void SetStopOrder(TestOrder stopOrder) {
      _stopOrder = stopOrder;
    }
    #endregion
    #region internal void SetLimitOrder(Order limitOrder)
    internal void SetLimitOrder(TestOrder limitOrder) {
      _limitOrder = limitOrder;
    }
    #endregion
    #region internal void SetParentOrderId(string parentOrderId)
    internal void SetParentOrderId(string parentOrderId) {
      _parentOrderId = parentOrderId;
    }
    #endregion
    #region internal void SetTradeId(string tradeId)
    internal void SetTradeId(string tradeId) {
      _tradeId = tradeId;
    }
    #endregion
    #region internal void SetOrderId(int orderId)
    //internal void SetOrderId(int orderId) {
    //  _orderId = orderId;
    //}
    #endregion
    #region internal void SetLots(int lots)
    internal void SetLots(int lots) {
      _lots = lots;
      if(this.StopOrder != null) {
        (this.StopOrder as TestOrder).SetLots(lots);
      }
      if(this.LimitOrder != null) {
        (this.LimitOrder as TestOrder).SetLots(lots);
      }
    }
    #endregion
    #region internal void SetRate(float newRate)
    internal void SetRate(float newRate) {
      _rate = newRate;
    }
    #endregion
  }
  #endregion

  #region class TestTradeList:ITradeList
  class TestTradeList:ITradeList {
    private TestTrade[] _trades;
    private int _size;
    private const int ASIZE = 32;

    public TestTradeList() {
      _size = 0;
      _trades = new TestTrade[0];
    }

    #region public int Count
    public int Count {
      get { return _size; }
    }
    #endregion

    #region public ITrade this[int index]
    public ITrade this[int index] {
      get { return _trades[index]; }
    }
    #endregion

    #region public ITrade GetTrade(string tradeId)
    public ITrade GetTrade(string tradeId) {
      for(int i = 0; i < _size; i++) {
        if(_trades[i].TradeId == tradeId)
          return _trades[i];
      }
      return null;
    }
    #endregion

    #region internal void Add(Trade trade)
    internal void Add(TestTrade trade) {
      if(_size == _trades.Length) {
        if (_trades.Length == 0)
          _trades = new TestTrade[ASIZE];
        TestTrade[] temp = new TestTrade[_trades.Length * 2];
        Array.Copy(_trades, 0, temp, 0, _trades.Length);
        _trades = temp;
      }
      _trades[_size++] = trade;
    }
    #endregion

    #region internal Trade Remove(string tradeId)
    internal TestTrade Remove(string tradeId) {
      for(int i = 0; i < this.Count; i++) {
        TestTrade trade = _trades[i];
        if(trade.TradeId == tradeId) {
          List<TestTrade> lst = new List<TestTrade>(_trades);
          lst.Remove(trade);
          _trades = lst.ToArray();
          _size--;
          return trade;
        }
      }
      return null;
    }
    #endregion
  }
  #endregion

  #region class TestOrderList:IOrderList
  class TestOrderList:IOrderList {
    private TestOrder[] _orders;
    private int _size;
    private const int ASIZE = 32;


    public TestOrderList() {
      _size = 0;
      _orders = new TestOrder[0];
    }

    #region public int Count
    public int Count {
      get { return _size; }
    }
    #endregion

    #region public IOrder this[int index]
    public IOrder this[int index] {
      get { return _orders[index]; }
    }
    #endregion

    #region public IOrder GetOrder(string orderId)
    public IOrder GetOrder(string orderId) {
      for(int i = 0; i < this.Count; i++) {
        if (_orders[i].OrderId == orderId)
          return _orders[i];
      }
      return null;
    }
    #endregion

    #region internal void Add(Order order)
    internal void Add(TestOrder order) {
      if(_size == _orders.Length) {
        if(_orders.Length == 0)
          _orders = new TestOrder[ASIZE];
        TestOrder[] temp = new TestOrder[_orders.Length * 2];
        Array.Copy(_orders, 0, temp, 0, _orders.Length);
        _orders = temp;
      }
      _orders[_size++] = order;
    }
    #endregion

    #region internal Order Remove(string orderId)
    internal TestOrder Remove(string orderId) {
      for(int i = 0; i < this.Count; i++) {
        TestOrder order = _orders[i];
        if(order.OrderId == orderId) {
          List<TestOrder> lst = new List<TestOrder>(_orders);
          lst.Remove(order);
          _orders = lst.ToArray();
          _size--;
          return order;
        }
      }
      return null;
    }
    #endregion
  }
  #endregion

  #region class JournalComment
  class JournalComment {
    private JournalCommentRecord[] _records;
    private int _size;

    public JournalComment() {
      _records = new JournalCommentRecord[32];
      _size = 0;
    }

    #region public int Count
    public int Count {
      get { return this._size; }
    }
    #endregion

    #region public JournalRecord this[int index]
    public JournalCommentRecord this[int index] {
      get { return this._records[index]; }
    }
    #endregion

    public void Add(DateTime time, string message) {
      Add(time, message, JournalRecordType.Unknow);
    }
    
    #region public void Add(DateTime time, string message, JournalRecordType rtype)
    public void Add(DateTime time, string message, JournalRecordType rtype) {
      JournalCommentRecord rec = new JournalCommentRecord(time, message);
      if(_size == _records.Length) {
        JournalCommentRecord[] temp = new JournalCommentRecord[_records.Length * 2];
        Array.Copy(_records, 0, temp, 0, _records.Length);
        _records = temp;
      }
      _records[_size++] = rec;
    }
    #endregion
  }
  #endregion

  #region class JournalCommentRecord
  class JournalCommentRecord {
    private DateTime _time;
    private string _message;
    private JournalRecordType _rtype;

    public JournalCommentRecord(DateTime time, string message):this(time, message, JournalRecordType.Unknow) {
    }

    public JournalCommentRecord(DateTime time, string message, JournalRecordType rtype) {
      _time = time;
      _message = message;
      _rtype = rtype;
    }

    #region public DateTime Time
    public DateTime Time {
      get { return _time; }
    }
    #endregion

    #region public string Message
    public string Message {
      get { return this._message; }
    }
    #endregion

    #region public JournalRecordType RecordType
    public JournalRecordType RecordType {
      get { return this._rtype; }
    }
    #endregion
  }
  #endregion

  #region enum JournalRecordType
  enum JournalRecordType{
    Unknow,
    Sell,
    Buy
  }
  #endregion

  #region class TestServerLog: IServerLog
  class TestServerLog: IServerLog {

    private List<TestServerLogRecord> _log;

    #region public TestServerLog()
    public TestServerLog() {
      _log = new List<TestServerLogRecord>();
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _log.Count;}
    }
    #endregion

    #region public IServerLogRecord this[int index]
    public IServerLogRecord this[int index] {
      get { return _log[index]; }
    }
    #endregion

    #region public void Add(TestServerLogRecord record)
    public void Add(TestServerLogRecord record) {
      _log.Add(record);
    }
    #endregion
  }
  #endregion

  #region class TestServerLogRecord:IServerLogRecord
  class TestServerLogRecord:IServerLogRecord {

    private ServerCommand _command;
    private float _balance;
    private string _tradeId;
    private string _orderId;
    private TradeType _ttype;
    private OrderType _orderType;
    private TradeCloseType _closetype;
    private int _lots;
    private float _rate, _stop, _limit, _netpl;
    private DateTime _time;
    private ISymbol _symbol;

    public TestServerLogRecord(
      ServerCommand commnad,
      float balance,
      string tradeid,
      TradeType tradeType,
      TradeCloseType closeType,
      int lots,
      float rate, float stop, float limit, float netpl,
      DateTime time
      ) {
      _command = commnad;
      _balance = balance;
      _tradeId = tradeid;
      _ttype = tradeType;
      _closetype = closeType;
      _lots = lots;
      _rate = rate;
      _stop = stop;
      _limit = limit;
      _netpl = netpl;
      _time = time;
    }

    public TestServerLogRecord(ServerCommand commnad, float balance, DateTime time,
      float rate, TestTrade trade) {
      _command = commnad;
      _balance = balance;
      _time = time;
      _rate = rate;

      _tradeId = trade.TradeId;
      _orderId = trade.ParentOrderId;
      _ttype = trade.TradeType;
      _closetype = trade.CloseType;
      _lots = trade.Lots;

      _stop = trade.StopOrder != null? trade.StopOrder.Rate: 0;
      _limit = trade.LimitOrder != null?trade.LimitOrder.Rate:0;
      _netpl = trade.NetPL;
      _symbol = trade.Symbol;
    }

    public TestServerLogRecord(ServerCommand command, float balance, DateTime time,
      TestOrder order) {
      _command = command;
      _balance = balance;
      _time = time;
      _rate = order.Rate;

      _orderId = order.OrderId;
      _ttype = order.TradeType;
      _orderType = order.OrderType;
      _lots = order.Lots;

      _stop = order.StopOrder != null ? order.StopOrder.Rate : 0;
      _limit = order.LimitOrder != null ? order.LimitOrder.Rate : 0;
      _symbol = order.Symbol;
    }

    #region public ServerCommand Command
    public ServerCommand Command {
      get { return _command; }
    }
    #endregion
    #region public float Balance
    public float Balance {
      get { return _balance; }
    }
    #endregion
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
    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _ttype; }
    }
    #endregion
    #region public Gordago.Analysis.TradeCloseType CloseAt
    public Gordago.TradeCloseType CloseAt {
      get { return _closetype; }
    }
    #endregion
    #region public int Lots
    public int Lots {
      get { return _lots; }
    }
    #endregion
    #region public float Rate
    public float Rate {
      get { return _rate; }
    }
    #endregion
    #region public float Stop
    public float Stop {
      get { return _stop; }
    }
    #endregion
    #region public float Limit
    public float Limit {
      get { return _limit; }
    }
    #endregion
    #region public float NetPL
    public float NetPL {
      get { return _netpl; }
    }
    #endregion
    #region public DateTime Time
    public DateTime Time {
      get { return _time; }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion

    #region public OrderType OrderType
    public OrderType OrderType {
      get { return _orderType; }
    }
    #endregion
  }
  #endregion

  class TestServer:IServer {

    private TestTradeList _trades, _historyTrades;
    private TestOrderList _orders;
    
    private const int MIN_STOP = 4;
    private const int MIN_LIMIT = 4;

    private const int MIN_ENTRY_ORDER = 1;

    private int _nextTradeId, _nextOrderId;
    private JournalComment _journal;
    private DateTime _time;
    private SymbolsProperty _sprops;
    private int _currentUpdateIndex = 0;
    private int _prevUpdateIndex = 0;
    private int _lotsize = 100;

    private float _balance, _netpl;
    private int _lots;
    private TestServerLog _log;
    private int _marginCall;
    private float _fee;

    public TestServer(SymbolsProperty sprops, float balance, int marginCall, float fee) {
      _sprops = sprops;
      _trades = new TestTradeList();
      _historyTrades = new TestTradeList();
      _orders = new TestOrderList();
      _nextTradeId = _nextOrderId = 1;
      _journal = new JournalComment();
      _balance = balance;
      _marginCall = marginCall;
      _fee = fee;
      _lots = 0;
      _netpl = 0;
      _log = new TestServerLog();
    }

    #region public float Balance
    public float Balance {
      get { return _balance; }
    }
    #endregion

    #region public float Equity
    public float Equity {
      get { return _balance - _netpl; }
    }
    #endregion

    #region public float UsedMargin
    public float UsedMargin {
      get { return _lots * _lotsize; }
    }
    #endregion

    #region public float UsableMargin
    public float UsableMargin {
      get { return this.Equity - this.UsedMargin; }
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

    #region public ITradeList Trades
    public ITradeList Trades {
      get { return _trades; }
    }
    #endregion

    #region public ITradeList TradesHistory
    public ITradeList TradesHistory {
      get { return _historyTrades; }
    }
    #endregion

    #region public IOrderList Orders
    public IOrderList Orders {
      get { return _orders; }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get { return _time; }
    }
    #endregion

    #region public IServerLog Log
    public IServerLog Log {
      get { return _log; }
    }
    #endregion

    #region internal Journal Journal
    internal JournalComment Journal {
      get { return _journal; }
    }
    #endregion

    #region internal void SetTime(DateTime time)
    internal void SetTime(DateTime time) {
      _time = time;
    }
    #endregion

    public ServerError TradeOpen(ISymbol symbol, TradeType tradetype, int lots, int slippage, float stopRate, float limitRate) {
      return this.TradeOpen(symbol, tradetype, lots, slippage, stopRate, limitRate, 0, null);
    }

    public ServerError TradeOpen(ISymbol symbol, TradeType tradetype, int lots, int slippage) {
      return TradeOpen(symbol, tradetype, lots, slippage, float.NaN, float.NaN);
    }

    private ServerError TradeOpen(ISymbol symbol, TradeType tradetype, int lots, int slippage, float stopRate, float limitRate, int trailPoint, TestOrder fromOrder) {
      ServerError error = TradeOpenMethod(symbol, tradetype, lots, slippage, stopRate, limitRate, trailPoint, fromOrder);
      string msg = "Open " + (tradetype == TradeType.Sell ? "sell " : "buy ");

      JournalRecordType rtype = JournalRecordType.Unknow;
      if(tradeId < 1) {
        msg += ConvertServerError(tradeId);
      } else {
        TestTrade trade = _trades.GetTrade(tradeId) as TestTrade;
        msg += "#" + tradeId.ToString();
        msg += " Rate: " + SymbolManager.ConvertToCurrencyString(trade.OpenRate, trade.Symbol.DecimalDigits);
        if(trade.StopOrder != null)
          msg += " Stop: " + SymbolManager.ConvertToCurrencyString(trade.StopOrder.Rate, trade.Symbol.DecimalDigits);
        if(trade.LimitOrder != null)
          msg += " Limit: " + SymbolManager.ConvertToCurrencyString(trade.LimitOrder.Rate, trade.Symbol.DecimalDigits);

        rtype = tradetype == TradeType.Sell ? JournalRecordType.Sell : JournalRecordType.Buy;
        _log.Add(new TestServerLogRecord(ServerCommand.TradeOpen, this.Balance, this.Time, trade.OpenRate, trade));
      }
      JournalAddRec(msg, rtype, symbol);
      return tradeId;
    }

    private ServerError TradeOpenMethod(ISymbol symbol, TradeType tradetype, int lots, int slippage, float stopRate, float limitRate, int trailPoint, TestOrder fromOrder) {

      TestSymbol tsymbol = symbol as TestSymbol;
      if(tsymbol.Property == null) {
        tsymbol.Property = _sprops.GetProperty(tsymbol.Name);
      }

      float rate = tradetype == TradeType.Sell ? tsymbol.Bid : tsymbol.Ask;
      float spread = tsymbol.Ask - tsymbol.Bid;

      bool useStop = false, useLimit = false;

      if(!float.IsNaN(stopRate) && stopRate > 0) {
        if(!CheckStopLimit(tsymbol, tradetype, OrderStopLimit.Stop, rate, stopRate))
          return ServerError.TradeOpenError;
        useStop = true;
      }

      if(!float.IsNaN(limitRate) && limitRate > 0) {
        if(!CheckStopLimit(tsymbol, tradetype, OrderStopLimit.Limit, rate, limitRate))
            return ServerError.TradeOpenError;
        useLimit = true;
      }

      TestTrade trade = new TestTrade(tsymbol, Convert.ToString(_nextTradeId++), tradetype, lots, rate, tsymbol.Time);
      _trades.Add(trade);
      string tradeId = trade.TradeId;

      if(useStop) 
        CreateStopLimitOnTrade(trade, OrderType.Stop, stopRate);

      if(useLimit) 
        CreateStopLimitOnTrade(trade, OrderType.Limit, limitRate);

      _balance += trade.NetPL;

      if(fromOrder != null) {
        trade.SetParentOrderId(fromOrder.OrderId);
      }
      this.UpdatePrice((TestSymbol)trade.Symbol);

      return tradeId;
    }

    private bool CheckStopLimit(ISymbol symbol, TradeType tradeType, OrderStopLimit orderStopLimit, float rate, float checkRate) {

      if(float.IsNaN(checkRate) || checkRate == 0)
        return true;

      float bid = symbol.Bid, ask = symbol.Ask;
      float spread = ask - bid;

      float minpoint = (ask - bid) + symbol.Point;

      if(orderStopLimit == OrderStopLimit.Stop) {

        float minstop = MIN_STOP * symbol.Point;
        if(tradeType == TradeType.Sell) {
          if(checkRate - spread - minstop - rate < 0)
            return false;
        } else {
          if(rate - minstop - checkRate < 0)
            return false;
        }
        return true;
      } else {

        float minlimit = MIN_LIMIT * symbol.Point;
        if(tradeType == TradeType.Sell) {
          if(rate - spread - minlimit - checkRate < 0)
            return false;
        } else {
          if(checkRate - minlimit - rate < 0)
            return false;
        }
        return true;
      }
    }

    private void CreateStopLimitOnTrade(TestTrade trade, OrderType orderType, float rate) {
      TradeType closeOrderTradeType = trade.TradeType == TradeType.Sell ? TradeType.Buy : TradeType.Sell;

      TestOrder order = new TestOrder(trade.Symbol, Convert.ToString(_nextOrderId++), closeOrderTradeType, orderType, trade.Lots, rate, trade.OpenTime);
      order.SetTradeId(trade.TradeId);
      
      if(orderType == OrderType.Stop)
        trade.SetStopOrder(order);
      else
        trade.SetLimitOrder(order);

      _orders.Add(order);
    }

    public IServerError TradeModify(string tradeId, OrderStopLimit orderStopLimit, float newRate) {
      int error = ModifyTradeMethod(tradeId, orderStopLimit, newRate);
      string msg = "Modify #"+tradeId.ToString();
      msg += " " + (orderStopLimit == OrderStopLimit.Stop ? "Stop" : "Limit");
      TestTrade trade = _trades.GetTrade(tradeId) as TestTrade;

      if(error < 0) {
        msg += " " + ConvertServerError(error);
      } else {
        msg += " Rate: " + SymbolManager.ConvertToCurrencyString(newRate, trade.Symbol.DecimalDigits);
      }
      JournalAddRec(msg, JournalRecordType.Unknow, trade == null ? null : trade.Symbol);
      return error;
    }

    private IServerError ModifyTradeMethod(int tradeId, OrderStopLimit orderStopLimit, float newRate) {
      TestTrade trade = this._trades.GetTrade(tradeId) as TestTrade;
      if(trade == null) return ServerError.TRADE_MODIFY_ERROR;
      newRate = newRate > 0 ? newRate : 0;
      TestOrder order = (orderStopLimit == OrderStopLimit.Stop ? trade.StopOrder : trade.LimitOrder) as TestOrder;
      float rate = trade.TradeType == TradeType.Sell ? trade.Symbol.Bid : trade.Symbol.Ask;

      if(order != null && newRate == 0) { /* delete order */
        if(orderStopLimit == OrderStopLimit.Stop) {
          trade.SetStopOrder(null);
        } else {
          trade.SetLimitOrder(null);
        }
        this.OrderDeleteMethod(order.OrderId);
      } if(order == null && newRate > 0) { /* Create order */
        if(orderStopLimit == OrderStopLimit.Stop) {
          if(!CheckStopLimit(trade.Symbol, trade.TradeType, OrderStopLimit.Stop, rate, newRate))
            return ServerError.TRADE_MODIFY_ERROR;
          CreateStopLimitOnTrade(trade, OrderType.Stop, newRate);
        } else {
          if(!CheckStopLimit(trade.Symbol, trade.TradeType, OrderStopLimit.Limit, rate, newRate))
            return ServerError.TRADE_MODIFY_ERROR;
          CreateStopLimitOnTrade(trade, OrderType.Limit, newRate);
        }
      }else if (order != null && newRate > 0){ /* Modify order */
        if(orderStopLimit == OrderStopLimit.Stop) {
          if(!CheckStopLimit(trade.Symbol, trade.TradeType, OrderStopLimit.Stop, rate, newRate))
            return ServerError.TRADE_MODIFY_ERROR;
        } else {
          if(!CheckStopLimit(trade.Symbol, trade.TradeType, OrderStopLimit.Limit, rate, newRate))
            return ServerError.TRADE_MODIFY_ERROR;
        }
        order.SetRate(newRate);
      }

      _log.Add(new TestServerLogRecord(ServerCommand.TradeModify, this.Balance, this.Time, rate, trade));

      return ServerError.OK;
    }

    public IServerError TradeClose(string tradeId, int lots, int slippage) {
      return CloseTrade(tradeId, lots, slippage, TradeCloseType.Custom);
    }

    private IServerError CloseTrade(int tradeId, int lots, int slippage, TradeCloseType tcloseType) {
      TestTrade trade = _trades.GetTrade(tradeId) as TestTrade;
      int numerror = CloseTradeMethod(tradeId, lots, slippage, tcloseType);

      string msg = "";

      if(numerror < 0) {
        msg = "Close #" + tradeId.ToString() + " " + ConvertServerError(numerror);
      } else {
        msg = "Close " + (trade.TradeType == TradeType.Sell ? "sell " : "buy ") +
          "#" + tradeId.ToString();

        msg += " Rate: " + SymbolManager.ConvertToCurrencyString(trade.OpenRate, trade.Symbol.DecimalDigits);

        if(trade.CloseType == TradeCloseType.Limit)
          msg += " by Limit";
        else if(trade.CloseType == TradeCloseType.Stop)
          msg += " by Stop";
      }

      msg += " Balance: " + SymbolManager.ConvertToCurrencyString(this.Balance);

      JournalAddRec(msg, JournalRecordType.Unknow, trade == null ? null : trade.Symbol);
      return numerror;
    }

    private IServerError CloseTradeMethod(int tradeId, int lots, int slippage, TradeCloseType tcloseType) {
      if(lots == 0)
        return ServerError.OK;

      TestTrade trade = this._trades.GetTrade(tradeId) as TestTrade;
      if(trade == null)
        return ServerError.TRADE_CLOSE_ERROR;

      TestSymbol tsymbol = trade.Symbol as TestSymbol;
      float rate = trade.TradeType == TradeType.Sell ? tsymbol.Ask : tsymbol.Bid;

      TestTrade ctrade = null;

      if(lots < trade.Lots) {
        trade.SubLots(lots);
        if(trade.StopOrder != null)
          (trade.StopOrder as TestOrder).SetLots(trade.Lots);
        if(trade.LimitOrder != null)
          (trade.LimitOrder as TestOrder).SetLots(trade.Lots);

        ctrade = new TestTrade(trade.Symbol, trade.TradeId, trade.TradeType, lots, trade.OpenRate, trade.OpenTime);
      } else {
        if(lots > trade.Lots)
          lots = trade.Lots;

        _trades.Remove(trade.TradeId);
        ctrade = trade;
        
        if(trade.StopOrder != null) 
          this.OrderDeleteMethod(trade.StopOrder.OrderId);
        if(trade.LimitOrder != null)
          this.OrderDeleteMethod(trade.LimitOrder.OrderId);
      }

      ctrade.Close(rate, tsymbol.Time, tcloseType);
      _historyTrades.Add(ctrade);

      _balance += ctrade.NetPL;
      this.UpdatePrice((TestSymbol)trade.Symbol);

      _log.Add(new TestServerLogRecord(ServerCommand.TradeClose, this.Balance, this.Time, rate, ctrade));

      return ServerError.OK;
    }

    public IServerError OrderCreate(ISymbol symbol, CreateEntryOrderType entryOrderType, int lots, float rate, float stopRate, float limitRate) {
      return this.CreateOrderMethod(symbol, entryOrderType, lots, rate, stopRate, limitRate);
    }

    public IServerError OrderCreate(ISymbol symbol, CreateEntryOrderType entryOrderType, int lots, float rate) {
      return this.OrderCreate(symbol, entryOrderType, lots, rate, 0, 0);
    }

    private IServerError CreateOrderMethod(ISymbol symbol, CreateEntryOrderType entryOrderType, int lots, float rate, float stopRate, float limitRate) {

      if(!this.CheckOrderValues(symbol, entryOrderType, rate, stopRate, limitRate))
        return ServerError.ORDER_CREATE_ERROR;

      float bid = symbol.Bid;
      float ask = symbol.Ask;
      stopRate = stopRate > 0 ? stopRate : 0;
      limitRate = limitRate > 0 ? limitRate : 0;

      TradeType tradeType = TradeType.Sell;
      OrderType orderType = OrderType.Stop;

      switch(entryOrderType) {
        case CreateEntryOrderType.BuyLimit:
          tradeType = TradeType.Buy;
          orderType = OrderType.EntryLimit;
          break;
        case CreateEntryOrderType.BuyStop:
          tradeType = TradeType.Buy;
          orderType = OrderType.EntryStop;
          break;
        case CreateEntryOrderType.SellLimit:
          tradeType = TradeType.Sell;
          orderType = OrderType.EntryLimit;
          break;
        case CreateEntryOrderType.SellStop:
          tradeType = TradeType.Sell;
          orderType = OrderType.EntryStop;
          break;
      }

      TestOrder order = new TestOrder(symbol, _nextOrderId++, tradeType, orderType, lots, rate, this.Time);
      _orders.Add(order);
      OrderModifyMethod(order, lots, rate, stopRate, limitRate);
      _log.Add(new TestServerLogRecord(ServerCommand.EntryOrderCreate, this.Balance, this.Time, order));
      return order.OrderId;
    }

    public IServerError OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate) {
      newStopRate = newStopRate > 0 ? newStopRate : 0;
      newLimitRate = newLimitRate > 0 ? newLimitRate : 0;
      TestOrder order = _orders.GetOrder(orderId) as TestOrder;
      if(order != null) {
        float rate = order.Rate;
        float stop = order.StopOrder != null ? order.StopOrder.Rate : 0;
        float limit = order.LimitOrder != null ? order.LimitOrder.Rate : 0;
        if(rate == newRate && stop == newStopRate && limit == newLimitRate)
          return ServerError.OK;
      } else {
        return ServerError.ORDER_MODIFY_ERROR;
      }

      int error = this.OrderModifyMethod(orderId, lots, newRate, newStopRate, newLimitRate);

      string msg = "Modify order ";

      JournalRecordType rtype = JournalRecordType.Unknow;
      if(error < 0) {
        msg += ConvertServerError(orderId);
      } else {
        msg += "#" + orderId.ToString();
        msg += " Rate: " + SymbolManager.ConvertToCurrencyString(order.Rate, order.Symbol.DecimalDigits);
        if(order.StopOrder != null)
          msg += " Stop: " + SymbolManager.ConvertToCurrencyString(order.StopOrder.Rate, order.Symbol.DecimalDigits);
        if(order.LimitOrder != null)
          msg += " Limit: " + SymbolManager.ConvertToCurrencyString(order.LimitOrder.Rate, order.Symbol.DecimalDigits);

        _log.Add(new TestServerLogRecord(ServerCommand.EntryOrderModify, this.Balance, this.Time, order));
        JournalAddRec(msg, rtype, order.Symbol);
      }

      return orderId;
    }

    private IServerError OrderModifyMethod(string orderId, int lots, float newRate, float newStopRate, float newLimitRate) {
      TestOrder order = _orders.GetOrder(orderId) as TestOrder;
      if(order == null)
        return ServerError.ORDER_MODIFY_ERROR;

      CreateEntryOrderType entryOrderType;

      if(order.OrderType == OrderType.EntryLimit) {
        entryOrderType = order.TradeType == TradeType.Sell ? CreateEntryOrderType.SellLimit : CreateEntryOrderType.BuyLimit;
      } else if(order.OrderType == OrderType.EntryStop) {
        entryOrderType = order.TradeType == TradeType.Sell ? CreateEntryOrderType.SellStop : CreateEntryOrderType.BuyStop;
      } else {
        return ServerError.ORDER_MODIFY_ERROR;
      }

      if(!this.CheckOrderValues(order.Symbol, entryOrderType, newRate, newStopRate, newLimitRate))
        return ServerError.ORDER_MODIFY_ERROR;

      this.OrderModifyMethod(order, lots, newRate, newStopRate, newLimitRate);

      return ServerError.OK;
    }

    #region private void OrderModifyMethod(Order order, int lots, float rate, float stopRate, float limitRate)
    private void OrderModifyMethod(TestOrder order, int lots, float rate, float stopRate, float limitRate) {
      this.OrderModifyStopLimit(order, OrderType.Stop, stopRate);
      this.OrderModifyStopLimit(order, OrderType.Limit, limitRate);
      order.SetRate(rate);
      order.SetLots(lots);
    }
    #endregion

    #region private void OrderModifyStopLimit(Order entryOrder, OrderType orderType, float rate)
    /// <summary>
    /// Р_Р_Р_РёС"РёРєР°С┼РёС_(С_Р_Р·Р_Р°Р_РёРч, С_Р_Р°Р>РчР_РёРч) С_С'Р_Рї-Р>РёР_РёС' Р_С_Р_РчС_Р° Р_Р° Р_С'Р>Р_РРчР_Р_Р_Р_ Р_С_Р_РчС_Рч.
    /// </summary>
    private void OrderModifyStopLimit(TestOrder entryOrder, OrderType orderType, float rate) {
      TradeType closeOrderTradeType = entryOrder.TradeType == TradeType.Sell ? TradeType.Buy : TradeType.Sell;
      rate = rate > 0 ? rate : 0;

      TestOrder curorder = orderType == OrderType.Stop ? (TestOrder)entryOrder.StopOrder : (TestOrder)entryOrder.LimitOrder;

      if(curorder == null && rate > 0) { // create
        TestOrder order = new TestOrder(entryOrder.Symbol, _nextOrderId++, closeOrderTradeType, orderType, entryOrder.Lots, rate, entryOrder.Time);

        if(orderType == OrderType.Stop)
          entryOrder.SetStopOrder(order);
        else
          entryOrder.SetLimitOrder(order);

        _orders.Add(order);
      } else if(curorder != null && rate > 0) { // modify
        curorder.SetRate(rate);
      } else if (curorder != null && rate == 0){ // delete
        
        if(orderType == OrderType.Stop)
          entryOrder.SetStopOrder(null);
        else
          entryOrder.SetLimitOrder(null);
        this.OrderDeleteMethod(curorder.OrderId);
      }
    }
    #endregion

    #region private bool CheckOrderValues(ISymbol symbol, CreateEntryOrderType entryOrderType, float rate, float stopRate, float limitRate)
    private bool CheckOrderValues(ISymbol symbol, CreateEntryOrderType entryOrderType, float rate, float stopRate, float limitRate) {
      float bid = symbol.Bid;
      float ask = symbol.Ask;

      float minpoint = (ask - bid) + symbol.Point;

      switch(entryOrderType) {
        case CreateEntryOrderType.BuyLimit:
          if(rate + minpoint >= ask)
            return false;
          break;
        case CreateEntryOrderType.BuyStop:
          if(rate - minpoint <= ask)
            return false;
          break;
        case CreateEntryOrderType.SellLimit:
          if(rate - minpoint <= bid)
            return false;
          break;
        case CreateEntryOrderType.SellStop:
          if(rate + minpoint >= bid)
            return false;
          break;
      }

      switch(entryOrderType) {
        case CreateEntryOrderType.BuyLimit:
        case CreateEntryOrderType.BuyStop:
          if(!this.CheckStopLimit(symbol, TradeType.Buy, OrderStopLimit.Stop, rate, stopRate))
            return false;
          if(!this.CheckStopLimit(symbol, TradeType.Buy, OrderStopLimit.Limit, rate, limitRate))
            return false;
          break;
        case CreateEntryOrderType.SellLimit:
        case CreateEntryOrderType.SellStop:
          if(!this.CheckStopLimit(symbol, TradeType.Sell, OrderStopLimit.Stop, rate, stopRate))
            return false;
          if(!this.CheckStopLimit(symbol, TradeType.Sell, OrderStopLimit.Limit, rate, limitRate))
            return false;
          break;
      }
      return true;
    }
    #endregion

    public IServerError OrderDelete(string orderId) {
      TestOrder order = OrderDeleteMethod(orderId);
      if( order == null) {
        return ServerError.ORDER_DELETE_ERROR;
      }
      _log.Add(new TestServerLogRecord(ServerCommand.EntryOrderDelete, this.Balance, this.Time, order));
      return ServerError.OK;
    }

    private TestOrder OrderDeleteMethod(string orderId) {
      TestOrder order = _orders.Remove(orderId);
      if(order == null)
        return null;

      if(order.StopOrder != null)
        this.OrderDeleteMethod(order.StopOrder.OrderId);

      if(order.LimitOrder != null)
        this.OrderDeleteMethod(order.LimitOrder.OrderId);

      return order;
    }

    #region private void ReCalculateValues()
    /// <summary>
    /// Р_РчС_РчС_С╪РчС' Р_С_РчС: Р·Р_Р°С╪РчР_РёР№ (Р_Р>Р°Р_Р°С_С%Р°С_ РїС_РёР+С<Р>С_, Р'Р°Р>Р°Р_С_ Рё РїС_Р_С╪РчРч)
    /// </summary>
    private void ReCalculateValues() {
      if(_currentUpdateIndex == _prevUpdateIndex) return;

      _prevUpdateIndex = _currentUpdateIndex;
    }
    #endregion

    #region internal void UpdatePrice(TestSymbol symbol)
    internal void UpdatePrice(TestSymbol symbol) {

      /* РїС_Р_Р_РчС_РєР° Р_С'Р>Р_РРчР_Р_С<С: Р_С_Р_РчС_Р_Р_, С_С'Р_РїР_Р_, С'С_РчР№Р>Р_Р_ Рё Р>РёР_РёС'Р_Р_ */
      for(int i = 0; i < _orders.Count; i++) {
        TestOrder order = _orders[i] as TestOrder;
        TestSymbol tsymbol = order.Symbol as TestSymbol;
        float bid = tsymbol.Bid;
        float ask = tsymbol.Ask;
        float rate = order.Rate;
        TradeType ttype = order.TradeType;

        bool isactive = false;
        switch(order.OrderType) {
          case OrderType.Stop:
            if(order.TradeId > 0 && ttype == TradeType.Sell && bid <= rate) isactive = true;
            else if(order.TradeId > 0 && ttype == TradeType.Buy && ask >= rate) isactive = true;
            break;
          case OrderType.Limit:
            if(order.TradeId > 0 && ttype == TradeType.Sell && bid >= rate) isactive = true;
            else if(order.TradeId > 0 && ttype == TradeType.Buy && ask <= rate) isactive = true;
            break;
          case OrderType.EntryStop:
            if(ttype == TradeType.Sell && bid <= rate) isactive = true;
            else if(ttype == TradeType.Buy && ask >= rate) isactive = true;
            break;
          case OrderType.EntryLimit:
            if(ttype == TradeType.Sell && bid >= rate) isactive = true;
            else if(ttype == TradeType.Buy && ask <= rate) isactive = true;
            break;
        }
        if(isactive) {
          if(order.OrderType == OrderType.Stop || order.OrderType == OrderType.Limit) {
            this.CloseTrade(order.TradeId, order.Lots, 0, order.OrderType == OrderType.Stop ? TradeCloseType.Stop : TradeCloseType.Limit);
          } else {
            this.ActivateEntryOrder(order);
          }
        }
      }

      _netpl = 0;
      _lots = 0;
      /* Р_Р+Р_Р_Р_Р>РчР_РёРч С_С'Р°С'РёС_С'РёРєРё РїР_ Р_С'РєС_С<С'С<Р_ РїР_Р·РёС┼РёС_Р_ */
      for(int i = 0; i < _trades.Count; i++) {
        TestTrade trade = _trades[i] as TestTrade;
        trade.UpdateStatistic();
        _netpl += trade.NetPL;
        _lots += trade.Lots;
      }
    }
    #endregion

    #region private int ActivateEntryOrder(Order order)
    /// <summary>
    /// Р_РєС'РёР_Р°С┼РёС_ Р_С'Р>Р_РРчР_Р_Р_Р_Р_ Р_С_Р_РчС_Р°
    /// </summary>
    /// <param name="order">Р_С'Р>Р_РРчР_Р_С<Р№ Р_С_Р_РчС_</param>
    /// <returns>Р_Р_РчР_С'РёС"РёРєР°С'Р_С_ Р_С'РєС_С<С'Р_Р№ РїР_Р·РёС┼РёРё, Р>РёР+Р_ Р_Р_Р_РчС_ Р_С_РёР+РєРё</returns>
    private int ActivateEntryOrder(TestOrder order) {
      float stopRate = order.StopOrder != null ? order.StopOrder.Rate : 0;
      float limitRate = order.LimitOrder != null ? order.LimitOrder.Rate : 0;

      this.OrderDeleteMethod(order.OrderId);

      return TradeOpen(order.Symbol, order.TradeType, order.Lots, 0, stopRate, limitRate, 0, order);
    }
    #endregion

    #region public void JournalAddRec(string text, JournalRecordType rtype, ISymbol symbol)
    internal void JournalAddRec(string text, JournalRecordType rtype, ISymbol symbol) {
      string msg = _time.ToShortDateString() + " " + _time.ToShortTimeString() ;
      if(symbol != null)
        msg += " " +symbol.Name+ " " + SymbolManager.ConvertToCurrencyString(symbol.Bid, symbol.DecimalDigits);
//          " Ask=" + SymbolEngine.ConvertToCurrencyString(symbol.Ask, symbol.DecimalDigits);
      msg += ": " + text;
      this.Journal.Add(DateTime.Now, msg, rtype);
    }
    #endregion

    public string ConvertServerError(ServerError error) {
      switch(error) {
        case ServerError.None:
          return "OK";
        case ServerError.TradeOpenError:
          return "Error: Open trade";
        case ServerError.TradeModifyError:
          return "Error: Modify trade";
        case ServerError.TradeCloseError:
          return "Error: Close trade";
        case ServerError.OrderCreateError:
          return "Error: Create order";
        case ServerError.OrderModifyError:
          return "Error: Modify order";
        case ServerError.OrderDeleteError:
          return "Error: Delete order";
      }
      return "Unknow error";
    }

    #region public void Comment(string text)
    public void Comment(string text) {
      _journal.Add(DateTime.Now,string.Format("{0} {1} Comment: {2}", _time.ToShortDateString(), _time.ToShortTimeString(), text));
    }
    #endregion

  }
}
