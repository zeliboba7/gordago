namespace Gordago.LangEditor.AppList {
  partial class AddLanguageForm {
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
      this._lblApplication = new System.Windows.Forms.Label();
      this._lblChooseLanguage = new System.Windows.Forms.Label();
      this._cmbLanguages = new System.Windows.Forms.ComboBox();
      this._btnOK = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _lblApplication
      // 
      this._lblApplication.AutoSize = true;
      this._lblApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblApplication.Location = new System.Drawing.Point(13, 13);
      this._lblApplication.Name = "_lblApplication";
      this._lblApplication.Size = new System.Drawing.Size(120, 17);
      this._lblApplication.TabIndex = 0;
      this._lblApplication.Text = "Application ID: {0}";
      // 
      // _lblChooseLanguage
      // 
      this._lblChooseLanguage.AutoSize = true;
      this._lblChooseLanguage.Location = new System.Drawing.Point(13, 56);
      this._lblChooseLanguage.Name = "_lblChooseLanguage";
      this._lblChooseLanguage.Size = new System.Drawing.Size(94, 13);
      this._lblChooseLanguage.TabIndex = 1;
      this._lblChooseLanguage.Text = "Choose Language";
      // 
      // _cmbLanguages
      // 
      this._cmbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbLanguages.FormattingEnabled = true;
      this._cmbLanguages.Location = new System.Drawing.Point(122, 53);
      this._cmbLanguages.Name = "_cmbLanguages";
      this._cmbLanguages.Size = new System.Drawing.Size(444, 21);
      this._cmbLanguages.TabIndex = 2;
      // 
      // _btnOK
      // 
      this._btnOK.Location = new System.Drawing.Point(378, 150);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(75, 23);
      this._btnOK.TabIndex = 3;
      this._btnOK.Text = "OK";
      this._btnOK.UseVisualStyleBackColor = true;
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(470, 150);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 3;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // AddLanguageForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(578, 198);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.Controls.Add(this._cmbLanguages);
      this.Controls.Add(this._lblChooseLanguage);
      this.Controls.Add(this._lblApplication);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "AddLanguageForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Add Language";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label _lblApplication;
    private System.Windows.Forms.Label _lblChooseLanguage;
    private System.Windows.Forms.ComboBox _cmbLanguages;
    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Button _btnCancel;
  }
}