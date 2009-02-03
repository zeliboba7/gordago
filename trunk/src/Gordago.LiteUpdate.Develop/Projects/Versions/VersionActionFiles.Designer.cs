namespace Gordago.LiteUpdate.Develop.Projects {
  partial class VersionActionFiles {
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
      this._lstFiles = new ActionFilesListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // _lstFiles
      // 
      this._lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this._lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this._lstFiles.Location = new System.Drawing.Point(3, 16);
      this._lstFiles.Name = "_lstFiles";
      this._lstFiles.Size = new System.Drawing.Size(364, 224);
      this._lstFiles.TabIndex = 0;
      this._lstFiles.UseCompatibleStateImageBehavior = false;
      this._lstFiles.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "File";
      this.columnHeader1.Width = 500;
      // 
      // VersionActionFiles
      // 
      this.Controls.Add(this._lstFiles);
      this.Size = new System.Drawing.Size(370, 243);
      this.ResumeLayout(false);

    }

    #endregion

    private ActionFilesListView _lstFiles;
    private System.Windows.Forms.ColumnHeader columnHeader1;
  }
}
