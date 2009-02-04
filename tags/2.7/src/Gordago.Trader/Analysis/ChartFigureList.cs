/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Chart {
  public sealed class ChartFigureList {

    private List<ChartFigure> _figures;
    private ChartBox _chartBox;
    
    internal ChartFigureList(ChartBox chartBox) {
      _chartBox = chartBox;
      _figures = new List<ChartFigure>();
    }

    #region internal List<ChartFigure> Figures
    internal List<ChartFigure> Figures {
      get { return this._figures; }
    }
    #endregion

    #region public ChartFigure this[int index]
    public ChartFigure this[int index] {
      get { return this._figures[index]; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return this._figures.Count; }
    }
    #endregion

    #region public void Add(ChartFigure figure)
    public void Add(ChartFigure figure) {
      if(figure == null) return;

      for(int i = 0; i < _figures.Count; i++) {
        if(figure.Name == _figures[i].Name) {
          this.RemoveAt(figure.Name);
        }
      }
      figure.SetChartBox(this._chartBox);
      this._chartBox.PFScaleChanged = true;
      (this._chartBox.ChartManager as ChartManager).Invalidate();
      _figures.Add(figure);
    }
    #endregion

    #region public void RemoveAt(string name)
    public void RemoveAt(string name) {
      for (int i = 0; i < this._figures.Count; i++) {
        ChartFigure figure = _figures[i];
        if (figure.Name == name) {
          this.Remove(figure);
          return;
        } 
      }
    }
    #endregion

    #region public void Remove(ChartFigure figure)
    public void Remove(ChartFigure figure) {
      if (figure == null) return;

      ChartManager cm = this._chartBox.ChartManager as ChartManager;

      if(figure == cm.SelectedFigureMouse)
        cm.SelectFigureMouse(null);
      if(figure == cm.SelectedFigure)
        cm.SelectedFigure = null;

      if (_figures.Remove(figure)) {
        figure.OnDestroy();
        RefreshStatusSelected(figure);
      }
      this.RefreshFromRemove();
    }
    #endregion

    #region private void RefreshStatusSelected(ChartFigure figure)
    private void RefreshStatusSelected(ChartFigure figure) {
      ChartManager manager = _chartBox.ChartManager as ChartManager;
      if(manager.SelectedFigure == figure) {
        manager.SelectedFigure = null;
      }
    }
    #endregion

    #region private void RefreshFromRemove()
    private void RefreshFromRemove() {
      (this._chartBox.ChartManager as ChartManager).DeleteEmptyChartBox();
      this._chartBox.PFScaleChanged = true;
      (this._chartBox.ChartManager as ChartManager).Invalidate();
    }
    #endregion

    #region public void Insert(ChartFigure figure)
    public void Insert(ChartFigure figure) {
      this.Insert(0, figure);
    }
    #endregion

    #region public void Insert(int index, ChartFigure figure)
    public void Insert(int index, ChartFigure figure) {
      this._figures.Insert(index, figure);

      figure.SetChartBox(this._chartBox);
      this._chartBox.PFScaleChanged = true;
      (this._chartBox.ChartManager as ChartManager).Invalidate();
    }
    #endregion

    #region public ChartFigure[] GetFigures(Type type)
    public ChartFigure[] GetFigures(Type type) {
      List<ChartFigure> list = new List<ChartFigure>();
      for(int i = 0; i < _figures.Count; i++) {
        if(_figures[i].GetType() == type)
          list.Add(_figures[i]);
      }
      return list.ToArray();
    }
    #endregion

    #region public ChartFigure GetFigure(string name)
    public ChartFigure GetFigure(string name) {
      foreach(ChartFigure figure in this._figures) {
        if(figure.Name == name)
          return figure;
      }
      return null;
    }
    #endregion

    #region public ChartFigure GetFigure(ChartFigure figure)
    public ChartFigure GetFigure(ChartFigure figure) {
      for(int i = 0; i < this._figures.Count; i++) {
        if(this._figures[i] == figure)
          return figure;
      }
      return null;
    }
    #endregion

    #region public void Destroy()
    public void Destroy() {
      List<ChartFigure> figures = new List<ChartFigure>(this._figures.ToArray());
      foreach(ChartFigure figure in figures){
        figure.OnDestroy();
      }
    }
    #endregion


    public void ChangeIndex(ChartFigure figure, int newIndex) {
      if (this.GetFigure(figure) == null)
        return;

      _figures.Remove(figure);
      _figures.Insert(newIndex, figure);
    }
  }
}
