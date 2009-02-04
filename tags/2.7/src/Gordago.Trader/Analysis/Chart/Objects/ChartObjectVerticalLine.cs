/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace Gordago.Analysis.Chart {
  public class ChartObjectVerticalLine: ChartObject {

    private Color _lineColor, _infoBackColor, _infoForeColor;
    private Pen _linePen;
    private Brush _infoBackBrush, _infoForeBrush;
    private StringFormat _sformat;
    private bool _vInfoVisible = true;

    public ChartObjectVerticalLine(string name): base(name, new COPointManagerPoint()) {
      this.LineColor = Color.Red;
      this.InfoBackColor = Color.Red;
      this.InfoForeColor = Color.Black;

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;

      this.TypeName = "Vertical Line";
      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COVerLine));
      this.Image = global::Gordago.Properties.Resources.m_co_vertline;
    }

    #region public Color LineColor
    [Category("Style"), DisplayName("Line Color")]
    public Color LineColor {
      get { return _lineColor; }
      set {
        this._lineColor = value;
        this._linePen = new Pen(value);
      }
    }
    #endregion

    #region public int LineWidth
    [Category("Style"), DisplayName("Line Width")]
    public int LineWidth {
      get { return Convert.ToInt32(this._linePen.Width); }
      set { this._linePen.Width = value; }
    }
    #endregion

    #region public bool InfoVisible
    [Category("Scale Info"), DisplayName("Visible")]
    public bool InfoVisible {
      get { return this._vInfoVisible; }
      set { this._vInfoVisible = value; }
    }
    #endregion

    #region public Color InfoBackColor
    /// <summary>
    /// Цвет линии
    /// </summary>
    [Category("Scale Info"), DisplayName("Back Color")]
    public Color InfoBackColor {
      get { return this._infoBackColor; }
      set {
        this._infoBackColor = value;
        this._infoBackBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color InfoForeColor
    [Category("Scale Info"), DisplayName("Fore Color")]
    public Color InfoForeColor {
      get { return this._infoForeColor; }
      set {
        this._infoForeColor = value;
        this._infoForeBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region protected internal override void OnPaint(Graphics g)
    protected internal override void OnPaint(Graphics g) {
      if (this.COPoints[0] != null) {
        int y = this.ChartBox.Height / 2;
        float price = this.ChartBox.GetPrice(y);
        this.COPoints[0].SetValue(this.COPoints[0].BarIndex, price);
      }
      base.OnPaint(g);
    }
    #endregion

    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {

      COPoint copoint = this.GetCOPoint(0);
      if (copoint == null) return;

      int beginindex = this.ChartBox.ChartManager.Position;
      int endindex = this.ChartBox.ChartManager.Position + this.Map.Length - 1;

      int barIndex = copoint.BarIndex;

      if(barIndex < beginindex || barIndex > endindex) return;

      int x = this.Map[barIndex - beginindex];
      int y1 = 0, y2 = this.ChartBox.Height;

      Pen pen = _linePen;

      if(this.ChartBox.ChartManager.SelectedFigureMouse == this) {
        pen = (Pen)_linePen.Clone();
        pen.Width = _linePen.Width + 3;
      }

      g.DrawLine(pen, x, y1, x, y2);
      this.GraphicsPath.AddLine(x, y1, x, y2);

      if (InfoVisible) {
        DateTime time = copoint.Time;
        string str = time.ToShortDateString() + " " + time.ToShortTimeString();

        int w = 80;
        int h = 15;
        int y = this.ChartBox.Height - h;

        g.FillRectangle(_infoBackBrush, x - w / 2, y, w, h);

        g.DrawString(str, this.ChartBox.ChartManager.Style.ScaleFont, _infoForeBrush,
          new RectangleF(x - w / 2, y, w, h), _sformat);
      }
    }
    #endregion

    #region protected override void SaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("LineColor", this.LineColor);
      nodeManager.SetAttribute("LineWidth", this._linePen.Width);

      nodeManager.SetAttribute("InfoVisible", this.InfoVisible);
      nodeManager.SetAttribute("InfoBackColor", this.InfoBackColor);
      nodeManager.SetAttribute("InfoForeColor", this.InfoForeColor);
    }
    #endregion

    #region protected override void LoatTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);

      this.LineColor = nodeManager.GetAttributeColor("LineColor", Color.Red);
      this.LineWidth = nodeManager.GetAttributeInt32("LineWidth", 1);

      this.InfoVisible = nodeManager.GetAttributeBoolean("InfoVisible", true);
      this.InfoBackColor = nodeManager.GetAttributeColor("InfoBackColor", Color.Red);
      this.InfoForeColor = nodeManager.GetAttributeColor("InfoForeColor", Color.Black);
    }
    #endregion
  }
}
