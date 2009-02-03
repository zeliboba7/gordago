/**
* @version $Id: ChartPanel.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  using System.Windows.Forms;

  [ToolboxItem(false)]
  public partial class ChartPanel : Component, IComparer<ChartPanel> {

    public event MouseEventHandler MouseMove;
    public event MouseEventHandler MouseDown;
    public event MouseEventHandler MouseUp;

    private Padding _padding = new Padding();
    private int _x = 0, _y = 0, _width = 0, _height = 0;
    private ChartControl _owner = null;
    private Color _backColor = Color.FromArgb(-723457);

    private string _guid = Guid.NewGuid().ToString();

    private int _tabIndex = 0;

    #region public ChartPanel()
    public ChartPanel() {
      InitializeComponent();
    }
    #endregion

    #region public ChartPanel(IContainer container)
    public ChartPanel(IContainer container) {
      container.Add(this);
      InitializeComponent();
    }
    #endregion

    #region public string GUID
    public string GUID {
      get { return _guid; }
    }
    #endregion

    #region public int TabIndex
    public int TabIndex {
      get { return this._tabIndex; }
      set { 
        this._tabIndex = value;
        if (this.Owner != null) {
          this.Owner.ChartPanels.Sort();
          this.Invalidate();
        }
      }
    }
    #endregion

    #region internal int NativeTabIndex
    internal int NativeTabIndex {
      get { return this._tabIndex; }
      set { this._tabIndex = value; }
    }
    #endregion

    #region public Color BackColor
    public Color BackColor {
      get { return this._backColor; }
      set { 
        this._backColor = value;
        this.Invalidate();
      }
    }
    #endregion

    #region public Padding Padding
    public Padding Padding {
      get { return this._padding; }
      set { this._padding = value; }
    }
    #endregion

    #region public ChartControl Owner
    public ChartControl Owner {
      get { return _owner; }
    }
    #endregion

    #region public Cursor Cursor
    public Cursor Cursor {
      get {
        if (this.Owner == null)
          return Cursors.Default;
        return this.Owner.Cursor;
      }
      set {
        if (this.Owner == null)
          return;
        this.Owner.Cursor = value;
      }
    }
    #endregion

    #region public int Left
    public int Left {
      get { return _x; }
      set {
        this.SetBounds(value, _y, _width, _height);
      }
    }
    #endregion

    #region public int Top
    public int Top {
      get { return _y; }
      set {
        this.SetBounds(_x, value, _width, _height);
      }
    }
    #endregion

    #region public int Right
    public int Right {
      get { return _x + _width; }
    }
    #endregion

    #region public int Bottom
    public int Bottom {
      get { return _y + _height; }
    }
    #endregion

    #region public int Width
    public int Width {
      get { return this._width; }
      set {
        this.SetBounds(this._x, this._y, value, _height);
      }
    }
    #endregion

    #region public int Height
    public int Height {
      get { return this._height; }
      set {
        this.SetBounds(_x, _y, _width, value);
      }
    }
    #endregion

    #region public Size Size
    public Size Size {
      get { return new Size(_width, _height); }
    }
    #endregion

    #region public Rectangle Bounds
    public Rectangle Bounds {
      get { return new Rectangle(_x, _y, _width, _height); }
    }
    #endregion

    #region protected IBarsData Bars
    protected IBarsData Bars {
      get {
        if (this._owner == null)
          return null;
        return _owner.Bars;
      }
    }
    #endregion

    #region public void SetBounds(int x, int y, int width, int height)
    public void SetBounds(int x, int y, int width, int height) {
      if (_x == x && _y == y && _width == width && _height == height)
        return;
      _x = x;
      _y = y;
      _width = width;
      _height = height;
      _width = Math.Max(_width, 10);
      _height = Math.Max(_height, 10);
      this.OnResize();
    }
    #endregion

    #region public void SetBounds(Rectangle rect)
    public void SetBounds(Rectangle rect) {
      this.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
    }
    #endregion

    #region internal void SetOwner(ChartControl owner)
    internal void SetOwner(ChartControl owner) {
      if (owner != null && _owner != null && owner != _owner) {
        throw (new Exception("The specified chart control is a top-level control, or a circular control reference would result if this chart control were added to the ChartControl collection."));
      }
      _owner = owner;
    }
    #endregion

    #region public void Invalidate()
    public void Invalidate() {
      if (this.Owner == null)
        return;
      this.Owner.InvalidateQueryChild();
    }
    #endregion

    #region internal void Paint(ChartGraphics g)
    internal void Paint(ChartGraphics g) {
      Rectangle rect = new Rectangle(this.Left, this.Top, this.Width, this.Height);
       g.CreatePaintWindow(rect);

      try {
        g.SetBackColor(this.BackColor);
        g.SelectBrush(this.BackColor);
        g.FillRectangle(0, 0, this.Width, this.Height);

        this.OnPaint(g);
      } finally {
        g.RestorePaintWindow();
      }
    }
    #endregion

    #region protected virtual void OnPaint(ChartGraphics g)
    protected virtual void OnPaint(ChartGraphics g) { }
    #endregion

    #region protected virtual void OnResize()
    protected virtual void OnResize() { }
    #endregion

    #region internal void WmMouseMove(MouseEventArgs e)
    internal void WmMouseMove(MouseEventArgs e) {
      this.OnMouseMove(e);
    }
    #endregion

    #region protected virtual void OnMouseMove(MouseEventArgs e)
    protected virtual void OnMouseMove(MouseEventArgs e) {
      if (this.MouseMove != null)
        this.MouseMove(this, e);
    }
    #endregion

    #region internal void WmMouseDown(MouseEventArgs e)
    internal void WmMouseDown(MouseEventArgs e) {
      this.OnMouseDown(e);
    }
    #endregion

    #region protected virtual void OnMouseDown(MouseEventArgs e)
    protected virtual void OnMouseDown(MouseEventArgs e) {
      if (this.MouseDown != null) {
        this.MouseDown(this, e);
      }
    }
    #endregion

    #region internal void WmMouseUp(MouseEventArgs e)
    internal void WmMouseUp(MouseEventArgs e) {
      this.OnMouseMove(e);
    }
    #endregion

    #region protected virtual void OnMouseUp(MouseEventArgs e)
    protected virtual void OnMouseUp(MouseEventArgs e) {
      if (this.MouseUp != null)
        this.MouseUp(this, e);
    }
    #endregion

    #region internal void WmMouseEnter(EventArgs e)
    /* входим в область */
    internal void WmMouseEnter(EventArgs e) {
      this.OnMouseEnter(e);
    }
    #endregion

    #region protected virtual void OnMouseEnter(EventArgs e)
    protected virtual void OnMouseEnter(EventArgs e) { }
    #endregion

    #region internal void WmMouseHover(EventArgs e)
    /* ходим по области */
    internal void WmMouseHover(EventArgs e) {
      this.OnMouseHover(e);
    }
    #endregion

    #region protected virtual void OnMouseHover(EventArgs e)
    protected virtual void OnMouseHover(EventArgs e) { }
    #endregion

    #region internal void WmMouseLeave(EventArgs e)
    /* выходим из области */
    internal void WmMouseLeave(EventArgs e) {
      this.OnMouseLeave(e);
    }
    #endregion

    #region protected virtual void OnMouseLeave(EventArgs e)
    protected virtual void OnMouseLeave(EventArgs e) { }
    #endregion

    #region public int Compare(ChartPanel x, ChartPanel y)
    public int Compare(ChartPanel x, ChartPanel y) {
      return x.TabIndex.CompareTo(y.TabIndex);
    }
    #endregion
  }
}
