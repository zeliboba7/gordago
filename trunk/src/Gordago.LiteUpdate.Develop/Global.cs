/**
* @version $Id: Global.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.LiteUpdate.Develop.Projects;
  using WeifenLuo.WinFormsUI.Docking;
  using Gordago.LiteUpdate.Develop.Docking;
  using Gordago.Core;
  using Gordago.LiteUpdate.Develop.Updates;

  static class Global {

    public static SetupInfo Setup = new SetupInfo();
    public static IDE MainForm;
    public static EasyProperties Properties;
    public static BuildManager Builder = new BuildManager();

    public static LUDockManager DockManager;

    public static AppUpdateManager UpdateManager = new AppUpdateManager();

    public static LanguageManager Languages;
  }
}
