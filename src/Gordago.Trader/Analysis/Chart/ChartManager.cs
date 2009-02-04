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
using System.IO;
using System.Text;
using System.Drawing.Imaging;
#endregion

namespace Gordago.Analysis.Chart {

	#region internal enum ChartManagerStatus
	internal enum ChartManagerStatus{
		None,
		ChangePosition,
    ChangeHorzScale,
    ChangeHeightBox,
		MouseMoveOnFigure,
		SelectFigure,
    AddObjectFromMouse,
    ObjectMove
	}
	#endregion

	/// <summary>
	/// Ценовой график.
	/// Может находиться в двух состояниях:
	/// без цен (баров) - тогда отрисовка по координатам;
	/// с ценами
	/// </summary>
  public class ChartManager : Control, IChartManager {

    #region Public Events
    public event EventHandler SelectedFigureChanged, SelectedFigureMouseChanged;
    public event EventHandler MouseTypeChanged;

    public event EventHandler PositionChanged;
    public event EventHandler ZoomChanged;
    public event EventHandler BarTypeChanged;
    public event EventHandler TimeFrameChanged;
    public event EventHandler GridVisibleChanged;
    public event EventHandler ChartShiftChanged;
    public event EventHandler PeriodSeparatorsChanged;

    public event EventHandler ChartBoxAdding;
    public event EventHandler ChartBoxRemoving;
    #endregion

    private IChartBox[] _boxes;

    private int _position = 0;
    private Point _prevmousepoint = new Point(0, 0);

    private ChartStyle _style;
    private ChartZoom _zoom;

    private bool _autodeletechartbox = true;
    private ISymbol _symbol;
    private ChartAnalyzer _analyzer;
    private ChartFigure _selectedfigure, _selectedFigureMouse;
    private ChartManagerStatus _status;
    private ChartFigureBarType _bartype;
    private bool _gridvisible = true, _periodSeparators = false;

    private IBarList _bars;

    private bool _chartshift = true;

    private ZoomValue[] _zoomValues;

    private ChartObject _mouseType;

    private Cursor _cursVCHorsScale;

    private BarIndexCacheManager _barIndexCacheManager;
    private IndicatorManager _imanager;

    private int _countTick;

    private int _chartBoxHeightChanged;

    private int _savedWidht, _savedHeight, _savedChartBoxCount;

    private ChartObjectManager _objectManager;
    private ChartPanelManager _panelManager;

    private ChartPanelList _panels;

    #region public ChartManager()
    public ChartManager() {

      _cursVCHorsScale = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.VCHorsScale));

      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

      this.TabStop = true;
      this.TabIndex = 0;

      _zoomValues = new ZoomValue[]{
																			 new ZoomValue(1, 32),
																			 new ZoomValue(2, 16),
																			 new ZoomValue(4, 8),
																			 new ZoomValue(8, 4),
																			 new ZoomValue(16, 2),
																			 new ZoomValue(32, 2)
																		 };

      _style = new ChartStyle();
      _zoom = ChartZoom.Larger;

      _boxes = new ChartBox[] { };
      _panels = new ChartPanelList(this);

      this.CreateChartBox();
      this.Focus();
    }
    #endregion

    #region public ChartPanelList ChartPanels
    public ChartPanelList ChartPanels {
      get {
        return this._panels;
      }
    }
    #endregion

    #region Public Property

    #region internal ZoomValue[] ZoomValues
    internal ZoomValue[] ZoomValues {
      get { return this._zoomValues; }
    }
    #endregion

    #region internal struct ZoomValue
    /// <summary>
    /// С учетом масштаба соответствующие константы для прорисовки
    /// </summary>
    internal struct ZoomValue {
      public int DeltaX;
      public int DeltaBarGrid;

      public ZoomValue(int deltaX, int deltaBarGrid) {
        this.DeltaX = deltaX;
        this.DeltaBarGrid = deltaBarGrid;
      }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
    }
    #endregion

    #region public int Position
    public int Position {
      get { return this._position; }
    }
    #endregion

    #region public ChartZoom Zoom
    public ChartZoom Zoom {
      get { return this._zoom; }
    }
    #endregion

    #region public ChartFigureBarType BarType
    public ChartFigureBarType BarType {
      get { return this._bartype; }
    }
    #endregion

    #region public bool GridVisible
    public bool GridVisible {
      get { return this._gridvisible; }
    }
    #endregion

    #region public bool PeriodSeparators
    /// <summary>
    /// Разделитель периодов
    /// </summary>
    public bool PeriodSeparators {
      get { return this._periodSeparators; }
    }
    #endregion

    #region public bool ChartShift
    public bool ChartShift {
      get { return this._chartshift; }
    }
    #endregion

    #region public IBarList Bars
    public IBarList Bars {
      get { return this._bars; }
    }
    #endregion

    #region public ChartAnalyzer Analyzer
    public ChartAnalyzer Analyzer {
      get { return this._analyzer; }
    }
    #endregion

    #region public ChartFigure SelectedFigure
    public ChartFigure SelectedFigure {
      get { return this._selectedfigure; }
      set {
        if (this._selectedfigure == value)
          return;


        if (value != null && !value.PropertyEnable) {
        } else {
          this._selectedfigure = value;
        }
        this.OnSelectedFigureChanged(this, new EventArgs());
        this.Invalidate();
      }
    }
    #endregion

    #region public ChartFigure SelectedFigureMouse
    public ChartFigure SelectedFigureMouse {
      get { return this._selectedFigureMouse; }
    }
    #endregion

    #region internal void SelectFigureMouse(ChartFigure figure)
    internal void SelectFigureMouse(ChartFigure figure) {
      this._selectedFigureMouse = figure;
    }
    #endregion

    #region public bool AutoDeleteEmptyChartBox
    public bool AutoDeleteEmptyChartBox {
      get { return this._autodeletechartbox; }
      set {
        this._autodeletechartbox = value;
      }
    }
    #endregion

    #region public ChartStyle Style
    public ChartStyle Style {
      get { return this._style; }
    }
    #endregion

    #region public ChartObject MouseType
    public ChartObject MouseType {
      get { return this._mouseType; }
    }
    #endregion

    #region public ChartAnalyzer ChartAnalyzer
    public ChartAnalyzer ChartAnalyzer {
      get { return _analyzer; }
    }
    #endregion
    #endregion

    #region public IChartBox[] ChartBoxes
    public IChartBox[] ChartBoxes {
      get { return _boxes; }
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose(bool disposing) {
      if (disposing) {
      }
      base.Dispose(disposing);
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this.Invalidate();
    }
    #endregion

    #region protected virtual void OnSelectedFigureChanged(object sender, EventArgs e)
    protected virtual void OnSelectedFigureChanged(object sender, EventArgs e) {
      if (this.SelectedFigureChanged != null)
        this.SelectedFigureChanged(sender, e);
    }
    #endregion

    #region protected virtual void OnMouseMoveOnFigure(object sender, EventArgs e)
    protected virtual void OnMouseMoveOnFigure(object sender, EventArgs e) {
      if (this.SelectedFigureMouseChanged != null)
        this.SelectedFigureMouseChanged(this, e);
    }
    #endregion

    #region public void SetTraderComponent(ChartPanelManager panelManager, ChartObjectManager objectManager, ISymbol symbol, IndicatorManager imanager, TimeFrame initTimeFrame)
    public void SetTraderComponent(ChartPanelManager panelManager, ChartObjectManager objectManager, ISymbol symbol, IndicatorManager imanager, TimeFrame initTimeFrame) {
      _panelManager = panelManager;
      _objectManager = objectManager;
      _barIndexCacheManager = new BarIndexCacheManager(symbol);
      _symbol = symbol;
      _imanager = imanager;
      this._analyzer = imanager.CreateChartAnalyzer(symbol);
      this.SetTimeFrame(initTimeFrame);
    }
    #endregion

    #region public void SetSymbol(ISymbol symbol)
    public void SetSymbol(ISymbol symbol) {
      _barIndexCacheManager = new BarIndexCacheManager(symbol);
      _symbol = symbol;
      this._analyzer = _imanager.CreateChartAnalyzer(symbol);
      if (this._bars != null) {
        IBarList bars = _bars;
        _bars = null;
        this.SetTimeFrame(bars.TimeFrame);
      }
      this.Invalidate();
    }
    #endregion

    #region public void SetTimeFrame(TimeFrame tf)
    public void SetTimeFrame(TimeFrame tf) {
      bool evt = false;

      evt = _bars == null ? true : _bars.TimeFrame.Second != tf.Second;

      long currentTime = 0;

      if (_bars != null) {
        int pos = Math.Min(_bars.Count - 1, this.Position);
        currentTime = _bars[pos].Time.Ticks;
      }


      this._bars = _symbol.Ticks.GetBarList(tf.Second);


      if (currentTime != 0)
        this.SetPosition(new DateTime(currentTime));
      else
        this.SetPositionToEnd();

      this.Analyzer.Cache.Clear();
      this.Analyzer.SetPosition(this._position, tf);

      foreach (ChartBox cbox in this._boxes) {
        cbox.PFScaleChanged = true;
        cbox.Analyzer = this._analyzer;
        cbox.SetBars(_bars);
      }
      if (evt)
        this.OnTimeFrameChanged(new EventArgs());
      this.Invalidate();
    }
    #endregion

    #region protected virtual void OnTimeFrameChanged(EventArgs e)
    protected virtual void OnTimeFrameChanged(EventArgs e) {
      if (this.TimeFrameChanged != null)
        this.TimeFrameChanged(this, e);
    }
    #endregion

    #region public void SetStyle(ChartStyle style)
    public void SetStyle(ChartStyle style) {
      this._style = style;
      this.Invalidate();
    }
    #endregion

    #region Methods - Position

    #region public void SetPosition(int position)
    public void SetPosition(int position) {

      if (this.Bars == null || this.Analyzer == null)
        return;

      foreach (ChartBox cbox in this._boxes)
        position = cbox.SetPosition(position);

      if (this.Analyzer.SetPosition(position, this.Bars.TimeFrame))
        this.Analyzer.Cache.Clear();
      bool evt = _position != position;
      this._position = position;

      if (evt)
        this.OnPositionChanged(new EventArgs());

      this.Invalidate();
    }
    #endregion

    #region public void SetPosition(DateTime time)
    public void SetPosition(DateTime time) {
      int position = this.Bars.GetBarIndex(time);
      this.SetPosition(position);
    }
    #endregion

    #region protected virtual void OnPositionChanged(EventArgs e)
    protected virtual void OnPositionChanged(EventArgs e) {
      if (this.PositionChanged != null)
        this.PositionChanged(this, e);
    }
    #endregion

    #region public void SetPositionToEnd()
    public void SetPositionToEnd() {
      if (this._bars == null) return;
      this.SetPosition(this._bars.Count);
    }
    #endregion
    #endregion

    #region Methods - Zoom

    #region public void SetZoom(ChartZoom zoom)
    public void SetZoom(ChartZoom zoom) {
      bool evt = _zoom != zoom;
      if ((int)zoom < (int)ChartZoom.Smaller)
        this._zoom = ChartZoom.Smaller;
      else if ((int)zoom > (int)ChartZoom.BigLarge)
        this._zoom = ChartZoom.BigLarge;
      else
        _zoom = zoom;

      foreach (ChartBox cbox in this._boxes) {
        cbox.PFScaleChanged = true;
      }
      if (evt)
        this.OnZoomChanged(new EventArgs());

      this.Invalidate();
    }
    #endregion

    #region protected virtual void OnZoomChanged(EventArgs e)
    protected virtual void OnZoomChanged(EventArgs e) {
      if (this.ZoomChanged != null)
        this.ZoomChanged(this, e);
    }
    #endregion

    #region public void SetZoomIn()
    public void SetZoomIn() {
      if (!IsZoomIn()) return;
      int sc = (int)this.Zoom;
      sc++;
      this.SetZoom((ChartZoom)sc);
    }
    #endregion

    #region public void SetZoomOut()
    public void SetZoomOut() {
      if (!IsZoomOut()) return;
      int scs = (int)this.Zoom;
      scs--;
      this.SetZoom((ChartZoom)scs);
    }
    #endregion

    #region public bool IsZoomIn()
    /// <summary>
    /// Возможность увеличения масштаба
    /// </summary>
    /// <returns></returns>
    public bool IsZoomIn() {
      return !((int)_zoom >= (int)ChartZoom.BigLarge);
    }
    #endregion

    #region public bool IsZoomOut()
    /// <summary>
    /// Возможность уменшения масштаба
    /// </summary>
    /// <returns></returns>
    public bool IsZoomOut() {
      return !((int)_zoom <= (int)ChartZoom.Smaller);
    }
    #endregion
    #endregion

    #region public void SetBarType(ChartFigureBarType bartype)
    public void SetBarType(ChartFigureBarType bartype) {
      bool evt = _bartype != bartype;
      _bartype = bartype;
      ChartFigureBar cfbar = (ChartFigureBar)this._boxes[0].Figures[0];
      cfbar.BarType = bartype;
      (this._boxes[0] as ChartBox).PFScaleChanged = true;
      if (evt)
        this.OnBarTypeChanged(new EventArgs());
      this.Invalidate();
    }
    #endregion

    #region protected virtual void OnBarTypeChanged(EventArgs e)
    protected virtual void OnBarTypeChanged(EventArgs e) {
      if (BarTypeChanged != null)
        this.BarTypeChanged(this, e);
    }
    #endregion

    #region public void SetGridVisible(bool value)
    public void SetGridVisible(bool value) {
      bool evt = _gridvisible != value;
      this._gridvisible = value;

      if (evt)
        this.OnGridVisibleChanged(new EventArgs());
      this.Invalidate();
    }
    #endregion

    #region public void SetPeriodSeparators (bool value)
    public void SetPeriodSeparators(bool value) {
      bool evt = _periodSeparators != value;
      this._periodSeparators = value;
      if (evt)
        this.OnPeriodSeparatorsChanged(new EventArgs());
      this.Invalidate();
    }
    #endregion

    #region protected virtual void OnGridVisibleChanged(EventArgs e)
    protected virtual void OnGridVisibleChanged(EventArgs e) {
      if (this.GridVisibleChanged != null)
        this.GridVisibleChanged(this, e);
    }
    #endregion

    #region protected virtual void OnPeriodSeparatorsChanged(EventArgs e)
    protected virtual void OnPeriodSeparatorsChanged(EventArgs e) {
      if (this.PeriodSeparatorsChanged != null)
        this.PeriodSeparatorsChanged(this, e);
    }
    #endregion

    #region public void SetChartShift(bool chartshift)
    public void SetChartShift(bool chartshift) {
      bool evt = _chartshift != chartshift;
      this._chartshift = chartshift;
      if (evt)
        this.OnChartShiftChanged(new EventArgs());
      this.SetPositionToEnd();
    }
    #endregion

    #region protected virtual void OnChartShiftChanged(EventArgs e)
    protected virtual void OnChartShiftChanged(EventArgs e) {
      if (this.ChartShiftChanged != null)
        this.ChartShiftChanged(this, e);
    }
    #endregion

    #region protected override void OnBackgroundImageChanged(EventArgs e)
    protected override void OnBackgroundImageChanged(EventArgs e) {
      //			base.OnBackgroundImageChanged (e);
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      if (_savedWidht != this.Width || _savedHeight != this.Height || _savedChartBoxCount != this._boxes.Length) {

        if (_savedChartBoxCount > this._boxes.Length) {

          int boxOverHeigh = 0;

          for (int i = 1; i < this._boxes.Length; i++) {
            ChartBox cbox = _boxes[i] as ChartBox;

            boxOverHeigh += cbox.BoxHeight;
          }

          int y = 0;
          for (int i = 0; i < this._boxes.Length; i++) {
            ChartBox cbox = _boxes[i] as ChartBox;

            int h = 0;
            if (i == 0) {
              h = this.Height - boxOverHeigh;
            } else {
              h = cbox.BoxHeight;
            }
            cbox.PFScaleChanged = true;
            cbox.HorizontalScaleVisible = i == this._boxes.Length - 1;
            cbox.SetBounds(0, y, this.Width, h);
            y += h;
          }

        } else {
          float percent = 0;
          for (int i = 0; i < this._boxes.Length; i++) {
            ChartBox cbox = _boxes[i] as ChartBox;

            if (cbox.PercentHeight == 0) {
              break;
            }
            percent += cbox.PercentHeight;
          }
          percent = Convert.ToInt32(percent);
          if (percent > 100) {
            float onepercent = 100 / _boxes.Length;
            for (int k = 0; k < _boxes.Length; k++)
              (_boxes[k] as ChartBox).PercentHeight = onepercent;
          }

          int y = 0;
          for (int i = 0; i < this._boxes.Length; i++) {
            ChartBox cbox = _boxes[i] as ChartBox;

            int h = Convert.ToInt32((float)this.Height / 100 * cbox.PercentHeight);
            cbox.PFScaleChanged = true;
            cbox.HorizontalScaleVisible = i == this._boxes.Length - 1;
            cbox.SetBounds(0, y, this.Width, h);
            y += h;
          }
        }

        this.SetPosition(this.Position);

        _savedWidht = this.Width;
        _savedHeight = this.Height;
        _savedChartBoxCount = this._boxes.Length;
      }

      Graphics g = e.Graphics;
      this.PaintBitmap(e.Graphics);
    }
    #endregion

    #region private void PaintBitmap(Graphics g)
    private void PaintBitmap(Graphics g) {
      try {

        bool isticksdatacaching = (this.Symbol != null) && (this.Symbol.Ticks as ITickManager).Status == TickManagerStatus.DataCaching;
        if (isticksdatacaching) {
          for (int i = 0; i < this._boxes.Length; i++) {
            ChartBox box = _boxes[i] as ChartBox;
            for (int k = 0; k < box.Figures.Count; k++) {
              if (box.Figures[k] is ChartFigureBar && box.Figures[k].Visible) {
                return;
              }
            }
          }
        }

        g.InterpolationMode = InterpolationMode.Low;
        g.SmoothingMode = SmoothingMode.None;
        g.CompositingMode = CompositingMode.SourceOver;
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

        g.Clear(_style.BackColor);


        int cnttick = this.Symbol == null ? 0 : this.Symbol.Ticks.Count;
        bool recalc = false;
        if (cnttick != _countTick) {
          recalc = true;
          _countTick = cnttick;
        }

        for (int i = 0; i < this._boxes.Length; i++) {
          if (recalc)
            (_boxes[i] as ChartBox).PFScaleChanged = true;

          (_boxes[i] as ChartBox).PaintMethod(g);
        }
      } catch { }
    }
    #endregion

    #region public override bool PreProcessMessage(ref Message msg)
    public override bool PreProcessMessage(ref Message msg) {
      if (msg.Msg == WMsg.WM_KEYDOWN) {
        switch ((int)msg.WParam) {
          case ProcessMsg.VK_RIGHT:
            this.SetPosition(this.Position + 1);
            return false;
          case ProcessMsg.VK_LEFT:
            this.SetPosition(this.Position - 1);
            return false;
          case ProcessMsg.VK_HOME:
            this.SetPosition(0);
            return false;
          case ProcessMsg.VK_END:
            this.SetPosition(this._bars.Count);
            return false;
          case ProcessMsg.VK_PAGEDOWN:
            this.SetPosition(this.Position + (this._boxes[0] as ChartBox).Map.Length);
            return false;
          case ProcessMsg.VK_PAGEUP:
            this.SetPosition(this.Position - (this._boxes[0] as ChartBox).Map.Length);
            return false;
          case ProcessMsg.VK_ADD:
            this.SetZoomIn();
            return false;
          case ProcessMsg.VK_SUB:
            this.SetZoomOut();
            return false;
          case ProcessMsg.VK_1:
            this.SetBarType(ChartFigureBarType.Line);
            return false;
          case ProcessMsg.VK_2:
            this.SetBarType(ChartFigureBarType.Bar);
            return false;
          case ProcessMsg.VK_3:
            this.SetBarType(ChartFigureBarType.CandleStick);
            return false;
          case ProcessMsg.VK_DEL:
            if (this.SelectedFigure != null) {
              this.SelectedFigure.ChartBox.Figures.Remove(this.SelectedFigure);
              // this.DeleteFigure(this.SelectedFigure);
            }
            return false;
        }
      }
      return base.PreProcessMessage(ref msg);
    }
    #endregion

    #region public int CreateChartBox()
    public int CreateChartBox() {
      ChartBox chartbox = new ChartBox(this);
      chartbox.Analyzer = _analyzer;
      chartbox.SetBars(this._bars);

      if (this._boxes.Length > 0) {
        chartbox.SetPosition(this._position);
        ChartBox cboxFirst = _boxes[0] as ChartBox;
        for (int i = 0; i < cboxFirst.Figures.Count; i++) {
          ChartFigure figure = cboxFirst.Figures[i];
          if (figure is ChartObjectVerticalLine) {
            ChartObjectVerticalLine vertline = figure as ChartObjectVerticalLine;
            ChartObjectVerticalLine newVertLine = new ChartObjectVerticalLine(vertline.Name);

            chartbox.Figures.Add(newVertLine);
          }
        }
      }

      List<IChartBox> list = new List<IChartBox>(_boxes);
      list.Add(chartbox);
      this._boxes = list.ToArray();

      if (this.ChartBoxAdding != null)
        this.ChartBoxAdding(chartbox, new EventArgs());
      //      _savedCountChartBox = 0;
      return this._boxes.Length - 1;
    }
    #endregion

    #region public void DeleteChartBox(int indexChartBox)
    public void DeleteChartBox(int indexChartBox) {
      if (indexChartBox == 0)
        throw (new Exception("It is impossible to remove the main ChartBox."));

      if (indexChartBox >= this._boxes.Length)
        throw (new Exception("The index of ChartBox out of range."));
      ChartBox cbox = this._boxes[indexChartBox] as ChartBox;

      ArrayList al = new ArrayList(this._boxes);
      al.RemoveAt(indexChartBox);
      this._boxes = (ChartBox[])al.ToArray(typeof(ChartBox));

      float onepercent = 100 / _boxes.Length;
      for (int i = 0; i < _boxes.Length; i++) {
        (_boxes[i] as ChartBox).PercentHeight = onepercent;
      }

      //      _savedCountChartBox = 0;
      if (this.ChartBoxRemoving != null)
        this.ChartBoxRemoving(cbox, new EventArgs());
    }
    #endregion

    #region public void DeleteEmptyChartBox()
    public void DeleteEmptyChartBox() {
      if (this._boxes.Length == 1)
        return;

      int cntbox = this._boxes.Length - 1;
      for (int i = cntbox; i > 0; i--) {
        int cnt = 0;
        for (int index = 0; index < _boxes[i].Figures.Count; index++) {
          ChartFigure figure = _boxes[i].Figures[index];
          if (figure is ChartFigureIndicator)
            cnt++;
        }
        if (cnt == 0)
          this.DeleteChartBox(i);
        cntbox = this._boxes.Length - 1;
      }
      //      _savedCountChartBox = 0;
    }
    #endregion

    #region public int GetIndexChartBox(Point p)
    public int GetIndexChartBox(Point p) {
      for (int i = 0; i < this._boxes.Length; i++) {
        ChartBox cbox = _boxes[i] as ChartBox;
        if (p.Y >= cbox.Top && p.Y <= cbox.Top + cbox.BoxHeight)
          return i;
      }
      return 0;
    }
    #endregion

    #region public void SetMouseType(ChartObject chartObject)
    public void SetMouseType(ChartObject chartObject) {
      if (chartObject == null) {
        for (int i = 0; i < this._boxes.Length; i++) {
          for (int j = 0; j < this._boxes[i].Figures.Count; j++) {
            ChartFigure figure = this._boxes[i].Figures[j];
            if (figure is ChartObject) {
              ChartObject co = figure as ChartObject;
              if (!co.COPoints.IsCreate) {
                this._boxes[i].Figures.Remove(figure);
                j--;
              }
            }
          }
        }
      }
      this.Cursor = Cursors.Default;
      bool evt = _mouseType != chartObject;

      _mouseType = chartObject;
      _status = ChartManagerStatus.AddObjectFromMouse;

      if (evt)
        this.OnMouseTypeChanged(new EventArgs());

      this.Invalidate();
    }
    #endregion

    #region protected virtual void OnMouseTypeChanged(EventArgs args)
    protected virtual void OnMouseTypeChanged(EventArgs args) {
      if (this.MouseTypeChanged != null)
        this.MouseTypeChanged(this, args);
    }
    #endregion

    #region private bool OjbectFromMouseMove(MouseEventArgs e)
    private bool OjbectFromMouseMove(MouseEventArgs e) {
      bool isrefresh = false;
      int indexChartBox;
      ChartBox cbox;
      Point pbox;
      COPoint chartPoint;

      pbox = new Point(e.X + 6, e.Y + 6);
      indexChartBox = this.GetIndexChartBox(pbox);
      cbox = this._boxes[indexChartBox] as ChartBox;
      pbox = cbox.PointToClient(pbox);
      chartPoint = cbox.GetChartPoint(cbox.PointToClient(pbox));
      if (chartPoint.Price < -10000000 || chartPoint.Price > 10000000)
        return false;

      if (_mouseType != null) {
        if (cbox.Figures.GetFigure(_mouseType) == null) {
          /* проверка, нет ли этой фигуры в других окнах */
          if (_mouseType.ChartBox != null) {
            /* Данная фигура уже принадлежит другому окну.
             * Если не произошло начало создание фигуры, 
             * то есть возможность начать строить данную 
             * фигуру в другом окне */
            if (_mouseType.MouseDownCOPoint != null || _mouseType.COPoints.CountCOPointCreated > 0) {

            } else {
              _mouseType.ChartBox.Figures.Remove(_mouseType);
              _mouseType = this._objectManager.Create(_mouseType.GetType().FullName);
              cbox.Figures.Add(_mouseType);
            }
          } else {
            cbox.Figures.Add(_mouseType);
          }
        }

        if (_mouseType.Cursor != null)
          this.Cursor = _mouseType.Cursor;
        if (_mouseType.ChartBox != null)
          _mouseType.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, pbox.X, pbox.Y, e.Delta));

        isrefresh = true;
      }
      return isrefresh;
    }
    #endregion

    #region protected override void OnMouseMove(MouseEventArgs e)
    protected override void OnMouseMove(MouseEventArgs e) {
      if (e.X < 5 || e.Y < 5 || e.X > this.Width - 5 || e.Y > this.Height - 5)
        return;

      bool isrefresh = false;
      int indexChartBox;
      ChartBox cbox;
      Point pbox;
      COPoint chartPoint;

      switch (_status) {
        #region case ChartManagerStatus.ObjectMove:
        case ChartManagerStatus.ObjectMove:
          if (!(this.SelectedFigure is ChartObject))
            break;

          indexChartBox = this.GetIndexChartBox(new Point(e.X, e.Y));
          cbox = this._boxes[indexChartBox] as ChartBox;
          Point cboxPoint = cbox.PointToClient(new Point(e.X, e.Y));
          chartPoint = cbox.GetChartPoint(cboxPoint);
          if (chartPoint.Price < -10000000 || chartPoint.Price > 10000000)
            return;
          (this.SelectedFigure as ChartObject).OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, cboxPoint.X, cboxPoint.Y, e.Delta));
          isrefresh = true;
          break;
        #endregion
        #region case ChartManagerStatus.AddObjectFromMouse:
        case ChartManagerStatus.AddObjectFromMouse:
          isrefresh = this.OjbectFromMouseMove(e);
          break;
        #endregion
        #region case ChartManagerStatus.None:
        case ChartManagerStatus.None:

          Point p = new Point(e.X, e.Y);
          indexChartBox = this.GetIndexChartBox(p);
          cbox = this._boxes[indexChartBox] as ChartBox;
          pbox = cbox.PointToClient(p);

          if (PointInHeighChangeLine(p) > 0) {
            this.Cursor = Cursors.HSplit;
          } else if (cbox.PointIsHorizontalScale(pbox)) {
            this.Cursor = _cursVCHorsScale;
          } else {
            this.Cursor = Cursors.Default;
            ChartFigure oldselmouse = this.SelectedFigureMouse;
            this.SelectFigureMouse(null);
            if (cbox.CheckPointInRect(pbox, 6, 6)) {
              for (int ii = 0; ii < cbox.Figures.Count; ii++) {
                ChartFigure figure = cbox.Figures[ii];
                figure.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, pbox.X, pbox.Y, e.Delta));
                if (figure != this.SelectedFigure) {
                  if (figure.CheckFigure(pbox)) {
                    this.SelectFigureMouse(figure);
                  } else {
                  }
                }
                if (figure.IsInvalidate)
                  isrefresh = true;
              }
            }
            if (oldselmouse != this.SelectedFigureMouse) {
              this.OnMouseMoveOnFigure(this, e);
              isrefresh = true;
            }
          }
          break;
        #endregion
        #region case ChartManagerStatus.ChangeHeightBox:
        case ChartManagerStatus.ChangeHeightBox:
          if (e.Button != MouseButtons.Left) {
            _status = ChartManagerStatus.None;
            break;
          }
          int my = _prevmousepoint.Y;
          int emy = e.Y;
          if (my == emy) break;
          int dy = emy - my;
          ChartBox cbox1 = _boxes[_chartBoxHeightChanged - 1] as ChartBox;
          ChartBox cbox2 = _boxes[_chartBoxHeightChanged] as ChartBox;
          if (((cbox1.Height < 50 && dy > 0) || cbox1.Height >= 50) &&
            ((cbox2.Height < 50 && dy < 0) || cbox2.Height >= 50)) {
            cbox1.SetBounds(cbox1.Left, cbox1.Top, cbox1.BoxWidth, cbox1.BoxHeight + dy);
            cbox2.SetBounds(cbox2.Left, cbox2.Top + dy, cbox2.BoxWidth, cbox2.BoxHeight - dy);

            /* рассчет процентов */
            for (int k = 0; k < _boxes.Length; k++) {
              ChartBox cb = _boxes[k] as ChartBox;
              cb.PercentHeight = (100F / (float)this.Height) * (float)cb.BoxHeight;
            }

            isrefresh = true;
            this._prevmousepoint = new Point(e.X, e.Y);
          }
          break;
        #endregion
        #region case ChartManagerStatus.ChangePosition:
        case ChartManagerStatus.ChangePosition:
        case ChartManagerStatus.ChangeHorzScale:
          if (e.Button != MouseButtons.Left) {
            _status = ChartManagerStatus.None;
            break;
          }

          int mx = _prevmousepoint.X;
          int emx = e.X;
          if (mx == emx) break;

          int deltaX = (this._boxes[0] as ChartBox).DeltaX;
          if (_status == ChartManagerStatus.ChangeHorzScale) {
            if (Math.Abs(mx - emx) < 10)
              break;
            if (mx < emx) {
              this.SetZoomIn();
            } else {
              this.SetZoomOut();
            }
          } else {

            int dx = 0;
            if (mx > emx) {
              dx = ((mx - emx) / deltaX + 1);
            } else if (mx < emx) {
              dx = -((emx - mx) / deltaX + 1);
            }

            int position = this.Position + dx * (Math.Abs(dx) < 4 ? 1 : 2);
            this.SetPosition(position);
          }
          this._prevmousepoint = new Point(e.X, e.Y);
          isrefresh = true;
          break;
        #endregion
      }
      if (isrefresh) this.Invalidate();
    }
    #endregion

    #region protected override void OnMouseDown(MouseEventArgs e)
    protected override void OnMouseDown(MouseEventArgs e) {

      if (e.Button != MouseButtons.Left) {
        if (this._mouseType != null) {
          this.SetMouseType(null);
        } else if (e.Button == MouseButtons.Middle) {
          this.SetMouseType(new ChartObjectCrosshair("FromMiddleButtom"));
          this.OjbectFromMouseMove(e);
        }
        return;
      }

      _prevmousepoint = new Point(e.X, e.Y);
      int i = GetIndexChartBox(_prevmousepoint);
      ChartBox cbox = _boxes[i] as ChartBox;
      Point pbox = cbox.PointToClient(_prevmousepoint);

      switch (_status) {
        #region case ChartManagerStatus.AddObjectFromMouse:
        case ChartManagerStatus.AddObjectFromMouse:
          Point pboxCurs = new Point(pbox.X + 6, pbox.Y + 6);
          COPoint chartPoint = cbox.GetChartPoint(pboxCurs);
          if (chartPoint.Price < -10000000 || chartPoint.Price > 10000000)
            return;

          if (_mouseType != null) {
            _mouseType.OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, chartPoint.X, chartPoint.Y, e.Delta));
            //            _mouseType.OnMouseDown(pboxCurs);
            this.Invalidate();
          }
          break;
        #endregion
        #region case ChartManagerStatus.None:
        case ChartManagerStatus.None:

          if (this.SelectedFigure != null && this.SelectedFigure is ChartObject) {

            /* есть фигура, которая выбрана и готова менять свое положение */

            ChartObject cobject = this.SelectedFigure as ChartObject;
            int indexAnchor = cobject.COPoints.GetAnchorIndex(pbox);
            if (indexAnchor > -1) {
              cobject.OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, pbox.X, pbox.Y, e.Delta));
              this._status = ChartManagerStatus.ObjectMove;
              this.Invalidate();
              return;
            }
          }


          if (this.SelectedFigureMouse != null) return;

          _chartBoxHeightChanged = this.PointInHeighChangeLine(_prevmousepoint);
          if (_chartBoxHeightChanged > 0) {
            this._status = ChartManagerStatus.ChangeHeightBox;
            this.Cursor = Cursors.HSplit;
          } else if (cbox.PointIsHorizontalScale(pbox)) {
            this._status = ChartManagerStatus.ChangeHorzScale;
            this.Cursor = _cursVCHorsScale;
          } else {
            this._status = ChartManagerStatus.ChangePosition;
            this.Cursor = Cursors.SizeWE;
          }
          break;
        #endregion
      }
    }
    #endregion

    #region protected override void OnMouseUp(MouseEventArgs e)
    protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);
      Point p = new Point(e.X, e.Y);
      ChartBox cbox = _boxes[GetIndexChartBox(p)] as ChartBox; ;
      Point pbox = cbox.PointToClient(p);

      switch (_status) {
        #region case ChartManagerStatus.AddObjectFromMouse:
        case ChartManagerStatus.AddObjectFromMouse:

          Point pboxCurs = new Point(pbox.X + 6, pbox.Y + 6);

          if (_mouseType != null) {
            _mouseType.OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, pboxCurs.X, pboxCurs.Y, e.Delta));
            bool isRemove = cbox.Figures.GetFigure(_mouseType) == null;
            if (_mouseType.COPoints.IsCreate || isRemove) {
              this.SetMouseType(null);
              _status = ChartManagerStatus.None;
            }
          }

          break;
        #endregion
        case ChartManagerStatus.ChangePosition:
          break;
        #region case ChartManagerStatus.ObjectMove:
        case ChartManagerStatus.ObjectMove:

          if (this.SelectedFigure is ChartObject) {

            cbox = this._boxes[this.GetIndexChartBox(new Point(e.X, e.Y))] as ChartBox;
            Point cboxPoint = cbox.PointToClient(new Point(e.X, e.Y));

            (this.SelectedFigure as ChartObject).OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, cboxPoint.X, cboxPoint.Y, e.Delta));
          }
          _status = ChartManagerStatus.None;
          this.Invalidate();
          break;
        #endregion
        case ChartManagerStatus.None:
          if (cbox.CheckPointInRect(pbox, 6, 6)) {
            for (int ii = 0; ii < cbox.Figures.Count; ii++) {
              ChartFigure figure = cbox.Figures[ii];
              figure.OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, pbox.X, pbox.Y, e.Delta));
            }
          }

          break;
      }
      this.Cursor = Cursors.Default;
    }
    #endregion

    #region protected override void OnMouseLeave(EventArgs e)
    protected override void OnMouseLeave(EventArgs e) {
      //bool isrefresh = false;
      //switch(_mouseType) {
      //  case ChartManagerMouseType.CreateVHLine:
      //    DeleteVertHorsLine();
      //    isrefresh = true;
      //    break;
      //}
      this.SelectFigureMouse(null);
      //foreach (ChartBox cbox in this._boxes){
      //  foreach (ChartFigure figure in cbox.Figures.Figures){
      //    if (figure.Status == ChartFigureStatus.MouseEnter){
      //      figure.SetStatus(ChartFigureStatus.Default);
      //      isrefresh = true;
      //    }
      //  }
      //}
      //if (isrefresh)

      if (_mouseType != null && _mouseType.COPoints.CountCOPointCreated == 0) {
        if (_mouseType.ChartBox != null)
          _mouseType.ChartBox.Figures.Remove(_mouseType);
      }

      this.Invalidate();
      base.OnMouseLeave(e);
    }
    #endregion

    #region protected override void OnMouseWheel(MouseEventArgs e)
    protected override void OnMouseWheel(MouseEventArgs e) {
      int cnt = (this._boxes[0] as ChartBox).Map.Length / 8;

      this.SetPosition(this.Position + e.Delta / 120 * cnt);

      base.OnMouseWheel(e);
    }
    #endregion

    #region protected override void OnDoubleClick(EventArgs e)
    protected override void OnDoubleClick(EventArgs e) {
      if (this.SelectedFigureMouse != null && this.SelectedFigureMouse.PropertyEnable) {
        this.SelectedFigure = this.SelectedFigureMouse;
      } else
        this.SelectedFigure = null;
      base.OnDoubleClick(e);
    }
    #endregion

    #region protected override void OnLostFocus(EventArgs e)
    protected override void OnLostFocus(EventArgs e) {
      this.SelectFigureMouse(null);
      base.OnLostFocus(e);
    }
    #endregion

    #region protected override void OnClick(EventArgs e)
    protected override void OnClick(EventArgs e) {
      if (!this.Focused)
        this.Focus();


    }
    #endregion

    #region public int GetBarIndex(DateTime time)
    public int GetBarIndex(DateTime time) {
      if (_barIndexCacheManager == null) return 0;
      return _barIndexCacheManager.GetBarIndex(this._bars.TimeFrame, time);
    }
    #endregion

    #region private int PointInHeighChangeLine(Point p)
    private int PointInHeighChangeLine(Point p) {
      if (_boxes.Length <= 1) return -1;

      for (int i = 1; i < _boxes.Length; i++) {
        ChartBox box = _boxes[i] as ChartBox;
        if (p.Y >= box.Top - 3 && p.Y <= box.Top + 3)
          return i;
      }
      return -1;
    }
    #endregion

    #region public void SaveValuesReport(string filename)
    /// <summary>
    /// Сохранить отчет значений графика по открытой области
    /// </summary>
    public void SaveValuesReport(string filename) {

      FileStream fs = new FileStream(filename, FileMode.CreateNew);
      TextWriter tw = new StreamWriter(fs, Encoding.GetEncoding("windows-1251"));

      tw.WriteLine("<html><head><title>Gordago Forex Optimizer TT</title>");
      tw.WriteLine("<meta http-equiv=Content-Type content='text/html; charset=windows-1251'>");
      tw.WriteLine("</head><body>");
      tw.WriteLine("<style type='text/css'>");
      tw.WriteLine("H1{FONT-WEIGHT: bold; FONT-SIZE: 14px; COLOR: #4669b1; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      tw.WriteLine("H2 {FONT-SIZE: 12px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      tw.WriteLine("H3 {FONT-SIZE: 14px; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      tw.WriteLine("H4 {FONT-SIZE: 12px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      tw.WriteLine("H5 {FONT-SIZE: 10px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      tw.WriteLine("H6 {FONT-SIZE: 8px; COLOR: #c7c7c7; FONT-FAMILY: Tahoma, Verdana, Arial, helvetica, sans-serif}");
      tw.WriteLine("A:visited {COLOR: #1f89b5; TEXT-DECORATION: underline}");
      tw.WriteLine("A:hover {COLOR: #FF5400; TEXT-DECORATION: underline}");
      tw.WriteLine("A:link {COLOR: #1f89b5; text-decoration:none}");
      tw.WriteLine("A:active {COLOR: #1f89b5; TEXT-DECORATION: none}");
      tw.WriteLine("</style>");

      tw.WriteLine("<h1>Gordago Forex Optimizer TT: Chart Report</h1>");

      tw.WriteLine(string.Format("<p>{0}({1})</p>", this.Symbol.Name, this.Bars.TimeFrame.Name));

      string thead = "<tr>";
      thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", "Date");
      thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", "Time");
      thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", "Open");
      thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", "Low");
      thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", "High");
      thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", "Close");

      int col = 6;

      for (int i = 0; i < this.ChartBoxes.Length; i++) {
        for (int k = 0; k < this.ChartBoxes[i].Figures.Count; k++) {
          ChartFigureIndicator find = this.ChartBoxes[i].Figures[k] as ChartFigureIndicator;
          if (find != null) {
            for (int j = 0; j < find.Indicator.Functions.Length; j++) {
              thead += string.Format("<td bgcolor='#E4EFFC'>{0}</td>", find.Indicator.Functions[j].ShortName);
              col++;
            }
          }
        }
      }
      thead += "</tr>";

      tw.WriteLine("<table border='0' cellpadding='1' cellspacing='1' bgcolor='#78B5FD'>");
      tw.WriteLine(string.Format("<tr><td bgcolor='#BBD6F7' colspan='{0}' align='center'><b>{1}</b></td></tr>", col, this.Symbol + " (" + this.Bars.TimeFrame.Name + ")"));
      tw.WriteLine(thead);

      #region Запись значений

      ChartBox ccbox = this.ChartBoxes[0] as ChartBox;
      int dg = this.Symbol.DecimalDigits;

      for (int index = 0; index < ccbox.Map.Length; index++) {
        int pos = this.Position + index;
        if (pos < this.Bars.Count) {
          Bar bar = this.Bars[pos];

          tw.WriteLine("<tr bgcolor='#FFFFFF'>");

          tw.WriteLine(string.Format("<td>{0}</td>", bar.Time.ToShortDateString()));
          tw.WriteLine(string.Format("<td>{0}</td>", bar.Time.ToShortTimeString()));
          tw.WriteLine(string.Format("<td>{0}</td>", SymbolManager.ConvertToCurrencyString(bar.Open, dg)));
          tw.WriteLine(string.Format("<td>{0}</td>", SymbolManager.ConvertToCurrencyString(bar.Low, dg)));
          tw.WriteLine(string.Format("<td>{0}</td>", SymbolManager.ConvertToCurrencyString(bar.High, dg)));
          tw.WriteLine(string.Format("<td>{0}</td>", SymbolManager.ConvertToCurrencyString(bar.Close, dg)));

          for (int i = 0; i < this.ChartBoxes.Length; i++) {
            for (int k = 0; k < this.ChartBoxes[i].Figures.Count; k++) {
              ChartFigureIndicator find = this.ChartBoxes[i].Figures[k] as ChartFigureIndicator;
              if (find != null) {
                for (int j = 0; j < find.Indicator.Functions.Length; j++) {
                  string val = "";
                  float fval = find.GetValue(j, index);
                  if (!float.IsNaN(fval)) {
                    val = SymbolManager.ConvertToCurrencyString(fval, dg);
                  }
                  tw.WriteLine(string.Format("<td>{0}</td>", val));
                }
              }
            }
          }
          tw.WriteLine("</tr>");
        }
      }

      #endregion

      tw.WriteLine("</table>");
      tw.WriteLine("<br><center><a href='http://www.gordago.com'>© Copyright 2005-2007 Gordago Software Ltd.</a></center>");
      tw.WriteLine("</body></html>");
      tw.Flush();
      fs.Close();
    }
    #endregion

    #region public void SaveBitmap(string filename, ImageFormat imageFormat)
    public void SaveBitmap(string filename, ImageFormat imageFormat) {
      Bitmap bitmap = new Bitmap(this.Width, this.Height);
      Graphics g = Graphics.FromImage(bitmap);
      this.PaintBitmap(g);
      Color color = this.Style.ScaleForeColor;
      Font font = this.Style.ScaleFont;
      g.DrawString("www.gordago.com, © 2004-2007 Gordago Software Ltd.", font, new SolidBrush(color), 10, this.Height - 33);

      if (imageFormat == ImageFormat.Gif) {
        try {
          ImageCodecInfo gif = null;
          foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders()) {
            if (codec.MimeType == "image/gif") {
              gif = codec;
              break;
            }
          }
          //2. Выставить параметры преобразования:
          EncoderParameters prms = new EncoderParameters(2);
          prms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 32L);
          prms.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

          bitmap.Save(filename, gif, prms);
        } catch {
          bitmap.Save(filename, imageFormat);
        }
      } else {
        bitmap.Save(filename, imageFormat);
      }
    }
    #endregion

  }

  #region class BarIndexCacheManager
  class BarIndexCacheManager {
    
    private ISymbol _symbol;

    private Dictionary<int, IDictionary> _cache;
    private int _prevCountTicks = 0;

    public BarIndexCacheManager(ISymbol symbol) {
      _symbol = symbol;
      _cache = new Dictionary<int, IDictionary>();
    }

    public int GetBarIndex(TimeFrame tf, DateTime dtm) {
      int cnttick = _symbol.Ticks.Count;
      if(Math.Abs(cnttick - _prevCountTicks) > 10) {
        _prevCountTicks = cnttick;
        _cache.Clear();
        return this.GetBarIndex(tf, dtm);
      }
      if(!_cache.ContainsKey(tf.Second)) {
        Dictionary<DateTime, int> row = new Dictionary<DateTime, int>();
        _cache.Add(tf.Second, row);
        return this.GetBarIndex(tf, dtm);
      }
      Dictionary<DateTime, int> retrow = (Dictionary<DateTime, int>)_cache[tf.Second];
      if(!retrow.ContainsKey(dtm)) {
        IBarList bars = _symbol.Ticks.GetBarList(tf.Second);
        int barIndex = bars.GetBarIndex(dtm);
        retrow.Add(dtm, barIndex);
        return barIndex;
      }
      return retrow[dtm];
    }
  }
  #endregion
}
