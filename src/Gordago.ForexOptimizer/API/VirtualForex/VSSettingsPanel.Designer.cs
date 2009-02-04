namespace Gordago.API.VirtualForex {
  partial class VSSettingsPanel {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
      this.Save();
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._cmbUseTimeFrame = new System.Windows.Forms.ComboBox();
      this._lblUseTimeFrame = new System.Windows.Forms.Label();
      this._lblDeposit = new System.Windows.Forms.Label();
      this._cmbLotSize = new System.Windows.Forms.ComboBox();
      this._lblLeverage = new System.Windows.Forms.Label();
      this._cmbLeverage = new System.Windows.Forms.ComboBox();
      this._lblLotSize = new System.Windows.Forms.Label();
      this._nudCommission = new System.Windows.Forms.NumericUpDown();
      this._lblCommission = new System.Windows.Forms.Label();
      this._nudBalance = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this._nudCommission)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudBalance)).BeginInit();
      this.SuspendLayout();
      // 
      // _cmbUseTimeFrame
      // 
      this._cmbUseTimeFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbUseTimeFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbUseTimeFrame.FormattingEnabled = true;
      this._cmbUseTimeFrame.Location = new System.Drawing.Point(116, 98);
      this._cmbUseTimeFrame.Name = "_cmbUseTimeFrame";
      this._cmbUseTimeFrame.Size = new System.Drawing.Size(94, 21);
      this._cmbUseTimeFrame.TabIndex = 3;
      // 
      // _lblUseTimeFrame
      // 
      this._lblUseTimeFrame.AutoSize = true;
      this._lblUseTimeFrame.Location = new System.Drawing.Point(3, 101);
      this._lblUseTimeFrame.Name = "_lblUseTimeFrame";
      this._lblUseTimeFrame.Size = new System.Drawing.Size(50, 13);
      this._lblUseTimeFrame.TabIndex = 1;
      this._lblUseTimeFrame.Text = "Use data";
      // 
      // _lblDeposit
      // 
      this._lblDeposit.AutoSize = true;
      this._lblDeposit.Location = new System.Drawing.Point(3, 5);
      this._lblDeposit.Name = "_lblDeposit";
      this._lblDeposit.Size = new System.Drawing.Size(43, 13);
      this._lblDeposit.TabIndex = 1;
      this._lblDeposit.Text = "Deposit";
      // 
      // _cmbLotSize
      // 
      this._cmbLotSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbLotSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbLotSize.FormattingEnabled = true;
      this._cmbLotSize.Location = new System.Drawing.Point(116, 49);
      this._cmbLotSize.Name = "_cmbLotSize";
      this._cmbLotSize.Size = new System.Drawing.Size(94, 21);
      this._cmbLotSize.TabIndex = 3;
      // 
      // _lblLeverage
      // 
      this._lblLeverage.AutoSize = true;
      this._lblLeverage.Location = new System.Drawing.Point(3, 76);
      this._lblLeverage.Name = "_lblLeverage";
      this._lblLeverage.Size = new System.Drawing.Size(55, 13);
      this._lblLeverage.TabIndex = 1;
      this._lblLeverage.Text = "Leverage ";
      // 
      // _cmbLeverage
      // 
      this._cmbLeverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbLeverage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbLeverage.FormattingEnabled = true;
      this._cmbLeverage.Location = new System.Drawing.Point(116, 73);
      this._cmbLeverage.Name = "_cmbLeverage";
      this._cmbLeverage.Size = new System.Drawing.Size(94, 21);
      this._cmbLeverage.TabIndex = 3;
      // 
      // _lblLotSize
      // 
      this._lblLotSize.AutoSize = true;
      this._lblLotSize.Location = new System.Drawing.Point(3, 52);
      this._lblLotSize.Name = "_lblLotSize";
      this._lblLotSize.Size = new System.Drawing.Size(45, 13);
      this._lblLotSize.TabIndex = 1;
      this._lblLotSize.Text = "Lot Size";
      // 
      // _nudCommission
      // 
      this._nudCommission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._nudCommission.DecimalPlaces = 2;
      this._nudCommission.Location = new System.Drawing.Point(116, 26);
      this._nudCommission.Name = "_nudCommission";
      this._nudCommission.Size = new System.Drawing.Size(94, 20);
      this._nudCommission.TabIndex = 2;
      this._nudCommission.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudCommission.Value = new decimal(new int[] {
            125,
            0,
            0,
            131072});
      // 
      // _lblCommission
      // 
      this._lblCommission.AutoSize = true;
      this._lblCommission.Location = new System.Drawing.Point(3, 28);
      this._lblCommission.Name = "_lblCommission";
      this._lblCommission.Size = new System.Drawing.Size(62, 13);
      this._lblCommission.TabIndex = 1;
      this._lblCommission.Text = "Commission";
      // 
      // _nudBalance
      // 
      this._nudBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._nudBalance.DecimalPlaces = 2;
      this._nudBalance.Location = new System.Drawing.Point(116, 3);
      this._nudBalance.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
      this._nudBalance.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this._nudBalance.Name = "_nudBalance";
      this._nudBalance.Size = new System.Drawing.Size(94, 20);
      this._nudBalance.TabIndex = 2;
      this._nudBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudBalance.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // VSSettingsPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._cmbUseTimeFrame);
      this.Controls.Add(this._lblUseTimeFrame);
      this.Controls.Add(this._nudBalance);
      this.Controls.Add(this._lblDeposit);
      this.Controls.Add(this._lblCommission);
      this.Controls.Add(this._cmbLotSize);
      this.Controls.Add(this._nudCommission);
      this.Controls.Add(this._lblLeverage);
      this.Controls.Add(this._lblLotSize);
      this.Controls.Add(this._cmbLeverage);
      this.Name = "VSSettingsPanel";
      this.Size = new System.Drawing.Size(213, 125);
      ((System.ComponentModel.ISupportInitialize)(this._nudCommission)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudBalance)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox _cmbUseTimeFrame;
    private System.Windows.Forms.Label _lblUseTimeFrame;
    private System.Windows.Forms.Label _lblDeposit;
    private System.Windows.Forms.ComboBox _cmbLotSize;
    private System.Windows.Forms.Label _lblLeverage;
    private System.Windows.Forms.ComboBox _cmbLeverage;
    private System.Windows.Forms.Label _lblLotSize;
    private System.Windows.Forms.NumericUpDown _nudCommission;
    private System.Windows.Forms.Label _lblCommission;
    private System.Windows.Forms.NumericUpDown _nudBalance;
  }
}
