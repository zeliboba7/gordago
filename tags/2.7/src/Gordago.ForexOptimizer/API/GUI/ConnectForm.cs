/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Gordago.WebUpdate;
using Cursit.Applic.AConfig;
using Language;
using System.Diagnostics;
using Gordago.API.VirtualForex;
#endregion

namespace Gordago.API {
	class ConnectForm : System.Windows.Forms.Form {

		#region private property
		private System.Windows.Forms.Label _lblLogin;
		private System.Windows.Forms.TextBox _txtLogin;
		private System.Windows.Forms.TextBox _txtPass;
		private System.Windows.Forms.Label _lblPass;
		private System.Windows.Forms.Button _btnConnectBroker;
		private System.Windows.Forms.Button _btnWokOffline;
		private System.Windows.Forms.Button _btnSetting;
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.ComboBox _cmbAType;
		private System.Windows.Forms.Label _lblAType;
		private System.Windows.Forms.GroupBox _gboxServerBroker;
		private bool _isdemo;
		private System.Windows.Forms.LinkLabel _lnkOpenAccount;
		private System.Windows.Forms.GroupBox _gboxOpenAccount;
    private GroupBox groupBox3;
    private CheckBox _chkToShowAtStartup;
    private ProxySetting _proxy;
    private GroupBox _gboxVirtualServer;
    private DateTimePicker _dtmVSFromTime;
    private TrackBar _trackBar;
    private TabControl _tbcMain;
    private TabPage _tbpServerOfBroker;
    private TabPage _tbpVirtualServer;
    private Gordago.API.VirtualForex.VSSettingsPanel _vsSettings;
    private Label _lblVSDate;

    private ConfigValue _cfgval;
    #endregion

    private DateTime _startVSTime, _endVSTime;
    private CheckBox _chkUseAllSymbols;
    private Button _btnConnectVServer;

    public ConnectForm() {

      InitializeComponent();
      this.Icon = Gordago.Properties.Resources.MainIcon;
      this._cmbAType.Items.Add("Demo");
      this._cmbAType.Items.Add("Live");

      _cfgval = Config.Users["API"];

      this.IsDemo = _cfgval["Demo", true];
      this.UserName = _cfgval["Login", ""];
      byte[] userpasskey = _cfgval["Key", new byte[] { }];
      this._chkToShowAtStartup.Checked = GetConfigShowAtStartup();

      this.IsVirtualServer = _cfgval["IsVS", true];

      this.UserPass = Cursit.Utils.Password.CreatePass(UserName, userpasskey);
      _proxy = GordagoMain.MainForm.LoadProxySetting();

      #region Dictionary
      this.Text = Dictionary.GetString(31, 1, "Connect");
      this._lblLogin.Text = Dictionary.GetString(31, 2, "User Name");
      this._lblPass.Text = Dictionary.GetString(31, 3, "Password:");
      this._lblAType.Text = Dictionary.GetString(31, 4, "Account Type:");
      this._btnWokOffline.Text = Dictionary.GetString(31, 6, "Work offline");
      this._btnSetting.Text = Dictionary.GetString(31, 7, "Settings");
      this._lnkOpenAccount.Text = Dictionary.GetString(31, 8, "Open account");
      this._chkToShowAtStartup.Text = Dictionary.GetString(31, 11, "To show at startup");
      this._tbpServerOfBroker.Text = Dictionary.GetString(31, 12, "Сервер брокера");
      this._tbpVirtualServer.Text = Dictionary.GetString(31, 13, "Виртуальный сервер");
      this._btnConnectBroker.Text = Dictionary.GetString(31, 5, "Connect");
      this._btnConnectVServer.Text = Dictionary.GetString(31, 14, "Start a Virtual Server");
      this._lblVSDate.Text = Dictionary.GetString(31, 21, "Дата старта");
      this._chkUseAllSymbols.Text = Dictionary.GetString(31, 22, "Использовать все инструменты");
      #endregion

      _chkUseAllSymbols.Checked = _cfgval["VSUseAllSmb", false];

      /* Определим положение дат с историями */
      DateTime[] dtms = VirtualForex.VirtualBroker.GetPeriodHistory(GordagoMain.MainForm.Symbols);
      _startVSTime = dtms[0];
      _endVSTime = dtms[1];
      DateTime startTime = _cfgval["VSTime", DateTime.Now];

      if (_startVSTime.Ticks > (new DateTime(1970, 1, 1)).Ticks) {

        if (startTime.Ticks < _startVSTime.Ticks || startTime.Ticks > _endVSTime.Ticks)
          startTime = _startVSTime;

        long hoursWidth = (_endVSTime.Ticks - _startVSTime.Ticks) / 10000000L / 3600;
        long current = (startTime.Ticks - _startVSTime.Ticks) / 10000000L / 3600;

        _trackBar.Maximum = (int)hoursWidth;
        _trackBar.Value = (int)current;
        _dtmVSFromTime.MinDate = _startVSTime;
        _dtmVSFromTime.MaxDate = _endVSTime;
        _dtmVSFromTime.Value = startTime;

      } else
        _trackBar.Enabled = false;

      this.VSStartTime = startTime;
    }

    #region public bool UseAllSymbols
    public bool UseAllSymbols {
      get { return this._chkUseAllSymbols.Checked; }
      set { this._chkUseAllSymbols.Checked = value; }
    }
    #endregion

    #region public VSSettingsPanel VSSettings
    public VSSettingsPanel VSSettings {
      get { return _vsSettings; }
    }
    #endregion

    #region public DateTime VSStartTime
    public DateTime VSStartTime {
      get { return this._dtmVSFromTime.Value; }
      set { this._dtmVSFromTime.Value = value; }
    }
    #endregion

    #region public bool IsVirtualServer
    public bool IsVirtualServer {
      get { 
        return _tbcMain.SelectedIndex == 1;
      }
      set {
        _tbcMain.SelectedIndex = value ? 1 : 0;
      }
    }
    #endregion

    #region public ProxySetting ProxySetting
    public ProxySetting ProxySetting{
			get{return _proxy;}
		}
		#endregion

		#region public string UserName
		public string UserName{
			get{
				return this._txtLogin.Text;
			}
			set{this._txtLogin.Text = value;}
		}
		#endregion

		#region public string UserPass
		public string UserPass{
			get{return this._txtPass.Text;}
			set{this._txtPass.Text = value;}
		}
		#endregion

		#region public bool IsDemo
		public bool IsDemo{
			get{
				if (this._cmbAType.SelectedIndex == 1)
					return false;
				return true;
			}
			set{
				this._isdemo = value;
				if (_isdemo)
					this._cmbAType.SelectedIndex = 0;
				else
					this._cmbAType.SelectedIndex = 1;
			}
		}
		#endregion

    #region public bool IsShowAsStartup
    public bool IsShowAsStartup {
      get { return this._chkToShowAtStartup.Checked; }
    }
    #endregion

    #region internal static bool GetConfigShowAtStartup()
    internal static bool GetConfigShowAtStartup() { 
      return Config.Users["API"]["ShowConnAtStart", true];
    }
    #endregion

    #region protected override void Dispose( bool disposing )
    protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
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
      this._lblLogin = new System.Windows.Forms.Label();
      this._txtLogin = new System.Windows.Forms.TextBox();
      this._txtPass = new System.Windows.Forms.TextBox();
      this._lblPass = new System.Windows.Forms.Label();
      this._btnConnectBroker = new System.Windows.Forms.Button();
      this._btnWokOffline = new System.Windows.Forms.Button();
      this._btnSetting = new System.Windows.Forms.Button();
      this._lblAType = new System.Windows.Forms.Label();
      this._cmbAType = new System.Windows.Forms.ComboBox();
      this._gboxServerBroker = new System.Windows.Forms.GroupBox();
      this._lnkOpenAccount = new System.Windows.Forms.LinkLabel();
      this._gboxOpenAccount = new System.Windows.Forms.GroupBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this._chkToShowAtStartup = new System.Windows.Forms.CheckBox();
      this._gboxVirtualServer = new System.Windows.Forms.GroupBox();
      this._chkUseAllSymbols = new System.Windows.Forms.CheckBox();
      this._lblVSDate = new System.Windows.Forms.Label();
      this._dtmVSFromTime = new System.Windows.Forms.DateTimePicker();
      this._vsSettings = new Gordago.API.VirtualForex.VSSettingsPanel();
      this._trackBar = new System.Windows.Forms.TrackBar();
      this._tbcMain = new System.Windows.Forms.TabControl();
      this._tbpServerOfBroker = new System.Windows.Forms.TabPage();
      this._tbpVirtualServer = new System.Windows.Forms.TabPage();
      this._btnConnectVServer = new System.Windows.Forms.Button();
      this._gboxServerBroker.SuspendLayout();
      this._gboxOpenAccount.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this._gboxVirtualServer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._trackBar)).BeginInit();
      this._tbcMain.SuspendLayout();
      this._tbpServerOfBroker.SuspendLayout();
      this._tbpVirtualServer.SuspendLayout();
      this.SuspendLayout();
      // 
      // _lblLogin
      // 
      this._lblLogin.Location = new System.Drawing.Point(8, 18);
      this._lblLogin.Name = "_lblLogin";
      this._lblLogin.Size = new System.Drawing.Size(80, 16);
      this._lblLogin.TabIndex = 0;
      this._lblLogin.Text = "Username";
      this._lblLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _txtLogin
      // 
      this._txtLogin.Location = new System.Drawing.Point(96, 16);
      this._txtLogin.Name = "_txtLogin";
      this._txtLogin.Size = new System.Drawing.Size(160, 20);
      this._txtLogin.TabIndex = 1;
      // 
      // _txtPass
      // 
      this._txtPass.Location = new System.Drawing.Point(96, 48);
      this._txtPass.Name = "_txtPass";
      this._txtPass.PasswordChar = '*';
      this._txtPass.Size = new System.Drawing.Size(160, 20);
      this._txtPass.TabIndex = 1;
      // 
      // _lblPass
      // 
      this._lblPass.Location = new System.Drawing.Point(8, 50);
      this._lblPass.Name = "_lblPass";
      this._lblPass.Size = new System.Drawing.Size(80, 16);
      this._lblPass.TabIndex = 0;
      this._lblPass.Text = "Password";
      this._lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _btnConnectBroker
      // 
      this._btnConnectBroker.Location = new System.Drawing.Point(7, 257);
      this._btnConnectBroker.Name = "_btnConnectBroker";
      this._btnConnectBroker.Size = new System.Drawing.Size(264, 23);
      this._btnConnectBroker.TabIndex = 2;
      this._btnConnectBroker.Text = "Connect";
      this._btnConnectBroker.Click += new System.EventHandler(this._btnconnect_Click);
      // 
      // _btnWokOffline
      // 
      this._btnWokOffline.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnWokOffline.Location = new System.Drawing.Point(12, 376);
      this._btnWokOffline.Name = "_btnWokOffline";
      this._btnWokOffline.Size = new System.Drawing.Size(289, 23);
      this._btnWokOffline.TabIndex = 2;
      this._btnWokOffline.Text = "Work offline";
      this._btnWokOffline.Click += new System.EventHandler(this._btnWokOffline_Click);
      // 
      // _btnSetting
      // 
      this._btnSetting.Location = new System.Drawing.Point(134, 155);
      this._btnSetting.Name = "_btnSetting";
      this._btnSetting.Size = new System.Drawing.Size(124, 23);
      this._btnSetting.TabIndex = 2;
      this._btnSetting.Text = "Settings";
      this._btnSetting.Click += new System.EventHandler(this._btnSetting_Click);
      // 
      // _lblAType
      // 
      this._lblAType.Location = new System.Drawing.Point(8, 82);
      this._lblAType.Name = "_lblAType";
      this._lblAType.Size = new System.Drawing.Size(80, 16);
      this._lblAType.TabIndex = 0;
      this._lblAType.Text = "Account Type";
      this._lblAType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // _cmbAType
      // 
      this._cmbAType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbAType.Location = new System.Drawing.Point(96, 80);
      this._cmbAType.Name = "_cmbAType";
      this._cmbAType.Size = new System.Drawing.Size(160, 21);
      this._cmbAType.TabIndex = 3;
      // 
      // _gboxServerBroker
      // 
      this._gboxServerBroker.Controls.Add(this._lblLogin);
      this._gboxServerBroker.Controls.Add(this._txtLogin);
      this._gboxServerBroker.Controls.Add(this._txtPass);
      this._gboxServerBroker.Controls.Add(this._lblPass);
      this._gboxServerBroker.Controls.Add(this._lblAType);
      this._gboxServerBroker.Controls.Add(this._cmbAType);
      this._gboxServerBroker.Controls.Add(this._btnSetting);
      this._gboxServerBroker.Location = new System.Drawing.Point(7, 6);
      this._gboxServerBroker.Name = "_gboxServerBroker";
      this._gboxServerBroker.Size = new System.Drawing.Size(264, 190);
      this._gboxServerBroker.TabIndex = 4;
      this._gboxServerBroker.TabStop = false;
      // 
      // _lnkOpenAccount
      // 
      this._lnkOpenAccount.Location = new System.Drawing.Point(8, 16);
      this._lnkOpenAccount.Name = "_lnkOpenAccount";
      this._lnkOpenAccount.Size = new System.Drawing.Size(250, 16);
      this._lnkOpenAccount.TabIndex = 5;
      this._lnkOpenAccount.TabStop = true;
      this._lnkOpenAccount.Text = "Open account";
      this._lnkOpenAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this._lnkOpenAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkOpenAccount_LinkClicked);
      // 
      // _gboxOpenAccount
      // 
      this._gboxOpenAccount.Controls.Add(this._lnkOpenAccount);
      this._gboxOpenAccount.Location = new System.Drawing.Point(7, 204);
      this._gboxOpenAccount.Name = "_gboxOpenAccount";
      this._gboxOpenAccount.Size = new System.Drawing.Size(264, 40);
      this._gboxOpenAccount.TabIndex = 6;
      this._gboxOpenAccount.TabStop = false;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this._chkToShowAtStartup);
      this.groupBox3.Location = new System.Drawing.Point(12, 330);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(289, 40);
      this.groupBox3.TabIndex = 7;
      this.groupBox3.TabStop = false;
      // 
      // _chkToShowAtStartup
      // 
      this._chkToShowAtStartup.AutoSize = true;
      this._chkToShowAtStartup.Location = new System.Drawing.Point(8, 17);
      this._chkToShowAtStartup.Name = "_chkToShowAtStartup";
      this._chkToShowAtStartup.Size = new System.Drawing.Size(114, 17);
      this._chkToShowAtStartup.TabIndex = 0;
      this._chkToShowAtStartup.Text = "To show at startup";
      this._chkToShowAtStartup.UseVisualStyleBackColor = true;
      this._chkToShowAtStartup.CheckedChanged += new System.EventHandler(this._chkToShowAtStartup_CheckedChanged);
      // 
      // _gboxVirtualServer
      // 
      this._gboxVirtualServer.Controls.Add(this._chkUseAllSymbols);
      this._gboxVirtualServer.Controls.Add(this._lblVSDate);
      this._gboxVirtualServer.Controls.Add(this._dtmVSFromTime);
      this._gboxVirtualServer.Controls.Add(this._vsSettings);
      this._gboxVirtualServer.Controls.Add(this._trackBar);
      this._gboxVirtualServer.Location = new System.Drawing.Point(3, 3);
      this._gboxVirtualServer.Name = "_gboxVirtualServer";
      this._gboxVirtualServer.Size = new System.Drawing.Size(275, 248);
      this._gboxVirtualServer.TabIndex = 6;
      this._gboxVirtualServer.TabStop = false;
      // 
      // _chkUseAllSymbols
      // 
      this._chkUseAllSymbols.AutoSize = true;
      this._chkUseAllSymbols.Location = new System.Drawing.Point(13, 214);
      this._chkUseAllSymbols.Name = "_chkUseAllSymbols";
      this._chkUseAllSymbols.Size = new System.Drawing.Size(114, 17);
      this._chkUseAllSymbols.TabIndex = 9;
      this._chkUseAllSymbols.Text = "To use all Symbols";
      this._chkUseAllSymbols.UseVisualStyleBackColor = true;
      // 
      // _lblVSDate
      // 
      this._lblVSDate.AutoSize = true;
      this._lblVSDate.Location = new System.Drawing.Point(10, 142);
      this._lblVSDate.Name = "_lblVSDate";
      this._lblVSDate.Size = new System.Drawing.Size(65, 13);
      this._lblVSDate.TabIndex = 8;
      this._lblVSDate.Text = "Date started";
      // 
      // _dtmVSFromTime
      // 
      this._dtmVSFromTime.Checked = false;
      this._dtmVSFromTime.Location = new System.Drawing.Point(109, 138);
      this._dtmVSFromTime.Name = "_dtmVSFromTime";
      this._dtmVSFromTime.Size = new System.Drawing.Size(156, 20);
      this._dtmVSFromTime.TabIndex = 1;
      this._dtmVSFromTime.ValueChanged += new System.EventHandler(this._dtmVSFromTime_ValueChanged);
      // 
      // _vsSettings
      // 
      this._vsSettings.Location = new System.Drawing.Point(7, 13);
      this._vsSettings.Name = "_vsSettings";
      this._vsSettings.Size = new System.Drawing.Size(262, 124);
      this._vsSettings.TabIndex = 7;
      // 
      // _trackBar
      // 
      this._trackBar.BackColor = System.Drawing.SystemColors.Control;
      this._trackBar.Location = new System.Drawing.Point(9, 163);
      this._trackBar.Maximum = 1000;
      this._trackBar.Name = "_trackBar";
      this._trackBar.Size = new System.Drawing.Size(256, 45);
      this._trackBar.TabIndex = 3;
      this._trackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
      this._trackBar.ValueChanged += new System.EventHandler(this._trackBar_ValueChanged);
      // 
      // _tbcMain
      // 
      this._tbcMain.Controls.Add(this._tbpServerOfBroker);
      this._tbcMain.Controls.Add(this._tbpVirtualServer);
      this._tbcMain.Location = new System.Drawing.Point(12, 12);
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(289, 312);
      this._tbcMain.TabIndex = 9;
      // 
      // _tbpServerOfBroker
      // 
      this._tbpServerOfBroker.BackColor = System.Drawing.SystemColors.Control;
      this._tbpServerOfBroker.Controls.Add(this._gboxServerBroker);
      this._tbpServerOfBroker.Controls.Add(this._gboxOpenAccount);
      this._tbpServerOfBroker.Controls.Add(this._btnConnectBroker);
      this._tbpServerOfBroker.Location = new System.Drawing.Point(4, 22);
      this._tbpServerOfBroker.Name = "_tbpServerOfBroker";
      this._tbpServerOfBroker.Padding = new System.Windows.Forms.Padding(3);
      this._tbpServerOfBroker.Size = new System.Drawing.Size(281, 286);
      this._tbpServerOfBroker.TabIndex = 0;
      this._tbpServerOfBroker.Text = "Server Of Broker";
      // 
      // _tbpVirtualServer
      // 
      this._tbpVirtualServer.BackColor = System.Drawing.SystemColors.Control;
      this._tbpVirtualServer.Controls.Add(this._btnConnectVServer);
      this._tbpVirtualServer.Controls.Add(this._gboxVirtualServer);
      this._tbpVirtualServer.Location = new System.Drawing.Point(4, 22);
      this._tbpVirtualServer.Name = "_tbpVirtualServer";
      this._tbpVirtualServer.Padding = new System.Windows.Forms.Padding(3);
      this._tbpVirtualServer.Size = new System.Drawing.Size(281, 286);
      this._tbpVirtualServer.TabIndex = 1;
      this._tbpVirtualServer.Text = "Virtual Server";
      // 
      // _btnConnectVServer
      // 
      this._btnConnectVServer.Location = new System.Drawing.Point(10, 257);
      this._btnConnectVServer.Name = "_btnConnectVServer";
      this._btnConnectVServer.Size = new System.Drawing.Size(264, 23);
      this._btnConnectVServer.TabIndex = 9;
      this._btnConnectVServer.Text = "Connect";
      this._btnConnectVServer.Click += new System.EventHandler(this._btnConnectVServer_Click);
      // 
      // ConnectForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(315, 408);
      this.Controls.Add(this._tbcMain);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this._btnWokOffline);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ConnectForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Connect";
      this._gboxServerBroker.ResumeLayout(false);
      this._gboxServerBroker.PerformLayout();
      this._gboxOpenAccount.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this._gboxVirtualServer.ResumeLayout(false);
      this._gboxVirtualServer.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._trackBar)).EndInit();
      this._tbcMain.ResumeLayout(false);
      this._tbpServerOfBroker.ResumeLayout(false);
      this._tbpVirtualServer.ResumeLayout(false);
      this.ResumeLayout(false);

		}
		#endregion

    #region private void _btnSetting_Click(object sender, System.EventArgs e)
    private void _btnSetting_Click(object sender, System.EventArgs e) {
			ConnectSettingForm csf = new ConnectSettingForm();
			csf.HTTPSettingPanel.SetProxy(_proxy);
			if (csf.ShowDialog() != DialogResult.OK)
				return;
			_proxy = csf.HTTPSettingPanel.GetProxy();
			GordagoMain.MainForm.SaveProxySetting(_proxy);
    }
    #endregion

    #region private void _btnWokOffline_Click(object sender, System.EventArgs e)
    private void _btnWokOffline_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

		#region private void _btnconnect_Click(object sender, System.EventArgs e) 
		private void _btnconnect_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		#endregion

    #region private void _lnkOpenAccount_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    private void _lnkOpenAccount_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
			string link = "";
			if (GordagoMain.Lang == "rus"){
				link = "http://www.gordago.ru/?act=account";
			}else{
				link = "http://www.gordago.com/?act=account";
			}

      link += "&vers=" + Application.ProductVersion +
          "&cst=" + GordagoMain.MainForm.CountStartApp.ToString();

      this.Cursor = Cursors.WaitCursor;
      try {
        ProcessStartInfo psi = new ProcessStartInfo("iexplore", link);
        psi.WorkingDirectory = "C:\\";
        Process.Start(psi);
			}catch{}
      this.Cursor = Cursors.Default;
    }
    #endregion

    #region private void _chkToShowAtStartup_CheckedChanged(object sender, EventArgs e)
    private void _chkToShowAtStartup_CheckedChanged(object sender, EventArgs e) {
    }
    #endregion

    #region private void _btnVSSettings_Click(object sender, EventArgs e)
    private void _btnVSSettings_Click(object sender, EventArgs e) {
      VirtualBrokerSettingsForm vbsForm = new VirtualBrokerSettingsForm();
      vbsForm.ShowDialog();
    }
    #endregion

    #region private void _trackBar_ValueChanged(object sender, EventArgs e)
    private void _trackBar_ValueChanged(object sender, EventArgs e) {
      long current = _startVSTime.Ticks + _trackBar.Value * 10000000L * 3600;
      _dtmVSFromTime.Value = new DateTime(current);
    }
    #endregion

    #region private void _btnConnectVServer_Click(object sender, EventArgs e)
    private void _btnConnectVServer_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region protected override void OnClosed(EventArgs e)
    protected override void OnClosed(EventArgs e) {
      base.OnClosed(e);
      this._vsSettings.Save();
    }
    #endregion

    #region private void _dtmVSFromTime_ValueChanged(object sender, EventArgs e)
    private void _dtmVSFromTime_ValueChanged(object sender, EventArgs e) {
      try {
        _trackBar.Value = (int)((_dtmVSFromTime.Value.Ticks - _startVSTime.Ticks) / 10000000L / 3600);
      }catch{}
    }
    #endregion
  }
}
