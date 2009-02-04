/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Chart {

  public interface IChartManager {
    
    event EventHandler PositionChanged;
    event EventHandler ZoomChanged;
    event EventHandler BarTypeChanged;
    event EventHandler TimeFrameChanged;
    event EventHandler GridVisibleChanged;
    event EventHandler ChartShiftChanged;
    event EventHandler ChartBoxAdding;
    event EventHandler ChartBoxRemoving;
    event EventHandler PeriodSeparatorsChanged;

    IChartBox[] ChartBoxes { get;}
    ChartFigure SelectedFigure { get;}
    ChartFigure SelectedFigureMouse { get;}

    #region bool ChartShift { get;}
    /// <summary>
    /// Отступ от конца графика
    /// </summary>
    bool ChartShift { get;}
    #endregion

    int Position { get;}
    ChartZoom Zoom { get;}
    ChartFigureBarType BarType { get;}
    bool GridVisible { get;}
    bool PeriodSeparators { get;}

    IBarList Bars { get;}

    ChartStyle Style { get;}
    ISymbol Symbol { get;}

    ChartAnalyzer ChartAnalyzer { get;}

    void SetTimeFrame(TimeFrame tf);
    void SetStyle(ChartStyle style);
    void SetPosition(int position);
    void SetZoom(ChartZoom zoom);
    void SetBarType(ChartFigureBarType bartype);
    void SetGridVisible(bool value);
    void SetChartShift(bool chartshift);

    int GetBarIndex(DateTime time);

    void Refresh();

    void SetSymbol(ISymbol symbol);
  }
}
