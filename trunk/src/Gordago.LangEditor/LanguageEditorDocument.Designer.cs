namespace Gordago.LangEditor {
  partial class LanguageEditorDocument {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._lblDisplayName = new System.Windows.Forms.Label();
      this._txtDisplayName = new System.Windows.Forms.TextBox();
      this._lblEnglishName = new System.Windows.Forms.Label();
      this._txtEnglishName = new System.Windows.Forms.TextBox();
      this._lblCode = new System.Windows.Forms.Label();
      this._txtCode = new System.Windows.Forms.TextBox();
      this._lstGroup = new System.Windows.Forms.ListView();
      this._columnLstGroup = new System.Windows.Forms.ColumnHeader();
      this._dataGridView = new System.Windows.Forms.DataGridView();
      this._dgvColNN = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._dgvColLang = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._dgvColGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._dgvColPhraseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._dgvColPhraseValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // _lblDisplayName
      // 
      this._lblDisplayName.AutoSize = true;
      this._lblDisplayName.Location = new System.Drawing.Point(13, 13);
      this._lblDisplayName.Name = "_lblDisplayName";
      this._lblDisplayName.Size = new System.Drawing.Size(72, 13);
      this._lblDisplayName.TabIndex = 0;
      this._lblDisplayName.Text = "Display Name";
      // 
      // _txtDisplayName
      // 
      this._txtDisplayName.Location = new System.Drawing.Point(12, 29);
      this._txtDisplayName.Name = "_txtDisplayName";
      this._txtDisplayName.ReadOnly = true;
      this._txtDisplayName.Size = new System.Drawing.Size(152, 20);
      this._txtDisplayName.TabIndex = 0;
      this._txtDisplayName.TextChanged += new System.EventHandler(this._txtDisplayName_TextChanged);
      // 
      // _lblEnglishName
      // 
      this._lblEnglishName.AutoSize = true;
      this._lblEnglishName.Location = new System.Drawing.Point(186, 13);
      this._lblEnglishName.Name = "_lblEnglishName";
      this._lblEnglishName.Size = new System.Drawing.Size(72, 13);
      this._lblEnglishName.TabIndex = 0;
      this._lblEnglishName.Text = "English Name";
      // 
      // _txtEnglishName
      // 
      this._txtEnglishName.Location = new System.Drawing.Point(180, 29);
      this._txtEnglishName.Name = "_txtEnglishName";
      this._txtEnglishName.ReadOnly = true;
      this._txtEnglishName.Size = new System.Drawing.Size(137, 20);
      this._txtEnglishName.TabIndex = 1;
      // 
      // _lblCode
      // 
      this._lblCode.AutoSize = true;
      this._lblCode.Location = new System.Drawing.Point(336, 13);
      this._lblCode.Name = "_lblCode";
      this._lblCode.Size = new System.Drawing.Size(32, 13);
      this._lblCode.TabIndex = 0;
      this._lblCode.Text = "Code";
      // 
      // _txtCode
      // 
      this._txtCode.Location = new System.Drawing.Point(330, 29);
      this._txtCode.Name = "_txtCode";
      this._txtCode.ReadOnly = true;
      this._txtCode.Size = new System.Drawing.Size(137, 20);
      this._txtCode.TabIndex = 2;
      // 
      // _lstGroup
      // 
      this._lstGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this._lstGroup.BackColor = System.Drawing.Color.White;
      this._lstGroup.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._columnLstGroup});
      this._lstGroup.FullRowSelect = true;
      this._lstGroup.Location = new System.Drawing.Point(13, 65);
      this._lstGroup.MultiSelect = false;
      this._lstGroup.Name = "_lstGroup";
      this._lstGroup.Size = new System.Drawing.Size(151, 290);
      this._lstGroup.TabIndex = 3;
      this._lstGroup.UseCompatibleStateImageBehavior = false;
      this._lstGroup.View = System.Windows.Forms.View.Details;
      this._lstGroup.SelectedIndexChanged += new System.EventHandler(this._lstGroup_SelectedIndexChanged);
      // 
      // _columnLstGroup
      // 
      this._columnLstGroup.Text = "Group";
      this._columnLstGroup.Width = 130;
      // 
      // _dataGridView
      // 
      this._dataGridView.AllowUserToAddRows = false;
      this._dataGridView.AllowUserToDeleteRows = false;
      this._dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._dataGridView.BackgroundColor = System.Drawing.Color.White;
      this._dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._dgvColNN,
            this._dgvColLang,
            this._dgvColGroup,
            this._dgvColPhraseName,
            this._dgvColPhraseValue});
      this._dataGridView.Location = new System.Drawing.Point(170, 65);
      this._dataGridView.MultiSelect = false;
      this._dataGridView.Name = "_dataGridView";
      this._dataGridView.RowHeadersVisible = false;
      this._dataGridView.Size = new System.Drawing.Size(502, 290);
      this._dataGridView.TabIndex = 4;
      // 
      // _dgvColNN
      // 
      this._dgvColNN.DataPropertyName = "nn";
      this._dgvColNN.HeaderText = "NN";
      this._dgvColNN.Name = "_dgvColNN";
      this._dgvColNN.ReadOnly = true;
      this._dgvColNN.Width = 50;
      // 
      // _dgvColLang
      // 
      this._dgvColLang.DataPropertyName = "lang";
      this._dgvColLang.HeaderText = "Column1";
      this._dgvColLang.Name = "_dgvColLang";
      this._dgvColLang.ReadOnly = true;
      this._dgvColLang.Visible = false;
      // 
      // _dgvColGroup
      // 
      this._dgvColGroup.DataPropertyName = "group";
      this._dgvColGroup.HeaderText = "Column1";
      this._dgvColGroup.Name = "_dgvColGroup";
      this._dgvColGroup.ReadOnly = true;
      this._dgvColGroup.Visible = false;
      // 
      // _dgvColPhraseName
      // 
      this._dgvColPhraseName.DataPropertyName = "phrase_name";
      this._dgvColPhraseName.HeaderText = "Column1";
      this._dgvColPhraseName.Name = "_dgvColPhraseName";
      this._dgvColPhraseName.ReadOnly = true;
      this._dgvColPhraseName.Visible = false;
      // 
      // _dgvColPhraseValue
      // 
      this._dgvColPhraseValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this._dgvColPhraseValue.DataPropertyName = "phrase_value";
      this._dgvColPhraseValue.HeaderText = "Phrase";
      this._dgvColPhraseValue.Name = "_dgvColPhraseValue";
      // 
      // LanguageEditorDocument
      // 
      this.ClientSize = new System.Drawing.Size(684, 367);
      this.Controls.Add(this._dataGridView);
      this.Controls.Add(this._lstGroup);
      this.Controls.Add(this._txtCode);
      this.Controls.Add(this._txtEnglishName);
      this.Controls.Add(this._txtDisplayName);
      this.Controls.Add(this._lblCode);
      this.Controls.Add(this._lblEnglishName);
      this.Controls.Add(this._lblDisplayName);
      this.Name = "LanguageEditorDocument";
      this.TabText = "LanguageEditorDocument";
      this.Text = "LanguageEditorDocument";
      ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    private System.Windows.Forms.Label _lblDisplayName;
    private System.Windows.Forms.TextBox _txtDisplayName;
    private System.Windows.Forms.Label _lblEnglishName;
    private System.Windows.Forms.TextBox _txtEnglishName;
    private System.Windows.Forms.Label _lblCode;
    private System.Windows.Forms.TextBox _txtCode;
    private System.Windows.Forms.ListView _lstGroup;
    private System.Windows.Forms.ColumnHeader _columnLstGroup;
    private System.Windows.Forms.DataGridView _dataGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColNN;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColLang;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColGroup;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColPhraseName;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColPhraseValue;
  }
}