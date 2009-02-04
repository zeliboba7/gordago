/**
* @version $Id: Figure.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;

  public abstract class Figure {

    public event MouseEventHandler MouseMove;
    public event MouseEventHandler MouseDown;
    public event MouseEventHandler MouseUp;

    private string _name = "Figure";
    private string _guid = Guid.NewGuid().ToString();
    private ChartBox _owner = null;
    private ChartFigureStyle _figureStyle;

    #region public string Name
    public string Name {
      get { return this._name; }
      set { this._name = value; }
    }
    #endregion

    #region protected ChartBox Owner
    protected ChartBox Owner {
      get { return _owner; }
    }
    #endregion

    #region public void Invalidate()
    public void Invalidate() {
      if (_owner == null)
        return;
      this._owner.Invalidate();
    }
    #endregion

    #region protected void SetStyle(ChartFigureStyle flag, bool value)
    protected void SetStyle(ChartFigureStyle flag, bool value) {
      _figureStyle = value ? (_figureStyle | flag) : (_figureStyle & ~flag);
    }
    #endregion

    #region public bool GetStyle(ChartFigureStyle flag)
    public bool GetStyle(ChartFigureStyle flag) {
      return ((_figureStyle & flag) == flag);
    }
    #endregion

    #region internal void SetChartBox(ChartBox owner)
    internal void SetChartBox(ChartBox owner) {
      if (_owner != null && _owner != owner) {
        // _owner.remo
      }

      this._owner = owner;
    }
    #endregion

    #region internal void WmPaint(ChartGraphics g)
    internal void WmPaint(ChartGraphics g) {
      this.OnPaint(g);
    }
    #endregion

    #region protected virtual void OnPaint(ChartGraphics g)
    protected virtual void OnPaint(ChartGraphics g) {
    }
    #endregion

    #region internal void WmMouseMove(MouseEventArgs e)
    internal void WmMouseMove(MouseEventArgs e) {
      this.OnMouseMove(e);
    }
    #endregion

    #region protected virtual void OnMouseMove(MouseEventArgs e)
    protected virtual void OnMouseMove(MouseEventArgs e) {
      if (this.MouseMove != null)
        this.MouseMove(this, e);
    }
    #endregion

    #region internal void WmMouseDown(MouseEventArgs e)
    internal void WmMouseDown(MouseEventArgs e) {
      this.OnMouseDown(e);
    }
    #endregion

    #region protected virtual void OnMouseDown(MouseEventArgs e)
    protected virtual void OnMouseDown(MouseEventArgs e) {
      if (this.MouseDown != null) {
        this.MouseDown(this, e);
      }
    }
    #endregion

    #region internal void WmMouseUp(MouseEventArgs e)
    internal void WmMouseUp(MouseEventArgs e) {
      this.OnMouseMove(e);
    }
    #endregion

    #region protected virtual void OnMouseUp(MouseEventArgs e)
    protected virtual void OnMouseUp(MouseEventArgs e) {
      if (this.MouseUp != null)
        this.MouseUp(this, e);
    }
    #endregion

    #region internal void WmCaclulateScale()
    internal void WmCaclulateScale() {
      if (this.GetStyle(ChartFigureStyle.CalculateScale))
        this.OnCalculateScale();
    }
    #endregion

    protected virtual void OnCalculateScale() { }
  }
}
