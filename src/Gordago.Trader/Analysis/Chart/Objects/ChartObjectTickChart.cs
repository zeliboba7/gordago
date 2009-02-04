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

namespace Gordago.Analysis.Chart{
  //public class ChartObjectTickChart:ChartObject {

  //  private Color _borderColor;
  //  private Pen _borderPen;

  //  private DateTime _savedTime1, _savedTime2;
  //  private float _savedPrice1, _savedPrice2;
  //  private int _x1, _x2, _y1, _y2;

  //  private bool _mouseDown = false;

  //  public ChartObjectTickChart(string name) : base(name, new COPointManagerLine()) {
  //    _savedPrice1 = _savedPrice2 = 0;
  //    _savedTime1 = _savedTime2 = DateTime.Now;

  //    this.BorderColor = Color.Blue;

  //    this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COTrendLine));
  //    this.Image = Gordago.Properties.Resources.m_co_tradeline;
  //    this.TypeName = "Tick Chart";
  //  }

  //  #region public Color BorderColor
  //  [Category("Style"), DisplayName("Border Color")]
  //  public Color BorderColor {
  //    get { return this._borderColor; }
  //    set {
  //      if (_borderPen == null)
  //        this._borderPen = new Pen(value);
  //      this._borderPen.Color = value;
  //      this._borderColor = value; 
  //    }
  //  }
  //  #endregion

  //  #region protected Pen BorderPen
  //  protected Pen BorderPen {
  //    get { return this._borderPen; }
  //  }
  //  #endregion

  //  protected internal override void OnPaint(Graphics g) {
  //    if (this.COPoints.IsCreate) {
  //      this.COPoints.Anchors[0].X = 100;
  //      this.COPoints.Anchors[0].Y = 100;
  //      this.COPoints.Anchors[1].X = 200;
  //      this.COPoints.Anchors[1].Y = 200;
  //    }
  //    base.OnPaint(g);
  //  }

  //  protected override void OnPaintObject(System.Drawing.Graphics g) {
  //    COPoint copoint2 =  this.GetCOPoint(1);
  //    if (copoint2 == null)
  //      return;

  //    if (this.COPoints[0].Time != this._savedTime1 || this.COPoints[0].Price != this._savedPrice1 ||
  //      copoint2.Time != this._savedTime2 || copoint2.Price != this._savedPrice2) {

  //      int xx1 = this.COPoints[0].X;
  //      int yy1 = this.COPoints[0].Y;

  //      int xx2 = copoint2.X;
  //      int yy2 = copoint2.Y;

  //      _x1 = Math.Min(xx1, xx2);
  //      _x2 = Math.Max(xx1, xx2);
  //      _y1 = Math.Min(yy1, yy2);
  //      _y2 = Math.Max(yy1, yy2);
  //    }

  //    int w = _x2 - _x1;
  //    int h = _y2 - _y1;

  //    Pen pen = this.BorderPen;
  //    if (this.ChartBox.ChartManager.SelectedFigureMouse == this) {
  //      pen = (Pen)this.BorderPen.Clone();
  //      pen.Width = 3;
  //    }

  //    Rectangle rect = new Rectangle(_x1, _y1, w, h);
  //    g.DrawRectangle(pen, rect);

  //    this.GraphicsPath.AddRectangle(rect);
  //  }

  //  #region protected internal override void OnMouseDown(Point p)
  //  protected internal override void OnMouseDown(Point p) {
  //    base.OnMouseDown(p);
  //    _mouseDown = true;
  //  }
  //  #endregion

  //  #region protected internal override void OnMouseUp(Point p)
  //  protected internal override void OnMouseUp(Point p) {
  //    base.OnMouseUp(p);
  //    _mouseDown = false;


  //  }
  //  #endregion
  //}
}
