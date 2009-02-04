/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Cursit.Table {
	public class TableRowCollection: IEnumerable{
		
    private TableColumnCollection _columns;
		private TableRow[] _rows;
		private int _selectrowindex;
		private bool _headervisible;
		private TableControl _table;

    private int _rowheight = 17;
    private int _firstViewRow;

		internal TableRowCollection(TableControl table) {
			
			_table = table;
			_columns = table.Columns;
			_rows = new TableRow[]{};
			System.Data.DataTable tbl = new System.Data.DataTable();
			_selectrowindex = -1;

		}

    #region public int FirstViewRow
    /// <summary>
    /// Ќомер первой просматриваемой строки
    /// </summary>
    public int FirstViewRow {
      get { return this._firstViewRow; }
      set {
        if (_rows.Length == 0)
          value = 0;
        else {
          value = Math.Max(0, value);
          value = Math.Min(this._rows.Length - 1, value);
        }
        this._firstViewRow = value; 
      }
    }
    #endregion

		#region public TableRow this[int index]
		public TableRow this[int index]{
			get{return this._rows[index];}
		}
		#endregion

		#region public int Count
		public int Count{
			get{return this._rows.Length;}
		}
		#endregion

		#region public bool HeaderVisible
		public bool HeaderVisible{
			get{return this._headervisible;}
			set{this._headervisible = value;}
		}
		#endregion

    #region public int RowHeight
    public int RowHeight {
      get { return this._rowheight; }
      set { this._rowheight = value; }
    }
    #endregion

    #region internal int RowsHeight
    internal int RowsHeight {
      get {
        return _rowheight * this.Count;
      }
    }
    #endregion

    #region internal TableRow NewRow()
    internal TableRow NewRow(){
			TableRow row = new TableRow(_table);
			return row;
		}
		#endregion

		#region public void AddRow(TableRow row)
		public void AddRow(TableRow row){
			ArrayList al = new ArrayList(this._rows);
			al.Add(row);
			this._rows = (TableRow[])al.ToArray(typeof(TableRow));
		}
		#endregion

		#region public void RemoveAt(int index)
		public void RemoveAt(int index){
			ArrayList al = new ArrayList(this._rows);
			al.RemoveAt(index);
			_rows = (TableRow[])al.ToArray(typeof (TableRow));
		}
		#endregion

		#region public void Clear()
		public void Clear(){
      if(_table.SelectedRow != null)
        _table.SelectedRow = null;
			_rows = new TableRow[]{};
		}
		#endregion

		#region internal void CheckRowsSyncronizedColumns()
		internal void CheckRowsSyncronizedColumns(){
			foreach (TableRow row in this._rows){
				row.CheckCells();
			}
		}
		#endregion

		#region internal int SelectedRowIndex
		internal int SelectedRowIndex{
			get{
				if (_selectrowindex >= _rows.Length)
					return -1;
				return this._selectrowindex;
			}
			set{this._selectrowindex = value;}
		}
		#endregion

		#region internal TableRow SelectedRow
		internal TableRow SelectedRow{
			get{
				int sindex = this.SelectedRowIndex;
				if (sindex < 0)
					return null;
				return _rows[sindex];
			}
			set{
				_selectrowindex = -1;
				for (int i=0;i<_rows.Length;i++){
					if (_rows[i] == value){
						_selectrowindex = i;
						break;
					}
				}
			}
		}
		#endregion

		#region public TableRow[] FindRow(object additionObject)
		public TableRow[] FindRow(object additionObject){
			ArrayList al = new ArrayList();
			foreach (TableRow row in this._rows){
				if (row.AdditionObject == additionObject){
					al.Add(row);
				}
			}
			return (TableRow[])al.ToArray(typeof(TableRow));
		}
		#endregion

		#region public IEnumerator GetEnumerator() 
		public IEnumerator GetEnumerator() {
			return new TableRowEnumerator(this);
		}
		#endregion

		#region private class TableRowEnumerator:IEnumerator 
		private class TableRowEnumerator:IEnumerator {
			private int _index;
			private TableRowCollection _rows;

			public TableRowEnumerator(TableRowCollection rows){
				_rows = rows;
				_index = -1;
			}

			public void Reset() {
				_index = -1;
			}

			public object Current {
				get {
					return _rows._rows[_index];
				}
			}

			public bool MoveNext() {
				_index++;
				return (_index < _rows._rows.GetLength(0));
			}
		}
		#endregion
	}
}
