namespace Gordago.API {
  partial class VirtualBrokerSettingsForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VirtualBrokerSettingsForm));
      this._btnOk = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this._settingsPanel = new Gordago.API.VirtualForex.VSSettingsPanel();
      this.SuspendLayout();
      // 
      // _btnOk
      // 
      this._btnOk.Location = new System.Drawing.Point(48, 138);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(70, 23);
      this._btnOk.TabIndex = 0;
      this._btnOk.Text = "OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(138, 138);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(70, 23);
      this._btnCancel.TabIndex = 0;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // _settingsPanel
      // 
      this._settingsPanel.Location = new System.Drawing.Point(4, 6);
      this._settingsPanel.Name = "_settingsPanel";
      this._settingsPanel.Size = new System.Drawing.Size(240, 126);
      this._settingsPanel.TabIndex = 1;
      // 
      // VirtualBrokerSettingsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(248, 169);
      this.Controls.Add(this._settingsPanel);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._btnOk);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "VirtualBrokerSettingsForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Settings";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Button _btnCancel;
    private Gordago.API.VirtualForex.VSSettingsPanel _settingsPanel;
  }
}