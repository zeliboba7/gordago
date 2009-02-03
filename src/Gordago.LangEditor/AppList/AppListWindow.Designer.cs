namespace Gordago.LangEditor.AppList {
  partial class AppListWindow {
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
      this.components = new System.ComponentModel.Container();
      this._treeView = new System.Windows.Forms.TreeView();
      this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._mniAddLanguage = new System.Windows.Forms.ToolStripMenuItem();
      this._mniEditLanguage = new System.Windows.Forms.ToolStripMenuItem();
      this._contextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // _treeView
      // 
      this._treeView.ContextMenuStrip = this._contextMenuStrip;
      this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this._treeView.FullRowSelect = true;
      this._treeView.Location = new System.Drawing.Point(0, 0);
      this._treeView.Name = "_treeView";
      this._treeView.Size = new System.Drawing.Size(217, 285);
      this._treeView.TabIndex = 0;
      this._treeView.DoubleClick += new System.EventHandler(this._treeView_DoubleClick);
      // 
      // _contextMenuStrip
      // 
      this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniAddLanguage,
            this._mniEditLanguage});
      this._contextMenuStrip.Name = "_contextMenuStrip";
      this._contextMenuStrip.Size = new System.Drawing.Size(155, 48);
      this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuStrip_Opening);
      // 
      // _mniAddLanguage
      // 
      this._mniAddLanguage.Name = "_mniAddLanguage";
      this._mniAddLanguage.Size = new System.Drawing.Size(154, 22);
      this._mniAddLanguage.Text = "Add Language";
      this._mniAddLanguage.Click += new System.EventHandler(this._mniAddLanguage_Click);
      // 
      // _mniEditLanguage
      // 
      this._mniEditLanguage.Name = "_mniEditLanguage";
      this._mniEditLanguage.Size = new System.Drawing.Size(154, 22);
      this._mniEditLanguage.Text = "Edit";
      this._mniEditLanguage.Click += new System.EventHandler(this._mniEditLanguage_Click);
      // 
      // AppListWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(217, 285);
      this.Controls.Add(this._treeView);
      this.Name = "AppListWindow";
      this.TabText = "Applications";
      this.Tag = "Applications";
      this.Text = "Applications";
      this._contextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView _treeView;
    private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem _mniAddLanguage;
    private System.Windows.Forms.ToolStripMenuItem _mniEditLanguage;

  }
}
