/**
* @version $Id: RecentProject.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  public class RecentProject {
    private readonly FileInfo _file;

    public RecentProject(FileInfo file) {
      _file = file;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public string Name
    public string Name {
      get {
        return Path.GetFileNameWithoutExtension(_file.FullName);
      }
    }
    #endregion
  }
}
