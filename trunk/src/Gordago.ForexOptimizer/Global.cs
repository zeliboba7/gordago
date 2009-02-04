/**
* @version $Id: Global.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO {
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;
  using Gordago.FO.Updates;
  using Gordago.Trader.Data;
  using Gordago.FO.Quotes;
  using Gordago.Trader.Indicators;
  using Gordago.FO.Instruments;

  static class Global {
    public static SetupInfo Setup = new SetupInfo();

    public static ExtDockManager DockManager;

    public static MainForm MainForm;
    public static EasyProperties Properties;

    public static LanguageManager Languages;
    public static AppUpdateManager UpdateManager = new AppUpdateManager();
    public static QuotesManager Quotes;

    public static IndicatorsManager Indicators;
    public static ProjectsManager Projects;

  }
}
