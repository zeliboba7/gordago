/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Gordago.Analysis.Chart {
  public interface IChartBox {

    event PaintEventHandler Paint;

    int Width { get;}
    int Height { get;}
    int DecimalDigits { get;}
    float PercentHeight { get;set;}

    Size HorizontalScaleSize { get;}
    Size VerticalScaleSize { get;}

    IChartManager ChartManager { get;}
    ChartFigureList Figures { get;}

    int GetY(float val);

    int GetX(int barIndex);
    int GetXFromAnalyzer(int barIndex);

    int GetBarIndex(int x);
    DateTime GetTime(int barIndex);
    float GetPrice(int y);

    COPoint GetChartPoint(Point p);
    

  }
}
