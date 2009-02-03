/**
* @version $Id: Program.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Gordago.Core;

  static class Program {

    [STAThread]
    static void Main() {

      try {
        Global.Properties = new EasyProperties();
        Global.Properties.Load(Global.Setup.AppConfigFile);
      } catch { }
      Global.Languages = new LanguageManager(Global.Setup.LanguagesDirectory, "Gordago.LangEditor");
      Global.Languages.Select(Global.Properties.GetValue<string>("Language", "en-US"));

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());

      try {
        Global.Properties.Save(Global.Setup.AppConfigFile);
        Global.Languages.BuildComplete();
      } catch { }
    }
  }
}