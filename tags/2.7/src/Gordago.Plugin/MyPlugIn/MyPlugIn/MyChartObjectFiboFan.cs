using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Gordago.Analysis.Chart;
using System.ComponentModel;

namespace MyPlugIn {
  class MyChartObjectFiboFan:ChartObjectFibo {

    #region private static float[] FAN_PERCENT
    private static float[] FAN_PERCENT = new float[]{
        0.0F,
        38.2F,
        50F,
        61.8F,
        76.4F,
        85,4F
      };
    #endregion

    private static int FAN_ALPHA = 50;

    #region private static Color[] FAN_COLORS
    private static Color[] FAN_COLORS = new Color[]{
        Color.FromArgb(FAN_ALPHA, Color.Crimson), 
        Color.FromArgb(FAN_ALPHA, Color.Brown), 
        Color.FromArgb(FAN_ALPHA, Color.Chocolate), 
        Color.FromArgb(FAN_ALPHA, Color.Blue),
        Color.FromArgb(FAN_ALPHA, Color.Gray),
        Color.FromArgb(FAN_ALPHA, Color.Silver), 
        Color.FromArgb(FAN_ALPHA, Color.Honeydew)
      };
    #endregion

    private int _fanAlpha = 50;
    private Brush[] _fanBrushs;

    public MyChartObjectFiboFan(string name) : base(name) {
      _fanBrushs = new SolidBrush[0];
      this.Levels = new FibonacciLevel[FAN_PERCENT.Length];
      for (int i = 0; i < FAN_PERCENT.Length; i++) {
        this.Levels[i] = new FibonacciLevel();
        this.Levels[i].Value = FAN_PERCENT[i];
      }
    }

    #region public int FanAlpha
    [Category("Style"), DisplayName("FanAlpha")]
    public int FanAlpha {
      get { return this._fanAlpha; }
      set { this._fanAlpha = value; }
    }
    #endregion

    #region private Brush[] FanBrushs
    private Brush[] FanBrushs {
      get {
        if (this._fanBrushs.Length != this.Levels.Length) {
          _fanBrushs = new SolidBrush[this.Levels.Length];
          int k = 0;
          for (int i = 0; i < _fanBrushs.Length; i++) {
            if (k >= FAN_COLORS.Length)
              k = 0;
            _fanBrushs[i] = new SolidBrush(FAN_COLORS[k]);
            k++;
          }
        }
        return this._fanBrushs; 
      }
    }
    #endregion

    protected override void OnPaintObject(Graphics g) {
      base.OnPaintObject(g);
      COPoint copoint2 = this.GetCOPoint(1);
      
      if (copoint2 == null)
        return;

      int x1 = this.COPoints[0].X;
      int y1 = this.COPoints[0].Y;

      int x2 = copoint2.X;
      int y2 = copoint2.Y;

      int m = 10;
      Point prevPoint = new Point();
      for (int i = 0; i < this.Levels.Length; i++) {

        int xx2 = x2 + (x2 - x1) * m;
        float dy = ((y2 - y1) * this.Levels[i].Value) / 100F;
        int ylevel = y2 - (int)dy;
        int yy2 = ylevel + (ylevel - y1) * m;

        g.DrawLine(this.LinePen, x1, y1, xx2, yy2);

        if (i > 0) {
          Point[] ps = new Point[]{
            new Point(x1, y1),
            new Point(x2, ylevel),
            prevPoint
          };
          g.FillPolygon(this.FanBrushs[i - 1], ps);
        }
        prevPoint = new Point(x2, ylevel);
      }
    }
  }
}
