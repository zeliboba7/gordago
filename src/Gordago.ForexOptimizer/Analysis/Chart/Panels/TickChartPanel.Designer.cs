namespace Gordago.Analysis.Chart {
  partial class TickChartPanel {
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
      this._chartTicksManager = new Gordago.Analysis.Chart.ChartTicksManager();
      this.SuspendLayout();
      // 
      // _chartTicksManager
      // 
      this._chartTicksManager.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chartTicksManager.GridVisible = true;
      this._chartTicksManager.Location = new System.Drawing.Point(0, 0);
      this._chartTicksManager.Name = "_chartTicksManager";
      this._chartTicksManager.Size = new System.Drawing.Size(150, 150);
      this._chartTicksManager.Symbol = null;
      this._chartTicksManager.TabIndex = 0;
      this._chartTicksManager.Text = "chartTicksManager1";
      // 
      // TicksChartPanel
      // 
      this.Controls.Add(this._chartTicksManager);
      this.Name = "TicksChartPanel";
      this.ResumeLayout(false);

    }

    #endregion

    private ChartTicksManager _chartTicksManager;
  }
}
