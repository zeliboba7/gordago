/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  class TradeCalculator {

    private TradeType _tradeType;
    private float _rate, _stopRate, _limitRate;
    private int _decdig = 4;
    private IOnlineRate _onlineRate;
    
    public TradeCalculator(TradeType tradeType) {
      _tradeType = tradeType;
    }

    #region public IOnlineRate OnlineRate
    public IOnlineRate OnlineRate {
      get { return this._onlineRate; }
    }
    #endregion

    #region public float Rate
    public float Rate {
      get { return _rate; }
    }
    #endregion

    #region public float StopRate
    public float StopRate {
      get { return _stopRate; }
    }
    #endregion

    #region public float LimitRate
    public float LimitRate {
      get { return this._limitRate; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return this._decdig; }
    }
    #endregion

    public void SetValues(IOnlineRate onlineRate, int stop, int limit) {
      _onlineRate = onlineRate;

      float bid = onlineRate.SellRate;
      float ask = onlineRate.BuyRate;
      float point = onlineRate.Symbol.Point;
      int dig = onlineRate.Symbol.DecimalDigits;
      _decdig = dig;
      _rate = _tradeType == TradeType.Sell ? bid : ask;
      _stopRate = float.NaN;
      _limitRate = float.NaN;

      if (stop > 0) {
        float sl = stop * point;
        _stopRate = _tradeType == TradeType.Sell ? ask + sl : bid - sl;
      }

      if (limit > 0) {
        float tp = limit * point;
        _limitRate = _tradeType == TradeType.Sell ? bid - tp : ask + tp;
      }
    }
  }
}
