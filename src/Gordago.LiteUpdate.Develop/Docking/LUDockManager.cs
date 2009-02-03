/**
* @version $Id: LUDockManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Docking
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using WeifenLuo.WinFormsUI.Docking;
  using System.Windows.Forms;
  using System.IO;
  using Gordago.LiteUpdate.Develop.Projects;
  using Gordago.Docking;
  using System.Reflection;
  using Gordago.Core;

  public class LUDockManager : DockManager {

    private ToolStripMenuItem _viewMenuItem;
    private ToolStrip _toolStrip;
    private DeserializeDockContent _deserializeDockContent;
    private Project _project;

    private LanguageMenuManager _languageMenuManager;


    public LUDockManager() {
      _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
    }

    #region private string ConfigFile
    private string ConfigFile {
      get {
        Assembly assembly = Assembly.GetExecutingAssembly();
        return Path.Combine(Global.Setup.OptionsDirectory.FullName, Path.GetFileNameWithoutExtension(assembly.Location) + ".DockPanel.xml"); 
      }
    }
    #endregion

    #region public ToolStrip ToolStrip
    public ToolStrip ToolStrip {
      get { return _toolStrip; }
      set {
        if (_toolStrip == value)
          return;
        _toolStrip = value;
        if (value == null)
          return;

        List<ToolStripItem> list = new List<ToolStripItem>();
        list.Add(new ToolStripSeparator());
        list.Add(new AppToolStripButton(AppAction.ViewProjectWindow));
        list.Add(new AppToolStripButton(AppAction.ViewOutputWindow));
        list.Add(new AppToolStripButton(AppAction.ViewErrorListWindow));
        list.Add(new AppToolStripButton(AppAction.ViewStartPage));
        _toolStrip.Items.AddRange(list.ToArray());
        this.SetToolStripItemClickEvent(value);
      }
    }
    #endregion

    #region public ToolStripMenuItem ViewMenuItem
    public ToolStripMenuItem ViewMenuItem {
      get { return _viewMenuItem; }
      set {
        if (_viewMenuItem == value)
          return;

        _viewMenuItem = value;

        if (value == null)
          return;

        value.Text = Global.Languages["Menu"]["View"];
        value.DropDownItems.Clear();
        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.ViewProjectWindow));
        menuItemList.Add(new AppMenuItem(AppAction.ViewOutputWindow));
        menuItemList.Add(new AppMenuItem(AppAction.ViewErrorListWindow));
        menuItemList.Add(new AppMenuItem(AppAction.ViewStartPage));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);

        ToolStripMenuItem mniLanguages = new ToolStripMenuItem();
        _languageMenuManager = new LanguageMenuManager(Global.Languages, mniLanguages);
        _languageMenuManager.LanguageChanged += new EventHandler(_languageMenuManager_LanguageChanged);
        value.DropDownItems.Insert(0, mniLanguages);
      }
    }
    #endregion

    private void _languageMenuManager_LanguageChanged(object sender, EventArgs e) {
      Global.Properties.SetValue<string>("Language", _languageMenuManager.SelectedLanguage);
    }

    #region private IDockContent GetContentFromPersistString(string persistString)
    private IDockContent GetContentFromPersistString(string persistString) {
      if (persistString == typeof(ProjectWindow).ToString()) {
        return this.ShowProjectWindow();
      } else if (persistString == typeof(OutputWindow).ToString()) {
        return this.ShowOutputWindow();
      }else if (persistString == typeof(ErrorListWindow).ToString()){
        return this.ShowErrorListWindow();
      } else {
        return null;
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
        case AppAction.ViewProjectWindow:
          this.ShowProjectWindow();
          break;
        case AppAction.ViewOutputWindow:
          this.ShowOutputWindow();
          break;
        case AppAction.ViewStartPage:
          this.ShowStartPageDocument();
          break;
        case AppAction.ViewErrorListWindow:
          this.ShowErrorListWindow();
          break;
      }
    }
    #endregion

    #region public void Load()
    public void Load() {
      if (File.Exists(this.ConfigFile))
        this.LoadFromXml(this.ConfigFile, _deserializeDockContent);
    }
    #endregion

    #region public void Save()
    public void Save() {
      this.CloseAllDocuments();
      this.SaveAsXml(this.ConfigFile);
    }
    #endregion

    #region public void SetProject(Project project)
    public void SetProject(Project project) {

      _project = project;

      ProjectWindow projectWindow = this.GetWindow(typeof(ProjectWindow)) as ProjectWindow;
      if (projectWindow != null)
        projectWindow.SetProject(project);

      if (project == null) 
        return;

      this.ShowFileSystemDocument(project);
    }
    #endregion

    #region public ProjectWindow ShowProjectWindow()
    public ProjectWindow ShowProjectWindow() {
      ProjectWindow window = this.GetWindow(typeof(ProjectWindow)) as ProjectWindow;
      if (window == null) {
        window = new ProjectWindow();
        window.SetProject(Global.MainForm.ProjectManager.Project);
      }
      window.Show(this);
      return window;
    }
    #endregion

    #region public OutputWindow ShowOutputWindow()
    public OutputWindow ShowOutputWindow() {
      OutputWindow output = this.GetWindow(typeof(OutputWindow)) as OutputWindow;

      if (output == null)
        output = new OutputWindow();
      output.Show(this);
      return output;
    }
    #endregion

    #region public ErrorListWindow ShowErrorListWindow()
    public ErrorListWindow ShowErrorListWindow() {
      ErrorListWindow errorList = this.GetWindow(typeof(ErrorListWindow)) as ErrorListWindow;

      if (errorList == null)
        errorList = new ErrorListWindow();
      errorList.Show(this);
      return errorList;
    }
    #endregion

    #region public FileSystemDocument ShowFileSystemDocument(Project project)
    public FileSystemDocument ShowFileSystemDocument(Project project) {
      ProjectDocumtenKey key = new FileSystemDocumentKey(project);
      FileSystemDocument document = GetDocument(key) as FileSystemDocument;
      if (document == null) {
        document = new FileSystemDocument(project);
      }
      document.Show(this);
      return document;
    }
    #endregion

    #region public ProjectPropertiesDocument ShowProjectPropertiesDocument(Project project)
    public ProjectPropertiesDocument ShowProjectPropertiesDocument(Project project) {
      ProjectPropertiesDocumentKey key = new ProjectPropertiesDocumentKey(project);
      ProjectPropertiesDocument document = GetDocument(key) as ProjectPropertiesDocument;
      if (document == null) {
        document = new ProjectPropertiesDocument(project);
      }
      document.Show(this);
      return document;
    }
    #endregion

    #region public void ShowVersionFilesModifyDocument(int version)
    public void ShowVersionFilesModifyDocument(int version) {
      VersionInfo versionInfo = _project.OpenVersion(version);
      VersionFilesModifyDocument.DocumentKey key = new VersionFilesModifyDocument.DocumentKey(versionInfo);
      VersionFilesModifyDocument document = this.GetDocument(key) as VersionFilesModifyDocument;
      if (document == null)
        document = new VersionFilesModifyDocument(versionInfo);
      document.Show(this);
    }
    #endregion

    #region public void ShowVersionUserInfoDocument(int version)
    public void ShowVersionUserInfoDocument(int version) {
      VersionInfo versionInfo = _project.OpenVersion(version);
      VersionProductInfoDocument.DocumentKey key = new VersionProductInfoDocument.DocumentKey(versionInfo);
      VersionProductInfoDocument document = this.GetDocument(key) as VersionProductInfoDocument;
      if (document == null)
        document = new VersionProductInfoDocument(versionInfo);
      document.Show(this);
    }
    #endregion

    #region public void ShowStartPageDocument()
    public void ShowStartPageDocument() {
      StartPageDocument.DocumentKey key = new StartPageDocument.DocumentKey();
      StartPageDocument document = this.GetDocument(key) as StartPageDocument;
      if (document == null)
        document = new StartPageDocument();
      document.Show(this);
    }
    #endregion

    #region public void CloseProjectDocuments()
    public void CloseProjectDocuments() {
      List<TabbedDocument> list = new List<TabbedDocument>();
      foreach (TabbedDocument document in this.TabbedDocuments.Values) {
        //if (document is FileSystemDocument ||
        //  document is VersionFilesModifyDocument ||
        //  document is VersionUserInfoDocument)
        if (document is ProjectDocument)
          list.Add(document);
      }

      foreach (TabbedDocument document in list)
        document.Close();
    }
    #endregion

    #region public void ShowProjectError(TMFile file)
    public void ShowProjectError(TMFile file) {
      FileSystemDocument document = this.ShowFileSystemDocument(_project);
      document.ShowError(file);
    }
    #endregion


  }
}
