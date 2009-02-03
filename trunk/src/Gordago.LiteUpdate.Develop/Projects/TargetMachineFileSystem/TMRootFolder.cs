/**
* @version $Id: TMRootFolder.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public class TMRootFolder : TMFolder {

    public event EventHandler DataChanged;


    private LURootFolderType _rootFolderType;

    public TMRootFolder(LURootFolderType folderType) : base(string.Format("[{0}]", folderType.ToString()), null) {
      _rootFolderType = folderType;
    }

    #region public new string Name
    public new string Name {
      get { return string.Format("[{0}]", _rootFolderType.ToString()); }
    }
    #endregion

    #region public LURootFolderType RootFolderType
    public LURootFolderType RootFolderType {
      get { return _rootFolderType; }
      set { _rootFolderType = value; }
    }
    #endregion

    #region public new TMRootFolder Clone(TMFolder parent)
    public new TMRootFolder Clone(TMFolder parent) {
      TMRootFolder rootFolder = new TMRootFolder(_rootFolderType);

      foreach (TMFolder childFolder in this.Folders) {
        rootFolder.Folders.Add(childFolder.Clone(rootFolder));
      }
      foreach (TMFile file in this.Files) {
        rootFolder.Files.Add(file.Clone(rootFolder));
      }
      return rootFolder;
    }
    #endregion

    #region public new TMFolder FindFolder(string path)
    public new TMFolder FindFolder(string path) {
      return base.FindFolder(path);
    }
    #endregion

    #region protected internal void OnDataChanged()
    protected internal void OnDataChanged() {
      if (this.DataChanged != null)
        this.DataChanged(this, EventArgs.Empty);
    }
    #endregion
  }
}
