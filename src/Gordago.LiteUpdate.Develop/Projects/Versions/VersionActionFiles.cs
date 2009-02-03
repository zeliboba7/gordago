/**
* @version $Id: VersionActionFiles.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public partial class VersionActionFiles : GroupBox {

    #region class ActionFilesListView : ListView
    class ActionFilesListView : ListView {

      private bool _readonly = true;
      private List<string> _files;

      #region public bool Readonly
      public bool Readonly {
        get { return _readonly; }
        set { _readonly = false; }
      }
      #endregion

      #region protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
      protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
        if (!this.Readonly) {
          if (keyData == Keys.Delete) {
            for (int i = 0; i < this.SelectedItems.Count; i++) {
              ListViewItem lvi = this.SelectedItems[i];
              this.Items.Remove(lvi);
              _files.Remove(lvi.Text);
            }
          }
        }
        return base.ProcessCmdKey(ref msg, keyData);
      }
      #endregion

      #region public void SetFiles(List<string> files)
      public void SetFiles(List<string> files) {
        _files = files;
        foreach (string file in files) {
          this.Items.Add(new ListViewItem(file));
        }
      }
      #endregion
    }
    #endregion

    public VersionActionFiles() {
      InitializeComponent();

      try {
        _lstFiles.Columns[0].Text = Global.Languages["Project/VFM"]["File"];
      } catch { }
    }

    #region public bool ReadOnly
    public bool ReadOnly {
      get { return _lstFiles.Readonly; }
      set { _lstFiles.Readonly = value; }
    }
    #endregion

    #region public void SetFiles(List<string> files)
    public void SetFiles(List<string> files) {
      _lstFiles.SetFiles(files);
    }
    #endregion
  }
}
