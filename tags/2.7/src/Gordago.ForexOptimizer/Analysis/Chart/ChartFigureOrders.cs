/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gordago.API;
using System.Collections.Generic;

namespace Gordago.Analysis.Chart {
  public class ChartFigureOrders : ChartFigure {

    #region class ChartOrder
    class ChartOrder {
      private IOrder _order;
      private int _indexTime;
      private Rectangle _bounds;
      private int _yRate, _yStop, _yLimit;

      public ChartOrder(IOrder order, int indexTime) {
        _order = order;
        _indexTime = indexTime;
      }

      #region public Rectangle Bounds
      public Rectangle Bounds {
        get { return this._bounds; }
        set { this._bounds = value; }
      }
      #endregion

      #region public IOrder Order
      public IOrder Order {
        get { return this._order; }
      }
      #endregion

      #region public int IndexTime
      public int IndexTime {
        get { return this._indexTime; }
        set { _indexTime = value; }
      }
      #endregion

      #region public int YRate
      public int YRate {
        get { return this._yRate; }
        set { _yRate = value; }
      }
      #endregion

      #region public int YStop
      public int YStop {
        get { return _yStop; }
        set { _yStop = value; }
      }
      #endregion

      #region public int YLimit
      public int YLimit {
        get { return _yLimit; }
        set { this._yLimit = value; }
      }
      #endregion

      #region public void Clear()
      public void Clear() {
        _yRate = _yLimit = _yStop = -100;
      }
      #endregion
    }
    #endregion

    private Font _font;
    private Color _foreColor, _borderColor, _backColor;
    private Color _buyColor, _sellColor, _stopColor, _limitColor;
    private Pen _buyPenLine, _sellPenLine, _stopPenLine, _limitPenLine, _borderPen;
    private Brush _foreBrush, _backBrush;
    private int _savedStyleId;

    private IOrder[] _orders;
    private float[] _dashPatternOut = new float[] { 8, 8, 3, 8 };


    private List<ChartOrder> _chartOrders;
    private int _savedOrdersCount = -1, _savedCountBars = -100;
    private Point _cursorPosition;
    private ChartFigureComment _comment;
    private int _paintSize;
    private ChartOrder _chartOrder;

    public ChartFigureOrders(string name, IOrder[] orders) : base(name) {
      this.SetOrders(orders);
      this.Font = new Font("Microsoft Sans Serif", 7);
      _comment = new ChartFigureComment("");
      _chartOrders = new List<ChartOrder>();
    }

    #region public Font Font
    public Font Font {
      get { return this._font; }
      set { this._font = value; }
    }
    #endregion

    #region public Color ForeColor
    public Color ForeColor {
      get { return this._foreColor; }
      set {
        this._foreColor = value;
        this._foreBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color StopColor
    public Color StopColor {
      get { return this._stopColor; }
      set {
        this._stopColor = value;
        this._stopPenLine = new Pen(value);
        this._stopPenLine.DashStyle = DashStyle.Custom;
        this._stopPenLine.DashPattern = _dashPatternOut;
      }
    }
    #endregion

    #region public Color LimitColor
    public Color LimitColor {
      get { return this._limitColor; }
      set {
        this._limitColor = value;
        this._limitPenLine = new Pen(value);
        this._limitPenLine.DashStyle = DashStyle.Custom;
        this._limitPenLine.DashPattern = _dashPatternOut;
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

    #region public Color BackColor
    public Color BackColor {
      get { return this._backColor; }
      set {
        this._backColor = value;
        this._backBrush = new SolidBrush(Color.FromArgb(200, value));
      }
    }
    #endregion

    #region public Color BuyColor
    public Color BuyColor {
      get { return this._buyColor; }
      set {
        this._buyColor = value;
        this._buyPenLine = new Pen(value);
        this._buyPenLine.DashStyle = DashStyle.Custom;
        this._buyPenLine.DashPattern = _dashPatternOut;

      }
    }
    #endregion

    #region public Color SellColor
    public Color SellColor {
      get { return this._sellColor; }
      set {
        this._sellColor = value;
        this._sellPenLine = new Pen(value);
        this._sellPenLine.DashStyle = DashStyle.Custom;
        this._sellPenLine.DashPattern = _dashPatternOut;
      }
    }
    #endregion

    #region private ChartStyle Style
    private ChartStyle Style {
      get { return this.ChartBox.ChartManager.Style; }
    }
    #endregion

    #region public void SetOrders(IOrder[] orders)
    public void SetOrders(IOrder[] orders) {
      this._orders = orders;
      this.Invalidate();
    }
    #endregion

    protected override void OnPaint(Graphics g) {
      IBarList bars = this.ChartBox.ChartManager.Bars;
      if (bars == null)
        return;

      if (_savedStyleId != this.Style.Id) {
        _savedStyleId = this.Style.Id;
        this.ForeColor = Style.ScaleForeColor;
        this.BackColor = Style.BackColor;
        this.SellColor = Style.SellOrderColor;
        this.BuyColor = this.Style.BuyOrderColor;
        this.StopColor = this.Style.StopOrderColor;
        this.LimitColor = this.Style.LimitOrderColor;
      }

      _comment.Text = "";

      _paintSize = Math.Max(ChartFigureBar.BAR_WIDTH[(int)this.ChartBox.ChartManager.Zoom], 8);

      int barsCount = bars.Count;
      bool changeBars = false, changeCount = false;
      if (Math.Abs(barsCount - _savedCountBars) >= 2)
        changeBars = true;

      _savedCountBars = barsCount;
      if (_savedOrdersCount != _orders.Length)
        changeCount = true;

      if (changeBars && !changeCount && _chartOrders.Count > 0) {
        ChartOrder chartOrder = _chartOrders[_chartOrders.Count - 1];
        int indexTime = bars.GetBarIndex(chartOrder.Order.Time);
        if (chartOrder.IndexTime == indexTime)
          changeBars = false;
      }

      if (changeBars || changeCount) {
        if (changeCount)
          _chartOrders.Clear();

        _savedOrdersCount = _orders.Length;
        int j = 0;
        string sname = this.ChartBox.ChartManager.Symbol.Name;
        for (int i = 0; i < _orders.Length; i++) {
          IOrder order = _orders[i];
          if (order.OnlineRate.Symbol.Name == sname && (order.OrderType == OrderType.EntryLimit || order.OrderType == OrderType.EntryStop)) {
            int indexTime = bars.GetBarIndex(order.Time);
            if (changeCount) {
              _chartOrders.Add(new ChartOrder(order, indexTime));
            } else {
              _chartOrders[j].IndexTime = indexTime;
            }
            j++;
          }
        }
      }

      for (int i = 0; i < _chartOrders.Count; i++) {
        ChartOrder chartOrder = _chartOrders[i];
        this.PaintOrder(g, chartOrder);
      }
      if (_chartOrder != null) {
        IOrder order = _chartOrder.Order;
        int decdig = order.OnlineRate.Symbol.DecimalDigits;

        List<string> sa = new List<string>();
        string text = (order.TradeType == TradeType.Sell ? "sell" : "buy") + " "+
          (order.OrderType == OrderType.EntryStop ? "stop" : "limit") + " #" + order.OrderId;
        
        sa.Add(text);
        sa.Add("Rate " + SymbolManager.ConvertToCurrencyString(order.Rate, decdig));
        sa.Add("Lots " + order.Lots.ToString());

        if (order.StopOrder != null)
          sa.Add("Stop " + SymbolManager.ConvertToCurrencyString(order.StopOrder.Rate, decdig));

        if (order.LimitOrder != null)
          sa.Add("Limit " + SymbolManager.ConvertToCurrencyString(order.LimitOrder.Rate, decdig));

        sa.Add(order.Time.ToShortDateString() + " " + order.Time.ToShortTimeString());
        _comment.Text = string.Join("\n", sa.ToArray());
        this.DrawComment(g, _comment, _cursorPosition.X, _cursorPosition.Y, ContentAlignment.MiddleCenter);
      }
    }

    #region private void PaintOrder(Graphics g, ChartOrder co)
    private void PaintOrder(Graphics g, ChartOrder co) {
      co.Clear();

      int w = _paintSize;
      int h = _paintSize;

      int x = this.ChartBox.GetX(co.IndexTime);

      int x2 = this.ChartBox.Width;

      bool select = _chartOrder == co;

      IOrder order = co.Order;
      Pen penTrade = order.TradeType == TradeType.Sell ? _sellPenLine : _buyPenLine;
      Pen penLimit = _limitPenLine, penStop = _stopPenLine;
      if (select) {
        penTrade = (Pen)penTrade.Clone();
        penTrade.Width = 2;

        penLimit = (Pen)penLimit.Clone();
        penLimit.Width = 2;

        penStop = (Pen)penStop.Clone();
        penStop.Width = 2;
      }

      string text = (order.TradeType == TradeType.Sell ? "sell" : "buy") + " " +
        (order.OrderType == OrderType.EntryStop ? "stop" : "limit");

      co.YRate = this.ChartBox.GetY(co.Order.Rate);

      this.PaintLine(g, penTrade, co.YRate, "#" + order.OrderId + " " + text);

      if (order.StopOrder != null) {
        co.YStop = this.ChartBox.GetY(order.StopOrder.Rate);
        this.PaintLine(g, penStop, co.YStop, "#" + order.OrderId + " stop");
      }

      if (order.LimitOrder != null) {
        co.YLimit = this.ChartBox.GetY(order.LimitOrder.Rate);
        this.PaintLine(g, penLimit, co.YLimit, "#" + order.OrderId + " limit");
      }
    }
    #endregion

    #region private void PaintLine (Graphics g, Pen pen, int y, string text)
    private void PaintLine(Graphics g, Pen pen, int y, string text) {

      g.DrawLine(pen, 0, y, this.ChartBox.Width, y);
      if (text.Length > 0) {
        SizeF sizef = g.MeasureString(text, this.Font, 10000);

        Rectangle rect = new Rectangle(1, y - 10, (int)sizef.Width + 2, (int)sizef.Height - 2);

        g.FillRectangle(_backBrush, rect);
        g.DrawString(text, this.Font, _foreBrush, rect.X + 1, rect.Y - 2);
      }
    }
    #endregion
    #region private bool CheckY(int yMouse, int y)
    private bool CheckY(int yMouse, int y) {
      if (yMouse >= y - 1 && yMouse <= y + 1)
        return true;
      return false;
    }
    #endregion

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) {
      Point p = e.Location;

      _cursorPosition = p;
      ChartOrder savedOrder = _chartOrder;
      _chartOrder = null;
      for (int i = 0; i < _chartOrders.Count; i++) {
        ChartOrder ct = _chartOrders[i];
        if (CheckY(p.Y, ct.YRate) || CheckY(p.Y, ct.YLimit) || CheckY(p.Y, ct.YStop)) {
          _chartOrder = ct;
          this.Invalidate();
          return;
        }
      }
      if (_chartOrder != savedOrder)
        this.Invalidate();
    }
  }

}
