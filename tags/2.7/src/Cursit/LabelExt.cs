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

  public class LabelExt:Control {
    private Color _borderColor;
    private Pen _borderPen;
    private bool _borderVisible;
    private StringFormat _sformat;
    
    public LabelExt() {
      this.BackColor = Color.White;
      this.BorderColor = Color.FromArgb(172, 168, 153);
      this.BorderVisible = true;

      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.DoubleBuffered = true;
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
      this.Text = "Text";
    }

    #region public override string Text
    public override string Text {
      get {
        return base.Text;
      }
      set {
        base.Text = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public override Color BackColor
    public override Color BackColor {
      get {
        return base.BackColor;
      }
      set {
        base.BackColor = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public override Color ForeColor
    public override Color ForeColor {
      get {
        return base.ForeColor;
      }
      set {
        base.ForeColor = value;
        this.Invalidate();
      }
    }
    #endregion

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

    protected override void OnPaint(PaintEventArgs e) {
      Graphics g = e.Graphics;
      g.Clear(this.BackColor);

      using (SolidBrush brush = new SolidBrush(this.ForeColor)) {
        g.DrawString(this.Text, this.Font, brush, new RectangleF(0, 0, this.Width, this.Height), _sformat);
      }

      if (this.BorderVisible)
        g.DrawRectangle(_borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }
  }
}
