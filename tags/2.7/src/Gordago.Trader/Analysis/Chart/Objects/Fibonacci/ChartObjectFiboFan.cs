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

namespace Gordago.Analysis.Chart {

  /// <summary>
  /// Fibonacci Fan
  /// </summary>
  public class ChartObjectFiboFan:ChartObjectFibo {

    #region private readonly static float[] PERCENT_INIT
    private readonly static float[] PERCENT_INIT =
      new float[]{
        23.6F,
        38.2F,
        50F,
        61.8F
      };
    #endregion

    private StringFormat _sformat;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Наименование фигуры</param>
    public ChartObjectFiboFan(string name): base(name) {
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;

      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COFiboFan));
      this.Image = Gordago.Properties.Resources.m_co_fibo_fan;
      this.TypeName = "Fibo Fan";

      this.Levels = new FibonacciLevel[PERCENT_INIT.Length];
      for (int i = 0; i < PERCENT_INIT.Length; i++) {
        this.Levels[i] = new FibonacciLevel();
        this.Levels[i].Value = PERCENT_INIT[i];
      }
    }

    /// <summary>
    /// Прорисовка фигуры
    /// </summary>
    /// <param name="g"></param>
    protected override void OnPaintObject(Graphics g) {
      base.OnPaintObject(g);

      COPoint copoint2 = this.GetCOPoint(1);
      if(copoint2 == null) return;

      int x1 = this.COPoints[0].X;
      int y1 = this.COPoints[0].Y;

      int x2 = copoint2.X;
      int y2 = copoint2.Y;
      
      for(int i = 0; i < this.Levels.Length; i++) {
        int m = 10;
        int xx2 = x2 + (x2 - x1) * m;

        float dy = ((y2 - y1) * this.Levels[i].Value) / 100F;
        int ylevel = y2 - (int)dy;
        int yy2 = ylevel + (ylevel - y1) * m;

        g.DrawLine(this.FiboPen, x1, y1, xx2, yy2);

        if (this.Levels[i].Description != string.Empty) {

          Size size = this.GetStringSize(g, this.Levels[i].Description + "W");
          int w = Convert.ToInt32(size.Width);
          int h = Convert.ToInt32(size.Height);

          int px = x2 - w / 2;
          int py = ylevel - h / 2;

          RectangleF rectf = new RectangleF(px, py, w, h);
          g.DrawString(this.Levels[i].Description, this.Font, this.ForeBrush, rectf, _sformat);
        }
      }
    }
  }
}
