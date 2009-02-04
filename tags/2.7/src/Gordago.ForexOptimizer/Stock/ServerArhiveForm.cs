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
using Language;

using Gordago.LstObject;
using Gordago.Docs;
using Gordago.WebUpdate;

using Cursit.Applic.AConfig;
#endregion

using System.Collections.Generic;

namespace Gordago.Stock.Loader {
	public class ServerArhiveForm : System.Windows.Forms.Form {

		#region private property
		private System.Windows.Forms.Button _btnDownload;
		private System.Windows.Forms.Button _btnClose;

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListView _lstInfo;
		private System.Windows.Forms.Label _lblWarning;
		private System.Windows.Forms.Button _btnSAll;
		private System.Windows.Forms.Button _btnUSAll;

		private bool _firstactivate;
		private System.Windows.Forms.Label _lblPlaseWait;
		private System.Windows.Forms.Label _lblMessage;
		private System.Windows.Forms.ProgressBar _pgs;
		private DownloaderArhiveHistory _dah;
    #endregion

    private Label _lblSymbol;
    private ComboBox _cmbSymbol;

    private string _symbolName;

    public ServerArhiveForm(string symbolName) {
      _symbolName = symbolName;
			InitializeComponent();
			_firstactivate = true;
			this._pgs.Visible = false;
			this._lblMessage.Visible = false;
			this.Text = Dictionary.GetString(23,1,"Закачка архива котировок с сервера Gordago");
			this._lblWarning.Text = Language.Dictionary.GetString(23,11);
			this._btnSAll.Text = Dictionary.GetString(23,12,"Выбрать все");
			this._btnUSAll.Text = Dictionary.GetString(23,18,"Отменить все");
			this._btnDownload.Text = Dictionary.GetString(23,14,"Скачать");
			this._btnClose.Text = Dictionary.GetString(23,15,"Закрыть");
			this.AddColumn(this._lstInfo);
      this._lblSymbol.Visible =
        this._cmbSymbol.Visible = false;

			_dah = new DownloaderArhiveHistory(symbolName);
			_dah.StartingCommnadEvent += new DAHHandler(_dah_StartingCommand);
			_dah.StoppingCommnadEvent += new DAHHandler(_dah_StoppingCommand);
      _dah.DownloadedPartEvent += new DAHDownPartHandler(this._dah_DownloadedPart);
    }

    #region private void _dah_DownloadedPart(string message, int total, int current)
    private void _dah_DownloadedPart(string message, int total, int current){
      if(this.InvokeRequired == false) {
        this._lblMessage.Text = message;
        this._pgs.Maximum = total;
        this._pgs.Value = current;
      } else {
        this.Invoke(new DAHDownPartHandler(this._dah_DownloadedPart), 
          new object[] {message, total, current });
      }
    }
    #endregion

    #region private void _dah_StartingCommand(DAHCommnad cmd)
    private void _dah_StartingCommand(DAHCommnad cmd){
      if(this.InvokeRequired == false) {
        switch(cmd) {
          case DAHCommnad.DownloadHistoryInfo:
            this.Invalidate();
            break;
          case DAHCommnad.DownloadFiles:
          case DAHCommnad.UnPack:
          case DAHCommnad.Update:
            this._pgs.Visible = true;
            this._lblMessage.Visible = true;
            this._btnUSAll.Enabled =
              this._btnDownload.Enabled =
              this._btnSAll.Enabled = 
              this._btnClose.Enabled =  false;
            break;
        }
      } else {
        this.Invoke(new DAHHandler(this._dah_StartingCommand), new object[] {cmd });
      }
    }
    #endregion

    #region private void _dah_StoppingCommand(DAHCommnad cmd)
    private void _dah_StoppingCommand(DAHCommnad cmd){
      if(!this.InvokeRequired) {
        switch(cmd) {
          case DAHCommnad.DownloadHistoryInfo:
            if(_dah.LastError == DAHError.OK) {
              this.UpdateDAHRows();
              this._lblPlaseWait.Visible = false;
            } else {
              this._lblPlaseWait.Text = "Connecting error";
            }
            break;
          case DAHCommnad.UnPack:
          case DAHCommnad.DownloadFiles:
          case DAHCommnad.Update:
            this._btnUSAll.Enabled =
              this._btnDownload.Enabled =
              this._btnSAll.Enabled = 
              this._btnClose.Enabled = true;

            if(cmd == DAHCommnad.Update)
              this.UpdateDAHRows();
            break;
        }
      } else {
        this.Invoke(new DAHHandler(this._dah_StoppingCommand), new object[] { cmd });
      }
    }
    #endregion

    #region protected override void OnActivated(EventArgs e)
    protected override void OnActivated(EventArgs e) {
      base.OnActivated(e);
      if(_firstactivate) {
        _firstactivate = false;
        _dah.GetInfoServer();
      }
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
      this._btnDownload = new System.Windows.Forms.Button();
      this._btnClose = new System.Windows.Forms.Button();
      this._lstInfo = new System.Windows.Forms.ListView();
      this._lblWarning = new System.Windows.Forms.Label();
      this._btnSAll = new System.Windows.Forms.Button();
      this._btnUSAll = new System.Windows.Forms.Button();
      this._lblPlaseWait = new System.Windows.Forms.Label();
      this._lblMessage = new System.Windows.Forms.Label();
      this._pgs = new System.Windows.Forms.ProgressBar();
      this._lblSymbol = new System.Windows.Forms.Label();
      this._cmbSymbol = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // _btnDownload
      // 
      this._btnDownload.Location = new System.Drawing.Point(8, 408);
      this._btnDownload.Name = "_btnDownload";
      this._btnDownload.Size = new System.Drawing.Size(104, 23);
      this._btnDownload.TabIndex = 2;
      this._btnDownload.Text = "Download";
      this._btnDownload.Click += new System.EventHandler(this._btnDownload_Click);
      // 
      // _btnClose
      // 
      this._btnClose.Location = new System.Drawing.Point(296, 440);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(75, 23);
      this._btnClose.TabIndex = 2;
      this._btnClose.Text = "Close";
      this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
      // 
      // _lstInfo
      // 
      this._lstInfo.FullRowSelect = true;
      this._lstInfo.Location = new System.Drawing.Point(8, 81);
      this._lstInfo.MultiSelect = false;
      this._lstInfo.Name = "_lstInfo";
      this._lstInfo.Size = new System.Drawing.Size(360, 279);
      this._lstInfo.TabIndex = 3;
      this._lstInfo.UseCompatibleStateImageBehavior = false;
      this._lstInfo.View = System.Windows.Forms.View.Details;
      // 
      // _lblWarning
      // 
      this._lblWarning.Location = new System.Drawing.Point(8, 8);
      this._lblWarning.Name = "_lblWarning";
      this._lblWarning.Size = new System.Drawing.Size(360, 48);
      this._lblWarning.TabIndex = 9;
      this._lblWarning.Text = "label1";
      // 
      // _btnSAll
      // 
      this._btnSAll.Location = new System.Drawing.Point(152, 408);
      this._btnSAll.Name = "_btnSAll";
      this._btnSAll.Size = new System.Drawing.Size(96, 23);
      this._btnSAll.TabIndex = 2;
      this._btnSAll.Text = "Select All";
      this._btnSAll.Visible = false;
      this._btnSAll.Click += new System.EventHandler(this._btnSAll_Click);
      // 
      // _btnUSAll
      // 
      this._btnUSAll.Location = new System.Drawing.Point(256, 408);
      this._btnUSAll.Name = "_btnUSAll";
      this._btnUSAll.Size = new System.Drawing.Size(112, 23);
      this._btnUSAll.TabIndex = 2;
      this._btnUSAll.Text = "UnSelect All";
      this._btnUSAll.Click += new System.EventHandler(this._btnUSAll_Click);
      // 
      // _lblPlaseWait
      // 
      this._lblPlaseWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblPlaseWait.ForeColor = System.Drawing.Color.Red;
      this._lblPlaseWait.Location = new System.Drawing.Point(8, 408);
      this._lblPlaseWait.Name = "_lblPlaseWait";
      this._lblPlaseWait.Size = new System.Drawing.Size(368, 56);
      this._lblPlaseWait.TabIndex = 10;
      this._lblPlaseWait.Text = "Please wait...";
      // 
      // _lblMessage
      // 
      this._lblMessage.Location = new System.Drawing.Point(8, 368);
      this._lblMessage.Name = "_lblMessage";
      this._lblMessage.Size = new System.Drawing.Size(352, 16);
      this._lblMessage.TabIndex = 11;
      // 
      // _pgs
      // 
      this._pgs.Location = new System.Drawing.Point(8, 384);
      this._pgs.Name = "_pgs";
      this._pgs.Size = new System.Drawing.Size(360, 16);
      this._pgs.TabIndex = 12;
      // 
      // _lblSymbol
      // 
      this._lblSymbol.Location = new System.Drawing.Point(8, 57);
      this._lblSymbol.Name = "_lblSymbol";
      this._lblSymbol.Size = new System.Drawing.Size(100, 19);
      this._lblSymbol.TabIndex = 13;
      this._lblSymbol.Text = "Symbol";
      // 
      // _cmbSymbol
      // 
      this._cmbSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbSymbol.FormattingEnabled = true;
      this._cmbSymbol.Location = new System.Drawing.Point(89, 56);
      this._cmbSymbol.Name = "_cmbSymbol";
      this._cmbSymbol.Size = new System.Drawing.Size(277, 21);
      this._cmbSymbol.TabIndex = 14;
      this._cmbSymbol.SelectedIndexChanged += new System.EventHandler(this._cmbSymbol_SelectedIndexChanged);
      // 
      // ServerArhiveForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(378, 466);
      this.Controls.Add(this._cmbSymbol);
      this.Controls.Add(this._lblSymbol);
      this.Controls.Add(this._pgs);
      this.Controls.Add(this._lblMessage);
      this.Controls.Add(this._lblPlaseWait);
      this.Controls.Add(this._lblWarning);
      this.Controls.Add(this._lstInfo);
      this.Controls.Add(this._btnDownload);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._btnSAll);
      this.Controls.Add(this._btnUSAll);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ServerArhiveForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.ResumeLayout(false);

		}
		#endregion

		#region private void _btnUpdate_Click(object sender, System.EventArgs e) 
		private void _btnUpdate_Click(object sender, System.EventArgs e) {
			_dah.GetInfoServer();
		}
		#endregion

		#region private void AddColumn(ListView lst)
		private void AddColumn(ListView lst){
			lst.CheckBoxes = true;
			System.Windows.Forms.ColumnHeader colSymbolName = new ColumnHeader();
			System.Windows.Forms.ColumnHeader colSize = new ColumnHeader();
			System.Windows.Forms.ColumnHeader colBDate = new ColumnHeader();
			System.Windows.Forms.ColumnHeader colEDate = new ColumnHeader();
			
			colSymbolName.Width = 108;
			colBDate.Width = 86;
			colEDate.Width = 86;
			colSize.Width = 64;

			colSymbolName.Text = Dictionary.GetString(23,6,"Символ");
			colSize.Text = Dictionary.GetString(23,7,"Размер");
			colBDate.Text = Dictionary.GetString(23,9,"От");
			colEDate.Text = Dictionary.GetString(23,10,"До");
			lst.Columns.Add(colSymbolName);
			lst.Columns.Add(colBDate);
			lst.Columns.Add(colEDate);
			lst.Columns.Add(colSize);
		}
		#endregion

		#region private void AddDownFile(ListView lst, UpdateRow urow)
		private void AddDownFile(ListView lst, UpdateRow urow){

      if(_symbolName.Length > 0 && urow.SymbolName != _symbolName) return;
			
			ISymbol symbol = GordagoMain.SymbolEngine.GetSymbol(urow.SymbolName);

			bool isdown = false;
			if (symbol != null){
				isdown = (symbol.Ticks as ITickManager).CheckPeriodInMap(urow.BeginDTime, urow.EndDTime, urow.CountTick);
			}

			ListObjItem lvi = new ListObjItem(urow.SymbolName, 0, urow);
			int fsize = urow.FileSizeArhive / 1024;
			lvi.SubItems.Add(urow.BeginDTime.ToShortDateString());
			lvi.SubItems.Add(urow.EndDTime.ToShortDateString());
			lvi.SubItems.Add(fsize.ToString() + " Kb");
			if (isdown)
				lvi.ForeColor = Color.Green;
			lst.Items.Add(lvi);
		}
		#endregion

		#region private void _btnDownload_Click(object sender, System.EventArgs e)
		private void _btnDownload_Click(object sender, System.EventArgs e) {
			this.CheckSelectPeriod();

			ArrayList al = new ArrayList();
			foreach (ListViewItem lvi in this._lstInfo.Items){
				if (lvi.Checked){
					UpdateRow urow = (lvi as ListObjItem).OverObject as UpdateRow;
					DownloadFile df = new DownloadFile();
					df.ShortFileName = urow.FileName;
					df.Size = urow.FileSizeArhive;
					al.Add(df);
				}
			}
			if (al.Count < 1)
				return;
			DownloadFile[] dfs = (DownloadFile[])al.ToArray(typeof(DownloadFile));
			_dah.DownloadFiles(dfs);
		}
		#endregion

		#region private void _btnClose_Click(object sender, System.EventArgs e) 
		private void _btnClose_Click(object sender, System.EventArgs e) {
			this._dah.Abort();
			this.Close();
		}
		#endregion

		#region private void CheckSelectPeriod()
		private void CheckSelectPeriod(){
			string sname = "";
			int countcheck = 0;
			DateTime sbdtm = DateTime.Now;
			DateTime sedtm = DateTime.Now;

			foreach(ListViewItem lvi in this._lstInfo.Items){
				UpdateRow urow = (lvi as ListObjItem).OverObject as UpdateRow;
				if (sname != urow.SymbolName){
					if (countcheck > 1){
						SelectPeriod(sname, sbdtm, sedtm);
					}
					countcheck = 0;
					sname = urow.SymbolName;
				}
				if (lvi.Checked && countcheck == 0){
					sbdtm = urow.BeginDTime;
					countcheck++;
				}else if (lvi.Checked && countcheck >= 1){
					sedtm = urow.BeginDTime;
					countcheck++;
				}
			}
		}
		#endregion

		#region private void SelectPeriod(string symbolname, DateTime bdtm, DateTime edtm)
		private void SelectPeriod(string symbolname, DateTime bdtm, DateTime edtm){
			foreach(ListViewItem lvi in this._lstInfo.Items){
				UpdateRow urow = (lvi as ListObjItem).OverObject as UpdateRow;
				if (symbolname == urow.SymbolName && 
					urow.BeginDTime.Ticks > bdtm.Ticks &&
					urow.BeginDTime.Ticks < edtm.Ticks){
					lvi.Checked = true;
				}
			}
		}
		#endregion

		#region private void _btnSAll_Click(object sender, System.EventArgs e) 
		private void _btnSAll_Click(object sender, System.EventArgs e) {
			foreach(ListViewItem lvi in this._lstInfo.Items){
				lvi.Checked = true;
			}		
		}
		#endregion

		#region private void _btnUSAll_Click(object sender, System.EventArgs e) 
		private void _btnUSAll_Click(object sender, System.EventArgs e) {
			foreach(ListViewItem lvi in this._lstInfo.Items){
				lvi.Checked = false;
			}		
		}
		#endregion

    #region private void UpdateDAHRows()
    private void UpdateDAHRows(){
      if(_symbolName.Length > 0) {
        this.UpdateList();
      } else {
        this._lblSymbol.Visible =
          _cmbSymbol.Visible = true;

        Dictionary<string, string> dict = new Dictionary<string, string>();
        this._cmbSymbol.Items.Clear();
        foreach(UpdateRow urow in _dah.DocUpdates.UpdateRows) {
          if(!dict.ContainsKey(urow.SymbolName)) {
            dict.Add(urow.SymbolName, urow.SymbolName);
            this._cmbSymbol.Items.Add(urow.SymbolName);
          }
        }
      }
		}
		#endregion

    private void UpdateList() {
      this._lstInfo.Items.Clear();
      string sname = "";
      if (_symbolName.Length > 0){
        sname = _symbolName;
      }else{
        if(this._cmbSymbol.SelectedItem == null) return;
        sname = this._cmbSymbol.SelectedItem as string;
      }

      foreach(UpdateRow urow in _dah.DocUpdates.UpdateRows) {
        if(urow.SymbolName == sname)
          this.AddDownFile(this._lstInfo, urow);
      }
    }

    private void _cmbSymbol_SelectedIndexChanged(object sender, EventArgs e) {
      this.UpdateList();
    }
    
	}
}
