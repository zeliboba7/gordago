namespace Gordago.IOTicks {
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._statusStrip = new System.Windows.Forms.StatusStrip();
      this._lblStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
      this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
      this._dataGridView = new System.Windows.Forms.DataGridView();
      this._columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._columnDigits = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._columnFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._columnTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._columnTickCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._btnImport = new System.Windows.Forms.Button();
      this._btnClose = new System.Windows.Forms.Button();
      this._pgsSymbols = new System.Windows.Forms.ProgressBar();
      this._statusStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // _statusStrip
      // 
      this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblStatusInfo,
            this._progressBar});
      this._statusStrip.Location = new System.Drawing.Point(0, 430);
      this._statusStrip.Name = "_statusStrip";
      this._statusStrip.Size = new System.Drawing.Size(747, 22);
      this._statusStrip.TabIndex = 0;
      this._statusStrip.Text = "statusStrip1";
      // 
      // _lblStatusInfo
      // 
      this._lblStatusInfo.AutoSize = false;
      this._lblStatusInfo.Name = "_lblStatusInfo";
      this._lblStatusInfo.Size = new System.Drawing.Size(200, 17);
      this._lblStatusInfo.Text = "Ready";
      this._lblStatusInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // _progressBar
      // 
      this._progressBar.Name = "_progressBar";
      this._progressBar.Size = new System.Drawing.Size(100, 16);
      this._progressBar.Visible = false;
      // 
      // _dataGridView
      // 
      this._dataGridView.AllowUserToAddRows = false;
      this._dataGridView.AllowUserToDeleteRows = false;
      this._dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._columnName,
            this._columnDigits,
            this._columnFrom,
            this._columnTo,
            this._columnTickCount});
      this._dataGridView.Location = new System.Drawing.Point(12, 12);
      this._dataGridView.Name = "_dataGridView";
      this._dataGridView.ReadOnly = true;
      this._dataGridView.RowHeadersVisible = false;
      this._dataGridView.Size = new System.Drawing.Size(723, 355);
      this._dataGridView.TabIndex = 1;
      // 
      // _columnName
      // 
      this._columnName.DataPropertyName = "name";
      this._columnName.HeaderText = "Symbol";
      this._columnName.Name = "_columnName";
      this._columnName.ReadOnly = true;
      this._columnName.Width = 200;
      // 
      // _columnDigits
      // 
      this._columnDigits.DataPropertyName = "digits";
      this._columnDigits.HeaderText = "Digits";
      this._columnDigits.Name = "_columnDigits";
      this._columnDigits.ReadOnly = true;
      // 
      // _columnFrom
      // 
      this._columnFrom.DataPropertyName = "from";
      this._columnFrom.HeaderText = "From";
      this._columnFrom.Name = "_columnFrom";
      this._columnFrom.ReadOnly = true;
      // 
      // _columnTo
      // 
      this._columnTo.DataPropertyName = "to";
      this._columnTo.HeaderText = "To";
      this._columnTo.Name = "_columnTo";
      this._columnTo.ReadOnly = true;
      // 
      // _columnTickCount
      // 
      this._columnTickCount.DataPropertyName = "ticks_count";
      this._columnTickCount.HeaderText = "Ticks Count";
      this._columnTickCount.Name = "_columnTickCount";
      this._columnTickCount.ReadOnly = true;
      // 
      // _btnImport
      // 
      this._btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnImport.Location = new System.Drawing.Point(13, 393);
      this._btnImport.Name = "_btnImport";
      this._btnImport.Size = new System.Drawing.Size(111, 23);
      this._btnImport.TabIndex = 2;
      this._btnImport.Text = "Import";
      this._btnImport.UseVisualStyleBackColor = true;
      this._btnImport.Click += new System.EventHandler(this._btnImport_Click);
      // 
      // _btnClose
      // 
      this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnClose.Location = new System.Drawing.Point(629, 393);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(106, 23);
      this._btnClose.TabIndex = 3;
      this._btnClose.Text = "Close";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
      // 
      // _pgsSymbols
      // 
      this._pgsSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pgsSymbols.Location = new System.Drawing.Point(12, 373);
      this._pgsSymbols.Name = "_pgsSymbols";
      this._pgsSymbols.Size = new System.Drawing.Size(723, 10);
      this._pgsSymbols.TabIndex = 4;
      this._pgsSymbols.Visible = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(747, 452);
      this.Controls.Add(this._pgsSymbols);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._btnImport);
      this.Controls.Add(this._dataGridView);
      this.Controls.Add(this._statusStrip);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "IO Ticks";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this._statusStrip.ResumeLayout(false);
      this._statusStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip _statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel _lblStatusInfo;
    private System.Windows.Forms.ToolStripProgressBar _progressBar;
    private System.Windows.Forms.DataGridView _dataGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn _columnName;
    private System.Windows.Forms.DataGridViewTextBoxColumn _columnDigits;
    private System.Windows.Forms.DataGridViewTextBoxColumn _columnFrom;
    private System.Windows.Forms.DataGridViewTextBoxColumn _columnTo;
    private System.Windows.Forms.DataGridViewTextBoxColumn _columnTickCount;
    private System.Windows.Forms.Button _btnImport;
    private System.Windows.Forms.Button _btnClose;
    private System.Windows.Forms.ProgressBar _pgsSymbols;
  }
}

