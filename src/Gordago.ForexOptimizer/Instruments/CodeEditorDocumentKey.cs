/**
* @version $Id: CodeEditorDocumentKey.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using Gordago.Docking;
using System.IO;
  
  class CodeEditorDocumentKey : TabbedDocument.AbstractDocumentKey {
    private readonly int _hashCode;
    private readonly FileInfo _file;

    public CodeEditorDocumentKey(FileInfo file) {
      _hashCode = file.FullName.GetHashCode();
      _file = file;
    }

    #region public override int GetHashCode()
    public override int GetHashCode() {
      return _hashCode;
    }
    #endregion

    #region public override bool Equals(object obj)
    public override bool Equals(object obj) {
      if (!(obj is CodeEditorDocumentKey))
        return false;

      return (obj as CodeEditorDocumentKey)._hashCode == _hashCode;
    }
    #endregion

  }
}
