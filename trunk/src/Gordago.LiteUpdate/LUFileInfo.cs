/**
* @version $Id: LUFileInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public abstract class LUFileInfo {

    #region public enum LUFileAction
    public enum LUFileAction {
      Update, // Prefix "+"
      Delete // Prefix "-"
    }
    #endregion

    private LUFileAction _action = LUFileAction.Update;

    public LUFileInfo() {}

    public abstract int Id { get;}

    /// <summary>
    /// Sample: "exe,dll"
    /// </summary>
    public abstract string Extensions { get;}

    #region public LUFileAction Action
    public LUFileAction Action {
      get { return _action; }
      set { _action = value; }
    }
    #endregion

    protected virtual void OnBeforeUpdate() {}

    protected virtual void OnAfterUpdate() {}
  }
}
