/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
#endregion

namespace Gordago.Analysis.Chart {

  #region public enum ChartFigureBarType
  public enum ChartFigureBarType:int {
    CandleStick = 0,
    Bar = 1,
    Line = 2,
    None = 3
  }
  #endregion

  #region struct BarElement
  struct BarElement {
    private int _yLow, _yOpen, _yClose, _yHigh;

    public BarElement(int yLow, int yOpen, int yClose, int yHigh) {
      _yLow = yLow;
      _yOpen = yOpen;
      _yHigh = yHigh;
      _yClose = yClose;
    }

    #region public int YLow
    public int YLow {
      get { return _yLow; }
      set { _yLow = value; }
    }
    #endregion
    #region public int YOpen
    public int YOpen {
      get { return this._yOpen; }
      set { this._yOpen = value; }
    }
    #endregion
    #region public int YClose
    public int YClose {
      get { return this._yClose; }
      set { this._yClose = value; }
    }
    #endregion
    #region public int YHigh
    public int YHigh {
      get { return this._yHigh; }
      set { this._yHigh = value; }
    }
    #endregion
  }
  #endregion

  public class ChartFigureBar:ChartFigure {

    private Point[] _linepoint;
    private BarElement[] _elements;
    private int _lpsize;
    private bool _repaint;

    private GraphicsPath _gpath;

    private ChartFigureBarType _bartype;
    private GDI.POINT _p;

    private int _selectmousebarindex;
    private Point _mousePosition;
    private int _selectedBar = -1;
    private ChartFigureComment _comment;

    public static readonly int[] BAR_WIDTH = new int[]{1, 1, 2, 4, 10, 24};

    private Color _barColor, _barUpColor, _barDownColor;
    private Pen _barPen, _barPenSel;
    private Brush _barUpBrush, _barDownBrush;
    private int _savedStyleId = 0;

    public ChartFigureBar(string name): base(name, true) {
      this.EnableScale = true;
      _bartype = ChartFigureBarType.CandleStick;
      _elements = new BarElement[0];
      _comment = new ChartFigureComment("");
    }

    #region public Color BarColor
    public Color BarColor {
      get { return _barColor; }
      set {
        this._barColor = value;
        this._barPen = new Pen(value);
        this._barPenSel = new Pen(value, 2);
      }
    }
    #endregion

    #region public Color BarUpColor
    public Color BarUpColor {
      get { return _barUpColor; }
      set {
        this._barUpColor = value;
        this._barUpBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color BarDownColor
    public Color BarDownColor {
      get { return this._barDownColor; }
      set {
        this._barDownColor = value;
        this._barDownBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public ChartStyle Style
    public ChartStyle Style {
      get { return this.ChartBox.ChartManager.Style; }
    }
    #endregion

    #region protected internal override int DecimalDigits
    protected internal override int DecimalDigits {
      get {
        return this.ChartBox.ChartManager.Symbol.DecimalDigits;
      }
      set {
        base.DecimalDigits = value;
      }
    }
    #endregion

    #region protected override GraphicsPath GraphicsPath
    protected override GraphicsPath GraphicsPath {
      get {
        return _gpath;
      }
    }
    #endregion

    #region public ChartFigureBarType BarType
    public ChartFigureBarType BarType {
      get { return this._bartype; }
      set { this._bartype = value; }
    }
    #endregion

    #region protected internal override void OnCalculateScale()
    protected internal override void OnCalculateScale() {
      if(_bartype == ChartFigureBarType.Line && (_linepoint == null || _linepoint.Length != Map.Length)) {
        _linepoint = new Point[Map.Length];
      }
      if(_elements.Length != Map.Length)
        _elements = new BarElement[Map.Length];
      _repaint = true;

      for(int index = 0; index < Map.Length; index++) {

        int barindex = index + this.ChartBox.ChartManager.Position - this.Shift;
        IBarList bars = this.ChartBox.ChartManager.Bars;
        if(barindex >= 0 && barindex < bars.Count){
          Bar bar = bars[barindex];
          this.SetScaleValue(bar.Low, bar.High);
        }
      }
    }
    #endregion

    #region private void Paint(IntPtr hdc)
    private void Paint(IntPtr hdc) {
      _lpsize = 0;

      if (_savedStyleId != this.Style.Id) {
        _savedStyleId = this.Style.Id;
        this.BarColor = this.Style.BarColor;
        this.BarUpColor = this.Style.BarUpColor;
        this.BarDownColor = this.Style.BarDownColor;
      }

      IntPtr ptrPen = GDI.CreatePen(0, (int)_barPen.Width, ColorTranslator.ToWin32(this.BarColor));
      IntPtr ptrPenSel = GDI.CreatePen(0, (int)_barPenSel.Width, ColorTranslator.ToWin32(this.BarColor));

      IntPtr ptrBrushUp = GDI.CreateSolidBrush(ColorTranslator.ToWin32(this.BarUpColor));
      IntPtr ptrBrushDown = GDI.CreateSolidBrush(ColorTranslator.ToWin32(this.BarDownColor));

      IntPtr ptrPenOld = IntPtr.Zero;
      IntPtr ptrPenSelOld = IntPtr.Zero;
      IntPtr ptrBrushUpOld = IntPtr.Zero;
      IntPtr ptrBrushDownOld = IntPtr.Zero;

      for(int index = 0; index < Map.Length; index++) {

        int barindex = index + this.ChartBox.ChartManager.Position - this.Shift;

        IBarList bars = this.ChartBox.ChartManager.Bars;

        if(barindex >= 0 && barindex < bars.Count ) {

          int x = Map[index];

          Bar bar = bars[barindex];
          if(_repaint) {
            _elements[index].YLow = this.ChartBox.GetY(bar.Low);
            _elements[index].YOpen = this.ChartBox.GetY(bar.Open);
            _elements[index].YClose = this.ChartBox.GetY(bar.Close);
            _elements[index].YHigh = this.ChartBox.GetY(bar.High);
            _gpath = new GraphicsPath();
          }
          BarElement element = _elements[index];

          if(_bartype == ChartFigureBarType.Line) {
            _linepoint[_lpsize].X = x;
            _linepoint[_lpsize].Y = element.YClose;
            _lpsize++;
            if(index == 0) {
              ptrPenOld = GDI.SelectObject(hdc, ptrPen);
              GDI.MoveToEx(hdc, x, element.YClose, out _p);
            } else {
              GDI.LineTo(hdc, x, element.YClose);
            }
          } else {
            int bw = BAR_WIDTH[(int)this.ChartBox.ChartManager.Zoom];
            if(_selectedBar == barindex) {
              _selectedBar = barindex;
              ptrPenSelOld = GDI.SelectObject(hdc, ptrPenSel);
            } else {
              ptrPenOld = GDI.SelectObject(hdc, ptrPen);
            }

            DrawLine(hdc, x, element.YLow, x, element.YHigh);

            if(bw > 1) {
              int x1 = x - bw / 2;
              int x2 = x + bw / 2;

              if(_bartype == ChartFigureBarType.Bar) {
                DrawLine(hdc, x1, element.YOpen, x, element.YOpen);
                DrawLine(hdc, x, element.YClose, x2, element.YClose);

              } else if(_bartype == ChartFigureBarType.CandleStick) {
                if(bar.Open == bar.Close) {

                  DrawLine(hdc, x1, element.YOpen, x2, element.YOpen);

                } else if(bar.Open < bar.Close) {
                  ptrBrushUpOld = GDI.SelectObject(hdc, ptrBrushUp);
                  GDI.Rectangle(hdc, x1, element.YClose, x2 + 1, element.YOpen);
                } else {
                  ptrBrushDownOld = GDI.SelectObject(hdc, ptrBrushDown);
                  GDI.Rectangle(hdc, x1, element.YOpen, x2 + 1, element.YClose);
                }
              }
            }
          }
        }
      }
      if(ptrPenOld != IntPtr.Zero) {
        GDI.SelectObject(hdc, ptrPenOld);
        GDI.DeleteObject(ptrPen);
      }
      if(ptrPenSelOld != IntPtr.Zero) {
        GDI.SelectObject(hdc, ptrPenSelOld);
        GDI.DeleteObject(ptrPenSel);
      }
      if(ptrBrushUpOld != IntPtr.Zero) {
        GDI.SelectObject(hdc, ptrBrushUpOld);
        GDI.DeleteObject(ptrBrushUp);
      }

      if(ptrBrushDownOld != IntPtr.Zero) {
        GDI.SelectObject(hdc, ptrBrushDownOld);
        GDI.DeleteObject(ptrBrushDown);
      }

      _repaint = false;
    }
    #endregion

    #region protected internal override void OnPaint(Graphics g)
    protected internal override void OnPaint(Graphics g) {
      IntPtr hdc = IntPtr.Zero;
      hdc = g.GetHdc();
      Paint(hdc);
      g.ReleaseHdc(hdc);

      if (_selectedBar > -1) {
        int decDig = this.ChartBox.ChartManager.Symbol.DecimalDigits;
        Bar bar = this.ChartBox.ChartManager.Bars[this._selectedBar];
        _comment.Text = "Open " + SymbolManager.ConvertToCurrencyString(bar.Open, decDig) + "\n" +
          "High " + SymbolManager.ConvertToCurrencyString(bar.High, decDig) + "\n" +
          "Low " + SymbolManager.ConvertToCurrencyString(bar.Low, decDig) + "\n" +
          "Close " + SymbolManager.ConvertToCurrencyString(bar.Close, decDig) + "\n" +
          "Volume " + bar.Volume.ToString() + "\n" +
          "Time " + bar.Time.ToShortTimeString() + "\n" +
          "Date " + bar.Time.ToShortDateString();

        this.DrawComment(g, _comment, _mousePosition.X, _mousePosition.Y, ContentAlignment.MiddleCenter);

      }
    }
    #endregion

    #region public void DrawLine(IntPtr hdc, int x1, int y1, int x2, int y2)
    public void DrawLine(IntPtr hdc, int x1, int y1, int x2, int y2) {
      GDI.MoveToEx(hdc, x1, y1, out _p);
      GDI.LineTo(hdc, x2, y2);
    }
    #endregion

    #region private int CheckBarIndex(Point p)
    private int CheckBarIndex(Point p) {

      int deltax = 1;
      if (this.Map.Length > 1) {
        deltax = this.Map[1];
      }

      int num = (int)Math.Round((double)p.X / (double)deltax);
      int selectBarIndex = num + this.ChartBox.ChartManager.Position - this.Shift;

      if (_bartype == ChartFigureBarType.Line && _lpsize > 1) {
        _gpath = new GraphicsPath();
        _gpath.AddLines(_linepoint);
      } else {

        COPoint cpoint = this.ChartBox.GetChartPoint(p);
        int index = cpoint.BarIndex - this.ChartBox.ChartManager.Position;

        int barindex = index + this.ChartBox.ChartManager.Position - this.Shift;
        IBarList bars = this.ChartBox.ChartManager.Bars;

        if (barindex < 0 || barindex >= bars.Count || index < 0 || index >= Map.Length)
          return -1;

        int x = Map[index];
        if (index < 0 || index >= _elements.Length) return -1;

        Bar bar = bars[barindex];
        _elements[index].YLow = this.ChartBox.GetY(bar.Low);
        _elements[index].YOpen = this.ChartBox.GetY(bar.Open);
        _elements[index].YClose = this.ChartBox.GetY(bar.Close);
        _elements[index].YHigh = this.ChartBox.GetY(bar.High);

        _gpath = new GraphicsPath();

        BarElement element = _elements[index];

        int bw = BAR_WIDTH[(int)this.ChartBox.ChartManager.Zoom];

        this.GraphicsPath.AddLine(x, element.YLow, x, element.YHigh);
        this.GraphicsPath.CloseFigure();

        if (bw > 1) {
          int x1 = x - bw / 2;
          int x2 = x + bw / 2;

          if (_bartype == ChartFigureBarType.Bar) {
          } else if (_bartype == ChartFigureBarType.CandleStick) {
            if (bar.Open == bar.Close) {

              this.GraphicsPath.AddLine(x1, element.YOpen, x2, element.YOpen);
              this.GraphicsPath.CloseFigure();
            } else if (bar.Open < bar.Close) {
              this.GraphicsPath.AddRectangle(new Rectangle(x1, element.YClose, bw, element.YOpen - element.YClose));
              this.GraphicsPath.CloseFigure();
            } else {
              this.GraphicsPath.AddRectangle(new Rectangle(x1, element.YOpen, bw, element.YClose - element.YOpen));
              this.GraphicsPath.CloseFigure();
            }
          }
        }
      }
      if (base.CheckFigure(p))
        return selectBarIndex;
      return -1;
    }
    #endregion

    protected internal override void OnMouseMove(MouseEventArgs e) {
      this._mousePosition = e.Location;
      this._selectedBar = this.CheckBarIndex(e.Location);
      if (_selectedBar > -1)
        this.Invalidate();
    }
  }
}
