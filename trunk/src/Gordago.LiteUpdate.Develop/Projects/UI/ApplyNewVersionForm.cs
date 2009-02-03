/**
* @version $Id: ApplyNewVersionForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Text;
  using System.Windows.Forms;

  public partial class ApplyNewVersionForm : Form {

    private readonly VersionInfo _version;
    
    public ApplyNewVersionForm(VersionInfo version) {
      _version = version;

      InitializeComponent();
      _versionFilesModifyControl.SetVersion(version);

      try {
        this.Text = string.Format(Global.Languages["CheckVersion"]["Version {0}"], version.Number);
        this._lblChangesInFileSystem.Text = Global.Languages["CheckVersion"].Phrase("LblChanges", "Changes in file system.");
        this._lblToApplyChanges.Text = Global.Languages["CheckVersion"].Phrase("LblToApply", "To apply these changes for the new version?");
        this._btnYes.Text = Global.Languages["CheckVersion"]["Create New"];
        this._btnNo.Text = Global.Languages["CheckVersion"]["Apply Current"];
        this._btnCancel.Text = Global.Languages["Button"]["Cancel"];
      } catch { }
    }

    #region private void _btnYes_Click(object sender, EventArgs e)
    private void _btnYes_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Yes;
      this.Close();
    }
    #endregion

    #region private void _btnNo_Click(object sender, EventArgs e)
    private void _btnNo_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.No;
      this.Close();
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