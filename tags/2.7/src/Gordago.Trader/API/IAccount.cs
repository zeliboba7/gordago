/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {

  public interface IAccount {

    ITradeList Trades { get;}
    IOrderList Orders { get;}

    bool Active { get; }
    string AccountId { get;}
    float Commission { get;}
    #region float Balance { get;} - Баланс
    /// <summary>
    /// Баланс
    /// </summary>
    float Balance { get;}
    #endregion
    #region float Equity { get;} - Депозит (Капитал)
    /// <summary>
    /// Депозит (Капитал)
    /// </summary>
    float Equity { get;}
    #endregion
    #region float UsedMargin { get;} - Использованно маржи
    /// <summary>
    /// Использованно маржи
    /// </summary>
    float UsedMargin { get;}
    #endregion
    #region float UsableMargin { get;} - Свободно маржи
    /// <summary>
    /// Свободно маржи
    /// </summary>
    float UsableMargin { get;}
    #endregion
    #region float NetPL { get; } - Плавающий убыток
    /// <summary>
    /// Плавающий убыток
    /// </summary>
    float NetPL { get; }
    #endregion
    #region int Lots { get;} - Кол-во лотов
    /// <summary>
    /// Кол-во лотов
    /// </summary>
    int Lots { get;}
    #endregion
    float Premium { get;}

    string MoneyOwner { get;}
  }

  public interface IAccountList {
    int Count { get;}
    IAccount this[int index] { get;}
    IAccount GetAccount(string accountId);
  }

}
