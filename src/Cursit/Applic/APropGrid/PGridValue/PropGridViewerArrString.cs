using System;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid{
	/// <summary>
	/// Summary description for PropGridViewerArrStrings.
	/// </summary>
	public class PropGridViewerArrString:  PropGridViewerPrimitive {
				
		private class Viewer: ComboBox{
			private PropGridValueArrString _pgvArrString;
			public Viewer(PropGridValueArrString pgvArrString):base(){
				this._pgvArrString = pgvArrString;
				string[] strs = pgvArrString.Values;
				this.DropDownStyle = ComboBoxStyle.DropDownList;
				this.Items.AddRange(strs);
				int selindex = -1;
				for (int i=0;i<strs.Length;i++){
					if (strs[i] == _pgvArrString.Value)
						selindex = i;
				}
				if(selindex > -1)
					this.SelectedIndex = selindex;
			}


			protected override void OnSelectedItemChanged(EventArgs e) {
				base.OnSelectedItemChanged (e);
			}
			protected override void OnSelectedIndexChanged(EventArgs e) {
				base.OnSelectedIndexChanged (e);
				_pgvArrString.Value = (string)this.SelectedItem;
			}


		}
		private Viewer _viewer;
		public PropGridViewerArrString(PropGridValueArrString pgvArrString):base(pgvArrString){
			_viewer = new Viewer(pgvArrString);
			this.SetVViewer(_viewer);
		}
	}
}
