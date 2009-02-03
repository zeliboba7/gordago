using Gordago.Core;
namespace Gordago.LiteUpdate.Develop {
  partial class OutputWindow {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindow));
      this._outputTextBox = new Gordago.Core.OutputTextBox();
      this.SuspendLayout();
      // 
      // _outputTextBox
      // 
      this._outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this._outputTextBox.Location = new System.Drawing.Point(0, 0);
      this._outputTextBox.Name = "_outputTextBox";
      this._outputTextBox.Size = new System.Drawing.Size(414, 202);
      this._outputTextBox.TabIndex = 0;
      this._outputTextBox.Text = "";
      // 
      // OutputWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(414, 202);
      this.Controls.Add(this._outputTextBox);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "OutputWindow";
      this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
      this.TabText = "Output";
      this.Text = "Output";
      this.ResumeLayout(false);

    }
    #endregion

    private OutputTextBox _outputTextBox;
  }
}
