/**
* @version $Id: ProjectForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.AppStructureEditor.Projects
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;
  using System.IO;

  partial class ProjectForm : Form {

    private readonly Project _project;

    public ProjectForm(Project project) {
      InitializeComponent();
      _project = project;
    }

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      this.Text = "Settings";
      this._txtBinDirectory.Text = _project.BinDirectory.FullName;
      this._txtAppRootDir.Text = _project.AppRootDirectory.FullName;
      this._txtLanguagesDir.Text = _project.LanguagesDirectory.FullName;
      this._txtOptionsDirectory.Text = _project.OptionsDirectory.FullName;
      base.OnLoad(e);
    }
    #endregion

    #region private void _btnBrowseAppRootDir_Click(object sender, EventArgs e)
    private void _btnBrowseAppRootDir_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtAppRootDir.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtAppRootDir.Text = fbd.SelectedPath;
      _project.SetAppRootDirectory(new DirectoryInfo(_txtAppRootDir.Text));
    }
    #endregion

    #region private void _btnBrowseLanguagesDir_Click(object sender, EventArgs e)
    private void _btnBrowseLanguagesDir_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtLanguagesDir.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtLanguagesDir.Text = fbd.SelectedPath;
      _project.SetLanguagesDirectory(new DirectoryInfo(_txtLanguagesDir.Text));
    }
    #endregion

    #region private void _btnBrowseOptionsDirectory_Click(object sender, EventArgs e)
    private void _btnBrowseOptionsDirectory_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtOptionsDirectory.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtOptionsDirectory.Text = fbd.SelectedPath;
      _project.SetOptionsDirectory(new DirectoryInfo(_txtOptionsDirectory.Text));
    }
    #endregion
  }
}
