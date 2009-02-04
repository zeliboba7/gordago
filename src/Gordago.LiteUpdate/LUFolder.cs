/**
* @version $Id: LUFolder.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class LUFolder {

    private string _name = "";
    //private readonly LUFolderCollection _luFolders = new LUFolderCollection();
    //private readonly LUFileCollection _luFiles = new LUFileCollection();

    public LUFolder(string name) {
      _name = name;
    }

    #region public string Name
    public string Name {
      get { return _name; }
      set { this._name = value; }
    }
    #endregion

    //#region public LUFolderCollection Folders
    //public LUFolderCollection Folders {
    //  get { return _luFolders; }
    //}
    //#endregion

    //#region public LUFileCollection Files
    //public LUFileCollection Files {
    //  get { return _luFiles; }
    //}
    //#endregion

  }
}
