namespace Gordago.Strategy {
  partial class EditorForm {
    private System.ComponentModel.IContainer components = null;

    #region protected override void Dispose(bool disposing)
    protected override void Dispose(bool disposing) {
      if(disposing ) {
        if (components != null)
          components.Dispose();
        this.IsDestroy = true;
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
      this._tbcMain = new System.Windows.Forms.TabControl();
      this._tbpSell = new System.Windows.Forms.TabPage();
      this._splitSellMain = new System.Windows.Forms.SplitContainer();
      this._lblSellEnter = new Gordago.Strategy.LabelVertical();
      this._editEnterSell = new Gordago.Strategy.SEditorVariants();
      this._lblSellExit = new Gordago.Strategy.LabelVertical();
      this._editExitSell = new Gordago.Strategy.SEditorVariants();
      this._tbllauoutSLTSell = new System.Windows.Forms.TableLayoutPanel();
      this._chkpSStop = new Gordago.Strategy.CheckPeriod();
      this._chkpSLimit = new Gordago.Strategy.CheckPeriod();
      this._chkpSTrail = new Gordago.Strategy.CheckPeriod();
      this._tbpSellPO = new System.Windows.Forms.TabPage();
      this._splitSellPO = new System.Windows.Forms.SplitContainer();
      this.panel1 = new System.Windows.Forms.Panel();
      this._chkPOSellModify = new System.Windows.Forms.CheckBox();
      this.lblPOSellPrice = new Gordago.Strategy.LabelVertical();
      this._editPOSell = new Gordago.Strategy.SEditorVariants();
      this._lblPOSell = new Gordago.Strategy.LabelVertical();
      this._editPOPriceSell = new Gordago.Strategy.POPriceCaclulator();
      this._editPOSellDelete = new Gordago.Strategy.SEditorVariants();
      this._lblSellPODelete = new Gordago.Strategy.LabelVertical();
      this._tbpBuy = new System.Windows.Forms.TabPage();
      this._splitBuyMain = new System.Windows.Forms.SplitContainer();
      this._lblEnterBuy = new Gordago.Strategy.LabelVertical();
      this._editEnterBuy = new Gordago.Strategy.SEditorVariants();
      this._lblExitBuy = new Gordago.Strategy.LabelVertical();
      this._editExitBuy = new Gordago.Strategy.SEditorVariants();
      this._tbllayoutSLTBuy = new System.Windows.Forms.TableLayoutPanel();
      this._chkpBStop = new Gordago.Strategy.CheckPeriod();
      this._chkpBLimit = new Gordago.Strategy.CheckPeriod();
      this._chkpBTrail = new Gordago.Strategy.CheckPeriod();
      this._tbpBuyPO = new System.Windows.Forms.TabPage();
      this._splitPOBuy = new System.Windows.Forms.SplitContainer();
      this.panel2 = new System.Windows.Forms.Panel();
      this._chkPOBuyModify = new System.Windows.Forms.CheckBox();
      this._lblPOBuy = new Gordago.Strategy.LabelVertical();
      this._editPOBuy = new Gordago.Strategy.SEditorVariants();
      this._editPOPriceBuy = new Gordago.Strategy.POPriceCaclulator();
      this._lblPOBuyPrice = new Gordago.Strategy.LabelVertical();
      this._editPOBuyDelete = new Gordago.Strategy.SEditorVariants();
      this._lblPOBuyDelete = new Gordago.Strategy.LabelVertical();
      this._tbpAdditional = new System.Windows.Forms.TabPage();
      this._gbxDescription = new System.Windows.Forms.GroupBox();
      this._txtdesc = new System.Windows.Forms.RichTextBox();
      this._gbxName = new System.Windows.Forms.GroupBox();
      this._txtname = new System.Windows.Forms.TextBox();
      this._gbxSound = new System.Windows.Forms.GroupBox();
      this._btnplay = new System.Windows.Forms.Button();
      this._cmbsound = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.poPriceCaclulator1 = new Gordago.Strategy.POPriceCaclulator();
      this.labelVertical1 = new Gordago.Strategy.LabelVertical();
      this.sEditorVariants1 = new Gordago.Strategy.SEditorVariants();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.checkPeriod1 = new Gordago.Strategy.CheckPeriod();
      this.checkPeriod2 = new Gordago.Strategy.CheckPeriod();
      this._tbcMain.SuspendLayout();
      this._tbpSell.SuspendLayout();
      this._splitSellMain.Panel1.SuspendLayout();
      this._splitSellMain.Panel2.SuspendLayout();
      this._splitSellMain.SuspendLayout();
      this._tbllauoutSLTSell.SuspendLayout();
      this._tbpSellPO.SuspendLayout();
      this._splitSellPO.Panel1.SuspendLayout();
      this._splitSellPO.Panel2.SuspendLayout();
      this._splitSellPO.SuspendLayout();
      this.panel1.SuspendLayout();
      this._tbpBuy.SuspendLayout();
      this._splitBuyMain.Panel1.SuspendLayout();
      this._splitBuyMain.Panel2.SuspendLayout();
      this._splitBuyMain.SuspendLayout();
      this._tbllayoutSLTBuy.SuspendLayout();
      this._tbpBuyPO.SuspendLayout();
      this._splitPOBuy.Panel1.SuspendLayout();
      this._splitPOBuy.Panel2.SuspendLayout();
      this._splitPOBuy.SuspendLayout();
      this.panel2.SuspendLayout();
      this._tbpAdditional.SuspendLayout();
      this._gbxDescription.SuspendLayout();
      this._gbxName.SuspendLayout();
      this._gbxSound.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // _tbcMain
      // 
      this._tbcMain.Controls.Add(this._tbpSell);
      this._tbcMain.Controls.Add(this._tbpSellPO);
      this._tbcMain.Controls.Add(this._tbpBuy);
      this._tbcMain.Controls.Add(this._tbpBuyPO);
      this._tbcMain.Controls.Add(this._tbpAdditional);
      this._tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tbcMain.Location = new System.Drawing.Point(0, 0);
      this._tbcMain.Margin = new System.Windows.Forms.Padding(0);
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(526, 422);
      this._tbcMain.TabIndex = 0;
      this._tbcMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._tbcMain_DrawItem);
      this._tbcMain.SelectedIndexChanged += new System.EventHandler(this._tbcMain_SelectedIndexChanged);
      // 
      // _tbpSell
      // 
      this._tbpSell.Controls.Add(this._splitSellMain);
      this._tbpSell.Controls.Add(this._tbllauoutSLTSell);
      this._tbpSell.Location = new System.Drawing.Point(4, 22);
      this._tbpSell.Margin = new System.Windows.Forms.Padding(0);
      this._tbpSell.Name = "_tbpSell";
      this._tbpSell.Padding = new System.Windows.Forms.Padding(3);
      this._tbpSell.Size = new System.Drawing.Size(518, 396);
      this._tbpSell.TabIndex = 0;
      this._tbpSell.Text = "Sell";
      this._tbpSell.UseVisualStyleBackColor = true;
      // 
      // _splitSellMain
      // 
      this._splitSellMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._splitSellMain.BackColor = System.Drawing.Color.White;
      this._splitSellMain.Location = new System.Drawing.Point(0, 0);
      this._splitSellMain.Margin = new System.Windows.Forms.Padding(0);
      this._splitSellMain.Name = "_splitSellMain";
      this._splitSellMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitSellMain.Panel1
      // 
      this._splitSellMain.Panel1.Controls.Add(this._lblSellEnter);
      this._splitSellMain.Panel1.Controls.Add(this._editEnterSell);
      // 
      // _splitSellMain.Panel2
      // 
      this._splitSellMain.Panel2.Controls.Add(this._lblSellExit);
      this._splitSellMain.Panel2.Controls.Add(this._editExitSell);
      this._splitSellMain.Size = new System.Drawing.Size(518, 331);
      this._splitSellMain.SplitterDistance = 165;
      this._splitSellMain.TabIndex = 1;
      // 
      // _lblSellEnter
      // 
      this._lblSellEnter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblSellEnter.Location = new System.Drawing.Point(0, 0);
      this._lblSellEnter.Margin = new System.Windows.Forms.Padding(0);
      this._lblSellEnter.Name = "_lblSellEnter";
      this._lblSellEnter.Size = new System.Drawing.Size(18, 165);
      this._lblSellEnter.TabIndex = 1;
      this._lblSellEnter.Text = "Enter";
      // 
      // _editEnterSell
      // 
      this._editEnterSell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editEnterSell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editEnterSell.Location = new System.Drawing.Point(18, 0);
      this._editEnterSell.Margin = new System.Windows.Forms.Padding(0);
      this._editEnterSell.Name = "_editEnterSell";
      this._editEnterSell.Size = new System.Drawing.Size(500, 165);
      this._editEnterSell.TabIndex = 0;
      // 
      // _lblSellExit
      // 
      this._lblSellExit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblSellExit.Location = new System.Drawing.Point(0, 0);
      this._lblSellExit.Margin = new System.Windows.Forms.Padding(0);
      this._lblSellExit.Name = "_lblSellExit";
      this._lblSellExit.Size = new System.Drawing.Size(18, 162);
      this._lblSellExit.TabIndex = 1;
      this._lblSellExit.Text = "Exit";
      // 
      // _editExitSell
      // 
      this._editExitSell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editExitSell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editExitSell.Location = new System.Drawing.Point(18, 0);
      this._editExitSell.Margin = new System.Windows.Forms.Padding(0);
      this._editExitSell.Name = "_editExitSell";
      this._editExitSell.Size = new System.Drawing.Size(500, 162);
      this._editExitSell.TabIndex = 0;
      // 
      // _tbllauoutSLTSell
      // 
      this._tbllauoutSLTSell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbllauoutSLTSell.ColumnCount = 3;
      this._tbllauoutSLTSell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllauoutSLTSell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllauoutSLTSell.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllauoutSLTSell.Controls.Add(this._chkpSStop, 0, 0);
      this._tbllauoutSLTSell.Controls.Add(this._chkpSLimit, 1, 0);
      this._tbllauoutSLTSell.Controls.Add(this._chkpSTrail, 2, 0);
      this._tbllauoutSLTSell.Location = new System.Drawing.Point(0, 331);
      this._tbllauoutSLTSell.Margin = new System.Windows.Forms.Padding(0);
      this._tbllauoutSLTSell.Name = "_tbllauoutSLTSell";
      this._tbllauoutSLTSell.RowCount = 1;
      this._tbllauoutSLTSell.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._tbllauoutSLTSell.Size = new System.Drawing.Size(518, 65);
      this._tbllauoutSLTSell.TabIndex = 0;
      //_tbpSell.Controls.Add(this._chkpSStop);
      //_tbpSell.Controls.Add(this._chkpSLimit);
      //_tbpSell.Controls.Add(this._chkpSTrail);
      

      // 
      // _chkpSStop
      // 
      this._chkpSStop.Checked = false;
      this._chkpSStop.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chkpSStop.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpSStop.Location = new System.Drawing.Point(1, 1);
      this._chkpSStop.Margin = new System.Windows.Forms.Padding(1);
      this._chkpSStop.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._chkpSStop.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpSStop.Name = "_chkpSStop";
      this._chkpSStop.NumberBegin = 20;
      this._chkpSStop.NumberEnd = 20;
      this._chkpSStop.OptVar = null;
      this._chkpSStop.Size = new System.Drawing.Size(170, 63);
      this._chkpSStop.Step = 1;
      this._chkpSStop.TabIndex = 0;
      this._chkpSStop.TextPeriod = "Stop";
      // 
      // _chkpSLimit
      // 
      this._chkpSLimit.Checked = false;
      this._chkpSLimit.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chkpSLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpSLimit.Location = new System.Drawing.Point(173, 1);
      this._chkpSLimit.Margin = new System.Windows.Forms.Padding(1);
      this._chkpSLimit.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._chkpSLimit.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpSLimit.Name = "_chkpSLimit";
      this._chkpSLimit.NumberBegin = 50;
      this._chkpSLimit.NumberEnd = 50;
      this._chkpSLimit.OptVar = null;
      this._chkpSLimit.Size = new System.Drawing.Size(170, 63);
      this._chkpSLimit.Step = 1;
      this._chkpSLimit.TabIndex = 0;
      this._chkpSLimit.TextPeriod = "Limit";
      // 
      // _chkpSTrail
      // 
      this._chkpSTrail.Checked = false;
      this._chkpSTrail.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chkpSTrail.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpSTrail.Location = new System.Drawing.Point(345, 1);
      this._chkpSTrail.Margin = new System.Windows.Forms.Padding(1);
      this._chkpSTrail.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._chkpSTrail.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpSTrail.Name = "_chkpSTrail";
      this._chkpSTrail.NumberBegin = 15;
      this._chkpSTrail.NumberEnd = 15;
      this._chkpSTrail.OptVar = null;
      this._chkpSTrail.Size = new System.Drawing.Size(172, 63);
      this._chkpSTrail.Step = 1;
      this._chkpSTrail.TabIndex = 0;
      this._chkpSTrail.TextPeriod = "Trailing";
      // 
      // _tbpSellPO
      // 
      this._tbpSellPO.AutoScroll = true;
      this._tbpSellPO.Controls.Add(this._splitSellPO);
      this._tbpSellPO.Location = new System.Drawing.Point(4, 22);
      this._tbpSellPO.Name = "_tbpSellPO";
      this._tbpSellPO.Size = new System.Drawing.Size(518, 396);
      this._tbpSellPO.TabIndex = 3;
      this._tbpSellPO.Text = "PO Sell";
      // 
      // _splitSellPO
      // 
      this._splitSellPO.BackColor = System.Drawing.Color.White;
      this._splitSellPO.Dock = System.Windows.Forms.DockStyle.Fill;
      this._splitSellPO.Location = new System.Drawing.Point(0, 0);
      this._splitSellPO.Name = "_splitSellPO";
      this._splitSellPO.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitSellPO.Panel1
      // 
      this._splitSellPO.Panel1.Controls.Add(this.panel1);
      // 
      // _splitSellPO.Panel2
      // 
      this._splitSellPO.Panel2.Controls.Add(this._editPOSellDelete);
      this._splitSellPO.Panel2.Controls.Add(this._lblSellPODelete);
      this._splitSellPO.Size = new System.Drawing.Size(518, 396);
      this._splitSellPO.SplitterDistance = 210;
      this._splitSellPO.TabIndex = 6;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.SystemColors.Control;
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this._chkPOSellModify);
      this.panel1.Controls.Add(this.lblPOSellPrice);
      this.panel1.Controls.Add(this._editPOSell);
      this.panel1.Controls.Add(this._lblPOSell);
      this.panel1.Controls.Add(this._editPOPriceSell);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(518, 210);
      this.panel1.TabIndex = 6;
      // 
      // _chkPOSellModify
      // 
      this._chkPOSellModify.AutoSize = true;
      this._chkPOSellModify.Location = new System.Drawing.Point(3, 3);
      this._chkPOSellModify.Name = "_chkPOSellModify";
      this._chkPOSellModify.Size = new System.Drawing.Size(57, 17);
      this._chkPOSellModify.TabIndex = 6;
      this._chkPOSellModify.Text = "Modify";
      this._chkPOSellModify.UseVisualStyleBackColor = true;
      this._chkPOSellModify.CheckedChanged += new System.EventHandler(this._chkPOSellModify_CheckedChanged);
      // 
      // lblPOSellPrice
      // 
      this.lblPOSellPrice.BackColor = System.Drawing.Color.White;
      this.lblPOSellPrice.Location = new System.Drawing.Point(0, 23);
      this.lblPOSellPrice.Margin = new System.Windows.Forms.Padding(0);
      this.lblPOSellPrice.Name = "lblPOSellPrice";
      this.lblPOSellPrice.Size = new System.Drawing.Size(18, 44);
      this.lblPOSellPrice.TabIndex = 5;
      this.lblPOSellPrice.Text = "Price";
      // 
      // _editPOSell
      // 
      this._editPOSell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editPOSell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editPOSell.Location = new System.Drawing.Point(18, 67);
      this._editPOSell.Margin = new System.Windows.Forms.Padding(0);
      this._editPOSell.Name = "_editPOSell";
      this._editPOSell.Size = new System.Drawing.Size(498, 141);
      this._editPOSell.TabIndex = 2;
      // 
      // _lblPOSell
      // 
      this._lblPOSell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblPOSell.BackColor = System.Drawing.Color.White;
      this._lblPOSell.Location = new System.Drawing.Point(0, 67);
      this._lblPOSell.Margin = new System.Windows.Forms.Padding(0);
      this._lblPOSell.Name = "_lblPOSell";
      this._lblPOSell.Size = new System.Drawing.Size(18, 141);
      this._lblPOSell.TabIndex = 3;
      this._lblPOSell.Text = "Create or Modify";
      // 
      // _editPOPriceSell
      // 
      this._editPOPriceSell.AllowDrop = true;
      this._editPOPriceSell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editPOPriceSell.AutoScroll = true;
      this._editPOPriceSell.BackColor = System.Drawing.Color.White;
      this._editPOPriceSell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editPOPriceSell.Cursor = System.Windows.Forms.Cursors.IBeam;
      this._editPOPriceSell.Location = new System.Drawing.Point(18, 23);
      this._editPOPriceSell.Name = "_editPOPriceSell";
      this._editPOPriceSell.Size = new System.Drawing.Size(499, 44);
      this._editPOPriceSell.TabIndex = 4;
      this._editPOPriceSell.TimeFrameId = 0;
      // 
      // _editPOSellDelete
      // 
      this._editPOSellDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editPOSellDelete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editPOSellDelete.Location = new System.Drawing.Point(18, 0);
      this._editPOSellDelete.Margin = new System.Windows.Forms.Padding(0);
      this._editPOSellDelete.Name = "_editPOSellDelete";
      this._editPOSellDelete.Size = new System.Drawing.Size(501, 182);
      this._editPOSellDelete.TabIndex = 5;
      // 
      // _lblSellPODelete
      // 
      this._lblSellPODelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblSellPODelete.BackColor = System.Drawing.Color.White;
      this._lblSellPODelete.Location = new System.Drawing.Point(0, 0);
      this._lblSellPODelete.Margin = new System.Windows.Forms.Padding(0);
      this._lblSellPODelete.Name = "_lblSellPODelete";
      this._lblSellPODelete.Size = new System.Drawing.Size(18, 182);
      this._lblSellPODelete.TabIndex = 4;
      this._lblSellPODelete.Text = "Delete";
      // 
      // _tbpBuy
      // 
      this._tbpBuy.Controls.Add(this._splitBuyMain);
      this._tbpBuy.Controls.Add(this._tbllayoutSLTBuy);
      this._tbpBuy.Location = new System.Drawing.Point(4, 22);
      this._tbpBuy.Name = "_tbpBuy";
      this._tbpBuy.Padding = new System.Windows.Forms.Padding(3);
      this._tbpBuy.Size = new System.Drawing.Size(518, 396);
      this._tbpBuy.TabIndex = 1;
      this._tbpBuy.Text = "Buy";
      this._tbpBuy.UseVisualStyleBackColor = true;
      // 
      // _splitBuyMain
      // 
      this._splitBuyMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._splitBuyMain.BackColor = System.Drawing.Color.White;
      this._splitBuyMain.Location = new System.Drawing.Point(0, 0);
      this._splitBuyMain.Margin = new System.Windows.Forms.Padding(0);
      this._splitBuyMain.Name = "_splitBuyMain";
      this._splitBuyMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitBuyMain.Panel1
      // 
      this._splitBuyMain.Panel1.Controls.Add(this._lblEnterBuy);
      this._splitBuyMain.Panel1.Controls.Add(this._editEnterBuy);
      // 
      // _splitBuyMain.Panel2
      // 
      this._splitBuyMain.Panel2.Controls.Add(this._lblExitBuy);
      this._splitBuyMain.Panel2.Controls.Add(this._editExitBuy);
      this._splitBuyMain.Size = new System.Drawing.Size(518, 331);
      this._splitBuyMain.SplitterDistance = 165;
      this._splitBuyMain.TabIndex = 2;
      // 
      // _lblEnterBuy
      // 
      this._lblEnterBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblEnterBuy.Location = new System.Drawing.Point(0, 0);
      this._lblEnterBuy.Margin = new System.Windows.Forms.Padding(0);
      this._lblEnterBuy.Name = "_lblEnterBuy";
      this._lblEnterBuy.Size = new System.Drawing.Size(18, 165);
      this._lblEnterBuy.TabIndex = 1;
      this._lblEnterBuy.Text = "Enter";
      // 
      // _editEnterBuy
      // 
      this._editEnterBuy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editEnterBuy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editEnterBuy.Location = new System.Drawing.Point(18, 0);
      this._editEnterBuy.Margin = new System.Windows.Forms.Padding(0);
      this._editEnterBuy.Name = "_editEnterBuy";
      this._editEnterBuy.Size = new System.Drawing.Size(500, 165);
      this._editEnterBuy.TabIndex = 0;
      // 
      // _lblExitBuy
      // 
      this._lblExitBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblExitBuy.Location = new System.Drawing.Point(0, 0);
      this._lblExitBuy.Margin = new System.Windows.Forms.Padding(0);
      this._lblExitBuy.Name = "_lblExitBuy";
      this._lblExitBuy.Size = new System.Drawing.Size(18, 162);
      this._lblExitBuy.TabIndex = 1;
      this._lblExitBuy.Text = "Exit";
      // 
      // _editExitBuy
      // 
      this._editExitBuy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editExitBuy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editExitBuy.Location = new System.Drawing.Point(18, 0);
      this._editExitBuy.Margin = new System.Windows.Forms.Padding(0);
      this._editExitBuy.Name = "_editExitBuy";
      this._editExitBuy.Size = new System.Drawing.Size(500, 162);
      this._editExitBuy.TabIndex = 0;
      // 
      // _tbllayoutSLTBuy
      // 
      this._tbllayoutSLTBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbllayoutSLTBuy.ColumnCount = 3;
      this._tbllayoutSLTBuy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllayoutSLTBuy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllayoutSLTBuy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this._tbllayoutSLTBuy.Controls.Add(this._chkpBStop, 0, 0);
      this._tbllayoutSLTBuy.Controls.Add(this._chkpBLimit, 1, 0);
      this._tbllayoutSLTBuy.Controls.Add(this._chkpBTrail, 2, 0);
      this._tbllayoutSLTBuy.Location = new System.Drawing.Point(0, 331);
      this._tbllayoutSLTBuy.Margin = new System.Windows.Forms.Padding(0);
      this._tbllayoutSLTBuy.Name = "_tbllayoutSLTBuy";
      this._tbllayoutSLTBuy.RowCount = 1;
      this._tbllayoutSLTBuy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this._tbllayoutSLTBuy.Size = new System.Drawing.Size(518, 65);
      this._tbllayoutSLTBuy.TabIndex = 1;
      // 
      // _chkpBStop
      // 
      this._chkpBStop.Checked = false;
      this._chkpBStop.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chkpBStop.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpBStop.Location = new System.Drawing.Point(1, 1);
      this._chkpBStop.Margin = new System.Windows.Forms.Padding(1);
      this._chkpBStop.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._chkpBStop.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpBStop.Name = "_chkpBStop";
      this._chkpBStop.NumberBegin = 20;
      this._chkpBStop.NumberEnd = 20;
      this._chkpBStop.OptVar = null;
      this._chkpBStop.Size = new System.Drawing.Size(170, 63);
      this._chkpBStop.Step = 1;
      this._chkpBStop.TabIndex = 0;
      this._chkpBStop.TextPeriod = "Stop";
      // 
      // _chkpBLimit
      // 
      this._chkpBLimit.Checked = false;
      this._chkpBLimit.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chkpBLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpBLimit.Location = new System.Drawing.Point(173, 1);
      this._chkpBLimit.Margin = new System.Windows.Forms.Padding(1);
      this._chkpBLimit.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._chkpBLimit.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpBLimit.Name = "_chkpBLimit";
      this._chkpBLimit.NumberBegin = 50;
      this._chkpBLimit.NumberEnd = 50;
      this._chkpBLimit.OptVar = null;
      this._chkpBLimit.Size = new System.Drawing.Size(170, 63);
      this._chkpBLimit.Step = 1;
      this._chkpBLimit.TabIndex = 0;
      this._chkpBLimit.TextPeriod = "Limit";
      // 
      // _chkpBTrail
      // 
      this._chkpBTrail.Checked = false;
      this._chkpBTrail.Dock = System.Windows.Forms.DockStyle.Fill;
      this._chkpBTrail.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpBTrail.Location = new System.Drawing.Point(345, 1);
      this._chkpBTrail.Margin = new System.Windows.Forms.Padding(1);
      this._chkpBTrail.Max = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this._chkpBTrail.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._chkpBTrail.Name = "_chkpBTrail";
      this._chkpBTrail.NumberBegin = 15;
      this._chkpBTrail.NumberEnd = 15;
      this._chkpBTrail.OptVar = null;
      this._chkpBTrail.Size = new System.Drawing.Size(172, 63);
      this._chkpBTrail.Step = 1;
      this._chkpBTrail.TabIndex = 0;
      this._chkpBTrail.TextPeriod = "Trailing";
      // 
      // _tbpBuyPO
      // 
      this._tbpBuyPO.Controls.Add(this._splitPOBuy);
      this._tbpBuyPO.Location = new System.Drawing.Point(4, 22);
      this._tbpBuyPO.Name = "_tbpBuyPO";
      this._tbpBuyPO.Size = new System.Drawing.Size(518, 396);
      this._tbpBuyPO.TabIndex = 4;
      this._tbpBuyPO.Text = "PO Buy";
      this._tbpBuyPO.UseVisualStyleBackColor = true;
      // 
      // _splitPOBuy
      // 
      this._splitPOBuy.BackColor = System.Drawing.Color.White;
      this._splitPOBuy.Dock = System.Windows.Forms.DockStyle.Fill;
      this._splitPOBuy.Location = new System.Drawing.Point(0, 0);
      this._splitPOBuy.Name = "_splitPOBuy";
      this._splitPOBuy.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitPOBuy.Panel1
      // 
      this._splitPOBuy.Panel1.Controls.Add(this.panel2);
      // 
      // _splitPOBuy.Panel2
      // 
      this._splitPOBuy.Panel2.Controls.Add(this._editPOBuyDelete);
      this._splitPOBuy.Panel2.Controls.Add(this._lblPOBuyDelete);
      this._splitPOBuy.Size = new System.Drawing.Size(518, 396);
      this._splitPOBuy.SplitterDistance = 209;
      this._splitPOBuy.TabIndex = 11;
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.SystemColors.Control;
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this._chkPOBuyModify);
      this.panel2.Controls.Add(this._lblPOBuy);
      this.panel2.Controls.Add(this._editPOBuy);
      this.panel2.Controls.Add(this._editPOPriceBuy);
      this.panel2.Controls.Add(this._lblPOBuyPrice);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(518, 209);
      this.panel2.TabIndex = 11;
      // 
      // _chkPOBuyModify
      // 
      this._chkPOBuyModify.AutoSize = true;
      this._chkPOBuyModify.Location = new System.Drawing.Point(3, 3);
      this._chkPOBuyModify.Name = "_chkPOBuyModify";
      this._chkPOBuyModify.Size = new System.Drawing.Size(57, 17);
      this._chkPOBuyModify.TabIndex = 11;
      this._chkPOBuyModify.Text = "Modify";
      this._chkPOBuyModify.UseVisualStyleBackColor = true;
      this._chkPOBuyModify.CheckedChanged += new System.EventHandler(this._chkPOBuyModify_CheckedChanged);
      // 
      // _lblPOBuy
      // 
      this._lblPOBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblPOBuy.BackColor = System.Drawing.Color.White;
      this._lblPOBuy.Location = new System.Drawing.Point(0, 67);
      this._lblPOBuy.Margin = new System.Windows.Forms.Padding(0);
      this._lblPOBuy.Name = "_lblPOBuy";
      this._lblPOBuy.Size = new System.Drawing.Size(18, 140);
      this._lblPOBuy.TabIndex = 8;
      this._lblPOBuy.Text = "Create or Modify";
      // 
      // _editPOBuy
      // 
      this._editPOBuy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editPOBuy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editPOBuy.Location = new System.Drawing.Point(18, 67);
      this._editPOBuy.Margin = new System.Windows.Forms.Padding(0);
      this._editPOBuy.Name = "_editPOBuy";
      this._editPOBuy.Size = new System.Drawing.Size(498, 140);
      this._editPOBuy.TabIndex = 7;
      // 
      // _editPOPriceBuy
      // 
      this._editPOPriceBuy.AllowDrop = true;
      this._editPOPriceBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editPOPriceBuy.AutoScroll = true;
      this._editPOPriceBuy.BackColor = System.Drawing.Color.White;
      this._editPOPriceBuy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editPOPriceBuy.Cursor = System.Windows.Forms.Cursors.IBeam;
      this._editPOPriceBuy.Location = new System.Drawing.Point(18, 23);
      this._editPOPriceBuy.Name = "_editPOPriceBuy";
      this._editPOPriceBuy.Size = new System.Drawing.Size(498, 44);
      this._editPOPriceBuy.TabIndex = 9;
      this._editPOPriceBuy.TimeFrameId = 0;
      // 
      // _lblPOBuyPrice
      // 
      this._lblPOBuyPrice.BackColor = System.Drawing.Color.White;
      this._lblPOBuyPrice.Location = new System.Drawing.Point(0, 23);
      this._lblPOBuyPrice.Margin = new System.Windows.Forms.Padding(0);
      this._lblPOBuyPrice.Name = "_lblPOBuyPrice";
      this._lblPOBuyPrice.Size = new System.Drawing.Size(18, 44);
      this._lblPOBuyPrice.TabIndex = 10;
      this._lblPOBuyPrice.Text = "Price";
      // 
      // _editPOBuyDelete
      // 
      this._editPOBuyDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._editPOBuyDelete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._editPOBuyDelete.Location = new System.Drawing.Point(18, 0);
      this._editPOBuyDelete.Margin = new System.Windows.Forms.Padding(0);
      this._editPOBuyDelete.Name = "_editPOBuyDelete";
      this._editPOBuyDelete.Size = new System.Drawing.Size(500, 183);
      this._editPOBuyDelete.TabIndex = 7;
      // 
      // _lblPOBuyDelete
      // 
      this._lblPOBuyDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lblPOBuyDelete.BackColor = System.Drawing.Color.White;
      this._lblPOBuyDelete.Location = new System.Drawing.Point(0, 0);
      this._lblPOBuyDelete.Margin = new System.Windows.Forms.Padding(0);
      this._lblPOBuyDelete.Name = "_lblPOBuyDelete";
      this._lblPOBuyDelete.Size = new System.Drawing.Size(18, 183);
      this._lblPOBuyDelete.TabIndex = 6;
      this._lblPOBuyDelete.Text = "Delete";
      // 
      // _tbpAdditional
      // 
      this._tbpAdditional.Controls.Add(this._gbxDescription);
      this._tbpAdditional.Controls.Add(this._gbxName);
      this._tbpAdditional.Controls.Add(this._gbxSound);
      this._tbpAdditional.Location = new System.Drawing.Point(4, 22);
      this._tbpAdditional.Name = "_tbpAdditional";
      this._tbpAdditional.Size = new System.Drawing.Size(518, 396);
      this._tbpAdditional.TabIndex = 2;
      this._tbpAdditional.Text = "Additional";
      this._tbpAdditional.UseVisualStyleBackColor = true;
      // 
      // _gbxDescription
      // 
      this._gbxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gbxDescription.Controls.Add(this._txtdesc);
      this._gbxDescription.Location = new System.Drawing.Point(0, 48);
      this._gbxDescription.Name = "_gbxDescription";
      this._gbxDescription.Size = new System.Drawing.Size(518, 294);
      this._gbxDescription.TabIndex = 8;
      this._gbxDescription.TabStop = false;
      this._gbxDescription.Text = "Description";
      // 
      // _txtdesc
      // 
      this._txtdesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtdesc.Location = new System.Drawing.Point(8, 16);
      this._txtdesc.Name = "_txtdesc";
      this._txtdesc.Size = new System.Drawing.Size(502, 270);
      this._txtdesc.TabIndex = 0;
      this._txtdesc.Text = "";
      // 
      // _gbxName
      // 
      this._gbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gbxName.Controls.Add(this._txtname);
      this._gbxName.Location = new System.Drawing.Point(0, 0);
      this._gbxName.Name = "_gbxName";
      this._gbxName.Size = new System.Drawing.Size(518, 48);
      this._gbxName.TabIndex = 7;
      this._gbxName.TabStop = false;
      this._gbxName.Text = "Name";
      // 
      // _txtname
      // 
      this._txtname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._txtname.Location = new System.Drawing.Point(8, 16);
      this._txtname.Name = "_txtname";
      this._txtname.Size = new System.Drawing.Size(502, 20);
      this._txtname.TabIndex = 0;
      // 
      // _gbxSound
      // 
      this._gbxSound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gbxSound.Controls.Add(this._btnplay);
      this._gbxSound.Controls.Add(this._cmbsound);
      this._gbxSound.Location = new System.Drawing.Point(0, 348);
      this._gbxSound.Name = "_gbxSound";
      this._gbxSound.Size = new System.Drawing.Size(518, 48);
      this._gbxSound.TabIndex = 6;
      this._gbxSound.TabStop = false;
      this._gbxSound.Text = "Sound";
      // 
      // _btnplay
      // 
      this._btnplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnplay.Location = new System.Drawing.Point(184, 16);
      this._btnplay.Name = "_btnplay";
      this._btnplay.Size = new System.Drawing.Size(154, 23);
      this._btnplay.TabIndex = 2;
      this._btnplay.Text = "Play";
      this._btnplay.Click += new System.EventHandler(this._btnplay_Click);
      // 
      // _cmbsound
      // 
      this._cmbsound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbsound.Location = new System.Drawing.Point(8, 16);
      this._cmbsound.Name = "_cmbsound";
      this._cmbsound.Size = new System.Drawing.Size(165, 21);
      this._cmbsound.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.BackColor = System.Drawing.Color.White;
      this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label1.Location = new System.Drawing.Point(0, 262);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(517, 23);
      this.label1.TabIndex = 10;
      this.label1.Text = "Pending Order price";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // poPriceCaclulator1
      // 
      this.poPriceCaclulator1.AllowDrop = true;
      this.poPriceCaclulator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.poPriceCaclulator1.AutoScroll = true;
      this.poPriceCaclulator1.BackColor = System.Drawing.Color.White;
      this.poPriceCaclulator1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.poPriceCaclulator1.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.poPriceCaclulator1.Location = new System.Drawing.Point(0, 284);
      this.poPriceCaclulator1.Name = "poPriceCaclulator1";
      this.poPriceCaclulator1.Size = new System.Drawing.Size(517, 44);
      this.poPriceCaclulator1.TabIndex = 9;
      this.poPriceCaclulator1.TimeFrameId = 0;
      // 
      // labelVertical1
      // 
      this.labelVertical1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.labelVertical1.BackColor = System.Drawing.Color.White;
      this.labelVertical1.Location = new System.Drawing.Point(0, 0);
      this.labelVertical1.Margin = new System.Windows.Forms.Padding(0);
      this.labelVertical1.Name = "labelVertical1";
      this.labelVertical1.Size = new System.Drawing.Size(18, 255);
      this.labelVertical1.TabIndex = 8;
      this.labelVertical1.Text = "Create or Modify";
      // 
      // sEditorVariants1
      // 
      this.sEditorVariants1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.sEditorVariants1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.sEditorVariants1.Location = new System.Drawing.Point(18, 0);
      this.sEditorVariants1.Margin = new System.Windows.Forms.Padding(0);
      this.sEditorVariants1.Name = "sEditorVariants1";
      this.sEditorVariants1.Size = new System.Drawing.Size(500, 255);
      this.sEditorVariants1.TabIndex = 7;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel3.ColumnCount = 3;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel3.Controls.Add(this.checkPeriod1, 0, 0);
      this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 100);
      this.tableLayoutPanel3.TabIndex = 0;
      // 
      // checkPeriod1
      // 
      this.checkPeriod1.Checked = false;
      this.checkPeriod1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.checkPeriod1.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.checkPeriod1.Location = new System.Drawing.Point(1, 1);
      this.checkPeriod1.Margin = new System.Windows.Forms.Padding(1);
      this.checkPeriod1.Max = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.checkPeriod1.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
      this.checkPeriod1.Name = "checkPeriod1";
      this.checkPeriod1.NumberBegin = 0;
      this.checkPeriod1.NumberEnd = 0;
      this.checkPeriod1.OptVar = null;
      this.checkPeriod1.Size = new System.Drawing.Size(64, 98);
      this.checkPeriod1.Step = 1;
      this.checkPeriod1.TabIndex = 0;
      this.checkPeriod1.TextPeriod = "Stop";
      // 
      // checkPeriod2
      // 
      this.checkPeriod2.Checked = false;
      this.checkPeriod2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.checkPeriod2.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.checkPeriod2.Location = new System.Drawing.Point(67, 1);
      this.checkPeriod2.Margin = new System.Windows.Forms.Padding(1);
      this.checkPeriod2.Max = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.checkPeriod2.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
      this.checkPeriod2.Name = "checkPeriod2";
      this.checkPeriod2.NumberBegin = 0;
      this.checkPeriod2.NumberEnd = 0;
      this.checkPeriod2.OptVar = null;
      this.checkPeriod2.Size = new System.Drawing.Size(64, 98);
      this.checkPeriod2.Step = 1;
      this.checkPeriod2.TabIndex = 0;
      this.checkPeriod2.TextPeriod = "Limit";
      // 
      // EditorForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(526, 422);
      this.Controls.Add(this._tbcMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "EditorForm";
      this.ShowInTaskbar = false;
      this.Text = "EditorForm";
      this._tbcMain.ResumeLayout(false);
      this._tbpSell.ResumeLayout(false);
      this._splitSellMain.Panel1.ResumeLayout(false);
      this._splitSellMain.Panel2.ResumeLayout(false);
      this._splitSellMain.ResumeLayout(false);
      this._tbllauoutSLTSell.ResumeLayout(false);
      this._tbpSellPO.ResumeLayout(false);
      this._splitSellPO.Panel1.ResumeLayout(false);
      this._splitSellPO.Panel2.ResumeLayout(false);
      this._splitSellPO.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this._tbpBuy.ResumeLayout(false);
      this._splitBuyMain.Panel1.ResumeLayout(false);
      this._splitBuyMain.Panel2.ResumeLayout(false);
      this._splitBuyMain.ResumeLayout(false);
      this._tbllayoutSLTBuy.ResumeLayout(false);
      this._tbpBuyPO.ResumeLayout(false);
      this._splitPOBuy.Panel1.ResumeLayout(false);
      this._splitPOBuy.Panel2.ResumeLayout(false);
      this._splitPOBuy.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this._tbpAdditional.ResumeLayout(false);
      this._gbxDescription.ResumeLayout(false);
      this._gbxName.ResumeLayout(false);
      this._gbxName.PerformLayout();
      this._gbxSound.ResumeLayout(false);
      this.tableLayoutPanel3.ResumeLayout(false);
      this.ResumeLayout(false);

}

    #endregion

    private System.Windows.Forms.TabControl _tbcMain;
    private System.Windows.Forms.TabPage _tbpSell;
    private System.Windows.Forms.TabPage _tbpBuy;
    private System.Windows.Forms.TabPage _tbpAdditional;
    private System.Windows.Forms.TableLayoutPanel _tbllauoutSLTSell;
    private CheckPeriod _chkpSStop;
    private CheckPeriod _chkpSLimit;
    private CheckPeriod _chkpSTrail;
    private System.Windows.Forms.SplitContainer _splitSellMain;
    private SEditorVariants _editEnterSell;
    private SEditorVariants _editExitSell;
    private LabelVertical _lblSellEnter;
    private LabelVertical _lblSellExit;
    private System.Windows.Forms.SplitContainer _splitBuyMain;
    private LabelVertical _lblEnterBuy;
    private SEditorVariants _editEnterBuy;
    private LabelVertical _lblExitBuy;
    private SEditorVariants _editExitBuy;
    private System.Windows.Forms.TableLayoutPanel _tbllayoutSLTBuy;
    private CheckPeriod _chkpBStop;
    private CheckPeriod _chkpBLimit;
    private CheckPeriod _chkpBTrail;
    private System.Windows.Forms.GroupBox _gbxDescription;
    private System.Windows.Forms.RichTextBox _txtdesc;
    private System.Windows.Forms.GroupBox _gbxName;
    private System.Windows.Forms.TextBox _txtname;
    private System.Windows.Forms.GroupBox _gbxSound;
    private System.Windows.Forms.Button _btnplay;
    private System.Windows.Forms.ComboBox _cmbsound;
    private System.Windows.Forms.TabPage _tbpSellPO;
    private System.Windows.Forms.TabPage _tbpBuyPO;
    private LabelVertical _lblPOSell;
    private SEditorVariants _editPOSell;
    private POPriceCaclulator _editPOPriceSell;
    private POPriceCaclulator _editPOPriceBuy;
    private LabelVertical _lblPOBuy;
    private SEditorVariants _editPOBuy;
    private System.Windows.Forms.Label label1;
    private POPriceCaclulator poPriceCaclulator1;
    private LabelVertical labelVertical1;
    private SEditorVariants sEditorVariants1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private CheckPeriod checkPeriod1;
    private CheckPeriod checkPeriod2;
    private System.Windows.Forms.SplitContainer _splitSellPO;
    private LabelVertical _lblSellPODelete;
    private SEditorVariants _editPOSellDelete;
    private LabelVertical lblPOSellPrice;
    private System.Windows.Forms.SplitContainer _splitPOBuy;
    private LabelVertical _lblPOBuyPrice;
    private SEditorVariants _editPOBuyDelete;
    private LabelVertical _lblPOBuyDelete;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckBox _chkPOSellModify;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.CheckBox _chkPOBuyModify;
  }
}