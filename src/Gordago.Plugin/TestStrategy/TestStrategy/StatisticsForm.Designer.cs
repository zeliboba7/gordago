namespace TestStrategy {
  partial class StatisticsForm {
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
      this._btnOK = new System.Windows.Forms.Button();
      this._lstView = new System.Windows.Forms.ListView();
      this._colName = new System.Windows.Forms.ColumnHeader();
      this._colValue = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // _btnOK
      // 
      this._btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnOK.Location = new System.Drawing.Point(261, 275);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 0;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      // 
      // _lstView
      // 
      this._lstView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._lstView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colName,
            this._colValue});
      this._lstView.FullRowSelect = true;
      this._lstView.GridLines = true;
      this._lstView.Location = new System.Drawing.Point(2, 3);
      this._lstView.Name = "_lstView";
      this._lstView.Size = new System.Drawing.Size(344, 266);
      this._lstView.TabIndex = 1;
      this._lstView.UseCompatibleStateImageBehavior = false;
      this._lstView.View = System.Windows.Forms.View.Details;
      // 
      // _colName
      // 
      this._colName.Text = "Name";
      this._colName.Width = 161;
      // 
      // _colValue
      // 
      this._colValue.Text = "Value";
      this._colValue.Width = 178;
      // 
      // StatisticsForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnOK;
      this.ClientSize = new System.Drawing.Size(348, 303);
      this.Controls.Add(this._lstView);
      this.Controls.Add(this._btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "StatisticsForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Gordago Statistics Sample";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.ListView _lstView;
    private System.Windows.Forms.ColumnHeader _colName;
    private System.Windows.Forms.ColumnHeader _colValue;
  }
}