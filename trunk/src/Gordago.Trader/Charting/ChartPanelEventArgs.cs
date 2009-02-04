/**
* @version $Id: ChartPanelEventArgs.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Trader.Charting {
  public class ChartPanelEventArgs: EventArgs {

    private ChartPanel _chartPanel;

    public ChartPanelEventArgs(ChartPanel chartPanel)
      : base() {
      _chartPanel = chartPanel;
    }

    #region public ChartPanel ChartPanel
    public ChartPanel ChartPanel {
      get { return this._chartPanel; }
    }
    #endregion
  }
}
