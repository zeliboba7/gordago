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
using System.Windows.Forms;

namespace Gordago.Analysis.Chart {
	public class ChartFigureTrades: ChartFigure {

    #region class ChartTrade
    class ChartTrade {
      private ITrade _trade;
      private int _indexTime;
      private Rectangle _bounds;
      private int _yRate, _yStop, _yLimit;

      public ChartTrade(ITrade trade, int indexTime) {
        _trade = trade;
        _indexTime = indexTime;
      }

      #region public Rectangle Bounds
      public Rectangle Bounds {
        get { return this._bounds; }
        set { this._bounds = value; }
      }
      #endregion

      #region public ITrade Trade
      public ITrade Trade {
        get { return _trade; }
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

    private ITrade[] _trades;

    private Font _font;
		private Color _foreColor, _borderColor, _backColor;
		private Color _buyColor, _sellColor,_stopColor, _limitColor;
		private Pen _buyPenLine, _sellPenLine, _stopPenLine, _limitPenLine, _borderPen;
    private Brush _foreBrush, _backBrush;
    private int _savedStyleId;

		private float[] _dashPatternOut = new float[]{8,4,3,4};
    

    private List<ChartTrade> _chartTrades;
    private int _savedTradesCount = -1, _savedCountBars = -100;
    private Point _cursorPosition;
    private ChartFigureComment _comment;
    private int _paintSize;
    private ChartTrade _chartTrade;

		public ChartFigureTrades(string name, ITrade[] trades):base(name){
			this.SetTrades(trades);
			this.Font = new Font("Microsoft Sans Serif", 7);
      _chartTrades = new List<ChartTrade>();
      _comment = new ChartFigureComment("");
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
    public Color StopColor{
			get{return this._stopColor;}
			set{
				this._stopColor = value;
				this._stopPenLine = new Pen(value);
				this._stopPenLine.DashStyle = DashStyle.Custom;
				this._stopPenLine.DashPattern = _dashPatternOut;
			}
		}
		#endregion

    #region public Color LimitColor
    public Color LimitColor{
			get{return this._limitColor;}
			set{this._limitColor = value;
				this._limitPenLine = new Pen(value);
				this._limitPenLine.DashStyle = DashStyle.Custom;
				this._limitPenLine.DashPattern = _dashPatternOut;
			}
		}
		#endregion

		#region public Color BorderColor
		public Color BorderColor{
			get{return this._borderColor;}
			set{this._borderColor = value;
				this._borderPen = new Pen(value);
			}
		}
		#endregion

		#region public Color BackColor
		public Color BackColor{
			get{return this._backColor;}
			set{this._backColor = value;
				this._backBrush = new SolidBrush(Color.FromArgb(200,value));
			}
		}
		#endregion

		#region public Color BuyColor
		public Color BuyColor{
			get{return this._buyColor;}
			set{this._buyColor = value;
				this._buyPenLine = new Pen(value);
				this._buyPenLine.DashStyle =  DashStyle.Custom;
				this._buyPenLine.DashPattern = _dashPatternOut;

			}
		}
		#endregion

		#region public Color SellColor
		public Color SellColor{
			get{return this._sellColor;}
			set{
				this._sellColor = value;
				this._sellPenLine = new Pen(value);
				this._sellPenLine.DashStyle =  DashStyle.Custom;
				this._sellPenLine.DashPattern = _dashPatternOut;
			}
		}
		#endregion

    #region private ChartStyle Style
    private ChartStyle Style {
      get { return this.ChartBox.ChartManager.Style; }
    }
    #endregion

    #region public void SetTrades(ITrade[] trades)
    public void SetTrades(ITrade[] trades){
			this._trades = trades;
			this.ReCalculateScale();
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
      if (_savedTradesCount != _trades.Length)
        changeCount = true;

      if (changeBars && !changeCount && _chartTrades.Count > 0) {
        ChartTrade chartTrade = _chartTrades[_chartTrades.Count - 1];
        int indexTime = bars.GetBarIndex(chartTrade.Trade.OpenTime);
        if (chartTrade.IndexTime == indexTime)
          changeBars = false;
      }

      if (changeBars || changeCount) {
        if (changeCount)
          _chartTrades.Clear();

        _savedTradesCount = _trades.Length;
        int j = 0;
        string sname = this.ChartBox.ChartManager.Symbol.Name;
        for (int i = 0; i < _trades.Length; i++) {
          ITrade trade = _trades[i];
          if (trade.OnlineRate.Symbol.Name == sname) {
            int indexTime = bars.GetBarIndex(trade.OpenTime);
            if (changeCount) {
              _chartTrades.Add(new ChartTrade(trade, indexTime));
            } else {
              _chartTrades[j].IndexTime = indexTime;
            }
            j++;
          }
        }
      }

      for (int i = 0; i < _chartTrades.Count; i++) {
        ChartTrade chartTrade = _chartTrades[i];
        this.PaintTrade(g, chartTrade);
      }
      if (_chartTrade != null) {
        ITrade trade = _chartTrade.Trade;
        int decdig = trade.OnlineRate.Symbol.DecimalDigits;

        List<string> sa = new List<string>();
        sa.Add((trade.TradeType == TradeType.Sell ? "sell":"buy") + " #" + trade.TradeId);
        sa.Add("Lots " + trade.Lots.ToString());
        sa.Add("Profit " + SymbolManager.ConvertToCurrencyString(trade.NetPL));

        if (trade.StopOrder != null) 
          sa.Add("Stop " + SymbolManager.ConvertToCurrencyString(trade.StopOrder.Rate, decdig) );
        
        if (trade.LimitOrder != null) 
          sa.Add("Limit " + SymbolManager.ConvertToCurrencyString(trade.LimitOrder.Rate, decdig));
        
        sa.Add(trade.OpenTime.ToShortDateString() + " " + trade.OpenTime.ToShortTimeString());
        _comment.Text = string.Join("\n", sa.ToArray());
        this.DrawComment(g, _comment, _cursorPosition.X, _cursorPosition.Y, ContentAlignment.MiddleCenter);
      }
    }

    #region private void PaintTrade(Graphics g, ChartTrade ct)
    private void PaintTrade(Graphics g, ChartTrade ct) {
      ct.Clear();

      int w = _paintSize;
      int h = _paintSize;

      int x = this.ChartBox.GetX(ct.IndexTime);

      int x2 = this.ChartBox.Width;

      bool select = _chartTrade == ct;

      ITrade trade = ct.Trade;
      Pen penTrade = trade.TradeType == TradeType.Sell ? _sellPenLine : _buyPenLine;
      Pen penLimit = _limitPenLine, penStop = _stopPenLine;
      if (select) {
        penTrade = (Pen)penTrade.Clone();
        penTrade.Width = 2;

        penLimit = (Pen)penLimit.Clone();
        penLimit.Width = 2;

        penStop = (Pen)penStop.Clone();
        penStop.Width = 2;
      }

      string text = trade.TradeType == TradeType.Sell ? "sell" : "buy";

      ct.YRate = this.ChartBox.GetY(ct.Trade.OpenRate);
      
      this.PaintLine(g, penTrade, ct.YRate, "#" + trade.TradeId + " " + text);

      if (trade.StopOrder != null) {
        ct.YStop = this.ChartBox.GetY(trade.StopOrder.Rate);
        this.PaintLine(g, penStop, ct.YStop, "#" + trade.TradeId + " stop");
      }

      if (trade.LimitOrder != null) {
        ct.YLimit = this.ChartBox.GetY(trade.LimitOrder.Rate);
        this.PaintLine(g, penLimit, ct.YLimit, "#" + trade.TradeId + " limit");
      }
    }
    #endregion

    #region private void PaintLine (Graphics g, Pen pen, int y, string text)
    private void PaintLine (Graphics g, Pen pen, int y, string text){
      
			g.DrawLine(pen, 0, y, this.ChartBox.Width, y);
      if (text.Length > 0) {
        SizeF sizef = g.MeasureString(text, this.Font, 10000);

        Rectangle rect = new Rectangle(1, y - 10, (int)sizef.Width+2, (int)sizef.Height-2);

        g.FillRectangle(_backBrush, rect);
        g.DrawString(text, this.Font, _foreBrush, rect.X+1, rect.Y-2);
      }
		}
		#endregion

    #region protected override void OnMouseMove(MouseEventArgs e)
    protected override void OnMouseMove(MouseEventArgs e) {
      Point p = e.Location;
      _cursorPosition = p;
      ChartTrade savedTrade = _chartTrade;
      _chartTrade = null;
      for (int i = 0; i < _chartTrades.Count; i++) {
        ChartTrade ct = _chartTrades[i];
        if (CheckY(p.Y, ct.YRate) || CheckY(p.Y, ct.YLimit) || CheckY(p.Y, ct.YStop)) {
          _chartTrade = ct;
          this.Invalidate();
          return;
        }
      }
      if (_chartTrade != savedTrade)
        this.Invalidate();
    }
    #endregion

    #region private bool CheckY(int yMouse, int y)
    private bool CheckY(int yMouse, int y) {
      if (yMouse >= y - 1 && yMouse <= y + 1)
        return true;
      return false;
    }
    #endregion
  }
	
}
