/**
* @version $Id: OpenProjectForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public partial class OpenProjectForm : Form {
    
    public OpenProjectForm() {
      InitializeComponent();
    }

    #region public DirectoryInfo AppBinDirectory
    public DirectoryInfo AppBinDirectory {
      get { return new DirectoryInfo(_txtBinDirectory.Text); }
    }
    #endregion

    #region protected override void OnLoad(EventArgs e)
    protected override void OnLoad(EventArgs e) {
      DirectoryInfo dir = new DirectoryInfo(Global.Properties["Projects"].GetValue<string>("AppBinDirectory", Global.Setup.BinDirectory.FullName));
      if (!dir.Exists)
        dir = Global.Setup.BinDirectory;
      _txtBinDirectory.Text = dir.FullName;
      base.OnLoad(e);
    }
    #endregion

    #region private void _btnOK_Click(object sender, EventArgs e)
    private void _btnOK_Click(object sender, EventArgs e) {
      try {
        DirectoryInfo dir = new DirectoryInfo(_txtBinDirectory.Text);
        if (!dir.Exists)
          return;
      } catch {
        return;
      }
      Global.Properties["Projects"].SetValue<string>("AppBinDirectory", _txtBinDirectory.Text);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    #endregion

    #region private void _btnBrowse_Click(object sender, EventArgs e)
    private void _btnBrowse_Click(object sender, EventArgs e) {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.SelectedPath = _txtBinDirectory.Text;
      if (fbd.ShowDialog() != DialogResult.OK)
        return;
      _txtBinDirectory.Text = fbd.SelectedPath;
    }
    #endregion

    #region private void _btnCancel_Click(object sender, EventArgs e)
    private void _btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    #endregion
  }
}