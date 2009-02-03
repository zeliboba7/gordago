/**
* @version $Id: AppAction.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.AppStructureEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  enum AppAction {
    ProjectOpen,
    ProjectClose,
    AppExit
  }

  interface IAppAction {
    AppAction Action { get;}
    bool Enabled { get;set;}
  }

  class AppMenuItem : ToolStripMenuItem, IAppAction {
    private readonly AppAction _action;

    public AppMenuItem(AppAction appAction) {
      _action = appAction;
      switch (appAction) {
        case AppAction.AppExit:
          this.Text = "Exit";
          break;
        case AppAction.ProjectOpen:
          this.Text = "Open";
          break;
      }
    }

    #region public AppAction Action
    public AppAction Action {
      get { return _action; }
    }
    #endregion

    #region public new bool Enabled
    public new bool Enabled {
      get {
        return base.Enabled;
      }
      set {
        base.Enabled = value;
      }
    }
    #endregion
  }
}
