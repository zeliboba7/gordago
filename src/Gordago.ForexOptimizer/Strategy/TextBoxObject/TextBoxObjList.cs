/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Gordago.Strategy.FIndicator;

namespace Gordago.Strategy {
	/// <summary>
	/// Таблица - лист редакторов TextBoxObject
	/// </summary>
	class TextBoxObjList : System.Windows.Forms.ScrollableControl {
		public event EventHandler RowsLocationChanged;

    private SEditorTable _table;

		#region private propertyes
		private System.ComponentModel.Container components = null;
		private TextBoxObjGroup _tbog;
		#endregion

    public TextBoxObjList(SEditorTable table) {
      _table = table;
			this.Cursor = Cursors.IBeam;
			this.AutoScroll = true;
			this.BackColor = Color.White;
			_tbog = new TextBoxObjGroup();
			this.SuspendLayout();
			_tbog.MinWidth = this.ClientSize.Width;
			this.Controls.Add(_tbog);
			this.ResumeLayout();
      this.AllowDrop = true;
			_tbog.LocationChanged += new EventHandler(this.TextBoxObjGroup_LocationChanged);
    }

    #region public TextBoxObject[] Rows
    public TextBoxObject[] Rows{
			get{return _tbog.Rows;}
		}
		#endregion

		#region public int RowsWidth
		public int RowsWidth{
			get{return this._tbog.Width;}
		}
		#endregion

		#region public int RowsHeight
		public int RowsHeight{
			get{return this._tbog.Height;}
		}
		#endregion

		#region public int RowsTop - Позиция колонок 
		/// <summary>
		/// Позиция колонок 
		/// </summary>
		public int RowsTop{
			get{return _tbog.Top;}
		}
		#endregion

		#region public int RowsLeft
		public int RowsLeft{
			get{return _tbog.Left;}
		}
		#endregion

		#region public TextBoxObject CreateNewRow()
		public TextBoxObject CreateNewRow(){
			return this._tbog.CreateNewRow();
		}
		#endregion

		#region public void RemoveAt(int index)
		public void RemoveAt(int index){
			this._tbog.RemoveAt(index);
		}
		#endregion

		#region public void Remove(TextBoxObject tbo)
		public void Remove(TextBoxObject tbo){
			this._tbog.Remove(tbo);
		}
		#endregion

		#region protected override void OnResize(EventArgs e)
		protected override void OnResize(EventArgs e) {
			base.OnResize (e);
			_tbog.MinWidth = this.ClientSize.Width;
			_tbog.Invalidate();
		}
		#endregion

		#region protected override void Dispose( bool disposing )
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}
		#endregion

		#region private void TextBoxObjGroup_LocationChanged(object sender, EventArgs e)
		private void TextBoxObjGroup_LocationChanged(object sender, EventArgs e){
			if (this.RowsLocationChanged != null)
				this.RowsLocationChanged(this, new EventArgs());
		}
		#endregion

		#region public int GetRow(int y) - Определяет номер строки относительно позиции по вертикали
		/// <summary>
		/// Определяет номер строки относительно позиции по вертикали
		/// </summary>
		/// <param name="y">позиция по вертикали</param>
		/// <returns>Возвращает номер строки. Если строки отсутствуют то -1
		/// </returns>
		public int GetRow(int y){
			if (this.Rows.Length == 0) return -1;

			int i=0;
			foreach( TextBoxObject tbo in this.Rows){
				int y1 = tbo.Top + this._tbog.Top;
				int y2 = tbo.Top + tbo.Height + _tbog.Top;
				if (y >= y1 && y <= y2)
					return i;
				i++;
			}
			return this.Rows.Length - 1;
		}
		#endregion

		#region public void SetPostionInRow(int numrow, int x)
		public void SetPostionInRow(int numrow, int x){
			TextBoxObject row = this.Rows[numrow];
			row.Focus();
			row.SetCaretPosition(x - this._tbog.Left);
		}
		#endregion

		#region public int GetSelectedRowIndex(TextBoxObject tbof) - Получает номер в массиве 
		/// <summary>
		/// Получает номер в массиве 
		/// </summary>
		public int GetSelectedRowIndex(TextBoxObject tbof){
			int i=0;
			foreach (TextBoxObject tbo in this.Rows){
				if(tbo == tbof)
					return i;
				i++;
			}
			return -1;
		}
		#endregion

    #region protected override void OnDragOver(DragEventArgs drgevent)
    protected override void OnDragOver(DragEventArgs drgevent) {
      if(drgevent.Data.GetDataPresent(typeof(IndicFunction).ToString())) {
        drgevent.Effect = DragDropEffects.Move;
      } else
        return;
      Point cp = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      int numrow = this.GetRow(cp.Y);
      if(numrow < 0) return;
      this.SetPostionInRow(numrow, cp.X);
    }
    #endregion

    protected override void OnDragDrop(DragEventArgs drgevent) {
      if(!drgevent.Data.GetDataPresent(typeof(IndicFunction).ToString()))
        return;

      Point cp = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      int numrow = this.GetRow(cp.Y);
      TextBoxObject tbo = null;
      if(numrow < 0)
        tbo = _table.CreateNewRow().TextBoxObject;
      else
        tbo = this.Rows[numrow];

      IndicFunction indf = (IndicFunction)drgevent.Data.GetData(typeof(IndicFunction).ToString());
      IndicFunctionBox indfbox = _table.AddIndicFunction(tbo, indf);

      IIndicatorBoxCollection iIBox = GordagoMain.MainForm.ActiveMdiChild as IIndicatorBoxCollection;
      if (iIBox != null)
        iIBox.SelectIndicFunctionBox(null);

      ExplorerPanel iibox = GordagoMain.MainForm.Explorer;
      iibox.UnSelectAllRowInViewIndicators();
      iibox.ClearIndicatorProperty();

    }
	}
}
