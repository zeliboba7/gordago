/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
#endregion
namespace Cursit.Table {
	public abstract class TableCell {
		
		private TableControl _table;
		private TableColumn _column;
		private TableCellStyle _style;
		private string _text;
		private int _width;
		private int _height;

		#region internal TableCell(TableControl table, TableColumn column)
		internal TableCell(TableControl table, TableColumn column) {
			_table = table;
			_column = column;
			_style = null;
		}
		#endregion

		#region internal TableColumn Column
		/// <summary>
		/// Колонка к которой пренадлежит данная ячейка
		/// </summary>
		internal TableColumn Column{
			get{return this._column;}
		}
		#endregion

		#region public TableCellStyle Style
		public TableCellStyle Style{
			get{return this._style;}
			set{this._style = value;}
		}
		#endregion

		#region public string Text
		public string Text{
			get{return this._text;}
			set{
				bool change = this._text != value;
				this._text = value;
				if (change)
					this._table.isCellValueChange = true;
			}
		}
		#endregion

		#region public int Width
		public int Width{
			get{return this._width;}
		}
		#endregion

		#region public int Height
		public int Height{
			get{return this._height;}
		}
		#endregion

		public abstract void Paint(Graphics g, TableCellStyle style,  int x, int y, int width,  int height);

		#region internal void SetSize(int width, int height)
		internal void SetSize(int width, int height){
			this._width = width;
			this._height = height;
		}
		#endregion
	}
}
