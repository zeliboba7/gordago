/**
* @version $Id: TMFolderNode.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  class TMFolderNode:TreeNode {

    private readonly TMFolder _tmFolder;

    public TMFolderNode(TMFolder tmFolder):base(tmFolder.Name) {
      _tmFolder = tmFolder;
      foreach (TMFolder folder in tmFolder.Folders) {
        this.Nodes.Add(new TMFolderNode(folder));
      }
      this.SetDirectoryState(false);
    }

    #region public TMFolder Folder
    public TMFolder Folder {
      get { return _tmFolder; }
    }
    #endregion

    #region public void SetDirectoryState(bool open)
    public void SetDirectoryState(bool open) {
      if (open) {
        this.SelectedImageKey = this.ImageKey = TMFileSystemControl.IMG_FOLDER_OPEN;
      } else {
        this.SelectedImageKey = this.ImageKey = TMFileSystemControl.IMG_FOLDER_CLOSE;
      }
    }
    #endregion
  }
}
