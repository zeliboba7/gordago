namespace Gordago.LiteUpdate.Develop.Projects {
  partial class ProjectWindow {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectWindow));
      this._treeView = new System.Windows.Forms.TreeView();
      this._imageList = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // _treeView
      // 
      this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this._treeView.ImageIndex = 0;
      this._treeView.ImageList = this._imageList;
      this._treeView.Location = new System.Drawing.Point(0, 0);
      this._treeView.Name = "_treeView";
      this._treeView.SelectedImageIndex = 0;
      this._treeView.Size = new System.Drawing.Size(291, 245);
      this._treeView.TabIndex = 0;
      this._treeView.DoubleClick += new System.EventHandler(this._treeView_DoubleClick);
      // 
      // _imageList
      // 
      this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
      this._imageList.TransparentColor = System.Drawing.Color.Transparent;
      this._imageList.Images.SetKeyName(0, "ProjectExplorer.png");
      this._imageList.Images.SetKeyName(1, "ProjectFileSystem.png");
      this._imageList.Images.SetKeyName(2, "ProjectVersion.png");
      this._imageList.Images.SetKeyName(3, "TextFile.png");
      this._imageList.Images.SetKeyName(4, "ProjectProperties.png");
      // 
      // ProjectWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(291, 245);
      this.Controls.Add(this._treeView);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ProjectWindow";
      this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
      this.TabText = "Project";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView _treeView;
    private System.Windows.Forms.ImageList _imageList;
  }
}
