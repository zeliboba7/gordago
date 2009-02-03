/**
* @version $Id: ChartGraphics.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  
  using System.Drawing;
  using System.Drawing.Drawing2D;
  using System.Windows.Forms;
  using System.Drawing.Imaging;
  using System.Drawing.Text;
  using System.Collections;
  using System.Diagnostics;
  using Gordago.Trader.Common;

  public sealed class ChartGraphics {

    #region class GdiWindow
    class GdiWindow {
      private readonly Rectangle _winRect;
      private readonly IntPtr _rgnIntersect;

      public GdiWindow(Rectangle winRect, IntPtr rgn) {
        _winRect = winRect;
        _rgnIntersect = rgn;
      }

      #region public Rectangle WinRect
      public Rectangle WinRect {
        get { return _winRect; }
      }
      #endregion

      #region public IntPtr Region
      public IntPtr Region {
        get { return _rgnIntersect; }
      }
      #endregion
    }
    #endregion

    private readonly Dictionary<GdiPen.PenKey, GdiPen> _penTable = new Dictionary<GdiPen.PenKey, GdiPen>();
    private readonly Dictionary<GdiBrush.BrushKey, GdiBrush> _brushTable = new Dictionary<GdiBrush.BrushKey, GdiBrush>();
    private readonly Dictionary<GdiFont.FontKey, GdiFont> _fontTable = new Dictionary<GdiFont.FontKey, GdiFont>();
    private readonly Stack<GdiWindow> _windows = new Stack<GdiWindow>();

    private Graphics _graphics;
    private IntPtr _hdc, _hdcGraphics, _hbmMem, _hOld;
    private bool _doubleBuffer = true;
    private int _winWidht = 0, _winHeight = 0;

    private GdiPen _currentPen;
    private GdiBrush _currentBrush;
    private GdiFont _currentFont;
    private Color _backColor = Color.White;

    #region internal ChartGraphics()
    internal ChartGraphics() {
    }
    #endregion

    #region ~ChartGraphics()
    ~ChartGraphics() {
      this.DeleteBM();
      this.ClearPens();
      this.ClearBrushs();
      this.ClearFonts();
    }
    #endregion

    #region public IntPtr HDC
    public IntPtr HDC {
      get { return _hdc; }
    }
    #endregion

    #region public Color BackColor
    public Color BackColor {
      get { return this._backColor; }
      set { this._backColor = value; }
    }
    #endregion

    #region public GdiFont Font
    public GdiFont Font {
      get {
        return _currentFont;
      }
    }
    #endregion

    #region public GdiPen Pen
    /// <summary>
    /// Current pen
    /// </summary>
    public GdiPen Pen {
      get { 
        return _currentPen; 
      }
    }
    #endregion

    #region public GdiBrush Brush
    public GdiBrush Brush {
      get { return _currentBrush; }
    }
    #endregion

    #region internal bool DoubleBuffer
    internal bool DoubleBuffer {
      get { return _doubleBuffer; }
      set { this._doubleBuffer = value; }
    }
    #endregion

    #region private void DeleteBM()
    private void DeleteBM() {
      if (_hbmMem == IntPtr.Zero)
        return;
      Win32.DeleteObject(_hbmMem);
      _hbmMem = IntPtr.Zero;
    }
    #endregion

    #region internal void BeginPaint(Graphics graphics, int widht, int height)
    internal void BeginPaint(Graphics graphics, int winWidht, int winHeight) {
      _graphics = graphics;
      _hdcGraphics = graphics.GetHdc();

      if (_doubleBuffer) {
        _hdc = Win32.CreateCompatibleDC(_hdcGraphics);
        if (winWidht != _winWidht || winHeight != this._winHeight) {
          this.DeleteBM();
          _hbmMem = Win32.CreateCompatibleBitmap(_hdcGraphics, winWidht, winHeight);
          _winWidht = winWidht;
          _winHeight = winHeight;
        }
        _hOld = Win32.SelectObject(_hdc, _hbmMem);
      } else {
        _hdc = _hdcGraphics;
      }

      Win32.SetBkMode(_hdc, 2);

      this.SelectPen(Color.Black);
      this.SelectBrush(Color.White);
      this.SelectFont(new Font("Microsoft Sans Serif", 8));
    }
    #endregion

    #region internal void EndPaint()
    internal void EndPaint() {

      if (_doubleBuffer) {
        Win32.BitBlt(_hdcGraphics, 0, 0, _winWidht, _winHeight, _hdc, 0, 0, (uint)Win32.TernaryRasterOperations.SRCCOPY);
        Win32.SelectObject(_hdc, _hOld);
        Win32.DeleteDC(_hdc);
      }

      this._graphics.ReleaseHdc(this._hdcGraphics);
      this._graphics = null;
      this._hdc = IntPtr.Zero;
    }
    #endregion

    #region private void ClearPens()
    private void ClearPens() {
      foreach (GdiPen pen in _penTable.Values) {
        pen.Release();
      }
      this._penTable.Clear();
    }
    #endregion

    #region private void ClearBrushs()
    private void ClearBrushs() {
      foreach (GdiBrush brush in _brushTable.Values) {
        brush.Release();
      }
      _brushTable.Clear();
    }
    #endregion

    #region private void ClearFonts()
    private void ClearFonts() {
      foreach (GdiFont font in _fontTable.Values) {
        font.Release();
      }
      _fontTable.Clear();
    }
    #endregion

    #region private void CheckNativeBrush(GdiBrush brush)
    private void CheckNativeBrush(GdiBrush brush) {
      if (brush.NativeBrush == IntPtr.Zero)
        brush.SetNativeBrush(Win32.CreateSolidBrush(brush.GdiColor));
    }
    #endregion

    #region public GdiBrush SelectBrush(Color color)
    public GdiBrush SelectBrush(Color color) {
      return this.SelectBrush(new GdiBrush(color));
    }
    #endregion

    #region public GdiBrush SelectBrush(GdiBrush brush)
    public GdiBrush SelectBrush(GdiBrush brush) {
      if (brush == null)
        throw (new ArgumentNullException("pen"));

      if (brush.NativeBrush != IntPtr.Zero){
        this.SelectObject(brush);
        return brush;
      }

      GdiBrush gdiBrush = null;
      _brushTable.TryGetValue(brush.Key, out gdiBrush);
      if (gdiBrush == null) {
        gdiBrush = brush;
        _brushTable.Add(gdiBrush.Key, gdiBrush);
      }
      this.CheckNativeBrush(gdiBrush);
      this.SelectObject(gdiBrush);
      return gdiBrush;
    }
    #endregion

    #region private void CheckNativePen(GdiPen gdiPen)
    private void CheckNativePen(GdiPen gdiPen) {
      if (gdiPen.NativePen == IntPtr.Zero)
        gdiPen.SetNativePen(Win32.CreatePen(gdiPen.GdiStyle, gdiPen.Width, gdiPen.GdiColor));
    }
    #endregion

    #region public GdiPen SelectPen(Color color)
    public GdiPen SelectPen(Color color) {
      return this.SelectPen(new GdiPen(color));
    }
    #endregion

    #region public GdiPen SelectPen(GdiPen pen)
    public GdiPen SelectPen(GdiPen pen) {

      if (pen == null)
        throw(new ArgumentNullException("pen"));

      if (pen.NativePen != IntPtr.Zero) {
        this.SelectObject(pen);
        return pen;
      }

      GdiPen gdiPen = null;
      _penTable.TryGetValue(pen.Key, out gdiPen);
      if (gdiPen == null) {
        gdiPen = pen;
        _penTable.Add(pen.Key, pen);
      }
      this.CheckNativePen(gdiPen);
      this.SelectObject(gdiPen);
      return gdiPen;
    }
    #endregion

    #region public GdiFont SelectFont(Font font)
    public GdiFont SelectFont(Font font) {
      if (font == null)
        throw (new ArgumentNullException("font"));

      GdiFont.FontKey key = new GdiFont.FontKey(font);
      GdiFont gdiFont = null;
      _fontTable.TryGetValue(key, out gdiFont);

      if (gdiFont == null) {
        gdiFont = new GdiFont(font);
        _fontTable.Add(gdiFont.Key, gdiFont);
      }
      this.SelectObject(gdiFont);
      return gdiFont;
    }
    #endregion

    #region public GdiFont SelectFont(GdiFont gdiFont)
    public GdiFont SelectFont(GdiFont gdiFont) {
      if (gdiFont == null)
        throw (new ArgumentNullException("gdiFont"));
      this.SelectObject(gdiFont);
      return gdiFont;
    }
    #endregion

    #region public void SetTextColor(Color color)
    public void SetTextColor(Color color) {
      Win32.SetTextColor(_hdc, Win32.ColorToGdiColor(color));
    }
    #endregion

    #region public void SetBackColor(Color color)
    public void SetBackColor(Color color) {
      Win32.SetBkColor(_hdc, Win32.ColorToGdiColor(color));
    }
    #endregion

    #region private void SelectObject(object obj)
    private void SelectObject(object obj) {
      if (obj == null)
        throw(new ArgumentNullException("obj"));

      if (obj is GdiPen) {
        
        GdiPen pen = (GdiPen)obj;
        _currentPen = pen;
        Win32.SelectObject(_hdc, pen.NativePen);

      } else if (obj is GdiBrush) {

        GdiBrush brush = (GdiBrush)obj;
        _currentBrush = brush;
        Win32.SelectObject(_hdc, brush.NativeBrush);
      
      }else if(obj is GdiFont){
        
        GdiFont font = (GdiFont)obj;
        _currentFont = font;
        Win32.SelectObject(_hdc, font.NativeFont);

      }else 
        throw(new Exception("Unknow gdi object"));
    }
    #endregion

    #region public void DrawLine(int x1, int y1, int x2, int y2)
    public void DrawLine(int x1, int y1, int x2, int y2) {
      Win32.MoveToEx(_hdc, x1, y1, IntPtr.Zero);

      if (x1 == x2) y2++;
      if (y1 == y2) x2++;

      Win32.LineTo(_hdc, x2, y2);
    }
    #endregion

    #region public void MoveTo(int x, int y)
    public void MoveTo(int x, int y) {
      Win32.MoveToEx(_hdc, x, y, IntPtr.Zero);
    }
    #endregion

    #region public void LineTo(int x, int y)
    public void LineTo(int x, int y) {
      Win32.LineTo(_hdc, x, y);
    }
    #endregion

    #region public void DrawLineExt(GdiPen gdiPen, int x1, int y1, int x2, int y2)
    public void DrawLineExt(GdiPen gdiPen, int x1, int y1, int x2, int y2) {
      if (gdiPen == null)
        throw (new ArgumentNullException("gdiPen"));
      this.CheckNativePen(gdiPen);
      Win32.SelectObject(_hdc, gdiPen.NativePen);
      this.DrawLine(x1, y1, x2, y2);
      Win32.SelectObject(_hdc, _currentPen.NativePen);
    }
    #endregion

    #region public void DrawRectangle(Rectangle rect)
    public void DrawRectangle(Rectangle rect) {
      this.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
    }
    #endregion

    #region public void DrawRectangle(int x, int y, int width, int height)
    public void DrawRectangle(int x, int y, int width, int height) {
      Win32.FrameRect(this._hdc, x, y, width + 1, height + 1, _currentPen.NativePen);
    }
    #endregion

    #region public void DrawRectangleExt(GdiPen pen, Rectangle rect)
    public void DrawRectangleExt(GdiPen pen, Rectangle rect) {
      this.DrawRectangleExt(pen, rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion

    #region public void DrawRectangleExt(GdiPen gdiPen, int x, int y, int width, int height)
    public void DrawRectangleExt(GdiPen gdiPen, int x, int y, int width, int height) {
      if (gdiPen == null)
        throw (new ArgumentNullException("gdiPen"));
      this.CheckNativePen(gdiPen);
      Win32.FrameRect(this._hdc, x, y, width + 1, height + 1, gdiPen.NativePen);
    }
    #endregion

    #region public void DrawPolygon(Point[] points)
    public void DrawPolygon(Point[] points) {
      if (points == null)
        throw(new ArgumentNullException("points"));

      Win32.Polygon(this._hdc, points, points.Length);
    }
    #endregion

    #region public void FillRectangle(Rectangle rect)
    public void FillRectangle(Rectangle rect) {
      this.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion

    #region public void FillRectangle(int x, int y, int width, int height)
    public void FillRectangle(int x, int y, int width, int height) {
      Win32.FillRect(this._hdc, x, y, width, height, this._currentBrush.NativeBrush);
    }
    #endregion

    #region public void FillRectangleExt(GdiBrush brush, Rectangle rect)
    public void FillRectangleExt(GdiBrush brush, Rectangle rect) {
      this.FillRectangleExt(brush, rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion

    #region public void FillRectangleExt(GdiBrush brush, int x, int y, int width, int height)
    public void FillRectangleExt(GdiBrush brush, int x, int y, int width, int height) {
      if (brush == null)
        throw (new ArgumentNullException("brush"));
      this.CheckNativeBrush(brush);
      Win32.FillRect(this._hdc, x, y, width, height, brush.NativeBrush);
    }
    #endregion

    #region public void TextOut(string s, int x, int y)
    public void TextOut(string s, int x, int y) {
      Win32.TextOut(_hdc, x, y, s, s.Length);
    }
    #endregion

    //    public void TextOut(string text, int len, Rectangle rect) {
//      this.TextOut(text, len, rect, rect.Left, rect.Top, false, false, -1);
//    }

//    public void TextOut(string text, int len, int x, int y) {
//      this.TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, false, false, -1);
//    }

//    public void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque) {
//      this.TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, -1);
//    }

//    public void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque, int space) {
//      this.TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, space);
//    }

//    public void TextOut(string text, int len, int x, int y, bool clipped, bool opaque) {
//      this.TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, clipped, opaque, -1);
//    }

//    public void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque) {
//      this.TextOut(text, len, rect, x, y, clipped, opaque, -1);
//    }

//    public void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque, int space) {
//      if (text == null) return;

//      if (_textColor != _selectTextColor) {
//        Win32.SetTextColor(this._hdc, Win32.ColorToGdiColor(_textColor));
//        _selectTextColor = _textColor;
//      }

////      Win32.ExtTextOut(this._hdc, rect, x, y, (clipped ? 4 : 0) | (opaque ? 2 : 0), text, len, (space > 0) ? this.GetBuffer((len < 0) ? text.Length : 0, space) : null);
//      opaque = true;
//      Win32.ExtTextOutW(this._hdc, rect, x, y, (clipped ? 4 : 0) | (opaque ? 2 : 0), text, len, null);
//    }

    //private int[] GetBuffer(int len) {
    //  return this.GetBuffer(len, -1);
    //}

    //private int[] GetBuffer(int len, int space) {
    //  if (space < 0) {
    //    return null;
    //  }
    //  if (this._bufferSize < len) {
    //    this._bufferSize = len;
    //    this._buffer = new int[this._bufferSize];
    //    for (int i = 0; i < this._bufferSize; i++) {
    //      this._buffer[i] = (space >= 0) ? space : base.FontWidth;
    //    }
    //  }
    //  return this._buffer;
    //}

    #region public void SetWorldTransform(int x, int y, float scaleX, float scaleY)
    public void SetWorldTransform(int x, int y, float scaleX, float scaleY) {
      Win32.SetGraphicsMode(this._hdc, 2);
      Win32.XFORM xForm = new Win32.XFORM(scaleX, 0f, 0f, scaleY, x * scaleX, y * scaleY);
      Win32.SetWorldTransform(this._hdc, ref xForm);
    }
    #endregion

    #region public void IntersectClipRect(Rectangle rect)
    public void IntersectClipRect(Rectangle rect) {
      this.IntersectClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
    }
    #endregion

    #region public void IntersectClipRect(int x, int y, int width, int height)
    public void IntersectClipRect(int x, int y, int width, int height) {
      Win32.IntersectClipRect(this._hdc, x, y, x + width, y + height);
    }
    #endregion

    #region public void ExcludeClipRect(Rectangle rect)
    public void ExcludeClipRect(Rectangle rect) {
      this.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
    }
    #endregion

    #region public void ExcludeClipRect(int x, int y, int width, int height)
    /// <summary>
    /// Creates a new clipping region that consists of the existing clipping region minus the specified rectangle.
    /// </summary>
    /// <param name="x">Specifies the logical x-coordinate of the upper-left corner of the rectangle.</param>
    /// <param name="y">Specifies the logical y-coordinate of the upper-left corner of the rectangle.</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void ExcludeClipRect(int x, int y, int width, int height) {
      Win32.ExcludeClipRect(this._hdc, x, y, x + width, y + height);
    }
    #endregion

    #region public IntPtr SaveClip(Rectangle rect)
    public IntPtr SaveClip(Rectangle rect) {
      IntPtr rgn = Win32.CreateRectRgnIndirect(rect);
      Win32.GetClipRgn(this._hdc, rgn);
      return rgn;
    }
    #endregion

    #region public void RestoreClip(IntPtr rgn)
    public void RestoreClip(IntPtr rgn) {
      Win32.SelectClipRgn(this._hdc, rgn);
      Win32.DeleteObject(rgn);
    }
    #endregion

    #region public void SetViewport(Rectangle rect)
    public void SetViewport(Rectangle rect) {
      this.SetViewport(rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion

    #region public void SetViewport(int x, int y, int width, int height)
    public void SetViewport(int x, int y, int width, int height) {
      Win32.SetViewportOrgEx(_hdc, x, y, IntPtr.Zero);
      Win32.SetViewportExtEx(_hdc, width, height, IntPtr.Zero);

//      Trace.WriteLine(string.Format("SetViewport: {0}", new Rectangle(x, y, width, height)));
    }
    #endregion

    #region public void CreatePaintWindow(Rectangle rect)
    public void CreatePaintWindow(Rectangle rect) {
//      Trace.WriteLine("CreatePaintWindow: " + callStack);
      GdiWindow parent = null;
      if (_windows.Count >0 )
        parent = this._windows.Peek();

      Rectangle frect = rect;
      if (parent != null) {
        Rectangle prect = parent.WinRect;
        frect.X += prect.X;
        frect.Y += prect.Y;
      }

      this.SetViewport(frect);
      IntPtr rgn = this.SaveClip(frect);
      this.IntersectClipRect(0, 0, frect.Width, frect.Height);
      this._windows.Push(new GdiWindow(frect, rgn));
    }
    #endregion

    #region public void RestorePaintWindow()
    public void RestorePaintWindow() {
      // Trace.WriteLine("RestorePaintWindow: " + callStack);
      if (_windows.Count == 0)
        return;
      GdiWindow current = this._windows.Pop();
      this.RestoreClip(current.Region);

      Rectangle frect = new Rectangle(0, 0, this._winWidht, this._winHeight);
      if (_windows.Count > 0) {
        current = this._windows.Peek();
        frect = current.WinRect;
      }
      this.SetViewport(frect);
    }
    #endregion
  }
}
