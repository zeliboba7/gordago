using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;

namespace IFXMarkets {
  class AccountList : IAccountList {
    private List<IAccount> _accounts;
    private IFXMarketsBroker _broker;

    public AccountList(IFXMarketsBroker broker) {
      _broker = broker;
      _accounts = new List<IAccount>();
      for (int i = 0; i < broker.FxTraderApi.Accounts.Count; i++) {
        _accounts.Add(new Account(broker, broker.FxTraderApi.Accounts.get_Account(i)));
      }
    }

    #region public int Count
    public int Count {
      get {
        return this._accounts.Count;
      }
    }
    #endregion

    #region public IAccount this[int index]
    public IAccount this[int index] {
      get { return this._accounts[index]; }
    }
    #endregion

    #region public IAccount GetAccount(string accountId)
    public IAccount GetAccount(string accountId) {
      for (int i = 0; i < this._accounts.Count; i++) {
        if (_accounts[i].AccountId == accountId)
          return _accounts[i];
      }
      return null;
    }
    #endregion

    #region public Account Update(FxAccount fxaccount, FxMessageType mtype)
    public Account Update(FxAccount fxaccount, FxMessageType mtype) {
      Account account = this.GetAccount(fxaccount.AccountId) as Account;
      if (account == null) {
        _accounts.Add(new Account(_broker, fxaccount));
      } else {
        account.SetAccount(fxaccount);
      }
      return account;
    }
    #endregion
  }
}
