/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  #region public enum TradeType
  /// <summary>
  /// Тип позиции
  /// </summary>
  public enum TradeType {
    /// <summary>
    /// Продажа
    /// </summary>
    Sell,
    /// <summary>
    /// Покупка
    /// </summary>
    Buy
  }
  #endregion

  #region public enum OrderType
  public enum OrderType {
    Stop,
    Limit,
    EntryStop,
    EntryLimit
  }
  #endregion

  //#region public enum CreateEntryOrderType
  //public enum CreateEntryOrderType {
  //  SellStop,
  //  SellLimit,
  //  BuyStop,
  //  BuyLimit
  //}
  //#endregion

  #region public enum ServerCommand
  public enum ServerCommand {
    TradeOpen,
    TradeModify,
    TradeClose,
    EntryOrderCreate,
    EntryOrderModify,
    EntryOrderDelete
  }
  #endregion

  public enum BrokerConnectionStatus {
    Offline,
    Online,
    LoadingData,
    WaitingForConnection
  }
}
