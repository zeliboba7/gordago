namespace Gordago.API {
  partial class HOCPDialog {
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
      this._lblText = new System.Windows.Forms.Label();
      this._btnYes = new System.Windows.Forms.Button();
      this._btnNo = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _lblText
      // 
      this._lblText.Location = new System.Drawing.Point(12, 9);
      this._lblText.Name = "_lblText";
      this._lblText.Size = new System.Drawing.Size(267, 84);
      this._lblText.TabIndex = 0;
      this._lblText.Text = "Вы собираетесь:";
      // 
      // _btnYes
      // 
      this._btnYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(245)))), ((int)(((byte)(145)))));
      this._btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._btnYes.Location = new System.Drawing.Point(56, 106);
      this._btnYes.Name = "_btnYes";
      this._btnYes.Size = new System.Drawing.Size(75, 23);
      this._btnYes.TabIndex = 1;
      this._btnYes.Text = "OK";
      this._btnYes.UseVisualStyleBackColor = false;
      this._btnYes.Click += new System.EventHandler(this._btnYes_Click);
      // 
      // _btnNo
      // 
      this._btnNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(145)))), ((int)(((byte)(152)))));
      this._btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this._btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._btnNo.Location = new System.Drawing.Point(160, 106);
      this._btnNo.Name = "_btnNo";
      this._btnNo.Size = new System.Drawing.Size(75, 23);
      this._btnNo.TabIndex = 2;
      this._btnNo.Text = "Cancel";
      this._btnNo.UseVisualStyleBackColor = false;
      this._btnNo.Click += new System.EventHandler(this._btnNo_Click);
      // 
      // HOCPDialog
      // 
      this.AcceptButton = this._btnYes;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.CancelButton = this._btnNo;
      this.ClientSize = new System.Drawing.Size(291, 144);
      this.Controls.Add(this._btnNo);
      this.Controls.Add(this._btnYes);
      this.Controls.Add(this._lblText);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "HOCPDialog";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Forex Optimizer";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label _lblText;
    private System.Windows.Forms.Button _btnYes;
    private System.Windows.Forms.Button _btnNo;
  }
}