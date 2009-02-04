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
using System.Runtime.InteropServices;
using Gordago.Analysis.Chart;

namespace Gordago.Windows.Forms {

  /// <summary>
  /// 
  /// </summary>
  class ToolStripControlManager : Control {

    private ToolStripItemPanel _tsiPanel;

    private Color _captionOnFocusColor = Color.Blue, _captionOffFocusColor = Color.DarkMagenta;
    private Brush _captionOnFocusBrush, _captionOffForusBrush;

    private Color _borderColor = Color.Silver;
    private Pen _borderPen;

    private Color _captionForeColor;
    private Brush _captionForeBrush;
    private Font _captionFont;
    private StringFormat _sformat;

    public const int CaptionSize = 14;
    private Panel _panel;

    private ExtButton _extBtnClose;
    private const int MinimumHeight = 18;

    private Control _control;

    private Orientation _savedOrientation = Orientation.Horizontal;

    private bool _isFocused = false;

    public ToolStripControlManager(ToolStripItemPanel tsiPanel) {
      _tsiPanel = tsiPanel;

      this.CaptionOnFocusColor = Color.FromArgb(57, 125, 231);
      this.CaptionOffFocusColor = Color.Silver;
      this.CaptionForeColor = Color.White;
      
      this.BorderColor = Color.Silver;

      this._captionFont = new Font("Microsoft Sans Serif", 6.5F);
      _sformat = new StringFormat();
      _sformat.FormatFlags = StringFormatFlags.NoWrap;

      this.Padding = new Padding(0);
      this.Margin = new Padding(0);

      this.Dock = DockStyle.Fill;
      this.Size = new Size(100, 100);
      _panel = new Panel();

      _panel.Padding = new Padding(0);
      _panel.Margin = new Padding(0);

      _extBtnClose = new ExtButton();

      this._extBtnClose.Checked = true;
      this._extBtnClose.ExtButtonType = ExtButtonType.Close;
      this._extBtnClose.Name = "_extBtnClose";
      this._extBtnClose.Size = new Size(10, 10);
      this._extBtnClose.Click += new EventHandler(this._extBtnClose_Click);

      this.Controls.Add(_extBtnClose);
      this.Controls.Add(_panel);
      _savedOrientation = _tsiPanel.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
      this.UpdateOrientation();
    }

    #region public new bool IsFocused
    public new bool IsFocused {
      get { return this._isFocused; }
      set {
        bool evt = _isFocused != value;
        this._isFocused = value;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set {
        this._borderColor = value;
        this._borderPen = new Pen(value);
      }
    }
    #endregion

    //#region public new string Text
    //public new string Text {
    //  get { return _tsiPanel.Text; }
    //  set { _tsiPanel.Text = value; }
    //}
    //#endregion

    #region public Control Item
    public Control Item {
      get { return _control; }
      set {
        _panel.Controls.Clear();
        _control = value;
        if (_control == null)
          return;

        int w = this.Width - _panel.Width;
        int h = this.Height - _panel.Height;

        this.Size = new Size(w + value.Width, h + value.Height);

        _tsiPanel.DefaultSize = this.Size;
        int minw = Math.Max(w + value.MinimumSize.Width, 30);
        int minh = Math.Max(h + value.MinimumSize.Height, 30);
        this.MinimumSize = new Size(minw, minh);
        this._tsiPanel.MinimumSize = this.MinimumSize;
        //_tsiPanel.Text = _control.Text;
        _control.Dock = DockStyle.Fill;
        _panel.Controls.Add(value);
      }
    }
    #endregion

    #region public Color CaptionOnFocusColor
    public Color CaptionOnFocusColor {
      get { return _captionOnFocusColor; }
      set {
        this._captionOnFocusColor = value;
        this._captionOnFocusBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color CaptionOffFocusColor
    public Color CaptionOffFocusColor {
      get { return this._captionOffFocusColor; }
      set {
        this._captionOffFocusColor = value;
        this._captionOffForusBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color CaptionForeColor
    public Color CaptionForeColor {
      get { return this._captionForeColor; }
      set {
        _captionForeColor = value;
        _captionForeBrush = new SolidBrush(value);
      }
    }
    #endregion

    //#region private void _control_TextChanged(object sender, EventArgs e)
    //private void _control_TextChanged(object sender, EventArgs e) {
    //  _tsiPanel.Text = _control.Text;
    //  this.Invalidate();
    //}
    //#endregion

    #region private void _extBtnClose_Click(object sender, EventArgs e)
    private void _extBtnClose_Click(object sender, EventArgs e) {
      _tsiPanel.Visible = false;
    }
    #endregion

    #region private void UpdateOrientation()
    private void UpdateOrientation() {
      if (_savedOrientation == _tsiPanel.Orientation)
        return;
      _savedOrientation = _tsiPanel.Orientation;

      this.SuspendLayout();
      _panel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

      if (_tsiPanel.Orientation == Orientation.Vertical) {
        _sformat.Alignment = StringAlignment.Near;
        _sformat.LineAlignment = StringAlignment.Center;

        _panel.Location = new Point(3, CaptionSize + 3);
        _panel.Size = new Size(this.Width - 5, this.Height - 5 - CaptionSize);

        this._extBtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._extBtnClose.Location = new Point(this.Width - 14, 4);
      } else {

        _sformat.Alignment = StringAlignment.Near;
        _sformat.LineAlignment = StringAlignment.Center;


        _panel.Location = new Point(CaptionSize + 3, 3);
        _panel.Size = new Size(this.Width - 5 - CaptionSize, this.Height - 5);

        this._extBtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
        this._extBtnClose.Location = new Point(4,4);
      }
      this.ResumeLayout(true);
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      Graphics g = e.Graphics;
      Brush brush = this.IsFocused ? _captionOnFocusBrush : _captionOffForusBrush;

      this.UpdateOrientation();
      if (_tsiPanel.Orientation == Orientation.Vertical) {
        Rectangle rect = new Rectangle(2, 2, this.Width - 4, CaptionSize);
        g.FillRectangle(brush, rect);

        RectangleF rectF = new RectangleF(4, 2, this.Width - 20, CaptionSize - 1);
        g.DrawString(this._tsiPanel.Text, this._captionFont, this._captionForeBrush, rectF, _sformat);
        g.DrawRectangle(_borderPen, 2, 2, this.Width - 4, this.Height - 4);
      } else {
        Rectangle rect = new Rectangle(2, 2, CaptionSize, this.Height - 4);
        g.FillRectangle(brush, rect);

        g.DrawRectangle(_borderPen, 2, 2, this.Width - 4, this.Height - 4);

        RectangleF rectF = new RectangleF(-this.Height+3, 4, this.Height - 20, CaptionSize - 1);
        g.RotateTransform(-90, System.Drawing.Drawing2D.MatrixOrder.Append);
        g.DrawString(this._tsiPanel.Text, this._captionFont, this._captionForeBrush, rectF, _sformat);
        g.Flush();
      }
    }
    #endregion

    #region public MouseControlType GetMouseControlType()
    public MouseControlType GetMouseControlType() {

      Point p = this.PointToClient(Cursor.Position);
      DockStyle ds = this._tsiPanel.Parent.Dock;
      
      int ml = 4;
      Rectangle rect = new Rectangle(ml, ml, this.Width - ml * 2, CaptionSize);

      if (_tsiPanel.Orientation == Orientation.Horizontal) 
        rect = new Rectangle(ml, ml, CaptionSize, this.Height - ml * 2);

      if (p.X >= rect.X && p.Y >= rect.Y && p.X <= rect.X + rect.Width && p.Y <= rect.Y + rect.Height) {
        return MouseControlType.Move;
      } else if (p.X >= ml && p.X <= this.Width - ml && p.Y < ml && ds == DockStyle.Bottom) {
        return MouseControlType.Top;
      } else if (p.X > this.Width - ml && p.Y > ml && p.Y < this.Height - ml && ds == DockStyle.Left) {
        return MouseControlType.Right;
      } else if (p.X >= ml && p.X <= this.Width - ml && p.Y > this.Height - ml && ds == DockStyle.Top) {
        return MouseControlType.Bottom;
      } else if (p.X < ml && p.Y < this.Height - ml && p.Y > ml && ds == DockStyle.Right) {
        return MouseControlType.Left;
      } else {
        return MouseControlType.None;
      }
    }
    #endregion

  }
  #region  ToolStripControlHostPanel:ToolStripControlHost
  class ToolStripControlHostPanel : ToolStripControlHost {

    public ToolStripControlHostPanel(ToolStripControlManager tsmng)
      : base(tsmng) {
      this.Margin = new Padding(0);
      this.Padding = new Padding(0);
    }

    public ToolStripControlHostPanel(ToolStripControlManager tsmng, string name)
      : this(tsmng) {
      this.Name = name;
    }
  }
  #endregion
}
