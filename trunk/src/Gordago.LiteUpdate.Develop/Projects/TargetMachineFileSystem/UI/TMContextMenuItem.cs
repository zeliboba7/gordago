/**
* @version $Id: TMContextMenuItem.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public class TMContextMenuItem:ToolStripMenuItem {

    private TMContextMenuAction _action;

    public TMContextMenuItem(TMContextMenuAction action, EventHandler click) {
      _action = action;
      this.Click += click;
      switch (action) {
        case TMContextMenuAction.AddFile:
          this.Text = Global.Languages["Project/FileSystem/CM"]["File..."];
          break;
        case TMContextMenuAction.AddFolder:
          this.Text = Global.Languages["Project/FileSystem/CM"]["Folder"];
          break;
        case TMContextMenuAction.Rename:
          this.Text = Global.Languages["Project/FileSystem/CM"]["Rename"];
          break;
        case TMContextMenuAction.Delete:
          this.Text = Global.Languages["Project/FileSystem/CM"]["Delete"];
          break;
      }
    }

    #region public TMContextMenuAction Action
    public TMContextMenuAction Action {
      get { return _action; }
    }
    #endregion
  }
}
