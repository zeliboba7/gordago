/**
* @version $Id: TMFileSystem.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class TMFileSystem : IEnumerable<TMRootFolder> {

    public event EventHandler DataChanged;

    public class TMRootFolderCollection : Dictionary<LURootFolderType, TMRootFolder> { }

    private readonly TMRootFolderCollection _rootFolders = new TMRootFolderCollection();

    #region public void Add(TMRootFolder rootFolder)
    public void Add(TMRootFolder rootFolder) {

      if (_rootFolders.ContainsKey(rootFolder.RootFolderType))
        return;
      _rootFolders.Add(rootFolder.RootFolderType, rootFolder);
      rootFolder.DataChanged += new EventHandler(rootFolder_DataChanged);
    }
    #endregion

    #region private void rootFolder_DataChanged(object sender, EventArgs e)
    private void rootFolder_DataChanged(object sender, EventArgs e) {
      this.OnDataChanged();
    }
    #endregion

    #region public bool Remove(LURootFolderType rootFolderType)
    public bool Remove(LURootFolderType rootFolderType) {
      return _rootFolders.Remove(rootFolderType);
    }
    #endregion

    #region public bool Remove(TMFolder folder)
    public bool Remove(TMFolder folder) {
      
      if (folder is TMRootFolder) {
        return this.Remove((folder as TMRootFolder).RootFolderType);
      } else {
        foreach (TMRootFolder rootFolder in this) {
          if (rootFolder.Remove(folder))
            return true;
        }
      }
      return false;
    }
    #endregion

    #region public IEnumerator<TMRootFolder> GetEnumerator()
    public IEnumerator<TMRootFolder> GetEnumerator() {
      return _rootFolders.Values.GetEnumerator();
    }
    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion

    #region public int GetCountFiles()
    public int GetCountFiles() {
      int count = 0;
      foreach (TMRootFolder rootFolder in this) {
        count += rootFolder.GetCountFiles();
      }
      return count;
    }
    #endregion

    #region public TMFileSystem Clone()
    public TMFileSystem Clone() {
      TMFileSystem fs = new TMFileSystem();
      foreach (TMRootFolder rootFolder in this) {
        fs.Add(rootFolder.Clone(null));
      }
      return fs;
    }
    #endregion

    #region public ITMObject[] GetTMObjects()
    public ITMObject[] GetTMObjects() {
      List<ITMObject> list = new List<ITMObject>();
      foreach (TMRootFolder rootFolder in this) {
        list.AddRange(rootFolder.GetTMObjects());
      }
      return list.ToArray();
    }
    #endregion

    #region public TMFile[] GetFiles()
    public TMFile[] GetFiles() {
      List<TMFile> list = new List<TMFile>();
      foreach (TMRootFolder rootFolder in this) {
        list.AddRange(rootFolder.GetFiles());
      }
      return list.ToArray();
    }
    #endregion

    #region protected virtual void OnDataChanged()
    protected virtual void OnDataChanged() {
      if (this.DataChanged != null)
        this.DataChanged(this, EventArgs.Empty);
    }
    #endregion

    #region public TMFile[] GetFilesError()
    public TMFile[] GetFilesError() {
      TMFile[] files = this.GetFiles();
      List<TMFile> list = new List<TMFile>();
      foreach (TMFile file in files) {
        if (!file.File.Exists)
          list.Add(file);
      }
      return list.ToArray();
    }
    #endregion
  }
}
