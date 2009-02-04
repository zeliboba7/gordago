namespace Gordago {
  partial class ReportBrowserForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportBrowserForm));
      this._webBrowser = new System.Windows.Forms.WebBrowser();
      this._btnSaveAs = new System.Windows.Forms.Button();
      this._btnClose = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _webBrowser
      // 
      this._webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._webBrowser.Location = new System.Drawing.Point(0, -2);
      this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
      this._webBrowser.Name = "_webBrowser";
      this._webBrowser.Size = new System.Drawing.Size(783, 518);
      this._webBrowser.TabIndex = 0;
      // 
      // _btnSaveAs
      // 
      this._btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnSaveAs.Location = new System.Drawing.Point(12, 524);
      this._btnSaveAs.Name = "_btnSaveAs";
      this._btnSaveAs.Size = new System.Drawing.Size(128, 23);
      this._btnSaveAs.TabIndex = 1;
      this._btnSaveAs.Text = "Save As...";
      this._btnSaveAs.UseVisualStyleBackColor = true;
      this._btnSaveAs.Click += new System.EventHandler(this._btnSaveAs_Click);
      // 
      // _btnClose
      // 
      this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnClose.Location = new System.Drawing.Point(675, 524);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(95, 23);
      this._btnClose.TabIndex = 1;
      this._btnClose.Text = "Close";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
      // 
      // ReportBrowserForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnClose;
      this.ClientSize = new System.Drawing.Size(782, 556);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._btnSaveAs);
      this.Controls.Add(this._webBrowser);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ReportBrowserForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Report";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.WebBrowser _webBrowser;
    private System.Windows.Forms.Button _btnSaveAs;
    private System.Windows.Forms.Button _btnClose;
  }
}