/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  #region public class BrokerCommandResult
  //public class BrokerCommandResult {
  //  private object _brokerObject;
  //  private BrokerError _error;
  //  private BrokerCommand _command;

  //  public BrokerCommandResult(BrokerCommand command) : this(command, null, null) { }
  //  public BrokerCommandResult(BrokerCommand command, BrokerError error) : this(command, error, null) { }
  //  public BrokerCommandResult(BrokerCommand command, BrokerError error, object brokerObject) {
  //    _command = command;
  //    _brokerObject = brokerObject;
  //    _error = error;
  //  }

  //  #region public BrokerError Error
  //  public BrokerError Error {
  //    get { return this._error; }
  //  }
  //  #endregion

  //  #region public object BrokerObject
  //  public object BrokerObject {
  //    get { return this._brokerObject; }
  //  }
  //  #endregion

  //  #region public BrokerCommand Command
  //  public BrokerCommand Command {
  //    get { return this._command; }
  //  }
  //  #endregion
  //}
  #endregion

  #region public enum BrokerCommandType
  public enum BrokerCommandType {
    Empty,
    Logon,
    Logoff,
    CreateTrade,
    CloseTrade,
    CreateOrder,
    DeleteOrder,
    ChangeOrder
  }
  #endregion

  #region public class BrokerCommandEventArgs : EventArgs
  //public class BrokerCommandEventArgs : EventArgs {
  //  private BrokerCommand _command;
  //  private BrokerCommandResult _error;

  //  public BrokerCommandEventArgs(BrokerCommand command)
  //    : this(command, null) {
  //  }

  //  public BrokerCommandEventArgs(BrokerCommand command, BrokerCommandResult error) {
  //    _command = command;
  //    _error = error;
  //  }

  //  #region public BrokerCommand Command
  //  public BrokerCommand Command {
  //    get { return this._command; }
  //  }
  //  #endregion

  //  #region public IBrokerError Error
  //  public BrokerCommandResult Error {
  //    get { return this._error; }
  //  }
  //  #endregion
  //}
  #endregion

  public abstract class BrokerCommand {}

  #region public class BrokerCommandLogon:BrokerCommand
  public class BrokerCommandLogon:BrokerCommand {
    private string _login;
    private string _password;
    private BrokerProxyInfo _proxy;
    private bool _demo;
    public BrokerCommandLogon(string login, string password, BrokerProxyInfo proxy, bool demo) {
      _login = login;
      _password = password;
      _proxy = proxy;
      _demo = demo;
    }

    #region public string Login
    public string Login {
      get { return this._login; }
    }
    #endregion

    #region public string Password
    public string Password {
      get { return this._password; }
    }
    #endregion

    #region public BrokerProxyInfo Proxy
    public BrokerProxyInfo Proxy {
      get { return this._proxy; }
    }
    #endregion

    #region public bool Demo
    public bool Demo {
      get { return this._demo; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerCommandLogoff : BrokerCommand
  public class BrokerCommandLogoff : BrokerCommand {
    public BrokerCommandLogoff() {
    }
  }
  #endregion

  #region public class BrokerCommandTradeClose : BrokerCommand
  public class BrokerCommandTradeClose : BrokerCommand {
    private ITrade _trade;
    private int _lots;
    private int _slippage;
    private string _comment;

    public BrokerCommandTradeClose(ITrade trade, int lots, int slippage, string comment) {
      _comment = comment;
      _trade = trade;
      _lots = lots;
      _slippage = slippage;
    }

    #region public ITrade Trade
    public ITrade Trade {
      get { return _trade; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return this._lots; }
    }
    #endregion

    #region public int Slippage
    public int Slippage {
      get { return this._slippage; }
    }
    #endregion

    #region public string Comment
    public string Comment {
      get { return this._comment; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerCommandTradeModify : BrokerCommand
  public class BrokerCommandTradeModify : BrokerCommand {
    private ITrade _trade;
    private float _stopRate, _limitRate;

    public BrokerCommandTradeModify(ITrade trade, float stopRate, float limitRate) {
      _trade = trade;
      _stopRate = stopRate;
      _limitRate = limitRate;
    }
    #region public ITrade Trade
    public ITrade Trade {
      get { return _trade; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return _stopRate; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerCommandTradeOpen : BrokerCommand
  public class BrokerCommandTradeOpen : BrokerCommand {
    private string _accountId;
    private IOnlineRate _onlineRate;
    private TradeType _tradeType;
    private int _lotCount;
    private int _slipPage;
    private float _stopRate;
    private float _limitRate;
    private string _comment;

    public BrokerCommandTradeOpen(string accountId, IOnlineRate onlineRate, TradeType tradeType,
      int lotCount, int slipPage, float stopRate, float limitRate, string comment) {
      _accountId = accountId;
      _onlineRate = onlineRate;
      _tradeType = tradeType;
      _lotCount = lotCount;
      _slipPage = slipPage;
      _stopRate = stopRate;
      _limitRate = limitRate;
      _comment = comment;
    }

    #region public string Comment
    public string Comment {
      get { return this._comment; }
    }
    #endregion

    #region public string AccountId
    public string AccountId {
      get { return this._accountId; }
    }
    #endregion

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return this._onlineRate; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return this._tradeType; }
    }
    #endregion

    #region public int LotCount
    public int LotCount {
      get { return this._lotCount; }
    }
    #endregion

    #region public int Slippage
    public int Slippage {
      get { return _slipPage; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return this._stopRate; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerCommandEntryOrderCreate : BrokerCommand
  public class BrokerCommandEntryOrderCreate : BrokerCommand {
    private OrderType _orderType;
    private string _accountId;
    private IOnlineRate _onlineRate;
    private int _lots;
    private TradeType _tradeType;
    private float _rate, _stopRate, _limitRate;
    private string _comment;

    public BrokerCommandEntryOrderCreate(string accountId, IOnlineRate onlineRate, OrderType orderType,
      TradeType tradeType, int lots, float orderRate, float stopRate, float limitRate, string comment) {
      _comment = comment;
      _orderType = orderType;
      _accountId = accountId;
      _onlineRate = onlineRate;
      _lots = lots;
      _tradeType = tradeType;
      _rate = orderRate;
      _stopRate = stopRate;
      _limitRate = limitRate;
    }

    #region public string Comment
    public string Comment {
      get { return this._comment; }
    }
    #endregion

    #region public OrderType OrderType
    public OrderType OrderType {
      get { return this._orderType; }
    }
    #endregion

    #region public string AccountId
    public string AccountId {
      get { return this._accountId; }
    }
    #endregion

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return this._onlineRate; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get { return this._lots; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return this._tradeType; }
    }
    #endregion

    #region public float Rate
    public float Rate {
      get { return this._rate; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return this._stopRate; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerCommandEntryOrderModify : BrokerCommand
  public class BrokerCommandEntryOrderModify : BrokerCommand {

    private IOrder _order;
    private float _newRate, _newStopRate, _newLimitRate;
    private int _newLots;

    public BrokerCommandEntryOrderModify(IOrder order, int newLots, float newRate, float newStopRate, float newLimitRate) {
      _newLots = newLots;
      _order = order;
      _newRate = newRate;
      _newStopRate = newStopRate;
      _newLimitRate = newLimitRate;
    }

    #region public IOrder Order
    public IOrder Order {
      get { return this._order; }
    }
    #endregion

    #region public int NewLots
    public int NewLots {
      get { return this._newLots; }
    }
    #endregion

    #region public float NewRate
    public float NewRate {
      get { return this._newRate; }
    }
    #endregion

    #region public float NewStopRate
    public float NewStopRate {
      get { return this._newStopRate; }
    }
    #endregion

    #region public float NewLimitRate
    public float NewLimitRate {
      get { return this._newLimitRate; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerCommandEntryOrderDelete:BrokerCommand
  public class BrokerCommandEntryOrderDelete:BrokerCommand {
    private IOrder _order;
    private string _comment;

    public BrokerCommandEntryOrderDelete(IOrder order, string comment) {
      _order = order;
      _comment = comment;  
    }
    #region public IOrder Order
    public IOrder Order {
      get { return this._order; }
    }
    #endregion

    #region public string Comment
    public string Comment {
      get { return this._comment; }
    }
    #endregion
  }
  #endregion
}
