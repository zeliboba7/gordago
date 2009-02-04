/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gordago.Analysis.Chart {

  public abstract class ChartFigure {
		private ChartBox _chartbox;
		private bool _iscalculscale = true, _visible = true;
		private float _scaleminvalue, _scalemaxvalue;
		private GraphicsPath _graphicspath;
		private int _shift = 0;
		private string _name;
    
		private bool _propertyEnable = false;

    private bool _usingsymboldata = false;

    private string _toolTipText;

    private string _id;

    private bool _enableScale = false;
    private int _decimalDigits = 0;
    private bool _invalidate = false;

    public ChartFigure(string name, bool usingSymbolData){
			this._name = name;
      this._usingsymboldata = usingSymbolData;
      _id = Guid.NewGuid().ToString();
		}

    public ChartFigure(string name):this(name, true) { }

    #region internal bool IsInvalidate
    internal bool IsInvalidate {
      get { return _invalidate; }
    }
    #endregion

    #region internal protected bool EnableScale
    /// <summary>
    /// Разрешить фигуре влиять на масштаб графика
    /// </summary>
    internal protected bool EnableScale {
      get { return _enableScale; }
      set {
        if (this.ChartBox != null) {
          (this.ChartBox as ChartBox).ChangeFlagScaleInFigure = true;
        }
        _enableScale = true; 
      }
    }
    #endregion

    #region internal virtual protected int DecimalDigits
    /// <summary>
    /// Кол-во знаков после запятой
    /// </summary>
    internal virtual protected int DecimalDigits {
      get { return _decimalDigits; }
      set {
        _decimalDigits = Math.Max(0, value);
      }
    }
    #endregion

    #region public bool Id
    /// <summary>
    /// Идентификатор фигуры
    /// </summary>
    [Browsable(false)]
    public string Id {
      get { return this._id; }
    }
    #endregion

    #region internal bool IsUsingSymbolData
    /// <summary>
    /// Использует ли фигура данные валютной пары
    /// </summary>
    internal bool IsUsingSymbolData {
      get { return this._usingsymboldata; }
    }
    #endregion

    #region internal bool IsCalculateScale
    internal bool IsCalculateScale {
      get { return this._iscalculscale; }
      set { this._iscalculscale = value; }
    }
    #endregion

    #region internal float ScaleMaxValue
    internal float ScaleMaxValue {
      get { return this._scalemaxvalue; }
      set { this._scalemaxvalue = value; }
    }
    #endregion

    #region internal float ScaleMinValue
    internal float ScaleMinValue {
      get { return this._scaleminvalue; }
      set { this._scaleminvalue = value; }
    }
    #endregion

    #region public bool PropertyEnable
    /// <summary>
    /// Визуальное редактирование свойств фигуры
    /// </summary>
    [Browsable(false)]
    public bool PropertyEnable {
      get { return this._propertyEnable; }
      set { this._propertyEnable = value; }
    }
    #endregion

    #region public string ToolTipText
    /// <summary>
    /// Текст всплывающей подсказки
    /// </summary>
    [Browsable(false)]
    public virtual string ToolTipText {
      get { return this._toolTipText; }
      set { this._toolTipText = value; }
    }
    #endregion

    #region public int Shift
    [Browsable(false)]
    public int Shift {
			get{return this._shift;}
			set{
				if (_shift != value && _chartbox != null)
					this._chartbox.PFScaleChanged = true;
				
				this._shift = value;
			}
		}
		#endregion

		#region public string Name
    [Category("Main"), DisplayName("Name")]
    public string Name {
			get{return this._name;}
      set {
        if(this._chartbox == null) return;
        IChartBox[] boxes = this._chartbox.ChartManager.ChartBoxes;
        for(int i = 0; i < boxes.Length; i++) {
          IChartBox cbox = boxes[i];
          for(int k = 0; k < cbox.Figures.Count; k++) {
            ChartFigure figure = cbox.Figures[k];
            if(figure.Name == value)
              return;
          }
        }
        this._name = value;
      }
		}
		#endregion

		#region public bool Visible
    [Browsable(false)]
    public bool Visible {
			get{return this._visible;}
			set{this._visible = value;}
		}
		#endregion

    #region public IChartBox ChartBox
    [Browsable(false)]
    public IChartBox ChartBox {
      get { return this._chartbox; }
    }
    #endregion

    #region protected int[] Map
    protected int[] Map {
      get { return this._chartbox.Map; }
    }
    #endregion

    #region protected virtual GraphicsPath GraphicsPath
    protected virtual GraphicsPath GraphicsPath { 
			get{return this._graphicspath;}
    }
    #endregion

    #region protected internal virtual void OnCalculateScale()
    /// <summary>
    /// Рассчет вертикальной шкалы
    /// </summary>
    protected internal virtual void OnCalculateScale() { }
    #endregion

    #region internal void PaintInitialize()
    internal void PaintInitialize() {
      _graphicspath = new GraphicsPath();
    }
    #endregion

    #region public void ReCalculateScale()
    public void ReCalculateScale() {
      if(_chartbox != null)
        this._chartbox.PFScaleChanged = true;
    }
    #endregion

    #region protected internal virtual void OnPaint(Graphics g)
    /// <summary>
    /// Прорисовка фигуры
    /// </summary>
    /// <param name="g"></param>
    protected internal virtual void OnPaint(Graphics g) { }
    #endregion

    /// <summary>
    /// Прорисовка горизонтальной шкалы
    /// </summary>
    /// <param name="g"></param>
    protected internal virtual void OnPaintHorizontalScale(Graphics g) { }

    /// <summary>
    /// Прорисовка вертикальной шкалы
    /// </summary>
    /// <param name="g"></param>
    protected internal virtual void OnPaintVerticalScale(Graphics g) { }

    /// <summary>
    /// Удаление фигуры с графика
    /// </summary>
    protected internal virtual void OnDestroy() { }

    //protected internal virtual void OnMouseMove(Point p) {    }
    //protected internal virtual void OnMouseUp(Point p) { }

    protected internal virtual void OnMouseDown(MouseEventArgs e) { }
    protected internal virtual void OnMouseUp(MouseEventArgs e) {}
    protected internal virtual void OnMouseMove(MouseEventArgs e) { }

    protected virtual void OnSaveTemplate(XmlNodeManager nodeManager) { }
    protected virtual void OnLoadTemplate(XmlNodeManager nodeManager) { }

    protected virtual void OnAddedOnChartBox() { }

		#region internal virtual void SetChartBox(ChartBox chartBox)
		internal virtual void SetChartBox(ChartBox chartBox){
      if(chartBox == null && this._chartbox != null) {
        _chartbox = null;
        return;
      }

			if (this._chartbox != null)
				throw (new Exception("The figure is already added on the chart"));
			_chartbox = chartBox;
      this.OnAddedOnChartBox();
		}
		#endregion

		#region internal void CalculateScaleInit()
		internal void CalculateScaleInit(){
			_scaleminvalue = float.MaxValue;
			_scalemaxvalue = float.MinValue;
		}
		#endregion

		#region protected void SetScaleValue(float value)
		protected void SetScaleValue(float value){
			this.SetScaleValue(value, value);
		}
		#endregion

		#region protected void SetScaleValue(float min, float max)
		protected void SetScaleValue(float min, float max){
			_scaleminvalue = Math.Min(min, _scaleminvalue);
			_scalemaxvalue = Math.Max(max, _scalemaxvalue);
			this._iscalculscale = true;
		}
		#endregion

		#region public virtual bool CheckFigure(Point p) 
		public virtual bool CheckFigure(Point p) {
			if (this.GraphicsPath == null) return false;

      bool fret = this.GraphicsPath.IsOutlineVisible(p, new Pen(Color.Transparent, 3));
			if (!fret){
				fret = this.GraphicsPath.IsVisible(p);
			}
			return fret;
		}
		#endregion

    #region public void SaveTemplate(XmlNodeManager nodeManager)
    public void SaveTemplate(XmlNodeManager nodeManager) {
      this.OnSaveTemplate(nodeManager);
    }
    #endregion

    #region public void LoadTemplate(XmlNodeManager nodeManager)
    public void LoadTemplate(XmlNodeManager nodeManager) {
      this.OnLoadTemplate(nodeManager);
    }
    #endregion

    #region public void Invalidate()
    public void Invalidate() {
      _invalidate = true;
    }
    #endregion

    protected void DrawComment(Graphics g, ChartFigureComment comment, int x, int y, ContentAlignment contentAlignment) {

      if (comment.AutoStyle && comment.ForeColor != this.ChartBox.ChartManager.Style.ScaleForeColor) {
        comment.ForeColor = this.ChartBox.ChartManager.Style.ScaleForeColor;
        comment.BackColor = this.ChartBox.ChartManager.Style.BackColor;
        comment.BorderColor = this.ChartBox.ChartManager.Style.BorderColor;
      }

      if (contentAlignment == ContentAlignment.MiddleCenter) {
        contentAlignment = ChartFigureComment.GetContentAlignment(new Point(x, y), new Size(this.ChartBox.Width, this.ChartBox.Height));
        //int dx = this.ChartBox.Width / 3;
        //int dy = this.ChartBox.Height / 3;
        //if (x <= dx && y <= dy)
        //  contentAlignment = ContentAlignment.TopLeft;
        //else if (x >= dx && x <= dx * 2 && y < dy)
        //  contentAlignment = ContentAlignment.TopCenter;
        //else if (x >= dx * 2 && y < dy)
        //  contentAlignment = ContentAlignment.TopRight;
        //else if (x < dx && y >= dy && y < dy * 2)
        //  contentAlignment = ContentAlignment.MiddleLeft;
        //else if (x > dx * 2 && y >= dy && y < dy * 2)
        //  contentAlignment = ContentAlignment.MiddleRight;
        //else if (x < dx && y >= dy * 2)
        //  contentAlignment = ContentAlignment.BottomLeft;
        //else if (x >= dx && x <= dx * 2 && y >= dy * 2)
        //  contentAlignment = ContentAlignment.BottomCenter;
        //else if (x >= dx * 2 && y >= dy * 2)
        //  contentAlignment = ContentAlignment.BottomRight;
        ////else if (x >= dx && x <= dx * 2 && y >= dy && y < dy * 2)
        ////  contentAlignment = ContentAlignment.BottomLeft;
        //else
        //  contentAlignment = ContentAlignment.TopRight;
      }
      SizeF sizef = g.MeasureString(comment.Text, comment.Font, 10000);
      int deltaBorder = 2;
      sizef.Width += deltaBorder;
      sizef.Height += deltaBorder;

      int left, top, w =(int)sizef.Width, h = (int)sizef.Height;
      int outx = 15, outy = 15;
      int rw = (int)sizef.Width, rh = (int)sizef.Height;

      Point p = new Point(x, y);
      Point[] pts = null;

      int pips = 4;

      switch (contentAlignment) {
        #region case ContentAlignment.TopLeft:
        case ContentAlignment.TopLeft:
          left = x + outx ;
          top = y + outy ;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, pips+outx, outy),
										 p = PointOffset(p, -pips, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, w, 0),
										 p = PointOffset(p, 0, -h),
										 p = PointOffset(p, -(w-pips*2), 0),
										 new Point(x, y)};
          break;
        #endregion
        #region case ContentAlignment.TopCenter:
        case ContentAlignment.TopCenter:
          left = x - w / 2 ;
          top = y + outy;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, pips/2, outy),
										 p = PointOffset(p, w/2-pips/2, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, -w, 0),
										 p = PointOffset(p, 0, -h),
										 p = PointOffset(p, w/2-pips/2, 0),
										 new Point(x, y)};
          break;
        #endregion
        #region case ContentAlignment.TopRight:
        case ContentAlignment.TopRight:
          left = x - w - outx;
          top = y + outy;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, -(pips+outx), +outy),
										 p = PointOffset(p, pips, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, -w, 0),
										 p = PointOffset(p, 0, -h),
										 p = PointOffset(p, w-pips*2, 0),
										 new Point(x, y)};

          break;
        #endregion
        #region case ContentAlignment.MiddleLeft:
        case ContentAlignment.MiddleLeft:
          left = x + outx;
          top = y - h/2;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, outx, -pips/2),
										 p = PointOffset(p, 0, -(h/2-pips/2)),
										 p = PointOffset(p, w, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, -w, 0),
										 p = PointOffset(p, 0, -(h/2-pips/2)),
										 new Point(x, y)};
          break;
        #endregion
        #region case ContentAlignment.MiddleRight:
        case ContentAlignment.MiddleRight:
          left = x - w - outx;
          top = y - h / 2;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, -outx, -pips/2),
										 p = PointOffset(p, 0, -(h/2-pips/2)),
										 p = PointOffset(p, -w, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, w, 0),
										 p = PointOffset(p, 0, -(h/2-pips/2)),
										 new Point(x, y)};
          break;
        #endregion
        #region case ContentAlignment.BottomCenter:
        case ContentAlignment.BottomCenter:
          left = x - w / 2;
          top = y - outy-h;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, pips/2, -outy),
										 p = PointOffset(p, w/2-pips/2, 0),
										 p = PointOffset(p, 0, -h),
										 p = PointOffset(p, -w, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, w/2-pips/2, 0),
										 new Point(x, y)};
          break;
        #endregion
        #region case ContentAlignment.BottomRight:
        case ContentAlignment.BottomRight:
          left = x - w - outx;
          top = y - outy-h;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, -(pips+outx), -outy),
										 p = PointOffset(p, pips, 0),
										 p = PointOffset(p, 0, -h),
										 p = PointOffset(p, -w, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, w-pips*2, 0),
										 new Point(x, y)};
          break;
        #endregion
        #region case ContentAlignment.BottomLeft:
        case ContentAlignment.BottomLeft:
        default:
          left = x + outx;
          top = y - outy-h;
          pts = new Point[]{
										 p,
										 p = PointOffset(p, pips+outx, -outy),
										 p = PointOffset(p, -pips, 0),
										 p = PointOffset(p, 0, -h),
										 p = PointOffset(p, w, 0),
										 p = PointOffset(p, 0, h),
										 p = PointOffset(p, -(w-pips*2), 0),
										 new Point(x, y)};
          break;
        #endregion
      }

      if (pts == null) return;

      g.FillPolygon(comment.BackBrush, pts);
      g.DrawPolygon(comment.BorderPen, pts);

      g.DrawString(comment.Text, comment.Font, comment.ForeBrush, left+2, top, comment.StringFormat);
    }

    #region private Point PointOffset(Point p, int dx, int dy)
    private Point PointOffset(Point p, int dx, int dy) {
      return new Point(p.X + dx, p.Y + dy);
    }
    #endregion
  }

  public class ChartFigureComment {

    private string _text;
    private Font _font;
    private Color _forecolor;
    private Brush _forebrush;
    private Color _backcolor;
    private Brush _backbrush;
    private Color _bordercolor;
    private Pen _borderpen;
    private StringFormat _sformat;

    private bool _autoStyle;

    public ChartFigureComment(string text) {
      _text = text;
      this.Font = new Font("Microsoft Sans Serif", 7);
      _sformat = new StringFormat();
      _sformat.LineAlignment = StringAlignment.Near;
      _sformat.Alignment = StringAlignment.Near;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;
      this.AutoStyle = true;
    }

    #region public bool AutoStyle
    public bool AutoStyle {
      get { return _autoStyle; }
      set { _autoStyle = value; }
    }
    #endregion

    #region public Font Font
    public Font Font {
      get { return this._font; }
      set { this._font = value; }
    }
    #endregion

    #region public Color ForeColor
    public Color ForeColor {
      get { return this._forecolor; }
      set {
        this._forecolor = value;
        this._forebrush = new SolidBrush(value);
      }
    }
    #endregion

    #region internal Brush ForeBrush
    internal Brush ForeBrush {
      get { return _forebrush; }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._bordercolor; }
      set {
        this._bordercolor = value;
        this._borderpen = new Pen(value);
      }
    }
    #endregion

    #region internal Pen BorderPen
    internal Pen BorderPen {
      get { return _borderpen; }
    }
    #endregion

    #region public Color BackColor
    public Color BackColor {
      get { return this._backcolor; }
      set {
        this._backcolor = value;
        this._backbrush = new SolidBrush(Color.FromArgb(200, value));
      }
    }
    #endregion

    #region internal Brush BackBrush
    internal Brush BackBrush {
      get { return _backbrush; }
    }
    #endregion

    #region public string Text
    public string Text {
      get { return _text; }
      set { _text = value; }
    }
    #endregion

    #region public StringFormat StringFormat
    public StringFormat StringFormat {
      get { return _sformat; }
      set { _sformat = value; }
    }
    #endregion

    #region public static ContentAlignment GetContentAlignment(int width, int height)
    public static ContentAlignment GetContentAlignment(Point p, Size size) {
      int dx = size.Width / 3;
      int dy = size.Height / 3;
      int x = p.X;
      int y = p.Y;
      ContentAlignment contentAlignment;
      if (x <= dx && y <= dy)
        contentAlignment = ContentAlignment.TopLeft;
      else if (x >= dx && x <= dx * 2 && y < dy)
        contentAlignment = ContentAlignment.TopCenter;
      else if (x >= dx * 2 && y < dy)
        contentAlignment = ContentAlignment.TopRight;
      else if (x < dx && y >= dy && y < dy * 2)
        contentAlignment = ContentAlignment.MiddleLeft;
      else if (x > dx * 2 && y >= dy && y < dy * 2)
        contentAlignment = ContentAlignment.MiddleRight;
      else if (x < dx && y >= dy * 2)
        contentAlignment = ContentAlignment.BottomLeft;
      else if (x >= dx && x <= dx * 2 && y >= dy * 2)
        contentAlignment = ContentAlignment.BottomCenter;
      else if (x >= dx * 2 && y >= dy * 2)
        contentAlignment = ContentAlignment.BottomRight;
      //else if (x >= dx && x <= dx * 2 && y >= dy && y < dy * 2)
      //  contentAlignment = ContentAlignment.BottomLeft;
      else
        contentAlignment = ContentAlignment.TopRight;
      return contentAlignment;
    }
    #endregion

    public static ContentAlignment GetContentAlignmentOpposite(Point p, Size size) {
      ContentAlignment conentAlignment = GetContentAlignment(p, size);
      switch (conentAlignment) {
        case ContentAlignment.BottomCenter:
          return ContentAlignment.TopCenter;
        case ContentAlignment.BottomLeft:
          return ContentAlignment.TopRight;
        case ContentAlignment.BottomRight:
          return ContentAlignment.TopLeft;
        case ContentAlignment.MiddleCenter:
          return ContentAlignment.MiddleCenter;
        case ContentAlignment.MiddleLeft:
          return ContentAlignment.MiddleRight;
        case ContentAlignment.MiddleRight:
          return ContentAlignment.MiddleLeft;
        case ContentAlignment.TopCenter:
          return ContentAlignment.BottomCenter;
        case ContentAlignment.TopLeft:
          return ContentAlignment.BottomRight;
        case ContentAlignment.TopRight:
          return ContentAlignment.BottomLeft;
      }
      return ContentAlignment.BottomLeft;
    }
  }
}
