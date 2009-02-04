using System;
using System.Collections.Generic;
using System.Text;
using FxComApiTrader;
using Gordago.API;

namespace IFXMarkets {
  class OrderList : IOrderList {

    private List<Order> _orders;
    private IFXMarketsBroker _broker;

    public OrderList(IFXMarketsBroker broker) {
      _broker = broker;
      _orders = new List<Order>();

      for (int i = 0; i < broker.FxTraderApi.Orders.Count; i++) {
        FxOrder fxOrder = broker.FxTraderApi.Orders.get_Order(i);
        _orders.Add(new Order(broker, fxOrder));
      }
      for (int i = 0; i < _orders.Count; i++) {
        _orders[i].Update(_orders[i].FxOrder);
      }
    }

    #region public int Count
    public int Count {
      get { return _orders.Count; }
    }
    #endregion

    #region public IOrder GetOrder(string orderId)
    public IOrder GetOrder(string orderId) {
      for (int i = 0; i < _orders.Count; i++) {
        if (_orders[i].OrderId == orderId)
          return _orders[i];
      }
      return null;
    }
    #endregion

    #region public IOrder this[int index]
    public IOrder this[int index] {
      get { return _orders[index]; }
    }
    #endregion

    #region public Order Add(FxOrder fxOrder)
    public Order Add(FxOrder fxOrder) {
      switch (fxOrder.OrderType) {
        case FxOrderType.ot_Close:
        case FxOrderType.ot_EntryFailed:
        case FxOrderType.ot_Init:
        case FxOrderType.ot_InitFailed:
        case FxOrderType.ot_LimitFailed:
        case FxOrderType.ot_Margin:
        case FxOrderType.ot_MinEquity:
        case FxOrderType.ot_RejectClose:
        case FxOrderType.ot_RejectInit:
        case FxOrderType.ot_StopFailed:
          return null;
        case FxOrderType.ot_EntryLimit:
        case FxOrderType.ot_EntryStop:
        case FxOrderType.ot_Stop:
        case FxOrderType.ot_Limit:
          break;
      }
      Order order = new Order(_broker, fxOrder);
      this._orders.Add(order);
      return order;
    }
    #endregion

    #region public Order Remove(FxOrder fxOrder)
    public Order Remove(FxOrder fxOrder) {
      Order order = this.GetOrder(fxOrder.OrderId) as Order;
      if (order == null)
        return null;
      this._orders.Remove(order);
      return order;
    }
    #endregion

    #region public Order Update(FxOrder fxOrder)
    public Order Update(FxOrder fxOrder) {
      Order order = this.GetOrder(fxOrder.OrderId) as Order;
      if (order == null)
        return null;
      order.Update(fxOrder);
      return order;
    }
    #endregion

    #region public void Update()
    public void Update() {
      for (int i = 0; i < _orders.Count; i++) {
        _orders[i].Update(_orders[i].FxOrder);
      }
    }
    #endregion
  }
}
