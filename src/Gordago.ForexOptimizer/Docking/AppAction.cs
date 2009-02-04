/**
* @version $Id: AppAction.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;


  #region interface IAppAction
  interface IAppAction {
    AppAction Action { get;}
    bool Enabled { get;set;}
  }
  #endregion

  enum AppAction {
    FileNew,
    FileOpen,
    FileSave,
    FileSaveAs,
    FileSaveAll,
    FileClose,
    FileRecent,
    CheckForUpdates,
    ViewLanguages,
    ViewSymbols,
    ViewIndicators,
    ViewProperties,
    AppExit,
    AppAbout,
    ChartCandleSticks,
    ChartBarChart,
    ChartLineChart,
    ChartAutoScroll,
    ChartChartShift,
    ChartGrid,
    ChartPeriodSeparators,
    ChartZoomIn,
    ChartZoomOut,
    ChartSaveAsPicture,
    ChartSaveAsReport,
    ChartTimeFrames,
    ChartTemplate,
    ChartTemplateSave,
    ChartTemplateLoad,
    ChartTemplateRemove,
    BuildProject
  }
}
