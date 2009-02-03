/**
* @version $Id: ProjectManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.LiteUpdate.Develop.Projects.Wizard;
  using System.IO;
  using WeifenLuo.WinFormsUI.Docking;
  using Gordago.Docking;

  public partial class ProjectManager : Component{

    private readonly RecentProjectManager _recentProjectManager = new RecentProjectManager();

    private ToolStripMenuItem _fileMenuItem;
    private ToolStripMenuItem _projectMenuItem;

    private ToolStrip _toolStrip;

    private Project _project;

    private bool _buildStarted = false;

    #region public ProjectsManager()
    public ProjectManager() {
      InitializeComponent();
    }
    #endregion

    #region public ProjectsManager(IContainer container)
    public ProjectManager(IContainer container) {
      container.Add(this);

      InitializeComponent();
    }
    #endregion

    #region public RecentProjectManager RecentProjects
    public RecentProjectManager RecentProjects {
      get { return _recentProjectManager; }
    }
    #endregion

    #region public Project Project
    public Project Project {
      get { return _project; }
    }
    #endregion

    #region public ToolStrip ToolStrip
    public ToolStrip ToolStrip {
      get { return _toolStrip; }
      set {
        if (this._toolStrip == value)
          return;

        _toolStrip = value;

        if (value == null)
          return;

        List<ToolStripItem> list = new List<ToolStripItem>();
        list.Add(new AppToolStripButton(AppAction.ProjectNew));
        list.Add(new AppToolStripButton(AppAction.ProjectOpen));
        list.Add(new AppToolStripButton(AppAction.ProjectSave));
        list.Add(new ToolStripSeparator());
        list.Add(new AppToolStripButton(AppAction.ProjectBuild));

        value.Items.AddRange(list.ToArray());
        this.SetToolStripItemClickEvent(value);
      }
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

        value.Text = Global.Languages["Menu"]["File"];
        value.DropDownItems.Clear();
        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.ProjectNew));
        menuItemList.Add(new AppMenuItem(AppAction.ProjectOpen));
        menuItemList.Add(new AppMenuItem(AppAction.ProjectSave));
        menuItemList.Add(new AppMenuItem(AppAction.ProjectSaveAs));
        menuItemList.Add(new AppMenuItem(AppAction.ProjectClose));
        menuItemList.Add(new ToolStripSeparator());
        AppMenuItem recentProjectMenuItem = new AppMenuItem(AppAction.RecentProjects);
        _recentProjectManager.RecentProjectMenu = recentProjectMenuItem;
        menuItemList.Add(recentProjectMenuItem);
        menuItemList.Add(new ToolStripSeparator());
        menuItemList.Add(new AppMenuItem(AppAction.AppExit));

        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);
      }
    }
    #endregion

    #region public ToolStripMenuItem ProjectMenuItem
    public ToolStripMenuItem ProjectMenuItem {
      get { return _projectMenuItem; }
      set {
        if (_projectMenuItem == value)
          return;
        _projectMenuItem = value;
        
        if (value == null)
          return;

        value.Text = Global.Languages["Menu"]["Project"];
        value.DropDownItems.Clear();
        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.ProjectCheckVersion));
        menuItemList.Add(new AppMenuItem(AppAction.ProjectBuild));
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

    #region private void SetToolStripItemClickEvent(ToolStrip ts)
    private void SetToolStripItemClickEvent(ToolStrip ts) {
      foreach (ToolStripItem item in ts.Items) {
        if (item is AppToolStripButton)
          (item as AppToolStripButton).Click += new EventHandler(this.appMenuItem_Click);
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
        case AppAction.AppExit:
          Global.MainForm.Close();
          break;
        case AppAction.ProjectNew:
          this.ProjectNew();
          break;
        case AppAction.ProjectSave:
          this.SaveProject();
          break;
        case AppAction.ProjectClose:
          this.CloseProject();
          break;
        case AppAction.ProjectOpen:
          this.OpenProject();
          break;
        case AppAction.ProjectSaveAs:
          this.SaveProjectAs();
          break;
        case AppAction.ProjectBuild:
          this.Build();
          break;
        case AppAction.ProjectCheckVersion:
          this.CheckVersion();
          break;
      }
    }
    #endregion

    #region public void ProjectNew()
    public void ProjectNew() {
      WizardForm form = new WizardForm();
      if (form.ShowDialog() != DialogResult.OK)
        return;
      this.OpenProject(form.Project);
    }
    #endregion

    #region public void CloseProject()
    public void CloseProject() {
      this.ProjectDisconnect();
    }
    #endregion

    #region public void OpenProject(Project project)
    public void OpenProject(Project project) {
      if (_project != null) {
        if (_project.File.FullName.ToUpper() == project.File.FullName.ToUpper())
          return;
      }

      this.ProjectConnect(project);
    }
    #endregion

    #region public void OpenProject()
    public void OpenProject() {
      FileInfo file =  Project.GetOpenFile();
      this.OpenProject(file);
    }
    #endregion

    #region public void OpenProject(FileInfo file)
    public void OpenProject(FileInfo file) {
      
      if (file == null || !file.Exists)
        return;
      try {
        Project project = new Project(file);
        this.OpenProject(project);
      } catch (Exception ex) {
        MessageBox.Show("Can not load project:\n" + ex.Message);
      }
    }
    #endregion

    #region public void SaveProject()
    public void SaveProject() {
      _project.Save();
      this.RecentProjects.Update(_project);
      this.UpdateIDE();
    }
    #endregion

    #region public void SaveProjectAs()
    public void SaveProjectAs() {
      _project.SaveAs();
      this.RecentProjects.Update(_project);
      this.UpdateIDE();
    }
    #endregion

    #region public void Load()
    public void Load() {

      _recentProjectManager.Load();

      this.UpdateIDE();

      Global.Builder.StartBuild += new BuilderEventHandler(Builder_StartBuild);
      Global.Builder.StopBuild += new BuilderEventHandler(Builder_StopBuild);
    }
    #endregion

    #region private void _project_DataChanged(object sender, EventArgs e)
    private void _project_DataChanged(object sender, EventArgs e) {
      this.UpdateIDE();
    }
    #endregion

    #region private void ProjectConnect(Project project)
    private void ProjectConnect(Project project) {
      if (project == _project)
        return;
      this.ProjectDisconnect();

      _project = project;
      _recentProjectManager.Update(project);
      Global.DockManager.SetProject(project);
      this.UpdateIDE();
      _project.DataChanged += new EventHandler(_project_DataChanged);
    }
    #endregion

    #region private void ProjectDisconnect()
    private void ProjectDisconnect() {
      if (_project == null)
        return;

      _project.DataChanged -= new EventHandler(_project_DataChanged);

      Global.DockManager.CloseProjectDocuments();
      Global.DockManager.SetProject(null);

      _project = null;
      this.UpdateIDE();
    }
    #endregion

    #region private void Builder_StartBuild(object sender, BuilderEventArgs e)
    private void Builder_StartBuild(object sender, BuilderEventArgs e) {
      if (!Global.MainForm.InvokeRequired) {
        _buildStarted = true;
        this.UpdateIDE();
      } else {
        Global.MainForm.Invoke(new BuilderEventHandler(Builder_StartBuild), sender, e);
      }
    }
    #endregion

    #region private void Builder_StopBuild(object sender, BuilderEventArgs e)
    private void Builder_StopBuild(object sender, BuilderEventArgs e) {
      if (!Global.MainForm.InvokeRequired) {
        _buildStarted = false;
        this.UpdateIDE();
      } else {
        Global.MainForm.Invoke(new BuilderEventHandler(Builder_StopBuild), sender, e);
      }
    }
    #endregion

    #region private void UpdateIDE()
    private void UpdateIDE() {
      bool projectLoad = _project != null;
      bool projectSaved = projectLoad && _project.IsSaved;
      
      IDE.GetAppMenuItem(_fileMenuItem, AppAction.ProjectSave).Enabled = !projectSaved;
      IDE.GetAppToolStripButtom(_toolStrip, AppAction.ProjectSave).Enabled = !projectSaved;

      IDE.GetAppMenuItem(_fileMenuItem, AppAction.ProjectSaveAs).Enabled = projectLoad;
      IDE.GetAppMenuItem(_fileMenuItem, AppAction.ProjectClose).Enabled = projectLoad;

      _projectMenuItem.Visible = projectLoad;
      IDE.GetAppToolStripButtom(_toolStrip, AppAction.ProjectBuild).Enabled = projectLoad && !_buildStarted;
    }
    #endregion

    #region public bool CheckVersion()
    public bool CheckVersion() {
      ErrorListWindow win = Global.DockManager.GetWindow(typeof(ErrorListWindow)) as ErrorListWindow;
      if (win != null)
        win.Clear();

      TMFile[] files = _project.FileSystem.GetFilesError();
      if (files.Length > 0) {
        Global.DockManager.ShowErrorListWindow().SetErrors(files);
        return false;
      }

      VersionInfo newVersion = _project.CreateNewVersion();
      if (newVersion != null) {
        ApplyNewVersionForm form = new ApplyNewVersionForm(newVersion);
        switch (form.ShowDialog()) {
          case DialogResult.Cancel:
            return false;
          case DialogResult.No:
            _project.ApplyVersion(newVersion, false);
            break;
          case DialogResult.Yes:
            _project.ApplyVersion(newVersion, true);
            break;
        }
      }
      return true;
    }
    #endregion

    #region public void Build()
    public void Build() {
      Project project = _project;

      if (!this.CheckVersion())
        return;

      _project.Save();

      Global.Builder.Build(project);
    }
    #endregion
  }
}

