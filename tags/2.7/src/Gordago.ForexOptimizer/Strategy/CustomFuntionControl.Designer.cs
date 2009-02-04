namespace Gordago.Strategy {
  partial class CustomFuntionControl {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._txtName = new System.Windows.Forms.TextBox();
      this._lblName = new System.Windows.Forms.Label();
      this._bilder = new Gordago.Strategy.FunctionBilderControl();
      this.SuspendLayout();
      // 
      // _txtName
      // 
      this._txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtName.Location = new System.Drawing.Point(144, 6);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new System.Drawing.Size(183, 20);
      this._txtName.TabIndex = 2;
      this._txtName.Text = "CustomFunctionName";
      this._txtName.TextChanged += new System.EventHandler(this._txtName_TextChanged);
      // 
      // _lblName
      // 
      this._lblName.AutoSize = true;
      this._lblName.Location = new System.Drawing.Point(6, 9);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new System.Drawing.Size(35, 13);
      this._lblName.TabIndex = 1;
      this._lblName.Text = "Name";
      // 
      // _bilder
      // 
      this._bilder.AllowDrop = true;
      this._bilder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._bilder.AutoScroll = true;
      this._bilder.BackColor = System.Drawing.Color.White;
      this._bilder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._bilder.Cursor = System.Windows.Forms.Cursors.IBeam;
      this._bilder.Location = new System.Drawing.Point(4, 32);
      this._bilder.Name = "_bilder";
      this._bilder.Size = new System.Drawing.Size(323, 50);
      this._bilder.TabIndex = 3;
      // 
      // CustomFuntionControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._bilder);
      this.Controls.Add(this._txtName);
      this.Controls.Add(this._lblName);
      this.Name = "CustomFuntionControl";
      this.Size = new System.Drawing.Size(333, 87);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox _txtName;
    private System.Windows.Forms.Label _lblName;
    private FunctionBilderControl _bilder;
  }
}
