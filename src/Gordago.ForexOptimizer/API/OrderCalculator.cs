/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  class OrderCalculator {

    private TradeType _tradeType;
    private IOnlineRate _onlineRate;
    private bool _error;
    private float _rate, _stopRate, _limitRate;
    private int _decdig;
    private OrderType _orderType;

    public OrderCalculator(TradeType tradeType) {
      _tradeType = tradeType;
    }

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return this._onlineRate; }
    }
    #endregion

    #region public float Rate
    public float Rate {
      get { return this._rate; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return this._stopRate; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion

    #region public bool Error
    public bool Error {
      get { return this._error; }
    }
    #endregion

    #region public OrderType OrderType
    public OrderType OrderType {
      get { return this._orderType; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return _decdig; }
    }
    #endregion

    public void SetValues(IOnlineRate onlineRate, float rate, int stop, int limit) {
      _onlineRate = onlineRate;
      int dig = onlineRate.Symbol.DecimalDigits;
      _decdig = dig;
      float point = onlineRate.Symbol.Point;

      float bid = onlineRate.SellRate;
      float ask = onlineRate.BuyRate;

      _rate = rate;
      _stopRate = float.NaN;
      _limitRate = float.NaN;
      _error = false;
      if (_tradeType == TradeType.Sell) {
        if (_rate > bid) {
          _orderType = OrderType.Limit;
        } else if (_rate < bid) {
          _orderType = OrderType.Stop;
        } else {
          _error = true;
        }
      } else {
        if (_rate < ask) {
          _orderType = OrderType.Limit;
        } else if (_rate > ask) {
          _orderType = OrderType.Stop;
        } else {
          _error = true;
        }
      }

      _stopRate = float.NaN;
      _limitRate = float.NaN;

      if (stop > 0) {
        float sl = stop * point;
        _stopRate = _tradeType == TradeType.Sell ? _rate + sl : _rate - sl;
      }

      if (limit > 0) {
        float tp = limit * point;
        _limitRate = _tradeType == TradeType.Sell ? _rate - tp : _rate + tp;
      }

    }
  }
}
