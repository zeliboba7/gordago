/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Gordago.LstObject;

namespace Gordago.Strategy.IO {
	class StrategyExportMQLCustomIndicForm : System.Windows.Forms.Form {
		#region private property
		private System.Windows.Forms.Button _btnOK;
		private System.Windows.Forms.Button _btnCancel;
		private System.Windows.Forms.Label _lblCustomIndic;
		private System.Windows.Forms.ListView _lstCustomIndic;
		private System.ComponentModel.Container components = null;
		#endregion

		private DefineIndicator[] _defindics;

		public DefineIndicator[] DefineIndicators{
			get{return this._defindics;}
		}

		public StrategyExportMQLCustomIndicForm(Strategy.IO.StrategyExportMQL4 mql4) {
			InitializeComponent();
			_defindics = new DefineIndicator[]{};

			ColumnHeader col = new ColumnHeader();
			col.Text = "";
			this._lblCustomIndic.Text = Language.Dictionary.GetString(1,9);

			this._lstCustomIndic.Columns.Add(col);
			this._lstCustomIndic.Items.Clear();
			foreach (Strategy.IO.DefineIndicator dind in mql4.DefIndicators){
				ListObjItem lvi = new ListObjItem(dind.Name, 0, dind);
				this._lstCustomIndic.Items.Add(lvi);
			}			
			col.Width = this._lstCustomIndic.ClientSize.Width;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._btnOK = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			this._lblCustomIndic = new System.Windows.Forms.Label();
			this._lstCustomIndic = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// _btnOK
			// 
			this._btnOK.Location = new System.Drawing.Point(376, 288);
			this._btnOK.Name = "_btnOK";
			this._btnOK.TabIndex = 0;
			this._btnOK.Text = "OK";
			this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
			// 
			// _btnCancel
			// 
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnCancel.Location = new System.Drawing.Point(464, 288);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.TabIndex = 1;
			this._btnCancel.Text = "Cancel";
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _lblCustomIndic
			// 
			this._lblCustomIndic.Location = new System.Drawing.Point(8, 8);
			this._lblCustomIndic.Name = "_lblCustomIndic";
			this._lblCustomIndic.Size = new System.Drawing.Size(544, 160);
			this._lblCustomIndic.TabIndex = 2;
			// 
			// _lstCustomIndic
			// 
			this._lstCustomIndic.CheckBoxes = true;
			this._lstCustomIndic.FullRowSelect = true;
			this._lstCustomIndic.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._lstCustomIndic.Location = new System.Drawing.Point(8, 176);
			this._lstCustomIndic.MultiSelect = false;
			this._lstCustomIndic.Name = "_lstCustomIndic";
			this._lstCustomIndic.Size = new System.Drawing.Size(536, 97);
			this._lstCustomIndic.TabIndex = 3;
			this._lstCustomIndic.View = System.Windows.Forms.View.Details;
			// 
			// StrategyExportMQLCustomIndicForm
			// 
			this.AcceptButton = this._btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._btnCancel;
			this.ClientSize = new System.Drawing.Size(554, 322);
			this.Controls.Add(this._lstCustomIndic);
			this.Controls.Add(this._lblCustomIndic);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "StrategyExportMQLCustomIndicForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Options...";
			this.ResumeLayout(false);

		}
		#endregion

		private void _btnOK_Click(object sender, System.EventArgs e) {
			ArrayList al = new ArrayList();
			
			foreach(ListObjItem lvi in this._lstCustomIndic.Items){
				if (lvi.Checked){
					al.Add(lvi.OverObject as DefineIndicator);
				}
			}
			this._defindics = (DefineIndicator[])al.ToArray(typeof(DefineIndicator));
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void _btnCancel_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
