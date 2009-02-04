/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Gordago.Strategy {
	internal class TextBoxObjGroup: Control {

		private TextBoxObject[] _tbos;
		private int _minheight, _minwidth;
		private Color _gridColor;
		private Pen _gridPen;

		#region public TextBoxObjGroup() - конструктор
		public TextBoxObjGroup() {
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			_tbos = new TextBoxObject[]{};
			this.GridColor = Color.LightGray;
		}
		#endregion

		#region public Color GridColor
		public Color GridColor{
			get{return this._gridColor;}
			set{
				this._gridColor = value;
				this._gridPen = new Pen(value);
			}
		}
		#endregion

		#region public TextBoxObject[] Rows
		public TextBoxObject[] Rows{
			get{return this._tbos;}
		}
		#endregion
		
		#region public int MinWidth
		public int MinWidth{
			get{return _minwidth;}
			set{
				this._minwidth = value;
//				if (this.Width < value)
//					this.Width = value;
//				else
//					this.Width = value;
				this.ReCalculWidth();
				this.Invalidate();
			}
		}
		#endregion

		#region public int MinHeight
		public int MinHeight{
			get{return _minheight;}
			set{
				_minheight = value;
			}
		}
		#endregion

		#region public TextBoxObject CreateNewRow()
		public TextBoxObject CreateNewRow(){
			TextBoxObject tbo = new TextBoxObject();
			this.Add(tbo);
			return tbo;
		}
		#endregion

		#region public void Add(TextBoxObject tbo)
		public void Add(TextBoxObject tbo){
			tbo.ReCalculPosition += new EventHandler(this.TBO_ReCalculPosition);
			tbo.PositionChanged += new EventHandler(this.TBO_PositionChanged);
			tbo.HeighChanged += new EventHandler(this.TBO_HeightChanged);

			ArrayList tbos = new ArrayList(this._tbos);
			tbos.Add(tbo);
			this._tbos = (TextBoxObject[])tbos.ToArray(typeof(TextBoxObject));
			this.Controls.Add(tbo);
			this.ReCalculHeight();
		}
		#endregion

		#region public void RemoveAt(int index)
		public void RemoveAt(int index){
			TextBoxObject tbo = _tbos[index];
			this.Remove(tbo);
		}
		#endregion

		#region public void Remove(TextBoxObject tbo)
		public void Remove(TextBoxObject tbo){
			tbo.ReCalculPosition -= new EventHandler(TBO_ReCalculPosition);
			tbo.PositionChanged -= new EventHandler(this.TBO_PositionChanged);
			this.Controls.Remove(tbo);

			ArrayList tbos = new ArrayList(this._tbos);
			tbos.Remove(tbo);
			_tbos = new TextBoxObject[tbos.Count];
			tbos.CopyTo(_tbos);
			this.ReCalculHeight();
		}
		#endregion

		#region private void ReCalculHeight()
		private void ReCalculHeight(){
			int h = 0;
			foreach(TextBoxObject tbo in Rows){
				tbo.Top = h;
				h += tbo.Height+1;
			}
			this.Height = h;
		}
		#endregion

		#region protected override void OnPaint(PaintEventArgs e)
		protected override void OnPaint(PaintEventArgs e) {
			int h = 0;
			foreach(TextBoxObject tbo in Rows){
				h += tbo.Height+1;
				e.Graphics.DrawLine(this._gridPen, 0, h-1, this.Width, h-1);
			}
		}
		#endregion

		#region protected override void OnMouseDown(MouseEventArgs e)
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown (e);
			int h = 0;
			if (_tbos.Length < 1) return;
			foreach(TextBoxObject tbo in this._tbos){
				h += tbo.Height + 1;
				if (e.Y < h){
					tbo.Position = tbo.Elements.Length;
					tbo.SetCaretPosition();
					tbo.Focus();
					return;
				}
			}
			_tbos[_tbos.Length-1].Focus();
		}
		#endregion

		#region private void TBO_ReCalculPosition(object sender, EventArgs e)
		private void TBO_ReCalculPosition(object sender, EventArgs e){
			this.ReCalculWidth();
		}
		#endregion

		#region private void TBO_HeightChanged(object sender, EventArgs e)
		private void TBO_HeightChanged(object sender, EventArgs e){
			this.ReCalculHeight();
		}
		#endregion

		#region public void ReCalculWidth()
		public void ReCalculWidth(){
			int w = this.GetMaxWidthInRows();
			if (w < this.MinWidth)
				w = MinWidth;
			this.Width = w;
		}
		#endregion

		#region private int GetMaxWidthInRows()
		private int GetMaxWidthInRows(){
			int w = 0;
			foreach(TextBoxObject tbo in Rows){
				w = Math.Max(tbo.Width, w);
			}
			return w;
		}
		#endregion

		#region private void TBO_PositionChanged(object sender, EventArgs e)
		private void TBO_PositionChanged(object sender, EventArgs e){
			TextBoxObject tbo = sender as TextBoxObject;

			ScrollableControl sc = this.Parent as ScrollableControl;
			int pwidth = sc.ClientSize.Width;
			
			int dx = this.Left;
			int cx = tbo.CaretPositionX;
			int abs_x = dx + cx;
			if (abs_x > pwidth){
				sc.AutoScrollPosition = new Point(cx+10, sc.AutoScrollPosition.Y);
			}else if (abs_x < 0){
				sc.AutoScrollPosition = new Point(cx-10, sc.AutoScrollPosition.Y);
			}
		}
		#endregion
	}
}
