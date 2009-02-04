/**
* @version $Id: ChartDraw.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  using System.Collections;

  class ChartDraw {
    private Color backColor;
    private Hashtable brushTable = new Hashtable();
    private Font font;
    private FontInfos fontInfos;
    private System.Drawing.FontStyle fontStyle;
    private Hashtable fontTable = new Hashtable();
    private Color foreColor;
    protected IntPtr measureDC;
    private IntPtr measureHFont;
    private IntPtr oldMeasureFont;
    private bool opaque = true;
    private Hashtable penTable = new Hashtable();
    private System.Drawing.StringFormat stringFormat;
    private Color textColor;

    public ChartDraw() {
      IntPtr dC = Win32.GetDC(IntPtr.Zero);
      System.Drawing.Font font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f);
      try {
        this.measureDC = Win32.CreateCompatibleDC(dC);
        this.measureHFont = font.ToHfont();
        this.oldMeasureFont = Win32.SelectObject(this.measureDC, this.measureHFont);
      } finally {
        Win32.ReleaseDC(IntPtr.Zero, dC);
      }
      this.Init();
    }

    #region ~ChartDraw()
    ~ChartDraw() {
      this.ClearBrushes();
      this.ClearPens();
      this.ClearFonts();
      Win32.SelectObject(this.measureDC, this.oldMeasureFont);
      if (this.measureHFont != IntPtr.Zero) {
        Win32.DeleteObject(this.measureHFont);
      }
      Win32.DeleteDC(this.measureDC);
    }
    #endregion

    public virtual void Clear() {
      this.ClearPens();
      this.ClearBrushes();
      this.ClearColors();
      this.ClearFonts();
      this.font = null;
      this.fontInfos = null;
      this.Init();
    }

    protected virtual void ClearBrushes() {
      this.brushTable.Clear();
    }

    protected virtual void ClearColors() {
      this.backColor = Color.Empty;
      this.textColor = Color.Empty;
      this.foreColor = Color.Empty;
    }

    protected virtual void ClearFonts() {
      this.fontTable.Clear();
    }

    protected virtual void ClearPens() {
      this.penTable.Clear();
    }

    protected virtual object CreateBrush(Color color) {
      return null;
    }

    protected virtual FontInfos CreateFontInfos(System.Drawing.Font font) {
      return new FontInfos(font, this.measureDC);
    }

    protected virtual object CreatePen(Color color) {
      return null;
    }

    protected virtual object GetFontKey(System.Drawing.Font font) {
      return (font.Name + "|" + font.Size.ToString());
    }

    protected void Init() {
      this.Font = new System.Drawing.Font(FontFamily.GenericMonospace, 10f);
      this.TextColor = Color.Black;
      this.BackColor = Color.White;
      this.ForeColor = Color.Black;
      this.Opaque = true;
      this.stringFormat = StringFormat.GenericTypographic;
      this.stringFormat.Trimming = StringTrimming.None;
      this.stringFormat.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
      this.SelectStringFormat(this.stringFormat);
    }

    protected virtual object SelectBrush(Color color) {
      object obj2 = this.brushTable[color];
      if (obj2 == null) {
        obj2 = this.CreateBrush(color);
        this.brushTable.Add(color, obj2);
      }
      return obj2;
    }

    protected virtual void SelectBrush(Color color, bool select) {
    }

    protected virtual void SelectFont(System.Drawing.FontStyle fontStyle) {
      this.fontStyle = fontStyle;
      this.fontInfos.InitStyle(fontStyle);
      this.font = (this.CurrentInfo != null) ? this.CurrentInfo.Font : null;
    }

    protected virtual void SelectFont(System.Drawing.Font font, System.Drawing.FontStyle fontStyle) {
      if (font != null) {
        this.font = font;
        this.fontStyle = fontStyle;
        object fontKey = this.GetFontKey(font);
        this.fontInfos = (FontInfos)this.fontTable[fontKey];
        if (this.fontInfos == null) {
          this.fontInfos = this.CreateFontInfos(font);
          this.fontTable.Add(fontKey, this.fontInfos);
        }
        this.fontInfos.InitStyle(font.Style);
      }
    }

    protected virtual void  SelectOpaque(bool value) {
    }

    protected virtual object SelectPen(Color color) {
      object obj2 = this.penTable[color];
      if (obj2 == null) {
        obj2 = this.CreatePen(color);
        this.penTable.Add(color, obj2);
      }
      return obj2;
    }

    protected virtual void SelectPen(Color color, bool select) {
    }

    protected virtual void SelectStringFormat(System.Drawing.StringFormat format) {
    }

    protected virtual void SelectTextColor(Color color) {
    }

    public Color BackColor {
      get {
        return this.backColor;
      }
      set {
        if (this.backColor != value) {
          this.backColor = value;
          this.SelectBrush(this.backColor, true);
        }
      }
    }

    protected Hashtable BrushTable {
      get {
        return this.brushTable;
      }
    }

    protected FontInfo CurrentInfo {
      get {
        if (this.fontInfos == null) {
          return null;
        }
        return this.fontInfos.CurrentInfo;
      }
    }

    public System.Drawing.Font Font {
      get {
        return this.font;
      }
      set {
        if (this.font != value) {
          this.font = value;
          if (value != null) {
            this.SelectFont(this.font, this.font.Style);
          }
        }
      }
    }

    public int FontHeight {
      get {
        return this.fontInfos.FontHeight;
      }
    }

    public System.Drawing.FontStyle FontStyle {
      get {
        return this.fontStyle;
      }
      set {
        if (this.fontStyle != value) {
          this.SelectFont(value);
        }
      }
    }

    protected Hashtable FontTable {
      get {
        return this.fontTable;
      }
    }

    public int FontWidth {
      get {
        if (this.CurrentInfo == null) {
          return 0;
        }
        return this.CurrentInfo.FontWidth;
      }
    }

    public Color ForeColor {
      get {
        return this.foreColor;
      }
      set {
        if (this.foreColor != value) {
          this.foreColor = value;
          this.SelectPen(this.foreColor, true);
        }
      }
    }

    public bool IsMonoSpaced {
      get {
        return this.fontInfos.IsMonoSpaced;
      }
    }

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

    protected Hashtable PenTable {
      get {
        return this.penTable;
      }
    }

    public System.Drawing.StringFormat StringFormat {
      get {
        return this.stringFormat;
      }
      set {
        if (this.stringFormat != value) {
          this.stringFormat = value;
          this.SelectStringFormat(value);
        }
      }
    }

    public Color TextColor {
      get {
        return this.textColor;
      }
      set {
        if (this.textColor != value) {
          this.textColor = value;
          this.SelectTextColor(value);
        }
      }
    }

  }
}
