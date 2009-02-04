/**
* @version $Id: ChartBox.ChartBoxArea.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  
  using System.Diagnostics;
  using System.Windows.Forms;

  partial class ChartBox {
    
    public abstract class ChartBoxArea {

      private int _x = 0, _y = 0, _width = 0, _height = 0;
      private readonly ChartBox _owner;
      private bool _visible = true;

      internal ChartBoxArea(ChartBox  owner) {
        _owner = owner;
      }

      #region public ChartBox Owner
      public ChartBox Owner {
        get { return _owner; }
      }
      #endregion

      #region public int Left
      public int Left {
        get { return _x; }
      }
      #endregion

      #region public int Top
      public int Top {
        get { return _y; }
      }
      #endregion

      #region public int Right
      public int Right {
        get { return _x + _width; }
      }
      #endregion

      #region public int Bottom
      public int Bottom {
        get { return _y + _height; }
      }
      #endregion

      #region public int Width
      public int Width {
        get { return this._width; }
      }
      #endregion

      #region public int Height
      public int Height {
        get { return this._height; }
      }
      #endregion

      #region public Size Size
      public Size Size {
        get { return new Size(_width, _height); }
      }
      #endregion

      #region public bool Visible
      public bool Visible {
        get { return this._visible; }
        set {
          _visible = value;
          this.Owner.LayoutChild();
        }
      }
      #endregion

      #region public Rectangle Bounds
      public Rectangle Bounds {
        get { return new Rectangle(_x, _y, _width, _height); }
      }
      #endregion

      #region internal void SetBounds(int x, int y, int width, int height)
      internal void SetBounds(int x, int y, int width, int height) {
        if (_x == x && _y == y && _width == width && _height == height)
          return;
        _x = x;
        _y = y;
        _width = width;
        _height = height;
        _width = Math.Max(_width, 10);
        _height = Math.Max(_height, 10);
      }
      #endregion

      #region internal void WmPaint(ChartGraphics g)
      internal void WmPaint(ChartGraphics g) {
        Rectangle rect = new Rectangle(this.Left, this.Top, this.Width, this.Height);
        g.CreatePaintWindow(rect);
        try {
          if (this.Owner.Owner.Bars != null && this.Owner.Owner.Bars.Count > 0) {
            this.OnPaint(g);
          }
          
        } finally {
          g.RestorePaintWindow();
        }
      }
      #endregion

      protected abstract void OnPaint(ChartGraphics g);

      #region internal void WmMouseMove(MouseEventArgs e)
      internal void WmMouseMove(MouseEventArgs e) {
        this.OnMouseMove(e);
      }
      #endregion

      protected virtual void OnMouseMove(MouseEventArgs e) {
      }

      #region internal void WmMouseDown(MouseEventArgs e)
      internal void WmMouseDown(MouseEventArgs e) {
        this.OnMouseDown(e);
      }
      #endregion

      protected virtual void OnMouseDown(MouseEventArgs e) {
      }

      #region internal void WmMouseUp(MouseEventArgs e)
      internal void WmMouseUp(MouseEventArgs e) {
        this.OnMouseMove(e);
      }
      #endregion

      #region protected virtual void OnMouseUp(MouseEventArgs e)
      protected virtual void OnMouseUp(MouseEventArgs e) {
      }
      #endregion

      #region public void Invalidate()
      public void Invalidate() {
        if (Owner == null)
          return;
        this.Owner.Invalidate();
      }
      #endregion
    }
  }
}
