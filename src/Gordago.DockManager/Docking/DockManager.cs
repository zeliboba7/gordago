/**
* @version $Id: DockManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Docking
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;
  using WeifenLuo.WinFormsUI.Docking;

  public class DockManager : DockPanel {

    private readonly Dictionary<TabbedDocument.AbstractDocumentKey, TabbedDocument> _documents = new Dictionary<TabbedDocument.AbstractDocumentKey, TabbedDocument>();
    private readonly Dictionary<Type, DockWindowPanel> _windows = new Dictionary<Type, DockWindowPanel>();

    public DockManager() {
    }

    #region protected Dictionary<TabbedDocument.AbstractDocumentKey, TabbedDocument> TabbedDocuments
    protected Dictionary<TabbedDocument.AbstractDocumentKey, TabbedDocument> TabbedDocuments {
      get { return _documents; }
    }
    #endregion

    #region protected Dictionary<Type, DockWindowPanel> WindowPanels
    protected Dictionary<Type, DockWindowPanel> WindowPanels {
      get { return _windows; }
    }
    #endregion

    #region protected override void OnContentAdded(DockContentEventArgs e)
    protected override void OnContentAdded(DockContentEventArgs e) {
      if (e.Content is TabbedDocument) {
        TabbedDocument document = e.Content as TabbedDocument;
        _documents.Add(document.Key, document);
      } else if (e.Content is DockWindowPanel) {
        _windows.Add(e.Content.GetType(), e.Content as DockWindowPanel);
      }

      base.OnContentAdded(e);
    }
    #endregion

    #region protected override void OnContentRemoved(DockContentEventArgs e)
    protected override void OnContentRemoved(DockContentEventArgs e) {
      if (e.Content is TabbedDocument) {
        TabbedDocument document = e.Content as TabbedDocument;
        _documents.Remove(document.Key);
      } else if (e.Content is DockWindowPanel) {
        _windows.Remove(e.Content.GetType());
      }
      base.OnContentRemoved(e);
    }
    #endregion

    #region public void CloseAllDocument()
    public void CloseAllDocuments() {
      List<TabbedDocument> list = new List<TabbedDocument>();
      foreach (TabbedDocument document in _documents.Values) {
        list.Add(document);
      }

      foreach (TabbedDocument document in list)
        document.Close();
    }
    #endregion

    #region public DockTabbedDocument GetDocument(DockTabbedDocument.AbstractDocumentKey key)
    public TabbedDocument GetDocument(TabbedDocument.AbstractDocumentKey key) {
      TabbedDocument document = null;
      _documents.TryGetValue(key, out document);
      return document;
    }
    #endregion

    #region public DockWindowPanel GetWindow(Type type)
    public DockWindowPanel GetWindow(Type type) {
      
      DockWindowPanel window = null;

      _windows.TryGetValue(type, out window);
      return window;
    }
    #endregion

    #region public TabbedDocument[] GetDocumentArray()
    public TabbedDocument[] GetDocumentArray() {
      List<TabbedDocument> list = new List<TabbedDocument>();
      foreach (TabbedDocument document in _documents.Values) {
        list.Add(document);
      }
      return list.ToArray();
    }
    #endregion
  }
}
