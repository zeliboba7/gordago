/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace Cursit {
  public class DataList:ListView {

    #region class DataListViewItem : ListViewItem
    class DataListViewItem : ListViewItem {
      
      private int _id;
      private DataRow _row;
      private DataGridTableStyle _ts;

      #region public DataListViewItem(DataRow row, DataGridTableStyle ts)
      public DataListViewItem(DataRow row, DataGridTableStyle ts) {
        _ts = ts;
        _id = Convert.ToInt32(row["id"]);
        _row = row;
        this.Text = this.GetValue(row[ts.GridColumnStyles[0].MappingName]);
        
        if (ts.GridColumnStyles.Count == 1)
          return;

        for (int i = 1; i < ts.GridColumnStyles.Count; i++) {
          this.SubItems.Add(this.GetValue(row[ts.GridColumnStyles[i].MappingName]));
        }
      }
      #endregion

      #region public int Id
      public int Id {
        get { return this._id; }
      }
      #endregion

      #region public DataRow Row
      public DataRow Row {
        get { return _row; }
      }
      #endregion

      #region public DataGridTableStyle TableStyle
      public DataGridTableStyle TableStyle {
        get { return this._ts; }
      }
      #endregion

      #region private string GetValue(object value)
      private string GetValue(object value) {
        if (value is System.DBNull) {
          return "";
        } else if (value is DateTime) {
          return ((DateTime)value).ToString("dd-MM-yyyy");
        }
        return value.ToString();
      }
      #endregion

    }
    #endregion

    private DataTable _table;
    private int _selectedId;
    private DataRow _selectedRow;
    private DataGridTableStyle _ts;

    #region public DataList()
    public DataList() {
      this.FullRowSelect = true;
      this.View = View.Details;
      this.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    }
    #endregion

    #region public int SelectedId
    public int SelectedId {
      get { return this._selectedId; }
    }
    #endregion

    #region public DataRow SelectedRow
    public DataRow SelectedRow {
      get { return this._selectedRow; }
    }
    #endregion

    #region public DataGridTableStyle TableStyle
    public DataGridTableStyle TableStyle {
      get { return this._ts; }
    }
    #endregion

    #region public void SetTable(DataTable table, DataGridTableStyle ts)
    public void SetTable(DataTable table, DataGridTableStyle ts) {
      if (!ts.ColumnHeadersVisible) {
        this.HeaderStyle = ColumnHeaderStyle.None;
      }
      _table = table;
      _ts = ts;
      this.Items.Clear();
      this.Columns.Clear();

      for (int i = 0; i < ts.GridColumnStyles.Count; i++) {
        DataGridColumnStyle cs = ts.GridColumnStyles[i];
        this.Columns.Add(cs.HeaderText, cs.Width);
      }

      for (int i = 0; i < _table.Rows.Count; i++) {
        DataRow row = this._table.Rows[i];
        if (row.RowState != DataRowState.Deleted) {
          this.Items.Add(new DataListViewItem(row, ts));
        }
      }
    }
    #endregion

    #region protected override void OnSelectedIndexChanged(EventArgs e)
    protected override void OnSelectedIndexChanged(EventArgs e) {
      if (this.SelectedItems.Count == 0) {
        _selectedId = -1;
        _selectedRow = null;
      } else {
        DataListViewItem lvi = this.SelectedItems[0] as DataListViewItem;
        _selectedId = lvi.Id;
        _selectedRow = lvi.Row;
      }
      base.OnSelectedIndexChanged(e);
    }
    #endregion

    #region public void UpdateData()
    public void UpdateData() {
      this.SetTable(_table, _ts);
    }
    #endregion

    #region protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
    protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e) {
      base.OnColumnWidthChanged(e);
      _ts.GridColumnStyles[e.ColumnIndex].Width = this.Columns[e.ColumnIndex].Width;
    }
    #endregion

    #region public void UpdateStyle()
    public void UpdateStyle() {
      for (int i = 0; i < _ts.GridColumnStyles.Count; i++) {
        DataGridColumnStyle cs = _ts.GridColumnStyles[i];
        this.Columns[i].Width = cs.Width;
      }
    }
    #endregion

    #region public string GetConfig()
    public string GetConfig() {
      List<string> list = new List<string>();

      for (int i = 0; i < _ts.GridColumnStyles.Count; i++) {
        DataGridColumnStyle cs = _ts.GridColumnStyles[i];
        list.Add(cs.MappingName + ":" + cs.Width.ToString());
      }
      return string.Join("|", list.ToArray());
    }
    #endregion

    #region public void SetConfig(string config)
    public void SetConfig(string config) {
      try {
        string[] list = config.Split('|');
        foreach (string s in list) {
          string[] sa = s.Split(':');
          if (sa.Length != 2)
            continue;
          string name = sa[0];
          int width = Convert.ToInt32(sa[1]);

          if (!_ts.GridColumnStyles.Contains(name))
            continue;
          DataGridColumnStyle cs = _ts.GridColumnStyles[name];
          if (cs != null)
            cs.Width = width;
        }
      } catch { }
      this.UpdateStyle();
    }
    #endregion
  }
}
