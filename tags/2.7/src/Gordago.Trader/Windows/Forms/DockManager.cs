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
using Gordago.Analysis.Chart;
using System.Xml;

namespace Gordago.Windows.Forms {

  interface IToolStripManager {
    MouseControlType GetMouseControlType();
  }

  public class DockManager:IMessageFilter {

    #region struct RowPanel
    struct RowPanel{
      public ToolStripPanel Panel;
      public int NumRow;
      
      /// <summary>
      /// true - Вхождение в панель, false - за пределами ее
      /// </summary>
      public bool InPanel;

      public RowPanel(ToolStripPanel panel, int numRow, bool inPanel) {
        Panel = panel;
        NumRow = numRow;
        InPanel = inPanel;
      }
    }
    #endregion

    private Form _form;

    private ToolStripPanel _tspLeft, _tspTop, _tspRight, _tspBottom;
    private Size _savedSizeLeft, _savedSizeRigh, _savedSizeTop, _savedSizeBottom;

    private bool _firstActivated = false;

    private RectangleForm _rectForm = null;
    private List<ToolStripItemPanel> _tsiPanels;
    private List<ToolStripPanel> _tsPanels;
    private List<ToolStrip> _tStrips;
    private List<ToolStripItem> _tsItems;

    private ToolStripItemPanel _focusedTSIPanel = null;

    private MouseControlType _currentMType = MouseControlType.None;

    public DockManager(Form form) {
      Application.AddMessageFilter(this);
      _form = form;
      _form.SizeChanged += new EventHandler(this.Form_SizeChanged);
      
      
      _tsItems = new List<ToolStripItem>();
      _tStrips = new List<ToolStrip>();
      _tsiPanels = new List<ToolStripItemPanel>();
      _tsPanels = new List<ToolStripPanel>();

      for (int i = 0; i < _form.Controls.Count; i++) {
        if (_form.Controls[i] is ToolStripPanel)
          this.AddTSPanel(_form.Controls[i] as ToolStripPanel);
      }
    }

    #region public Cursor Cursor
    public Cursor Cursor {
      get { return _form.Cursor; }
      set { 
        _form.Cursor = value;
      }
    }
    #endregion

    #region private MouseEventArgs GetMouseEvents()
    private MouseEventArgs GetMouseEvents() {
      Point p = _form.PointToClient(Cursor.Position);
      return new MouseEventArgs(Control.MouseButtons, 1, p.X, p.Y, 1);
    }
    #endregion

    #region public bool PreFilterMessage(ref Message m)
    public bool PreFilterMessage(ref Message m) {
      switch (m.Msg) {
        case WMsg.WM_LBUTTONDOWN:
          this.MouseDown(GetMouseEvents());
          break;
        case WMsg.WM_LBUTTONUP:
          this.MouseUp(GetMouseEvents());
          break;
        case WMsg.WM_MOUSEMOVE:
          this.MouseMove(GetMouseEvents());
          break;
        case WMsg.WM_MOUSELEAVE:
          break;
      }
      return false;
    }
    #endregion

    #region private void AddTSPanel(ToolStripPanel tsPanel)
    private void AddTSPanel(ToolStripPanel tsPanel) {
      // tsPanel.SizeChanged += new EventHandler(this.TSPanel_SizeChanged);
      tsPanel.ControlAdded += new ControlEventHandler(tsPanel_ControlAdded);
      tsPanel.SizeChanged += new EventHandler(tsPanel_SizeChanged);
      if (tsPanel.Dock == DockStyle.Top)
        _tspTop = tsPanel;
      else if (tsPanel.Dock == DockStyle.Right)
        _tspRight = tsPanel;
      else if (tsPanel.Dock == DockStyle.Bottom)
        _tspBottom = tsPanel;
      else if (tsPanel.Dock == DockStyle.Left)
        _tspLeft = tsPanel;

      for (int i = 0; i < tsPanel.Controls.Count; i++) {
        if (tsPanel.Controls[i] is ToolStrip) {

          ToolStrip ts = tsPanel.Controls[i] as ToolStrip;
          AddToolStrip(ts);
        }
      }
      _tsPanels.Add(tsPanel);
      this.UpdateLayoutPanels();
    }
    #endregion

    #region private void AddToolStrip(ToolStrip ts)
    private void AddToolStrip(ToolStrip ts) {
      for (int i = 0; i < _tStrips.Count; i++) {
        if (_tStrips[i] == ts)
          return;
      }
      _tStrips.Add(ts);
      for (int j = 0; j < ts.Items.Count; j++) {
        _tsItems.Add(ts.Items[j]);
      }
    }
    #endregion

    #region void tsPanel_ControlAdded(object sender, ControlEventArgs e)
    void tsPanel_ControlAdded(object sender, ControlEventArgs e) {
      if (e.Control is ToolStrip)
        this.AddToolStrip(e.Control as ToolStrip);
    }
    #endregion

    #region void tsPanel_SizeChanged(object sender, EventArgs e)
    void tsPanel_SizeChanged(object sender, EventArgs e) {
      this.UpdateLayoutPanels();
    }
    #endregion

    #region private void Form_SizeChanged(object sender, EventArgs e)
    private void Form_SizeChanged(object sender, EventArgs e) {
      this.UpdateLayoutPanels();
    }
    #endregion

    #region private Size[] GetEmptySize(ToolStripPanel panel, ToolStrip itemIgnore, int numRow)
    private Size[] GetEmptySize(ToolStripPanel panel, ToolStrip itemIgnore, int numRow) {
      int fullWidth = 0;
      int fullHeigh = 0;
      int minWidth = 0, minHeigh=0;

      ToolStripPanelRow row = panel.Rows[numRow];
      for (int i = 0; i < row.Controls.Length; i++) {
        if (row.Controls[i] != itemIgnore) {
          fullWidth += row.Controls[i].Width;
          fullHeigh += row.Controls[i].Height;
          minWidth += row.Controls[i].MinimumSize.Width;
          minHeigh += row.Controls[i].MinimumSize.Height;
        }
      }
      return new Size[] { new Size(minWidth, minHeigh), new Size(fullWidth, fullHeigh)};
    }
    #endregion

    #region private bool InRegion(Point p, Rectangle rect)
    private bool InRegion(Point p, Rectangle rect) {
      if (rect.X <= p.X && rect.Y <= p.Y && p.X <= rect.Right && p.Y <= rect.Bottom)
        return true;
      return false;
    }
    #endregion

    #region private Rectangle GetRegionOnScreen(Control c)
    private Rectangle GetRegionOnScreen(Control c) {
      return new Rectangle(c.PointToScreen(new Point(0, 0)), c.Size);
    }
    #endregion

    #region private ToolStripPanel GetToolStripPanel(Point p)
    private ToolStripPanel GetToolStripPanel(Point p) {
      if (InRegion(p, this.GetRegionOnScreen(_tspLeft)))
        return _tspLeft;

      if (InRegion(p, this.GetRegionOnScreen(_tspTop)))
        return _tspTop;

      if (InRegion(p, this.GetRegionOnScreen(_tspRight)))
        return _tspRight;

      if (InRegion(p, this.GetRegionOnScreen(_tspBottom)))
        return _tspBottom;

      return null;
    }
    #endregion

    #region private int GetNumRow(ToolStripPanel tsp, Point p)
    /// <summary>
    /// Получить номер строки на панели
    /// </summary>
    /// <param name="tsp"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    private int GetNumRow(ToolStripPanel tsp, Point p) {
      Rectangle rect = this.GetRegionOnScreen(tsp);
      if (InRegion(p, rect)) {
        for (int i = 0; i < tsp.Rows.Length; i++) {
          ToolStripPanelRow row = tsp.Rows[i];
          Rectangle bounds = new Rectangle(tsp.PointToScreen(row.Bounds.Location), row.Bounds.Size);
          if (InRegion(p, bounds)) {
            return i;
          }
        }
      } else {
        DockStyle posType = this.GetNearControlType(tsp, p);
        if (tsp.Orientation == Orientation.Horizontal) {
          switch (posType) {
            case DockStyle.Top:
              return -1;
            case DockStyle.Bottom:
              return tsp.Rows.Length;
            case DockStyle.Left:
            case DockStyle.Right:
              if (p.Y < rect.Y)
                return -1;
              else if (p.Y > rect.Bottom)
                return tsp.Rows.Length;
              return this.GetNumRow(tsp, new Point(rect.X + 1, p.Y));
          }
        } else {
          switch(posType){
            case DockStyle.Left:
              return -1;
            case DockStyle.Right:
              return tsp.Rows.Length + 1;
            case DockStyle.Top:
            case DockStyle.Bottom:
              if (p.X < rect.X)
                return -1;
              else if (p.X>rect.Right)
                return tsp.Rows.Length + 1;
              return this.GetNumRow(tsp, new Point(p.X, rect.Y + 1));
          }

        }
      }
      return 0;
    }
    #endregion

    #region private Size GetNearControlsSize(Control c, Point p)
    /// <summary>
    /// Расстояние до контрола
    /// </summary>
    /// <param name="c"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    private Size GetNearControlsSize(Control c, Point p) {
      Rectangle rect = this.GetRegionOnScreen(c);
      int w = 0;
      if (p.X >= rect.Left && p.X <= rect.Right) {
        w = 0;
      } else if (p.X < rect.Left) {
        w = rect.Left - p.X;
      } else if (p.X > rect.Right) {
        w = p.X - rect.Right;
      }
      int h = 0;
      if (p.Y >= rect.Top && p.Y <= rect.Bottom) {
        h = 0;
      } else if (p.Y < rect.Top) {
        h = rect.Top - p.Y;
      } else if (p.Y > rect.Bottom) {
        h = p.Y - rect.Bottom;
      }
      return new Size(w, h);
    }
    #endregion

    #region private DockStyle GetNearControlType(Control control, Point p)
    /// <summary>
    /// С какой стороны удален курсор относительно контрола
    /// </summary>
    /// <returns></returns>
    private DockStyle GetNearControlType(Control control, Point p) {
      Rectangle rect = this.GetRegionOnScreen(control);
      int w = 0;
      int l = 0, r = 0, t = 0, b = 0;
      
      if (p.X >= rect.Left && p.X <= rect.Right) {
        w = 0;
      } else if (p.X < rect.Left) {
        l = w = rect.Left - p.X;
      } else if (p.X > rect.Right) {
        r = w = p.X - rect.Right;
      }
      int h = 0;
      if (p.Y >= rect.Top && p.Y <= rect.Bottom) {
        h = 0;
      } else if (p.Y < rect.Top) {
        t = h = rect.Top - p.Y;
      } else if (p.Y > rect.Bottom) {
        b = h = p.Y - rect.Bottom;
      }

      if (l > 0 && b == 0 && t == 0) {
        return DockStyle.Left;
      } else if (l > 0 && t > 0) {
        return l < t ? DockStyle.Left : DockStyle.Top;
      } else if (t > 0 && l == 0 && r == 0) {
        return DockStyle.Top;
      } else if (t > 0 && r > 0) {
        return t < r ? DockStyle.Top : DockStyle.Right;
      } else if (r > 0 && t == 0 && b == 0) {
        return DockStyle.Right;
      } else if (r > 0 && b > 0) {
        return r < b ? DockStyle.Right : DockStyle.Bottom;
      } else if (b > 0 && l == 0 && r == 0) {
        return DockStyle.Bottom;
      } else if (l > 0 && b > 0) {
        return l < b ? DockStyle.Left : DockStyle.Bottom;
      }
      return DockStyle.Fill;
    }
    #endregion

    #region private ToolStripPanel GetNearToolStripPanel(ToolStripPanel[] panels, Point p)
    private ToolStripPanel GetNearToolStripPanel(ToolStripPanel[] panels, Point p) {
      Size size = new Size(10000, 10000);
      ToolStripPanel retTSP = null;
      foreach (ToolStripPanel tsp in panels) {
        Size tsize = this.GetNearControlsSize(tsp, p);
        int oldLenght = size.Width + size.Height;
        int newLenght = tsize.Width + tsize.Height;
        if (newLenght < oldLenght) {
          retTSP = tsp;
          size = tsize;
        }
      }
      return retTSP;
    }
    #endregion

    #region private RowPanel GetNumRow(Point p)
    private RowPanel GetNumRow(Point p) {
      int numRow = 0;
      bool inPanel = true;
      ToolStripPanel panel = null;

      if (InRegion(p, this.GetRegionOnScreen(_tspLeft))) {
        numRow = GetNumRow(_tspLeft, p);
        panel = _tspLeft;
      } else if (InRegion(p, this.GetRegionOnScreen(_tspTop))) {
        numRow = GetNumRow(_tspTop, p);
        panel = _tspTop;
      } else if (InRegion(p, this.GetRegionOnScreen(_tspRight))) {
        numRow = GetNumRow(_tspRight, p);
        panel = _tspRight;
      } else if (InRegion(p, this.GetRegionOnScreen(_tspBottom))) {
        numRow = GetNumRow(_tspBottom, p);
        panel = _tspBottom;
      } else {
        inPanel = false;
        panel = GetNearToolStripPanel(this._tsPanels.ToArray(), p);
        numRow = GetNumRow(panel, p);
      }

      return new RowPanel(panel, numRow, inPanel);
    }
    #endregion

    #region private Rectangle GetRectFromRow(ToolStripPanel panel,ToolStripItemPanel itemIgnore, int numRow)
    private Rectangle GetRectFromRow(ToolStripPanel panel,ToolStripItemPanel itemIgnore, int numRow) {
      Rectangle rect = this.GetRegionOnScreen(panel);
      if (numRow < 0) {
        if (panel.Orientation == Orientation.Horizontal) {
          rect.Height = itemIgnore.DefaultSize.Height;
        } else {
          rect.Width = itemIgnore.DefaultSize.Width;
        }
      } else if (numRow >= panel.Rows.Length) {
        if (panel.Orientation == Orientation.Horizontal) {
          rect.Y = rect.Bottom - itemIgnore.DefaultSize.Height;
          rect.Height = itemIgnore.DefaultSize.Height;
        } else {
          rect.X = rect.Right - itemIgnore.DefaultSize.Width;
          rect.Width = itemIgnore.DefaultSize.Width;
        }
      } else {
        rect = new Rectangle(panel.PointToScreen(panel.Rows[numRow].Bounds.Location), panel.Rows[numRow].Bounds.Size);
        if (panel.Orientation == Orientation.Horizontal) {
          int dx = rect.X + rect.Width / 2;
          if (Cursor.Position.X < dx) {
            rect.Width = rect.Width / 2;
          } else {
            rect.Width = rect.Right - dx;
            rect.X = dx;
          }
        } else {
          int dy = rect.Y + rect.Height / 2;
          if (Cursor.Position.Y < dy) {
            rect.Height = rect.Height / 2;
          } else {
            rect.Height = rect.Bottom - dy;
            rect.Y = dy;
          }
        }
      }
      return rect;
    }
    #endregion

    #region private Rectangle GetRectFormBounds(RowPanel rowPanel, ToolStripItemPanel itemIgnore, Point p)
    private Rectangle GetRectFormBounds(RowPanel rowPanel, ToolStripItemPanel itemIgnore, Point p) {
      Rectangle rect = this.GetRegionOnScreen(rowPanel.Panel);
      int numRow = rowPanel.NumRow;
      ToolStripPanel panel = rowPanel.Panel;
      Size emptySize = panel.Size;
      Size minSize = panel.Size;
      if (rowPanel.InPanel) { /* Курсор в регионе панели */
        Size[] sizes = this.GetEmptySize(panel, itemIgnore, numRow);
        minSize = sizes[0];
        emptySize = sizes[1];
        rect = new Rectangle(panel.PointToScreen(panel.Rows[numRow].Bounds.Location), panel.Rows[numRow].Bounds.Size);

        Rectangle rectItem = this.GetRegionOnScreen(itemIgnore);
        if (this.InRegion(Cursor.Position, rectItem)) {
          rect = rectItem;
        } else {
          rect = GetRectFromRow(panel, itemIgnore, numRow);
        }
      } else { /* Курсор за его пределами */
        DockStyle posType = this.GetNearControlType(panel, p);
        rect = GetRectFromRow(panel, itemIgnore, numRow);
      }

      return rect;
    }
    #endregion

    #region private void MouseDown(MouseEventArgs m)
    private void MouseDown(MouseEventArgs m) {
      if (m.Button == MouseButtons.Left) {

        ToolStripItemPanel tsip = this.GetTSIPanel();
        if (tsip == null) {
          //if (_focusedTSIPanel != null) {
          //  _focusedTSIPanel.IsFocused = false;
          //  _focusedTSIPanel = null;
          //}
          return;
        }
        if (_focusedTSIPanel != null && _focusedTSIPanel != tsip ) {
          _focusedTSIPanel.IsFocused = false;
        }
        _focusedTSIPanel = tsip;
        _focusedTSIPanel.IsFocused = true;

        MouseControlType mtype = tsip.GetMouseControlType();
        if (mtype == MouseControlType.None)
          return;

        ToolStripPanel panel = tsip.ToolStripPanel;
        Point pt = panel.PointToScreen(new Point(0, 0));
        Rectangle rectPanel = new Rectangle(pt.X, pt.Y, panel.Width, panel.Height);
        Rectangle rectItem = new Rectangle(tsip.PointToScreen(new Point(0,0)), new Size(tsip.Width, tsip.Height));
        Rectangle maxRegion = new Rectangle(_form.PointToScreen(new Point(_form.ClientRectangle.X, _form.ClientRectangle.Y)), _form.ClientSize);

        if (panel.Dock == DockStyle.Left) {
          Point pz = _tspRight.PointToScreen(new Point(0, 0));
          maxRegion.Location = pt;
          maxRegion.Size = new Size(pz.X - pt.X, panel.Height);

          rectPanel.X = rectItem.X;
          rectPanel.Width = rectItem.Width;
        } else if (panel.Dock == DockStyle.Top) {
          Point pz = _tspBottom.PointToScreen(new Point(0, 0));
          maxRegion.Location = pt;
          maxRegion.Size = new Size(panel.Width, pz.Y - pt.Y);

          rectPanel.Y = rectItem.Y;
          rectPanel.Height = rectItem.Height;
        } else if (panel.Dock == DockStyle.Right) {
          Point pz1 = _tspLeft.PointToScreen(new Point(_tspLeft.Width, _tspLeft.Height));
          Point pz2 = panel.PointToScreen(new Point(panel.Width, panel.Height));
          maxRegion.Location = pz1;
          maxRegion.Size = new Size(pz2.X - pz1.X, panel.Height);

          rectPanel.X = rectItem.X;
          rectPanel.Width = rectItem.Width;
        } else if (panel.Dock == DockStyle.Bottom) {
          Point pz1 = _tspTop.PointToScreen(new Point(_tspTop.Width, _tspTop.Height));
          Point pz2 = panel.PointToScreen(new Point(panel.Width, panel.Height));
          maxRegion.Location = pz1;
          maxRegion.Size = new Size(panel.Width, pz2.Y - pz1.Y);

          rectPanel.Y = rectItem.Y;
          rectPanel.Height = rectItem.Height;
        }

        ScreenRectTSP currentSRectTSP = new ScreenRectTSP(panel, rectPanel, maxRegion);

        Rectangle rect = rectPanel;
        DockStyle borderStyle = DockStyle.None;

        Point p = tsip.PointToScreen(new Point(0, 0));

        switch (mtype) {
          case MouseControlType.Move:
            //rect = new Rectangle(p.X, p.Y, tsip.Width, tsip.Height);
            //borderStyle = DockStyle.Fill;
            //Cursor = Cursors.SizeAll;
            break;
          case MouseControlType.Left:
            borderStyle = DockStyle.Left;
            break;
          case MouseControlType.Top:
            borderStyle = DockStyle.Top;
            break;
          case MouseControlType.Right:
            borderStyle = DockStyle.Right;
            break;
          case MouseControlType.Bottom:
            borderStyle = DockStyle.Bottom;
            break;
        }

        _rectForm = new RectangleForm(rect, borderStyle, currentSRectTSP, tsip, mtype);
        _rectForm.MinimumSize = tsip.MinimumSize;
        _rectForm.Show();
      }
    }
    #endregion

    #region private void MouseUp(MouseEventArgs m)
    private void MouseUp(MouseEventArgs m) {
      this.Cursor = Cursors.Default;

      if (_rectForm != null) {
        /* Фиксирование положения панели */
        switch (_rectForm.MouseType) {
          case MouseControlType.Left:
            _rectForm.TSIP.Width = _rectForm.Width;
            break;
          case MouseControlType.Top:
            _rectForm.TSIP.Height = _rectForm.Height;
            break;
          case MouseControlType.Right:
            _rectForm.TSIP.Width = _rectForm.Width;
            break;
          case MouseControlType.Bottom:
            _rectForm.TSIP.Height = _rectForm.Height;
            break;
          case MouseControlType.Move:
            
            //if (InRegion(p, this.GetRegionOnScreen(_tspLeft))) {

            //} else if (InRegion(p, this.GetRegionOnScreen(_tspTop))) {

            //} else if (InRegion(p, this.GetRegionOnScreen(_tspRight))) {

            //} else if (InRegion(p, this.GetRegionOnScreen(_tspBottom))) {

            //}
            break;
        }
        _rectForm.Close();
        _rectForm = null;
      }

      this.UpdateLayoutPanels();
    }
    #endregion

    #region private void MouseMove(MouseEventArgs m)
    private void MouseMove(MouseEventArgs m) {

      if (m.Button == MouseButtons.None) {

        ToolStripItemPanel tsip = this.GetTSIPanel();
        if (tsip != null) {
          MouseControlType mtype = tsip.GetMouseControlType();
          bool evt = _currentMType != mtype;
          _currentMType = mtype;
          if (evt) {
            switch (mtype) {
              case MouseControlType.LeftTop:
                this.Cursor = Cursors.SizeNWSE;
                break;
              case MouseControlType.Top:
                this.Cursor = Cursors.SizeNS;
                break;
              case MouseControlType.RightTop:
                this.Cursor = Cursors.SizeNESW;
                break;
              case MouseControlType.Right:
                this.Cursor = Cursors.SizeWE;
                break;
              case MouseControlType.RightBottom:
                this.Cursor = Cursors.SizeNWSE;
                break;
              case MouseControlType.Bottom:
                this.Cursor = Cursors.SizeNS;
                break;
              case MouseControlType.LeftBottom:
                this.Cursor = Cursors.SizeNESW;
                break;
              case MouseControlType.Left:
                this.Cursor = Cursors.SizeWE;
                break;
              default:
                this.Cursor = Cursors.Default;
                break;
            }
          }
        } else {
          if (_currentMType != MouseControlType.None) {
            _currentMType = MouseControlType.None;
            this.Cursor = Cursors.Default;
          }
        }

      } else if (m.Button == MouseButtons.Left && _rectForm != null) {
        Point p = _rectForm.TSIP.PointToScreen(new Point(0, 0));
        int crsDelta = 3;
        Rectangle bounds = _rectForm.Bounds;
        Rectangle region = _rectForm.SRTSP.Region;
        switch (_rectForm.MouseType) {
          case MouseControlType.Move:
            bounds.Location = new Point(Cursor.Position.X - _rectForm.DeltaLocation.Width, Cursor.Position.Y - _rectForm.DeltaLocation.Height);

            RowPanel rowPanel = this.GetNumRow(Cursor.Position);

            if (rowPanel.Panel != null) {
              bounds = this.GetRectFormBounds(rowPanel, _rectForm.TSIP, Cursor.Position);
            }
            break;
          #region case MouseControlType.Left:
          case MouseControlType.Left:
            bounds.X = Math.Max(Cursor.Position.X, region.X);
            bounds.X = Math.Min(bounds.X, region.X + region.Width - _rectForm.TSIP.MinimumSize.Width);
            bounds.Width = _rectForm.StartBounds.Right - bounds.X;
            break;
          #endregion
          #region case MouseControlType.Top:
          case MouseControlType.Top:
            bounds.Y = Math.Max(Cursor.Position.Y, region.Y);
            bounds.Y = Math.Min(bounds.Y, region.Y + region.Height - _rectForm.TSIP.MinimumSize.Height);
            bounds.Height = _rectForm.StartBounds.Bottom - bounds.Y;
            break;
          #endregion
          #region case MouseControlType.Right:
          case MouseControlType.Right:
            bounds.Width = Math.Min(Cursor.Position.X - _rectForm.StartBounds.X + crsDelta, _rectForm.SRTSP.Region.Width);
            break;
          #endregion
          #region case MouseControlType.Bottom:
          case MouseControlType.Bottom:
            bounds.Height = Math.Min(Cursor.Position.Y - _rectForm.StartBounds.Y + crsDelta, _rectForm.SRTSP.Region.Height);
            break;
          #endregion
        }
        _rectForm.Bounds = bounds;

      }
    }
    #endregion

    #region private void MouseEnter(EventArgs e)
    private void MouseEnter(EventArgs e) {
    }
    #endregion

    #region private void MouseLeave(EventArgs e)
    private void MouseLeave(EventArgs e) {
      //_currentTSIP = null;
      //this.Cursor = Cursors.Default;
    }
    #endregion

    #region private ToolStripItemPanel GetTSIPanel()
    /// <summary>
    /// Получить панель по текущей позиции курсора
    /// </summary>
    /// <returns></returns>
    private ToolStripItemPanel GetTSIPanel() {
      IntPtr ptr = User32.WindowFromPoint(Cursor.Position);
      Control ctrl = Control.FromHandle(ptr);
      if (ctrl == null)
        return null;
      if (ctrl.FindForm() != this._form)
        return null;

      for (int i = 0; i < _tsiPanels.Count; i++) {
        ToolStripItemPanel tsip = _tsiPanels[i];
        if (tsip.Visible) {
          Point p = tsip.PointToClient(Cursor.Position);
          if (p.X >= 0 && p.Y >= 0 && p.X <= tsip.Width && p.Y <= tsip.Height) {
            return tsip;
          }
        }
      }
      return null;
    }
    #endregion

    #region public void RegisterItemPanel(ToolStripItemPanel tsiPanel)
    /// <summary>
    /// Регистрация панели для управление
    /// </summary>
    /// <param name="tsiPanel"></param>
    public void RegisterItemPanel(ToolStripItemPanel tsiPanel) {
      _tsiPanels.Add(tsiPanel);
      tsiPanel.VisibleChanged += new EventHandler(this.ToolStripItemPanel_VisibleChanged);
    }
    #endregion

    #region private void ToolStripItemPanel_VisibleChanged(object sender, EventArgs e)
    private void ToolStripItemPanel_VisibleChanged(object sender, EventArgs e) {
      _savedSizeTop = new Size(0,0);
      _savedSizeBottom = new Size(0, 0);
      _savedSizeLeft = new Size(0, 0);
      _savedSizeRigh = new Size(0, 0);
      this.UpdateLayoutPanels();
    }
    #endregion

    #region public void RemoveItemPanel(ToolStripItemPanel tsiPanel)
    public void RemoveItemPanel(ToolStripItemPanel tsiPanel) {
      _tsiPanels.Remove(tsiPanel);
      tsiPanel.VisibleChanged -= new EventHandler(this.ToolStripItemPanel_VisibleChanged);
    }
    #endregion

    #region public XmlDocument SaveToXml()
    public XmlDocument SaveToXml() {

      bool ws = _form.WindowState == FormWindowState.Maximized;
      int w = 800, h = 600;
      if (!ws) {
        w = _form.Size.Width;
        h = _form.Size.Height;
      }

      string xmlstr = string.Format("<Gordago Version=\"1.1\" State=\"{0}\" X=\"{1}\" Y=\"{2}\" Width=\"{3}\" Height=\"{4}\"></Gordago>", ws.ToString(), _form.Location.X.ToString(), _form.Location.Y.ToString(), w.ToString(), h.ToString());

      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlstr);

      XmlNode node = doc.CreateElement("DockManager");
      doc.DocumentElement.AppendChild(node);


      for (int i = 0; i < _form.Controls.Count; i++) {
        if (_form.Controls[i] is ToolStripPanel) {
          ToolStrip[] items = SaveToXml(_form.Controls[i] as ToolStripPanel, node);
          foreach (ToolStrip ts in items){
            ToolStripItem[] tsis = SaveToXml(ts, node);
            foreach (ToolStripItem tsi in tsis) {
              SaveToXml(tsi, node);
            }
          }
        }
      }
      return doc;
    }
    #endregion

    #region private ToolStrip[] SaveToXml(ToolStripPanel tsPanel, XmlNode mainNode)
    private ToolStrip[] SaveToXml(ToolStripPanel tsPanel, XmlNode mainNode) {
      XmlNode node = mainNode.OwnerDocument.CreateElement("ToolStripPanel");
      mainNode.AppendChild(node);
      XmlNodeManager nodeM = new XmlNodeManager(node);
      nodeM.SetAttribute("Name", tsPanel.Name);
      
      List<ToolStrip> tss = new List<ToolStrip>();
      List<string> sa = new List<string>();
      for (int i = 0; i < tsPanel.Controls.Count; i++) {
        if (tsPanel.Controls[i] is ToolStrip) {
          ToolStrip ts = tsPanel.Controls[i] as ToolStrip;
          sa.Add(ts.Name);
          tss.Add(ts);
        }
      }
      string items = string.Join(",", sa.ToArray());
      nodeM.SetAttribute("Items", items);
      return tss.ToArray();
    }
    #endregion

    #region private ToolStripItem[] SaveToXml(ToolStrip ts, XmlNode mainNode)
    private ToolStripItem[] SaveToXml(ToolStrip ts, XmlNode mainNode) {

      XmlNode node = mainNode.OwnerDocument.CreateElement("ToolStrip");
      mainNode.AppendChild(node);
      XmlNodeManager nodeM = new XmlNodeManager(node);
      nodeM.SetAttribute("Name", ts.Name);
      nodeM.SetAttribute("X", ts.Bounds.X);
      nodeM.SetAttribute("Y", ts.Bounds.Y);
      nodeM.SetAttribute("Width", ts.Bounds.Width);
      nodeM.SetAttribute("Height", ts.Bounds.Height);
      nodeM.SetAttribute("Visible", ts.Visible);

      List<ToolStripItem> tsis = new List<ToolStripItem>();
      List<string> sa = new List<string>();
      if (!(ts is ToolStripItemPanel)) {
        for (int i = 0; i < ts.Items.Count; i++) {
          ToolStripItem tsi = ts.Items[i];
          tsis.Add(tsi);
          sa.Add(tsi.Name);
        }
      }
      string items = string.Join(",", sa.ToArray());
      nodeM.SetAttribute("Items", items);

      int row = GetNumRow(ts);
      nodeM.SetAttribute("Row", row);
      return tsis.ToArray();
    }
    #endregion

    #region private int GetNumRow(ToolStrip ts)
    private int GetNumRow(ToolStrip ts) {
      ToolStripPanel tsp = ts.Parent as ToolStripPanel;
      if (tsp != null) {
        for (int r = 0; r < tsp.Rows.Length; r++) {
          for (int c = 0; c < tsp.Rows[r].Controls.Length; c++) {
            if (tsp.Rows[r].Controls[c] == ts)
              return r;
          }
        }
      }
      return 0;
    }
    #endregion

    #region private void SaveToXml(ToolStripItem tsi, XmlNode mainNode)
    private void SaveToXml(ToolStripItem tsi, XmlNode mainNode) {
      XmlNode node = mainNode.OwnerDocument.CreateElement("ToolStripItem");
      mainNode.AppendChild(node);

      XmlNodeManager nodeM = new XmlNodeManager(node);
      nodeM.SetAttribute("Name", tsi.Name);
      nodeM.SetAttribute("X", tsi.Bounds.X);
      nodeM.SetAttribute("Y", tsi.Bounds.Y);
      nodeM.SetAttribute("Width", tsi.Bounds.Width);
      nodeM.SetAttribute("Height", tsi.Bounds.Height);
      nodeM.SetAttribute("Visible", tsi.Visible);
    }
    #endregion

    #region public void Load(XmlDocument xmlDoc)
    public void Load(XmlDocument xmlDoc) {

      XmlNode nodeG = xmlDoc["Gordago"];
      if (nodeG == null)
        return;


      /* Сохранение положения элементов панелей */
      List<SavedTSItem> savedTSItems = new List<SavedTSItem>();

      for (int i = 0; i < _tsPanels.Count; i++) {
        for (int j = 0; j < _tsPanels[i].Controls.Count; j++) {
          SavedTSItem stsi = new SavedTSItem(_tsPanels[i], _tsPanels[i].Controls[j] as ToolStrip);
          savedTSItems.Add(stsi);
        }
      }

      _form.SuspendLayout();
      for (int i = 0; i < _tsPanels.Count; i++) 
        _tsPanels[i].SuspendLayout();

      XmlNodeManager nodemG = new XmlNodeManager(nodeG);
      int x = nodemG.GetAttributeInt32("X", 0);
      int y = nodemG.GetAttributeInt32("Y", 0);
      int w = nodemG.GetAttributeInt32("Width", 800);
      int h = nodemG.GetAttributeInt32("Height", 600);
      bool state = nodemG.GetAttributeBoolean("State", true);
      _form.StartPosition = FormStartPosition.Manual;
      _form.Bounds = new Rectangle(x, y, w, h);
      if (state)
        _form.WindowState = FormWindowState.Maximized;

      XmlNode nodeDM = nodeG["DockManager"];
      if (nodeDM != null) {

        foreach (XmlNode node in nodeDM.ChildNodes) {
          if (node.Name == "ToolStripPanel") {

            XmlNodeManager nodem = new XmlNodeManager(node);
            string tsPanelName = nodem.GetAttributeString("Name", "");
            ToolStripPanel tsp = this.GetToolStripPanel(tsPanelName);
            if (tsp != null) {
              tsp.Controls.Clear();
              string[] TSNames = nodem.GetAttributeString("Items", "").Split(',');
              foreach (string tsName in TSNames) {
                ToolStrip ts = this.GetToolStrip(tsName);
                if (ts != null) {
                  foreach (XmlNode nodeTS in nodeDM.ChildNodes) {
                    XmlNodeManager nodemTS = new XmlNodeManager(nodeTS);
                    if (nodemTS.GetAttributeString("Name", "") == tsName) {
                      //tsp.Join(ts, row);
                      tsp.Controls.Add(ts);

                      x = nodemTS.GetAttributeInt32("X", 0);
                      y = nodemTS.GetAttributeInt32("Y", 0);
                      w = nodemTS.GetAttributeInt32("Width", 1);
                      h = nodemTS.GetAttributeInt32("Height", 1);
                      int row = nodemTS.GetAttributeInt32("Row", 0);
                      ts.Visible = nodemTS.GetAttributeBoolean("Visible", true);
                      ts.Location = new Point(x, y);
                      if (ts is ToolStripItemPanel) 
                        (ts as ToolStripItemPanel).Size = new Size(w, h);
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }

      for (int i = 0; i < savedTSItems.Count; i++) {
        SavedTSItem stsi = savedTSItems[i];
        if (this.GetToolStripPanel(stsi.Item) == null) {
          stsi.Panel.Controls.Add(stsi.Item);
        }
      }

      for (int i = 0; i < _tsPanels.Count; i++) {
        _tsPanels[i].ResumeLayout(false);
      }
      _form.ResumeLayout(false);
      this.UpdateLayoutPanels();
    }
    #endregion

    #region private ToolStripPanel GetToolStripPanel(ToolStrip ts)
    /// <summary>
    /// Получить панель, к которой принадлежит элемент ToolStrip
    /// </summary>
    /// <param name="ts"></param>
    /// <returns></returns>
    private ToolStripPanel GetToolStripPanel(ToolStrip ts) {

      for (int i = 0; i < _tsPanels.Count; i++) {
        for (int j = 0; j < _tsPanels[i].Controls.Count; j++) {
          if (_tsPanels[i].Controls[j] as ToolStrip == ts)
            return _tsPanels[i];
        }
      }
      return null;
    }
    #endregion

    #region private ToolStrip GetToolStrip(string name)
    private ToolStrip GetToolStrip(string name) {
      for (int i = 0; i < _tStrips.Count; i++) {
        if (_tStrips[i].Name == name)
          return _tStrips[i];
      }
      return null;
    }
    #endregion

    #region private ToolStripPanel GetToolStripPanel(string name)
    private ToolStripPanel GetToolStripPanel(string name) {
      for (int i = 0; i < _tsPanels.Count; i++) {
        if (_tsPanels[i].Name == name)
          return _tsPanels[i];
      }
      return null;
    }
    #endregion

    #region public void Refresh()
    public void Refresh() {
      this.UpdateLayoutPanels();
    }
    #endregion

    #region private void UpdateLayoutPanel(ToolStripPanel panel)
    private void UpdateLayoutPanel(ToolStripPanel panel) {

      List<ToolStripItemPanel> tsips = new List<ToolStripItemPanel>();
      for (int r = 0; r < panel.Rows.Length; r++) {
        ToolStripPanelRow row = panel.Rows[r];
        if (panel.Orientation == Orientation.Horizontal) {
          int fullWidth = 0;
          int allWidthTSIP = 0;
          tsips.Clear();
          for (int i = 0; i < row.Controls.Length; i++) {
            if (row.Controls[i].Visible || !_form.Visible) {
              if (!(row.Controls[i] is ToolStripItemPanel)) {
                fullWidth += row.Controls[i].Width;
              } else {
                allWidthTSIP += row.Controls[i].Width;
                tsips.Add(row.Controls[i] as ToolStripItemPanel);
              }
            }
          }
          if (allWidthTSIP == 0) continue;
          int nw = 0;
          int emptyWidth = panel.ClientSize.Width - fullWidth;
          for (int i = 0; i < tsips.Count; i++) {
            float percent = Math.Min(tsips[i].Width / ((float)allWidthTSIP / 100), 100);
            int newWidth = Convert.ToInt32(((float)emptyWidth / 100) * percent);
            nw += newWidth;
            tsips[i].Width = newWidth;
          }
        } else {
          int fullHeight = 0;
          int allHeightTSIP = 0;
          tsips.Clear();
          for (int i = 0; i < row.Controls.Length; i++) {
            if (row.Controls[i].Visible || !_form.Visible) {
              if (!(row.Controls[i] is ToolStripItemPanel)) {
                fullHeight += row.Controls[i].Height;
              } else {
                allHeightTSIP += row.Controls[i].Height;
                tsips.Add(row.Controls[i] as ToolStripItemPanel);
              }
            }
          }
          if (allHeightTSIP == 0) continue;
          int emptyHeight = panel.ClientSize.Height - fullHeight;
          for (int i = 0; i < tsips.Count; i++) {
            float percent = Math.Min(tsips[i].Height / ((float)allHeightTSIP / 100), 100);
            int newHeight = Convert.ToInt32(((float)emptyHeight / 100) * percent);
            tsips[i].Height = newHeight;
          }
        }
      }
    }
    #endregion

    #region private void UpdateLayoutPanels()
    private void UpdateLayoutPanels() {
      _form.SuspendLayout();
      for (int i = 0; i < _tsPanels.Count; i++) {
        _tsPanels[i].SuspendLayout();
      }
      _savedSizeBottom = _savedSizeLeft = _savedSizeRigh = _savedSizeTop = new Size(123, 123);

      if (_tspTop != null && _savedSizeTop != _tspTop.Size) {
        _savedSizeTop = _tspTop.Size;
        this.UpdateLayoutPanel(_tspTop);
      }

      if (_tspBottom != null && _savedSizeBottom != _tspBottom.Size) {
        _savedSizeBottom = _tspBottom.Size;
        this.UpdateLayoutPanel(_tspBottom);
      }

      if (_tspLeft != null && _savedSizeLeft != _tspLeft.Size) {
        _savedSizeLeft = _tspLeft.Size;
        this.UpdateLayoutPanel(_tspLeft);
      }

      if (_tspRight != null && _savedSizeRigh != _tspRight.Size) {
        _savedSizeRigh = _tspRight.Size;
        this.UpdateLayoutPanel(_tspRight);
      }
      _form.ResumeLayout(true);
      for (int i = 0; i < _tsPanels.Count; i++) {
        _tsPanels[i].ResumeLayout(true);
        _tsPanels[i].Invalidate();
      }
    }
    #endregion
  }

  #region class SavedTSItem
  class SavedTSItem {
    private ToolStripPanel _panel;
    private ToolStrip _item;

    public SavedTSItem(ToolStripPanel panel, ToolStrip item) {
      _panel = panel;
      _item = item;
    }

    public ToolStripPanel Panel {
      get { return _panel; }
    }

    public ToolStrip Item {
      get { return _item; }
    }
  }
  #endregion
}
