/**
* @version $Id: FilesModify.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class FilesModify {

    private readonly List<string> _filesAdded = new List<string>();
    private readonly List<string> _filesRemoved = new List<string>();
    private readonly List<string> _filesUpdated = new List<string>();

    public FilesModify() {
    }

    #region public List<string> FilesAdded
    public List<string> FilesAdded {
      get { return _filesAdded; }
    }
    #endregion

    #region public List<string> FilesRemoved
    public List<string> FilesRemoved {
      get { return _filesRemoved; }
    }
    #endregion

    #region public List<string> FilesUpdated
    public List<string> FilesUpdated {
      get { return _filesUpdated; }
    }
    #endregion
  }
}
