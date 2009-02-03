/**
* @version $Id: TabbedDocument.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Docking
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using WeifenLuo.WinFormsUI.Docking;

  public class TabbedDocument : DockContent {
    public abstract class AbstractDocumentKey {}

    public TabbedDocument() {
      this.DockAreas = DockAreas.Document; 
    }
   
    private AbstractDocumentKey _key;

    #region public AbstractDocumentKey Key
    public AbstractDocumentKey Key {
      get { return _key; }
    }
    #endregion

    #region protected void SetKey(AbstractDocumentKey key)
    protected void SetKey(AbstractDocumentKey key) {
      _key = key;
    }
    #endregion
  }
}
