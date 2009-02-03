/**
* @version $Id: ExtDockManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{

  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;
  using System.Windows.Forms;
  using System.Reflection;
  using WeifenLuo.WinFormsUI.Docking;
  using System.IO;
  using Gordago.Docking;
  using Gordago.FO.Quotes;
  using Gordago.FO.Charting;
  using Gordago.Trader;
  using Gordago.FO.Instruments;

  class ExtDockManager : DockManager {
    private DeserializeDockContent _deserializeDockContent;

    private ToolStripMenuItem _viewMenuItem;

    private LanguageMenuManager _languageMenuManager;

    public ExtDockManager() {
      _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
    }

    #region private DirectoryInfo DocumentsDirectory
    private DirectoryInfo DocumentsDirectory {
      get {
        return new DirectoryInfo(Path.Combine(Global.Setup.OptionsDirectory.FullName, "Documents"));
      }
    }
    #endregion

    #region private string ConfigFile
    private string ConfigFile {
      get {
        Assembly assembly = Assembly.GetExecutingAssembly();
        return Path.Combine(Global.Setup.OptionsDirectory.FullName, Path.GetFileNameWithoutExtension(assembly.Location) + ".DockPanel.xml");
      }
    }
    #endregion

    #region private FileInfo DocumentsListFile
    private FileInfo DocumentsListFile {
      get {
        return new FileInfo(Path.Combine(this.DocumentsDirectory.FullName, "documents.xml"));
      }
    }
    #endregion

    #region private IDockContent GetContentFromPersistString(string persistString)
    private IDockContent GetContentFromPersistString(string persistString) {
      if (persistString == typeof(QuotesWindow).ToString())
        return this.ShowQuotesWindow();
      if (persistString == typeof(IndicatorsWindow).ToString())
        return this.ShowIndicatorsWindow();
      if (persistString == typeof(PropertiesWindow).ToString())
        return this.ShowPropertiesWindow();

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
        menuItemList.Add(new AppMenuItem(AppAction.ViewSymbols));
        menuItemList.Add(new AppMenuItem(AppAction.ViewIndicators));
        menuItemList.Add(new AppMenuItem(AppAction.ViewProperties));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);

        ToolStripMenuItem mniLanguages = new ToolStripMenuItem();
        _languageMenuManager = new LanguageMenuManager(Global.Languages, mniLanguages);
        _languageMenuManager.LanguageChanged += new EventHandler(_languageMenuManager_LanguageChanged);
        value.DropDownItems.Insert(0, mniLanguages);
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
      } else if (sender is AppToolStripButton)
        action = (sender as AppToolStripButton).Action;
      else
        return;

      switch (action) {
        case AppAction.AppExit:
          Global.MainForm.Close();
          break;
        case AppAction.ViewSymbols:
          this.ShowQuotesWindow();
          break;
        case AppAction.ViewIndicators:
          this.ShowIndicatorsWindow();
          break;
        case AppAction.ViewProperties:
          this.ShowPropertiesWindow();
          break;
      }
    }
    #endregion

    #region private void SaveDocuments()
    private void SaveDocuments() {
      DirectoryInfo dir = this.DocumentsDirectory;
      dir.Create();
      foreach (FileInfo file in dir.GetFiles()) 
        file.Delete();

      EasyProperties ps = new EasyProperties();
      foreach (IDockContent docContent in this.Documents) {

        ITabbedDocument document = docContent as ITabbedDocument;
        if (document == null)
          continue;

        EasyPropertiesNode node = ps[document.GUID];
        node.SetValue<string>("Type", docContent.GetType().FullName);

        FileInfo file = new FileInfo(Path.Combine(dir.FullName, document.GUID + ".xml"));
        document.SaveProperties(file);
      }
      ps.Save(this.DocumentsListFile);
    }
    #endregion

    #region private void LoadDocuments()
    private void LoadDocuments() {
      DirectoryInfo dir = this.DocumentsDirectory;
      FileInfo documentsListFile = this.DocumentsListFile;
      if (!documentsListFile.Exists)
        return;

      EasyProperties ps = new EasyProperties();
      ps.Load(documentsListFile);
      foreach (EasyPropertiesNode node in ps.GetChildProperties()){
        FileInfo file = new FileInfo(Path.Combine(dir.FullName, node.Name + ".xml"));
        if (!file.Exists)
          continue;
        try {
          string typeName = node.GetValue<string>("Type", "");
          Type type = Type.GetType(typeName);
          TabbedDocument document = Activator.CreateInstance(type) as TabbedDocument;
          (document as ITabbedDocument).LoadProperties(file);
          document.Show(this);
        } catch { }
      }
    }
    #endregion

    #region public void Load()
    public void Load() {
      if (File.Exists(this.ConfigFile))
        this.LoadFromXml(this.ConfigFile, _deserializeDockContent);
      this.LoadDocuments();
    }
    #endregion

    #region public void Save()
    public void Save() {
      this.SaveDocuments();
      this.CloseAllDocuments();
      this.SaveAsXml(this.ConfigFile);
    }
    #endregion

    #region public QuotesWindow ShowQuotesWindow()
    public QuotesWindow ShowQuotesWindow() {
      QuotesWindow window = this.GetWindow(typeof(QuotesWindow)) as QuotesWindow;
      if (window == null) {
        window = new QuotesWindow();
      }
      window.Show(this);
      return window;
    }
    #endregion

    #region public IndicatorsWindow ShowIndicatorsWindow()
    public IndicatorsWindow ShowIndicatorsWindow() {
      IndicatorsWindow window = this.GetWindow(typeof(IndicatorsWindow)) as IndicatorsWindow;
      if (window == null) {
        window = new IndicatorsWindow();
      }
      window.Show(this);
      return window;
    }
    #endregion

    #region public PropertiesWindow ShowPropertiesWindow()
    public PropertiesWindow ShowPropertiesWindow() {
      PropertiesWindow window = this.GetWindow(typeof(PropertiesWindow)) as PropertiesWindow;
      if (window == null) {
        window = new PropertiesWindow();
      }
      window.Show(this);
      return window;
    }
    #endregion

    #region public ChartDocument ShowChartDocument(ISymbol symbol)
    public ChartDocument ShowChartDocument(ISymbol symbol) {
      ChartDocumentKey key = new ChartDocumentKey();
      ChartDocument document = new ChartDocument(symbol);
      document.Show(this);
      return document;
    }
    #endregion

    #region public CodeEditorDocument ShowCodeEditorDocument(FileInfo file)
    public CodeEditorDocument ShowCodeEditorDocument(FileInfo file) {
      CodeEditorDocumentKey key = new CodeEditorDocumentKey(file);
      CodeEditorDocument document = this.GetDocument(key) as CodeEditorDocument;
      if (document == null)
        document = new CodeEditorDocument(file);
      document.Show(this);
      return document;
    }
    #endregion
  }
}
