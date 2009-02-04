using System;
using System.Collections.Generic;
using System.Text;
using Gordago.API;
using FxComApiTrader;

namespace IFXMarkets {
  class Order:IOrder {
    private IFXMarketsBroker _broker;
    private Account _account;
    private OnlineRate _onlineRate;
    private FxOrder _fxOrder;

    private OrderType _orderType;
    private TradeType _tradeType;
    private Order _limitOrder, _stopOrder;

    private DateTime _time;

    private float _rate;
    private string _orderId, _tradeId;
    private int _lots;

    private string _comment = "";

		public Order(IFXMarketsBroker broker, FxOrder order) {
      _fxOrder = order;
      _broker = broker;
      _account = (Account)broker.Accounts.GetAccount(order.AccountId);
      _onlineRate = (_broker.OnlineRates as OnlineRateList).GetOnlineRateFromPairId(order.PairId);
      this.Update(order);
    }

    #region public FxOrder FxOrder
    public FxOrder FxOrder {
      get { return this._fxOrder; }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
			get {
				return _time;
			}
		}
		#endregion

		#region public float Rate 
		public float Rate {
			get {
				return _rate;
			}
		}
		#endregion

		#region public string OrderId 
		public string OrderId {
      get { return _orderId; }
		}
		#endregion

    #region public int Lots
    public int Lots {
      get { return _lots; }
    }
    #endregion

    #region public IOrder StopOrder
    public IOrder StopOrder{
			get{return this._stopOrder;}
    }
    #endregion

		#region public string TradeId 
		public string TradeId {
      get { return _tradeId; }
		}
		#endregion

    #region public IAccount Account
    public IAccount Account {
      get { return _account; }
    }
    #endregion

    #region public IOrder LimitOrder
    public IOrder LimitOrder {
      get { return _limitOrder; }
    }
    #endregion

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return _onlineRate; }
    }
    #endregion

    #region public TradeType TradeType
    public TradeType TradeType {
      get { return _tradeType; }
    }
    #endregion

    #region public OrderType OrderType
    public OrderType OrderType {
      get { return _orderType; }
    }
    #endregion

    public void Update(FxOrder fxOrder) {
      _fxOrder = fxOrder;
      _time = _broker.GetGmtTime(_fxOrder.Time);
      _rate = Convert.ToSingle(_fxOrder.Rate);
      _orderId = _fxOrder.OrderId;
      _lots = _fxOrder.Amount / _onlineRate.LotSize;
      _tradeId = _fxOrder.TradeId;
      _tradeType = IFXMarketsBroker.GetTradeType(_fxOrder.BuySell);


      bool findSL = false;

      Order oldStopOrder = _stopOrder;
      Order oldLimitOrder = _limitOrder;

      _stopOrder = null;
      _limitOrder = null;
      switch (_fxOrder.OrderType) {
        case FxOrderType.ot_EntryLimit:
          _orderType = OrderType.EntryLimit;
          findSL = true;
          break;
        case FxOrderType.ot_EntryStop:
          _orderType = OrderType.EntryStop;
          findSL = true;
          break;
        case FxOrderType.ot_Limit:
          _orderType = OrderType.Limit;
          break;
        case FxOrderType.ot_Stop:
          _orderType = OrderType.Stop;
          break;
        default:
          throw new Exception("Unknow order type");
      }
      if (!findSL)
        return;
      if (_broker.Orders == null)
        return;
      for (int i = 0; i < _broker.Orders.Count; i++) {
        Order order = _broker.Orders[i] as Order;
        if (order.TradeId == this.OrderId) {
          if (order.OrderType == OrderType.Limit) {
            _limitOrder = order;
          } else if (order.OrderType == OrderType.Stop) {
            _stopOrder = order;
          }
        }
      }
      if (_limitOrder != oldLimitOrder || _stopOrder != oldStopOrder) {
        _broker.OnOrderUpdateEvents(this);
      }
    }

    #region public string Comment
    public string Comment {
      get { return _comment; }
    }
    #endregion
  }
}
