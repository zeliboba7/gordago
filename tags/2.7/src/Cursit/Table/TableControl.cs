/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/

#region using
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Collections.Generic;
#endregion

namespace Cursit.Table {

  public class TableControl : Control {

    public event EventHandler SelectedIndexChanged;

    private TableRowCollection _rows;
    private TableColumnCollection _columns;
    private TableCellStyle _style;
    private VScrollBar _vScrollBar;
    private HScrollBar _hScrollBar;
    private bool _vSBVisible = false, _hSBVisible = false;

    private Color _borderColor;
    private Pen _borderpen;
    private int _borderWidth = 1;
    private bool _borderVisible = true;

    private bool _viewhgridlines = false;

    private bool _captionvisible = true;

    private bool _cellValueChanged = false;
    private int _savedCountRow, _savedCountCol, _savedWidth, _savedHeight;
    private int _savedListWidth, _saveListHeigh;
    private bool _hideSelection;

    public bool _autoColumnSize = false;

    private int _positionx;

    private int _currentColumnSplit = -1;
    private int _currentColumnSplitX = -1;

    private int _headerHeight = 18;

    private Color _captionbackcolor;
    private Brush _captionbrush;

    private Color _mouseHoverSelectColor;
    private Brush _mouseHoverSelectBrush;
    private int _mouseHoverSelectNumRow = -2;
    private int _deltawidth;

    private Bitmap _bitmap;

    private Color _selectedColor;
    private Brush _selectedBrush;

    private bool _isfocus = false;

    private Color _firstRowColor, _secondRowColor;
    private Brush _firstRowBrush, _secondRowBrush;

    public TableControl() {

      _vScrollBar = new VScrollBar();
      _hScrollBar = new HScrollBar();
      _style = new TableCellStyle();
      _style.Font = this.Font;
      _bitmap = new Bitmap(1, 1);

      this._rows = new TableRowCollection(this);
      this._columns = new TableColumnCollection(this);

      this.Controls.Add(_vScrollBar);
      this.Controls.Add(_hScrollBar);

      _vScrollBar.Visible = false;
      _vScrollBar.ValueChanged += new EventHandler(this._vscrollBar_ValueChanged);

      _hScrollBar.Visible = false;
      _hScrollBar.ValueChanged += new EventHandler(this._hscrollBar_ValueChanged);

      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.UserMouse, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

      this.BorderColor = Color.FromArgb(172, 168, 153);
      this.CaptionBackColor = Color.FromArgb(204, 236, 242);
      this.MouseHoverSelectRowColor = Color.Blue;
      this.SelectedColor = Color.Blue;

      this.RowColorFirst = Color.White;
      this.RowColorSecond = Color.FromArgb(244,243,243);
    }

    #region public Color BorderColor
    public Color BorderColor {
      get { return _borderColor; }
      set {
        _borderColor = value;
        _borderpen = new Pen(value);
        this.Invalidate();
      }
    }
    #endregion

    #region public TableRowCollection Rows
    public TableRowCollection Rows {
      get { return this._rows; }
    }
    #endregion

    #region public TableColumnCollection Columns
    public TableColumnCollection Columns {
      get { return this._columns; }
    }
    #endregion

    #region public Color CaptionBackColor
    public Color CaptionBackColor {
      get { return this._captionbackcolor; }
      set {
        this._captionbackcolor = value;
        this._captionbrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color RowColorFirst
    public Color RowColorFirst {
      get { return this._firstRowColor; }
      set { 
        this._firstRowColor = value;
        this._firstRowBrush = new SolidBrush(value);
      }
    }
    #endregion

    #region public Color RowColorSecond
    public Color RowColorSecond {
      get { return this._secondRowColor; }
      set { 
        this._secondRowColor = value;
        this._secondRowBrush = new SolidBrush(value);

      }
    }
    #endregion

    #region public Color MouseHoverSelectRowColor
    /// <summary>
    /// Цвет выделения строки, при прохождение по ней мыши
    /// </summary>
    public Color MouseHoverSelectRowColor {
      get { return this._mouseHoverSelectColor; }
      set {
        this._mouseHoverSelectColor = value;
        this._mouseHoverSelectBrush = new SolidBrush(Color.FromArgb(50, value));
      }
    }
    #endregion

    #region public Color SelectedColor
    public Color SelectedColor {
      get { return this._selectedColor; }
      set {
        this._selectedColor = value;
        this._selectedBrush = new SolidBrush(Color.FromArgb(120, value));
      }
    }
    #endregion

    #region public int HeaderHeight
    public int HeaderHeight {
      get {
        if (this.CaptionVisible)
          return _headerHeight;
        return 0;
      }
      set { this._headerHeight = value; }
    }
    #endregion

    #region internal int DelataWidth
    internal int DelataWidth {
      get { return this._deltawidth; }
      set { this._deltawidth = value; }
    }
    #endregion

    #region internal bool IsFocus
    internal bool IsFocus {
      get { return this._isfocus; }
      set { this._isfocus = value; }
    }
    #endregion

    #region public bool HideSelection
    public bool HideSelection {
      get { return this._hideSelection; }
      set { _hideSelection = value; }
    }
    #endregion

    #region public TableCellStyle Style
    public TableCellStyle Style {
      get { return this._style; }
      set { this._style = value; }
    }
    #endregion

    #region internal bool isCellValueChange
    /// <summary>
    /// Если в ячейки меняеться значение, этот флаг = true.
    /// Необходим для перерисовки
    /// </summary>
    internal bool isCellValueChange {
      get { return this._cellValueChanged; }
      set {
        this._cellValueChanged = value;
        if (value) {
          //this._tlistpanel.SetRepaintFlag();
          this.Invalidate();
        }
      }
    }
    #endregion

    #region public bool ViewHGdirLines - Отрисовка сетки
    /// <summary>
    /// Отрисовка сетки
    /// </summary>
    public bool ViewHGdirLines {
      get { return this._viewhgridlines; }
      set { this._viewhgridlines = value; }
    }
    #endregion

    #region public bool CaptionVisible
    public bool CaptionVisible {
      get { return this._captionvisible; }
      set { this._captionvisible = value; }
    }
    #endregion

    #region public bool BorderVisible
    public bool BorderVisible {
      get { return this._borderVisible; }
      set { this._borderVisible = value; }
    }
    #endregion

    #region private int BorderWidth
    private int BorderWidth {
      get {
        if (_borderVisible)
          return _borderWidth;
        return 0;
      }
    }
    #endregion

    #region private bool VSBVisible
    private bool VSBVisible {
      get { return this._vSBVisible; }
      set {
        _vSBVisible = value;
        _vScrollBar.Visible = value;
        if (value) {
          _vScrollBar.Top = this.BorderWidth;
          _vScrollBar.Left = this.Width - _vScrollBar.Width - this.BorderWidth;
          _vScrollBar.Height = _bitmap.Height;
        } else {
          this._rows.FirstViewRow = 0;
        }
      }
    }
    #endregion

    #region public bool HSBVisible
    public bool HSBVisible {
      get { return this._hSBVisible; }
      set {
        this._hSBVisible = value;
        this._hScrollBar.Visible = value;
        if (value) {
          _hScrollBar.Left = this.BorderWidth;
          _hScrollBar.Top = this.Height - _hScrollBar.Height - this.BorderWidth;
          _hScrollBar.Width = _bitmap.Width;
        } else {
          this._positionx = 0;
        }
      }
    }
    #endregion

    #region public int SelectedIndex
    public int SelectedIndex {
      get { return this.Rows.SelectedRowIndex; }
      set {
        bool ischanged = this.Rows.SelectedRowIndex != value;
        this.Rows.SelectedRowIndex = value;
        if (ischanged)
          this.OnSelectedIndexChanged();
      }
    }
    #endregion

    #region public TableRow SelectedRow
    public TableRow SelectedRow {
      get { return this.Rows.SelectedRow; }
      set {
        bool ischanged = this.Rows.SelectedRow != null && value != null && this.Rows.SelectedRow != value;
        this.Rows.SelectedRow = value;
        if (ischanged)
          this.OnSelectedIndexChanged();
        this.Invalidate();
      }
    }
    #endregion

    #region public new Size ClientSize
    public new Size ClientSize {
      get {
        this.CalculateBitmap();
        return this._bitmap.Size; 
      }
    }
    #endregion

    #region public bool AutoColumnSize
    public bool AutoColumnSize {
      get { return _autoColumnSize; }
      set { 
        this._autoColumnSize = value;
        this.CalculateAutoColumnSize();
      }
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this.Invalidate();
      if (_autoColumnSize)
        this.CalculateAutoColumnSize();
    }
    #endregion

    #region private void CalculateAutoColumnSize()
    private void CalculateAutoColumnSize() {
      int w = this.Columns.ColumnsWidth;
      int nw = this.ClientSize.Width;

      for (int i = 0; i < this.Columns.Count; i++) {
        TableColumn col = this.Columns[i];
        if (col.Visible) {
          float percent = Math.Min(col.Width / ((float)w / 100), 100);
          int newWidth = Convert.ToInt32(((float)nw / 100) * percent);
          col.Width = newWidth;
        }
      }
      this.Invalidate();
    }
    #endregion

    #region private void _vscrollBar_ValueChanged(object sender, EventArgs e)
    private void _vscrollBar_ValueChanged(object sender, EventArgs e) {
      this.Rows.FirstViewRow = _vScrollBar.Value / 10;
      this.Invalidate();
    }
    #endregion

    #region private void _hscrollBar_ValueChanged(object sender, EventArgs e)
    private void _hscrollBar_ValueChanged(object sender, EventArgs e) {
      _positionx = _hScrollBar.Value;
      this.Invalidate();
    }
    #endregion

    #region public TableRow NewRow() - Создание пустой строки для этой таблицы
    /// <summary>
    /// Создание пустой строки для этой таблицы
    /// </summary>
    /// <returns></returns>
    public TableRow NewRow() {
      return this.Rows.NewRow();
    }
    #endregion

    #region internal static StringAlignment[] ConvertContentAlignmentToSAlignment(ContentAlignment contentalignment)
    internal static StringAlignment[] ConvertContentAlignmentToSAlignment(ContentAlignment contentalignment) {
      StringAlignment hal = StringAlignment.Far, val = StringAlignment.Far;

      switch (contentalignment) {
        case ContentAlignment.BottomCenter:
          hal = StringAlignment.Center;
          val = StringAlignment.Near;
          break;
        case ContentAlignment.BottomLeft:
          hal = StringAlignment.Near;
          val = StringAlignment.Near;
          break;
        case ContentAlignment.BottomRight:
          hal = StringAlignment.Far;
          val = StringAlignment.Near;
          break;
        case ContentAlignment.MiddleCenter:
          hal = StringAlignment.Center;
          val = StringAlignment.Center;
          break;
        case ContentAlignment.MiddleLeft:
          hal = StringAlignment.Near;
          val = StringAlignment.Center;
          break;
        case ContentAlignment.MiddleRight:
          hal = StringAlignment.Far;
          val = StringAlignment.Center;
          break;
        case ContentAlignment.TopCenter:
          hal = StringAlignment.Center;
          val = StringAlignment.Far;
          break;
        case ContentAlignment.TopLeft:
          hal = StringAlignment.Near;
          val = StringAlignment.Far;
          break;
        case ContentAlignment.TopRight:
          hal = StringAlignment.Far;
          val = StringAlignment.Far;
          break;
      }
      return new StringAlignment[] { val, hal };
    }
    #endregion

    #region public void UnSelectAllRow()
    public void UnSelectAllRow() {
      Rows.SelectedRowIndex = -1;
      this.Invalidate();
    }
    #endregion

    #region public void SetColumnsToLevel()
    /// <summary>
    /// Выравнивание колонок по ширине
    /// </summary>
    //public void SetColumnsToLevel() {

    //  int minwidth = Columns.Count * TableColumn.MinimunWidth;
    //  int towidth = Math.Max(this.Width - _borderwidth, minwidth);

    //  float onepercent = 100F / Convert.ToSingle(_bitmap.ListWidth);

    //  for (int i = 0; i < Columns.Count; i++) {
    //    float cpercent = Convert.ToSingle(Columns[i].Width) * onepercent;
    //    Columns[i].Width = Convert.ToInt32(Convert.ToSingle(towidth) / cpercent);
    //  }
    //  this.Invalidate();
    //}
    #endregion

    #region private int GetColumnSplitter(int x)
    private int GetColumnSplitter(int x) {
      for (int i = 0; i < _columns.Count; i++) {
        if (_columns[i].Visible) {
          int dy = _columns[i].XSplit - this._positionx;
          if (dy > 0 && dy < this.Width && x > dy - 2 && x < dy + 2)
            return i;
        }
      }
      return -1;
    }
    #endregion

    #region private int GetNumberRow(int x, int y) - Возвращает номер строки на которой находиться курсор
    /// <summary>
    /// Возвращает номер строки на которой находиться курсор:
    /// -2 нет строки
    /// -1 строка заголовка (caption)
    /// > -1 номер строки
    /// </summary>
    private int GetNumberRowView(int x, int y) {
      if (y < this.HeaderHeight)
        return -1;
      return (y - this.HeaderHeight) / this.Rows.RowHeight;
    }
    #endregion

    #region private int GetNumberInTable(int numberRowView)
    private int GetNumberInTable(int numberRowView) {
      int numrow = numberRowView + this.Rows.FirstViewRow;
      if (numberRowView + this.Rows.FirstViewRow >= _rows.Count) {
        return -1;
      }
      return numrow;
    }
    #endregion

    #region protected virtual void OnSelectedIndexChanged()
    protected virtual void OnSelectedIndexChanged() {
      if (this.SelectedIndexChanged != null)
        this.SelectedIndexChanged(this, new EventArgs());
    }
    #endregion

    #region protected override void OnMouseLeave(EventArgs e)
    protected override void OnMouseLeave(EventArgs e) {
      base.OnMouseLeave(e);
      this._mouseHoverSelectNumRow = -2;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnMouseMove(MouseEventArgs me)
    protected override void OnMouseMove(MouseEventArgs me) {

      if (me.Button == MouseButtons.Left && _currentColumnSplit > -1 && _currentColumnSplitX != me.X) {
        int x = me.X - _columns[_currentColumnSplit].XSplit + _positionx;

        _columns[_currentColumnSplit].Width += x;
        this.Invalidate();
        return;
      }

      this._currentColumnSplit = this.GetColumnSplitter(me.X);
      if (this._autoColumnSize) {
        if (_currentColumnSplit == _columns.CountVisible - 1) 
          _currentColumnSplit = -1;
      }
      int currow = -2;

      if (_currentColumnSplit > -1) {
        this.Cursor = Cursors.VSplit;
        _currentColumnSplitX = me.X;
      } else {
        this.Cursor = Cursors.Default;
        _currentColumnSplitX = -1;

        currow = GetNumberRowView(me.X, me.Y);
        if (this.GetNumberInTable(currow) < 0) {
          if (_mouseHoverSelectNumRow > -2) {
            _mouseHoverSelectNumRow = -2;
            this.Invalidate();
          }
          return;
        }
      }
      if (this._mouseHoverSelectNumRow != currow) {
        _mouseHoverSelectNumRow = currow;
        this.Invalidate();
      }
    }
    #endregion

    #region protected override void OnMouseUp(MouseEventArgs e)
    protected override void OnMouseUp(MouseEventArgs e) {

      if (this._mouseHoverSelectNumRow > -1 && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)) {
        int numrow = this.GetNumberInTable(this._mouseHoverSelectNumRow);
        if (numrow > -1) {
          this.Focus();
          this.SelectedIndex = numrow;
        } else {
          this.SelectedRow = null;
        }
        Invalidate();
      }
    }
    #endregion

    #region protected override void OnMouseDown(MouseEventArgs e)
    protected override void OnMouseDown(MouseEventArgs e) {
      this.Focus();
      base.OnMouseDown(e);
    }
    #endregion

    #region protected override void OnMouseWheel(MouseEventArgs e)
    protected override void OnMouseWheel(MouseEventArgs e) {
      base.OnMouseWheel(e);
      int inc = e.Delta > 0 ? -1 : 1;

      this.Rows.FirstViewRow += inc;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnGotFocus(EventArgs e)
    protected override void OnGotFocus(EventArgs e) {
      base.OnGotFocus(e);
      _isfocus = true;
      this.Invalidate();
    }
    #endregion

    #region protected override void OnLostFocus(EventArgs e)
    protected override void OnLostFocus(EventArgs e) {
      base.OnLostFocus(e);
      _isfocus = false;
      this.Invalidate();
    }
    #endregion

    #region private void CalculateBitmap()
    private void CalculateBitmap() {
      if (this._savedCountRow != this.Rows.Count || _savedCountCol != this.Columns.Count ||
          this._savedWidth != this.Width || this._savedHeight != this.Height ||
          this._savedListWidth != this.Columns.ColumnsWidth) {

        _savedCountRow = this.Rows.Count;
        _savedWidth = this.Width;
        _savedHeight = this.Height;
        _savedCountCol = this.Columns.Count;
        _savedListWidth = this.Columns.ColumnsWidth;

        int maxWidthV = this.Width - this.BorderWidth * 2;
        int minWidthV = maxWidthV - _vScrollBar.Width;

        int maxHeightV = this.Height - this.BorderWidth * 2;
        int minHeightV = maxHeightV - _hScrollBar.Height;

        int widthList = this.Columns.ColumnsWidth;
        int heighList = this._rows.RowsHeight + this.HeaderHeight;

        int calcWidth = maxWidthV, calcHeight = maxHeightV;
        bool vVisible = false, hVisible = false, ischange = false;

        if (widthList > maxWidthV) { calcHeight = minHeightV; hVisible = true; }
        if (heighList > calcHeight) { calcWidth = minWidthV; vVisible = true; }

        if (_bitmap.Width != calcWidth)
          ischange = true;
        if (_bitmap.Height != calcHeight)
          ischange = true;

        calcWidth = Math.Max(calcWidth, 1);
        calcHeight = Math.Max(calcHeight, 1);

        if (ischange)
          _bitmap = new Bitmap(calcWidth, calcHeight);

        this.VSBVisible = vVisible;
        this.HSBVisible = hVisible;

        int divV = -((this.Height - this.HeaderHeight) / this.Rows.RowHeight - _rows.Count);
        if (divV > 0) {
          int val = (divV + 1) * 10;
          _vScrollBar.Minimum = 0;
          _vScrollBar.Maximum = val;
          _vScrollBar.LargeChange = val / 5;
          _vScrollBar.SmallChange = val / 10;
          _vScrollBar.Maximum += _vScrollBar.LargeChange;
          _vScrollBar.Value = Math.Min(val, _vScrollBar.Value);
        }

        int divH = (-(this.Width - widthList)) + (_hScrollBar.Visible ? 10 : 0);
        if (divH > 0) {
          _hScrollBar.Maximum = divH;
          _hScrollBar.LargeChange = divH - divH / 10;
          _hScrollBar.SmallChange = divH / 5;
          _hScrollBar.Maximum += _hScrollBar.LargeChange;
        }

        int dw = 0;
        for (int i = 0; i < this._columns.Count; i++) {
          if (_columns[i].Visible) {
            _columns[i].X = dw;
            dw += this._columns[i].Width;
          }
        }

      }
    }
    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      
      this.CalculateBitmap();

      Graphics g = Graphics.FromImage(_bitmap);
      g.InterpolationMode = InterpolationMode.Low;
      g.SmoothingMode = SmoothingMode.None;
      g.CompositingMode = CompositingMode.SourceOver;
      g.CompositingQuality = CompositingQuality.HighSpeed;
      g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
      g.Clear(this.Style.BackColor);

      int beginColumn = 0, columnCount = _columns.Count;
      TableRow rowselected = this._rows.SelectedRow;

      for (int i = 0, numrow = this.Rows.FirstViewRow; numrow < _rows.Count; numrow++, i++) {
        TableRow row = this._rows[numrow];
        int y = i * this.Rows.RowHeight + this.HeaderHeight;

        if (y > this.Height)
          break;

        int tmp = numrow / 2;
        Brush brush = _secondRowBrush;
        if (tmp * 2 == numrow) {
          brush = _firstRowBrush;
        }

        Rectangle rect = new Rectangle(0, y + 1, this.Width, this.Rows.RowHeight - 1);

        g.FillRectangle(brush, rect);

        if (rowselected != null && row == rowselected && (this._isfocus || this.HideSelection)) {
          g.FillRectangle(_selectedBrush, rect);
        }
        for (int numcol = beginColumn; numcol < columnCount; numcol++) {
          int x = _columns[numcol].X - this._positionx;
          TableColumn column = _columns[numcol];
          if (column.Visible) {
            if (x + column.Width < 0) {
              beginColumn = numcol + 1;
            } else if (x > this.Width) {
              columnCount = numcol;
            } else {
              TableCellStyle cs = row.Style == null ? this.Style : row.Style;
              cs.TextAlignment = column.CellAlignment;
              row[numcol].Paint(g, cs, x+1, y+1, column.Width-1,  this.Rows.RowHeight-1);
            }
          }
        }
        if (this.ViewHGdirLines)
          g.DrawLine(this.Style.GridLinePen, 0, y + this.Rows.RowHeight, this.Width, y + this.Rows.RowHeight);
      }
      if (this.CaptionVisible) 
        g.FillRectangle(_captionbrush, 0, 0, this.Width, this.HeaderHeight);
      
      for (int numcol = beginColumn; numcol < columnCount; numcol++) {
        TableColumn column = _columns[numcol];
        if (column.Visible) {
          int x = _columns[numcol].X - this._positionx;
          int xs = _columns[numcol].XSplit - this._positionx;
          g.DrawLine(this.Style.GridLinePen, xs, 1, xs, this.Height - 1);

          if (this.CaptionVisible) {
            Font captionfont = column.CaptionFont == null ? this.Style.Font : column.CaptionFont;
            g.DrawString(column.Caption, captionfont, column.CaptionForeBrush, new RectangleF(x + 2, 2, column.Width - 2, this.HeaderHeight - 2), column.CaptionStringFormat);
          }
        }
      }
      if (this.CaptionVisible) {
        g.DrawLine(this._borderpen, 0, this.HeaderHeight, this.Width, this.HeaderHeight);
      }

      if (this._mouseHoverSelectNumRow > -1) {
        int y = _mouseHoverSelectNumRow * this.Rows.RowHeight + this.HeaderHeight;
        g.FillRectangle(_mouseHoverSelectBrush, 0, y + 1, this.Width, this.Rows.RowHeight - 1);
      }
      e.Graphics.DrawImageUnscaled(this._bitmap, 0, 0, _bitmap.Width, _bitmap.Height);

      g = e.Graphics;
      g.InterpolationMode = InterpolationMode.Low;
      g.SmoothingMode = SmoothingMode.None;
      g.CompositingMode = CompositingMode.SourceOver;
      g.CompositingQuality = CompositingQuality.HighSpeed;
      g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

      if (_borderVisible)
        g.DrawRectangle(_borderpen, 0, 0, this.Width - this.BorderWidth, this.Height - this.BorderWidth);
    }
    #endregion

    #region public void SaveCSV(string fileName)
    public void SaveCSV(string fileName) {
      if (File.Exists(fileName))       
        File.Delete(fileName);

      FileStream fs = new FileStream(fileName, FileMode.CreateNew);
      TextWriter tw = new StreamWriter(fs, Encoding.GetEncoding("windows-1251"));

      List<string> sa = new List<string>();

      for (int r = 0; r < this.Rows.Count; r++) {
        TableRow row = this.Rows[r];
        sa.Clear();
        for (int c = 0; c < this.Columns.Count; c++) {
          string s = row[c].Text;
          if (this.Columns[c].Visible && s != null) {
            if (s.IndexOf(",")>-1) {
              s = s.Replace(",", ".");
            }
            sa.Add(s);
          }
        }
        tw.WriteLine(string.Join(",", sa.ToArray()));
      }
      tw.Flush();
      fs.Close();
    }
    #endregion
  }
}
