using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;

namespace IFXMarkets {
  class Account:IAccount {
    private IFxAccount _fxaccount;
    private string _accountId;

    private IFXMarketsBroker _broker;
    private int _savedSession;

    private float _fee, _netPL, _balance;
    private float _premium, _commission, _usedMargin, _usableMargin, _equity;
    private bool _active;
    private int _lots;

    private string _moneyOwner;

    public Account(IFXMarketsBroker broker, IFxAccount fxAccount) {
      _broker = broker;
      this.SetAccount(fxAccount);
    }

    #region public float Premium
    public float Premium {
      get {
        this.CalculateValue();
        return _premium;
      }
    }
    #endregion

    #region public float Commission
    public float Commission {
      get {
        this.CalculateValue();
        return _commission;
      }
    }
    #endregion

    #region public string AccountId
    public string AccountId {
      get {
        return _accountId;
      }
    }
    #endregion

    #region public float UsedMargin
    public float UsedMargin {
      get {
        this.CalculateValue();
        return _usedMargin;
      }
    }
    #endregion

    #region public float UsableMargin
    public float UsableMargin {
      get {
        this.CalculateValue();
        return _usableMargin;
      }
    }
    #endregion

    #region public float Equity
    public float Equity {
      get {
        this.CalculateValue();
        return _equity;
      }
    }
    #endregion

    #region public float Fee
    public float Fee {
      get {
        this.CalculateValue();
        return _fee;
      }
    }
    #endregion

    #region public float NetPL
    public float NetPL {
      get {
        this.CalculateValue();
        return _netPL;
      }
    }
    #endregion

    #region public float Balance
    public float Balance {
      get {
        this.CalculateValue();
        return _balance;
      }
    }
    #endregion

    #region public bool Active
    public bool Active {
      get {
        this.CalculateValue();
        return _active;
      }
    }
    #endregion

    #region public int Lots
    public int Lots {
      get {
        this.CalculateValue();

        return _lots;
      }
    }
    #endregion

    #region public float Commision
    public float Commision {
      get {
        this.CalculateValue();

        return _commission;
      }
    }
    #endregion

    #region public ITradeList Trades
    public ITradeList Trades {
      get { throw new Exception("The method or operation is not implemented."); }
    }
    #endregion

    #region public IOrderList Orders
    public IOrderList Orders {
      get { throw new Exception("The method or operation is not implemented."); }
    }
    #endregion

    #region public string MoneyOwner
    public string MoneyOwner {
      get {
        this.CalculateValue();
        return _moneyOwner;
      }
    }
    #endregion

    #region public void SetAccount(IFxAccount fxAccount)
    public void SetAccount(IFxAccount fxAccount) {
      _fxaccount = fxAccount;
      _accountId = fxAccount.AccountId;
    }
    #endregion

    private void CalculateValue() {
      if (_savedSession == _broker.SessionId)
        return;
      _savedSession = _broker.SessionId;

      _balance = Convert.ToSingle(_fxaccount.Balance);
      _netPL = Convert.ToSingle(_fxaccount.NetPL);
      _fee = Convert.ToSingle(_fxaccount.Fee);
      
      _premium = Convert.ToSingle(_fxaccount.Premium);
      _commission = Convert.ToSingle(_fxaccount.Commission);
      _usedMargin = Convert.ToSingle(_fxaccount.UsedMargin);
      _usableMargin = Convert.ToSingle(_fxaccount.UsableMargin);
      _equity = Convert.ToSingle(_fxaccount.Equity);
      _active = FxComApiTrader.FxLogic.lg_True == _fxaccount.Active;
      _lots = _fxaccount.OpenPositions;
      _commission = Convert.ToSingle(_fxaccount.Commission);
      _moneyOwner = _fxaccount.MoneyOwner; 
    }
  }
}
