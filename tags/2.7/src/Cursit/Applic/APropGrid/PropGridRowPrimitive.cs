using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid {

		public delegate void PropGridRowEventHandler(object sender, System.EventArgs e);

		public class PropGridRowPrimitive : System.Windows.Forms.Control{

				/// <summary>
				/// Событие на изменение положение сплиттера.
				/// </summary>
				public event PropGridRowEventHandler SplitterChanged;

				private System.ComponentModel.Container components = null;
				private Brush _foreBrush;
				private Brush _foreBrushSelect;
				private Brush _selectBrush;
				private Pen _penSplitter;
				private EventArgs _eventArgs;
				internal int _id;
				
				/// <summary>
				/// Разделитель между описанием свойства, и редактором этого свойства
				/// </summary>
				internal int _splitter;
				/// <summary>
				/// Ограничения сплитера справа
				/// </summary>
				private int _lfSplit;
				/// <summary>
				/// ограничения сплитера слева
				/// </summary>
				private int _rgSplit;

				/// <summary>
				/// True - курсор находится в состяния изменения Splitter-а, False
				/// </summary>
				private bool _selectSplitter;

				private bool _isSelect = false;
				private PropGridViewerPrimitive _propGridVP;

				private void PropGridValueClick(object sender, System.EventArgs e){
						this.OnClick(new System.EventArgs());
				}

			
				public PropGridRowPrimitive(PropGridViewerPrimitive propGridVP){
						this._propGridVP = propGridVP;
						
						_eventArgs = new EventArgs();
						this._propGridVP.Click += new EventHandler(this.PropGridValueClick);
						this._lfSplit = 10;

						this.Size = new System.Drawing.Size(300, 18);
						this.BackColor = Color.White;
						this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
								ControlStyles.UserPaint, true);

						_propGridVP.Height = this.Height-2;
						this.Controls.Add(_propGridVP);
				}

				protected override void Dispose( bool disposing ) {
						if( disposing ) {
								if (this._foreBrush != null)
										this._foreBrush.Dispose();
								if (this._foreBrushSelect != null)
										this._foreBrushSelect.Dispose();
								if (this._selectBrush != null)
										this._selectBrush.Dispose();
								if (this._penSplitter != null)
										this._penSplitter.Dispose();

								if( components != null )
										components.Dispose();
						}
						base.Dispose( disposing );
				}

				public PropGridValue PropGridValue{
						get{return this._propGridVP.PropGridValue;}
				}

				/// <summary>
				/// Разделитель между описанием свойства, и редактором этого свойства
				/// </summary>
				public virtual int Splitter{
						get{return this._splitter;}
						set{
								if (value == this._splitter) return;
								if (value < _lfSplit ) value = _lfSplit;
								if (value > _rgSplit) value = _rgSplit;

								this._splitter = value;
								this.RecalculPropGridRowValue();
						}
				}

				protected override void CreateHandle() {
						base.CreateHandle ();
						this._foreBrush = new SolidBrush(base.ForeColor);
						this._foreBrushSelect = new SolidBrush(Color.White);
						this._selectBrush = new SolidBrush(Color.BlueViolet);
						this._penSplitter = new Pen(Color.LightGray);
				}

				[Description("Цвет шрифта")]
				public override Color ForeColor {
						get {
								return base.ForeColor;
						}
						set {
								if (base.ForeColor == value) return;
								if (this.IsHandleCreated)
										this._foreBrush.Dispose();
								base.ForeColor = value;
								if (this.IsHandleCreated)
										this._foreBrush = new SolidBrush(value);
								this.Invalidate();
						}
				}

				[DefaultValue(false)]
				public bool IsSelect{
						get{return this._isSelect;}
						set{this._isSelect = value;}
				}

				/// <summary>
				/// Идентификатор записи
				/// </summary>
				public int Id{
						get{return _id;}
				}

				/// <summary>
				/// Пересчет позиции и ширина редактора свойтва
				/// </summary>
				private void RecalculPropGridRowValue(){
						_propGridVP.Location = new Point(this._splitter + 3, 1);
						_propGridVP.Width = this.Width-this._splitter-5;
				}
				
				protected override void OnResize(EventArgs e) {
						this._rgSplit = this.Width - 10;
						this.Splitter = this.Width / 2+this.Width/8;

						base.OnResize (e);
				}

				protected override void OnPaint(PaintEventArgs pe) {
						Graphics g = pe.Graphics;
						int h = this.Height;
						int w = this.Width;
						string text = this._propGridVP.PropGridValue.Caption;
						RectangleF rect = new Rectangle(0,0, this.Splitter, h-1);
						StringFormat sf = new StringFormat();
						sf.Alignment = StringAlignment.Near;
						sf.FormatFlags = StringFormatFlags.NoWrap;
						sf.LineAlignment = StringAlignment.Center;
						if (this._isSelect){
								g.FillRectangle(this._selectBrush, 0,0, this._splitter, this.Height-1);
								g.DrawString(text, this.Font, this._foreBrushSelect, rect, sf);
						}else
								g.DrawString(text, this.Font, this._foreBrush, rect, sf);
						g.DrawLine(this._penSplitter, 0, h-1, w, h-1);
						g.DrawLine(this._penSplitter, this._splitter, 0, this._splitter, h);
						base.OnPaint(pe);
				}

				
				protected override void OnMouseMove(MouseEventArgs e) {
						if (this._selectSplitter){
								this.Splitter = e.X;
								if (this.SplitterChanged != null)
										this.SplitterChanged(this, _eventArgs);
						}else{
								int ws = 3;
								if (e.X > this._splitter - ws && e.X < this._splitter + ws)
										this.Cursor = Cursors.VSplit;
								else
										this.Cursor = Cursors.Default;
						}
						base.OnMouseMove (e);
				}

				protected override void OnMouseDown(MouseEventArgs e) {
						if (this.Cursor == Cursors.VSplit){
								this._selectSplitter = true;
						}
						base.OnMouseDown (e);
				}

				protected override void OnMouseLeave(EventArgs e) {
						this.Cursor = Cursors.Default;
						base.OnMouseLeave (e);
				}

				protected override void OnMouseUp(MouseEventArgs e) {
						this._selectSplitter = false;
						base.OnMouseUp (e);
				}

				protected override void OnClick(EventArgs e) {
						if (!this.IsSelect){
								this.Focus();
								base.OnClick (e);
								this._propGridVP.SelectRow();
						}
				}
		}
}
