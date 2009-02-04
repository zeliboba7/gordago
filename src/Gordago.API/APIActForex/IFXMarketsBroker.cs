using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;
using Gordago;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace IFXMarkets {
  public class IFXMarketsBroker : Broker, IBroker {

    public const int HourGMT = 4;
    private const string FX_SCHEMA_DEMO = "demo.16";
    private const string FX_SCHEMA_REAL = "deal.16";

    private FxTraderApi _fxapi;

    private AccountList _accounts;
    private OnlineRateList _onlineRates;
    private OrderList _orders;
    private TradeList _trades;
    private ClosedTradeList _closedTrades;

    private bool _disposing = false;

    private string _workDir;
    private string _libFileName = "", _regCommand, _unRegCommand;

    private int _minStopPoint = 3, _minLimitPoint = 3;

    private BrokerConnectionStatus _connectionStatus = BrokerConnectionStatus.Offline;

    private int _sessionId = -10000000;

    private string _closeOrderId = "", _initOrderId = "";
    private int _slipPage = 0;
    private FxOrder _rejectCloseOrder = null, _rejectInitOrder = null;

    #region private long _timeDifference - Разность времени сервера с местным временем
    /// <summary>
    /// Разность времени сервера с местным временем
    /// </summary>
    private long _timeDifference = 0;
    #endregion

    #region private DateTime _lastGetServerTime - Время последнего запроса времени с сервера
    /// <summary>
    /// Время последнего запроса времени с сервера
    /// </summary>
    private DateTime _lastGetServerTime = DateTime.Now.AddHours(-1);
    #endregion

    #region private bool _busy - Свободный - нет команды на выполнение в данный момент
    /// <summary>
    /// Свободный - нет команды на выполнение в данный момент
    /// </summary>
    private bool _busy = true;
    #endregion

    #region public IFXMarketsBroker(ISymbolList symbols, params object[] parameters):base(symbols, parameters)
    public IFXMarketsBroker(ISymbolList symbols, params object[] parameters)
      : base(symbols, parameters) {

      _workDir = System.Windows.Forms.Application.StartupPath + "\\brokers";
      string filenewapi = _workDir + "\\newapi.txt";
      _libFileName = _workDir + "\\IFXMarketsLib.dll";
      _regCommand = string.Format("regsvr32.exe /s \"{0}\"", _libFileName);
      _unRegCommand = string.Format("regsvr32.exe /s /u \"{0}\"", _libFileName);

      if (System.IO.File.Exists(filenewapi))
        UnRegisterLibrary(_workDir);

      try {
        _fxapi = new FxTraderApi();
        string mewstr = _fxapi.ProxyServer;
        System.Diagnostics.Debug.WriteLine(_fxapi.Version);
      } catch {
        string file = _workDir + "\\tmpreg.bat";
        if (System.IO.File.Exists(file))
          System.IO.File.Delete(file);

        TextWriter tw = System.IO.File.CreateText(file);
        tw.WriteLine(_regCommand);
        tw.Flush();
        tw.Close();

        System.Diagnostics.Process.Start(file);
        System.Threading.Thread.Sleep(5000);
        if (System.IO.File.Exists(file))
          System.IO.File.Delete(file);
        _fxapi = new FxTraderApi();
      }
      _fxapi.OnAccountMessage += new IFxTraderApiEvents_OnAccountMessageEventHandler(this.FxAPI_OnAccountMessage);
      _fxapi.OnError += new IFxTraderApiEvents_OnErrorEventHandler(this.FxAPI_OnError);
      _fxapi.OnMarginCallMessage += new IFxTraderApiEvents_OnMarginCallMessageEventHandler(this.FxAPI_OnMarginCallMessage);
      _fxapi.OnOrderMessage += new IFxTraderApiEvents_OnOrderMessageEventHandler(this.FxAPI_OnOrderMessage);
      _fxapi.OnPairMessage += new IFxTraderApiEvents_OnPairMessageEventHandler(this.FxAPI_OnPairMessage);
      _fxapi.OnRulesChange += new IFxTraderApiEvents_OnRulesChangeEventHandler(this.FxAPI_OnRulesChange);
      _fxapi.OnStatusChange += new IFxTraderApiEvents_OnStatusChangeEventHandler(this.FxAPI_OnStatusChange);
      _fxapi.OnTextMessage += new IFxTraderApiEvents_OnTextMessageEventHandler(this.FxAPI_OnTextMessage);
      _fxapi.OnTradeMessage += new IFxTraderApiEvents_OnTradeMessageEventHandler(this.FxAPI_OnTradeMessage);
#if DEBUG
       //_fxapi.SetDebugLog(Application.StartupPath + "\\brokers\\IFXDebugLog.ini");
#endif
    }
    #endregion

    #region ~IFXMarkets()
    ~IFXMarketsBroker() {
      _connectionStatus = BrokerConnectionStatus.Offline;
      _disposing = true;
    }
    #endregion

    #region public int SessionId
    public int SessionId {
      get { return this._sessionId; }
    }
    #endregion

    #region public string WorkDir
    public string WorkDir {
      get { return _workDir; }
    }
    #endregion

    #region public FxTraderApi FxTraderApi
    public FxTraderApi FxTraderApi {
      get { return this._fxapi; }
    }
    #endregion

    #region public DateTime Time
    /// <summary>
    /// Время севера.
    /// Каждые 5 минут необходимо синхронизировать время с сервером. 
    /// </summary>
    public DateTime Time {
      get {

        if (_busy) {
          if ((DateTime.Now.Ticks - _lastGetServerTime.Ticks) / 10000000L * 60 > 5) {
            _lastGetServerTime = DateTime.Now;
            this.SetTimeDifference();
          }
        }
        return this.GetGmtTime(new DateTime(DateTime.Now.Ticks - _timeDifference));
      }
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
      get { return _closedTrades; }
    }
    #endregion

    #region public BrokerConnectionStatus ConnectionStatus
    public BrokerConnectionStatus ConnectionStatus {
      get { return this._connectionStatus; }
    }
    #endregion

    #region public void UnRegisterLibrary(string workdir)
    public void UnRegisterLibrary(string workdir) {
      string file = workdir + "\\tmpunreg.bat";
      if (System.IO.File.Exists(file))
        System.IO.File.Delete(file);

      System.IO.TextWriter tw = System.IO.File.CreateText(file);
      tw.WriteLine(_unRegCommand);
      tw.Flush();
      tw.Close();

      System.Diagnostics.Process.Start(file);
      System.Threading.Thread.Sleep(3000);
      if (System.IO.File.Exists(file))
        System.IO.File.Delete(file);

      string filenewapi = workdir + "\\newapi.txt";

      if (System.IO.File.Exists(filenewapi))
        System.IO.File.Delete(filenewapi);
    }
    #endregion

    #region private DateTime SetTimeDifference()
    /// <summary>
    /// Установка разности времени с времением сервера
    /// </summary>
    /// <returns>Время сервера</returns>
    private DateTime SetTimeDifference() {
      if (_connectionStatus != BrokerConnectionStatus.Online)
        return new DateTime();
      DateTime time;
      FxError error = _fxapi.GetServerTime(out time);
      if (error != null)
        return new DateTime();

      System.Diagnostics.Debug.WriteLine("Время сервера: " + time);
      _timeDifference = DateTime.Now.Ticks - time.Ticks;
      return time;
    }
    #endregion

    #region static private BrokerMessageType GetMType(FxMessageType mtype)
    private static BrokerMessageType GetMType(FxMessageType mtype) {
      switch (mtype) {
        case FxMessageType.mt_Add:
          return BrokerMessageType.Add;
        case FxMessageType.mt_Delete:
          return BrokerMessageType.Delete;
        default:
          return BrokerMessageType.Update;
      }
    }
    #endregion

    #region public static TradeType GetTradeType(FxBuySell buySell)
    public static TradeType GetTradeType(FxBuySell buySell) {
      if (buySell == FxBuySell.bs_Buy)
        return TradeType.Buy;
      return TradeType.Sell;
    }
    #endregion

    #region private void FxAPI_OnStatusChange(FxConnectionStatus status)
    private void FxAPI_OnStatusChange(FxConnectionStatus fxstatus) {
      if (_disposing) return;

      switch (fxstatus) {
        case FxConnectionStatus.cs_Online:
          _connectionStatus = BrokerConnectionStatus.Online;
          _onlineRates = new OnlineRateList(this);
          _accounts = new AccountList(this);
          _orders = new OrderList(this);
          _orders.Update();
          _trades = new TradeList(this);
          _closedTrades = new ClosedTradeList(this);
          break;
        case FxConnectionStatus.cs_Offline:
          _connectionStatus = BrokerConnectionStatus.Offline;
          _onlineRates = null;
          _accounts = null;
          _orders = null;
          _trades = null;
          _closedTrades = null;
          break;
        case FxConnectionStatus.cs_LoadingData:
          _connectionStatus = BrokerConnectionStatus.LoadingData;
          break;
        case FxConnectionStatus.cs_WaitingForConnection:
          _connectionStatus = BrokerConnectionStatus.WaitingForConnection;
          break;
      }
      _sessionId = -1000000;
      base.OnConnectionStatusChanged();
    }
    #endregion

    #region private void FxAPI_OnAccountMessage(FxAccount fxaccount, FxMessageType fxmtype)
    private void FxAPI_OnAccountMessage(FxAccount fxaccount, FxMessageType fxmtype) {
      if (_disposing)
        return;
      _sessionId++;

      Account account = _accounts.Update(fxaccount, fxmtype);
      base.OnAccountsChanged(new BrokerAccountsEventArgs(account, BrokerMessageType.Update));
    }
    #endregion

    #region private void FxAPI_OnError(FxError error)
    private void FxAPI_OnError(FxError fxerror) {
      if (_disposing) return;
      //			if (fxerror.Details.LastIndexOf("<p v=\"") > -1 ||
      //				fxerror.Details.LastIndexOf("<pair act=\"U\"") > -1)
      //				return;
      //_log.Add("API_OnError: " + fxerror.Details);
      //APIError error = new APIError(fxerror);
      //if (this.ErrorEvent != null)
      //  this.ErrorEvent(error);
    }
    #endregion

    #region private void FxAPI_OnMarginCallMessage(FxMarginCallMessage message)
    private void FxAPI_OnMarginCallMessage(FxMarginCallMessage message) {
      if (_disposing)
        return;
      _sessionId++;

      base.OnMarginCallCreated(new BrokerMarginCallEventArgs(""));

      //_log.Add(String.Format("OnMarginCallMessage: AccountID:{0}, MarginNumber:{1}, MarginLevel:{2}, MarginTime:{3}", message.AccountId, message.MagrinNumber, message.MarginLevel, message.MarginTime));
      //APIMarginCallMessage mcmsg = new APIMarginCallMessage(message);

      //if (this.MarginCallMessageEvent != null)
      //  this.MarginCallMessageEvent(mcmsg);
    }
    #endregion

    #region private void FxAPI_OnOrderMessage(FxOrder order,FxMessageType message)
    private void FxAPI_OnOrderMessage(FxOrder fxOrder, FxMessageType message) {
      if (_disposing)
        return;
      _sessionId++;

      if (_closeOrderId == fxOrder.OrderId) {
        if (fxOrder.OrderType == FxOrderType.ot_Close) {
        } else if (fxOrder.OrderType == FxOrderType.ot_RejectClose) {
          _rejectCloseOrder = fxOrder;
        }
        _closeOrderId = "";
      }

      if (_initOrderId == fxOrder.OrderId) {
        if (fxOrder.OrderType == FxOrderType.ot_RejectInit) {
          _rejectInitOrder = fxOrder;
        }
        _initOrderId = "";
      }

      switch (fxOrder.OrderType) {
        case FxOrderType.ot_Close:
        case FxOrderType.ot_EntryFailed:
        case FxOrderType.ot_Init:
        case FxOrderType.ot_InitFailed:
        case FxOrderType.ot_LimitFailed:
        case FxOrderType.ot_Margin:
        case FxOrderType.ot_MinEquity:
        case FxOrderType.ot_RejectClose:
        case FxOrderType.ot_RejectInit:
        case FxOrderType.ot_StopFailed:
          return;
        case FxOrderType.ot_EntryLimit:
        case FxOrderType.ot_EntryStop:
        case FxOrderType.ot_Stop:
        case FxOrderType.ot_Limit:
          break;
      }

      Order order = null;
      BrokerMessageType mtype = BrokerMessageType.Update;
      switch (message) {
        case FxMessageType.mt_Add:
          order = _orders.Add(fxOrder);
          mtype = BrokerMessageType.Add;
          break;
        case FxMessageType.mt_Delete:
          order = _orders.Remove(fxOrder);
          mtype = BrokerMessageType.Delete;
          break;
        case FxMessageType.mt_Update:
          order = _orders.Update(fxOrder);
          mtype = BrokerMessageType.Update;
          break;
      }
      this.OnOrdersChanged(new BrokerOrdersEventArgs(order, mtype));
      //      if (order.OrderType == OrderType.Limit || order.OrderType == OrderType.Stop) {
      _orders.Update();
      _trades.Update();
      //      }
    }
    #endregion

    #region private void FxAPI_OnPairMessage(FxPair pair, FxMessageType message)
    private void FxAPI_OnPairMessage(FxPair fxpair, FxMessageType message) {
      if (_disposing)
        return;
      _sessionId++;

      OnlineRate onlineRate = this._onlineRates.GetOnlineRateFromPairId(fxpair.PairId);
      onlineRate.UpdateRate(fxpair);
      Tick tick = new Tick(this.GetGmtTime(fxpair.Time).Ticks, Convert.ToSingle(fxpair.SellRate));
      (onlineRate.Symbol.Ticks as ITickList).Add(tick);

      this.OnOnlineRatesChanged(new BrokerOnlineRatesEventArgs(onlineRate, BrokerMessageType.Update));
    }
    #endregion

    #region private void FxAPI_OnRulesChange()
    private void FxAPI_OnRulesChange() { }
    #endregion

    #region private void FxAPI_OnTextMessage(FxTextMessage message)
    private void FxAPI_OnTextMessage(FxTextMessage message) {
      if (_disposing) return;

      // _log.Add(String.Format("OnTextMessage: <{0}>:{1}", message.Sender, message.Text));
    }
    #endregion

    #region private void FxAPI_OnTradeMessage(FxTrade trade, FxMessageType mtype)
    private void FxAPI_OnTradeMessage(FxTrade fxtrade, FxMessageType message) {
      if (_disposing)
        return;
      _sessionId++;

      Trade trade = null;
      BrokerMessageType mtype = BrokerMessageType.Update;
      switch (message) {
        case FxMessageType.mt_Add:
          trade = _trades.Add(fxtrade);
          mtype = BrokerMessageType.Add;
          break;
        case FxMessageType.mt_Delete:
          trade = _trades.Remove(fxtrade);
          mtype = BrokerMessageType.Delete;
          break;
        case FxMessageType.mt_Update:
          trade = _trades.Update(fxtrade);
          mtype = BrokerMessageType.Update;
          break;
      }


      if (mtype == BrokerMessageType.Delete) {
        IAccount account = this.Accounts.GetAccount(fxtrade.AccountId);
        this._closedTrades.Update(account);
        bool find = false;
        for (int i=0;i<this.ClosedTrades.Count;i++){
          IClosedTrade closeTrade = this.ClosedTrades[i];
          if (closeTrade.TradeId == fxtrade.TradeId) {
            find = true;
          }
        }
        if (!find) {
          ClosedTrade ct = new ClosedTrade(this, fxtrade, "");
          _closedTrades.Add(ct);
        }
      }
      this.OnTradesChanged(new BrokerTradesEventArgs(trade, mtype));
    }
    #endregion

    #region internal void OnTradeUpdateEvents(Trade trade)
    internal void OnTradeUpdateEvents(Trade trade) {
      this.OnTradesChanged(new BrokerTradesEventArgs(trade, BrokerMessageType.Update));
    }
    #endregion

    #region internal void OnOrderUpdateEvents(Order order)
    internal void OnOrderUpdateEvents(Order order) {
      this.OnOrdersChanged(new BrokerOrdersEventArgs(order, BrokerMessageType.Update));
    }
    #endregion

    #region public DateTime GetGmtTime(DateTime serverTime)
    /// <summary>
    /// Преобразование времени сервера к GMT
    /// </summary>
    /// <param name="serverTime"></param>
    /// <returns></returns>
    public DateTime GetGmtTime(DateTime serverTime) {
      return serverTime.AddHours(HourGMT);
    }
    #endregion

    #region public DateTime ConvertToServerTime(DateTime timeGMT)
    /// <summary>
    /// Преобразование текущего времени, к времени сервера
    /// </summary>
    /// <param name="timeGMT"></param>
    /// <returns></returns>
    public DateTime ConvertToServerTime(DateTime timeGMT) {
      return timeGMT.AddHours(-HourGMT);
    }
    #endregion

    #region public BrokerResult Logon(string userName, string password, BrokerProxyInfo proxy, bool demo)
    public BrokerResult Logon(string userName, string password, BrokerProxyInfo proxy, bool demo) {
      _busy = false;

      if (proxy != null) {
        _fxapi.ProxyServer = proxy.Server;
        _fxapi.ProxyPort = proxy.Port;
        _fxapi.ProxyUserName = proxy.UserName;
        _fxapi.ProxyPassword = proxy.Password;
      } else {
        _fxapi.ProxyServer = "";
        _fxapi.ProxyPort = 0;
        _fxapi.ProxyUserName = "";
        _fxapi.ProxyPassword = "";
      }
      string schema = demo ? FX_SCHEMA_DEMO : FX_SCHEMA_REAL;
      FxError fxerror = _fxapi.Logon(schema, userName, password);

      BrokerError error = null;
      if (fxerror != null)
        error = new BrokerError(BrokerErrorType.LogonUnknow, fxerror.Message);

      this.SetTimeDifference();
      _busy = true;
      return new BrokerResult(error);
    }
    #endregion

    #region public BrokerResult Logoff()
    public BrokerResult Logoff() {
      _busy = false;
      BrokerError error = null;
      FxError fxerror = _fxapi.Logoff();
      if (fxerror != null)
        error = new BrokerError(BrokerErrorType.Logoff);

      _busy = true;
      return new BrokerResult(error);
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate) {
      return OrderCreate(accountId, symbolName, tradeType, lots, rate, 0, 0);
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate) {
      return this.OrderCreate(accountId, symbolName, tradeType, lots, rate, stopRate, limitRate, "");
    }
    #endregion

    #region public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment)
    public BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment) {

      OnlineRate onlineRate = (OnlineRate)_onlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OnlineRateNotFound));


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

      FxStopLimit fxStopLimit = FxStopLimit.sl_Limit;

      if (tradeType == TradeType.Sell) {
        if (rate > onlineRate.SellRate) {
          fxStopLimit = FxStopLimit.sl_Limit;
        } else if (rate < onlineRate.SellRate) {
          fxStopLimit = FxStopLimit.sl_Stop;
        } else {
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.PendingOrderRateError));
        }
      } else {
        if (rate > onlineRate.BuyRate) {
          fxStopLimit = FxStopLimit.sl_Stop;
        } else if (rate < onlineRate.BuyRate) {
          fxStopLimit = FxStopLimit.sl_Limit;
        } else {
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.PendingOrderRateError));
        }
      }

      FxBuySell fxBuySell = tradeType == TradeType.Sell ? FxBuySell.bs_Sell : FxBuySell.bs_Buy;
      FxLotList fxlots = _fxapi.NewLotList;

      string entryOrderId;

      FxError fxerror = _fxapi.CreateEntryOrder(fxStopLimit, accountId, "", onlineRate.PairId, lots, fxBuySell, rate, 1, out entryOrderId, "", fxlots);
      if (fxerror != null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.Unknow));

      Order order = null;
      DateTime startTime = DateTime.Now;
      while (order == null) {
        /* Ожидание события на создание ордера (не более 3-х минут)*/
        order = _orders.GetOrder(entryOrderId) as Order;
        if ((DateTime.Now.Ticks - startTime.Ticks) / 10000000L * 60 >= 3)
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.Unknow));
        Thread.Sleep(10);
      }

      if (useStop) {
        BrokerResult sresult = OrderModifyStopLimit(OrderType.Stop, order.OrderId, stopRate);
      }

      if (useLimit) {
        BrokerResult sresult = OrderModifyStopLimit(OrderType.Limit, order.OrderId, limitRate);
      }

      return new BrokerResultOrder(order, null);
    }
    #endregion

    #region private BrokerResult OrderModifyStopLimit(OrderType orderType, string orderId, float rate)
    private BrokerResult OrderModifyStopLimit(OrderType orderType, string orderId, float rate) {
      FxStopLimit fxsl = orderType == OrderType.Limit ? FxStopLimit.sl_Limit : FxStopLimit.sl_Stop;
//      FxStopLimit fxsl = orderType == OrderType.Stop ? FxStopLimit.sl_Limit : FxStopLimit.sl_Stop;

      string stopLimitOrderId = null;
      FxError error = _fxapi.ChangeStopLimitOnEntryOrder(fxsl, orderId, Convert.ToDouble(rate), out stopLimitOrderId, "");
      if (error != null) {
        BrokerErrorType erType = orderType == OrderType.Limit ? BrokerErrorType.LimitOrderRateError : BrokerErrorType.StopOrderRateError;
        return new BrokerResult(new BrokerError(erType));
      }
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResult OrderDelete(string orderId)
    public BrokerResult OrderDelete(string orderId) {
      return OrderDelete(orderId, "");
    }
    #endregion

    #region public BrokerResult OrderDelete(string orderId, string comment)
    public BrokerResult OrderDelete(string orderId, string comment) {
      Order order = this.Orders.GetOrder(orderId) as Order;
      if (order == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OrderNotFound));

      FxError fxerror = _fxapi.DeleteOrder(orderId);
      if (fxerror != null)
        return new BrokerResult(new BrokerError(BrokerErrorType.Unknow));
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResultOrder OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate)
    public BrokerResultOrder OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate) {
      Order order = this.Orders.GetOrder(orderId) as Order;
      if (order == null)
        return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.OrderNotFound));

      if (order.Rate != newRate) {
        FxError fxerror = _fxapi.ChangeEntryOrderRate(order.OrderId, Convert.ToDouble(newRate));
        if (fxerror != null)
          return new BrokerResultOrder(null, new BrokerError(BrokerErrorType.PendingOrderRateError));
      }

      newStopRate = float.IsNaN(newStopRate) ? 0 : newStopRate;
      newLimitRate = float.IsNaN(newLimitRate) ? 0 : newLimitRate;

      float oldStopRate = order.StopOrder == null ? 0 : order.StopOrder.Rate;
      float oldLimitRate = order.LimitOrder == null ? 0 : order.LimitOrder.Rate;

      if (newStopRate != oldStopRate) {
        if (newStopRate == 0) {
          this.OrderDelete(order.StopOrder.OrderId);
        } else {
          BrokerResult sresult = OrderModifyStopLimit(OrderType.Stop, order.OrderId, newStopRate);
          if (sresult.Error != null)
            return new BrokerResultOrder(null, sresult.Error);
        }
      }

      if (newLimitRate != oldLimitRate) {
        if (newLimitRate == 0) {
          this.OrderDelete(order.LimitOrder.OrderId);
        } else {
          BrokerResult sresult = OrderModifyStopLimit(OrderType.Limit, order.OrderId, newLimitRate);
          if (sresult.Error != null)
            return new BrokerResultOrder(null, sresult.Error);
        }
      }

      return new BrokerResultOrder(order, null);
    }
    #endregion

    #region public BrokerResultTrade TradeModify(string tradeId, float newStopRate, float newLimitRate)
    public BrokerResultTrade TradeModify(string tradeId, float newStopRate, float newLimitRate) {

      Trade trade = this._trades.GetTrade(tradeId) as Trade;
      if (trade == null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.TradeNotFound));


      newStopRate = float.IsNaN(newStopRate) ? 0 : newStopRate;
      newLimitRate = float.IsNaN(newLimitRate) ? 0 : newLimitRate;

      float oldStopRate = trade.StopOrder == null ? 0 : trade.StopOrder.Rate;
      float oldLimitRate = trade.LimitOrder == null ? 0 : trade.LimitOrder.Rate;

      if (newStopRate != oldStopRate) {
        if (newStopRate == 0) {
          this.OrderDelete(trade.StopOrder.OrderId);
        } else {
          BrokerResult sresult = CreateStopLimitOnTrade(trade, OrderType.Stop, newStopRate);
          if (sresult.Error != null)
            return new BrokerResultTrade(null, sresult.Error);
        }
      }

      if (newLimitRate != oldLimitRate) {
        if (newLimitRate == 0) {
          this.OrderDelete(trade.LimitOrder.OrderId);
        } else {
          BrokerResult sresult = CreateStopLimitOnTrade(trade, OrderType.Limit, newLimitRate);
          if (sresult.Error != null)
            return new BrokerResultTrade(null, sresult.Error);
        }
      }
      return new BrokerResultTrade(trade, null);
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage) {
      return this.TradeOpen(accountId, symbolName, tradeType, lots, slippage, 0, 0, "");
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate) {
      return this.TradeOpen(accountId, symbolName, tradeType, lots, slippage, stopRate, limitRate, "");
    }
    #endregion

    #region public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment)
    public BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment) {


      OnlineRate onlineRate = (OnlineRate)_onlineRates.GetOnlineRate(symbolName);
      if (onlineRate == null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.OnlineRateNotFound));

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

      _initOrderId = "";
      FxLotList fxlots = _fxapi.NewLotList;
      FxBuySell bs = tradeType == TradeType.Buy ? FxBuySell.bs_Buy : FxBuySell.bs_Sell;

      FxError fxerror = _fxapi.CreateInitOrder(accountId, "", onlineRate.PairId, lots, bs, 1, out _initOrderId, "", fxlots);
      if (fxerror != null)
        return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.Unknow));

      string initOrderId = _initOrderId;

      DateTime startTime = DateTime.Now;
      while (_initOrderId != "") {
        if ((DateTime.Now.Ticks - startTime.Ticks) / 10000000L * 60 >= 2)
          return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.Unknow));
        Thread.Sleep(1);
        if (_rejectInitOrder != null) {
          break;
        }
      }
      _initOrderId = "";

      if (_rejectInitOrder != null) {
        FxOrder fxROrder = _rejectInitOrder;
        _rejectInitOrder = null;
        _fxapi.AcceptRejectedOrder(fxROrder.OrderId);
      }

      Trade trade = null;
      FxOrder fxOrder = null;
      startTime = DateTime.Now;
      while (fxOrder == null && trade == null) {
        /* Ожидание события на создание ордера (не более 3-х минут)*/

        for (int i = 0; i < _fxapi.Orders.Count; i++) {
          FxOrder fxTmpOrder = _fxapi.Orders.get_Order(i);
          if (fxTmpOrder != null) {
            if (fxTmpOrder.OrderId == initOrderId) {
              if (fxTmpOrder.OrderType == FxOrderType.ot_RejectInit)
                return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.Unknow));
              break;
            }
          }
        }

        for (int i = 0; i < this.Trades.Count; i++) {
          if (this._trades[i].ParentOrderId == initOrderId) {
            trade = _trades[i] as Trade;
            break;
          }
        }

        if ((DateTime.Now.Ticks - startTime.Ticks) / 10000000L * 60 >= 2)
          return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.Unknow));
        Thread.Sleep(1);
      }

      if (useStop) {
        BrokerResult sresult = CreateStopLimitOnTrade(trade, OrderType.Stop, stopRate);
      }

      if (useLimit) {
        BrokerResult sresult = CreateStopLimitOnTrade(trade, OrderType.Limit, limitRate);
      }

      return new BrokerResultTrade(trade, null);
    }
    #endregion

    #region private BrokerResult CreateStopLimitOnTrade(Trade trade, OrderType orderType, float rate)
    private BrokerResult CreateStopLimitOnTrade(Trade trade, OrderType orderType, float rate) {
      FxStopLimit fxsl = orderType == OrderType.Limit ? FxStopLimit.sl_Limit : FxStopLimit.sl_Stop;

      string stopOrderId = null;
      FxError error = _fxapi.ChangeStopLimitOnTrade(fxsl, trade.TradeId, Convert.ToDouble(rate), out stopOrderId, "");
      if (error != null) {
        BrokerErrorType erType = orderType == OrderType.Limit ? BrokerErrorType.LimitOrderRateError : BrokerErrorType.StopOrderRateError;
        return new BrokerResult(new BrokerError(erType));
      }
      return new BrokerResult(null);
    }
    #endregion

    #region public BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment)
    public BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment) {
      return TradeClose(tradeId, lots, slippage);
    }
    #endregion

    #region public BrokerResult TradeClose(string tradeId, int lots, int slippage)
    public BrokerResult TradeClose(string tradeId, int lots, int slippage) {
      _slipPage = slippage;
      _closeOrderId = "";
      bool closeWithHedge = false;
      FxLogic fxCloseWithHedge = closeWithHedge ? FxLogic.lg_True : FxLogic.lg_False;
      FxError fxerror = this._fxapi.CloseTrade(tradeId, 0, lots, out _closeOrderId, "", fxCloseWithHedge, null);
      BrokerError error = null;
      if (fxerror != null) {
        _closeOrderId = "";
        error = new BrokerError(BrokerErrorType.TradeClose, fxerror.Message);
      }
      DateTime startTime = DateTime.Now;
      while (_closeOrderId != "") {
        if ((DateTime.Now.Ticks - startTime.Ticks) / 10000000L * 60 >= 2)
          return new BrokerResultTrade(null, new BrokerError(BrokerErrorType.Unknow));
        Thread.Sleep(1);
        if (_rejectCloseOrder != null) {
          break;
        }
      }
      _closeOrderId = "";

      if (_rejectCloseOrder != null) {
        FxOrder fxOrder = _rejectCloseOrder;
        _rejectCloseOrder = null;
        _fxapi.AcceptRejectedOrder(fxOrder.OrderId);
      }

      return new BrokerResult(error);
    }
    #endregion

    #region public BrokerResultClosedTrades GetClosedTrades(string accountId, string fromTradeId, string toTradeId, DateTime fromTime, DateTime toTime)
    public BrokerResultClosedTrades GetClosedTrades(string accountId, string fromTradeId, string toTradeId, DateTime fromTime, DateTime toTime) {
      FxTradeList fxTradeList = null;
      FxError error = _fxapi.GetClosedTrades(fromTradeId, toTradeId, fromTime, toTime, accountId, "", out fxTradeList);
      BrokerError berror = null;
      List<IClosedTrade> closedTrades = new List<IClosedTrade>();
      if (error != null) {
        berror = new BrokerError(BrokerErrorType.Unknow);
      } else if (fxTradeList != null) {
        for (int i = 0; i < fxTradeList.Count; i++) {
          FxTrade fxTrade = fxTradeList.get_Trade(i);
          ClosedTrade cTrade = new ClosedTrade(this, fxTrade, "");
          closedTrades.Add(cTrade);
        }
      }
      return new BrokerResultClosedTrades(closedTrades.ToArray(), berror);
    }
    #endregion

    #region public BrokerResultTickHistory GetTickHistory(IOnlineRate onlineRate, DateTime timeGMT1, DateTime timeGMT2)
    public BrokerResultTickHistory GetTickHistory(IOnlineRate onlineRate, DateTime timeGMT1, DateTime timeGMT2) {
      List<Tick> ticks = new List<Tick>();
      BrokerError error = null;

      DateTime serverTime = this.Time;

      if (serverTime.Ticks < (new DateTime(2005, 1, 1)).Ticks)
        return new BrokerResultTickHistory(ticks.ToArray(), new BrokerError(BrokerErrorType.Unknow));

      FxPair fxPair = (onlineRate as OnlineRate).FxPair;

      DateTime serverTimeGMT = this.Time;

      DateTime time1 = ConvertToServerTime(timeGMT1).AddMinutes(-1);
      DateTime time2 = ConvertToServerTime(timeGMT2).AddMinutes(5);

      System.Diagnostics.Debug.WriteLine(string.Format("Закачиваю историю: {0}, {1}, {2}", onlineRate.Symbol.Name, time1, time2));

      FxTickList fxticks;
      FxError fxError = _fxapi.GetTickHistory(fxPair.PairId, 0, time1, time2, out fxticks);

      int cnt = 0;
      if (fxticks != null)
        cnt = fxticks.Count;

      for (int i = 0; i < cnt; i++) {
        FxTick fxtick = fxticks.get_Tick(i);
        DateTime tm = this.GetGmtTime(fxtick.Time);
        Tick tick = new Tick(tm.Ticks, Convert.ToSingle(fxtick.Rate));

        bool isok = true;
        if (tm.DayOfWeek == DayOfWeek.Saturday) {
          isok = false;
        } else if (tm.DayOfWeek == DayOfWeek.Sunday) {
          DateTime t = new DateTime(tm.Year, tm.Month, tm.Day, 18, 30, 0, 0);
          if (tm < t)
            isok = false;
        } else if (tm.DayOfWeek == DayOfWeek.Friday) {
          DateTime t = new DateTime(tm.Year, tm.Month, tm.Day, 21, 30, 0, 0);

          if (tm > t)
            isok = false;
        }
        if (isok)
          ticks.Add(tick);
      }

      return new BrokerResultTickHistory(ticks.ToArray(), error);
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
  }
}
