/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Gordago.Analysis.Chart;

namespace Gordago.API.VirtualForex {
  
  class VirtualBroker : Broker, IBroker {

    private bool _useAllSymbols = true;

    private DateTime _time;
    private BrokerConnectionStatus _connectionStatus = BrokerConnectionStatus.Offline;

    private AccountList _accounts;
    private OnlineRateList _onlineRates;
    private TradeList _trades;
    private OrderList _orders;
    private ClosedTradeList _closedTrades;

    private int _minStopPoint = 2, _minLimitPoint = 2;
    private int _speed = 1000;
    private bool _pause;
    private int _currentTradeId = 1;
    

    private long _sessionId = 0;

    private VSSettingsPanel _accountSettings;

    /// <summary>
    /// Для тестера
    /// </summary>
    private long _fromTime = -1;

    private bool _isTester = false;

    private Gordago.Analysis.Strategy _strategy;

    

#if DEMO
    private static int _countWorkSecond = 0;
    // private static int _secondWorkMaximum = 5 * 60;
    private static int _secondWorkMaximum = 3 * 60;

    #region public static int SeconWorkMaximum
    public static int SeconWorkMaximum {
      get { return _secondWorkMaximum; }
    }
    #endregion

    #region public static int SecondWork
    public static int SecondWork {
      get { return _countWorkSecond; }
    }
    #endregion

    #region public static bool IsDemoLimit
    public static bool IsDemoLimit {
      get { return _countWorkSecond > SeconWorkMaximum; }
    }
    #endregion
#endif

//    public VirtualBroker(ISymbolList symbols) : base(symbols) {
//#if DEMO
//      _countWorkSecond = 0;
//#endif
//    }

    public VirtualBroker(ISymbolList symbols, params object[] parameters) : base(symbols, parameters) {
#if DEMO
      _countWorkSecond = 0;
#endif

      _accountSettings = (VSSettingsPanel)parameters[0];
      _time = (DateTime)parameters[1];
      _fromTime = Convert.ToInt64(parameters[2]);
      _isTester = (bool)parameters[3];
      _useAllSymbols = (bool)parameters[4];
    }

    ~VirtualBroker() {
      _connectionStatus = BrokerConnectionStatus.Offline;
    }

    #region public bool UseAllSymbols
    public bool UseAllSymbols {
      get { return _useAllSymbols; }
    }
    #endregion

    #region public VSSettingsPanel Settings
    public VSSettingsPanel Settings {
      get { return _accountSettings; }
    }
    #endregion

    #region private bool IsTester
    private bool IsTester {
      get { return _isTester; }
    }
    #endregion

    #region public long FromTime
    public long FromTime {
      get { return _fromTime; }
    }
    #endregion

    #region public Gordago.Analysis.Strategy Strategy
    public Gordago.Analysis.Strategy Strategy {
      get { return _strategy; }
    }
    #endregion

    #region public long SessionId
    public long SessionId {
      get { return this._sessionId; }
    }
    #endregion

    #region public IClosedTradeList ClosedTrades
    public IClosedTradeList ClosedTrades {
      get { return this._closedTrades; }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get { return _time; }
    }
    #endregion

    #region public BrokerConnectionStatus ConnectionStatus
    public BrokerConnectionStatus ConnectionStatus {
      get { return _connectionStatus; }
    }
    #endregion

    #region public IAccountList Accounts
    public IAccountList Accounts {
      get { return _accounts; }
    }
    #endregion

    #region public ITradeList Trades
    public ITradeList Trades {
      get { return _trades; }
    }
    #endregion

    #region public IOrderList Orders
    public IOrderList Orders {
      get { return _orders; }
    }
    #endregion

    #region public IOnlineRateList OnlineRates
    public IOnlineRateList OnlineRates {
      get { return _onlineRates; }
    }
    #endregion

    #region public int Speed
    public int Speed {
      get { return this._speed; }
      set { _speed = value; }
    }
    #endregion

    #region public bool Pause
    public bool Pause {
      get { return this._pause; }
      set { this._pause = value; }
    }
    #endregion

    #region public static DateTime[] GetPeriodHistory(ISymbolList symbols)
    public static DateTime[] GetPeriodHistory(ISymbolList symbols) {
      DateTime dtm1 = new DateTime();
      DateTime dtm2 = new DateTime();
      bool init = false;

      for (int i = 0; i < symbols.Count; i++) {
        ITickList ticks = symbols[i].Ticks;
        if (ticks.Count > 100) {
          if (!init) {
            init = true;
            dtm1 = ticks.TimeFrom;
            dtm2 = ticks.TimeTo;
          } else {
            dtm1 = new DateTime(Math.Min(dtm1.Ticks, ticks.TimeFrom.Ticks));
            dtm2 = new DateTime(Math.Max(dtm2.Ticks, ticks.TimeTo.Ticks));
          }
        }
      }
      return new DateTime[] { dtm1, dtm2};
    }
    #endregion

    #region public void SetStrategy(Gordago.Analysis.Strategy strategy)
    public void SetStrategy(Gordago.Analysis.Strategy strategy) {
      _strategy = strategy;
    }
    #endregion

    #region private void Proccess()
    private void Proccess() {
#if DEMO
      DateTime demoStartTime = DateTime.Now;
#endif

      int movedBarsStep = this.Settings.UseTimeFrame != null ? this.Settings.UseTimeFrame.Second : 0;

      while (_connectionStatus != BrokerConnectionStatus.Offline) {
        bool isSleep = true;
        if (!_pause) {
          int s = 1000 / _speed;
          if (movedBarsStep > 0) {
            s = movedBarsStep;
          }
          isSleep = this.MoveNextSecond(s);
        }
        if (isSleep)
          Thread.Sleep(_speed);

        if (GordagoMain.IsCloseProgram)
          return;
#if DEMO
        long sec = (DateTime.Now.Ticks - demoStartTime.Ticks) / 10000000L;
        if (sec > 60) {
          _countWorkSecond += (int)sec;
          demoStartTime = DateTime.Now;
        }

        if (IsDemoLimit) {
          this.Logoff();
        }
#endif
      }
    }
    #endregion

    #region public bool MoveNextSecond(int second)
    public bool MoveNextSecond(int second) {
      _time = _time.AddSeconds(second);

      bool isok = true;
      if (_time.DayOfWeek == DayOfWeek.Saturday) {
        isok = false;
      } else if (_time.DayOfWeek == DayOfWeek.Sunday) {
        DateTime t = new DateTime(_time.Year, _time.Month, _time.Day, 18, 30, 0, 0);
        if (_time < t)
          isok = false;
      } else if (_time.DayOfWeek == DayOfWeek.Friday) {
        DateTime t = new DateTime(_time.Year, _time.Month, _time.Day, 21, 30, 0, 0);

        if (_time > t)
          isok = false;
      }

      if (!isok)
        return false;

      for (int i = 0; i < this.Symbols.Count; i++) {
        ViTickList vtlst = (Symbols[i].Ticks as ViTickList);
        if (vtlst != null)
          vtlst.MoveNext(this, _time);
      }
      return true;
    }
    #endregion

    #region public void IncrementSession()
    public void IncrementSession() {
      _sessionId++;
    }
    #endregion

    #region public bool OnlineRateChanged(OnlineRate onlineRate)
    public bool OnlineRateChanged(OnlineRate onlineRate) {

      float lastSellRate = onlineRate.LastSellRate;
      float lastBuyRate = onlineRate.LastBuyRate;

      float sellRate = onlineRate.SellRate;
      float buyRate = onlineRate.BuyRate;

      this.IncrementSession();

      /* Проверка на Margin Call*/
      for (int i = 0; i < this.Accounts.Count; i++) {
        Account account = this.Accounts[i] as Account;
        float balance = account.Balance;
      }

      #region Проверка на срабатывание ордера
      bool isUp = lastSellRate < sellRate;

      OrderList orders = isUp ? onlineRate.OrdersUp : onlineRate.OrdersDown;

      for (int i = 0; i < orders.Count; i++) {
        Order order = orders[i] as Order;
        if (order.CheckOrderDoExecute(isUp)) {
          if (order.OrderType == OrderType.Limit || order.OrderType == OrderType.Stop) {
            Trade trade = this._trades.GetTrade(order.TradeId) as Trade;
            this.TradeCloseMethod(trade.TradeId, trade.Lots, 0, "");
          } else {
            this.TradeOpenMethod(order);
          }
        }
      }
      #endregion

      return true;
    }
    #endregion

    #region public void OnlineRateChangedEvent(OnlineRate onlineRate)
    public void OnlineRateChangedEvent(OnlineRate onlineRate) {
      OnOnlineRatesChanged(new BrokerOnlineRatesEventArgs(onlineRate, BrokerMessageType.Update));
    }
    #endregion

    #region protected override void OnDispose()
    protected override void OnDispose() {
      base.OnDispose();
      _connectionStatus = BrokerConnectionStatus.Offline;
    }
    #endregion

    #region protected override void OnTradesChanged(BrokerTradesEventArgs be)
    protected override void OnTradesChanged(BrokerTradesEventArgs be) {
      this.IncrementSession();
      base.OnTradesChanged(be);
      this.OnAccountsChanged(new BrokerAccountsEventArgs(be.Trade.Account, BrokerMessageType.Update));
    }
    #endregion

    #region protected override void OnAccountsChanged(BrokerAccountsEventArgs be)
    protected override void OnAccountsChanged(BrokerAccountsEventArgs be) {
      this.IncrementSession();
      base.OnAccountsChanged(be);
    }
    #endregion

    #region private void SetConnectionStatus(BrokerConnectionStatus status)
    private void SetConnectionStatus(BrokerConnectionStatus status) {
      _connectionStatus = status;
      this.OnConnectionStatusChanged();
    }
    #endregion

    #region public void AddSymbol(ISymbol symbol)
    public void AddSymbol(ISymbol symbol) {
      if (!_onlineRates.AddSymbol(symbol))
        return;
      SymbolsProperty.Load(_onlineRates);
    }
    #endregion

    #region private long GetRoundTime(TimeFrame tf, long time)
    private long GetRoundTime(TimeFrame tf, long time) {
      int second = tf.Second;
      long nt = (time / 10000000L) / second;
      return (nt * second) * 10000000L;
    }
    #endregion

    #region public BrokerResult Logon(string userName, string password, BrokerProxyInfo proxy, bool demo)
    public BrokerResult Logon(string userName, string password, BrokerProxyInfo proxy, bool demo) {

      this.SetConnectionStatus(BrokerConnectionStatus.LoadingData);

      long minTime = DateTime.Now.Ticks;

      for (int i = 0; i < this.Symbols.Count; i++) {
        ISymbol symbol = this.Symbols[i];
        if (symbol.Ticks.Count > 0) {
          minTime = Math.Min(symbol.Ticks.TimeFrom.Ticks, minTime);
        }
      }

      _time = new DateTime(Math.Max(minTime, _time.Ticks));

      if (_accountSettings.UseTimeFrame != null) {
        _time = new DateTime(this.GetRoundTime(_accountSettings.UseTimeFrame, _time.Ticks));
      }

      _onlineRates = new OnlineRateList(this);

      if (this.UseAllSymbols) {
        for (int i = 0; i < this.Symbols.Count; i++)
          _onlineRates.AddSymbol(this.Symbols[i]);
      } else {
        foreach (Form frm in GordagoMain.MainForm.MdiChildren) {
          if (frm is ChartForm) {
            _onlineRates.AddSymbol((frm as ChartForm).Symbol);
          }
        }
      }

      SymbolsProperty.Load(_onlineRates);

      _accounts = new AccountList(this, 1);
      _trades = new TradeList();
      _orders = new OrderList();
      _closedTrades = new ClosedTradeList();

      this.SetConnectionStatus(BrokerConnectionStatus.Online);

      for (int i = 0; i < _accounts.Count; i++)
        this.OnAccountsChanged(new BrokerAccountsEventArgs(_accounts[i], BrokerMessageType.Update));

      for (int i = 0; i < _onlineRates.Count; i++)
        this.OnlineRateChanged(_onlineRates[i] as OnlineRate);

      if (!this.IsTester) {
        Thread th = new Thread(new ThreadStart(this.Proccess));
        th.IsBackground = true;
        th.Priority = ThreadPriority.Lowest;
        th.Name = "VirtualBroker Thread";
        th.Start();
      }
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResult Logoff()
    public BrokerResult Logoff() {
      for (int i = 0; i < this.Symbols.Count; i++) {
        if (Symbols[i].Ticks is ViTickList) {
          Symbols[i].Ticks = (Symbols[i].Ticks as ViTickList).FiTicks;
        }
      }

      this.SetConnectionStatus(BrokerConnectionStatus.Offline);
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment) {
      return this.TradeOpenMethod(accountId, symbolName, tradeType, lots, slippage, stopRate, limitRate, null, comment);
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate) {
      return this.TradeOpenMethod(accountId, symbolName, tradeType, lots, slippage, stopRate, limitRate, null, "");
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage) {
      return this.TradeOpen(accountId, symbolName, tradeType, lots, slippage, 0, 0);
    }
    #endregion

    #region private BrokerResultTrade TradeOpenMethod(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, Order fromOrder, string comment)
    private BrokerResultTrade TradeOpenMethod(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, Order fromOrder, string comment) {
      Account account = (Account)_accounts.GetAccount(accountId);
      if (account == null) {
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.AccountNotFound));
      }

      OnlineRate onlineRate = (OnlineRate)_onlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null) {
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.OnlineRateNotFound));
      }

      float rate = tradeType == TradeType.Sell ? onlineRate.SellRate : onlineRate.BuyRate;

      bool useStop = false, useLimit = false;

      if (!float.IsNaN(stopRate) && stopRate > 0) {
        if (!CheckStopLimit(onlineRate, tradeType, OrderType.Stop, rate, stopRate)) 
          return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.StopOrderRateError));
        useStop = true;
      }

      if (!float.IsNaN(limitRate) && limitRate > 0) {
        if (!CheckStopLimit(onlineRate, tradeType, OrderType.Limit, rate, limitRate))
          return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.LimitOrderRateError));
        useLimit = true;
      }

      string tradeId = Convert.ToString(_currentTradeId);
      _currentTradeId++;
      Trade trade = new Trade(this, account, onlineRate, tradeId, tradeType, lots, this.Time, comment);

      if (useStop)
        CreateStopLimitOnTrade(trade, OrderType.Stop, stopRate);

      if (useLimit)
        CreateStopLimitOnTrade(trade, OrderType.Limit, limitRate);

      if (fromOrder != null) 
        trade.SetParentOrderId(fromOrder.OrderId);

      this.TradeAddInTable(trade);
      return new BrokerResultTrade(trade, null);
    }
    #endregion

    #region private void TradeOpenMethod(Order order)
    /// <summary>
    /// Открытие позиции по ордеру
    /// </summary>
    /// <param name="order"></param>
    private void TradeOpenMethod(Order order) {
      Account account = order.Account as Account;
      OnlineRate onlineRate = order.OnlineRate as OnlineRate;

      string tradeId = Convert.ToString(_currentTradeId);
      _currentTradeId++;


      string orderText = "#order#|" + order.OrderId + "|" + ((long)(order.Time.Ticks / 10000000L)).ToString();

      string comment = order.Comment;
      comment = orderText + "\n" + comment;
      Trade trade = new Trade(this, account, onlineRate, tradeId, order.TradeType, order.Lots, this.Time, comment);

      trade.SetOpenRate(order.Rate);
      trade.SetParentOrderId(order.OrderId);

      Order stopOrder = order.StopOrder as Order;
      Order limitOrder = order.LimitOrder as Order;

      trade.SetStopOrder(stopOrder);
      trade.SetLimitOrder(limitOrder);
      order.SetStopOrder(null);
      order.SetLimitOrder(null);

      if (stopOrder != null) {
        stopOrder.SetTradeId(trade.TradeId);
        if (trade.TradeType == TradeType.Sell) {
          onlineRate.OrdersUp.Add(stopOrder);
        } else {
          onlineRate.OrdersDown.Add(stopOrder);
        }
      } 
      if (limitOrder != null){
        limitOrder.SetTradeId(trade.TradeId);
        if (trade.TradeType == TradeType.Sell) {
          onlineRate.OrdersDown.Add(limitOrder);
        } else {
          onlineRate.OrdersUp.Add(limitOrder);
        }
      }
      this.OrderRemoveMethod(order.OrderId, "");
      this.TradeAddInTable(trade);
    }
    #endregion

    #region private void TradeAddInTable(Trade trade)
    private void TradeAddInTable(Trade trade) {
      _trades.Add(trade);
      (trade.Account.Trades as TradeList).Add(trade);
      this.OnTradesChanged(new BrokerTradesEventArgs(trade, BrokerMessageType.Add));
    }
    #endregion

    #region private bool CheckStopLimit(OnlineRate onlineRate, TradeType tradeType, OrderType orderType, float rate, float checkRate)
    private bool CheckStopLimit(IOnlineRate onlineRate, TradeType tradeType, OrderType orderType, float rate, float checkRate) {

      if (float.IsNaN(checkRate) || checkRate == 0)
        return true;

      float bid = onlineRate.SellRate, ask = onlineRate.BuyRate;
      float spread = ask - bid;

      float minpoint = (ask - bid) + onlineRate.Symbol.Point;

      if (orderType == OrderType.Stop) {

        float minstop = _minStopPoint * onlineRate.Symbol.Point;
        if (tradeType == TradeType.Sell) {
          if (checkRate - spread - minstop - rate < 0)
            return false;
        } else {
          if (rate - minstop - checkRate < 0)
            return false;
        }
        return true;
      } else {

        float minlimit = _minLimitPoint * onlineRate.Symbol.Point;
        if (tradeType == TradeType.Sell) {
          if (rate - spread - minlimit - checkRate < 0)
            return false;
        } else {
          if (checkRate - minlimit - rate < 0)
            return false;
        }
        return true;
      }
    }
    #endregion

    #region public Order OrderRemoveMethod(string orderId, string comment)
    public Order OrderRemoveMethod(string orderId, string comment) {
      Order order = (Order)this.Orders.GetOrder(orderId);
      (this.Orders as OrderList).Remove(order);
      (order.Account.Orders as OrderList).Remove(order);
      (order.OnlineRate as OnlineRate).OrdersUp.Remove(order);
      (order.OnlineRate as OnlineRate).OrdersDown.Remove(order);

      if (order.StopOrder != null) {
        this.OrderRemoveMethod(order.StopOrder.OrderId, "");
      }
      if (order.LimitOrder != null) {
        this.OrderRemoveMethod(order.LimitOrder.OrderId, "");
      }
      this.OnOrdersChanged(new BrokerOrdersEventArgs(order, BrokerMessageType.Delete));
      return order;
    }
    #endregion

    #region public Order OrderCreateMethod(Account account, OnlineRate onlineRate, TradeType tradeType, OrderType orderType, int lots, float rate, string comment)
    public Order OrderCreateMethod(Account account, OnlineRate onlineRate, TradeType tradeType, OrderType orderType, int lots, float rate, string comment) {
      string orderId = Convert.ToString(_currentTradeId);
      _currentTradeId++;
      Order order = new Order(account, onlineRate, orderId, tradeType, orderType, lots, rate, this.Time, comment);
      _orders.Add(order);
      (account.Orders as OrderList).Add(order);
      return order;
    }
    #endregion

    #region private Order CreateStopLimitOnTrade(Trade trade, OrderType orderType, float rate)
    private Order CreateStopLimitOnTrade(Trade trade, OrderType orderType, float rate) {
      TradeType closeOrderTradeType = trade.TradeType == TradeType.Sell ? TradeType.Buy : TradeType.Sell;
      Order order = this.OrderCreateMethod((Account)trade.Account, (OnlineRate)trade.OnlineRate, closeOrderTradeType, orderType, trade.Lots, rate, "");
      order.SetTradeId(trade.TradeId);

      OnlineRate onlineRate = trade.OnlineRate as OnlineRate;

      if (orderType == OrderType.Stop) {
        trade.SetStopOrder(order);
        if (trade.TradeType == TradeType.Sell) {
          onlineRate.OrdersUp.Add(order);
        } else {
          onlineRate.OrdersDown.Add(order);
        }
      } else {
        trade.SetLimitOrder(order);
        if (trade.TradeType == TradeType.Sell) {
          onlineRate.OrdersDown.Add(order);
        } else {
          onlineRate.OrdersUp.Add(order);
        }
      }
      return order;
    }
    #endregion

    #region public BrokerResultTrade TradeModify(string tradeId, float stopRate, float limitRate)
    public BrokerResultTrade TradeModify(string tradeId, float stopRate, float limitRate) {
      Trade trade = this._trades.GetTrade(tradeId) as Trade;
      if (trade == null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.TradeNotFound));

      BrokerResultTrade result = this.TradeModifyMethod(tradeId, OrderType.Stop, stopRate);
      if (result.Error != null)
        return result;

      this.OnTradesChanged(new BrokerTradesEventArgs(trade, BrokerMessageType.Update));

      result = this.TradeModifyMethod(tradeId, OrderType.Limit, limitRate);
      if (result.Error != null)
        return result;

      this.OnTradesChanged(new BrokerTradesEventArgs(trade, BrokerMessageType.Update));
      return new BrokerResultTrade(trade, null);
    }
    #endregion

    #region private BrokerResultTrade TradeModifyMethod(string tradeId, OrderType orderStopLimit, float newRate)
    private BrokerResultTrade TradeModifyMethod(string tradeId, OrderType orderStopLimit, float newRate) {
      
      Trade trade = this._trades.GetTrade(tradeId) as Trade;
      if (trade == null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.TradeNotFound));


      newRate = newRate > 0 ? newRate : 0;
      Order order = (orderStopLimit == OrderType.Stop ? trade.StopOrder : trade.LimitOrder) as Order;

      float rate = trade.TradeType == TradeType.Sell ? trade.OnlineRate.SellRate : trade.OnlineRate.BuyRate;

      if (order != null && newRate == 0) { /* delete order */
        if (orderStopLimit == OrderType.Stop) {
          trade.SetStopOrder(null);
        } else {
          trade.SetLimitOrder(null);
        }
        this.OrderRemoveMethod(order.OrderId, "");
      } if (order == null && newRate > 0) { /* Create order */
        if (orderStopLimit == OrderType.Stop) {
          if (!CheckStopLimit(trade.OnlineRate, trade.TradeType, OrderType.Stop, rate, newRate))
            return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.StopOrderRateError));

          CreateStopLimitOnTrade(trade, OrderType.Stop, newRate);
        } else {
          if (!CheckStopLimit(trade.OnlineRate, trade.TradeType, OrderType.Limit, rate, newRate))
            return new BrokerResultTrade(null, new BrokerError( BrokerErrorType.LimitOrderRateError));
          CreateStopLimitOnTrade(trade, OrderType.Limit, newRate);
        }
      } else if (order != null && newRate > 0) { /* Modify order */
        if (orderStopLimit == OrderType.Stop) {
          if (!CheckStopLimit(trade.OnlineRate, trade.TradeType, OrderType.Stop, rate, newRate))
            return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.StopOrderRateError));
        } else {
          if (!CheckStopLimit(trade.OnlineRate, trade.TradeType, OrderType.Limit, rate, newRate))
            return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.LimitOrderRateError));
        }
        order.SetRate(newRate);
      }
      return new BrokerResultTrade(trade, null);
    }
    #endregion

    #region public BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment)
    public BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment) {
      return TradeCloseMethod(tradeId, lots, slippage, comment);
    }
    #endregion

    #region public BrokerResult TradeClose(string tradeId, int lots, int slippage)
    public BrokerResult TradeClose(string tradeId, int lots, int slippage) {
      return TradeCloseMethod(tradeId, lots, slippage, "");
    }
    #endregion

    #region private BrokerResult TradeCloseMethod(string tradeId, int lots, int slippage, string comment)
    private BrokerResult TradeCloseMethod(string tradeId, int lots, int slippage, string comment) {

      if (lots == 0)
        return new BrokerResult(new BrokerError(BrokerErrorType.CountLotIsZero));

      Trade trade = this._trades.GetTrade(tradeId) as Trade;
      if (trade == null)
        return new BrokerResult(new BrokerError(BrokerErrorType.TradeNotFound));

      OnlineRate onlineRate = trade.OnlineRate as OnlineRate;
      
      float rate = trade.TradeType == TradeType.Sell ? onlineRate.BuyRate : onlineRate.SellRate;

      Trade ctrade = null;

      if (lots < trade.Lots) {
        trade.SubLots(lots);
        if (trade.StopOrder != null)
          (trade.StopOrder as Order).SetLots(trade.Lots);
        if (trade.LimitOrder != null)
          (trade.LimitOrder as Order).SetLots(trade.Lots);
        ctrade = new Trade(this, (Account)trade.Account, (OnlineRate)trade.OnlineRate, trade.TradeId, trade.TradeType, lots, trade.OpenTime, trade.Comment);
      } else {
        if (lots > trade.Lots) lots = trade.Lots;
        ctrade = trade;

        _trades.Remove(trade);
        (trade.Account.Trades as TradeList).Remove(trade);

        if (trade.StopOrder != null)
          this.OrderRemoveMethod(trade.StopOrder.OrderId, "");

        if (trade.LimitOrder != null)
          this.OrderRemoveMethod(trade.LimitOrder.OrderId, "");
      }

      ctrade.Close(this.Time);
      ClosedTrade closedTrade = new ClosedTrade(ctrade, comment);
      _closedTrades.Add(closedTrade);
      (ctrade.Account as Account).CloseTrade(closedTrade);
      this.IncrementSession();
      this.OnTradesChanged(new BrokerTradesEventArgs(ctrade, BrokerMessageType.Delete));
      this.OnAccountsChanged(new BrokerAccountsEventArgs(trade.Account, BrokerMessageType.Update));
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment) {
      BrokerResultOrder result = this.OrderCreateMethod(accountId, symbolName, tradeType, lots, rate, stopRate, limitRate, comment);
      return result;
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate) {
      return this.OrderCreate(accountId, symbolName, tradeType, lots, rate, stopRate, limitRate, "");
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate) {
      return OrderCreate(accountId, symbolName, tradeType, lots, rate, 0, 0);
    }
    #endregion

    #region private BrokerResultOrder OrderCreateMethod(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment)
    private BrokerResultOrder OrderCreateMethod(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment) {

      Account account = (Account)_accounts.GetAccount(accountId);
      if (account == null) {
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.AccountNotFound));
      }

      OnlineRate onlineRate = (OnlineRate)_onlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null) {
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OnlineRateNotFound));
      }
      bool useStop = false, useLimit = false;

      if (!float.IsNaN(stopRate) && stopRate > 0) {
        if (!CheckStopLimit(onlineRate, tradeType, OrderType.Stop, rate, stopRate))
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.StopOrderRateError));
        useStop = true;
      }

      if (!float.IsNaN(limitRate) && limitRate > 0) {
        if (!CheckStopLimit(onlineRate, tradeType, OrderType.Limit, rate, limitRate))
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.LimitOrderRateError));
        useLimit = true;
      }

      OrderType orderType = OrderType.Stop;

      bool isUp = false;
      if (tradeType == TradeType.Sell) {
        if (rate > onlineRate.SellRate) {
          isUp = true;
          orderType = OrderType.EntryLimit;
        } else if (rate < onlineRate.SellRate) {
          isUp = false;
          orderType = OrderType.EntryStop;
        } else {
          return new BrokerResultOrder(null, new BrokerError( BrokerErrorType.PendingOrderRateError));
        }
      } else {
        if (rate > onlineRate.BuyRate) {
          isUp = true;
          orderType = OrderType.EntryStop;
        } else if (rate < onlineRate.BuyRate) {
          isUp = false;
          orderType = OrderType.EntryLimit;
        } else {
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.PendingOrderRateError));
        }
      }

      Order order = this.OrderCreateMethod(account, onlineRate, tradeType, orderType, lots, rate, comment);
      if (useStop)
        this.OrderModifyStopLimit(order, OrderType.Stop, stopRate);
      if (useLimit)
        this.OrderModifyStopLimit(order, OrderType.Limit, limitRate);

      if (isUp)
        onlineRate.OrdersUp.Add(order);
      else
        onlineRate.OrdersDown.Add(order);

      this.OnOrdersChanged(new BrokerOrdersEventArgs(order, BrokerMessageType.Add));
      return new BrokerResultOrder(order, null);
    }
    #endregion

    #region public BrokerResultOrder OrderModify(string orderId, int lots, float rate, float stopRate, float limitRate)
    public BrokerResultOrder OrderModify(string orderId, int lots, float rate, float stopRate, float limitRate) {

      Order order = this.Orders.GetOrder(orderId) as Order;
      if (order == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OrderNotFound));

      if (lots < 1)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.CountLotIsZero));
      if (order.Lots != lots) {
        order.SetLots(lots);
        this.OnOrdersChanged(new BrokerOrdersEventArgs(order, BrokerMessageType.Update));
      }

      if (order.Rate != rate) {
        order.SetRate(rate);
        this.OnOrdersChanged(new BrokerOrdersEventArgs(order, BrokerMessageType.Update));
      }

      IOnlineRate onlineRate = order.OnlineRate;

      if (!float.IsNaN(stopRate) && stopRate > 0) {
        if (!CheckStopLimit(onlineRate, order.TradeType, OrderType.Stop, rate, stopRate))
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.StopOrderRateError));
      }

      if (!float.IsNaN(limitRate) && limitRate > 0) {
        if (!CheckStopLimit(onlineRate, order.TradeType, OrderType.Limit, rate, limitRate))
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.LimitOrderRateError));
      }

      this.OrderModifyStopLimit(order, OrderType.Stop, stopRate);
      this.OrderModifyStopLimit(order, OrderType.Limit, limitRate);

       this.OnOrdersChanged(new BrokerOrdersEventArgs(order, BrokerMessageType.Update));
      return new BrokerResultOrder(order, null);
    }
    #endregion

    #region private void OrderModifyStopLimit(Order entryOrder, OrderType orderType, float rate)
    /// <summary>
    /// Модификация(создание, удаление) стоп-лимит ордера на отложенном ордере.
    /// </summary>
    private void OrderModifyStopLimit(Order entryOrder, OrderType orderType, float rate) {
      TradeType closeOrderTradeType = entryOrder.TradeType == TradeType.Sell ? TradeType.Buy : TradeType.Sell;
      rate = rate > 0 ? rate : 0;

      Order curorder = orderType == OrderType.Stop ? (Order)entryOrder.StopOrder : (Order)entryOrder.LimitOrder;

      if (curorder == null && rate > 0) { // create
        Order order = this.OrderCreateMethod((Account)entryOrder.Account, (OnlineRate)entryOrder.OnlineRate, closeOrderTradeType, orderType, entryOrder.Lots, rate, "");

        if (orderType == OrderType.Stop)
          entryOrder.SetStopOrder(order);
        else
          entryOrder.SetLimitOrder(order);

      } else if (curorder != null && rate > 0) { // modify
        curorder.SetRate(rate);
      } else if (curorder != null && rate == 0) { // delete

        if (orderType == OrderType.Stop)
          entryOrder.SetStopOrder(null);
        else
          entryOrder.SetLimitOrder(null);
        this.OrderRemoveMethod(curorder.OrderId, "");
      }
    }
    #endregion

    #region public BrokerResult OrderDelete(string orderId)
    public BrokerResult OrderDelete(string orderId) {
      return OrderDelete(orderId, "");
    }
    #endregion

    #region public BrokerResult OrderDelete(string orderId, string comment)
    public BrokerResult OrderDelete(string orderId, string comment) {
      Order order = this.OrderRemoveMethod(orderId, comment);
      if (order == null)
        return new BrokerResult(new BrokerError(BrokerErrorType.OrderNotFound));
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResultTickHistory GetTickHistory(IOnlineRate onlineRate, DateTime time1, DateTime time2)
    public BrokerResultTickHistory GetTickHistory(IOnlineRate onlineRate, DateTime time1, DateTime time2) {
      return new BrokerResultTickHistory(new Tick[0], null);
    }
    #endregion

    #region public BrokerResultClosedTrades GetClosedTrades(string accountId, DateTime fromTime, DateTime toTime)
    public BrokerResultClosedTrades GetClosedTrades(string accountId, DateTime fromTime, DateTime toTime) {
      List<IClosedTrade> trades = new List<IClosedTrade>();
      for (int i = 0; i < _closedTrades.Count; i++) {
        if (_closedTrades[i].AccountId == accountId)
          trades.Add((ClosedTrade)_closedTrades[i]);
      }
      return new BrokerResultClosedTrades(trades.ToArray(), null);
    }
    #endregion


  }
}
