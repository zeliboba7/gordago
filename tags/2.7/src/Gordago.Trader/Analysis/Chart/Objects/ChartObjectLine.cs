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
using System.ComponentModel;
#endregion

namespace Gordago.Analysis.Chart {

  #region public class COPointManagerPoint : COPointManager
  public class COPointManagerPoint : COPointManager {
    public COPointManagerPoint() : base(1, 1) {
    }
  }
  #endregion

  #region public class COPointManagerLine : COPointManager
  public class COPointManagerLine : COPointManager {
    public COPointManagerLine() : base(2, 3) {
    }
    protected internal override void OnCalculateAnchors() {
      base.OnCalculateAnchors();
      int x1 = this.Anchors[0].X;
      int y1 = this.Anchors[0].Y;
      int x2 = this.Anchors[1].X;
      int y2 = this.Anchors[1].Y;

      this.Anchors[2].X = x1 + (x2 - x1) / 2;
      this.Anchors[2].Y = y1 + (y2 - y1) / 2;
    }
  }
  #endregion

  public abstract class ChartObjectLine:ChartObject {
    
    private Color _color;
    private Pen _pen;
    private Brush _brush;

    private bool _isVector = true;

    #region public ChartObjectLine(string name, bool isVector) : base(name, 2)
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Наименование фигуры</param>
    /// <param name="isVector">true - линия является вектором, false - отрезок</param>
    public ChartObjectLine(string name, bool isVector) : base(name, new COPointManagerLine()) {
      _isVector = isVector;
      this.LineColor = Color.Red;
    }
    #endregion

    #region public bool IsVector
    [Category("Style"), DisplayName("Is Vector")]
    public bool IsVector {
      get { return _isVector; }
      set {
        this._isVector = value;
      }
    }
    #endregion

    #region public Color LineColor
    [Browsable(false)]
    /// <summary>
    /// Цвет линии
    /// </summary>
    public Color LineColor {
      get { return this._color; }
      set {
        this._color = value;
        if(_pen == null) {
          this._pen = new Pen(value);
        } else {
          _pen.Color = value;
        }
        this._brush = new SolidBrush(value);
      }
    }
    #endregion 

    #region protected Pen LinePen
    /// <summary>
    /// Карандаш
    /// </summary>
    [Browsable(false)]
    protected Pen LinePen {
      get { return this._pen; }
      set {
        this._pen = value;
        this.LineColor = _pen.Color;
      }
    }
    #endregion

    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {

      COPoint copoint2 = this.GetCOPoint(1);
      if(copoint2 == null) return;

      int x1 = this.COPoints[0].X;
      int y1 = this.COPoints[0].Y;

      int x2 = copoint2.X;
      int y2 = copoint2.Y;

      int m = _isVector ? 2 : 0;

      int dx2 = x2 + (x2 - x1) * m;
      int dy2 = y2 + (y2 - y1) * m;

      Pen pen = _pen;

      if(this.ChartBox.ChartManager.SelectedFigureMouse == this) {
        pen = (Pen)_pen.Clone();
        pen.Width = 3;
      }

      g.DrawLine(pen, x1, y1, dx2, dy2);
      this.GraphicsPath.AddLine(x1, y1, dx2, dy2);
    }
    #endregion

    #region protected override void SaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("BasisLineColor", this.LineColor);
      nodeManager.SetAttribute("BasisLineWidth", this.LinePen.Width);
      nodeManager.SetAttribute("IsVector", this.IsVector);
    }
    #endregion

    #region protected override void LoatTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);
      this.LineColor = nodeManager.GetAttributeColor("BasisLineColor", Color.Red);
      this.LinePen.Width = nodeManager.GetAttributeInt32("BasisLineWidth", 1);
      this.IsVector = nodeManager.GetAttributeBoolean("IsVector", true);
    }
    #endregion
  }
}
