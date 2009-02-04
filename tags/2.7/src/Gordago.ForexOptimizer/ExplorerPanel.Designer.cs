namespace Gordago {
  partial class ExplorerPanel {
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
      this.components = new System.ComponentModel.Container();
      this._splitContainerMain = new System.Windows.Forms.SplitContainer();
      this._trvExplorer = new System.Windows.Forms.TreeView();
      this._lblExplorer = new System.Windows.Forms.Label();
      this._splitContrainerIndicators = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this._trvIndicators = new System.Windows.Forms.TreeView();
      this._imageListIndicator = new System.Windows.Forms.ImageList(this.components);
      this._lblIndicators = new System.Windows.Forms.Label();
      this._pgIndic = new Cursit.Applic.APropGrid.PropGrid();
      this._pnlInsertIndicators = new System.Windows.Forms.Panel();
      this._lblInsertIndicator = new System.Windows.Forms.Label();
      this._pngPGChartObject = new System.Windows.Forms.Panel();
      this._splitPropChartObject = new System.Windows.Forms.SplitContainer();
      this._treeViewChartObject = new System.Windows.Forms.TreeView();
      this._pgChartObject = new System.Windows.Forms.PropertyGrid();
      this._imageList = new System.Windows.Forms.ImageList(this.components);
      this._lstIndicatorInsert = new Gordago.LstObject.ListObject();
      this._splitContainerMain.Panel1.SuspendLayout();
      this._splitContainerMain.Panel2.SuspendLayout();
      this._splitContainerMain.SuspendLayout();
      this._splitContrainerIndicators.Panel1.SuspendLayout();
      this._splitContrainerIndicators.Panel2.SuspendLayout();
      this._splitContrainerIndicators.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this._pnlInsertIndicators.SuspendLayout();
      this._pngPGChartObject.SuspendLayout();
      this._splitPropChartObject.Panel1.SuspendLayout();
      this._splitPropChartObject.Panel2.SuspendLayout();
      this._splitPropChartObject.SuspendLayout();
      this.SuspendLayout();
      // 
      // _splitContainerMain
      // 
      this._splitContainerMain.BackColor = System.Drawing.Color.White;
      this._splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this._splitContainerMain.Location = new System.Drawing.Point(0, 0);
      this._splitContainerMain.Margin = new System.Windows.Forms.Padding(0);
      this._splitContainerMain.Name = "_splitContainerMain";
      this._splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitContainerMain.Panel1
      // 
      this._splitContainerMain.Panel1.Controls.Add(this._trvExplorer);
      this._splitContainerMain.Panel1.Controls.Add(this._lblExplorer);
      this._splitContainerMain.Panel1Collapsed = true;
      // 
      // _splitContainerMain.Panel2
      // 
      this._splitContainerMain.Panel2.Controls.Add(this._splitContrainerIndicators);
      this._splitContainerMain.Size = new System.Drawing.Size(195, 504);
      this._splitContainerMain.SplitterDistance = 153;
      this._splitContainerMain.TabIndex = 0;
      // 
      // _trvExplorer
      // 
      this._trvExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._trvExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this._trvExplorer.Location = new System.Drawing.Point(0, 18);
      this._trvExplorer.Name = "_trvExplorer";
      this._trvExplorer.Size = new System.Drawing.Size(195, 135);
      this._trvExplorer.TabIndex = 0;
      // 
      // _lblExplorer
      // 
      this._lblExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._lblExplorer.Location = new System.Drawing.Point(0, 0);
      this._lblExplorer.Name = "_lblExplorer";
      this._lblExplorer.Size = new System.Drawing.Size(195, 19);
      this._lblExplorer.TabIndex = 1;
      this._lblExplorer.Text = "Explorer";
      this._lblExplorer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _splitContrainerIndicators
      // 
      this._splitContrainerIndicators.Dock = System.Windows.Forms.DockStyle.Fill;
      this._splitContrainerIndicators.Location = new System.Drawing.Point(0, 0);
      this._splitContrainerIndicators.Name = "_splitContrainerIndicators";
      this._splitContrainerIndicators.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitContrainerIndicators.Panel1
      // 
      this._splitContrainerIndicators.Panel1.Controls.Add(this.splitContainer2);
      // 
      // _splitContrainerIndicators.Panel2
      // 
      this._splitContrainerIndicators.Panel2.Controls.Add(this._pnlInsertIndicators);
      this._splitContrainerIndicators.Size = new System.Drawing.Size(195, 504);
      this._splitContrainerIndicators.SplitterDistance = 431;
      this._splitContrainerIndicators.TabIndex = 0;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this._trvIndicators);
      this.splitContainer2.Panel1.Controls.Add(this._lblIndicators);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this._pgIndic);
      this.splitContainer2.Size = new System.Drawing.Size(195, 431);
      this.splitContainer2.SplitterDistance = 270;
      this.splitContainer2.TabIndex = 0;
      // 
      // _trvIndicators
      // 
      this._trvIndicators.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this._trvIndicators.Dock = System.Windows.Forms.DockStyle.Fill;
      this._trvIndicators.ImageIndex = 0;
      this._trvIndicators.ImageList = this._imageListIndicator;
      this._trvIndicators.Location = new System.Drawing.Point(0, 0);
      this._trvIndicators.Name = "_trvIndicators";
      this._trvIndicators.SelectedImageIndex = 0;
      this._trvIndicators.Size = new System.Drawing.Size(195, 270);
      this._trvIndicators.TabIndex = 0;
      this._trvIndicators.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._trvIndicators_AfterSelect);
      this._trvIndicators.Click += new System.EventHandler(this._trvIndicators_Click);
      // 
      // _imageListIndicator
      // 
      this._imageListIndicator.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this._imageListIndicator.ImageSize = new System.Drawing.Size(16, 16);
      this._imageListIndicator.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // _lblIndicators
      // 
      this._lblIndicators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblIndicators.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._lblIndicators.Location = new System.Drawing.Point(0, 0);
      this._lblIndicators.Name = "_lblIndicators";
      this._lblIndicators.Size = new System.Drawing.Size(195, 19);
      this._lblIndicators.TabIndex = 1;
      this._lblIndicators.Text = "Indicators";
      this._lblIndicators.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _pgIndic
      // 
      this._pgIndic.AutoScroll = true;
      this._pgIndic.BackColor = System.Drawing.Color.White;
      this._pgIndic.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pgIndic.Location = new System.Drawing.Point(0, 0);
      this._pgIndic.Name = "_pgIndic";
      this._pgIndic.Size = new System.Drawing.Size(195, 157);
      this._pgIndic.TabIndex = 0;
      // 
      // _pnlInsertIndicators
      // 
      this._pnlInsertIndicators.Controls.Add(this._lstIndicatorInsert);
      this._pnlInsertIndicators.Controls.Add(this._lblInsertIndicator);
      this._pnlInsertIndicators.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pnlInsertIndicators.Location = new System.Drawing.Point(0, 0);
      this._pnlInsertIndicators.Name = "_pnlInsertIndicators";
      this._pnlInsertIndicators.Size = new System.Drawing.Size(195, 69);
      this._pnlInsertIndicators.TabIndex = 2;
      // 
      // _lblInsertIndicator
      // 
      this._lblInsertIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lblInsertIndicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(236)))), ((int)(((byte)(242)))));
      this._lblInsertIndicator.Location = new System.Drawing.Point(-1, -1);
      this._lblInsertIndicator.Name = "_lblInsertIndicator";
      this._lblInsertIndicator.Size = new System.Drawing.Size(195, 19);
      this._lblInsertIndicator.TabIndex = 1;
      this._lblInsertIndicator.Text = "Insert";
      this._lblInsertIndicator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _pngPGChartObject
      // 
      this._pngPGChartObject.Controls.Add(this._splitPropChartObject);
      this._pngPGChartObject.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pngPGChartObject.Location = new System.Drawing.Point(0, 0);
      this._pngPGChartObject.Name = "_pngPGChartObject";
      this._pngPGChartObject.Size = new System.Drawing.Size(195, 504);
      this._pngPGChartObject.TabIndex = 2;
      // 
      // _splitPropChartObject
      // 
      this._splitPropChartObject.Dock = System.Windows.Forms.DockStyle.Fill;
      this._splitPropChartObject.Location = new System.Drawing.Point(0, 0);
      this._splitPropChartObject.Name = "_splitPropChartObject";
      this._splitPropChartObject.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitPropChartObject.Panel1
      // 
      this._splitPropChartObject.Panel1.Controls.Add(this._treeViewChartObject);
      // 
      // _splitPropChartObject.Panel2
      // 
      this._splitPropChartObject.Panel2.Controls.Add(this._pgChartObject);
      this._splitPropChartObject.Size = new System.Drawing.Size(195, 504);
      this._splitPropChartObject.SplitterDistance = 203;
      this._splitPropChartObject.TabIndex = 2;
      // 
      // _treeViewChartObject
      // 
      this._treeViewChartObject.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this._treeViewChartObject.Dock = System.Windows.Forms.DockStyle.Fill;
      this._treeViewChartObject.ImageIndex = 0;
      this._treeViewChartObject.ImageList = this._imageListIndicator;
      this._treeViewChartObject.LineColor = System.Drawing.Color.DimGray;
      this._treeViewChartObject.Location = new System.Drawing.Point(0, 0);
      this._treeViewChartObject.Margin = new System.Windows.Forms.Padding(0);
      this._treeViewChartObject.Name = "_treeViewChartObject";
      this._treeViewChartObject.SelectedImageIndex = 0;
      this._treeViewChartObject.Size = new System.Drawing.Size(195, 203);
      this._treeViewChartObject.TabIndex = 0;
      this._treeViewChartObject.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeViewChartObject_AfterSelect);
      // 
      // _pgChartObject
      // 
      this._pgChartObject.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pgChartObject.HelpVisible = false;
      this._pgChartObject.Location = new System.Drawing.Point(0, 0);
      this._pgChartObject.Name = "_pgChartObject";
      this._pgChartObject.Size = new System.Drawing.Size(195, 297);
      this._pgChartObject.TabIndex = 1;
      this._pgChartObject.ToolbarVisible = false;
      this._pgChartObject.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this._pgChartObject_PropertyValueChanged);
      // 
      // _imageList
      // 
      this._imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this._imageList.ImageSize = new System.Drawing.Size(16, 16);
      this._imageList.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // _lstIndicatorInsert
      // 
      this._lstIndicatorInsert.AllowDrop = true;
      this._lstIndicatorInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._lstIndicatorInsert.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this._lstIndicatorInsert.FullRowSelect = true;
      this._lstIndicatorInsert.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this._lstIndicatorInsert.HeaderText = "GSO name";
      this._lstIndicatorInsert.Location = new System.Drawing.Point(0, 17);
      this._lstIndicatorInsert.MultiSelect = false;
      this._lstIndicatorInsert.Name = "_lstIndicatorInsert";
      this._lstIndicatorInsert.Size = new System.Drawing.Size(195, 52);
      this._lstIndicatorInsert.TabIndex = 2;
      this._lstIndicatorInsert.UseCompatibleStateImageBehavior = false;
      this._lstIndicatorInsert.View = System.Windows.Forms.View.Details;
      // 
      // ExplorerPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.Controls.Add(this._pngPGChartObject);
      this.Controls.Add(this._splitContainerMain);
      this.Name = "ExplorerPanel";
      this.Size = new System.Drawing.Size(195, 504);
      this._splitContainerMain.Panel1.ResumeLayout(false);
      this._splitContainerMain.Panel2.ResumeLayout(false);
      this._splitContainerMain.ResumeLayout(false);
      this._splitContrainerIndicators.Panel1.ResumeLayout(false);
      this._splitContrainerIndicators.Panel2.ResumeLayout(false);
      this._splitContrainerIndicators.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.ResumeLayout(false);
      this._pnlInsertIndicators.ResumeLayout(false);
      this._pngPGChartObject.ResumeLayout(false);
      this._splitPropChartObject.Panel1.ResumeLayout(false);
      this._splitPropChartObject.Panel2.ResumeLayout(false);
      this._splitPropChartObject.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer _splitContainerMain;
    private System.Windows.Forms.TreeView _trvExplorer;
    private System.Windows.Forms.SplitContainer _splitContrainerIndicators;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.TreeView _trvIndicators;
    private Cursit.Applic.APropGrid.PropGrid _pgIndic;
    private System.Windows.Forms.Panel _pnlInsertIndicators;
    private System.Windows.Forms.Label _lblInsertIndicator;
    private System.Windows.Forms.ImageList _imageList;
    private System.Windows.Forms.ImageList _imageListIndicator;
    private Gordago.LstObject.ListObject _lstIndicatorInsert;
    private System.Windows.Forms.Label _lblIndicators;
    private System.Windows.Forms.Label _lblExplorer;
    private System.Windows.Forms.PropertyGrid _pgChartObject;
    private System.Windows.Forms.Panel _pngPGChartObject;
    private System.Windows.Forms.SplitContainer _splitPropChartObject;
    private System.Windows.Forms.TreeView _treeViewChartObject;
  }
}
