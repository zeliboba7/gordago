/**
* @version $Id: GdiBrush.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  public sealed class GdiBrush {
    private Color _color;
    private int _gdiColor;
    private IntPtr _nativeBrush = IntPtr.Zero;
    private BrushKey _key;
    
    public GdiBrush(Color color) {
      _color = color;
      _gdiColor = Win32.ColorToGdiColor(color);
      _key = new BrushKey(this);
    }

    #region ~GdiBrush()
    ~GdiBrush() {
      this.Release();
    }
    #endregion

    #region public Color Color
    public Color Color {
      get { return _color; }
    }
    #endregion

    #region internal int GdiColor
    internal int GdiColor {
      get { return _gdiColor; }
    }
    #endregion

    #region internal IntPtr NativeBrush
    internal IntPtr NativeBrush {
      get { return _nativeBrush; }
    }
    #endregion

    #region internal BrushKey Key
    internal BrushKey Key {
      get { return _key; }
    }
    #endregion

    #region internal void SetNativeBrush(IntPtr nativeBrush)
    internal void SetNativeBrush(IntPtr nativeBrush) {
      if (nativeBrush == IntPtr.Zero) {
        throw new ArgumentNullException("nativeBrush");
      }

      if (_nativeBrush != IntPtr.Zero) {
        throw new Exception("Error initialize native brush");
      }
      _nativeBrush = nativeBrush;
    }
    #endregion

    #region internal void Release()
    internal void Release() {
      if (_nativeBrush == IntPtr.Zero)
        return;
      Win32.DeleteObject(_nativeBrush);
      _nativeBrush = IntPtr.Zero;
    }
    #endregion

    #region internal struct BrushKey
    internal struct BrushKey {
      private int _gdiColor;

      internal BrushKey(GdiBrush brush){
        _gdiColor = brush.GdiColor;
      }

      public override bool Equals(object obj) {
        if (!(obj is BrushKey))
          return false;
        return ((BrushKey)obj)._gdiColor.Equals(_gdiColor);
      }

      public override int GetHashCode() {
        return _gdiColor.GetHashCode();
      }
    }
    #endregion
  }
}
