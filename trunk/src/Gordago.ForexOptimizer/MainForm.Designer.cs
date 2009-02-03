namespace Gordago.FO {
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
      this._menuStrip = new System.Windows.Forms.MenuStrip();
      this._mniFile = new System.Windows.Forms.ToolStripMenuItem();
      this._mniView = new System.Windows.Forms.ToolStripMenuItem();
      this._mniChart = new System.Windows.Forms.ToolStripMenuItem();
      this._mniBuild = new System.Windows.Forms.ToolStripMenuItem();
      this._mniHelp = new System.Windows.Forms.ToolStripMenuItem();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this._stripStatus = new System.Windows.Forms.ToolStripStatusLabel();
      this._tsStandart = new System.Windows.Forms.ToolStrip();
      this._menuStrip.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _menuStrip
      // 
      this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniFile,
            this._mniView,
            this._mniChart,
            this._mniBuild,
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
      // _mniChart
      // 
      this._mniChart.Name = "_mniChart";
      this._mniChart.Size = new System.Drawing.Size(46, 20);
      this._mniChart.Text = "Chart";
      // 
      // _mniBuild
      // 
      this._mniBuild.Name = "_mniBuild";
      this._mniBuild.Size = new System.Drawing.Size(41, 20);
      this._mniBuild.Text = "Build";
      // 
      // _mniHelp
      // 
      this._mniHelp.Name = "_mniHelp";
      this._mniHelp.Size = new System.Drawing.Size(40, 20);
      this._mniHelp.Text = "Help";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._stripStatus});
      this.statusStrip1.Location = new System.Drawing.Point(0, 401);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(614, 22);
      this.statusStrip1.TabIndex = 2;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // _stripStatus
      // 
      this._stripStatus.AutoSize = false;
      this._stripStatus.Name = "_stripStatus";
      this._stripStatus.Size = new System.Drawing.Size(150, 17);
      this._stripStatus.Text = "Ready";
      this._stripStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _tsStandart
      // 
      this._tsStandart.Location = new System.Drawing.Point(0, 24);
      this._tsStandart.Name = "_tsStandart";
      this._tsStandart.Size = new System.Drawing.Size(614, 25);
      this._tsStandart.TabIndex = 4;
      this._tsStandart.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(614, 423);
      this.Controls.Add(this._tsStandart);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this._menuStrip);
      this.IsMdiContainer = true;
      this.MainMenuStrip = this._menuStrip;
      this.Name = "MainForm";
      this.Text = "Gordago Forex Optimizer 2.8";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this._menuStrip.ResumeLayout(false);
      this._menuStrip.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip _menuStrip;
    private System.Windows.Forms.ToolStripMenuItem _mniFile;
    private System.Windows.Forms.ToolStripMenuItem _mniView;
    private System.Windows.Forms.ToolStripMenuItem _mniHelp;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel _stripStatus;
    private System.Windows.Forms.ToolStripMenuItem _mniChart;
    private System.Windows.Forms.ToolStripMenuItem _mniBuild;
    private System.Windows.Forms.ToolStrip _tsStandart;
  }
}

