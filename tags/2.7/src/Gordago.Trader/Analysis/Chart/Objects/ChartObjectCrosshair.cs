/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.ComponentModel;

namespace Gordago.Analysis.Chart {
  public class ChartObjectCrosshair:ChartObject {

    private Brush _infoBackBrush, _infoForeBrush;
    private Pen _linePen;

    private Color _infoBackColor, _infoForeColor;
    private StringFormat _sformat;

    public ChartObjectCrosshair(string name) : base(name, new COPointManagerPoint()) {
      this.InfoBackColor = Color.Red;
      this.InfoForeColor = Color.Black;

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;


      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COVHLine));
      this.Image = global::Gordago.Properties.Resources.m_col_verhorline;
      this.TypeName = "Crosshair";
    }

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

    #region private Pen LinePen
    private Pen LinePen {
      get {
        if (_linePen == null)
          _linePen = new Pen(this.ChartBox.ChartManager.Style.ScaleForeColor);
        return this._linePen; 
      }
    }
    #endregion

    protected internal override void OnMouseDown(MouseEventArgs e) {
      
      COPoint coPoint = this.ChartBox.GetChartPoint(e.Location);
      if (coPoint.Price < -10000000 || coPoint.Price > 10000000)
        return;
    }

    protected internal override void OnMouseUp(MouseEventArgs e) {
      if (e.Button == MouseButtons.Middle)
        return;
      this.ChartBox.Figures.Remove(this);
    }

    #region protected internal override void OnPaintVScale(Graphics g)
    protected internal override void OnPaintVerticalScale(Graphics g) {
      if (g == null) return;

      COPoint copoint = this.GetCOPoint(0);
      if (copoint == null)
        return;

      int w = this.ChartBox.VerticalScaleSize.Width - 2;
      int h = 12;

      int y = this.ChartBox.GetY(copoint.Price);

      g.DrawLine(this.LinePen, 0, y, 2, y);

      g.FillRectangle(_infoBackBrush, 2, y - h / 2, w, h);

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

      int beginindex = this.ChartBox.ChartManager.Position;
      int endindex = this.ChartBox.ChartManager.Position + this.Map.Length - 1;

      int barIndex = copoint.BarIndex;

      if (barIndex < beginindex || barIndex > endindex)
        return;

      int y = this.ChartBox.GetY(copoint.Price);

      int x1 = 0, x2 = this.ChartBox.Width;

      g.DrawLine(this.LinePen, x1, y, x2, y);

      int x = this.Map[barIndex - beginindex];
      int y1 = 0, y2 = this.ChartBox.Height;

      g.DrawLine(LinePen, x, y1, x, y2);
      DateTime time = copoint.Time;
      string str = time.ToShortDateString() + " " + time.ToShortTimeString();

      int w = 80;
      int h = 15;
      y = this.ChartBox.Height - h;

      g.FillRectangle(_infoBackBrush, x - w / 2, y, w, h);

      g.DrawString(str, this.ChartBox.ChartManager.Style.ScaleFont, _infoForeBrush,
        new RectangleF(x - w / 2, y, w, h), _sformat);
    }
    #endregion
  }
}
