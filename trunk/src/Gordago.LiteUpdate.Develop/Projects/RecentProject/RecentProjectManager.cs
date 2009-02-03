/**
* @version $Id: RecentProjectManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;
  using System.IO;
  using System.Windows.Forms;

  public class RecentProjectManager : IEnumerable<RecentProject> {

    #region class RecentProjectMenuItem : ToolStripMenuItem
    class RecentProjectMenuItem : ToolStripMenuItem {

      private readonly RecentProject _recentProject;

      public RecentProjectMenuItem(RecentProject recentProject) {
        _recentProject = recentProject;

        FileInfo file = recentProject.File;
        this.Text = file.FullName;
      }

      public RecentProject RecentProject {
        get { return _recentProject; }
      }
    }
    #endregion

    public event EventHandler ListChanged;

    private readonly Dictionary<FileKey, RecentProject> _recentProjects = new Dictionary<FileKey, RecentProject>();

    private ToolStripMenuItem _menuItem;

    #region public ToolStripMenuItem RecentProjectMenu
    public ToolStripMenuItem RecentProjectMenu {
      get { return _menuItem; }
      set {
        _menuItem = value;
      }
    }
    #endregion

    #region public void Load()
    public void Load() {
      string[] files = Global.Properties["RecentProject"].GetValue<string[]>("Files", new string[0]);
      foreach (string file in files) {
        this.Update(new FileInfo(file));
      }
    }
    #endregion

    #region private void Save()
    private void Save() {
      List<string> files = new List<string>();
      foreach (RecentProject rproject in this) {
        files.Add(rproject.File.FullName);
      }
      Global.Properties["RecentProject"].SetValue<string[]>("Files", files.ToArray());
    }
    #endregion

    #region private static int CompareByTime(RecentProject rproject1, RecentProject rproject2)
    private static int CompareByTime(RecentProject rproject1, RecentProject rproject2) {
      return rproject2.File.LastWriteTime.CompareTo(rproject1.File.LastWriteTime);
    }
    #endregion

    #region public void Update(Project project)
    public void Update(Project project) {
      this.Update(project.File);
    }
    #endregion

    #region private void Update(FileInfo file)
    private void Update(FileInfo file) {
      FileKey key = new FileKey(file);
      List<RecentProject> list = new List<RecentProject>();

      bool flagChanged = false;

      if (!_recentProjects.ContainsKey(key)) {
        _recentProjects.Add(key, new RecentProject(file));
        flagChanged = true;
      }

      foreach (RecentProject rproject in _recentProjects.Values) {
        if (rproject.File.Exists)
          list.Add(rproject);
      }
      RecentProject[] temp = list.ToArray();
      list.Sort(CompareByTime);

      _recentProjects.Clear();
      for (int i = 0; i < list.Count; i++) {
        if (!flagChanged) {
          if (temp[i] != list[i])
            flagChanged = true;
        }
        _recentProjects.Add(new FileKey(list[i].File), list[i]);
      }

      if (flagChanged) {
        this.UpdateMenuItem();
        this.Save();
        this.OnListChanged();
      }
    }
    #endregion

    #region private void UpdateMenuItem()
    private void UpdateMenuItem() {
      _menuItem.DropDownItems.Clear();
      foreach (RecentProject rproject in this) {
        RecentProjectMenuItem menuItem = new RecentProjectMenuItem(rproject);
        menuItem.Click += new EventHandler(menuItem_Click);
        _menuItem.DropDownItems.Add(menuItem);
      }
    }
    #endregion

    #region private void menuItem_Click(object sender, EventArgs e)
    private void menuItem_Click(object sender, EventArgs e) {
      RecentProjectMenuItem menuItem = sender as RecentProjectMenuItem;
      Global.MainForm.ProjectManager.OpenProject(menuItem.RecentProject.File);
    }
    #endregion

    #region protected virtual void OnListChanged()
    protected virtual void OnListChanged() {
      if (this.ListChanged != null)
        this.ListChanged(this, EventArgs.Empty);
    }
    #endregion

    #region public IEnumerator<RecentProject> GetEnumerator()
    public IEnumerator<RecentProject> GetEnumerator() {
      return _recentProjects.Values.GetEnumerator();
    }
    #endregion

    #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return _recentProjects.Values.GetEnumerator();
    }
    #endregion
  }
}
