/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Chart {

  public class ChartPanelManager {

    public event EventHandler<TypeEventArgs> TypeRegister;

    private List<Type> _chartPanelTypes;

    public ChartPanelManager() {
      _chartPanelTypes = new List<Type>();
    }

    #region public int Count
    public int Count {
      get { return this._chartPanelTypes.Count; }
    }
    #endregion

    #region public Type this[int index]
    public Type this[int index] {
      get { return this._chartPanelTypes[index]; }
    }
    #endregion

    #region public bool Register(Type type)
    public bool Register(Type type) {
      if (type.BaseType != typeof(ChartPanel) && !type.IsPublic)
        return false;
      for (int i = 0; i < _chartPanelTypes.Count; i++) {
        if (type == _chartPanelTypes[i])
          return false;
      }
      _chartPanelTypes.Add(type);
      if (this.TypeRegister != null) {
        this.TypeRegister(this, new TypeEventArgs(type));
      }
      return true;
    }
    #endregion

    #region public ChartPanel Create(string typeFullName, string name)
    public ChartPanel Create(string typeFullName, string name) {
      for (int i = 0; i < _chartPanelTypes.Count; i++) {
        if (typeFullName == _chartPanelTypes[i].FullName) {
          ChartPanel chartPanel = Activator.CreateInstance(_chartPanelTypes[i]) as ChartPanel;
          return chartPanel;
        }
      }
      return null;
    }
    #endregion
  }
}
