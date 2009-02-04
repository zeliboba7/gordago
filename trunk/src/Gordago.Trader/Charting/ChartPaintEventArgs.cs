/**
* @version $Id: ChartPaintEventArgs.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public delegate void ChartPaintEventHandler(object sender, ChartPaintEventArgs cpe);

  public class ChartPaintEventArgs:EventArgs {

    private readonly ChartGraphics _g;

    public ChartPaintEventArgs(ChartGraphics g) {
      _g = g;
    }

    #region public ChartGraphics ChartGraphics
    public ChartGraphics ChartGraphics {
      get { return this._g; }
    }
    #endregion
  } 
}
