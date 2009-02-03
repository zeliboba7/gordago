/**
* @version $Id: AddFolderForm.cs 4 2009-02-03 13:20:59Z AKuzmin $
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
  using System.IO;

  public partial class AddFolderForm : Form {

    public AddFolderForm() {
      InitializeComponent();

      this.Text = Global.Languages["AddFolder"]["Add Folder"];
      _lblFolderName.Text = Global.Languages["AddFolder"]["Folder Name"];
      _btnOk.Text = Global.Languages["Button"]["OK"];
      _btnCancel.Text = Global.Languages["Button"]["Cancel"];
    }


    #region public string FolderName
    public string FolderName {
      get { return _txtFolderName.Text; }
    }
    #endregion 

    #region private void _btnOk_Click(object sender, EventArgs e)
    private void _btnOk_Click(object sender, EventArgs e) {
      if (this.FolderName == "") {
        MessageBox.Show("You must enter a name");
        return;
      }

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
  }
}