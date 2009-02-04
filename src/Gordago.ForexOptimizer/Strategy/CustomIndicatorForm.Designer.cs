namespace Gordago.Strategy {
  partial class CustomIndicatorForm {
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
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._nudDecimalDigits = new System.Windows.Forms.NumericUpDown();
      this._lblDecDig = new System.Windows.Forms.Label();
      this._txtName = new System.Windows.Forms.TextBox();
      this._lblName = new System.Windows.Forms.Label();
      this._tbcMain = new System.Windows.Forms.TabControl();
      this._btnAdd = new System.Windows.Forms.Button();
      this._btnDel = new System.Windows.Forms.Button();
      this._btnClose = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this._nudDecimalDigits)).BeginInit();
      this.SuspendLayout();
      // 
      // _nudDecimalDigits
      // 
      this._nudDecimalDigits.Location = new System.Drawing.Point(140, 30);
      this._nudDecimalDigits.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this._nudDecimalDigits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
      this._nudDecimalDigits.Name = "_nudDecimalDigits";
      this._nudDecimalDigits.Size = new System.Drawing.Size(58, 20);
      this._nudDecimalDigits.TabIndex = 2;
      this._nudDecimalDigits.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudDecimalDigits.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
      // 
      // _lblDecDig
      // 
      this._lblDecDig.AutoSize = true;
      this._lblDecDig.Location = new System.Drawing.Point(12, 32);
      this._lblDecDig.Name = "_lblDecDig";
      this._lblDecDig.Size = new System.Drawing.Size(74, 13);
      this._lblDecDig.TabIndex = 4;
      this._lblDecDig.Text = "Decimal Digits";
      // 
      // _txtName
      // 
      this._txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtName.Enabled = false;
      this._txtName.Location = new System.Drawing.Point(140, 6);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new System.Drawing.Size(248, 20);
      this._txtName.TabIndex = 1;
      this._txtName.Text = "CustomIndicatorName";
      this._txtName.TextChanged += new System.EventHandler(this._txtName_TextChanged);
      // 
      // _lblName
      // 
      this._lblName.AutoSize = true;
      this._lblName.Location = new System.Drawing.Point(12, 9);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new System.Drawing.Size(35, 13);
      this._lblName.TabIndex = 6;
      this._lblName.Text = "Name";
      // 
      // _tbcMain
      // 
      this._tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbcMain.Location = new System.Drawing.Point(5, 56);
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(383, 118);
      this._tbcMain.TabIndex = 3;
      // 
      // _btnAdd
      // 
      this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnAdd.Location = new System.Drawing.Point(12, 188);
      this._btnAdd.Name = "_btnAdd";
      this._btnAdd.Size = new System.Drawing.Size(75, 23);
      this._btnAdd.TabIndex = 4;
      this._btnAdd.Text = "Add";
      this._btnAdd.UseVisualStyleBackColor = true;
      this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
      // 
      // _btnDel
      // 
      this._btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnDel.Location = new System.Drawing.Point(93, 188);
      this._btnDel.Name = "_btnDel";
      this._btnDel.Size = new System.Drawing.Size(75, 23);
      this._btnDel.TabIndex = 5;
      this._btnDel.Text = "Remove";
      this._btnDel.UseVisualStyleBackColor = true;
      this._btnDel.Click += new System.EventHandler(this._btnDel_Click);
      // 
      // _btnClose
      // 
      this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnClose.Location = new System.Drawing.Point(308, 188);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(75, 23);
      this._btnClose.TabIndex = 0;
      this._btnClose.Text = "Close";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
      // 
      // CustomIndicatorForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(395, 223);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._btnDel);
      this.Controls.Add(this._btnAdd);
      this.Controls.Add(this._tbcMain);
      this.Controls.Add(this._txtName);
      this.Controls.Add(this._lblName);
      this.Controls.Add(this._nudDecimalDigits);
      this.Controls.Add(this._lblDecDig);
      this.Name = "CustomIndicatorForm";
      this.Text = "CustomIndicatorForm";
      ((System.ComponentModel.ISupportInitialize)(this._nudDecimalDigits)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NumericUpDown _nudDecimalDigits;
    private System.Windows.Forms.Label _lblDecDig;
    private System.Windows.Forms.TextBox _txtName;
    private System.Windows.Forms.Label _lblName;
    private System.Windows.Forms.TabControl _tbcMain;
    private System.Windows.Forms.Button _btnAdd;
    private System.Windows.Forms.Button _btnDel;
    private System.Windows.Forms.Button _btnClose;
  }
}