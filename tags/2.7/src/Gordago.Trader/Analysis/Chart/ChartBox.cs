/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
#endregion

namespace Gordago.Analysis.Chart {

	class ChartBox : IChartBox{

    public event PaintEventHandler Paint;

    #region private property

		private Bitmap _bitmap, _bitmapHS, _bitmapVS;

		private Graphics _graphics;

		private bool _pfscaleChange;
		private int _position;
		private int[] _map;
		
		private int _bouttop = 2, _boutleft = 2, _boutbottom = 1, _boutright = 2;

		private bool _horizontalScaleVisible = true, _verticalScaleVisible = true;
		private Size _horizontalSize, _verticalSize;

		private float _vsMinVal, _vsMaxVal;

		private ChartAnalyzer _analyzer;
		private int _deltax = 1;

		private int _width, _height, _left, _top;

		private bool _scaleerror, _isticksdatacaching = false;

		private ChartManager _parent;

    private float _calcul;

    private int _tbout = 20;

    private ChartFigureList _figures;
    #endregion

    private float _heightPercent = 100;
    private int _savedWidth, _savedHeight;

    private int _savedFiguresCount = -1;
    private ChartFigure[] _figuresScaled;
    private bool _changeFlagScaleInFigure = true;

    private int _decimalDigits = 0;
    private bool _enableCustomScale = false;
    private float[] _customScaleValues = new float[] { };

    private int _savedStyleId;

    private Color _gridColor, _borderColor;
    private Pen _gridPen, _borderPen, _periodSeparatorPen;
    private Color _scaleForeColor;
    private Brush _scaleForeBrush;

    #region public ChartBox(ChartManager cmanager)
    public ChartBox(ChartManager cmanager) {
			_parent = cmanager;
			_position = 0;
			_map = new int[]{};
      _figures = new ChartFigureList(this);

			this.HorizontalScaleVisible = this.VerticalScaleVisible = true;
    }
    #endregion

    #region public Color GridColor
    public Color GridColor {
      get { return this._gridColor; }
      set {
        this._gridColor = value;
        this._gridPen = new Pen(value);
        this._gridPen.DashStyle = DashStyle.Custom;
        this._gridPen.DashPattern = new float[] { 1, 2 };
        this._periodSeparatorPen = new Pen(value);
        this._periodSeparatorPen.DashStyle = DashStyle.Custom;
        this._periodSeparatorPen.DashPattern = new float[] { 3, 2 };
      }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set { 
        this._borderColor = value;
        this._borderPen = new Pen(value);
      }
    }
    #endregion

    #region public Color ScaleForeColor
    public Color ScaleForeColor {
      get { return this._scaleForeColor; }
      set { 
        this._scaleForeColor = value;
        this._scaleForeBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region internal bool ChangeFlagScaleInFigure
    /// <summary>
    /// Если фигура меняет статус "Включить влияние на масштаб", то она меняет этот флаг на true
    /// </summary>
    internal bool ChangeFlagScaleInFigure {
      get { return _changeFlagScaleInFigure; }
      set { _changeFlagScaleInFigure = true; }
    }
    #endregion

    #region Public Property

    #region internal ChartManager Manager
    internal ChartManager Manager{
			get{return this._parent;}
    }
    #endregion

    #region internal int[] Map
    internal int[] Map {
      get { return this._map; }
    }
    #endregion

    #region public IChartManager ChartManager
    public IChartManager ChartManager {
      get { return _parent; }
    }
    #endregion

    #region public ChartFigureList Figures
    public ChartFigureList Figures{
			get{return this._figures;}
		}
		#endregion

		#region internal bool PFScaleChanged
		/// <summary>
		/// Флаг изменения масштаба (позиции и т.п.)
		/// </summary>
		internal bool PFScaleChanged{
			get{return this._pfscaleChange;}
			set{this._pfscaleChange = value;}
		}
		#endregion

		#region internal float ScaleMinValue
		internal float ScaleMinValue{
			get{return this._vsMinVal;}
			set{this._vsMinVal = value;}
		}
		#endregion

		#region internal float ScaleMaxValue
		internal float ScaleMaxValue{
			get{return this._vsMaxVal;}
			set{this._vsMaxVal = value;}
		}
		#endregion

		#region internal int DeltaX
		internal int DeltaX{
			get{return this._deltax;}
		}
		#endregion

		#region public bool HorizontalScaleVisible
		public bool HorizontalScaleVisible{
			get{return this._horizontalScaleVisible;}
			set{
				this._horizontalScaleVisible = value;
        this._savedHeight = 0;
			}
		}
		#endregion

		#region public bool VerticalScaleVisible
		public bool VerticalScaleVisible{
			get{return this._verticalScaleVisible;}
			set{
				this._verticalScaleVisible = value;
        this._savedHeight = 0;
			}
		}
		#endregion

		#region internal ChartAnalyzer Analyzer
		internal ChartAnalyzer Analyzer{
			get{return this._analyzer;}
			set{this._analyzer = value;}
		}
		#endregion

		#region public int Position
		public int Position{
			get{return this._position;}
		}
		#endregion

		#region internal Bitmap Bitmap
		internal Bitmap Bitmap{
			get{return this._bitmap;}
		}
		#endregion

    #region internal Bitmap BitmapVScale
    internal Bitmap BitmapVScale {
      get { return _bitmapVS; }
    }
    #endregion

    #region internal Bitmap BitmapHScale
    internal Bitmap BitmapHScale {
      get { return _bitmapHS; }
    }
    #endregion

    #region public int Width
    /// <summary>
    /// Ширина рабочей области графика
    /// </summary>
    public int Width {
      get {
        if(_bitmap == null) return 0;
        return this._bitmap.Width; 
      }
    }
    #endregion

    #region public int Height
    public int Height {
      get {
        if(_bitmap == null) return 0;
        return this._bitmap.Height; 
      }
    }
    #endregion
    
    #region public int BoxWidth
    /// <summary>
    /// Полная ширина окна
    /// </summary>
    public int BoxWidth{
       get{return this._width;}
    }
    #endregion

    #region public int BoxHeight
    public int BoxHeight{
			get{return this._height;}
    }
    #endregion

    #region public int Top
    public int Top{
			get{return this._top;}
			set{this._top = value;}
		}
		#endregion

		#region public int Left
		public int Left{
			get{return this._left;}
			set{
				this._left = value;
			}
		}
		#endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get {
        return _decimalDigits;
      }
    }
    #endregion
    #endregion

    #region public float PercentHeight
    /// <summary>
    /// Высота в процентном соотношении
    /// </summary>
    public float PercentHeight {
      get { return this._heightPercent; }
      set {
        this._heightPercent = value;
        _savedHeight = 0;
      }
    }
    #endregion

		#region public void SetBounds(int x, int y, int width, int height)
		public void SetBounds(int x, int y, int width, int height){
			this._left = x;
			this._top = y;
			this._width = width;
			this._height = height;
		}
		#endregion

    #region internal void SetBars(IBarList bars)
    internal void SetBars(IBarList bars){
			this.PFScaleChanged = true;
    }
    #endregion

    #region public int GetY(float val)
    /// <summary>
		/// Расчет значения по вертикали, исходя из масштаба
		/// </summary>
		public int GetY(float val){
			if (_scaleerror) return 0;
			int height = _bitmap.Height - _tbout * 2;
			float tmpval = _vsMaxVal - _vsMinVal ;
			_calcul = 0;
			if (tmpval != 0)
				_calcul = height / tmpval;

      int y = Convert.ToInt32(_calcul * (_vsMaxVal - val) + _tbout);

      return y;
    }
    #endregion

    #region public COPoint GetChartPoint(Point p)
    /// <summary>
    /// Получить ценовую точку из физической точки на графике.
    /// </summary>
    /// <param name="p">Физическая точка</param>
    /// <returns>Ценовая точка</returns>
    public COPoint GetChartPoint(Point p) {
      int pos = (p.X + this.DeltaX / 2) / this.DeltaX + this.Position;
      float price = _vsMaxVal - (p.Y - _tbout) / _calcul;

      COPoint copoint = new COPoint(pos, price);

      copoint.SetPoint(
        this.GetX(copoint.BarIndex),
        this.GetY(copoint.Price));

      copoint.Check(this);
      return copoint;
    }
    #endregion

    #region public int GetBarIndex(int x)
    public int GetBarIndex(int x) {
      int pos = (x + this.DeltaX / 2) / this.DeltaX + this.Position;

      return pos;
    }
    #endregion

    #region public DateTime GetTime(int barIndex)
    public DateTime GetTime(int barIndex) {
      IBarList bars = this.ChartManager.Bars;
      int tfSecond = bars.TimeFrame.Second;
      if (barIndex < bars.Count && barIndex >= 0) {
        return bars[barIndex].Time;
      } else if (barIndex < 0) {
        DateTime beginTime = bars[0].Time;

        return new DateTime(beginTime.Ticks + barIndex * tfSecond * 10000000L);
      } else {

        DateTime endTime = bars[bars.Count - 1].Time;

        int delta = barIndex - bars.Count + 1;

        return new DateTime(endTime.Ticks + delta * tfSecond * 10000000L);
      }
    }
    #endregion

    #region public float GetPrice(int y)
    public float GetPrice(int y) {
      float price = _vsMaxVal - (y - _tbout) / _calcul;
      return price;
    }
    #endregion

    #region public Point PointToClient(Point point)
    public Point PointToClient(Point point){
			return new Point(point.X - this._boutleft - this.Left, point.Y - this._bouttop - this.Top);
		}
		#endregion

		#region public int SetPosition(int position)
		public int SetPosition(int position){
      if(this.ChartManager.Bars == null) return 0;
      int cntbar = this.ChartManager.Bars.Count;
			
			if (cntbar == 0) return 0;
			position = Math.Max(0, position);
			
			if (this.Map.Length <= 1){
				this.CreateBitmap();
				this.CalculateBitmap();
			}

      int shiftof = Map.Length-1;
      if (this.Manager.ChartShift)
        shiftof = this.Map.Length / 2 + this.Map.Length / 3;

			int maxpos = Math.Max(cntbar - shiftof, 0);

			if (position > maxpos)
				return this.SetPosition(maxpos);
			
			this.PFScaleChanged = true;
			return _position = position;
		}
		#endregion

    #region public Size HorizontalScaleSize
    /// <summary>
    /// Горизонтальная шкала
    /// </summary>
    public Size HorizontalScaleSize {
      get {
        if(this.BitmapHScale == null) return new Size();

        return this.BitmapHScale.Size;
      }
    }
    #endregion

    #region public Size VerticalScaleSize
    /// <summary>
    /// Вертикальная шкала
    /// </summary>
    public Size VerticalScaleSize {
      get {
        if(this.BitmapVScale == null) return new Size();
        return this.BitmapVScale.Size;
      }
    }
    #endregion

    #region public void PaintMethod(Graphics g)
    public void PaintMethod(Graphics g) {

      bool isticksdatacaching = (this._parent.Symbol != null) && (this._parent.Symbol.Ticks as ITickManager).Status == TickManagerStatus.DataCaching;

      if(_bitmap != null && this.Position > 0 && isticksdatacaching) {
        isticksdatacaching = false;
      }

      if(_savedWidth != this.BoxWidth || _savedHeight != this.BoxHeight) {
        CreateBitmap();
        _savedWidth = this.BoxWidth;
        _savedHeight = this.BoxHeight;
        this.PFScaleChanged = true;
      }

      if(isticksdatacaching != _isticksdatacaching) {
        this._pfscaleChange = true;
        _isticksdatacaching = isticksdatacaching;
      }

      if(this.PFScaleChanged) {
        CalculateBitmap();
        this.PFScaleChanged = false;
      }
      _scaleerror = this._vsMinVal >= this._vsMaxVal;

      if (this._savedStyleId != this.ChartManager.Style.Id) {
        _savedStyleId = this.ChartManager.Style.Id;
        this.GridColor = this.ChartManager.Style.GridColor;
        this.BorderColor = this.ChartManager.Style.BorderColor;
        this.ScaleForeColor = this.ChartManager.Style.ScaleForeColor;
      }

      PaintBitmap();

      g.DrawImageUnscaled(_bitmap, this.Left + _boutleft, this.Top + _bouttop, _bitmap.Width, _bitmap.Height);
      if(this.HorizontalScaleVisible)
        g.DrawImageUnscaled(_bitmapHS, this.Left + _boutleft, this.Top + _bitmap.Height + _bouttop, _bitmapHS.Width, _bitmapHS.Height);
      if(this.VerticalScaleVisible)
        g.DrawImageUnscaled(_bitmapVS, this.Left + _bitmap.Width + _boutleft, this.Top + _bouttop, _bitmapVS.Width, _bitmapVS.Height);
    }
    #endregion

    #region public void CreateBitmap()
    public void CreateBitmap() {
      int dw = 0, dh = 0;
      int width = Math.Max(this.BoxWidth - this._boutleft - this._boutright, 1);
      int height = Math.Max(this.BoxHeight - this._bouttop - this._boutbottom, 1);

      if(this.HorizontalScaleVisible) {
        dh = 15;
        _bitmapHS = new Bitmap(width, dh);
      }
      _horizontalSize = new Size(width, dh);

      if(this.VerticalScaleVisible) {
        dw = 38;
        _bitmapVS = new Bitmap(dw, Math.Max(height - dh, 1));
      }
      _verticalSize = new Size(dw, height - dh);

      _bitmap = new Bitmap(Math.Max(width - dw, 1), Math.Max(height - dh, 1));
      _graphics = Graphics.FromImage(_bitmap);
      _graphics.InterpolationMode = InterpolationMode.Low;
      _graphics.SmoothingMode = SmoothingMode.None;
      _graphics.CompositingMode = CompositingMode.SourceOver;
      _graphics.CompositingQuality = CompositingQuality.HighSpeed;
      _graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
    }
    #endregion

    #region internal virtual void CalculateBitmap()
    internal void CalculateBitmap() {

      if (_savedFiguresCount != _figures.Count || _changeFlagScaleInFigure) {
        _changeFlagScaleInFigure = false;
        _savedFiguresCount = _figures.Count;
        List<ChartFigure> list = new List<ChartFigure>();
        for (int i = 0; i < _figures.Count; i++) {
          if (_figures[i].EnableScale)
            list.Add(_figures[i]);
        }
        _figuresScaled = list.ToArray();
      }

      this._deltax = this.Manager.ZoomValues[(int)this.Manager.Zoom].DeltaX;
      this._map = new int[this._bitmap.Width / _deltax + 1];

      _vsMinVal = float.MaxValue;
      _vsMaxVal = float.MinValue;

      _decimalDigits = 0;
      for (int indexfigure = 0; indexfigure < this._figuresScaled.Length; indexfigure++) {

        ChartFigure figure = this._figuresScaled[indexfigure];
        if(figure.Visible && figure.IsUsingSymbolData && !_isticksdatacaching) {

          figure.IsCalculateScale = false;
          figure.CalculateScaleInit();
          figure.OnCalculateScale();
          
          _decimalDigits = Math.Max(_decimalDigits, figure.DecimalDigits);

          if(figure.IsCalculateScale) {
            _vsMinVal = Math.Min(figure.ScaleMinValue, _vsMinVal);
            _vsMaxVal = Math.Max(figure.ScaleMaxValue, _vsMaxVal);
          }
          if (figure is ChartFigureIndicator) {
            ChartFigureIndicator indicFigure = figure as ChartFigureIndicator;
            if (indicFigure.Indicator.EnableCustomScale && indicFigure.Indicator.CustomScaleValues.Length>0) {
              _enableCustomScale = true;
              _customScaleValues = indicFigure.Indicator.CustomScaleValues;
            }
          }
        }
      }
    }
		#endregion

    private float _savedVSMinVal = 0, _savedVSMaxVal = 0;

    #region private Graphics PaintVerticalScale()
    private Graphics PaintVerticalScale() {
      if (!this.VerticalScaleVisible)
        return null;

      Graphics gvscale = Graphics.FromImage(_bitmapVS);

      _savedVSMaxVal = _vsMaxVal;
      _savedVSMinVal = _vsMinVal;

      gvscale.Clear(Manager.Style.BackColor);

      if (this._scaleerror || this.ChartManager.Bars == null)
        return gvscale;

      if (!_enableCustomScale) {

        int m = SymbolManager.GetDelimiter(this.DecimalDigits);
        int minval = Convert.ToInt32(_vsMinVal * m);
        int maxval = Convert.ToInt32(_vsMaxVal * m);
        int minstep = Math.Max(Convert.ToInt32(_bitmap.Height / 60), 1);
        int oneval = Math.Max(maxval - minval, 1);
        int minvalstep = oneval / Math.Min(oneval, minstep);

        for (int ival = minval; ival <= maxval; ival += minvalstep) {
          float val = (float)ival / m;
          int y = this.GetY(val);

          if (this.Manager.GridVisible)
            _graphics.DrawLine(this._gridPen, 0, y, _bitmap.Width, y);

          if (this.VerticalScaleVisible) {
            gvscale.DrawLine(_borderPen, 0, y, 2, y);
            gvscale.DrawString(
              SymbolManager.ConvertToCurrencyString(val, this.DecimalDigits, ""),
              this.Manager.Style.ScaleFont, _scaleForeBrush, 3, y);
          }
        }
      } else {
        for (int i = 0; i < _customScaleValues.Length; i++) {
          float val = _customScaleValues[i];
          int y = this.GetY(val);

          if (this.Manager.GridVisible)
            _graphics.DrawLine(_gridPen, 0, y, _bitmap.Width, y);

          if (this.VerticalScaleVisible) {
            gvscale.DrawLine(_gridPen, 0, y, 2, y);
            gvscale.DrawString(
              SymbolManager.ConvertToCurrencyString(val, this.DecimalDigits, ""),
              this.Manager.Style.ScaleFont, _scaleForeBrush, 3, y);
          }

        }
      }
      return gvscale;
    }
    #endregion

    #region private class PeriodSeparatorsCalc
    private class PeriodSeparatorsCalc {
      private int _curPos;
      private IBarList _bars;
      private int _tfSeps = 86400;
      private Bar _lastBar;

      #region public PeriodSeparatorsCalc(IBarList bars, int pos)
      public PeriodSeparatorsCalc(IBarList bars, int pos) {
        _curPos = pos;
        _bars = bars;
        int btf = _bars.TimeFrame.Second;
        if (btf < 3600){
          _tfSeps = 3600;
        } else if (btf < 86400) {
          _tfSeps = 86400;
        } else {
          _tfSeps = -1;
        }
      }
      #endregion

      #region public bool NewPeriod(int index)
      public bool NewPeriod(int index) {
        if (index < 0 || index >= _bars.Count || _tfSeps < 0) {
          return false;
        }
        if (index == _curPos && _curPos > 0) {
          _lastBar = _bars[index - 1];
        }
        Bar bar = _bars[index];

        long sec1 = bar.Time.Ticks / 10000000L / _tfSeps;
        long sec2 = _lastBar.Time.Ticks / 10000000L / _tfSeps;
        bool newbar = sec1 - sec2 > 0;

        _lastBar = bar;
        return newbar;
      }
      #endregion
    }
    #endregion

    #region protected virtual void PaintBitmap()
    protected virtual void PaintBitmap(){

      _graphics.Clear(Manager.Style.BackColor);

      Graphics ghscale = null;

      if (this.HorizontalScaleVisible) {
        ghscale = Graphics.FromImage(_bitmapHS);
        ghscale.Clear(Manager.Style.BackColor);
      }
      
      Graphics gvscale = PaintVerticalScale();

      if (!this._scaleerror && this.ChartManager.Bars != null) {

        int cnt = this.Manager.ZoomValues[(int)this.Manager.Zoom].DeltaBarGrid;
        int cntj = cnt * 2;
        int j = cntj, ii = 0;
        bool l = true;
        bool ll = false;
        int barcount = this.ChartManager.Bars.Count;

        PeriodSeparatorsCalc pscalc = null;
        if (this.ChartManager.PeriodSeparators)
          pscalc = new PeriodSeparatorsCalc(this.ChartManager.Bars, this.Position);
        for (int i = 0; i < _map.Length; i++) {
          this._map[i] = i * this.DeltaX;
          int barindex = i + this.Position;
          if (j >= cntj) {
            if (this.HorizontalScaleVisible && this.ChartManager.Bars != null && barindex < barcount) {

              string sc = "";
              DateTime dtm = this.ChartManager.Bars[barindex].Time;
              if (l) {
                sc = dtm.ToString("d MMM yyyy");
                l = false;
              } else
                sc = dtm.ToString("d MMM ") + dtm.ToShortTimeString();

              ghscale.DrawLine(_borderPen, _map[i], 0, _map[i], 2);
              ghscale.DrawString(sc, Manager.Style.ScaleFont, _scaleForeBrush, _map[i], 2);
            }

            j = 0;
          }
          j++;

          if (ii >= cnt && this.Manager.GridVisible) {
            if (ll)
              _graphics.DrawLine(_gridPen, _map[i], 0, _map[i], this._bitmap.Height);
            ll = !ll;
            ii = 0;
          }
          ii++;

          if (pscalc != null) {
            if (pscalc.NewPeriod(barindex)) {
              _graphics.DrawLine(_periodSeparatorPen, _map[i], 0, _map[i], this._bitmap.Height);
            }
          }
        }
      }

      for(int indexfigure = 0; indexfigure < this._figures.Count; indexfigure++) {
        ChartFigure figure = this._figures[indexfigure];
        if(figure.Visible && 
          (figure.IsUsingSymbolData && !_isticksdatacaching || !figure.IsUsingSymbolData)) {

          figure.PaintInitialize();
          figure.OnPaint(_graphics);

          figure.OnPaintHorizontalScale(ghscale);
          figure.OnPaintVerticalScale(gvscale);
        }
      }
      if(this.Paint != null) {
        this.Paint(this, new PaintEventArgs(_graphics, new Rectangle(0, 0, _bitmap.Width, _bitmap.Height)));
      }
      _graphics.DrawRectangle(_borderPen, 0, 0, this._bitmap.Width - 1, this._bitmap.Height - 1);
		}
		#endregion

    #region public bool PointIsHorizontalScale(Point p)
    /// <summary>
    /// Мыщь находиться на поле горизонтальной шкалы
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public bool PointIsHorizontalScale(Point p) {
      if(!this.HorizontalScaleVisible) return false;

      if(p.Y > this.BoxHeight - 15-this._boutbottom-_bouttop) return true;
      return false;
    }
    #endregion

    #region public Color GetPixel(Point p)
    public Color GetPixel(Point p) {
      if(_bitmap == null) return Color.Transparent;
      Graphics g = Graphics.FromImage(_bitmap);
      IntPtr hdc = g.GetHdc();
      int colorRef = GDI.GetPixel(hdc, p.X, p.Y);
      g.ReleaseHdc(hdc);
      return ColorTranslator.FromWin32(colorRef);
    }
    #endregion

    #region public bool CheckPointInRect(Point p, int widht, int height)
    /// <summary>
    /// Проверка на наличие объекта на графике
    /// </summary>
    /// <param name="p"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool CheckPointInRect(Point p, int widht, int height) {
      int x = p.X - widht / 2;
      int y = p.Y - widht / 2;

      for(int ix = x; ix <= x + widht; ix++) {
        for(int iy = y; iy <= y + height; iy++) {
          Color c = GetPixel(new Point(ix, iy));
          if(c != this.Manager.Style.BackColor)
            return true;
        }
      }
      return false;
    }
    #endregion

    #region public int GetX(int barIndex)
    /// <summary>
    /// Получить точку X на графике из индекса бара
    /// </summary>
    /// <param name="barIndex">Индекс бара</param>
    /// <returns>Точка Х на графике</returns>
    public int GetX(int barIndex) {

      int delta = barIndex - this.Position;
      return delta * this._deltax;
    }
    #endregion

    #region public int GetXFromAnalyzer(int barIndex)
    public int GetXFromAnalyzer(int barIndex) {
     return  this.GetX(barIndex + this.Analyzer.Offset);
   }
    #endregion
 }
}
