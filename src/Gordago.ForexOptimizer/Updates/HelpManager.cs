/**
* @version $Id: HelpManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  class HelpManager {

    private ToolStripMenuItem _helpMenuItem;

    public HelpManager() {
      Global.UpdateManager.StartCheckForUpdates += new EventHandler(UpdateManager_StartCheckForUpdates);
      Global.UpdateManager.StopCheckForUpdates += new EventHandler(UpdateManager_StopCheckForUpdates);
    }

    #region public ToolStripMenuItem HelpMenuItem
    public ToolStripMenuItem HelpMenuItem {
      get { return _helpMenuItem; }
      set {
        if (_helpMenuItem == value)
          return;
        _helpMenuItem = value;

        if (value == null)
          return;

        value.Text = Global.Languages["Menu"]["Help"]; ;
        value.DropDownItems.Clear();

        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.CheckForUpdates));
        menuItemList.Add(new ToolStripSeparator());
        menuItemList.Add(new AppMenuItem(AppAction.AppAbout));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);
      }
    }
    #endregion

    #region private void SetMenuItemClickEvent(ToolStripMenuItem menuItem)
    private void SetMenuItemClickEvent(ToolStripMenuItem menuItem) {
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
      } else
        return;

      switch (action) {
        case AppAction.AppAbout:
          AboutBox abox = new AboutBox();
          abox.ShowDialog();
          break;
        case AppAction.CheckForUpdates:
          Global.UpdateManager.CheckForUpdates();
          break;
      }
    }
    #endregion

    #region private void UpdateManager_StartCheckForUpdates(object sender, EventArgs e)
    private void UpdateManager_StartCheckForUpdates(object sender, EventArgs e) {
      if (!Global.MainForm.InvokeRequired) {
        this.UpdateIDE();
      } else {
        Global.MainForm.Invoke(new EventHandler(UpdateManager_StartCheckForUpdates), sender, e);
      }
    }
    #endregion

    #region private void UpdateManager_StopCheckForUpdates(object sender, EventArgs e)
    private void UpdateManager_StopCheckForUpdates(object sender, EventArgs e) {
      if (!Global.MainForm.InvokeRequired) {
        this.UpdateIDE();
      } else {
        Global.MainForm.Invoke(new EventHandler(UpdateManager_StopCheckForUpdates), sender, e);
      }
    }
    #endregion

    #region private void UpdateIDE()
    private void UpdateIDE() {
      MainForm.GetAppMenuItem(_helpMenuItem, AppAction.CheckForUpdates).Enabled = !Global.UpdateManager.IsCheckForUpdateProcess;
    }
    #endregion
  }
}
