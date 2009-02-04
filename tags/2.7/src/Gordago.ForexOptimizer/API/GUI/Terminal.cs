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
using Cursit.Applic.AConfig;
using Language;
using Cursit;
#endregion

namespace Gordago.API {
	class Terminal : UserControl, IBrokerEvents {

		#region private property

		private System.ComponentModel.Container components = null;
    private TabPage _tbpAccounts;
		private TableControl _tableAccounts;
		private TabPage _tbpTrades;
		private TableControl _tableTrades;
		private TabPage _tbpOrders;
    private Cursit.Table.TableControl _tableOrders;
		private System.Windows.Forms.TabPage _tbpStrategyOperate;
		private Gordago.Strategy.SULines _sulines;
		private TabControlExt _tbcMain;
    private System.Windows.Forms.Button _btnSetAsDefaultAccount;
		private Gordago.API.OrdersOperatePanel _ordersOperatePanel;
		private Gordago.API.TradersOperatePanel _tradesOperatePanel;
				
		private TableCellStyle _styledefaccout;
    private TabPage _tbpLog;
    private TabPage _tbpClosedTrades;
    private TableControl _tableJournal;
    private Label _lblTAccountPL;
    private TableControl _tableClosedTrades;
		#endregion

    public Terminal() {
      InitializeComponent();

      try {
        _styledefaccout = _tableAccounts.Style.Clone();
        _styledefaccout.Font = new Font(_styledefaccout.Font.FontFamily, _styledefaccout.Font.Size, FontStyle.Bold);
        SetAccountTableColumns();
        SetTradesTableColumns();
        SetOrdersTableColumns();
        SetClosedTradesTableColumns();
        SetJournalTableColumns();

        this._tbpAccounts.Text = Dictionary.GetString(30, 2);
        this._tbpOrders.Text = Dictionary.GetString(30, 4);
        this._tbpStrategyOperate.Text = Dictionary.GetString(30, 6);
        this._tbpTrades.Text = Dictionary.GetString(30, 3);
        this._tbpClosedTrades.Text = Dictionary.GetString(30, 106, "�������");
        _tbpLog.Text = Dictionary.GetString(30, 107, "������");
      } catch { }

      GordagoMain.MainForm.SetContextMenuOnTable(_tableAccounts);
      GordagoMain.MainForm.SetContextMenuOnTable(_tableClosedTrades);
      GordagoMain.MainForm.SetContextMenuOnTable(_tableJournal);
      GordagoMain.MainForm.SetContextMenuOnTable(_tableOrders);
      GordagoMain.MainForm.SetContextMenuOnTable(_tableTrades);
    }

    #region private BrokerCommandManager BCM
    private BrokerCommandManager BCM {
      get { return GordagoMain.MainForm.BCM; }
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) 
					components.Dispose();
				Config.Users["Terminal"]["AccountsTableWidths"].SetValue(_tableAccounts.Columns.GetConfigWidths());
        Config.Users["Terminal"]["TradesTableWidths"].SetValue(_tableTrades.Columns.GetConfigWidths());
				Config.Users["Terminal"]["OrdersTableWidths"].SetValue(_tableOrders.Columns.GetConfigWidths());
        Config.Users["Terminal"]["ClTradesTableWidths"].SetValue(_tableClosedTrades.Columns.GetConfigWidths());
        Config.Users["Terminal"]["JournalTableWidths"].SetValue(_tableJournal.Columns.GetConfigWidths());
			}
			base.Dispose( disposing );
		}
		#endregion

    #region private void InitializeComponent()
    private void InitializeComponent() {
      Cursit.Table.TableCellStyle tableCellStyle1 = new Cursit.Table.TableCellStyle();
      Cursit.Table.TableCellStyle tableCellStyle2 = new Cursit.Table.TableCellStyle();
      Cursit.Table.TableCellStyle tableCellStyle3 = new Cursit.Table.TableCellStyle();
      Cursit.Table.TableCellStyle tableCellStyle4 = new Cursit.Table.TableCellStyle();
      Cursit.Table.TableCellStyle tableCellStyle5 = new Cursit.Table.TableCellStyle();
      this._tbcMain = new Cursit.TabControlExt();
      this._tbpAccounts = new System.Windows.Forms.TabPage();
      this._btnSetAsDefaultAccount = new System.Windows.Forms.Button();
      this._lblTAccountPL = new System.Windows.Forms.Label();
      this._tableAccounts = new Cursit.Table.TableControl();
      this._tbpTrades = new System.Windows.Forms.TabPage();
      this._tradesOperatePanel = new Gordago.API.TradersOperatePanel();
      this._tableTrades = new Cursit.Table.TableControl();
      this._tbpOrders = new System.Windows.Forms.TabPage();
      this._ordersOperatePanel = new Gordago.API.OrdersOperatePanel();
      this._tableOrders = new Cursit.Table.TableControl();
      this._tbpClosedTrades = new System.Windows.Forms.TabPage();
      this._tableClosedTrades = new Cursit.Table.TableControl();
      this._tbpStrategyOperate = new System.Windows.Forms.TabPage();
      this._sulines = new Gordago.Strategy.SULines();
      this._tbpLog = new System.Windows.Forms.TabPage();
      this._tableJournal = new Cursit.Table.TableControl();
      this._tbcMain.SuspendLayout();
      this._tbpAccounts.SuspendLayout();
      this._tbpTrades.SuspendLayout();
      this._tbpOrders.SuspendLayout();
      this._tbpClosedTrades.SuspendLayout();
      this._tbpStrategyOperate.SuspendLayout();
      this._tbpLog.SuspendLayout();
      this.SuspendLayout();
      // 
      // _tbcMain
      // 
      this._tbcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this._tbcMain.BackColorTabPages = System.Drawing.Color.White;
      this._tbcMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tbcMain.BorderVisible = false;
      this._tbcMain.Controls.Add(this._tbpAccounts);
      this._tbcMain.Controls.Add(this._tbpTrades);
      this._tbcMain.Controls.Add(this._tbpOrders);
      this._tbcMain.Controls.Add(this._tbpClosedTrades);
      this._tbcMain.Controls.Add(this._tbpStrategyOperate);
      this._tbcMain.Controls.Add(this._tbpLog);
      this._tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tbcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this._tbcMain.Location = new System.Drawing.Point(0, 0);
      this._tbcMain.Margin = new System.Windows.Forms.Padding(0);
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.Padding = new System.Drawing.Point(0, 0);
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(699, 184);
      this._tbcMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
      this._tbcMain.TabIndex = 4;
      // 
      // _tbpAccounts
      // 
      this._tbpAccounts.BackColor = System.Drawing.Color.White;
      this._tbpAccounts.Controls.Add(this._btnSetAsDefaultAccount);
      this._tbpAccounts.Controls.Add(this._lblTAccountPL);
      this._tbpAccounts.Controls.Add(this._tableAccounts);
      this._tbpAccounts.Location = new System.Drawing.Point(4, 4);
      this._tbpAccounts.Margin = new System.Windows.Forms.Padding(0);
      this._tbpAccounts.Name = "_tbpAccounts";
      this._tbpAccounts.Size = new System.Drawing.Size(691, 158);
      this._tbpAccounts.TabIndex = 4;
      this._tbpAccounts.Text = "Accounts";
      // 
      // _btnSetAsDefaultAccount
      // 
      this._btnSetAsDefaultAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnSetAsDefaultAccount.Location = new System.Drawing.Point(559, 129);
      this._btnSetAsDefaultAccount.Name = "_btnSetAsDefaultAccount";
      this._btnSetAsDefaultAccount.Size = new System.Drawing.Size(128, 23);
      this._btnSetAsDefaultAccount.TabIndex = 3;
      this._btnSetAsDefaultAccount.Text = "Set as Default";
      this._btnSetAsDefaultAccount.UseVisualStyleBackColor = false;
      this._btnSetAsDefaultAccount.Visible = false;
      this._btnSetAsDefaultAccount.Click += new System.EventHandler(this._btnSetAsDefaultAccount_Click);
      // 
      // _lblTAccountPL
      // 
      this._lblTAccountPL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._lblTAccountPL.AutoSize = true;
      this._lblTAccountPL.Location = new System.Drawing.Point(13, 135);
      this._lblTAccountPL.Name = "_lblTAccountPL";
      this._lblTAccountPL.Size = new System.Drawing.Size(99, 13);
      this._lblTAccountPL.TabIndex = 2;
      this._lblTAccountPL.Text = "����� ������ = 0";
      this._lblTAccountPL.Visible = false;
      // 
      // _tableAccounts
      // 
      this._tableAccounts.AutoColumnSize = false;
      this._tableAccounts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tableAccounts.BorderVisible = true;
      this._tableAccounts.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._tableAccounts.CaptionVisible = true;
      this._tableAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tableAccounts.HeaderHeight = 18;
      this._tableAccounts.HideSelection = false;
      this._tableAccounts.HSBVisible = false;
      this._tableAccounts.Location = new System.Drawing.Point(0, 0);
      this._tableAccounts.MouseHoverSelectRowColor = System.Drawing.Color.Blue;
      this._tableAccounts.Name = "_tableAccounts";
      this._tableAccounts.RowColorFirst = System.Drawing.Color.White;
      this._tableAccounts.RowColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
      this._tableAccounts.SelectedColor = System.Drawing.Color.Blue;
      this._tableAccounts.SelectedIndex = -1;
      this._tableAccounts.SelectedRow = null;
      this._tableAccounts.Size = new System.Drawing.Size(691, 158);
      tableCellStyle1.BackColor = System.Drawing.Color.White;
      tableCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      tableCellStyle1.ForeColor = System.Drawing.Color.Black;
      tableCellStyle1.GridLineColor = System.Drawing.Color.Gainsboro;
      tableCellStyle1.SelectColor = System.Drawing.Color.BlueViolet;
      tableCellStyle1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      this._tableAccounts.Style = tableCellStyle1;
      this._tableAccounts.TabIndex = 1;
      this._tableAccounts.ViewHGdirLines = false;
      // 
      // _tbpTrades
      // 
      this._tbpTrades.BackColor = System.Drawing.Color.White;
      this._tbpTrades.Controls.Add(this._tradesOperatePanel);
      this._tbpTrades.Controls.Add(this._tableTrades);
      this._tbpTrades.Location = new System.Drawing.Point(4, 4);
      this._tbpTrades.Margin = new System.Windows.Forms.Padding(0);
      this._tbpTrades.Name = "_tbpTrades";
      this._tbpTrades.Size = new System.Drawing.Size(691, 158);
      this._tbpTrades.TabIndex = 2;
      this._tbpTrades.Text = "Trades";
      this._tbpTrades.UseVisualStyleBackColor = true;
      this._tbpTrades.Visible = false;
      // 
      // _tradesOperatePanel
      // 
      this._tradesOperatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._tradesOperatePanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tradesOperatePanel.FontVerticalText = new System.Drawing.Font("Microsoft Sans Serif", 7F);
      this._tradesOperatePanel.Location = new System.Drawing.Point(0, 0);
      this._tradesOperatePanel.Margin = new System.Windows.Forms.Padding(0);
      this._tradesOperatePanel.Maximized = true;
      this._tradesOperatePanel.Name = "_tradesOperatePanel";
      this._tradesOperatePanel.Size = new System.Drawing.Size(300, 158);
      this._tradesOperatePanel.TabIndex = 1;
      this._tradesOperatePanel.StatusChanged += new System.EventHandler(this._tradesOperatePanel_StatusChanged);
      // 
      // _tableTrades
      // 
      this._tableTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tableTrades.AutoColumnSize = false;
      this._tableTrades.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tableTrades.BorderVisible = true;
      this._tableTrades.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._tableTrades.CaptionVisible = true;
      this._tableTrades.HeaderHeight = 18;
      this._tableTrades.HideSelection = false;
      this._tableTrades.HSBVisible = false;
      this._tableTrades.Location = new System.Drawing.Point(303, 0);
      this._tableTrades.MouseHoverSelectRowColor = System.Drawing.Color.Blue;
      this._tableTrades.Name = "_tableTrades";
      this._tableTrades.RowColorFirst = System.Drawing.Color.White;
      this._tableTrades.RowColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
      this._tableTrades.SelectedColor = System.Drawing.Color.Blue;
      this._tableTrades.SelectedIndex = -1;
      this._tableTrades.SelectedRow = null;
      this._tableTrades.Size = new System.Drawing.Size(388, 158);
      tableCellStyle2.BackColor = System.Drawing.Color.White;
      tableCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      tableCellStyle2.ForeColor = System.Drawing.Color.Black;
      tableCellStyle2.GridLineColor = System.Drawing.Color.Gainsboro;
      tableCellStyle2.SelectColor = System.Drawing.Color.BlueViolet;
      tableCellStyle2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      this._tableTrades.Style = tableCellStyle2;
      this._tableTrades.TabIndex = 0;
      this._tableTrades.ViewHGdirLines = false;
      this._tableTrades.DoubleClick += new System.EventHandler(this._tableTrades_DoubleClick);
      this._tableTrades.Click += new System.EventHandler(this._tableTrades_Click);
      this._tableTrades.SelectedIndexChanged += new System.EventHandler(this._tableTrades_SelectedIndexChanged);
      // 
      // _tbpOrders
      // 
      this._tbpOrders.BackColor = System.Drawing.Color.White;
      this._tbpOrders.Controls.Add(this._ordersOperatePanel);
      this._tbpOrders.Controls.Add(this._tableOrders);
      this._tbpOrders.Location = new System.Drawing.Point(4, 4);
      this._tbpOrders.Margin = new System.Windows.Forms.Padding(0);
      this._tbpOrders.Name = "_tbpOrders";
      this._tbpOrders.Size = new System.Drawing.Size(691, 158);
      this._tbpOrders.TabIndex = 3;
      this._tbpOrders.Text = "Orders";
      this._tbpOrders.Visible = false;
      // 
      // _ordersOperatePanel
      // 
      this._ordersOperatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._ordersOperatePanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._ordersOperatePanel.FontVerticalText = new System.Drawing.Font("Microsoft Sans Serif", 7F);
      this._ordersOperatePanel.Location = new System.Drawing.Point(0, 0);
      this._ordersOperatePanel.Margin = new System.Windows.Forms.Padding(0);
      this._ordersOperatePanel.Maximized = true;
      this._ordersOperatePanel.Name = "_ordersOperatePanel";
      this._ordersOperatePanel.Size = new System.Drawing.Size(300, 158);
      this._ordersOperatePanel.TabIndex = 2;
      this._ordersOperatePanel.StatusChanged += new System.EventHandler(this._ordersOperatePanel_StatusChanged);
      // 
      // _tableOrders
      // 
      this._tableOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tableOrders.AutoColumnSize = false;
      this._tableOrders.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tableOrders.BorderVisible = true;
      this._tableOrders.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._tableOrders.CaptionVisible = true;
      this._tableOrders.HeaderHeight = 18;
      this._tableOrders.HideSelection = false;
      this._tableOrders.HSBVisible = false;
      this._tableOrders.Location = new System.Drawing.Point(303, 0);
      this._tableOrders.MouseHoverSelectRowColor = System.Drawing.Color.Blue;
      this._tableOrders.Name = "_tableOrders";
      this._tableOrders.RowColorFirst = System.Drawing.Color.White;
      this._tableOrders.RowColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
      this._tableOrders.SelectedColor = System.Drawing.Color.Blue;
      this._tableOrders.SelectedIndex = -1;
      this._tableOrders.SelectedRow = null;
      this._tableOrders.Size = new System.Drawing.Size(388, 158);
      tableCellStyle3.BackColor = System.Drawing.Color.White;
      tableCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      tableCellStyle3.ForeColor = System.Drawing.Color.Black;
      tableCellStyle3.GridLineColor = System.Drawing.Color.Gainsboro;
      tableCellStyle3.SelectColor = System.Drawing.Color.BlueViolet;
      tableCellStyle3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      this._tableOrders.Style = tableCellStyle3;
      this._tableOrders.TabIndex = 1;
      this._tableOrders.ViewHGdirLines = false;
      this._tableOrders.DoubleClick += new System.EventHandler(this._tableOrders_DoubleClick);
      this._tableOrders.Click += new System.EventHandler(this._tableOrders_Click);
      this._tableOrders.SelectedIndexChanged += new System.EventHandler(this._tableOrders_SelectedIndexChanged);
      // 
      // _tbpClosedTrades
      // 
      this._tbpClosedTrades.BackColor = System.Drawing.Color.White;
      this._tbpClosedTrades.Controls.Add(this._tableClosedTrades);
      this._tbpClosedTrades.Location = new System.Drawing.Point(4, 4);
      this._tbpClosedTrades.Name = "_tbpClosedTrades";
      this._tbpClosedTrades.Size = new System.Drawing.Size(691, 158);
      this._tbpClosedTrades.TabIndex = 8;
      this._tbpClosedTrades.Text = "History";
      // 
      // _tableClosedTrades
      // 
      this._tableClosedTrades.AutoColumnSize = false;
      this._tableClosedTrades.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tableClosedTrades.BorderVisible = true;
      this._tableClosedTrades.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._tableClosedTrades.CaptionVisible = true;
      this._tableClosedTrades.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tableClosedTrades.HeaderHeight = 18;
      this._tableClosedTrades.HideSelection = false;
      this._tableClosedTrades.HSBVisible = false;
      this._tableClosedTrades.Location = new System.Drawing.Point(0, 0);
      this._tableClosedTrades.MouseHoverSelectRowColor = System.Drawing.Color.Blue;
      this._tableClosedTrades.Name = "_tableClosedTrades";
      this._tableClosedTrades.RowColorFirst = System.Drawing.Color.White;
      this._tableClosedTrades.RowColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
      this._tableClosedTrades.SelectedColor = System.Drawing.Color.Blue;
      this._tableClosedTrades.SelectedIndex = -1;
      this._tableClosedTrades.SelectedRow = null;
      this._tableClosedTrades.Size = new System.Drawing.Size(691, 158);
      tableCellStyle4.BackColor = System.Drawing.Color.White;
      tableCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      tableCellStyle4.ForeColor = System.Drawing.Color.Black;
      tableCellStyle4.GridLineColor = System.Drawing.Color.Gainsboro;
      tableCellStyle4.SelectColor = System.Drawing.Color.BlueViolet;
      tableCellStyle4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      this._tableClosedTrades.Style = tableCellStyle4;
      this._tableClosedTrades.TabIndex = 2;
      this._tableClosedTrades.ViewHGdirLines = false;
      // 
      // _tbpStrategyOperate
      // 
      this._tbpStrategyOperate.BackColor = System.Drawing.Color.White;
      this._tbpStrategyOperate.Controls.Add(this._sulines);
      this._tbpStrategyOperate.Location = new System.Drawing.Point(4, 4);
      this._tbpStrategyOperate.Name = "_tbpStrategyOperate";
      this._tbpStrategyOperate.Size = new System.Drawing.Size(691, 158);
      this._tbpStrategyOperate.TabIndex = 6;
      this._tbpStrategyOperate.Text = "Strategy";
      this._tbpStrategyOperate.Visible = false;
      // 
      // _sulines
      // 
      this._sulines.BackColor = System.Drawing.Color.White;
      this._sulines.Dock = System.Windows.Forms.DockStyle.Fill;
      this._sulines.Location = new System.Drawing.Point(0, 0);
      this._sulines.Name = "_sulines";
      this._sulines.Size = new System.Drawing.Size(691, 158);
      this._sulines.TabIndex = 4;
      // 
      // _tbpLog
      // 
      this._tbpLog.BackColor = System.Drawing.Color.White;
      this._tbpLog.Controls.Add(this._tableJournal);
      this._tbpLog.Location = new System.Drawing.Point(4, 4);
      this._tbpLog.Name = "_tbpLog";
      this._tbpLog.Padding = new System.Windows.Forms.Padding(3);
      this._tbpLog.Size = new System.Drawing.Size(691, 158);
      this._tbpLog.TabIndex = 7;
      this._tbpLog.Text = "Log";
      // 
      // _tableJournal
      // 
      this._tableJournal.AutoColumnSize = false;
      this._tableJournal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tableJournal.BorderVisible = true;
      this._tableJournal.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._tableJournal.CaptionVisible = true;
      this._tableJournal.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tableJournal.HeaderHeight = 18;
      this._tableJournal.HideSelection = false;
      this._tableJournal.HSBVisible = false;
      this._tableJournal.Location = new System.Drawing.Point(3, 3);
      this._tableJournal.MouseHoverSelectRowColor = System.Drawing.Color.Blue;
      this._tableJournal.Name = "_tableJournal";
      this._tableJournal.RowColorFirst = System.Drawing.Color.White;
      this._tableJournal.RowColorSecond = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
      this._tableJournal.SelectedColor = System.Drawing.Color.Blue;
      this._tableJournal.SelectedIndex = -1;
      this._tableJournal.SelectedRow = null;
      this._tableJournal.Size = new System.Drawing.Size(685, 152);
      tableCellStyle5.BackColor = System.Drawing.Color.White;
      tableCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      tableCellStyle5.ForeColor = System.Drawing.Color.Black;
      tableCellStyle5.GridLineColor = System.Drawing.Color.Gainsboro;
      tableCellStyle5.SelectColor = System.Drawing.Color.BlueViolet;
      tableCellStyle5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      this._tableJournal.Style = tableCellStyle5;
      this._tableJournal.TabIndex = 3;
      this._tableJournal.ViewHGdirLines = false;
      // 
      // Terminal
      // 
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this._tbcMain);
      this.Margin = new System.Windows.Forms.Padding(0);
      this.MinimumSize = new System.Drawing.Size(400, 184);
      this.Name = "Terminal";
      this.Size = new System.Drawing.Size(699, 184);
      this._tbcMain.ResumeLayout(false);
      this._tbpAccounts.ResumeLayout(false);
      this._tbpAccounts.PerformLayout();
      this._tbpTrades.ResumeLayout(false);
      this._tbpOrders.ResumeLayout(false);
      this._tbpClosedTrades.ResumeLayout(false);
      this._tbpStrategyOperate.ResumeLayout(false);
      this._tbpLog.ResumeLayout(false);
      this.ResumeLayout(false);

		}
		#endregion

		#region private void SetAccountTableColumns()
		private void SetAccountTableColumns(){
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_ACCOUNT, TableColumnType.Label, 55);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_TYPEACCOUNT, TableColumnType.Label, 55);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_BALANCE, TableColumnType.Label, 55, ContentAlignment.MiddleRight);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_CAPITAL, TableColumnType.Label, 55, ContentAlignment.MiddleRight);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_USEDMARGIN, TableColumnType.Label, 55, ContentAlignment.MiddleRight);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_FREEMARGIN, TableColumnType.Label, 55, ContentAlignment.MiddleRight);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_LOTS, TableColumnType.Label, 55, ContentAlignment.MiddleRight);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_PREMIUM, TableColumnType.Label, 55, ContentAlignment.MiddleRight);
			_tableAccounts.Columns.Add(BrokerCommandManager.LNG_TBL_NETPLALL, TableColumnType.Label, 55, ContentAlignment.MiddleRight);

			string cfgs = Config.Users["Terminal"]["AccountsTableWidths", ""];
			_tableAccounts.Columns.SetConfigWidths(cfgs);

		}
		#endregion

    #region private void SetClosedTradesTableColumns()
    private void SetClosedTradesTableColumns() {
      BrokerCloseTradeInitializeTable(_tableClosedTrades);
      string cfgs = Config.Users["Terminal"]["ClTradesTableWidths", ""];
      _tableClosedTrades.Columns.SetConfigWidths(cfgs);
    }
    #endregion

		#region private void SetOrdersTableColumns()
		private void SetOrdersTableColumns(){
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_ACCOUNT, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_ORDER, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_SYMBOL, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_TYPE, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_BUYSELL, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_AMOUNT, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_OPENRATE, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_STOP, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_LIMIT, TableColumnType.Label, 55);
			_tableOrders.Columns.Add(BrokerCommandManager.LNG_TBL_OPENTIME, TableColumnType.Label, 55);

			string cfgs = Config.Users["Terminal"]["OrdersTableWidths", ""];
			_tableOrders.Columns.SetConfigWidths(cfgs);
		}
		#endregion

    #region public void UpdateAccountsTable()
    public void UpdateAccountsTable() {
      
      float netplall = 0;
      for (int j = 0; j < BCM.Broker.Accounts.Count; j++) {
        IAccount account = BCM.Broker.Accounts[j];
        TableRow row = null;
        for (int i = 0; i < _tableAccounts.Rows.Count; i++) {
          IAccount facc = _tableAccounts.Rows[i].AdditionObject as IAccount;
          if (facc.AccountId == account.AccountId) {
            row = _tableAccounts.Rows[i];
            row.AdditionObject = account;
            break;
          }
        }
        if (row == null) {
          row = _tableAccounts.NewRow();
          row.AdditionObject = account;
          _tableAccounts.Rows.AddRow(row);
        }

        float netpl = account.NetPL;
        netplall += netpl;
        int n = 0;
        row.AdditionObject = account;
        row[n++].Text = account.AccountId;
        row[n++].Text = account.MoneyOwner;
        row[n++].Text = SymbolManager.ConvertToCurrencyString(account.Balance, 2, " ");
        row[n++].Text = SymbolManager.ConvertToCurrencyString(account.Equity, 2, " ");
        row[n++].Text = SymbolManager.ConvertToCurrencyString(account.UsedMargin, 0, " ");
        row[n++].Text = SymbolManager.ConvertToCurrencyString(account.UsableMargin, 2, " ");
        row[n++].Text = Convert.ToString(account.Lots);
        row[n++].Text = SymbolManager.ConvertToCurrencyString(account.Premium, 2, " ");
        row[n++].Text = SymbolManager.ConvertToCurrencyString(netpl, 2, " ");

        if (GordagoMain.MainForm.DefaultAccountId == account.AccountId) {
          row.Style = _styledefaccout;
        } else {
          row.Style = null;
        }
      }
      this._lblTAccountPL.Text = SymbolManager.ConvertToCurrencyString(netplall, 2, " ");
      this._btnSetAsDefaultAccount.Visible = BCM.Broker.Accounts.Count > 1;
      this._tableAccounts.Invalidate();
    }
		#endregion

    #region private void SetTradesTableColumns()
    private void SetTradesTableColumns() {
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_ACCOUNT, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_TRADE, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_OPENTIME, TableColumnType.Label, 95);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_BUYSELL, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_AMOUNT, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_SYMBOL, TableColumnType.Label, 65);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_OPENRATE, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_STOP, TableColumnType.Label, 60);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_LIMIT, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_CLOSERATE, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_PREMIUM, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_COMMISION, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_FEE, TableColumnType.Label, 55);
      _tableTrades.Columns.Add(BrokerCommandManager.LNG_TBL_NETPIRCE, TableColumnType.Label, 55);

      string cfgs = Config.Users["Terminal"]["TradesTableWidths", ""];
      _tableTrades.Columns.SetConfigWidths(cfgs);
      this.SetRightAlignmentTable(_tableTrades);
    }
    #endregion

    #region private void SetRightAlignmentTable(TableControl table)
    private void SetRightAlignmentTable(TableControl table) {
      for (int i = 0; i < table.Columns.Count; i++) {
        table.Columns[i].CellAlignment = ContentAlignment.MiddleRight;
      }
    }
    #endregion

    #region private void TradesTableInit()
    private void TradesTableInit() {
      this._tradesOperatePanel.Trade = null;

      this._tableTrades.Rows.Clear();
      for (int i = 0; i < this.BCM.Broker.Trades.Count;i++ ) {
        ITrade trade = this.BCM.Broker.Trades[i];
        TableRow row = _tableTrades.NewRow();
        this.TradesTableRowUpdate(row, trade);
        _tableTrades.Rows.AddRow(row);
      }
			this._tableTrades.Invalidate();
		}
		#endregion
		#region private void TradesTableUpdate(ITrade trade)
		private void TradesTableUpdate(ITrade trade){
			foreach (TableRow row in _tableTrades.Rows){
				ITrade ftrade = row.AdditionObject as ITrade;
				if (ftrade.TradeId == trade.TradeId){
					this.TradesTableRowUpdate(row, trade);
					break;
				}
			}
			this._tableTrades.Invalidate();
		}
		#endregion
		#region private void TradesTableAdd(ITrade trade)
		private void TradesTableAdd(ITrade trade){
			TableRow row = _tableTrades.NewRow();
      row.AdditionObject = trade;
			this.TradesTableRowUpdate(row, trade);
			_tableTrades.Rows.AddRow(row);
			this._tableTrades.Invalidate();
		}
		#endregion
		#region private void TradesTableDelete(ITrade trade)
		private void TradesTableDelete(ITrade trade){
			for (int i=0;i<_tableTrades.Rows.Count;i++){
				ITrade ftrade = _tableTrades.Rows[i].AdditionObject as ITrade;
				if (trade.TradeId == ftrade.TradeId){
					_tableTrades.Rows.RemoveAt(i);
					break;
				}
			}
			this._tableTrades.Invalidate();
      this.ClosedTradesUpdate();
		}
		#endregion
		#region private void TradesTableRowUpdate(TableRow row, ITrade trade)
		private void TradesTableRowUpdate(TableRow row, ITrade trade){

			string stoprate = "", limitrate = "";
			int decdig = trade.OnlineRate.Symbol.DecimalDigits;
			if (trade.StopOrder != null){
				stoprate = SymbolManager.ConvertToCurrencyString(trade.StopOrder.Rate, decdig);
			}
			if (trade.LimitOrder != null)
				limitrate = SymbolManager.ConvertToCurrencyString(trade.LimitOrder.Rate, decdig);
			

			float closeprice = trade.TradeType == TradeType.Sell ? trade.OnlineRate.BuyRate : trade.OnlineRate.SellRate;

			string strcloserate = SymbolManager.ConvertToCurrencyString(closeprice, decdig);

			row.AdditionObject = trade;
			int n=0;
			row[n++].Text = trade.Account.AccountId;
			row[n++].Text = trade.TradeId;
      row[n++].Text = trade.OpenTime.ToShortDateString() + " " + trade.OpenTime.ToShortTimeString();
      row[n++].Text = BrokerCommandManager.GetLngBuySell(trade.TradeType);
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.Lots * trade.OnlineRate.LotSize, 0, " ");
      row[n++].Text = trade.OnlineRate.Symbol.Name;
			row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.OpenRate, decdig);
//      row[n++].Text = trade.NetPLPoint.ToString();
			row[n++].Text = stoprate;
			row[n++].Text = limitrate;
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.CloseRate, decdig);
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.Premium, 2, " ");
			row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.Commission, 2, " ");
			row[n++].Text = SymbolManager.ConvertToCurrencyString(-trade.Fee, 2, " ");
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.NetPL, 2, " ");
		}
		#endregion
		
    #region private bool TradesTablePairUpdate(IOnlineRate onlineRate)
		private bool TradesTablePairUpdate(IOnlineRate onlineRate){
			bool isrefresh = false;
			foreach (TableRow row in this._tableTrades.Rows){
				ITrade trade = row.AdditionObject as ITrade;
				if (trade.OnlineRate == onlineRate){
					this.TradesTableRowUpdate(row, trade);
					isrefresh = true;
				}
			}
			if (isrefresh)
				this._tableTrades.Invalidate();
			return isrefresh;
		}
		#endregion

    #region private void ClosedTradesUpdate()
    private void ClosedTradesUpdate() {
      this._tableClosedTrades.Rows.Clear();

//      for (int i = 0; i < this.BCM.Broker.ClosedTrades.Count; i++) {
      for (int i = this.BCM.Broker.ClosedTrades.Count-1; i >=0; i--) {
        IClosedTrade trade = BCM.Broker.ClosedTrades[i];
        BrokerCloseTradeRecordAdd(_tableClosedTrades, trade);
      }
      this._tableClosedTrades.Invalidate();
    }
    #endregion

    #region private bool IsViewOrderInTable(IOrder order)
    private bool IsViewOrderInTable(IOrder order){
			switch(order.OrderType){
				case OrderType.Limit:
				case OrderType.Stop:
					return false;
			}
			return true;
		}
		#endregion

		#region public void OrdersTableInit()
    public void OrdersTableInit() {
      this._ordersOperatePanel.Order = null;
      this._tableOrders.Rows.Clear();
      for (int i = 0; i < this.BCM.Broker.Orders.Count; i++) {
        IOrder order = this.BCM.Broker.Orders[i];
        if (IsViewOrderInTable(order)) {
          TableRow row = _tableOrders.NewRow();
          this.OrdersTableRowUpdate(row, order);
          _tableOrders.Rows.AddRow(row);
        }
      }
    }
		#endregion
		
		#region public void OrdersTableUpdate(IOrder order)
		public void OrdersTableUpdate(IOrder order){
      if (!IsViewOrderInTable(order)) {
        foreach (TableRow row in this._tableOrders.Rows) {
          IOrder forder = row.AdditionObject as IOrder;
          this.OrdersTableRowUpdate(row, forder);
          this.Invalidate();
        }
      } else {
        foreach (TableRow row in this._tableOrders.Rows) {
          IOrder forder = row.AdditionObject as IOrder;
          if (forder.OrderId == order.OrderId) {
            this.OrdersTableRowUpdate(row, order);
            this.Invalidate();
            break;
          }
        }
      }
		}
		#endregion
		#region public void OrdersTableAdd(IOrder order)
		public void OrdersTableAdd(IOrder order){
			if (!IsViewOrderInTable(order)) return;

			TableRow row = this._tableOrders.NewRow();
			this.OrdersTableRowUpdate(row, order);
			this._tableOrders.Rows.AddRow(row);
		}
		#endregion
		#region public void OrdersTableDelete(IOrder order)
		public void OrdersTableDelete(IOrder order){
			if (!IsViewOrderInTable(order)) return;
			for (int i=0;i<_tableOrders.Rows.Count;i++){
				TableRow row = _tableOrders.Rows[i];
				IOrder corder = row.AdditionObject as IOrder;
				if (order == corder){
					_tableOrders.Rows.RemoveAt(i);
					break;
				}
			}
		}
		#endregion
		#region public void OrdersTableRowUpdate(TableRow row, IOrder order)
		public void OrdersTableRowUpdate(TableRow row, IOrder order){
			row.AdditionObject = order;
			string orderprice = SymbolManager.ConvertToCurrencyString(order.Rate, order.OnlineRate.Symbol.DecimalDigits);

			string stopprice = "", limitprice = "";
			if (order.StopOrder != null)
				stopprice = SymbolManager.ConvertToCurrencyString(order.StopOrder.Rate, order.OnlineRate.Symbol.DecimalDigits);
			if (order.LimitOrder != null)
				limitprice = SymbolManager.ConvertToCurrencyString(order.LimitOrder.Rate, order.OnlineRate.Symbol.DecimalDigits);
			
			int n=0;
      row[n++].Text = order.Account.AccountId;
			row[n++].Text = order.OrderId;
//			row[n++].Text = order.TradeId;
			row[n++].Text = order.OnlineRate.Symbol.Name;
			row[n++].Text = BrokerCommandManager.GetLngOrderType(order.OrderType);
			row[n++].Text = BrokerCommandManager.GetLngBuySell(order.TradeType);
			row[n++].Text = SymbolManager.ConvertToCurrencyString(order.Lots, 0, " ");
			row[n++].Text = orderprice;
			row[n++].Text = stopprice;
			row[n++].Text = limitprice;
			row[n++].Text = order.Time.ToString();
		}
		#endregion

		#region Component events

		#region private void _tableTrades_SelectedIndexChanged() 
    private void _tableTrades_SelectedIndexChanged(object sender, EventArgs e) {
			TableRow row = this._tableTrades.SelectedRow;
			ITrade trade = null;
			if (row != null)
				trade = row.AdditionObject as ITrade;
      this._tradesOperatePanel.Trade = trade;
			foreach (TableRow frow in this._tableTrades.Rows){
				if (frow == row){
					frow.Style = _styledefaccout;
				}else{
					frow.Style = null;
				}
			}
			this._tableTrades.Invalidate();
		}
		#endregion

		#region private void _tableOrders_SelectedIndexChanged() 
		private void _tableOrders_SelectedIndexChanged(object sender, EventArgs e) {
			TableRow row = this._tableOrders.SelectedRow;
			IOrder order = null;
			if (row != null)
				order = row.AdditionObject as IOrder;
      this._ordersOperatePanel.Order = order;
			foreach (TableRow frow in this._tableOrders.Rows){
				if (frow == row){
					frow.Style = _styledefaccout;
				}else{
					frow.Style = null;
				}
			}
		}
		#endregion

		#region private void _tableTrades_Click(object sender, System.EventArgs e) 
		private void _tableTrades_Click(object sender, System.EventArgs e) {
			this._tableTrades_SelectedIndexChanged(sender, e);
		}
		#endregion

		#region private void _tableTrades_DoubleClick(object sender, System.EventArgs e) 
		private void _tableTrades_DoubleClick(object sender, System.EventArgs e) {
			TableRow row = this._tableTrades.SelectedRow;
			if (row != null){
				ITrade trade = row.AdditionObject as ITrade;
        GordagoMain.MainForm.ChartShowForm(trade.OnlineRate.Symbol);
			}
		}
		#endregion

		#region private void _tableOrders_Click(object sender, System.EventArgs e) 
		private void _tableOrders_Click(object sender, System.EventArgs e) {
			this._tableOrders_SelectedIndexChanged(sender, e);
		}
		#endregion

		#region private void _tableOrders_DoubleClick(object sender, System.EventArgs e) 
		private void _tableOrders_DoubleClick(object sender, System.EventArgs e) {
			TableRow row = this._tableOrders.SelectedRow;
			if (row != null){
				IOrder order = row.AdditionObject as IOrder;
        GordagoMain.MainForm.ChartShowForm(order.OnlineRate.Symbol);
			}
		}
		#endregion

		#region private void _btnSetAsDefaultAccount_Click(object sender, System.EventArgs e) 
		private void _btnSetAsDefaultAccount_Click(object sender, System.EventArgs e) {
			TableRow row = this._tableAccounts.SelectedRow;
			if (row == null)
				return;

			IAccount account = row.AdditionObject as IAccount;
      GordagoMain.MainForm.DefaultAccountId = account.AccountId;
      Config.Users["Terminal"]["AccoutId"].SetValue(GordagoMain.MainForm.DefaultAccountId);
			this.UpdateAccountsTable();
      GordagoMain.MainForm.UpdateStatusString();
		}
		#endregion
		#endregion

    #region private void AddLog(string text)
    private void AddLog(string text) {
      //try {
      //  text = DateTime.Now.ToString() + " " + text;
      //  this._lstLog.Items.Insert(0, text);
      //} catch { }
    }
    #endregion

    #region public void BrokerConnectionStatusChanged(BrokerConnectionStatus status)
    public void BrokerConnectionStatusChanged(BrokerConnectionStatus status) {
      this._sulines.BrokerConnectionStatusChanged(status);
      switch (status) {
        case BrokerConnectionStatus.LoadingData:
          this.AddLog(BrokerCommandManager.LNG_CMD_LOADINGDATA);
          break;
        case BrokerConnectionStatus.WaitingForConnection:
          this.AddLog(BrokerCommandManager.LNG_CMD_LOGON_WAITING);
          break;
        case BrokerConnectionStatus.Offline:
          this.AddLog(BrokerCommandManager.LNG_CMD_OFFLINE);
          this._tableAccounts.Rows.Clear();
          this._tableOrders.Rows.Clear();
          this._tableTrades.Rows.Clear();
          this._tableClosedTrades.Rows.Clear();
          this._tableAccounts.Dock = DockStyle.Fill;
          this._btnSetAsDefaultAccount.Visible = false;
          break;
        case BrokerConnectionStatus.Online:
          this.AddLog(BrokerCommandManager.LNG_CMD_LOGON_ONLINE);
          if (this.BCM.Broker.Accounts.Count > 1) {
            this._tableAccounts.Dock = DockStyle.None;
            this._btnSetAsDefaultAccount.Visible = true;
          }
          this.UpdateAccountsTable();
          this.TradesTableInit();
          this.OrdersTableInit();
          this.ClosedTradesUpdate();
          break;
      }
      _tradesOperatePanel.BrokerConnectionStatusChanged(status);
      _ordersOperatePanel.BrokerConnectionStatusChanged(status);
      this.Refresh();
    }
    #endregion

    #region public void BrokerAccountsChanged(BrokerAccountsEventArgs be)
    public void BrokerAccountsChanged(BrokerAccountsEventArgs be) {
      this.UpdateAccountsTable();
    }
    #endregion

    #region public void BrokerOrdersChanged(BrokerOrdersEventArgs be)
    public void BrokerOrdersChanged(BrokerOrdersEventArgs be) {
      switch (be.MessageType) {
        case BrokerMessageType.Add:
          this.OrdersTableAdd(be.Order);
          break;
        case BrokerMessageType.Delete:
          this._ordersOperatePanel.Order = null;
          this.OrdersTableDelete(be.Order);
          break;
        case BrokerMessageType.Update:
          this.OrdersTableUpdate(be.Order);
          break;
      }
      this._tableOrders_SelectedIndexChanged(null, null);
      this._tableOrders.Invalidate();
    }
    #endregion

    #region public void BrokerTradesChanged(BrokerTradesEventArgs be)
    public void BrokerTradesChanged(BrokerTradesEventArgs be) {
      switch (be.MessageType) {
        case BrokerMessageType.Add:
          this.TradesTableAdd(be.Trade);
          break;
        case BrokerMessageType.Delete:
          this._tradesOperatePanel.Trade = null;
          this.TradesTableDelete(be.Trade);
          break;
        case BrokerMessageType.Update:
          this.TradesTableUpdate(be.Trade);
          break;
      }
      this._tableOrders_SelectedIndexChanged(null, null);
      this._tableTrades.Invalidate();
    }
    #endregion

    #region public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be)
    public void BrokerOnlineRatesChanged(BrokerOnlineRatesEventArgs be) {
      try {
        this._tradesOperatePanel.BrokerOnlineRatesChanged(be);
        this._ordersOperatePanel.BrokerOnlineRatesChanged(be);

        if (this.TradesTablePairUpdate(be.OnlineRate))
          this.UpdateAccountsTable();
      } catch { }
    }
    #endregion

    #region private void SetJournalTableColumns()
    private void SetJournalTableColumns() {
      BrokerJournalInitializeTable(_tableJournal);

      string cfgs = Config.Users["Terminal"]["JournalTableWidths", ""];
      _tableJournal.Columns.SetConfigWidths(cfgs);
    }
    #endregion

    #region public static void BrokerCloseTradeInitializeTable(TableControl table)
    public static void BrokerCloseTradeInitializeTable(TableControl table) {
      table.Columns.Add(BrokerCommandManager.LNG_TBL_ACCOUNT, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_TRADE, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_SYMBOL, TableColumnType.Label, 65);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_BUYSELL, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_AMOUNT, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_OPENRATE, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_CLOSERATE, TableColumnType.Label, 55);
      //table.Columns.Add(BrokerCommandManager.LNG_TBL_STOP, TableColumnType.Label, 55);
      //table.Columns.Add(BrokerCommandManager.LNG_TBL_LIMIT, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_PREMIUM, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_COMMISION, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_FEE, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_NETPIRCE, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_OPENTIME, TableColumnType.Label, 55);
      table.Columns.Add(BrokerCommandManager.LNG_TBL_CLOSETIME, TableColumnType.Label, 55);
    }
    #endregion

    #region public static void BrokerCloseTradeRecordAdd(TableControl table, IClosedTrade trade)
    public static void BrokerCloseTradeRecordAdd(TableControl table, IClosedTrade trade) {
      TableRow row = table.NewRow();

      string stoprate = "", limitrate = "";

      ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(trade.SymbolName);
      int decdig = symbol == null ? 4 : symbol.DecimalDigits;

      if (trade.StopOrderRate > 0)
        stoprate = SymbolManager.ConvertToCurrencyString(trade.StopOrderRate, decdig);

      if (trade.LimitOrderRate > 0)
        limitrate = SymbolManager.ConvertToCurrencyString(trade.LimitOrderRate, decdig);

      string strcloserate = SymbolManager.ConvertToCurrencyString(trade.CloseRate, decdig);

      row.AdditionObject = trade;
      int n = 0;
      row[n++].Text = trade.AccountId;
      row[n++].Text = trade.TradeId;
      row[n++].Text = trade.SymbolName;
      row[n++].Text = BrokerCommandManager.GetLngBuySell(trade.TradeType);
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.Amount, 0, " ");
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.OpenRate, decdig);
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.CloseRate, decdig);
      //row[n++].Text = stoprate;
      //row[n++].Text = limitrate;
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.Premium, 2, " ");
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.Commission, 2, " ");
      row[n++].Text = SymbolManager.ConvertToCurrencyString(-trade.Fee, 2, " ");
      row[n++].Text = SymbolManager.ConvertToCurrencyString(trade.NetPL, 2, " ");
      row[n++].Text = trade.OpenTime.ToShortDateString() + " " + trade.OpenTime.ToShortTimeString();
      row[n++].Text = trade.CloseTime.ToShortDateString() + " " + trade.CloseTime.ToShortTimeString();
      table.Rows.AddRow(row);
    }
    #endregion

    #region public static void BrokerJournalInitializeTable(TableControl table)
    public static void BrokerJournalInitializeTable(TableControl table) {
      table.Columns.Add(Dictionary.GetString(30, 108, "����"), TableColumnType.Label, 60);
      table.Columns.Add(Dictionary.GetString(30, 109, "�����"), TableColumnType.Label, 60);
      table.Columns.Add(Dictionary.GetString(30, 21, "���� �"), TableColumnType.Label, 60);
      table.Columns.Add(Dictionary.GetString(30, 13, "������"), TableColumnType.Label, 60);
      table.Columns.Add(Dictionary.GetString(30, 110, "���������"), TableColumnType.Label, 800);
    }
    #endregion

    #region public static void BrokerJournalRecordAdd(TableControl table, BrokerJournalRecord rec)
    public static void BrokerJournalRecordAdd(TableControl table, BrokerJournalRecord rec) {
      string msg = "";

      if (rec is BJRConnectionStatus) {
        BJRConnectionStatus bjr = rec as BJRConnectionStatus;
        if (bjr.ConnectionStatus == BrokerConnectionStatus.Online) {
          msg += BrokerCommandManager.LNG_TBL_TYPEACCOUNT + ": " + bjr.MoneyOwner + ", ";
          msg += BrokerCommandManager.LNG_TBL_CAPITAL + ": " + SymbolManager.ConvertToCurrencyString(bjr.Equity, 2, " ") + ", ";
          msg += BrokerCommandManager.LNG_TBL_USEDMARGIN + ": " + SymbolManager.ConvertToCurrencyString(bjr.UsedMargin, 0, " ") + ", ";
          msg += BrokerCommandManager.LNG_TBL_FREEMARGIN + ": " + SymbolManager.ConvertToCurrencyString(bjr.UsableMargin, 2, " ") + ", ";
          msg += BrokerCommandManager.LNG_TBL_LOTS + ": " + Convert.ToString(bjr.Lots) + ", ";
          msg += BrokerCommandManager.LNG_TBL_PREMIUM + ": " + SymbolManager.ConvertToCurrencyString(bjr.Premium, 2, " ") + ", ";
          msg += BrokerCommandManager.LNG_TBL_NETPLALL + ": " + SymbolManager.ConvertToCurrencyString(bjr.NetPL, 2, " ");
        } else {
          msg = "Offline";
        }
      } else if (rec is BJRTrade) {
        BJRTrade bjr = rec as BJRTrade;
        if (bjr.MessageType == BrokerMessageType.Add) {
          msg = "Trade Open: ";
        } else if (bjr.MessageType == BrokerMessageType.Update) {
          msg = "Trade Modify: ";
        } else if (bjr.MessageType == BrokerMessageType.Delete) {
          msg = "Trade Close: ";
        }

        int decdig = bjr.SymbolDecimalDigits;
        msg += "#" + bjr.TradeId + ", ";
        msg += bjr.SymbolName + ", ";
        msg += BrokerCommandManager.GetLngBuySell(bjr.TradeType) + ", ";
        msg += SymbolManager.ConvertToCurrencyString(bjr.Amount, 0, " ") + ", ";
        msg += BrokerCommandManager.LNG_TBL_OPENRATE + ": " + SymbolManager.ConvertToCurrencyString(bjr.OpenRate, decdig) + ", ";
        msg += BrokerCommandManager.LNG_TBL_NETPIRCE + ": " + SymbolManager.ConvertToCurrencyString(bjr.NetPL, 2, " ") + ", ";
        msg += BrokerCommandManager.LNG_TBL_CLOSERATE + ": " + SymbolManager.ConvertToCurrencyString(bjr.CloseRate, decdig) + ", ";
        if (bjr.StopRate > 0)
          msg += BrokerCommandManager.LNG_TBL_STOP + ": " + SymbolManager.ConvertToCurrencyString(bjr.StopRate, decdig) + ", ";
        if (bjr.LimitRate > 0)
          msg += BrokerCommandManager.LNG_TBL_LIMIT + ": " + SymbolManager.ConvertToCurrencyString(bjr.LimitRate, decdig) + ", ";
        msg += BrokerCommandManager.LNG_TBL_PREMIUM + ": " + SymbolManager.ConvertToCurrencyString(bjr.Premium, 2, " ") + ", ";
        msg += BrokerCommandManager.LNG_TBL_COMMISION + ": " + SymbolManager.ConvertToCurrencyString(bjr.Commission, 2, " ") + ", ";
        msg += BrokerCommandManager.LNG_TBL_FEE + ": " + SymbolManager.ConvertToCurrencyString(bjr.Fee, 2, " ");

      } else if (rec is BJROrder) {
        BJROrder bjr = rec as BJROrder;
        if (bjr.OrderType == OrderType.Limit || bjr.OrderType == OrderType.Stop)
          return;

        if (bjr.MessageType == BrokerMessageType.Add) {
          msg = "Order Create: ";
        } else if (bjr.MessageType == BrokerMessageType.Update) {
          msg = "Order Modify: ";
        } else if (bjr.MessageType == BrokerMessageType.Delete) {
          msg = "Order Delete: ";
        }
        int decdig = bjr.SymbolDecimalDigits;
        msg += "#" + bjr.OrderId + ", ";
        msg += bjr.SymbolName + ", ";
        msg += BrokerCommandManager.GetLngOrderType(bjr.OrderType) + ", ";
        msg += BrokerCommandManager.GetLngBuySell(bjr.TradeType) + ", ";
        msg += bjr.Amount.ToString() + ", ";
        msg += BrokerCommandManager.LNG_TBL_OPENRATE + ": " + SymbolManager.ConvertToCurrencyString(bjr.Rate, decdig) + ", ";
        if (bjr.StopRate > 0)
          msg += BrokerCommandManager.LNG_TBL_STOP + ": " + SymbolManager.ConvertToCurrencyString(bjr.StopRate, decdig) + ", ";
        if (bjr.LimitRate > 0)
          msg += BrokerCommandManager.LNG_TBL_LIMIT + ": " + SymbolManager.ConvertToCurrencyString(bjr.LimitRate, decdig);
      } else if (rec is BJRComment){
        msg = (rec as BJRComment).Comment;
      }

      TableRow row = table.NewRow();
      row.AdditionObject = rec;
      row[0].Text = rec.Time.ToShortDateString();
      row[1].Text = rec.Time.ToShortTimeString();

      if (rec is BJRAccount) {
        BJRAccount bjr = rec as BJRAccount;
        row[2].Text = bjr.AccountId;
        row[3].Text = SymbolManager.ConvertToCurrencyString(bjr.Balance);
      }
      row[4].Text = msg;
      table.Rows.AddRow(row);
      table.Invalidate();
    }
    #endregion

    #region public void BrokerJournalRecordAdded(BrokerJornalEventArgs e)
    public void BrokerJournalRecordAdded(BrokerJornalEventArgs e) {
      BrokerJournalRecordAdd(_tableJournal, e.Record);
    }
    #endregion

    #region public void BrokerCommandStarting(BrokerCommand command)
    public void BrokerCommandStarting(BrokerCommand command) {
      this._tradesOperatePanel.BrokerCommandStarting(command);
      this._ordersOperatePanel.BrokerCommandStarting(command);

      if (command is BrokerCommandLogon) {
        this.AddLog(BrokerCommandManager.LNG_CMD_LOGON_START);
      }else if (command is BrokerCommandTradeOpen){

      }

      //switch (command) {
      //  #region case APICommand.ChangeEntryOrder:
      //  case APICommand.ChangeEntryOrder:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_CHANGEENTRYORDER);
      //    break;
      //  #endregion
      //  #region case APICommand.ChangeStopLimitTrailOnEntryOrder:
      //  case APICommand.ChangeStopLimitTrailOnOrder:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_CHANGE_SLT_ENTRYORDER);
      //    break;
      //  #endregion
      //  #region case APICommand.ChangeStopLimitTrailOnTrade:
      //  case APICommand.ChangeStopLimitTrailOnTrade:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_CHANGE_SLT_TRADE);
      //    break;
      //  #endregion
      //  #region case APICommand.CloseTrade:
      //  case APICommand.CloseTrade:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_CLOSETRADE);
      //    break;
      //  #endregion
      //  #region case APICommand.CreateEntryOrder:
      //  case APICommand.CreateOrder:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_CREATEENTRYORDER);
      //    break;
      //  #endregion
      //  #region case APICommand.CreateInitOrder:
      //  case APICommand.CreateTrade:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_CREATETRADE);
      //    break;
      //  #endregion
      //  #region case APICommand.DeleteOrder:
      //  case APICommand.DeleteOrder:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_DELETEORDER);
      //    break;
      //  #endregion
      //  #region case APICommand.AcceptRejectedOrder:
      //  case APICommand.AcceptRejectedOrder:
      //    this.AddLog(BrokerCommandManager.LNG_CMD_ACCEPTREJECTORDER);
      //    break;
      //  #endregion
      //}
    }
    #endregion

    #region public void BrokerCommandStopping(BrokerCommand command, BrokerResult result)
    public void BrokerCommandStopping(BrokerCommand command, BrokerResult result) {
      this._tradesOperatePanel.BrokerCommandStopping(command, result);
      this._ordersOperatePanel.BrokerCommandStopping(command, result);

      //switch (command) {
      //  #region case APICommand.Logon:
      //  case APICommand.Logon:
      //    if (error == null) {
      //      //						this.AddLog(TraderAPI.LNG_CMD_LOGON_ONLINE);
      //    } else {
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_LOGON_ERROR, error.Message));
      //    }
      //    break;
      //  #endregion
      //  #region case APICommand.ChangeEntryOrder:
      //  case APICommand.ChangeEntryOrder:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_CHANGEENTRYORDER_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_CHANGEENTRYORDER_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.ChangeStopLimitTrailOnEntryOrder:
      //  case APICommand.ChangeStopLimitTrailOnOrder:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_CHANGE_SLT_ENTRYORDER_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_CHANGE_SLT_ENTRYORDER_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.ChangeStopLimitTrailOnTrade:
      //  case APICommand.ChangeStopLimitTrailOnTrade:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_CHANGE_SLT_TRADE_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_CHANGE_SLT_TRADE_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.CloseTrade:
      //  case APICommand.CloseTrade:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_CLOSETRADE_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_CLOSETRADE_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.CreateEntryOrder:
      //  case APICommand.CreateOrder:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_CREATEENTRYORDER_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_CREATEENTRYORDER_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.CreateInitOrder:
      //  case APICommand.CreateTrade:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_CREATETRADE_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_CREATETRADE_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.DeleteOrder:
      //  case APICommand.DeleteOrder:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_DELETEORDER_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_DELETEORDER_ERROR, error.Message));
      //    break;
      //  #endregion
      //  #region case APICommand.AcceptRejectedOrder:
      //  case APICommand.AcceptRejectedOrder:
      //    if (error == null)
      //      this.AddLog(BrokerCommandManager.LNG_CMD_ACCEPTREJECTORDER_STOP);
      //    else
      //      this.AddLog(string.Format(BrokerCommandManager.LNG_CMD_ACCEPTREJECTORDER_ERROR, error.Message));
      //    break;
      //  #endregion
      //}
    }
    #endregion

    #region private void _ordersOperatePanel_StatusChanged(object sender, EventArgs e)
    private void _ordersOperatePanel_StatusChanged(object sender, EventArgs e) {
      this.SuspendLayout();
      this._tbpOrders.SuspendLayout();
      this._tableOrders.SuspendLayout();
      this._tbcMain.SuspendLayout();

      Rectangle rect = this._tbpOrders.ClientRectangle;
      int left = _tableOrders.Left = this._ordersOperatePanel.Width + 3;
      _tableOrders.Width = rect.Width - left;
      
      this._tbcMain.ResumeLayout();
      this._tableOrders.ResumeLayout(false);
      this._tbpOrders.ResumeLayout(false);
      this.ResumeLayout(false);
    }
    #endregion

    #region private void _tradesOperatePanel_StatusChanged(object sender, EventArgs e)
    private void _tradesOperatePanel_StatusChanged(object sender, EventArgs e) {
      this.SuspendLayout();
      this._tbpTrades.SuspendLayout();
      this._tableTrades.SuspendLayout();
      this._tbcMain.SuspendLayout();

      Rectangle rect = this._tbpTrades.ClientRectangle;
      int left = _tableTrades.Left = this._tradesOperatePanel.Width + 3;
      _tableTrades.Width = rect.Width - left;

      this._tbcMain.ResumeLayout(false);
      this._tableTrades.ResumeLayout(false);
      this._tbpTrades.ResumeLayout(false);
      this.ResumeLayout(false);
    }
    #endregion

    #region public void MainFormMdiChildActivate()
    public void MainFormMdiChildActivate() { 
      _tradesOperatePanel.MainFormMdiChildActivate();
      _ordersOperatePanel.MainFormMdiChildActivate();
    }
    #endregion
  }
}
