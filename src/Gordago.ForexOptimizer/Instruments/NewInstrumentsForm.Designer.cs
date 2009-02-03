namespace Gordago.FO.Instruments {
  partial class NewInstrumentsForm {
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
      this._treeView = new System.Windows.Forms.TreeView();
      this._lblName = new System.Windows.Forms.Label();
      this._txtName = new System.Windows.Forms.TextBox();
      this._btnOK = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _treeView
      // 
      this._treeView.Location = new System.Drawing.Point(12, 12);
      this._treeView.Name = "_treeView";
      this._treeView.Size = new System.Drawing.Size(190, 143);
      this._treeView.TabIndex = 0;
      // 
      // _lblName
      // 
      this._lblName.AutoSize = true;
      this._lblName.Location = new System.Drawing.Point(9, 168);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new System.Drawing.Size(35, 13);
      this._lblName.TabIndex = 1;
      this._lblName.Text = "Name";
      // 
      // _txtName
      // 
      this._txtName.Location = new System.Drawing.Point(12, 185);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new System.Drawing.Size(190, 20);
      this._txtName.TabIndex = 2;
      // 
      // _btnOK
      // 
      this._btnOK.Location = new System.Drawing.Point(35, 225);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 22);
      this._btnOK.TabIndex = 3;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(116, 225);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 22);
      this._btnCancel.TabIndex = 3;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // NewInstrumentsForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(226, 259);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.Controls.Add(this._txtName);
      this.Controls.Add(this._lblName);
      this.Controls.Add(this._treeView);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "NewInstrumentsForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "New Instrument";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TreeView _treeView;
    private System.Windows.Forms.Label _lblName;
    private System.Windows.Forms.TextBox _txtName;
    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Button _btnCancel;
  }
}