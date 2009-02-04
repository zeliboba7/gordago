﻿/**
* @version $Id: ChartFigureStyle.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;

  [Flags]
  public enum ChartFigureStyle {
    UserMouse = 2,
    Selectable = 4,
    CalculateScale = 8
  }
}
