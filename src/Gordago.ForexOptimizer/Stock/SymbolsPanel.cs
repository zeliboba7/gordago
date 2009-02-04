/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Cursit.Table;
using Gordago.Analysis.Chart;

using Gordago.API;
using Language;
using Cursit.Applic.AConfig;
#endregion

namespace Gordago.Stock {
	public partial class SymbolsPanel: UserControl, IBrokerEvents{

    private TableControl _table;
		
		private bool _enabled = true;
		private TableCellStyle _styleup;
		private TableCellStyle _styledown;

		private int _cwidth0 = 55, _cwidth1 = 55, _cwidth2 = 55;
    private ConfigValue _cfgval;

		public SymbolsPanel() {
      this.InitializeComponent();
      try {
        _cfgval = Config.Users["Terminal"];

        _table = new TableControl();
        _table.Dock = DockStyle.Fill;
        _table.ViewHGdirLines = true;

        _table.Columns.Add(Dictionary.GetString(18, 1, "Инструменты"), TableColumnType.Label, _cwidth0);
        _table.Columns.Add(Dictionary.GetString(18, 2, "Bid"), TableColumnType.Label, _cwidth1, ContentAlignment.MiddleRight);
        _table.Columns.Add(Dictionary.GetString(18, 3, "Ask"), TableColumnType.Label, _cwidth2, ContentAlignment.MiddleRight);
        _table.Columns.Add(Dictionary.GetString(18, 4, "Pip cost"), TableColumnType.Label, 55, ContentAlignment.MiddleRight);
        _table.Columns.Add(Dictionary.GetString(18, 5, "Spread"), TableColumnType.Label, 55, ContentAlignment.MiddleRight);

        _table.DoubleClick += new EventHandler(this._table_DoubleClick);
        _table.Click += new EventHandler(_table_Click);
        _table.SelectedIndexChanged += new EventHandler(this._table_SelectedIndexChanged);

        _styleup = _table.Style.Clone();
        _styleup.ForeColor = Color.Blue;

        _styledown = _table.Style.Clone();
        _styledown.ForeColor = Color.Red;
        this.UpdateSymbolTable();
        this.BrokerConnectionStatusChanged(BrokerConnectionStatus.Offline);

        string cfgs = _cfgval["SymbolsTW", "55|41|48|39|23"];
        _table.Columns.SetConfigWidths(cfgs);
        this.Controls.Add(_table);

        _mniNewChart.Text = Dictionary.GetString(18, 6, "Chart window");
        _mniHideSymbol.Text = Dictionary.GetString(18, 9, "Hide");
        _mniShowSymbol.Text = Dictionary.GetString(18, 10, "Show");
        _mniShowAllSymbol.Text = Dictionary.GetString(18, 11, "Show All");
        _mniHideAllSymbol.Text = Dictionary.GetString(18, 12, "Hide All");
      } catch { }
      _table.ContextMenuStrip = _cntxMenu;
      GordagoMain.MainForm.SetContextMenuOnTable(_table);
    }

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      base.Dispose(disposing);
      _cfgval["SymbolsTW"].SetValue(_table.Columns.GetConfigWidths());
    }
    #endregion

    #region public TableControl Table
    public TableControl Table {
      get { return this._table; }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region private void VisibleOnlineColumns(bool vis)
    private void VisibleOnlineColumns(bool vis) {
      _table.Columns[1].Visible = vis;
      _table.Columns[2].Visible = vis;
      _table.Columns[3].Visible = vis;
      _table.Columns[4].Visible = vis;
    }
    #endregion

    #region private void SymbolsTableUpdateRow(TableRow row, IOnlineRate onlineRate)
    private void SymbolsTableUpdateRow(TableRow row, IOnlineRate onlineRate){
      int decdig = onlineRate.Symbol.DecimalDigits;
      row.AdditionObject = onlineRate.Symbol;
      row[0].Text = onlineRate.Symbol.Name;
      row[1].Text = SymbolManager.ConvertToCurrencyString(onlineRate.SellRate, decdig);
      row[2].Text = SymbolManager.ConvertToCurrencyString(onlineRate.BuyRate, decdig);
      row[3].Text = SymbolManager.ConvertToCurrencyString(onlineRate.PipCost, 2);
      row[4].Text = Broker.CalculatePoint(onlineRate.SellRate, onlineRate.BuyRate, decdig, true).ToString();
			if (onlineRate.BuyRate > onlineRate.LastBuyRate){
				row.Style = this._styleup;
      } else if (onlineRate.BuyRate < onlineRate.LastBuyRate) {
				row.Style = this._styledown;
			}else{
				row.Style = null;
			}
    }
    #endregion

    #region private void _table_DoubleClick(object sender, EventArgs e)
    private void _table_DoubleClick(object sender, EventArgs e) {
			if (!this._enabled) return;

			if (_table.SelectedIndex < 0)
				return;

      string txt = _table.Rows[_table.SelectedIndex][0].Text;

			ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(txt);
      ChartForm cform = GordagoMain.MainForm.ChartShowNewForm(symbol);
      cform.ChartManager.SetPositionToEnd();
    }
    #endregion

    #region public void UpdateSymbolTable()
    public void UpdateSymbolTable() {
      _table.Rows.Clear();
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        if (!GordagoMain.MainForm.GetSymbolIsHide(symbol)) {
          TableRow row = _table.NewRow();
          row.AdditionObject = symbol;
          row[0].Text = symbol.Name;
          _table.Rows.AddRow(row);
          if (BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
            for (int j = 0; j < BCM.Broker.OnlineRates.Count; j++) {
              IOnlineRate onlineRate = this.BCM.Broker.OnlineRates[j];
              if (onlineRate.Symbol == symbol) {
                this.SymbolsTableUpdateRow(row, onlineRate);
                break;
              }
            }
          }
        }
      }
      _table.Invalidate();
    }
    #endregion

    #region private void _table_SelectedIndexChanged()
    private void _table_SelectedIndexChanged(object sender, EventArgs e) {
      if(_table.SelectedIndex < 0)
				return;

      string txt = _table.Rows[_table.SelectedIndex][0].Text;

      ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(txt);
    }
    #endregion

    #region private void _table_Click(object sender, EventArgs e)
    private void _table_Click(object sender, EventArgs e) {
			_table_SelectedIndexChanged(sender, e);
    }
    #endregion

    #region private void _cntxMenu_Opening(object sender, CancelEventArgs e)
    private void _cntxMenu_Opening(object sender, CancelEventArgs e) {
      _mniHideAllSymbol.Enabled =
        _mniHideSymbol.Enabled =
        _mniNewChart.Enabled = _table.Rows.Count != 0;

      ArrayList al = new ArrayList();
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        if (GordagoMain.MainForm.GetSymbolIsHide(symbol)) {
          ToolStripMenuItem mni = new ToolStripMenuItem(symbol.Name);
          mni.Name = "_mniShowSymbol_" + symbol.Name;
          mni.Click += new EventHandler(_mniShowIsSymbol_Click);
          al.Add(mni);
        }
      }
      _mniShowSymbol.Visible = al.Count > 0;
      _mniShowSymbol.DropDownItems.Clear();
      _mniShowSymbol.DropDownItems.AddRange((ToolStripMenuItem[])al.ToArray(typeof(ToolStripMenuItem)));
    }
    #endregion

    #region private void _mniShowIsSymbol_Click(object sender, EventArgs e)
    private void _mniShowIsSymbol_Click(object sender, EventArgs e) {
      ToolStripMenuItem mni = sender as ToolStripMenuItem;
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        if (mni.Name == "_mniShowSymbol_" + symbol.Name) {
          GordagoMain.MainForm.SetSymbolHide(symbol, false);
          foreach(TableRow row in this._table.Rows) {
            ISymbol smb = row.AdditionObject as ISymbol;
            if(smb == symbol) {
              _table.SelectedRow = row;
              break;
            }
          }
          break;
        }
      }
    }
    #endregion

    #region private void _mniHideSymbol_Click(object sender, EventArgs e)
    private void _mniHideSymbol_Click(object sender, EventArgs e) {
      if(_table.SelectedRow == null) return;
      ISymbol symbol = _table.SelectedRow.AdditionObject as ISymbol;
      GordagoMain.MainForm.SetSymbolHide(symbol, true);
    }
    #endregion

    #region private void _mniShowAllSymbol_Click(object sender, EventArgs e)
    private void _mniShowAllSymbol_Click(object sender, EventArgs e) {
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        GordagoMain.MainForm.SetSymbolHide(symbol, false);
      }
    }
    #endregion

    #region private void _mniHideAllSymbol_Click(object sender, EventArgs e)
    private void _mniHideAllSymbol_Click(object sender, EventArgs e) {
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        GordagoMain.MainForm.SetSymbolHide(symbol, true);
      }
    }
    #endregion

    #region private void _mniNewChart_Click(object sender, EventArgs e)
    private void _mniNewChart_Click(object sender, EventArgs e) {
      if(_table.SelectedRow == null) return;
      ISymbol symbol = _table.SelectedRow.AdditionObject as ISymbol;
      GordagoMain.MainForm.ChartShowNewForm(symbol);
    }
    #endregion

    //#region private void _mniTrade_Click(object sender, EventArgs e)
    //private void _mniTrade_Click(object sender, EventArgs e) {
    //  if(_table.SelectedRow == null) return;
    //  GordagoMain.MainForm.StatusTTPanel = true;
    //  ISymbol symbol = _table.SelectedRow.AdditionObject as ISymbol;
    //}
    //#endregion

    #region private void _mniDownHistory_Click(object sender, EventArgs e)
    private void _mniDownHistory_Click(object sender, EventArgs e) {
      if(_table.SelectedRow == null) return;
      ISymbol symbol = _table.SelectedRow.AdditionObject as ISymbol;
      GordagoMain.MainForm.ShowDownloadHistory(symbol);
    }
    #endregion

    #region public void BrokerConnectionStatusChanged(BrokerConnectionStatus status)
    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      switch (status) {
        case BrokerConnectionStatus.Offline:
        case BrokerConnectionStatus.WaitingForConnection:
        case BrokerConnectionStatus.LoadingData:
          //_table.AutoColumnSize = true;
          //_table.Columns[0].Width = _cwidth0 + _cwidth1 + _cwidth2;
//          this.VisibleOnlineColumns(false);
          this.UpdateSymbolTable();
          break;
        case BrokerConnectionStatus.Online:
          //_table.Columns[0].Width = _cwidth0;
          //_table.AutoColumnSize = false;
//          this.VisibleOnlineColumns(true);
          this.UpdateSymbolTable();
          break;
      }
      this.Refresh();
    }
    #endregion

    #region public void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {

    }
    #endregion

    #region public void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
    }
    #endregion

    #region public void BrokerTradesChanged(BrokerTradesEventArgs be)
    public void BrokerTradesChanged(BrokerTradesEventArgs be) {

    }
    #endregion

    #region public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      for (int i = 0; i < _table.Rows.Count; i++) {
        TableRow row = _table.Rows[i];
        if (row.AdditionObject == be.OnlineRate.Symbol) {
          this.SymbolsTableUpdateRow(row, be.OnlineRate);
          break;
        }
      }
      this.Invalidate();
    }
    #endregion

    #region public void BrokerCommandStarting(BrokerCommand command)
    public void BrokerCommandStarting(BrokerCommand command) {
    }
    #endregion

    #region public void BrokerCommandStopping(BrokerCommand command, BrokerResult result)
    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
    }
    #endregion

    //private void _mniTrade_Click_1(object sender, EventArgs e) {

    //}

  }
}
