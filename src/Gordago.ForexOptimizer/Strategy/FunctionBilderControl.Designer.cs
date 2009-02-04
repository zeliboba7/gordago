namespace Gordago.Strategy {
  partial class FunctionBilderControl {
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
      this._tbo = new Gordago.Strategy.TextBoxObject();
      this.SuspendLayout();
      // 
      // _tbo
      // 
      this._tbo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._tbo.BackColor = System.Drawing.Color.White;
      this._tbo.Cursor = System.Windows.Forms.Cursors.IBeam;
      this._tbo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
      this._tbo.Location = new System.Drawing.Point(3, 1);
      this._tbo.Name = "_tbo";
      this._tbo.Position = 0;
      this._tbo.Size = new System.Drawing.Size(336, 35);
      this._tbo.TabIndex = 0;
      // 
      // POPriceCaclulator
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = System.Drawing.Color.White;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this._tbo);
      this.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.Name = "POPriceCaclulator";
      this.Size = new System.Drawing.Size(342, 50);
      this.ResumeLayout(false);

    }

    #endregion

    private TextBoxObject _tbo;
  }
}
