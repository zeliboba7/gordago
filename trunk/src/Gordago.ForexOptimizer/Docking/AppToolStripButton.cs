/**
* @version $Id: AppToolStripButton.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.Windows.Forms;

  class AppToolStripButton : ToolStripButton, IAppAction {
    private readonly AppAction _action;

    public AppToolStripButton(AppAction action) {
      _action = action;

      switch (action) {
        case AppAction.FileNew:
          this.ToolTipText = Global.Languages["Menu/File"]["New..."];
          this.Image = global::Gordago.FO.Properties.Resources.ProjectNew;
          break;
        case AppAction.FileOpen:
          this.ToolTipText = Global.Languages["Menu/File"]["Open"];
          this.Image = global::Gordago.FO.Properties.Resources.Open;
          break;
        case AppAction.FileSave:
          this.ToolTipText = Global.Languages["Menu/File"]["Save"];
          this.Image = global::Gordago.FO.Properties.Resources.Save;
          break;
        case AppAction.FileSaveAll:
          this.ToolTipText = Global.Languages["Menu/File"]["Save All"];
          this.Image = global::Gordago.FO.Properties.Resources.SaveAll;
          break;
      }
    }

    #region public AppAction Action
    public AppAction Action {
      get { return _action; }
    }
    #endregion
  }
}
