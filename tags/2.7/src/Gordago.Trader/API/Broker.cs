/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  /// <summary>
  /// Абстрактный класс брокера позволяет вести обмен 
  /// данными с сервером брокера
  /// </summary>
  public abstract class Broker {

    public event EventHandler ConnectioStatusChanged;
    public event EventHandler<BrokerAccountsEventArgs> AccountsChanged;
    public event EventHandler<BrokerMarginCallEventArgs> MarginCallCreated;
    public event EventHandler<BrokerTradesEventArgs> TradesChanged;
    public event EventHandler<BrokerOrdersEventArgs> OrdersChanged;
    public event EventHandler<BrokerOnlineRatesEventArgs> OnlineRatesChanged;

    private ISymbolList _symbols;
    private BrokerJournal _journal;
    private object[] _parameters;

    public Broker(ISymbolList symbols) {
      _symbols = symbols;
      _journal = new BrokerJournal(this);
      _parameters = new object[0];
    }

    public Broker(ISymbolList symbols, params object[] parameters) {
      _symbols = symbols;
      _journal = new BrokerJournal(this);
      _parameters = parameters;
    }

    #region protected object[] Parameters
    protected object[] Parameters {
      get { return this._parameters; }
    }
    #endregion

    #region public virtual ISymbolList Symbols
    public virtual ISymbolList Symbols {
      get { return this._symbols; }
    }
    #endregion

    #region public BrokerJournal Journal
    public BrokerJournal Journal {
      get { return this._journal; }
    }
    #endregion

    #region public static int CalculatePoint(float price1, float price2, int decDig, bool abs)
    public static int CalculatePoint(float price1, float price2, int decDig, bool abs) {
      if (float.IsNaN(price1) || float.IsNaN(price2))
        return 0;
      int dil = SymbolManager.GetDelimiter(decDig);
      if (abs)
        return Convert.ToInt32(Math.Abs(price1 - price2) * dil);
      else
        return Convert.ToInt32((price1 - price2) * dil);
    }
    #endregion

    #region public static float CalculatePrice(float price, int point, int decDig) 
    public static float CalculatePrice(float price, int point, int decDig) {
      float onepoint = (float)point / SymbolManager.GetDelimiter(decDig);
      return price + onepoint;
    }
    #endregion

    #region protected void OnConnectionStatusChanged()
    protected virtual void OnConnectionStatusChanged() {
      _journal.BrokerConnectionStatusChanged();
      if (this.ConnectioStatusChanged != null)
        this.ConnectioStatusChanged(this, new EventArgs());
    }
    #endregion

    #region protected void OnOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    protected virtual void OnOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      if (this.OnlineRatesChanged != null) {
        this.OnlineRatesChanged(this, be);
      }
    }
    #endregion

    #region protected void OnAccountsChanged(BrokerAccountsEventArgs be)
    protected virtual void OnAccountsChanged(BrokerAccountsEventArgs be) {
      _journal.BrokerAccountsChanged(be);
      if (this.AccountsChanged != null)
        this.AccountsChanged(this, be);
    }
    #endregion

    #region protected void OnOrdersChanged(BrokerOrdersEventArgs be)
    protected virtual void OnOrdersChanged(BrokerOrdersEventArgs be) {
      _journal.BrokerOrdersChanged(be);
      if (this.OrdersChanged != null)
        this.OrdersChanged(this, be);
    }
    #endregion

    #region protected void OnTradesChanged(BrokerTradesEventArgs be)
    protected virtual void OnTradesChanged(BrokerTradesEventArgs be) {
      _journal.BrokerTradesChanged(be);
      if (this.TradesChanged != null)
        this.TradesChanged(this, be);
    }
    #endregion

    #region protected void OnMarginCallCreated(BrokerMarginCallEventArgs be)
    protected virtual void OnMarginCallCreated(BrokerMarginCallEventArgs be) {
      this._journal.BrokerMarginCallCreated(be);
      if (this.MarginCallCreated != null)
        this.MarginCallCreated(this, be);
    }
    #endregion

    #region public void Dispose()
    public void Dispose() {
      this.OnDispose();
    }
    #endregion

    #region protected virtual void OnDispose()
    protected virtual void OnDispose() { }
    #endregion

    #region public void Comment(string message)
    public void Comment(string message) {
      _journal.BrokerComment(message);
    }
    #endregion
  }

  #region public enum BrokerMessageType
  public enum BrokerMessageType {
    Add,
    Update,
    Delete
  }
  #endregion

  #region public class BrokerAccountsEventArgs : EventArgs
  public class BrokerAccountsEventArgs : EventArgs {
    private IAccount _account;
    private BrokerMessageType _messageType;
    public BrokerAccountsEventArgs(IAccount account, BrokerMessageType messageType){
      _account = account;
      _messageType = messageType;
    }
    #region public IAccount Account
    public IAccount Account {
      get { return this._account; }
    }
    #endregion
    #region public BrokerMessageType MessageType
    public BrokerMessageType MessageType {
      get { return this._messageType; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerOrdersEventArgs : EventArgs
  public class BrokerOrdersEventArgs : EventArgs {
    private IOrder _order;
    private BrokerMessageType _messageType;
    public BrokerOrdersEventArgs(IOrder order, BrokerMessageType messageType) {
      _order = order;
      _messageType = messageType;
    }

    #region public IOrder Order
    public IOrder Order {
      get { return this._order; }
    }
    #endregion

    #region public BrokerMessageType MessageType
    public BrokerMessageType MessageType {
      get { return this._messageType; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerTradesEventArgs : EventArgs
  public class BrokerTradesEventArgs : EventArgs {
    private ITrade _trade;
    private BrokerMessageType _messageType;
    public BrokerTradesEventArgs(ITrade trade, BrokerMessageType messageType) {
      _trade = trade;
      _messageType = messageType;
    }

    #region public ITrade Trade
    public ITrade Trade {
      get { return this._trade; }
    }
    #endregion

    #region public BrokerMessageType MessageType
    public BrokerMessageType MessageType {
      get { return this._messageType; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerOnlineRatesEventArgs : EventArgs
  public class BrokerOnlineRatesEventArgs : EventArgs {
    private IOnlineRate _onlineRate;
    private BrokerMessageType _messageType;
    public BrokerOnlineRatesEventArgs(IOnlineRate onlineRate, BrokerMessageType messageType) {
      _onlineRate = onlineRate;
      _messageType = messageType;
    }

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return this._onlineRate; }
    }
    #endregion

    #region public BrokerMessageType MessageType
    public BrokerMessageType MessageType {
      get { return this._messageType; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerMarginCallEventArgs : EventArgs
  public class BrokerMarginCallEventArgs : EventArgs {
    private string _message;
    public BrokerMarginCallEventArgs(string message) {
      _message = message;
    }
    #region public string Message
    public string Message {
      get { return this._message; }
    }
    #endregion
  }
  #endregion

  #region public class BrokerProxyInfo
  public class BrokerProxyInfo {
    private string _serverName;
    private int _port;
    private string _userName;
    private string _password;

    public BrokerProxyInfo(string server, int port, string userName, string password) {
      _serverName = server;
      _port = port;
      _userName = userName;
      _password = password;
    }

    #region public string Server
    public string Server {
      get { return this._serverName; }
    }
    #endregion

    #region public int Port
    public int Port {
      get { return this._port; }
    }
    #endregion
    
    #region public string UserName
    public string UserName {
      get { return this._userName; }
    }
    #endregion

    #region public string Password
    public string Password {
      get { return this._password; }
    }
    #endregion
  }
  #endregion
}
