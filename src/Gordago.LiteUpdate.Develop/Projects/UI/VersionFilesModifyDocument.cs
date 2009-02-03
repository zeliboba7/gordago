/**
* @version $Id: VersionFilesModifyDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Text;
  using System.Windows.Forms;
  using Gordago.LiteUpdate.Develop.Docking;
  using Gordago.Docking;

  public partial class VersionFilesModifyDocument : ProjectDocument {

    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private VersionActionFiles _filesRemoveControl;
    private VersionActionFiles _filesAddControl;
    private VersionActionFiles _filesUpdateControl;

    #region public class DocumentKey : AbstractDocumentKey
    public class DocumentKey : AbstractDocumentKey {
      private readonly int _hashCode;
      
      public DocumentKey(VersionInfo version) {
        _hashCode = ((string)(version.Number.ToString() + "FileModify")).GetHashCode();
      }

      public override int GetHashCode() {
        return _hashCode;
      }

      public override bool Equals(object obj) {
        if (!(obj is DocumentKey))
          return false;
        return (obj as DocumentKey)._hashCode == _hashCode;
      }
    }
    #endregion

    private readonly VersionInfo _version;

    public VersionFilesModifyDocument(VersionInfo version) {
      InitializeComponent();
      _version = version;

      _filesAddControl.SetFiles(version.Modify.FilesAdded);
      _filesRemoveControl.SetFiles(version.Modify.FilesRemoved);
      _filesUpdateControl.SetFiles(version.Modify.FilesUpdated);
      this.SetKey(new DocumentKey(version));

      this.Text = this.TabText = string.Format("Version {0}: Files Modify", version.Number);
    }

    #region protected override void Dispose(bool disposing)
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
    #endregion

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
      this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 395);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // _filesRemoveControl
      // 
      this._filesRemoveControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._filesRemoveControl.Location = new System.Drawing.Point(3, 265);
      this._filesRemoveControl.Name = "_filesRemoveControl";
      this._filesRemoveControl.Size = new System.Drawing.Size(574, 127);
      this._filesRemoveControl.TabIndex = 5;
      this._filesRemoveControl.TabStop = false;
      this._filesRemoveControl.Text = "Remove";
      // 
      // _filesAddControl
      // 
      this._filesAddControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._filesAddControl.Location = new System.Drawing.Point(3, 134);
      this._filesAddControl.Name = "_filesAddControl";
      this._filesAddControl.Size = new System.Drawing.Size(574, 125);
      this._filesAddControl.TabIndex = 4;
      this._filesAddControl.TabStop = false;
      this._filesAddControl.Text = "Add";
      // 
      // _filesUpdateControl
      // 
      this._filesUpdateControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._filesUpdateControl.Location = new System.Drawing.Point(3, 3);
      this._filesUpdateControl.Name = "_filesUpdateControl";
      this._filesUpdateControl.Size = new System.Drawing.Size(574, 125);
      this._filesUpdateControl.TabIndex = 3;
      this._filesUpdateControl.TabStop = false;
      this._filesUpdateControl.Text = "Update";
      // 
      // VersionFilesModifyDocument
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(580, 395);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "VersionFilesModifyDocument";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

  }
}
