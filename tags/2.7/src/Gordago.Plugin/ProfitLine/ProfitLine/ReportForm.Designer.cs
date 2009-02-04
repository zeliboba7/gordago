namespace Gordago.Analysis {
  partial class ReportForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if(disposing && (components != null)) {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
      this._btnOK = new System.Windows.Forms.Button();
      this._lstReport = new System.Windows.Forms.ListView();
      this._colName = new System.Windows.Forms.ColumnHeader();
      this._colValue = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // _btnOK
      // 
      this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnOK.Location = new System.Drawing.Point(321, 369);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 0;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _lstReport
      // 
      this._lstReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lstReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._lstReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colName,
            this._colValue});
      this._lstReport.GridLines = true;
      this._lstReport.Location = new System.Drawing.Point(4, 4);
      this._lstReport.Name = "_lstReport";
      this._lstReport.Size = new System.Drawing.Size(400, 360);
      this._lstReport.TabIndex = 1;
      this._lstReport.UseCompatibleStateImageBehavior = false;
      this._lstReport.View = System.Windows.Forms.View.Details;
      // 
      // _colName
      // 
      this._colName.Text = "Name";
      this._colName.Width = 191;
      // 
      // _colValue
      // 
      this._colValue.Text = "Value";
      this._colValue.Width = 193;
      // 
      // ReportForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnOK;
      this.ClientSize = new System.Drawing.Size(408, 395);
      this.Controls.Add(this._lstReport);
      this.Controls.Add(this._btnOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ReportForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Report";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.ListView _lstReport;
    private System.Windows.Forms.ColumnHeader _colName;
    private System.Windows.Forms.ColumnHeader _colValue;
  }
}