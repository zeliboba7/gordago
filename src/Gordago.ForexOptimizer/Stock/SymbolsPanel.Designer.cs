using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Cursit.Applic.AConfig;

namespace Gordago.Stock {
  public partial class SymbolsPanel {
    private ContextMenuStrip _cntxMenu;
    private IContainer components;
    private ToolStripMenuItem _mniHideSymbol;
    private ToolStripMenuItem _mniShowSymbol;
    private ToolStripMenuItem _mniShowAllSymbol;
    private ToolStripMenuItem _mniHideAllSymbol;
    private ToolStripMenuItem _mniNewChart;

    #region private void InitializeComponent()
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
      this._cntxMenu.Size = new System.Drawing.Size(154, 142);
      this._cntxMenu.Opening += new System.ComponentModel.CancelEventHandler(this._cntxMenu_Opening);
      // 
      // _mniNewChart
      // 
      this._mniNewChart.Name = "_mniNewChart";
      this._mniNewChart.Size = new System.Drawing.Size(153, 22);
      this._mniNewChart.Text = "Chart Window";
      this._mniNewChart.Click += new System.EventHandler(this._mniNewChart_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
      // 
      // _mniHideSymbol
      // 
      this._mniHideSymbol.Name = "_mniHideSymbol";
      this._mniHideSymbol.Size = new System.Drawing.Size(153, 22);
      this._mniHideSymbol.Text = "Hide";
      this._mniHideSymbol.Click += new System.EventHandler(this._mniHideSymbol_Click);
      // 
      // _mniShowSymbol
      // 
      this._mniShowSymbol.Name = "_mniShowSymbol";
      this._mniShowSymbol.Size = new System.Drawing.Size(153, 22);
      this._mniShowSymbol.Text = "Show";
      // 
      // _mniHideAllSymbol
      // 
      this._mniHideAllSymbol.Name = "_mniHideAllSymbol";
      this._mniHideAllSymbol.Size = new System.Drawing.Size(153, 22);
      this._mniHideAllSymbol.Text = "Hide All";
      this._mniHideAllSymbol.Click += new System.EventHandler(this._mniHideAllSymbol_Click);
      // 
      // _mniShowAllSymbol
      // 
      this._mniShowAllSymbol.Name = "_mniShowAllSymbol";
      this._mniShowAllSymbol.Size = new System.Drawing.Size(153, 22);
      this._mniShowAllSymbol.Text = "Show All";
      this._mniShowAllSymbol.Click += new System.EventHandler(this._mniShowAllSymbol_Click);
      // 
      // SymbolsPanel
      // 
      this.ContextMenuStrip = this._cntxMenu;
      this.Name = "SymbolsPanel";
      this.Size = new System.Drawing.Size(185, 300);
      this._cntxMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    private ToolStripSeparator toolStripSeparator2;


  }
}
