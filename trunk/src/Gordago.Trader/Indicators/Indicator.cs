/**
* @version $Id: Indicator.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using Gordago.Trader.Charting;
  using System.Reflection;
  using System.ComponentModel;
  using System.Drawing;

  public abstract class Indicator {

    public event ChartPaintEventHandler Paint;

    #region internal void CmPaint(ChartGraphics g)
    internal void CmPaint(ChartGraphics g) {
      this.OnPaint(g);
    }
    #endregion

    #region protected virtual void OnPaint(ChartGraphics g)
    protected virtual void OnPaint(ChartGraphics g) { }
    #endregion
  }

  
}
