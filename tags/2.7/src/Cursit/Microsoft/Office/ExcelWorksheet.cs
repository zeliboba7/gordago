/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Cursit.Microsoft.Office {
  public class ExcelWorksheet {
    private object _worksheetObject;

    internal ExcelWorksheet(object worksheetObject) {
      _worksheetObject = worksheetObject;
    }

    #region internal object WorksheetObject
    internal object WorksheetObject {
      get { return _worksheetObject; }
    }
    #endregion
  }
}
