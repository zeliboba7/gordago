/**
* @version $Id: QuotesManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Quotes
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Trader;
  using Gordago.Trader.Data;
  using System.IO;

  class QuotesManager : HistoryManager {

    public QuotesManager()
      : base(Global.Setup.HistoryDirectory) {
      this.Load();
    }

  }
}
