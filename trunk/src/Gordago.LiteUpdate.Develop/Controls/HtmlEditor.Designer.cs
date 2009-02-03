namespace Gordago.LiteUpdate.Develop.Controls {
  partial class HtmlEditor {
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
      this._tbcMain = new Gordago.LiteUpdate.Develop.Controls.TabControlExt();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this._codeEditor = new System.Windows.Forms.RichTextBox();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this._webBrowser = new System.Windows.Forms.WebBrowser();
      this._tbcMain.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _tbcMain
      // 
      this._tbcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this._tbcMain.BackColorTabPages = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
      this._tbcMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
      this._tbcMain.BorderVisible = false;
      this._tbcMain.Controls.Add(this.tabPage2);
      this._tbcMain.Controls.Add(this.tabPage1);
      this._tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tbcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this._tbcMain.Location = new System.Drawing.Point(0, 0);
      this._tbcMain.Name = "_tbcMain";
      this._tbcMain.SelectedIndex = 0;
      this._tbcMain.Size = new System.Drawing.Size(530, 434);
      this._tbcMain.TabIndex = 0;
      this._tbcMain.SelectedIndexChanged += new System.EventHandler(this._tbcMain_SelectedIndexChanged);
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this._codeEditor);
      this.tabPage2.Location = new System.Drawing.Point(4, 4);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(522, 408);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Code";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // _codeEditor
      // 
      this._codeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this._codeEditor.Location = new System.Drawing.Point(3, 3);
      this._codeEditor.Name = "_codeEditor";
      this._codeEditor.Size = new System.Drawing.Size(516, 402);
      this._codeEditor.TabIndex = 0;
      this._codeEditor.Text = "";
      this._codeEditor.TextChanged += new System.EventHandler(this._codeEditor_TextChanged);
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this._webBrowser);
      this.tabPage1.Location = new System.Drawing.Point(4, 4);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(522, 408);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Preview";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // _webBrowser
      // 
      this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this._webBrowser.Location = new System.Drawing.Point(3, 3);
      this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
      this._webBrowser.Name = "_webBrowser";
      this._webBrowser.Size = new System.Drawing.Size(516, 402);
      this._webBrowser.TabIndex = 0;
      // 
      // HtmlEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._tbcMain);
      this.Name = "HtmlEditor";
      this.Size = new System.Drawing.Size(530, 434);
      this._tbcMain.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private TabControlExt _tbcMain;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.WebBrowser _webBrowser;
    private System.Windows.Forms.RichTextBox _codeEditor;
  }
}
