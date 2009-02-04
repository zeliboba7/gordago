/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region Using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Cursit.Table;
using Gordago.Analysis.Chart;
using System.Collections.Generic;
#endregion

namespace Gordago.API {

	public class OrdersOperatePanel : UserControl, IBrokerEvents {
		#region private property
    private System.ComponentModel.Container components = null;

		private IOrder _order = null;
		private BrokerCommandManager _traderapi;

		#endregion

    public event EventHandler StatusChanged;

    private Cursit.TabControlExt _tbcMain;
    private TabPage _tbpDelete;
    private TabPage _tbpModify;
    private Label _lblLotsOrders;
    private Button _btnModifyOrders;
    private NumericUpDown _nudLotsModify;
    private Label _lblslp;
    private CheckNudPoint _cnpStopModify;
    private Label _lblPriceOrders;
    private CheckNudPoint _cnpLimitModify;
    private Label _lbltpp;
    private CheckBox _chkDepend;
    private NumericUpDown _nudRateModify;
    private Button _btnDeleteOrders;
    private Gordago.Analysis.Chart.ExtButton _btnMinMax;

		private int _lots;

    private Color _borderColor;
    private Pen _borderPen;
    private TabPage _tbpCreate;
    private string _text;
    private Cursit.PanelExt panelExt1;
    private CheckBox _chkStopCreate;
    private NumericUpDown _nudStopCreate;
    private Label _lblLotsCreate;
    private NumericUpDown _nudLimitCreate;
    private NumericUpDown _nudLotsCreate;
    private CheckBox _chkLimitCreate;
    private Cursit.LabelExt _lblCreateInfo;
    private Gordago.API.OperatePanel.HOCPActionButton _btnCreateBuy;
    private Gordago.API.OperatePanel.HOCPActionButton _btnCreateSell;
    private ComboBox _cmbSymbols;
    private NumericUpDown _nudRateCreate;
    private Font _font;

    private IOrder _savedOrder = null;
    private IOnlineRate _savedOnlineRate = null;
    private OrderCalculator _sellCalculator, _buyCalculator;
    private Cursit.LabelExt _lblModifyInfo;
    private Cursit.LabelExt _lblDeleteInfo;
    private Label _lblRateCreate;

		public OrdersOperatePanel() {
			InitializeComponent();
      this.BorderColor = Color.FromArgb(172, 168, 153);

      try {

        this._lblPriceOrders.Text = BrokerCommandManager.LNG_TBL_RATE;
        //this._lblDepend.Text = BrokerCommandManager.LNG_DEPEND;
        this._cnpStopModify.Caption = BrokerCommandManager.LNG_TBL_STOP;
        this._cnpLimitModify.Caption = BrokerCommandManager.LNG_TBL_LIMIT;
        this._btnDeleteOrders.Text = BrokerCommandManager.LNG_BTN_DELETE;
        this._btnModifyOrders.Text = BrokerCommandManager.LNG_BTN_MODIFY;

        this.Text = "Order Manager";
        this.FontVerticalText = new Font("Microsoft Sans Serif", 7);

        _sellCalculator = new OrderCalculator(TradeType.Sell);
        _buyCalculator = new OrderCalculator(TradeType.Buy);

        this._lblCreateInfo.Text =
          this._lblDeleteInfo.Text =
          this._lblModifyInfo.Text = "";

        this._btnCreateSell.Text = "PO Sell";
        this._btnCreateBuy.Text = "PO Buy";

        this.OnOrderChanged();
      } catch { 
      }
    }

    #region protected override void Dispose( bool disposing )
    protected override void Dispose(bool disposing) {
      if (disposing) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #endregion

    #region Component Designer generated code
    private void InitializeComponent() {
      this._tbcMain = new Cursit.TabControlExt();
      this._tbpDelete = new System.Windows.Forms.TabPage();
      this._lblDeleteInfo = new Cursit.LabelExt();
      this._btnDeleteOrders = new System.Windows.Forms.Button();
      this._tbpModify = new System.Windows.Forms.TabPage();
      this._lblModifyInfo = new Cursit.LabelExt();
      this._lblLotsOrders = new System.Windows.Forms.Label();
      this._btnModifyOrders = new System.Windows.Forms.Button();
      this._nudLotsModify = new System.Windows.Forms.NumericUpDown();
      this._lblslp = new System.Windows.Forms.Label();
      this._cnpStopModify = new Gordago.API.CheckNudPoint();
      this._lblPriceOrders = new System.Windows.Forms.Label();
      this._cnpLimitModify = new Gordago.API.CheckNudPoint();
      this._lbltpp = new System.Windows.Forms.Label();
      this._chkDepend = new System.Windows.Forms.CheckBox();
      this._nudRateModify = new System.Windows.Forms.NumericUpDown();
      this._tbpCreate = new System.Windows.Forms.TabPage();
      this.panelExt1 = new Cursit.PanelExt();
      this._nudRateCreate = new System.Windows.Forms.NumericUpDown();
      this._chkStopCreate = new System.Windows.Forms.CheckBox();
      this._nudStopCreate = new System.Windows.Forms.NumericUpDown();
      this._lblRateCreate = new System.Windows.Forms.Label();
      this._lblLotsCreate = new System.Windows.Forms.Label();
      this._nudLimitCreate = new System.Windows.Forms.NumericUpDown();
      this._nudLotsCreate = new System.Windows.Forms.NumericUpDown();
      this._chkLimitCreate = new System.Windows.Forms.CheckBox();
      this._lblCreateInfo = new Cursit.LabelExt();
      this._btnCreateBuy = new Gordago.API.OperatePanel.HOCPActionButton();
      this._btnCreateSell = new Gordago.API.OperatePanel.HOCPActionButton();
      this._cmbSymbols = new System.Windows.Forms.ComboBox();
      this._btnMinMax = new Gordago.Analysis.Chart.ExtButton();
      this._tbcMain.SuspendLayout();
      this._tbpDelete.SuspendLayout();
      this._tbpModify.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsModify)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudRateModify)).BeginInit();
      this._tbpCreate.SuspendLayout();
      this.panelExt1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudRateCreate)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudStopCreate)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimitCreate)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsCreate)).BeginInit();
      this.SuspendLayout();
      // 
      // _tbcMain
      // 
      this._tbcMain.Alignment = System.Windows.Forms.TabAlignment.Left;
      this._tbcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbcMain.BackColorTabPages = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
      this._tbcMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tbcMain.BorderVisible = true;
      this._tbcMain.Controls.Add(this._tbpDelete);
      this._tbcMain.Controls.Add(this._tbpModify);
      this._tbcMain.Controls.Add(this._tbpCreate);
      this._tbcMain.Location = new System.Drawing.Point(14, 0);
      this._tbcMain.Multiline = true;
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(286, 150);
      this._tbcMain.TabIndex = 24;
      this._tbcMain.SelectedIndexChanged += new System.EventHandler(this._tbcMain_SelectedIndexChanged);
      // 
      // _tbpDelete
      // 
      this._tbpDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
      this._tbpDelete.Controls.Add(this._lblDeleteInfo);
      this._tbpDelete.Controls.Add(this._btnDeleteOrders);
      this._tbpDelete.Location = new System.Drawing.Point(23, 4);
      this._tbpDelete.Name = "_tbpDelete";
      this._tbpDelete.Padding = new System.Windows.Forms.Padding(3);
      this._tbpDelete.Size = new System.Drawing.Size(259, 142);
      this._tbpDelete.TabIndex = 0;
      this._tbpDelete.Text = "Delete";
      // 
      // _lblDeleteInfo
      // 
      this._lblDeleteInfo.BackColor = System.Drawing.Color.White;
      this._lblDeleteInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._lblDeleteInfo.BorderVisible = true;
      this._lblDeleteInfo.Location = new System.Drawing.Point(6, 6);
      this._lblDeleteInfo.Name = "_lblDeleteInfo";
      this._lblDeleteInfo.Size = new System.Drawing.Size(247, 103);
      this._lblDeleteInfo.TabIndex = 6;
      this._lblDeleteInfo.Text = "Text";
      // 
      // _btnDeleteOrders
      // 
      this._btnDeleteOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._btnDeleteOrders.Enabled = false;
      this._btnDeleteOrders.Location = new System.Drawing.Point(3, 115);
      this._btnDeleteOrders.Name = "_btnDeleteOrders";
      this._btnDeleteOrders.Size = new System.Drawing.Size(253, 24);
      this._btnDeleteOrders.TabIndex = 5;
      this._btnDeleteOrders.Text = "Delete";
      this._btnDeleteOrders.UseVisualStyleBackColor = false;
      this._btnDeleteOrders.Click += new System.EventHandler(this._btnDeleteOrders_Click);
      // 
      // _tbpModify
      // 
      this._tbpModify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
      this._tbpModify.Controls.Add(this._lblModifyInfo);
      this._tbpModify.Controls.Add(this._lblLotsOrders);
      this._tbpModify.Controls.Add(this._btnModifyOrders);
      this._tbpModify.Controls.Add(this._nudLotsModify);
      this._tbpModify.Controls.Add(this._lblslp);
      this._tbpModify.Controls.Add(this._cnpStopModify);
      this._tbpModify.Controls.Add(this._lblPriceOrders);
      this._tbpModify.Controls.Add(this._cnpLimitModify);
      this._tbpModify.Controls.Add(this._lbltpp);
      this._tbpModify.Controls.Add(this._chkDepend);
      this._tbpModify.Controls.Add(this._nudRateModify);
      this._tbpModify.Location = new System.Drawing.Point(23, 4);
      this._tbpModify.Name = "_tbpModify";
      this._tbpModify.Padding = new System.Windows.Forms.Padding(3);
      this._tbpModify.Size = new System.Drawing.Size(259, 142);
      this._tbpModify.TabIndex = 1;
      this._tbpModify.Text = "Modify";
//      this._tbpModify.Click += new System.EventHandler(this._btnModifyOrders_Click);
      // 
      // _lblModifyInfo
      // 
      this._lblModifyInfo.BackColor = System.Drawing.Color.White;
      this._lblModifyInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._lblModifyInfo.BorderVisible = true;
      this._lblModifyInfo.Location = new System.Drawing.Point(75, 4);
      this._lblModifyInfo.Name = "_lblModifyInfo";
      this._lblModifyInfo.Size = new System.Drawing.Size(178, 59);
      this._lblModifyInfo.TabIndex = 26;
      this._lblModifyInfo.Text = "Text";
      // 
      // _lblLotsOrders
      // 
      this._lblLotsOrders.AutoSize = true;
      this._lblLotsOrders.Location = new System.Drawing.Point(6, 5);
      this._lblLotsOrders.Name = "_lblLotsOrders";
      this._lblLotsOrders.Size = new System.Drawing.Size(27, 13);
      this._lblLotsOrders.TabIndex = 20;
      this._lblLotsOrders.Text = "Lots";
      this._lblLotsOrders.Visible = false;
      // 
      // _btnModifyOrders
      // 
      this._btnModifyOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._btnModifyOrders.Enabled = false;
      this._btnModifyOrders.Location = new System.Drawing.Point(3, 115);
      this._btnModifyOrders.Name = "_btnModifyOrders";
      this._btnModifyOrders.Size = new System.Drawing.Size(253, 24);
      this._btnModifyOrders.TabIndex = 16;
      this._btnModifyOrders.Text = "Modify";
      this._btnModifyOrders.UseVisualStyleBackColor = false;
      this._btnModifyOrders.Click += new System.EventHandler(this._btnModifyOrders_Click);
      // 
      // _nudLotsModify
      // 
      this._nudLotsModify.Enabled = false;
      this._nudLotsModify.Location = new System.Drawing.Point(6, 21);
      this._nudLotsModify.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLotsModify.Name = "_nudLotsModify";
      this._nudLotsModify.Size = new System.Drawing.Size(58, 20);
      this._nudLotsModify.TabIndex = 18;
      this._nudLotsModify.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLotsModify.ValueChanged += new System.EventHandler(this._nudLotsModify_ValueChanged);
      // 
      // _lblslp
      // 
      this._lblslp.AutoSize = true;
      this._lblslp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblslp.Location = new System.Drawing.Point(140, 89);
      this._lblslp.Name = "_lblslp";
      this._lblslp.Size = new System.Drawing.Size(21, 15);
      this._lblslp.TabIndex = 23;
      this._lblslp.Text = "=0";
      // 
      // _cnpStopModify
      // 
      this._cnpStopModify.Caption = "Stop";
      this._cnpStopModify.Checked = false;
      this._cnpStopModify.DecimalDigits = 0;
      this._cnpStopModify.Location = new System.Drawing.Point(77, 69);
      this._cnpStopModify.Maximum = 100F;
      this._cnpStopModify.Minimum = 0F;
      this._cnpStopModify.Name = "_cnpStopModify";
      this._cnpStopModify.Size = new System.Drawing.Size(62, 40);
      this._cnpStopModify.TabIndex = 22;
      this._cnpStopModify.Value = float.NaN;
      this._cnpStopModify.ValueChanged += new System.EventHandler(this._cnpSL_ValueChanged);
      this._cnpStopModify.CheckedChanged += new System.EventHandler(this._cnpSL_CheckedChanged);
      // 
      // _lblPriceOrders
      // 
      this._lblPriceOrders.AutoSize = true;
      this._lblPriceOrders.Location = new System.Drawing.Point(6, 45);
      this._lblPriceOrders.Name = "_lblPriceOrders";
      this._lblPriceOrders.Size = new System.Drawing.Size(31, 13);
      this._lblPriceOrders.TabIndex = 19;
      this._lblPriceOrders.Text = "Price";
      // 
      // _cnpLimitModify
      // 
      this._cnpLimitModify.Caption = "Limit";
      this._cnpLimitModify.Checked = false;
      this._cnpLimitModify.DecimalDigits = 0;
      this._cnpLimitModify.Location = new System.Drawing.Point(170, 68);
      this._cnpLimitModify.Maximum = 100F;
      this._cnpLimitModify.Minimum = 0F;
      this._cnpLimitModify.Name = "_cnpLimitModify";
      this._cnpLimitModify.Size = new System.Drawing.Size(62, 40);
      this._cnpLimitModify.TabIndex = 21;
      this._cnpLimitModify.Value = float.NaN;
      this._cnpLimitModify.ValueChanged += new System.EventHandler(this._cnpTP_ValueChanged);
      this._cnpLimitModify.CheckedChanged += new System.EventHandler(this._cnpTP_CheckedChanged);
      // 
      // _lbltpp
      // 
      this._lbltpp.AutoSize = true;
      this._lbltpp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lbltpp.Location = new System.Drawing.Point(231, 89);
      this._lbltpp.Name = "_lbltpp";
      this._lbltpp.Size = new System.Drawing.Size(21, 15);
      this._lbltpp.TabIndex = 24;
      this._lbltpp.Text = "=0";
      // 
      // _chkDepend
      // 
      this._chkDepend.AutoSize = true;
      this._chkDepend.Checked = true;
      this._chkDepend.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkDepend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkDepend.Location = new System.Drawing.Point(6, 89);
      this._chkDepend.Name = "_chkDepend";
      this._chkDepend.Size = new System.Drawing.Size(61, 17);
      this._chkDepend.TabIndex = 25;
      this._chkDepend.Text = "Depend";
      this._chkDepend.CheckedChanged += new System.EventHandler(this._chkDepend_CheckedChanged);
      // 
      // _nudRateModify
      // 
      this._nudRateModify.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudRateModify.Enabled = false;
      this._nudRateModify.Location = new System.Drawing.Point(6, 65);
      this._nudRateModify.Name = "_nudRateModify";
      this._nudRateModify.Size = new System.Drawing.Size(58, 20);
      this._nudRateModify.TabIndex = 17;
      this._nudRateModify.ValueChanged += new System.EventHandler(this._nudPriceOrders_ValueChanged);
      // 
      // _tbpCreate
      // 
      this._tbpCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
      this._tbpCreate.Controls.Add(this.panelExt1);
      this._tbpCreate.Controls.Add(this._lblCreateInfo);
      this._tbpCreate.Controls.Add(this._btnCreateBuy);
      this._tbpCreate.Controls.Add(this._btnCreateSell);
      this._tbpCreate.Controls.Add(this._cmbSymbols);
      this._tbpCreate.Location = new System.Drawing.Point(23, 4);
      this._tbpCreate.Name = "_tbpCreate";
      this._tbpCreate.Size = new System.Drawing.Size(259, 142);
      this._tbpCreate.TabIndex = 2;
      this._tbpCreate.Text = "Create";
      // 
      // panelExt1
      // 
      this.panelExt1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelExt1.BackColor = System.Drawing.Color.White;
      this.panelExt1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this.panelExt1.BorderVisible = true;
      this.panelExt1.Controls.Add(this._nudRateCreate);
      this.panelExt1.Controls.Add(this._chkStopCreate);
      this.panelExt1.Controls.Add(this._nudStopCreate);
      this.panelExt1.Controls.Add(this._lblRateCreate);
      this.panelExt1.Controls.Add(this._lblLotsCreate);
      this.panelExt1.Controls.Add(this._nudLimitCreate);
      this.panelExt1.Controls.Add(this._nudLotsCreate);
      this.panelExt1.Controls.Add(this._chkLimitCreate);
      this.panelExt1.Location = new System.Drawing.Point(3, 88);
      this.panelExt1.Name = "panelExt1";
      this.panelExt1.Size = new System.Drawing.Size(253, 54);
      this.panelExt1.TabIndex = 45;
      // 
      // _nudRateCreate
      // 
      this._nudRateCreate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudRateCreate.Enabled = false;
      this._nudRateCreate.Location = new System.Drawing.Point(56, 5);
      this._nudRateCreate.Name = "_nudRateCreate";
      this._nudRateCreate.Size = new System.Drawing.Size(56, 20);
      this._nudRateCreate.TabIndex = 46;
      this._nudRateCreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudRateCreate.ValueChanged += new System.EventHandler(this._nudRateCreate_ValueChanged);
      // 
      // _chkStopCreate
      // 
      this._chkStopCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._chkStopCreate.Checked = true;
      this._chkStopCreate.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkStopCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkStopCreate.Location = new System.Drawing.Point(130, 5);
      this._chkStopCreate.Name = "_chkStopCreate";
      this._chkStopCreate.Size = new System.Drawing.Size(62, 20);
      this._chkStopCreate.TabIndex = 42;
      this._chkStopCreate.Text = "Stop";
      this._chkStopCreate.CheckedChanged += new System.EventHandler(this._chkStopCreate_CheckedChanged);
      // 
      // _nudStopCreate
      // 
      this._nudStopCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._nudStopCreate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudStopCreate.Enabled = false;
      this._nudStopCreate.Location = new System.Drawing.Point(200, 5);
      this._nudStopCreate.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this._nudStopCreate.Name = "_nudStopCreate";
      this._nudStopCreate.Size = new System.Drawing.Size(49, 20);
      this._nudStopCreate.TabIndex = 43;
      this._nudStopCreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudStopCreate.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this._nudStopCreate.ValueChanged += new System.EventHandler(this._nudStopCreate_ValueChanged);
      // 
      // _lblRateCreate
      // 
      this._lblRateCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblRateCreate.AutoSize = true;
      this._lblRateCreate.Location = new System.Drawing.Point(3, 9);
      this._lblRateCreate.Name = "_lblRateCreate";
      this._lblRateCreate.Size = new System.Drawing.Size(30, 13);
      this._lblRateCreate.TabIndex = 38;
      this._lblRateCreate.Text = "Rate";
      // 
      // _lblLotsCreate
      // 
      this._lblLotsCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblLotsCreate.AutoSize = true;
      this._lblLotsCreate.Location = new System.Drawing.Point(3, 32);
      this._lblLotsCreate.Name = "_lblLotsCreate";
      this._lblLotsCreate.Size = new System.Drawing.Size(27, 13);
      this._lblLotsCreate.TabIndex = 38;
      this._lblLotsCreate.Text = "Lots";
      // 
      // _nudLimitCreate
      // 
      this._nudLimitCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._nudLimitCreate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLimitCreate.Enabled = false;
      this._nudLimitCreate.Location = new System.Drawing.Point(200, 28);
      this._nudLimitCreate.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this._nudLimitCreate.Name = "_nudLimitCreate";
      this._nudLimitCreate.Size = new System.Drawing.Size(49, 20);
      this._nudLimitCreate.TabIndex = 44;
      this._nudLimitCreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudLimitCreate.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this._nudLimitCreate.ValueChanged += new System.EventHandler(this._nudLimitCreate_ValueChanged);
      // 
      // _nudLotsCreate
      // 
      this._nudLotsCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._nudLotsCreate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._nudLotsCreate.Location = new System.Drawing.Point(56, 28);
      this._nudLotsCreate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLotsCreate.Name = "_nudLotsCreate";
      this._nudLotsCreate.Size = new System.Drawing.Size(56, 20);
      this._nudLotsCreate.TabIndex = 37;
      this._nudLotsCreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this._nudLotsCreate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this._nudLotsCreate.ValueChanged += new System.EventHandler(this._nudLotsCreate_ValueChanged);
      // 
      // _chkLimitCreate
      // 
      this._chkLimitCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._chkLimitCreate.Checked = true;
      this._chkLimitCreate.CheckState = System.Windows.Forms.CheckState.Checked;
      this._chkLimitCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._chkLimitCreate.Location = new System.Drawing.Point(130, 28);
      this._chkLimitCreate.Name = "_chkLimitCreate";
      this._chkLimitCreate.Size = new System.Drawing.Size(62, 20);
      this._chkLimitCreate.TabIndex = 41;
      this._chkLimitCreate.Text = "Limit";
      this._chkLimitCreate.CheckedChanged += new System.EventHandler(this._chkLimitCreate_CheckedChanged);
      // 
      // _lblCreateInfo
      // 
      this._lblCreateInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblCreateInfo.BackColor = System.Drawing.Color.White;
      this._lblCreateInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this._lblCreateInfo.BorderVisible = true;
      this._lblCreateInfo.Location = new System.Drawing.Point(162, 24);
      this._lblCreateInfo.Name = "_lblCreateInfo";
      this._lblCreateInfo.Size = new System.Drawing.Size(94, 62);
      this._lblCreateInfo.TabIndex = 44;
      this._lblCreateInfo.Text = "Text";
      // 
      // _btnCreateBuy
      // 
      this._btnCreateBuy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._btnCreateBuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(255)))), ((int)(((byte)(216)))));
      this._btnCreateBuy.BigFont = new System.Drawing.Font("Microsoft Sans Serif", 23.2F, System.Drawing.FontStyle.Bold);
      this._btnCreateBuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this._btnCreateBuy.Cursor = System.Windows.Forms.Cursors.Hand;
      this._btnCreateBuy.Location = new System.Drawing.Point(81, 0);
      this._btnCreateBuy.Margin = new System.Windows.Forms.Padding(0);
      this._btnCreateBuy.Name = "_btnCreateBuy";
      this._btnCreateBuy.Price = 0.0001F;
      this._btnCreateBuy.Size = new System.Drawing.Size(78, 85);
      this._btnCreateBuy.SmallFont = new System.Drawing.Font("Microsoft Sans Serif", 14.3F, System.Drawing.FontStyle.Bold);
      this._btnCreateBuy.Symbol = null;
      this._btnCreateBuy.TabIndex = 43;
      this._btnCreateBuy.UseToolTip = false;
      this._btnCreateBuy.Click += new System.EventHandler(this._btnCreateBuy_Click);
      this._btnCreateBuy.MouseMove += new System.Windows.Forms.MouseEventHandler(this._btnCreateBuy_MouseMove);
      this._btnCreateBuy.MouseEnter += new System.EventHandler(this._btnCreateBuy_MouseEnter);
      this._btnCreateBuy.MouseLeave += new System.EventHandler(this._btnCreateBuy_MouseLeave);
      // 
      // _btnCreateSell
      // 
      this._btnCreateSell.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._btnCreateSell.BackColor = System.Drawing.Color.White;
      this._btnCreateSell.BigFont = new System.Drawing.Font("Microsoft Sans Serif", 23.2F, System.Drawing.FontStyle.Bold);
      this._btnCreateSell.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
      this._btnCreateSell.Cursor = System.Windows.Forms.Cursors.Hand;
      this._btnCreateSell.Location = new System.Drawing.Point(3, 0);
      this._btnCreateSell.Margin = new System.Windows.Forms.Padding(0);
      this._btnCreateSell.Name = "_btnCreateSell";
      this._btnCreateSell.Price = 0.0001F;
      this._btnCreateSell.Size = new System.Drawing.Size(78, 85);
      this._btnCreateSell.SmallFont = new System.Drawing.Font("Microsoft Sans Serif", 14.3F, System.Drawing.FontStyle.Bold);
      this._btnCreateSell.Symbol = null;
      this._btnCreateSell.TabIndex = 42;
      this._btnCreateSell.UseToolTip = false;
      this._btnCreateSell.Click += new System.EventHandler(this._btnCreateSell_Click);
      this._btnCreateSell.MouseMove += new System.Windows.Forms.MouseEventHandler(this._btnCreateSell_MouseMove);
      this._btnCreateSell.MouseEnter += new System.EventHandler(this._btnCreateSell_MouseEnter);
      this._btnCreateSell.MouseLeave += new System.EventHandler(this._btnCreateSell_MouseLeave);
      // 
      // _cmbSymbols
      // 
      this._cmbSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._cmbSymbols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbSymbols.FormattingEnabled = true;
      this._cmbSymbols.Location = new System.Drawing.Point(162, 0);
      this._cmbSymbols.Name = "_cmbSymbols";
      this._cmbSymbols.Size = new System.Drawing.Size(94, 21);
      this._cmbSymbols.TabIndex = 41;
      this._cmbSymbols.SelectedIndexChanged += new System.EventHandler(this._cmbSymbols_SelectedIndexChanged);
      // 
      // _btnMinMax
      // 
      this._btnMinMax.Alignment = Gordago.Analysis.Chart.ExtButton.ExtButtonAlignment.Left;
      this._btnMinMax.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._btnMinMax.BorderVisible = true;
      this._btnMinMax.Checked = true;
      this._btnMinMax.ExtButtonType = Gordago.Analysis.Chart.ExtButtonType.Hide;
      this._btnMinMax.Location = new System.Drawing.Point(1, 1);
      this._btnMinMax.Name = "_btnMinMax";
      this._btnMinMax.Size = new System.Drawing.Size(12, 12);
      this._btnMinMax.TabIndex = 25;
      this._btnMinMax.CheckedChanged += new System.EventHandler(this._btnMinMax_CheckedChanged);
      // 
      // OrdersOperatePanel
      // 
      this.Controls.Add(this._btnMinMax);
      this.Controls.Add(this._tbcMain);
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "OrdersOperatePanel";
      this.Size = new System.Drawing.Size(300, 150);
      this._tbcMain.ResumeLayout(false);
      this._tbpDelete.ResumeLayout(false);
      this._tbpModify.ResumeLayout(false);
      this._tbpModify.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsModify)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudRateModify)).EndInit();
      this._tbpCreate.ResumeLayout(false);
      this.panelExt1.ResumeLayout(false);
      this.panelExt1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._nudRateCreate)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudStopCreate)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLimitCreate)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._nudLotsCreate)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

    #region public bool Maximized
    public bool Maximized {
      get { return this._btnMinMax.Checked; }
      set { this._btnMinMax.Checked = value; }
    }
    #endregion 

    #region public Font FontVerticalText
    public Font FontVerticalText  {
      get { return this._font; }
      set {this._font = value;}
    }
    #endregion

    #region public new string Text
    public new string Text {
      get { return this._text; }
      set { this._text = value; }
    }
    #endregion

    #region public Color BorderColor
    public Color BorderColor {
      get { return this._borderColor; }
      set {
        _borderColor = value;
        _borderPen = new Pen(value);
        this.Invalidate();
      }
    }
    #endregion

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region public IOrder Order
    public IOrder Order{
			get{return this._order;}
      set {
        bool evt = _order != value;
        _order = value;
        this.OnOrderChanged();
      }
		}
		#endregion

    #region private void OnOrderChanged()
    private void OnOrderChanged() {
      if (_savedOrder != this._order || !this.Visible){
        this._savedOrder = _order;

        this._btnDeleteOrders.Enabled =
          this._btnModifyOrders.Enabled =
          this._nudLotsModify.Enabled =
          this._nudRateModify.Enabled =
          this._cnpStopModify.Enabled =
          this._cnpLimitModify.Enabled =
          this._lblslp.Enabled =
          this._lbltpp.Enabled = _order != null;

        if (_order == null) {
          this._nudLotsModify.Value = 1;
          this._nudRateModify.Value = 0;
          this._cnpStopModify.Value = 0;
          this._cnpLimitModify.Value = 0;
          this._cnpStopModify.DecimalDigits = this._cnpLimitModify.DecimalDigits = 0;
        } else {

          if (_order.Lots > 0) {
            _lots = _order.Lots;
            this._nudLotsModify.Value = _lots;
            this._nudLotsModify.Enabled = true;
          } else {
            this._nudLotsModify.Value = 1;
            this._nudLotsModify.Enabled = false;
          }


          this._nudRateModify.Maximum = Convert.ToDecimal(_order.Rate * 10);
          this._nudRateModify.Value = Convert.ToDecimal(_order.Rate);

          this._nudRateModify.DecimalPlaces = _order.OnlineRate.Symbol.DecimalDigits;
          this._nudRateModify.Increment = Convert.ToDecimal(_order.OnlineRate.Symbol.Point);

          this._cnpStopModify.Maximum =
            this._cnpLimitModify.Maximum = _order.Rate * 10;

          this._cnpStopModify.DecimalDigits =
            this._cnpLimitModify.DecimalDigits = _order.OnlineRate.Symbol.DecimalDigits;

          this._cnpLimitModify.Checked = _order.LimitOrder != null;
          this._cnpStopModify.Checked = _order.StopOrder != null;

        }
      }
      bool modify = false;
      if (_order != null) {

        float price = Convert.ToSingle(_nudRateModify.Value);
        if (this._chkDepend.Checked) {
          float dprice = _order.Rate - price;
          if (_order.StopOrder != null) {
            _cnpStopModify.Value = _order.StopOrder.Rate - dprice;
          }
          if (_order.LimitOrder != null) {
            _cnpLimitModify.Value = _order.LimitOrder.Rate - dprice;
          }
        }

        int dig = SymbolManager.GetDelimiter(_order.OnlineRate.Symbol.DecimalDigits);

        float newstop = this._cnpStopModify.Value;
        float stop = _order.StopOrder != null ? _order.StopOrder.Rate : float.NaN;

        float newlimit = this._cnpLimitModify.Value;
        float limit = _order.LimitOrder != null ? _order.LimitOrder.Rate : float.NaN;

        modify = price != _order.Rate ||
          Convert.ToInt32(_nudLotsModify.Value) != _lots ||
          stop != newstop || limit != newlimit;


        this._lblslp.Text = "=" + this.GetStopPoint(this._cnpStopModify.Value).ToString();
        this._lbltpp.Text = "=" + this.GetLimitPoint(this._cnpLimitModify.Value).ToString();

        if (!BCM.Busy)
          modify = false;
        this._btnDeleteOrders.Enabled = BCM.Busy;



        List<string> sa = new List<string>();
        int decdig = _order.OnlineRate.Symbol.DecimalDigits;
        string text = (_order.TradeType == TradeType.Sell ? "sell" : "buy") + " " +
          (_order.OrderType == OrderType.EntryStop ? "stop" : "limit") + " #" + _order.OrderId;

        sa.Add(text + "  " + "Rate " + SymbolManager.ConvertToCurrencyString(_order.Rate, decdig) + " " + 
          "Lots " + _order.Lots.ToString());

        if (_order.StopOrder != null)
          sa.Add("Stop " + SymbolManager.ConvertToCurrencyString(_order.StopOrder.Rate, decdig));

        if (_order.LimitOrder != null)
          sa.Add("Limit " + SymbolManager.ConvertToCurrencyString(_order.LimitOrder.Rate));

        sa.Add(_order.Time.ToShortDateString() + " " + _order.Time.ToShortTimeString());
        this._lblDeleteInfo.Text = 
          this._lblModifyInfo.Text = string.Join("\n", sa.ToArray());

      } else {
        this._lblModifyInfo.Text = "";
        this._lblDeleteInfo.Text = "";
      }
      this._btnModifyOrders.Enabled = modify;
    }
    #endregion

		#region private void _cnpSL_CheckedChanged(object sender, System.EventArgs e) 
		private void _cnpSL_CheckedChanged(object sender, System.EventArgs e) {
			if (_cnpStopModify.Checked){
				ISymbol symbol = _order.OnlineRate.Symbol;
				if (_order.StopOrder != null){
					this._cnpStopModify.Value = _order.StopOrder.Rate;
				}else{
					this._cnpStopModify.Value = this.GetStopPrice(30);
				}
			}
      this.OnOrderChanged();
		}
		#endregion

		#region private void _cnpTP_CheckedChanged(object sender, System.EventArgs e) 
		private void _cnpTP_CheckedChanged(object sender, System.EventArgs e) {
			if (_cnpLimitModify.Checked){
				ISymbol symbol = _order.OnlineRate.Symbol;
				if (_order.LimitOrder != null){
					this._cnpLimitModify.Value = _order.LimitOrder.Rate;
				}else{
					this._cnpLimitModify.Value = this.GetLimitPrice(30);
				}
			}
      this.OnOrderChanged();
    }
		#endregion 

		#region POINT FUNCTION
//		#region private int GetSavedStop()
//		private int GetSavedStop(){
//			if (_order == null)
//				return TraderAPI.MIN_STOP_LOSS;
//			else{
//				Symbol symbol = _order.OnlineRate.Symbol;
//				return Config.Users["Symbol"][symbol.Name]["Stop", 50];
//			}
//		}
//		#endregion

//		#region private int GetSavedLimit()
//		private int GetSavedLimit(){
//			if (_order == null)
//				return TraderAPI.MIN_TAKE_PROFIT;
//			else{
//				Symbol symbol = _order.OnlineRate.Symbol;
//				return Config.Users["Symbol"][symbol.Name]["Limit", 50];
//			}
//		}
//		#endregion
		#endregion

		#region private int GetStopPoint(float price)
		private int GetStopPoint(float price){
			ISymbol symbol = _order.OnlineRate.Symbol;
			float rate = Convert.ToSingle(this._nudRateModify.Value);
			if (_order.TradeType == TradeType.Buy){

				return Broker.CalculatePoint(rate, price, symbol.DecimalDigits, false);
			}else{
				return Broker.CalculatePoint(price, rate, symbol.DecimalDigits, false);
			}
		}
		#endregion

		#region private int GetLimitPoint(float price)
		private int GetLimitPoint(float price){
			if (_order == null) return 0;

			ISymbol symbol = _order.OnlineRate.Symbol;
			float rate = Convert.ToSingle(this._nudRateModify.Value);

			if (_order.TradeType == TradeType.Buy){
				return Broker.CalculatePoint(price, rate, symbol.DecimalDigits, false);
			}else{
				return Broker.CalculatePoint(rate, price, symbol.DecimalDigits, false);
			}
		}
		#endregion

		#region private float GetStopPrice(int point)
		private float GetStopPrice(int point){
//			if (!_cnpSL.Checked) return float.NaN;
			ISymbol symbol = _order.OnlineRate.Symbol;
			if (_order.TradeType == TradeType.Buy){
				return Broker.CalculatePrice(_order.Rate, -point, symbol.DecimalDigits);
			}else{
				return Broker.CalculatePrice(_order.Rate, +point, symbol.DecimalDigits);
			}
		}
		#endregion

		#region private float GetLimitPrice(int point)
		private float GetLimitPrice(int point){
//			if (!_cnpTP.Checked) return float.NaN;
			ISymbol symbol = _order.OnlineRate.Symbol;

			if (_order.TradeType == TradeType.Buy){
				return Broker.CalculatePrice(_order.Rate, +point, symbol.DecimalDigits);
			}else{
				return Broker.CalculatePrice(_order.Rate, -point, symbol.DecimalDigits);
			}
		}
		#endregion

		#region private void _cnpTP_ValueChanged(object sender, System.EventArgs e)
		private void _cnpTP_ValueChanged(object sender, System.EventArgs e) {
			this.OnOrderChanged();
		}
		#endregion

		#region private void _cnpSL_ValueChanged(object sender, System.EventArgs e) 
		private void _cnpSL_ValueChanged(object sender, System.EventArgs e) {
      this.OnOrderChanged();
		}
		#endregion

		#region private void _nudPriceOrders_ValueChanged(object sender, System.EventArgs e)
		private void _nudPriceOrders_ValueChanged(object sender, System.EventArgs e) {
      this.OnOrderChanged();
		}
		#endregion

		#region private void _chkDepend_CheckedChanged(object sender, System.EventArgs e) 
		private void _chkDepend_CheckedChanged(object sender, System.EventArgs e) {
      this.OnOrderChanged();
		}
		#endregion

    #region IBrokerEvents Members

    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      this.UpdateCreateTab();
    }

    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      this.OnOrderChanged();
    }

    public void BrokerCommandStarting(BrokerCommand command) {
      this.OnOrderChanged();
    }

    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      this.OnOrderChanged();
    }

    #endregion

    #region protected override void OnPaint(PaintEventArgs e)
    protected override void OnPaint(PaintEventArgs e) {
      Graphics g = e.Graphics;
      Color c =this.Focused? Color.FromArgb(53, 116, 215):Color.FromArgb(216,212,202);
      g.Clear(c);
      _btnMinMax.BackColor = c;

      this.DrawVerticalString(g, this.Text, 1, 18, 14, this.Height - 30);

      g.DrawRectangle(_borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }
    #endregion

    #region private void _btnMinMax_CheckedChanged(object sender, EventArgs e)
    private void _btnMinMax_CheckedChanged(object sender, EventArgs e) {
      if (!this._btnMinMax.Checked) {
        this._tbcMain.Visible = false;
        this.Width = 15;
      } else {
        this._tbcMain.Visible = true;
        this.Width = 300;
      }
      this.Refresh();
      if (this.StatusChanged != null)
        this.StatusChanged(this, new EventArgs());
    }
    #endregion

    #region private void DrawVerticalString(Graphics g, string s, int x, int y, int width, int height)
    private void DrawVerticalString(Graphics g, string s, int x, int y, int width, int height) {
      Bitmap bitmap = new Bitmap(width, height);
      Graphics gbm = Graphics.FromImage(bitmap);


      RectangleF rect = new RectangleF(-height, 0, height, width);
      gbm.RotateTransform(-90, System.Drawing.Drawing2D.MatrixOrder.Append);

      using (StringFormat sformat = new StringFormat()) {
        sformat.Alignment = StringAlignment.Center;
        sformat.LineAlignment = StringAlignment.Center;
        gbm.DrawString(s, this.FontVerticalText, new SolidBrush(this.ForeColor), rect, sformat);
      }
      gbm.Flush();
      g.DrawImageUnscaled(bitmap, x, y);
    }
    #endregion

    #region private void InitCreateTab()
    private void InitCreateTab() {
      if (_tbcMain.SelectedTab != this._tbpCreate || BCM.ConnectionStatus != BrokerConnectionStatus.Online)
        return;

      _cmbSymbols.Items.Clear();

      TableControl symbolsTable = GordagoMain.MainForm.SymbolsPanel.Table;
      if (symbolsTable.Rows.Count != this._cmbSymbols.Items.Count) {
        this._cmbSymbols.Items.Clear();
        for (int i = 0; i < symbolsTable.Rows.Count; i++) {
          TableRow row = symbolsTable.Rows[i];
          _cmbSymbols.Items.Add(row.AdditionObject);
        }
      }
      if (GordagoMain.MainForm.ActiveMdiChild is ChartForm) {
        ChartForm chartForm = GordagoMain.MainForm.ActiveMdiChild as ChartForm;
        this._cmbSymbols.SelectedItem = chartForm.Symbol;
      }
    }
    #endregion

    #region private void UpdateCreateTab()
    private void UpdateCreateTab() {

      if (this._cmbSymbols.SelectedItem == null) {
        if (GordagoMain.MainForm.ActiveMdiChild is ChartForm)
          _cmbSymbols.SelectedItem = (GordagoMain.MainForm.ActiveMdiChild as ChartForm).Symbol;
      }

      ISymbol symbol = this._cmbSymbols.SelectedItem as ISymbol;
      this._btnCreateSell.Symbol = symbol;
      this._btnCreateBuy.Symbol = symbol;
      IOnlineRate onlineRate = null;
      if (this.BCM.ConnectionStatus == BrokerConnectionStatus.Online && symbol != null) {
        onlineRate = this.BCM.Broker.OnlineRates.GetOnlineRate(symbol.Name);
      }

      if (_savedOnlineRate != onlineRate || !this.Visible) {
        _savedOnlineRate = onlineRate;

          _chkStopCreate.Checked =
          _chkStopCreate.Enabled =
          _chkLimitCreate.Checked =
          _chkLimitCreate.Enabled =
          _nudRateCreate.Enabled = 
          _nudLotsCreate.Enabled = onlineRate != null;

          if (onlineRate != null) {
            _nudRateCreate.DecimalPlaces = onlineRate.Symbol.DecimalDigits;
            _nudRateCreate.Value = Convert.ToDecimal(onlineRate.SellRate);
            _nudRateCreate.Increment = Convert.ToDecimal(onlineRate.Symbol.Point);
          }
      }

      if (onlineRate == null) {
        _btnCreateSell.Price = _btnCreateBuy.Price = 0;
        _nudRateCreate.Value = 0;
      } else {
        float rate = Convert.ToSingle(_nudRateCreate.Value);
        _btnCreateSell.Price = rate;
        _btnCreateBuy.Price = rate;
        _sellCalculator.SetValues(onlineRate, rate, 
          _chkStopCreate.Checked ? (int)_nudStopCreate.Value : 0, 
          _chkLimitCreate.Checked ? (int)_nudLimitCreate.Value : 0);
        _buyCalculator.SetValues(onlineRate, rate, _chkStopCreate.Checked ? (int)_nudStopCreate.Value : 0, _chkLimitCreate.Checked ? (int)_nudLimitCreate.Value : 0);
      }
    }
    #endregion

    #region private void _cmbSymbols_SelectedIndexChanged(object sender, EventArgs e)
    private void _cmbSymbols_SelectedIndexChanged(object sender, EventArgs e) {
      this.UpdateCreateTab();
    }
    #endregion

    #region public void MainFormMdiChildActivate()
    public void MainFormMdiChildActivate() {
      if (GordagoMain.MainForm.ActiveMdiChild is ChartForm) 
        _cmbSymbols.SelectedItem = (GordagoMain.MainForm.ActiveMdiChild as ChartForm).Symbol;

      this.UpdateCreateTab();
    }
    #endregion

    #region private void ViewPreview(TradeType tradeType)
    private void ViewPreview(TradeType tradeType) {
      string s = "";
      if (this.BCM.ConnectionStatus == BrokerConnectionStatus.Online) {
        OrderCalculator calc = tradeType == TradeType.Sell ? _sellCalculator : _buyCalculator;

        if (calc.Error) {
          s = "Error";
          _lblCreateInfo.ForeColor = Color.Red;
        } else {
          _lblCreateInfo.ForeColor = Color.Black;
          s = (tradeType == TradeType.Sell ? "Sell " : "Buy ") +
            (calc.OrderType == OrderType.Stop ? "Stop" : "Limit") + "\n" +
            BrokerCommandManager.LNG_HO_RATE + ": {0}\n" +
            BrokerCommandManager.LNG_HO_LOTS + ": {1}";

          s = string.Format(s, calc.Rate, (int)_nudLotsCreate.Value);

          if (!float.IsNaN(calc.StopRate)) {
            s += "\n" + BrokerCommandManager.LNG_HO_STOP + ": " + SymbolManager.ConvertToCurrencyString(calc.StopRate, calc.DecimalDigits);
          }
          if (!float.IsNaN(calc.LimitRate)) {
            s += "\n" + BrokerCommandManager.LNG_HO_LIMIT + ": " + SymbolManager.ConvertToCurrencyString(calc.LimitRate, calc.DecimalDigits);
          }
        }
      }
      _lblCreateInfo.Text = s;
    }
    #endregion

    #region private void _btnCreateSell_MouseEnter(object sender, EventArgs e)
    private void _btnCreateSell_MouseEnter(object sender, EventArgs e) {
      this.UpdateCreateTab();
      this.ViewPreview(TradeType.Sell);
    }
    #endregion

    #region private void _btnCreateSell_MouseLeave(object sender, EventArgs e)
    private void _btnCreateSell_MouseLeave(object sender, EventArgs e) {
      _lblCreateInfo.Text = "";
    }
    #endregion

    #region private void _btnCreateSell_MouseMove(object sender, MouseEventArgs e)
    private void _btnCreateSell_MouseMove(object sender, MouseEventArgs e) {
      this.ViewPreview(TradeType.Sell);
    }
    #endregion

    #region private void _btnCreateBuy_MouseEnter(object sender, EventArgs e)
    private void _btnCreateBuy_MouseEnter(object sender, EventArgs e) {
      this.UpdateCreateTab();
      this.ViewPreview(TradeType.Buy);
    }
    #endregion

    #region private void _btnCreateBuy_MouseLeave(object sender, EventArgs e)
    private void _btnCreateBuy_MouseLeave(object sender, EventArgs e) {
      _lblCreateInfo.Text = "";
    }
    #endregion

    #region private void _btnCreateBuy_MouseMove(object sender, MouseEventArgs e)
    private void _btnCreateBuy_MouseMove(object sender, MouseEventArgs e) {
      this.ViewPreview(TradeType.Buy);
    }
    #endregion

    #region private void _chkStopCreate_CheckedChanged(object sender, EventArgs e)
    private void _chkStopCreate_CheckedChanged(object sender, EventArgs e) {
      _nudStopCreate.Enabled = _chkStopCreate.Checked;
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _chkLimitCreate_CheckedChanged(object sender, EventArgs e)
    private void _chkLimitCreate_CheckedChanged(object sender, EventArgs e) {
      _nudLimitCreate.Enabled = _chkLimitCreate.Checked;
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _nudStopCreate_ValueChanged(object sender, EventArgs e)
    private void _nudStopCreate_ValueChanged(object sender, EventArgs e) {
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _nudLimitCreate_ValueChanged(object sender, EventArgs e)
    private void _nudLimitCreate_ValueChanged(object sender, EventArgs e) {
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _nudLotsCreate_ValueChanged(object sender, EventArgs e)
    private void _nudLotsCreate_ValueChanged(object sender, EventArgs e) {
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _nudSlippageCreate_ValueChanged(object sender, EventArgs e)
    private void _nudSlippageCreate_ValueChanged(object sender, EventArgs e) {
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _btnCreateSell_Click(object sender, EventArgs e)
    private void _btnCreateSell_Click(object sender, EventArgs e) {
      this.SendOrderFromHandOperate(TradeType.Sell);
    }
    #endregion

    #region private void _btnCreateBuy_Click(object sender, EventArgs e)
    private void _btnCreateBuy_Click(object sender, EventArgs e) {
      this.SendOrderFromHandOperate(TradeType.Buy);
    }
    #endregion

    #region private void SendOrderFromHandOperate(TradeType tradeType)
    private void SendOrderFromHandOperate(TradeType tradeType) {

      if (!this.BCM.Busy) return;

      int lots = Convert.ToInt32(_nudLotsCreate.Value);

      OrderCalculator calc = tradeType == TradeType.Sell ? _sellCalculator : _buyCalculator;

      BrokerCommandEntryOrderCreate command =
        new BrokerCommandEntryOrderCreate(GordagoMain.MainForm.DefaultAccountId,
        calc.OnlineRate, calc.OrderType, tradeType, (int)_nudLotsCreate.Value, 
        calc.Rate, calc.StopRate, calc.LimitRate, "");
      BCM.ExecuteCommand(command);
    }
    #endregion

    #region private void _btnModifyOrders_Click(object sender, System.EventArgs e)
    private void _btnModifyOrders_Click(object sender, System.EventArgs e) {
      float newprice = Convert.ToSingle(_nudRateModify.Value);

      BCM.ExecuteCommand(new BrokerCommandEntryOrderModify(_order, (int)_nudLotsModify.Value, newprice, _cnpStopModify.Value, _cnpLimitModify.Value));
    }
    #endregion

    #region private void _btnDeleteOrders_Click(object sender, System.EventArgs e)
    private void _btnDeleteOrders_Click(object sender, System.EventArgs e) {
      if (_order == null) return;
      this.BCM.ExecuteCommand(new BrokerCommandEntryOrderDelete(_order, ""));
    }
    #endregion



    #region private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e)
    private void _tbcMain_SelectedIndexChanged(object sender, EventArgs e) {
      this.InitCreateTab();
    }
    #endregion

    #region private void _nudRateCreate_ValueChanged(object sender, EventArgs e)
    private void _nudRateCreate_ValueChanged(object sender, EventArgs e) {
      this.UpdateCreateTab();
    }
    #endregion

    #region private void _nudLotsModify_ValueChanged(object sender, EventArgs e)
    private void _nudLotsModify_ValueChanged(object sender, EventArgs e) {
      this.OnOrderChanged();
    }
    #endregion

  }
}
