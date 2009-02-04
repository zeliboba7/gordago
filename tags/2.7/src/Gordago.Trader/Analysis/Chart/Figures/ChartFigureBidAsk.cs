/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Gordago.Analysis.Chart {
	public class ChartFigureBidAsk: ChartFigureBar {

		public const string FigureName = "__FigureBidAsk";
		private float _bid, _ask;
		
		private Color _bidcolor;
		private Brush _bidbrush;
		private Pen _bidpen;
		private Pen _bidlinepen;

		private Color _askcolor;
		private Brush _askbrush;
		private Pen _askpen;
		private Pen _asklinepen;

		private Color _forecolor;
		private Brush _forebrush;
    private StringFormat _sformat;

		private int _beginx = 0;
    private bool _viewAsk = false;

		public ChartFigureBidAsk():base(ChartFigureBidAsk.FigureName) {
      this.EnableScale = true;
			this.BidColor = Color.Red;
			this.AskColor = Color.White;
      this.ForeColor = Color.Black;

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;

		}

		#region public Color BidColor
		public Color BidColor{
			get{return this._bidcolor;}
			set{
				this._bidcolor = value;
				this._bidbrush = new SolidBrush(value);
				this._bidpen = new Pen(value);
				this._bidlinepen = new Pen(value);
				this._bidlinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			}
		}
		#endregion

		#region private Color AskColor
		private Color AskColor{
			get{return this._askcolor;}
			set{
				this._askcolor = value;
				this._askbrush = new SolidBrush(value);
				this._askpen = new Pen(value);
				this._asklinepen = new Pen(value);
				this._asklinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			}
		}
		#endregion

		#region public Color ForeColor
		public Color ForeColor{
			get{return this._forecolor;}
			set{this._forecolor = value;
				this._forebrush = new SolidBrush(value);
			}
		}
		#endregion

    #region public bool ViewAsk
    public bool ViewAsk {
      get { return this._viewAsk; }
      set { this._viewAsk = value; }
    }
    #endregion

    #region public void SetBidAsk(float bid, float ask)
    public void SetBidAsk(float bid, float ask){
			_bid = bid;
			_ask = ask;
			this.ReCalculateScale();
		}
		#endregion

    #region protected internal override void OnCalculateScale()
    protected internal override void OnCalculateScale() {
      //this.SetScaleValue(_bid);
      //this.SetScaleValue(_ask);
      _beginx = 0;
      IBarList bars = this.ChartBox.ChartManager.Bars;

      _beginx = bars.Count - this.ChartBox.ChartManager.Position-1;
      if(_beginx > this.Map.Length)
        _beginx = -1;
    }
    #endregion

    #region protected internal override void OnPaint(Graphics g)
    protected internal override void OnPaint(Graphics g) {
      if(_beginx < 0 || _beginx >= this.Map.Length) return;
      

      int x = this.Map[_beginx];
      int w = this.ChartBox.Width - x;
      int ybid = ChartBox.GetY(_bid);
      int yask = ChartBox.GetY(_ask);
      g.DrawLine(_bidlinepen, x, ybid, this.ChartBox.Width, ybid);
      
      if (this.ViewAsk)
        g.DrawLine(_asklinepen, x, yask, this.ChartBox.Width, yask);
    }
    #endregion

    #region protected internal override void OnPaintVScale(Graphics g)
    protected internal override void OnPaintVerticalScale(Graphics g) {
      if(g == null) return;

      int w = this.ChartBox.VerticalScaleSize.Width - 2;
      int h = 12;

      int ybid = ChartBox.GetY(_bid);

      g.DrawLine(_bidpen, 0, ybid, 2, ybid);
      g.FillRectangle(_bidbrush, 2, ybid - h / 2, w, 12);
      g.DrawString(
        SymbolManager.ConvertToCurrencyString(_bid, this.ChartBox.DecimalDigits, ""),
        this.ChartBox.ChartManager.Style.ScaleFont, _forebrush, new RectangleF(2, ybid - h / 2, w, h), _sformat);

      if(this.ViewAsk) {
        int yask = ChartBox.GetY(_ask);

        g.DrawLine(_askpen, 0, yask, 2, yask);
        g.FillRectangle(_askbrush, 2, yask - h / 2, w, 12);
        g.DrawString(
          SymbolManager.ConvertToCurrencyString(_ask, this.ChartBox.DecimalDigits, ""),
          this.ChartBox.ChartManager.Style.ScaleFont, _forebrush, new RectangleF(2, yask - h / 2, w, h), _sformat);
      }
    }
    #endregion

  }

}
