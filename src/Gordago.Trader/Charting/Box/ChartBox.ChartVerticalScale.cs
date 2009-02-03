/**
* @version $Id: ChartBox.ChartVerticalScale.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Collections;
  using System.Drawing;
  using System.Windows.Forms;
  using Gordago.Trader.Common;

  partial class ChartBox {

    #region public enum ChartVerticalScalePosition
    public enum ChartVerticalScalePosition {
      Left, Right
    }
    #endregion

    public class ChartVerticalScale : ChartBoxArea {
      
      #region internal struct GridLine
      internal struct GridLine {
        public float Value;
        public int Y;
        public string Desc;
        public GridLine(float value, int y, string desc) {
          this.Value = value;
          this.Y = y;
          this.Desc = desc;
        }
      }
      #endregion

      #region internal enum MouseAction
      internal enum MouseAction {
        None, 
        ChangeScale
      }
      #endregion

      private readonly List<GridLine> _gridLines = new List<GridLine>();

      private float _minValue = 0, _maxValue = 1;

      private bool _isFixedScaleValues = false;
      private ValueCollection _fixedScaleValues = null;
      private ChartVerticalScalePosition _dock = ChartVerticalScalePosition.Right;

      private int _digits = 0;
      private int _widht = 40;

      /// <summary>
      /// Масштаб в процентах
      /// </summary>
      private float _zoom = 0.05F;
      private float _minValueZoom, _maxValueZoom, _calcul;
      private int _savedHeight;

      private MouseAction _action = MouseAction.None;
      private Point _savedMousePostion;

      internal ChartVerticalScale(ChartBox owner)
        : base(owner) {
      }

      #region public float Zoom
      public float Zoom {
        get { return _zoom; }
        set {
          value = Math.Max(value, 0);
          bool evt = _zoom != value;
          this._zoom = value;
          if (evt)
            this.Owner.Invalidate();
        }
      }
      #endregion

      #region public new int Width
      public new int Width {
        get { return _widht; }
        set { this._widht = value; }
      }
      #endregion

      #region public new Size Size
      public new Size Size {
        get { return new Size(this.Width, this.Height); }
      }
      #endregion

      #region public ChartVerticalScalePosition Dock
      public ChartVerticalScalePosition Dock {
        get { return _dock; }
        set {
          this._dock = value;
          this.Owner.LayoutChild();
        }
      }
      #endregion

      #region public bool IsFixedScaleValue
      public bool IsFixedScaleValue {
        get { return this._isFixedScaleValues; }
        set { this._isFixedScaleValues = value; }
      }
      #endregion

      #region private List<Figure> Figures
      private List<Figure> Figures {
        get { return this.Owner.Figures.FiguresCalcScale; }
      }
      #endregion

      #region public ChartVerticalScaleValueCollection FixedScaleValues
      public ValueCollection FixedScaleValues {
        get { return this._fixedScaleValues; }
      }
      #endregion

      #region internal List<GridLine> GridLines
      internal List<GridLine> GridLines {
        get { return _gridLines; }
      }
      #endregion

      #region public class ValueCollection : ICollection<ChartVerticalScaleValue>
      public class ValueCollection : ICollection<ChartVerticalScaleValue> {

        private readonly List<ChartVerticalScaleValue> _list = new List<ChartVerticalScaleValue>();


        public void Add(ChartVerticalScaleValue item) {
          _list.Add(item);
        }

        public void Clear() {
          _list.Clear();
        }

        public bool Contains(ChartVerticalScaleValue item) {
          return _list.Contains(item);
        }

        public void CopyTo(ChartVerticalScaleValue[] array, int arrayIndex) {
          _list.CopyTo(array, arrayIndex);
        }

        public int Count {
          get { return _list.Count; }
        }

        #region public bool IsReadOnly
        public bool IsReadOnly {
          get { return false; }
        }
        #endregion

        #region public bool Remove(ChartVerticalScaleValue item)
        public bool Remove(ChartVerticalScaleValue item) {
          return _list.Remove(item);
        }
        #endregion

        #region public IEnumerator<ChartVerticalScaleValue> GetEnumerator()
        public IEnumerator<ChartVerticalScaleValue> GetEnumerator() {
          return _list.GetEnumerator();
        }
        #endregion

        #region IEnumerator IEnumerable.GetEnumerator()
        IEnumerator IEnumerable.GetEnumerator() {
          return _list.GetEnumerator();
        }
        #endregion
      }
      #endregion

      #region public void SetScaleValue(float minValue, float maxValue, int digits)
      public void SetScaleValue(float minValue, float maxValue, int digits) {
        _minValue = minValue;
        _maxValue = maxValue;
        _digits = digits;
      }
      #endregion

      #region public int GetY(float val)
      public int GetY(float val) {
        return Convert.ToInt32(_calcul * (_maxValueZoom - val));
      }
      #endregion

      #region internal void CmCalculateScale()
      internal void CmCalculateScale() {
        float savedMinValue = _minValueZoom;
        float savedMaxValue = _maxValueZoom;

        for (int i = 0; i < this.Figures.Count; i++) {
          this.Figures[i].WmCaclulateScale();
        }

        float razn = _maxValue - _minValue;
        float zoom = razn * _zoom;

        float minValueZoom = _minValue - zoom;
        float maxValueZoom = _maxValue + zoom;

        if (float.IsInfinity(minValueZoom) || float.IsInfinity(maxValueZoom) ||
          float.IsNaN(minValueZoom) || float.IsNaN(maxValueZoom)) {
          _minValueZoom = savedMinValue;
          _maxValueZoom = savedMaxValue;
          return;
        }

        _minValueZoom = minValueZoom;
        _maxValueZoom = maxValueZoom;

        bool evt = savedMinValue != _minValueZoom || savedMaxValue != _maxValueZoom ||
          _savedHeight != this.Height;
        if (!evt)
          return;
        _savedHeight = this.Height;

        int height = this.Height;
        float tmpval = _maxValueZoom - _minValueZoom;
        _calcul = 0;
        if (tmpval != 0)
          _calcul = height / tmpval;

        int m = Convert.ToInt32(Math.Pow(10, _digits));
        

        int minval = Convert.ToInt32(_minValueZoom * m);
        int maxval = Convert.ToInt32(_maxValueZoom * m);
        int minstep = Math.Max(Convert.ToInt32(this.Height / 40), 1);
        int oneval = Math.Max(maxval - minval, 1);
        int minvalstep = oneval / Math.Min(oneval, minstep);

        _gridLines.Clear();
        string format = "N" + this._digits.ToString();

        for (int ival = minval; ival <= maxval; ival += minvalstep) {
          float val = (float)ival / m;
          int y = this.GetY(val);

          string str = val.ToString(format);
          this._gridLines.Add(new GridLine(val, y, str));
        }
      }
      #endregion

      #region protected override void OnPaint(ChartGraphics g)
      protected override void OnPaint(ChartGraphics g) {
        g.SelectBrush(this.Owner.BackColor);
        GdiFont gdiFont = g.SelectFont(this.Owner.ScaleFont);
        int fh = (int)gdiFont.Font.GetHeight();

        this.Owner.Area.SelectGridPen(g);
        g.SetTextColor(this.Owner.ForeColor);
        for (int i = 0; i < _gridLines.Count; i++) {
          GridLine gl = _gridLines[i];
          g.DrawLine(0, gl.Y, 2, gl.Y);
          g.TextOut(gl.Desc, 7, gl.Y - fh/2);
        }
      }
      #endregion

      #region protected override void OnMouseMove(MouseEventArgs e)
      protected override void OnMouseMove(MouseEventArgs e) {
        if (e.Button == MouseButtons.Left && _action == MouseAction.ChangeScale) {
          int dy = e.Y - _savedMousePostion.Y;
          _savedMousePostion = e.Location;

          this.Zoom += dy * 0.002F;
        }
      }
      #endregion

      #region protected override void OnMouseDown(MouseEventArgs e)
      protected override void OnMouseDown(MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
          _savedMousePostion = e.Location;
          _action = MouseAction.ChangeScale;
          this.Owner.Cursor = Cursors.SizeNS;
        }
      }
      #endregion

      #region protected override void OnMouseUp(MouseEventArgs e)
      protected override void OnMouseUp(MouseEventArgs e) {
        _action = MouseAction.None;
        this.Owner.Cursor = Cursors.Default;
      }
      #endregion
    }
  }
}
