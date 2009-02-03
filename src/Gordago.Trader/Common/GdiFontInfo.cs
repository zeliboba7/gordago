/**
* @version $Id: GdiFontInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Collections;
  using System.Drawing;

  class GdiFontInfo : FontInfo {
    private int[] asciTable;
    private Hashtable chartable;

    public GdiFontInfo(Font font, IntPtr dc)
      : base(font, dc) {
      this.asciTable = new int[0x100];
      if (!this.IsFontMonoSpaced()) {
        this.InitChartable();
      }
    }

    public int CharWidth(char ch) {
      if (this.IsFontMonoSpaced()) {
        return base.FontWidth;
      }
      if (ch <= '\x00ff') {
        return this.asciTable[ch];
      }
      object charWidth = this.chartable[ch];
      if (charWidth == null) {
        charWidth = this.GetCharWidth(ch);
        this.chartable.Add(ch, charWidth);
      }
      return (int)charWidth;
    }

    protected int GetCharWidth(char ch) {
      Size size = new Size(0, 0);
      Win32.SelectObject(base.dc, base.HFont);
      Win32.GetTextExtentPoint32(base.dc, ch.ToString(), 1, ref size);
      return size.Width;
    }

    protected void InitChartable() {
      this.chartable = new Hashtable();
      Win32.GetCharABCWidths(base.dc, 0, 0xff, ref this.asciTable);
    }

    protected virtual bool IsFontMonoSpaced() {
      return false;
    }
  }
}
