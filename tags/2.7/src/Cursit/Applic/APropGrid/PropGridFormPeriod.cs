using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Cursit.Applic.APropGrid {
	public class PropGridFormPeriod : System.Windows.Forms.Form {

		public static string LngBeginText = "From:";
		public static string LngEndText = "To:";
		public static string LngStepText = "Step:";

		public static string LngOkText = "OK";
		public static string LngCancelText = "Cancel";

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label _lblBegin;
		private System.Windows.Forms.Label _lblEnd;
		private System.Windows.Forms.NumericUpDown _nudBegin;
		private System.Windows.Forms.NumericUpDown _nudEnd;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button _btnOK;
		private System.Windows.Forms.Button _btnCancel;
		private System.Windows.Forms.Label _lblStep;
		private System.Windows.Forms.NumericUpDown _nudStep;
		private PropGridValuePeriod _pgvPeriod;


		public PropGridFormPeriod(PropGridValuePeriod pgvPeriod)		{
			InitializeComponent();
			this._nudBegin.Minimum = this._nudEnd.Minimum = pgvPeriod.Min;
			this._nudBegin.Maximum = this._nudEnd.Maximum = pgvPeriod.Max;

			this._pgvPeriod = pgvPeriod;

			this._nudBegin.Value = pgvPeriod.ValueBegin;
			this._nudEnd.Value = pgvPeriod.ValueEnd;
			this._nudStep.Value = pgvPeriod.Step;

			this._lblEnd.Text = LngEndText;
			this._lblBegin.Text = LngBeginText;
			this._lblStep.Text = LngStepText;
			this._btnCancel.Text = LngCancelText;
			this._btnOK.Text = LngOkText;
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

		#region public string LblBeginText
		public string LblBeginText{
			get{return this._lblBegin.Text;}
			set{this._lblBegin.Text = value;}
		}
		#endregion

		#region public string LblEndText
		private string LblEndText{
			get{return this._lblEnd.Text;}
			set{this._lblEnd.Text = value;}
		}
		#endregion

		public string BtnOkText{
			get{return this._btnOK.Text;}
			set{this._btnOK.Text = value;}
		}

		public string BtnCancelText{
			get{return this._btnCancel.Text;}
			set{this._btnCancel.Text = value;}
		}

		#region Windows Form Designer generated code
		private void InitializeComponent() {
			this.panel1 = new System.Windows.Forms.Panel();
			this._nudStep = new System.Windows.Forms.NumericUpDown();
			this._btnOK = new System.Windows.Forms.Button();
			this._nudBegin = new System.Windows.Forms.NumericUpDown();
			this._nudEnd = new System.Windows.Forms.NumericUpDown();
			this._lblBegin = new System.Windows.Forms.Label();
			this._lblEnd = new System.Windows.Forms.Label();
			this._btnCancel = new System.Windows.Forms.Button();
			this._lblStep = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._nudStep)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._nudBegin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._nudEnd)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this._nudStep);
			this.panel1.Controls.Add(this._btnOK);
			this.panel1.Controls.Add(this._nudBegin);
			this.panel1.Controls.Add(this._nudEnd);
			this.panel1.Controls.Add(this._lblBegin);
			this.panel1.Controls.Add(this._lblEnd);
			this.panel1.Controls.Add(this._btnCancel);
			this.panel1.Controls.Add(this._lblStep);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(100, 88);
			this.panel1.TabIndex = 0;
			// 
			// _nudStep
			// 
			this._nudStep.Location = new System.Drawing.Point(43, 40);
			this._nudStep.Minimum = new System.Decimal(new int[] {
																														 1,
																														 0,
																														 0,
																														 0});
			this._nudStep.Name = "_nudStep";
			this._nudStep.Size = new System.Drawing.Size(56, 20);
			this._nudStep.TabIndex = 1;
			this._nudStep.Value = new System.Decimal(new int[] {
																													 1,
																													 0,
																													 0,
																													 0});
			// 
			// _btnOK
			// 
			this._btnOK.BackColor = System.Drawing.SystemColors.Control;
			this._btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._btnOK.Location = new System.Drawing.Point(1, 64);
			this._btnOK.Name = "_btnOK";
			this._btnOK.Size = new System.Drawing.Size(42, 18);
			this._btnOK.TabIndex = 2;
			this._btnOK.Text = "ОК";
			this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
			// 
			// _nudBegin
			// 
			this._nudBegin.Location = new System.Drawing.Point(43, 0);
			this._nudBegin.Name = "_nudBegin";
			this._nudBegin.Size = new System.Drawing.Size(56, 20);
			this._nudBegin.TabIndex = 1;
			this._nudBegin.ValueChanged += new System.EventHandler(this._nudBegin_ValueChanged);
			// 
			// _nudEnd
			// 
			this._nudEnd.Location = new System.Drawing.Point(43, 20);
			this._nudEnd.Name = "_nudEnd";
			this._nudEnd.Size = new System.Drawing.Size(56, 20);
			this._nudEnd.TabIndex = 1;
			this._nudEnd.ValueChanged += new System.EventHandler(this._nudEnd_ValueChanged);
			// 
			// _lblBegin
			// 
			this._lblBegin.Location = new System.Drawing.Point(0, 2);
			this._lblBegin.Name = "_lblBegin";
			this._lblBegin.Size = new System.Drawing.Size(48, 16);
			this._lblBegin.TabIndex = 0;
			this._lblBegin.Text = "От";
			// 
			// _lblEnd
			// 
			this._lblEnd.Location = new System.Drawing.Point(0, 22);
			this._lblEnd.Name = "_lblEnd";
			this._lblEnd.Size = new System.Drawing.Size(48, 16);
			this._lblEnd.TabIndex = 0;
			this._lblEnd.Text = "До";
			// 
			// _btnCancel
			// 
			this._btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._btnCancel.Location = new System.Drawing.Point(46, 64);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(51, 18);
			this._btnCancel.TabIndex = 2;
			this._btnCancel.Text = "Отмена";
			this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// _lblStep
			// 
			this._lblStep.Location = new System.Drawing.Point(1, 41);
			this._lblStep.Name = "_lblStep";
			this._lblStep.Size = new System.Drawing.Size(48, 16);
			this._lblStep.TabIndex = 0;
			this._lblStep.Text = "Шаг";
			// 
			// PropGridFormPeriod
			// 
			this.AcceptButton = this._btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._btnCancel;
			this.ClientSize = new System.Drawing.Size(100, 88);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximumSize = new System.Drawing.Size(100, 88);
			this.MinimumSize = new System.Drawing.Size(100, 88);
			this.Name = "PropGridFormPeriod";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "PropGridFormPeriod";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._nudStep)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._nudBegin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._nudEnd)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, EventArgs e){
			this.CloseThisForm();
		}
		private void CloseThisForm(){
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void _btnOK_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.OK;

			if (this._nudEnd.Value < this._nudBegin.Value){
				this._nudBegin.Value = this._nudEnd.Value;
			}

			if (this._nudBegin.Value > this._nudEnd.Value){
				this._nudEnd.Value = this._nudBegin.Value;
			}

			this._pgvPeriod.ValueBegin = this._nudBegin.Value;
			this._pgvPeriod.ValueEnd = this._nudEnd.Value;
			this._pgvPeriod.Step = this._nudStep.Value;

			this._pgvPeriod.Value = this._nudBegin.Value;
			this.Close();
		}

		private void _nudBegin_ValueChanged(object sender, System.EventArgs e) {
			if (this._nudBegin.Value > this._nudEnd.Value){
				this._nudEnd.Value = this._nudBegin.Value;
			}
		}

		private void _nudEnd_ValueChanged(object sender, System.EventArgs e) {
			if (this._nudEnd.Value < this._nudBegin.Value){
				this._nudBegin.Value = this._nudEnd.Value;
			}
		}
	}
}
