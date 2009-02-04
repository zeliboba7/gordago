/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cursit.Applic.AGUI {
		public class AButtonUpDownDomain: Control{
				
				private AButtonUpDomain _btnUpDomain;
				private AButtonDownDomain _btnDownDomain;

				public event System.EventHandler ClickUp;
				public event System.EventHandler ClickDown;
				private EventArgs _e;
				public AButtonUpDownDomain():base() {
						this.Width = 16;
						this.Height = 18;
						_e = new EventArgs();
						_btnUpDomain = new AButtonUpDomain();
						_btnUpDomain.Click += new EventHandler(this.ButtonUpDomain_Click);
						this.Controls.Add(_btnUpDomain);
						
						_btnDownDomain = new AButtonDownDomain();
						_btnDownDomain.Click += new EventHandler(this.ButtonDownDomain_Click);
						_btnDownDomain.Top = 7;
						this.Controls.Add(_btnDownDomain);
				}

				private void ButtonUpDomain_Click(object sender, EventArgs e){
						if (this.ClickUp != null)
								this.ClickUp(this, _e);
				}
				private void ButtonDownDomain_Click(object sender, EventArgs e){
						if (this.ClickDown != null)
								this.ClickDown(this, _e);
				}
				#region AButtonUpDomain
				private class AButtonUpDomain: Primitive.AButtonPrimitive{

						private Brush _tbrush;
						public AButtonUpDomain():base(){
								this.Width = 16;
								this.Height = 8;
								_tbrush = new SolidBrush(Color.Black);
						}
						protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
								base.OnPaint (e);
								Graphics g = e.Graphics;
								int center = this.Width/2;
								int x1 = 2;
								int y1 = this.Height-2;

								int x2 = this.Width-2;
								int y2 = this.Height-2;
								int x3 = center;
								int y3 = 1;

								Point[] p = new Point[]{new Point(x1, y1), new Point(x2, y2), new Point(x3, y3)};
								g.FillPolygon(_tbrush, p);
						}
				}
				#endregion
				#region AButtonDownDomain
				private class AButtonDownDomain: Primitive.AButtonPrimitive{

						private Brush _tbrush;
						public AButtonDownDomain():base(){
								this.Width = 16;
								this.Height = 8;
								_tbrush = new SolidBrush(Color.Black);
						}
						protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
								base.OnPaint (e);
								Graphics g = e.Graphics;
								int center = this.Width/2;
								int x1 = 2;
								int y1 = 2;

								int x2 = this.Width-2;
								int y2 = 2;
								int x3 = center;
								int y3 = this.Height-1;

								Point[] p = new Point[]{new Point(x1, y1), new Point(x2, y2), new Point(x3, y3)};
								g.FillPolygon(_tbrush, p);
						}
				}
				#endregion
		}
}
