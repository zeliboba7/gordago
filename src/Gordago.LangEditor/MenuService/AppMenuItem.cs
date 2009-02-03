/**
* @version $Id: AppMenuItem.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  class AppMenuItem : ToolStripMenuItem, IAppAction {
    private readonly AppAction _action;

    public AppMenuItem(AppAction appAction) {
      _action = appAction;
      switch (appAction) {
        case AppAction.AppExit:
          this.Text = Global.Languages["Menu"]["Exit"]; ;
          break;
        case AppAction.ViewAppList:
          this.Text = Global.Languages["Menu"]["Applications"];
          break;
        case AppAction.AppAbout:
          this.Text = Global.Languages["Menu"]["About"];
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
