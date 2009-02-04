using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid {
	public class PropGridFormMulti : System.Windows.Forms.Form {

		public static string LngCancelText = "Cancel";
		public static string LngOkText = "OK";

		#region private propertyes
		private System.ComponentModel.Container components = null;

		private PropGridValueMulti _pgvMulti;
		private Button _btnCancel;
		private Button _btnOk;

		private int _rowHeight;
		private LabelRow[] _labels;

		private bool _multirow;
		private bool _isactive = false;

		#endregion

		#region private class LabelRow: CheckBox
		private class LabelRow: CheckBox{
			public event EventHandler IsSelectChanged;

			private bool _isselect;

			public LabelRow(string text):base(){
				this.Text = text;
				this.BackColor = Color.White;
			}

			public bool IsSelect{
				get{return _isselect;}
				set{
					_isselect = value;
					this.Checked = value;

					if (value)
						this.BackColor = Color.FromArgb(225, 225, 225);
					else
						this.BackColor = Color.White;
				}
			}

			protected override void OnClick(EventArgs e) {
				base.OnClick (e);
				this.IsSelect = !this.IsSelect;
				if (IsSelect){
					if (this.IsSelectChanged != null)
						this.IsSelectChanged(this, new EventArgs());
				}
			}
		}
		#endregion

		public string BtnCancelText{
			get{return this._btnCancel.Text;}
			set{this._btnCancel.Text = value;}
		}

		public string BtnOkText{
			get{return this._btnOk.Text;}
			set{this._btnOk.Text = value;}
		}

		#region public PropGridFormMulti(PropGridValueMulti pgvMulti, int width) 
		public PropGridFormMulti(PropGridValueMulti pgvMulti, int width) {
			this.MultiRow = true;
			this._pgvMulti = pgvMulti;
			_rowHeight = 16;
			string[] strs = pgvMulti.InValues;
			_labels = new LabelRow[strs.Length];
			int rowsheight = strs.Length * _rowHeight;
			this.Width = width;

			this.Size = new System.Drawing.Size(width, rowsheight+19);
			
			int i=0;
			foreach(string s in strs){
				_labels[i] = new LabelRow(s);
				_labels[i].IsSelectChanged += new EventHandler(this.LabelRow_IsSelectChanged);
				_labels[i].Name= "label" + i.ToString();
				_labels[i].Bounds = new Rectangle(1, i*this._rowHeight+1, this.Width-2, this._rowHeight-2);

				for (int ii=0;ii<_pgvMulti.Value.Length;ii++){
					string s1 = _pgvMulti.Value[ii];
					if (s1.IndexOf(s)>-1){
						_labels[i].IsSelect = true;
					}
				}

				this.Controls.Add(_labels[i]);
				i++;
			}

			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = SizeGripStyle.Hide;
			this.StartPosition = FormStartPosition.Manual;

			_btnOk = new Button();
			_btnOk.Top = rowsheight;
			_btnOk.Left = 1;
			_btnOk.Width = this.Width/2-1;
			_btnOk.Height = _rowHeight+3;
			_btnOk.Text = LngOkText;

			_btnCancel = new Button();
			_btnCancel.Top = rowsheight+1;
			_btnCancel.Left = this.Width/2+1;
			_btnCancel.Width = this.Width/2-2;
			_btnCancel.Height = _rowHeight+1;
			_btnCancel.Text = LngCancelText;
			
			_btnOk.BackColor = _btnCancel.BackColor = SystemColors.Control;
			_btnOk.Font = _btnCancel.Font = new Font("Microsoft Sans Serif", 7);
			_btnOk.TextAlign = _btnCancel.TextAlign = ContentAlignment.BottomCenter;

			this._btnOk.Click += new EventHandler(this.btnOk_Click); 
			_btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

			this.AcceptButton = _btnOk;
			this.CancelButton=_btnCancel;

			this.Controls.Add(_btnOk);
			this.Controls.Add(_btnCancel);

			this.BackColor = Color.FromArgb(50,50,50);

			this._isactive = true;
		}
		#endregion

		#region private void LabelRow_IsSelectChanged(object sender, EventArgs e)
		private void LabelRow_IsSelectChanged(object sender, EventArgs e){
			if (this.MultiRow) return;
			LabelRow lblr = sender as LabelRow;
			foreach (Control ctrl in this.Controls){
				if (ctrl is LabelRow && lblr != ctrl){
					(ctrl as LabelRow).IsSelect = false;
				}
			}
		}
		#endregion

		#region public bool MultiRow - Использование мультивыбора
		/// <summary>
		/// Использование мультивыбора
		/// </summary>
		public bool MultiRow{
			get{return this._multirow;}
			set{_multirow = value;}
		}
		#endregion

		#region private void btnCancel_Click(object sender, EventArgs e)
		private void btnCancel_Click(object sender, EventArgs e){
			this.DialogResult = DialogResult.Cancel;
			this.Close();
			this._isactive = false;
		}
		#endregion

		#region private void btnOk_Click(object sender, EventArgs e)
		private void btnOk_Click(object sender, EventArgs e){
			this.DialogResult = DialogResult.OK;

			int cnt = _labels.Length;
			int cntsel = 0;
			System.Collections.ArrayList vals = new ArrayList();
			for (int i=0;i<cnt;i++){
				if (_labels[i].IsSelect){
					vals.Add(_pgvMulti.InValues[i]);
					cntsel++;
				}
			}
			if (cntsel > 0){
				string[] valitog = new string[cntsel]; 
				for(int i=0;i<cntsel;i++){
					valitog[i] = (string)vals[i];
				}
				_pgvMulti.Value = valitog;
			}
			this.Close();
			this._isactive = false;
		}
		#endregion

		public bool IsActive{
			get{return this._isactive;}
		}

		#region protected override void Dispose( bool disposing ) 
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region protected override void OnDeactivate(EventArgs e) 
		protected override void OnDeactivate(EventArgs e) {
			base.OnDeactivate (e);
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

	}
}
