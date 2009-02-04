using System;
using System.Collections.Generic;
using System.Text;
using FxComApiTrader;
using Gordago.API;

namespace IFXMarkets {

  class OnlineRateList : IOnlineRateList {

    private IFXMarketsBroker _broker;
    private List<OnlineRate> _onlineRates;

    public OnlineRateList(IFXMarketsBroker broker) {
      _broker = broker;
      _onlineRates = new List<OnlineRate>();
      for (int i = 0; i < broker.FxTraderApi.Pairs.Count; i++) {
        OnlineRate onlineRate = new OnlineRate(broker, broker.FxTraderApi.Pairs.get_Pair(i));
        _onlineRates.Add(onlineRate);
#if DEBUG
        System.Diagnostics.Debug.WriteLine(onlineRate);
#endif
      }
    }

    #region public int Count
    public int Count {
      get { return _onlineRates.Count; }
    }
    #endregion

    #region public IOnlineRate this[int index]
    public IOnlineRate this[int index] {
      get { return _onlineRates[index]; }
    }
    #endregion

    #region public IOnlineRate GetOnlineRate(string symbolName)
    public IOnlineRate GetOnlineRate(string symbolName) {
      for (int i = 0; i < _onlineRates.Count; i++) {
        if (_onlineRates[i].Symbol.Name == symbolName)
          return _onlineRates[i];
      }
      return null;
    }
    #endregion

    #region public OnlineRate GetOnlineRateFromPairId(string pairId)
    public OnlineRate GetOnlineRateFromPairId(string pairId) {
      for (int i = 0; i < _onlineRates.Count; i++) {
        if (_onlineRates[i].PairId == pairId)
          return _onlineRates[i];
      }
      return null;
    }
    #endregion

    #region public void Update(FxPair pair, FxMessageType mtype)
    public void Update(FxPair pair, FxMessageType mtype) {
      OnlineRate onlineRate = (OnlineRate)this.GetOnlineRate(pair.Name);
      if (onlineRate == null) {
        onlineRate = new OnlineRate(_broker, pair);
        _onlineRates.Add(onlineRate);
      }
      onlineRate.UpdateRate(pair);
    }
    #endregion
  }
}
