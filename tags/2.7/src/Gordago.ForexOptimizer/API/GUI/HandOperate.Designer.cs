namespace Gordago.API {
  partial class HandOperate {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if(disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._btnBuyOperate = new System.Windows.Forms.Button();
      this._btnSellOperate = new System.Windows.Forms.Button();
      this._gboxAmountOperate = new System.Windows.Forms.GroupBox();
      this._lblLotsOperate = new System.Windows.Forms.Label();
      this._nudLots = new System.Windows.Forms.NumericUpDown();
      this._nudSlipPage = new System.Windows.Forms.NumericUpDown();
      this._lblSlipPage = new System.Windows.Forms.Label();
      this._gboxPriceOperate = new System.Windows.Forms.GroupBox();
      this._nudPO = new System.Windows.Forms.NumericUpDown();
      this._chkPO = new System.Windows.Forms.CheckBox();
      this._gboxSLTSTPOperate = new System.Windows.Forms.GroupBox();
      this._chkStop = new System.Windows.Forms.CheckBox();
      this._nudStop = new System.Windows.Forms.NumericUpDown();
      this._nudLimit = new System.Windows.Forms.NumericUpDown();
      this._chkLimit = new System.Windows.Forms.CheckBox();
      this._btnClose = new System.Windows.Forms.Button();
      this._tprevBuy = new Gordago.API.HandTradePreview();
      this._tprevSell = new Gordago.API.HandTradePreview();
      this._lblSymbolOperate = new System.Windows.Forms.Label();
      this._lblBidOperate = new System.Windows.Forms.Label();
      this._lblAskOperate = new System.Windows.Forms.Label();
      this._gboxAmountOperate.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudLots)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudSlipPage)).BeginInit();
      this._gboxPriceOperate.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudPO)).BeginInit();
      this._gboxSLTSTPOperate.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudStop)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimit)).BeginInit();
      this.SuspendLayout();
      // 
      // _btnBuyOperate
      // 
      this._btnBuyOperate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(224)))), ((int)(((byte)(221)))));
      this._btnBuyOperate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnBuyOperate.Location = new System.Drawing.Point(92, 22);
      this._btnBuyOperate.Name = "_btnBuyOperate";
      this._btnBuyOperate.Size = new System.Drawing.Size(80, 23);
      this._btnBuyOperate.TabIndex = 16;
      this._btnBuyOperate.Text = "Buy";
      this._btnBuyOperate.UseVisualStyleBackColor = false;
      this._btnBuyOperate.Click += new System.EventHandler(this._btnBuyOperate_Click);
      // 
      // _btnSellOperate
      // 
      this._btnSellOperate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(181)))), ((int)(((byte)(234)))));
      this._btnSellOperate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnSellOperate.Location = new System.Drawing.Point(3, 22);
      this._btnSellOperate.Name = "_btnSellOperate";
      this._btnSellOperate.Size = new System.Drawing.Size(80, 23);
      this._btnSellOperate.TabIndex = 15;
      this._btnSellOperate.Text = "Sell";
      this._btnSellOperate.UseVisualStyleBackColor = false;
      this._btnSellOperate.Click += new System.EventHandler(this._btnSellOperate_Click);
      // 
      // _gboxAmountOperate
      // 
      this._gboxAmountOperate.Controls.Add(this._lblLotsOperate);
      this._gboxAmountOperate.Controls.Add(this._nudLots);
      this._gboxAmountOperate.Controls.Add(this._nudSlipPage);
      this._gboxAmountOperate.Controls.Add(this._lblSlipPage);
      this._gboxAmountOperate.Location = new System.Drawing.Point(3, 112);
      this._gboxAmountOperate.Name = "_gboxAmountOperate";
      this._gboxAmountOperate.Size = new System.Drawing.Size(170, 52);
      this._gboxAmountOperate.TabIndex = 20;
      this._gboxAmountOperate.TabStop = false;
      // 
      // _lblLotsOperate
      // 
      this._lblLotsOperate.AutoSize = true;
      this._lblLotsOperate.Location = new System.Drawing.Point(8, 8);
      this._lblLotsOperate.Name = "_lblLotsOperate";
      this._lblLotsOperate.Size = new System.Drawing.Size(27, 13);
      this._lblLotsOperate.TabIndex = 9;
      this._lblLotsOperate.Text = "Lots";
      // 
      // _nudLots
      // 
      this._nudLots.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLots.Location = new System.Drawing.Point(11, 27);
      this._nudLots.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLots.Name = "_nudLots";
      this._nudLots.Size = new System.Drawing.Size(48, 20);
      this._nudLots.TabIndex = 8;
      this._nudLots.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLots.ValueChanged += new System.EventHandler(this._nudLots_ValueChanged);
      // 
      // _nudSlipPage
      // 
      this._nudSlipPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudSlipPage.Location = new System.Drawing.Point(108, 27);
      this._nudSlipPage.Name = "_nudSlipPage";
      this._nudSlipPage.Size = new System.Drawing.Size(55, 20);
      this._nudSlipPage.TabIndex = 8;
      // 
      // _lblSlipPage
      // 
      this._lblSlipPage.AutoSize = true;
      this._lblSlipPage.Location = new System.Drawing.Point(105, 8);
      this._lblSlipPage.Name = "_lblSlipPage";
      this._lblSlipPage.Size = new System.Drawing.Size(31, 13);
      this._lblSlipPage.TabIndex = 9;
      this._lblSlipPage.Text = "SlipP";
      // 
      // _gboxPriceOperate
      // 
      this._gboxPriceOperate.Controls.Add(this._nudPO);
      this._gboxPriceOperate.Controls.Add(this._chkPO);
      this._gboxPriceOperate.Location = new System.Drawing.Point(3, 159);
      this._gboxPriceOperate.Name = "_gboxPriceOperate";
      this._gboxPriceOperate.Size = new System.Drawing.Size(170, 33);
      this._gboxPriceOperate.TabIndex = 19;
      this._gboxPriceOperate.TabStop = false;
      // 
      // _nudPO
      // 
      this._nudPO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudPO.Enabled = false;
      this._nudPO.Location = new System.Drawing.Point(99, 9);
      this._nudPO.Name = "_nudPO";
      this._nudPO.Size = new System.Drawing.Size(64, 20);
      this._nudPO.TabIndex = 8;
      this._nudPO.ValueChanged += new System.EventHandler(this._nudPO_ValueChanged);
      // 
      // _chkPO
      // 
      this._chkPO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._chkPO.Location = new System.Drawing.Point(6, 10);
      this._chkPO.Name = "_chkPO";
      this._chkPO.Size = new System.Drawing.Size(106, 19);
      this._chkPO.TabIndex = 7;
      this._chkPO.Text = "Pending order";
      this._chkPO.CheckedChanged += new System.EventHandler(this._chkPO_CheckedChanged);
      // 
      // _gboxSLTSTPOperate
      // 
      this._gboxSLTSTPOperate.Controls.Add(this._chkStop);
      this._gboxSLTSTPOperate.Controls.Add(this._nudStop);
      this._gboxSLTSTPOperate.Controls.Add(this._nudLimit);
      this._gboxSLTSTPOperate.Controls.Add(this._chkLimit);
      this._gboxSLTSTPOperate.Location = new System.Drawing.Point(3, 187);
      this._gboxSLTSTPOperate.Name = "_gboxSLTSTPOperate";
      this._gboxSLTSTPOperate.Size = new System.Drawing.Size(170, 56);
      this._gboxSLTSTPOperate.TabIndex = 21;
      this._gboxSLTSTPOperate.TabStop = false;
      // 
      // _chkStop
      // 
      this._chkStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkStop.Location = new System.Drawing.Point(6, 9);
      this._chkStop.Name = "_chkStop";
      this._chkStop.Size = new System.Drawing.Size(82, 20);
      this._chkStop.TabIndex = 6;
      this._chkStop.Text = "Stop";
      this._chkStop.CheckedChanged += new System.EventHandler(this._chkStop_CheckedChanged);
      // 
      // _nudStop
      // 
      this._nudStop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudStop.Enabled = false;
      this._nudStop.Location = new System.Drawing.Point(107, 9);
      this._nudStop.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this._nudStop.Name = "_nudStop";
      this._nudStop.Size = new System.Drawing.Size(56, 20);
      this._nudStop.TabIndex = 8;
      this._nudStop.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
      this._nudStop.ValueChanged += new System.EventHandler(this._nudStop_ValueChanged);
      // 
      // _nudLimit
      // 
      this._nudLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLimit.Enabled = false;
      this._nudLimit.Location = new System.Drawing.Point(108, 31);
      this._nudLimit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this._nudLimit.Name = "_nudLimit";
      this._nudLimit.Size = new System.Drawing.Size(56, 20);
      this._nudLimit.TabIndex = 8;
      this._nudLimit.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this._nudLimit.ValueChanged += new System.EventHandler(this._nudLimit_ValueChanged);
      // 
      // _chkLimit
      // 
      this._chkLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkLimit.Location = new System.Drawing.Point(6, 31);
      this._chkLimit.Name = "_chkLimit";
      this._chkLimit.Size = new System.Drawing.Size(82, 20);
      this._chkLimit.TabIndex = 6;
      this._chkLimit.Text = "Limit";
      this._chkLimit.CheckedChanged += new System.EventHandler(this._chkLimit_CheckedChanged);
      // 
      // _btnClose
      // 
      this._btnClose.BackColor = System.Drawing.SystemColors.Control;
      this._btnClose.Image = global::Gordago.Properties.Resources.close_button;
      this._btnClose.Location = new System.Drawing.Point(157, 1);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(16, 16);
      this._btnClose.TabIndex = 24;
      this._btnClose.UseVisualStyleBackColor = false;
      // 
      // _tprevBuy
      // 
      this._tprevBuy.Location = new System.Drawing.Point(92, 46);
      this._tprevBuy.Name = "_tprevBuy";
      this._tprevBuy.Size = new System.Drawing.Size(80, 69);
      this._tprevBuy.TabIndex = 23;
      // 
      // _tprevSell
      // 
      this._tprevSell.IsSellType = true;
      this._tprevSell.Location = new System.Drawing.Point(3, 46);
      this._tprevSell.Name = "_tprevSell";
      this._tprevSell.Size = new System.Drawing.Size(80, 69);
      this._tprevSell.TabIndex = 22;
      // 
      // _lblSymbolOperate
      // 
      this._lblSymbolOperate.AutoSize = true;
      this._lblSymbolOperate.Location = new System.Drawing.Point(5, 3);
      this._lblSymbolOperate.Name = "_lblSymbolOperate";
      this._lblSymbolOperate.Size = new System.Drawing.Size(53, 13);
      this._lblSymbolOperate.TabIndex = 27;
      this._lblSymbolOperate.Text = "EURUSD";
      // 
      // _lblBidOperate
      // 
      this._lblBidOperate.AutoSize = true;
      this._lblBidOperate.Location = new System.Drawing.Point(67, 3);
      this._lblBidOperate.Name = "_lblBidOperate";
      this._lblBidOperate.Size = new System.Drawing.Size(22, 13);
      this._lblBidOperate.TabIndex = 26;
      this._lblBidOperate.Text = "0.0";
      // 
      // _lblAskOperate
      // 
      this._lblAskOperate.AutoSize = true;
      this._lblAskOperate.Location = new System.Drawing.Point(110, 3);
      this._lblAskOperate.Name = "_lblAskOperate";
      this._lblAskOperate.Size = new System.Drawing.Size(22, 13);
      this._lblAskOperate.TabIndex = 25;
      this._lblAskOperate.Text = "0.0";
      // 
      // HandOperateNew
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this._lblSymbolOperate);
      this.Controls.Add(this._lblBidOperate);
      this.Controls.Add(this._lblAskOperate);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._tprevBuy);
      this.Controls.Add(this._tprevSell);
      this.Controls.Add(this._gboxAmountOperate);
      this.Controls.Add(this._gboxPriceOperate);
      this.Controls.Add(this._gboxSLTSTPOperate);
      this.Controls.Add(this._btnBuyOperate);
      this.Controls.Add(this._btnSellOperate);
      this.Name = "HandOperateNew";
      this.Size = new System.Drawing.Size(176, 246);
      this._gboxAmountOperate.ResumeLayout(false);
      this._gboxAmountOperate.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudLots)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudSlipPage)).EndInit();
      this._gboxPriceOperate.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this._nudPO)).EndInit();
      this._gboxSLTSTPOperate.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this._nudStop)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimit)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button _btnBuyOperate;
    private System.Windows.Forms.Button _btnSellOperate;
    private System.Windows.Forms.GroupBox _gboxAmountOperate;
    private System.Windows.Forms.Label _lblLotsOperate;
    private System.Windows.Forms.NumericUpDown _nudLots;
    private System.Windows.Forms.NumericUpDown _nudSlipPage;
    private System.Windows.Forms.Label _lblSlipPage;
    private System.Windows.Forms.GroupBox _gboxPriceOperate;
    private System.Windows.Forms.CheckBox _chkPO;
    private System.Windows.Forms.NumericUpDown _nudPO;
    private System.Windows.Forms.GroupBox _gboxSLTSTPOperate;
    private System.Windows.Forms.CheckBox _chkStop;
    private System.Windows.Forms.NumericUpDown _nudStop;
    private System.Windows.Forms.NumericUpDown _nudLimit;
    private System.Windows.Forms.CheckBox _chkLimit;
    private HandTradePreview _tprevBuy;
    private HandTradePreview _tprevSell;
    private System.Windows.Forms.Button _btnClose;
    private System.Windows.Forms.Label _lblSymbolOperate;
    private System.Windows.Forms.Label _lblBidOperate;
    private System.Windows.Forms.Label _lblAskOperate;
  }
}
