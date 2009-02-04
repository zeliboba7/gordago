/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  public interface IClosedTrade {
    string AccountId { get;}
    string SymbolName { get;}
    string TradeId { get;}
    string ParentOrderId { get;}

    float OpenRate { get;}
    DateTime OpenTime { get;}

    float CloseRate { get;}
    DateTime CloseTime { get;}

    int Amount { get;}
    TradeType TradeType { get;}

    string OpenComment { get;}
    string CloseComment { get;}

    #region float NetPL { get;} - Net Profit/Loss value
    /// <summary>
    /// Net Profit/Loss value
    /// </summary>
    float NetPL { get;}
    #endregion

    #region float Commission { get;} - ��������� ����������
    /// <summary>
    /// ��������� ����������
    /// </summary>
    float Commission { get;}
    #endregion

    #region float Fee { get;} - ����� �� ������ ���, ��������� ��������
    /// <summary>
    /// ����� �� ������ ���, ���������� ��������
    /// </summary>
    float Fee { get;}
    #endregion

    #region float Premium { get;} - ������. ���������� �� ��, ��� ������ ��������� �� ����
    /// <summary>
    /// ������. ���������� �� ��, ��� ������ ��������� �� ����
    /// </summary>
    float Premium { get;}
    #endregion

    float StopOrderRate { get;}
    float LimitOrderRate { get;}
  }

  public interface IClosedTradeList {
    int Count { get;}
    IClosedTrade this[int index] { get; }
    // IClosedTrade GetClosedTrade(string closeTradeId);
  }
}
