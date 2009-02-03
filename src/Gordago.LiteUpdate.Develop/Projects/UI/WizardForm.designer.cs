namespace Gordago.LiteUpdate.Develop.Projects.Wizard {
  partial class WizardForm {
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
      this._txtProjectName = new System.Windows.Forms.TextBox();
      this._lblName = new System.Windows.Forms.Label();
      this._lblLocation = new System.Windows.Forms.Label();
      this._txtProjectLocation = new System.Windows.Forms.TextBox();
      this._btnBrowseProjLocation = new System.Windows.Forms.Button();
      this._txtAppLocation = new System.Windows.Forms.TextBox();
      this._lblApplication = new System.Windows.Forms.Label();
      this._btnBrowseAppFolder = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _btnOK
      // 
      this._btnOK.Location = new System.Drawing.Point(324, 200);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 6;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(405, 200);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 7;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // _txtProjectName
      // 
      this._txtProjectName.Location = new System.Drawing.Point(105, 12);
      this._txtProjectName.Name = "_txtProjectName";
      this._txtProjectName.Size = new System.Drawing.Size(377, 20);
      this._txtProjectName.TabIndex = 1;
      // 
      // _lblName
      // 
      this._lblName.AutoSize = true;
      this._lblName.Location = new System.Drawing.Point(12, 15);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new System.Drawing.Size(38, 13);
      this._lblName.TabIndex = 2;
      this._lblName.Text = "Name:";
      // 
      // _lblLocation
      // 
      this._lblLocation.AutoSize = true;
      this._lblLocation.Location = new System.Drawing.Point(12, 49);
      this._lblLocation.Name = "_lblLocation";
      this._lblLocation.Size = new System.Drawing.Size(51, 13);
      this._lblLocation.TabIndex = 2;
      this._lblLocation.Text = "Location:";
      // 
      // _txtProjectLocation
      // 
      this._txtProjectLocation.Location = new System.Drawing.Point(105, 46);
      this._txtProjectLocation.Name = "_txtProjectLocation";
      this._txtProjectLocation.Size = new System.Drawing.Size(279, 20);
      this._txtProjectLocation.TabIndex = 2;
      // 
      // _btnBrowseProjLocation
      // 
      this._btnBrowseProjLocation.Location = new System.Drawing.Point(390, 44);
      this._btnBrowseProjLocation.Name = "_btnBrowseProjLocation";
      this._btnBrowseProjLocation.Size = new System.Drawing.Size(90, 23);
      this._btnBrowseProjLocation.TabIndex = 3;
      this._btnBrowseProjLocation.Text = "Browse...";
      this._btnBrowseProjLocation.UseVisualStyleBackColor = true;
      this._btnBrowseProjLocation.Click += new System.EventHandler(this._btnBrowseProjLocation_Click);
      // 
      // _txtAppLocation
      // 
      this._txtAppLocation.Location = new System.Drawing.Point(105, 81);
      this._txtAppLocation.Name = "_txtAppLocation";
      this._txtAppLocation.Size = new System.Drawing.Size(279, 20);
      this._txtAppLocation.TabIndex = 4;
      // 
      // _lblApplication
      // 
      this._lblApplication.AutoSize = true;
      this._lblApplication.Location = new System.Drawing.Point(12, 84);
      this._lblApplication.Name = "_lblApplication";
      this._lblApplication.Size = new System.Drawing.Size(62, 13);
      this._lblApplication.TabIndex = 2;
      this._lblApplication.Text = "Application:";
      // 
      // _btnBrowseAppFolder
      // 
      this._btnBrowseAppFolder.Location = new System.Drawing.Point(390, 79);
      this._btnBrowseAppFolder.Name = "_btnBrowseAppFolder";
      this._btnBrowseAppFolder.Size = new System.Drawing.Size(90, 23);
      this._btnBrowseAppFolder.TabIndex = 5;
      this._btnBrowseAppFolder.Text = "Browse...";
      this._btnBrowseAppFolder.UseVisualStyleBackColor = true;
      this._btnBrowseAppFolder.Click += new System.EventHandler(this._btnBrowseAppFolder_Click);
      // 
      // WizardForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(494, 235);
      this.Controls.Add(this._btnBrowseAppFolder);
      this.Controls.Add(this._btnBrowseProjLocation);
      this.Controls.Add(this._lblApplication);
      this.Controls.Add(this._lblLocation);
      this.Controls.Add(this._lblName);
      this.Controls.Add(this._txtAppLocation);
      this.Controls.Add(this._txtProjectLocation);
      this.Controls.Add(this._txtProjectName);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "WizardForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "New Project";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.TextBox _txtProjectName;
    private System.Windows.Forms.Label _lblName;
    private System.Windows.Forms.Label _lblLocation;
    private System.Windows.Forms.TextBox _txtProjectLocation;
    private System.Windows.Forms.Button _btnBrowseProjLocation;
    private System.Windows.Forms.TextBox _txtAppLocation;
    private System.Windows.Forms.Label _lblApplication;
    private System.Windows.Forms.Button _btnBrowseAppFolder;
  }
}