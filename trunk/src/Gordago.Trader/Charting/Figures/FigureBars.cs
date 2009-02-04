/**
* @version $Id: FigureBars.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting.Figures
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  
  using System.ComponentModel;
  using Gordago.Trader.Indicators;
  using Gordago.Trader.Common;

  public class FigureBars : Figure {

    private FigureBarsStyle _barsStyle = FigureBarsStyle.Candle;

    private Color _color = Color.FromArgb(-14013910);
    private Color _colorUp = Color.FromArgb(-1076);
    private Color _colorDown = Color.FromArgb(-8323200);

    private int _savedCountTick = -1;
    private int _savedPosition = -1;
    private int _savedWidth = -1;

    public FigureBars() {
      this.SetStyle(ChartFigureStyle.UserMouse, true);
      this.SetStyle(ChartFigureStyle.CalculateScale, true);
    }

    #region public Color Color
    [Parameter("Color")]
    [Category("Style")]
    public Color Color {
      get { return this._color; }
      set { this._color = value; }
    }
    #endregion

    #region public Color ColorUp
    [Parameter("ColorUp"), Category("Style"), DisplayName("Color Up")]
    public Color ColorUp {
      get { return this._colorUp; }
      set { this._colorUp = value; }
    }
    #endregion

    #region public Color ColorDown
    [Parameter("ColorDown"), Category("Style"), DisplayName("Color Down")]
    public Color ColorDown {
      get { return this._colorDown; }
      set { this._colorDown = value; }
    }
    #endregion

    #region public FigureBarsStyle BarsStyle
    public FigureBarsStyle BarsStyle {
      get { return this._barsStyle; }
      set {
        bool evt = this._barsStyle != value;
        _barsStyle = value;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region protected override void OnCalculateScale()
    protected override void OnCalculateScale() {
      int countTick = this.Owner.Owner.Symbol.Ticks.Count;
      int beginIndex = this.Owner.HorizontalScale.Position;

      if (_savedCountTick == countTick && _savedPosition == beginIndex && _savedWidth == this.Owner.Width)
        return;
      _savedCountTick = countTick;
      _savedPosition = beginIndex;
      _savedWidth = this.Owner.Width;

      int endIndex = beginIndex + this.Owner.HorizontalScale.CountBarView;
      IBarsData bars = this.Owner.Owner.Bars;
      int countBar = bars.Count;
      

      endIndex = Math.Min(endIndex, countBar);
      if (endIndex - beginIndex == 0)
        return;

      iBars ibars = this.Owner.Owner.iBars;

      float min = float.MaxValue;
      float max = float.MinValue;
      for (int i = beginIndex; i < endIndex; i++) {
        Bar bar = ibars.GetBar(i);
        min = Math.Min(bar.Low, min);
        max = Math.Max(bar.High, max);
      }
      this.Owner.VerticalScale.SetScaleValue(min, max, this.Owner.Owner.Symbol.Digits);
    }
    #endregion

    #region private int GetY(float val)
    private int GetY(float val) {
      return this.Owner.VerticalScale.GetY(val);
    }
    #endregion

    protected override void OnPaint(ChartGraphics g) {
      int beginIndex = this.Owner.HorizontalScale.Position;
      int endIndex = beginIndex + this.Owner.HorizontalScale.CountBarView;
      IBarsData bars = this.Owner.Owner.Bars;
      int countBar = bars.Count;
      endIndex = Math.Min(endIndex, countBar);
      g.SelectPen(this.Color);
      GdiBrush brushUp = g.SelectBrush(ColorUp);
      GdiBrush brushDown = g.SelectBrush(ColorDown);

      int barWidth = 0;
      switch (this.Owner.HorizontalScale.Zoom) {
        case ChartBox.ChartHorizontalScale.HorizontalZoom.Smaller:
        case ChartBox.ChartHorizontalScale.HorizontalZoom.Small:
          barWidth = 0;
          break;
        case ChartBox.ChartHorizontalScale.HorizontalZoom.Medium:
          barWidth = 2;
          break;
        case ChartBox.ChartHorizontalScale.HorizontalZoom.Larger:
          barWidth = 4;
          break;
        case ChartBox.ChartHorizontalScale.HorizontalZoom.Large:
          barWidth = 10;
          break;
        case ChartBox.ChartHorizontalScale.HorizontalZoom.BigLarge:
          barWidth = 24;
          break;
      }

      iBars ibars = this.Owner.Owner.iBars;

      for (int i = beginIndex; i < endIndex; i++) {

        Bar bar = ibars.GetBar(i);

        int x = this.Owner.HorizontalScale.GetX(i);
        int yLow = this.GetY(bar.Low);
        int yHigh = this.GetY(bar.High);

        if (_barsStyle != FigureBarsStyle.Line) {
          g.DrawLine(x, yLow, x, yHigh);

          if (barWidth == 0)
            continue;

          int yClose = this.GetY(bar.Close);
          int yOpen = this.GetY(bar.Open);

          int barWD = barWidth / 2;

          int x1 = x - barWD, h = 0, y = 0;

          if (bar.Close > bar.Open) {
            h = yOpen - yClose;
            y = yClose;
            g.FillRectangleExt(brushUp, x1, y, barWidth, h);
          } else if (bar.Close < bar.Open) {
            h = yClose - yOpen;
            y = yOpen;
            g.FillRectangleExt(brushDown, x1, y, barWidth, h);
          }

          if (h > 0) {
            g.DrawRectangle(x1, y, barWidth, h);
          }
        } else {

        }
      }
    }
  }
}
