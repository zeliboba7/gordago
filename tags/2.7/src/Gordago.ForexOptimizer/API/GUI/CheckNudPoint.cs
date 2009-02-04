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
using System.Data;
using System.Windows.Forms;
#endregion

namespace Gordago.API {

  public class CheckNudPoint: System.Windows.Forms.UserControl {
		private System.Windows.Forms.NumericUpDown _nudNumber;
    private CheckBox _chkText;
		private System.ComponentModel.Container components = null;

		public event EventHandler CheckedChanged;
		public event EventHandler ValueChanged;

    private decimal _savedValue = 0;
    private bool _savedChecked = false;

		#region public CheckNudPoint()
		public CheckNudPoint() {
			InitializeComponent();
			this.Checked = false;
			this.Caption = this.Name;
		}
		#endregion

		#region public new Color ForeColor
		public new Color ForeColor{
			get{return this._chkText.ForeColor;}
			set{this._chkText.ForeColor = value;}
		}
		#endregion

		#region public string Caption
		public string Caption{
			get{return this._chkText.Text;}
			set{this._chkText.Text = value;}
		}
		#endregion

		#region public int DecimalDigits
		public int DecimalDigits{
			get{return this._nudNumber.DecimalPlaces;}
			set{
        if (this._nudNumber.DecimalPlaces == value)
          return;

				this._nudNumber.DecimalPlaces = value;
				decimal incr = 1;
				if (value > 0){
					int del = SymbolManager.GetDelimiter(value);
					float onepoint = 1f / del;
					incr = Convert.ToDecimal(onepoint);
				}
				this._nudNumber.Increment = incr;
			}
		}
		#endregion

		#region public float Minimum
		public float Minimum{
			get{return Convert.ToSingle(this._nudNumber.Minimum);}
			set{
        if (float.IsNaN(value))
          value = 0;

        decimal newValue = Convert.ToDecimal(value);

        if (this._nudNumber.Minimum == newValue)
          return;

        this._nudNumber.Minimum = newValue;
      }
		}
		#endregion

		#region public float Maximum
		public float Maximum{
			get{return Convert.ToSingle(this._nudNumber.Maximum);}
			set{
        if (float.IsNaN(value))
          value = 0;

        decimal newValue = Convert.ToDecimal(value);
        if (this._nudNumber.Maximum == newValue)
          return;
        this._nudNumber.Maximum = Math.Min(newValue, 1000);
      }
		}
		#endregion

		#region public float Value
		public float Value{
			get{
				if (!this._chkText.Checked)
					return float.NaN;
				return Convert.ToSingle(this._nudNumber.Value);

			}
			set{

        if (float.IsNaN(value))
          value = 0;

        decimal newValue = Convert.ToDecimal(value);
        
        if (_nudNumber.Value == newValue)
          return;

				this._nudNumber.Value = Math.Max(newValue, this._nudNumber.Minimum);
			}
		}
		#endregion

		#region public bool Checked
		public bool Checked{
			get{return this._chkText.Checked;}
			set{
        if (this._chkText.Checked == value)
          return;
				this._chkText.Checked = value;
				this.RefreshStatus();
			}
		}
		#endregion

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

		#region Component Designer generated code
		private void InitializeComponent() {
      this._chkText = new System.Windows.Forms.CheckBox();
      this._nudNumber = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this._nudNumber)).BeginInit();
      this.SuspendLayout();
      // 
      // _chkText
      // 
      this._chkText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._chkText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._chkText.Location = new System.Drawing.Point(0, 0);
      this._chkText.Name = "_chkText";
      this._chkText.Size = new System.Drawing.Size(55, 16);
      this._chkText.TabIndex = 14;
      this._chkText.Text = "SL";
      this._chkText.UseVisualStyleBackColor = false;
      this._chkText.CheckedChanged += new System.EventHandler(this._chkText_CheckedChanged);
      // 
      // _nudNumber
      // 
      this._nudNumber.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._nudNumber.Enabled = false;
      this._nudNumber.Location = new System.Drawing.Point(0, 19);
      this._nudNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudNumber.Name = "_nudNumber";
      this._nudNumber.Size = new System.Drawing.Size(63, 20);
      this._nudNumber.TabIndex = 15;
      this._nudNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudNumber.ValueChanged += new System.EventHandler(this._nudNumber_ValueChanged);
      // 
      // CheckNudPoint
      // 
      this.Controls.Add(this._chkText);
      this.Controls.Add(this._nudNumber);
      this.Name = "CheckNudPoint";
      this.Size = new System.Drawing.Size(63, 39);
      ((System.ComponentModel.ISupportInitialize)(this._nudNumber)).EndInit();
      this.ResumeLayout(false);

		}
		#endregion

		#region private void RefreshStatus()
		private void RefreshStatus(){
			this._nudNumber.Enabled = this.Checked;
		}
		#endregion

		#region private void _chkText_CheckedChanged(object sender, System.EventArgs e) 
		private void _chkText_CheckedChanged(object sender, System.EventArgs e) {
			this.RefreshStatus();
			this.OnCheckedChanged(sender, e);
		}
		#endregion

		#region protected virtual void OnCheckedChanged(object sender, EventArgs e)
		protected virtual void OnCheckedChanged(object sender, EventArgs e){

			if (_savedChecked != this.Checked && this.CheckedChanged != null)
				this.CheckedChanged(sender, e);
      this._savedChecked = this.Checked;
		}
		#endregion

		#region private void _nudNumber_ValueChanged(object sender, System.EventArgs e) 
		private void _nudNumber_ValueChanged(object sender, System.EventArgs e) {

			if (this._savedValue != this._nudNumber.Value && this.ValueChanged != null)
				this.ValueChanged(this, e);
      this._savedValue = this._nudNumber.Value;
		}
		#endregion
	}
}
