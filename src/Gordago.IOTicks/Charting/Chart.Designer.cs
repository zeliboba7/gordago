namespace Gordago.IOTicks.Charting {
  partial class Chart {
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
      this._chartControl = new Gordago.Trader.Charting.ChartControl();
      this.SuspendLayout();
      // 
      // _chartControl
      // 
      this._chartControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chartControl.Location = new System.Drawing.Point(0, 0);
      this._chartControl.Name = "_chartControl";
      this._chartControl.Size = new System.Drawing.Size(694, 530);
      // 
      // 
      // 
      this._chartControl.Styles.Default.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(247)))), ((int)(((byte)(202)))));
      this._chartControl.Styles.Default.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(0)))), ((int)(((byte)(211)))));
      this._chartControl.Styles.Default.BarDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(151)))), ((int)(((byte)(225)))));
      this._chartControl.Styles.Default.BarUpColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(100)))));
      this._chartControl.Styles.Default.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
      this._chartControl.Styles.Default.BuyOrderColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(175)))), ((int)(((byte)(255)))));
      this._chartControl.Styles.Default.BuyTradeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(175)))), ((int)(((byte)(255)))));
      this._chartControl.Styles.Default.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(205)))));
      this._chartControl.Styles.Default.LimitOrderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(38)))));
      this._chartControl.Styles.Default.LimitTradeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(38)))));
      this._chartControl.Styles.Default.Name = "Custom";
      this._chartControl.Styles.Default.ScaleFont = new System.Drawing.Font("Microsoft Sans Serif", 7F);
      this._chartControl.Styles.Default.ScaleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
      this._chartControl.Styles.Default.SellOrderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
      this._chartControl.Styles.Default.SellTradeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
      this._chartControl.Styles.Default.StopOrderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this._chartControl.Styles.Default.StopTradeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this._chartControl.Styles.Styles.Add(this._chartControl.Styles.Default);
      this._chartControl.TabIndex = 0;
      this._chartControl.Text = "chartControl1";
      // 
      // Chart
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(694, 530);
      this.Controls.Add(this._chartControl);
      this.Name = "Chart";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Chart";
      this.ResumeLayout(false);

    }

    #endregion

    private Gordago.Trader.Charting.ChartControl _chartControl;
  }
}