/**
* @version $Id: ChartBox.ChartHorizontalScale.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Diagnostics;
  using System.Windows.Forms;
  using System.Drawing;
  using Gordago.Trader.Indicators;
  using Gordago.Trader.Common;
  

  partial class ChartBox {

    #region public enum ChartHorizontalScalePosition
    public enum ChartHorizontalScalePosition {
      Top, Bottom
    }
    #endregion

    public class ChartHorizontalScale:ChartBoxArea {
      #region enum MouseAction
      enum MouseAction {
        None,
        ChangeScale
      }
      #endregion

      #region internal struct GridLine
      internal struct GridLine {
        
        public int BarIndex;
        public int X;
        public string Desc;

        public GridLine(int barIndex, int x, string desc) {
          this.BarIndex = barIndex;
          this.X = x;
          this.Desc = desc;
        }
      }
      #endregion

      #region public enum HorizontalZoom : int
      public enum HorizontalZoom : int {
        Smaller = 0,
        Small = 1,
        Medium = 2,
        Larger = 3,
        Large = 4,
        BigLarge = 5
      }
      #endregion

      private int[] _zoomWidth = new int[] { 1, 2, 4, 8, 16, 32 };

      private int _viewCountBar;
      private int _minimumHeight = 30;
      private ChartHorizontalScalePosition _dock = ChartHorizontalScalePosition.Bottom;

      private HorizontalZoom _zoom = HorizontalZoom.Medium;
      private int _position;

      private readonly List<GridLine> _gridLines = new List<GridLine>();
      private MouseAction _action = MouseAction.None;
      private Point _savedMousePostion;

      internal ChartHorizontalScale(ChartBox owner):base(owner) { }

      #region public new int Height
      public new int Height {
        get {
          return Math.Max(base.Height, this.MinimumHeight);
        }
      }
      #endregion

      #region public new int Width
      public new int Width {
        get {
          return this.Owner.Area.Width;
        }
      }
      #endregion

      #region public int MinimumHeigh
      public int MinimumHeight {
        get { return _minimumHeight; }
        set {
          _minimumHeight = Math.Max(value, 3);
          if (_minimumHeight > this.Height) {
            this.SetBounds(this.Left, this.Top, this.Width, _minimumHeight);
          }
          this.Owner.LayoutChild();
        }
      }
      #endregion

      #region public ChartHorizontalScalePosition Dock
      public ChartHorizontalScalePosition Dock {
        get { return _dock; }
        set {
          this._dock = value;
          this.Owner.LayoutChild();
        }
      }
      #endregion

      #region public bool CanZoomIn
      /// <summary>
      /// Возможность увеличения масштаба
      /// </summary>
      /// <returns></returns>
      public bool CanZoomIn {
        get{
          return !((int)_zoom >= (int)HorizontalZoom.BigLarge);
        }
      }
      #endregion

      #region public bool CanZoomOut
      /// <summary>
      /// Возможность уменшения масштаба
      /// </summary>
      /// <returns></returns>
      public bool CanZoomOut {
        get{
          return !((int)_zoom <= (int)HorizontalZoom.Smaller);
        }
      }
      #endregion

      #region public HorizontalZoom Zoom
      public HorizontalZoom Zoom {
        get { return this._zoom; }
        set {
          if (_zoom == value)
            return;

          if ((int)value < (int)HorizontalZoom.Smaller)
            this._zoom = HorizontalZoom.Smaller;
          else if ((int)value > (int)HorizontalZoom.BigLarge)
            this._zoom = HorizontalZoom.BigLarge;
          else
            _zoom = value;

          this.Invalidate();
        }
      }
      #endregion

      #region public int Position
      public int Position {
        get { return this._position; }
        set {
          value = Math.Max(value, 0);
          if (_position == value)
            return;
          _position = value;
          this.Invalidate();
        }
      }
      #endregion

      #region public int DeltaX
      public int DeltaX {
        get { return _zoomWidth[(int)_zoom]; }
      }
      #endregion

      #region public int CountBarView
      public int CountBarView {
        get {
          return _viewCountBar;
        }
      }
      #endregion

      #region internal List<GridLine> GridLines
      internal List<GridLine> GridLines {
        get { return _gridLines; }
      }
      #endregion

      #region public int GetBarIndex(int x)
      public int GetBarIndex(int x) {
        int pos = (x + this.DeltaX / 2) / this.DeltaX + this.Position;

        return pos;
      }
      #endregion

      #region public int GetX(int barIndex)
      public int GetX(int barIndex) {
        int delta = barIndex - this.Position;
        return delta * this.DeltaX;
      }
      #endregion

      #region private bool Multiple(int value, int multiple)
      private bool Multiple(int value, int multiple) {
        double val = value;
        double mul = multiple;
        return val == (value / multiple) * multiple;
      }
      #endregion

      #region internal void CmCalculateScale()
      internal void CmCalculateScale() {
        _viewCountBar = this.Width / this.DeltaX;

        int dx = 64;
        int countGLine = this.Width / dx;
        _gridLines.Clear();


        iBars ibars = this.Owner.Owner.iBars;
        int bcount = this.Owner.Bars.Count;

        for (int i = 0; i < countGLine; i++) {
          int x = i * dx;
          int barIndex = this.Position + x;
          string desc = "";

          if (barIndex < bcount) {
            Bar bar = ibars.GetBar(barIndex);
            if (i == 0) {
              desc = bar.Time.ToString("d MMM yyyy");
            } else {
              desc = bar.Time.ToString("d MMM ") + bar.Time.ToShortTimeString();
            }
          }
          _gridLines.Add(new GridLine(barIndex, x, desc));
        }
      }
      #endregion

      #region public void ZoomIn()
      public void ZoomIn() {
        if (!CanZoomIn) return;
        int sc = (int)this.Zoom;
        sc++;
        this.Zoom = (HorizontalZoom)sc;
      }
      #endregion

      #region public void ZoomOut()
      public void ZoomOut() {
        if (!CanZoomOut) return;
        int scs = (int)this.Zoom;
        scs--;
        this.Zoom = (HorizontalZoom)scs;
      }
      #endregion

      #region protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
      protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) {
        if (e.Button == MouseButtons.Left && _action == MouseAction.ChangeScale) {

          int mx = _savedMousePostion.X;
          int emx = e.X;

          int deltaX = this.DeltaX;
          if (Math.Abs(mx - emx) < 10)
            return;
          if (mx < emx) {
            this.ZoomIn();
          } else {
            this.ZoomOut();
          }

          _savedMousePostion = e.Location;
        }
      }
      #endregion

      #region protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
      protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
          _savedMousePostion = e.Location;
          _action = MouseAction.ChangeScale;
          this.Owner.Cursor = Cursors.SizeWE;
        }
      }
      #endregion

      #region protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
      protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
        _action = MouseAction.None;
        this.Owner.Cursor = Cursors.Default;
      }
      #endregion

      #region protected override void OnPaint(ChartGraphics g)
      protected override void OnPaint(ChartGraphics g) {

        g.SelectBrush(this.Owner.BackColor);
        GdiFont gdiFont = g.SelectFont(this.Owner.ScaleFont);
        
        this.Owner.Area.SelectGridPen(g);
        g.SetTextColor(this.Owner.ForeColor);

        for (int i = 0; i < _gridLines.Count; i++) {
          GridLine gl = _gridLines[i];
          g.DrawLine(gl.X, 0, gl.X, 2);
          g.TextOut(gl.Desc, gl.X, 3);
        }
        
        //for (int i = 0; i < _viewCountBar; i++) {
        //  int x = i * this.DeltaX;
        //  int barindex = i + this.Position;
        //  if (Multiple(x, 32)) {
        //    //if (this.HorizontalScaleVisible && this.ChartManager.Bars != null && barindex < barcount) {

        //    //  string sc = "";
        //    //  DateTime dtm = this.ChartManager.Bars[barindex].Time;
        //    //  if (l) {
        //    //    sc = dtm.ToString("d MMM yyyy");
        //    //    l = false;
        //    //  } else
        //    //    sc = dtm.ToString("d MMM ") + dtm.ToShortTimeString();

        //    //  ghscale.DrawLine(_borderPen, _map[i], 0, _map[i], 2);
        //    //  ghscale.DrawString(sc, Manager.Style.ScaleFont, _scaleForeBrush, _map[i], 2);
        //    //}

        //  }
        //}
      }
      #endregion
    }
  }
}
