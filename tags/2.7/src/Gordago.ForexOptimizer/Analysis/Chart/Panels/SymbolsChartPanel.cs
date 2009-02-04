/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cursit.Table;
using Cursit.Applic.AConfig;
using Gordago.API;
using Language;
using System.Collections;
#endregion

namespace Gordago.Analysis.Chart {

  public partial class SymbolsChartPanel : ChartPanel, IBrokerEvents {

    private TableControl _table;

    private TableCellStyle _styleup;
    private TableCellStyle _styledown;

    private ConfigValue _cfgval;

    public SymbolsChartPanel() {
      InitializeComponent();
      this.Properties = new SymbolsChartPanelProperties(this);
      for (int i = 0; i< GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];

        if (GordagoMain.MainForm.GetSymbolIsHide(symbol)) {
          this.SProperies.AddHideSymbol(symbol.Name);
        }
      }

      try {
        _cfgval = Config.Users["Terminal"];

        _table = new TableControl();
        _table.CaptionVisible = false;
        _table.Dock = DockStyle.Fill;
        _table.ViewHGdirLines = true;
        _table.BorderVisible = false;

        _table.Columns.Add(Dictionary.GetString(18, 1, "Инструменты"), TableColumnType.Label, 50);
        _table.Columns.Add(Dictionary.GetString(18, 2, "Bid"), TableColumnType.Label, 50, ContentAlignment.MiddleRight);
        _table.Columns.Add(Dictionary.GetString(18, 3, "Ask"), TableColumnType.Label, 50, ContentAlignment.MiddleRight);

        _table.DoubleClick += new EventHandler(this._table_DoubleClick);

        this.Text = Dictionary.GetString(18, 1, "Инструменты");

        _table.Style.Font = new Font("Microsoft Sans Serif", 7);

        _styleup = _table.Style.Clone();
        _styleup.ForeColor = Color.Blue;

        _styledown = _table.Style.Clone();
        _styledown.ForeColor = Color.Red;

        this.UpdateSymbolTable();
        this.BrokerConnectionStatusChanged(BrokerConnectionStatus.Offline);

        string cfgs = _cfgval["SymbolsTableWidths", ""];
        _table.Columns.SetConfigWidths(cfgs);
        this.Controls.Add(_table);

        _mniNewChart.Text = Dictionary.GetString(18, 6, "Chart window");
        _mniHideSymbol.Text = Dictionary.GetString(18, 9, "Hide");
        _mniShowSymbol.Text = Dictionary.GetString(18, 10, "Show");
        _mniShowAllSymbol.Text = Dictionary.GetString(18, 11, "Show All");
        _mniHideAllSymbol.Text = Dictionary.GetString(18, 12, "Hide All");
      } catch { }
    }

    #region private SymbolsChartPanelProperties SProperies
    private SymbolsChartPanelProperties SProperies {
      get { return this.Properties as SymbolsChartPanelProperties; }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region private void SymbolsTableUpdateRow(TableRow row, IOnlineRate onlineRate)
    private void SymbolsTableUpdateRow(TableRow row, IOnlineRate onlineRate) {
      int decdig = onlineRate.Symbol.DecimalDigits;
      row.AdditionObject = onlineRate.Symbol;
      row[0].Text = onlineRate.Symbol.Name;
      row[1].Text = SymbolManager.ConvertToCurrencyString(onlineRate.SellRate, decdig);
      row[2].Text = SymbolManager.ConvertToCurrencyString(onlineRate.BuyRate, decdig);
      if (onlineRate.SellRate > onlineRate.LastSellRate) {
        row.Style = this._styleup;
      } else if (onlineRate.SellRate < onlineRate.LastSellRate) {
        row.Style = this._styledown;
      } else {
        row.Style = null;
      }
    }
    #endregion

    #region private void _table_DoubleClick(object sender, EventArgs e)
    private void _table_DoubleClick(object sender, EventArgs e) {
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

        if (!this.SProperies.CheckHideSymbol(symbol.Name)) {
          TableRow row = _table.NewRow();
          row.AdditionObject = symbol;
          row[0].Text = symbol.Name;
          _table.Rows.AddRow(row);
          if (this.BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
            for (int j = 0; j < this.BCM.Broker.OnlineRates.Count; j++) {
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

    #region private void _cntxMenu_Opening(object sender, CancelEventArgs e)
    private void _cntxMenu_Opening(object sender, CancelEventArgs e) {
      _mniHideAllSymbol.Enabled =
        _mniHideSymbol.Enabled =
        _mniNewChart.Enabled = _table.Rows.Count != 0;

      ArrayList al = new ArrayList();
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];

        if (this.SProperies.CheckHideSymbol(symbol.Name)) {
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
          this.SProperies.RemoveHideSymbol(symbol.Name);
          this.UpdateSymbolTable();
          break;
        }
      }
    }
    #endregion

    #region private void _mniHideSymbol_Click(object sender, EventArgs e)
    private void _mniHideSymbol_Click(object sender, EventArgs e) {
      if (_table.SelectedRow == null)
        return;
      ISymbol symbol = _table.SelectedRow.AdditionObject as ISymbol;
      this.SProperies.AddHideSymbol(symbol.Name);
      this.UpdateSymbolTable();
    }
    #endregion

    #region private void _mniShowAllSymbol_Click(object sender, EventArgs e)
    private void _mniShowAllSymbol_Click(object sender, EventArgs e) {
      this.SProperies.ClearHideSymbol();
      this.UpdateSymbolTable();
    }
    #endregion

    #region private void _mniHideAllSymbol_Click(object sender, EventArgs e)
    private void _mniHideAllSymbol_Click(object sender, EventArgs e) {
      for (int i = 0; i < GordagoMain.SymbolEngine.Count; i++) {
        ISymbol symbol = GordagoMain.SymbolEngine[i];
        this.SProperies.AddHideSymbol(symbol.Name);
      }
      this.UpdateSymbolTable();
    }
    #endregion

    #region private void _mniNewChart_Click(object sender, EventArgs e)
    private void _mniNewChart_Click(object sender, EventArgs e) {
      if (_table.SelectedRow == null)
        return;
      ISymbol symbol = _table.SelectedRow.AdditionObject as ISymbol;
      GordagoMain.MainForm.ChartShowNewForm(symbol);
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      this.ReCalculateColWidth();
    }
    #endregion

    #region private void ReCalculateColWidth()
    private void ReCalculateColWidth() {
      bool connect = this.BCM.ConnectionStatus == BrokerConnectionStatus.Online;
      int w = this._table.ClientSize.Width;
      if (!connect) {
        _table.Columns[0].Width = w;
        if (_table.Columns[1].Visible) {
          _table.Columns[1].Visible = false;
          _table.Columns[2].Visible = false;
        }
        return;
      }
      if (!_table.Columns[1].Visible) {
        _table.Columns[1].Visible = true;
        _table.Columns[2].Visible = true;
      }

      int w1 = Convert.ToInt32((float)w * 0.30F);
      _table.Columns[0].Width = w-w1*2;
      _table.Columns[1].Width = w1;
      _table.Columns[2].Width = w1;
    }
    #endregion

    #region protected override void OnLoadSettingsCompleate()
    protected override void OnLoadSettingsCompleate() {
      this.UpdateSymbolTable();
    }
    #endregion

    #region IBrokerEvents Members

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      this.UpdateSymbolTable();
      this.ReCalculateColWidth();
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
      for (int i = 0; i < _table.Rows.Count; i++) {
        TableRow row = _table.Rows[i];
        if (row.AdditionObject == be.OnlineRate.Symbol) {
          this.SymbolsTableUpdateRow(row, be.OnlineRate);
          break;
        }
      }
      this.Invalidate();
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