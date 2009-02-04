/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {
  class OnlineRate:IOnlineRate {

    private ISymbol _symbol;
    private int _lotSize;
    private float _spread = 0;
    private float _pipCost = 1;

    private OrderList _ordersUp;

    private OrderList _ordersDown;

    private float _sellRate, _buyRate, _lastSellRate, _lastBuyRate;
    private VirtualBroker _broker;

    public OnlineRate( VirtualBroker broker, ISymbol symbol, int lotSize) {
      _broker = broker;
      _symbol = symbol;
      _lotSize = lotSize;

      _ordersUp = new OrderList();
      _ordersDown = new OrderList();
    }

    #region public OrderList OrdersUp
    /// <summary>
    /// ќрдера, которые провер€ютс€ когда цена идет вверх
    /// </summary>
    public OrderList OrdersUp {
      get { return this._ordersUp; }
    }
    #endregion

    #region public OrderList OrdersDown
    /// <summary>
    /// ќрдера, которые провер€ютс€ когда цена идет вниз
    /// </summary>
    public OrderList OrdersDown {
      get { return this._ordersDown; }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return _symbol; }
    }
    #endregion

    #region public int LotSize
    public int LotSize {
      get { return _lotSize; }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get {
        if (_symbol.Ticks.Count == 0)
          return _broker.Time;
        return new DateTime(_symbol.Ticks.Current.Time);
      }
    }
    #endregion

    #region public float PipCost
    public float PipCost {
      get { return _pipCost; }
    }
    #endregion

    #region public float SellRate
    public float SellRate {
      get {
        //if (_symbol.Ticks.Count == 0)
        //  return 0;
        //return _symbol.Ticks.Current.Price;
        return _sellRate;
      }
    }
    #endregion

    #region public float BuyRate
    public float BuyRate {
      get { return _buyRate; }
    }
    #endregion

    #region public float LastSellRate
    public float LastSellRate {
      get {
        //if (_symbol.Ticks.Count == 0)
        //  return 0;
        //if (_symbol.Ticks.Count == 1)
        //  return this.SellRate;
        //return _symbol.Ticks[_symbol.Ticks.Count - 2].Price;
        return _lastSellRate;
      }
    }
    #endregion

    #region public float LastBuyRate
    public float LastBuyRate {
      get {
        //return this.LastSellRate + _spread;
        return _lastBuyRate;
      }
    }
    #endregion

    #region public bool Active
    public bool Active {
      get {
        if (this._symbol.Ticks.Count == 0)
          return false;
        return true;
      }
    }
    #endregion

    #region public void SetPipCost(float pipcost)
    public void SetPipCost(float pipcost) {
      _pipCost = pipcost;
    }
    #endregion

    #region public void SetSpread(int spread)
    public void SetSpread(int spread) {
      _spread = spread * _symbol.Point;
    }
    #endregion

    #region public void SetRate(Tick tick)
    public void SetRate(Tick tick) {
      _lastSellRate = _sellRate;
      _lastBuyRate = _buyRate;
      _sellRate = tick.Price;
      _buyRate = _sellRate + _spread;
    }
    #endregion
  }

  #region class OnlineRateList : IOnlineRateList
  class OnlineRateList : IOnlineRateList {

    private List<OnlineRate> _onlineRates;
    private int _lotSize = 0;
    private VirtualBroker _broker;

    public OnlineRateList(VirtualBroker broker) {
      _broker = broker;
      _lotSize = broker.Settings.LotSize;
      _onlineRates = new List<OnlineRate>();
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
    
    public bool AddSymbol(ISymbol symbol) {

      for (int i = 0; i < _onlineRates.Count; i++) {
        if (_onlineRates[i].Symbol.Name == symbol.Name)
          return false;
      }

      OnlineRate onlineRate = new OnlineRate(_broker, symbol, _lotSize);
      _onlineRates.Add(onlineRate);
      ViTickList viTicks = new ViTickList(_broker, symbol.Ticks, _broker.Time);
      symbol.Ticks = viTicks;
      viTicks.SetOnlineRate(onlineRate);

      return true;
    }
  }
  #endregion
}
