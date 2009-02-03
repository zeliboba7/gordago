namespace Gordago.LiteUpdate.Develop.Projects {
  partial class ErrorListWindow {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorListWindow));
      this._lstErrors = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader("(none)");
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this._imageList = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // _lstErrors
      // 
      this._lstErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this._lstErrors.Dock = System.Windows.Forms.DockStyle.Fill;
      this._lstErrors.FullRowSelect = true;
      this._lstErrors.Location = new System.Drawing.Point(0, 0);
      this._lstErrors.MultiSelect = false;
      this._lstErrors.Name = "_lstErrors";
      this._lstErrors.Size = new System.Drawing.Size(548, 133);
      this._lstErrors.SmallImageList = this._imageList;
      this._lstErrors.TabIndex = 0;
      this._lstErrors.UseCompatibleStateImageBehavior = false;
      this._lstErrors.View = System.Windows.Forms.View.Details;
      this._lstErrors.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._lstErrors_MouseDoubleClick);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "NN";
      this.columnHeader1.Width = 40;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Description";
      this.columnHeader2.Width = 500;
      // 
      // _imageList
      // 
      this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
      this._imageList.TransparentColor = System.Drawing.Color.Transparent;
      this._imageList.Images.SetKeyName(0, "Error.png");
      // 
      // ErrorListWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(548, 133);
      this.Controls.Add(this._lstErrors);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ErrorListWindow";
      this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
      this.TabText = "Error List";
      this.Text = "Error List";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView _lstErrors;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ImageList _imageList;
  }
}
