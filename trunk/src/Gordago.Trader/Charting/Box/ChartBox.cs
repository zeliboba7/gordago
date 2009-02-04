/**
* @version $Id: ChartBox.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  using System.ComponentModel;
  using System.Collections;
  using System.Windows.Forms;
  using System.Diagnostics;

  [Serializable, ToolboxItem(true)]
  public partial class ChartBox: ChartPanel {

    #region enum ChartBoxAction
    enum ChartBoxAction {
      None,
      ChangeHSplitTop,
      ChangeHSplitBottom,
      EnterHSplitTop,
      EnterHSplitBottom
    }
    #endregion

    public event FigureEventHandler FigureAdded;
    public event FigureEventHandler FigureRemoved;

    private readonly ChartAreaManager _chartArea;
    private readonly ChartHorizontalScale _horizontalScale;
    private readonly ChartVerticalScale _verticalScale;
    private bool _splitBottom, _splitTop;
    private Rectangle _splitTopRect, _splitBottomRect;
    private ChartBoxAction _action = ChartBoxAction.None;
    private int _dy = 0;

    private readonly List<ChartBoxArea> _listArea = new List<ChartBoxArea>();
    private readonly List<ChartBoxArea> _panelsMouseMove = new List<ChartBoxArea>();
    private readonly List<Figure> _figures = new List<Figure>();
    private readonly FigureCollection _figureCollection;

    private Color _borderColor = Color.FromArgb(-9868951);
    private Color _gridColor = Color.FromArgb(1342177485);
    private Font _scaleFont = new Font("Microsoft Sans Serif", 7);
    private bool _gridVisible = false;
    private Color _foreColor = Color.FromArgb(-16777216);

    public ChartBox() {
      _listArea.Add(_chartArea = new ChartAreaManager(this));
      _listArea.Add(_horizontalScale = new ChartHorizontalScale(this));
      _listArea.Add(_verticalScale = new ChartVerticalScale(this));
      _figureCollection = new FigureCollection(this);

      this.Padding = new Padding(3, 1, 3, 1);
    }

    #region public bool GridVisible
    public bool GridVisible {
      get { return this._gridVisible; }
      set {

        bool evt = this._gridVisible != value;
        _gridVisible =value ;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region public Color BorderColor
    [Parameter("BorderColor")]
    [DisplayName("Border Color")]
    public Color BorderColor {
      get { return this._borderColor; }
      set {
        bool evt = this._borderColor != value;
        this._borderColor = value;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region public Color GridColor 
    [Parameter("GridColor")]
    [DisplayName("Grid Color")]
    public Color GridColor {
      get { return this._gridColor; }
      set {
        bool evt = this._gridColor != value;
        this._gridColor = value;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region public Font ScaleFont
    public Font ScaleFont {
      get{
        return _scaleFont;
      }
      set {
        bool evt = _scaleFont != value;
        _scaleFont = value;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region public Color ForeColor
    public Color ForeColor {
      get { return this._foreColor; }
      set {
        bool evt = _foreColor != value;
        _foreColor = value;
        if (evt)
          this.Invalidate();
      }
    }
    #endregion

    #region public ChartAreaManager Area
    public ChartAreaManager Area {
      get {
        return _chartArea;
      }
    }
    #endregion

    #region public BoxVerticalScale VerticalScale
    public ChartVerticalScale VerticalScale {
      get { return this._verticalScale; }
    }
    #endregion

    #region public ChartHorizontalScale HorizontalScale
    public ChartHorizontalScale HorizontalScale {
      get { return _horizontalScale; }
    }
    #endregion

    #region internal bool SplitBottom
    internal bool SplitBottom {
      get { return this._splitBottom; }
      set { this._splitBottom = value;  }
    }
    #endregion

    #region internal bool SplitTop
    internal bool SplitTop {
      get { return this._splitTop; }
      set { this._splitTop = value; }
    }
    #endregion

    #region private void LayoutChild()
    private void LayoutChild() {
      this.Layout();
      this.Invalidate();
    }
    #endregion

    #region private void Layout()
    private void Layout() {

      int outLeft = this.Padding.Left;
      int outTop = this.Padding.Top;
      int outRight = this.Padding.Right;
      int outBottom = this.Padding.Bottom;

      int w = this.Width - outLeft - outRight;
      int h = this.Height - outTop - outBottom;

      int xArea = outLeft, yArea = outTop, wArea = w, hArea = h;

      if (this.HorizontalScale.Visible) {
        hArea = h - this.HorizontalScale.Height;

        int xH = outLeft, yH, wH = w, hH=this.HorizontalScale.Height;

        if (this.HorizontalScale.Dock == ChartHorizontalScalePosition.Top) {
          yH = outTop;
          yArea = outTop + this.HorizontalScale.Height;
        } else {
          yH = this.Height - outBottom - this.HorizontalScale.Height;
          yArea = outTop ;
        }
        this.HorizontalScale.SetBounds(xH, yH, wH, hH);
      }

      if (this.VerticalScale.Visible) {
        wArea = w - this.VerticalScale.Width;
        int xV, yV = yArea, wV = this.VerticalScale.Width, hV = hArea;
        if (this.VerticalScale.Dock == ChartVerticalScalePosition.Left) {
          xV = xArea;
          xArea = xV + wV;
        } else {
          xV = xArea + wArea;
        }
        this.VerticalScale.SetBounds(xV, yV, wV, hV);
      }

      _splitTopRect = new Rectangle(0, 0, this.Width, outTop);

      _splitBottomRect = new Rectangle(0, this.Height - outBottom, this.Width, outBottom);

      this.Area.SetBounds(xArea, yArea, wArea, hArea);
    }
    #endregion

    #region public FigureCollection Figures
    public FigureCollection Figures {
      get { return this._figureCollection; }
    }
    #endregion

    #region protected override void OnResize()
    protected override void OnResize() {
      this.Layout();
      base.OnResize();
    }
    #endregion

    #region private void SetAction(ChartBoxAction action, string trace)
    private void SetAction(ChartBoxAction action, string trace) {
      // Trace.WriteLine(string.Format("{0}:{1}", trace, action));
      _action = action;
    }
    #endregion

    #region public void MoveUp()
    public void MoveUp() {
      this.TabIndex--;
    }
    #endregion

    #region public void MoveDown()
    public void MoveDown() {
      this.TabIndex++;
    }
    #endregion

    #region public ChartBox GetPrevChartBox()
    public ChartBox GetPrevChartBox() {
      for (int i = this.Owner.Boxes.Count - 1; i >= 0; i--) {
        if (this.Owner.Boxes[i].TabIndex < this.TabIndex)
          return this.Owner.Boxes[i];
      }
      return null;
    }
    #endregion

    #region public ChartBox GetNextChartBox()
    public ChartBox GetNextChartBox() {
      for (int i = 0; i < this.Owner.Boxes.Count; i++) {
        if (this.Owner.Boxes[i].TabIndex > this.TabIndex)
          return this.Owner.Boxes[i];
      }
      return null;
    }
    #endregion

    #region private void ChangeLocation(MouseEventArgs e)
    private void ChangeLocation(MouseEventArgs e) {
      int oldTop = this.Top;
      this.Top = e.Y + this.Top + _dy;
      int dy = this.Top - oldTop;
      
      ChartBox cbox = this.GetPrevChartBox();
      if (cbox == null) return;

      int cboxOldHeight = cbox.Height;
      cbox.Height += dy;

      dy = cboxOldHeight - cbox.Height - dy;
      this.Top -= dy;

      this.Owner.ChartBoxesLayout();
    }
    #endregion

    #region private void ChangeSize(MouseEventArgs e)
    /// <summary>
    /// Изменения размера текущего контрола с контролом ниже.
    /// </summary>
    /// <param name="e"></param>
    private void ChangeSize(MouseEventArgs e) {

      int oldHeigh = this.Height;
      
      this.Height = e.Y+_dy;

      int dy = this.Height - oldHeigh;

      ChartBox cbox = this.GetNextChartBox();
      if (cbox == null) 
        return;

      int cboxOldHeight = cbox.Height;
      cbox.Height -= dy;

      dy = cboxOldHeight - cbox.Height - dy;
      this.Height += dy;
 
      this.Owner.ChartBoxesLayout();
    }
    #endregion

    #region private MouseEventArgs MouseEventToClient(ChartBoxArea panel, MouseEventArgs e)
    private MouseEventArgs MouseEventToClient(ChartBoxArea panel, MouseEventArgs e) {
      int x = e.X - panel.Left;
      int y = e.Y - panel.Top;
      return new MouseEventArgs(e.Button, e.Clicks, x, y, e.Delta);
    }
    #endregion

    #region protected override void OnMouseMove(MouseEventArgs e)
    protected override void OnMouseMove(MouseEventArgs e) {

      if (e.Button == MouseButtons.None) {
        ChartBoxAction newAction = ChartBoxAction.None;

        if (this.SplitTop) {
          if (_splitTopRect.Contains(e.Location))
            newAction = ChartBoxAction.EnterHSplitTop;
        }
        if (this.SplitBottom) {
          if (_splitBottomRect.Contains(e.Location))
            newAction = ChartBoxAction.EnterHSplitBottom;
        }

        if (newAction == ChartBoxAction.EnterHSplitBottom || newAction == ChartBoxAction.EnterHSplitTop) {
          this.Cursor = Cursors.HSplit;
        } else {
          this.Cursor = Cursors.Default;
        }
        this.SetAction(newAction, "OnMouseMove");
      } else {
        switch (e.Button) {
          case MouseButtons.Left:
            switch (_action) {
              case ChartBoxAction.ChangeHSplitBottom:
                this.ChangeSize(e);
                break;
              case ChartBoxAction.ChangeHSplitTop:
                this.ChangeLocation(e);
                break;
            }
            break;
        }
      }

      /* сообщение клиенту */
      if (e.Button == MouseButtons.None) {
        _panelsMouseMove.Clear();
        for (int i = 0; i < _listArea.Count; i++) {
          ChartBoxArea area = _listArea[i];
          if (area.Visible && area.Bounds.Contains(e.Location)) {
            _panelsMouseMove.Add(area);
            area.WmMouseMove(this.MouseEventToClient(area, e));
          }
        }
      } else {
        for (int i = 0; i < _panelsMouseMove.Count; i++) {
          ChartBoxArea area = _panelsMouseMove[i];
          area.WmMouseMove(this.MouseEventToClient(area, e));
        }
      }

      base.OnMouseMove(e);
    }
    #endregion

    #region protected override void OnMouseDown(MouseEventArgs e)
    protected override void OnMouseDown(MouseEventArgs e) {
      switch (e.Button) {
        case MouseButtons.Left:
          switch (_action) {
            case ChartBoxAction.EnterHSplitBottom:
              this.SetAction(ChartBoxAction.ChangeHSplitBottom, "OnMouseDown");
              _dy = this.Height - e.Y;
              break;
            case ChartBoxAction.EnterHSplitTop:
              this.SetAction(ChartBoxAction.ChangeHSplitTop, "OnMouseDown");
              _dy = e.Y;
              break;
          }
          break;
      }

      /* сообщение клиенту */
      for (int i = 0; i < _listArea.Count; i++) {
        ChartBoxArea panel = _listArea[i];
        if (panel.Visible && panel.Bounds.Contains(e.Location)) {
          panel.WmMouseDown(this.MouseEventToClient(panel, e));
        }
      }
      base.OnMouseDown(e);
    }
    #endregion

    #region protected override void OnMouseUp(MouseEventArgs e)
    protected override void OnMouseUp(MouseEventArgs e) {
      switch (_action) {
        case ChartBoxAction.ChangeHSplitBottom:
        case ChartBoxAction.ChangeHSplitTop:
          this.Cursor = Cursors.Default;
          this.SetAction(ChartBoxAction.None, "OnMouseUp  ");
          break;
      }

      /* сообщение клиенту */
      for (int i = 0; i < _listArea.Count; i++) {
        ChartBoxArea panel = _listArea[i];
        if (panel.Visible && panel.Bounds.Contains(e.Location)) {
          panel.WmMouseUp(this.MouseEventToClient(panel, e));
        }
      }
      base.OnMouseUp(e);
    }
    #endregion

    #region protected override void OnPaint(ChartGraphics g)
    protected override void OnPaint(ChartGraphics g) {
      if (this.Owner.Bars == null)
        return;
      this.HorizontalScale.CmCalculateScale();
      this.VerticalScale.CmCalculateScale();
      
      if (this.HorizontalScale.Visible)
        this.HorizontalScale.WmPaint(g);

      if (VerticalScale.Visible)
        this.VerticalScale.WmPaint(g);

      this.Area.WmPaint(g);
    }
    #endregion

  }
}
