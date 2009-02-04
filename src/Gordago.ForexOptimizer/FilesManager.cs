/**
* @version $Id: FilesManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Gordago.FO.Instruments;
  using System.IO;
  using Gordago.Core.RecentFiles;
  using Gordago.Docking;

  class FilesManager {
    private ToolStripMenuItem _fileMenuItem;
    private ToolStrip _toolStrip;
    private RecentFilesManager _recentFilesManager;

    private AppMenuItem _menuItemFileSave;
    private AppMenuItem _menuItemFileSaveAs;
    private AppMenuItem _menuItemFileClose;

    private AppToolStripButton _btnFileSave;

    public FilesManager() {
      Global.DockManager.ActiveDocumentChanged += new EventHandler(DockManager_ActiveDocumentChanged);
      Global.DockManager.ContentAdded += new EventHandler<WeifenLuo.WinFormsUI.Docking.DockContentEventArgs>(DockManager_ContentAdded);
      Global.DockManager.ContentRemoved += new EventHandler<WeifenLuo.WinFormsUI.Docking.DockContentEventArgs>(DockManager_ContentRemoved);
    }

    #region public ToolStrip ToolStrip
    public ToolStrip ToolStrip {
      get { return _toolStrip; }
      set {
        this._toolStrip = value;
        List<ToolStripItem> btnList = new List<ToolStripItem>();
        btnList.Add(new AppToolStripButton(AppAction.FileNew));
        btnList.Add(new AppToolStripButton(AppAction.FileOpen));

        _btnFileSave = new AppToolStripButton(AppAction.FileSave);
        btnList.Add(_btnFileSave);
        btnList.Add(new AppToolStripButton(AppAction.FileSaveAll));

        this.SetToolStripItemClickEvent(btnList.ToArray());
        this._toolStrip.Items.AddRange(btnList.ToArray());
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
        menuItemList.Add(new AppMenuItem(AppAction.FileNew));
        menuItemList.Add(new AppMenuItem(AppAction.FileOpen));
        menuItemList.Add(new ToolStripSeparator());

        _menuItemFileSave = new AppMenuItem(AppAction.FileSave);
        _menuItemFileSaveAs = new AppMenuItem(AppAction.FileSaveAs);
        _menuItemFileSaveAs = new AppMenuItem(AppAction.FileSaveAll);
        _menuItemFileClose = new AppMenuItem(AppAction.FileClose);

        menuItemList.Add(_menuItemFileSave);
        menuItemList.Add(_menuItemFileSaveAs);
        menuItemList.Add(new ToolStripSeparator());
        menuItemList.Add(_menuItemFileClose);
        menuItemList.Add(new ToolStripSeparator());
        AppMenuItem menuRecent = new AppMenuItem(AppAction.FileRecent);
        menuItemList.Add(menuRecent);
        menuItemList.Add(new ToolStripSeparator());
        menuItemList.Add(new AppMenuItem(AppAction.AppExit));
        value.DropDownItems.AddRange(menuItemList.ToArray());
        this.SetMenuItemClickEvent(value);
        _recentFilesManager = new RecentFilesManager(menuRecent, Global.Properties["RecentFiles"]);
        _recentFilesManager.RecentMenuItemClick += new RecentFilesEventHanler(_recentFilesManager_RecentMenuItemClick);
      }
    }
    #endregion

    #region private void SetToolStripItemClickEvent(ToolStripItem[] items)
    private void SetToolStripItemClickEvent(ToolStripItem[] items) {
      foreach (ToolStripItem item in items) {
        if (item is AppToolStripButton)
          (item as AppToolStripButton).Click += new EventHandler(this.appMenuItem_Click);
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
        case AppAction.AppExit:
          Global.MainForm.Close();
          break;
        case AppAction.FileNew:
          this.FileNew();
          break;
        case AppAction.FileOpen:
          this.FileOpen();
          break;
        case AppAction.FileSave:
          this.FileSave();
          break;
        case AppAction.FileSaveAs:
          this.FileSaveAs();
          break;
        case AppAction.FileSaveAll:
          this.FileSaveAll();
          break;
        case AppAction.FileClose:
          this.FileClose();
          break;
      }
    }
    #endregion

    #region private void _recentFilesManager_RecentMenuItemClick(object sender, RecentFilesEventArgs e)
    private void _recentFilesManager_RecentMenuItemClick(object sender, RecentFilesEventArgs e) {
      this.FileOpen(e.File);
    }
    #endregion

    #region private void DockManager_ActiveDocumentChanged(object sender, EventArgs e)
    private void DockManager_ActiveDocumentChanged(object sender, EventArgs e) {
      this.UpdateIDE();
    }
    #endregion

    #region private void DockManager_ContentAdded(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e)
    private void DockManager_ContentAdded(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e) {
      ITabbedFileDocument fileDoc = e.Content as ITabbedFileDocument;
      if (fileDoc == null)
        return;
      this._recentFilesManager.Add(fileDoc.ContentFile);
      fileDoc.ContentChanged += new EventHandler(fileDoc_ContentChanged);
      fileDoc.ContentSaved += new EventHandler(fileDoc_ContentSaved);
    }
    #endregion

    #region private void DockManager_ContentRemoved(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e)
    private void DockManager_ContentRemoved(object sender, WeifenLuo.WinFormsUI.Docking.DockContentEventArgs e) {
      ITabbedFileDocument fileDoc = e.Content as ITabbedFileDocument;
      if (fileDoc == null)
        return;
      this._recentFilesManager.Update();
      fileDoc.ContentChanged -= new EventHandler(fileDoc_ContentChanged);
      fileDoc.ContentSaved -= new EventHandler(fileDoc_ContentSaved);
    }
    #endregion

    #region private void fileDoc_ContentSaved(object sender, EventArgs e)
    private void fileDoc_ContentSaved(object sender, EventArgs e) {
      this.UpdateIDE();
    }
    #endregion

    #region private void fileDoc_ContentChanged(object sender, EventArgs e)
    private void fileDoc_ContentChanged(object sender, EventArgs e) {
      this.UpdateIDE();
    }
    #endregion

    #region public void UpdateIDE()
    public void UpdateIDE() {
      ITabbedFileDocument fileDoc = Global.DockManager.ActiveDocument as ITabbedFileDocument;

      _btnFileSave.Enabled = _menuItemFileSave.Enabled = fileDoc != null && !fileDoc.IsContentSaved;
      _menuItemFileSaveAs.Enabled = fileDoc != null;
      _menuItemFileClose.Enabled = fileDoc != null;
    }
    #endregion

    #region public void FileNew()
    public void FileNew() {
      NewInstrumentsForm form = new NewInstrumentsForm();
      if (form.ShowDialog() != DialogResult.OK)
        return;
      InstrumentType instType = form.InstrumentType;
      CodeLang lang = form.CodeLang;
      string instName = form.InstrumentName;

      string codeTemplateName = "";
      DirectoryInfo dir = null;

      if (instType == InstrumentType.Indicator) {
        codeTemplateName = "Indicator";
        dir = Global.Setup.IndicatorsDirectory;
      } else if (instType == InstrumentType.Strategy) {
        codeTemplateName = "Strategy";
        dir = Global.Setup.StrategyDirectory;
      }

      string ext = "." + (lang == CodeLang.CSharp ? "cs" : "vb");

      FileInfo file = new FileInfo(Path.Combine(dir.FullName, instName + ext));
      if (!file.Exists) {

        codeTemplateName += ext;
        string template = "";
        FileInfo fileTemplate = new FileInfo(Path.Combine(Global.Setup.TemplateDirectory.FullName, codeTemplateName));
        if (fileTemplate.Exists) {
          template = File.ReadAllText(fileTemplate.FullName);
        }
        File.WriteAllText(file.FullName, template);
        file.Refresh();
      }
      this.FileOpen(file);
    }
    #endregion

    #region public void FileOpen()
    public void FileOpen() {
      OpenFileDialog ofd = new OpenFileDialog();
      DirectoryInfo dir = new DirectoryInfo(Global.Properties.GetValue<string>("LastDir", Global.Setup.InstrumentsDirectory.FullName));
      if (!dir.Exists) {
        dir = new DirectoryInfo(Global.Setup.InstrumentsDirectory.FullName);
      }
      ofd.InitialDirectory = dir.FullName;
      ofd.Filter = "Visual C# (*.cs)|*.cs|Visual Basic (*.vb)|*.vb|All files (*.*)|*.*";
      if (ofd.ShowDialog() != DialogResult.OK)
        return;

      FileInfo file = null;
      foreach (string sfile in ofd.FileNames) {
        file = new FileInfo(sfile);
        this.FileOpen(file);
      }
      Global.Properties.SetValue<string>("LastDir", file.Directory.FullName);
    }
    #endregion

    #region public void FileOpen(FileInfo file)
    public void FileOpen(FileInfo file) {
      string ext = file.Extension.ToLower();

      switch (ext) {
        case ".cs":
        case ".vb":
          Global.DockManager.ShowCodeEditorDocument(file);
          break;
      }
    }
    #endregion

    #region public void FileSave()
    public void FileSave() {
      ITabbedFileDocument fileDoc = Global.DockManager.ActiveDocument as ITabbedFileDocument;
      if (fileDoc == null)
        return;
      fileDoc.SaveContent();
      this._recentFilesManager.Add(fileDoc.ContentFile);
    }
    #endregion

    #region public void FileSaveAs()
    public void FileSaveAs() {
      SaveFileDialog sfd = new SaveFileDialog();
      DirectoryInfo dir = new DirectoryInfo(Global.Properties.GetValue<string>("LastDir", Global.Setup.InstrumentsDirectory.FullName));
      if (!dir.Exists) {
        dir = new DirectoryInfo(Global.Setup.InstrumentsDirectory.FullName);
      }
      sfd.InitialDirectory = dir.FullName;
      sfd.Filter = "Visual C# (*.cs)|*.cs|Visual Basic (*.vb)|*.vb|All files (*.*)|*.*";
      if (sfd.ShowDialog() != DialogResult.OK)
        return;

      ITabbedFileDocument fileDoc = Global.DockManager.ActiveDocument as ITabbedFileDocument;
      if (fileDoc == null)
        return;

      fileDoc.SaveAsContent(new FileInfo(sfd.FileName));
      this._recentFilesManager.Add(fileDoc.ContentFile);

      Global.Properties.SetValue<string>("LastDir", fileDoc.ContentFile.Directory.FullName);
    }
    #endregion

    #region public void FileSaveAll()
    public void FileSaveAll() {
      foreach (TabbedDocument document in Global.DockManager.GetDocumentArray()) {
        ITabbedFileDocument fileDoc = document as ITabbedFileDocument;
        if (fileDoc == null)
          continue;
        fileDoc.SaveContent();
      }
    }
    #endregion

    #region public void FileClose()
    public void FileClose() {
      TabbedDocument document = Global.DockManager.ActiveDocument as TabbedDocument;
      if (document == null)
        return;
      document.Close();
    }
    #endregion
  }
}