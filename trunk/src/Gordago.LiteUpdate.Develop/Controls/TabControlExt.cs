/**
* @version $Id: TabControlExt.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Controls
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.Drawing;
  using System.ComponentModel;

  public class TabControlExt : TabControl {

    private Rectangle[] _rectItems;
    private Rectangle _itemsRect, _tabPagesRect;
    private Color _backColor, _backColorTabPages;
    private Brush _backBrush, _backBrushTabPages;
    private Color _borderColor;
    private Pen _borderPen;
    private bool _borderVisible;
    private StringFormat _sformat;
    private Color _foreColor;

    public TabControlExt()
      : base() {
      this.DrawMode = TabDrawMode.OwnerDrawFixed;
      _rectItems = new Rectangle[0];
      this.BackColor = Color.FromArgb(244, 242, 232);
      this.BackColorTabPages = Color.FromArgb(252, 252, 254); ;
      this.BorderColor = Color.FromArgb(172, 168, 153);
      this.BorderVisible = false;
      this.ForeColor = Color.Black;
      _sformat = new StringFormat();
      _sformat.Alignment = StringAlignment.Center;
      _sformat.LineAlignment = StringAlignment.Center;
    }

    #region public new Color ForeColor
    public new Color ForeColor {
      get { return this._foreColor; }
      set {
        this._foreColor = value;
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

    #region public new Color BackColor
    public new Color BackColor {
      get { return _backColor; }
      set {
        _backColor = value;
        _backBrush = new SolidBrush(value);
        this.Invalidate();
      }
    }
    #endregion

    #region public Color BackColorTabPages
    public Color BackColorTabPages {
      get { return this._backColorTabPages; }
      set {
        this._backColorTabPages = value;
        this._backBrushTabPages = new SolidBrush(value);
        this.Invalidate();
      }
    }
    #endregion

    #region private new TabDrawMode DrawMode
    [Browsable(false)]
    private new TabDrawMode DrawMode {
      get { return base.DrawMode; }
      set { base.DrawMode = value; }
    }
    #endregion

    #region protected override void OnDrawItem(DrawItemEventArgs e)
    protected override void OnDrawItem(DrawItemEventArgs e) {
      if (_rectItems.Length != this.TabPages.Count)
        _rectItems = new Rectangle[this.TabPages.Count];

      int i = e.Index;
      if (i < 0 || i >= this.TabPages.Count) return;

      //if (this.TabPages[i].BackColor != this.BackColorTabPages) 
      //  this.TabPages[i].BackColor = this.BackColorTabPages;

      _rectItems[i] = this.GetTabRect(i);

      if (i == 0) {
        _itemsRect = _rectItems[i];
        TabPage tp = this.TabPages[0];
        Point p = this.PointToClient(tp.PointToScreen(new Point(0, 0)));
        _tabPagesRect = tp.Bounds;
        _tabPagesRect.X = p.X;
        _tabPagesRect.Y = p.Y;
      } else {
        switch (this.Alignment) {
          case TabAlignment.Bottom:
          case TabAlignment.Top:
            _itemsRect.Width += _rectItems[e.Index].Width;
            break;
          case TabAlignment.Left:
          case TabAlignment.Right:
            _itemsRect.Height += _rectItems[e.Index].Height;
            break;
        }

      }
    }
    #endregion

    #region protected override void WndProc(ref Message m)
    protected override void WndProc(ref Message m) {

      if (m.Msg == 0x000F) { // WM_PAINT
        base.WndProc(ref m);
        Graphics g = Graphics.FromHwnd(this.Handle);
        g.Clear(this.BackColorTabPages);
        this.PaintExt(g);
        return;
      }
      base.WndProc(ref m);
    }
    #endregion

    #region private void PaintExt(Graphics g)
    private void PaintExt(Graphics g) {
      if (_rectItems.Length == 0 || this.TabPages.Count == 0 || _rectItems.Length != this.TabPages.Count)
        return;

      int index = this.SelectedIndex;
      if (index >= 0) {
        Rectangle rect = _rectItems[index];
        List<Point> line = new List<Point>();
        List<Point> polygon = new List<Point>();

        int x1 = rect.X, x2 = rect.Right, y1 = rect.Y, y2 = rect.Bottom;

        switch (this.Alignment) {
          #region case TabAlignment.Bottom:
          case TabAlignment.Bottom:
            line.Add(new Point(0, y1));
            line.Add(new Point(x1, y1));
            line.Add(new Point(x1, y2 - 2));
            line.Add(new Point(x1 + 2, y2));
            line.Add(new Point(x2 - 2, y2));
            line.Add(new Point(x2, y2 - 2));
            line.Add(new Point(x2, y1));
            line.Add(new Point(this.Width, y1));

            polygon.AddRange(line.ToArray());
            polygon.Add(new Point(this.Width, this.Height));
            polygon.Add(new Point(0, this.Height));
            break;
          #endregion
          #region case TabAlignment.Top:
          case TabAlignment.Top:
            line.Add(new Point(0, y2));
            line.Add(new Point(x1, y2));
            line.Add(new Point(x1, y1 + 2));
            line.Add(new Point(x1 + 2, y1));
            line.Add(new Point(x2 - 2, y1));
            line.Add(new Point(x2, y1 + 2));
            line.Add(new Point(x2, y2));
            line.Add(new Point(this.Width, y2));

            polygon.AddRange(line.ToArray());
            polygon.Add(new Point(this.Width, 0));
            polygon.Add(new Point(0, 0));
            break;
          #endregion
          #region case TabAlignment.Right:
          case TabAlignment.Right:
            line.Add(new Point(x1, 0));
            line.Add(new Point(x1, y1));
            line.Add(new Point(x2 - 2, y1));
            line.Add(new Point(x2, y1 + 2));
            line.Add(new Point(x2, y2 - 2));
            line.Add(new Point(x2 - 2, y2));
            line.Add(new Point(x1, y2));
            line.Add(new Point(x1, Height));

            polygon.AddRange(line.ToArray());
            polygon.Add(new Point(this.Width, this.Height));
            polygon.Add(new Point(this.Width, 0));
            break;
          #endregion
          #region case TabAlignment.Left:
          case TabAlignment.Left:
            line.Add(new Point(x2, 0));
            line.Add(new Point(x2, y1));
            line.Add(new Point(x1 + 2, y1));
            line.Add(new Point(x1, y1 + 2));
            line.Add(new Point(x1, y2 - 2));
            line.Add(new Point(x1 + 2, y2));
            line.Add(new Point(x2, y2));
            line.Add(new Point(x2, Height));

            polygon.AddRange(line.ToArray());
            polygon.Add(new Point(0, this.Height));
            polygon.Add(new Point(0, 0));
            break;
          #endregion
        }
        g.FillPolygon(_backBrush, polygon.ToArray());
        g.DrawLines(_borderPen, line.ToArray());
      }

      for (int i = 0; i < _rectItems.Length; i++) {
        Rectangle rect = _rectItems[i];
        using (Brush brush = new SolidBrush(this.ForeColor)) {
          RectangleF rectF = new RectangleF(rect.X, _rectItems[i].Y, _rectItems[i].Width, _rectItems[i].Height);
          switch (this.Alignment) {
            case TabAlignment.Bottom:
            case TabAlignment.Top:
              g.DrawString(this.TabPages[i].Text, this.Font, brush, rectF, _sformat);
              break;
            case TabAlignment.Left:
            case TabAlignment.Right:
              this.DrawVerticalString(g, this.TabPages[i].Text, rect.X, rect.Y, rect.Width, rect.Height);
              break;
          }
        }
        if (i > 0) {
          switch (this.Alignment) {
            case TabAlignment.Bottom:
            case TabAlignment.Top:
              g.DrawLine(_borderPen, rect.X, rect.Y + 3, rect.X, rect.Bottom - 2);
              break;
            case TabAlignment.Left:
            case TabAlignment.Right:
              if (rect.Y > 10)
                g.DrawLine(_borderPen, rect.X + 2, rect.Y, rect.Right - 3, rect.Y);
              break;
          }
        }
      }
      if (BorderVisible)
        g.DrawRectangle(_borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }
    #endregion

    #region private void DrawVerticalString(Graphics g, string s, int x, int y, int width, int height)
    private void DrawVerticalString(Graphics g, string s, int x, int y, int width, int height) {
      Bitmap bitmap = new Bitmap(width, height);
      Graphics gbm = Graphics.FromImage(bitmap);

      RectangleF rect = new RectangleF(-height, 0, height, width);
      gbm.RotateTransform(-90, System.Drawing.Drawing2D.MatrixOrder.Append);
      gbm.DrawString(s, this.Font, new SolidBrush(this.ForeColor), rect, _sformat);
      gbm.Flush();
      g.DrawImageUnscaled(bitmap, x, y);
    }
    #endregion
  }
}
