namespace Gordago.AppStructureEditor.Projects {
  partial class ProjectForm {
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
      this.label2 = new System.Windows.Forms.Label();
      this._txtAppRootDir = new System.Windows.Forms.TextBox();
      this._btnBrowseAppRootDir = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this._txtLanguagesDir = new System.Windows.Forms.TextBox();
      this._btnBrowseLanguagesDir = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this._txtOptionsDirectory = new System.Windows.Forms.TextBox();
      this._btnBrowseOptionsDirectory = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Bin Directory";
      // 
      // _txtBinDirectory
      // 
      this._txtBinDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtBinDirectory.Location = new System.Drawing.Point(13, 30);
      this._txtBinDirectory.Name = "_txtBinDirectory";
      this._txtBinDirectory.ReadOnly = true;
      this._txtBinDirectory.Size = new System.Drawing.Size(447, 20);
      this._txtBinDirectory.TabIndex = 1;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 65);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(130, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Application Root Directory";
      // 
      // _txtAppRootDir
      // 
      this._txtAppRootDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtAppRootDir.Location = new System.Drawing.Point(13, 82);
      this._txtAppRootDir.Name = "_txtAppRootDir";
      this._txtAppRootDir.Size = new System.Drawing.Size(366, 20);
      this._txtAppRootDir.TabIndex = 2;
      // 
      // _btnBrowseAppRootDir
      // 
      this._btnBrowseAppRootDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnBrowseAppRootDir.Location = new System.Drawing.Point(385, 80);
      this._btnBrowseAppRootDir.Name = "_btnBrowseAppRootDir";
      this._btnBrowseAppRootDir.Size = new System.Drawing.Size(75, 23);
      this._btnBrowseAppRootDir.TabIndex = 3;
      this._btnBrowseAppRootDir.Text = "Browse...";
      this._btnBrowseAppRootDir.UseVisualStyleBackColor = true;
      this._btnBrowseAppRootDir.Click += new System.EventHandler(this._btnBrowseAppRootDir_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 114);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(105, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Languages Directory";
      // 
      // _txtLanguagesDir
      // 
      this._txtLanguagesDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtLanguagesDir.Location = new System.Drawing.Point(13, 131);
      this._txtLanguagesDir.Name = "_txtLanguagesDir";
      this._txtLanguagesDir.Size = new System.Drawing.Size(366, 20);
      this._txtLanguagesDir.TabIndex = 2;
      // 
      // _btnBrowseLanguagesDir
      // 
      this._btnBrowseLanguagesDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnBrowseLanguagesDir.Location = new System.Drawing.Point(385, 129);
      this._btnBrowseLanguagesDir.Name = "_btnBrowseLanguagesDir";
      this._btnBrowseLanguagesDir.Size = new System.Drawing.Size(75, 23);
      this._btnBrowseLanguagesDir.TabIndex = 3;
      this._btnBrowseLanguagesDir.Text = "Browse...";
      this._btnBrowseLanguagesDir.UseVisualStyleBackColor = true;
      this._btnBrowseLanguagesDir.Click += new System.EventHandler(this._btnBrowseLanguagesDir_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 165);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(88, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Options Directory";
      // 
      // _txtOptionsDirectory
      // 
      this._txtOptionsDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtOptionsDirectory.Location = new System.Drawing.Point(13, 182);
      this._txtOptionsDirectory.Name = "_txtOptionsDirectory";
      this._txtOptionsDirectory.Size = new System.Drawing.Size(366, 20);
      this._txtOptionsDirectory.TabIndex = 2;
      // 
      // _btnBrowseOptionsDirectory
      // 
      this._btnBrowseOptionsDirectory.Location = new System.Drawing.Point(385, 180);
      this._btnBrowseOptionsDirectory.Name = "_btnBrowseOptionsDirectory";
      this._btnBrowseOptionsDirectory.Size = new System.Drawing.Size(75, 23);
      this._btnBrowseOptionsDirectory.TabIndex = 4;
      this._btnBrowseOptionsDirectory.Text = "Browse...";
      this._btnBrowseOptionsDirectory.UseVisualStyleBackColor = true;
      this._btnBrowseOptionsDirectory.Click += new System.EventHandler(this._btnBrowseOptionsDirectory_Click);
      // 
      // ProjectForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(472, 337);
      this.Controls.Add(this._btnBrowseOptionsDirectory);
      this.Controls.Add(this._btnBrowseLanguagesDir);
      this.Controls.Add(this._btnBrowseAppRootDir);
      this.Controls.Add(this._txtOptionsDirectory);
      this.Controls.Add(this._txtLanguagesDir);
      this.Controls.Add(this._txtAppRootDir);
      this.Controls.Add(this.label4);
      this.Controls.Add(this._txtBinDirectory);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "ProjectForm";
      this.Text = "ProjectForm";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _txtBinDirectory;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _txtAppRootDir;
    private System.Windows.Forms.Button _btnBrowseAppRootDir;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox _txtLanguagesDir;
    private System.Windows.Forms.Button _btnBrowseLanguagesDir;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox _txtOptionsDirectory;
    private System.Windows.Forms.Button _btnBrowseOptionsDirectory;
  }
}