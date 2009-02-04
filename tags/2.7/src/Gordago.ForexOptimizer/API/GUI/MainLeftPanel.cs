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
using Cursit.Applic.AConfig;
using Cursit.Table;
using Language;
using Gordago.Stock;

namespace Gordago.API {
  public partial class MainLeftPanel:UserControl, IBrokerEvents {

    private float _heightHO;

    public MainLeftPanel() {
      this.InitializeComponent();
      _heightHO = this._tableLayout.RowStyles[1].Height;
      this.SetSymbol(null);
    }

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public SymbolsPanel SymbolsPanel
    public SymbolsPanel SymbolsPanel {
      get { return this._symbolsPanel; }
    }
    #endregion

    #region public HandOperateNew HandOperate
    public HandOperate HandOperate {
      get { return this._handOperate; }
    }
    #endregion

    #region public void CloseHandOperatePanel()
    public void CloseHandOperatePanel() {
      this._tableLayout.RowStyles[1].Height = 0;
    }
    #endregion

    public void SetSymbol(ISymbol symbol) {
      bool online = BCM.ConnectionStatus == BrokerConnectionStatus.Online;

      if(symbol != null && online) {
        this._handOperate.SetSymbol(symbol);
        this._tableLayout.RowStyles[1].Height = _heightHO;
      } else {
        this.CloseHandOperatePanel();
      }
    }

    #region IBrokerEvents Members

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      if (this._handOperate.Symbol == null) return;
      if (this._handOperate.Symbol != be.OnlineRate.Symbol) return;

      this.SetSymbol(be.OnlineRate.Symbol);
    }

    public void BrokerCommandStarting(BrokerCommand command) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
