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
#endregion

using Language;
using Cursit.Applic.AConfig;
using System.Threading;

namespace Gordago.WebUpdate {
  public class UpdateForm:System.Windows.Forms.Form {

    public static bool IsNewUpdate = false;

		#region private property
		private System.Windows.Forms.Button _btnUpdate;
		private System.Windows.Forms.Label _lblAction;
		private System.ComponentModel.Container components = null;

		private UpdateEngine _ue;
		private bool _processUpdate;

		private System.Windows.Forms.Label _lblLogin;
		private System.Windows.Forms.TextBox _txtLogin;
		private System.Windows.Forms.TextBox _txtPassword;
		private System.Windows.Forms.Label _lblPassword;
		private System.Windows.Forms.CheckBox _chkCommerc;
		private System.Windows.Forms.GroupBox _gboxCommerc;

		#endregion

		public string LngCaption = "Update...";
		public string LngBtnStart = "Start";
		public string LngBtnCancel = "Cancel";
		public string LngFindUpdate = "Идет процесс поиска обновлений...";
		public string LngDownload = "Download:";
		public string LngErrorListFile = "Ошибка получения файла версии";
		public string LngMsgNotUpdates = "На данный момент нет доступных обновлений";
		public string LngErrorProcessUpdate = "Ошибка в процессе обновления";
		public string LngMsgUpdateOk = "Обновление успешно завершено, необходимо перезапустить программу";
		public string LngBtnClose = "Закрыть";
		public string LngLblLogin = "Login";
		public string LngLblPassw = "Password";
		public string LngChkComerc = "Коммерческое";

		public string LngErrOnServer = "Ошибка на стороне сервера";
		public string LngErrAut = "Неправельное имя или пароль";
		public string LngMsgCommercyOnDemo = "Производиться попытка обновление с полной на ДЕМО версию.";

		public static string LngSavePass = "Сохранить";

		private System.Windows.Forms.CheckBox _chkIsSavePass;

    private RichTextBox _txtlogs;
    private ProgressBar _pgsproc;
    private Label _lblCommercy;
    private GroupBox groupBox1;
    private CheckBox _chkCheckUpdateIsStartingProgramm;
		private ProxySetting _proxy;
    private Button _btnGetUP;

    private ConfigValue _cfgval;
    private bool _updateOK = false;

		public UpdateForm() {
			InitializeComponent();

      LngBtnStart = Dictionary.GetString(19, 1);
      LngBtnCancel = Dictionary.GetString(19, 2);
      LngFindUpdate = Dictionary.GetString(19, 3);
      LngDownload = Dictionary.GetString(19, 4);
      LngErrorListFile = Dictionary.GetString(19, 5);
      LngMsgNotUpdates = Dictionary.GetString(19, 6);
      LngErrorProcessUpdate = Dictionary.GetString(19, 7);
      LngMsgUpdateOk = Dictionary.GetString(19, 8);
      LngBtnClose = Dictionary.GetString(19, 9);
      LngLblLogin = Dictionary.GetString(19, 10);
      LngLblPassw = Dictionary.GetString(19, 11);
      LngChkComerc = Dictionary.GetString(19, 12);
      LngErrOnServer = Dictionary.GetString(19, 13);
      LngErrAut = Dictionary.GetString(19, 14);
      LngMsgCommercyOnDemo = Dictionary.GetString(19, 15);
      LngSavePass = Dictionary.GetString(19, 17);

			this._btnUpdate.Text = LngBtnStart;
      this.Text = Dictionary.GetString(19, 19, "Обновление программы");

			this._lblLogin.Text = LngLblLogin;
			this._lblPassword.Text = LngLblPassw;
			this._chkCommerc.Text = "";
      this._lblCommercy.Text = LngChkComerc;
			this._chkIsSavePass.Text = LngSavePass;
      this._chkCheckUpdateIsStartingProgramm.Text = Dictionary.GetString(19, 18, "To check updating at start of the program");
			this.Commercy = false;

      if(IsNewUpdate) {
        this._lblAction.Text = Dictionary.GetString(19, 20, "It is necessary to update the current version");
        this._lblAction.ForeColor = Color.Red;
      }

      _cfgval = Config.Users["Update"];
      this.Commercy = _cfgval["Commercial", false];
      this.IsSavePass = _cfgval["IsSaveLP", false];
      this.Proxy = GordagoMain.MainForm.LoadProxySetting();
      if(this.IsSavePass) {
        this.Login = _cfgval["Login", ""];
        byte[] encpass = _cfgval["Profile", new byte[] { }];

        this.Password = Cursit.Utils.Password.CreatePass(this.Login, encpass);
      }
      this._chkCheckUpdateIsStartingProgramm.Checked = _cfgval["CheckIsStart", true];
      this._btnGetUP.Text = Dictionary.GetString(19, 21, "Получить логин и пароль");
    }

    #region public ProxySetting Proxy
    public ProxySetting Proxy{
			get{return _proxy;}
			set{_proxy = value;}
    }
    #endregion

    #region public bool Commercy
    public bool Commercy{
			get{return this._chkCommerc.Checked;}
			set{
				this._chkCommerc.Checked = value;
				this._gboxCommerc.Enabled = value;
			}
		}
		#endregion

		#region public string Login
		public string Login{
			get{return this._txtLogin.Text;}
			set{
				this._txtLogin.Text = value;
			}
		}
		#endregion

		#region public string Password
		public string Password{
			get{
				return this._txtPassword.Text;
			}
			set{
				this._txtPassword.Text = value;
			}
		}
		#endregion

		#region public string TextAction
		public string TextAction{
			get{return this._lblAction.Text;}
			set{this._lblAction.Text = value;}
		}
		#endregion

    #region public bool CheckUpdateIsStartProg
    public bool CheckUpdateIsStartProg {
      get { return this._chkCheckUpdateIsStartingProgramm.Checked; }
      set { this._chkCheckUpdateIsStartingProgramm.Checked = value; }
    }
    #endregion

    #region public bool IsSavePass
    public bool IsSavePass{
			get{return this._chkIsSavePass.Checked;}
			set{this._chkIsSavePass.Checked = value;}
		}
		#endregion

		#region public bool IsProcessUpdate
		public bool IsProcessUpdate{
			get{return this._processUpdate;}
			set{this._processUpdate = value;}
		}
		#endregion

		#region protected override void Dispose( bool disposing ) 
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region private void InitializeComponent()
		private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
      this._btnUpdate = new System.Windows.Forms.Button();
      this._lblAction = new System.Windows.Forms.Label();
      this._lblLogin = new System.Windows.Forms.Label();
      this._txtLogin = new System.Windows.Forms.TextBox();
      this._txtPassword = new System.Windows.Forms.TextBox();
      this._lblPassword = new System.Windows.Forms.Label();
      this._gboxCommerc = new System.Windows.Forms.GroupBox();
      this._chkIsSavePass = new System.Windows.Forms.CheckBox();
      this._chkCommerc = new System.Windows.Forms.CheckBox();
      this._txtlogs = new System.Windows.Forms.RichTextBox();
      this._pgsproc = new System.Windows.Forms.ProgressBar();
      this._lblCommercy = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._chkCheckUpdateIsStartingProgramm = new System.Windows.Forms.CheckBox();
      this._btnGetUP = new System.Windows.Forms.Button();
      this._gboxCommerc.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _btnUpdate
      // 
      this._btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(210)))), ((int)(((byte)(238)))));
      this._btnUpdate.Location = new System.Drawing.Point(422, 34);
      this._btnUpdate.Name = "_btnUpdate";
      this._btnUpdate.Size = new System.Drawing.Size(75, 56);
      this._btnUpdate.TabIndex = 3;
      this._btnUpdate.Text = "Отменить";
      this._btnUpdate.UseVisualStyleBackColor = false;
      this._btnUpdate.Click += new System.EventHandler(this._btnUpdate_Click);
      // 
      // _lblAction
      // 
      this._lblAction.Location = new System.Drawing.Point(9, 107);
      this._lblAction.Name = "_lblAction";
      this._lblAction.Size = new System.Drawing.Size(488, 24);
      this._lblAction.TabIndex = 1;
      // 
      // _lblLogin
      // 
      this._lblLogin.AutoSize = true;
      this._lblLogin.Location = new System.Drawing.Point(5, 22);
      this._lblLogin.Name = "_lblLogin";
      this._lblLogin.Size = new System.Drawing.Size(38, 13);
      this._lblLogin.TabIndex = 2;
      this._lblLogin.Text = "Логин";
      this._lblLogin.Click += new System.EventHandler(this._lblLogin_Click);
      // 
      // _txtLogin
      // 
      this._txtLogin.Location = new System.Drawing.Point(8, 40);
      this._txtLogin.Name = "_txtLogin";
      this._txtLogin.Size = new System.Drawing.Size(256, 20);
      this._txtLogin.TabIndex = 0;
      // 
      // _txtPassword
      // 
      this._txtPassword.Location = new System.Drawing.Point(272, 40);
      this._txtPassword.Name = "_txtPassword";
      this._txtPassword.PasswordChar = '*';
      this._txtPassword.Size = new System.Drawing.Size(130, 20);
      this._txtPassword.TabIndex = 1;
      // 
      // _lblPassword
      // 
      this._lblPassword.AutoSize = true;
      this._lblPassword.Location = new System.Drawing.Point(269, 22);
      this._lblPassword.Name = "_lblPassword";
      this._lblPassword.Size = new System.Drawing.Size(45, 13);
      this._lblPassword.TabIndex = 2;
      this._lblPassword.Text = "Пароль";
      // 
      // _gboxCommerc
      // 
      this._gboxCommerc.Controls.Add(this._chkIsSavePass);
      this._gboxCommerc.Controls.Add(this._txtLogin);
      this._gboxCommerc.Controls.Add(this._lblPassword);
      this._gboxCommerc.Controls.Add(this._txtPassword);
      this._gboxCommerc.Controls.Add(this._lblLogin);
      this._gboxCommerc.Controls.Add(this._btnGetUP);
      this._gboxCommerc.Location = new System.Drawing.Point(8, 8);
      this._gboxCommerc.Name = "_gboxCommerc";
      this._gboxCommerc.Size = new System.Drawing.Size(408, 96);
      this._gboxCommerc.TabIndex = 5;
      this._gboxCommerc.TabStop = false;
      // 
      // _chkIsSavePass
      // 
      this._chkIsSavePass.Location = new System.Drawing.Point(8, 69);
      this._chkIsSavePass.Name = "_chkIsSavePass";
      this._chkIsSavePass.Size = new System.Drawing.Size(189, 16);
      this._chkIsSavePass.TabIndex = 2;
      this._chkIsSavePass.Text = "checkBox1";
      // 
      // _chkCommerc
      // 
      this._chkCommerc.Location = new System.Drawing.Point(24, 8);
      this._chkCommerc.Name = "_chkCommerc";
      this._chkCommerc.Size = new System.Drawing.Size(25, 16);
      this._chkCommerc.TabIndex = 6;
      this._chkCommerc.CheckedChanged += new System.EventHandler(this._chkCommerc_CheckedChanged);
      // 
      // _txtlogs
      // 
      this._txtlogs.Location = new System.Drawing.Point(3, 158);
      this._txtlogs.Name = "_txtlogs";
      this._txtlogs.Size = new System.Drawing.Size(500, 75);
      this._txtlogs.TabIndex = 7;
      this._txtlogs.Text = "";
      // 
      // _pgsproc
      // 
      this._pgsproc.Location = new System.Drawing.Point(3, 136);
      this._pgsproc.Name = "_pgsproc";
      this._pgsproc.Size = new System.Drawing.Size(500, 17);
      this._pgsproc.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this._pgsproc.TabIndex = 8;
      // 
      // _lblCommercy
      // 
      this._lblCommercy.AutoSize = true;
      this._lblCommercy.Location = new System.Drawing.Point(43, 8);
      this._lblCommercy.Name = "_lblCommercy";
      this._lblCommercy.Size = new System.Drawing.Size(83, 13);
      this._lblCommercy.TabIndex = 3;
      this._lblCommercy.Text = "Коммерческий";
      this._lblCommercy.Click += new System.EventHandler(this._lblCommercy_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._chkCheckUpdateIsStartingProgramm);
      this.groupBox1.Location = new System.Drawing.Point(5, 239);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(496, 37);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      // 
      // _chkCheckUpdateIsStartingProgramm
      // 
      this._chkCheckUpdateIsStartingProgramm.AutoSize = true;
      this._chkCheckUpdateIsStartingProgramm.Location = new System.Drawing.Point(7, 14);
      this._chkCheckUpdateIsStartingProgramm.Name = "_chkCheckUpdateIsStartingProgramm";
      this._chkCheckUpdateIsStartingProgramm.Size = new System.Drawing.Size(315, 17);
      this._chkCheckUpdateIsStartingProgramm.TabIndex = 0;
      this._chkCheckUpdateIsStartingProgramm.Text = "Проверять наличие обновления при запуске программы";
      this._chkCheckUpdateIsStartingProgramm.UseVisualStyleBackColor = true;
      // 
      // _btnGetUP
      // 
      this._btnGetUP.Location = new System.Drawing.Point(203, 66);
      this._btnGetUP.Name = "_btnGetUP";
      this._btnGetUP.Size = new System.Drawing.Size(199, 22);
      this._btnGetUP.TabIndex = 3;
      this._btnGetUP.Text = "Get Login and Password";
      this._btnGetUP.UseVisualStyleBackColor = false;
      this._btnGetUP.Click += new System.EventHandler(this._btnGetUP_Click);
      // 
      // UpdateForm
      // 
      this.AcceptButton = this._btnUpdate;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(506, 283);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this._lblCommercy);
      this.Controls.Add(this._pgsproc);
      this.Controls.Add(this._txtlogs);
      this.Controls.Add(this._chkCommerc);
      this.Controls.Add(this._gboxCommerc);
      this.Controls.Add(this._lblAction);
      this.Controls.Add(this._btnUpdate);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "UpdateForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Обновление программы";
      this._gboxCommerc.ResumeLayout(false);
      this._gboxCommerc.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

		}
		#endregion

    #region private void _btnUpdate_Click(object sender, System.EventArgs e)
    private void _btnUpdate_Click(object sender, System.EventArgs e) {
			if (this._processUpdate){
        if(_ue != null)
          _ue.Abort();
				this.Close();
			}else{
        this._processUpdate = true;

        this._btnUpdate.Text = LngBtnCancel;

        this._lblAction.Text = LngFindUpdate;
        this._lblAction.ForeColor = Color.Black;
				Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(this.ProcessUpdate));
        t.IsBackground = true;
				t.Start();
			}
    }
    #endregion

    #region public bool CheckAutoUpdate(UpdateEngineCheckUpdateResult curesultevent)
    /// <summary>
    /// Автоматически проверить обновление.
    /// При этой проверки не происходит проверки на логин и пароль 
    /// для полной версии
    /// </summary>
    /// <returns></returns>
    public bool CheckAutoUpdate(UpdateEngineCheckUpdateResult curesultevent) {
      
      string login = "free";
      string pass = "";

      if(this.Commercy) {
        login = this.Login;
        pass = this.Password;
      }

      _ue = new UpdateEngine(this.Proxy);
      _ue.CheckUpdateResultEvent += curesultevent;

      UpdateEngineCheckUpdateType uecut = _ue.CheckUpdate(login, pass, true);
      return uecut == UpdateEngineCheckUpdateType.Yes;
    }
    #endregion

    #region private void ProcessUpdate()
    private void ProcessUpdate(){
			string login = "free";
			string pass = "";

			if (this.Commercy){
				login = this.Login;
				pass = this.Password;
			}
      _ue = new UpdateEngine(this.Proxy);

      UpdateEngineCheckUpdateType uecut = _ue.CheckUpdate(login, pass, false);
      switch(uecut) {
        case UpdateEngineCheckUpdateType.ErrorOnServer:
          this.SetEnd(LngErrOnServer);
          return;
        case UpdateEngineCheckUpdateType.ErrorLoginOrPassword:
          this.SetEnd(LngErrAut);
          return;
        case UpdateEngineCheckUpdateType.UpdateRealToDemo:
          this.SetEnd(LngMsgCommercyOnDemo);
          return;
        case UpdateEngineCheckUpdateType.ErrorDownloadListFile:
          this.SetEnd(LngErrorListFile);
          return;
        case UpdateEngineCheckUpdateType.None:
          this.SetEnd(LngMsgNotUpdates);
          return;
      }

      _ue.StartingDownloadFileEvent += new UpdateEngineStartingDownloadFileHandler(this.UE_StartingDownloadFile);
      _ue.StoppingDownloadFileEVent += new UpdateEngineStoppingDownloadFileHandler(this.UI_StoppingDownloadFile);
      _ue.PartDownloadFileEvent += new UpdateEnginePartDownloadFileHandler(this.UI_PartDownloadFile);
                                              
      if(!_ue.DownloadUpdateFiles()) {
        this.SetEnd(LngErrorProcessUpdate);
        return;
      }

      this._updateOK = true;
      this.SetEnd(LngMsgUpdateOk);
      
      GordagoMain.UpdateEngine = _ue;
			UpdateEngine.IsUpdateComplete = true;
    }
    #endregion

    #region private void SetEnd(string msg)
    private void SetEnd(string msg){
      PUEventArgs puea = new PUEventArgs(msg);
      if(!this.InvokeRequired) {
        this._processUpdate_LblSetting(this, puea);
      } else {
        this.Invoke(new EventHandler(this._processUpdate_LblSetting), new object[] {this, puea });
      }
    }
    #endregion

    #region private void UE_StartingDownloadFile(string filename)
    private void UE_StartingDownloadFile(string filename) {
      if(!this.InvokeRequired) {
        this.TextAction = LngDownload + " " + filename;
        string fn = filename.Replace(Application.StartupPath + "\\" + UpdateEngine.TEMP_DIR_DOWNLOAD + "\\", "");
        this._txtlogs.Text += fn;
        this.Refresh();
      } else {
        this.Invoke(new UpdateEngineStartingDownloadFileHandler(this.UE_StartingDownloadFile), new object[] { filename });
      }
    }
    #endregion

    #region private void UI_PartDownloadFile(string filename, int current, int total)
    private void UI_PartDownloadFile(string filename, int current, int total) {
      if(!this.InvokeRequired) {
        _pgsproc.Maximum = total;
        _pgsproc.Value = current;
      } else {
        this.Invoke(new UpdateEnginePartDownloadFileHandler(this.UI_PartDownloadFile), new object[] { filename, current, total});
      }
    }
    #endregion

    #region private void UI_StoppingDownloadFile(string filename, bool isok)
    private void UI_StoppingDownloadFile(string filename, bool isok) {
      if(!this.InvokeRequired) {
        _pgsproc.Maximum = 100;
        _pgsproc.Value = 0;

        this._txtlogs.Text += " - " + (isok ? "ok" : "error") + "\n";
      } else {
        this.Invoke(new UpdateEngineStoppingDownloadFileHandler(this.UI_StoppingDownloadFile), new object[] { filename , isok});
      }
    }
    #endregion

    #region private class PUEventArgs: EventArgs
    private class PUEventArgs:EventArgs {
      public string Text;
      public PUEventArgs(string text)
        : base() {
        Text = text;
      }
    }
    #endregion

    #region private void _processUpdate_LblSetting(object sender, EventArgs e)
    private void _processUpdate_LblSetting(object sender, EventArgs e) {
      PUEventArgs puea = e as PUEventArgs;
      this.TextAction = puea.Text;
      this._btnUpdate.Text = LngBtnClose;
      this.Refresh();
      if (_updateOK) {
        MessageBox.Show(puea.Text, GordagoMain.MessageCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }
    #endregion

    #region private void _chkCommerc_CheckedChanged(object sender, System.EventArgs e)
    private void _chkCommerc_CheckedChanged(object sender, System.EventArgs e) {
			this.Commercy = _chkCommerc.Checked;
		}
		#endregion

    #region private void _lblLogin_Click(object sender, EventArgs e)
    private void _lblLogin_Click(object sender, EventArgs e) {

    }
    #endregion

    #region private void _lblCommercy_Click(object sender, EventArgs e)
    private void _lblCommercy_Click(object sender, EventArgs e) {
      this._chkCommerc.Checked = !this._chkCommerc.Checked;
    }
    #endregion

    #region private void _btnGetUP_Click(object sender, EventArgs e)
    private void _btnGetUP_Click(object sender, EventArgs e) {
      GordagoMain.MainForm.ShowRegPage();
    }
    #endregion
  }
}
