/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  public interface ITrader {
    #region DateTime Time { get; }
    /// <summary>
    /// Время сервера GMT+0
    /// </summary>
    DateTime Time { get; }
    #endregion

    IAccountList Accounts { get;}
    ITradeList Trades { get;}
    IOrderList Orders { get;}
    IOnlineRateList OnlineRates { get;}
    IClosedTradeList ClosedTrades { get;}

    BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate, string comment);
    BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage, float stopRate, float limitRate);
    BrokerResultTrade TradeOpen(string accountId, string symbolName, TradeType tradeType, int lots, int slippage);
    BrokerResultTrade TradeModify(string tradeId, float stopRate, float limitRate);

    BrokerResult TradeClose(string tradeId, int lots, int slippage, string comment);
    BrokerResult TradeClose(string tradeId, int lots, int slippage);

    BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate, string comment);
    BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate, float stopRate, float limitRate);
    BrokerResultOrder OrderCreate(string accountId, string symbolName, TradeType tradeType, int lots, float rate);
    BrokerResultOrder OrderModify(string orderId, int lots, float newRate, float newStopRate, float newLimitRate);
    BrokerResult OrderDelete(string orderId, string comment);
    BrokerResult OrderDelete(string orderId);
  }

  public interface IBroker : ITrader {

    BrokerConnectionStatus ConnectionStatus { get;}

    BrokerResult Logon(string userName, string password, BrokerProxyInfo proxy, bool demo);
    BrokerResult Logoff();
    BrokerResultTickHistory GetTickHistory(IOnlineRate onlineRate, DateTime time1, DateTime time2);
  }

}
