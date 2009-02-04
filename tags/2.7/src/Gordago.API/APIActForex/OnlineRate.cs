using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using Gordago;
using FxComApiTrader;

namespace IFXMarkets {
  class OnlineRate:IOnlineRate {

    private ISymbol _symbol;

    private float _sellRate, _buyRate, _pipCost, _lastSellRate, _lastBuyRate;
    private int _lotSize;
    private DateTime _time;
    private string _pairId;
    private IFXMarketsBroker _broker;
    private bool _active;
    private FxPair _fxPair;

    private int _sessionId = 0;

    public OnlineRate(IFXMarketsBroker broker, FxPair fxpair) {
      _fxPair = fxpair;
      _broker = (IFXMarketsBroker)broker;
      _pairId = fxpair.PairId;
      ISymbol symbol = broker.Symbols.GetSymbol(fxpair.Name);
      if (symbol == null) {
        string dell = Convert.ToString(Convert.ToInt32(1 / fxpair.Multiplier));
        symbol = broker.Symbols.Add(fxpair.Name, dell.Length - 1);
      }
      _symbol = symbol;
      this.UpdateRate(fxpair);
    }

    public override string ToString() {
      string str = string.Format("Name={0}, PipCost={1}, LotSize={2}, Multi={3}, MultiStep={4}",
        _symbol.Name,
        _fxPair.PipCost,
        _lotSize,
        _fxPair.Multiplier,
        _fxPair.MultiplierStep
        );
      return str;
    }

    #region public int SessionId
    public int SessionId {
      get { return this._sessionId; }
    }
    #endregion

    #region public FxPair FxPair
    public FxPair FxPair {
      get { return _fxPair; }
    }
    #endregion

    #region public void UpdateRate(FxPair fxpair)
    public void UpdateRate(FxPair fxPair) {
      _sessionId++;
      _fxPair = fxPair;
      _active = fxPair.Active == FxLogic.lg_True ? true : false;
      _sellRate = Convert.ToSingle(fxPair.SellRate);
      _buyRate = Convert.ToSingle(fxPair.BuyRate);
      _lotSize = fxPair.LotSize;
      _pipCost = Convert.ToSingle(fxPair.PipCost);
      _time = _broker.GetGmtTime(fxPair.Time);
      _lastSellRate = Convert.ToSingle(fxPair.LastSellRate);
      _lastBuyRate = Convert.ToSingle(fxPair.LastBuyRate);
    }
    #endregion

    #region public string PairId
    public string PairId {
      get { return this._pairId; }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion

    #region public int LotSize
    public int LotSize {
      get {
        return _lotSize;
      }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get {
        return _time;
      }
    }
    #endregion

    #region public float PipCost
    public float PipCost {
      get {
        return _pipCost;
      }
    }
    #endregion

    #region public float SellRate
    public float SellRate {
      get {
        return _sellRate;
      }
    }
    #endregion

    #region public float BuyRate
    public float BuyRate {
      get {
        return _buyRate;
      }
    }
    #endregion

    #region public float LastSellRate
    public float LastSellRate {
      get { return _lastSellRate; }
    }
    #endregion

    #region public float LastBuyRate
    public float LastBuyRate {
      get { return _lastBuyRate; }
    }
    #endregion

    #region public bool Active
    public bool Active {
      get { return _active; }
    }

    #endregion
  }

}
