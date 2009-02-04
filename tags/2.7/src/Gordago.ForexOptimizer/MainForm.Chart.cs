/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

using Language;
using Gordago.Strategy;
using Gordago.Strategy.IO;
using Gordago.Stock;
using Gordago.API;
using Gordago.Analysis.Chart;
using Cursit.Applic.AConfig;
using Cursit.Utils;
using Gordago.API.VirtualForex;
#endregion

namespace Gordago {
  partial class MainForm {

    #region public ChartForm ChartShowForm(Symbol symbol)
    public ChartForm ChartShowForm(ISymbol symbol) {
      ChartForm chform = this.ChartGetFormFromSymbol(symbol);
      if(chform != null) {
        chform.Activate();
        return chform;
      }
      return ChartShowNewForm(symbol);
    }
    #endregion

    #region public ChartsForm ChartShowNewForm(Symbol symbol)
    public ChartForm ChartShowNewForm(ISymbol symbol) {

      if (this.BCM.Broker != null && this.BCM.Broker is VirtualBroker) {
        VirtualBroker vb = this.BCM.Broker as VirtualBroker;
        if (!vb.UseAllSymbols)
          vb.AddSymbol(symbol);
      }

      int cnt = this.MdiChildren.Length;

      ChartForm cf = new ChartForm(symbol);
      cf.MdiParent = this;
      
      if (cnt == 0)
        cf.WindowState = FormWindowState.Maximized;

      cf.Visible = true;

      
      return cf;
    }
    #endregion

    #region internal void OnChartCreating(ChartManager cmanager)
    internal void OnChartCreating(ChartManager cmanager){
      if(ChartCreating != null) {
        ChartCreating(cmanager, new EventArgs());
      }
    }
    #endregion

    #region public ChartForm ChartGetFormFromSymbol(Symbol symbol)
    public ChartForm ChartGetFormFromSymbol(ISymbol symbol) {
      foreach(Form frm in this.MdiChildren) {
        if(frm is ChartForm) {
          ChartForm cform = frm as ChartForm;
          if(cform.Symbol == symbol) {
            return cform;
          }
        }
      }
      return null;
    }
    #endregion

    #region internal void _mniCTF_Click(object sender, EventArgs e)
    internal void _mniCTF_Click(object sender, EventArgs e) {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;

      string name = "_p_0";
      if(sender is ToolStripMenuItem) {
        name = (sender as ToolStripMenuItem).Name;
      } else if(sender is ToolStripButton) {
        name = (sender as ToolStripButton).Name;
      }
      string[] sa = name.Split('_');
      int second = 0;
      try {
        second = Convert.ToInt32(sa[2]);
      } catch { }

      cform.ChartManager.SetTimeFrame(TimeFrameManager.TimeFrames.GetTimeFrame(second));
    }
    #endregion

    #region internal bool ChartCheckTimeFrameNameFromItem(string name, int nsecond)
    internal bool ChartCheckTimeFrameNameFromItem(string name, int nsecond) {
      string[] sa = name.Split('_');
      int second = 0;
      try {
        second = Convert.ToInt32(sa[2]);
      } catch { }

      return TimeFrameManager.TimeFrames.GetTimeFrame(second).Second == nsecond;
    }
    #endregion

    #region internal void ChartSetTimeFrameView(int second)
    internal void ChartSetTimeFrameView(int second) {
      foreach(ToolStripMenuItem mnitf in _mniCTimeFrame.DropDownItems) 
        mnitf.Checked = ChartCheckTimeFrameNameFromItem(mnitf.Name, second);
      
      foreach(ToolStripMenuItem mnbtf in _mnbCTimeFrameList.DropDownItems) 
        mnbtf.Checked = ChartCheckTimeFrameNameFromItem(mnbtf.Name, second);
      
      foreach(ToolStripButton tfbtn in _tsTimeFrame.Items) 
        tfbtn.Checked = ChartCheckTimeFrameNameFromItem(tfbtn.Name, second);
      
    }
    #endregion

    #region private void SetMenuToolbarEnable(ToolStrip toolStrip, bool enable)
    private void SetMenuToolbarEnable(ToolStrip toolStrip, bool enable) {
      for (int i = 0; i < toolStrip.Items.Count; i++) {
        toolStrip.Items[i].Enabled = enable;
      }
    }
    #endregion

    #region private void ChartSetMenuEnabled(bool enabled)
    private void ChartSetMenuEnabled(bool enabled) {
      this._mniChart.Visible = enabled;
      this.SetMenuToolbarEnable(_tsChartObject, enabled);
      this.SetMenuToolbarEnable(_tsCharts, enabled);
      this.SetMenuToolbarEnable(_tsTimeFrame, enabled);


      if(!enabled) {
        this.ChartSetTimeFrameView(0);
        this.ChartSetBarType(ChartFigureBarType.None);
        this.ChartSetMouseType(null);
      } else {
        if(this.ActiveMdiChild is ChartForm) {
          this.ChartSetTimeFrameView((this.ActiveMdiChild as ChartForm).ChartManager.Bars.TimeFrame.Second);
        }
      }
    }
    #endregion

    #region private void ChartSetBarType(ChartFigureBarType btype)
    private void ChartSetBarType(ChartFigureBarType btype) {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetBarType(btype);
    }
    #endregion

    #region public void ChartSetBarTypeView()
    public void ChartSetBarTypeView(){
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;

      _mniCBar.Checked =
        _mnbCBar.Checked =
        _mniCCandle.Checked =
        _mnbCCandle.Checked =
        _mniCLine.Checked =
        _mnbCLine.Checked = false;

      switch(cform.ChartManager.BarType) {
        case ChartFigureBarType.Bar:
          _mniCBar.Checked = _mnbCBar.Checked = true;
          break;
        case ChartFigureBarType.CandleStick:
          _mniCCandle.Checked = _mnbCCandle.Checked = true;
          break;
        case ChartFigureBarType.Line:
          _mniCLine.Checked = _mnbCLine.Checked = true;
          break;
      }
    }
    #endregion

    #region private void ChartSetZoomIn()
    private void ChartSetZoomIn() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetZoomIn();
    }
    #endregion

    #region private void ChartSetZoomOut()
    private void ChartSetZoomOut() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetZoomOut();
    }
    #endregion

    #region public void ChartSetZoomView()
    public void ChartSetZoomView() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      this._mniCZoomIn.Enabled =
        this._mnbCZoomIn.Enabled = cform.ChartManager.IsZoomIn();
      this._mniCZoomOut.Enabled =
        this._mnbCZoomOut.Enabled = cform.ChartManager.IsZoomOut();
    }
    #endregion

    #region private void ChartSetGridVisible()
    private void ChartSetGridVisible() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetGridVisible(!cform.ChartManager.GridVisible);
    }
    #endregion

    #region private void ChartSetPeriodSeparators()
    private void ChartSetPeriodSeparators() {
      if (!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetPeriodSeparators(!cform.ChartManager.PeriodSeparators);
    }
    #endregion

    #region public void ChartSetGridVisibleView()
    public void ChartSetGridVisibleView() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      this._mniCGrid.Checked = cform.ChartManager.GridVisible;
    }
    #endregion

    public void ChartSetPeriodSeparatorsView() {
      if (!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      this._mniCPeriodSeparators.Checked = cform.ChartManager.PeriodSeparators;
    }

    #region private void ChartSetChartShift()
    private void ChartSetChartShift() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetChartShift(!cform.ChartManager.ChartShift);
    }
    #endregion

    #region public void ChartSetChartShiftView()
    public void ChartSetChartShiftView() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      this._mniCChartShift.Checked = this._mnbCChartShift.Checked = cform.ChartManager.ChartShift;
    }
    #endregion

    #region private void ChartSetAutoScroll()
    private void ChartSetAutoScroll() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartAutoScroll = !cform.ChartAutoScroll;
    }
    #endregion

    #region public void ChartSetAutoScrollView()
    public void ChartSetAutoScrollView() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      this._mniCAutoScroll.Checked = this._mnbCAutoScroll.Checked = cform.ChartAutoScroll;
    }
    #endregion

    public void ChartSetMouseType(ChartObject chartObject) {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;
      cform.ChartManager.SetMouseType(chartObject);
    }

    public void ChartSetMouseTypeView() {
      if(!(this.ActiveMdiChild is ChartForm)) return;
      ChartForm cform = this.ActiveMdiChild as ChartForm;

      string btnName = "";
      if(cform.ChartManager.MouseType != null) {
        btnName = GetChartObjectBTNNameFromTypeName(cform.ChartManager.MouseType.GetType().FullName);
      }

      for(int i = 0; i < _tsChartObject.Items.Count; i++) {
        ToolStripButton tsb = (_tsChartObject.Items[i] as ToolStripButton);
        if (tsb != null)
          tsb.Checked = tsb.Name == btnName;
      }


    }

    #region private void _mnbCODefault_Click(object sender, EventArgs e)
    private void _mnbCODefault_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    #region private void _mnbCOVerHorLine_Click(object sender, EventArgs e)
    private void _mnbCOVerHorLine_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    #region private void _mnbCOTrendLine_Click(object sender, EventArgs e)
    private void _mnbCOTrendLine_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    #region private void _mnbCOFibo_Click(object sender, EventArgs e)
    private void _mnbCOFibo_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    #region private void _mnbCOFiboFan_Click(object sender, EventArgs e)
    private void _mnbCOFiboFan_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    #region private void _mnbCOFiboArcs_Click(object sender, EventArgs e)
    private void _mnbCOFiboArcs_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    #region private void _mnbCOCycleLines_Click(object sender, EventArgs e)
    private void _mnbCOCycleLines_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }
    #endregion

    private void _mnbCOFiboTimeZone_Click(object sender, EventArgs e) {
      ChartSetMouseType(null);
    }

    #region private void _mniTemplate_DropDownOpening(object sender, EventArgs e)
    private void _mniTemplate_DropDownOpening(object sender, EventArgs e) {
      if(this.ActiveMdiChild != null && this.ActiveMdiChild is ChartForm)
        (this.ActiveMdiChild as ChartForm).Templates.Refresh(_mniTemplate);
    }
    #endregion

    #region private void _mniTSaveTemplate_Click(object sender, EventArgs e)
    private void _mniTSaveTemplate_Click(object sender, EventArgs e) {
      if(this.ActiveMdiChild != null && this.ActiveMdiChild is ChartForm)
        (this.ActiveMdiChild as ChartForm).Templates.Save();
    }
    #endregion

    #region private void _mniTLoadTemplate_Click(object sender, EventArgs e)
    private void _mniTLoadTemplate_Click(object sender, EventArgs e) {
      if(this.ActiveMdiChild != null && this.ActiveMdiChild is ChartForm)
        (this.ActiveMdiChild as ChartForm).Templates.Load();
    }
    #endregion

    private void _mniCSaveAsReport_Click(object sender, EventArgs e) {
      if (this.ActiveMdiChild != null && this.ActiveMdiChild is ChartForm)
        (this.ActiveMdiChild as ChartForm).SaveAsReport();
    }

    private void _mniCSaveAsPicture_Click(object sender, EventArgs e) {
      if (this.ActiveMdiChild != null && this.ActiveMdiChild is ChartForm)
        (this.ActiveMdiChild as ChartForm).SaveAsPicture();
    }
  }
}
