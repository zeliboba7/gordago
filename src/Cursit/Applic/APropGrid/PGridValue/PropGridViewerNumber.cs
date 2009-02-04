using System;
using System.Windows.Forms;

using Cursit.Applic.AGUI;

namespace Cursit.Applic.APropGrid{
	public class PropGridViewerNumber: PropGridViewerPrimitive  {
			
		#region Viewer
		private class Viewer: NumericUpDown{
			private PropGridValue _pgv;

			public Viewer(PropGridValue pgv):base(){
				_pgv = pgv;

				this.BorderStyle = BorderStyle.None;
				decimal val = Convert.ToDecimal(pgv.Value);

				if (pgv.ValueType == PropGridValueType.Float){
					PropGridValueFloat pgvf = pgv  as PropGridValueFloat;
					this.DecimalPlaces = pgvf.DecimalPlaces;
					this.Increment = Convert.ToDecimal(pgvf.Step);
					this.Minimum = Convert.ToDecimal(pgvf.Min);
					this.Maximum = Convert.ToDecimal(pgvf.Max);
				}

				if (pgv.ValueType == PropGridValueType.Number){
					PropGridValueNumber pgvn = pgv  as PropGridValueNumber;
					this.Minimum = pgvn.Min;
					this.Maximum = pgvn.Max;
				}
				this.Value = val;
			}
			
			protected override void OnTextChanged(EventArgs e) {
				base.OnTextChanged (e);
				this.UpdateValue();
			}

			protected override void OnValueChanged(EventArgs e) {
				base.OnValueChanged (e);
				this.UpdateValue();
			}
			protected override void UpdateEditText() {
				base.UpdateEditText();
				this.UpdateValue();
			}

			protected override void ValidateEditText() {
				base.ValidateEditText ();
				try{
					this.UpdateValue();
				}catch{}
			}
			private bool UpdateValue(){
				if (_pgv == null) return false;
				if (_pgv.ValueType == PropGridValueType.Float){
					float val = Convert.ToSingle(this.Value);
					_pgv.Value = val;

				} else if (_pgv.ValueType == PropGridValueType.Number){
					_pgv.Value = Convert.ToInt32(this.Value);
				}
				return true;
			}
		}

		#endregion

		private Viewer _viewer;

		public PropGridViewerNumber(PropGridValueNumber pgvNumber):base(pgvNumber){
			OverInit();
		}

		public PropGridViewerNumber(PropGridValueFloat pgvFloat):base(pgvFloat){
			OverInit();
		}
				
		private void OverInit(){
			this._viewer = new Viewer(base._pgValue);
			this.SetVViewer(_viewer);
			AButtonUpDownDomain btnUDD = new AButtonUpDownDomain();
		}
	}
}
