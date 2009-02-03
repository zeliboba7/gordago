/**
* @version $Id: ProjectPropertiesDocument.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;

  public class ProjectPropertiesDocument : ProjectDocument {

    private Label _lblProjectName;
    private TextBox _txtProjectName;
    private Label _lblUpdateDirectory;
    private TextBox _txtApplicationUpdateDirectory;
    private Button _btnBrowseApplicationUpdateDirectory;
    private GroupBox _gboxApplication;
    private TextBox _txtApplicationDirectory;
    private Button _btnBrowseApplicationDirectory;
    private Label _lblRootDirectory;

    private readonly Project _project;

    public ProjectPropertiesDocument(Project project) {
      this.InitializeComponent();
      _project = project;
      this.SetKey(new ProjectPropertiesDocumentKey(project));

      this.Text = this.TabText = Global.Languages["Project/Properties"]["Properties"];
      _lblProjectName.Text = Global.Languages["Project/Properties"]["Project Name"];
      _gboxApplication.Text = Global.Languages["Project/Properties"]["Application"];
      _lblRootDirectory.Text = Global.Languages["Project/Properties"]["Root Directory"];
      _lblUpdateDirectory.Text = Global.Languages["Project/Properties"]["Update Directory"];

      _btnBrowseApplicationDirectory.Text = Global.Languages["Button"]["Browse..."];
      _btnBrowseApplicationUpdateDirectory.Text = Global.Languages["Button"]["Browse..."];
    }

    #region private void InitializeComponent()
    private void InitializeComponent() {
      this._lblProjectName = new System.Windows.Forms.Label();
      this._txtProjectName = new System.Windows.Forms.TextBox();
      this._lblUpdateDirectory = new System.Windows.Forms.Label();
      this._txtApplicationUpdateDirectory = new System.Windows.Forms.TextBox();
      this._btnBrowseApplicationUpdateDirectory = new System.Windows.Forms.Button();
      this._gboxApplication = new System.Windows.Forms.GroupBox();
      this._txtApplicationDirectory = new System.Windows.Forms.TextBox();
      this._btnBrowseApplicationDirectory = new System.Windows.Forms.Button();
      this._lblRootDirectory = new System.Windows.Forms.Label();
      this._gboxApplication.SuspendLayout();
      this.SuspendLayout();
      // 
      // _lblProjectName
      // 
      this._lblProjectName.AutoSize = true;
      this._lblProjectName.Location = new System.Drawing.Point(13, 13);
      this._lblProjectName.Name = "_lblProjectName";
      this._lblProjectName.Size = new System.Drawing.Size(71, 13);
      this._lblProjectName.TabIndex = 0;
      this._lblProjectName.Text = "Project Name";
      // 
      // _txtProjectName
      // 
      this._txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtProjectName.Location = new System.Drawing.Point(90, 10);
      this._txtProjectName.Name = "_txtProjectName";
      this._txtProjectName.ReadOnly = true;
      this._txtProjectName.Size = new System.Drawing.Size(431, 20);
      this._txtProjectName.TabIndex = 1;
      // 
      // _lblUpdateDirectory
      // 
      this._lblUpdateDirectory.AutoSize = true;
      this._lblUpdateDirectory.Location = new System.Drawing.Point(8, 77);
      this._lblUpdateDirectory.Name = "_lblUpdateDirectory";
      this._lblUpdateDirectory.Size = new System.Drawing.Size(87, 13);
      this._lblUpdateDirectory.TabIndex = 0;
      this._lblUpdateDirectory.Text = "Update Directory";
      // 
      // _txtApplicationUpdateDirectory
      // 
      this._txtApplicationUpdateDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtApplicationUpdateDirectory.Location = new System.Drawing.Point(6, 93);
      this._txtApplicationUpdateDirectory.Name = "_txtApplicationUpdateDirectory";
      this._txtApplicationUpdateDirectory.Size = new System.Drawing.Size(431, 20);
      this._txtApplicationUpdateDirectory.TabIndex = 2;
      // 
      // _btnBrowseApplicationUpdateDirectory
      // 
      this._btnBrowseApplicationUpdateDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnBrowseApplicationUpdateDirectory.Location = new System.Drawing.Point(443, 91);
      this._btnBrowseApplicationUpdateDirectory.Name = "_btnBrowseApplicationUpdateDirectory";
      this._btnBrowseApplicationUpdateDirectory.Size = new System.Drawing.Size(75, 23);
      this._btnBrowseApplicationUpdateDirectory.TabIndex = 2;
      this._btnBrowseApplicationUpdateDirectory.Text = "Browse...";
      this._btnBrowseApplicationUpdateDirectory.UseVisualStyleBackColor = true;
      this._btnBrowseApplicationUpdateDirectory.Click += new System.EventHandler(this._btnBrowseApplicationUpdateDirectory_Click);
      // 
      // _gboxApplication
      // 
      this._gboxApplication.Controls.Add(this._txtApplicationDirectory);
      this._gboxApplication.Controls.Add(this._btnBrowseApplicationDirectory);
      this._gboxApplication.Controls.Add(this._txtApplicationUpdateDirectory);
      this._gboxApplication.Controls.Add(this._lblRootDirectory);
      this._gboxApplication.Controls.Add(this._btnBrowseApplicationUpdateDirectory);
      this._gboxApplication.Controls.Add(this._lblUpdateDirectory);
      this._gboxApplication.Location = new System.Drawing.Point(5, 48);
      this._gboxApplication.Name = "_gboxApplication";
      this._gboxApplication.Size = new System.Drawing.Size(524, 128);
      this._gboxApplication.TabIndex = 3;
      this._gboxApplication.TabStop = false;
      this._gboxApplication.Text = "Application";
      // 
      // _txtApplicationDirectory
      // 
      this._txtApplicationDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._txtApplicationDirectory.Location = new System.Drawing.Point(6, 40);
      this._txtApplicationDirectory.Name = "_txtApplicationDirectory";
      this._txtApplicationDirectory.Size = new System.Drawing.Size(431, 20);
      this._txtApplicationDirectory.TabIndex = 1;
      // 
      // _btnBrowseApplicationDirectory
      // 
      this._btnBrowseApplicationDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._btnBrowseApplicationDirectory.Location = new System.Drawing.Point(443, 38);
      this._btnBrowseApplicationDirectory.Name = "_btnBrowseApplicationDirectory";
      this._btnBrowseApplicationDirectory.Size = new System.Drawing.Size(75, 23);
      this._btnBrowseApplicationDirectory.TabIndex = 2;
      this._btnBrowseApplicationDirectory.Text = "Browse...";
      this._btnBrowseApplicationDirectory.UseVisualStyleBackColor = true;
      this._btnBrowseApplicationDirectory.Click += new System.EventHandler(this.button1_Click);
      // 
      // _lblRootDirectory
      // 
      this._lblRootDirectory.AutoSize = true;
      this._lblRootDirectory.Location = new System.Drawing.Point(7, 24);
      this._lblRootDirectory.Name = "_lblRootDirectory";
      this._lblRootDirectory.Size = new System.Drawing.Size(75, 13);
      this._lblRootDirectory.TabIndex = 0;
      this._lblRootDirectory.Text = "Root Directory";
      // 
      // ProjectPropertiesDocument
      // 
      this.ClientSize = new System.Drawing.Size(533, 316);
      this.Controls.Add(this._gboxApplication);
      this.Controls.Add(this._txtProjectName);
      this.Controls.Add(this._lblProjectName);
      this.Name = "ProjectPropertiesDocument";
      this.TabText = "Properties";
      this.Text = "Properties";
      this._gboxApplication.ResumeLayout(false);
      this._gboxApplication.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    #region private void _btnBrowseApplicationUpdateDirectory_Click(object sender, EventArgs e)
    private void _btnBrowseApplicationUpdateDirectory_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtApplicationUpdateDirectory.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtApplicationUpdateDirectory.Text = fbd.SelectedPath;
      _project.AppUpdateDirectory = new DirectoryInfo(fbd.SelectedPath);
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      this.UpdateProperties();
    }
    #endregion

    #region private void button1_Click(object sender, EventArgs e)
    private void button1_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtApplicationDirectory.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtApplicationDirectory.Text = fbd.SelectedPath;
      _project.AppUpdateDirectory = new DirectoryInfo(fbd.SelectedPath);
    }
    #endregion

    #region private void UpdateProperties()
    private void UpdateProperties() {
      _txtProjectName.Text = _project.Name;
      _txtApplicationUpdateDirectory.Text = _project.AppUpdateDirectory.FullName;
      _txtApplicationDirectory.Text = _project.AppDirectory.FullName;
    }
    #endregion
  }
}
