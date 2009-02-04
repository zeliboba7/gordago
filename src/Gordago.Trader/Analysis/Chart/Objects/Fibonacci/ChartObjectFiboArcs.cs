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

namespace Gordago.Analysis.Chart {

  /// <summary>
  /// Дуги Фибоначчи
  /// </summary>
  public class ChartObjectFiboArcs:ChartObjectFibo {

    #region private readonly static float[] PERCENT_INIT
    private readonly static float[] PERCENT_INIT =
      new float[]{
        23.6F,
        38.2F,
        50F,
        61.8F
      };
    #endregion

    private bool _enableCustomAngle;
    private int _startAngle, _sweepAngle;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Наименование фигуры</param>
    public ChartObjectFiboArcs(string name): base(name) {

      /* Курсор в момент создание фигуры на графике */
      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COFiboArcs));
      
      /* Иконка на панели инструментов */
      this.Image = Gordago.Properties.Resources.m_co_fibo_arcs;

      this.TypeName = "Fibo Arcs";

      this.Levels = new FibonacciLevel[PERCENT_INIT.Length];
      for (int i = 0; i < this.Levels.Length; i++) {
        this.Levels[i] = new FibonacciLevel();
        this.Levels[i].Value = PERCENT_INIT[i];
      }
    }

    #region public bool EnableCustomAngle
    /// <summary>
    /// Возможность указывать другой угол дуги
    /// </summary>
    [Category("Arc"), DisplayName("Enable Custom Angle")]
    public bool EnableCustomAngle {
      get { return this._enableCustomAngle; }
      set { this._enableCustomAngle = value; }
    }
    #endregion

    #region public int StartAngle
    /// <summary>
    /// Начало дуги
    /// </summary>
    [Category("Arc"), DisplayName("Start Angle")]
    public int StartAngle {
      get { return this._startAngle; }
      set { this._startAngle = value; }
    }
    #endregion

    #region public int SweepAngle
    /// <summary>
    /// Конец дуги
    /// </summary>
    [Category("Arc"), DisplayName("Sweep Angle")]
    public int SweepAngle {
      get { return this._sweepAngle; }
      set { this._sweepAngle = value; }
    }
    #endregion

    /// <summary>
    /// Метод прорисовки дуг Фибоначчи
    /// </summary>
    /// <param name="g"></param>
    protected override void OnPaintObject(Graphics g) {
      base.OnPaintObject(g);

      COPoint copoint2 = this.GetCOPoint(1);
      if (copoint2 == null)
        return;

      int deltaX = this.ChartBox.GetX(1) - this.ChartBox.GetX(0);

      int x1 = this.COPoints[0].X;
      int y1 = this.COPoints[0].Y;

      int x2 = copoint2.X;
      int y2 = copoint2.Y;

      int dpx = Math.Abs(this.COPoints[0].BarIndex - copoint2.BarIndex);
      int dpy = Convert.ToInt32(Math.Abs((this.COPoints[0].Price - copoint2.Price) / this.ChartBox.ChartManager.Symbol.Point));


      for (int i = 0; i < this.Levels.Length; i++) {

        int r = (int)Math.Sqrt(Convert.ToDouble(dpx * dpx + dpy * dpy));

        r = Convert.ToInt32(r * this.Levels[i].Value / 100F);

        int w = r * deltaX * 2;

        float dprice = copoint2.Price + (float)r * this.ChartBox.ChartManager.Symbol.Point;
        int ty = this.ChartBox.GetY(dprice);
        int h = Math.Abs(y2 - ty) * 2;

        int startAngle = 0, sweepAngle = -180;
        int dy = 0;
        if (_enableCustomAngle) {
          startAngle = _startAngle;
          sweepAngle = _sweepAngle;
        }else  if (y1 >= y2) {
          startAngle = 0;
          sweepAngle = 180;
          dy = h;
        }

        int py = y2 - h / 2;
        int px = x2 - w / 2;

        if (w > 0 && h > 0) {
          g.DrawArc(this.FiboPen, px, py, w, h, startAngle, sweepAngle);

          if (this.Levels[i].Description != string.Empty) {
            Size size = this.GetStringSize(g, this.Levels[i].Description);

            int sx = x2 - size.Width / 2;
            int sy = py - size.Height + dy;

            g.DrawString(this.Levels[i].Description, this.Font, this.ForeBrush, sx, sy);
          }
        }
      }
    }

    #region protected override void OnSaveTemplate(XmlNodeManager nodeManager)
    /// <summary>
    /// Сохранение настроек фигуры 
    /// </summary>
    /// <param name="nodeManager"></param>
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);
      nodeManager.SetAttribute("EnableCustomAngle", this.EnableCustomAngle);
      nodeManager.SetAttribute("StartAngle", this.StartAngle);
      nodeManager.SetAttribute("SweepAngle", this.SweepAngle);
    }
    #endregion

    #region protected override void OnLoadTemplate(XmlNodeManager nodeManager)
    /// <summary>
    /// Загрузка настроек фигуры
    /// </summary>
    /// <param name="nodeManager"></param>
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);
      this.EnableCustomAngle = nodeManager.GetAttributeBoolean("EnableCustomAngle", false);
      this.StartAngle = nodeManager.GetAttributeInt32("StartAngle", 0);
      this.SweepAngle = nodeManager.GetAttributeInt32("SweepAngle", -180);
    }
    #endregion

  }
}
