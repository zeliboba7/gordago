/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using Gordago.Properties;

namespace Gordago.Analysis.Chart {
  public class ChartObjectTrendLine: ChartObjectLine {

    public ChartObjectTrendLine(string name) : base(name, true) {
      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COTrendLine));
      this.Image = Resources.m_co_tradeline;
      this.TypeName = "Line";
    }

    #region public override string ToolTipText
    public override string ToolTipText {
      get {
        if(base.ToolTipText == "") {

          DateTime time1 = this.COPoints[0].Time;
          DateTime time2 = this.COPoints[1].Time;

          float price1 = this.COPoints[0].Price;
          float price2 = this.COPoints[1].Price;

          string str = "Trend line\n\n";
          str += "Point 1:\n";
          str += "  Time " + time1.ToShortDateString() + " " + time1.ToShortTimeString() + "\n";
          str += "  Price " + SymbolManager.ConvertToCurrencyString(price1, this.ChartBox.DecimalDigits) + "\n\n";
          str += "Point 2:" + "\n";
          str += "  Time " + time2.ToShortDateString() + " " + time2.ToShortTimeString() + "\n";
          str += "  Price " + SymbolManager.ConvertToCurrencyString(price2, this.ChartBox.DecimalDigits);

          return str;
        }
        return base.ToolTipText;
      }
      set {
        base.ToolTipText = value;
      }
    }
    #endregion

    #region public new Color Color
    [Category("Style"), DisplayName("Color")]
    public Color Color {
      get { return base.LineColor; }
      set { base.LineColor = value; }
    }
    #endregion

    #region public int Width
    [Category("Style"), DisplayName("Width")]
    public int Width {
      get { return Convert.ToInt32(base.LinePen.Width); }
      set { base.LinePen.Width = value; }
    }
    #endregion
  }
}
