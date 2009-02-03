/**
* @version $Id: ChartBox.FigureCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting {
  using System;
  using System.Collections.Generic;
  using System.Text;

  partial class ChartBox
  {

    #region protected virtual void OnFigureAdded(FigureEventArgs e)
    protected virtual void OnFigureAdded(FigureEventArgs e) {
      if (this.FigureAdded != null)
        this.FigureAdded(this, e);
    }
    #endregion

    #region public virtual void OnFigureRemoved(FigureEventArgs e)
    public virtual void OnFigureRemoved(FigureEventArgs e) {
      if (this.FigureRemoved != null)
        this.FigureRemoved(this, e);
    }
    #endregion

    #region internal void Add(Figure figure)
    internal void Add(Figure figure) {
      _figures.Add(figure);
      figure.SetChartBox(this);
      this._figureCollection.IncrementSession();
      this.OnFigureAdded(new FigureEventArgs(figure));
      this.Invalidate();
    }
    #endregion

    #region internal bool Remove(Figure figure)
    internal bool Remove(Figure figure) {
      bool ret = _figures.Remove(figure);
      this._figureCollection.IncrementSession();
      this.OnFigureRemoved(new FigureEventArgs(figure));
      this.Invalidate();
      return ret;
    }
    #endregion

    #region internal void Clear()
    internal void Clear() {
      while (_figures.Count > 0) {
        _figures.Remove(_figures[0]);
      }
    }
    #endregion

    #region public class FigureCollection:ICollection<Figure>
    public class FigureCollection:ICollection<Figure> {

      private int _sessionId = 0;

      
      private readonly ChartBox Owner;
      private readonly List<Figure> _figuresCalculateScale = new List<Figure>();

      #region internal FigureCollection(ChartBox owner)
      internal FigureCollection(ChartBox owner) {
        Owner = owner;
      }
      #endregion

      #region public int SessionId
      public int SessionId {
        get { return this._sessionId; }
      }
      #endregion

      public Figure this[int index] {
        get { return this.Owner._figures[index];}
      }

      #region internal List<Figure> FiguresCalcScale
      internal List<Figure> FiguresCalcScale {
        get {
          return this._figuresCalculateScale; 
        }
      }
      #endregion

      #region internal void IncrementSession()
      internal void IncrementSession() {
        _sessionId++;
        _figuresCalculateScale.Clear();
        for (int i = 0; i < this.Count; i++) {
          Figure figure = this.Owner._figures[i];
          if (figure.GetStyle(ChartFigureStyle.CalculateScale)) {
            _figuresCalculateScale.Add(figure);
          }
        }
      }
      #endregion

      #region public void Add(Figure item)
      public void Add(Figure item) {
        this.Owner.Add(item);
      }
      #endregion

      #region public void Clear()
      public void Clear() {
        this.Owner.Clear();
      }
      #endregion

      #region public bool Contains(Figure item)
      public bool Contains(Figure item) {
        return Owner._figures.Contains(item);
      }
      #endregion

      #region public void CopyTo(Figure[] array, int arrayIndex)
      public void CopyTo(Figure[] array, int arrayIndex) {
        Owner._figures.CopyTo(array, arrayIndex);
      }
      #endregion

      #region public int Count
      public int Count {
        get { return Owner._figures.Count; }
      }
      #endregion

      #region public bool IsReadOnly
      public bool IsReadOnly {
        get { return false; }
      }
      #endregion

      #region public bool Remove(Figure item)
      public bool Remove(Figure item) {
        return Owner.Remove(item);
      }
      #endregion

      #region public IEnumerator<Figure> GetEnumerator()
      public IEnumerator<Figure> GetEnumerator() {
        return Owner._figures.GetEnumerator();
      }
      #endregion

      #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        throw new Exception("The method or operation is not implemented.");
      }
      #endregion
    }
    #endregion
  }
}
