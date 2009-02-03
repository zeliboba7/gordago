/**
* @version $Id: ProjectsManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.AppStructureEditor.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  class ProjectsManager {

    private Project _project;

    private ToolStripMenuItem _fileMenuItem;

    #region public Project Project
    public Project Project {
      get { return _project; }
    }
    #endregion

    #region public ToolStripMenuItem FileMenuItem
    public ToolStripMenuItem FileMenuItem {
      get { return this._fileMenuItem; }
      set {
        if (this._fileMenuItem == value)
          return;

        _fileMenuItem = value;

        if (value == null)
          return;

        value.Text = "File";
        value.DropDownItems.Clear();
        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.ProjectOpen));
        menuItemList.Add(new ToolStripSeparator());
        menuItemList.Add(new AppMenuItem(AppAction.AppExit));

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
        case AppAction.AppExit:
          Global.MainForm.Close();
          break;
        case AppAction.ProjectOpen:
          this.OpenProject();
          break;
      }
    }
    #endregion

    public void OpenProject() {
      OpenProjectForm form = new OpenProjectForm();
      if (form.ShowDialog() == DialogResult.Cancel)
        return;

      _project = new Project(form.AppBinDirectory);
      ProjectForm pform = new ProjectForm(_project);
      pform.MdiParent = Global.MainForm;
      pform.Show();
    }
  }
}
