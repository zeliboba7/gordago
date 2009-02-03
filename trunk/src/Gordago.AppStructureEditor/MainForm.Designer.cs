namespace Gordago.AppStructureEditor {
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
      this._mainMenu = new System.Windows.Forms.MenuStrip();
      this._mniFile = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._mniAbout = new System.Windows.Forms.ToolStripMenuItem();
      this._mainMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // _mainMenu
      // 
      this._mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniFile,
            this.helpToolStripMenuItem});
      this._mainMenu.Location = new System.Drawing.Point(0, 0);
      this._mainMenu.Name = "_mainMenu";
      this._mainMenu.Size = new System.Drawing.Size(566, 24);
      this._mainMenu.TabIndex = 0;
      this._mainMenu.Text = "menuStrip1";
      // 
      // _mniFile
      // 
      this._mniFile.Name = "_mniFile";
      this._mniFile.Size = new System.Drawing.Size(35, 20);
      this._mniFile.Text = "File";
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniAbout});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // _mniAbout
      // 
      this._mniAbout.Name = "_mniAbout";
      this._mniAbout.Size = new System.Drawing.Size(114, 22);
      this._mniAbout.Text = "About";
      this._mniAbout.Click += new System.EventHandler(this._mniAbout_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(566, 398);
      this.Controls.Add(this._mainMenu);
      this.IsMdiContainer = true;
      this.MainMenuStrip = this._mainMenu;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Gordago AppStructure Editor";
      this._mainMenu.ResumeLayout(false);
      this._mainMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip _mainMenu;
    private System.Windows.Forms.ToolStripMenuItem _mniFile;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _mniAbout;
  }
}

