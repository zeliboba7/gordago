/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Chart {

  #region public class ChartPanelEventArgs : EventArgs
  public class ChartPanelEventArgs : EventArgs {
    private ChartPanel _chartPanel;
    public ChartPanelEventArgs(ChartPanel chartPanel) {
      _chartPanel = chartPanel;
    }

    #region public ChartPanel ChartPanel
    public ChartPanel ChartPanel {
      get { return this._chartPanel; }
    }
    #endregion
  }
  #endregion

  public class ChartPanelList {

    private List<ChartPanel> _panels;
    private ChartManager _chartManager;

    public event EventHandler<ChartPanelEventArgs> PanelAdded;
    
    internal ChartPanelList(ChartManager chartManager) {
      _chartManager = chartManager;
      _panels = new List<ChartPanel>();
    }

    #region public int Count
    public int Count {
      get { return this._panels.Count; }
    }
    #endregion

    #region public ChartPanel this[int index]
    public ChartPanel this[int index] {
      get { return this._panels[index]; }
    }
    #endregion

    #region public ChartPanel this[string typeFullName]
    public ChartPanel this[string typeFullName] {
      get {
        for (int i = 0; i < this._panels.Count; i++) {
          if (_panels[i].GetType().FullName == typeFullName)
            return _panels[i];
        }
        return null;
      }
    }
    #endregion

    #region public ChartPanel this[Type type]
    public ChartPanel this[Type type] {
      get {
        for (int i = 0; i < this._panels.Count; i++) {
          if (_panels[i].GetType() == type)
            return _panels[i];
        }
        return null;
      }
    }
    #endregion

    #region public bool Add(ChartPanel chartPanel)
    public bool Add(ChartPanel chartPanel) {
      for (int i = 0; i < _panels.Count; i++) {
        if (_panels[i].GetType().FullName == chartPanel.GetType().FullName) {
          return false;
        }
      }
      _panels.Add(chartPanel);
      ChartPanelContainer cp = new ChartPanelContainer(chartPanel);
      this._chartManager.Controls.Add(cp);
      if (this.PanelAdded != null)
        this.PanelAdded(this, new ChartPanelEventArgs(chartPanel));
      return true;
    }
    #endregion

    #region public void RemoveAt(string typeFullName)
    public void RemoveAt(string typeFullName) {
      ChartPanel chartPanel = this[typeFullName];
      if (chartPanel == null)
        return;
      this.Remove(chartPanel);
    }
    #endregion

    #region public void Remove(ChartPanel chartPanel)
    public void Remove(ChartPanel chartPanel) {
      if (chartPanel == null)
        return;
      _panels.Remove(chartPanel);
      if (chartPanel.Parent != null && chartPanel.Parent is ChartPanelContainer) 
        _chartManager.Controls.Remove(chartPanel.Parent);
    }
    #endregion
  }
}
