/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace Gordago.Analysis.Chart {

  #region enum MouseControlType
  enum MouseControlType {
    None,
    Move,
    LeftTop, 
    Top, 
    RightTop,
    Right,
    RightBottom,
    Bottom,
    LeftBottom,
    Left
  }
  #endregion

  partial class ChartPanelContainer : UserControl {

    private const int MinimumHeight = 18;

    private MouseButtons _mouseButtonDown;

    private Color _borderColor;
    private Pen _borderPen;

    private Color _captionBackColor, _captionForeColor;
    private Brush _captionBackBrush,_captionForeBrush;
    private Font _captionFont;
    private StringFormat _sformat;

    private int _savedHeight;
    private Point _savedMousePoint;
    private MouseControlType _mouseControlType = MouseControlType.None;

    private ChartPanel _chartPanel;
    private bool _resizeFromPanel = false;

    public ChartPanelContainer(ChartPanel chartPanel) {
      _chartPanel = chartPanel;
      Rectangle rect = chartPanel.Bounds;
      InitializeComponent();
      chartPanel.Bounds = rect;
      this.BorderColor = Color.Black;
      this.CaptionBackColor = Color.FromArgb(49, 106, 197);
      this.CaptionForeColor = Color.White;
      this._captionFont = new Font("Microsoft Sans Serif", 6.5F);

      this.SetStyle(ControlStyles.ResizeRedraw, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.DoubleBuffer, true);

      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Near;
      _sformat.LineAlignment = StringAlignment.Center;
      _sformat.FormatFlags = StringFormatFlags.NoWrap;

      this.Anchor = chartPanel.BaseAnchor;
      chartPanel.BaseAnchor = AnchorStyles.None;

      this.Location = chartPanel.BaseLocation;
      chartPanel.BaseLocation = new Point(1, MinimumHeight - 1);

      this.CheckSizeFromPanel();

      this.Controls.Add(_chartPanel);
      _chartPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;

      this.Text = _chartPanel.Text;
      this.MinimumSize = new Size(40, MinimumHeight);
    }

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set { 
        this._borderColor = value;
        if (_borderPen == null)
          _borderPen = new Pen(value);
        _borderPen.Color = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Color CaptionBackColor
    public Color CaptionBackColor {
      get { return this._captionBackColor; }
      set {
        this._captionBackColor = value;
        this._captionBackBrush = new SolidBrush(value);
        this._extBtnClose.BackColor = value;
        this._extBtnHide.BackColor = value;
        this._extBtnProperty.BackColor = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Color CaptionForeColor
    public Color CaptionForeColor {
      get { return this._captionForeColor; }
      set {
        this._captionForeColor = value;
        this._captionForeBrush = new SolidBrush(value);
        this.Invalidate();
      }
    }
    #endregion

    #region public bool IsMaximized
    public bool IsMaximized {
      get { return this._extBtnHide.Checked; }
      set { 
        this._extBtnHide.Checked = value; 
      }
    }
    #endregion

    #region public ChartPanelBorderStyle BorderStyle
    public new ChartPanelBorderStyle BorderStyle {
      get { return this._chartPanel.BorderStyle; }
      set { this._chartPanel.BorderStyle = value; }
    }
    #endregion

    #region private void _extBtnHide_CheckedChanged(object sender, EventArgs e)
    private void _extBtnHide_CheckedChanged(object sender, EventArgs e) {
      if (this.IsMaximized) {
        // this.Height = this._savedHeight;
        this._chartPanel.BaseVisible = true;
        this.CheckSizeFromPanel();
      } else {
        /* Сворачивание панели */
        this._chartPanel.BaseVisible = false;
        this._chartPanel.Anchor = AnchorStyles.None;
        this.Height = MinimumHeight;
      }
      this.Refresh();
    }
    #endregion

    #region private void _extBtnProperty_Click(object sender, EventArgs e)
    private void _extBtnProperty_Click(object sender, EventArgs e) {
      this._chartPanel.ShowProperty();
    }
    #endregion

    #region private void _extBtnClose_Click(object sender, EventArgs e)
    private void _extBtnClose_Click(object sender, EventArgs e) {
      this._chartPanel.Close();
    }
    #endregion

    #region private void CheckSizeFromPanel()
    /// <summary>
    /// Выравниваем размер по внутреннему контролу
    /// </summary>
    public void CheckSizeFromPanel() {
      _resizeFromPanel = true;

      int wnew = _chartPanel.Width + 2;
      int hnew = _chartPanel.Height + MinimumHeight;

      if (this.Size.Width != wnew || this.Size.Height != hnew) {
        this.Size = new Size(wnew, hnew);
      }

      int x = 1;
      int y = MinimumHeight - 1;
      if (_chartPanel.BaseLocation.X != x || _chartPanel.BaseLocation.Y != y) {
        _chartPanel.BaseLocation = new Point(x, y);
      }
      _resizeFromPanel = false;
    }
    #endregion

    #region private void CheckSizeFromManager()
    /// <summary>
    /// Выравниваем размер по внешнему контролу
    /// </summary>
    internal void CheckSizeFromManager() {

      if (_resizeFromPanel)
        return;

      int x = 1;
      int y = MinimumHeight - 1;
      if (_chartPanel.BaseLocation.X != x || _chartPanel.BaseLocation.Y != y) {
        _chartPanel.BaseLocation = new Point(x, y);
      }

      int wnew = Math.Max(this.Size.Width - 2, 1);
      int hnew = Math.Max(this.Size.Height - MinimumHeight, 1);
      if (this._chartPanel.Width != wnew || this._chartPanel.Height != hnew) {
        _chartPanel.Size = new Size(wnew, hnew);
      }
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      if (this.Height < MinimumHeight)
        this.Height = MinimumHeight;

      if (this.IsMaximized) {
        this._savedHeight = this.Height;
        this.CheckSizeFromManager();
      } else
        this.Height = MinimumHeight;
    }
    #endregion

    #region protected override void OnMouseMove(MouseEventArgs e)
    protected override void OnMouseMove(MouseEventArgs e) {
      base.OnMouseMove(e);

      if (e.Button == MouseButtons.None) {
        Point p = e.Location;
        if (p.X >= 4 && p.Y >= 4 && p.X <= this.Width - 36 && p.Y <= 16) {
          _mouseControlType = MouseControlType.Move;
          this.Cursor = Cursors.Default;
        } else if (p.X < 4 && p.Y < 4 && IsMaximized) {
          _mouseControlType = MouseControlType.LeftTop;
          this.Cursor = Cursors.SizeNWSE;
        } else if (p.X >= 4 && p.X <= this.Width - 36 && p.Y < 4 && IsMaximized) {
          _mouseControlType = MouseControlType.Top;
          this.Cursor = Cursors.SizeNS;
        } else if (p.X > this.Width - 4 && p.Y < 4 && IsMaximized) {
          _mouseControlType = MouseControlType.RightTop;
          this.Cursor = Cursors.SizeNESW;
        } else if (p.X > this.Width - 4 && p.Y > 4 && p.Y < this.Height - 4) {
          _mouseControlType = MouseControlType.Right;
          this.Cursor = Cursors.SizeWE;
        } else if (p.X > this.Width - 4 && p.Y > this.Height - 4 && IsMaximized) {
          _mouseControlType = MouseControlType.RightBottom;
          this.Cursor = Cursors.SizeNWSE;
        } else if (p.X >= 4 && p.X <= this.Width - 4 && p.Y > this.Height - 4 && IsMaximized) {
          _mouseControlType = MouseControlType.Bottom;
          this.Cursor = Cursors.SizeNS;
        } else if (p.X < 4 && p.Y > this.Height - 4 && IsMaximized) {
          _mouseControlType = MouseControlType.LeftBottom;
          this.Cursor = Cursors.SizeNESW;
        } else if (p.X < 4 && p.Y < this.Height - 4 &&p.Y>4) {
          _mouseControlType = MouseControlType.Left;
          this.Cursor = Cursors.SizeWE;
        } else {
          _mouseControlType = MouseControlType.None;
          this.Cursor = Cursors.Default;
        }
      } else if (e.Button == MouseButtons.Left) {
        #region Перемещение, изменение размеров панели
        Point pp = this.PointToScreen(e.Location);

        int dx = pp.X - _savedMousePoint.X;
        int dy = pp.Y - _savedMousePoint.Y;

//        _thisresize = true;
        switch (_mouseControlType) {
          case MouseControlType.Move:
            this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            break;
          case MouseControlType.LeftTop:
            this.Location = new Point(this.Location.X + dx, this.Location.Y + dy);
            this.Size = new Size(this.Size.Width - dx, this.Size.Height - dy);
            break;
          case MouseControlType.Top:
            this.Location = new Point(this.Location.X, this.Location.Y + dy);
            this.Size = new Size(this.Size.Width, this.Size.Height - dy);
            break;
          case MouseControlType.RightTop:
            this.Location = new Point(this.Location.X, this.Location.Y + dy);
            this.Size = new Size(this.Size.Width + dx, this.Size.Height - dy);
            break;
          case MouseControlType.Right:
            this.Location = new Point(this.Location.X, this.Location.Y);
            this.Size = new Size(this.Size.Width + dx, this.Size.Height);
            break;
          case MouseControlType.RightBottom:
            this.Location = new Point(this.Location.X, this.Location.Y);
            this.Size = new Size(this.Size.Width + dx, this.Size.Height + dy);
            break;
          case MouseControlType.Bottom:
            this.Location = new Point(this.Location.X, this.Location.Y);
            this.Size = new Size(this.Size.Width, this.Size.Height + dy);
            break;
          case MouseControlType.LeftBottom:
            this.Location = new Point(this.Location.X + dx, this.Location.Y);
            this.Size = new Size(this.Size.Width - dx, this.Size.Height + dy);
            break;
          case MouseControlType.Left:
            this.Location = new Point(this.Location.X + dx, this.Location.Y);
            this.Size = new Size(this.Size.Width - dx, this.Size.Height);
            break;
        }
//        _thisresize = false;
        this.CheckSizeFromManager();
        this._savedMousePoint = pp;
        this.Invalidate();
        #endregion
      }
    }
    #endregion

    #region protected override void OnMouseDown(MouseEventArgs e)
    protected override void OnMouseDown(MouseEventArgs e) {
      base.OnMouseDown(e);
      Point pp = this.PointToScreen(e.Location);
      _savedMousePoint = pp;
      _mouseButtonDown = e.Button;
    }
    #endregion

    #region protected override void OnMouseUp(MouseEventArgs e)
    protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);
      _mouseControlType = MouseControlType.None;
      _mouseButtonDown = MouseButtons.None;
    }
    #endregion

    #region protected override void OnMouseLeave(EventArgs e)
    protected override void OnMouseLeave(EventArgs e) {
      base.OnMouseLeave(e);
      if (_mouseButtonDown != MouseButtons.Left) {
        this.Cursor = Cursors.Default;
      }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {

      //if (_bitmap.Width!=this.Width)
      Graphics g = e.Graphics;
      g.InterpolationMode = InterpolationMode.Low;
      g.SmoothingMode = SmoothingMode.None;
      g.CompositingMode = CompositingMode.SourceOver;
      g.CompositingQuality = CompositingQuality.HighSpeed;
      g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

      g.FillRectangle(_captionBackBrush, 2, 2, this.Width - 4, MinimumHeight-4);

      g.DrawRectangle(_borderPen, 0, 0, this.Width - 1, this.Height - 1);
      string text = this.Text;
      if (this._chartPanel != null)
        text = _chartPanel.Text;

      g.DrawString(text, this._captionFont, this._captionForeBrush, new RectangleF(16, 1, this.Width - 36, MinimumHeight - 4), _sformat);
    }
    #endregion

    #region protected override void OnLocationChanged(EventArgs e)
    protected override void OnLocationChanged(EventArgs e) {
      base.OnLocationChanged(e);
      this.Refresh();
      if (Parent != null)
        this.Parent.Refresh();
    }
    #endregion
  }
}
