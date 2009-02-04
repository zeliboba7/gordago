/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Chart {

  #region public class TypeEventArgs:EventArgs
  public class TypeEventArgs:EventArgs {
    private Type _type;

    public TypeEventArgs(Type type) {
      _type = type;
    }

    #region public Type Type
    public Type Type {
      get { return this._type;}
    }
    #endregion
  }
  #endregion

  /// <summary>
  /// Менеджер графический обьектов
  /// </summary>
  public class ChartObjectManager {

    public event EventHandler<TypeEventArgs> TypeRegister;

    private List<Type> _chartObjectTypes;

    public ChartObjectManager() {
      _chartObjectTypes = new List<Type>();
    }

    #region public int Count
    public int Count {
      get { return this._chartObjectTypes.Count; }
    }
    #endregion

    #region public Type this[int index]
    public Type this[int index] {
      get { return _chartObjectTypes[index]; }
    }
    #endregion

    #region public bool Register(Type type)
    public bool Register(Type type) {
      if(type.BaseType != typeof(ChartObject) && !type.IsPublic)
        return false;
      for(int i = 0; i < _chartObjectTypes.Count; i++) {
        if(type == _chartObjectTypes[i])
          return false;
      }
      _chartObjectTypes.Add(type);
      if(this.TypeRegister != null) {
        this.TypeRegister(this, new TypeEventArgs(type));
      }
      return true;
    }
    #endregion

    #region public ChartObject Create(string typeFullName, string name)
    public ChartObject Create(string typeFullName, string name) {
      for(int i = 0; i < _chartObjectTypes.Count; i++) {
        if(typeFullName == _chartObjectTypes[i].FullName) {
          ChartObject chartObject = Activator.CreateInstance(_chartObjectTypes[i], name) as ChartObject;
          return chartObject;
        }
      }
      return null;
    }
    #endregion

    #region public ChartObject Create(string typeFullName)
    public ChartObject Create(string typeFullName) {
      string gui = "";
      string[] sa = typeFullName.Split('.');
      gui = sa[sa.Length - 1];
      sa = Guid.NewGuid().ToString().Split('-');
      gui = gui+"_"+ sa[sa.Length - 1];

      return this.Create(typeFullName, gui);
    }
    #endregion
  }
}
