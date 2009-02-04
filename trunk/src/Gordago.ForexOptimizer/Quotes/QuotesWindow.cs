/**
* @version $Id: QuotesWindow.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Quotes
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.Docking;
  using Gordago.Core;
  using Gordago.Trader;

  partial class QuotesWindow : DockWindowPanel {

    private readonly DataTable _table;

    public QuotesWindow() {
      InitializeComponent();
      _table = new DataTable();
      _table.Columns.Add("symbol", typeof(string));
      _table.Columns.Add("bid", typeof(string));
      _table.Columns.Add("ask", typeof(string));
      _dataGridView.DataSource = _table;
    }

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      Global.MainForm.TablesManager.SaveDataGridView(_dataGridView);
      base.Dispose(disposing);
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {

      this.Text = this.TabText = Global.Languages["Symbols Window"]["Symbols"];

      LanguageManager.PhraseGroup pgroup = Global.Languages["Trader"];

      _dgvColSymbolName.HeaderText = pgroup["Symbol"];
      _dgvColBid.HeaderText = pgroup["Bid"];
      _dgvColAsk.HeaderText = pgroup["Ask"];

      Global.MainForm.TablesManager.InitializeDataGridView(_dataGridView);

      this.UpdateTable();

      base.OnLoad(e);
    }
    #endregion

    #region private void UpdateRow(ISymbol symbol, DataRow row)
    private void UpdateRow(ISymbol symbol, DataRow row) {
      row["symbol"] = symbol.Name;
    }
    #endregion

    #region public void UpdateTable()
    public void UpdateTable() {
      _table.Rows.Clear();
      foreach (ISymbol symbol in Global.Quotes) {
        DataRow row = _table.NewRow();
        this.UpdateRow(symbol, row);
        _table.Rows.Add(row);
      }
    }
    #endregion

    private void _dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
      string symbolName = (string)_dataGridView.Rows[e.RowIndex].Cells[0].Value;
      ISymbol symbol = Global.Quotes[symbolName];
      Global.DockManager.ShowChartDocument(symbol);

    }
  }
}
