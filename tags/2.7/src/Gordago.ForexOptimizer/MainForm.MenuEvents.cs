/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gordago.Analysis.Chart;
using Gordago.API;

namespace Gordago {
  partial class MainForm {
    #region private void _mniExit_Click(object sender, EventArgs e)
    private void _mniExit_Click(object sender, EventArgs e) {
      this.Close();
    }
    #endregion

    #region private void _mniFileExportMQL_Click(object sender, EventArgs e)
    private void _mniFileExportMQL_Click(object sender, EventArgs e) {
      this.ExportMQL4();
    }
    #endregion

    #region private void _mniFileOpen_Click(object sender, EventArgs e)
    private void _mniFileOpen_Click(object sender, EventArgs e) {
      _sewfs.OpenFromFileDialog();
    }
    #endregion

    #region private void _mnbFileOpen_Click(object sender, EventArgs e)
    private void _mnbFileOpen_Click(object sender, EventArgs e) {
      _sewfs.OpenFromFileDialog();
    }
    #endregion

    #region private void _mniFileSymbol0_Click(object sender, EventArgs e)
    private void _mniFileSymbol0_Click(object sender, EventArgs e) {
      OpenStrategyFromMenuFastFile(0);
    }
    #endregion

    #region private void _mniFileSymbol1_Click(object sender, EventArgs e)
    private void _mniFileSymbol1_Click(object sender, EventArgs e) {
      OpenStrategyFromMenuFastFile(1);
    }
    #endregion

    #region private void _mniFileSymbol2_Click(object sender, EventArgs e)
    private void _mniFileSymbol2_Click(object sender, EventArgs e) {
      OpenStrategyFromMenuFastFile(2);
    }
    #endregion

    #region private void _mniFileSymbol3_Click(object sender, EventArgs e)
    private void _mniFileSymbol3_Click(object sender, EventArgs e) {
      OpenStrategyFromMenuFastFile(3);
    }
    #endregion

    #region private void _mniFileSymbol4_Click(object sender, EventArgs e)
    private void _mniFileSymbol4_Click(object sender, EventArgs e) {
      OpenStrategyFromMenuFastFile(4);
    }
    #endregion

    #region private void _mniFileNew_Click(object sender, EventArgs e)
    private void _mniFileNew_Click(object sender, EventArgs e) {
      CreateNewStrategy();
    }
    #endregion

    #region private void _mnbFileNew_Click(object sender, EventArgs e)
    private void _mnbFileNew_Click(object sender, EventArgs e) {
      CreateNewStrategy();
    }
    #endregion

    #region private void _mniVLEnglish_Click(object sender, EventArgs e)
    private void _mniVLEnglish_Click(object sender, EventArgs e) {
      SetLanguage("English");
    }
    #endregion

    #region private void _mniVLRussian_Click(object sender, EventArgs e)
    private void _mniVLRussian_Click(object sender, EventArgs e) {
      SetLanguage("Russian");
    }
    #endregion

    #region private void _mniSSettings_Click(object sender, EventArgs e)
    private void _mniSSettings_Click(object sender, EventArgs e) {
      GConfig.ConfigForm frmConfig = new Gordago.GConfig.ConfigForm();
      frmConfig.ShowDialog();
    }
    #endregion

    #region private void _mniSDownloadHistory_Click(object sender, EventArgs e)
    private void _mniSDownloadHistory_Click(object sender, EventArgs e) {
      this.ShowDownloadHistory(null);
    }
    #endregion

    #region private void _mniVSymbols_Click(object sender, EventArgs e)
    private void _mniVSymbols_Click(object sender, EventArgs e) {
      this.ViewSymbolPanel = !this.ViewSymbolPanel;
    }
    #endregion

    #region private void _mnbVSymbols_Click(object sender, EventArgs e)
    private void _mnbVSymbols_Click(object sender, EventArgs e) {
      this.ViewSymbolPanel = !this.ViewSymbolPanel;
    }
    #endregion

    #region private void _mniVTools_Click(object sender, EventArgs e)
    private void _mniVTools_Click(object sender, EventArgs e) {
      this.ViewIIBoxPanel = !this.ViewIIBoxPanel;
    }
    #endregion

    #region private void _mnbViewTools_Click(object sender, EventArgs e)
    private void _mnbViewTools_Click(object sender, EventArgs e) {
      this.ViewIIBoxPanel = !this.ViewIIBoxPanel;
    }
    #endregion

    #region private void _mniVControlPanel_Click(object sender, EventArgs e)
    private void _mniVControlPanel_Click(object sender, EventArgs e) {
      //this.ViewTesterOrTerminalPanel = !this.ViewTesterOrTerminalPanel;
      this.ViewTester = !this.ViewTester;
    }
    #endregion

    #region private void _mnbVControlPanel_Click(object sender, EventArgs e)
    private void _mnbVControlPanel_Click(object sender, EventArgs e) {
      this.ViewTester = !this.ViewTester;
    }
    #endregion

    #region private void _mniFileSaveAs_Click(object sender, EventArgs e)
    private void _mniFileSaveAs_Click(object sender, EventArgs e) {
      this.SaveAsStrategy();
    }
    #endregion

    #region private void _mniFileSave_Click(object sender, EventArgs e)
    private void _mniFileSave_Click(object sender, EventArgs e) {
      this.SaveStrategy();
    }
    #endregion

    #region private void _mnbFileSave_Click(object sender, EventArgs e)
    private void _mnbFileSave_Click(object sender, EventArgs e) {
      this.SaveStrategy();
    }
    #endregion

    #region private void _mniHAbout_Click(object sender, EventArgs e)
    private void _mniHAbout_Click(object sender, EventArgs e) {
      AboutForm afrm = new AboutForm();
      afrm.ShowDialog();
    }
    #endregion

    #region private void _mniSUpdate_Click(object sender, EventArgs e)
    private void _mniSUpdate_Click(object sender, EventArgs e) {
      this.UpdateApplic();
    }
    #endregion

    #region private void _mniHReg_Click(object sender, EventArgs e)
    private void _mniHReg_Click(object sender, EventArgs e) {
      ShowRegPage();
    }
    #endregion

    #region private void _mniWCascade_Click(object sender, EventArgs e)
    private void _mniWCascade_Click(object sender, EventArgs e) {
      this.LayoutMdi(MdiLayout.Cascade);
    }
    #endregion

    #region private void _mniWHoriz_Click(object sender, EventArgs e)
    private void _mniWHoriz_Click(object sender, EventArgs e) {
      this.LayoutMdi(MdiLayout.TileHorizontal);
    }
    #endregion

    #region private void _mniWVert_Click(object sender, EventArgs e)
    private void _mniWVert_Click(object sender, EventArgs e) {
      this.LayoutMdi(MdiLayout.TileVertical);
    }
    #endregion

    #region private void _mniVTStandart_Click(object sender, EventArgs e)
    private void _mniVTStandart_Click(object sender, EventArgs e) {
      this._tsStandart.Visible = !_tsStandart.Visible;
    }
    #endregion

    #region private void _mniVTLineStudies_Click(object sender, EventArgs e)
    private void _mniVTLineStudies_Click(object sender, EventArgs e) {
      _tsChartObject.Visible = !_tsChartObject.Visible;
    }
    #endregion

    #region private void _mncnLineStudies_Click(object sender, EventArgs e)
    private void _mncnLineStudies_Click(object sender, EventArgs e) {
      _tsChartObject.Visible = !_tsChartObject.Visible;
    }
    #endregion

    private void _mncnServer_Click(object sender, EventArgs e) {
      _tsVirtualServer.Visible = !this._tsVirtualServer.Visible;
    }

    #region private void _mniVTCharts_Click(object sender, EventArgs e)
    private void _mniVTCharts_Click(object sender, EventArgs e) {
      //this.ViewToolCharts = !this.ViewToolCharts;
      this._tsCharts.Visible = !this._tsCharts.Visible;
    }
    #endregion

    #region private void _mniVTTimeFrames_Click(object sender, EventArgs e)
    private void _mniVTTimeFrames_Click(object sender, EventArgs e) {
      //this.ViewToolTimeFrames = !this.ViewToolTimeFrames;
      this._tsTimeFrame.Visible = !this._tsTimeFrame.Visible;
    }
    #endregion

    private void _tsVirtualServer_VisibleChanged(object sender, EventArgs e) {
      UpdateToolsPanelView();
    }

    private void _mniVTBroker_Click(object sender, EventArgs e) {
      _tsVirtualServer.Visible = !this._tsVirtualServer.Visible;
    }

    #region private void _mnbCBar_Click(object sender, EventArgs e)
    private void _mnbCBar_Click(object sender, EventArgs e) {
      this.ChartSetBarType(ChartFigureBarType.Bar);
    }
    #endregion

    #region private void _mniCBar_Click(object sender, EventArgs e)
    private void _mniCBar_Click(object sender, EventArgs e) {
      this.ChartSetBarType(ChartFigureBarType.Bar);
    }
    #endregion

    #region private void _mnbCCandle_Click(object sender, EventArgs e)
    private void _mnbCCandle_Click(object sender, EventArgs e) {
      this.ChartSetBarType(ChartFigureBarType.CandleStick);
    }
    #endregion

    #region private void _mniCCandle_Click(object sender, EventArgs e)
    private void _mniCCandle_Click(object sender, EventArgs e) {
      this.ChartSetBarType(ChartFigureBarType.CandleStick);
    }
    #endregion

    #region private void _mnbCLine_Click(object sender, EventArgs e)
    private void _mnbCLine_Click(object sender, EventArgs e) {
      this.ChartSetBarType(ChartFigureBarType.Line);
    }
    #endregion

    #region private void _mniCLine_Click(object sender, EventArgs e)
    private void _mniCLine_Click(object sender, EventArgs e) {
      this.ChartSetBarType(ChartFigureBarType.Line);
    }
    #endregion

    #region private void _mnbAutoScroll_Click(object sender, EventArgs e)
    private void _mnbAutoScroll_Click(object sender, EventArgs e) {
      this.ChartSetAutoScroll();
    }
    #endregion

    #region private void _mniCAutoScroll_Click(object sender, EventArgs e)
    private void _mniCAutoScroll_Click(object sender, EventArgs e) {
      this.ChartSetAutoScroll();
    }
    #endregion

    #region private void _mnbChartShift_Click(object sender, EventArgs e)
    private void _mnbChartShift_Click(object sender, EventArgs e) {
      this.ChartSetChartShift();
    }
    #endregion

    #region private void _mniCChartShift_Click(object sender, EventArgs e)
    private void _mniCChartShift_Click(object sender, EventArgs e) {
      this.ChartSetChartShift();
    }
    #endregion

    #region private void _mniCZoomIn_Click(object sender, EventArgs e)
    private void _mniCZoomIn_Click(object sender, EventArgs e) {
      this.ChartSetZoomIn();
    }
    #endregion

    #region private void _mniCZoomOut_Click(object sender, EventArgs e)
    private void _mniCZoomOut_Click(object sender, EventArgs e) {
      this.ChartSetZoomOut();
    }
    #endregion

    #region private void _mnbCZoomIn_Click(object sender, EventArgs e)
    private void _mnbCZoomIn_Click(object sender, EventArgs e) {
      this.ChartSetZoomIn();
    }
    #endregion

    #region private void _mnbCZoomOut_Click(object sender, EventArgs e)
    private void _mnbCZoomOut_Click(object sender, EventArgs e) {
      this.ChartSetZoomOut();
    }
    #endregion

    #region private void _mniCGrid_Click(object sender, EventArgs e)
    private void _mniCGrid_Click(object sender, EventArgs e) {
      this.ChartSetGridVisible();
    }
    #endregion

    #region private void _mniCPeriodSeparators_Click(object sender, EventArgs e)
    private void _mniCPeriodSeparators_Click(object sender, EventArgs e) {
      this.ChartSetPeriodSeparators();
    }
    #endregion

    #region private void _mniConnect_Click(object sender, EventArgs e)
    private void _mniConnect_Click(object sender, EventArgs e) {
      this.ShowConnectForm();
    }
    #endregion

    #region private void _mnbConnect_Click(object sender, EventArgs e)
    private void _mnbConnect_Click(object sender, EventArgs e) {
      this.ShowConnectForm();
    }
    #endregion

    #region private void _mniDisconnect_Click(object sender, EventArgs e)
    private void _mniDisconnect_Click(object sender, EventArgs e) {
      BCM.ExecuteCommand(new BrokerCommandLogoff());
    }
    #endregion

    #region private void _mnbDisconnect_Click(object sender, EventArgs e)
    private void _mnbDisconnect_Click(object sender, EventArgs e) {
      BCM.ExecuteCommand(new BrokerCommandLogoff());
    }
    #endregion

    #region private void _mncnStandart_Click(object sender, EventArgs e)
    private void _mncnStandart_Click(object sender, EventArgs e) {
      this._tsStandart.Visible = !_tsStandart.Visible;
    }
    #endregion

    #region private void _mncnCharts_Click(object sender, EventArgs e)
    private void _mncnCharts_Click(object sender, EventArgs e) {
      this._tsCharts.Visible = !_tsCharts.Visible;
    }
    #endregion

    #region private void _mncnTimeFrame_Click(object sender, EventArgs e)
    private void _mncnTimeFrame_Click(object sender, EventArgs e) {
      this._tsTimeFrame.Visible = !this._tsTimeFrame.Visible;
    }
    #endregion

    #region private void _mniVCPTerminal_Click(object sender, EventArgs e)
    private void _mniVCPTerminal_Click(object sender, EventArgs e) {
      this.StatusTTPanel = true;
    }
    #endregion

    #region private void _mnbVCPTerminal_Click(object sender, EventArgs e)
    private void _mnbVCPTerminal_Click(object sender, EventArgs e) {
      this.StatusTTPanel = true;
    }
    #endregion



    #region private void _mniVCPTester_Click(object sender, EventArgs e)
    private void _mniVCPTester_Click(object sender, EventArgs e) {
      this.StatusTTPanel = false;
    }
    #endregion

    #region private void _mnbVCPTester_Click(object sender, EventArgs e)
    private void _mnbVCPTester_Click(object sender, EventArgs e) {
      this.StatusTTPanel = false;
    }
    #endregion

    #region private void _mniVStatusBar_Click(object sender, EventArgs e)
    private void _mniVStatusBar_Click(object sender, EventArgs e) {
      this.ViewStatusBar = !this.ViewStatusBar;
    }
    #endregion

    #region private void _mniVSConnect_Click(object sender, EventArgs e)
    private void _mniVSConnect_Click(object sender, EventArgs e) {
      this.ShowConnectForm();
    }
    #endregion

    #region private void _mniVSDisconnect_Click(object sender, EventArgs e)
    private void _mniVSDisconnect_Click(object sender, EventArgs e) {
      BCM.ExecuteCommand(new BrokerCommandLogoff());
    }
    #endregion

    #region private void _mnbVSConnect_Click(object sender, EventArgs e)
    private void _mnbVSConnect_Click(object sender, EventArgs e) {
      this.ShowConnectForm();
    }
    #endregion

    #region private void _mnbVSDisconnect_Click(object sender, EventArgs e)
    private void _mnbVSDisconnect_Click(object sender, EventArgs e) {
      BCM.ExecuteCommand(new BrokerCommandLogoff());
    }
    #endregion


  }
}
