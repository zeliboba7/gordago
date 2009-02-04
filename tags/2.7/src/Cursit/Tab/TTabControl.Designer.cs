namespace Cursit.Tab {
  partial class TTabControl {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._mainpanel = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // _mainpanel
      // 
      this._mainpanel.Location = new System.Drawing.Point(2, 0);
      this._mainpanel.Name = "_mainpanel";
      this._mainpanel.Size = new System.Drawing.Size(559, 192);
      this._mainpanel.TabIndex = 0;
      // 
      // TTabControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._mainpanel);
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "TTabControl";
      this.Size = new System.Drawing.Size(562, 220);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel _mainpanel;
  }
}
