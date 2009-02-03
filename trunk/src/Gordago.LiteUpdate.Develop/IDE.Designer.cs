namespace Gordago.LiteUpdate.Develop {
  partial class IDE {
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
      this.components = new System.ComponentModel.Container();
      this._menuStrip = new System.Windows.Forms.MenuStrip();
      this._mniFile = new System.Windows.Forms.ToolStripMenuItem();
      this._mniView = new System.Windows.Forms.ToolStripMenuItem();
      this._mniProject = new System.Windows.Forms.ToolStripMenuItem();
      this._mniHelp = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this._statusStrip = new System.Windows.Forms.StatusStrip();
      this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this._statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
      this._projectManager = new Gordago.LiteUpdate.Develop.Projects.ProjectManager(this.components);
      this._tsStandart = new System.Windows.Forms.ToolStrip();
      this._menuStrip.SuspendLayout();
      this._statusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // _menuStrip
      // 
      this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniFile,
            this._mniView,
            this._mniProject,
            this._mniHelp});
      this._menuStrip.Location = new System.Drawing.Point(0, 0);
      this._menuStrip.Name = "_menuStrip";
      this._menuStrip.Size = new System.Drawing.Size(614, 24);
      this._menuStrip.TabIndex = 0;
      this._menuStrip.Text = "menuStrip1";
      // 
      // _mniFile
      // 
      this._mniFile.Name = "_mniFile";
      this._mniFile.Size = new System.Drawing.Size(35, 20);
      this._mniFile.Text = "File";
      // 
      // _mniView
      // 
      this._mniView.Name = "_mniView";
      this._mniView.Size = new System.Drawing.Size(41, 20);
      this._mniView.Text = "View";
      // 
      // _mniProject
      // 
      this._mniProject.Name = "_mniProject";
      this._mniProject.Size = new System.Drawing.Size(53, 20);
      this._mniProject.Text = "Project";
      // 
      // _mniHelp
      // 
      this._mniHelp.Name = "_mniHelp";
      this._mniHelp.Size = new System.Drawing.Size(40, 20);
      this._mniHelp.Text = "Help";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
      // 
      // _statusStrip
      // 
      this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel,
            this._statusProgressBar});
      this._statusStrip.Location = new System.Drawing.Point(0, 410);
      this._statusStrip.Name = "_statusStrip";
      this._statusStrip.Size = new System.Drawing.Size(614, 22);
      this._statusStrip.TabIndex = 5;
      this._statusStrip.Text = "statusStrip1";
      // 
      // _statusLabel
      // 
      this._statusLabel.AutoSize = false;
      this._statusLabel.Name = "_statusLabel";
      this._statusLabel.Size = new System.Drawing.Size(200, 17);
      this._statusLabel.Text = "Ready";
      this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this._statusLabel.ToolTipText = " ";
      // 
      // _statusProgressBar
      // 
      this._statusProgressBar.Name = "_statusProgressBar";
      this._statusProgressBar.Size = new System.Drawing.Size(100, 16);
      this._statusProgressBar.Visible = false;
      // 
      // _projectManager
      // 
      this._projectManager.FileMenuItem = this._mniFile;
      this._projectManager.ProjectMenuItem = this._mniProject;
      this._projectManager.ToolStrip = null;
      // 
      // _tsStandart
      // 
      this._tsStandart.Location = new System.Drawing.Point(0, 24);
      this._tsStandart.Name = "_tsStandart";
      this._tsStandart.Size = new System.Drawing.Size(614, 25);
      this._tsStandart.TabIndex = 7;
      this._tsStandart.Text = "toolStrip1";
      // 
      // IDE
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(614, 432);
      this.Controls.Add(this._tsStandart);
      this.Controls.Add(this._statusStrip);
      this.Controls.Add(this._menuStrip);
      this.IsMdiContainer = true;
      this.MainMenuStrip = this._menuStrip;
      this.Name = "IDE";
      this.Text = "LiteUpdate Develop";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this._menuStrip.ResumeLayout(false);
      this._menuStrip.PerformLayout();
      this._statusStrip.ResumeLayout(false);
      this._statusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip _menuStrip;
    private System.Windows.Forms.ToolStripMenuItem _mniFile;
    private System.Windows.Forms.ToolStripMenuItem _mniHelp;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.StatusStrip _statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
    private Gordago.LiteUpdate.Develop.Projects.ProjectManager _projectManager;
    private System.Windows.Forms.ToolStripMenuItem _mniProject;
    private System.Windows.Forms.ToolStripProgressBar _statusProgressBar;
    private System.Windows.Forms.ToolStripMenuItem _mniView;
    private System.Windows.Forms.ToolStrip _tsStandart;
  }
}

