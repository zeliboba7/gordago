/**
* @version $Id: AppMenuItem.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  class AppMenuItem : ToolStripMenuItem, IAppAction {
    private readonly AppAction _action;

    public AppMenuItem(AppAction appAction) {
      _action = appAction;
      switch (appAction) {
        #region Menu/Over
        case AppAction.AppExit:
          this.Text = Global.Languages["Menu"]["Exit"]; ;
          break;
        case AppAction.AppAbout:
          this.Text = Global.Languages["Menu"]["About"];
          break;
        case AppAction.CheckForUpdates:
          this.Text = Global.Languages["Menu"]["Check For Updates"];
          break;
        #endregion
        #region Menu/View
        case AppAction.ViewSymbols:
          this.Text = Global.Languages["Menu/View"]["Symbols"];
          break;
        case AppAction.ViewIndicators:
          this.Text = Global.Languages["Menu/View"]["Indicators"];
          break;
        case AppAction.ViewProperties:
          this.Text = Global.Languages["Menu/View"]["Properties"];
          break;
        #endregion
        #region Menu/Chart
        case AppAction.ChartCandleSticks:
          this.Text = Global.Languages["Menu/Chart"]["Candlesticks"];
          break;
        case AppAction.ChartBarChart:
          this.Text = Global.Languages["Menu/Chart"]["Bar Chart"];
          break;
        case AppAction.ChartLineChart:
          this.Text = Global.Languages["Menu/Chart"]["Line Chart"];
          break;
        case AppAction.ChartAutoScroll:
          this.Text = Global.Languages["Menu/Chart"]["Auto Scroll"];
          break;
        case AppAction.ChartChartShift:
          this.Text = Global.Languages["Menu/Chart"]["Chart Shift"];
          break;
        case AppAction.ChartGrid:
          this.Text = Global.Languages["Menu/Chart"]["Grid"];
          break;
        case AppAction.ChartPeriodSeparators:
          this.Text = Global.Languages["Menu/Chart"]["Period Separators"];
          break;
        case AppAction.ChartZoomIn:
          this.Text = Global.Languages["Menu/Chart"]["Zoom In"];
          break;
        case AppAction.ChartZoomOut:
          this.Text = Global.Languages["Menu/Chart"]["Zoom Out"];
          break;
        case AppAction.ChartSaveAsPicture:
          this.Text = Global.Languages["Menu/Chart"]["Save As Picture..."];
          break;
        case AppAction.ChartSaveAsReport:
          this.Text = Global.Languages["Menu/Chart"]["Save As Report..."];
          break;
        case AppAction.ChartTimeFrames:
          this.Text = Global.Languages["Menu/Chart"]["Time Frame"];
          break;
        case AppAction.ChartTemplate:
          this.Text = Global.Languages["Menu/Chart"]["Template"];
          break;
        case AppAction.ChartTemplateSave:
          this.Text = Global.Languages["Menu/Chart"]["Save template..."];
          break;
        case AppAction.ChartTemplateLoad:
          this.Text = Global.Languages["Menu/Chart"]["Load template..."];
          break;
        case AppAction.ChartTemplateRemove:
          this.Text = Global.Languages["Menu/Chart"]["Remove"];
          break;
        #endregion
        #region Menu/File
        case AppAction.FileNew:
          this.Text = Global.Languages["Menu/File"]["New..."];
          this.Image = global::Gordago.FO.Properties.Resources.ProjectNew;
          break;
        case AppAction.FileOpen:
          this.Text = Global.Languages["Menu/File"]["Open"];
          this.Image = global::Gordago.FO.Properties.Resources.Open;
          break;
        case AppAction.FileSave:
          this.Text = Global.Languages["Menu/File"]["Save"];
          this.Image = global::Gordago.FO.Properties.Resources.Save;
          break;
        case AppAction.FileSaveAs:
          this.Text = Global.Languages["Menu/File"]["Save As..."];
          break;
        case AppAction.FileSaveAll:
          this.Text = Global.Languages["Menu/File"]["Save All"];
          this.Image = global::Gordago.FO.Properties.Resources.SaveAll;
          break;
        case AppAction.FileClose:
          this.Text = Global.Languages["Menu/File"]["Close"];
          break;
        case AppAction.FileRecent:
          this.Text = Global.Languages["Menu/File"]["Recent"];
          break;
        #endregion
        #region Menu/Build
        case AppAction.BuildProject:
          this.Text = Global.Languages["Menu/Build"]["Build Instrument"];
          break;
        #endregion
      }
    }

    #region public AppAction Action
    public AppAction Action {
      get { return _action; }
    }
    #endregion
  }
}
