namespace Gordago.Strategy {
  partial class VSBreakForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSBreakForm));
      this._textBox = new System.Windows.Forms.RichTextBox();
      this._btnClose = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _textBox
      // 
      this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._textBox.Location = new System.Drawing.Point(4, 3);
      this._textBox.Name = "_textBox";
      this._textBox.Size = new System.Drawing.Size(535, 378);
      this._textBox.TabIndex = 0;
      this._textBox.Text = "";
      // 
      // _btnClose
      // 
      this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnClose.Location = new System.Drawing.Point(455, 387);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new System.Drawing.Size(75, 23);
      this._btnClose.TabIndex = 1;
      this._btnClose.Text = "Close";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
      // 
      // VSBreakForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(542, 415);
      this.Controls.Add(this._btnClose);
      this.Controls.Add(this._textBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "VSBreakForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Break Point";
      this.Load += new System.EventHandler(this.VSBreakForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox _textBox;
    private System.Windows.Forms.Button _btnClose;
  }
}