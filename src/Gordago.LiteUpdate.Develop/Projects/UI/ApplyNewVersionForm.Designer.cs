namespace Gordago.LiteUpdate.Develop.Projects {
  partial class ApplyNewVersionForm {
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
      this._lblChangesInFileSystem = new System.Windows.Forms.Label();
      this._lblToApplyChanges = new System.Windows.Forms.Label();
      this._btnYes = new System.Windows.Forms.Button();
      this._btnNo = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this._versionFilesModifyControl = new Gordago.LiteUpdate.Develop.Projects.Versions.VersionFilesModifyControl();
      this.SuspendLayout();
      // 
      // _lblChangesInFileSystem
      // 
      this._lblChangesInFileSystem.AutoSize = true;
      this._lblChangesInFileSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblChangesInFileSystem.Location = new System.Drawing.Point(12, 9);
      this._lblChangesInFileSystem.Name = "_lblChangesInFileSystem";
      this._lblChangesInFileSystem.Size = new System.Drawing.Size(171, 20);
      this._lblChangesInFileSystem.TabIndex = 2;
      this._lblChangesInFileSystem.Text = "Changes in file system.";
      // 
      // _lblToApplyChanges
      // 
      this._lblToApplyChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblToApplyChanges.AutoSize = true;
      this._lblToApplyChanges.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblToApplyChanges.Location = new System.Drawing.Point(13, 403);
      this._lblToApplyChanges.Name = "_lblToApplyChanges";
      this._lblToApplyChanges.Size = new System.Drawing.Size(292, 17);
      this._lblToApplyChanges.TabIndex = 2;
      this._lblToApplyChanges.Text = "To apply these changes for the new version?";
      // 
      // _btnYes
      // 
      this._btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnYes.Location = new System.Drawing.Point(109, 433);
      this._btnYes.Name = "_btnYes";
      this._btnYes.Size = new System.Drawing.Size(170, 23);
      this._btnYes.TabIndex = 3;
      this._btnYes.Text = "Create New";
      this._btnYes.UseVisualStyleBackColor = true;
      this._btnYes.Click += new System.EventHandler(this._btnYes_Click);
      // 
      // _btnNo
      // 
      this._btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnNo.Location = new System.Drawing.Point(285, 433);
      this._btnNo.Name = "_btnNo";
      this._btnNo.Size = new System.Drawing.Size(157, 23);
      this._btnNo.TabIndex = 3;
      this._btnNo.Text = "Apply Current";
      this._btnNo.UseVisualStyleBackColor = true;
      this._btnNo.Click += new System.EventHandler(this._btnNo_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(448, 433);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 3;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // _versionFilesModifyControl
      // 
      this._versionFilesModifyControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._versionFilesModifyControl.Location = new System.Drawing.Point(3, 29);
      this._versionFilesModifyControl.Name = "_versionFilesModifyControl";
      this._versionFilesModifyControl.ReadOnly = false;
      this._versionFilesModifyControl.Size = new System.Drawing.Size(536, 371);
      this._versionFilesModifyControl.TabIndex = 0;
      // 
      // ApplyNewVersionForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(542, 468);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnNo);
      this.Controls.Add(this._btnYes);
      this.Controls.Add(this._lblToApplyChanges);
      this.Controls.Add(this._lblChangesInFileSystem);
      this.Controls.Add(this._versionFilesModifyControl);
      this.Name = "ApplyNewVersionForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "New Version";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Gordago.LiteUpdate.Develop.Projects.Versions.VersionFilesModifyControl _versionFilesModifyControl;
    private System.Windows.Forms.Label _lblChangesInFileSystem;
    private System.Windows.Forms.Label _lblToApplyChanges;
    private System.Windows.Forms.Button _btnYes;
    private System.Windows.Forms.Button _btnNo;
    private System.Windows.Forms.Button _btnCancel;

  }
}