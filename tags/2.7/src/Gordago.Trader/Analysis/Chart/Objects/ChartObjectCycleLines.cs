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
#endregion

namespace Gordago.Analysis.Chart{
  public class ChartObjectCycleLines:ChartObjectLine {
    
    private Color _linesColor;
    private Pen _linesPen;

    public ChartObjectCycleLines(string name):base(name, false) {
      this.Cursor = new Cursor(new MemoryStream(global::Gordago.Properties.Resources.COCycleLines));

      this.Image = Gordago.Properties.Resources.m_co_cycle_lines;
      this.TypeName = "Cycle Lines";

      this.LinesColor = Color.Red;
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

    #region public Color LinesColor
    [Category("Style"), DisplayName("Lines Color")]
    public Color LinesColor {
      get { return this._linesColor; }
      set {
        this._linesColor = value;
        if (_linesPen == null)
          _linesPen = new Pen(value);
        this._linesPen.Color = value;
      }
    }
    #endregion

    #region public int LinesWidth
    [Category("Style"), DisplayName("Lines Width")]
    public int LinesWidth {
      get { return Convert.ToInt32(this.LinesPen.Width); }
      set { this.LinesPen.Width = value; }
    }
    #endregion

    #region protected Pen LinesPen
    protected Pen LinesPen {
      get { return this._linesPen; }
    }
    #endregion

    #region protected override void OnPaintObject(Graphics g)
    protected override void OnPaintObject(Graphics g) {
      base.OnPaintObject(g);

      COPoint copoint2 = this.GetCOPoint(1);
      if (copoint2 == null)
        return;

      int barIndex1 = this.COPoints[0].BarIndex;
      int barIndex2 = copoint2.BarIndex;

      int delta = Math.Abs(barIndex2 - barIndex1);
      if(delta == 0) return;

      int p1 = Math.Min(barIndex1, barIndex2);
      int p2 = Math.Max(barIndex1, barIndex2);

      int count = p1 / delta;
      int first = p1 - count * delta;

      int pcount = this.ChartBox.ChartManager.Position / delta;
      int pfirst = first + pcount * delta;
      for(int i = pfirst; i < this.ChartBox.ChartManager.Position + this.Map.Length; i += delta) {

        bool draw = false;
        if(barIndex2 > barIndex1 && i >= barIndex1) {
          draw = true;
        } else if(barIndex2 < barIndex1 && i <= barIndex1) {
          draw = true;
        }
        
        if(draw) {
          int x = this.ChartBox.GetX(i);
          g.DrawLine(this.LinesPen, x, 0, x, this.ChartBox.Height);
        }
      }
    }
    #endregion

    #region protected override void SaveTemplate(XmlNodeManager nodeManager)
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("LinesColor", this.LinesColor);
      nodeManager.SetAttribute("LinesWidth", this.LinesPen.Width);
    }
    #endregion

    #region protected override void LoatTemplate(XmlNodeManager nodeManager)
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);

      this.LineColor = nodeManager.GetAttributeColor("LinesColor", this.LineColor);
      this.LinesWidth = nodeManager.GetAttributeInt32("LinesWidth", 1);
    }
    #endregion
  }
}
