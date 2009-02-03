/**
* @version $Id: EditorDocumentKey.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Docking;
  using Gordago.LangEditor.AppList;

  class EditorDocumentKey : TabbedDocument.AbstractDocumentKey {
    private readonly int _hashCode;

    private readonly AppItem _appItem;

    public EditorDocumentKey(AppItem appItem) {
      _appItem = appItem;

      string str = appItem.AppId;
      _hashCode = str.GetHashCode();
    }

    #region public AppItem AppItem
    public AppItem AppItem {
      get { return _appItem; }
    }
    #endregion

    #region public override int GetHashCode()
    public override int GetHashCode() {
      return _hashCode;
    }
    #endregion

    #region public override bool Equals(object obj)
    public override bool Equals(object obj) {
      if (!(obj is EditorDocumentKey))
        return false;

      return (obj as EditorDocumentKey)._hashCode == _hashCode;
    }
    #endregion
  }
}
