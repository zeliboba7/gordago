/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Windows.Forms;

using Cursit.Applic.AConfig;
using Gordago.Strategy.FIndicator;
using Gordago.Stock;

namespace Gordago.Strategy {
	/// <summary>
	/// Хранитель записи(левая часть из нескольких колонок) в редакторе стратегии
	/// </summary>
	class SEditorRow {

		private int _numrow;
		private SEditorRowCompile _ssr;
		private ComboBox _cmbTF;
		private TextBoxObject _tbo;
		private Control _parent;
		private int _widthNN, _widthOK, _widthTF;
		private Pen _pen;
		private StringFormat _sf;
		private Brush _brush;
		private Pen _penGray;
		private Pen _penWhiteSmoke;

		private bool _isselect;

		public static Image ImageOk;
		public static Image ImageNo;
		public static Image ImageMb;

		EditorForm _eform;

		public SEditorRow(EditorForm eform, Control parent, int n, TextBoxObject tbo, int widthNN, int widthOK, int widthTF, Pen pen) {
			_isselect = false;
			_eform = eform;
			_brush = new SolidBrush(Color.Black);
			_pen = pen;
			_tbo = tbo;
			_penGray = new Pen(Color.Gray);
			_penWhiteSmoke = new Pen(Color.WhiteSmoke);

			_widthNN = widthNN;
			_widthOK = widthOK;
			_widthTF = widthTF;

			tbo.ElementsChanged += new EventHandler(this.TextBoxObject_ElementsChanged);

			_parent = parent;
			this._numrow = n;

			ImageList imgList = new ImageList();
			imgList.ImageSize = new Size(22,22);
			if (SEditorRow.ImageOk == null){
				SEditorRow.ImageOk = GordagoImages.Images.GetImage("ok", "strg");
				SEditorRow.ImageNo = GordagoImages.Images.GetImage("no", "strg");
			}

			_ssr = new SEditorRowCompile(this);
			_ssr.Click += new EventHandler(this._ssr_Click);

			_cmbTF = new ComboBox();
			_cmbTF.Visible = false;
			_cmbTF.DropDownStyle = ComboBoxStyle.DropDownList;
      _cmbTF.MaxDropDownItems = TimeFrameManager.TimeFrames.Count;
			_cmbTF.Width = this._widthTF-3;
			_cmbTF.SelectedIndexChanged += new EventHandler(_cmbTF_SelectedIndexChanged);
      for(int i = 0; i < TimeFrameManager.TimeFrames.Count; i++) {
        _cmbTF.Items.Add(TimeFrameManager.TimeFrames[i]);
			}
			
			parent.Controls.Add(_ssr);
			parent.Controls.Add(_cmbTF);
		}

		#region public void RemoveThisComponent()
		public void RemoveThisComponent(){
			this._parent.Controls.Remove(_ssr);
			this._parent.Controls.Remove(_cmbTF);
		}
		#endregion

		#region public int TimeFrameId
		public int TimeFrameId{
			set{
				if (value == 0)
					this._cmbTF.SelectedItem = null;
				else
          this._cmbTF.SelectedItem = TimeFrameManager.TimeFrames.GetTimeFrame(value);
			}
			get{
				TimeFrame tfval = this._cmbTF.SelectedItem as TimeFrame;
				if (tfval != null)
					return tfval.Second;
				return 0;
			}
		}
		#endregion

		#region public bool IsSelect
		public bool IsSelect{
			get{return this._isselect;}
			set{_isselect = value;}
		}
		#endregion

		#region public ComboBox CmbTF
		public ComboBox CmbTF{
			get{return this._cmbTF;}
		}
		#endregion

		#region public SEditorRowCompileStatus Status
		public SEditorRowCompileStatus Status{
			get{return this._ssr.Status;}
			set{
				this._ssr.Status = value;
			}
		}
		#endregion

		#region public TextBoxObject TextBoxObject
		public TextBoxObject TextBoxObject{
			get{return this._tbo;}
		}
		#endregion

		#region public void Paint(Graphics g)
		public void Paint(Graphics g, int deltaTop){
			if (_sf == null){
				_sf = new StringFormat();
				_sf.Alignment = StringAlignment.Center;
				_sf.LineAlignment = StringAlignment.Center;
			}
			int top = _tbo.Top-1+deltaTop;
			int hght = _tbo.Height;
			int ly = top+hght+1;
			
			Rectangle rect = new Rectangle(0, top+1, this._widthNN, hght);
			
			g.FillRectangle(new SolidBrush(SystemColors.Control), rect);
				
			g.DrawLine(_pen, 0, ly, this._parent.Width, ly);

			g.DrawLine(_penGray, 0, ly, _widthNN, ly);
			g.DrawLine(_penWhiteSmoke, 0, top+1, _widthNN-1, top+1);

			Font fnt = this._parent.Font;
			if (this._isselect){
				fnt = new Font(fnt.FontFamily, fnt.Size, FontStyle.Bold);
				Pen penb = new Pen(Color.Black);

				int xx = this._widthNN-1;
				g.DrawLine(penb, 0, ly-1, xx, ly-1);
				g.DrawLine(penb, xx+1, top+1, xx+1, ly-1);
			}

			g.DrawString((this._numrow+1).ToString(), fnt, _brush, rect, _sf);

			int xOK = _widthNN+1;
			int yOK = top+2;

			this._ssr.Left = xOK;
			this._ssr.Top = yOK;
			this._ssr.Width = this._widthOK-1;
			this._ssr.Height = hght-1;

			int xTF = this._widthNN+_widthOK+2;
			int yTF = top+2;
			this._cmbTF.Location = new Point(xTF, yTF);

			if (this._cmbTF.SelectedIndex > -1){
				g.DrawString(((TimeFrame)_cmbTF.SelectedItem).Name, this._parent.Font, _brush, xTF+2, yTF+4);
			}
		}
		#endregion

		#region private void IndicFunction_SelectChanged(object sender, EventArgs e)
		/// <summary>
		/// Событие на изменения в индикаторе функции.
		/// Просматриваем на всем листе (по вариантам тоже) индикаторы, 
		/// и убираем выделения, если таковы присутствуют
		/// </summary>
		private void IndicFunction_SelectChanged(object sender, EventArgs e){
			IndicFunction ifc = sender as IndicFunction;
			// елси это отмена выделения, то просто выход, иначе выделяем эту группу, а 
			// все остальные группы антивыделяем :)
//			if (!ifc.IsSelect) {
//				if (this.IndicFunctionSelectChanged != null)
//					this.IndicFunctionSelectChanged(sender, e);
//				return;
//			}
//			
//			TextBoxObjGroup tbog =  _tbol.Items;
//			int cnt = tbog.Count;
//			for (int i=0;i<cnt;i++){
//				TextBoxObject tbo = tbog[i];
//				int cntt = tbo.Elements.Count;
//				for (int ii =0;ii<cntt;ii++){
//					TextBoxElement tbe = tbo.Elements[ii];
//					if (tbe.ElementType == TextBoxElType.Control){
//						IndicFunctionBox ifb = ((TextBoxElControl)tbe).Value as IndicFunctionBox;
//						if (ifc != ifb.IndicFunction && ifb.IndicFunction.IsSelect){
//							ifb.IndicFunction.IsSelect = false;
//						}
//					}
//				}
//			}
//			if (this.IndicFunctionSelectChanged != null)
//				this.IndicFunctionSelectChanged(sender, e);
		}
		#endregion

		#region private void _ssr_Click(object sender, EventArgs e)
		private void _ssr_Click(object sender, EventArgs e){
			switch (_ssr.Status){
				case SEditorRowCompileStatus.Compile:
					_ssr.Status = SEditorRowCompileStatus.Stop;
					break;
				case SEditorRowCompileStatus.Stop:
					this.Status = SEditorRowCompileStatus.Empty;
					this.SetCompile();
					break;
				default:
					return;
			}
			this._ssr.Refresh();
		}
		#endregion

		#region private void _cmbTF_SelectedIndexChanged (object sender, EventArgs e)
		private void _cmbTF_SelectedIndexChanged (object sender, EventArgs e){
			this.SetCompile();
			_eform.IsChangeUser = true;
		}
		#endregion

		#region private void TextBoxObject_ElementsChanged(object sender, EventArgs e) Событие на добавление, удаление элемента из редактора
		/// <summary>
		/// Событие на добавление, удаление элемента из редактора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextBoxObject_ElementsChanged(object sender, EventArgs e){
			this.SetCompile();

			_eform.IsChangeUser = true;
			SEditorTable seTable = _parent.Parent as SEditorTable;
			seTable.CheckEndRow();
		}
		#endregion

		#region private void SetCompile()
		internal void SetCompile(){
			if (this.Status == SEditorRowCompileStatus.Stop){ 
				_ssr.Refresh();
				return;
			}
			if (!CheckCompile()){
				_ssr.Status = SEditorRowCompileStatus.Empty;
			}else{
				_ssr.Status = SEditorRowCompileStatus.Compile;
			}
			_ssr.Refresh();
		}
		#endregion

		#region private bool CheckCompile()
		/// <summary>
		/// Проверка строки на возможность компиляции
		/// </summary>
		private bool CheckCompile(){
			if (this._cmbTF.SelectedIndex < 0) return false;
			if (this._tbo.Elements.Length < 1) return false;
			return true;
		}
		#endregion
	}
}
