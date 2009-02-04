/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Gordago.Design;
using System.ComponentModel.Design;
using System.Drawing.Design;
#endregion
 
namespace Gordago.Analysis.Chart {

  #region public class COPoint
  /// <summary>
  /// Точка на ценовом графике
  /// </summary>

  [DisplayName("Point")]
  public class COPoint {

    internal enum ChangeStatus {
      None,
      Time,
      BarIndex
    }

    private long _time = 0;
    private float _price;
    private int _barIndex;
    private int _saveCountBar;
    private int _x, _y;

    private ChangeStatus _changeStatus = ChangeStatus.None;

    #region public COPoint(DateTime time, float price)
    public COPoint(DateTime time, float price) {
      this.SetValue(time, price);
    }
    #endregion

    #region public COPoint(int barIndex, float price)
    public COPoint(int barIndex, float price) {
      this.SetValue(barIndex, price);
    }
    #endregion

    #region internal ChangeStatus ChangeStatus
    internal ChangeStatus ChangeSt {
      get { return this._changeStatus; }
    }
    #endregion

    #region public DateTime Time
    public DateTime Time {
      get { return new DateTime(this._time); }
      set { 
        _time = value.Ticks;
        _changeStatus = ChangeStatus.Time;
      }
    }
    #endregion

    #region public int BarIndex
    [Browsable(false)]
    public int BarIndex {
      get { 
        return this._barIndex;
      }
      set { 
        this._barIndex = value;
        _changeStatus = ChangeStatus.BarIndex;
      }
    }
    #endregion

    #region public float Price
    public float Price {
      get { return this._price; }
      set { this._price = value; }
    }
    #endregion

    #region public int X
    /// <summary>
    /// Координата X на графике
    /// </summary>
    [Browsable(false)]
    public int X {
      get { return this._x; }
    }
    #endregion

    #region public int Y
    [Browsable(false)]
    public int Y {
      get { return this._y; }
    }
    #endregion

    #region internal void SetPoint(int x, int y)
    /// <summary>
    /// Установить физические координаты на графике
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    internal void SetPoint(int x, int y) {
      _x = x;
      _y = y;
    }
    #endregion

    #region public void SetValue(DateTime time, float price)
    public void SetValue(DateTime time, float price) {
      _time = time.Ticks;
      _price = price;
      _saveCountBar = -1;
    }
    #endregion

    #region public void SetValue(int barIndex, float price)
    public void SetValue(int barIndex, float price) {
      _barIndex = barIndex;
      _price = price;
      _time = 0;
      _saveCountBar = -1;
    }
    #endregion

    #region public void SetOffset(int barIndex, float price)
    /// <summary>
    /// Смешение фигуры
    /// </summary>
    /// <param name="barIndex"></param>
    /// <param name="price"></param>
    public void SetOffset(int barIndex, float price) {
      _barIndex += barIndex;
      _price += price;
      _time = 0;
      _saveCountBar = 0;
    }
    #endregion

    #region public void Check(IChartBox chartbox)
    public void Check(IChartBox chartbox) {
      IBarList bars = chartbox.ChartManager.Bars;

      if (_saveCountBar == bars.Count)
        return;
      if (_time > 0) {
        _barIndex = bars.GetBarIndex(new DateTime(_time));
      } else {
        _time = chartbox.GetTime(_barIndex).Ticks;
      }
      _saveCountBar = bars.Count;
    }
    #endregion
  }
  #endregion

  #region public abstract class COPointManager
  /// <summary>
  /// Опорные точки обьекта на графике
  /// </summary>
  public abstract class COPointManager {

    private COPoint[] _copoints;

    /// <summary>
    /// Физическое положение опорных точек. 
    /// Массив на 1 больше, в последнем значение точка перемещение всей фигуры
    /// </summary>
    private Point[] _anchors;

    /// <summary>
    /// Кол-во созданных точек
    /// </summary>
    private int _countCOPointCreated;

    private ChartObject _chartObject;

    public COPointManager(int countCOPoints, int countAnchors) {
      _copoints = new COPoint[countCOPoints];
      _anchors = new Point[countAnchors];
      _countCOPointCreated = 0;
    }

    #region public int Count
    public int Count {
      get { return this._copoints.Length; }
    }
    #endregion

    #region public bool IsCreate
    /// <summary>
    /// Создана ли фигура. Истина - граф.объект имеет все точки, 
    /// иначе находиться на стадии создания
    /// </summary>
    public bool IsCreate {
      get {
        return this.Count == _countCOPointCreated; 
      }
    }
    #endregion

    #region public int CountCOPointCreated
    public int CountCOPointCreated {
      get { return this._countCOPointCreated; }
    }
    #endregion

    #region public COPoint this[int index]
    public COPoint this[int index] {
      get { return this._copoints[index]; }
    }
    #endregion

    #region public Point[] Anchors
    public Point[] Anchors {
      get { return this._anchors; }
    }
    #endregion

    #region protected internal virtual void OnCalculateAnchors()
    protected internal virtual void OnCalculateAnchors() {
      for (int i = 0; i < _copoints.Length; i++) {
        if (_copoints[i] != null) {
          _copoints[i].Check(this._chartObject.ChartBox);
          int x = _anchors[i].X = _chartObject.ChartBox.GetX(_copoints[i].BarIndex);
          int y = _anchors[i].Y = _chartObject.ChartBox.GetY(_copoints[i].Price);
          _copoints[i].SetPoint(x, y);
        }
      }
    }
    #endregion

    #region internal void SetChartObject(ChartObject chartObject)
    internal void SetChartObject(ChartObject chartObject) {
      _chartObject = chartObject;
    }
    #endregion

    #region public int GetAnchorIndex(Point p)
    /// <summary>
    /// Получить номер якоря для модификации обьекта
    /// </summary>
    /// <param name="p"></param>
    /// <returns>-1 вхождения нет</returns>
    public int GetAnchorIndex(Point p) {
      int size = 3;
      for(int i = 0; i < _anchors.Length; i++) {
        Point cp = _anchors[i];
        if(p.X >= cp.X - size && p.X <= cp.X + size &&
           p.Y >= cp.Y - size && p.Y <= cp.Y + size) {
          return i;
        }
      }
      return -1;
    }
    #endregion

    #region public void Move(Point p, int indexAnchor)
    /// <summary>
    /// Модификация фигуры за якорь, или ее перемеещение
    /// </summary>
    /// <param name="p"></param>
    /// <param name="indexAnchor"></param>
    public void Move(Point p, int indexAnchor) {
      COPoint chartPoint = _chartObject.ChartBox.GetChartPoint(p);
      if(indexAnchor == _anchors.Length - 1) {
        Point prevCOP = _anchors[_anchors.Length - 1];
        int dx = p.X - prevCOP.X;
        int dy = p.Y - prevCOP.Y;

        Point pone = new Point(_anchors[0].X + dx, _anchors[0].Y + dy);

        COPoint newCPOne = _chartObject.ChartBox.GetChartPoint(pone);
        int dBarIndex = newCPOne.BarIndex - _copoints[0].BarIndex;
        float dPrice = newCPOne.Price - _copoints[0].Price;

        for(int i = 0; i < _copoints.Length; i++) {
          _copoints[i].SetOffset(dBarIndex, dPrice);
        }
      } else {

      }
    }
    #endregion

    #region public void Move(int deltaBarIndex, float deltaPrice, int indexAnchor)
    public void Move(int deltaBarIndex, float deltaPrice, int indexAnchor) {
      if(indexAnchor >= this._copoints.Length) {
        /* перемещение всей фигуры */

        COPoint oldcopoint = _chartObject.ChartBox.GetChartPoint(this._anchors[_copoints.Length]);
        int dBarIndex = deltaBarIndex - oldcopoint.BarIndex;
        float dPrice = deltaPrice - oldcopoint.Price;

        for (int i = 0; i < _copoints.Length; i++) {
          _copoints[i].SetValue(_copoints[i].BarIndex + dBarIndex, _copoints[i].Price + dPrice);
        }
      } else {
        _copoints[indexAnchor].SetValue(deltaBarIndex, deltaPrice);
      }
    }
    #endregion

    #region public void Create(int barIndex, float price)
    public void Create(int barIndex, float price) {
      _copoints[_countCOPointCreated] = new COPoint(barIndex, price);
      this.CheckCreated();
    }
    #endregion

    #region public void Create(DateTime time, float price)
    public void Create(DateTime time, float price) {
      _copoints[_countCOPointCreated] = new COPoint(time, price);
      this.CheckCreated();
    }
    #endregion

    #region private void CheckCreated()
    private void CheckCreated() {
      for(int i = 0; i < _copoints.Length; i++) {
        if(_copoints[i] == null) {
          _countCOPointCreated = i;
          return;
        }
      }
      _countCOPointCreated = _copoints.Length;
    }
    #endregion
  }
  #endregion

  #region public class COPointArrayTypeConverter : TypeConverter
  public class COPointArrayTypeConverter : TypeConverter {

    #region public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
      if (sourceType == typeof(string)) {
        return true;
      }
      return base.CanConvertFrom(context, sourceType);
    }
    #endregion

    #region public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
      try {
        string sval = value as string;
        if (context.PropertyDescriptor.PropertyType == typeof(float[])) {
          string[] svals = sval.Split(new char[] { ';' });
          float[] fvals = new float[svals.Length];
          for (int i = 0; i < fvals.Length; i++) {
            fvals[i] = Convert.ToSingle(svals[i]);
          }
          return fvals;
        } else if (context.PropertyDescriptor.PropertyType == typeof(int[])) {
          string[] svals = sval.Split(new char[] { ';' });
          int[] ivals = new int[svals.Length];
          for (int i = 0; i < ivals.Length; i++) {
            ivals[i] = Convert.ToInt32(svals[i]);
          }
          return ivals;
        } else if (context.PropertyDescriptor.PropertyType == typeof(FibonacciLevel[])) {
          string[] svals = sval.Split(new char[] { ';' });
          FibonacciLevel[] fvals = new FibonacciLevel[svals.Length];
          for (int i = 0; i < fvals.Length; i++) {
            fvals[i] = new FibonacciLevel();
            fvals[i].Value = Convert.ToSingle(svals[i]);
          }
          return fvals;
        } else if (context.PropertyDescriptor.PropertyType == typeof(COPoint[])) {
          string[] svals = sval.Split(new char[] { ';' });
          int lenght = svals.Length / 2;
          COPoint[] copoints = new COPoint[lenght];
          int j = 0;

          for (int i = 0; i < lenght; i++) {
            float rate = Convert.ToSingle(svals[j++]);
            DateTime time = Convert.ToDateTime(svals[j++]);
            copoints[i] = new COPoint(time, rate);
          }
          return copoints;
        }
        return base.ConvertFrom(context, culture, value);
      } catch {
        if (context.PropertyDescriptor.PropertyType == typeof(float[])) {
          return new float[0];
        }
        return null;
      }
    }
    #endregion

    #region public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
      if (value is float[]) {
        float[] values = value as float[];
        string[] svalues = new string[values.Length];
        for (int i = 0; i < values.Length; i++) {
          svalues[i] = values[i].ToString();
        }
        return string.Join("; ", svalues);
      } else if (value is FibonacciLevel[]) {
        FibonacciLevel[] values = value as FibonacciLevel[];
        string[] svalues = new string[values.Length];
        for (int i = 0; i < values.Length; i++) {
          svalues[i] = values[i].Value.ToString();
        }
        return string.Join("; ", svalues);
      } else if (value is int[]) {
        int[] values = value as int[];
        string[] svalues = new string[values.Length];
        for (int i = 0; i < values.Length; i++) {
          svalues[i] = values[i].ToString();
        }
        return string.Join("; ", svalues);
      } else if(value is COPoint[]){
        COPoint[] cps = value as COPoint[];
        string[] sa = new string[cps.Length * 2];
        int j = 0;
        for (int i = 0; i < cps.Length; i++) {
          COPoint cp = cps[i];
          sa[j++] = cp.Price.ToString();
          sa[j++] = cp.Time.ToString();
        }
        return string.Join("; ", sa);
      } else {
        return base.ConvertTo(context, culture, value, destinationType);
      }
    }
    #endregion

    #region public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
      if (destinationType == typeof(float[]))
        return true;
      else if (destinationType == typeof(int[]))
        return true;
      else if (destinationType == typeof(FibonacciLevel))
        return true;
      return base.CanConvertTo(context, destinationType);
    }
    #endregion
  }
  #endregion

  public abstract class ChartObject:ChartFigure {

    #region Private property
    private COPointManager _copoints;

    private Cursor _cursor;
    private Image _image;

    private COPoint _mouseMoveCOPoint, _tempCOPointBegin;

    private Pen _tempLinePen;

    private Pen _boxPen;
    private Brush _boxBrush;

    private int _tempAnchorIndex = -1;
    private string _typeName;

    private int _savedCountBars = -1;

    private string _groupName;
    #endregion

    #region public ChartObject(string name, string groupName, COPointManager copoints) : base(name, true)
    public ChartObject(string name, string groupName, COPointManager copoints) : base(name, true) {
      this.PropertyEnable = true;
      _groupName = groupName;
      _copoints = copoints;
      copoints.SetChartObject(this);
      _tempLinePen = new Pen(Color.Black);
      _tempLinePen.DashStyle = DashStyle.Custom;
      _tempLinePen.DashPattern = new float[] { 5, 5 };

      _boxPen = new Pen(Color.Red);
      _boxBrush = new SolidBrush(Color.White);
    }
    #endregion

    public ChartObject(string name, COPointManager copoints) : this(name, "main", copoints) { }

    #region public COPointManager COPoints
    [Browsable(false)]
    public COPointManager COPoints {
      get {
        if (this.ChartBox != null && _savedCountBars != this.ChartBox.ChartManager.Bars.Count) {
          _savedCountBars = this.ChartBox.ChartManager.Bars.Count;
          this._copoints.OnCalculateAnchors();
        }
        return this._copoints; 
      }
    }
    #endregion

    [Category("Main"), DisplayName("Points")]
    [TypeConverter(typeof(COPointArrayTypeConverter))]
    [EditorAttribute(typeof(ArrayEditor), typeof(UITypeEditor))]
    public COPoint[] Points {
      get {
        COPointManager coPoints = this.COPoints;
        COPoint[] points = new COPoint[coPoints.Count];
        for (int i = 0; i < coPoints.Count; i++) {
          points[i] = coPoints[i];
        }
        return points; 
      }
      set {
        COPointManager coPoints = this.COPoints;
        if (value.Length != coPoints.Count)
          return;
        for (int i = 0; i < value.Length; i++) {
          coPoints[i].SetValue(value[i].Time, value[i].Price);
        }
      }
    }

    #region public string TypeName
      /// <summary>
      /// Наименование типа графического объекта
      /// </summary>
      [Browsable(false)]
    public string TypeName {
      get { return this._typeName; }
      set { this._typeName = value; }
    }
    #endregion

    #region public Cursor Cursor
    /// <summary>
    /// Курсор в момент добавление фигуры на график
    /// </summary>
    [Browsable(false)]
    public Cursor Cursor {
      get { return this._cursor; }
      set { this._cursor = value; }
    }
    #endregion

    #region public Image Image
    [Browsable(false)]
    public Image Image {
      get { return this._image; }
      set { this._image = value; }
    }
    #endregion

    #region protected COPoint MouseMoveCOPoint
    /// <summary>
    /// Точка перемещения мыши
    /// </summary>
    internal protected COPoint MouseMoveCOPoint {
      get { return this._mouseMoveCOPoint;}
    }
    #endregion

    #region internal protected COPoint MouseDownCOPoint
    /// <summary>
    /// В момент нажатие левой кнопки мыши, фиксируеться эта точка
    /// </summary>
    internal protected COPoint MouseDownCOPoint {
      get { return this._tempCOPointBegin; }
    }
    #endregion

    #region protected override void OnSaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      for(int i = 0; i < _copoints.Count; i++) {
        nodeManager.SetAttribute("Time" + i.ToString(), _copoints[i].Time);
        nodeManager.SetAttribute("Price" + i.ToString(), _copoints[i].Price);
      }
    }
    #endregion

    #region protected override void OnLoadTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      for(int i = 0; i < _copoints.Count; i++) {
        DateTime time = nodeManager.GetAttributeDateTime("Time" + i.ToString(), DateTime.Now);
        float price = nodeManager.GetAttributeFloat("Price" + i.ToString(), 0);
        this.COPoints.Create(time, price);
      }
    }
    #endregion

    #region public COPoint GetCOPoint(int index)
    /// <summary>
    /// Получить точку. Функцию полезно использовать, когда фигура 
    /// находиться на стадии создания. Например, если фигура из двух опорных
    /// точек, и вторая находиться на стадии создания, то функция вернет точку
    /// положение мыши. 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public COPoint GetCOPoint(int index) {

      if(this.COPoints.IsCreate) {
        return this.COPoints[index];
      } else if(this.MouseMoveCOPoint != null && this.COPoints.CountCOPointCreated == index) {
        return MouseMoveCOPoint;
      }
      return null;
    }
    #endregion

    #region protected internal override void OnPaint(Graphics g)
    protected internal override void OnPaint(Graphics g) {
      base.OnPaint(g);
      _copoints.OnCalculateAnchors();

      OnPaintMovementLine(g);
      OnPaintObject(g);

      if(this.ChartBox.ChartManager.SelectedFigure == this) {
        for(int i = 0; i < this.COPoints.Anchors.Length; i++) {
          this.PaintAnchor(g, i);
        }
      } else if(!this.COPoints.IsCreate) {
        for(int i = 0; i < this.COPoints.Count; i++) {
          if (this.COPoints[i] != null)
            this.PaintAnchor(g, i);
        }
      }
    }
    #endregion

    #region private void PaintAnchor(Graphics g, int i)
    private void PaintAnchor(Graphics g, int i) {
      int bs = 3;
      Rectangle rect = new Rectangle(this.COPoints.Anchors[i].X - bs, this.COPoints.Anchors[i].Y - bs, bs * 2, bs * 2);
      g.FillRectangle(_boxBrush, rect);
      g.DrawRectangle(_boxPen, rect);
    }
    #endregion

    #region protected virtual void OnPaintMovementLine(Graphics g)
    protected virtual void OnPaintMovementLine(Graphics g) {
      if(_mouseMoveCOPoint != null && _tempCOPointBegin != null) {
        /* Нажата кнопка мыши и идет передвижение */

        this.DrawLine(g, _mouseMoveCOPoint, _tempCOPointBegin);

      } 
      if(_mouseMoveCOPoint != null && !this.COPoints.IsCreate) {
        /* Не нажата кнопка мыши, но идет передвижение */

        if(this.COPoints.CountCOPointCreated - 1 >= 0) {
          this.DrawLine(g, this.COPoints[this.COPoints.CountCOPointCreated - 1], _mouseMoveCOPoint);
        }
      }
    }
    #endregion

    #region private void DrawLine(Graphics g, COPoint coPoint1, COPoint coPoint2)
    private void DrawLine(Graphics g, COPoint coPoint1, COPoint coPoint2) {
      _tempLinePen.Color = this.ChartBox.ChartManager.Style.ScaleForeColor;

      int x1 = this.ChartBox.GetX(coPoint1.BarIndex);
      int y1 = this.ChartBox.GetY(coPoint1.Price);

      int x2 = this.ChartBox.GetX(coPoint2.BarIndex);
      int y2 = this.ChartBox.GetY(coPoint2.Price);

      SolidBrush brush = new SolidBrush(_tempLinePen.Color);
      g.FillRectangle(brush, x2 - 1, y2 - 1, 3, 3);
      g.DrawLine(_tempLinePen, x1, y1, x2, y2);

    }
    #endregion

    protected virtual void OnPaintObject(Graphics g) { }

    #region protected internal override void OnMouseMove(MouseEventArgs e)
    protected internal override void OnMouseMove(MouseEventArgs e) {
      Point p = e.Location;
      COPoint coPoint = this.ChartBox.GetChartPoint(p);
      if (coPoint.Price < -10000000 || coPoint.Price > 10000000)
        return;

      _mouseMoveCOPoint = coPoint;
      if (_tempAnchorIndex > -1 && this.COPoints.IsCreate) {
        this.COPoints.Move(coPoint.BarIndex, coPoint.Price, _tempAnchorIndex);
      }
    }
    #endregion

    #region protected internal override void OnMouseDown(MouseEventArgs e)
    protected internal override void OnMouseDown(MouseEventArgs e) {
      Point p = e.Location;
      COPoint coPoint = this.ChartBox.GetChartPoint(p);
      if (coPoint.Price < -10000000 || coPoint.Price > 10000000)
        return;

      int anchorIndex = this.COPoints.GetAnchorIndex(p);

      if (!this.COPoints.IsCreate)
        anchorIndex = -1;

      _mouseMoveCOPoint = coPoint;
      _tempCOPointBegin = coPoint;
      _tempAnchorIndex = anchorIndex;
    }
    #endregion

    #region protected internal override void OnMouseUp(MouseEventArgs e)
    protected internal override void OnMouseUp(MouseEventArgs e) {
      Point p = e.Location;
      _mouseMoveCOPoint = null;
      _tempCOPointBegin = null;

      COPoint coPoint = this.ChartBox.GetChartPoint(p);
      if (coPoint.Price < -10000000 || coPoint.Price > 10000000)
        return;


      /* Фигура создана, значит производиться модификация или перемещение */
      if (this.COPoints.IsCreate) {
        if (_tempAnchorIndex > -1)
          this.COPoints.Move(coPoint.BarIndex, coPoint.Price, _tempAnchorIndex);
      } else {
        this.COPoints.Create(coPoint.BarIndex, coPoint.Price);
      }
      _tempAnchorIndex = -1;
    }
    #endregion
  }
}
