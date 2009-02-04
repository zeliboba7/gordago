using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;

namespace IFXMarkets {
  class TradeList : ITradeList {

    private List<Trade> _trades;
    private IFXMarketsBroker _broker;

    public TradeList(IFXMarketsBroker broker) {
      _broker = broker;
      _trades = new List<Trade>();
      for (int i = 0; i < broker.FxTraderApi.Trades.Count; i++) {
        this.Add(broker.FxTraderApi.Trades.get_Trade(i));
      }
    }

    #region public int Count
    public int Count {
      get { return _trades.Count; }
    }
    #endregion

    #region public ITrade GetTrade(string tradeId)
    public ITrade GetTrade(string tradeId) {
      for (int i = 0; i < _trades.Count; i++) {
        if (_trades[i].TradeId == tradeId)
          return _trades[i];
      }
      return null;
    }
    #endregion

    #region public ITrade this[int index]
    public ITrade this[int index] {
      get { return _trades[index]; }
    }
    #endregion

    #region public Trade Add(FxTrade fxTrade)
    public Trade Add(FxTrade fxTrade) {
      Trade trade = new Trade(_broker, fxTrade);
      _trades.Add(trade);
      return trade;
    }
    #endregion

    #region public Trade Remove(FxTrade fxTrade)
    public Trade Remove(FxTrade fxTrade) {
      Trade trade = this.GetTrade(fxTrade.TradeId) as Trade;
      if (trade == null)
        return null;
      _trades.Remove(trade);
      return trade;
    }
    #endregion

    #region public Trade Update(FxTrade fxTrade)
    public Trade Update(FxTrade fxTrade) {
      Trade trade = this.GetTrade(fxTrade.TradeId) as Trade;
      if (trade == null)
        return null;
      trade.Update(fxTrade);
      return trade;
    }
    #endregion

    #region public void Update()
    public void Update() {

      for (int i = 0; i < this._broker.FxTraderApi.Trades.Count; i++) {
        _trades[i].Update(this._broker.FxTraderApi.Trades.get_Trade(i));
      }
    }
    #endregion
  }
}
