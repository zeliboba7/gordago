namespace Gordago.WebUpdate {
  partial class GetFullVersionForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetFullVersionForm));
      this._lblCaption = new System.Windows.Forms.Label();
      this._btnOK = new System.Windows.Forms.Button();
      this._lngChoiseLang = new System.Windows.Forms.Label();
      this._rbtnEnglish = new System.Windows.Forms.RadioButton();
      this._rbtnRussian = new System.Windows.Forms.RadioButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this._btnCancel = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _lblCaption
      // 
      this._lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblCaption.Location = new System.Drawing.Point(12, 9);
      this._lblCaption.Name = "_lblCaption";
      this._lblCaption.Size = new System.Drawing.Size(297, 37);
      this._lblCaption.TabIndex = 0;
      this._lblCaption.Text = "Получить полную версию";
      // 
      // _btnOK
      // 
      this._btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(237)))));
      this._btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnOK.Location = new System.Drawing.Point(88, 169);
      this._btnOK.Name = "_btnOK";
      this._btnOK.Size = new System.Drawing.Size(114, 28);
      this._btnOK.TabIndex = 0;
      this._btnOK.Text = "Next >";
      this._btnOK.UseVisualStyleBackColor = false;
      this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
      // 
      // _lngChoiseLang
      // 
      this._lngChoiseLang.AutoSize = true;
      this._lngChoiseLang.Location = new System.Drawing.Point(12, 10);
      this._lngChoiseLang.Name = "_lngChoiseLang";
      this._lngChoiseLang.Size = new System.Drawing.Size(97, 13);
      this._lngChoiseLang.TabIndex = 2;
      this._lngChoiseLang.Text = "Choose Language:";
      // 
      // _rbtnEnglish
      // 
      this._rbtnEnglish.AutoSize = true;
      this._rbtnEnglish.Checked = true;
      this._rbtnEnglish.Location = new System.Drawing.Point(17, 39);
      this._rbtnEnglish.Name = "_rbtnEnglish";
      this._rbtnEnglish.Size = new System.Drawing.Size(59, 17);
      this._rbtnEnglish.TabIndex = 3;
      this._rbtnEnglish.TabStop = true;
      this._rbtnEnglish.Text = "English";
      this._rbtnEnglish.UseVisualStyleBackColor = true;
      this._rbtnEnglish.CheckedChanged += new System.EventHandler(this._rbtnEnglish_CheckedChanged);
      // 
      // _rbtnRussian
      // 
      this._rbtnRussian.AutoSize = true;
      this._rbtnRussian.Location = new System.Drawing.Point(17, 62);
      this._rbtnRussian.Name = "_rbtnRussian";
      this._rbtnRussian.Size = new System.Drawing.Size(67, 17);
      this._rbtnRussian.TabIndex = 3;
      this._rbtnRussian.Text = "Русский";
      this._rbtnRussian.UseVisualStyleBackColor = true;
      this._rbtnRussian.CheckedChanged += new System.EventHandler(this._rbtnRussian_CheckedChanged);
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this._lngChoiseLang);
      this.panel1.Controls.Add(this._rbtnRussian);
      this.panel1.Controls.Add(this._rbtnEnglish);
      this.panel1.Location = new System.Drawing.Point(12, 49);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(296, 103);
      this.panel1.TabIndex = 4;
      // 
      // _btnCancel
      // 
      this._btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(237)))));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnCancel.Location = new System.Drawing.Point(219, 169);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(89, 28);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = false;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // GetFullVersionForm
      // 
      this.AcceptButton = this._btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(233)))), ((int)(((byte)(250)))));
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(321, 209);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOK);
      this.Controls.Add(this._lblCaption);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GetFullVersionForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Registration";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label _lblCaption;
    private System.Windows.Forms.Button _btnOK;
    private System.Windows.Forms.Label _lngChoiseLang;
    private System.Windows.Forms.RadioButton _rbtnEnglish;
    private System.Windows.Forms.RadioButton _rbtnRussian;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button _btnCancel;
  }
}