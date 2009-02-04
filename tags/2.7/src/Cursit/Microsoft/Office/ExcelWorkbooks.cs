/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Cursit.Microsoft.Office {
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Reflection;
  using System.IO;

  public class ExcelWorkbooks {

    private object _workbooksObject = null;
    private Excel _excel = null;

    internal ExcelWorkbooks(Excel excel) {
      _excel = excel;
      _workbooksObject = excel.ExcelObject.GetType().InvokeMember("Workbooks",
        BindingFlags.GetProperty, null, excel.ExcelObject, null);
    }

    #region internal object Workbooks
    internal object Workbooks {
      get { return _workbooksObject; }
    }
    #endregion

    #region public Excel Excel
    public Excel Excel {
      get { return this._excel; }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        try {
          object[] args = new object[] { };
          object count = _workbooksObject.GetType().InvokeMember(
            "Count", BindingFlags.GetProperty, null, _workbooksObject, args);
          if (count != null)
            return (int)count;
        } catch { }
        return 0;
      }
    }
    #endregion

    #region public ExcelWorkbook this[int index]
    public ExcelWorkbook this[int index]{
      get {
        try {
          object[] args = new object[] { index};
          // Получаем ссылку на первую книгу в коллекции Excel
          object workbook = _workbooksObject.GetType().InvokeMember(
            "Item", BindingFlags.GetProperty, null, _workbooksObject, args);
          if (workbook != null)
            return new ExcelWorkbook(this, workbook);
        } catch {}
        return null;
      }
    }
    #endregion

    #region public ExcelWorkbook this[string name]
    public ExcelWorkbook this[string name] {
      get {
        try {
          object[] args = new object[] { name};
          object workbook = _workbooksObject.GetType().InvokeMember(
            "Item", BindingFlags.GetProperty, null, _workbooksObject, args);
          if (workbook != null)
            return new ExcelWorkbook(this, workbook);
        } catch { }
        return null;
      }
    }
    #endregion

    #region public ExcelWorkbook Add()
    public ExcelWorkbook Add() {
      object[] args = new object[] {};

      object workbook = this._workbooksObject.GetType().InvokeMember("Add",
        BindingFlags.InvokeMethod, null, _workbooksObject, args);

      return new ExcelWorkbook(this, workbook);
    }
    #endregion

    #region public ExcelWorkbook OpenFile()
    public ExcelWorkbook OpenFile(string filename) {
      if (!File.Exists(filename))
        throw(new FileNotFoundException("File not found", filename));

      object[] args = new object[] { filename };

      object workbook = this._workbooksObject.GetType().InvokeMember("Open", 
        BindingFlags.InvokeMethod, null, _workbooksObject, args);
      return new ExcelWorkbook(this, workbook);
    }
    #endregion

  }
}
