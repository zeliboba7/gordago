/**
* @version $Id: FigureIndicator.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  
  using Gordago.Trader.Indicators;
  using Gordago.Trader.Builder;
  using System.Reflection;
  using System.ComponentModel;
  using Gordago.Trader.Common;

  public partial class FigureIndicator : Figure {

    private ClassBuilder _classBuilder;
    private readonly ParameterCollection _parameters = new ParameterCollection();

    private IndicatorView _currentIV;
    private readonly Dictionary<int, IndicatorView> _views = new Dictionary<int, IndicatorView>();

    private int _savedCountTick = -1;
    private int _savedPosition = -1;
    private int _savedWidth = -1;

    private int _limitCompute = -1;

    public FigureIndicator(Type indicatorType) {
      this.SetStyle(ChartFigureStyle.CalculateScale | ChartFigureStyle.UserMouse | ChartFigureStyle.Selectable, true);

      _classBuilder = new ClassBuilder(indicatorType);
      _parameters.AddRange(_classBuilder.Parameters);
    }

    #region public ParameterCollection Parameters
    public ParameterCollection Parameters {
      get { return _parameters; }
    }
    #endregion

    #region public int LimitCompute
    public int LimitCompute {
      get { return this._limitCompute; }
      set {
        if (this._limitCompute == value)
          return;
        this._limitCompute = value;
        _savedCountTick = 0;
        this.Invalidate();
      }
    }
    #endregion

    #region private iBars iBars
    private iBars iBars {
      get {
        return this.Owner.Owner.iBars;
      }
    }
    #endregion

    #region protected override void OnCalculateScale()
    protected override void OnCalculateScale() {
      IBarsData bars = this.Owner.Owner.Bars;

      if (_currentIV == null || 
          _currentIV.TimeFrame.Second != this.Owner.Owner.Bars.TimeFrame.Second) {

        _views.TryGetValue(bars.TimeFrame.Second, out _currentIV);
        if (_currentIV == null) {

          ParameterCollection inputs = new ParameterCollection();
          inputs.AddRange(_classBuilder.Parameters);

          Parameter iBarsParam = inputs["iBars"];

          if (iBarsParam == null) {
            iBarsParam = new Parameter(this.iBars, "iBars");
            inputs.Add(iBarsParam);
          }

          iBarsParam.Value = this.iBars;

          Indicator indicator = _classBuilder.CreateInstance(inputs.ToArray()) as Indicator;

          PropertyInfo[] properties = _classBuilder.ClassType.GetProperties();

          List<FunctionViewInfo> functions = new List<FunctionViewInfo>();
          foreach (PropertyInfo property in properties) {
            FunctionViewInfo fview = new FunctionViewInfo(property);
            if ((int)fview.Error == -1) {
              functions.Add(fview);
              fview.Join(indicator);
            }
          }
          _currentIV = new IndicatorView(bars.TimeFrame, indicator, functions.ToArray());
          _views.Add(bars.TimeFrame.Second, _currentIV);
        }
        _savedCountTick = -1;
      }

      int countTick = this.Owner.Owner.Symbol.Ticks.Count;
      int beginIndex = this.Owner.HorizontalScale.Position;

      if (_savedCountTick == countTick && _savedPosition == beginIndex && _savedWidth == this.Owner.Width)
        return;
      _savedCountTick = countTick;
      _savedPosition = beginIndex;
      _savedWidth = this.Owner.Width;

      int cntBarW = this.Owner.HorizontalScale.CountBarView;
      int endIndex = beginIndex + cntBarW;
      int countBar = bars.Count;
      this.iBars.Limit = this.LimitCompute;
      this.iBars.SetLimit(beginIndex, cntBarW);

      endIndex = Math.Min(endIndex, countBar);
      if (endIndex - beginIndex == 0)
        return;

      float min = float.MaxValue;
      float max = float.MinValue;
      FunctionViewInfo[] fvs = _currentIV.FunctionsView;
      float val = float.NaN;

        for ( int j = 0; j < fvs.Length; j++) {
          Function f = fvs[j].Function as Function;
          f.NativeCompute();
          for (int i = beginIndex; i < endIndex; i++) {
            val = f.NativeGetItem(i);
            if (!float.IsNaN(val)) {
              min = Math.Min(min, val);
              max = Math.Max(max, val);
            }
          }
      }
      this.Owner.VerticalScale.SetScaleValue(min, max, this.Owner.Owner.Symbol.Digits);
    }
    #endregion

    #region protected override void OnPaint(ChartGraphics g)
    protected override void OnPaint(ChartGraphics g) {
      FunctionViewInfo[] fvs = _currentIV.FunctionsView;
      for (int j = 0; j < fvs.Length; j++) {
        PaintFunction(g, fvs[j]);
      }
    }
    #endregion

    #region private int GetY(float val)
    private int GetY(float val) {
      return this.Owner.VerticalScale.GetY(val);
    }
    #endregion

    #region private void PaintFunction(ChartGraphics g, FunctionView function)
    private void PaintFunction(ChartGraphics g, FunctionViewInfo function) {
      int beginIndex = this.Owner.HorizontalScale.Position;
      int endIndex = beginIndex + this.Owner.HorizontalScale.CountBarView;
      int countBar = function.Function.Count;
      endIndex = Math.Min(endIndex, countBar);

      g.SelectPen(new GdiPen(function.Color, function.Width));
      
      bool first = true;
      int zeroY = 0;

      for (int i = beginIndex; i < endIndex; i++) {
        float value = function.Function.Items[i];
        if (float.IsNaN(value))
          continue;

        int x = this.Owner.HorizontalScale.GetX(i);
        int y = this.GetY(value);

        if (function.Style == FunctionStyle.Histogram) {
          if (first){
            first = false;
            zeroY = this.GetY(0);
          }
          g.DrawLine(x, zeroY, x, y);
        } else {
          if (first) {
            first = false;
            g.MoveTo(x, y);
          } else {
            g.LineTo(x, y);
          }
        }
      }
    }
    #endregion
  }
}
