namespace Gordago.API {
  partial class HandTradePreview {
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
      this._lblinfo = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // _lblinfo
      // 
      this._lblinfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._lblinfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this._lblinfo.Location = new System.Drawing.Point(0, 0);
      this._lblinfo.Name = "_lblinfo";
      this._lblinfo.Size = new System.Drawing.Size(116, 137);
      this._lblinfo.TabIndex = 0;
      // 
      // HandTradePreview
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._lblinfo);
      this.Name = "HandTradePreview";
      this.Size = new System.Drawing.Size(116, 137);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label _lblinfo;
  }
}
