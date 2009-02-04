/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using Gordago.Design;
using System.ComponentModel.Design;
#endregion

namespace Gordago.Analysis.Chart {
  public class ChartObjectFiboTimeZones : ChartObjectLine {

    #region private readonly static int[] PERCENT_INIT
    private readonly static int[] PERCENT_INIT =
      new int[]{
        0, 1, 2,
        3, 5, 8, 13, 21, 34
      };
    #endregion

    private int[] _levels;

    private Color _fiboColor;
    private Pen _fiboPen;

    private int _swidth = 35, _sHeight = 15;
    private StringFormat _sformat;

    private int _savedStyleId;
    private Brush _scaleForeBrush;

    public ChartObjectFiboTimeZones(string name)
      : base(name, false) {
      _levels = PERCENT_INIT;

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;

      this.LinePen = new Pen(Color.Red);
      this.LinePen.DashStyle = DashStyle.Custom;
      this.LinePen.DashPattern = new float[] { 5, 5 };

      this.FiboColor = Color.Yellow;

      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COFibo));
      this.Image = Gordago.Properties.Resources.m_co_fibo_timezone;
      this.TypeName = "Fibo Time Zones";
    }

    #region public Color BasisLineColor
    [Category("Style"), DisplayName("Basis Color")]
    public Color BasisLineColor {
      get { return base.LineColor; }
      set { base.LineColor = value; }
    }
    #endregion

    #region public int BasisLineWidth
    [Category("Style"), DisplayName("Basis Width")]
    public int BasisLineWidth {
      get { return Convert.ToInt32(base.LinePen.Width); }
      set { base.LinePen.Width = value; }
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

    #region public int[] Levels
    [Category("Main")]
    [TypeConverter(typeof(ArrayTypeConverter))]
    [EditorAttribute(typeof(ArrayEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public int[] Levels {
      get { return _levels; }
      set {
        this._levels = value;
      }
    }
    #endregion

    #region protected Pen LinesPen
    protected Pen LinesPen {
      get { return this._fiboPen; }
    }
    #endregion

    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {
      if (_savedStyleId != this.ChartBox.ChartManager.Style.Id) {
        _savedStyleId = this.ChartBox.ChartManager.Style.Id;
        _scaleForeBrush = new SolidBrush(this.ChartBox.ChartManager.Style.ScaleForeColor);
      }
      base.OnPaintObject(g);

      COPoint copoint2 = this.GetCOPoint(1);
      if (copoint2 == null)
        return;

      int barIndex1 = this.COPoints[0].BarIndex;
      int barIndex2 = copoint2.BarIndex;


      int p1 = Math.Min(barIndex1, barIndex2);
      int p2 = Math.Max(barIndex1, barIndex2);

      int delta = p2 - p1;
      if (delta == 0)
        return;

      int m = barIndex2 > barIndex1 ? 1 : -1;

      for (int i = 0; i < _levels.Length; i++) {
        int index = barIndex1 + _levels[i] * delta * m;
        if (index < 0)
          return;
        int x = this.ChartBox.GetX(index);

        g.DrawLine(_fiboPen, x, 0, x, this.ChartBox.Height);

        g.DrawString(this._levels[i].ToString(), this.ChartBox.ChartManager.Style.ScaleFont,
          _scaleForeBrush,
          new RectangleF(x - _swidth / 2, this.ChartBox.Height / 2 - _sHeight / 2, _swidth, _sHeight),
          _sformat);
      }
    }
    #endregion

    #region protected override void SaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("FiboColor", this.FiboColor);
      nodeManager.SetAttribute("FiboWidth", this.FiboPen.Width);

      string[] sa = new string[this.Levels.Length];

      for (int i = 0; i < this.Levels.Length; i++) {
        sa[i] = this.Levels[i].ToString();
      }

      string sstr = string.Join("|", sa);
      nodeManager.SetAttribute("FiboLevels", sstr);
    }
    #endregion

    #region protected override void LoatTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);

      this.FiboColor = nodeManager.GetAttributeColor("FiboColor", this.FiboColor);
      this.FiboPen.Width = nodeManager.GetAttributeInt32("FiboWidth", 1);

      string sstr = nodeManager.GetAttributeString("FiboLevels", "error");
      if (sstr != "error") {

        string[] sa = sstr.Split('|');
        this.Levels = new int[sa.Length];
        for (int i = 0; i < sa.Length; i++) {
          this.Levels[i] = Convert.ToInt32(sa[i]);
        }
      }
    }
    #endregion

  }
}
