/**
* @version $Id: ChartControl.override.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.Drawing;
  using System.Diagnostics;

  partial class ChartControl {

    /// <summary>
    /// Если кнопка мыши была нажата в области панели, то информация о перемещение
    /// мыши продолжают поступать в нее.
    /// </summary>
    private readonly List<ChartPanel> _panelsMouseMove = new List<ChartPanel>();
   
    #region private MouseEventArgs MouseEventToClient(ChartPanel panel, MouseEventArgs e)
    private MouseEventArgs MouseEventToClient(ChartPanel panel, MouseEventArgs e) {
      int x = e.X - panel.Left;
      int y = e.Y - panel.Top;
      return new MouseEventArgs(e.Button, e.Clicks, x, y, e.Delta);
    }
    #endregion

    #region protected override void OnMouseMove(MouseEventArgs e)
    protected override void OnMouseMove(MouseEventArgs e) {

      if (e.Button == MouseButtons.None) {
        _panelsMouseMove.Clear();
        for (int i = 0; i < _chartPanelCollection.Count; i++) {
          ChartPanel panel = _chartPanelCollection[i];
          if (panel.Bounds.Contains(e.Location)) {
            _panelsMouseMove.Add(panel);
            panel.WmMouseMove(this.MouseEventToClient(panel, e));
          }
        }
      } else {
        for (int i = 0; i < _panelsMouseMove.Count; i++) {
          ChartPanel panel = _panelsMouseMove[i];
          panel.WmMouseMove(this.MouseEventToClient(panel, e));
        }
      }

      base.OnMouseMove(e);
    }
    #endregion

    #region protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
      for (int i = 0; i < _chartPanelCollection.Count; i++) {
        ChartPanel panel = _chartPanelCollection[i];
        if (panel.Bounds.Contains(e.Location)) {
          panel.WmMouseDown(this.MouseEventToClient(panel, e));
        }
      }
      base.OnMouseDown(e);
    }
    #endregion

    #region protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
    protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
      for (int i = 0; i < _chartPanelCollection.Count; i++) {
        ChartPanel panel = _chartPanelCollection[i];
        if (panel.Bounds.Contains(e.Location)) {
          panel.WmMouseUp(this.MouseEventToClient(panel, e));
        }
      }

      base.OnMouseUp(e);
    }
    #endregion

    #region protected override void OnMouseEnter(EventArgs e)
    /* входим в область */
    protected override void OnMouseEnter(EventArgs e) {
      //for (int i = 0; i < _chartPanelCollection.Count; i++) {
      //  ChartPanel panel = _chartPanelCollection[i];
      //  if (panel.Bounds.Contains(e.Location)) {
      //    panel.WmMouseEnter(EventArgs.Empty);
      //  }
      //}

      base.OnMouseEnter(e);
    }
    #endregion

    #region protected override void OnMouseHover(EventArgs e)
    /* ходим по области */
    protected override void OnMouseHover(EventArgs e) {
      //for (int i = 0; i < _chartPanelCollection.Count; i++) {
      //  ChartPanel panel = _chartPanelCollection[i];
      //  if (panel.Bounds.Contains(e.Location)) {
      //    panel.WmMouseHover(EventArgs.Empty);
      //  }
      //}
      base.OnMouseHover(e);
    }
    #endregion

    #region protected override void OnMouseLeave(EventArgs e)
    /* выходим из области */
    protected override void OnMouseLeave(EventArgs e) {
      //for (int i = 0; i < _chartPanelCollection.Count; i++) {
      //  ChartPanel panel = _chartPanelCollection[i];
      //  if (panel.Bounds.Contains(e.Location)) {
      //    panel.WmMouseLeave(EventArgs.Empty);
      //  }
      //}
      base.OnMouseLeave(e);
    }
    #endregion

    #region internal void InvalidateQueryChild()
    internal void InvalidateQueryChild() {
      if (_isPainting)
        return;
      this.Invalidate();
    }
    #endregion
  }
}
