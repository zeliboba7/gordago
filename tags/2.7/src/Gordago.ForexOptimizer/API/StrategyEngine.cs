/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Gordago.Analysis;

namespace Gordago.API {

  #region class StrategyId
  class StrategyId {
    private int _id;
    private Gordago.Analysis.Strategy _strategy;

    public StrategyId(int id, Gordago.Analysis.Strategy strategy) {
      _id = id;
      _strategy = strategy;
    }

    #region public int Id
    public int Id {
      get { return this._id; }
    }
    #endregion

    #region public Gordago.Analysis.Strategy Strategy
    public Gordago.Analysis.Strategy Strategy {
      get { return _strategy; }
    }
    #endregion
  }
  #endregion

  class StrategyEngine:IBrokerEvents,  ITrader {

    private List<Gordago.Analysis.Strategy> _list;

    private IOnlineRate _onlineRate = null;
    private AnalyzerManager _analyzerManager;
    private BrokerCommandManager _bcm;

    private IAccountList _accounts;
    private ITradeList _trades;
    private IOrderList _orders;
    private IClosedTradeList _closeTrades;
    private IOnlineRateList _onlineRates;
    private bool _run = false;

    private bool _oneExecuteCommand = false;

    private BrokerCommand _currentCommand;
    private BrokerResult _currentResult;

    public StrategyEngine(BrokerCommandManager bcm) {
      _bcm = bcm;
      _list = new List<Gordago.Analysis.Strategy>();
    }

    #region public BrokerCommandManager BCM
    public BrokerCommandManager BCM {
      get { return this._bcm; }
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

    #region public IClosedTradeList ClosedTrades
    public IClosedTradeList ClosedTrades {
      get { return _closeTrades; }
    }
    #endregion

    #region private int GetIndexStrategy(Gordago.Analysis.Strategy strategy)
    private int GetIndexStrategy(Gordago.Analysis.Strategy strategy) {
      for (int i = 0; i < _list.Count; i++) {
        if (strategy == _list[i])
          return i;
      }
      return -1;
    }
    #endregion

    #region public bool Start(Gordago.Analysis.Strategy strategy)
    public bool Start(Gordago.Analysis.Strategy strategy) {
      if (GetIndexStrategy(strategy) > -1)
        return false;

      if (!strategy.OnLoad())
        return false;

      if (BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
        strategy.SetEngine(this, _analyzerManager, false);
        strategy.OnConnect();
      }

      _list.Add(strategy);

      if (_list.Count == 1 && !_run) {
        Thread th = new Thread(new ThreadStart(this.Proccess));
        th.IsBackground = true;
        th.Start();
      }
      return true;
    }
    #endregion

    #region public void Stop(Gordago.Analysis.Strategy strategy)
    public void Stop(Gordago.Analysis.Strategy strategy) {
      if (this.GetIndexStrategy(strategy) < 0)
        return;

      if (BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
        strategy.SetEngine(this, null, false);
        strategy.OnDisconnect();
      }
      strategy.OnDestroy();
      _list.Remove(strategy);
    }
    #endregion

    private void Proccess() {
      _run = true;
      while (_list.Count > 0) {
        if (GordagoMain.IsCloseProgram)
          break;

        if (_onlineRate != null) {
          if (this.BCM.Busy) {
            _oneExecuteCommand = false;
            IOnlineRate onlineRate = _onlineRate;
            for (int i = 0; i < _list.Count; i++) {
              if (!_oneExecuteCommand) {
                /* если за все время не было исполнение команды, то проверка 
                 * на выполнение следующию стратегию */
                try {
                  _list[i].OnOnlineRateChanged(onlineRate);
                } catch { }
              }
            }
          }
          _onlineRate = null;
        }
        Thread.Sleep(1);
      }
      _run = false;
    }

    #region public void BrokerConnectionStatusChanged(BrokerConnectionStatus status)
    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      if (status == BrokerConnectionStatus.Online) {
        _accounts = BCM.Broker.Accounts;
        _trades = BCM.Broker.Trades;
        _orders = BCM.Broker.Orders;
        _closeTrades = BCM.Broker.ClosedTrades;
        _onlineRates = BCM.Broker.OnlineRates;
        _analyzerManager = new AnalyzerManager(GordagoMain.IndicatorManager, GordagoMain.SymbolEngine, typeof(VirtualAnalyzer));

        for (int i = 0; i < _list.Count; i++) {
          _list[i].SetEngine(this, _analyzerManager, false);
          _list[i].OnConnect();
        }
      } else if (status == BrokerConnectionStatus.Offline) {
        _accounts = null;
        _trades = null;
        _orders = null;
        _closeTrades = null;
        _analyzerManager = null;
        _onlineRates = null;
        for (int i = 0; i < _list.Count; i++) {
          _list[i].SetEngine(null, null, false);
          _list[i].OnDisconnect();
        }
      }
    }
    #endregion

    #region public void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {

    }
    #endregion

    #region public void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {

    }
    #endregion

    #region public void BrokerTradesChanged(BrokerTradesEventArgs be)
    public void BrokerTradesChanged(BrokerTradesEventArgs be) {

    }
    #endregion

    #region public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      if (_onlineRate != null)
        return;
      _onlineRate = be.OnlineRate;
    }
    #endregion

    #region public void BrokerCommandStarting(BrokerCommand command)
    public void BrokerCommandStarting(BrokerCommand command) {
      _oneExecuteCommand = true;
    }
    #endregion

    #region public void BrokerCommandStopping(BrokerCommand command, BrokerResult result)
    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      if (_currentCommand != null) {
        _currentResult = result;
      }
    }
    #endregion

    #region public BrokerResultClosedTrades GetClosedTrades(string accountId, DateTime fromTime, DateTime toTime)
    public BrokerResultClosedTrades GetClosedTrades(string accountId, DateTime fromTime, DateTime toTime) {
      return null;
    }
    #endregion

    #region private void WaitCommand()
    private void WaitCommand() {
      while (_currentResult == null) {
        Thread.Sleep(1);
      }
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment) {
      IOnlineRate onlineRate = this.OnlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.OnlineRateNotFound));
      BrokerCommandTradeOpen cmd = new BrokerCommandTradeOpen(accountId, onlineRate, tradeType, lots, slippage, stopRate, limitRate, comment);
      _currentResult = null;
      _currentCommand = cmd;
      BCM.ExecuteCommand(cmd);
      this.WaitCommand();
      _currentCommand = null;
      BrokerResultTrade result = _currentResult as BrokerResultTrade;
      _currentResult = null;
      return result;
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate) {
      return this.TradeOpen(accountId, symbolName, tradeType, lots, slippage, stopRate, limitRate, "");
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage) {
      return TradeOpen(accountId, symbolName, tradeType, lots, slippage, 0, 0);
    }
    #endregion

    #region public BrokerResultTrade TradeModify(string tradeId, float stopRate, float limitRate)
    public BrokerResultTrade TradeModify(string tradeId, float stopRate, float limitRate) {
      ITrade trade = this.Trades.GetTrade(tradeId);
      if (trade == null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.TradeNotFound));
      BrokerCommandTradeModify cmd = new BrokerCommandTradeModify(trade, stopRate, limitRate);
      _currentCommand = cmd;
      _currentResult = null;
      BCM.ExecuteCommand(cmd);
      this.WaitCommand();
      _currentCommand = null;
      BrokerResultTrade result = _currentResult as BrokerResultTrade;
      _currentResult = null;
      return result;
    }
    #endregion

    #region public BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment)
    public BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment) {
      ITrade trade = this.Trades.GetTrade(tradeId);
      if (trade == null)
        return new BrokerResult(new BrokerError(BrokerErrorType.TradeNotFound));

      BrokerCommandTradeClose cmd = new BrokerCommandTradeClose(trade, lots, slippage, comment);
      _currentCommand = cmd;
      _currentResult = null;
      BCM.ExecuteCommand(cmd);
      this.WaitCommand();
      _currentCommand = null;
      BrokerResult result = _currentResult as BrokerResult;
      _currentResult = null;
      return result;
    }
    #endregion

    #region public BrokerResult TradeClose(string tradeId, int lots, int slippage)
    public BrokerResult TradeClose(string tradeId, int lots, int slippage) {
      return this.TradeClose(tradeId, lots, slippage, "");
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate) {
      return this.OrderCreate(accountId, symbolName, tradeType, lots, rate, stopRate, limitRate, "");
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment) {
      IOnlineRate onlineRate = this.OnlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OrderNotFound));

      OrderType orderType = OrderType.EntryLimit;
      bool error = false;

      if (tradeType == TradeType.Sell) {
        if (rate > onlineRate.SellRate) {
          orderType = OrderType.Limit;
        } else if (rate < onlineRate.SellRate) {
          orderType = OrderType.Stop;
        } else {
          error = true;
        }
      } else {
        if (rate < onlineRate.BuyRate) {
          orderType = OrderType.Limit;
        } else if (rate > onlineRate.BuyRate) {
          orderType = OrderType.Stop;
        } else {
          error = true;
        }
      }

      if (error)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.PendingOrderRateError));

      BrokerCommandEntryOrderCreate cmd = new BrokerCommandEntryOrderCreate(accountId, onlineRate, orderType, tradeType, lots, rate, stopRate, limitRate, comment);
      _currentCommand = cmd;
      _currentResult = null;
      BCM.ExecuteCommand(cmd);
      this.WaitCommand();
      _currentCommand = null;
      BrokerResultOrder result = _currentResult as BrokerResultOrder;
      _currentResult = null;
      return result;
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate) {
      return OrderCreate(accountId, symbolName, tradeType, lots, rate, 0, 0);
    }
    #endregion

    #region public BrokerResultOrder OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate)
    public BrokerResultOrder OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate) {
      IOrder order = this.Orders.GetOrder(orderId);
      if (order == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OrderNotFound));

      BrokerCommandEntryOrderModify cmd = new BrokerCommandEntryOrderModify(order, lots, newRate, newStopRate, newLimitRate);
      _currentCommand = cmd;
      _currentResult = null;
      BCM.ExecuteCommand(cmd);
      this.WaitCommand();
      _currentCommand = null;
      BrokerResultOrder result = _currentResult as BrokerResultOrder;
      _currentResult = null;
      return result;
    }
    #endregion

    #region public BrokerResult OrderDelete(string orderId)
    public BrokerResult OrderDelete(string orderId) {
      return OrderDelete(orderId, "");
    }
    #endregion

    #region public BrokerResult OrderDelete(string orderId, string comment)
    public BrokerResult OrderDelete(string orderId, string comment) {
      IOrder order = this.Orders.GetOrder(orderId);
      if (order == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OrderNotFound));
      BrokerCommandEntryOrderDelete cmd = new BrokerCommandEntryOrderDelete(order, comment);
      _currentCommand = cmd;
      _currentResult = null;
      BCM.ExecuteCommand(cmd);
      this.WaitCommand();
      _currentCommand = null;
      BrokerResult result = _currentResult as BrokerResult;
      _currentResult = null;
      return result;
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get {
        if (_bcm.ConnectionStatus == BrokerConnectionStatus.Online)
          return _bcm.Broker.Time;
        return DateTime.Now; 
      }
    }
    #endregion




  }
}
