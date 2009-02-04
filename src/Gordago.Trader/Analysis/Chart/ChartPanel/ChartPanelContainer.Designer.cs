namespace Gordago.Analysis.Chart {
  partial class ChartPanelContainer {
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
      this._extBtnProperty = new Gordago.Analysis.Chart.ExtButton();
      this._extBtnHide = new Gordago.Analysis.Chart.ExtButton();
      this._extBtnClose = new Gordago.Analysis.Chart.ExtButton();
      this.SuspendLayout();
      // 
      // _extBtnProperty
      // 
      this._extBtnProperty.Checked = true;
      this._extBtnProperty.ExtButtonType = Gordago.Analysis.Chart.ExtButtonType.Property;
      this._extBtnProperty.Location = new System.Drawing.Point(3, 3);
      this._extBtnProperty.Name = "_extBtnProperty";
      this._extBtnProperty.Size = new System.Drawing.Size(12, 12);
      this._extBtnProperty.TabIndex = 2;
      this._extBtnProperty.Click += new System.EventHandler(this._extBtnProperty_Click);
      this._extBtnProperty.CheckedChanged += new System.EventHandler(this._extBtnHide_CheckedChanged);
      // 
      // _extBtnHide
      // 
      this._extBtnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._extBtnHide.Checked = true;
      this._extBtnHide.ExtButtonType = Gordago.Analysis.Chart.ExtButtonType.Hide;
      this._extBtnHide.Location = new System.Drawing.Point(170, 3);
      this._extBtnHide.Name = "_extBtnHide";
      this._extBtnHide.Size = new System.Drawing.Size(12, 12);
      this._extBtnHide.TabIndex = 2;
      this._extBtnHide.CheckedChanged += new System.EventHandler(this._extBtnHide_CheckedChanged);
      // 
      // _extBtnClose
      // 
      this._extBtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._extBtnClose.Checked = true;
      this._extBtnClose.ExtButtonType = Gordago.Analysis.Chart.ExtButtonType.Close;
      this._extBtnClose.Location = new System.Drawing.Point(185, 3);
      this._extBtnClose.Name = "_extBtnClose";
      this._extBtnClose.Size = new System.Drawing.Size(12, 12);
      this._extBtnClose.TabIndex = 1;
      this._extBtnClose.Click += new System.EventHandler(this._extBtnClose_Click);
      // 
      // ChartPanelContainer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.Controls.Add(this._extBtnProperty);
      this.Controls.Add(this._extBtnHide);
      this.Controls.Add(this._extBtnClose);
      this.Name = "ChartPanelContainer";
      this.Size = new System.Drawing.Size(202, 238);
      this.ResumeLayout(false);
    }

    #endregion

    private ExtButton _extBtnHide;
    private ExtButton _extBtnClose;
    private ExtButton _extBtnProperty;




  }
}
