/**
* @version $Id: AppMenuItem.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public class AppMenuItem : ToolStripMenuItem, IAppAction {
    private readonly AppAction _action;

    public AppMenuItem(AppAction appAction) {
      _action = appAction;
      switch (appAction) {
        case AppAction.AppExit:
          this.Text = Global.Languages["Menu/File"]["Exit"]; 
          break;
        case AppAction.ProjectNew:
          this.Text = Global.Languages["Menu/File"]["New..."]; 
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.ProjectNew;
          break;
        case AppAction.RecentProjects:
          this.Text = Global.Languages["Menu/File"]["Recent Projects"];
          break;
        case AppAction.ProjectOpen:
          this.Text = Global.Languages["Menu/File"]["Open..."];
          break;
        case AppAction.ProjectSave:
          this.Text = Global.Languages["Menu/File"]["Save"];
          break;
        case AppAction.ProjectSaveAll:
          this.Text = Global.Languages["Menu/File"]["Save All"];
          break;
        case AppAction.ProjectSaveAs:
          this.Text = Global.Languages["Menu/File"]["Save As..."];
          break;
        case AppAction.ProjectClose:
          this.Text = Global.Languages["Menu/File"]["Close"];
          break;
        case AppAction.ProjectBuild:
          this.Text = Global.Languages["Menu/Project"]["Build"];
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.Build;
          break;
        case AppAction.ProjectCheckVersion:
          this.Text = Global.Languages["Menu/Project"]["Check Version..."];
          break;
        case AppAction.ViewProjectWindow:
          this.Text = Global.Languages["Menu/View"]["Project Explorer"];
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.ProjectExporerWindow;
          break;
        case AppAction.ViewOutputWindow:
          this.Text = Global.Languages["Menu/View"]["Output"];
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.OutputWindow;
          break;
        case AppAction.ViewStartPage:
          this.Text = Global.Languages["Menu/View"]["Start Page"];
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.StartPage;
          break;
        case AppAction.ViewErrorListWindow:
          this.Text = Global.Languages["Menu/View"]["Error List"];
          this.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.ErrorListWindow;
          break;
        case AppAction.CheckForUpdates:
          this.Text = Global.Languages["Menu/Help"]["Check for Updates"];
          break;
        case AppAction.AboutLiteUpdateDevelop:
          this.Text = Global.Languages["Menu/Help"]["About LiteUpdate Delevop"];
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
