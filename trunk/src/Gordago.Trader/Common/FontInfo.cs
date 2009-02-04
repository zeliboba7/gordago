/**
* @version $Id: FontInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  public class FontInfo {
    protected IntPtr dc;
    private Font font;
    private Win32.TextMetrics fontMetrics;
    private IntPtr hFont;
    private bool isMonoSpaced;

    public FontInfo(System.Drawing.Font font, IntPtr dc) {
      this.font = font;
      this.hFont = font.ToHfont();
      this.dc = dc;
      Win32.SelectObject(dc, this.hFont);
      Win32.GetTextMetrics(dc, ref this.fontMetrics);
      this.isMonoSpaced = (this.fontMetrics.PitchAndFamily & 1) == 0;
    }

    ~FontInfo() {
      if (this.hFont != IntPtr.Zero) {
        Win32.DeleteObject(this.hFont);
      }
    }

    public System.Drawing.Font Font {
      get {
        return this.font;
      }
    }

    public int FontHeight {
      get {
        return this.fontMetrics.Height;
      }
    }

    public int FontWidth {
      get {
        return this.fontMetrics.AveCharWidth;
      }
    }

    public IntPtr HFont {
      get {
        return this.hFont;
      }
    }

    public bool IsMonoSpaced {
      get {
        return this.isMonoSpaced;
      }
    }
  }
}
