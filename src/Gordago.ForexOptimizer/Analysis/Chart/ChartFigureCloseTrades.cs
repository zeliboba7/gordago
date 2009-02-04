/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gordago.Strategy;
using Gordago.API;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gordago.Analysis.Chart {

  class ChartFigureCloseTrades: ChartFigure {

    #region class ChartCloseTrade
    class ChartCloseTrade {
      private IClosedTrade _closeTrade;
      private int _indexOpenTime, _indexCloseTime, _indexOrderTime;
      private Rectangle _rectOpen, _rectClose;
      private bool _openFromOrderFlag = false;
      private DateTime _orderTime;

      private string _commentOpen = "";

      public ChartCloseTrade(IClosedTrade closeTrade, int indexOpenTime, int indexCloseTime) {
        _closeTrade = closeTrade;
        _indexOpenTime = indexOpenTime;
        _indexCloseTime = indexCloseTime;

        string[] sa = closeTrade.OpenComment.Split('\n');
        _commentOpen = "";
        List<string> list = new List<string>();
        foreach (string s in sa) {
          if (s.IndexOf("#order#") > -1) {
            _openFromOrderFlag = true;
            string[] sa1 = s.Split('|');
            list.Add("Open from Order #" + sa1[1]);
            long time = Convert.ToInt64(sa1[2]) * 10000000L;
            _orderTime = new DateTime(time);
          } else {
            list.Add(s);
          }
        }
        _commentOpen = string.Join("\n", list.ToArray());
      }

      public bool OpenFromOrderFlag {
        get { return this._openFromOrderFlag; }
      }

      public DateTime OrderTime {
        get { return _orderTime; }
      }

      #region public int IndexOrderTime
      public int IndexOrderTime {
        get { return this._indexOrderTime; }
        set { _indexOrderTime = value; }
      }
      #endregion

      #region public string CommentOpen
      public string CommentOpen {
        get { return _commentOpen; }
      }
      #endregion

      #region public Rectangle RectOpen
      public Rectangle RectOpen {
        get { return this._rectOpen; }
        set { this._rectOpen = value; }
      }
      #endregion

      #region public Rectangle RectClose
      public Rectangle RectClose {
        get { return this._rectClose; }
        set { this._rectClose = value; }
      }
      #endregion

      #region public IClosedTrade CloseTrade
      public IClosedTrade CloseTrade {
        get { return _closeTrade; }
      }
      #endregion

      #region public int IndexOpenTime
      public int IndexOpenTime {
        get { return this._indexOpenTime; }
        set { _indexOpenTime = value; }
      }
      #endregion

      #region public int IndexCloseTime
      public int IndexCloseTime {
        get { return this._indexCloseTime; }
        set { this._indexCloseTime = value; }
      }
      #endregion

      #region public void SetIndex(int indexOpenTime, int indexCloseTime)
      public void SetIndex(int indexOpenTime, int indexCloseTime) {
        _indexOpenTime = indexOpenTime;
        _indexCloseTime = indexCloseTime;
      }
      #endregion
    }
    #endregion

    private Color _buyColor, _sellColor, _stopColor, _limitColor;
    private Pen _buyPenLine, _sellPenLine, _stopPenLine, _limitPenLine, _borderPen;
    private Brush _foreBrush, _backBrush, _sellBrush, _buyBrush;

    private int _savedStyleId;

    private float[] _dashPatternOut = new float[] { 8, 4, 3, 4 };


		private int _beginIndex = -1, _endIndex = -1;
    
    private IClosedTradeList _closeTradesAll;
    private int _savedCloseTradesAllCount=-1;
    private List<ChartCloseTrade> _chartCloseTrates;
    private int _savedPosition = -100;
    private int _savedCountBars = -100;
    private Point _cursorPosition;
    private ChartFigureComment _comment;
    private ContentAlignment _contentAlignment;
    private int _commentX, _commentY;
    private int _paintSize;
    private ChartCloseTrade _chartCloseTrade;
    private Pen _penStop, _penLimit;

		public ChartFigureCloseTrades(string name, IClosedTradeList closeTrades):base(name){
      _chartCloseTrates = new List<ChartCloseTrade>();
      this.SetCloseTrades(closeTrades);
			this.BuyColor = Color.Blue;
			this.SellColor = Color.Red;
      _comment = new ChartFigureComment("");
      _penStop = new Pen(Color.Red, 2);
      _penLimit = new Pen(Color.Green, 2);
    }

    #region public Color BuyColor
    public Color BuyColor {
      get { return this._buyColor; }
      set {
        this._buyColor = value;
        this._buyPenLine = new Pen(value);
        this._buyPenLine.DashStyle = DashStyle.Custom;
        this._buyPenLine.DashPattern = _dashPatternOut;
        this._buyBrush = new SolidBrush(Color.FromArgb(80, value));
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
        this._sellBrush = new SolidBrush(Color.FromArgb(80, value));
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

    #region public ChartStyle Style
    public ChartStyle Style {
      get { return ChartBox.ChartManager.Style; }
    }
    #endregion

    #region public void SetCloseTrades(IClosedTradeList closeTrades)
    public void SetCloseTrades(IClosedTradeList closeTrades) {
      this._closeTradesAll = closeTrades;
    }
    #endregion

    #region private Point PaintCloseTrade(Graphics g, bool isClosePaint, ChartCloseTrade cct)
    private Point PaintCloseTrade(Graphics g, bool isClosePaint, ChartCloseTrade cct) {
      IClosedTrade ct = cct.CloseTrade;
      DateTime dtime = isClosePaint ? ct.CloseTime : ct.OpenTime;
      float rate = isClosePaint ? cct.CloseTrade.CloseRate : cct.CloseTrade.OpenRate;
      int w = _paintSize;
      int h = _paintSize;
      int x = this.ChartBox.GetX(isClosePaint ? cct.IndexCloseTime : cct.IndexOpenTime);
      int y = this.ChartBox.GetY(rate);

      bool sell = ct.TradeType == TradeType.Sell;

      Brush dealbrush = sell ? _sellBrush : _buyBrush;
      Pen dealpen = sell ? _sellPenLine : _buyPenLine;

      Rectangle rect = new Rectangle(x - w / 2, y - h / 2, w, h);
      g.FillEllipse(dealbrush, rect);
      g.DrawArc(dealpen, rect, 0, 360);

      if (!isClosePaint) {
        cct.RectOpen = rect;
      } else {
        cct.RectClose = rect;
      }

      Point pc = _cursorPosition;
      if (pc.X >= rect.X && pc.Y >= rect.Y && pc.X <= rect.Right && pc.Y <= rect.Bottom) {
        int decdig = this.ChartBox.ChartManager.Symbol.DecimalDigits;

        List<string> sa = new List<string>();
        sa.Add((!isClosePaint ? "Open " : "Close ") + (sell ? "Sell" : "Buy") + " #" + Convert.ToString(ct.TradeId));
        sa.Add("Amount " + ct.Amount);
        sa.Add("Rate " + SymbolManager.ConvertToCurrencyString(rate, decdig, ""));
        sa.Add("Time " + dtime.ToShortTimeString());

        if (isClosePaint) {
          sa.Add("Profit " + SymbolManager.ConvertToCurrencyString(ct.NetPL, 2, " "));
          if (ct is Gordago.API.VirtualForex.ClosedTrade) {
            Gordago.API.VirtualForex.ClosedTrade closedTrade = ct as Gordago.API.VirtualForex.ClosedTrade;
            sa.Add("DrawDown " + closedTrade.GetDrawDownPoint().ToString());
            sa.Add("GrowUp " + closedTrade.GetGrowUpPoint().ToString());
          }
        }

        string comment = isClosePaint ? ct.CloseComment : cct.CommentOpen;
        if (comment.Length > 0)
          sa.Add(comment);

        _contentAlignment = ChartFigureComment.GetContentAlignmentOpposite(new Point(pc.X - rect.Left, pc.Y - rect.Top), new Size(w, h));
        _comment.Text = string.Join("\n", sa.ToArray());
        _commentX = x;
        _commentY = y;
        _chartCloseTrade = cct;
      }

      return new Point(x, y);
    }
    #endregion

    #region protected override void OnPaint(Graphics g)
    protected override void OnPaint(Graphics g) {
      IBarList bars = this.ChartBox.ChartManager.Bars;

      if (bars == null)
        return;

      if (_savedStyleId != this.Style.Id) {
        _savedStyleId = this.Style.Id;
        this.SellColor = Style.SellTradeColor;
        this.BuyColor = this.Style.BuyTradeColor;
        this.StopColor = this.Style.StopTradeColor;
        this.LimitColor = this.Style.LimitTradeColor;
      }

      _comment.Text = "";

      _paintSize = Math.Max(ChartFigureBar.BAR_WIDTH[(int)this.ChartBox.ChartManager.Zoom], 8);

      int barsCount = bars.Count;
      bool changeBars = false, changeCount = false;
      if (Math.Abs(barsCount - _savedCountBars) >= 2) 
        changeBars = true;
      
      _savedCountBars = barsCount;
      if (_savedCloseTradesAllCount != _closeTradesAll.Count)
        changeCount = true;

      if (changeBars && !changeCount && _chartCloseTrates.Count > 0) {
        ChartCloseTrade cct = _chartCloseTrates[_chartCloseTrates.Count - 1];
        int indexOpenTime = bars.GetBarIndex(cct.CloseTrade.OpenTime);
        int indexCloseTime = bars.GetBarIndex(cct.CloseTrade.CloseTime);
        if (cct.IndexOpenTime == indexOpenTime && cct.IndexCloseTime == indexCloseTime)
          changeBars = false;
      }

      if (changeBars || changeCount) {
        if (changeCount)
          _chartCloseTrates.Clear();

        _savedCloseTradesAllCount = _closeTradesAll.Count;
        int j = 0;
        string sname = this.ChartBox.ChartManager.Symbol.Name;
        for (int i = 0; i < _closeTradesAll.Count; i++) {
          IClosedTrade ct = _closeTradesAll[i];
          if (ct.SymbolName == sname) {
            int indexOpenTime = bars.GetBarIndex(ct.OpenTime);
            int indexCloseTime = bars.GetBarIndex(ct.CloseTime);
            if (changeCount) {
              ChartCloseTrade cct = new ChartCloseTrade(ct, indexOpenTime, indexCloseTime);
              if (cct.OpenFromOrderFlag) {
                cct.IndexOrderTime = bars.GetBarIndex(cct.OrderTime);
              }
              _chartCloseTrates.Add(cct);
            } else {
              _chartCloseTrates[j].SetIndex(indexOpenTime, indexCloseTime);
              if (_chartCloseTrates[j].OpenFromOrderFlag) {
                _chartCloseTrates[j].IndexOrderTime = bars.GetBarIndex(_chartCloseTrates[j].OrderTime);
              }
            }
            j++;
          }
        }
      }

      int position = this.ChartBox.ChartManager.Position;

      if (_savedPosition != position) {
        _savedPosition = position;
        _beginIndex = 0;
        _endIndex = _chartCloseTrates.Count;
      }

      bool fend = false;
      for (int i = _beginIndex; i < _endIndex; i++) {
        ChartCloseTrade ct = _chartCloseTrates[i];
        if (ct.IndexCloseTime < position - 10) {
          _beginIndex = i + 1;
        } else if (ct.IndexOpenTime > position + this.Map.Length + 10) {
          if (!fend) {
            _endIndex = i;
            fend = true;
          }
        } else {

          Point p1 = PaintCloseTrade(g, false, ct);
          Point p2 = PaintCloseTrade(g, true, ct);

          Pen pen = ct.CloseTrade.TradeType == TradeType.Sell ? _sellPenLine : _buyPenLine;
          g.DrawLine(pen, p1, p2);
        }

        if (ct.OpenFromOrderFlag) {
          int x = this.ChartBox.GetX(ct.IndexOrderTime);
          int y = this.ChartBox.GetY(ct.CloseTrade.OpenRate);

          bool sell = ct.CloseTrade.TradeType == TradeType.Sell;


          Brush dealbrush = sell ? _sellBrush : _buyBrush;
          Pen dealpen = sell ? _sellPenLine : _buyPenLine;


          int w = _paintSize;
          int h = _paintSize;

          Rectangle rect = new Rectangle(x - w / 4, y - h / 4, w / 2, h / 2);
          g.FillEllipse(dealbrush, rect);
          g.DrawArc(dealpen, rect, 0, 360);

          using (Pen penLine = new Pen(dealpen.Color)) {
            penLine.DashStyle = DashStyle.Dot;
            int x2 = this.ChartBox.GetX(ct.IndexOpenTime);
            g.DrawLine(penLine, x, y, x2, y);
          }
        }
      }


      if (_comment.Text.Length > 0) {

        int sizeSL = 20;
        int xStop = 0, xLimit = 0;
        int yStop = 0, yLimit = 0;

        if (_chartCloseTrade.CloseTrade.StopOrderRate > 0) {
          xStop = this.ChartBox.GetX(_chartCloseTrade.IndexOpenTime);
          yStop = this.ChartBox.GetY(_chartCloseTrade.CloseTrade.StopOrderRate);
          g.DrawLine(_penStop, xStop - sizeSL / 2, yStop, xStop + sizeSL / 2, yStop);
        }

        if (_chartCloseTrade.CloseTrade.LimitOrderRate > 0) {
          xLimit = this.ChartBox.GetX(_chartCloseTrade.IndexOpenTime);
          yLimit = this.ChartBox.GetY(_chartCloseTrade.CloseTrade.LimitOrderRate);
          g.DrawLine(_penLimit, xLimit - sizeSL / 2, yLimit, xLimit + sizeSL / 2, yLimit);
        }

        IClosedTrade ct = _chartCloseTrade.CloseTrade;

        float closeRate = ct.CloseRate;
        float limitRate = ct.LimitOrderRate;
        float stopRate = ct.StopOrderRate;
        bool closeStop = false, closeLimit = false;

        /* Определение критерия закрытия */
        if (ct.TradeType == TradeType.Sell) {
          if (stopRate > 0 && closeRate >= stopRate)
            closeStop = true;
          if (limitRate > 0 && closeRate <= limitRate)
            closeLimit = true;
        } else {
          if (stopRate > 0 && closeRate <= stopRate)
            closeStop = true;
          if (limitRate > 0 && closeRate >= limitRate)
            closeLimit = true;
        }


        if (closeLimit || closeStop) {

          int x1 = 0, y1 = 0;

          int x2 = this.ChartBox.GetX(_chartCloseTrade.IndexCloseTime);
          int y2 = this.ChartBox.GetY(_chartCloseTrade.CloseTrade.CloseRate);

          if (closeLimit) {
            x1 = xLimit + sizeSL / 2;
            y1 = yLimit;
          }
          if (closeStop) {
            x1 = xStop + sizeSL / 2;
            y1 = yStop;
          }
          if (x2 - x1 > 0) {
            using (Pen pen = closeLimit ? (Pen)_penLimit.Clone() : (Pen)_penStop.Clone()) {
              pen.DashStyle = DashStyle.DashDotDot;
              g.DrawLine(pen, x1, y1, x2, y2);
            }
          }
        }

        this.DrawComment(g, _comment, _commentX, _commentY, _contentAlignment);
      }
    }
    #endregion


    #region protected override void OnMouseMove(MouseEventArgs e)
    protected override void OnMouseMove(MouseEventArgs e) {
      Point p = e.Location;
      _cursorPosition = p;

      for (int i = _beginIndex; i < _endIndex; i++) {
        ChartCloseTrade ct = _chartCloseTrates[i];
        Rectangle rect = ct.RectOpen;
        if (p.X >= rect.X && p.Y >= rect.Y && p.X <= rect.Right && p.Y <= rect.Bottom) {
          this.Invalidate();
          return;
        }
        rect = ct.RectClose;
        if (p.X >= rect.X && p.Y >= rect.Y && p.X <= rect.Right && p.Y <= rect.Bottom) {
          this.Invalidate();
          return;
        }
      }
    }
    #endregion
  }
	
}
