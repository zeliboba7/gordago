namespace Gordago.Analysis.Chart {
  partial class SymbolsChartPanel {
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
      this._cntxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._mniNewChart = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this._mniHideSymbol = new System.Windows.Forms.ToolStripMenuItem();
      this._mniShowSymbol = new System.Windows.Forms.ToolStripMenuItem();
      this._mniHideAllSymbol = new System.Windows.Forms.ToolStripMenuItem();
      this._mniShowAllSymbol = new System.Windows.Forms.ToolStripMenuItem();
      this._cntxMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // _cntxMenu
      // 
      this._cntxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniNewChart,
            this.toolStripSeparator2,
            this._mniHideSymbol,
            this._mniShowSymbol,
            this._mniHideAllSymbol,
            this._mniShowAllSymbol});
      this._cntxMenu.Name = "_cntxMenu";
      this._cntxMenu.Size = new System.Drawing.Size(178, 142);
      this._cntxMenu.Opening += new System.ComponentModel.CancelEventHandler(this._cntxMenu_Opening);
      // 
      // _mniNewChart
      // 
      this._mniNewChart.Name = "_mniNewChart";
      this._mniNewChart.Size = new System.Drawing.Size(177, 22);
      this._mniNewChart.Text = "New Chart Window";
      this._mniNewChart.Click += new System.EventHandler(this._mniNewChart_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(174, 6);
      // 
      // _mniHideSymbol
      // 
      this._mniHideSymbol.Name = "_mniHideSymbol";
      this._mniHideSymbol.Size = new System.Drawing.Size(177, 22);
      this._mniHideSymbol.Text = "Hide";
      this._mniHideSymbol.Click += new System.EventHandler(this._mniHideSymbol_Click);
      // 
      // _mniShowSymbol
      // 
      this._mniShowSymbol.Name = "_mniShowSymbol";
      this._mniShowSymbol.Size = new System.Drawing.Size(177, 22);
      this._mniShowSymbol.Text = "Show";
      this._mniShowSymbol.Click += new System.EventHandler(this._mniShowIsSymbol_Click);
      // 
      // _mniHideAllSymbol
      // 
      this._mniHideAllSymbol.Name = "_mniHideAllSymbol";
      this._mniHideAllSymbol.Size = new System.Drawing.Size(177, 22);
      this._mniHideAllSymbol.Text = "Hide All";
      this._mniHideAllSymbol.Click += new System.EventHandler(this._mniHideAllSymbol_Click);
      // 
      // _mniShowAllSymbol
      // 
      this._mniShowAllSymbol.Name = "_mniShowAllSymbol";
      this._mniShowAllSymbol.Size = new System.Drawing.Size(177, 22);
      this._mniShowAllSymbol.Text = "Show All";
      this._mniShowAllSymbol.Click += new System.EventHandler(this._mniShowAllSymbol_Click);
      // 
      // SymbolsChartPanel
      // 
      this.ContextMenuStrip = this._cntxMenu;
      this.Name = "SymbolsChartPanel";
      this._cntxMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip _cntxMenu;
    private System.Windows.Forms.ToolStripMenuItem _mniNewChart;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem _mniHideSymbol;
    private System.Windows.Forms.ToolStripMenuItem _mniShowSymbol;
    private System.Windows.Forms.ToolStripMenuItem _mniHideAllSymbol;
    private System.Windows.Forms.ToolStripMenuItem _mniShowAllSymbol;
  }
}
