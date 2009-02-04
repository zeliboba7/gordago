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
  public class ChartObjectRectagle:ChartObject {

    private Color _borderColor, _backColor;
    private Pen _borderPen;
    private SolidBrush _backBrush;

    private int _backColorAlpha = 50;

    public ChartObjectRectagle(string name) : base(name, new COPointManagerLine()) {

      this.BorderColor = Color.Red;
      this.BackColor = Color.FromArgb(138, 250, 196);

      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.CORectangle));
      this.Image = Gordago.Properties.Resources.m_co_rectangle;
      this.TypeName = "Rectangle";
    }

    #region public Color BorderColor
    [Category("Style"), DisplayName("Border Color")]
    public Color BorderColor {
      get { return this._borderColor; }
      set {
        if (_borderPen == null)
          this._borderPen = new Pen(value);
        this._borderPen.Color = value;
        this._borderColor = value; 
      }
    }
    #endregion

    #region public Color BackColor
    [Category("Style"), DisplayName("Back Color")]
    public Color BackColor {
      get { return this._backColor; }
      set {
        this._backColor = value;
      }
    }
    #endregion

    #region public int BackColorAlpha
    [Category("Style"), DisplayName("Back Color Alpha")]
    public int BackColorAlpha {
      get { return this._backColorAlpha; }
      set {
        value = Math.Max(value, 1);
        value = Math.Min(value, 255);
        this._backColorAlpha = value; 
      }
    }
    #endregion

    #region protected SolidBrush BackBrush
    protected SolidBrush BackBrush {
      get {
        if (_backBrush == null)
          _backBrush = new SolidBrush(this.BackColor);
        _backBrush.Color = Color.FromArgb(this._backColorAlpha, this.BackColor);
        return _backBrush;
      }
    }
    #endregion

    #region protected override void OnPaintObject(System.Drawing.Graphics g)
    protected override void OnPaintObject(System.Drawing.Graphics g) {
      COPoint copoint2 = this.GetCOPoint(1);
      if (copoint2 == null)
        return;

      int xx1 = this.COPoints[0].X;
      int yy1 = this.COPoints[0].Y;

      int xx2 = copoint2.X;
      int yy2 = copoint2.Y;

      int x1 = Math.Min(xx1, xx2);
      int x2 = Math.Max(xx1, xx2);

      int y1 = Math.Min(yy1, yy2);
      int y2 = Math.Max(yy1, yy2);

      int w = x2 - x1;
      int h = y2 - y1;

      if (w == 0 || h == 0)
        return;

      Pen pen = this._borderPen;

      if (this.ChartBox.ChartManager.SelectedFigureMouse == this) {
        pen = (Pen)this._borderPen.Clone();
        pen.Width = 3;
      }

      Rectangle rect = new Rectangle(x1, y1, w, h);
      this.GraphicsPath.AddRectangle(rect);

      g.FillRectangle(this.BackBrush, rect);
      g.DrawRectangle(pen, rect);
    }
    #endregion

    #region protected override void OnSaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);
      nodeManager.SetAttribute("RectBackColor", this.BackColor);
      nodeManager.SetAttribute("RectBackColorAlpha", this.BackColorAlpha);
      nodeManager.SetAttribute("RectBorderColor", this.BorderColor);
    }
    #endregion

    #region protected override void OnLoadTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);

      this.BackColor = nodeManager.GetAttributeColor("RectBackColor", Color.Red);
      this.BackColorAlpha = nodeManager.GetAttributeInt32("RectBackColorAlpha", 50);
      this.BorderColor = nodeManager.GetAttributeColor("RectBorderColor", Color.FromArgb(138, 250, 196));
    }
    #endregion
  }
}
