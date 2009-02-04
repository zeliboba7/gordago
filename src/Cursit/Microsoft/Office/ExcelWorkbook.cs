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
  public class ExcelWorkbook {

    private object _workbookObject;
    private ExcelWorkbooks _workbooks;
    private ExcelWorksheets _worksheets;

    internal ExcelWorkbook(ExcelWorkbooks workbooks, object workbookObject) {
      _workbooks = workbooks;
      _workbookObject = workbookObject;
      _worksheets = new ExcelWorksheets(this);
    }

    #region public ExcelWorkbooks Workbooks
    public ExcelWorkbooks Workbooks {
      get { return this._workbooks; }
    }
    #endregion

    #region internal object WorkbookObject
    internal object WorkbookObject {
      get { return this._workbookObject; }
    }
    #endregion

    #region public void Close()
    /// <summary>
    /// Закрытие книги с принятием всех изменений
    /// </summary>
    public void Close() {
      // с принятием всех изменений
      object[] args = new object[] { true };
      // Пробуем закрыть книгу
      _workbookObject.GetType().InvokeMember(
        "Close", BindingFlags.InvokeMethod, null, _workbookObject, args);
      
      // Вариант 2. Закрываем книгу с принятием всех изменений
      //object[] args = new object[2];
      //args[0] = true;
      //// И под определенным названием
      //args[1] = @"D:\book2.xls";
      //// Пробуем закрыть книгу
      //workbook.GetType().InvokeMember(
      //  "Close", BindingFlags.InvokeMethod, null, workbook, args); 
    }
    #endregion

    #region public void Save()
    public void Save() {
      // Просто сохраняем книгу
      _workbookObject.GetType().InvokeMember(
        "Save", BindingFlags.InvokeMethod, null, _workbookObject, null);
    }
    #endregion

    #region public void SaveAs(string fileName)
    public void SaveAs(string fileName) {
      // Задаем параметры метода SaveAs - имя файла
      object[] args = new object[1]{fileName};

      // Сохраняем книгу в файле d:\d1.xls
      _workbookObject.GetType().InvokeMember(
        "SaveAs", BindingFlags.InvokeMethod, null, _workbookObject, args);
    }
    #endregion
  }
}
