/**
* @version $Id: Global.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.IOTicks
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;
  using Gordago.Trader.Data;

  static class Global {
    public readonly static Setup Setup = new Setup();
    public readonly static EasyProperties Properties = new EasyProperties();
    public readonly static CommandManager CommandManager = new CommandManager();
    public readonly static HistoryManager History = new HistoryManager(Global.Setup.HistoryDirectory);
  }
}
