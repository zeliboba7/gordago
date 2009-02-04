/**
* @version $Id: ChartBoxEventArgs.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Trader.Charting {
  public delegate void ChartControlEventHandler(object sender, ChartBoxEventArgs e);

  /// <summary>
  /// Provides data for the Selected and Deselected events of a ChartControl control. 
  /// </summary>
  public class ChartBoxEventArgs:EventArgs {

    private ChartBox _chartBox;
    private int _chartBoxIndex;
    private ChartControlAction _action;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartBoxEventArgs"/> class. 
    /// </summary>
    /// <param name="chartBox">The <see cref="ChartBox"/> the event is occurring for.</param>
    /// <param name="chartBoxIndex">The zero-based index of chartBox in the <see cref="ChartBox"/> collection.</param>
    /// <param name="action">One of the <see cref="ChartControlAction"/> values.</param>
    public ChartBoxEventArgs(ChartBox chartBox, int chartBoxIndex, ChartControlAction action):base() {
      _chartBox = chartBox;
      _chartBoxIndex = chartBoxIndex;
      _action = action;
    }

    public ChartBox ChartBox {
      get { return _chartBox; }
    }

    public int ChartBoxIndex {
      get { return this._chartBoxIndex; }
    }

    /// <summary>
    /// Gets a value indicating which event is occurring.
    /// </summary>
    /// <returns>One of the <see cref="ChartControlAction"/> values.</returns>
    public ChartControlAction Action {
      get { return this._action; }
    }
  }
}
