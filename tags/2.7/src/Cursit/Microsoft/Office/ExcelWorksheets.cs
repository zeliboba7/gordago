/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Cursit.Microsoft.Office {
  public class ExcelWorksheets {
    
    private object _worksheetsObject;
    private ExcelWorkbook _excelWorkbook;

    internal ExcelWorksheets(ExcelWorkbook workbook) {
      _excelWorkbook = workbook;
      _worksheetsObject = workbook.WorkbookObject.GetType().InvokeMember(
        "Worksheets", BindingFlags.GetProperty, null, workbook.WorkbookObject, null);
    }

    #region public ExcelWorkbook Workbook
    public ExcelWorkbook Workbook {
      get { return this._excelWorkbook; }
    }
    #endregion

    #region internal object WorksheetsObject
    internal object WorksheetsObject {
      get { return this._worksheetsObject; }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        try {
          object count = _worksheetsObject.GetType().InvokeMember(
            "Count", BindingFlags.GetProperty, null, _worksheetsObject, null);
          if (count != null)
            return (int)count;
        } catch { }
        return 0;
      }
    }
    #endregion

    #region public ExcelWorksheet this[int index]
    public ExcelWorksheet this[int index] {
      get {
        try {
          object[] args = new object[] { index };
          // Получаем ссылку на эту страницу
          object worksheet = _worksheetsObject.GetType().InvokeMember(
            "Item", BindingFlags.GetProperty, null, _worksheetsObject, args);
          if (worksheet != null)
            return new ExcelWorksheet(worksheet);
        } catch { }
        return null;
      }
    }
    #endregion

    #region public ExcelWorksheet this[string name]
    public ExcelWorksheet this[string name]{
      get {
        try {
          object[] args = new object[] { name };
          // Получаем ссылку на эту страницу
          object worksheet = _worksheetsObject.GetType().InvokeMember(
            "Item", BindingFlags.GetProperty, null, _worksheetsObject, args);
          if (worksheet != null)
            return new ExcelWorksheet(worksheet);
        } catch { }
        return null;
      }
    }
    #endregion

  }
}
