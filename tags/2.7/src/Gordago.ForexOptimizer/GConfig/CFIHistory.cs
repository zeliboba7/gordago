/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Cursit.Applic.AConfig;
using Language;
#endregion
using Cursit.Table;

namespace Gordago.GConfig {
	public class CFIHistory : Gordago.GConfig.ConfigFormItem {
		private System.Windows.Forms.Label _lblloadHistoryPeriod;
		private System.Windows.Forms.ComboBox _cmbHstPeriod;
		private System.ComponentModel.Container components = null;

		private ConfigValue _cfg;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label _lblTimeFrame;
		private System.Windows.Forms.Button _btnAddTF;
		private System.Windows.Forms.Button _btnDelTF;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label _lblTFSecond;
		private System.Windows.Forms.TextBox _txtTFSecond;
		private System.Windows.Forms.Button _btnSetAsDefault;
		private LoadHstPeriod[] _lhp;
		private Cursit.Table.TableControl _tblTimeFrames;
		private System.Windows.Forms.Label _lblWarning;
		private string _timeframesdata = "";

		public CFIHistory() {
			InitializeComponent();
			this.Text = Dictionary.GetString(29,1,"История");
			_cfg = Config.Users["Symbol"];

			_lhp = new LoadHstPeriod[]{
																	new LoadHstPeriod(0, Dictionary.GetString(29,2,"Вся история")),
																	new LoadHstPeriod(31, Dictionary.GetString(29,3,"За последний месяц")),
																	new LoadHstPeriod(31*3, Dictionary.GetString(29,4,"За последнии 3 месяца")),
																	new LoadHstPeriod(31*6, Dictionary.GetString(29,5,"За последнии 6 месяцев")),
																	new LoadHstPeriod(31*9, Dictionary.GetString(29,6,"За последнии 9 месяцев")),
																	new LoadHstPeriod(365, Dictionary.GetString(29,7,"За последний год"))
																};
			this._cmbHstPeriod.Items.AddRange(_lhp);
			this._cmbHstPeriod.SelectedItem = GetLoadHstPeriod(_cfg["LoadDayPeriod", 0]);
			_lblloadHistoryPeriod.Text = Dictionary.GetString(29,8,"Загружать историю за период");

			_timeframesdata = Config.Users["Symbol"]["TimeFrames", GordagoMain.TIMEFRAMES_INIT_DATA];
			_tblTimeFrames.Columns.Add(Dictionary.GetString(29,26,"Имя"), TableColumnType.Label, 60);
			_tblTimeFrames.Columns.Add(Dictionary.GetString(29,27,"Секунды"), TableColumnType.Label, 60);
			this.UpdateTimeFramesList(_timeframesdata);

			this._lblTimeFrame.Text = Dictionary.GetString(29,11,"Имеющиеся таймфреймы");
			this._lblTFSecond.Text = Dictionary.GetString(29,12,"Введите секунды");
			this._btnAddTF.Text = Dictionary.GetString(29,13,"Add");
			this._btnDelTF.Text = Dictionary.GetString(29,14,"Remove");
			this._btnSetAsDefault.Text = Dictionary.GetString(29,15,"Use Default");
			this._lblWarning.Text = Dictionary.GetString(29,16,"Изменения периодов вступят в силу после перезапуска программы");
		}

		public override void SaveConfig() {
			base.SaveConfig ();
			LoadHstPeriod lhp = this._cmbHstPeriod.SelectedItem as LoadHstPeriod;
			_cfg["LoadDayPeriod"].SetValue(lhp == null ? _lhp[0].Id : lhp.Id);
			Config.Users["Symbol"]["TimeFrames"].SetValue(_timeframesdata);
		}


		#region private LoadHstPeriod GetLoadHstPeriod(int id)
		private LoadHstPeriod GetLoadHstPeriod(int id){
			foreach (LoadHstPeriod lhp in this._lhp){
				if (lhp.Id == id)
					return lhp;
			}
			return _lhp[0];
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._lblloadHistoryPeriod = new System.Windows.Forms.Label();
			this._cmbHstPeriod = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._tblTimeFrames = new Cursit.Table.TableControl();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this._txtTFSecond = new System.Windows.Forms.TextBox();
			this._lblTFSecond = new System.Windows.Forms.Label();
			this._btnAddTF = new System.Windows.Forms.Button();
			this._btnDelTF = new System.Windows.Forms.Button();
			this._lblTimeFrame = new System.Windows.Forms.Label();
			this._btnSetAsDefault = new System.Windows.Forms.Button();
			this._lblWarning = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lblloadHistoryPeriod
			// 
			this._lblloadHistoryPeriod.AutoSize = true;
			this._lblloadHistoryPeriod.Location = new System.Drawing.Point(8, 16);
			this._lblloadHistoryPeriod.Name = "_lblloadHistoryPeriod";
			this._lblloadHistoryPeriod.Size = new System.Drawing.Size(163, 16);
			this._lblloadHistoryPeriod.TabIndex = 0;
			this._lblloadHistoryPeriod.Text = "Загружать историю за период";
			// 
			// _cmbHstPeriod
			// 
			this._cmbHstPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._cmbHstPeriod.Location = new System.Drawing.Point(8, 40);
			this._cmbHstPeriod.Name = "_cmbHstPeriod";
			this._cmbHstPeriod.Size = new System.Drawing.Size(392, 21);
			this._cmbHstPeriod.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._cmbHstPeriod);
			this.groupBox1.Controls.Add(this._lblloadHistoryPeriod);
			this.groupBox1.Location = new System.Drawing.Point(8, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(408, 72);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._tblTimeFrames);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Controls.Add(this._btnDelTF);
			this.groupBox2.Controls.Add(this._lblTimeFrame);
			this.groupBox2.Controls.Add(this._btnSetAsDefault);
			this.groupBox2.Location = new System.Drawing.Point(8, 80);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(408, 200);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			// 
			// _tblTimeFrames
			// 
			this._tblTimeFrames.CaptionVisible = true;
			this._tblTimeFrames.Location = new System.Drawing.Point(8, 32);
			this._tblTimeFrames.Name = "_tblTimeFrames";
			this._tblTimeFrames.SelectedIndex = -1;
			this._tblTimeFrames.SelectedRow = null;
			this._tblTimeFrames.Size = new System.Drawing.Size(184, 160);
			this._tblTimeFrames.TabIndex = 5;
			this._tblTimeFrames.Text = "tableControl1";
			this._tblTimeFrames.ViewHGdirLines = false;
			this._tblTimeFrames.SelectedIndexChanged += new EventHandler(this._tblTimeFrames_SelectedIndexChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this._txtTFSecond);
			this.groupBox3.Controls.Add(this._lblTFSecond);
			this.groupBox3.Controls.Add(this._btnAddTF);
			this.groupBox3.Location = new System.Drawing.Point(200, 24);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(200, 88);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			// 
			// _txtTFSecond
			// 
			this._txtTFSecond.Location = new System.Drawing.Point(8, 56);
			this._txtTFSecond.Name = "_txtTFSecond";
			this._txtTFSecond.Size = new System.Drawing.Size(96, 20);
			this._txtTFSecond.TabIndex = 1;
			this._txtTFSecond.Text = "";
			// 
			// _lblTFSecond
			// 
			this._lblTFSecond.Location = new System.Drawing.Point(8, 16);
			this._lblTFSecond.Name = "_lblTFSecond";
			this._lblTFSecond.Size = new System.Drawing.Size(184, 32);
			this._lblTFSecond.TabIndex = 0;
			this._lblTFSecond.Text = "Second";
			// 
			// _btnAddTF
			// 
			this._btnAddTF.Location = new System.Drawing.Point(112, 56);
			this._btnAddTF.Name = "_btnAddTF";
			this._btnAddTF.Size = new System.Drawing.Size(75, 24);
			this._btnAddTF.TabIndex = 2;
			this._btnAddTF.Text = "Add";
			this._btnAddTF.Click += new System.EventHandler(this._btnAddTF_Click);
			// 
			// _btnDelTF
			// 
			this._btnDelTF.Location = new System.Drawing.Point(208, 128);
			this._btnDelTF.Name = "_btnDelTF";
			this._btnDelTF.Size = new System.Drawing.Size(176, 23);
			this._btnDelTF.TabIndex = 3;
			this._btnDelTF.Text = "Remove";
			this._btnDelTF.Click += new System.EventHandler(this._btnDelTF_Click);
			// 
			// _lblTimeFrame
			// 
			this._lblTimeFrame.AutoSize = true;
			this._lblTimeFrame.Location = new System.Drawing.Point(8, 16);
			this._lblTimeFrame.Name = "_lblTimeFrame";
			this._lblTimeFrame.Size = new System.Drawing.Size(52, 16);
			this._lblTimeFrame.TabIndex = 0;
			this._lblTimeFrame.Text = "Периоды";
			// 
			// _btnSetAsDefault
			// 
			this._btnSetAsDefault.Location = new System.Drawing.Point(208, 160);
			this._btnSetAsDefault.Name = "_btnSetAsDefault";
			this._btnSetAsDefault.Size = new System.Drawing.Size(176, 23);
			this._btnSetAsDefault.TabIndex = 3;
			this._btnSetAsDefault.Text = "Use Defaults";
			this._btnSetAsDefault.Click += new System.EventHandler(this._btnSetAsDefault_Click);
			// 
			// _lblWarning
			// 
			this._lblWarning.Location = new System.Drawing.Point(16, 288);
			this._lblWarning.Name = "_lblWarning";
			this._lblWarning.Size = new System.Drawing.Size(400, 112);
			this._lblWarning.TabIndex = 0;
			this._lblWarning.Text = "Изменения периодов вступит в силу после перезапуска программы";
			// 
			// CFIHistory
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this._lblWarning);
			this.Name = "CFIHistory";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region private class LoadHstPeriod
		private class LoadHstPeriod{
			public int Id;
			public string Name;

			public LoadHstPeriod(int id, string name){
				Id = id;
				Name = name;
			}

			public override string ToString() {
				return Name;
			}
		}
		#endregion

		#region private void UpdateTimeFramesList(string tfdata)
		private void UpdateTimeFramesList(string tfdata){
			string[] sa = tfdata.Split('|');

			TimeFrameManager tfm = new TimeFrameManager();
			
			for (int i=0;i<sa.Length;i++){
				int second = Convert.ToInt32(sa[i]);
        tfm.AddTimeFrame(TimeFrameManager.TimeFrames.CreateNew(second));
			}
			this.UpdateTimeFramesList(tfm);
		}
		#endregion

    #region private void UpdateTimeFramesList(TimeFrameManager tfm)
    private void UpdateTimeFramesList(TimeFrameManager tfm){
			_tblTimeFrames.Rows.Clear();
			foreach (TimeFrame tf in tfm){
				TableRow row = _tblTimeFrames.NewRow();
				row.AdditionObject = tf;
				row[0].Text = tf.Name;
				row[1].Text = tf.Second.ToString();
				_tblTimeFrames.Rows.AddRow(row);
			}
			RefreshTFStatus();
			_tblTimeFrames.Invalidate();
		}
		#endregion

		#region private void RefreshTFStatus()
		private void RefreshTFStatus(){
			bool enabled = false;
			if (this._tblTimeFrames.SelectedIndex >= 0)
				enabled = true;
			this._btnDelTF.Enabled = enabled;
		}
		#endregion

		#region private void _tblTimeFrames_SelectedIndexChanged() 
		private void _tblTimeFrames_SelectedIndexChanged(object sender, EventArgs e) {
			this.RefreshTFStatus();
		}
		#endregion

		#region private void _btnDelTF_Click(object sender, System.EventArgs e) 
		private void _btnDelTF_Click(object sender, System.EventArgs e) {
			if (this._tblTimeFrames.SelectedIndex < 0)
				return;
			this._tblTimeFrames.Rows.RemoveAt(_tblTimeFrames.SelectedIndex);
			this._tblTimeFrames.Invalidate();
			this.CreateTimeFrameData();
			this.RefreshTFStatus();
		}
		#endregion

		#region private void CreateTimeFrameData()
		private void CreateTimeFrameData(){
			string[] sa = new string[_tblTimeFrames.Rows.Count];
			for (int i=0;i<sa.Length;i++){
				TimeFrame tf = _tblTimeFrames.Rows[i].AdditionObject as TimeFrame;
				sa[i] = Convert.ToString(tf.Second);
			}
			_timeframesdata = string.Join("|", sa);
		}
		#endregion

		#region private void _btnSetAsDefault_Click(object sender, System.EventArgs e) 
		private void _btnSetAsDefault_Click(object sender, System.EventArgs e) {
			this._timeframesdata = GordagoMain.TIMEFRAMES_INIT_DATA;
			this.UpdateTimeFramesList(_timeframesdata);
		}
		#endregion

		#region private void _btnAddTF_Click(object sender, System.EventArgs e) 
		private void _btnAddTF_Click(object sender, System.EventArgs e) {
			int second = 0;
			try{
				second = Convert.ToInt32(this._txtTFSecond.Text);
			}catch{}

			this._txtTFSecond.Text = "";
			if (second <= 0)
				return;

			TimeFrameManager tfm = new TimeFrameManager();
			for (int i=0;i<_tblTimeFrames.Rows.Count;i++){
				TimeFrame tf = _tblTimeFrames.Rows[i].AdditionObject as TimeFrame;
				tfm.AddTimeFrame(tf);
			}
			TimeFrame tfnew = tfm.CreateNew(second);
			tfm.AddTimeFrame(tfnew);
			this.UpdateTimeFramesList(tfm);
			this.CreateTimeFrameData();
		}
		#endregion
	}
}
