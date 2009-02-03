namespace Gordago.LiteUpdate.Develop.Projects.Versions {
  partial class VersionFilesModifyControl {
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
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this._filesRemoveControl = new Gordago.LiteUpdate.Develop.Projects.VersionActionFiles();
      this._filesAddControl = new Gordago.LiteUpdate.Develop.Projects.VersionActionFiles();
      this._filesUpdateControl = new Gordago.LiteUpdate.Develop.Projects.VersionActionFiles();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this._filesRemoveControl, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this._filesAddControl, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this._filesUpdateControl, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(602, 435);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // _filesRemoveControl
      // 
      this._filesRemoveControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._filesRemoveControl.Location = new System.Drawing.Point(3, 293);
      this._filesRemoveControl.Name = "_filesRemoveControl";
      this._filesRemoveControl.Size = new System.Drawing.Size(596, 139);
      this._filesRemoveControl.TabIndex = 5;
      this._filesRemoveControl.TabStop = false;
      this._filesRemoveControl.Text = "Remove";
      // 
      // _filesAddControl
      // 
      this._filesAddControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._filesAddControl.Location = new System.Drawing.Point(3, 148);
      this._filesAddControl.Name = "_filesAddControl";
      this._filesAddControl.Size = new System.Drawing.Size(596, 139);
      this._filesAddControl.TabIndex = 4;
      this._filesAddControl.TabStop = false;
      this._filesAddControl.Text = "Add";
      // 
      // _filesUpdateControl
      // 
      this._filesUpdateControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._filesUpdateControl.Location = new System.Drawing.Point(3, 3);
      this._filesUpdateControl.Name = "_filesUpdateControl";
      this._filesUpdateControl.Size = new System.Drawing.Size(596, 139);
      this._filesUpdateControl.TabIndex = 3;
      this._filesUpdateControl.TabStop = false;
      this._filesUpdateControl.Text = "Update";
      // 
      // VersionFilesModifyControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "VersionFilesModifyControl";
      this.Size = new System.Drawing.Size(602, 435);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private VersionActionFiles _filesRemoveControl;
    private VersionActionFiles _filesAddControl;
    private VersionActionFiles _filesUpdateControl;
  }
}
