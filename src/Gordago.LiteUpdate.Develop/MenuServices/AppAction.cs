/**
* @version $Id: AppAction.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public enum AppAction {
    ProjectNew,
    ProjectOpen,
    ProjectSave,
    ProjectSaveAs,
    ProjectSaveAll,
    ProjectClose,
    RecentProjects,
    ViewProjectWindow,
    ViewOutputWindow,
    ViewErrorListWindow,
    ViewStartPage,
    ProjectBuild,
    ProjectCheckVersion,
    AboutLiteUpdateDevelop,
    CheckForUpdates,
    AppExit
  }
}
