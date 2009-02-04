/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  public interface IOrder { 
    IAccount Account { get;}
    IOnlineRate OnlineRate { get;}
    string OrderId { get;}
    string TradeId { get;}

    TradeType TradeType { get;}
    OrderType OrderType { get;}
    int Lots { get;}
    float Rate { get;}
    DateTime Time { get;}
    IOrder StopOrder { get;}
    IOrder LimitOrder { get;}

    string Comment { get;}
  }

  public interface IOrderList {
    int Count { get;}
    IOrder this[int index] { get;}
    IOrder GetOrder(string orderId);
  }
}
