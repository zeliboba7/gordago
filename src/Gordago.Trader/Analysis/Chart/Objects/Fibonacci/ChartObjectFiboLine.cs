/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace Gordago.Analysis.Chart {
  public class ChartObjectFiboRetracement:ChartObjectFibo {

    #region private readonly static float[] PERCENT_INIT
    private readonly static float[] PERCENT_INIT =
      new float[]{
        0.0F,
        23.6F,
        38.2F,
        50F,
        61.8F,
        100F,
        161.8F
      };
    #endregion

    private StringFormat _sformat;

    public ChartObjectFiboRetracement(string name) : base(name) {
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Far;
      _sformat.LineAlignment = StringAlignment.Far;

      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COFibo));
      this.Image = Gordago.Properties.Resources.m_co_fibo;
      this.TypeName = "Fibo";
      this.Levels = new FibonacciLevel[PERCENT_INIT.Length];
      
      for (int i = 0; i < this.Levels.Length; i++) {
        this.Levels[i] = new FibonacciLevel();
        this.Levels[i].Value = PERCENT_INIT[i];
      }
    }

    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {
      base.OnPaintObject(g);

      COPoint copoint2 = this.GetCOPoint(1);
      if(copoint2 == null) return;

      int x1 = this.COPoints[0].X;
      int y1 = this.COPoints[0].Y;

      int x2 = copoint2.X;
      int y2 = copoint2.Y;

      int xMax = Math.Max(x1, x2);
      int dxAbs = Math.Abs(x2 - x1);

      int xx1 = Math.Min(x1, x2);
      int xx2 = Math.Max(xMax + dxAbs * 3, xMax + 35);

      int xs = Math.Min(this.ChartBox.Width, x2);

      for(int i = 0; i < this.Levels.Length; i++) {
        float dy = ((y2 - y1) * this.Levels[i].Value) / 100F;
        int y = y2 - (int)dy;

        g.DrawLine(this.FiboPen, xx1, y, xx2, y);

        if(xx1 < this.ChartBox.Width && xx2 > 0) {
          if (this.Levels[i].Description != string.Empty) {

            SizeF sizef = g.MeasureString(this.Levels[i].Description + "W", this.Font, new PointF(xs, y), _sformat);
            int w = Convert.ToInt32(sizef.Width);
            int h = Convert.ToInt32(sizef.Height);

            int px = xs - w / 2;
            int py = y - h / 2;

            RectangleF rectf = new RectangleF(px, py, w, h);
            g.DrawString(this.Levels[i].Description, this.Font, this.ForeBrush, rectf, _sformat);
          }
        }
      }
    }
    #endregion
  }
}
