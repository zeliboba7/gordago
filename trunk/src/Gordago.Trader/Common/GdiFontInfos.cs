/**
* @version $Id: GdiFontInfos.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Gordago.Trader.Common {
  public class GdiFontInfos : FontInfos {
    public GdiFontInfos(Font font, IntPtr dc)
      : base(font, dc) {
    }

    protected override FontInfo CreateFontInfo(Font font) {
      return new GdiFontInfo(font, base.dc);
    }

    protected override void SelectFontInfo(FontInfo fontInfo) {
      Win32.SelectObject(base.dc, fontInfo.HFont);
    }
  }
}
