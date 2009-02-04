/**
* @version $Id: ChartBox.ChartAreaManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  using System.Windows.Forms;
  using Gordago.Trader.Common;

  partial class ChartBox {

    public class ChartAreaManager : ChartBoxArea {
      
      #region enum MouseAction
      enum MouseAction {
        None,
        ChangePosition
      }
      #endregion

      private GdiPen _gridPen;
      private MouseAction _action = MouseAction.None;
      private Point _savedMousePosition;

      internal ChartAreaManager(ChartBox owner) : base(owner) { }

      #region protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
      protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
          if (_action == MouseAction.ChangePosition) {

            int mx = _savedMousePosition.X;
            int emx = e.X;

            int deltaX = this.Owner.HorizontalScale.DeltaX;

            int dx = 0;
            if (mx > emx) {
              dx = ((mx - emx) / deltaX + 1);
            } else if (mx < emx) {
              dx = -((emx - mx) / deltaX + 1);
            }
            this.Owner.HorizontalScale.Position += dx * (Math.Abs(dx) < 4 ? 1 : 2);
            _savedMousePosition = e.Location;
          }
        }
      }
      #endregion

      #region protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
      protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
          _savedMousePosition = e.Location;
          _action = MouseAction.ChangePosition;
          this.Owner.Cursor = Cursors.NoMoveHoriz;
        }
      }
      #endregion

      #region protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
      protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
        _action = MouseAction.None;
        this.Owner.Cursor = Cursors.Default;
      }
      #endregion

      #region internal void SelectGridPen(ChartGraphics g) 
      internal void SelectGridPen(ChartGraphics g) {
        if (_gridPen == null || _gridPen.Color != this.Owner.GridColor) {
          _gridPen = new GdiPen(this.Owner.GridColor, 1, System.Drawing.Drawing2D.DashStyle.Dot);
        }
        g.SelectPen(_gridPen);
      }
      #endregion

      private void DrawGridLine(ChartGraphics g, Color color, int x1, int y1, int x2, int y2) {
        int num = 0;
        IntPtr hdc = g.HDC;

        if (x1 == x2) {
          int l = y2 - y1;
          while (num < l) {
            Win32.SetPixel(hdc, x1, num + y1, color);
            num += 4;
          }
        } else if (y1 == y2) {
          int l = x2 - x1;
          while (num < l) {
            Win32.SetPixel(hdc, num + x1, y1, color);
            num += 4;
          }
        }
      }

      #region protected override void OnPaint(ChartGraphics g) 
      protected override void OnPaint(ChartGraphics g) {
        if (this.Owner.GridVisible) {
          this.SelectGridPen(g);

          List<ChartHorizontalScale.GridLine> hlines = this.Owner.HorizontalScale.GridLines;
          for (int i = 0; i < hlines.Count; i++) {
            int x = hlines[i].X;
            this.DrawGridLine(g, this.Owner.GridColor, x, 0, x, this.Height);
          }

          List<ChartVerticalScale.GridLine> vlines = this.Owner.VerticalScale.GridLines;
          for (int i = 0; i < vlines.Count; i++) {
            int y = vlines[i].Y;
            this.DrawGridLine(g, this.Owner.GridColor, 0, y, this.Width, y);
          }

          
        }
        for (int i = 0; i < this.Owner.Figures.Count; i++) {
          this.Owner.Figures[i].WmPaint(g);
        }
        g.SelectPen(this.Owner.BorderColor);
        g.DrawRectangle(0, 0, this.Width - 1, this.Height - 1);
      }
      #endregion
    }
  }
}
