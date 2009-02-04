/**
* @version $Id: GdiChartDraw.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.Drawing;
  using System.Drawing.Drawing2D;
  using System.Drawing.Imaging;
  using System.Collections;
  using System.Drawing.Text;
  using System.Diagnostics;

  class GdiChartDraw : ChartDraw, IChartDraw {

    #region Private Method
    private IntPtr _brushPtr;
    private int[] _buffer;
    private int _bufferSize;
    private int _formatFlags;
    private Graphics _graphics;
    private IntPtr _hdc;
    private int _oldBackColor;
    private IntPtr _oldBrushPtr;
    private IntPtr _oldFontPtr;
    private int _oldMode = 1;
    private bool _savedOpaque;
    private IntPtr _savedPenPtr;
    private int _savedTextColor;
    private IntPtr _penPtr;
    private bool _safeTransformed;
    private Win32.XFORM _safexForm;
    private bool _transFormed;
    private Win32.XFORM _xForm;
    #endregion

    #region public Graphics Graphics
    public Graphics Graphics {
      get {
        return this._graphics;
      }
    }
    #endregion

    #region public IntPtr Handle
    public IntPtr Handle {
      get {
        return this._hdc;
      }
    }
    #endregion

    #region public Matrix Transformation
    public Matrix Transformation {
      get {
        if (this._graphics != null) {
          return this._graphics.Transform;
        }
        return null;
      }
    }
    #endregion

    #region public void BeginPaint(Graphics graphics)
    public void BeginPaint(Graphics graphics) {
      this._graphics = graphics;
      this._hdc = graphics.GetHdc();
      this._oldFontPtr = (base.CurrentInfo != null) ? Win32.SelectObject(this._hdc, base.CurrentInfo.HFont) : IntPtr.Zero;
      this._savedTextColor = Win32.SetTextColor(this._hdc, Win32.ColorToGdiColor(base.TextColor));
      this._oldBackColor = Win32.SetBkColor(this._hdc, Win32.ColorToGdiColor(base.BackColor));
      this._savedOpaque = Win32.SetBkMode(this._hdc, base.Opaque ? 2 : 1) == 2;
      this._oldMode = Win32.GetGraphicsMode(this._hdc);
      this.SelectBrush(base.BackColor, true);
      this.SelectPen(base.ForeColor, false);
      this._savedPenPtr = Win32.SelectObject(this._hdc, this._penPtr);
      this._oldBrushPtr = Win32.SelectObject(this._hdc, this._brushPtr);
    }
    #endregion

    #region public void EndPaint()
    public void EndPaint() {
      if (this._oldFontPtr != IntPtr.Zero) {
        Win32.SelectObject(this._hdc, this._oldFontPtr);
      }
      Win32.SetTextColor(this._hdc, this._savedTextColor);
      Win32.SetBkColor(this._hdc, this._oldBackColor);
      Win32.SetBkMode(this._hdc, this._savedOpaque ? 2 : 1);
      Win32.SelectObject(this._hdc, this._savedPenPtr);
      Win32.SelectObject(this._hdc, this._oldBrushPtr);
      this.EndTransform();
      this._graphics.ReleaseHdc(this._hdc);
      this._graphics = null;
      this._hdc = IntPtr.Zero;
    }
    #endregion

    #region public virtual void DrawLine(int x1, int y1, int x2, int y2)
    public virtual void DrawLine(int x1, int y1, int x2, int y2) {
      Win32.MoveToEx(this._hdc, x1, y1, IntPtr.Zero);
      
      if (x1 == x2) y2++;
      if (y1 == y2) x2++;

      Win32.LineTo(this._hdc, x2, y2);
    }
    #endregion

    #region public virtual void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle style)
    public virtual void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle style) {

      IntPtr ptr = Win32.CreatePen(this.DashStyleToPenStyle(style), width, Win32.ColorToGdiColor(color));
      IntPtr ptr2 = Win32.SelectObject(this._hdc, ptr);
      this.DrawLine(x1, y1, x2, y2);
      Win32.SelectObject(this._hdc, ptr2);
      Win32.DeleteObject(ptr);
    }
    #endregion

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

    #region public int CharWidth(char ch, int count)
    public int CharWidth(char ch, int count) {
      return this.CharWidth(base.CurrentInfo, ch, count);
    }
    #endregion

    #region public int CharWidth(char ch, int width, out int count)
    public int CharWidth(char ch, int width, out int count) {
      return this.CharWidth(base.CurrentInfo, ch, width, out count);
    }
    #endregion

    #region private int CharWidth(FontInfo info, char ch, int width, out int count)
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
    #endregion

    #region public override void Clear()
    public override void Clear() {
      this.FreeBuffer();
      this._penPtr = IntPtr.Zero;
      this._brushPtr = IntPtr.Zero;
      base.Clear();
    }
    #endregion

    #region protected override void ClearBrushes()
    protected override void ClearBrushes() {
      foreach (DictionaryEntry entry in base.BrushTable) {
        Win32.DeleteObject((IntPtr)entry.Value);
      }
      base.ClearBrushes();
    }
    #endregion

    #region protected override void ClearPens()
    protected override void ClearPens() {
      foreach (DictionaryEntry entry in base.PenTable) {
        Win32.DeleteObject((IntPtr)entry.Value);
      }
      base.ClearPens();
    }
    #endregion

    #region protected override object CreateBrush(Color color)
    protected override object CreateBrush(Color color) {
      return Win32.CreateSolidBrush(Win32.ColorToGdiColor(color));
    }
    #endregion

    #region protected override FontInfos CreateFontInfos(Font font)
    protected override FontInfos CreateFontInfos(Font font) {
      return new GdiFontInfos(font, base.measureDC);
    }
    #endregion

    #region protected override object CreatePen(Color color)
    protected override object CreatePen(Color color) {
      return Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
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

    #region public virtual void DrawDotLine(int x1, int y1, int x2, int y2, Color color)
    public virtual void DrawDotLine(int x1, int y1, int x2, int y2, Color color) {
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
    #endregion

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

    public void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr) {
    }

    #region public virtual void DrawPolygon(Point[] points, Color color)
    public virtual void DrawPolygon(Point[] points, Color color) {
      if (points != null) {
        base.BackColor = color;
        IntPtr ptr = Win32.SelectObject(this._hdc, this._brushPtr);
        IntPtr ptr2 = Win32.CreatePen(0, 1, Win32.ColorToGdiColor(color));
        IntPtr ptr3 = Win32.SelectObject(this._hdc, ptr2);
        Win32.Polygon(this._hdc, points, points.Length);
        Win32.SelectObject(this._hdc, ptr);
        Win32.SelectObject(this._hdc, ptr3);
        Win32.DeleteObject(ptr2);
      }
    }
    #endregion

    #region public void DrawRectangle(Rectangle rect)
    public void DrawRectangle(Rectangle rect) {
      this.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
    }
    #endregion

    public virtual void DrawRectangle(int x, int y, int width, int height) {
      Win32.FrameRect(this._hdc, x, y, width + 1, height + 1, this._brushPtr);
    }

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

    public void ExcludeClipRect(Rectangle rect) {
      this.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public virtual void ExcludeClipRect(int x, int y, int width, int height) {
      Win32.ExcludeClipRect(this._hdc, x, y, x + width, y + height);
    }

    public void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2) {
      this.FillGradient(rect.Left, rect.Top, rect.Width, rect.Height, beginColor, endColor, point1, point2);
    }

    public virtual void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2) {
      System.Drawing.Graphics gr = this.SafeEndPaint();
      try {
        Brush brush = new LinearGradientBrush(point1, point2, beginColor, endColor);
        gr.FillRectangle(brush, x, y, width, height);
        brush.Dispose();
      } finally {
        this.SafeBeginPaint(gr);
      }
    }

    public virtual void FillPolygon(Color color, Point[] points) {
      Color backColor = base.BackColor;
      Color foreColor = base.ForeColor;
      try {
        base.BackColor = color;
        base.ForeColor = color;
        Win32.Polygon(this._hdc, points, points.Length);
      } finally {
        base.BackColor = backColor;
        base.ForeColor = foreColor;
      }
    }

    public void FillRectangle(Rectangle rect) {
      this.FillRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public void FillRectangle(Color color, Rectangle rect) {
      Color backColor = base.BackColor;
      try {
        base.BackColor = color;
        this.FillRectangle(rect);
      } finally {
        base.BackColor = backColor;
      }
    }

    public virtual void FillRectangle(int x, int y, int width, int height) {
      Win32.FillRect(this._hdc, x, y, width, height, this._brushPtr);
    }

    public void FillRectangle(Color color, int x, int y, int width, int height) {
      Color backColor = base.BackColor;
      try {
        base.BackColor = color;
        this.FillRectangle(x, y, width, height);
      } finally {
        base.BackColor = backColor;
      }
    }

    private void FreeBuffer() {
      this._buffer = null;
      this._bufferSize = 0;
    }

    protected internal int[] GetBuffer(int len) {
      return this.GetBuffer(len, -1);
    }

    protected internal int[] GetBuffer(int len, int space) {
      if (!this.IsFontMonoSpaced() && (space < 0)) {
        return null;
      }
      if (this._bufferSize < len) {
        this.FreeBuffer();
        this._bufferSize = len;
        this._buffer = new int[this._bufferSize];
        for (int i = 0; i < this._bufferSize; i++) {
          this._buffer[i] = (space >= 0) ? space : base.FontWidth;
        }
      }
      return this._buffer;
    }

    private void InitDC(IntPtr hFont) {
      if (this._hdc != IntPtr.Zero) {
        Win32.SelectObject(this._hdc, hFont);
      }
    }

    protected virtual bool IsFontMonoSpaced() {
      return false;
    }

    public virtual void RestoreClip(IntPtr rgn) {
      Win32.SelectClipRgn(this._hdc, rgn);
      Win32.DeleteObject(rgn);
    }

    protected internal void SafeBeginPaint(System.Drawing.Graphics gr) {
      if (this._safeTransformed) {
        gr.Transform = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
      }
      this.BeginPaint(gr);
      if (this._safeTransformed) {
        this._xForm = this._safexForm;
        Win32.SetWorldTransform(this._hdc, ref this._xForm);
        this._transFormed = true;
      }
    }

    protected internal System.Drawing.Graphics SafeEndPaint() {
      this._safexForm = this._xForm;
      this._safeTransformed = this._transFormed;
      System.Drawing.Graphics graphics = this._graphics;
      this.EndPaint();
      if (this._safeTransformed) {
        graphics.Transform = new Matrix(this._safexForm.eM11, this._safexForm.eM12, this._safexForm.eM21, this._safexForm.eM22, this._safexForm.eDx, this._safexForm.eDy);
      }
      return graphics;
    }

    public virtual IntPtr SaveClip(Rectangle rect) {
      IntPtr rgn = Win32.CreateRectRgnIndirect(rect);
      Win32.GetClipRgn(this._hdc, rgn);
      return rgn;
    }

    protected override void SelectBrush(Color color, bool select) {
      this._brushPtr = (IntPtr)this.SelectBrush(color);
      if (select && (this._hdc != IntPtr.Zero)) {
        Win32.SetBkColor(this._hdc, Win32.ColorToGdiColor(color));
      }
    }

    protected override void SelectFont(FontStyle fontStyle) {
      base.SelectFont(fontStyle);
      if (base.CurrentInfo != null) {
        this.InitDC(base.CurrentInfo.HFont);
      }
    }

    protected override void SelectFont(Font font, FontStyle fontStyle) {
      base.SelectFont(font, fontStyle);
      if (base.CurrentInfo != null) {
        this.InitDC(base.CurrentInfo.HFont);
      }
    }

    protected override void SelectOpaque(bool value) {
      if (this._hdc != IntPtr.Zero) {
        Win32.SetBkMode(this._hdc, value ? 2 : 1);
      }
    }

    protected override void SelectPen(Color color, bool select) {
      this._penPtr = (IntPtr)this.SelectPen(color);
      if (select && (this._hdc != IntPtr.Zero)) {
        Win32.SelectObject(this._hdc, this._penPtr);
      }
    }

    protected override void SelectStringFormat(StringFormat format) {
      this._formatFlags = 0;
      switch (format.Alignment) {
        case StringAlignment.Near:
          this._formatFlags = this._formatFlags;
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
          this._formatFlags = this._formatFlags;
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

    protected override void SelectTextColor(Color color) {
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
      return this.StringWidth(base.CurrentInfo, text, pos, len);
    }

    protected virtual int StringWidth(FontInfo info, string text, int pos, int len) {
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
      return this.StringWidth(base.CurrentInfo, text, pos, len, width, out count, exact);
    }

    protected virtual int StringWidth(FontInfo info, string text, int pos, int len, int width, out int count, bool exact) {
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

    public void TextOut(string text, int len, Rectangle rect) {
      this.TextOut(text, len, rect, rect.Left, rect.Top, false, false, -1);
    }

    public void TextOut(string text, int len, int x, int y) {
      this.TextOut(text, len, new Rectangle(x, y, 0, 0), x, y, false, false, -1);
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

    public void Transform(int x, int y, float scaleX, float scaleY) {
      this._transFormed = true;
      this._oldMode = Win32.SetGraphicsMode(this._hdc, 2);
      this._xForm = new Win32.XFORM(scaleX, 0f, 0f, scaleY, x * scaleX, y * scaleY);
      Win32.SetWorldTransform(this._hdc, ref this._xForm);
      //Trace.WriteLine(string.Format("Transform: {0},{1}", x, y));
    }

    public void EndTransform() {
      if (this._transFormed) {
        this._xForm = new Win32.XFORM(1f, 0f, 0f, 1f, 0f, 0f);
        Win32.SetWorldTransform(this._hdc, ref this._xForm);
        this._transFormed = false;
      }
    }

    public void IntersectClipRect(Rectangle rect) {
      this.IntersectClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public virtual void IntersectClipRect(int x, int y, int width, int height) {
      Win32.IntersectClipRect(this._hdc, x, y, x + width, y + height);
      //Trace.WriteLine(string.Format("IntersectClipRect: {0}", new Rectangle(x,y, width, height)));
    }

  }
}
