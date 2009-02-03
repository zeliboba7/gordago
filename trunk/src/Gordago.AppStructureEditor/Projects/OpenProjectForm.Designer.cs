namespace Gordago.AppStructureEditor.Projects {
  partial class OpenProjectForm {
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
      this.label1 = new System.Windows.Forms.Label();
      this._txtBinDirectory = new System.Windows.Forms.TextBox();
      this._btnBrowse = new System.Windows.Forms.Button();
      this._btnOK = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(122, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Application Bin Directory";
      // 
      // _txtBinDirectory
      // 
      this._txtBinDirectory.Location = new System.Drawing.Point(13, 30);
      this._txtBinDirectory.Name = "_txtBinDirectory";
      this._txtBinDirectory.Size = new System.Drawing.Size(328, 20);
      this._txtBinDirectory.TabIndex = 1;
      // 
      // _btnBrowse
      // 
      this._btnBrowse.Location = new System.Drawing.Point(347, 28);
      this._btnBrowse.Name = "_btnBrowse";
      this._btnBrowse.Size = new System.Drawing.Size(79, 23);
      this._btnBrowse.TabIndex = 2;
      this._btnBrowse.Text = "Browse...";
      this._btnBrowse.UseVisualStyleBackColor = true;
      this._btnBrowse.Click += new System.EventHandler(this._btnBrowse_Click);
      // 
      // _btnOK
      // 
      this._btnOK.Location = new System.Drawing.Point(247, 93);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 3;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.Location = new System.Drawing.Point(338, 93);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 3;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // OpenProjectForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(442, 133);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.Controls.Add(this._btnBrowse);
      this.Controls.Add(this._txtBinDirectory);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "OpenProjectForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Open project";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _txtBinDirectory;
    private System.Windows.Forms.Button _btnBrowse;
    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Button _btnCancel;
  }
}