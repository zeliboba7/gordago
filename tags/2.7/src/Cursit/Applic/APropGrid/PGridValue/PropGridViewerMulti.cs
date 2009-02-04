using System;
using System.Windows.Forms;

using Cursit.Applic.AGUI;

namespace Cursit.Applic.APropGrid{
	public class PropGridViewerMulti:  PropGridViewerPrimitive {
		
		#region private class Viewer: Control
		private class Viewer: Control{

			private Label _lblView;
			private PropGridValueMulti _pgvMulti;
			public Viewer(PropGridValueMulti pgvMulti){
				_lblView = new Label();
				this.Controls.Add(_lblView);
				_pgvMulti = pgvMulti;
				this.ReDraw();
			}

			#region protected override void OnResize(EventArgs e) 
			protected override void OnResize(EventArgs e) {
				base.OnResize (e);
				_lblView.Width = this.Width;
				_lblView.Height = this.Height;
			}
			#endregion

			#region public void ReDraw()
			public void ReDraw(){
				int cnt = _pgvMulti.Value.Length ;
				if (cnt < 1) return;
				string[] sv = new string[cnt];
				if (cnt == 1){
					sv[0] = _pgvMulti.Value[0];
				}else{
					for (int i=0;i<cnt;i++){
						sv[i] =  _pgvMulti.Value[i].Substring(0,1);
					}
				}
				this._lblView.Text = string.Join(",", sv);
				this.Invalidate();
			}
			#endregion
		}
		#endregion

		private Viewer _viewer;

		public PropGridViewerMulti(PropGridValueMulti pgvMulti):base(pgvMulti) {
			_viewer = new Viewer(pgvMulti);
			this.SetVViewer(_viewer);

			AButton3Point btn3p= new AButton3Point();
			btn3p.Click += new EventHandler(this.VButton_Click);
			this.SetVButton(btn3p);
		}

		private void VButton_Click(object sender, System.EventArgs e) {
			PropGridFormMulti pgfMulti = new PropGridFormMulti(this.PropGridValue as PropGridValueMulti, this.Width);
			pgfMulti.MultiRow = (this.PropGridValue as PropGridValueMulti).MultiRow;
			System.Drawing.Point p = this.PointToScreen(new System.Drawing.Point(0,20));
			int deltax = 0;
			if (this.Width < pgfMulti.Width)
				deltax = pgfMulti.Width - this.Width;

			pgfMulti.Location = new System.Drawing.Point(p.X-deltax, p.Y);
			
			pgfMulti.ShowDialog();
			//if (pgfMulti.ShowDialog() != DialogResult.OK) 			return;
			_viewer.ReDraw();
		}

	}
}
