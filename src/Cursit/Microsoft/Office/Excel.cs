/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Cursit.Microsoft.Office {
  public class Excel {

    private object _excelObject;
    private OfficeError _error;
    private ExcelWorkbooks _workbooks;

    public Excel() {

      string appProgID = "Excel.Application";

      try {
        _excelObject = Marshal.GetActiveObject(appProgID);
      } catch { }

      if (_excelObject == null) {
        /* Запуск */
        try {
          Type excelType = Type.GetTypeFromProgID(appProgID);
          _excelObject = Activator.CreateInstance(excelType);
        } catch {
          _error = OfficeError.NotInstall;
        }
      } 
      if (_excelObject != null){
        _workbooks = new ExcelWorkbooks(this);
      }
    }

    #region ~Excel()
    ~Excel() {
      if (_excelObject == null)
        return;
      try {
        // Уничтожение объекта Excel.
        Marshal.ReleaseComObject(_excelObject);
        // Вызываем сборщик мусора для немедленной очистки памяти
        GC.GetTotalMemory(true);
      } catch { 
      }
    }
    #endregion

    #region internal object ExcelObject
    internal object ExcelObject {
      get { return _excelObject; }
    }
    #endregion

    #region public OfficeError Error
    public OfficeError Error {
      get {
        if (_error == OfficeError.None && _excelObject == null)
          return OfficeError.Unknow;
        return _error; 
      }
    }
    #endregion

    #region public ExcelWorkbooks Workbooks
    public ExcelWorkbooks Workbooks {
      get { return this._workbooks;}
    }
    #endregion

    #region public void Show()
    public void Show() {
      object value = _excelObject.GetType().InvokeMember(
       "Visible", BindingFlags.SetProperty, null, _excelObject, new object[] { true});
    }
    #endregion
  }
}
