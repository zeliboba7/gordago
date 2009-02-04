/**
* @version $Id: GdiPen.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public sealed class GdiPen {
    private Color _color;
    private int _gdiColor;
    private DashStyle _style;
    private int _gdiStyle;
    private int _width;
    private IntPtr _nativePen = IntPtr.Zero;
    private PenKey _key;

    public GdiPen(Color color) : this(color, 1, DashStyle.Solid) {
    }

    public GdiPen(Color color, int width):this(color, width, DashStyle.Solid) {
    }

    public GdiPen(Color color, int width, DashStyle style) {
      _color = color;
      _gdiColor = Win32.ColorToGdiColor(color);

      _width = width;

      _style = style;      

      _gdiStyle = DashStyleToPenStyle(style);
      _key = new PenKey(this);
    }

    #region ~GdiPen()
    ~GdiPen() {
      this.Release();
    }
    #endregion

    #region public Color Color
    public Color Color {
      get { return this._color; }
    }
    #endregion

    #region public int Width
    public int Width {
      get { return this._width; }
    }
    #endregion

    #region public DashStyle DashStyle
    public DashStyle DashStyle {
      get { return this._style; }
    }
    #endregion

    #region internal int GdiColor
    internal int GdiColor {
      get { return _gdiColor; }
    }
    #endregion

    #region internal int GdiStyle
    internal int GdiStyle {
      get { return _gdiStyle; }
    }
    #endregion

    #region internal IntPtr NativePen
    internal IntPtr NativePen {
      get { return _nativePen; }
    }
    #endregion

    #region internal PenKey Key
    internal PenKey Key {
      get { return _key; }
    }
    #endregion

    #region internal int DashStyleToPenStyle(DashStyle style)
    internal int DashStyleToPenStyle(DashStyle style) {
      switch (style) {
        case DashStyle.Dash:
          return 1;

        case DashStyle.Dot:
          return 2;

        case DashStyle.DashDot:
          return 3;

        case DashStyle.DashDotDot:
          return 4;
      }
      return 0;
    }
    #endregion

    #region internal void SetNativePen(IntPtr nativePen)
    internal void SetNativePen(IntPtr nativePen) {
      if (nativePen == IntPtr.Zero) {
        throw new ArgumentNullException("nativePen");
      }

      if (_nativePen != IntPtr.Zero) {
        throw new Exception("Error initialize Native Pen");
      }
      this._nativePen = nativePen;
    }
    #endregion

    #region internal void Release()
    internal void Release() {
      if (_nativePen == IntPtr.Zero)
        return;
      Win32.DeleteObject(_nativePen);
      _nativePen = IntPtr.Zero;
    }
    #endregion

    #region internal struct PenKey
    internal struct PenKey {

      private int _gdiColor, _gdiStyle, _width;
      private int _hashCode;

      internal PenKey(GdiPen pen) {
        _gdiColor = pen._gdiColor;
        _gdiStyle = pen._gdiStyle;
        _width = pen._width;
        _hashCode = _gdiColor << 24 | _gdiStyle << 16 | _width << 8;
      }

      #region public override bool Equals(object obj)
      public override bool Equals(object obj) {
        if (!(obj is PenKey))
          return false;
        PenKey key = (PenKey)obj;

        return key._gdiColor.Equals(_gdiColor) && key._gdiStyle.Equals(_gdiStyle) && key._width.Equals(_width);
      }
      #endregion

      #region public override int GetHashCode()
      public override int GetHashCode() {
        return _hashCode;
      }
      #endregion
    }
    #endregion
  }
}

