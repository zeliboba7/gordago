/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
using System.Collections;
#endregion

namespace Cursit.Table {

	public class TableColumnCollection {
		
		private TableColumn[] _columns;
		private int _width;
    private TableControl _parent;

    private bool _isChangedWidth = true;

    internal TableColumnCollection(TableControl parent) {
			_columns = new TableColumn[]{};
      _parent = parent;
		}

		#region public TableColumn this[int index]
		public TableColumn this[int index]{
			get{return this._columns[index];}
		}
		#endregion

		#region public int Count
		public int Count{
			get{return this._columns.Length;}
		}
		#endregion

    #region public int CountVisible
    public int CountVisible {
      get {
        int cnt = 0;
        for (int i = 0; i < _columns.Length; i++) {
          if (_columns[i].Visible)
            cnt++;
        }
        return cnt;
      }
    }
    #endregion

    #region internal int ColumnsWidth
    /// <summary>
		/// Суммарная ширина колонок
		/// </summary>
		internal int ColumnsWidth{
			get{
        if (_isChangedWidth) {
          _isChangedWidth = false;
          _width = 0;
          for (int i = 0; i < this._columns.Length; i++) {
            TableColumn column = this._columns[i];
            if (column.Visible) {
              _width += column.Width;
            }
          }
        }
        return this._width;
      }
		}
		#endregion

    #region internal TableControl Parent
    internal TableControl Parent {
      get { return this._parent; }
    }
    #endregion

    #region internal void Refresh()
    internal void Refresh() {
      _isChangedWidth = true;
      if (_parent != null) {
        _parent.Invalidate();
      }
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _columns = new TableColumn[0];
      this.Refresh();
    }
    #endregion

    #region public void Add(TableColumn column)
    public void Add(TableColumn column) {
      column.SetParent(this);
      ArrayList al = new ArrayList(_columns);
      al.Add(column);
      _columns = (TableColumn[])al.ToArray(typeof(TableColumn));
      this.Refresh();
    }
    #endregion

    #region public void Add(string caption, TableColumnType type, int width)
    public void Add(string caption, TableColumnType type, int width) {
      TableColumn col = new TableColumn(caption, type, width);
      this.Add(col);
    }
    #endregion

    #region public void Add(string caption, TableColumnType type, int width, ContentAlignment cellAlignment)
    public void Add(string caption, TableColumnType type, int width, ContentAlignment cellAlignment) {
      TableColumn col = new TableColumn(caption, type, width);
      col.CellAlignment = cellAlignment;
      this.Add(col);
    }
    #endregion

    #region public void SetConfigWidths(string cfgString)
    public void SetConfigWidths(string cfgString){
			if (cfgString == "")
				return;

			string[] sa = cfgString.Split('|');
			for(int i=0;i<sa.Length;i++){
				int width = 0;
        bool visible = true;
				try{
          string[] sa1 = sa[i].Split(':');
					width = Convert.ToInt32(sa1[0]);
          if (sa1.Length > 1) {
            visible = sa1[1] != "0";
          }
        
				}catch{};
        if (width > 0 && i < this._columns.Length) {
          this._columns[i].Width = width;
          this._columns[i].Visible = visible;
        }
			}
    }
    #endregion

    #region public string GetConfigWidths()
    public string GetConfigWidths() {
      string[] sa = new string[_columns.Length];
      for (int i = 0; i < _columns.Length; i++) {
        string svis = _columns[i].Visible ? "1" : "0";
        sa[i] = Convert.ToString(_columns[i].Width) + ":" + svis;
      }

      return string.Join("|", sa);
    }
    #endregion
  }
}
