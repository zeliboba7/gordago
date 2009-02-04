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

namespace Gordago.Analysis.Chart {
  
  #region enum ExtButtonType
  enum ExtButtonType {
    Close,
    Hide,
    Property
  }
  #endregion

  class ExtButton: UserControl {

    public event EventHandler CheckedChanged;

    private bool _onMouseEnter = false;
    private Color _activeColor = Color.FromArgb(236, 233, 216);
    private ExtButtonType _extButtonType = ExtButtonType.Close;
    private bool _checked;

    #region public ExtButton()
    public ExtButton() {
      this.Width = 12;
      this.Height = 12;
      this.Checked = true;
    }
    #endregion

    #region public ExtButtonType ExtButtonType
    public ExtButtonType ExtButtonType {
      get { return this._extButtonType; }
      set { 
        this._extButtonType = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public bool Checked
    public bool Checked {
      get { return this._checked; }
      set {
        bool evt = this._checked != value;

        this._checked = value;
        this.Invalidate();
        if (evt && this.CheckedChanged != null)
          this.CheckedChanged(this, new EventArgs());
      }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      Graphics g = e.Graphics;

      if (this._onMouseEnter) {
        g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), 0, 0, this.Width, this.Height);
        g.DrawRectangle(new Pen(_activeColor), 0, 0, this.Width - 1, this.Height - 1);
      }

      Pen pen = new Pen(Color.Black, 1);
      int x1 = 3, y1 = 3;
      int x2 = this.Width - 3, y2 = this.Height - 3;

      switch (_extButtonType) {
        case ExtButtonType.Close:
          g.DrawLine(pen, x1, y1, x2, y2);
          g.DrawLine(pen, x1, y2, x2, y1);
          break;
        case ExtButtonType.Hide:
          if (this.Checked) {
            g.DrawLine(pen, x1, y1, x2, y1);
          } else {
            g.DrawRectangle(pen, x1, y1, x2 - x1, y2 - y1);
          }
          break;
        case ExtButtonType.Property:
          pen = new Pen(Color.Black, 1);
          g.DrawLine(pen, x1, y1, x2, y1);
          g.DrawLine(pen, x1, y1 + 2, x2, y1 + 2);
          g.DrawLine(pen, x1, y1 + 4, x2, y1 + 4);
          g.DrawLine(pen, x1, y1 + 6, x2, y1 + 6);
          break;
      }
    }
    #endregion

    #region protected override void OnMouseEnter(EventArgs e)
    protected override void OnMouseEnter(EventArgs e) {
      base.OnMouseEnter(e);
      this._onMouseEnter = true;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnMouseLeave(EventArgs e)
    protected override void OnMouseLeave(EventArgs e) {
      base.OnMouseLeave(e);
      this._onMouseEnter = false;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnClick(EventArgs e)
    protected override void OnClick(EventArgs e) {
      base.OnClick(e);
      if (this.ExtButtonType == ExtButtonType.Hide) {
        this.Checked = !this.Checked;
      }
    }
    #endregion
  }
}
