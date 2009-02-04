/**
* @version $Id: BuildManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  class BuildManager {
    private ToolStripMenuItem _buildMenuItem;

    public BuildManager() {
      Global.DockManager.ActiveDocumentChanged += new EventHandler(DockManager_ActiveDocumentChanged);
    }

    #region public ToolStripMenuItem BuildMenuItem
    public ToolStripMenuItem BuildMenuItem {
      get { return _buildMenuItem; }
      set {
        if (_buildMenuItem == value)
          return;

        _buildMenuItem = value;
        if (value == null)
          return;

        value.Text = Global.Languages["Menu"]["Build"];
        value.DropDownItems.Clear();
        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.BuildProject));
        value.DropDownItems.AddRange(menuItemList.ToArray());

        this.SetMenuItemClickEvent(value);
        this.UpdateIDE();
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
      } else if (sender is AppToolStripButton)
        action = (sender as AppToolStripButton).Action;
      else
        return;

      switch (action) {
        case AppAction.BuildProject:
          break;
      }
    }
    #endregion

    #region private void DockManager_ActiveDocumentChanged(object sender, EventArgs e)
    private void DockManager_ActiveDocumentChanged(object sender, EventArgs e) {
      this.UpdateIDE();
    }
    #endregion

    #region private void UpdateIDE()
    private void UpdateIDE() {
      CodeEditorDocument codeEditor = Global.DockManager.ActiveDocument as CodeEditorDocument;
      _buildMenuItem.Visible = codeEditor != null;
    }
    #endregion
  }
}
