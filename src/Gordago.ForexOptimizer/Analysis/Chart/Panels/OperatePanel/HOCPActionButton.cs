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

namespace Gordago.API.OperatePanel {

  public partial class HOCPActionButton : UserControl {

    private float _price = 0;

    private Font _smallFont, _bigFont,_textFont;
    private StringFormat _sformat;
    private int _smallPercent = 30;
    private int _textPercent = 20;

    private string _smallString = "0.00", _bigString = "00", _text = "Sell";

    private Color _foreColor;
    private Brush _foreBrush;

    private ISymbol _symbol;

    private Color _borderColor;
    private Pen _borderPen;

    private Brush _backBrush;

    private bool _mouseEnter = false;
    private bool _resize = true;
    private bool _useToolTipText = true;

    public HOCPActionButton() {
      InitializeComponent();

      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

      this._smallFont = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
      this._bigFont = new Font("Microsoft Sans Serif", 26, FontStyle.Bold);
      this._textFont = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
      
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;

      this.Price = 0.0001F;
      this.ForeColor = Color.FromArgb(198, 198, 213);
      this.BorderColor = Color.FromArgb(100, 126, 161);
      _toolTipText.SetToolTip(this, "Connection error");
    }

    #region public bool UseToolTip
    public bool UseToolTip {
      get { return _useToolTipText; }
      set {
        _useToolTipText = value;
        if (!value)
          _toolTipText.SetToolTip(this, "");
      }
    }
    #endregion

    #region public float Price
    public float Price {
      get { return this._price; }
      set { 
        this._price = value;

        if (_symbol != null) {
          string sprice = SymbolManager.ConvertToCurrencyString(_price, _symbol.DecimalDigits);
          _smallString = sprice.Substring(0, sprice.Length - 2);
          _bigString = sprice.Substring(sprice.Length - 2, 2);
        } else {
          _smallString = "0.00";
          _bigString = "00";
       }

        this.Invalidate();
      }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
      set { this._symbol = value; }
    }
    #endregion

    #region public Color ForeColor
    public new Color ForeColor {
      get { return this._foreColor; }
      set { 
        this._foreColor = value;
        this._foreBrush = new SolidBrush(value);
        this.Invalidate();
      }
    }
    #endregion

    #region public Font SmallFont
    public Font SmallFont {
      get { return this._smallFont; }
      set {
        this._smallFont = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Font BigFont
    public Font BigFont {
      get { return this._bigFont; }
      set {
        this._bigFont = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set { 
        this._borderColor = value;
        _borderPen = new Pen(value);
        this.Invalidate();
      }
    }
    #endregion

    #region public override Color BackColor
    public override Color BackColor {
      get {
        return base.BackColor;
      }
      set {
        _backBrush = new SolidBrush(value);
        base.BackColor = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public new string Text
    [Browsable(true)]
    [Category("Misc"), DisplayName("Text")]
    public new string Text {
      get { return this._text; }
      set {
        this._text = value;
        this.Invalidate();
      }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      Graphics g = e.Graphics;
      g.InterpolationMode = InterpolationMode.Low;
      g.SmoothingMode = SmoothingMode.None;
      g.CompositingMode = CompositingMode.SourceOver;
      g.CompositingQuality = CompositingQuality.HighSpeed;
      g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

      int height = this.Height - 4;

      int h1 = height * _smallPercent / 100;
      int h3 = height * _textPercent / 100;
      int h2 = height - h1 - h3;

      if (_resize) {
        _resize = false;
        _smallFont = this.GetCorrectFontSize(g, new Size(this.Width, h1), _smallFont, _smallString);
        _bigFont = this.GetCorrectFontSize(g, new Size(this.Width, h2), _bigFont, _bigString);
        _textFont = this.GetCorrectFontSize(g, new Size(this.Width, h3), _textFont, _text);
      }

      g.Clear(this.BackColor);
      if (_mouseEnter) {
        // g.DrawRectangle(new Pen(Color.Yellow), 2, 2, this.Width - 3, this.Width - 3);
      }
      g.DrawString(_smallString, this._smallFont, _foreBrush, new RectangleF(0,0,this.Width, h1), _sformat);
      g.DrawString(_bigString, this._bigFont, _foreBrush, new RectangleF(0, h1, this.Width, h2),_sformat);
      g.DrawString(_text, this._textFont, _foreBrush, new RectangleF(0, h1 + h2, this.Width, h3), _sformat);

      //g.DrawRectangle(_borderPen, 0,0, this.Width-1, this.Height-1);
      int x2 = this.Width - 1, y2 = this.Height - 1;
      List<Point> line = new List<Point>();

      line.Add(new Point(2, 0));
      line.Add(new Point(x2 - 2, 0));
      line.Add(new Point(x2, 2));
      line.Add(new Point(x2, y2 - 2));
      line.Add(new Point(x2 - 2, y2));
      line.Add(new Point(2, y2));
      line.Add(new Point(0, y2 - 2));
      line.Add(new Point(0, 2));
      g.DrawPolygon(_borderPen, line.ToArray());

    }
    #endregion

    #region protected override void OnMouseEnter(EventArgs e)
    protected override void OnMouseEnter(EventArgs e) {
      base.OnMouseEnter(e);
      _mouseEnter = true;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnMouseLeave(EventArgs e)
    protected override void OnMouseLeave(EventArgs e) {
      base.OnMouseLeave(e);
      _mouseEnter = false;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      _resize = true;
      this.Invalidate();
    }
    #endregion

    #region private Font GetCorrectFontSize(Graphics g, Size sizeBox, Font font, string str)
    private Font GetCorrectFontSize(Graphics g, Size sizeBox, Font font, string str) {
      Font newfont = (Font)font.Clone();
      SizeF sizef = g.MeasureString(str + "W", newfont, 10000);

      /* true - строка больше чем прямоугольник */
      bool strLarge = sizef.Width > sizeBox.Width || sizef.Height > sizeBox.Height;

      float deltaSize = strLarge ? -0.1f : 0.1f;
      
      while (true) {
        float emsize = newfont.SizeInPoints + deltaSize;
        if (emsize < 1)
          break;
        newfont = new Font(newfont.FontFamily, emsize, font.Style);
        sizef = g.MeasureString(str + "W", newfont, 10000);
        bool strLargeNew = sizef.Width > sizeBox.Width || sizef.Height > sizeBox.Height;
        if (strLarge != strLargeNew)
          break;
      }
      return newfont;
    }
    #endregion

    #region public void SetToolTipText(string text)
    public void SetToolTipText(string text) {
      if (!_useToolTipText) return;
      _toolTipText.SetToolTip(this, text);
    }
    #endregion
  }
}
