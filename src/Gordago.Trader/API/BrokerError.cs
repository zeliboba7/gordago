/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  #region public class BrokerResult
  public class BrokerResult {
    private BrokerError _error;

    public BrokerResult(BrokerError error) {
      _error = error;
    }

    #region public BrokerError Error
    public BrokerError Error {
      get { return _error; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerResultTrade : BrokerResult
  public class BrokerResultTrade : BrokerResult {
    private ITrade _trade;

    public BrokerResultTrade(ITrade trade, BrokerError error):base(error) {
      _trade = trade;
    }
    #region public ITrade Trade
    public ITrade Trade {
      get { return this._trade; }
    }
    #endregion 
  }
  #endregion

  #region public class BrokerResultClosedTrades : BrokerResult
  public class BrokerResultClosedTrades : BrokerResult {
    private IClosedTrade[] _trades;

    public BrokerResultClosedTrades(IClosedTrade[] trades, BrokerError error):base(error) {
      _trades = trades;
    }

    #region public IClosedTrade[] ClosedTrades
    public IClosedTrade[] ClosedTrades {
      get { return this._trades; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerResultOrder : BrokerResult
  public class BrokerResultOrder : BrokerResult {
    private IOrder _order;
    public BrokerResultOrder(IOrder order, BrokerError error) : base(error) {
      _order = order;
    }
    #region public IOrder Order
    public IOrder Order {
      get { return this._order; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerResultTickHistory : BrokerResult
  public class BrokerResultTickHistory : BrokerResult {
    private Tick[] _ticks;

    public BrokerResultTickHistory(Tick[] ticks, BrokerError error) : base(error) {
      _ticks = ticks;
    }

    #region public Tick[] Ticks
    public Tick[] Ticks {
      get { return _ticks; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerError
  public class BrokerError {
    private BrokerErrorType _errorType;
    private string _message;

    public BrokerError(BrokerErrorType errorType) : this(errorType, "") { }

    public BrokerError(BrokerErrorType errorType, string message) {
      _message = message;
      _errorType = errorType;
    }

    #region public BrokerErrorType ErrorType
    public BrokerErrorType ErrorType {
      get { return this._errorType; }
    }
    #endregion

    #region public string Message
    public string Message {
      get { return this._message; }
    }
    #endregion
  }
  #endregion

  #region public enum BrokerErrorType
  public enum BrokerErrorType {
    Unknow,
    AccountNotFound,
    OnlineRateNotFound,
    TradeNotFound,
    OrderNotFound,
    StopOrderRateError,
    LimitOrderRateError,
    PendingOrderRateError,
    CountLotIsZero,
    LogonUnknow,
    LogonBadUserNameOrPassword,
    Logoff,
    TradeOpen,
    TradeModify,
    TradeClose,
    OrderCreate,
    OrderModify,
    OrderDelete
  }
  #endregion
}
