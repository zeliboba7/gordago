/**
* @version $Id: AppToolStripButton.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  public class AppToolStripButton: ToolStripButton, IAppAction{
    private readonly AppAction _action;

    public AppToolStripButton(AppAction action) {
      _action = action;

      switch (action) {
        case AppAction.ProjectNew:
          this.ToolTipText = "New Project";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.ProjectNew;
          break;
        case AppAction.ProjectOpen:
          this.ToolTipText = "Open Project";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.Open;
          break;
        case AppAction.ProjectSave:
          this.ToolTipText = "Save Project";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.Save;
          break;
        case AppAction.ProjectBuild:
          this.ToolTipText = "Build Project";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.Build;
          break;
        case AppAction.ViewProjectWindow:
          this.ToolTipText = "Project Explorer";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.ProjectExporerWindow;
          break;
        case AppAction.ViewOutputWindow:
          this.ToolTipText = "Output";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.OutputWindow;
          break;
        case AppAction.ViewStartPage:
          this.ToolTipText = "Start Page";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.StartPage;
          break;
        case AppAction.ViewErrorListWindow:
          this.ToolTipText = "Error List";
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.ErrorListWindow;
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
