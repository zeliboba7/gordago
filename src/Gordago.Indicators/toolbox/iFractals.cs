using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iFractals : Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "Fractals";
			this.ShortName = "Fractals";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;
			this.SetCustomDrawingIndicator(typeof (iFractalsCustomDrawing));
			
			RegFunction("FractalUp");
			RegFunction("FractalDown");
		}
  }

  #region public class FractalUp: Function
  [Function("FractalUp")]
	public class FractalUp: Function{
		
		protected override void Initialize() {
      ParameterVector pvhigh = new ParameterVector("High", "High");
      pvhigh.Visible = false;
      RegParameter(pvhigh);
      RegParameter(new ParameterInteger("Period", new string[] { "Period", "Период" }, 5, 5, 100, 2));
      RegParameter(new ParameterColor("ColorUp", Color.Red));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector high = (IVector)parameters[0];
      int period = (int)parameters[1];

      if (period / 2 * 2 == period) {
        period++;
      }

      int ci = period / 2;

      if(result == null)
        result = new Vector();

      if(result.Count == high.Count)
        result.RemoveLastValue();

      for(int i = result.Count; i < high.Count; i++) {
        if(i < period-1)
          result.Add(float.NaN);
        else {
          if (period == 5) {
            if (high[i - 4] <= high[i - 3] &&
              high[i - 3] <= high[i - 2] &&
              high[i - 2] >= high[i - 1] &&
              high[i - 1] >= high[i]) {
              result.Add(high[i - 2]);
            } else
              result.Add(float.NaN);
          } else {
            bool visible = true;

            for (int j = i - ci * 2 + 1; j <= i; j++) {
              if (j <= i - ci) 
                visible = high[j - 1] <= high[j];
              else 
                visible = high[j-1] >= high[j];
              if (!visible)
                break;
            }
            if (visible)
              result.Add(high[i - ci]);
            else
              result.Add(float.NaN);
          }
        }
      }
      return result;
    }
  }
  #endregion

  #region public class FractalDown:Function
  [Function("FractalDown")]
  public class FractalDown:Function {

    protected override void Initialize() {
      ParameterVector pvlow = new ParameterVector("Low", "Low");
      pvlow.Visible = false;
      RegParameter(pvlow);
      RegParameter(new ParameterInteger("Period", new string[] { "Period", "Период" }, 5, 5, 100, 2));
      RegParameter(new ParameterColor("ColorDown", Color.Blue));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      IVector low = (IVector)parameters[0];
      int period = (int)parameters[1];
      if (period / 2 * 2 == period) {
        period++;
      }
      int ci = period / 2;
      

      if(result == null)
        result = new Vector();

      if(result.Count == low.Count)
        result.RemoveLastValue();

      for(int i = result.Count; i < low.Count; i++) {
        if(i < period-1)
          result.Add(float.NaN);
        else {
          if (period == 5) {
            if (low[i - 4] >= low[i - 3] &&
              low[i - 3] >= low[i - 2] &&
              low[i - 2] <= low[i - 1] &&
              low[i - 1] <= low[i]) {
              result.Add(low[i - 2]);
            } else
              result.Add(float.NaN);
          } else {
            bool visible = true;

            for (int j = i - ci * 2 + 1; j <= i; j++) {
              if (j <= i - ci)
                visible = low[j - 1] >= low[j];
              else
                visible = low[j - 1] <= low[j];
              if (!visible)
                break;
            }
            if (visible)
              result.Add(low[i - ci]);
            else
              result.Add(float.NaN);
          }
        }
      }
      return result;
    }
  }
  #endregion

  public class iFractalsCustomDrawing:DrawingIndicator {
    private Color _colorUp, _colorDown;
    private Pen _penup, _pendown;
    private Brush _brushup, _brushdown;
    private Pen _selpenup, _selpendown;
    private int _period = 5;

    public iFractalsCustomDrawing() {
      this.ColorUp = Color.Red;
      this.ColorDown = Color.Blue;
    }

    #region public Color ColorUp
    public Color ColorUp {
      get { return this._colorUp; }
      set {
        if(_colorUp == value) return;
        this._colorUp = value;
        _penup = new Pen(value, 1);
        _selpenup = new Pen(value, 2);
        _brushup = new SolidBrush(Color.FromArgb(50, value));
      }
    }
    #endregion

    #region public Color ColorDown
    public Color ColorDown {
      get { return this._colorDown; }
      set {
        if(_colorDown == value)
          return;
        this._colorDown = value;
        _pendown = new Pen(value, 1);
        _selpendown = new Pen(value, 2);
        _brushdown = new SolidBrush(Color.FromArgb(50, value));
      }
    }
    #endregion

    #region protected override void PaintElementInit(Graphics g)
    protected override void PaintElementInit(Graphics g) {
      base.PaintElementInit(g);
      _period = (this.Figure.GetParameter("Period") as ParameterInteger).Value;

      if (_period / 2 * 2 == _period) {
        _period++;
      }


      Parameter prm = this.Figure.GetParameter("ColorUp") as ParameterColor;

      if(prm != null) {
        this.ColorUp = (prm as ParameterColor).Value;
      }

      prm = this.Figure.GetParameter("ColorDown") as ParameterColor;
      if(prm != null) {
        this.ColorDown = (prm as ParameterColor).Value;
      }
    }
    #endregion

    protected override void PaintElement(Graphics g, int funcIndex, int vectorIndex, int x) {

      IVector vector = this.Vectors[funcIndex];

      int index = vectorIndex + _period / 2 + 1;
      
      if(index >= vector.Count)
        return;

      float val = vector[vectorIndex];

      x = this.ChartBox.GetXFromAnalyzer(vectorIndex - _period / 2);

      if(funcIndex == 0) {

        if(val > 0) {
          int w = Math.Max(this.BarWidth, 6) + 4;
          int h = Math.Max(w / 3, 2) + 2;
          int y = this.ChartBox.GetY(val) - 7;
          Point p = new Point(x, y);
          Point[] pts = new Point[]{
							p,
							p = this.PointOffset(p, -w/2, h),
							p = this.PointOffset(p, w, 0)
					};
          g.FillPolygon(_brushup, pts);
          g.DrawPolygon(_penup, pts);
        }
      } else {
        if(val > 0) {
          int w = Math.Max(this.BarWidth, 6) + 4;
          int h = Math.Max(w / 3, 2) + 2;
          int y = this.ChartBox.GetY(val) + 7;
          Point p = new Point(x, y);
          Point[] pts = new Point[]{
							p,
							p = this.PointOffset(p, -w/2, -h),
							p = this.PointOffset(p, w, 0)
					};
          g.FillPolygon(_brushdown, pts);
          g.DrawPolygon(_pendown, pts);
        }
      }
    }

    #region private Point PointOffset(Point p, int dx, int dy)
    private Point PointOffset(Point p, int dx, int dy) {
      return new Point(p.X + dx, p.Y + dy);
    }
    #endregion
  }
}
