/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

using Gordago.Strategy.FIndicator;
using Language;

#endregion
namespace Gordago.Strategy {
	/// <summary>
	/// Основная таблица стратегии
	/// </summary>
	class SEditorTable: UserControl{

		#region private propertyes
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ToolTip _toolTip;
		private TextBoxObjList _tbol;
		private SEditorRow[] _seRows;

		private int _widthNN;
		private int _widthOK;
		private int _widthTF;
		private int _heightCaption;

		private string _captNN;
		private string _captOK;
		private string _captTF;
		private string _captEx;

		private Pen _pen;
		private Pen _penGrid;
		private Brush _brush;
		private Pen _penGray;

		private PanelDoubleBuffer _panelR;
		private PanelDoubleBuffer _panelL;

		private StringFormat _sf;

		private string _variantName;
		private EditorForm _eform;
		#endregion

		#region public SEditorTable()  - Конструктор
		public SEditorTable(EditorForm wf) {
			this._eform = wf;
			this.components = new System.ComponentModel.Container();
			this.SuspendLayout();

			_toolTip = new System.Windows.Forms.ToolTip(this.components);
			_toolTip.AutoPopDelay = 10000;
			_toolTip.InitialDelay = 1000;
			_toolTip.ReshowDelay = 500;

			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			_pen = new Pen(Color.Black);
			_penGrid = new Pen(Color.LightGray);
			_brush = new SolidBrush(Color.Black);
			_penGray = new Pen(Color.Gray);

			_widthNN = 20;
			_widthOK = 30;
			_widthTF = 50;
			_heightCaption = 25;

			int wdth = _widthNN+_widthOK+_widthTF;
			int top = this._heightCaption+1;
			int hght = this.Height - this._heightCaption-2;

			_panelL = new PanelDoubleBuffer();
			_panelL.BackColor = Color.White;
			_panelL.Top = top;
			_panelL.Left = 1;
			_panelL.Height = hght;
			_panelL.Width = wdth;
			_panelL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top;
			_panelL.Paint += new PaintEventHandler(this.PanelL_Paint);
			_panelL.MouseDown += new MouseEventHandler(_panelL_MouseDown);
			this.Controls.Add(_panelL);

			_panelR = new PanelDoubleBuffer();
			_panelR.BackColor = Color.Bisque;
			_panelR.Left = wdth+1;
			_panelR.Width = WidthTBO;
			_panelR.Top = top;
			_panelR.Height = hght;
			_panelR.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			_panelR.AutoScroll = false;
			this.Controls.Add(_panelR);

			_seRows = new SEditorRow[]{};
			_tbol = new TextBoxObjList(this);
			_tbol.RowsLocationChanged += new EventHandler(this.TextBoxObjList_RowsLocationChanged);
			_tbol.Dock = DockStyle.Fill;
			_panelR.Controls.Add(_tbol);

			_captNN = Dictionary.GetString(10,10);
			_captOK = Dictionary.GetString(10,11);
			_captTF = Dictionary.GetString(10,12);
			_captEx = Dictionary.GetString(10,13);
			this.ResumeLayout(false);
			this.ContextMenu = new ContextMenu();
		}
		#endregion

    #region public EditorForm ParentEditorForm
    public EditorForm ParentEditorForm{
			get{return _eform;}
    }
    #endregion

    #region public string VariantName
    public string VariantName{
			get{return this._variantName;}
			set{this._variantName = value;}
		}
		#endregion

		#region private void _panelL_MouseDown(object sender, MouseEventArgs me)
		private void _panelL_MouseDown(object sender, MouseEventArgs me){
			int index = this._tbol.GetRow(me.Y);
			if (index > -1  && index < this._tbol.Rows.Length){
				_tbol.Rows[index].Focus();
			}
		}
		#endregion

		#region protected override void OnControlAdded(ControlEventArgs e)
		protected override void OnControlAdded(ControlEventArgs e) {
			base.OnControlAdded (e);
			e.Control.GotFocus += new EventHandler(this.Control_GotFocus);
		}
		#endregion

		#region protected override void OnControlRemoved(ControlEventArgs e)
		protected override void OnControlRemoved(ControlEventArgs e) {
			base.OnControlRemoved (e);
			e.Control.GotFocus -= new EventHandler(this.Control_GotFocus);
		}
		#endregion

		#region private void Control_GotFocus(object sender, EventArgs e)
		private void Control_GotFocus(object sender, EventArgs e){	
			if (this.ActiveControl is TextBoxObject){
				TextBoxObject tbo = this.ActiveControl as TextBoxObject;
				int index = this._tbol.GetSelectedRowIndex(tbo);
				if (index < 0) return;
				ComboBox cmbtf = this.SERows[index].CmbTF;
				ShowCmbTF(cmbtf);
			}else if (this.ActiveControl is ComboBox){
				ShowCmbTF(this.ActiveControl as ComboBox);
			}else
				this.HideAllCmbTF();
		}
		#endregion

		#region protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
		protected override void OnValidating(System.ComponentModel.CancelEventArgs e) {
			base.OnValidating (e);
			this.HideAllCmbTF();
		}
		#endregion

		#region private void HideAllCmbTF()
		private void HideAllCmbTF(){
			foreach (SEditorRow serow in this.SERows){
				serow.CmbTF.Visible = false;
				serow.IsSelect = false;
			}
			this._panelL.Invalidate();
		}
		#endregion

		#region private void ShowCmbTF(ComboBox cmbtf)
		private void ShowCmbTF(ComboBox cmbtf){
			foreach (SEditorRow serow in this.SERows){
				if (cmbtf == serow.CmbTF){
					serow.CmbTF.Visible = true;
					serow.IsSelect = true;
				}	else{
					serow.CmbTF.Visible = false;
					serow.IsSelect = false;
				}
			}
			this._panelL.Invalidate();
		}
		#endregion

		#region public SEditorRow[] SERows
		public SEditorRow[] SERows{
			get{return this._seRows;}
		}
		#endregion

		#region private int WidthTBO
		private int WidthTBO{
			get{return this.Width-(_widthNN+_widthOK+_widthTF)-2;}
		}
		#endregion

		#region public ToolTip ToolTip
		public ToolTip ToolTip{
			get{return _toolTip;}
		}
		#endregion

		#region private void PanelL_Paint(object sender, PaintEventArgs e)
		private void PanelL_Paint(object sender, PaintEventArgs e){
			Graphics g = e.Graphics;
			g.DrawLine(this._penGray, this._widthNN,0, this._widthNN, _panelL.Height);
			g.DrawLine(this._penGrid, this._widthNN+_widthOK,0, this._widthNN+_widthOK, _panelL.Height);
			g.DrawLine(this._penGrid, this._panelL.Width-1,0, this._panelL.Width-1, _panelL.Height);
			foreach(SEditorRow ser in this._seRows){
				ser.Paint(g, this._tbol.RowsTop);
			}
		}
		#endregion
		
		#region protected override void OnPaint(PaintEventArgs e)
		protected override void OnPaint(PaintEventArgs e) {
			Graphics g = e.Graphics;
			if (_sf == null){
				_sf = new StringFormat();
				_sf.Alignment = StringAlignment.Center;
				_sf.LineAlignment = StringAlignment.Center;
			}

			int x1 = _widthNN+1;
			int x2 = x1+_widthOK;
			int x3 = x2+_widthTF-1;
			int y1 = this._heightCaption;
			using (Pen penwhite = new Pen(Color.WhiteSmoke)){
				g.DrawLine(penwhite, x1+1, 0, x1+1, this._heightCaption);
				g.DrawLine(penwhite, x2+1, 0, x2+1, this._heightCaption);
				g.DrawLine(penwhite, x3+1, 0, x3+1, this._heightCaption);
			}

			using (Pen pencap = new Pen(Color.Gray)){
				g.DrawLine(pencap, x1, 0, x1, this._heightCaption);
				g.DrawLine(pencap, x2, 0, x2, this._heightCaption);
				g.DrawLine(pencap, x3, 0, x3, this._heightCaption);
				g.DrawLine(pencap, 0, this._heightCaption, this.Width, this._heightCaption);
			}

			g.DrawString(this._captNN, this.Font, this._brush, new Rectangle(1,1, this._widthNN+1, y1), _sf);
			g.DrawString(this._captOK, this.Font, this._brush, new Rectangle(x1,1, this._widthOK+1, y1), _sf);
			g.DrawString(this._captTF, this.Font, this._brush, new Rectangle(x2,1, this._widthTF+1, y1), _sf);
			g.DrawString(this._captEx.Trim(), this.Font, this._brush, new Rectangle(x3,1, this._panelR.Width, y1), _sf);
			g.DrawRectangle(_pen, 0,0, this.Width-1, this.Height-1);
		}
		#endregion
		
		#region private void TextBoxObjList_RowsLocationChanged(object sender, EventArgs e)
		private void TextBoxObjList_RowsLocationChanged(object sender, EventArgs e){
			this.ChangeRowPosition();
		}
		#endregion
		
		#region private void ChangeRowPosition()
		/// <summary>
		/// Корректировка позиции строк
		/// </summary>
		private void ChangeRowPosition(){
			this.Refresh();
		}
		#endregion
		
		#region public void CreateNewRow()
		public SEditorRow CreateNewRow(){
			TextBoxObject tbo =  _tbol.CreateNewRow();
			tbo.GotFocus += new EventHandler(this.Control_GotFocus);

			ArrayList rows = new ArrayList(this._seRows);
			SEditorRow seRow = new SEditorRow(this._eform, this._panelL, this._seRows.Length, tbo, _widthNN, _widthOK, _widthTF, _penGrid);
			rows.Add(seRow);
			_seRows = new SEditorRow[rows.Count];
			rows.CopyTo(_seRows);
			this.Invalidate();
			return seRow;
		}
		#endregion

		#region private void DeleteRow(int index)
		private void DeleteRow(int index){
			if (this.SERows.Length  < 1) return;
			ArrayList rows = new ArrayList(this._seRows);
			SEditorRow seRow = SERows[index];
			this._tbol.Remove(seRow.TextBoxObject);
			seRow.RemoveThisComponent();
			rows.RemoveAt(index);
			_seRows = new SEditorRow[rows.Count];
			rows.CopyTo(_seRows);
		}
		#endregion

    #region public IndicFunctionBox AddIndicFunction(TextBoxObject tbo, IndicFunction indf)
    public IndicFunctionBox AddIndicFunction(TextBoxObject tbo, IndicFunction indf){
			IndicFunction indins = null;
			IndicatorGUI indicNew = indf.Parent.Clone();
			indins = indicNew.GetIndicFunction(indf.Name);
			
			if (indf.Parent.WhoIs != IndicatorGUI.WhoIsWho.Editor){
				indicNew.GroupId = ParentEditorForm.GroupId++;
			}else{
				indins.Parent.GroupId = indf.Parent.GroupId;
			}

			indicNew.WhoIs = IndicatorGUI.WhoIsWho.Editor;
			ParentEditorForm.GroupId = Math.Max(ParentEditorForm.GroupId, indins.Parent.GroupId+1);
			
			IndicFunctionBox indbox = new IndicFunctionBox(indins, _toolTip);
			tbo.Insert(tbo.Position, indbox);
			tbo.Position++;
			tbo.Invalidate();
			indbox.Invalidate();
			return indbox;
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
			base.OnResize (e);
			this.SetRowCountFromSize();
		}
		#endregion

		#region private void SetRowCountFromSize()
		private void SetRowCountFromSize(){
			int hght = this.Height - _heightCaption;
			int hghtRows = this._tbol.RowsHeight+2;
			int hRow = TextBoxObject.HEIGHT_ROW;
			if (hghtRows + hRow < hght){
				this.CreateNewRow();
				this.SetRowCountFromSize();
			}else if (hghtRows > hght){
				if (DeleteEndRow()){
					this.SetRowCountFromSize();
				}
			}
		}
		#endregion

		#region private bool DeleteEndRow()
		private bool DeleteEndRow(){
			if (this.SERows.Length < 2) return false;
			if (SERows[this.SERows.Length-2].TextBoxObject.Elements.Length != 0)
				return false;

			SEditorRow seRow = this.SERows[this.SERows.Length-1];
			if (seRow.TextBoxObject.Elements.Length != 0)
				return false;

			this.DeleteRow(this.SERows.Length-1);
			return true;
		}
		#endregion

		#region internal void CheckEndRow()
		internal void CheckEndRow(){
			if (this.SERows.Length < 2) return;
			
			if (SERows[this.SERows.Length-1].TextBoxObject.Elements.Length == 0) {
				this.SetRowCountFromSize();
			}else{
				this.CreateNewRow();
				this._tbol.Height++;
				this._tbol.Height--;
			}
			this.Refresh();
		}
		#endregion

    #region public IndicFunction[] GetAllIndicFunction()
    /// <summary>
		/// Формирование всех функций на стратегии
		/// </summary>
		public IndicFunction[] GetAllIndicFunction(){
			ArrayList al = new ArrayList();
			foreach (SEditorRow row in this._seRows){
				foreach(TextBoxObjElement tbel in row.TextBoxObject.Elements){
					if (tbel is TextBoxObjElementCtrl){
						al.Add(((tbel as TextBoxObjElementCtrl).Element as IndicFunctionBox).IndicFunction);
					}
				}
			}
			return (IndicFunction[])al.ToArray(typeof(IndicFunction));
    }
    #endregion
  }
	#region internal class PanelDoubleBuffer: Panel
	internal class PanelDoubleBuffer: Panel{
		public PanelDoubleBuffer():base(){
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}
	}
	#endregion

}
