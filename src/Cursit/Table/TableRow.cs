/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Cursit.Table {
	public class TableRow {

		private TableColumnCollection _columns;
		private TableCell[] _cells;
		private TableControl _table;
		private TableCellStyle _style;
		private object _additionobject;

		internal TableRow(TableControl table) {
			_table = table;
			_columns = table.Columns;
			_cells = new TableCell[]{};
			this.CheckCells();
		}

		#region public TableCell this[int indexColumn]
		public TableCell this[int indexColumn]{
			get{return this._cells[indexColumn];}
		}
		#endregion

		#region public TableCell this[TableColumn column]
		public TableCell this[TableColumn column]{
			get{
				foreach (TableCell cell in this._cells){
					if (cell.Column == column)
						return cell;
				}
				return null;
			}
		}
		#endregion

		#region public object AdditionObject
		/// <summary>
		/// Дополнительный объект. 
		/// </summary>
		public object AdditionObject{
			get{return this._additionobject;}
			set{this._additionobject = value;}
		}
		#endregion

		#region public TableCellStyle Style
		/// <summary>
		/// Стиль отрисовки строки.
		/// При установке значения, стиль самих ячеек заNULLяеться
		/// </summary>
		public TableCellStyle Style{
			get{return this._style;}
			set{
				bool change = this._style != value;
				this._style = value;
				foreach(TableCell cell in this._cells){
					cell.Style = null;
				}
				if (change)
					this._table.isCellValueChange = true;
			}
		}
		#endregion

		#region internal void CheckCells()
		/// <summary>
		/// Синхронизация последовательности колонок со значениями
		/// Пока рассматриваеться вариант с добавлением колонок
		/// </summary>
		internal void CheckCells(){
			ArrayList al = new ArrayList();
			for (int i=0;i<_columns.Count;i++){
				TableColumn column = _columns[i];
				TableCell cell = this[column];
				if (cell == null){
					switch(column.Type){
						case TableColumnType.Label:
							cell = new TableCellLabel(_table, column);
							break;
					}
				}
				al.Add(cell);
			}
			_cells = (TableCell[])al.ToArray(typeof(TableCell));
		}
		#endregion

	}
}
