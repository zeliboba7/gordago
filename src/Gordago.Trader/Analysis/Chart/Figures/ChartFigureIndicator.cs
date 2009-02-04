/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion

namespace Gordago.Analysis.Chart {
	public class ChartFigureIndicator: ChartFigure {

		private Indicator _indicator;
		private bool _recalcul;
		private object[] _fparams;
		private IVector[] _results;
		private Parameter[] _inparams;

		private int _cntfuncs;
		private int[] _cntpoints, _curindex;
		private object[] _points;
		private GraphicsPath[] _gpaths;
		private int _nummouseenterline = -1;
		private Pen _checkfigurepen = new Pen(Color.Transparent, 3);
		private DrawingIndicator _drawingIndicator;
		private int _analyzerOffset;
		private int _savedCountBar;
    private int _savedCountTick;

    private FunctionStyle[] _fstyles;

    private int _selectMouseBarIndex = -1;
    private Point _mousePosition;

		public ChartFigureIndicator(string name, Indicator indicator, Parameter[] parameters):base(name) {
      this.EnableScale = true;
      this.PropertyEnable = true;
			this._indicator = indicator;
			if (indicator.CustomDrawingIndicatorType != null){
				_drawingIndicator = Activator.CreateInstance(indicator.CustomDrawingIndicatorType) as DrawingIndicator;
			}
			_cntfuncs = this._indicator.Functions.Length;
			_inparams = parameters;
			_recalcul = true;
			_cntpoints = new int[_cntfuncs];
			_curindex = new int[_cntfuncs];
			_points = new object[_cntfuncs];
			_gpaths = new GraphicsPath[_cntfuncs];
			for (int i=0;i<this._gpaths.Length;i++)
				_gpaths[i] = new GraphicsPath();

      _fstyles = new FunctionStyle[indicator.FunctionStyles.Length];
      for (int i = 0; i < _fstyles.Length; i++) {
        _fstyles[i] = (FunctionStyle)indicator.FunctionStyles[i].Clone();
      }
    }

    #region protected internal override int DecimalDigits
    protected internal override int DecimalDigits {
      get {
        return _indicator.DecimalDigits;
      }
      set {
        base.DecimalDigits = value;
      }
    }
    #endregion

    #region public override string ToolTipText
    public override string ToolTipText {
      get {
        if(this._selectMouseBarIndex < 0) return "";

        IBarList bars = this.ChartBox.ChartManager.Bars;

        Bar bar = bars[this._selectMouseBarIndex];
        DateTime dtm = bar.Time;

        List<string> scrtt = new List<string>();

        for(int i = 0; i < Parameters.Length; i++)
          scrtt.Add(this.Parameters[i].ToString());

        string sprms = string.Join(";", scrtt.ToArray());

        Function function = this._indicator.Functions[this.SelectedNumberLine];
        string sname = function.ShortName;
        string sshift = "";
        if(this.Shift != 0) {
          sshift = "[" + this.Shift.ToString()+"]" ;
        }

        string tt = sname + "(" + sprms+")"+sshift+ "\n" +
          "Value: " + this.Results[this.SelectedNumberLine][this._selectMouseBarIndex - this._analyzerOffset].ToString() + "\n" +
          dtm.ToShortDateString() + " " + dtm.ToShortTimeString();

        return tt;
      }
      set {
        base.ToolTipText = value;
      }
    }
    #endregion

    #region public Vector[] Results
    public IVector[] Results{
			get{return this._results;}
		}
		#endregion

		#region public int SelectedNumberLine
		public int SelectedNumberLine{
			get{return this._nummouseenterline;}
		}
		#endregion

		#region public Indicator Indicator
		public Indicator Indicator{
			get{return this._indicator;}
		}
		#endregion

    #region public FunctionStyle[] FunctionStyles
    public FunctionStyle[] FunctionStyles {
      get { return this._fstyles; }
    }
    #endregion

    #region public Parameter[] Parameters
    public Parameter[] Parameters{
			get{return this._inparams;}
		}
		#endregion

		#region public void SetParameters(Parameter[] parameters)
		public void SetParameters(Parameter[] parameters){
			_recalcul = true;
			_results = new Vector[this._indicator.Functions.Length];
			_fparams = new object[this._indicator.Functions.Length];

			for(int i=0; i<_indicator.Functions.Length;i++){
        Parameter[] prms = _indicator.Functions[i].GetParameters(parameters);
				_fparams[i] = prms;

				this.SetTimeFrameMethod(prms);

        for(int k = prms.Length-1; k >=0 ; k--) {
          if(prms[k] is ParameterColor) {
            Color color = (prms[k] as ParameterColor).Value;
            this.FunctionStyles[i].Pen.Color = color;
            this.FunctionStyles[i].SelectPen.Color = color;
            break;
          }
        }
			}
		}
		#endregion

		#region private void SetTimeFrame()
		private void SetTimeFrame(){
			for(int i=0;i<_fparams.Length;i++){
				this.SetTimeFrameMethod((Parameter[])_fparams[i]);
			}
		}
		#endregion

		#region private void SetTimeFrameMethod(Parameter[] prms)
    private void SetTimeFrameMethod(Parameter[] prms) {
      for (int i = 0; i < prms.Length; i++) {
        Parameter prm = prms[i];
        if (prm.Name.ToLower() == Gordago.Analysis.Function.PNameTimeFrame.ToLower())
          prm.Value = this.ChartBox.ChartManager.Bars.TimeFrame.Second;
        else if (prm is ParameterVector)
          this.SetTimeFrameMethod((prm as ParameterVector).Parameters);
      }
    }
		#endregion

    #region internal override void SetChartBox(ChartBox chartBox)
    internal override void SetChartBox(ChartBox chartBox) {
      base.SetChartBox(chartBox);
      if(_drawingIndicator != null)
        _drawingIndicator.SetChartBox(chartBox, this);
    }
    #endregion

    #region public void Compute()
    public void Compute() {
      IBarList bars = this.ChartBox.ChartManager.Bars;

      if(bars == null || bars.Count == 0) return;
      this.SetParameters(_inparams);
      _recalcul = false;
      //ChartAnalyzer analyzer = this.ChartBox.ChartManager.ChartAnalyzer;

      for(int i = 0; i < this._indicator.Functions.Length; i++) {
        Parameter[] prms = (Parameter[])this._fparams[i];
        _results[i] = this.GetComputeParams(_indicator.Functions[i], prms);
        //System.Diagnostics.Debug.WriteLine(_results[i].Current);
      }
      if(_drawingIndicator != null) {
        _drawingIndicator.SetVectors(_results);
      }
      this.ReCalculateScale();
    }
    #endregion

    #region private IVector GetComputeParams(Function function, Parameter[] prms)
    private IVector GetComputeParams(Function function, Parameter[] prms) {

      ChartAnalyzer analyzer = this.ChartBox.ChartManager.ChartAnalyzer;
      List<object> newPrms = new List<object>();

      for (int k = 0; k < prms.Length; k++) {
        object p = prms[k];
        if (p is ParameterVector) {
          ParameterVector pvector = p as ParameterVector;
          Function tmpFunc = analyzer.IndicatorManager.GetFunction(pvector.Value);
          newPrms.Add(this.GetComputeParams(tmpFunc, pvector.Parameters));
        } else if (p is Parameter) {
          if (p is ParameterColor) {
          } else {
            newPrms.Add(prms[k].Value);
          }
        } else
          newPrms.Add(p);
      }
      IVector vector = analyzer.Compute(function, newPrms.ToArray());
      return vector;
    }
    #endregion

    #region protected internal override void OnCalculateScale()
    protected internal override void OnCalculateScale() {

      if(_analyzerOffset != this.ChartBox.ChartManager.ChartAnalyzer.Offset) {
        _analyzerOffset = this.ChartBox.ChartManager.ChartAnalyzer.Offset;
        _recalcul = true;
      }

      IBarList bars = this.ChartBox.ChartManager.Bars;
      if (this.ChartBox.ChartManager.Symbol.Ticks.Count != _savedCountTick) {
        _recalcul = true;
        _savedCountTick = this.ChartBox.ChartManager.Symbol.Ticks.Count;
      }

      int curcountbar = bars.Count;
      if(curcountbar != _savedCountBar) {
        _savedCountBar = curcountbar;
        _recalcul = true;
      }

      if(_recalcul) this.Compute();

      for(int i = 0; i < _cntpoints.Length; i++)
        _cntpoints[i] = 0;

      for(int i = 0; i < this._cntfuncs; i++) {
        if(this.FunctionStyles[i].DrawStyle == FunctionDrawStyle.Histogram)
          this.SetScaleValue(0);
      }

      for(int index = 0; index < Map.Length; index++) {
        int barindex = index + this.ChartBox.ChartManager.Position - this.Shift;
        if(barindex >= 0 && barindex < bars.Count && this.ChartBox.ChartManager.ChartAnalyzer != null) {
          for(int i = 0; i < this._results.Length; i++) {
            float val = _results[i][barindex - this.ChartBox.ChartManager.ChartAnalyzer.Offset];
            if(!float.IsNaN(val)) {
              this.SetScaleValue(val);
              _cntpoints[i]++;
            }
          }
        }
      }


      if (_indicator.EnableCustomScale && _indicator.CustomScaleValues.Length >0) {
        float min = float.MaxValue;
        float max = float.MinValue;

        for (int i = 0; i < _indicator.CustomScaleValues.Length; i++) {
          min = Math.Min(min, _indicator.CustomScaleValues[i]);
          max = Math.Max(max, _indicator.CustomScaleValues[i]);
        }
        this.SetScaleValue(min, max);
      }
    }
    #endregion

    #region public float GetValue(int indexFunction, int index)
    /// <summary>
    /// Получить значение индикатора
    /// </summary>
    /// <param name="numFunction"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetValue(int indexFunction, int index) {
      int barIndex = index + this.ChartBox.ChartManager.Position - this.Shift;
      if (barIndex >= 0 && barIndex < this.ChartBox.ChartManager.Bars.Count && this.ChartBox.ChartManager.ChartAnalyzer != null) {
        float val = _results[indexFunction][barIndex - this.ChartBox.ChartManager.ChartAnalyzer.Offset];
        return val;
      }
      return float.NaN;
    }
    #endregion

    #region protected internal override void OnPaint(Graphics g)
    protected internal override void OnPaint(Graphics g) {
      for (int i = 0; i < _cntfuncs; i++) {
        _points[i] = new Point[_cntpoints[i]];
        _curindex[i] = 0;
      }

      #region if (_drawingind != null) { ... }
      if (_drawingIndicator != null) {

        if (this.ChartBox.ChartManager.SelectedFigure == this) {
          _drawingIndicator.SetStatus(DrawingIndicatorStatus.MouseSelected);
        } else if (this.ChartBox.ChartManager.SelectedFigureMouse == this) {
          _drawingIndicator.SetStatus(DrawingIndicatorStatus.UserSelected);
        } else {
          _drawingIndicator.SetStatus(DrawingIndicatorStatus.Default);
        }
        _drawingIndicator.SetBarWidth(ChartFigureBar.BAR_WIDTH[(int)this.ChartBox.ChartManager.Zoom]);
        _drawingIndicator.PaintElementInit(g);
      }
      #endregion

      IBarList bars = this.ChartBox.ChartManager.Bars;

      for (int index = 0; index < Map.Length; index++) {
        int barindex = index + this.ChartBox.ChartManager.Position - this.Shift;
        if (barindex >= 0 && barindex < bars.Count && this.ChartBox.ChartManager.ChartAnalyzer != null) {

          for (int i = 0; i < this._results.Length; i++) {
            FunctionStyle fs = this.FunctionStyles[i];
            IVector vector = _results[i];
            int vectorIndex = barindex - this.ChartBox.ChartManager.ChartAnalyzer.Offset;

            if (vectorIndex < vector.Count) {
              float val = vector[vectorIndex];
              if (!float.IsNaN(val)) {
                Point[] points = ((Point[])_points[i]);
                int findex = _curindex[i]++;
                if (findex < points.Length) {
                  Point p1 = points[findex] = new Point(Map[index], ChartBox.GetY(val));
                  if (_drawingIndicator != null) {
                    _drawingIndicator.PaintElement(g, i, barindex - this.ChartBox.ChartManager.ChartAnalyzer.Offset, this.Map[index]);
                  } else {
                    if (this.ChartBox.ChartManager.SelectedFigure == this) {
                      int w = 2;
                      g.DrawLine(fs.Pen, p1.X - w, p1.Y - w, p1.X + w, p1.Y + w);
                      g.DrawLine(fs.Pen, p1.X - w, p1.Y + w, p1.X + w, p1.Y - w);
                    }
                    if (fs.DrawStyle == FunctionDrawStyle.Histogram) {

                      Pen pen = this.FunctionStyles[i].Pen;
                      if (this.ChartBox.ChartManager.SelectedFigureMouse == this && this._nummouseenterline == i) {
                        pen = this.FunctionStyles[i].SelectPen;
                      }
                      Point p2 = new Point(Map[index], ChartBox.GetY(0));
                      g.DrawLine(pen, p1, p2);
                    }
                  }
                }
              }
            }
          }
        }
      }

      if (this._results == null)
        return;
      if (_drawingIndicator != null) {
        _drawingIndicator.PaintElementFinal(g);
      }
      for (int i = 0; i < this._results.Length; i++) {
        if (_drawingIndicator == null) {

          if (((Point[])_points[i]).Length > 1) {
            FunctionStyle fs = FunctionStyles[i];
            Pen pen = this.FunctionStyles[i].Pen;
            if (this.ChartBox.ChartManager.SelectedFigureMouse == this && this._nummouseenterline == i) {
              pen = this.FunctionStyles[i].SelectPen;
            }
            switch (fs.DrawStyle) {
              case FunctionDrawStyle.Line:
                g.DrawLines(pen, ((Point[])_points[i]));
                break;
              case FunctionDrawStyle.Histogram:
                break;
            }
          }
        }

        this._gpaths[i].Reset();
        Point[] pts = ((Point[])_points[i]);
        if (pts.Length > 0)
          this._gpaths[i].AddLines(pts);
      }
    }
    #endregion

    #region public override bool CheckFigure(Point p)
    public override bool CheckFigure(Point p) {

      _selectMouseBarIndex = -1;

			bool fret = false;
			for (int i=0;i<this._gpaths.Length;i++){
				if (_gpaths[i] == null) return false;
				fret = _gpaths[i].IsOutlineVisible(p, _checkfigurepen);
				if (fret){
					_nummouseenterline = i;
					return fret;
				}
			}
			_nummouseenterline = -1;
			return false;
		}
		#endregion

    protected internal override void OnMouseMove(MouseEventArgs e) {
      Point p = e.Location;
      int deltax = 1;
      if (this.Map.Length >= 1) {
        deltax = this.Map[0];
      }

      int num = (int)Math.Round((double)p.X / (double)deltax);

      this._selectMouseBarIndex = num + this.ChartBox.ChartManager.Position - this.Shift;

      this._mousePosition = p;
    }

    #region public Parameter GetParameter(string name)
    public Parameter GetParameter(string name) {
      for(int i = 0; i < _inparams.Length; i++) {
        if(name.ToLower() == _inparams[i].Name.ToLower())
          return _inparams[i];
      }
      return null;
    }
    #endregion
  }
}
