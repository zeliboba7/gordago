/**
* @version $Id: Program.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using System.Drawing;
  using Gordago.Core;

  static class Program {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
      //System.IO.FileInfo file = new System.IO.FileInfo(System.IO.Path.Combine(Global.Setup.OptionsDirectory.FullName, "LiteUpdate.Develop.Properties.xml"));

      try {
        Global.Properties = new EasyProperties();
        Global.Properties.Load(Global.Setup.AppConfigFile);
      } catch { }

      Global.Languages = new LanguageManager(Global.Setup.LanguagesDirectory, "Gordago.LiteUpdate.Develop");
      Global.Languages.Select(Global.Properties.GetValue<string>("Language", "en-US"));

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new IDE());

      try {
        Global.Properties.Save(Global.Setup.AppConfigFile);
        Global.Languages.BuildComplete();
      } catch { }
    }
  }
}