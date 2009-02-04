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
using Cursit.Applic.AConfig;
#endregion
using Language;
using Cursit.Utils;

namespace Gordago.Strategy.IO {
	public class StrategyExportMQL4OptionsForm : System.Windows.Forms.Form {
		#region private property
		private System.Windows.Forms.CheckBox _chkApplyAccount;
		private System.Windows.Forms.TextBox _txtNumAccount;
		private System.Windows.Forms.CheckBox _chkUseSound;
		private System.Windows.Forms.TextBox _txtSound;
		private System.Windows.Forms.Button _btnGenerate;
		private System.Windows.Forms.Button _btnCancel;
		private System.ComponentModel.Container components = null;
		
		private ConfigValue _cfg;
		private System.Windows.Forms.Label _lblFromHout;
		private System.Windows.Forms.NumericUpDown _nudFromHour;
		private System.Windows.Forms.Label _lblToHour;
		private System.Windows.Forms.NumericUpDown _nudToHour;
		private System.Windows.Forms.CheckBox _chkUseHourTrade;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.NumericUpDown _nudSlipPage;
		private System.Windows.Forms.Label _lblSlipPage;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label _lblLots;
		private System.Windows.Forms.NumericUpDown _numLots;
		private System.Windows.Forms.Label _lblMagic;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button _btnNewMNumber;
		private System.Windows.Forms.TextBox _txtMagic;
		private System.Windows.Forms.CheckBox _chkUseMagic;
		private System.Windows.Forms.Label _lbltemplatefile;
		private System.Windows.Forms.ComboBox _cmbTemplate;
		private MQL4Options _mtoptions;
		#endregion

		#region public StrategyExportMQL4OptionsForm()
		public StrategyExportMQL4OptionsForm() {
			InitializeComponent();
			_cfg = Config.Users["Export"]["MQL4"];
			this._chkApplyAccount.Checked = _cfg["ApplyAccount", false];
			this._txtNumAccount.Text = _cfg["Account", ""];
			this._chkUseSound.Checked = _cfg["UseSound", true];
			this._txtSound.Text = _cfg["SoundFile", "alert.wav"];

			this._chkUseHourTrade.Checked = _cfg["UseHourTrade", false];
			this._nudFromHour.Value = _cfg["FromHourTrade", 0];
			this._nudToHour.Value = _cfg["ToHourTrade", 23];

			this._nudSlipPage.Value = _cfg["SlipPage", 5];

			this._chkUseMagic.Checked = _cfg["UseMagic", true];
			this._txtMagic.Text = _cfg["Magic", this.MagicRnd()];

			int reslots = _cfg["Lots", 10];
			decimal lots = Convert.ToDecimal(reslots)/100;
			this._numLots.Value = lots;

			_mtoptions = new MQL4Options();
			this._chkApplyAccount.Text = Dictionary.GetString(25,1);
			this._chkUseSound.Text     = Dictionary.GetString(25,2);
			this._btnGenerate.Text     = Dictionary.GetString(25,3);
			this._btnCancel.Text       = Dictionary.GetString(25,4);
			this._chkUseHourTrade.Text = Dictionary.GetString(25,5,"Использовать время торговли");
			this._lblFromHout.Text = Dictionary.GetString(25,6,"Время начала торговли");
			this._lblToHour.Text = Dictionary.GetString(25,7,"Время конца торговли");
			this._lblLots.Text = Dictionary.GetString(25,8,"Количество лотов");
			this._lblSlipPage.Text = Dictionary.GetString(25,9,"Проскальзывание цены");
			this._lbltemplatefile.Text = Dictionary.GetString(25,14,"Шаблон");

			this._chkUseMagic.Text = Dictionary.GetString(25,10,"Привязать советника к сделкам");
			this._lblMagic.Text = Dictionary.GetString(25,11,"MAGIC number");
			this._btnNewMNumber.Text = Dictionary.GetString(25,12,"New MAGIC");

			this.CheckApplyAccount();
			this.CheckUseSound();
			this.CheckUseHourTrade();
			this.CheckUseMagic();

			string[] files = System.IO.Directory.GetFiles(Application.StartupPath + "\\mqltemplate");
			Cursit.Utils.FileEngine.DisplayFile[] dfs = Cursit.Utils.FileEngine.GetDisplayFiles(files);
			this._cmbTemplate.Items.AddRange(dfs);
			string deffile = _cfg["Pattern", "gordago.mq4"];
			foreach (Cursit.Utils.FileEngine.DisplayFile df in dfs){
				if (df.DisplayName == deffile){
					this._cmbTemplate.SelectedItem = df;
					break;
				}
			}
		}
		#endregion

		#region public MQL4Options MQL4Options
		public MQL4Options MQL4Options{
			get{return this._mtoptions;}
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._chkApplyAccount = new System.Windows.Forms.CheckBox();
			this._txtNumAccount = new System.Windows.Forms.TextBox();
			this._chkUseSound = new System.Windows.Forms.CheckBox();
			this._txtSound = new System.Windows.Forms.TextBox();
			this._btnGenerate = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			this._chkUseHourTrade = new System.Windows.Forms.CheckBox();
			this._lblFromHout = new System.Windows.Forms.Label();
			this._nudFromHour = new System.Windows.Forms.NumericUpDown();
			this._lblToHour = new System.Windows.Forms.Label();
			this._nudToHour = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this._nudSlipPage = new System.Windows.Forms.NumericUpDown();
			this._lblSlipPage = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this._lblLots = new System.Windows.Forms.Label();
			this._numLots = new System.Windows.Forms.NumericUpDown();
			this._txtMagic = new System.Windows.Forms.TextBox();
			this._chkUseMagic = new System.Windows.Forms.CheckBox();
			this._lblMagic = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this._btnNewMNumber = new System.Windows.Forms.Button();
			this._lbltemplatefile = new System.Windows.Forms.Label();
			this._cmbTemplate = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this._nudFromHour)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._nudToHour)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._nudSlipPage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._numLots)).BeginInit();
			this.SuspendLayout();
			// 
			// _chkApplyAccount
			// 
			this._chkApplyAccount.Location = new System.Drawing.Point(8, 5);
			this._chkApplyAccount.Name = "_chkApplyAccount";
			this._chkApplyAccount.Size = new System.Drawing.Size(232, 16);
			this._chkApplyAccount.TabIndex = 0;
			this._chkApplyAccount.Text = "Привязка к счету №";
			this._chkApplyAccount.CheckedChanged += new System.EventHandler(this._chkApplyAccount_CheckedChanged);
			// 
			// _txtNumAccount
			// 
			this._txtNumAccount.Location = new System.Drawing.Point(248, 4);
			this._txtNumAccount.Name = "_txtNumAccount";
			this._txtNumAccount.Size = new System.Drawing.Size(96, 20);
			this._txtNumAccount.TabIndex = 1;
			this._txtNumAccount.Text = "";
			// 
			// _chkUseSound
			// 
			this._chkUseSound.Location = new System.Drawing.Point(8, 112);
			this._chkUseSound.Name = "_chkUseSound";
			this._chkUseSound.Size = new System.Drawing.Size(232, 24);
			this._chkUseSound.TabIndex = 0;
			this._chkUseSound.Text = "Использовать звуковой сигнал";
			this._chkUseSound.CheckedChanged += new System.EventHandler(this._chkUseSound_CheckedChanged);
			// 
			// _txtSound
			// 
			this._txtSound.Location = new System.Drawing.Point(248, 112);
			this._txtSound.Name = "_txtSound";
			this._txtSound.Size = new System.Drawing.Size(96, 20);
			this._txtSound.TabIndex = 1;
			this._txtSound.Text = "alert.wav";
			// 
			// _btnGenerate
			// 
			this._btnGenerate.Location = new System.Drawing.Point(120, 352);
			this._btnGenerate.Name = "_btnGenerate";
			this._btnGenerate.Size = new System.Drawing.Size(104, 23);
			this._btnGenerate.TabIndex = 2;
			this._btnGenerate.Text = "Generate";
			this._btnGenerate.Click += new System.EventHandler(this._btnGenerate_Click);
			// 
			// _btnCancel
			// 
			this._btnCancel.Location = new System.Drawing.Point(240, 352);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(104, 23);
			this._btnCancel.TabIndex = 2;
			this._btnCancel.Text = "Cancel";
			this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _chkUseHourTrade
			// 
			this._chkUseHourTrade.Location = new System.Drawing.Point(8, 152);
			this._chkUseHourTrade.Name = "_chkUseHourTrade";
			this._chkUseHourTrade.Size = new System.Drawing.Size(232, 24);
			this._chkUseHourTrade.TabIndex = 0;
			this._chkUseHourTrade.Text = "Использовать время торговли";
			this._chkUseHourTrade.CheckedChanged += new System.EventHandler(this._chkUseHourTrade_CheckedChanged);
			// 
			// _lblFromHout
			// 
			this._lblFromHout.Location = new System.Drawing.Point(16, 184);
			this._lblFromHout.Name = "_lblFromHout";
			this._lblFromHout.Size = new System.Drawing.Size(216, 16);
			this._lblFromHout.TabIndex = 3;
			this._lblFromHout.Text = "Время начала торговли";
			this._lblFromHout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _nudFromHour
			// 
			this._nudFromHour.Location = new System.Drawing.Point(248, 184);
			this._nudFromHour.Maximum = new System.Decimal(new int[] {
																																 23,
																																 0,
																																 0,
																																 0});
			this._nudFromHour.Name = "_nudFromHour";
			this._nudFromHour.Size = new System.Drawing.Size(48, 20);
			this._nudFromHour.TabIndex = 4;
			// 
			// _lblToHour
			// 
			this._lblToHour.Location = new System.Drawing.Point(16, 208);
			this._lblToHour.Name = "_lblToHour";
			this._lblToHour.Size = new System.Drawing.Size(216, 16);
			this._lblToHour.TabIndex = 3;
			this._lblToHour.Text = "Время конца торговли";
			this._lblToHour.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _nudToHour
			// 
			this._nudToHour.Location = new System.Drawing.Point(248, 208);
			this._nudToHour.Maximum = new System.Decimal(new int[] {
																															 23,
																															 0,
																															 0,
																															 0});
			this._nudToHour.Name = "_nudToHour";
			this._nudToHour.Size = new System.Drawing.Size(48, 20);
			this._nudToHour.TabIndex = 4;
			this._nudToHour.Value = new System.Decimal(new int[] {
																														 23,
																														 0,
																														 0,
																														 0});
			// 
			// groupBox1
			// 
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(344, 8);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(8, 136);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(344, 8);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// groupBox3
			// 
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(8, 232);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(344, 8);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			// 
			// _nudSlipPage
			// 
			this._nudSlipPage.Location = new System.Drawing.Point(248, 272);
			this._nudSlipPage.Name = "_nudSlipPage";
			this._nudSlipPage.Size = new System.Drawing.Size(64, 20);
			this._nudSlipPage.TabIndex = 4;
			this._nudSlipPage.Value = new System.Decimal(new int[] {
																															 5,
																															 0,
																															 0,
																															 0});
			// 
			// _lblSlipPage
			// 
			this._lblSlipPage.Location = new System.Drawing.Point(16, 272);
			this._lblSlipPage.Name = "_lblSlipPage";
			this._lblSlipPage.Size = new System.Drawing.Size(224, 16);
			this._lblSlipPage.TabIndex = 6;
			this._lblSlipPage.Text = "Проскальзывание цены";
			// 
			// groupBox4
			// 
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(8, 296);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(344, 8);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			// 
			// _lblLots
			// 
			this._lblLots.Location = new System.Drawing.Point(16, 248);
			this._lblLots.Name = "_lblLots";
			this._lblLots.Size = new System.Drawing.Size(224, 16);
			this._lblLots.TabIndex = 6;
			this._lblLots.Text = "Количество лотов";
			// 
			// _numLots
			// 
			this._numLots.DecimalPlaces = 2;
			this._numLots.Increment = new System.Decimal(new int[] {
																															 1,
																															 0,
																															 0,
																															 65536});
			this._numLots.Location = new System.Drawing.Point(248, 248);
			this._numLots.Maximum = new System.Decimal(new int[] {
																														 1000,
																														 0,
																														 0,
																														 0});
			this._numLots.Name = "_numLots";
			this._numLots.Size = new System.Drawing.Size(64, 20);
			this._numLots.TabIndex = 4;
			this._numLots.Value = new System.Decimal(new int[] {
																													 10,
																													 0,
																													 0,
																													 131072});
			// 
			// _txtMagic
			// 
			this._txtMagic.Location = new System.Drawing.Point(136, 69);
			this._txtMagic.Name = "_txtMagic";
			this._txtMagic.Size = new System.Drawing.Size(96, 20);
			this._txtMagic.TabIndex = 1;
			this._txtMagic.Text = "";
			// 
			// _chkUseMagic
			// 
			this._chkUseMagic.Location = new System.Drawing.Point(8, 40);
			this._chkUseMagic.Name = "_chkUseMagic";
			this._chkUseMagic.Size = new System.Drawing.Size(344, 16);
			this._chkUseMagic.TabIndex = 0;
			this._chkUseMagic.Text = "Привязать советника к сделкам";
			this._chkUseMagic.CheckedChanged += new System.EventHandler(this._chkUseMagic_CheckedChanged);
			// 
			// _lblMagic
			// 
			this._lblMagic.Location = new System.Drawing.Point(16, 70);
			this._lblMagic.Name = "_lblMagic";
			this._lblMagic.Size = new System.Drawing.Size(112, 16);
			this._lblMagic.TabIndex = 3;
			this._lblMagic.Text = "MAGIC number";
			this._lblMagic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox5
			// 
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(8, 27);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(344, 8);
			this.groupBox5.TabIndex = 5;
			this.groupBox5.TabStop = false;
			// 
			// _btnNewMNumber
			// 
			this._btnNewMNumber.Location = new System.Drawing.Point(240, 67);
			this._btnNewMNumber.Name = "_btnNewMNumber";
			this._btnNewMNumber.Size = new System.Drawing.Size(104, 23);
			this._btnNewMNumber.TabIndex = 7;
			this._btnNewMNumber.Text = "New MAGIC";
			this._btnNewMNumber.Click += new System.EventHandler(this._btnNewMNumber_Click);
			// 
			// _lbltemplatefile
			// 
			this._lbltemplatefile.Location = new System.Drawing.Point(16, 315);
			this._lbltemplatefile.Name = "_lbltemplatefile";
			this._lbltemplatefile.Size = new System.Drawing.Size(128, 16);
			this._lbltemplatefile.TabIndex = 6;
			this._lbltemplatefile.Text = "Шаблон";
			// 
			// _cmbTemplate
			// 
			this._cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbTemplate.Location = new System.Drawing.Point(152, 312);
			this._cmbTemplate.Name = "_cmbTemplate";
			this._cmbTemplate.Size = new System.Drawing.Size(200, 21);
			this._cmbTemplate.TabIndex = 8;
			// 
			// StrategyExportMQL4OptionsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(362, 386);
			this.Controls.Add(this._cmbTemplate);
			this.Controls.Add(this._btnNewMNumber);
			this.Controls.Add(this._lblSlipPage);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this._nudFromHour);
			this.Controls.Add(this._lblFromHout);
			this.Controls.Add(this._btnGenerate);
			this.Controls.Add(this._txtNumAccount);
			this.Controls.Add(this._txtSound);
			this.Controls.Add(this._chkApplyAccount);
			this.Controls.Add(this._chkUseSound);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._chkUseHourTrade);
			this.Controls.Add(this._lblToHour);
			this.Controls.Add(this._nudToHour);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this._nudSlipPage);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this._lblLots);
			this.Controls.Add(this._numLots);
			this.Controls.Add(this._txtMagic);
			this.Controls.Add(this._chkUseMagic);
			this.Controls.Add(this._lblMagic);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this._lbltemplatefile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "StrategyExportMQL4OptionsForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Generate MQL4 code";
			((System.ComponentModel.ISupportInitialize)(this._nudFromHour)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._nudToHour)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._nudSlipPage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._numLots)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region private void _btnGenerate_Click(object sender, System.EventArgs e)
		private void _btnGenerate_Click(object sender, System.EventArgs e) {
			_cfg["ApplyAccount"].SetValue(this._chkApplyAccount.Checked);
			this._mtoptions.UseApplyAccount = this._chkApplyAccount.Checked;
			_cfg["Account"].SetValue(this._txtNumAccount.Text);
			this._mtoptions.Account = this._txtNumAccount.Text;
			_cfg["UseSound"].SetValue(this._chkUseSound.Checked);
			this._mtoptions.UseSound = this._chkUseSound.Checked;
			_cfg["SoundFile"].SetValue(this._txtSound.Text);
			this._mtoptions.SoundFile = this._txtSound.Text;


			_cfg["UseHourTrade"].SetValue(this._chkUseHourTrade.Checked);
			this._mtoptions.UseHourTrade = this._chkUseHourTrade.Checked;
			_cfg["FromHourTrade"].SetValue((int)this._nudFromHour.Value);
			this._mtoptions.FromHourTrade = (int)this._nudFromHour.Value;
			_cfg["ToHourTrade"].SetValue((int)this._nudToHour.Value);
			this._mtoptions.ToHourTrade = (int)this._nudToHour.Value;

			_cfg["SlipPage"].SetValue((int)this._nudSlipPage.Value);
			this._mtoptions.SlipPage = (int)this._nudSlipPage.Value;

			int lots = (int)(this._numLots.Value * 100);
			_cfg["Lots"].SetValue(lots);
			this._mtoptions.Lots = this._numLots.Value;

			this._mtoptions.UseMagic = this._chkUseMagic.Checked;
			_cfg["UseMagic"].SetValue(this._chkUseMagic.Checked);

			this._mtoptions.Magic = this._txtMagic.Text;
			_cfg["Magic"].SetValue(this._txtMagic.Text);

			if (this._cmbTemplate.SelectedItem == null){
				this._mtoptions.PatternFile = "";
			}else{
				FileEngine.DisplayFile df = (FileEngine.DisplayFile)this._cmbTemplate.SelectedItem;
				_cfg["Pattern"].SetValue(df.DisplayName);
				this._mtoptions.PatternFile = df.DisplayName;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		#endregion

		#region private void _btnCancel_Click(object sender, System.EventArgs e) 
		private void _btnCancel_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

		#region private void _chkApplyAccount_CheckedChanged(object sender, System.EventArgs e)
		private void _chkApplyAccount_CheckedChanged(object sender, System.EventArgs e) {
			this.CheckApplyAccount();
		}
		#endregion

		#region private void CheckApplyAccount()
		private void CheckApplyAccount(){
			this._txtNumAccount.Enabled = this._chkApplyAccount.Checked;
		}
		#endregion

		#region private void _chkUseSound_CheckedChanged(object sender, System.EventArgs e)
		private void _chkUseSound_CheckedChanged(object sender, System.EventArgs e) {
			this.CheckUseSound();
		}
		#endregion

		#region private void CheckUseSound()
		private void CheckUseSound(){
			this._txtSound.Enabled = this._chkUseSound.Checked;
		}
		#endregion

		#region private void _chkUseHourTrade_CheckedChanged(object sender, System.EventArgs e) 
		private void _chkUseHourTrade_CheckedChanged(object sender, System.EventArgs e) {
			this.CheckUseHourTrade();
		}
		#endregion

		#region private void CheckUseHourTrade()
		private void CheckUseHourTrade(){
			this._lblFromHout.Enabled = 
				this._lblToHour.Enabled = 
				this._nudFromHour.Enabled = 
				this._nudToHour.Enabled = 
				this._chkUseHourTrade.Checked;
		}
		#endregion

		#region private void CheckUseMagic()
		private void CheckUseMagic(){
			this._lblMagic.Enabled = 
				this._btnNewMNumber.Enabled = 
				this._txtMagic.Enabled = 
				this._chkUseMagic.Checked;
		}
		#endregion

		#region private void _btnNewMNumber_Click(object sender, System.EventArgs e) 
		private void _btnNewMNumber_Click(object sender, System.EventArgs e) {
			this._txtMagic.Text = this.MagicRnd();
		}
		#endregion

		#region private string MagicRnd()
		private string MagicRnd(){
			Random rnd = new Random();
			return rnd.Next(1000, 1000000).ToString();
		}
		#endregion

		#region private void _chkUseMagic_CheckedChanged(object sender, System.EventArgs e) 
		private void _chkUseMagic_CheckedChanged(object sender, System.EventArgs e) {
			this.CheckUseMagic();
		}
		#endregion
	}
}
