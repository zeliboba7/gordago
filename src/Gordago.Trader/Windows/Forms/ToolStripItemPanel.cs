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
  [Flags]
  enum MoveBorder:short {
    None = 1,
    Left = 2,
    Top = 4,
    Right = 8,
    Bottom = 16
  }

  public class ToolStripItemPanel : ToolStrip {

    private ToolStripControlManager _tscm;
    private MoveBorder _moveBorder = MoveBorder.Bottom | MoveBorder.Left | MoveBorder.Right | MoveBorder.Top;

    private string _text;

    /// <summary>
    /// Стартовое смещение двух точек панели
    /// </summary>
    private const int MoveLineSize = 5;
    private Size _defaultSize;

    public ToolStripItemPanel():base() {
      this.Dock = DockStyle.Fill;
      this.Padding = new Padding(0);
      this.Margin = new Padding(0);
      this.GripMargin = new Padding(0);
      this.GripStyle = ToolStripGripStyle.Hidden;
      this.AutoSize = false;
      this.LayoutStyle = ToolStripLayoutStyle.Flow;

      _tscm = new ToolStripControlManager(this);

      this.Items.Add(new ToolStripControlHostPanel(_tscm));

      this.Text = "Panel";
      _defaultSize = this.Size = new Size(130,50);
      this.MinimumSize = new Size(50, 50);
      base.MinimumSize = new Size(40, 40);

      this.SetStyle(ControlStyles.UserPaint, true);
    }

    #region internal Size DefaultSize
    internal Size DefaultSize {
      get { return _defaultSize; }
      set { _defaultSize = value; }
    }
    #endregion

    #region internal new bool IsFocused
    internal new bool IsFocused {
      get { return _tscm.IsFocused; }
      set { this._tscm.IsFocused = value; }
    }
    #endregion

    #region public ToolStripPanel ToolStripPanel
    public ToolStripPanel ToolStripPanel {
      get {
        return this.Parent as ToolStripPanel;
      }
    }
    #endregion

    #region public new string Text
    public new string Text {
      get { return this._text; }
      set { 
        this._text = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Control Item
    public Control Item {
      get {  return _tscm.Item; }
      set {
        this._tscm.Item = value;
        if (value != null) {
          _defaultSize = value.Size;
          this._tscm.MinimumSize = value.MinimumSize;
        }
      }
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this._tscm.Size = this.Size;
      this._tscm.Invalidate();
    }
    #endregion

    #region internal MouseControlType GetMouseControlType()
    internal MouseControlType GetMouseControlType() {
      if (this.Parent == null)
        return MouseControlType.None;

      Point p = this.PointToClient(Cursor.Position);
      DockStyle ds = this.Parent.Dock;

      int ml = 4;
      Rectangle rect = new Rectangle(ml, ml, this.Width - ml * 2, ToolStripControlManager.CaptionSize);

      if (this.Orientation == Orientation.Horizontal)
        rect = new Rectangle(ml, ml, ToolStripControlManager.CaptionSize, this.Height - ml * 2);

      if (p.X >= rect.X && p.Y >= rect.Y && p.X <= rect.X + rect.Width && p.Y <= rect.Y + rect.Height) {
        return MouseControlType.Move;
      } else if ((_moveBorder & MoveBorder.Top) == MoveBorder.Top && p.X >= ml && p.X <= this.Width - ml && p.Y < ml && ds == DockStyle.Bottom) {
        return MouseControlType.Top;
      } else if ((_moveBorder & MoveBorder.Right) == MoveBorder.Right && p.X > this.Width - ml && p.Y > ml && p.Y < this.Height - ml && ds == DockStyle.Left) {
        return MouseControlType.Right;
      } else if ((_moveBorder & MoveBorder.Bottom) == MoveBorder.Bottom && p.X >= ml && p.X <= this.Width - ml && p.Y > this.Height - ml && ds == DockStyle.Top) {
        return MouseControlType.Bottom;
      } else if ((_moveBorder & MoveBorder.Left) == MoveBorder.Left && p.X < ml && p.Y < this.Height - ml && p.Y > ml && ds == DockStyle.Right) {
        return MouseControlType.Left;
      } else {
        return MouseControlType.None;
      }
    }
    #endregion

  }
}
