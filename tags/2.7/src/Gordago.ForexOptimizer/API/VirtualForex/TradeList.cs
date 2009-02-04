/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.VirtualForex {
  class TradeList : ITradeList {

    private List<Trade> _trades;

    public TradeList() {
      _trades = new List<Trade>();
    }

    #region public int Count
    public int Count {
      get { return _trades.Count; }
    }
    #endregion

    #region public ITrade this[int index]
    public ITrade this[int index] {
      get { return _trades[index]; }
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

    #region public void Add(Trade trade)
    public void Add(Trade trade) {
      if (this.GetTrade(trade.TradeId) != null)
        throw(new Exception("Error"));

      _trades.Add(trade);
    }
    #endregion

    #region public void Remove(Trade trade)
    public void Remove(Trade trade) {
      _trades.Remove(trade);
    }
    #endregion

  }
}
