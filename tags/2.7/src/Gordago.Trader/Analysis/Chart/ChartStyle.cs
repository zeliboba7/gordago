/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Gordago.Analysis.Chart {

	public class ChartStyle {

    public event EventHandler StyleChangedEvent;

    private Color _barColor, _barUpColor, _barDownColor;
    private Color _sellTradeColor, _buyTradeColor, _stopTradeColor, _limitTradeColor;
    private Color _sellOrderColor, _buyOrderColor, _stopOrderColor, _limitOrderColor;
    private Color _backcolor, _gridcolor, _bordercolor;

		private Color _scaleforecolor;
		private Font _scalefont;

		private string _name = "Custom";

    private int _editId;

		public ChartStyle() {
			this.BackColor = Color.FromArgb(202,247,202);
			this.GridColor = Color.FromArgb(80, 0,0,205);
			this.BorderColor = Color.FromArgb(0,0,255);
			this.ScaleForeColor = Color.FromArgb(0,0,255);
			this.ScaleFont = new Font("Microsoft Sans Serif", 7);
      this.BarColor = Color.FromArgb(148, 0, 211);
      this.BarUpColor = Color.FromArgb(255, 255, 100);
      this.BarDownColor = Color.FromArgb(175, 151, 225);

      this.SellTradeColor = Color.FromArgb(-29556);
      this.BuyTradeColor = Color.FromArgb(-6967297);
      this.StopTradeColor = Color.FromArgb(-8388608);
      this.LimitTradeColor = Color.FromArgb(-15045594);

      this.SellOrderColor = Color.FromArgb(-29556);
      this.BuyOrderColor = Color.FromArgb(-6967297);
      this.StopOrderColor = Color.FromArgb(-8388608);
      this.LimitOrderColor = Color.FromArgb(-15045594);

      Random rnd = new Random();
      _editId = rnd.Next(-100000, 100);
		}

		#region public string Name
    [Browsable(false)]
		public string Name{
			get{return this._name;}
			set{this._name = value;}
		}
		#endregion

    #region public int Id
    /// <summary>
    /// Идентификатор стиля (что то на подобие контрольной суммы)
    /// </summary>
    [Browsable(false)]
    public int Id {
      get { return _editId; }
    }
    #endregion

    #region public Color BackColor
    [Category("Chart"), DisplayName("Back Color")]
		public Color BackColor{
			get{return this._backcolor;}
			set{
        bool evt = this._backcolor != value;

				this._backcolor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());
			}
		}
		#endregion

		#region public Color GridColor
    [Category("Chart"), DisplayName("Grid Color")]
		public Color GridColor{
			get{return this._gridcolor;}
			set{
        bool evt = this._gridcolor != value;
        this._gridcolor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());
			}
		}
		#endregion


		#region public Color BorderColor
    [Category("Chart"), DisplayName("Border Color")]
    public Color BorderColor {
			get{return this._bordercolor;}
			set{
        bool evt = this._bordercolor != value;

				this._bordercolor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());
			}
		}
		#endregion

		#region public Color ScaleForeColor
    [Category("Chart"), DisplayName("Scale Fore Color")]
    public Color ScaleForeColor {
			get{return this._scaleforecolor;}
			set{
        bool evt = this._scaleforecolor != value;

				this._scaleforecolor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());
      }
		}
		#endregion

		#region public Font ScaleFont
    [Category("Chart"), DisplayName("Scale Font")]
    [Browsable(false)]
    [XmlIgnore]
    public Font ScaleFont {
			get{return this._scalefont;}
			set{this._scalefont = value;}
		}
		#endregion

    #region public Color BarColor
    [Category("Bar"), DisplayName("Bar Color")]
    public Color BarColor {
      get { return this._barColor; }
      set {
        bool evt = this._barColor != value;

        this._barColor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());

      }
    }
    #endregion

    #region public Color BarUpColor
    [Category("Bar"), DisplayName("Bar Up Color")]
    public Color BarUpColor {
      get { return this._barUpColor; }
      set {
        bool evt = this._barUpColor != value;

        this._barUpColor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());
      }
    }
    #endregion

    #region public Color BarDownColor
    [Category("Bar"), DisplayName("Bar Down Color")]
    public Color BarDownColor {
      get { return this._barDownColor; }
      set {
        bool evt = this._barDownColor != value;

        this._barDownColor = value;
        if(evt)
          this.OnStyleChanged(new EventArgs());
      }
    }
    #endregion

    [Category("Trade"), DisplayName("Sell Color")]
    public Color SellTradeColor {
      get { return this._sellTradeColor; }
      set {
        bool evt = this._sellTradeColor != value;

        this._sellTradeColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }

    [Category("Trade"), DisplayName("Buy Color")]
    public Color BuyTradeColor {
      get { return this._buyTradeColor; }
      set {
        bool evt = this._buyTradeColor != value;
        this._buyTradeColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }


    [Category("Trade"), DisplayName("Stop Color")]
    public Color StopTradeColor {
      get { return this._stopTradeColor; }
      set {
        bool evt = this._stopTradeColor != value;
        this._stopTradeColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }

    [Category("Trade"), DisplayName("Limit Color")]
    public Color LimitTradeColor {
      get { return this._limitTradeColor; }
      set {
        bool evt = this._limitTradeColor != value;
        this._limitTradeColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }


    [Category("Order"), DisplayName("Sell Color")]
    public Color SellOrderColor{
      get { return this._sellOrderColor; }
      set {
        bool evt = this._sellOrderColor != value;

        this._sellOrderColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }

    [Category("Order"), DisplayName("Buy Color")]
    public Color BuyOrderColor {
      get { return this._buyOrderColor; }
      set {
        bool evt = this._buyOrderColor != value;
        this._buyOrderColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }


    [Category("Order"), DisplayName("Stop Color")]
    public Color StopOrderColor {
      get { return this._stopOrderColor; }
      set {
        bool evt = this._stopOrderColor != value;
        this._stopOrderColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }

    [Category("Order"), DisplayName("Limit Color")]
    public Color LimitOrderColor {
      get { return this._limitOrderColor; }
      set {
        bool evt = this._limitOrderColor != value;
        this._limitOrderColor = value;
        if (evt)
          this.OnStyleChanged(new EventArgs());
      }
    }

    #region public override string ToString()
    public override string ToString() {
			return this.Name;
		}
		#endregion

    #region protected virtual void OnStyleChanged(EventArgs e)
    protected virtual void OnStyleChanged(EventArgs e) {
      _editId++;
      if (this.StyleChangedEvent != null)
        this.StyleChangedEvent(this, e);
    }
    #endregion

    public ChartStyle Clone() {
      ChartStyle cs = new ChartStyle();
      
      cs.Name = this.Name;

      cs.BackColor = this.BackColor;
      cs.BarColor = this.BarColor;
      cs.BarDownColor = this.BarDownColor;
      cs.BarUpColor = this.BarUpColor;
      cs.BorderColor = this.BorderColor;
      cs.GridColor = this.GridColor;
      cs.ScaleFont = this.ScaleFont;
      cs.ScaleForeColor = this.ScaleForeColor;

      cs.SellTradeColor = this.SellTradeColor;
      cs.SellOrderColor = this.SellOrderColor;
      cs.BuyTradeColor = this.BuyTradeColor;
      cs.BuyOrderColor = this.BuyOrderColor;
      cs.StopTradeColor = this.StopTradeColor;
      cs.StopOrderColor = this.StopOrderColor;
      cs.LimitTradeColor = this.LimitTradeColor;
      cs.LimitOrderColor = this.LimitOrderColor;

      return cs;
    }
  }
}
