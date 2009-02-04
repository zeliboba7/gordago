/**
* @version $Id: FigureEventArgs.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;

  public delegate void FigureEventHandler(object sender, FigureEventArgs e);

  public class FigureEventArgs:EventArgs {
    private readonly Figure _figure;
    public FigureEventArgs(Figure figure)
      : base() {
      _figure = figure;
    }

    #region public Figure Figure
    public Figure Figure {
      get { return this._figure; }
    }
    #endregion
  }
}
