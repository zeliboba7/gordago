using Cursit;
namespace Cursit {
  partial class ViewOpenWindows {
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
      this._tbcMain = new TabControlExt();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this._tbcMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // _tbcMain
      // 
      this._tbcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this._tbcMain.Controls.Add(this.tabPage1);
      this._tbcMain.Controls.Add(this.tabPage2);
      this._tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tbcMain.Location = new System.Drawing.Point(0, 0);
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(555, 22);
      this._tbcMain.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.Location = new System.Drawing.Point(4, 4);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(547, 0);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "tabPage1";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Location = new System.Drawing.Point(4, 4);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(547, -4);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "tabPage2";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // ViewOpenWindows
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._tbcMain);
      this.Name = "ViewOpenWindows";
      this.Size = new System.Drawing.Size(555, 22);
      this._tbcMain.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private TabControlExt _tbcMain;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
  }
}
