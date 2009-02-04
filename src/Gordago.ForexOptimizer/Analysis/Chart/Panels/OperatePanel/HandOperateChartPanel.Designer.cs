namespace Gordago.API {
  partial class HandOperateChartPanel {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._gboxSlippage = new System.Windows.Forms.GroupBox();
      this._nudSlipPage = new System.Windows.Forms.NumericUpDown();
      this._lblSlipPage = new System.Windows.Forms.Label();
      this._lblLotsOperate = new System.Windows.Forms.Label();
      this._nudLots = new System.Windows.Forms.NumericUpDown();
      this._gboxPO = new System.Windows.Forms.GroupBox();
      this._nudPO = new System.Windows.Forms.NumericUpDown();
      this._chkPO = new System.Windows.Forms.CheckBox();
      this._gboxSL = new System.Windows.Forms.GroupBox();
      this._chkStop = new System.Windows.Forms.CheckBox();
      this._nudStop = new System.Windows.Forms.NumericUpDown();
      this._nudLimit = new System.Windows.Forms.NumericUpDown();
      this._chkLimit = new System.Windows.Forms.CheckBox();
      this._tlpButtons = new System.Windows.Forms.TableLayoutPanel();
      this._btnBuy = new Gordago.API.OperatePanel.HOCPActionButton();
      this._btnSell = new Gordago.API.OperatePanel.HOCPActionButton();
      this._lblask = new System.Windows.Forms.Label();
      this._lblBid = new System.Windows.Forms.Label();
      this._tlpLabels = new System.Windows.Forms.TableLayoutPanel();
      this._chkOneClick = new System.Windows.Forms.CheckBox();
      this._chkExtended = new System.Windows.Forms.CheckBox();
      this._lblWait = new System.Windows.Forms.Label();
      this._tprevSell = new Gordago.API.HandTradePreview();
      this._tprevBuy = new Gordago.API.HandTradePreview();
      this._gboxSlippage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudSlipPage)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLots)).BeginInit();
      this._gboxPO.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudPO)).BeginInit();
      this._gboxSL.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudStop)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimit)).BeginInit();
      this._tlpButtons.SuspendLayout();
      this._tlpLabels.SuspendLayout();
      this.SuspendLayout();
      // 
      // _gboxSlippage
      // 
      this._gboxSlippage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxSlippage.Controls.Add(this._nudSlipPage);
      this._gboxSlippage.Controls.Add(this._lblSlipPage);
      this._gboxSlippage.Location = new System.Drawing.Point(3, 139);
      this._gboxSlippage.Name = "_gboxSlippage";
      this._gboxSlippage.Size = new System.Drawing.Size(174, 33);
      this._gboxSlippage.TabIndex = 23;
      this._gboxSlippage.TabStop = false;
      // 
      // _nudSlipPage
      // 
      this._nudSlipPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._nudSlipPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudSlipPage.Location = new System.Drawing.Point(107, 10);
      this._nudSlipPage.Name = "_nudSlipPage";
      this._nudSlipPage.Size = new System.Drawing.Size(64, 20);
      this._nudSlipPage.TabIndex = 8;
      // 
      // _lblSlipPage
      // 
      this._lblSlipPage.AutoSize = true;
      this._lblSlipPage.Location = new System.Drawing.Point(2, 14);
      this._lblSlipPage.Name = "_lblSlipPage";
      this._lblSlipPage.Size = new System.Drawing.Size(48, 13);
      this._lblSlipPage.TabIndex = 9;
      this._lblSlipPage.Text = "Slippage";
      // 
      // _lblLotsOperate
      // 
      this._lblLotsOperate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblLotsOperate.AutoSize = true;
      this._lblLotsOperate.Location = new System.Drawing.Point(6, 100);
      this._lblLotsOperate.Name = "_lblLotsOperate";
      this._lblLotsOperate.Size = new System.Drawing.Size(27, 13);
      this._lblLotsOperate.TabIndex = 9;
      this._lblLotsOperate.Text = "Lots";
      // 
      // _nudLots
      // 
      this._nudLots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._nudLots.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLots.Location = new System.Drawing.Point(6, 116);
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
      // _gboxPO
      // 
      this._gboxPO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxPO.Controls.Add(this._nudPO);
      this._gboxPO.Controls.Add(this._chkPO);
      this._gboxPO.Location = new System.Drawing.Point(3, 167);
      this._gboxPO.Name = "_gboxPO";
      this._gboxPO.Size = new System.Drawing.Size(174, 33);
      this._gboxPO.TabIndex = 22;
      this._gboxPO.TabStop = false;
      // 
      // _nudPO
      // 
      this._nudPO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._nudPO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudPO.Enabled = false;
      this._nudPO.Location = new System.Drawing.Point(107, 9);
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
      // _gboxSL
      // 
      this._gboxSL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxSL.Controls.Add(this._chkStop);
      this._gboxSL.Controls.Add(this._nudStop);
      this._gboxSL.Controls.Add(this._nudLimit);
      this._gboxSL.Controls.Add(this._chkLimit);
      this._gboxSL.Location = new System.Drawing.Point(3, 195);
      this._gboxSL.Name = "_gboxSL";
      this._gboxSL.Size = new System.Drawing.Size(174, 56);
      this._gboxSL.TabIndex = 24;
      this._gboxSL.TabStop = false;
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
      this._nudStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._nudStop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudStop.Enabled = false;
      this._nudStop.Location = new System.Drawing.Point(107, 9);
      this._nudStop.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this._nudStop.Name = "_nudStop";
      this._nudStop.Size = new System.Drawing.Size(64, 20);
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
      this._nudLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._nudLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLimit.Enabled = false;
      this._nudLimit.Location = new System.Drawing.Point(107, 31);
      this._nudLimit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this._nudLimit.Name = "_nudLimit";
      this._nudLimit.Size = new System.Drawing.Size(65, 20);
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
      // _tlpButtons
      // 
      this._tlpButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tlpButtons.ColumnCount = 2;
      this._tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tlpButtons.Controls.Add(this._btnBuy, 1, 0);
      this._tlpButtons.Controls.Add(this._btnSell, 0, 0);
      this._tlpButtons.Location = new System.Drawing.Point(0, 14);
      this._tlpButtons.Margin = new System.Windows.Forms.Padding(1);
      this._tlpButtons.Name = "_tlpButtons";
      this._tlpButtons.RowCount = 1;
      this._tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tlpButtons.Size = new System.Drawing.Size(180, 78);
      this._tlpButtons.TabIndex = 27;
      // 
      // _btnBuy
      // 
      this._btnBuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(255)))), ((int)(((byte)(216)))));
      this._btnBuy.BigFont = new System.Drawing.Font("Microsoft Sans Serif", 22.69999F, System.Drawing.FontStyle.Bold);
      this._btnBuy.BorderColor = System.Drawing.Color.Black;
      this._btnBuy.Cursor = System.Windows.Forms.Cursors.Hand;
      this._btnBuy.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnBuy.Location = new System.Drawing.Point(90, 0);
      this._btnBuy.Margin = new System.Windows.Forms.Padding(0);
      this._btnBuy.Name = "_btnBuy";
      this._btnBuy.Price = 0.0001F;
      this._btnBuy.Size = new System.Drawing.Size(90, 78);
      this._btnBuy.SmallFont = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Bold);
      this._btnBuy.Symbol = null;
      this._btnBuy.TabIndex = 0;
      this._btnBuy.Click += new System.EventHandler(this._btnBuyOperate_Click);
      // 
      // _btnSell
      // 
      this._btnSell.BackColor = System.Drawing.Color.White;
      this._btnSell.BigFont = new System.Drawing.Font("Microsoft Sans Serif", 22.69999F, System.Drawing.FontStyle.Bold);
      this._btnSell.BorderColor = System.Drawing.Color.Black;
      this._btnSell.Cursor = System.Windows.Forms.Cursors.Hand;
      this._btnSell.Dock = System.Windows.Forms.DockStyle.Fill;
      this._btnSell.Location = new System.Drawing.Point(0, 0);
      this._btnSell.Margin = new System.Windows.Forms.Padding(0);
      this._btnSell.Name = "_btnSell";
      this._btnSell.Price = 0.0001F;
      this._btnSell.Size = new System.Drawing.Size(90, 78);
      this._btnSell.SmallFont = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Bold);
      this._btnSell.Symbol = null;
      this._btnSell.TabIndex = 0;
      this._btnSell.Click += new System.EventHandler(this._btnSellOperate_Click);
      // 
      // _lblask
      // 
      this._lblask.BackColor = System.Drawing.Color.Black;
      this._lblask.Dock = System.Windows.Forms.DockStyle.Fill;
      this._lblask.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblask.ForeColor = System.Drawing.Color.White;
      this._lblask.Location = new System.Drawing.Point(91, 0);
      this._lblask.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
      this._lblask.Name = "_lblask";
      this._lblask.Size = new System.Drawing.Size(89, 14);
      this._lblask.TabIndex = 26;
      this._lblask.Text = "Ask   .";
      this._lblask.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _lblBid
      // 
      this._lblBid.BackColor = System.Drawing.Color.Black;
      this._lblBid.Dock = System.Windows.Forms.DockStyle.Fill;
      this._lblBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblBid.ForeColor = System.Drawing.Color.White;
      this._lblBid.Location = new System.Drawing.Point(0, 0);
      this._lblBid.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
      this._lblBid.Name = "_lblBid";
      this._lblBid.Size = new System.Drawing.Size(89, 14);
      this._lblBid.TabIndex = 25;
      this._lblBid.Text = ".   Bid";
      this._lblBid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _tlpLabels
      // 
      this._tlpLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tlpLabels.ColumnCount = 2;
      this._tlpLabels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tlpLabels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tlpLabels.Controls.Add(this._lblask, 1, 0);
      this._tlpLabels.Controls.Add(this._lblBid, 0, 0);
      this._tlpLabels.Location = new System.Drawing.Point(0, 0);
      this._tlpLabels.Margin = new System.Windows.Forms.Padding(0);
      this._tlpLabels.Name = "_tlpLabels";
      this._tlpLabels.RowCount = 1;
      this._tlpLabels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this._tlpLabels.Size = new System.Drawing.Size(180, 14);
      this._tlpLabels.TabIndex = 28;
      // 
      // _chkOneClick
      // 
      this._chkOneClick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._chkOneClick.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._chkOneClick.Checked = true;
      this._chkOneClick.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkOneClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkOneClick.Location = new System.Drawing.Point(95, 94);
      this._chkOneClick.Name = "_chkOneClick";
      this._chkOneClick.Size = new System.Drawing.Size(78, 23);
      this._chkOneClick.TabIndex = 6;
      this._chkOneClick.Text = "OnClick";
      this._chkOneClick.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._chkOneClick.CheckedChanged += new System.EventHandler(this._chkStop_CheckedChanged);
      // 
      // _chkExtended
      // 
      this._chkExtended.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._chkExtended.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._chkExtended.Checked = true;
      this._chkExtended.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkExtended.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkExtended.Location = new System.Drawing.Point(94, 116);
      this._chkExtended.Name = "_chkExtended";
      this._chkExtended.Size = new System.Drawing.Size(78, 23);
      this._chkExtended.TabIndex = 6;
      this._chkExtended.Text = "Extended";
      this._chkExtended.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this._chkExtended.CheckedChanged += new System.EventHandler(this._chkExtended_CheckedChanged);
      // 
      // _lblWait
      // 
      this._lblWait.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblWait.BackColor = System.Drawing.Color.Red;
      this._lblWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._lblWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblWait.Location = new System.Drawing.Point(0, 0);
      this._lblWait.Name = "_lblWait";
      this._lblWait.Size = new System.Drawing.Size(180, 15);
      this._lblWait.TabIndex = 29;
      this._lblWait.Text = "Please, wait...";
      this._lblWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this._lblWait.Visible = false;
      // 
      // _tprevSell
      // 
      this._tprevSell.IsSellType = true;
      this._tprevSell.Location = new System.Drawing.Point(9, 101);
      this._tprevSell.Name = "_tprevSell";
      this._tprevSell.Size = new System.Drawing.Size(10, 10);
      this._tprevSell.TabIndex = 26;
      this._tprevSell.Visible = false;
      // 
      // _tprevBuy
      // 
      this._tprevBuy.Location = new System.Drawing.Point(47, 101);
      this._tprevBuy.Name = "_tprevBuy";
      this._tprevBuy.Size = new System.Drawing.Size(11, 10);
      this._tprevBuy.TabIndex = 27;
      this._tprevBuy.Visible = false;
      // 
      // HandOperateChartPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._lblWait);
      this.Controls.Add(this._nudLots);
      this.Controls.Add(this._tlpButtons);
      this.Controls.Add(this._chkExtended);
      this.Controls.Add(this._chkOneClick);
      this.Controls.Add(this._lblLotsOperate);
      this.Controls.Add(this._tprevSell);
      this.Controls.Add(this._tprevBuy);
      this.Controls.Add(this._tlpLabels);
      this.Controls.Add(this._gboxSlippage);
      this.Controls.Add(this._gboxPO);
      this.Controls.Add(this._gboxSL);
      this.Location = new System.Drawing.Point(50, 50);
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "HandOperateChartPanel";
      this.Size = new System.Drawing.Size(180, 254);
      this._gboxSlippage.ResumeLayout(false);
      this._gboxSlippage.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudSlipPage)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLots)).EndInit();
      this._gboxPO.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this._nudPO)).EndInit();
      this._gboxSL.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this._nudStop)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimit)).EndInit();
      this._tlpButtons.ResumeLayout(false);
      this._tlpLabels.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Gordago.API.OperatePanel.HOCPActionButton _btnSell;
    private Gordago.API.OperatePanel.HOCPActionButton _btnBuy;
    private System.Windows.Forms.GroupBox _gboxSlippage;
    private System.Windows.Forms.Label _lblLotsOperate;
    private System.Windows.Forms.NumericUpDown _nudLots;
    private System.Windows.Forms.NumericUpDown _nudSlipPage;
    private System.Windows.Forms.Label _lblSlipPage;
    private System.Windows.Forms.GroupBox _gboxPO;
    private System.Windows.Forms.NumericUpDown _nudPO;
    private System.Windows.Forms.CheckBox _chkPO;
    private System.Windows.Forms.GroupBox _gboxSL;
    private System.Windows.Forms.CheckBox _chkStop;
    private System.Windows.Forms.NumericUpDown _nudStop;
    private System.Windows.Forms.NumericUpDown _nudLimit;
    private System.Windows.Forms.CheckBox _chkLimit;
    private HandTradePreview _tprevSell;
    private HandTradePreview _tprevBuy;
    private System.Windows.Forms.TableLayoutPanel _tlpButtons;
    private System.Windows.Forms.Label _lblask;
    private System.Windows.Forms.Label _lblBid;
    private System.Windows.Forms.TableLayoutPanel _tlpLabels;
    private System.Windows.Forms.CheckBox _chkOneClick;
    private System.Windows.Forms.CheckBox _chkExtended;
    private System.Windows.Forms.Label _lblWait;

  }
}
