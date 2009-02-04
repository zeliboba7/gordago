/**
* @version $Id: ChartControl.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{

  #region using
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.Collections;
  using System.ComponentModel;
  using System.Drawing;
  using System.ComponentModel.Design;
  using System.Drawing.Design;
  using Gordago.Design;
  using System.Diagnostics;
  using Gordago.Trader.Indicators;
  #endregion

  /* Придется использовать Win32 API. Сначала получает контекст устройства (DC) 
   * того окна, на котором будем рисовать. Если рисуем на форму, тогда
   * HDC hDC = GetDC(WindowHandle);
   * 
   * Далее, зададим режим преобразования координат, в котором имеем возможность 
   * использовать различные масштабы по осям
   * SetMapMode(hDC, MM_ANISOTROPIC);
   * 
   * Затем нужно определить координаты для Viewport, т.е. тот прямоугольник
   * внутри DC, куда будет рисоваться график. Кординаты прямоугольника в пикселях.
   * SetViewportOrgEx(hDC, 0, 0, NULL);
   * SetViewportExtEx(hDC, Width, Height, NULL);
   * 
   * Теперь задаем собственно систему координат. Если в пред. случае координаты 
   * были физическими, то здесь они уже логические. Для ясности предположим, что 
   * нам нужно задать систему координат, имеющую 20 логических единиц по оси Х (
   * направлена вправо) и 10 — по оси Y (направлена вверх) с центром, совпадающим 
   * с центром Viewportа:
   * SetWindowOrgEx(hDC, -10, 5, NULL);
   * SetWindowExtEx(hDC, 20, -10, NULL);
   * 
   * Здесь неоходимы некоторые пояснения: SetWindowOrgEx задает координаты левого
   * верхнего угла в задаваемой СК. В SetWindowExtEx высота отрицательна, значит 
   * логическая ось Х направлена противоположно оси физической.
   * 
   * Дальше, все в ваших руках, но с одним "НО". Функции API для рисования в 
   * контексте принимают целочисленные логические кординаты, в отличие от VB, 
   * где используется тип Single. Следовательно, о преобразованиях придется 
   * позаботиться вам.
   * 
   * Если нужно нарисовать, к примеру, график у = х, при х = 0..5, делаем так:
   * MoveToEx(hDC, 0, 0, NULL);
   * LineTo(hDC, 5, 5);
   */
  [ToolboxBitmap(typeof(ChartControl), "Gordago.Charting.chart.png")]
  [DefaultProperty("ChartBoxes"), DefaultEvent("SelectedIndexChanged")]
  public partial class ChartControl : Control {

    public event EventHandler TimeFrameChanged;
    public event EventHandler ZoomChanged;
    public event EventHandler PeriodSeparatorsChanged;
    public event EventHandler ChartShiftChanged;
    public event EventHandler BarTypeChanged;
    public event EventHandler GridVisibleChanged;

    public event ChartPanelEventHandler ChartPanelAdded;
    public event ChartPanelEventHandler ChartPanelRemoved;

    private readonly object _locked = new object();
    private bool _isPainting = false;

    private StyleManager _styleManager = new StyleManager();

    private int _savedStyleSessionId = -1;
    private ISymbol _symbol;

    private readonly ChartGraphics _graphics = new ChartGraphics();
    private TimeFrame _timeFrame;
    private IBarsData _barsData = null;
    private iBars _iBarsCurrent;
    private Dictionary<int, iBars> _iBarsCollection = new Dictionary<int, iBars>();

    public ChartControl() {
      _chartPanelCollection = new ChartPanelCollection(this);

      base.SetStyle(ControlStyles.StandardDoubleClick | ControlStyles.UserMouse | ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.Opaque | ControlStyles.UserPaint, true);
      this.OnChartBoxesLayout();

      _timeFrame = TimeFrameManager.TimeFrames.GetTimeFrame(3600);

      if (_timeFrame == null)
        _timeFrame = TimeFrameManager.TimeFrames[TimeFrameManager.TimeFrames.Count / 2];
    }

    #region public ISymbol Symbol
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ISymbol Symbol {
      get { return _symbol; }
      set {
        this._symbol = value;
        _barsData = null;
        _iBarsCurrent = null;
        _iBarsCollection.Clear();
      }
    }
    #endregion

    #region public TimeFrame TimeFrame
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TimeFrame TimeFrame {
      get { return _timeFrame; }
      set {
        _timeFrame = value;
        _iBarsCurrent = null;
        _barsData = null;
      }
    }
    #endregion

    #region public IBarsData Bars
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public IBarsData Bars {
      get {
        if (_barsData == null) {
          if (_symbol == null || _timeFrame == null)
            return null;
          _barsData = _symbol.Ticks.BarsDataList[_timeFrame];
        }
        return _barsData;
      }
    }
    #endregion

    #region internal iBars iBars
    internal iBars iBars {
      get {
        if (_iBarsCurrent == null) {
          if (_symbol == null || _timeFrame == null)
            return null;
          _iBarsCollection.TryGetValue(_timeFrame.Second, out _iBarsCurrent);
          if (_iBarsCurrent == null) {
            _iBarsCurrent = new iBars(this.Bars);
            _iBarsCollection.Add(_timeFrame.Second, _iBarsCurrent);
          }
        }
        return _iBarsCurrent;
      }
    }
    #endregion

    #region public StyleManager Styles
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public StyleManager Styles {
      get { return _styleManager; }
      set { this._styleManager = value; }
    }
    #endregion

    #region internal ChartGraphics ChartGraphics
    internal ChartGraphics ChartGraphics {
      get { return _graphics; }
    }
    #endregion

    #region internal List<ChartBox> Boxes
    internal List<ChartBox> Boxes {
      get { return this._boxes; }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {

      if (_boxes.Count == 0) {
        e.Graphics.Clear(this.BackColor);
        using (Brush brush = new SolidBrush(Color.Red)) {
          e.Graphics.DrawString("It is neccessary to add ChartPanel (ChartControl.ChartPanels.Add(ChartPanel))", this.Font, brush, new RectangleF(0, 0, this.Width, this.Height));
        }
        return;
      }
      lock (this._locked) {
        _isPainting = true;

        #region Comment
        //this._cGraph.BeginPaint(e.Graphics, this.Width, this.Height);
        //long beginTime = DateTime.Now.Ticks;
        //long count;

        //try {

        //  int w = this.Width;
        //  int h = this.Height;
        //  bool flag = false;
        //  GdiPen penRed = _cGraph.SelectPen(Color.Red);
        //  GdiPen penBlue = _cGraph.SelectPen(Color.Blue);

        //  for (int i = 0; i < w; i++) {
        //    _cGraph.SelectPen(flag ? penRed : penBlue);
        //    flag = !flag;
        //    _cGraph.DrawLine(i, 0, i, h);
        //  }

        //} finally {
        //  _cGraph.EndPaint();
        //}

        //count = (DateTime.Now.Ticks - beginTime) / 1000L;
        //e.Graphics.DrawString(count.ToString(), this.Font, new SolidBrush(Color.White), 10, 10);
        #endregion

        this._graphics.BeginPaint(e.Graphics, this.Width, this.Height);

        /* Без этого не работает !!!!! %) */
        Rectangle clientRect = this.ClientRectangle;
        IntPtr rgn = _graphics.SaveClip(clientRect);
        _graphics.RestoreClip(rgn);
        /* * * * * * * * * * * * * * * */

        try {
          for (int i = 0; i < this._listChartPanel.Count; i++) 
            _listChartPanel[i].Paint(_graphics);

          #region Comment
          //_cGraph.DrawLine(0, 0, this.Width, this.Height);

          //_graphics.FillRectangleExt(new GdiBrush(Color.Red), clientRect);
          // _graphics.FillRectangleExt(new GdiBrush(Color.Green), clientRect);

          //IntPtr rgn = IntPtr.Zero;
          //rgn = _graphics.SaveClip(clientRect);
          /* Регион 10.10.50.50 не закрашивает в зеленый цвет */
          //          _graphics.ExcludeClipRect(10, 10, 50, 50);
          //_graphics.RestoreClip(rgn);

          //rgn = _graphics.SaveClip(clientRect);
          //_graphics.IntersectClipRect(new Rectangle(100, 100, 200, 200));
          //_graphics.FillRectangleExt(new GdiBrush(Color.Yellow), clientRect);
          //_graphics.DrawLineExt(new GdiPen(Color.Black), 25, 25, 250, 250);
          //_graphics.RestoreClip(rgn);


          /* Установка окна для рисования */
          //Rectangle winRect = new Rectangle(400, 400, 100, 100);
          //_graphics.SetViewport(winRect);
          //rgn = _graphics.SaveClip(winRect);
          //_graphics.IntersectClipRect(0, 0, winRect.Width, winRect.Height);
          //_graphics.FillRectangleExt(new GdiBrush(Color.Black), 0, 0, 150, 150);
          //_graphics.FillRectangleExt(new GdiBrush(Color.Silver), 10, 10, 10, 10);
          //_graphics.DrawRectangleExt(new GdiPen(Color.Red), 0, 0, winRect.Width - 1, winRect.Height - 1);
          //_graphics.RestoreClip(rgn);
          //_graphics.SetViewport(clientRect);

          //_graphics.DrawRectangleExt(new GdiPen(Color.Yellow), 10, 500, 100, 100);


          ///* Установка окна для рисования */
          //winRect = new Rectangle(100, 400, 100, 100);
          //_graphics.SetViewport(winRect);
          //rgn = _graphics.SaveClip(winRect);
          //_graphics.IntersectClipRect(0, 0, winRect.Width, winRect.Height);
          //_graphics.FillRectangleExt(new GdiBrush(Color.Black), 0, 0, 150, 150);
          //_graphics.FillRectangleExt(new GdiBrush(Color.Silver), 10, 10, 10, 10);
          //_graphics.DrawRectangleExt(new GdiPen(Color.Red), 0, 0, winRect.Width - 1, winRect.Height - 1);
          //_graphics.RestoreClip(rgn);
          //_graphics.SetViewport(clientRect);

          //_graphics.DrawRectangleExt(new GdiPen(Color.Yellow), 500, 500, 100, 100);


          /* Перемещение ранее нарисованной области */
          // _graphics.SetWorldTransform(100, 100, 1f, 1f);


          /* Рисуем картинку с координатами, а потом ее перемещаем */
          //Rectangle myRect = new Rectangle(0, 0, 50, 50);
          //rgn = _graphics.SaveClip(myRect);
          //_graphics.IntersectClipRect(myRect);
          //_graphics.FillRectangleExt(new GdiBrush(Color.White), 0, 0, 100, 100);
          //_graphics.SetWorldTransform(-5, -130, 1f, 1f);
          //_graphics.RestoreClip(rgn);



          //_graphics.IntersectClipRect(0, 0, 100, 100);
          //_graphics.DrawLineExt(new GdiPen(Color.Black), 25, 25, 250, 250);
          //_graphics.FillRectangleExt(new GdiBrush(Color.Blue), 0, 0, 100, 100);
          #endregion

        } finally {
          this._graphics.EndPaint();
        }
        _isPainting = false;
      }
    }
    #endregion
  }
}
