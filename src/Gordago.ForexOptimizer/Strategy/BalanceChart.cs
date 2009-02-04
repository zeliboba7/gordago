/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Gordago.Analysis.Chart;

namespace Gordago.Strategy {
  public partial class BalanceChart : UserControl {
    private Bitmap _bitmap, _bitmapVS, _bitmapHS;

    private int _tbout = 7;
    private float _vsMinVal = 0, _vsMaxVal = 1;
    private float _calcul;
    private int _bouttop = 1, _boutleft = 1, _boutbottom = 1, _boutright = 1;
    private int _oldWidth, _oldHeight;
    private Graphics _graphics;
    private bool _scaleerror;
    private int _saveCountTicks;
    private int _emptyCountPixel = 10;
    private Tick[] _ticksCache = new Tick[0];
    private ChartStyle _style;
    private bool _gridVisible = true;

    private Color _bidcolor;
    private Brush _bidbrush;
    private Pen _bidpen;
    private Pen _bidlinepen;

    private StringFormat _sformat;
    private int _decimalDigigts = 0;

    public BalanceChart() {
      InitializeComponent();

      _bitmap = new Bitmap(1, 1);
      _bitmapHS = new Bitmap(1, 1);
      _bitmapVS = new Bitmap(1, 1);

      _style = new ChartStyle();
      this.BidColor = Color.Red;
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;
    }


    #region public ChartStyle Style
    public ChartStyle Style {
      get { return this._style; }
      set {
        this._style = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public bool GridVisible
    public bool GridVisible {
      get { return this._gridVisible; }
      set { this._gridVisible = value; }
    }
    #endregion

    #region public Color BidColor
    public Color BidColor {
      get { return this._bidcolor; }
      set {
        this._bidcolor = value;
        this._bidbrush = new SolidBrush(value);
        this._bidpen = new Pen(value);
        this._bidlinepen = new Pen(value);
        this._bidlinepen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
      }
    }
    #endregion

    #region public int GetY(float val)
    public int GetY(float val) {
      if (_scaleerror)
        return 0;
      int height = _bitmap.Height - _tbout * 2;
      float tmpval = _vsMaxVal - _vsMinVal;
      _calcul = 0;
      if (tmpval != 0)
        _calcul = height / tmpval;
      return Convert.ToInt32(_calcul * (_vsMaxVal - val) + _tbout);
    }
    #endregion

    #region protected override void OnPaintBackground(PaintEventArgs pevent)
    protected override void OnPaintBackground(PaintEventArgs pevent) {
      //base.OnPaintBackground(pevent);
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this.Invalidate();
    }
    #endregion

    private ISymbol _symbol = null;

    protected override void OnPaint(PaintEventArgs e) {
      if (this._symbol == null)
        return;

      bool pfScaleChange = false;

      if (_oldWidth != this.Width || _oldHeight != this.Height) {
        this.CreateBitmap();
        _oldWidth = this.Width;
        _oldHeight = this.Height;
        pfScaleChange = true;
      }

      if (pfScaleChange || _saveCountTicks != _symbol.Ticks.Count) {
        _vsMinVal = float.MaxValue;
        _vsMaxVal = float.MinValue;

        _saveCountTicks = _symbol.Ticks.Count;
        _ticksCache = new Tick[Math.Max(this._bitmap.Width - _emptyCountPixel, 2)];
        int c = _ticksCache.Length;
        int index;
        for (int i = 0; i < c; i++) {
          index = _symbol.Ticks.Count - (c - i);
          Tick tick = _ticksCache[i] = _symbol.Ticks[index];
          _vsMinVal = Math.Min(tick.Price, _vsMinVal);
          _vsMaxVal = Math.Max(tick.Price, _vsMaxVal);
        }
        _scaleerror = this._vsMinVal >= this._vsMaxVal;
      }

      this.PaintBitmap();

      Graphics g = e.Graphics;
      g.Clear(Style.BackColor);
      g.DrawImageUnscaled(_bitmap, _boutleft, _bouttop, _bitmap.Width, _bitmap.Height);
      g.DrawImageUnscaled(_bitmapHS, _boutleft, _bitmap.Height + _bouttop, _bitmapHS.Width, _bitmapHS.Height);
      g.DrawImageUnscaled(_bitmapVS, _bitmap.Width + _boutleft, _bouttop, _bitmapVS.Width, _bitmapVS.Height);
    }


    #region public void CreateBitmap()
    public void CreateBitmap() {
      int dw = 0, dh = 0;
      int width = Math.Max(this.Width - this._boutleft - this._boutright, 1);
      int height = Math.Max(this.Height - this._bouttop - this._boutbottom, 1);

      dh = 15;
      _bitmapHS = new Bitmap(width, dh);

      dw = 38;
      _bitmapVS = new Bitmap(dw, Math.Max(height - dh, 1));

      _bitmap = new Bitmap(Math.Max(width - dw, 1), Math.Max(height - dh, 1));
      _graphics = Graphics.FromImage(_bitmap);
      _graphics.InterpolationMode = InterpolationMode.Low;
      _graphics.SmoothingMode = SmoothingMode.None;
      _graphics.CompositingMode = CompositingMode.SourceOver;
      _graphics.CompositingQuality = CompositingQuality.HighSpeed;
      _graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
    }
    #endregion

    protected virtual void PaintBitmap() {
      Graphics ghscale = Graphics.FromImage(_bitmapHS);
      ghscale.Clear(Style.BackColor);

      Graphics gvscale = Graphics.FromImage(_bitmapVS);
      gvscale.Clear(Style.BackColor);

      _graphics.Clear(Style.BackColor);

      Point[] pline = new Point[_ticksCache.Length];

      #region Drawing scale
      if (!this._scaleerror) {
        int m = SymbolManager.GetDelimiter(0);
        int minval = Convert.ToInt32(_vsMinVal * m);
        int maxval = Convert.ToInt32(_vsMaxVal * m);
        int minstep = Math.Max(Convert.ToInt32(_bitmap.Height / 60), 1);
        int oneval = Math.Max(maxval - minval, 1);
        int minvalstep = oneval / Math.Min(oneval, minstep);

        for (int ival = minval; ival <= maxval; ival += minvalstep) {
          float val = (float)ival / m;
          int y = this.GetY(val);

          //if (this.GridVisible)
          //  _graphics.DrawLine(Style.GridPen, 0, y, _bitmap.Width, y);
          if (this.GridVisible)
            _graphics.DrawLine(null, 0, y, _bitmap.Width, y);

          gvscale.DrawLine(null, 0, y, 2, y);
          gvscale.DrawString(
            SymbolManager.ConvertToCurrencyString(val, 0, ""),
            this.Style.ScaleFont,
            this.Style.ScaleForeBrush, 3, y - 7);
          //gvscale.DrawLine(Style.BorderPen, 0, y, 2, y);
          //gvscale.DrawString(
          //  SymbolManager.ConvertToCurrencyString(val, 0, ""),
          //  this.Style.ScaleFont,
          //  this.Style.ScaleForeBrush, 3, y - 7);
        }

        int cnt = 32;
        int cntj = cnt * 2;
        int j = cntj, ii = 0;
        bool l = true;
        bool ll = false;
        int barcount = _ticksCache.Length;

        for (int i = 0; i < _ticksCache.Length; i++) {

          pline[i] = new Point(i, this.GetY(_ticksCache[i].Price));

          if (j >= cntj) {
            int barindex = i;

            string sc = "";
            DateTime dtm = new DateTime(_ticksCache[i].Time);
            if (l) {
              sc = dtm.ToString("d MMM yyyy");
              l = false;
            } else
              sc = dtm.ToShortTimeString();

            ghscale.DrawLine(Style.BorderPen, i, 0, i, 2);
            ghscale.DrawString(sc, Style.ScaleFont, Style.ScaleForeBrush, i, 2);
            j = 0;
          }
          j++;

          if (ii >= cnt && this.GridVisible) {
            if (ll)
              _graphics.DrawLine(Style.GridPen, i, 0, i, this._bitmap.Height);
            ll = !ll;
            ii = 0;
          }
          ii++;
        }

      }
      #endregion

      _graphics.DrawLines(Style.BarPen, pline);
      _graphics.DrawRectangle(Style.BorderPen, 0, 0, this._bitmap.Width - 1, this._bitmap.Height - 1);
    }
  }
}
