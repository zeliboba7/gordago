/**
* @version $Id: ChartControlAction.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Trader.Charting {
  /// <summary>
  /// Defines values representing <see cref="ChartControl"/> events.
  /// </summary>
  public enum ChartControlAction {
    Selected,
    Deselected
  }
}
