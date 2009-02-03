namespace Gordago.FO.Quotes {
  partial class QuotesWindow {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._dataGridView = new System.Windows.Forms.DataGridView();
      this._dgvColSymbolName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._dgvColBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this._dgvColAsk = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // _dataGridView
      // 
      this._dataGridView.AllowUserToAddRows = false;
      this._dataGridView.AllowUserToDeleteRows = false;
      this._dataGridView.BackgroundColor = System.Drawing.Color.White;
      this._dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._dgvColSymbolName,
            this._dgvColBid,
            this._dgvColAsk});
      this._dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this._dataGridView.Location = new System.Drawing.Point(0, 0);
      this._dataGridView.MultiSelect = false;
      this._dataGridView.Name = "_dataGridView";
      this._dataGridView.ReadOnly = true;
      this._dataGridView.RowHeadersVisible = false;
      this._dataGridView.RowTemplate.ReadOnly = true;
      this._dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this._dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this._dataGridView.Size = new System.Drawing.Size(195, 448);
      this._dataGridView.TabIndex = 0;
      this._dataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._dataGridView_CellMouseDoubleClick);
      // 
      // _dgvColSymbolName
      // 
      this._dgvColSymbolName.DataPropertyName = "symbol";
      this._dgvColSymbolName.HeaderText = "Symbol";
      this._dgvColSymbolName.Name = "_dgvColSymbolName";
      this._dgvColSymbolName.ReadOnly = true;
      // 
      // _dgvColBid
      // 
      this._dgvColBid.DataPropertyName = "bid";
      this._dgvColBid.HeaderText = "Bid";
      this._dgvColBid.Name = "_dgvColBid";
      this._dgvColBid.ReadOnly = true;
      // 
      // _dgvColAsk
      // 
      this._dgvColAsk.DataPropertyName = "ask";
      this._dgvColAsk.HeaderText = "Ask";
      this._dgvColAsk.Name = "_dgvColAsk";
      this._dgvColAsk.ReadOnly = true;
      // 
      // QuotesWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(195, 448);
      this.Controls.Add(this._dataGridView);
      this.Name = "QuotesWindow";
      ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView _dataGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColSymbolName;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColBid;
    private System.Windows.Forms.DataGridViewTextBoxColumn _dgvColAsk;
  }
}
