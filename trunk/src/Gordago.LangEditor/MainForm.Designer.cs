namespace Gordago.LangEditor {
  partial class MainForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this._mainMenu = new System.Windows.Forms.MenuStrip();
      this._mniFile = new System.Windows.Forms.ToolStripMenuItem();
      this._mniView = new System.Windows.Forms.ToolStripMenuItem();
      this._mniHelp = new System.Windows.Forms.ToolStripMenuItem();
      this._statusStrip = new System.Windows.Forms.StatusStrip();
      this._lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
      this._mainMenu.SuspendLayout();
      this._statusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // _mainMenu
      // 
      this._mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniFile,
            this._mniView,
            this._mniHelp});
      this._mainMenu.Location = new System.Drawing.Point(0, 0);
      this._mainMenu.Name = "_mainMenu";
      this._mainMenu.Size = new System.Drawing.Size(633, 24);
      this._mainMenu.TabIndex = 0;
      this._mainMenu.Text = "menuStrip1";
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
      // _mniHelp
      // 
      this._mniHelp.Name = "_mniHelp";
      this._mniHelp.Size = new System.Drawing.Size(40, 20);
      this._mniHelp.Text = "Help";
      // 
      // _statusStrip
      // 
      this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblStatus});
      this._statusStrip.Location = new System.Drawing.Point(0, 398);
      this._statusStrip.Name = "_statusStrip";
      this._statusStrip.Size = new System.Drawing.Size(633, 22);
      this._statusStrip.TabIndex = 1;
      this._statusStrip.Text = "statusStrip1";
      // 
      // _lblStatus
      // 
      this._lblStatus.AutoSize = false;
      this._lblStatus.Name = "_lblStatus";
      this._lblStatus.Size = new System.Drawing.Size(150, 17);
      this._lblStatus.Text = "Ready";
      this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(633, 420);
      this.Controls.Add(this._statusStrip);
      this.Controls.Add(this._mainMenu);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.IsMdiContainer = true;
      this.MainMenuStrip = this._mainMenu;
      this.Name = "MainForm";
      this.Text = "Language Editor";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this._mainMenu.ResumeLayout(false);
      this._mainMenu.PerformLayout();
      this._statusStrip.ResumeLayout(false);
      this._statusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip _mainMenu;
    private System.Windows.Forms.ToolStripMenuItem _mniFile;
    private System.Windows.Forms.ToolStripMenuItem _mniHelp;
    private System.Windows.Forms.StatusStrip _statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel _lblStatus;
    private System.Windows.Forms.ToolStripMenuItem _mniView;
  }
}

