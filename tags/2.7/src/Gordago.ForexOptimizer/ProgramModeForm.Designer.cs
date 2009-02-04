namespace Gordago {
  partial class ProgramModeForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramModeForm));
      this._lblInfo = new System.Windows.Forms.Label();
      this._btnVServer = new System.Windows.Forms.Button();
      this._btnStandart = new System.Windows.Forms.Button();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // _lblInfo
      // 
      this._lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblInfo.Location = new System.Drawing.Point(13, 13);
      this._lblInfo.Name = "_lblInfo";
      this._lblInfo.Size = new System.Drawing.Size(321, 50);
      this._lblInfo.TabIndex = 0;
      this._lblInfo.Text = "Operating mode of the program";
      // 
      // _btnVServer
      // 
      this._btnVServer.Location = new System.Drawing.Point(24, 113);
      this._btnVServer.Name = "_btnVServer";
      this._btnVServer.Size = new System.Drawing.Size(138, 23);
      this._btnVServer.TabIndex = 1;
      this._btnVServer.Text = "Virtual Server";
      this._btnVServer.UseVisualStyleBackColor = true;
      this._btnVServer.Click += new System.EventHandler(this._btnVServer_Click);
      // 
      // _btnStandart
      // 
      this._btnStandart.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnStandart.Location = new System.Drawing.Point(185, 113);
      this._btnStandart.Name = "_btnStandart";
      this._btnStandart.Size = new System.Drawing.Size(138, 23);
      this._btnStandart.TabIndex = 1;
      this._btnStandart.Text = "Standart";
      this._btnStandart.UseVisualStyleBackColor = true;
      this._btnStandart.Click += new System.EventHandler(this._btnStandart_Click);
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(13, 70);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(100, 17);
      this.checkBox1.TabIndex = 2;
      this.checkBox1.Text = "Show at startup";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // ProgramModeForm
      // 
      this.AcceptButton = this._btnVServer;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnStandart;
      this.ClientSize = new System.Drawing.Size(346, 157);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this._btnStandart);
      this.Controls.Add(this._btnVServer);
      this.Controls.Add(this._lblInfo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ProgramModeForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Gordago";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label _lblInfo;
    private System.Windows.Forms.Button _btnVServer;
    private System.Windows.Forms.Button _btnStandart;
    private System.Windows.Forms.CheckBox checkBox1;
  }
}