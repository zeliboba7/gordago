/**
* @version $Id: FontInfos.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace Gordago.Trader.Common {
  public class FontInfos {
    protected IntPtr dc;
    private Font font;
    private int fontHeight;
    private FontInfo fontInfo;
    private bool isMonoSpaced;
    private Hashtable stylesTable;

    public FontInfos(Font font, IntPtr dc) {
      this.font = font;
      this.dc = dc;
      this.stylesTable = new Hashtable();
    }

    protected virtual FontInfo CreateFontInfo(Font font) {
      return new FontInfo(font, this.dc);
    }

    ~FontInfos() {
      this.stylesTable.Clear();
    }

    public FontInfo InitStyle(FontStyle style) {
      if (this.font == null) {
        return null;
      }
      this.fontInfo = (FontInfo)this.stylesTable[style];
      if (this.fontInfo == null) {
        Font font = this.font;
        if (this.font.Style != style) {
          font = new Font(this.font.FontFamily, this.font.Size, style);
        }
        this.fontInfo = this.CreateFontInfo(font);
        this.stylesTable.Add(style, this.fontInfo);
        this.UpdateMonoSpaced();
      } else {
        this.SelectFontInfo(this.fontInfo);
      }
      return this.fontInfo;
    }

    protected virtual void SelectFontInfo(FontInfo fontInfo) {
    }

    private bool UpdateMonoSpaced() {
      bool isMonoSpaced = this.isMonoSpaced;
      int fontHeight = this.fontHeight;
      this.isMonoSpaced = false;
      this.fontHeight = 0;
      if (this.fontInfo != null) {
        this.isMonoSpaced = this.fontInfo.IsMonoSpaced;
        int fontWidth = this.fontInfo.FontWidth;
        IDictionaryEnumerator enumerator = this.stylesTable.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext()) {
          if (this.isMonoSpaced && (((FontInfo)enumerator.Value).FontWidth != fontWidth)) {
            this.isMonoSpaced = false;
          }
          this.fontHeight = Math.Max(this.fontHeight, ((FontInfo)enumerator.Value).FontHeight);
        }
      }
      if (isMonoSpaced == this.isMonoSpaced) {
        return (fontHeight != this.fontHeight);
      }
      return true;
    }

    public FontInfo CurrentInfo {
      get {
        return this.fontInfo;
      }
    }

    public int FontHeight {
      get {
        return this.fontHeight;
      }
    }

    public bool IsMonoSpaced {
      get {
        return this.isMonoSpaced;
      }
    }
  }
}
