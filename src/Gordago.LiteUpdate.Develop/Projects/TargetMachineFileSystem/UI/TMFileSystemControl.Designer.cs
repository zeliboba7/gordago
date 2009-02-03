namespace Gordago.LiteUpdate.Develop.Projects {
  partial class TMFileSystemControl {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TMFileSystemControl));
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this._treeViewFileSystem = new Gordago.LiteUpdate.Develop.Projects.TMTreeView();
      this._cxtmTMTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._mniAddSpecialFolder = new System.Windows.Forms.ToolStripMenuItem();
      this._imageList = new System.Windows.Forms.ImageList(this.components);
      this._lstDetails = new System.Windows.Forms.ListView();
      this._colName = new System.Windows.Forms.ColumnHeader();
      this._colType = new System.Windows.Forms.ColumnHeader();
      this._colVersion = new System.Windows.Forms.ColumnHeader();
      this._colModify = new System.Windows.Forms.ColumnHeader();
      this._cxtmFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._mniFileDelete = new System.Windows.Forms.ToolStripMenuItem();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this._cxtmTMTreeView.SuspendLayout();
      this._cxtmFiles.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this._treeViewFileSystem);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this._lstDetails);
      this.splitContainer1.Size = new System.Drawing.Size(503, 271);
      this.splitContainer1.SplitterDistance = 167;
      this.splitContainer1.TabIndex = 0;
      // 
      // _treeViewFileSystem
      // 
      this._treeViewFileSystem.ContextMenuStrip = this._cxtmTMTreeView;
      this._treeViewFileSystem.Dock = System.Windows.Forms.DockStyle.Fill;
      this._treeViewFileSystem.HideSelection = false;
      this._treeViewFileSystem.ImageIndex = 0;
      this._treeViewFileSystem.ImageList = this._imageList;
      this._treeViewFileSystem.LabelEdit = true;
      this._treeViewFileSystem.Location = new System.Drawing.Point(0, 0);
      this._treeViewFileSystem.Name = "_treeViewFileSystem";
      this._treeViewFileSystem.SelectedImageIndex = 0;
      this._treeViewFileSystem.ShowRootLines = false;
      this._treeViewFileSystem.Size = new System.Drawing.Size(167, 271);
      this._treeViewFileSystem.TabIndex = 0;
      this._treeViewFileSystem.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this._treeViewFileSystem_AfterLabelEdit);
      this._treeViewFileSystem.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this._treeViewFileSystem_BeforeSelect);
      // 
      // _cxtmTMTreeView
      // 
      this._cxtmTMTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniAddSpecialFolder});
      this._cxtmTMTreeView.Name = "_cxtmTMTreeView";
      this._cxtmTMTreeView.Size = new System.Drawing.Size(174, 26);
      this._cxtmTMTreeView.Opening += new System.ComponentModel.CancelEventHandler(this._cxtmTMTreeView_Opening);
      // 
      // _mniAddSpecialFolder
      // 
      this._mniAddSpecialFolder.Name = "_mniAddSpecialFolder";
      this._mniAddSpecialFolder.Size = new System.Drawing.Size(173, 22);
      this._mniAddSpecialFolder.Text = "Add Special Folder";
      // 
      // _imageList
      // 
      this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
      this._imageList.TransparentColor = System.Drawing.Color.Transparent;
      this._imageList.Images.SetKeyName(0, "TMFile.png");
      this._imageList.Images.SetKeyName(1, "Computer.png");
      this._imageList.Images.SetKeyName(2, "FolderClose.png");
      this._imageList.Images.SetKeyName(3, "FolderOpen.png");
      this._imageList.Images.SetKeyName(4, "Folder.png");
      this._imageList.Images.SetKeyName(5, "TMFileError.png");
      this._imageList.Images.SetKeyName(6, "TMFolderCloseError.png");
      this._imageList.Images.SetKeyName(7, "TMFolderOpenError.png");
      // 
      // _lstDetails
      // 
      this._lstDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colName,
            this._colType,
            this._colVersion,
            this._colModify});
      this._lstDetails.ContextMenuStrip = this._cxtmFiles;
      this._lstDetails.Dock = System.Windows.Forms.DockStyle.Fill;
      this._lstDetails.FullRowSelect = true;
      this._lstDetails.HideSelection = false;
      this._lstDetails.Location = new System.Drawing.Point(0, 0);
      this._lstDetails.MultiSelect = false;
      this._lstDetails.Name = "_lstDetails";
      this._lstDetails.Size = new System.Drawing.Size(332, 271);
      this._lstDetails.SmallImageList = this._imageList;
      this._lstDetails.TabIndex = 0;
      this._lstDetails.UseCompatibleStateImageBehavior = false;
      this._lstDetails.View = System.Windows.Forms.View.Details;
      this._lstDetails.DoubleClick += new System.EventHandler(this._lstDetails_DoubleClick);
      // 
      // _colName
      // 
      this._colName.Text = "Name";
      this._colName.Width = 158;
      // 
      // _colType
      // 
      this._colType.Text = "Type";
      this._colType.Width = 71;
      // 
      // _colVersion
      // 
      this._colVersion.Text = "Version";
      // 
      // _colModify
      // 
      this._colModify.Text = "Modify";
      this._colModify.Width = 120;
      // 
      // _cxtmFiles
      // 
      this._cxtmFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mniFileDelete});
      this._cxtmFiles.Name = "_cxtmFiles";
      this._cxtmFiles.Size = new System.Drawing.Size(117, 26);
      this._cxtmFiles.Opening += new System.ComponentModel.CancelEventHandler(this._cxtmFiles_Opening);
      // 
      // _mniFileDelete
      // 
      this._mniFileDelete.Image = global::Gordago.LiteUpdate.Develop.Properties.Resources.EditDelete;
      this._mniFileDelete.Name = "_mniFileDelete";
      this._mniFileDelete.Size = new System.Drawing.Size(116, 22);
      this._mniFileDelete.Text = "Delete";
      this._mniFileDelete.Click += new System.EventHandler(this._mniFileDelete_Click);
      // 
      // TMFileSystemControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer1);
      this.Name = "TMFileSystemControl";
      this.Size = new System.Drawing.Size(503, 271);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this._cxtmTMTreeView.ResumeLayout(false);
      this._cxtmFiles.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private TMTreeView _treeViewFileSystem;
    private System.Windows.Forms.ListView _lstDetails;
    private System.Windows.Forms.ColumnHeader _colName;
    private System.Windows.Forms.ColumnHeader _colType;
    private System.Windows.Forms.ImageList _imageList;
    private System.Windows.Forms.ContextMenuStrip _cxtmTMTreeView;
    private System.Windows.Forms.ToolStripMenuItem _mniAddSpecialFolder;
    private System.Windows.Forms.ColumnHeader _colVersion;
    private System.Windows.Forms.ColumnHeader _colModify;
    private System.Windows.Forms.ContextMenuStrip _cxtmFiles;
    private System.Windows.Forms.ToolStripMenuItem _mniFileDelete;
  }
}
