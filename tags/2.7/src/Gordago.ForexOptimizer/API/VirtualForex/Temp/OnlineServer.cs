/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis;
using System.Threading;
using Gordago.Strategy;

namespace Gordago.API {

  #region class Order:IOrder
  class Order:IOrder {

    private IAccount _account;
    private IOrder _order;
    private TestSymbol _symbol;
    private OnlineServer _server;

    public Order(OnlineServer server, IOrder order) {
      _order = order;
      _server = server;
      _symbol = server.Symbols[order.OnlineRate.Symbol];
    }

    internal IOrder IOrder {
      get { return _order; }
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
      get { return Convert.ToInt32(_order.OrderId); }
    }
    #endregion
    #region public string ParentOrderId
    public string ParentOrderId {
      get { return 0; }
    }
    #endregion
    #region public string TradeId
    public string TradeId {
      get {return  _order.TradeId; }
    }
    #endregion
    #region public TradeType TradeType
    public TradeType TradeType {
      get { return Trade.ConvertTradeType(_order.TradeType); }
    }
    #endregion
    #region public OrderType OrderType
    public OrderType OrderType {
      get {
        return ConvertOrderType(_order.OrderType);
      }
    }
    #endregion
    #region public int Lots
    public int Lots {
      get { return _order.Lots; }
    }
    #endregion
    #region public float Rate
    public float Rate {
      get { return _order.Rate; }
    }
    #endregion
    #region public DateTime Time
    public DateTime Time {
      get { return _order.Time; }
    }
    #endregion
    #region public IOrder StopOrder
    public IOrder StopOrder {
      get {
        if(_order.StopOrder == null)
          return null;
        return this._server.Orders.GetOrder(Convert.ToInt32(_order.StopOrder.OrderId)); 
      }
    }
    #endregion
    #region public IOrder LimitOrder
    public IOrder LimitOrder {
      get {
        if(_order.LimitOrder == null)
          return null;
        return this._server.Orders.GetOrder(Convert.ToInt32(_order.LimitOrder.OrderId));
      }
    }
    #endregion
    #region public static OrderType ConvertOrderType(OrderType apiOrderType)
    public static OrderType ConvertOrderType(OrderType apiOrderType) {
      switch (apiOrderType){
        case OrderType.EntryLimit:
          return OrderType.EntryLimit;
        case OrderType.EntryStop:
          return OrderType.EntryStop;
        case OrderType.Limit:
          return OrderType.Limit;
        case OrderType.Stop:
          return OrderType.Stop;
      }
      return OrderType.Stop;
    }
    #endregion
  }
  #endregion

  #region class OrderList:IOrderList
  class OrderList:IOrderList {
    private OnlineServer _server;
    private List<Order> _orders;

    public OrderList(OnlineServer server) {
      _orders = new List<Order>();
      _server = server;
      this.Update();
    }

    #region public int Count
    public int Count {
      get { return _orders.Count; }
    }
    #endregion
    #region public IOrder this[int index]
    public IOrder this[int index] {
      get { return _orders[index]; }
    }
    #endregion
    #region public IOrder GetOrder(string orderId)
    public IOrder GetOrder(string orderId) {
      for(int i = 0; i < _orders.Count; i++) {
        if(_orders[i].OrderId == orderId)
          return _orders[i];
      }
      return null;
    }
    #endregion

    #region public void Update()
    public void Update() {
      APIOrderList apiOrders = _server.API.APITrader.Orders;
      List<Order> neworders = new List<Order>();
      for(int i = 0; i < apiOrders.Count; i++) {
        IOrder apiorder = apiOrders[i];
        Order order = this.GetOrder(Convert.ToInt32(apiorder.OrderId)) as Order;
        if(order == null) {
          order = new Order(_server, apiorder);
        }
        neworders.Add(order);
      }
      _orders.Clear();
      _orders.AddRange(neworders.ToArray());
    }
    #endregion
  }
  #endregion

  #region class TradeList:ITradeList
  class TradeList:ITradeList {
    private List<Trade> _trades;
    private OnlineServer _server;

    public TradeList(OnlineServer server) {
      _server = server;
      _trades = new List<Trade>();
    }
    #region public int Count
    public int Count {
      get { return _trades.Count; }
    }
    #endregion
    #region public ITrade this[int index]
    public ITrade this[int index] {
      get { return this._trades[index]; }
    }
    #endregion
    #region public ITrade GetTrade(int tradeId)
    public ITrade GetTrade(string tradeId) {
      for(int i = 0; i < _trades.Count; i++) {
        if(_trades[i].TradeId == tradeId)
          return _trades[i];
      }
      return null;
    }
    #endregion

    #region public void Update()
    public void Update() {
      APITradeList apiTrades = _server.API.APITrader.Trades;
      List<Trade> newtrades = new List<Trade>();
      for(int i = 0; i < apiTrades.Count; i++) {
        ITrade apitrade = apiTrades[i];
        Trade trade = this.GetTrade(Convert.ToInt32(apitrade.TradeId)) as Trade;
        if(trade == null)
          trade = new Trade(_server, apitrade);
        newtrades.Add(trade);
      }
      _trades.Clear();
      _trades.AddRange(newtrades.ToArray());
    }
    #endregion
  }
  #endregion 

  #region class Trade:ITrade
  class Trade:ITrade {

    private ITrade _trade;
    private OnlineServer _server;
    private TestSymbol _symbol;
    private IAccount _account;

    public Trade(OnlineServer server, ITrade trade) {
      _trade = trade;
      _server = server;
      _symbol = server.Symbols[_trade.OnlineRate.Symbol];
    }

    #region public IAccount Account
    public IAccount Account {
      get { return _account; }
    }
    #endregion

    #region internal ITrade ITrade
    internal ITrade ITrade {
      get { return _trade; }
    }
    #endregion
    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { 
        return  _symbol; 
      }
    }
    #endregion
    #region public string TradeId
    public string TradeId {
      get { return Convert.ToInt32(_trade.TradeId); }
    }
    #endregion
    #region public string ParentOrderId
    public string ParentOrderId {
      get { return _trade.ParentOrderId; }
    }
    #endregion
    #region public int Lots
    public int Lots {
      get {return _trade.Lots; }
    }
    #endregion
    #region public TradeType TradeType
    public TradeType TradeType {
      get { return ConvertTradeType(_trade.TradeType); }
    }
    #endregion

    #region public float OpenRate
    public float OpenRate {
      get { return _trade.OpenRate; }
    }
    #endregion
    #region public DateTime OpenTime
    public DateTime OpenTime {
      get { return _trade.OpenTime; }
    }
    #endregion
    #region public float CloseRate
    public float CloseRate {
      get { return _trade.CloseRate; }
    }
    #endregion
    #region public DateTime CloseTime
    public DateTime CloseTime {
      get { return _trade.CloseTime; }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get { return _trade.GetNetPriceOnline(); }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get { return _trade.Commission; }
    }
    #endregion

    #region public float Fee
    public float Fee {
      get { return _trade.Fee; }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get { return _trade.Premium; }
    }
    #endregion

    #region public IOrder StopOrder
    public IOrder StopOrder {
      get {
        if(_trade.StopOrder == null)
          return null;
        return this._server.Orders.GetOrder(Convert.ToInt32(_trade.StopOrder.OrderId)); 
      }
    }
    #endregion

    #region public IOrder LimitOrder
    public IOrder LimitOrder {
      get {
        if(_trade.LimitOrder == null)
          return null;
        return this._server.Orders.GetOrder(Convert.ToInt32(_trade.LimitOrder.OrderId));
      }
    }
    #endregion

    #region public static TradeType ConvertTradeType(TradeType buysell)
    public static TradeType ConvertTradeType(TradeType buysell) {
      if(buysell == TradeType.Sell)
        return TradeType.Sell;
      return TradeType.Buy;
    }
    #endregion

    #region public static TradeType ConvertAPIBuySell(TradeType tradeType)
    public static TradeType ConvertAPIBuySell(TradeType tradeType) {
      if(tradeType == TradeType.Buy)
        return TradeType.Buy;
      return TradeType.Sell;
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get {
        if(this.StopOrder == null)
          return 0;
        return this.StopOrder.Rate;
      }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get {
        if(this.LimitOrder == null)
          return 0;
        return this.LimitOrder.Rate;
      }
    }
    #endregion
  }
  #endregion

  #region class OnlineSymbols
  class OnlineSymbols{

    private TestSymbol[] _symbols;
    
    public OnlineSymbols(ISymbolList sengine) {
      _symbols = new TestSymbol[sengine.Count];
      for(int i = 0; i < sengine.Count; i++) {
        ISymbol symbol = sengine[i];
        int indexfrom = Math.Max(symbol.Ticks.Count - 10000, 0);
        TestSymbol tsymbol = new TestSymbol(symbol, indexfrom);
        _symbols[i] = tsymbol;
      }
    }

    #region public TestSymbol this[int index]
    public TestSymbol this[int index] {
      get { return this._symbols[index]; }
    }
    #endregion

    #region public TestSymbol this[string name]
    public TestSymbol this[string name] {
      get {
        for(int i = 0; i < _symbols.Length; i++) {
          if(_symbols[i].Name == name)
            return _symbols[i];
        }
        return null;
      }
    }
    #endregion

    #region public TestSymbol this[ISymbol symbol]
    public TestSymbol this[ISymbol symbol]{
      get {
        for(int i = 0; i < _symbols.Length; i++) {
          if(_symbols[i].Symbol == symbol)
            return _symbols[i];
        }
        return null;
      }
    }
    #endregion
  }
  #endregion

  #region class StrategyChart
  class StrategyChart {

    private TestSymbol _symbol;
    private Gordago.Analysis.Strategy _strategy;
    private TestAnalyzer _analyzer;
    private int _id;

    public StrategyChart(int id, OnlineServer server, ISymbol symbol, Gordago.Analysis.Strategy strategy, TradeVariables variables) {
      _symbol = server.Symbols[symbol];
      _strategy = strategy;

      _analyzer = new TestAnalyzer(GordagoMain.IndicatorManager, _symbol);
      _strategy.SetEngine(server, _analyzer, false);
      if (strategy is VisualStrategy) {
        (strategy as VisualStrategy).Compile(variables);
      }
      _id = id;
    }

    #region public int Id
    public int Id {
      get { return this._id; }
    }
    #endregion

    #region public TestSymbol Symbol
    public TestSymbol Symbol {
      get { return this._symbol; }
    }
    #endregion

    #region public TestAnalyzer Analyzer
    public TestAnalyzer Analyzer {
      get { return this._analyzer; }
    }
    #endregion

    #region public Gordago.Analysis.Strategy Strategy
    public Gordago.Analysis.Strategy Strategy {
      get { return this._strategy; }
    }
    #endregion
  }
  #endregion

  class OnlineServer:IServer, IAPIEvents {

    private BrokerCommandManager _api;
    private APIAccount _account;
    private OnlineSymbols _symbols;

    private OrderList _orders;
    private TradeList _trades;

    private List<StrategyChart> _strategy;
    private int _strateIdCounter = 1;

    private APICommand _currentCommand = APICommand.Empty;
    private APIError _currentError = null;
    private object _currentData = null;
    private DateTime _lasttime;
    public OnlineServer() {
      _api = GordagoMain.TraderAPI;
      _account = _api.APITrader.Accounts[0];
      _symbols = new OnlineSymbols(GordagoMain.SymbolEngine);
      _orders = new OrderList(this);
      _trades = new TradeList(this);
      _strategy = new List<StrategyChart>();
      _api.AccountsChangegEvent += new APIAccountHandler(this.API_AccountsChangegEvent);
      _api.CommandStartingEvent += new APICommandStartingHandler(this.API_OnCommandStarting);
      _api.CommandStoppingEvent += new APICommandStoppingHandler(this.API_OnCommandStopping);
      _api.ConnectionStatusChangedEveng += new APIConnStatusHandler(this.API_ConnectionStatusChanged);
      _api.OrdersChangedEvent += new APIOrderHandler(this.API_OrdersChangedEvent);
      _api.PairsChangedEvent += new APIPairHandler(this.API_PairsChangedEvent);
      _api.TradesChangedEvent += new APITradeHandler(this.API_TradesChangedEvent);
    }

    #region internal TraderAPI API
    internal BrokerCommandManager API {
      get { return this._api; }
    }
    #endregion
    #region public OnlineSymbols Symbols
    public OnlineSymbols Symbols {
      get { return this._symbols; }
    }
    #endregion
    #region public float Balance
    public float Balance {
      get { return _account.Balance; }
    }
    #endregion
    #region public float Equity
    public float Equity {
      get { return _account.Equity; }
    }
    #endregion
    #region public float UsedMargin
    public float UsedMargin {
      get { return _account.UsedMargin; }
    }
    #endregion
    #region public float UsableMargin
    public float UsableMargin {
      get { return _account.UsableMargin; }
    }
    #endregion
    #region public float NetPL
    public float NetPL {
      get { return _account.NetPL; }
    }
    #endregion
    #region public int Lots
    public int Lots {
      get { return _account.OpenPositions; }
    }
    #endregion
    #region public DateTime Time
    public DateTime Time {
      get { return _lasttime; }
    }
    #endregion
    #region public ITradeList Trades
    public ITradeList Trades {
      get { return _trades; }
    }
    #endregion
    #region public ITradeList TradesHistory
    public ITradeList TradesHistory {
      get { return null;}
    }
    #endregion
    #region public IOrderList Orders
    public IOrderList Orders {
      get { return _orders; }
    }
    #endregion

    #region public int AddStrategy(Gordago.Analysis.Strategy strategy, Symbol symbol)
    /// <summary>
    /// Добавление стратегии на исполнение в реале
    /// </summary>
    /// <param name="strategy"></param>
    /// <param name="symbol"></param>
    /// <returns>Идентификатор стратегии, если -1, стратегия не добавлена, так как не прошла инициализацию</returns>
    public int AddStrategy(CompileDllData data, ISymbol symbol) {

      if(!data.Strategy.OnLoad()) return -1;

      StrategyChart schart = new StrategyChart(_strateIdCounter++, this, symbol, data.Strategy, data.Variables);
      _strategy.Add(schart);
      return schart.Id;
    }
    #endregion

    #region public void DeleteSrategy(int id)
    public void DeleteSrategy(int id) {
      foreach(StrategyChart schart in _strategy) {
        if(schart.Id == id) {
          schart.Strategy.OnDestroy();
          _strategy.Remove(schart);
          break;
        }
      }
    }
    #endregion

    #region private void WaitOfBisy()
    private void WaitOfBisy() {
      while(_currentCommand != APICommand.Empty)
        Thread.Sleep(5);
    }
    #endregion

    #region private OnlineRate GetPair(ISymbol symbol)
    private OnlineRate GetPair(ISymbol symbol) {
      APIPairList pairs = this.API.APITrader.Pairs;
      for(int i = 0; i < pairs.Count; i++) {
        if (pairs[i].Symbol.Name == symbol.Name)
          return pairs[i];
      }
      return null;
    }
    #endregion

    public ServerError TradeOpen(ISymbol symbol, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate) {
      this.WaitOfBisy();
      this.API.CreateInitOrder(_account.AccountId, this.GetPair(symbol), lots, Trade.ConvertAPIBuySell(tradeType), stopRate, 0, limitRate, slippage, "");
      this.WaitOfBisy();
      if(_currentData is Trade) {
        return (_currentData as Trade).TradeId;
      }
      return ServerError.TRADE_OPEN_ERROR;
    }

    public ServerError TradeOpen(ISymbol symbol, TradeType tradeType, int lots, int slippage) {
      return this.TradeOpen(symbol, tradeType, lots, slippage, 0, 0);
    }

    public ServerError TradeModify(string tradeId, OrderType orderStopLimit, float newRate) {
      Trade trade = _trades.GetTrade(tradeId) as Trade;
      if (trade == null) return ServerError.TRADE_MODIFY_ERROR;
      this.WaitOfBisy();
      if(orderStopLimit == OrderType.Stop) {
        this.API.ChangeStopLimitTrailOnTrade(trade.ITrade, newRate, 0, trade.LimitRate);
      } else {
        this.API.ChangeStopLimitTrailOnTrade(trade.ITrade, trade.StopRate, 0, newRate);
      }
      this.WaitOfBisy();
      if(_currentError != null) return ServerError.TRADE_MODIFY_ERROR;
      return ServerError.OK;
    }

    public ServerError TradeClose(string tradeId, int lots, int slippage) {
      Trade trade = _trades.GetTrade(tradeId) as Trade;
      if(trade == null) return ServerError.TRADE_CLOSE_ERROR;
      this.WaitOfBisy();
      this.API.CloseTrade(trade.ITrade, lots, slippage);
      this.WaitOfBisy();
      if(_currentError != null) return ServerError.TRADE_CLOSE_ERROR;
      return ServerError.OK;
    }

    public ServerError OrderCreate(ISymbol symbol, CreateEntryOrderType entryOrderType, int lots, float rate, float stopRate, float limitRate) {
      OrderType stoplimittype = OrderType.Limit;
      if (entryOrderType == CreateEntryOrderType.BuyStop || entryOrderType == CreateEntryOrderType.SellStop)
        stoplimittype = OrderType.Stop;

      TradeType bstype = TradeType.Buy;
      if (entryOrderType == CreateEntryOrderType.SellLimit || entryOrderType == CreateEntryOrderType.SellStop)
        bstype = TradeType.Sell;

      this.WaitOfBisy();
      this.API.CreateEntryOrder(stoplimittype, _account.AccountId, this.GetPair(symbol), lots, bstype, rate, stopRate, limitRate, "");
      this.WaitOfBisy();
      if(_currentError != null) return ServerError.ORDER_CREATE_ERROR;
      return ServerError.OK;
    }

    public ServerError OrderCreate(ISymbol symbol, CreateEntryOrderType entryOrderType, int lots, float rate) {
      return this.OrderCreate(symbol, entryOrderType, lots, rate, 0, 0);
    }

    public ServerError OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate) {
      Order order = _orders.GetOrder(orderId) as Order;
      if(order == null) return ServerError.ORDER_MODIFY_ERROR;
      this.WaitOfBisy();
      this.API.ChangeEntryOrder(order.IOrder, newRate, newStopRate, 0, newLimitRate);
      this.WaitOfBisy();
      if(_currentError != null) return ServerError.ORDER_MODIFY_ERROR;
      return ServerError.OK;
    }

    public ServerError OrderDelete(string orderId) {
      Order order = _orders.GetOrder(orderId) as Order;
      if(order == null) return ServerError.ORDER_DELETE_ERROR;
      this.WaitOfBisy();
      this.API.DeleteOrder(order.IOrder);
      this.WaitOfBisy();
      if(_currentError != null) return ServerError.ORDER_DELETE_ERROR;
      return ServerError.OK;
    }
    
    #region public void Comment(string text)
    public void Comment(string text) {

    }
    #endregion

    #region public void API_AccountsChangegEvent(APIAccount account, BrokerMessageType mtype)
    public void API_AccountsChangegEvent(APIAccount account, BrokerMessageType mtype) {
    }
    #endregion

    #region public void API_OrdersChangedEvent(IOrder order, BrokerMessageType mtype)
    public void API_OrdersChangedEvent(IOrder order, BrokerMessageType mtype) {
      this._orders.Update();
      _currentCommand = APICommand.Empty;
    }
    #endregion

    #region public void API_TradesChangedEvent(ITrade trade, BrokerMessageType mtype)
    public void API_TradesChangedEvent(ITrade trade, BrokerMessageType mtype) {
      this._trades.Update();

      try {
        if(_currentCommand == APICommand.CreateTrade && mtype == BrokerMessageType.Add &&
          _currentData != null) {

          string orderid = Convert.ToString(_currentData);
          if(trade.ParentOrderId == orderid) {
            _currentData = _trades.GetTrade(Convert.ToInt32(trade.TradeId));
          }
        }
      } catch { }
      _currentCommand = APICommand.Empty;
    }
    #endregion

    #region public void API_PairsChangedEvent(IOnlineRate onlineRate, BrokerMessageType mtype)
    public void API_PairsChangedEvent(IOnlineRate onlineRate, BrokerMessageType mtype) {
      _lasttime = pair.Time;

      if (_api.GetSymbolUpdateStatus(pair.Symbol) != SymbolUpdateStatus.Update) return;

      foreach(StrategyChart schart in _strategy) {
        if(schart.Symbol.Name == pair.Symbol.Name) {
          schart.Analyzer.CheckedMove();
          (schart.Strategy.Symbol as TestSymbol).SetTick(pair.SellRate, pair.BuyRate, pair.Time);
          schart.Strategy.OnExecute();
        }
      }
    }
    #endregion

    #region public void API_ConnectionStatusChanged(BrokerConnectionStatus status)
    public void API_ConnectionStatusChanged(BrokerConnectionStatus status) {
      _trades.Update();
      _orders.Update();
    }
    #endregion

    #region public void API_OnCommandStarting(APICommand command)
    public void API_OnCommandStarting(APICommand command) {
      _currentCommand = command;
      _currentError = null;
      _currentData = null;
    }
    #endregion

    #region public void API_OnCommandStopping(APICommand command, APIError error)
    public void API_OnCommandStopping(APICommand command, APIError error, object data) {
      
      if(_currentCommand != APICommand.Empty && _currentCommand != command) {
        _currentData = null;
        _currentCommand = APICommand.Empty;
      } else {
        _currentError = error;
        _currentData = data;
      }
    }
    #endregion
  }
}
