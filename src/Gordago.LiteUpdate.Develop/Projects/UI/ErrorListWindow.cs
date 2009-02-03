/**
* @version $Id: ErrorListWindow.cs 4 2009-02-03 13:20:59Z AKuzmin $
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
  using Gordago.Docking;

  public partial class ErrorListWindow : DockWindowPanel {

    #region class ErrorFileListViewItem : ListViewItem
    class ErrorFileListViewItem : ListViewItem {

      private readonly TMFile _file;

      public ErrorFileListViewItem(int n, TMFile file) {
        _file = file;
        this.ImageKey = "Error.png";
        this.Text = n.ToString();
        string msg = string.Format("File: '{0}' - File not found: {1}", file.FullName, file.File.FullName);
        this.SubItems.Add(msg);
      }

      public TMFile File {
        get { return _file; }
      }
    }
    #endregion

    public ErrorListWindow() {
      InitializeComponent();
      this.TabText = Global.Languages["ErrorList"]["Error List"];
      this._lstErrors.Columns[0].Text = Global.Languages["ErrorList"]["NN"]; ;
      this._lstErrors.Columns[1].Text = Global.Languages["ErrorList"]["Description"]; ;
    }

    #region public void Clear()
    public void Clear() {
      _lstErrors.Items.Clear();
    }
    #endregion

    #region public void SetErrors(TMFile[] files)
    public void SetErrors(TMFile[] files) {
      for (int i = 0; i < files.Length; i++) {
        this._lstErrors.Items.Add(new ErrorFileListViewItem(i+1, files[i]));
      }
    }
    #endregion

    #region protected override void OnResize(EventArgs e)
    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      columnHeader2.Width = this._lstErrors.Width - columnHeader1.Width-10;
    }
    #endregion

    private void _lstErrors_MouseDoubleClick(object sender, MouseEventArgs e) {
      if (_lstErrors.SelectedItems.Count != 1)
        return;

      ErrorFileListViewItem lvi = _lstErrors.SelectedItems[0] as ErrorFileListViewItem;
      if (lvi == null)
        return;
      Global.DockManager.ShowProjectError(lvi.File);
    }
  }
}
