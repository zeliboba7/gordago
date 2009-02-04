/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gordago.Strategy {
  public partial class LabelVertical:Control {

    private bool _borderVisible = true;
    
    public LabelVertical() {
      InitializeComponent();
    }

    #region public bool BorderVisible
    public bool BorderVisible {
      get { return _borderVisible; }
      set { this._borderVisible = value; }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs pe)
    protected override void OnPaint(PaintEventArgs pe) {
      StringFormat sf = new StringFormat();
      sf.Alignment = StringAlignment.Center;
      sf.LineAlignment = StringAlignment.Center;

      Graphics g = pe.Graphics;

      if (BorderVisible)
        g.DrawRectangle(Pens.Black, 0, 0, this.Width-1, this.Height-1);

      RectangleF rect = new RectangleF(-this.Height, 0, this.Height, this.Width);
      g.RotateTransform(-90, System.Drawing.Drawing2D.MatrixOrder.Append);
      g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), rect, sf);
      g.Flush();
    }
    #endregion

    #region protected override void OnTextChanged(EventArgs e)
    protected override void OnTextChanged(EventArgs e) {
      base.OnTextChanged(e);
      this.Invalidate();
    }
    #endregion

    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this.Invalidate();
    }
  }
}
