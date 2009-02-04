using System;
using System.Drawing;
using System.Windows.Forms;

using Cursit.Applic.AGUI;

namespace Cursit.Applic.APropGrid {
		/// <summary>
		/// Свойство цвета
		/// </summary>
		public class PropGridViewerColor:  PropGridViewerPrimitive {
				
				#region Viewer
				private class Viewer: Control{
						private Color _color;
						private Brush _colorBrush;
						private Pen _colorBorderPen;
						private PropGridValueColor _pgvColor;

						public Viewer(PropGridValueColor pgvColor):base(){
								this._pgvColor = pgvColor;
								this.Color = pgvColor.Value;
								this._colorBorderPen = new Pen(Color.Black);
						}
						

						public Color Color{
								get{return this._color;}
								set{
										if (_color == value) return;
										_color = value;
										this._colorBrush = new SolidBrush(value);
										this._pgvColor.Value = value;
										this.Invalidate();
								}
						}
						protected override void OnPaint(PaintEventArgs e) {
								base.OnPaint (e);
								Graphics g = e.Graphics;
								g.FillRectangle(this._colorBrush, this.Bounds);
								g.DrawRectangle(this._colorBorderPen, this.Bounds);
						}
				}
				#endregion

				Viewer _viewer;
				private AColorDialog _clrdlg;

				public PropGridViewerColor(PropGridValueColor pgValueColor):base(pgValueColor) {
						_viewer = new Viewer(pgValueColor);

						this.SetVViewer(_viewer);	
						AButton3Point btn3p= new AButton3Point();
						btn3p.Click += new EventHandler(this.VButton_Click);
						this.SetVButton(btn3p);
				}

				protected override void CreateHandle() {
						base.CreateHandle ();
						this._clrdlg = new AColorDialog();
				}

				private void VButton_Click(object sender, System.EventArgs e) {
						this._clrdlg.Color = _viewer.Color;
					
						if (_clrdlg.ShowDialog() == DialogResult.OK){
								_viewer.Color = _clrdlg.Color;
						}
				}
		}
}
