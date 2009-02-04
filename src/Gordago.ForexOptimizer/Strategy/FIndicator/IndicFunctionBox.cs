/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Gordago.Strategy.FIndicator;
using Gordago.Strategy.FIndicator.FIndicParam;


namespace Gordago.Strategy.FIndicator {
	/// <summary>
	/// Контрол - функция индиктора.
	/// Используется в редакторе стратегии
	/// </summary>
	class IndicFunctionBox : Control {

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ToolTip _toolTip;

		private IndicFunction _indicF;
		private Pen penb;
		private Brush brush;
		private bool _select;
		private StringFormat _sf;
		private string _displstring="";
		private string _tooltipstring = "";
		private Font _dsfont;
		private Image _icon = null;
		private bool _isfocused;

		public IndicFunctionBox(IndicFunction indf, ToolTip tooltip){

			this.Location = new Point(0,1);
			if (indf.Parent.Indicator.Image != null){
				Image img = indf.Parent.Indicator.Image;
				if (img.Width > 3 && img.Height > 3)
					_icon = img;
			}
			_toolTip = tooltip;

			this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);

			this._indicF = indf;
			_indicF.Parent.ParameterChanged += new EventHandler(this.IndicFuncParam_Changed);

			this.Size = new Size(100, 20);
			penb = new Pen(Color.Black);
			brush = new SolidBrush(Color.Black);
			this.BackColor = Color.White;
			this.Cursor = Cursors.Hand;
		}

		#region public bool IsSelect
		public bool IsSelect{
			get{return _select;}
			set{
				_select = value;
        this.Invalidate();
			}
		}
		#endregion

		#region public bool IsViewProperty
	  public bool IsViewProperty{
			get{return this._isfocused;}
			set{this._isfocused = value;}
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

		#region public IndicFunction IndicFunction
		public IndicFunction IndicFunction{
			get{return _indicF;}
		}
		#endregion

		#region protected override void OnPaint(PaintEventArgs pe) 
		protected override void OnPaint(PaintEventArgs pe) {
			Graphics g = pe.Graphics;

			if (_dsfont == null){
				_dsfont = new Font(this.Parent.Font.Name,7);
			}
			int imgwidth = 0;
			if (_icon != null){
				imgwidth = _icon.Width + 3;
			}

			string[] stra = this._indicF.GetIFBoxString();
			string str = stra[0];
			if (str != this._displstring){
				this._displstring = str;
				this._tooltipstring = stra[1];
				float swidth = Cursit.Utils.TextChartInfo.GetStringLenght(str, _dsfont).Width;
				this.Width = (int)swidth + 14+imgwidth;
			}

			int X = this.Width-1; 
			int Y = this.Height-1;  

			LinearGradientBrush silverBrush07;
			if (!this.IsSelect){
				silverBrush07 = new LinearGradientBrush(new Rectangle(2, 2, X-2, Y-2), 
					Color.FromArgb(253, 253, 253), 
					Color.FromArgb(201, 200, 220), 90.0f);
			}else{
				silverBrush07 = new LinearGradientBrush(new Rectangle(2, 2, X-2, Y-2), 
					Color.FromArgb(223, 223, 253), 
					Color.FromArgb(161, 160, 220), 90.0f);
			}
			float[] relativeIntensities = {0.0f, 0.008f, 1.0f}; 
			float[] relativePositions = {0.0f, 0.32f, 1.0f};

			Blend blend = new Blend();
			blend.Factors = relativeIntensities;
			blend.Positions = relativePositions; 
			silverBrush07.Blend = blend; 

			g.FillRectangle(silverBrush07, 2, 2, this.Width-2, this.Height-2);  
			silverBrush07.Dispose();
			
			Point[] points = 
				new Point[]{new Point(1, 0), new Point(X-1, 0), new Point(X-1, 1), new Point(X, 1), 
										 new Point(X, Y-1), new Point(X-1, Y-1), new Point(X-1, Y), new Point(1, Y), 
										 new Point(1, Y-1), new Point(0, Y-1), new Point(0, 1), new Point(1, 1)};

			g.DrawLines(penb, points);

			if (this._isfocused){
				X = this.Width-3; 
				Y = this.Height-3;  
				Point[] points1 = 
					new Point[]{new Point(3, 2), new Point(X-1, 2), new Point(X-1, 3), new Point(X, 3), 
											 new Point(X, Y-1), new Point(X-1, Y-1), new Point(X-1, Y), new Point(3, Y), 
											 new Point(3, Y-1), new Point(2, Y-1), new Point(2, 3), new Point(3, 3)};

				Pen pn = new Pen(Color.DimGray, 1);
				pn.DashStyle = DashStyle.Dash;

				g.DrawLines(pn, points1);
			}

			if (_sf == null){
				_sf = new StringFormat();
				_sf.Alignment = StringAlignment.Center;
				_sf.LineAlignment = StringAlignment.Center;
			}
			g.DrawString(str, this._dsfont, brush, new Rectangle(imgwidth,0, this.Width-imgwidth, this.Height), _sf);

			if (this._icon != null){
				int iy = this.Height/2 - _icon.Height/2;
				g.DrawImage(_icon, 5, iy, _icon.Width, _icon.Height);
			}
		}
		#endregion

		#region private void IndicFuncParam_Changed(object sender, System.EventArgs e)
		private void IndicFuncParam_Changed(object sender, System.EventArgs e){
			this.Invalidate();
			IndicFuncParam ifp = sender as IndicFuncParam;

      Form form = GordagoMain.MainForm.ActiveMdiChild;
      if (form is EditorForm) {
        EditorForm editForm = form as EditorForm;
        if (ifp.Parameter.Name != "__Shift")
          editForm.SynchronizedParam(this);
        editForm.IsChangeUser = true;
      }
		}
		#endregion

		#region internal void SyncronizedParam(IndicFunctionBox ifbox)
		/// <summary>
		/// Синхронизация параметров для групп
		/// синхронезирует все кроме пар-ра "Свячей назад"
		/// </summary>
		internal void SyncronizedParam(IndicFunctionBox ifbox){
			IndicFuncParams prmsafrom = ifbox.IndicFunction.Parent.Params;
			IndicFuncParams prmsato = this.IndicFunction.Parent.Params;
			for(int i=0;i<prmsafrom.Params.Length;i++){
				IndicFuncParam ifpfrom = prmsafrom.Params[i];
				IndicFuncParam ifpto = prmsato.Params[i];
				if (ifpfrom.Name != "__Shift"){
					ifpfrom.CopyTo(ifpto);
				}
			}
			this.Invalidate();
		}
		#endregion

		#region protected override void OnMouseEnter(EventArgs e)
		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter (e);
      if(_toolTip == null) return;
			_toolTip.SetToolTip(this, this._tooltipstring);
		}
		#endregion

		#region protected override void OnMouseLeave(EventArgs e)
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave (e);
      if(_toolTip == null) return;
			_toolTip.RemoveAll();
		}
		#endregion

		#region protected override void OnClick(EventArgs e)
		protected override void OnClick(EventArgs e) {
			base.OnClick (e);
      GordagoMain.MainForm.Explorer.ViewIndicatorProperty(this.IndicFunction.Parent, null, null);
      IIndicatorBoxCollection iibox = GordagoMain.MainForm.ActiveMdiChild as IIndicatorBoxCollection;
      if (iibox == null) 
        return;
      iibox.SelectIndicFunctionBox(this);
		}
		#endregion
	}
}
