/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Language;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Cursit.Utils;

namespace Gordago.API {

  #region public class UpdateSymbolEventArgs : EventArgs
  public class UpdateSymbolEventArgs : EventArgs {

    private ISymbol _symbol;

    public UpdateSymbolEventArgs(ISymbol symbol){
      _symbol = symbol;
    }

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion
  }
  #endregion

  class BrokerCommandManager:IDisposable {

    private int _minStopPoint = 5;
    private int _minLimitPoint = 5;
    private ISymbolList _symbols;
    private List<SymbolUpdate> _symbolsUpdate, _symbolsUpdateWait, _symbolsUpdateDown;
    private BrokerCommand _command = null;
    private IBroker _broker;
    private bool _abortProccess = false;
    private Type _brokerType;

    private StrategyEngine _strategyEngine;

    private BrokerCommandLogon _commandLogon;
    private object[] _parameters = null;

    public BrokerCommandManager(ISymbolList symbols) {
      _symbols = symbols;
      _symbolsUpdate = new List<SymbolUpdate>();
      _symbolsUpdateDown = new List<SymbolUpdate>();
      _symbolsUpdateWait = new List<SymbolUpdate>();
      _strategyEngine = new StrategyEngine(this);
      this.LoadLanguage();
    }

    #region public IBroker Broker
    public IBroker Broker {
      get { return _broker; }
    }
    #endregion

    #region public StrategyEngine StrategyEngine
    public StrategyEngine StrategyEngine {
      get { return _strategyEngine; }
    }
    #endregion

    #region public BrokerConnectionStatus ConnectionStatus
    public BrokerConnectionStatus ConnectionStatus {
      get {
        if (_broker == null)
          return BrokerConnectionStatus.Offline;
        return _broker.ConnectionStatus;
      }
    }
    #endregion

    #region public int MinStopPoint
    public int MinStopPoint {
      get { return this._minStopPoint; }
      set { this._minStopPoint = Math.Max(value, 1); }
    }
    #endregion

    #region public int MinLimitPoint
    public int MinLimitPoint {
      get { return this._minLimitPoint; }
      set { this._minLimitPoint = Math.Max(value, 1); }
    }
    #endregion

    #region public bool Busy
    /// <summary>
    /// Свободен ли менеджер команд брокера на исполнение. 
    /// True - свободен, False - есть команда на исполнение
    /// </summary>
    public bool Busy {
      get {
        return _command == null;
      }
    }
    #endregion

    #region private bool WaitBusy()
    private bool WaitBusy() {
      int limit = 1000;
      while (!Busy) {
        Thread.Sleep(3);
        limit--;
        if (limit < 0)
          return false;
      }
      return true;
    }
    #endregion

    #region private bool IsRun
    private bool IsRun {
      get { return !_abortProccess; }
    }
    #endregion

    #region private Assembly domain_AssemblyResolver(object sender, ResolveEventArgs e)
    private Assembly domain_AssemblyResolver(object sender, ResolveEventArgs e) {

      string[] sa = e.Name.Split(',');
      string asmname = sa[0].Trim() + ".dll";

      string dir = Application.StartupPath + "\\brokers";

      string[] files = Directory.GetFiles(dir, "*.dll");
      foreach (string file in files) {
        //try {
        //  Type[] types = Assembly.LoadFile(file).GetTypes();
        //  foreach (Type type in types) {
        //    if (type.BaseType.Name == asmname) {
        //      return type.Assembly;
        //    }
        //  }
        //} catch { }
        string fname = FileEngine.GetFileNameFromPath(file);
        if (fname == asmname) {
          return Assembly.LoadFile(file);
        }
      }

      return null;
    }
    #endregion

    #region public void Start(Type brokerType, BrokerCommandLogon commandLogon, params object[] parameters)
    public void Start(Type brokerType, BrokerCommandLogon commandLogon, params object[] parameters) {
      _commandLogon = commandLogon;
      _brokerType = brokerType;
      _parameters = parameters;

      Thread th = new Thread(new ThreadStart(this.Proccess));
      th.IsBackground = true;
      th.Name = "BrokerCommandThread";
      th.Start();
    }
    #endregion

    #region private void Proccess()
    private void Proccess() {
      Broker broker = null;
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.domain_AssemblyResolver);
      try {
        broker = Activator.CreateInstance(_brokerType, _symbols, _parameters) as Broker;
        _broker = (IBroker)broker;
        _abortProccess = false;
      } catch {
        _broker = null;
        _abortProccess = true;
      }
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(this.domain_AssemblyResolver);

      GordagoMain.MainForm.BrokerInitializeResult(_broker, new EventArgs());

      if (_broker == null)
        return;

      EventHandler connectionStatusEvent = new EventHandler(this._broker_ConnectionStatusChanged);
      EventHandler<BrokerJornalEventArgs> journalEvent = new EventHandler<BrokerJornalEventArgs>(this._broker_JournalRecordAdded);

      broker.ConnectioStatusChanged += connectionStatusEvent;
      broker.Journal.RecordAdded += journalEvent;

      this.OnCommandStarting(_commandLogon);
      BrokerResult result = _broker.Logon(_commandLogon.Login, _commandLogon.Password, _commandLogon.Proxy, _commandLogon.Demo);
      this.OnCommandStopping(_commandLogon, result);
      if (result.Error != null) {
        broker.Journal.RecordAdded -= journalEvent;
        broker.ConnectioStatusChanged -= connectionStatusEvent;
        return;
      }
      _commandLogon = null;

      EventHandler<BrokerAccountsEventArgs> accountsEvent = new EventHandler<BrokerAccountsEventArgs>(this._broker_AccountsChanged);
      EventHandler<BrokerTradesEventArgs> tradesEvent = new EventHandler<BrokerTradesEventArgs>(this._broker_TradesChanged);
      EventHandler<BrokerOrdersEventArgs> ordersEvent = new EventHandler<BrokerOrdersEventArgs>(this._broker_OrdersChanged);
      EventHandler<BrokerOnlineRatesEventArgs> onlineRatesEvent = new EventHandler<BrokerOnlineRatesEventArgs>(this._broker_OnlineRatesChanged);
      EventHandler<BrokerMarginCallEventArgs> marginCallEvent = new EventHandler<BrokerMarginCallEventArgs>(this._broker_MarginCallCreated);

      broker.AccountsChanged += accountsEvent;
      broker.OnlineRatesChanged += onlineRatesEvent;
      broker.OrdersChanged += ordersEvent;
      broker.TradesChanged += tradesEvent;
      broker.MarginCallCreated += marginCallEvent;

      result = null;
      while (!_abortProccess) {
        #region Закачка котировок

        if (_symbolsUpdateWait.Count > 0 && _symbolsUpdateDown.Count < 5 && _broker.ConnectionStatus == BrokerConnectionStatus.Online) {

          SymbolUpdate su = _symbolsUpdateWait[0];
          _symbolsUpdateWait.RemoveAt(0);
          _symbolsUpdateDown.Add(su);

          Thread th = new Thread(new ParameterizedThreadStart(this.SymbolUpdateProccess));
          th.IsBackground = true;
          th.Name = "SymbolUpdateProccess";
          th.Priority = ThreadPriority.Lowest;
          th.Start(su);
        }
        #endregion
        if (_command != null) {
          this.OnCommandStarting(_command);

          if (_broker.ConnectionStatus == BrokerConnectionStatus.Offline) {
            if (_command is BrokerCommandLogon) {
              BrokerCommandLogon cmdl = _command as BrokerCommandLogon;
              result = _broker.Logon(cmdl.Login, cmdl.Password, cmdl.Proxy, cmdl.Demo);
            }
          } else if (_broker.ConnectionStatus == BrokerConnectionStatus.Online) {
            if (_command is BrokerCommandLogoff) {
              result = _broker.Logoff();
              if (result.Error == null) {
                _abortProccess = true;
              }
            } else if (_command is BrokerCommandTradeOpen) {
              BrokerCommandTradeOpen cmdTO = _command as BrokerCommandTradeOpen;
              result = _broker.TradeOpen(cmdTO.AccountId, cmdTO.OnlineRate.Symbol.Name, cmdTO.TradeType, cmdTO.LotCount, cmdTO.Slippage, cmdTO.StopRate, cmdTO.LimitRate, cmdTO.Comment);
            } else if (_command is BrokerCommandTradeModify) {
              BrokerCommandTradeModify cmd = _command as BrokerCommandTradeModify;
              result = _broker.TradeModify(cmd.Trade.TradeId, cmd.StopRate, cmd.LimitRate);
            } else if (_command is BrokerCommandTradeClose) {
              BrokerCommandTradeClose cmd = _command as BrokerCommandTradeClose;
              result = _broker.TradeClose(cmd.Trade.TradeId, cmd.Lots, cmd.Slippage, cmd.Comment);
            } else if (_command is BrokerCommandEntryOrderCreate) {
              BrokerCommandEntryOrderCreate cmd = _command as BrokerCommandEntryOrderCreate;
              result = _broker.OrderCreate(cmd.AccountId, cmd.OnlineRate.Symbol.Name, cmd.TradeType, cmd.Lots, cmd.Rate, cmd.StopRate, cmd.LimitRate, cmd.Comment);
            } else if (_command is BrokerCommandEntryOrderModify) {
              BrokerCommandEntryOrderModify cmd = _command as BrokerCommandEntryOrderModify;
              result = _broker.OrderModify(cmd.Order.OrderId, cmd.NewLots, cmd.NewRate, cmd.NewStopRate, cmd.NewLimitRate);
            } else if (_command is BrokerCommandEntryOrderDelete) {
              BrokerCommandEntryOrderDelete cmd = _command as BrokerCommandEntryOrderDelete;
              result = _broker.OrderDelete(cmd.Order.OrderId, cmd.Comment);
            } else {
              System.Diagnostics.Debug.WriteLine("ERROR: " + _command.GetType().Name);
            }
          }
          BrokerCommand scmd = _command;
          _command = null;
          this.OnCommandStopping(scmd, result);
          result = null;
        }
        Thread.Sleep(10);
      }
      broker.AccountsChanged -= accountsEvent;
      broker.ConnectioStatusChanged -= connectionStatusEvent;
      broker.OnlineRatesChanged -= onlineRatesEvent;
      broker.OrdersChanged -= ordersEvent;
      broker.TradesChanged -= tradesEvent;
      broker.MarginCallCreated -= marginCallEvent;
      broker.Journal.RecordAdded -= journalEvent;

      ((Broker)_broker).Dispose();
      _broker = null;
    }
    #endregion

    #region public void Stop()
    public void Stop() {
      _abortProccess = true;
    }
    #endregion

    #region public void ExecuteCommand(BrokerCommand command)
    public void ExecuteCommand(BrokerCommand command) {
      if (!WaitBusy()) return;
      _command = command;
    }
    #endregion

    #region private void OnCommandStarting(BrokerCommand commnad)
    private void OnCommandStarting(BrokerCommand commnad) {
      GordagoMain.MainForm.BrokerCommandStarting(commnad);
      _strategyEngine.BrokerCommandStarting(commnad);
    }
    #endregion

    #region private void OnCommandStopping(BrokerCommand command, BrokerResult result) 
    private void OnCommandStopping(BrokerCommand command, BrokerResult result) {
      GordagoMain.MainForm.BrokerCommandStopping(command, result);
      _strategyEngine.BrokerCommandStopping(command, result);
    }
    #endregion

    #region private void _broker_AccountsChanged(object sender, BrokerAccountsEventArgs be)
    private void _broker_AccountsChanged(object sender, BrokerAccountsEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerAccountsChanged(be);
      _strategyEngine.BrokerAccountsChanged(be);
    }
    #endregion

    #region private void _broker_TradesChanged(object sender, BrokerTradesEventArgs be)
    private void _broker_TradesChanged(object sender, BrokerTradesEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerTradesChanged(be);
      _strategyEngine.BrokerTradesChanged(be);
    }
    #endregion

    #region private void _broker_OrdersChanged(object sender, BrokerOrdersEventArgs be)
    private void _broker_OrdersChanged(object sender, BrokerOrdersEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerOrdersChanged(be);
      _strategyEngine.BrokerOrdersChanged(be);
    }
    #endregion

    #region private void _broker_OnlineRatesChanged(object sender, BrokerOnlineRatesEventArgs be)
    private void _broker_OnlineRatesChanged(object sender, BrokerOnlineRatesEventArgs be) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerOnlineRatesChanged(be);
      _strategyEngine.BrokerOnlineRatesChanged(be);
    }
    #endregion

    #region private void _broker_ConnectionStatusChanged(object sender, EventArgs e)
    private void _broker_ConnectionStatusChanged(object sender, EventArgs e) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerConnectionStatusChanged(this._broker.ConnectionStatus);
      _strategyEngine.BrokerConnectionStatusChanged(this._broker.ConnectionStatus);
      if (_broker.ConnectionStatus == BrokerConnectionStatus.Offline)
        _abortProccess = true;
    }
    #endregion

    #region private void _broker_MarginCallCreated(object sender, BrokerMarginCallEventArgs be)
    private void _broker_MarginCallCreated(object sender, BrokerMarginCallEventArgs be) {
    }
    #endregion

    #region private void _broker_JournalRecordAdded(object sender, BrokerJornalEventArgs e)
    private void _broker_JournalRecordAdded(object sender, BrokerJornalEventArgs e) {
      if (GordagoMain.IsCloseProgram)
        return;
      GordagoMain.MainForm.BrokerJournalRecordAdded(sender, e);
    }
    #endregion

    #region protected virtual void OnUpdateSymbolStarting(UpdateSymbolEventArgs se)
    protected virtual void OnUpdateSymbolStarting(UpdateSymbolEventArgs se) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerUpdateSymbolStarting(se);
    }
    #endregion

    #region protected virtual void OnUpdateSymbolStopping(UpdateSymbolEventArgs se)
    protected virtual void OnUpdateSymbolStopping(UpdateSymbolEventArgs se) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerUpdateSymbolStopping(se);
    }
    #endregion

    #region protected virtual void OnUpdateSymbolDownloadPart(UpdateSymbolEventArgs se)
    protected virtual void OnUpdateSymbolDownloadPart(UpdateSymbolEventArgs se) {
      if (GordagoMain.IsCloseProgram) return;
      GordagoMain.MainForm.BrokerUpdateSymbolDownloadPart(se);
    }
    #endregion

    #region public void Dispose()
    public void Dispose() {
      _abortProccess = true;
    }
    #endregion

    #region public SymbolUpdateStatus GetSymbolUpdateStatus(ISymbol symbol)
    public SymbolUpdateStatus GetSymbolUpdateStatus(ISymbol symbol) {
      for (int i = 0; i < this._symbolsUpdate.Count; i++) {
        if (this._symbolsUpdate[i] == symbol)
          return SymbolUpdateStatus.Update;
      }

      for (int i = 0; i < this._symbolsUpdateWait.Count; i++) {
        if (_symbolsUpdateWait[i] == symbol)
          return SymbolUpdateStatus.Wait;
      }

      for (int i = 0; i < this._symbolsUpdateDown.Count; i++) {
        if (this._symbolsUpdateDown[i] == symbol)
          return SymbolUpdateStatus.Download;
      }

      return SymbolUpdateStatus.Hole;
    }
    #endregion

    #region public void UpdateSymbolHole(ISymbol symbol)
    public void UpdateSymbolHole(ISymbol symbol) {
      if (GetSymbolUpdateStatus(symbol) != SymbolUpdateStatus.Hole)
        return;

      _symbolsUpdateWait.Add(new SymbolUpdate(symbol));
    }
    #endregion

    #region private void SymbolUpdateProccess(object data)
    private void SymbolUpdateProccess(object data) {
      SymbolUpdate symbolupdate = data as SymbolUpdate;
      this.OnUpdateSymbolStarting(new UpdateSymbolEventArgs(symbolupdate.Symbol));

      this.SymbolUpdateProccessMethod(symbolupdate);

      _symbolsUpdate.Add(symbolupdate);
      _symbolsUpdateDown.Remove(symbolupdate);

      this.OnUpdateSymbolStopping(new UpdateSymbolEventArgs(symbolupdate.Symbol));
    }
    #endregion

    #region private void SymbolUpdateProccessMethod(SymbolUpdate symbolupdate)
    private void SymbolUpdateProccessMethod(SymbolUpdate symbolupdate) {
      ISymbol symbol = symbolupdate.Symbol;

      IOnlineRate onlineRate = _broker.OnlineRates.GetOnlineRate(symbol.Name);
      if (onlineRate == null)
        return;

      DateTime servertimeGMT = _broker.Time;

#if DEBUG
      int hoursPeriod = 24 * 7;
       // int hoursPeriod = 24 ;
#elif DEMO
      int hoursPeriod = 24*2;
#else
      int hoursPeriod = 24*7;
#endif

      long fromtime = Math.Max(symbol.Ticks.TimeTo.Ticks, 
        servertimeGMT.AddHours(-hoursPeriod).Ticks);

      DateTime t1 = new DateTime(fromtime);
      DateTime t2 = servertimeGMT;

      bool download = true;
      int counterror = 0, maxerror = 5;
      while (download && !_abortProccess) {
        DateTime tend = t1.AddHours(3);

        if (tend > t2) {
          tend = t2;
          download = false;
        }

        DateTime dtm1 = t1.AddMinutes(-1);
        DateTime dtm2 = tend.AddMinutes(2);
        BrokerResultTickHistory bcres = _broker.GetTickHistory(onlineRate, dtm1, dtm2);

        if (_abortProccess)
          return;

        Tick[] ticks = bcres.Ticks;

        if (bcres.Error != null) {
          counterror++;
          if (counterror >= maxerror)
            return;
        } else {
          ITickManager tmanager = symbol.Ticks as ITickManager;
          while (tmanager.Status != TickManagerStatus.Default) {
            Thread.Sleep(1);
          }

          try {
            tmanager.Update(new TickCollection(ticks), true);
          } catch { }
          // tmanager.UseDataCachingEvents = false;
          tmanager.DataCachingMethod();
          // tmanager.UseDataCachingEvents = true;

          if (_abortProccess) return;

          this.OnUpdateSymbolDownloadPart(new UpdateSymbolEventArgs(symbol));
          t1 = tend;
        }
      }
      System.Diagnostics.Debug.WriteLine(string.Format("Закачка по {0} закончена", onlineRate.Symbol.Name));
      return;
    }
    #endregion

    #region Language dictionary
    public static string LNG_TERMINAL = "Terminal";
//    public static string LNG_T_ACCOUNTS = "Accounts";
    public static string LNG_T_TRADES = "Trades";
    public static string LNG_T_ORDERS = "Orders";
    public static string LNG_T_OPERATE = "Operate";
    public static string LNG_T_STRATEGY = "Strategy";

    public static string LNG_BS_BUY = "Buy";
    public static string LNG_BS_SELL = "Sell";

    public static string LNG_BTN_MODIFY = "Modify";
    public static string LNG_BTN_DELETE = "Delete";
    public static string LNG_BTN_CLOSE = "Close";

    public static string LNG_TBL_TYPEACCOUNT = "Type";
    public static string LNG_TBL_BALANCE = "Balance";
    public static string LNG_TBL_CAPITAL = "Capital";
    public static string LNG_TBL_USEDMARGIN = "Used Margin";
    public static string LNG_TBL_FREEMARGIN = "Free Margin";
    public static string LNG_TBL_PREMIUM = "Premium";
    public static string LNG_TBL_COMMISION = "Commision";
    public static string LNG_TBL_FEE = "Fee";
    public static string LNG_TBL_NETPLALL = "Net All";

    public static string LNG_TBL_ACCOUNT = "Account";
    public static string LNG_TBL_TRADE = "Trade";
    public static string LNG_TBL_SYMBOL = "Symbol";
    public static string LNG_TBL_AMOUNT = "Amount";
    public static string LNG_TBL_BUYSELL = "B/S";
    public static string LNG_TBL_OPENRATE = "Open Rate";
    public static string LNG_TBL_CLOSERATE = "Close Rate";
    public static string LNG_TBL_STOP = "Stop";
    public static string LNG_TBL_LIMIT = "Limit";
    public static string LNG_TBL_TRAIL = "Trail";
    public static string LNG_TBL_NETPOINT = "Pips profit";
    public static string LNG_TBL_NETPIRCE = "NET (price)";
    public static string LNG_TBL_OPENTIME = "Open Time";
    public static string LNG_TBL_CLOSETIME = "Close Time";
    public static string LNG_TBL_TYPE = "Type";
    public static string LNG_TBL_ORDER = "Order";
    public static string LNG_TBL_LOTS = "Lots";
    public static string LNG_TBL_SLIPPAGE = "Slippage";
    public static string LNG_TBL_RATE = "Rate";
    public static string LNG_DEPEND = "Depend";

    public static string LNG_OT_INIT = "Init";
    public static string LNG_OT_REJECTINIT = "RejectInit";
    public static string LNG_OT_CLOSE = "Close";
    public static string LNG_OT_REJECTCLOSE = "RejectClose";
    public static string LNG_OT_ENTRYSTOP = "EntryStop";
    public static string LNG_OT_ENTRYLIMIT = "EntryLimit";
    public static string LNG_OT_STOP = "Stop";
    public static string LNG_OT_LIMIT = "Limit";
    public static string LNG_OT_MARGIN = "Margin";
    public static string LNG_OT_MINEQUITY = "MinEquity";
    public static string LNG_OT_INITFAILED = "InitFailed";
    public static string LNG_OT_ENTRYFAILED = "EntryFailed";
    public static string LNG_OT_STOPFAILED = "StopFailed";
    public static string LNG_OT_LIMITFAILED = "LimitFailed";

    public static string LNG_HO_SYMBOL = "Symbol";
    public static string LNG_HO_BID = "Bid";
    public static string LNG_HO_ASK = "Ask";
    public static string LNG_HO_ADD = "Add";
    public static string LNG_HO_DELETE = "Delete";
    public static string LNG_HO_SELL = "Sell";
    public static string LNG_HO_BUY = "Buy";
    public static string LNG_HO_SELLPRIVIEW = "Sell Preview";
    public static string LNG_HO_BUYPREVIEW = "Buy Preview";
    public static string LNG_HO_PENDINGORDER = "Pending Order";
    public static string LNG_HO_LOTS = "Lots";
    public static string LNG_HO_SLIPPAGE = "Slippage";
    public static string LNG_HO_STOP = "Stop";
    public static string LNG_HO_LIMIT = "Limit";

    public static string LNG_HO_RATE = "Rate";
    public static string LNG_HO_NONORDER = "Non order";
    public static string LNG_HO_LIMITORDER = "Limit order";
    public static string LNG_HO_STOPORDER = "Stop order";

    public static string LNG_CMD_OFFLINE = "Связь не установлена";
    public static string LNG_CMD_LOGON_START = "Установка соединения с сервером...";
    public static string LNG_CMD_LOGON_ONLINE = "Связь с сервером установлена";
    public static string LNG_CMD_LOADINGDATA = "Загрузка данных...";
    public static string LNG_CMD_LOGON_WAITING = "Ожидание соединения с сервером...";
    public static string LNG_CMD_LOGON_ERROR = "Ошибка соединения с сервером: {0}";

    public static string LNG_CMD_UPDATESYMBOLHOLE = "Загрузка данных {0} с сервера...";
    public static string LNG_CMD_UPDATESYMBOLHOLE_STOP = "Загрузка данных {0} с сервера завершена успешно";
    public static string LNG_CMD_UPDATESYMBOLHOLE_ERROR = "Ошбика загрузка данных {0} с сервера: {1}";

    public static string LNG_CMD_CHANGEENTRYORDER = "Изменение отложеного ордера...";
    public static string LNG_CMD_CHANGEENTRYORDER_STOP = "Изменение отложеного ордера завершено успешно";
    public static string LNG_CMD_CHANGEENTRYORDER_ERROR = "Ошибка изменение отложеного ордера: {0}";

    public static string LNG_CMD_CHANGE_SLT_ENTRYORDER = "Изменение Стоп, Лимит отложеного ордера...";
    public static string LNG_CMD_CHANGE_SLT_ENTRYORDER_STOP = "Изменение Стоп, Лимит отложеного ордера завершено успешно";
    public static string LNG_CMD_CHANGE_SLT_ENTRYORDER_ERROR = "Ошибка изменение Стоп, Лимит отложеного ордера: {0}";

    public static string LNG_CMD_CHANGE_SLT_TRADE = "Изменение Стоп, Лимит ордера...";
    public static string LNG_CMD_CHANGE_SLT_TRADE_STOP = "Изменение Стоп, Лимит ордера завершено успешно";
    public static string LNG_CMD_CHANGE_SLT_TRADE_ERROR = "Ошибка изменение Стоп, Лимит ордера: {0}";

    public static string LNG_CMD_CLOSETRADE = "Запрос на закрытие ордера...";
    public static string LNG_CMD_CLOSETRADE_STOP = "Запрос на закрытие ордера пройден успешно";
    public static string LNG_CMD_CLOSETRADE_ERROR = "Ошибка запроса на закрытие ордера: {0}";

    public static string LNG_CMD_CREATEENTRYORDER = "Создание отложеного ордера...";
    public static string LNG_CMD_CREATEENTRYORDER_STOP = "Создание отложеного ордера завершено успешно";
    public static string LNG_CMD_CREATEENTRYORDER_ERROR = "Ошибка создание отложеного ордера: {0}";

    public static string LNG_CMD_DELETEORDER = "Удаление отложеного ордера...";
    public static string LNG_CMD_DELETEORDER_STOP = "Удаление отложеного ордера прошло успешно";
    public static string LNG_CMD_DELETEORDER_ERROR = "Ошибка удаление отложеного ордера: {0}";

    public static string LNG_CMD_CREATETRADE = "Запрос на создание ордера...";
    public static string LNG_CMD_CREATETRADE_STOP = "Запрос на создание завершено успешно";
    public static string LNG_CMD_CREATETRADE_ERROR = "Ошибка запроса на создание ордера: {0}";

    public static string LNG_CMD_ACCEPTREJECTORDER = "Запрос на подтверждение ордера изменения цены в команде...";
    public static string LNG_CMD_ACCEPTREJECTORDER_STOP = "Запрос на подтверждение ордера изменения цены в команде завершен успешно";
    public static string LNG_CMD_ACCEPTREJECTORDER_ERROR = "Ошибка запроса на подтверждение ордера изменения цены в команде";

    #region public static string GetLngBuySell(TradeType tradeType)
    public static string GetLngBuySell(TradeType tradeType) {
      if (tradeType == TradeType.Buy)
        return LNG_BS_BUY;
      else
        return LNG_BS_SELL;
    }

    public static string GetLngOrderType(OrderType ot) {
      switch (ot) {
        case OrderType.EntryLimit:
          return LNG_OT_ENTRYLIMIT;
        case OrderType.EntryStop:
          return LNG_OT_ENTRYSTOP;
        case OrderType.Limit:
          return LNG_OT_LIMIT;
        case OrderType.Stop:
          return LNG_OT_STOP;
      }
      return "";
    }
    #endregion

    #region public void LoadLanguage()
    public void LoadLanguage() {
      LNG_TERMINAL = Dictionary.GetString(30, 1);
      LNG_T_OPERATE = Dictionary.GetString(30, 5);

      LNG_BS_BUY = Dictionary.GetString(30, 7);
      LNG_BS_SELL = Dictionary.GetString(30, 8);

      LNG_BTN_MODIFY = Dictionary.GetString(30, 9);
      LNG_BTN_DELETE = Dictionary.GetString(30, 10);
      LNG_BTN_CLOSE = Dictionary.GetString(30, 11);

      LNG_TBL_TYPEACCOUNT = Dictionary.GetString(30, 12);
      LNG_TBL_BALANCE = Dictionary.GetString(30, 13);
      LNG_TBL_CAPITAL = Dictionary.GetString(30, 14);
      LNG_TBL_USEDMARGIN = Dictionary.GetString(30, 15);
      LNG_TBL_FREEMARGIN = Dictionary.GetString(30, 16);
      LNG_TBL_PREMIUM = Dictionary.GetString(30, 17);
      LNG_TBL_COMMISION = Dictionary.GetString(30, 18);
      LNG_TBL_FEE = Dictionary.GetString(30, 19);
      LNG_TBL_NETPLALL = Dictionary.GetString(30, 20);

      LNG_TBL_ACCOUNT = Dictionary.GetString(30, 21);
      LNG_TBL_TRADE = Dictionary.GetString(30, 22);
      LNG_TBL_SYMBOL = Dictionary.GetString(30, 23);
      LNG_TBL_AMOUNT = Dictionary.GetString(30, 24);
      LNG_TBL_BUYSELL = Dictionary.GetString(30, 25);
      LNG_TBL_OPENRATE = Dictionary.GetString(30, 26);
      LNG_TBL_CLOSERATE = Dictionary.GetString(30, 27);
      LNG_TBL_STOP = Dictionary.GetString(30, 28);
      LNG_TBL_LIMIT = Dictionary.GetString(30, 29);
      LNG_TBL_TRAIL = Dictionary.GetString(30, 30);
      LNG_TBL_NETPOINT = Dictionary.GetString(30, 31);
      LNG_TBL_NETPIRCE = Dictionary.GetString(30, 32);
      LNG_TBL_OPENTIME = Dictionary.GetString(30, 111, "Open Time");
      LNG_TBL_CLOSETIME = Dictionary.GetString(30, 112, "Close Time");
      LNG_TBL_TYPE = Dictionary.GetString(30, 34);
      LNG_TBL_ORDER = Dictionary.GetString(30, 35);
      LNG_TBL_LOTS = Dictionary.GetString(30, 36);
      LNG_TBL_SLIPPAGE = Dictionary.GetString(30, 37);
      LNG_TBL_RATE = Dictionary.GetString(30, 38);
      LNG_DEPEND = Dictionary.GetString(30, 39);

      LNG_OT_INIT = Dictionary.GetString(30, 40);
      LNG_OT_REJECTINIT = Dictionary.GetString(30, 41);
      LNG_OT_CLOSE = Dictionary.GetString(30, 42);
      LNG_OT_REJECTCLOSE = Dictionary.GetString(30, 43);
      LNG_OT_ENTRYSTOP = Dictionary.GetString(30, 44);
      LNG_OT_ENTRYLIMIT = Dictionary.GetString(30, 45);
      LNG_OT_STOP = Dictionary.GetString(30, 46);
      LNG_OT_LIMIT = Dictionary.GetString(30, 47);
      LNG_OT_MARGIN = Dictionary.GetString(30, 48);
      LNG_OT_MINEQUITY = Dictionary.GetString(30, 49);
      LNG_OT_INITFAILED = Dictionary.GetString(30, 50);
      LNG_OT_ENTRYFAILED = Dictionary.GetString(30, 51);
      LNG_OT_STOPFAILED = Dictionary.GetString(30, 52);
      LNG_OT_LIMITFAILED = Dictionary.GetString(30, 53);

      LNG_HO_SYMBOL = Dictionary.GetString(30, 54);
      LNG_HO_BID = Dictionary.GetString(30, 55);
      LNG_HO_ASK = Dictionary.GetString(30, 56);
      LNG_HO_ADD = Dictionary.GetString(30, 57);
      LNG_HO_DELETE = Dictionary.GetString(30, 58);
      LNG_HO_SELL = Dictionary.GetString(30, 59);
      LNG_HO_BUY = Dictionary.GetString(30, 60);
      LNG_HO_SELLPRIVIEW = Dictionary.GetString(30, 61);
      LNG_HO_BUYPREVIEW = Dictionary.GetString(30, 62);
      LNG_HO_PENDINGORDER = Dictionary.GetString(30, 63);
      LNG_HO_LOTS = Dictionary.GetString(30, 64);
      LNG_HO_SLIPPAGE = Dictionary.GetString(30, 65);
      LNG_HO_STOP = Dictionary.GetString(30, 66);
      LNG_HO_LIMIT = Dictionary.GetString(30, 67);

      LNG_HO_RATE = Dictionary.GetString(30, 68);
      LNG_HO_NONORDER = Dictionary.GetString(30, 69);
      LNG_HO_LIMITORDER = Dictionary.GetString(30, 70);
      LNG_HO_STOPORDER = Dictionary.GetString(30, 71);

      LNG_CMD_OFFLINE = Dictionary.GetString(30, 72);
      LNG_CMD_LOGON_START = Dictionary.GetString(30, 73);
      LNG_CMD_LOGON_ONLINE = Dictionary.GetString(30, 74);
      LNG_CMD_LOADINGDATA = Dictionary.GetString(30, 75);
      LNG_CMD_LOGON_WAITING = Dictionary.GetString(30, 76);
      LNG_CMD_LOGON_ERROR = Dictionary.GetString(30, 77);

      LNG_CMD_UPDATESYMBOLHOLE = Dictionary.GetString(30, 78);
      LNG_CMD_UPDATESYMBOLHOLE_STOP = Dictionary.GetString(30, 79);
      LNG_CMD_UPDATESYMBOLHOLE_ERROR = Dictionary.GetString(30, 80);


      LNG_CMD_CHANGEENTRYORDER = Dictionary.GetString(30, 81);
      LNG_CMD_CHANGEENTRYORDER_STOP = Dictionary.GetString(30, 82);
      LNG_CMD_CHANGEENTRYORDER_ERROR = Dictionary.GetString(30, 83);

      LNG_CMD_CHANGE_SLT_ENTRYORDER = Dictionary.GetString(30, 84);
      LNG_CMD_CHANGE_SLT_ENTRYORDER_STOP = Dictionary.GetString(30, 85);
      LNG_CMD_CHANGE_SLT_ENTRYORDER_ERROR = Dictionary.GetString(30, 86);

      LNG_CMD_CHANGE_SLT_TRADE = Dictionary.GetString(30, 87);
      LNG_CMD_CHANGE_SLT_TRADE_STOP = Dictionary.GetString(30, 88);
      LNG_CMD_CHANGE_SLT_TRADE_ERROR = Dictionary.GetString(30, 89);

      LNG_CMD_CLOSETRADE = Dictionary.GetString(30, 90);
      LNG_CMD_CLOSETRADE_STOP = Dictionary.GetString(30, 91);
      LNG_CMD_CLOSETRADE_ERROR = Dictionary.GetString(30, 92);

      LNG_CMD_CREATEENTRYORDER = Dictionary.GetString(30, 93);
      LNG_CMD_CREATEENTRYORDER_STOP = Dictionary.GetString(30, 94);
      LNG_CMD_CREATEENTRYORDER_ERROR = Dictionary.GetString(30, 95);

      LNG_CMD_DELETEORDER = Dictionary.GetString(30, 96);
      LNG_CMD_DELETEORDER_STOP = Dictionary.GetString(30, 97);
      LNG_CMD_DELETEORDER_ERROR = Dictionary.GetString(30, 98);

      LNG_CMD_CREATETRADE = Dictionary.GetString(30, 99);
      LNG_CMD_CREATETRADE_STOP = Dictionary.GetString(30, 100);
      LNG_CMD_CREATETRADE_ERROR = Dictionary.GetString(30, 101);

      LNG_CMD_ACCEPTREJECTORDER = Dictionary.GetString(30, 102);
      LNG_CMD_ACCEPTREJECTORDER_STOP = Dictionary.GetString(30, 103);
      LNG_CMD_ACCEPTREJECTORDER_ERROR = Dictionary.GetString(30, 104);
    }
    #endregion
    #endregion
  }
  #region class SymbolUpdate
  class SymbolUpdate {
    private ISymbol _symbol;
    private bool _updateHole = false;

    public SymbolUpdate(ISymbol symbol) {
      _symbol = symbol;
    }

    public ISymbol Symbol {
      get { return this._symbol; }
    }

    #region public bool IsUpdateHole
    public bool IsUpdateHole {
      get { return this._updateHole; }
      set { this._updateHole = value; }
    }
    #endregion
  }
  #endregion

  #region enum SymbolUpdateStatus
  public enum SymbolUpdateStatus {
    /// <summary>
    /// История с дырками
    /// </summary>
    Hole,
    /// <summary>
    /// История без дырок
    /// </summary>
    Update,
    /// <summary>
    /// История в режиме ожидания прокачки дырок
    /// </summary>
    Wait,
    /// <summary>
    /// История в состоряние прокачки дырок
    /// </summary>
    Download
  }
  #endregion

}
