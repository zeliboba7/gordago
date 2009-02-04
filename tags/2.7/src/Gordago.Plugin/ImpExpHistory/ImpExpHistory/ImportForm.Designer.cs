namespace Gordago.PlugIn.ImpExpHistory {
  partial class ImportForm {
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
      this._btnOK = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this._txtFile = new System.Windows.Forms.TextBox();
      this._btnFile = new System.Windows.Forms.Button();
      this._gboxFile = new System.Windows.Forms.GroupBox();
      this._cboxOptions = new System.Windows.Forms.GroupBox();
      this._chkFirstLineHeader = new System.Windows.Forms.CheckBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this._txtDivFields = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this._gboxFile.SuspendLayout();
      this._cboxOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // _btnOK
      // 
      this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOK.Location = new System.Drawing.Point(276, 235);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 0;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.Location = new System.Drawing.Point(357, 235);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 0;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // _txtFile
      // 
      this._txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtFile.Location = new System.Drawing.Point(6, 19);
      this._txtFile.Name = "_txtFile";
      this._txtFile.Size = new System.Drawing.Size(387, 20);
      this._txtFile.TabIndex = 1;
      this._txtFile.TextChanged += new System.EventHandler(this._txtFile_TextChanged);
      // 
      // _btnFile
      // 
      this._btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnFile.Location = new System.Drawing.Point(399, 17);
      this._btnFile.Name = "_btnFile";
      this._btnFile.Size = new System.Drawing.Size(28, 23);
      this._btnFile.TabIndex = 2;
      this._btnFile.Text = "...";
      this._btnFile.UseVisualStyleBackColor = true;
      this._btnFile.Click += new System.EventHandler(this._btnFile_Click);
      // 
      // _gboxFile
      // 
      this._gboxFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxFile.Controls.Add(this._txtFile);
      this._gboxFile.Controls.Add(this._btnFile);
      this._gboxFile.Location = new System.Drawing.Point(5, 12);
      this._gboxFile.Name = "_gboxFile";
      this._gboxFile.Size = new System.Drawing.Size(435, 44);
      this._gboxFile.TabIndex = 3;
      this._gboxFile.TabStop = false;
      this._gboxFile.Text = "Source";
      // 
      // _cboxOptions
      // 
      this._cboxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._cboxOptions.Controls.Add(this._chkFirstLineHeader);
      this._cboxOptions.Controls.Add(this.textBox1);
      this._cboxOptions.Controls.Add(this._txtDivFields);
      this._cboxOptions.Controls.Add(this.label2);
      this._cboxOptions.Controls.Add(this.label1);
      this._cboxOptions.Location = new System.Drawing.Point(5, 63);
      this._cboxOptions.Name = "_cboxOptions";
      this._cboxOptions.Size = new System.Drawing.Size(435, 83);
      this._cboxOptions.TabIndex = 4;
      this._cboxOptions.TabStop = false;
      this._cboxOptions.Text = "Options";
      // 
      // _chkFirstLineHeader
      // 
      this._chkFirstLineHeader.AutoSize = true;
      this._chkFirstLineHeader.Location = new System.Drawing.Point(268, 15);
      this._chkFirstLineHeader.Name = "_chkFirstLineHeader";
      this._chkFirstLineHeader.Size = new System.Drawing.Size(112, 17);
      this._chkFirstLineHeader.TabIndex = 2;
      this._chkFirstLineHeader.Text = "First line is Header";
      this._chkFirstLineHeader.UseVisualStyleBackColor = true;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(108, 46);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(26, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.Text = ".";
      this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // _txtDivFields
      // 
      this._txtDivFields.Location = new System.Drawing.Point(108, 17);
      this._txtDivFields.Name = "_txtDivFields";
      this._txtDivFields.Size = new System.Drawing.Size(26, 20);
      this._txtDivFields.TabIndex = 1;
      this._txtDivFields.Text = ",";
      this._txtDivFields.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(8, 49);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(85, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Digits of Number";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(8, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(79, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Divider of fields";
      // 
      // ImportForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(444, 270);
      this.Controls.Add(this._cboxOptions);
      this.Controls.Add(this._gboxFile);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ImportForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Import";
      this._gboxFile.ResumeLayout(false);
      this._gboxFile.PerformLayout();
      this._cboxOptions.ResumeLayout(false);
      this._cboxOptions.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.TextBox _txtFile;
    private System.Windows.Forms.Button _btnFile;
    private System.Windows.Forms.GroupBox _gboxFile;
    private System.Windows.Forms.GroupBox _cboxOptions;
    private System.Windows.Forms.TextBox _txtDivFields;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.CheckBox _chkFirstLineHeader;
  }
}