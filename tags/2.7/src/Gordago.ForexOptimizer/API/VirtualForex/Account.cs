/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {
  class Account:IAccount {

    private string _accountId;
    private bool _active = true;
    private int _lots;
    private float _commision, _balance, _equity, _usedMargin, _usableMargin;
    private float _netPL, _premium;

    private TradeList _trades;
    private OrderList _orders;

    private VirtualBroker _broker;

    private long _savedSessionId = -955;

    /// <summary>
    /// Маржинальное плечо, если 1:100, то значит 100
    /// </summary>
    private int _leverage = 100;

    public Account(VirtualBroker broker, string accountId) {
      _broker = broker;
      _accountId = accountId;
      _leverage = broker.Settings.LeverageValue;
      _balance = broker.Settings.Deposit;
      
      _trades = new TradeList();
      _orders = new OrderList();
      this.CalculateValues();
    }

    #region public string AccountId
    public string AccountId {
      get { return _accountId; }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get {
        this.CalculateValues();
        return _commision;
      }
    }
    #endregion

    #region public float Balance
    public float Balance {
      get {
        this.CalculateValues();
        return _balance; 
      }
    }
    #endregion

    #region public float Equity
    public float Equity {
      get {
        this.CalculateValues();
        return _equity; 
      }
    }
    #endregion

    #region public float UsedMargin
    public float UsedMargin {
      get {
        this.CalculateValues();
        return _usedMargin; 
      }
    }
    #endregion 

    #region public float UsableMargin
    public float UsableMargin {
      get {
        this.CalculateValues();
        return _usableMargin; 
      }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get {
        this.CalculateValues();
        return _netPL; 
      }
    }
    #endregion

    #region public bool Active
    public bool Active {
      get { return _active; }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get {
        this.CalculateValues();
        return _lots; 
      }
    }
    #endregion

    #region public float Premium
    public float Premium {
      get {
        CalculateValues();
        return _premium; 
      }
    }
    #endregion

    #region public float Commision
    public float Commision {
      get {
        CalculateValues();
        return _commision; 
      }
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

    #region private void CalculateValues()
    private void CalculateValues() {
      if (_savedSessionId == _broker.SessionId)
        return;
      _savedSessionId = _broker.SessionId;

      _lots = 0;
      _netPL = 0;
      _commision = 0;
      _premium = 0;

      Dictionary<string, LotsSymbol> sLots = new Dictionary<string, LotsSymbol>();
      
      for (int i = 0; i < _trades.Count; i++) {
        Trade trade = (Trade)_trades[i];
        _lots += trade.Lots;
        _netPL += trade.NetPL;
        _commision += trade.Commission;
        _premium += trade.Premium;

        /* Определение затраченной маржи */
        string sname = trade.OnlineRate.Symbol.Name;
        LotsSymbol ls = null;

        if (!sLots.ContainsKey(sname)) {
          ls = new LotsSymbol(0, 0);
          sLots.Add(sname, ls);
        } else { ls = sLots[sname]; }

        if (trade.TradeType == TradeType.Sell)
          ls.AddSell(trade.Lots);
        else
          ls.AddBuy(trade.Lots);
      }

      Dictionary<string, LotsSymbol>.ValueCollection svals = sLots.Values;
      _usedMargin = 0;
      foreach (LotsSymbol ls in svals) {
        _usedMargin += ls.GetLots() * _leverage;
      }

      _equity = _balance + _netPL;
      _usableMargin = _equity - _usedMargin;
    }
    #endregion

    #region public void CloseTrade(ClosedTrade ctrade)
    public void CloseTrade(ClosedTrade ctrade) {
      _balance += ctrade.NetPL;
    }
    #endregion

    #region public string MoneyOwner
    public string MoneyOwner {
      get { return "Demo VS"; }
    }
    #endregion
  }

  #region class LotsSymbol
  class LotsSymbol {
    private int _sellLots;
    private int _buyLots;
    public LotsSymbol(int sellLots, int buyLots) {
      _sellLots = sellLots;
      _buyLots = buyLots;
    }

    public int SellLots {
      get { return _sellLots; }
    }

    public int BuyLots {
      get { return this._buyLots; }
    }

    public void AddSell(int lots) {
      _sellLots += lots;
    }

    public void AddBuy(int lots) {
      _buyLots += lots;
    }

    public int GetLots() {
      return _sellLots + _buyLots - Math.Min(_sellLots, _buyLots);
    }
  }
  #endregion

  #region class AccountList : IAccountList
  class AccountList : IAccountList {

    private List<Account> _accounts;

    public AccountList(VirtualBroker broker, int count) {
      _accounts = new List<Account>();
      for (int i = 1; i <= count; i++) {
        _accounts.Add(new Account(broker, i.ToString()));
      }
    }

    #region public int Count
    public int Count {
      get { return _accounts.Count;}
    }
    #endregion

    #region public IAccount this[int index]
    public IAccount this[int index] {
      get { return _accounts[index]; }
    }
    #endregion

    #region public IAccount GetAccount(string accountId)
    public IAccount GetAccount(string accountId) {
      for (int i = 0; i < _accounts.Count; i++) {
        if (accountId == _accounts[i].AccountId)
          return _accounts[i];
      }
      return null;
    }
    #endregion
  }
  #endregion
}
