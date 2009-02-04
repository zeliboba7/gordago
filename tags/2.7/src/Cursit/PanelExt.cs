/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Cursit {
  public class PanelExt:Panel {
    private Color _borderColor;
    private Pen _borderPen;
    private bool _borderVisible;

    public PanelExt() {
      this.BackColor = Color.White;
      this.BorderColor = Color.FromArgb(172, 168, 153);
      this.BorderVisible = true;
    }

    #region public bool BorderVisible
    public bool BorderVisible {
      get { return _borderVisible; }
      set {
        this._borderVisible = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set {
        _borderColor = value;
        _borderPen = new Pen(value);
        this.Invalidate();
      }
    }
    #endregion

    #region protected override void WndProc(ref Message m)
    protected override void WndProc(ref Message m) {

      if (m.Msg == Cursit.WMsg.WM_PAINT) {
        base.WndProc(ref m);
        Graphics g = Graphics.FromHwnd(this.Handle);
        g.Clear(this.BackColor);
        this.PaintExt(g);
        return;
      }
      base.WndProc(ref m);
    }
    #endregion


    private void PaintExt(Graphics g) {
      if (BorderVisible) {

        List<Point> polygon = new List<Point>();

        int x2 = this.Width-1, y2 = this.Height-1;

        List<Point> line = new List<Point>();

        line.Add(new Point(2, 0));
        line.Add(new Point(x2-2, 0));
        line.Add(new Point(x2, 2));
        line.Add(new Point(x2, y2 - 2));
        line.Add(new Point(x2-2, y2));
        line.Add(new Point(2, y2));
        line.Add(new Point(0, y2 - 2));
        line.Add(new Point(0, 2));
        g.DrawPolygon(_borderPen, line.ToArray());

        //g.DrawRectangle(_borderPen, 0, 0, this.Width - 1, this.Height - 1);
      }
    }
  }
}
