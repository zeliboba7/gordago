using System;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid {
		/// <summary>
		/// Текстовое свойство
		/// </summary>
		public class PropGridViewerText: PropGridViewerPrimitive {
			
				#region Viewer
				private class Viewer: TextBoxViewer{
						private PropGridValueText _pgvText;
						public Viewer(PropGridValueText pgvText):base(){
								this._pgvText = pgvText;
								this.Text = pgvText.Value;
						}
						protected override void OnTextChanged(EventArgs e) {
								base.OnTextChanged (e);
								this._pgvText.Value = this.Text;
						}
				}
				#endregion

				private Viewer _viewer;

				public PropGridViewerText(PropGridValueText pgValueText):base(pgValueText){
						this._viewer = new Viewer(pgValueText);
						this.SetVViewer(_viewer);
				}
				
				public PropGridViewerText():base(new PropGridValueText()){
						this._viewer = new Viewer(base._pgValue as PropGridValueText);
						this.SetVViewer(_viewer);
				}
		}
}
