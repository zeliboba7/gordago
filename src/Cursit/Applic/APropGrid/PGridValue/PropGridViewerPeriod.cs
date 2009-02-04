using System;
using System.Drawing;
using System.Windows.Forms;

using Cursit.Applic.AGUI;

namespace Cursit.Applic.APropGrid {
	public class PropGridViewerPeriod:  PropGridViewerPrimitive {
		private class Viewer: Control{

			private Label _lblView;
			private PropGridValuePeriod _pgvPeriod;

			public Viewer(PropGridValuePeriod pgvPeriod){
				_lblView = new Label();
				this.Controls.Add(_lblView);
				_pgvPeriod = pgvPeriod;
				this.ReDraw();
			}

			protected override void OnResize(EventArgs e) {
				base.OnResize (e);
				_lblView.Width = this.Width;
				_lblView.Height = this.Height;
			}

			public void ReDraw(){
				string str = Convert.ToString(_pgvPeriod.ValueBegin)+";"+
					Convert.ToString(_pgvPeriod.ValueEnd);

				this._lblView.Text = str;
			}
		}

		private Viewer _viewer;
		private PropGridValuePeriod _pgvPeriod;
		public PropGridViewerPeriod(PropGridValuePeriod pgvPeriod):base(pgvPeriod) {
			_viewer = new Viewer(pgvPeriod);
			_pgvPeriod = pgvPeriod;
			this.SetVViewer(_viewer);
			AButton3Point btn3p= new AButton3Point();
			btn3p.Click += new EventHandler(this.VButton_Click);
			this.SetVButton(btn3p);
		}

		private void VButton_Click(object sender, System.EventArgs e) {
			PropGridFormPeriod pgfPeriod = new PropGridFormPeriod(_pgValue as PropGridValuePeriod);
			Point p = this.PointToScreen(new System.Drawing.Point(0,20));
			int deltax = 0;
			if (this.Width < pgfPeriod.Width)
				deltax = pgfPeriod.Width - this.Width;

			pgfPeriod.Location = new Point(p.X-deltax, p.Y);
			if (pgfPeriod.ShowDialog() != DialogResult.OK) return;

			_viewer.ReDraw();
		}
	}
}
