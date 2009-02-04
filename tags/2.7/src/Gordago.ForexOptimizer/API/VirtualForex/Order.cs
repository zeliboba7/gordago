/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {
  class Order : IOrder {

    private Account _account;
    private OnlineRate _onlieRate;
    private string _orderId, _tradeId;
    private TradeType _ttype;
    private OrderType _otype;
    private float _rate;
    private DateTime _time;
    private Order _stopOrder, _limitOrder;
    private int _lots;
    private string _comment;

    public Order(Account account, OnlineRate onlineRate, string orderId, TradeType ttype, OrderType otype, int lots, float rate, DateTime time, string comment) {
      _comment = comment;
      _account = account;
      _onlieRate = onlineRate;
      _orderId = orderId;
      _ttype = ttype;
      _otype = otype;
      _lots = lots;
      _rate = rate;
      _time = time;
    }

    #region public string Comment
    public string Comment {
      get { return this._comment; }
    }
    #endregion

    #region public IAccount Account
    public IAccount Account {
      get { return this._account; }
    }
    #endregion

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return this._onlieRate; }
    }
    #endregion

    #region public string OrderId
    public string OrderId {
      get { return _orderId; }
    }
    #endregion
    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _ttype; }
    }
    #endregion
    #region public OrderType OrderType
    public OrderType OrderType {
      get { return _otype; }
    }
    #endregion
    #region public int Lots
    public int Lots {
      get { return _lots; }
    }
    #endregion
    #region public float Rate
    public float Rate {
      get { return _rate; }
    }
    #endregion
    #region public DateTime Time
    public DateTime Time {
      get { return _time; }
    }
    #endregion
    #region public IOrder StopOrder
    public IOrder StopOrder {
      get { return _stopOrder; }
    }
    #endregion
    #region public IOrder LimitOrder
    public IOrder LimitOrder {
      get { return _limitOrder; }
    }
    #endregion
    #region public string TradeId
    public string TradeId {
      get { return _tradeId; }
    }
    #endregion

    #region internal void SetStopOrder(Order stopOrder)
    internal void SetStopOrder(Order stopOrder) {
      _stopOrder = stopOrder;
    }
    #endregion
    #region internal void SetLimitOrder(Order limitOrder)
    internal void SetLimitOrder(Order limitOrder) {
      _limitOrder = limitOrder;
    }
    #endregion
    #region internal void SetTradeId(string tradeId)
    internal void SetTradeId(string tradeId) {
      _tradeId = tradeId;
    }
    #endregion
    #region internal void SetLots(int lots)
    internal void SetLots(int lots) {
      _lots = lots;
      if (this.StopOrder != null) {
        (this.StopOrder as Order).SetLots(lots);
      }
      if (this.LimitOrder != null) {
        (this.LimitOrder as Order).SetLots(lots);
      }
    }
    #endregion
    #region internal void SetRate(float newRate)
    internal void SetRate(float newRate) {
      _rate = newRate;
    }
    #endregion

    #region public bool CheckOrderDoExecute(bool isUp)
    /// <summary>
    /// Проверка ордера на срабатывание
    /// </summary>
    /// <param name="isUp"></param>
    /// <returns></returns>
    public bool CheckOrderDoExecute(bool isUp) {
      float rate = _ttype == TradeType.Sell ? _onlieRate.SellRate : _onlieRate.BuyRate;

      if (isUp) {
        if (rate >= _rate)
          return true;
      } else {
        if (rate <= _rate)
          return true;
      }
      return false;
    }
    #endregion
  }

  class OrderList : IOrderList {
    private List<Order> _orders;

    public OrderList() {
      _orders = new List<Order>();
    }

    #region public int Count
    public int Count {
      get { return _orders.Count; }
    }
    #endregion

    #region public IOrder this[int index]
    public IOrder this[int index] {
      get { return _orders[index]; }
    }
    #endregion

    #region public IOrder GetOrder(string orderId)
    public IOrder GetOrder(string orderId) {
      for (int i = 0; i < this.Count; i++) {
        if (_orders[i].OrderId == orderId)
          return _orders[i];
      }
      return null;
    }
    #endregion

    #region public void Add(Order order)
    public void Add(Order order) {
      if (this.GetOrder(order.OrderId) != null)
        throw(new Exception("Error"));
      _orders.Add(order);
    }
    #endregion

    #region public void Remove(Order order)
    public void Remove(Order order) {
      _orders.Remove(order);
    }
    #endregion
  }
}
