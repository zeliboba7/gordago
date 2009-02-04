/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using Gordago.Design;
using System.ComponentModel.Design;

namespace Gordago.Analysis.Chart {

  #region public class COPointManagerLineExpansion : COPointManager
  public class COPointManagerLineExpansion : COPointManager {
    public COPointManagerLineExpansion() : base(3, 4) {
    }

    protected internal override void OnCalculateAnchors() {
      base.OnCalculateAnchors();

      int x1 = this.Anchors[0].X;
      int y1 = this.Anchors[0].Y;
      int x2 = this.Anchors[1].X;
      int y2 = this.Anchors[1].Y;

      this.Anchors[3].X = x1 + (x2 - x1) / 2;
      this.Anchors[3].Y = y1 + (y2 - y1) / 2;

    }
  }
  #endregion

  public class ChartObjectFiboExpansion:ChartObject {

    #region private readonly static float[] PERCENT_INIT
    private readonly static float[] PERCENT_INIT =
      new float[]{
        61.8F,
        100F,
        161.8F
      };
    #endregion

    private Color _basisLineColor;
    private Pen _pen;
    private Brush _brush;

    private Color _fiboColor;
    private Pen _fiboPen;

    private Font _font;
    private Color _foreColor;
    private Brush _foreBrush;


    private float[] _levels;
    private string[] _slevels;
    private int[] _ylevels;

    private StringFormat _sformat;

    public ChartObjectFiboExpansion(string name) : base(name, new COPointManagerLineExpansion()) {
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;

      _ylevels = new int[0];
      _slevels = new string[0];

      this.BasisPen = new Pen(Color.Red);
      this.BasisPen.DashStyle = DashStyle.Custom;
      this.BasisPen.DashPattern = new float[] { 5, 5 };

      this.FiboColor = Color.Yellow;

      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COFiboExp));
      this.Image = Gordago.Properties.Resources.m_co_fibo_expansion;
      this.TypeName = "Fibo Expansion";
      this.Levels = PERCENT_INIT;
    }

    #region public Color BasisColor
    /// <summary>
    /// Цвет линии
    /// </summary>
    [Category("Style"), DisplayName("Basis Color")]
    public Color BasisColor {
      get { return this._basisLineColor; }
      set {
        this._basisLineColor = value;
        if (_pen == null) {
          this._pen = new Pen(value);
        } else {
          _pen.Color = value;
        }
        this._brush = new SolidBrush(value);
      }
    }
    #endregion

    #region protected Pen BasisPen
    /// <summary>
    /// Карандаш
    /// </summary>
    [Browsable(false)]
    protected Pen BasisPen {
      get { return this._pen; }
      set {
        this._pen = value;
        this.BasisColor = _pen.Color;
      }
    }
    #endregion

    #region public Color FiboColor
    [Category("Style"), DisplayName("Fibo Color")]
    public Color FiboColor {
      get { return this._fiboColor; }
      set {
        this._fiboColor = value;
        if (_fiboPen == null)
          this._fiboPen = new Pen(value);
        _fiboPen.Color = value;
      }
    }
    #endregion

    #region public int FiboWidth
    [Category("Style"), DisplayName("Fibo Width")]
    public int FiboWidth {
      get { return Convert.ToInt32(this._fiboPen.Width); }
      set {
        this._fiboPen.Width = value;
      }
    }
    #endregion

    #region protected Pen FiboPen
    protected Pen FiboPen {
      get { return this._fiboPen; }
    }
    #endregion

    #region public float[] Levels
    [Category("Main")]
    [TypeConverter(typeof(ArrayTypeConverter))]
    [EditorAttribute(typeof(ArrayEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public float[] Levels {
      get { return _levels; }
      set {
        this._levels = value;
        this._slevels = new string[0];
      }
    }
    #endregion

    #region protected string[] SLevels
    protected string[] SLevels {
      get { return this._slevels; }
    }
    #endregion

    #region protected int[] YLevels
    protected int[] YLevels {
      get { return this._ylevels; }
    }
    #endregion

    #region public Font Font
    [Category("Style"), DisplayName("Font")]
    public Font Font {
      get {
        if (this.ChartBox != null) {
          if (this._font == null)
            _font = this.ChartBox.ChartManager.Style.ScaleFont;
        } else {
          this.Font = new Font("Microsoft Sans Serif", 7);
        }

        return this._font;
      }
      set { this._font = value; }
    }
    #endregion

    #region public Color ForeColor
    [Category("Style"), DisplayName("Fore Color")]
    public Color ForeColor {
      get {
        if (_foreBrush == null) {
          _foreColor = this.ChartBox.ChartManager.Style.ScaleForeColor;
          _foreBrush = new SolidBrush(_foreColor);
        }
        return this._foreColor; 
      }
      set {
        this._foreBrush = new SolidBrush(value);
        this._foreColor = value;
      }
    }
    #endregion

    #region protected override void OnPaintObject(System.Drawing.Graphics g)
    protected override void OnPaintObject(System.Drawing.Graphics g) {
      COPoint copoint3 = this.GetCOPoint(2);
      if (copoint3 == null)
        return;

      int x1 = this.COPoints[0].X;
      int x2 = this.COPoints[1].X;
      int x3 = copoint3.X;

      int y1 = this.COPoints[0].Y;
      int y2 = this.COPoints[1].Y;
      int y3 = copoint3.Y;

      Pen pen = _pen;

      if (this.ChartBox.ChartManager.SelectedFigureMouse == this) {
        pen = (Pen)_pen.Clone();
        pen.Width = 3;
      }

      g.DrawLine(pen, x1, y1, x2, y2);
      g.DrawLine(pen, x2, y2, x3, y3);
      this.GraphicsPath.AddLine(x1, y1, x2, y2);
      this.GraphicsPath.AddLine(x2, y2, x3, y3);

      #region calc array
      if (_slevels.Length != this._levels.Length) {
        _slevels = new string[this._levels.Length];
        for (int i = 0; i < _levels.Length; i++) {
          _slevels[i] = SymbolManager.ConvertToCurrencyString(_levels[i], 1);
        }
      }

      if (_ylevels.Length != _levels.Length)
        _ylevels = new int[_levels.Length];
      #endregion

      for (int i = 0; i < this._levels.Length; i++) {
        float dy = ((y2 - y1) * this._levels[i]) / 100F;
        _ylevels[i] = y3 + (int)dy;
      }

      int xMax = Math.Max(x1, x2);
      int dxAbs = Math.Abs(x2 - x1);

      int xx1 = x1;
      int xx2 = this.ChartBox.Width;

      int xs = Math.Min(this.ChartBox.Width, x2);

      if (_foreBrush == null) {
        Color temp = this.ForeColor;
      }

      for (int i = 0; i < this.Levels.Length; i++) {
        int y = this.YLevels[i];
        g.DrawLine(this.FiboPen, xx1, y, xx2, y);

        if (xx1 < this.ChartBox.Width && xx2 > 0) {

          string str = "FE " + this.SLevels[i];

          SizeF sizef = g.MeasureString(str + "W", this.Font, new PointF(xx2, y), _sformat);
          int w = Convert.ToInt32(sizef.Width);
          int h = Convert.ToInt32(sizef.Height);

          RectangleF rectf = new RectangleF(xx2 - w, y - h, w, h);

          g.DrawString(str, this.Font, _foreBrush, rectf, _sformat);
        }
      }
    }
    #endregion

    #region protected override void SaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("BasisLineColor", this.BasisColor);
      nodeManager.SetAttribute("FiboColor", this.FiboColor);
      nodeManager.SetAttribute("FiboWidth", this.FiboPen.Width);
      nodeManager.SetAttribute("FiboForeColor", this.ForeColor);
      nodeManager.SetAttribute("FiboFont", this.Font);

      string[] sa = new string[this.Levels.Length];

      for (int i = 0; i < this.Levels.Length; i++) {
        sa[i] = XmlNodeManager.ConvertFloatToString(this.Levels[i]);
      }

      string sstr = string.Join("|", sa);
      nodeManager.SetAttribute("FiboLevels", sstr);
    }
    #endregion

    #region protected override void LoatTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);

      this.BasisColor = nodeManager.GetAttributeColor("BasisLineColor", Color.Red);
      this.FiboColor = nodeManager.GetAttributeColor("FiboColor", this.FiboColor);
      this.FiboPen.Width = nodeManager.GetAttributeInt32("FiboWidth", 1);
      this.ForeColor = nodeManager.GetAttributeColor("FiboForeColor", Color.Red);
      this.Font = nodeManager.GetAttributeFont("FiboFont", this.Font);

      string sstr = nodeManager.GetAttributeString("FiboLevels", "error");
      if (sstr != "error") {

        string[] sa = sstr.Split('|');
        this.Levels = new float[sa.Length];
        for (int i = 0; i < sa.Length; i++) {
          this.Levels[i] = XmlNodeManager.ConvertStringToFloat(sa[i]);
        }
      }
    }
    #endregion
  }
}
