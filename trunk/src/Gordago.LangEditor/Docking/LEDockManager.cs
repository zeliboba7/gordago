/**
* @version $Id: LEDockManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor.Docking
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Docking;
  using WeifenLuo.WinFormsUI.Docking;
  using System.IO;
  using System.Reflection;
  using System.Windows.Forms;
  using Gordago.LangEditor.AppList;
  using Gordago.Core;

  class LEDockManager : DockManager {
    private DeserializeDockContent _deserializeDockContent;
    
    private ToolStripMenuItem _viewMenuItem;
    private ToolStripMenuItem _fileMenuItem;
    private ToolStripMenuItem _helpMenuItem;

    private LanguageMenuManager _languageMenuManager;

    public LEDockManager() {
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

    #region private IDockContent GetContentFromPersistString(string persistString)
    private IDockContent GetContentFromPersistString(string persistString) {
      if (persistString == typeof(AppListWindow).ToString())
        return this.ShowAppListWindow();

      return null;
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
        menuItemList.Add(new AppMenuItem(AppAction.ViewAppList));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);
        
        ToolStripMenuItem mniLanguages = new ToolStripMenuItem();
        _languageMenuManager = new LanguageMenuManager(Global.Languages, mniLanguages);
        _languageMenuManager.LanguageChanged += new EventHandler(_languageMenuManager_LanguageChanged);
        value.DropDownItems.Insert(0, mniLanguages);
      }
    }
    #endregion

    #region public ToolStripMenuItem FileMenuItem
    public ToolStripMenuItem FileMenuItem {
      get { return _fileMenuItem; }
      set {
        if (_fileMenuItem == value)
          return;

        _fileMenuItem = value;

        if (value == null)
          return;

        value.Text = Global.Languages["Menu"]["File"];
        value.DropDownItems.Clear();
        List<ToolStripItem> menuItemList = new List<ToolStripItem>();
        menuItemList.Add(new AppMenuItem(AppAction.AppExit));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);
      }
    }
    #endregion

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
        menuItemList.Add(new AppMenuItem(AppAction.AppAbout));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);
      }
    }
    #endregion

    #region private void _languageMenuManager_LanguageChanged(object sender, EventArgs e)
    private void _languageMenuManager_LanguageChanged(object sender, EventArgs e) {
      Global.Properties.SetValue<string>("Language", _languageMenuManager.SelectedLanguage);
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
      } 
      //else if (sender is AppToolStripButton)
      //  action = (sender as AppToolStripButton).Action;
      else
        return;

      switch (action) {
        case AppAction.ViewAppList:
          this.ShowAppListWindow();
          break;
        case AppAction.AppAbout:
          AboutBox box = new AboutBox();
          box.ShowDialog();
          break;
        case AppAction.AppExit:
          Global.MainForm.Close();
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

    #region public AppListWindow ShowAppListWindow()
    public AppListWindow ShowAppListWindow() {
      AppListWindow window = this.GetWindow(typeof(AppListWindow)) as AppListWindow;
      if (window == null) {
        window = new AppListWindow();
      }
      window.Show(this);
      return window;
    }
    #endregion

    #region public LanguageEditorDocument ShowLanguageEditorDocument(AppItem appItem)
    public LanguageEditorDocument ShowLanguageEditorDocument(AppItem appItem) {
      EditorDocumentKey key = new EditorDocumentKey(appItem);
      LanguageEditorDocument document = GetDocument(key) as LanguageEditorDocument;

      if (document == null) 
        document = new LanguageEditorDocument(appItem);
      document.Show(this);
      return document;
    }
    #endregion
  }
}
