namespace Gordago.PlugIn.MetaQuotesHistory {
  partial class ConvertToTicksForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if(disposing && (components != null)) {
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
      this._lstMQHFiles = new System.Windows.Forms.ListView();
      this._colIndex = new System.Windows.Forms.ColumnHeader();
      this._colFile = new System.Windows.Forms.ColumnHeader();
      this._colSymbolName = new System.Windows.Forms.ColumnHeader();
      this._colTimeFrame = new System.Windows.Forms.ColumnHeader();
      this._colDateTimeFrom = new System.Windows.Forms.ColumnHeader();
      this._colDateTimeTo = new System.Windows.Forms.ColumnHeader();
      this._btnClose = new System.Windows.Forms.Button();
      this._btnConvert = new System.Windows.Forms.Button();
      this._pgsConvert = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // _lstMQHFiles
      // 
      this._lstMQHFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lstMQHFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colIndex,
            this._colFile,
            this._colSymbolName,
            this._colTimeFrame,
            this._colDateTimeFrom,
            this._colDateTimeTo});
      this._lstMQHFiles.FullRowSelect = true;
      this._lstMQHFiles.GridLines = true;
      this._lstMQHFiles.Location = new System.Drawing.Point(12, 12);
      this._lstMQHFiles.MultiSelect = false;
      this._lstMQHFiles.Name = "_lstMQHFiles";
      this._lstMQHFiles.Size = new System.Drawing.Size(559, 339);
      this._lstMQHFiles.TabIndex = 0;
      this._lstMQHFiles.UseCompatibleStateImageBehavior = false;
      this._lstMQHFiles.View = System.Windows.Forms.View.Details;
      this._lstMQHFiles.SelectedIndexChanged += new System.EventHandler(this._lstMQHFiles_SelectedIndexChanged);
      // 
      // _colIndex
      // 
      this._colIndex.Text = "NN";
      this._colIndex.Width = 32;
      // 
      // _colFile
      // 
      this._colFile.Text = "File";
      this._colFile.Width = 122;
      // 
      // _colSymbolName
      // 
      this._colSymbolName.Text = "Symbol";
      this._colSymbolName.Width = 59;
      // 
      // _colTimeFrame
      // 
      this._colTimeFrame.Text = "TF";
      this._colTimeFrame.Width = 39;
      // 
      // _colDateTimeFrom
      // 
      this._colDateTimeFrom.Text = "From";
      this._colDateTimeFrom.Width = 150;
      // 
      // _colDateTimeTo
      // 
      this._colDateTimeTo.Text = "To";
      this._colDateTimeTo.Width = 136;
      // 
      // _btnClose
      // 
      this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnClose.Location = new System.Drawing.Point(496, 373);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(75, 23);
      this._btnClose.TabIndex = 1;
      this._btnClose.Text = "Close";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
      // 
      // _btnConvert
      // 
      this._btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnConvert.Enabled = false;
      this._btnConvert.Location = new System.Drawing.Point(12, 373);
      this._btnConvert.Name = "_btnConvert";
      this._btnConvert.Size = new System.Drawing.Size(115, 23);
      this._btnConvert.TabIndex = 2;
      this._btnConvert.Text = "Convert";
      this._btnConvert.UseVisualStyleBackColor = true;
      this._btnConvert.Click += new System.EventHandler(this._btnConvert_Click);
      // 
      // _pgsConvert
      // 
      this._pgsConvert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._pgsConvert.Location = new System.Drawing.Point(12, 357);
      this._pgsConvert.Name = "_pgsConvert";
      this._pgsConvert.Size = new System.Drawing.Size(559, 10);
      this._pgsConvert.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this._pgsConvert.TabIndex = 3;
      // 
      // ConvertToTicksForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnClose;
      this.ClientSize = new System.Drawing.Size(583, 403);
      this.Controls.Add(this._pgsConvert);
      this.Controls.Add(this._btnConvert);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._lstMQHFiles);
      this.Name = "ConvertToTicksForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Convert to ticks";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView _lstMQHFiles;
    private System.Windows.Forms.Button _btnClose;
    private System.Windows.Forms.ColumnHeader _colIndex;
    private System.Windows.Forms.ColumnHeader _colFile;
    private System.Windows.Forms.ColumnHeader _colDateTimeFrom;
    private System.Windows.Forms.ColumnHeader _colDateTimeTo;
    private System.Windows.Forms.ColumnHeader _colSymbolName;
    private System.Windows.Forms.ColumnHeader _colTimeFrame;
    private System.Windows.Forms.Button _btnConvert;
    private System.Windows.Forms.ProgressBar _pgsConvert;

  }
}