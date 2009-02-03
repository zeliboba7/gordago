namespace Gordago.LiteUpdate.Develop.Projects {
  partial class AddFolderForm {
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
      this._lblFolderName = new System.Windows.Forms.Label();
      this._txtFolderName = new System.Windows.Forms.TextBox();
      this._btnOk = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _lblFolderName
      // 
      this._lblFolderName.AutoSize = true;
      this._lblFolderName.Location = new System.Drawing.Point(13, 13);
      this._lblFolderName.Name = "_lblFolderName";
      this._lblFolderName.Size = new System.Drawing.Size(67, 13);
      this._lblFolderName.TabIndex = 0;
      this._lblFolderName.Text = "Folder Name";
      // 
      // _txtFolderName
      // 
      this._txtFolderName.Location = new System.Drawing.Point(13, 30);
      this._txtFolderName.Name = "_txtFolderName";
      this._txtFolderName.Size = new System.Drawing.Size(420, 20);
      this._txtFolderName.TabIndex = 1;
      // 
      // _btnOk
      // 
      this._btnOk.Location = new System.Drawing.Point(250, 65);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 2;
      this._btnOk.Text = "OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(340, 65);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 2;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // AddFolderForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(445, 100);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._txtFolderName);
      this.Controls.Add(this._lblFolderName);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "AddFolderForm";
      this.Text = "Add Folder";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label _lblFolderName;
    private System.Windows.Forms.TextBox _txtFolderName;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Button _btnCancel;
  }
}