/**
* @version $Id: TMTreeView.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public class TMTreeView: TreeView {

    private TMFolderNode _savedSelecNode = null;

    protected override void OnBeforeCollapse(TreeViewCancelEventArgs e) {
      if (e.Node is TMFolderNode) 
        (e.Node as TMFolderNode).SetDirectoryState(false);
      base.OnBeforeCollapse(e);
    }

    protected override void OnBeforeExpand(TreeViewCancelEventArgs e) {
      if (e.Node is TMFolderNode) 
        (e.Node as TMFolderNode).SetDirectoryState(true);
      
      base.OnBeforeExpand(e);
    }

    protected override void OnBeforeSelect(TreeViewCancelEventArgs e) {
      TMFolderNode tmNode = e.Node as TMFolderNode;
      if (tmNode != null) {
        if (_savedSelecNode != null) {
          _savedSelecNode.SetDirectoryState(_savedSelecNode.IsExpanded);
        }
        tmNode.SetDirectoryState(true);
        _savedSelecNode = tmNode;
      }

      base.OnBeforeSelect(e);
    }
  }
}
