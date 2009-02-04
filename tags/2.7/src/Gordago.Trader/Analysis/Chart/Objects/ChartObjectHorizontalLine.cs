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
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
#endregion

namespace Gordago.Analysis.Chart {
  
  /// <summary>
  /// Горизонтальная линия
  /// </summary>
  public class ChartObjectHorizontalLine:ChartObject {

    private Color _lineColor, _infoBackColor, _infoForeColor;

    private Pen _linePen;
    private Brush _infoBackBrush, _infoForeBrush;
    private StringFormat _sformat;
    private bool _infoVisible = true;

    public ChartObjectHorizontalLine(string name) : base(name, new COPointManagerPoint()) {
      
      this.LineColor = Color.Red;
      this.InfoBackColor = Color.Red;
      this.InfoForeColor = Color.Black;

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;

      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COHorLine));
      this.Image = global::Gordago.Properties.Resources.m_co_horline;
      this.TypeName = "Horizontal Line";
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
      get { return this._infoVisible; }
      set { this._infoVisible = value; }
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
      if(this.COPoints[0] != null) {
        int pos = this.ChartBox.ChartManager.Position + this.Map.Length / 2;
        this.COPoints[0].SetValue(pos, this.COPoints[0].Price);
      }
      base.OnPaint(g);
    }
    #endregion

    #region protected internal override void OnPaintVScale(Graphics g)
    protected internal override void OnPaintVerticalScale(Graphics g) {
      if (g == null) return;
      if (!this.InfoVisible)
        return;

      COPoint copoint = this.GetCOPoint(0);
      if(copoint == null) return;

      int w = this.ChartBox.VerticalScaleSize.Width - 2;
      int h = 12;

      int y = this.ChartBox.GetY(copoint.Price);

      g.DrawLine(_linePen, 0, y, 2, y);

      g.FillRectangle(_infoBackBrush, 2, y-h/2, w, h);

      g.DrawString(
        SymbolManager.ConvertToCurrencyString(copoint.Price, this.ChartBox.DecimalDigits, ""),
        this.ChartBox.ChartManager.Style.ScaleFont, _infoForeBrush, new RectangleF(2, y - h / 2, w, h), _sformat);
    }
    #endregion 
  
    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {

      COPoint copoint = this.GetCOPoint(0);
      if (copoint == null)
        return;

      int y = this.ChartBox.GetY(copoint.Price);

      int x1 = 0, x2 = this.ChartBox.Width;

      Pen pen = _linePen;

      if(this.ChartBox.ChartManager.SelectedFigureMouse == this) {
        pen = (Pen)_linePen.Clone();
        pen.Width = 3;
      }

      g.DrawLine(pen, x1, y, x2, y);
      this.GraphicsPath.AddLine(x1, y, x2, y);
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
