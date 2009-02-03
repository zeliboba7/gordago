/**
* @version $Id: Global.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;
  using Gordago.LangEditor.Docking;

  static class Global {
    public static SetupInfo Setup = new SetupInfo();

    public static LEDockManager DockManager;

    public static MainForm MainForm;
    public static EasyProperties Properties;

    public static LanguageManager Languages;
  }
}
