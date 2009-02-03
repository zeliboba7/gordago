/**
* @version $Id: TMSpecFolderMenuItem.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Windows.Forms;

  public class TMSpecFolderMenuItem : TMContextMenuItem {

    private LURootFolderType _roolFolderType;

    public TMSpecFolderMenuItem(LURootFolderType roolFolderType, TMContextMenuAction action, EventHandler click)
      : base(action, click) {
      _roolFolderType = roolFolderType;

      this.Text = roolFolderType.ToString();
    }

    #region public LURootFolderType SpecialFolder
    public LURootFolderType SpecialFolder {
      get { return _roolFolderType; }
    }
    #endregion
  }
}
