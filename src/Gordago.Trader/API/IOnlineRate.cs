/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API {
  
  /// <summary>
  /// ��������� ���� �������/������� ����������� �����������
  /// </summary>
  public interface IOnlineRate {
    ISymbol Symbol { get;}
    int LotSize { get;}
    DateTime Time { get;}
    float PipCost { get;}
    float SellRate { get;}
    float BuyRate { get;}
    float LastSellRate { get;}
    float LastBuyRate { get;}
    bool Active { get;}
  }
  
  public interface IOnlineRateList {
    int Count { get;}
    IOnlineRate this[int index] { get;}
    
    /// <summary>
    /// �������� ������� ���� 
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    IOnlineRate GetOnlineRate(string symbolName);
  }
}
