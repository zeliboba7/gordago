/**
* @version $Id: GdiFunction_old.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Collections;
  using System.Drawing;
  using System.Windows.Forms;
  using System.Drawing.Text;

  public class GdiFunction {

    private Hashtable _brushTable = new Hashtable();
    private Hashtable _fontTable = new Hashtable();
    private Hashtable _penTable = new Hashtable();

    private IntPtr _hdc;
    private Color _backColor;
    private Font _font;
    private FontInfos _fontInfos;
    private FontStyle fontStyle;
    private Color _penColor;
    private IntPtr _measureDC, _measureHFont, _oldMeasureFont;
    private bool opaque = true;
    private StringFormat _stringFormat;
    private Color _textColor;
    private IntPtr _brushPtr, _penPtr;
    private int[] _buffer;
    private int _bufferSize, _formatFlags, _oldMode = 1;
    private bool _transFormed;
    private Win32.XFORM _xForm;

    #region internal GdiFunction()
    internal GdiFunction() {
      IntPtr dC = Win32.GetDC(IntPtr.Zero);
      System.Drawing.Font font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f);
      try {
        this._measureDC = Win32.CreateCompatibleDC(dC);
        this._measureHFont = font.ToHfont();
        this._oldMeasureFont = Win32.SelectObject(this._measureDC, this._measureHFont);
      } finally {
        Win32.ReleaseDC(IntPtr.Zero, dC);
      }
      this.Init();
    }
    #endregion

    #region ~GdiFunction()
    ~GdiFunction() {
      this.ClearBrushes();
      this.ClearPens();
      this.ClearFonts();
      Win32.SelectObject(this._measureDC, this._oldMeasureFont);
      if (this._measureHFont != IntPtr.Zero) {
        Win32.DeleteObject(this._measureHFont);
      }
      Win32.DeleteDC(this._measureDC);
    }
    #endregion

    internal void BeginPain(IntPtr hdc) {
      _hdc = hdc;
      _penColor = Color.White;
    }

    #region public Color BackColor
    public Color BackColor {
      get {
        return this._backColor;
      }
      set {
        if (this._backColor != value) {
          this._backColor = value;
          this.SelectBrush(this._backColor, true);
        }
      }
    }
    #endregion

    #region private Hashtable BrushTable
    private Hashtable BrushTable {
      get {
        return this._brushTable;
      }
    }
    #endregion

    #region private FontInfo CurrentInfo
    private FontInfo CurrentInfo {
      get {
        if (this._fontInfos == null) {
          return null;
        }
        return this._fontInfos.CurrentInfo;
      }
    }
    #endregion

    #region public Font Font
    public Font Font {
      get {
        return this._font;
      }
      set {
        if (this._font != value) {
          this._font = value;
          if (value != null) {
            this.SelectFont(this._font, this._font.Style);
          }
        }
      }
    }
    #endregion

    #region public int FontHeight
    public int FontHeight {
      get {
        return this._fontInfos.FontHeight;
      }
    }
    #endregion

    #region public FontStyle FontStyle
    public FontStyle FontStyle {
      get {
        return this.fontStyle;
      }
      set {
        if (this.fontStyle != value) {
          this.SelectFont(value);
        }
      }
    }
    #endregion

    #region private Hashtable FontTable
    private Hashtable FontTable {
      get {
        return this._fontTable;
      }
    }
    #endregion

    #region public int FontWidth
    public int FontWidth {
      get {
        if (this.CurrentInfo == null) {
          return 0;
        }
        return this.CurrentInfo.FontWidth;
      }
    }
    #endregion

    #region public Color PenColor
    public Color PenColor {
      get {
        return this._penColor;
      }
      set {
        if (this._penColor != value) {
          this._penColor = value;
          this.SelectPen(this._penColor, true);
        }
      }
    }
    #endregion

    #region public bool IsMonoSpaced
    public bool IsMonoSpaced {
      get {
        return this._fontInfos.IsMonoSpaced;
      }
    }
    #endregion

    #region public bool Opaque
    public bool Opaque {
      get {
        return this.opaque;
      }
      set {
        if (this.opaque != value) {
          this.opaque = value;
          this.SelectOpaque(value);
        }
      }
    }
    #endregion

    #region private Hashtable PenTable
    private Hashtable PenTable {
      get {
        return this._penTable;
      }
    }
    #endregion

    #region public StringFormat StringFormat
    public StringFormat StringFormat {
      get {
        return this._stringFormat;
      }
      set {
        if (this._stringFormat != value) {
          this._stringFormat = value;
          this.SelectStringFormat(value);
        }
      }
    }
    #endregion

    #region public Color TextColor
    public Color TextColor {
      get {
        return this._textColor;
      }
      set {
        if (this._textColor != value) {
          this._textColor = value;
          this.SelectTextColor(value);
        }
      }
    }
    #endregion

    #region public void TextOut(string text, int x, int y)
    public void TextOut(string text, int x, int y) {
      this.TextOut(text, x, y, text.Length);
    }
    #endregion

    #region public void TextOut(string text, int x, int y, int len)
    public void TextOut(string text, int x, int y, int len) {
      Win32.TextOut(_hdc, x, y, text, text.Length);
    }
    #endregion

    #region private int Border3dStyleToBorder(Border3DStyle border)
    private int Border3dStyleToBorder(Border3DStyle border) {
      switch (border) {
        case Border3DStyle.RaisedOuter:
          return 1;

        case Border3DStyle.SunkenOuter:
          return 2;

        case Border3DStyle.RaisedInner:
          return 4;

        case Border3DStyle.Raised:
          return 5;

        case Border3DStyle.Etched:
          return 6;

        case Border3DStyle.SunkenInner:
          return 8;

        case Border3DStyle.Bump:
          return 9;

        case Border3DStyle.Sunken:
          return 10;

        case Border3DStyle.Flat:
          return 0x4000;
      }
      return 0;
    }
    #endregion

    #region private int CharWidth(FontInfo info, char ch, int count)
    private int CharWidth(FontInfo info, char ch, int count) {
      if (count == 0x7fffffff) {
        return 0x7fffffff;
      }
      if (count <= 0) {
        return 0;
      }
      if (this.IsFontMonoSpaced()) {
        return (count * info.FontWidth);
      }
      return (count * ((GdiFontInfo)info).CharWidth(ch));
    }
    #endregion

    public int CharWidth(char ch, int count) {
      return this.CharWidth(this.CurrentInfo, ch, count);
    }

    public int CharWidth(char ch, int width, out int count) {
      return this.CharWidth(this.CurrentInfo, ch, width, out count);
    }

    private int CharWidth(FontInfo info, char ch, int width, out int count) {
      if (width == 0x7fffffff) {
        count = 0x7fffffff;
        return 0x7fffffff;
      }
      int fontWidth = 0;
      count = 0;
      if (width > 0) {
        if (this.IsFontMonoSpaced()) {
          fontWidth = info.FontWidth;
        } else {
          fontWidth = ((GdiFontInfo)info).CharWidth(ch);
        }
        if (fontWidth > 0) {
          count = width / fontWidth;
          return (count * fontWidth);
        }
      }
      return 0;
    }

    public void Clear() {
      this.FreeBuffer();
      this._penPtr = IntPtr.Zero;
      this._brushPtr = IntPtr.Zero;
      this.ClearPens();
      this.ClearBrushes();
      this.ClearColors();
      this.ClearFonts();
      this._font = null;
      this._fontInfos = null;
      this.Init();
    }

    private void ClearColors() {
      this._backColor = Color.Empty;
      this._textColor = Color.Empty;
      this._penColor = Color.Empty;
    }

    private void ClearFonts() {
      this._fontTable.Clear();
    }


    private void Init() {
      this.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f);
      this.TextColor = Color.Black;
      this.BackColor = Color.White;
      this.PenColor = Color.Black;
      this.Opaque = true;
      this._stringFormat = StringFormat.GenericTypographic;
      this._stringFormat.Trimming = StringTrimming.None;
      this._stringFormat.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
      this.SelectStringFormat(this._stringFormat);
    }

    private void ClearBrushes() {
      foreach (DictionaryEntry entry in this.BrushTable) {
        Win32.DeleteObject((IntPtr)entry.Value);
      }
      this._brushTable.Clear();
    }

    private void ClearPens() {
      foreach (DictionaryEntry entry in this.PenTable) {
        Win32.DeleteObject((IntPtr)entry.Value);
      }
      this._penTable.Clear();
    }

    private object CreateBrush(Color color) {
      return Win32.CreateSolidBrush(Win32.ColorToGdiColor(color));
    }

    private FontInfos CreateFontInfos(Font font) {
      return new GdiFontInfos(font, this._measureDC);
    }

    private object CreatePen(Color color) {
      return Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
    }

    public void DrawDotLine(int x1, int y1, int x2, int y2, Color color) {
      int num = 0;
      if (x1 == x2) {
        while (num < (y2 - y1)) {
          Win32.SetPixel(this._hdc, x1, num + y1, color);
          num += 2;
        }
      } else if (y1 == y2) {
        while (num < (x2 - x1)) {
          Win32.SetPixel(this._hdc, num + x1, y1, color);
          num += 2;
        }
      }
    }

    #region public void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides)
    public void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides) {
      this.DrawEdge(ref rect, border, sides, 0);
    }
    #endregion

    #region public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags)
    public virtual void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags) {
      Win32.DrawEdge(this._hdc, ref rect, this.Border3dStyleToBorder(border), this.Border3DSideToSide(sides) | flags);
    }
    #endregion

    #region public void DrawFocusRect(Rectangle rect, Color color)
    public void DrawFocusRect(Rectangle rect, Color color) {
      this.DrawFocusRect(rect.Left, rect.Top, rect.Width, rect.Height, color);
    }
    #endregion

    #region public virtual void DrawFocusRect(int x, int y, int width, int height, Color color)
    public virtual void DrawFocusRect(int x, int y, int width, int height, Color color) {
      this.DrawDotLine(x, y, x + width, y, color);
      this.DrawDotLine(x, y, x, y + height, color);
      this.DrawDotLine(x + width, y, x + width, y + height, color);
      this.DrawDotLine(x, y + height, x + width, y + height, color);
    }
    #endregion

    #region public virtual void DrawImage(ImageList images, int index, Rectangle rect)
    public virtual void DrawImage(ImageList images, int index, Rectangle rect) {
      if (images != null) {
        uint maxValue = uint.MaxValue;
        Win32.ImageList_DrawEx(images.Handle, index, this._hdc, rect.Left, rect.Top, rect.Width, rect.Height, (int)maxValue, (int)maxValue, 1);
      }
    }
    #endregion

    public virtual void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height) {
      Win32.RoundRect(this._hdc, left, top, right + 1, bottom + 1, width + 1, height + 1);
    }

    public virtual void DrawText(string text, int len, Rectangle rect) {
      if (text != null) {
        string s = (len == -1) ? text : text.Substring(0, len);
        Win32.DrawText(this._hdc, s, s.Length, ref rect, this._formatFlags);
      }
    }

    public virtual void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect) {
      Win32.GdiRect rect2 = new Win32.GdiRect(rect);
      Win32.DrawThemeBackground(handle, this._hdc, partID, stateID, ref rect2, IntPtr.Zero);
    }

    public virtual void DrawWave(Rectangle rect, Color color) {
      int num = rect.Left - (rect.Left % 6);
      int num2 = rect.Right % 6;
      int num3 = (num2 != 0) ? (rect.Right + (6 - num2)) : rect.Right;
      int count = (num3 - num) >> 1;
      if (count < 4) {
        count = 4;
      } else {
        num2 = (count - 4) / 3;
        if (((count - 4) % 3) != 0) {
          num2++;
        }
        count = 4 + (num2 * 3);
      }
      Point[] points = new Point[count];
      for (int i = 0; i < count; i++) {
        points[i].X = num + (i * 2);
        points[i].Y = rect.Bottom - 1;
        switch ((i % 3)) {
          case 0:
            points[i].Y -= 2;
            break;

          case 2:
            points[i].Y += 2;
            break;
        }
      }
      IntPtr ptr = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
      IntPtr ptr2 = Win32.SelectObject(this._hdc, ptr);
      Win32.PolyBezier(this._hdc, points, count);
      Win32.SelectObject(this._hdc, ptr2);
      Win32.DeleteObject(ptr);
    }

    public void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2) {
      this.FillGradient(rect.Left, rect.Top, rect.Width, rect.Height, beginColor, endColor, point1, point2);
    }

    public virtual void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2) {
      //System.Drawing.Graphics gr = this.SafeEndPaint();
      //try {
      //  Brush brush = new LinearGradientBrush(point1, point2, beginColor, endColor);
      //  gr.FillRectangle(brush, x, y, width, height);
      //  brush.Dispose();
      //} finally {
      //  this.SafeBeginPaint(gr);
      //}
    }

    public virtual void FillPolygon(Color color, Point[] points) {
      Color backColor = this.BackColor;
      Color foreColor = this.PenColor;
      try {
        this.BackColor = color;
        this.PenColor = color;
        Win32.Polygon(this._hdc, points, points.Length);
      } finally {
        this.BackColor = backColor;
        this.PenColor = foreColor;
      }
    }

    public void FillRectangle(Rectangle rect) {
      this.FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public void FillRectangle(Color color, Rectangle rect) {
      Color backColor = this.BackColor;
      try {
        this.BackColor = color;
        this.FillRectangle(rect);
      } finally {
        this.BackColor = backColor;
      }
    }

    public virtual void FillRectangle(int x, int y, int width, int height) {
      Win32.FillRect(this._hdc, x, y, width, height, this._brushPtr);
    }

    public void FillRectangle(Color color, int x, int y, int width, int height) {
      Color backColor = this.BackColor;
      try {
        this.BackColor = color;
        this.FillRectangle(x, y, width, height);
      } finally {
        this.BackColor = backColor;
      }
    }

    private void FreeBuffer() {
      this._buffer = null;
      this._bufferSize = 0;
    }

    private int[] GetBuffer(int len) {
      return this.GetBuffer(len, -1);
    }

    private int[] GetBuffer(int len, int space) {
      if (!this.IsFontMonoSpaced() && (space < 0)) {
        return null;
      }
      if (this._bufferSize < len) {
        this.FreeBuffer();
        this._bufferSize = len;
        this._buffer = new int[this._bufferSize];
        for (int i = 0; i < this._bufferSize; i++) {
          this._buffer[i] = (space >= 0) ? space : this.FontWidth;
        }
      }
      return this._buffer;
    }

    private void InitDC(IntPtr hFont) {
      if (this._hdc != IntPtr.Zero) {
        Win32.SelectObject(this._hdc, hFont);
      }
    }

    private bool IsFontMonoSpaced() {
      return false;
    }

    public virtual void RestoreClip(IntPtr rgn) {
      Win32.SelectClipRgn(this._hdc, rgn);
      Win32.DeleteObject(rgn);
    }

    #region private System.Drawing.Graphics SafeEndPaint()
    private System.Drawing.Graphics SafeEndPaint() {
      //this._safexForm = this._xForm;
      //this._safeTransformed = this._transFormed;
      //System.Drawing.Graphics graphics = this._graphics;
      //this.EndPaint();
      //if (this._safeTransformed) {
      //  graphics.Transform = new Matrix(this._safexForm.eM11, this._safexForm.eM12, this._safexForm.eM21, this._safexForm.eM22, this._safexForm.eDx, this._safexForm.eDy);
      //}
      //return graphics;
      return null;
    }
    #endregion

    #region private void SafeBeginPaint(System.Drawing.Graphics gr)
    private void SafeBeginPaint(System.Drawing.Graphics gr) {
      //if (this._safeTransformed) {
      //  gr.Transform = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
      //}
      //this.BeginPaint(gr);
      //if (this._safeTransformed) {
      //  this._xForm = this._safexForm;
      //  Win32.SetWorldTransform(this._hdc, ref this._xForm);
      //  this._transFormed = true;
      //}
    }
    #endregion

    public virtual IntPtr SaveClip(Rectangle rect) {
      IntPtr rgn = Win32.CreateRectRgnIndirect(rect);
      Win32.GetClipRgn(this._hdc, rgn);
      return rgn;
    }

    private void SelectBrush(Color color, bool select) {
      this._brushPtr = (IntPtr)this.SelectBrush(color);
      if (select && (this._hdc != IntPtr.Zero)) {
        Win32.SetBkColor(this._hdc, Win32.ColorToGdiColor(color));
      }
    }

    private object SelectBrush(Color color) {
      object obj2 = this._brushTable[color];
      if (obj2 == null) {
        obj2 = this.CreateBrush(color);
        this._brushTable.Add(color, obj2);
      }
      return obj2;
    }

    private void SelectFont(FontStyle fontStyle) {
      this.fontStyle = fontStyle;
      this._fontInfos.InitStyle(fontStyle);
      this._font = (this.CurrentInfo != null) ? this.CurrentInfo.Font : null;
      if (this.CurrentInfo != null) {
        this.InitDC(this.CurrentInfo.HFont);
      }
    }

    private object GetFontKey(System.Drawing.Font font) {
      return (font.Name + "|" + font.Size.ToString());
    }

    private void SelectFont(Font font, FontStyle fontStyle) {
      if (font != null) {
        this._font = font;
        this.fontStyle = fontStyle;
        object fontKey = this.GetFontKey(font);
        this._fontInfos = (FontInfos)this._fontTable[fontKey];
        if (this._fontInfos == null) {
          this._fontInfos = this.CreateFontInfos(font);
          this._fontTable.Add(fontKey, this._fontInfos);
        }
        this._fontInfos.InitStyle(font.Style);
      }
      if (this.CurrentInfo != null) {
        this.InitDC(this.CurrentInfo.HFont);
      }
    }

    private void SelectOpaque(bool value) {
      if (this._hdc != IntPtr.Zero) {
        Win32.SetBkMode(this._hdc, value ? 2 : 1);
      }
    }

    private object SelectPen(Color color) {
      object obj2 = this._penTable[color];
      if (obj2 == null) {
        obj2 = this.CreatePen(color);
        this._penTable.Add(color, obj2);
      }
      return obj2;
    }

    private void SelectPen(Color color, bool select) {
      this._penPtr = (IntPtr)this.SelectPen(color);
      if (select && (this._hdc != IntPtr.Zero)) {
        Win32.SelectObject(this._hdc, this._penPtr);
      }
    }

    private void SelectStringFormat(StringFormat format) {
      this._formatFlags = 0;
      switch (format.Alignment) {
        case StringAlignment.Near:
          break;

        case StringAlignment.Center:
          this._formatFlags |= 1;
          break;

        case StringAlignment.Far:
          this._formatFlags |= 2;
          break;
      }
      switch (format.LineAlignment) {
        case StringAlignment.Near:
          break;

        case StringAlignment.Center:
          this._formatFlags |= 0x24;
          break;

        case StringAlignment.Far:
          this._formatFlags |= 40;
          break;
      }
      switch (format.HotkeyPrefix) {
        case HotkeyPrefix.None:
          this._formatFlags |= 0x800;
          break;

        case HotkeyPrefix.Hide:
          this._formatFlags |= 0x100000;
          break;
      }
      switch (format.Trimming) {
        case StringTrimming.EllipsisCharacter:
          this._formatFlags |= 0x8000;
          break;

        case StringTrimming.EllipsisWord:
          this._formatFlags |= 0x40000;
          break;

        case StringTrimming.EllipsisPath:
          this._formatFlags |= 0x4000;
          break;
      }
      if ((format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != 0) {
        this._formatFlags |= 0x20000;
      }
      if ((format.FormatFlags & StringFormatFlags.NoClip) != 0) {
        this._formatFlags |= 0x100;
      }
    }

    private void SelectTextColor(Color color) {
      if (this._hdc != IntPtr.Zero) {
        Win32.SetTextColor(this._hdc, Win32.ColorToGdiColor(color));
      }
    }

    public void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image) {
    }

    public int StringWidth(string text) {
      return this.StringWidth(text, 0, -1);
    }

    public int StringWidth(string text, int pos, int len) {
      return this.StringWidth(this.CurrentInfo, text, pos, len);
    }

    private int StringWidth(FontInfo info, string text, int pos, int len) {
      if (len == 0x7fffffff) {
        return 0x7fffffff;
      }
      if ((text == null) || (info == null)) {
        return 0;
      }
      if (len == -1) {
        len = text.Length;
      }
      if (this.IsFontMonoSpaced()) {
        return (len * info.FontWidth);
      }
      int num = 0;
      for (int i = pos; i < (pos + len); i++) {
        num += ((GdiFontInfo)info).CharWidth(text[i]);
      }
      return num;
    }

    public int StringWidth(string text, int width, out int count, bool exact) {
      return this.StringWidth(text, 0, -1, width, out count, exact);
    }

    public int StringWidth(string text, int pos, int len, int width, out int count) {
      return this.StringWidth(text, pos, len, width, out count, true);
    }

    public int StringWidth(string text, int pos, int len, int width, out int count, bool exact) {
      return this.StringWidth(this.CurrentInfo, text, pos, len, width, out count, exact);
    }

    private int StringWidth(FontInfo info, string text, int pos, int len, int width, out int count, bool exact) {
      count = 0;
      if (text == null) {
        return 0;
      }
      if (len == -1) {
        len = text.Length - pos;
      }
      if (width == 0x7fffffff) {
        count = len;
        return 0x7fffffff;
      }
      if (this.IsFontMonoSpaced()) {
        int fontWidth = info.FontWidth;
        if (fontWidth == 0) {
          count = len;
        } else {
          if (exact) {
            count = width / fontWidth;
          } else {
            count = (int)Math.Round((double)(width / fontWidth));
          }
          count = Math.Min(count, len);
        }
        return (count * fontWidth);
      }
      for (int i = pos; i < (pos + len); i++) {
        int num2 = ((GdiFontInfo)info).CharWidth(text[i]);
        width -= num2;
        if (width < 0) {
          if (!exact && (width > (-num2 / 2))) {
            count++;
          }
          break;
        }
        count++;
      }
      return this.StringWidth(info, text, pos, count);
    }

    public void TextOut(string text, Rectangle rect) {
      this.TextOut(text, -1, rect);
    }

    public void TextOut(string text, int len, Rectangle rect) {
      this.TextOut(text, len, rect, rect.Left, rect.Top, false, false, -1);
    }

    public void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque) {
      this.TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, -1);
    }

    public void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque, int space) {
      this.TextOut(text, len, rect, rect.Left, rect.Top, clipped, opaque, space);
    }

    public void TextOut(string text, int len, int x, int y, bool clipped, bool opaque) {
      this.TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, clipped, opaque, -1);
    }

    public void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque) {
      this.TextOut(text, len, rect, x, y, clipped, opaque, -1);
    }

    public virtual void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque, int space) {
      if (text != null) {
        Win32.ExtTextOutW(this._hdc, rect, x, y, (clipped ? 4 : 0) | (opaque ? 2 : 0), text, len, (space > 0) ? this.GetBuffer((len < 0) ? text.Length : 0, space) : null);
      }
    }

    #region private int Border3DSideToSide(Border3DSide sides)
    private int Border3DSideToSide(Border3DSide sides) {
      int num = 0;
      if (sides == Border3DSide.All) {
        num |= 0x80f;
      }
      if ((sides & Border3DSide.Bottom) != 0) {
        num |= 8;
      }
      if ((sides & Border3DSide.Left) != 0) {
        num |= 1;
      }
      if ((sides & Border3DSide.Middle) != 0) {
        num |= 0x800;
      }
      if ((sides & Border3DSide.Right) != 0) {
        num |= 4;
      }
      if ((sides & Border3DSide.Top) != 0) {
        num |= 2;
      }
      return num;
    }
    #endregion

  }
}
