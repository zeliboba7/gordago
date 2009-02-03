/**
* @version $Id: TMFileNode.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public class TMFileNode :TreeNode{

    private readonly TMFile _tmFile;

    public TMFileNode(TMFile tmFile)
      : base(tmFile.File.Name) {
      _tmFile = tmFile;
    }

    #region public TMFile TMFile
    public TMFile TMFile {
      get { return _tmFile; }
    }
    #endregion
  }
}
