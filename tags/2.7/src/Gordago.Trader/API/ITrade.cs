/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  public interface ITrade {
    IAccount Account { get;}
    IOnlineRate OnlineRate { get;}
    string TradeId { get;}
    string ParentOrderId { get; }
    
    int Lots { get;}
    TradeType TradeType { get;}

    float OpenRate { get;}
    DateTime OpenTime { get;}

    float CloseRate { get;}
    DateTime CloseTime { get;}

    string Comment { get;}

    #region float NetPL { get;} - Net Profit/Loss value
    /// <summary>
    /// Net Profit/Loss value
    /// </summary>
    float NetPL { get;}
    #endregion

    #region int NetPLPoint { get;} - Net Profit/Loss in Point
    /// <summary>
    /// Net Profit/Loss in Point
    /// </summary>
    int NetPLPoint { get;}
    #endregion

    #region float Commission { get;} - Коммиссия субброкера
    /// <summary>
    /// Коммиссия субброкера
    /// </summary>
    float Commission { get;}
    #endregion

    #region float Fee { get;} - Плата за каждый лот, снимаеться брокером
    /// <summary>
    /// Плата за каждый лот, снимаеться брокером
    /// </summary>
    float Fee { get;}
    #endregion

    #region float Premium { get;} - Премия. Начисление за то, что сделка остаеться на ночь
    /// <summary>
    /// Премия. Начисление за то, что сделка остаеться на ночь
    /// </summary>
    float Premium { get;}
    #endregion

    IOrder StopOrder { get;}
    IOrder LimitOrder { get;}
  }

  public interface ITradeList {
    int Count { get;}
    ITrade this[int index] { get;}
    ITrade GetTrade(string tradeId);
  }
}
