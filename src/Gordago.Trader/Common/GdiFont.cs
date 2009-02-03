/**
* @version $Id: GdiFont.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;

  public sealed class GdiFont {

    private readonly Font _font;
    private IntPtr _nativeFont = IntPtr.Zero;
    private readonly FontKey _key;

    public GdiFont(Font font) {
      _font = font;
      _nativeFont = font.ToHfont();
      _key = new FontKey(font);
    }

    #region ~GdiFont()
    ~GdiFont() {
      this.Release();
    }
    #endregion

    #region public Font Font
    public Font Font {
      get { return _font; }
    }
    #endregion

    #region internal IntPtr NativeFont
    internal IntPtr NativeFont {
      get { return _nativeFont; }
    }
    #endregion

    #region internal FontKey Key
    internal FontKey Key {
      get { return this._key; }
    }
    #endregion

    #region internal void Release()
    internal void Release() {
      if (_nativeFont == IntPtr.Zero)
        return;
      Win32.DeleteObject(_nativeFont);
      _nativeFont = IntPtr.Zero;
    }
    #endregion

    #region internal struct FontKey
    internal struct FontKey {
      private int _hashCode;
      
      public FontKey(Font font) {
        string key = font.Name + "|" + font.Size.ToString() + font.Style.ToString();
        _hashCode = key.GetHashCode();
      }

      #region public override bool Equals(object obj)
      public override bool Equals(object obj) {
        if (!(obj is FontKey))
          return false;
        FontKey key = (FontKey)obj;

        return _hashCode == key._hashCode;
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
