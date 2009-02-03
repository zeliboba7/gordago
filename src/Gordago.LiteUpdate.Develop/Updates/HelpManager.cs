/**
* @version $Id: HelpManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  class HelpManager {

    private ToolStripMenuItem _helpMenuItem;
    private bool _checkForUpdateWithDialog = false;

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

        value.Text = Global.Languages["Menu"]["Help"];
        value.DropDownItems.Clear();

        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.CheckForUpdates));
        menuItemList.Add(new ToolStripSeparator());
        menuItemList.Add(new AppMenuItem(AppAction.AboutLiteUpdateDevelop));
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
        case AppAction.AboutLiteUpdateDevelop:
          AboutBox abox = new AboutBox();
          abox.ShowDialog();
          break;
        case AppAction.CheckForUpdates:
          _checkForUpdateWithDialog = true;
          Global.UpdateManager.CheckForUpdates();
          break;
      }
    }
    #endregion

    #region private void UpdateManager_StartCheckForUpdates(object sender, EventArgs e)
    private void UpdateManager_StartCheckForUpdates(object sender, EventArgs e) {
      if (!Global.MainForm.InvokeRequired) {
        this.UpdateIDE();
      }else{
        Global.MainForm.Invoke(new EventHandler(UpdateManager_StartCheckForUpdates), sender, e);
      }
    }
    #endregion

    #region private void UpdateManager_StopCheckForUpdates(object sender, EventArgs e)
    private void UpdateManager_StopCheckForUpdates(object sender, EventArgs e) {
      if (!Global.MainForm.InvokeRequired) {
        this.UpdateIDE();
        _checkForUpdateWithDialog = false;
      } else {
        Global.MainForm.Invoke(new EventHandler(UpdateManager_StopCheckForUpdates), sender, e);
      }
    }
    #endregion

    #region private void UpdateIDE()
    private void UpdateIDE() {
      IDE.GetAppMenuItem(_helpMenuItem, AppAction.CheckForUpdates).Enabled = !Global.UpdateManager.IsCheckForUpdateProcess;
    }
    #endregion
  }
}
