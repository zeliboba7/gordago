﻿/**
* @version $Id: FxMAMethod.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.ComponentModel;

  public enum FxMAMethod {
//    [DisplayName("Simple")]
    Simple,
//    [DisplayName("Exponential")]
    Exponential,
//    [DisplayName("Linear Weighted")]
    LinearWeighted,
//    [DisplayName("Smoothed")]
    Smoothed
  }
}
