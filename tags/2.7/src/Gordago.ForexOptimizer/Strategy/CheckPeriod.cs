/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion 

namespace Gordago.Strategy {
	public class CheckPeriod : System.Windows.Forms.UserControl {

		public event EventHandler ValueChanged;

		#region private property
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkPeriod;
		private System.Windows.Forms.Label lblText;
		#endregion

		private System.Windows.Forms.Label _lblBegin;
		private System.Windows.Forms.Label _lblEnd;
		private System.Windows.Forms.NumericUpDown _nudOt;
		private System.Windows.Forms.NumericUpDown _nudDo;
		private System.Windows.Forms.Label _lblStep;
		private System.Windows.Forms.NumericUpDown _nudStep;
    private TableLayoutPanel tableLayoutPanel1;

		private string _optvar;

		public CheckPeriod() {
			InitializeComponent();
			this.replCheckStat(false);
			this._lblBegin.Text = Language.Dictionary.GetString(7,3,"From:");
			this._lblEnd.Text = Language.Dictionary.GetString(7,4,"To:");
			this._lblStep.Text = Language.Dictionary.GetString(7,18,"Шаг");
		}

		public decimal Min{
			get{return this._nudOt.Minimum;}
			set{
				this._nudOt.Minimum = value;
				this._nudDo.Minimum= value;
			}
		}

		public decimal Max{
			get{return this._nudDo.Maximum;}
			set{
				this._nudDo.Maximum = value;
				this._nudOt.Maximum = value;
			}
		}

		public decimal Incriment{
			get{return this._nudOt.Increment;}
			set{
				this._nudOt.Increment = value;
				this._nudDo.Increment = value;
			}
		}

		public decimal Value{
			get{
				if (this.Checked)
					return this._nudOt.Value;
				return 0;
			}
		}

		public int Step{
			get{return Convert.ToInt32(this._nudStep.Value);}
			set{this._nudStep.Value = value;}
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

		#region private void InitializeComponent() 
		private void InitializeComponent() {
      this.chkPeriod = new System.Windows.Forms.CheckBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this._nudOt = new System.Windows.Forms.NumericUpDown();
      this._lblStep = new System.Windows.Forms.Label();
      this._lblEnd = new System.Windows.Forms.Label();
      this._lblBegin = new System.Windows.Forms.Label();
      this._nudDo = new System.Windows.Forms.NumericUpDown();
      this._nudStep = new System.Windows.Forms.NumericUpDown();
      this.lblText = new System.Windows.Forms.Label();
      this.groupBox2.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudOt)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudDo)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudStep)).BeginInit();
      this.SuspendLayout();
      // 
      // chkPeriod
      // 
      this.chkPeriod.Location = new System.Drawing.Point(8, -4);
      this.chkPeriod.Name = "chkPeriod";
      this.chkPeriod.Size = new System.Drawing.Size(16, 24);
      this.chkPeriod.TabIndex = 4;
      this.chkPeriod.CheckedChanged += new System.EventHandler(this.chkPeriod_CheckedChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.tableLayoutPanel1);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(202, 67);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.Controls.Add(this._nudOt, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this._lblStep, 2, 0);
      this.tableLayoutPanel1.Controls.Add(this._lblEnd, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this._lblBegin, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this._nudDo, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this._nudStep, 2, 1);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 23);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.81818F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.18182F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 41);
      this.tableLayoutPanel1.TabIndex = 4;
      // 
      // _nudOt
      // 
      this._nudOt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudOt.Dock = System.Windows.Forms.DockStyle.Fill;
      this._nudOt.Location = new System.Drawing.Point(3, 16);
      this._nudOt.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._nudOt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudOt.Name = "_nudOt";
      this._nudOt.Size = new System.Drawing.Size(60, 20);
      this._nudOt.TabIndex = 3;
      this._nudOt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudOt.ValueChanged += new System.EventHandler(this.nudOt_ValueChanged);
      // 
      // _lblStep
      // 
      this._lblStep.AutoSize = true;
      this._lblStep.Location = new System.Drawing.Point(135, 0);
      this._lblStep.Name = "_lblStep";
      this._lblStep.Size = new System.Drawing.Size(30, 13);
      this._lblStep.TabIndex = 0;
      this._lblStep.Text = "Шаг:";
      // 
      // _lblEnd
      // 
      this._lblEnd.AutoSize = true;
      this._lblEnd.Location = new System.Drawing.Point(69, 0);
      this._lblEnd.Name = "_lblEnd";
      this._lblEnd.Size = new System.Drawing.Size(25, 13);
      this._lblEnd.TabIndex = 0;
      this._lblEnd.Text = "До:";
      // 
      // _lblBegin
      // 
      this._lblBegin.AutoSize = true;
      this._lblBegin.Location = new System.Drawing.Point(3, 0);
      this._lblBegin.Name = "_lblBegin";
      this._lblBegin.Size = new System.Drawing.Size(23, 13);
      this._lblBegin.TabIndex = 0;
      this._lblBegin.Text = "От:";
      // 
      // _nudDo
      // 
      this._nudDo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudDo.Dock = System.Windows.Forms.DockStyle.Fill;
      this._nudDo.Location = new System.Drawing.Point(69, 16);
      this._nudDo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._nudDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudDo.Name = "_nudDo";
      this._nudDo.Size = new System.Drawing.Size(60, 20);
      this._nudDo.TabIndex = 3;
      this._nudDo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudDo.ValueChanged += new System.EventHandler(this.nudDo_ValueChanged);
      // 
      // _nudStep
      // 
      this._nudStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudStep.Dock = System.Windows.Forms.DockStyle.Fill;
      this._nudStep.Location = new System.Drawing.Point(135, 16);
      this._nudStep.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._nudStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudStep.Name = "_nudStep";
      this._nudStep.Size = new System.Drawing.Size(60, 20);
      this._nudStep.TabIndex = 3;
      this._nudStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // lblText
      // 
      this.lblText.AutoSize = true;
      this.lblText.Location = new System.Drawing.Point(24, 0);
      this.lblText.Name = "lblText";
      this.lblText.Size = new System.Drawing.Size(28, 13);
      this.lblText.TabIndex = 4;
      this.lblText.Text = "Text";
      this.lblText.Click += new System.EventHandler(this.lblText_Click);
      // 
      // CheckPeriod
      // 
      this.Controls.Add(this.lblText);
      this.Controls.Add(this.chkPeriod);
      this.Controls.Add(this.groupBox2);
      this.Name = "CheckPeriod";
      this.Size = new System.Drawing.Size(202, 67);
      this.groupBox2.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudOt)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudDo)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudStep)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}
		#endregion

		#region private void chkPeriod_CheckedChanged(object sender, System.EventArgs e)
		private void chkPeriod_CheckedChanged(object sender, System.EventArgs e) {
			this.replCheckStat(this.chkPeriod.Checked);
			if (this.ValueChanged != null)
				this.ValueChanged(this, new EventArgs());
		}
		#endregion

		#region private void replCheckStat(bool chkstat) 
		private void replCheckStat(bool chkstat) {
			_nudDo.Enabled = chkstat;
			_nudOt.Enabled = chkstat;
			_nudStep.Enabled = chkstat;
		}
		#endregion

		#region private void chageCkecked() 
		private void chageCkecked() {
			this.Checked = !this.Checked;
		}
		#endregion

		#region private void lblText_Click(object sender, System.EventArgs e)
		private void lblText_Click(object sender, System.EventArgs e) {
			this.chageCkecked();
		}
		#endregion

		#region private void nudOt_ValueChanged(object sender, System.EventArgs e) 
		private void nudOt_ValueChanged(object sender, System.EventArgs e) {
			if (_nudOt.Value > _nudDo.Value)
				_nudDo.Value = _nudOt.Value;
			if (this.ValueChanged != null)
				this.ValueChanged(this, new EventArgs());
		}
		#endregion

		#region private void nudDo_ValueChanged(object sender, System.EventArgs e) 
		private void nudDo_ValueChanged(object sender, System.EventArgs e) {
			if (_nudDo.Value < _nudOt.Value)
				_nudOt.Value = _nudDo.Value;
			if (this.ValueChanged != null)
				this.ValueChanged(this, new EventArgs());
		}
		#endregion
	
		#region public string TextPeriod 
		public string TextPeriod {
			get{return this.lblText.Text;}
			set{lblText.Text = value;}
		}
		#endregion

		#region public bool Checked 
		public bool Checked {
			get{return this.chkPeriod.Checked;}
			set{this.chkPeriod.Checked = value;}
		}
		#endregion

		public int NumberBegin{
			get{return Convert.ToInt32(_nudOt.Value);}
			set{_nudOt.Value = value;}
		}

		public int NumberEnd{
			get{return Convert.ToInt32(_nudDo.Value);}
			set{_nudDo.Value = value;}
		}

		#region public string OptVar - Заноситься имя переменной что передаеться в оптимизатор
		/// <summary>
		/// Заноситься имя переменной что передаеться в оптимизатор
		/// </summary>
		public string OptVar{
			get{
				return this._optvar;
			}
			set{this._optvar = value;}
		}
		#endregion
	}
}
