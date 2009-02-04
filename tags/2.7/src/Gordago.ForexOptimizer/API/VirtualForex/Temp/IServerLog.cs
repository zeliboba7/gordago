/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  public interface IServerLog {
    int Count { get;}
    IServerLogRecord this[int index] { get;}
  }
  

  public interface IServerLogRecord {
    ServerCommand Command { get;}
    float Balance { get;}
    ISymbol Symbol { get;}
    string TradeId { get; }
    string OrderId { get;}
    TradeType TradeType { get;}
    OrderType OrderType { get;}
    TradeCloseType CloseAt { get;}
    int Lots { get;}
    float Rate { get;}
    float Stop { get;}
    float Limit { get;}
    float NetPL { get;}
    DateTime Time { get;}
  }
}
