/**
* @version $Id: ChartManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Trader;
using System.IO;

  class ChartManager {

    #region class TimeFrameMenuItem : ToolStripMenuItem
    class TimeFrameMenuItem : ToolStripMenuItem {

      private readonly TimeFrame _tf;

      public TimeFrameMenuItem(TimeFrame tf):base(tf.Name) {
        _tf = tf;
      }

      #region public TimeFrame TimeFrame
      public TimeFrame TimeFrame {
        get { return _tf; }
      }
      #endregion
    }
    #endregion

    private ToolStripMenuItem _chartMenuItem;
    private ChartDocument _currentChartDocument;

    public ChartManager() {
      Global.DockManager.ActiveDocumentChanged += new EventHandler(DockManager_ActiveDocumentChanged);
    }

    #region public ToolStripMenuItem ViewMenuItem
    public ToolStripMenuItem ViewMenuItem {
      get { return _chartMenuItem; }
      set {
        if (_chartMenuItem == value)
          return;

        _chartMenuItem = value;

        if (value == null)
          return;

        value.Text = Global.Languages["Menu/Chart"]["Chart"];
        value.DropDownItems.Clear();
        value.DropDownItems.AddRange(this.CreateChartMenuItems());
        value.Visible = false;
        _chartMenuItem.DropDownOpened += new EventHandler(_chartMenuItem_DropDownOpened);
      }
    }
    #endregion

    #region private void _chartMenuItem_DropDownOpened(object sender, EventArgs e)
    private void _chartMenuItem_DropDownOpened(object sender, EventArgs e) {
      this.UpdateMenuItems((sender as ToolStripMenuItem).DropDownItems);
    }
    #endregion

    #region public void UpdateMenuItems(ToolStripItemCollection items)
    public void UpdateMenuItems(ToolStripItemCollection items){
      foreach (ToolStripItem item in items) {
        ToolStripMenuItem mitem = item as ToolStripMenuItem;
        if (mitem == null)
          continue;
        this.UpdateMenuItems(mitem.DropDownItems);

        mitem.Checked = false;

        if (mitem is AppMenuItem) {
          AppMenuItem appItem = mitem as AppMenuItem;
          switch (appItem.Action) {
            case AppAction.ChartCandleSticks:
              break;
            case AppAction.ChartBarChart:
              break;
            case AppAction.ChartLineChart:
              break;
            case AppAction.ChartAutoScroll:
              break;
            case AppAction.ChartChartShift:
              break;
            case AppAction.ChartGrid:
              break;
            case AppAction.ChartPeriodSeparators:
              break;
            case AppAction.ChartZoomIn:
              break;
            case AppAction.ChartZoomOut:
              break;
          }
        } else if (mitem is TimeFrameMenuItem) {
          TimeFrame tf = _currentChartDocument.ChartControl.TimeFrame;
          TimeFrameMenuItem timeFrameMenuItem = mitem as TimeFrameMenuItem;
          timeFrameMenuItem.Checked = tf.Second == timeFrameMenuItem.TimeFrame.Second;
        }
      }
    }
    #endregion

    #region public ToolStripItem[] CreateChartMenuItems()
    public ToolStripItem[] CreateChartMenuItems() {
      List<ToolStripItem> menuItemList = new List<ToolStripItem>();
      menuItemList.Add(new AppMenuItem(AppAction.ChartCandleSticks));
      menuItemList.Add(new AppMenuItem(AppAction.ChartBarChart));
      menuItemList.Add(new AppMenuItem(AppAction.ChartLineChart));
      menuItemList.Add(new ToolStripSeparator());
      menuItemList.Add(new AppMenuItem(AppAction.ChartAutoScroll));
      menuItemList.Add(new AppMenuItem(AppAction.ChartChartShift));
      menuItemList.Add(new ToolStripSeparator());
      menuItemList.Add(new AppMenuItem(AppAction.ChartGrid));
      menuItemList.Add(new AppMenuItem(AppAction.ChartPeriodSeparators));
      menuItemList.Add(new ToolStripSeparator());
      menuItemList.Add(new AppMenuItem(AppAction.ChartZoomIn));
      menuItemList.Add(new AppMenuItem(AppAction.ChartZoomOut));
      menuItemList.Add(new ToolStripSeparator());
      menuItemList.Add(new AppMenuItem(AppAction.ChartSaveAsPicture));
      menuItemList.Add(new AppMenuItem(AppAction.ChartSaveAsReport));
      menuItemList.Add(new ToolStripSeparator());

      AppMenuItem chartTimeFramesMenuItem = new AppMenuItem(AppAction.ChartTimeFrames);
      menuItemList.Add(chartTimeFramesMenuItem);
      foreach (TimeFrame tf in TimeFrameManager.TimeFrames) {
        TimeFrameMenuItem timeFrameMenuItem = new TimeFrameMenuItem(tf);
        timeFrameMenuItem.Click += new EventHandler(timeFrameMenuItem_Click);
        chartTimeFramesMenuItem.DropDownItems.Add(timeFrameMenuItem);
      }

      AppMenuItem chartTemplateMenuItem = new AppMenuItem(AppAction.ChartTemplate);
      chartTemplateMenuItem.DropDownItems.Add(new AppMenuItem(AppAction.ChartTemplateSave));
      chartTemplateMenuItem.DropDownItems.Add(new AppMenuItem(AppAction.ChartTemplateLoad));
      chartTemplateMenuItem.DropDownItems.Add(new AppMenuItem(AppAction.ChartTemplateRemove));
      menuItemList.Add(chartTemplateMenuItem);

      foreach (ToolStripItem item in menuItemList)
        this.SetMenuItemClickEvent(item);

      return menuItemList.ToArray();
    }
    #endregion

    #region private void timeFrameMenuItem_Click(object sender, EventArgs e)
    private void timeFrameMenuItem_Click(object sender, EventArgs e) {

    }
    #endregion

    #region private void SetMenuItemClickEvent(ToolStripItem item)
    private void SetMenuItemClickEvent(ToolStripItem item) {
      ToolStripMenuItem menuItem = item as ToolStripMenuItem;
      if (menuItem == null)
        return;

      if (menuItem is AppMenuItem)
        (menuItem as AppMenuItem).Click += new EventHandler(this.appMenuItem_Click);

      foreach (ToolStripItem tsi in menuItem.DropDownItems) {
        if (tsi is ToolStripMenuItem) {
          this.SetMenuItemClickEvent(tsi as ToolStripMenuItem);
        }
      }
    }
    #endregion

    #region private void appMenuItem_Click(object sender, EventArgs e)
    private void appMenuItem_Click(object sender, EventArgs e) {
      AppAction action;

      if (sender is AppMenuItem) {
        action = (sender as AppMenuItem).Action;
      } else if (sender is AppToolStripButton)
        action = (sender as AppToolStripButton).Action;
      else
        return;

      switch (action) {
        case AppAction.ChartCandleSticks:
          break;
        case AppAction.ChartBarChart:
          break;
        case AppAction.ChartLineChart:
          break;
        case AppAction.ChartAutoScroll:
          break;
        case AppAction.ChartChartShift:
          break;
        case AppAction.ChartGrid:
          break;
        case AppAction.ChartPeriodSeparators:
          break;
        case AppAction.ChartZoomIn:
          break;
        case AppAction.ChartZoomOut:
          break;
        case AppAction.ChartSaveAsPicture:
          break;
        case AppAction.ChartSaveAsReport:
          break;
      }
    }
    #endregion

    #region private void DockManager_ActiveDocumentChanged(object sender, EventArgs e)
    private void DockManager_ActiveDocumentChanged(object sender, EventArgs e) {
      _chartMenuItem.Visible = Global.DockManager.ActiveDocument is ChartDocument;
      _currentChartDocument = Global.DockManager.ActiveDocument as ChartDocument;
    }
    #endregion

  }
}
