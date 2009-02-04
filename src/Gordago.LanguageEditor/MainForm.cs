using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Language;

namespace LanguageEditor {
	public class EditLangForm : System.Windows.Forms.Form {
		
		#region private property
		private System.Windows.Forms.ListView lstLang;
		private System.Windows.Forms.ColumnHeader colNameGroup;
		private System.Windows.Forms.ColumnHeader colText;
		private System.Windows.Forms.Button btnAddGroup;
		private System.Windows.Forms.Button cmdDelGroup;
		private System.Windows.Forms.Button btnAddItem;
		private System.Windows.Forms.Button cmdDelItem;
		private System.Windows.Forms.ListView lstLngItem;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnEditItem;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button cmdEdit;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtQuery;
		private System.Windows.Forms.PictureBox picBox;
		private System.Windows.Forms.CheckBox chkNamspace;
		private System.Windows.Forms.Button _btnOpen;
		#endregion

		private readonly string FileFilter = "Gordago language (*.lng)|*.lng|All files (*.*)|*.*" ;
		private System.Windows.Forms.TextBox _txtEngName;

		private System.ComponentModel.Container components = null;

		public EditLangForm() {
			InitializeComponent();
			lngdata = new LanguageData();
		}

		#region protected override void Dispose( bool disposing )
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EditLangForm));
			this.lstLang = new System.Windows.Forms.ListView();
			this.colNameGroup = new System.Windows.Forms.ColumnHeader();
			this.lstLngItem = new System.Windows.Forms.ListView();
			this.colText = new System.Windows.Forms.ColumnHeader();
			this.btnAddGroup = new System.Windows.Forms.Button();
			this.cmdDelGroup = new System.Windows.Forms.Button();
			this.btnAddItem = new System.Windows.Forms.Button();
			this.cmdDelItem = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnEditItem = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.cmdEdit = new System.Windows.Forms.Button();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtQuery = new System.Windows.Forms.TextBox();
			this.picBox = new System.Windows.Forms.PictureBox();
			this.chkNamspace = new System.Windows.Forms.CheckBox();
			this._btnOpen = new System.Windows.Forms.Button();
			this._txtEngName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lstLang
			// 
			this.lstLang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.lstLang.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																							this.colNameGroup});
			this.lstLang.FullRowSelect = true;
			this.lstLang.HideSelection = false;
			this.lstLang.Location = new System.Drawing.Point(8, 40);
			this.lstLang.MultiSelect = false;
			this.lstLang.Name = "lstLang";
			this.lstLang.Size = new System.Drawing.Size(144, 304);
			this.lstLang.TabIndex = 0;
			this.lstLang.View = System.Windows.Forms.View.Details;
			this.lstLang.SelectedIndexChanged += new System.EventHandler(this.lstLang_SelectedIndexChanged);
			// 
			// colNameGroup
			// 
			this.colNameGroup.Text = "Группы";
			this.colNameGroup.Width = 139;
			// 
			// lstLngItem
			// 
			this.lstLngItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lstLngItem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																								 this.colText});
			this.lstLngItem.FullRowSelect = true;
			this.lstLngItem.HideSelection = false;
			this.lstLngItem.Location = new System.Drawing.Point(152, 40);
			this.lstLngItem.MultiSelect = false;
			this.lstLngItem.Name = "lstLngItem";
			this.lstLngItem.Size = new System.Drawing.Size(384, 304);
			this.lstLngItem.TabIndex = 1;
			this.lstLngItem.View = System.Windows.Forms.View.Details;
			this.lstLngItem.SelectedIndexChanged += new System.EventHandler(this.lstLngItem_SelectedIndexChanged);
			// 
			// colText
			// 
			this.colText.Text = "Текст";
			this.colText.Width = 379;
			// 
			// btnAddGroup
			// 
			this.btnAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddGroup.Location = new System.Drawing.Point(88, 352);
			this.btnAddGroup.Name = "btnAddGroup";
			this.btnAddGroup.Size = new System.Drawing.Size(72, 23);
			this.btnAddGroup.TabIndex = 2;
			this.btnAddGroup.Text = "Добавить";
			this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
			// 
			// cmdDelGroup
			// 
			this.cmdDelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdDelGroup.Location = new System.Drawing.Point(168, 352);
			this.cmdDelGroup.Name = "cmdDelGroup";
			this.cmdDelGroup.Size = new System.Drawing.Size(72, 23);
			this.cmdDelGroup.TabIndex = 3;
			this.cmdDelGroup.Text = "Удалить";
			this.cmdDelGroup.Click += new System.EventHandler(this.cmdDelGroup_Click);
			// 
			// btnAddItem
			// 
			this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddItem.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnAddItem.Location = new System.Drawing.Point(544, 88);
			this.btnAddItem.Name = "btnAddItem";
			this.btnAddItem.TabIndex = 4;
			this.btnAddItem.Text = "Добавить";
			this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
			// 
			// cmdDelItem
			// 
			this.cmdDelItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdDelItem.Location = new System.Drawing.Point(544, 120);
			this.cmdDelItem.Name = "cmdDelItem";
			this.cmdDelItem.TabIndex = 5;
			this.cmdDelItem.Text = "Удалить";
			this.cmdDelItem.Click += new System.EventHandler(this.cmdDelItem_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(544, 352);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 8;
			this.btnClose.Text = "Закрыть";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnEditItem
			// 
			this.btnEditItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditItem.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnEditItem.Location = new System.Drawing.Point(544, 56);
			this.btnEditItem.Name = "btnEditItem";
			this.btnEditItem.TabIndex = 4;
			this.btnEditItem.Text = "Править";
			this.btnEditItem.Click += new System.EventHandler(this.btnEditItem_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(448, 352);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 24);
			this.btnSave.TabIndex = 9;
			this.btnSave.Text = "Сохранить";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// cmdEdit
			// 
			this.cmdEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdEdit.Location = new System.Drawing.Point(8, 352);
			this.cmdEdit.Name = "cmdEdit";
			this.cmdEdit.TabIndex = 10;
			this.cmdEdit.Text = "Править";
			this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(8, 8);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(88, 20);
			this.txtName.TabIndex = 7;
			this.txtName.Text = "";
			// 
			// txtQuery
			// 
			this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtQuery.Location = new System.Drawing.Point(368, 8);
			this.txtQuery.Name = "txtQuery";
			this.txtQuery.Size = new System.Drawing.Size(216, 20);
			this.txtQuery.TabIndex = 12;
			this.txtQuery.Text = "";
			// 
			// picBox
			// 
			this.picBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picBox.Image = ((System.Drawing.Image)(resources.GetObject("picBox.Image")));
			this.picBox.Location = new System.Drawing.Point(592, 8);
			this.picBox.Name = "picBox";
			this.picBox.Size = new System.Drawing.Size(24, 24);
			this.picBox.TabIndex = 13;
			this.picBox.TabStop = false;
			this.picBox.Click += new System.EventHandler(this.picBox_Click);
			// 
			// chkNamspace
			// 
			this.chkNamspace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkNamspace.Checked = true;
			this.chkNamspace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkNamspace.Location = new System.Drawing.Point(272, 6);
			this.chkNamspace.Name = "chkNamspace";
			this.chkNamspace.Size = new System.Drawing.Size(88, 24);
			this.chkNamspace.TabIndex = 14;
			this.chkNamspace.Text = "Namespace";
			this.chkNamspace.CheckedChanged += new System.EventHandler(this.chkNamspace_CheckedChanged);
			// 
			// _btnOpen
			// 
			this._btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOpen.Location = new System.Drawing.Point(368, 352);
			this._btnOpen.Name = "_btnOpen";
			this._btnOpen.Size = new System.Drawing.Size(72, 24);
			this._btnOpen.TabIndex = 15;
			this._btnOpen.Text = "Открыть";
			this._btnOpen.Click += new System.EventHandler(this._btnOpen_Click);
			// 
			// _txtEngName
			// 
			this._txtEngName.Location = new System.Drawing.Point(112, 8);
			this._txtEngName.Name = "_txtEngName";
			this._txtEngName.Size = new System.Drawing.Size(88, 20);
			this._txtEngName.TabIndex = 7;
			this._txtEngName.Text = "";
			// 
			// EditLangForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(624, 381);
			this.Controls.Add(this._btnOpen);
			this.Controls.Add(this.chkNamspace);
			this.Controls.Add(this.picBox);
			this.Controls.Add(this.txtQuery);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.cmdEdit);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.cmdDelItem);
			this.Controls.Add(this.btnAddItem);
			this.Controls.Add(this.cmdDelGroup);
			this.Controls.Add(this.btnAddGroup);
			this.Controls.Add(this.lstLngItem);
			this.Controls.Add(this.lstLang);
			this.Controls.Add(this.btnEditItem);
			this.Controls.Add(this._txtEngName);
			this.Name = "EditLangForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Языковый редактор";
			this.ResumeLayout(false);

		}
		#endregion

		private LanguageData lngdata;

		[STAThread]
		static void Main() {
			Application.Run(new EditLangForm());
		}

		#region private void lstLang_SelectedIndexChanged(object sender, System.EventArgs e) 
		private void lstLang_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (this.lstLang.SelectedItems.Count > 0) {
				this.LoadLngVals(lstLang.SelectedItems[0].Text);
			}
		}
		#endregion

		#region private void LoadLngVals(string groupName)
		private void LoadLngVals(string groupName){
			this.lstLngItem.Items.Clear();
			LangGroup lg = lngdata[groupName];
			for (int i=0;i<lg.Count;i++){
				this.lstLngItem.Items.Add(new ListViewLangItem(lg[i]));
			}
			this.txtQuery.Text = "";
		}
		#endregion

		#region private void btnClose_Click(object sender, System.EventArgs e) 
		private void btnClose_Click(object sender, System.EventArgs e) {
			this.Close();
		}
		#endregion

		#region private void btnEditItem_Click(object sender, System.EventArgs e) 
		private void btnEditItem_Click(object sender, System.EventArgs e) {
			if (this.lstLngItem.SelectedItems.Count < 1) return;
			ListViewLangItem lvli = (ListViewLangItem)lstLngItem.SelectedItems[0];
			string txt = lvli.Text;
			EditTextForm etf = new EditTextForm();
			etf.txtEditor.Text = txt;
			if (etf.ShowDialog() == DialogResult.OK){
				if (etf.txtEditor.Text.Trim().Length > 0){
					lvli.Text = etf.txtEditor.Text;
				}
			}
		}
		#endregion

		#region private void btnAddItem_Click(object sender, System.EventArgs e) 
		private void btnAddItem_Click(object sender, System.EventArgs e) {
			if (this.lstLang.SelectedItems.Count < 1) return;

			string groupName = lstLang.SelectedItems[0].Text;
			EditTextForm etf = new EditTextForm();
			etf.Text = "Добавление в группу: " + groupName;
			etf.txtEditor.Text = "";
			if (etf.ShowDialog() == DialogResult.OK){
				string sval = etf.txtEditor.Text.Trim();
				if (sval.Length > 0){
					this.lngdata[groupName].CreateNewLangValue(sval);
				}
			}
			this.LoadLngVals(groupName);
		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e) {
			if (lngdata.FileName == ""){
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = this.FileFilter;
				sfd.ShowDialog();
				if (sfd.FileName == "") return;
				this.lngdata.FileName = sfd.FileName;
			}

			this.lngdata.Name = this.txtName.Text;
			this.lngdata.ThisLangName = this._txtEngName.Text;
			this.lngdata.Save();
		}

		#region private void cmdDelItem_Click(object sender, System.EventArgs e)
		private void cmdDelItem_Click(object sender, System.EventArgs e) {
			if (this.lstLngItem.SelectedItems.Count < 1) return;
			ListViewLangItem lvli = (ListViewLangItem)lstLngItem.SelectedItems[0];
			string groupName = lstLang.SelectedItems[0].Text;

			this.lngdata[groupName].RemoveFromId(lvli.Id);
			this.LoadLngVals(groupName);
		}
		#endregion

		#region private void cmdEdit_Click(object sender, System.EventArgs e) 
		private void cmdEdit_Click(object sender, System.EventArgs e) {
			if (this.lstLang.SelectedItems.Count < 1) return;

			ListViewItem lvi = lstLang.SelectedItems[0];
			string groupName = lvi.Text;
			LangGroup lgrp = lngdata[groupName];

			EditTextForm etf = new EditTextForm();
			etf.Text = "Редактирование группы";
			etf.txtEditor.Text = groupName;
			if (etf.ShowDialog() == DialogResult.OK){
				if (etf.txtEditor.Text.Trim().Length > 0){
					string snn = etf.txtEditor.Text; 
					lgrp.Name = snn;
					lvi.Text = snn;
				}
			}
		}
		#endregion

		#region private void lstLngItem_SelectedIndexChanged(object sender, System.EventArgs e) 
		private void lstLngItem_SelectedIndexChanged(object sender, System.EventArgs e) {
			this.SetQueryText();
		}
		#endregion

		#region private void SetQueryText()
		private void SetQueryText(){
			if (this.lstLngItem.SelectedItems.Count < 1 || this.lstLang.SelectedItems.Count < 1){
				this.txtQuery.Text = "";
			}else{
				string sq = "Dictionary.GetString(";
				if (this.chkNamspace.Checked){
					sq = "Language." + sq;
				}

				int lgrpId = ((ListViewLangGroupItem)lstLang.SelectedItems[0]).Id;
				int lgvId = ((ListViewLangItem)this.lstLngItem.SelectedItems[0]).Id;
				string lngmess = ((ListViewLangItem)this.lstLngItem.SelectedItems[0]).Text;
				sq += Convert.ToString(lgrpId) + "," + Convert.ToString(lgvId) + "," + "\"" + lngmess +"\")";
				this.txtQuery.Text = sq;
			}
		}
		#endregion 

		#region private void btnAddGroup_Click(object sender, System.EventArgs e) 
		private void btnAddGroup_Click(object sender, System.EventArgs e) {
			EditTextForm etf = new EditTextForm();
			etf.Text = "Создание новой группы";
			etf.txtEditor.Text = "";
			if (etf.ShowDialog() == DialogResult.OK){
				string sval = etf.txtEditor.Text.Trim();
				if (sval.Length > 0){
					LangGroup lgrp = lngdata.CreateNewLangGroup(sval);
					(lstLang.Items.Add(new ListViewLangGroupItem(lgrp))).Selected = true;
				}
			}
		}
		#endregion

		private void cmdDelGroup_Click(object sender, System.EventArgs e) {
			if (this.lstLang.SelectedItems.Count < 1) return ;

			ListViewLangGroupItem lvlgi = (ListViewLangGroupItem)this.lstLang.SelectedItems[0];
			lngdata.RemoveFromId(lvlgi.Id);
			this.lstLang.Items.RemoveAt(lvlgi.Index);
			this.lstLngItem.Items.Clear();
		}

		private void picBox_Click(object sender, System.EventArgs e) {
			this.txtQuery.SelectAll();
			this.txtQuery.Copy();
		}

		private void chkNamspace_CheckedChanged(object sender, System.EventArgs e) {
			this.SetQueryText();
		}

		private void _btnOpen_Click(object sender, System.EventArgs e) {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = FileFilter;
			ofd.ShowDialog();
			if (ofd.FileName == "") return;

			lngdata = new LanguageData();
			lngdata.Load(ofd.FileName);
			this.Text = ofd.FileName;

			for (int i=0;i<lngdata.Count;i++){
				ListViewLangGroupItem lvlgi = new ListViewLangGroupItem(lngdata[i]);
				this.lstLang.Items.Add(lvlgi);
			}
			this.txtName.Text = this.lngdata.Name;
			this._txtEngName.Text = this.lngdata.ThisLangName;

		}
	}
}
