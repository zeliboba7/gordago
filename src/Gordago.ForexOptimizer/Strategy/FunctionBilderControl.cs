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
using Gordago.Strategy.FIndicator;

namespace Gordago.Strategy {
  partial class FunctionBilderControl:UserControl {

    private CustomIndicatorForm _indicForm;

    public FunctionBilderControl() {
      InitializeComponent();
    }

    #region public TextBoxObject TextBoxObject
    public TextBoxObject TextBoxObject {
      get { return this._tbo; }
    }
    #endregion
    
    public void SetCurstomIndicatorForm(CustomIndicatorForm indicForm) {
      _indicForm = indicForm;
    }

    #region protected override void OnClick(EventArgs e)
    protected override void OnClick(EventArgs e) {
      base.OnClick(e);
      _tbo.Focus();
    }
    #endregion

    #region protected override void OnDragOver(DragEventArgs drgevent)
    protected override void OnDragOver(DragEventArgs drgevent) {
      if(drgevent.Data.GetDataPresent(typeof(IndicFunction).ToString())) {
        drgevent.Effect = DragDropEffects.Move;
      } else
        return;
      Point cp = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      this._tbo.SetCaretPosition(cp.X - this._tbo.Left);
    }
    #endregion

    #region protected override void OnDragDrop(DragEventArgs drgevent)
    protected override void OnDragDrop(DragEventArgs drgevent) {
      if(!drgevent.Data.GetDataPresent(typeof(IndicFunction).ToString()))
        return;

      Point cp = this.PointToClient(new Point(drgevent.X, drgevent.Y));

      IndicFunction indf = (IndicFunction)drgevent.Data.GetData(typeof(IndicFunction).ToString());
      IndicFunctionBox indfbox = this.AddIndicFunction(indf);

      _tbo.Focus();

      ExplorerPanel iibox = GordagoMain.MainForm.Explorer;
      iibox.UnSelectAllRowInViewIndicators();
      iibox.ClearIndicatorProperty();
    }
    #endregion

    #region public IndicFunctionBox AddIndicFunction(IndicFunction indf)
    public IndicFunctionBox AddIndicFunction(IndicFunction indf) {
      IndicFunction indins = null;
      IndicatorGUI indicNew = indf.Parent.Clone();
      indins = indicNew.GetIndicFunction(indf.Name);

      IndicFunctionBox indbox = new IndicFunctionBox(indins, null);
      _tbo.Insert(_tbo.Position, indbox);
      _tbo.Position++;
      _tbo.Invalidate();
      indbox.Invalidate();
      return indbox;
    }
    #endregion
  }
}
