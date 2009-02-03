/**
* @version $Id: WizardForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects.Wizard
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;

  public partial class WizardForm : Form {

    private Project _project = null;

    public WizardForm() {
      InitializeComponent();
      string projectDir = Global.Setup.ProjectsDirectory.FullName;
      _txtProjectLocation.Text = projectDir;
      _txtAppLocation.Text = new DirectoryInfo(Path.Combine(projectDir, "..")).FullName;

      DirectoryInfo[] directories = (new DirectoryInfo(projectDir)).GetDirectories();
      string projectName = "LUProject";

      for (int i = 1; i < 1000; i++) {
        bool find = false;
        string tempProjectName = projectName + i.ToString();

        foreach (DirectoryInfo dir in directories) {
          if (dir.Name.ToUpper() == tempProjectName.ToUpper()) {
            find = true;
            break;
          }
        }
        if (!find) {
          projectName = tempProjectName;
          break;
        }
      }
      _txtProjectName.Text = projectName;

      this.Text = Global.Languages["NewProject"]["New Project"];
      _lblName.Text = Global.Languages["NewProject"]["Name:"];
      _lblLocation.Text = Global.Languages["NewProject"]["Location:"];
      _lblApplication.Text = Global.Languages["NewProject"]["Application:"];
      _btnBrowseAppFolder.Text = Global.Languages["Button"]["Browse..."];
      _btnBrowseProjLocation.Text = Global.Languages["Button"]["Browse..."];
      _btnOK.Text = Global.Languages["Button"]["OK"];
      _btnCancel.Text = Global.Languages["Button"]["Cancel"];
    }

    #region public Project Project
    public Project Project {
      get { return _project; }
    }
    #endregion

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {

      if (_txtProjectName.Text == "") 
        MessageBox.Show("Can not create Project:\nPlease, enter valid project name.", "LiteUpdate Develop");

      DirectoryInfo appDir = null;
      try {
        appDir = new DirectoryInfo(_txtAppLocation.Text);
      } catch {
        MessageBox.Show("Can not create Project:\nPlease, enter valid application folder.", "LiteUpdate Develop");
        return;
      }
      if (!appDir.Exists) {
        MessageBox.Show("Can not create Project:\nApplication directory not found", "LiteUpdate Develop");
        return;
      }

      DirectoryInfo projectFolder = new DirectoryInfo(_txtProjectLocation.Text);
      try {
        projectFolder.Create();
      } catch {
        MessageBox.Show("Can not create Project:\nProject directory of location creating error.");
        return;
      }
      
      string projectName = _txtProjectName.Text ;

      DirectoryInfo subProjectFolder = new DirectoryInfo(Path.Combine(projectFolder.FullName, projectName));

      FileInfo projectFile = new FileInfo(Path.Combine(subProjectFolder.FullName, projectName + "." + Project.EXT));

      _project = new Project(projectFile, appDir);

      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion

    #region private void _btnBrowseProjLocation_Click(object sender, EventArgs e)
    private void _btnBrowseProjLocation_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtProjectLocation.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtProjectLocation.Text = fbd.SelectedPath;
    }
    #endregion

    #region private void _btnBrowseAppFolder_Click(object sender, EventArgs e)
    private void _btnBrowseAppFolder_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtAppLocation.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtAppLocation.Text = fbd.SelectedPath;
    }
    #endregion
  }
}