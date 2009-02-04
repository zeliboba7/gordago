/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis.Chart;
using Gordago.Analysis;
using System.Windows.Forms;

namespace Gordago {

  /// <summary>
  /// Главное окно программы
  /// </summary>
  public interface IMainForm {
    /// <summary>
    /// Событие на создание окна графика
    /// </summary>
    event EventHandler ChartCreating;
    /// <summary>
    /// Событие на активацию окна графика
    /// </summary>
    event EventHandler ChartActivate;

    /// <summary>
    /// Идентификатор языка: "eng" - аглийский, "rus" - русский
    /// </summary>
    string Lang { get;}

    /// <summary>
    /// Список инструментов
    /// </summary>
    ISymbolList Symbols { get;}

    /// <summary>
    /// Менеджер управления индикаторами
    /// </summary>
    IndicatorManager IndicatorManager { get;}

    /// <summary>
    /// Менеджер управления периодами
    /// </summary>
    TimeFrameManager TimeFrameManager { get;}

    /// <summary>
    /// Активный график
    /// </summary>
    IChartManager ActiveChart { get;}

    /// <summary>
    /// Менеджер управления графическими фигурами.
    /// </summary>
    ChartObjectManager ChartObjectManager { get;}

    /// <summary>
    /// Регистратор панелей графика
    /// </summary>
    ChartPanelManager ChartPanelManager { get;}

    /// <summary>
    /// Курсор
    /// </summary>
    Cursor Cursor { get;set;}

    /// <summary>
    /// Получить список графиков
    /// </summary>
    /// <returns>Список графиков</returns>
    IChartManager[] GetCharts();
  }
}
