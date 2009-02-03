/**
* @version $Id: TMFolder.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class TMFolder: ITMObject {

    #region public class TMFolderCollection:IEnumerable<TMFolder>
    public class TMFolderCollection:IEnumerable<TMFolder> {

      private readonly TMFolder _owner;
      private readonly List<TMFolder> _folders = new List<TMFolder>();

      public TMFolderCollection(TMFolder owner) {
        _owner = owner;
      }

      #region public int Count
      public int Count {
        get { return _folders.Count; }
      }
      #endregion

      #region public TMFolder this[int index]
      public TMFolder this[int index] {
        get { return _folders[index];}
      }
      #endregion

      #region public IEnumerator<TMFolder> GetEnumerator()
      public IEnumerator<TMFolder> GetEnumerator() {
        return _folders.GetEnumerator();
      }
      #endregion

      #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        return _folders.GetEnumerator();
      }
      #endregion

      #region public bool Remove(TMFolder folder)
      public bool Remove(TMFolder folder) {
        bool ret = _folders.Remove(folder);
        _owner.Root.OnDataChanged();
        return ret;
      }
      #endregion

      #region public void Add(TMFolder folder)
      public void Add(TMFolder folder) {
        _folders.Add(folder);
        this._owner.Root.OnDataChanged();
      }
      #endregion
    }
    #endregion

    #region public class TMFileCollection : IEnumerable<TMFile>
    public class TMFileCollection : IEnumerable<TMFile> {
      private readonly TMFolder _owner;
      private readonly List<TMFile> _files = new List<TMFile>();

      public TMFileCollection(TMFolder owner) {
        _owner = owner;
      }

      #region public int Count
      public int Count {
        get { return _files.Count; }
      }
      #endregion

      #region public TMFile this[int index]
      public TMFile this[int index] {
        get { return _files[index];}
      }
      #endregion

      #region public IEnumerator<TMFile> GetEnumerator()
      public IEnumerator<TMFile> GetEnumerator() {
        return _files.GetEnumerator();
      }
      #endregion

      #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        return _files.GetEnumerator();
      }
      #endregion

      #region public bool Remove(TMFile file)
      public bool Remove(TMFile file) {
        bool ret = _files.Remove(file);
        _owner.Root.OnDataChanged();
        return ret;
      }
      #endregion

      #region public void Add(TMFile file)
      public void Add(TMFile file) {

        foreach (TMFile ffile in _files) {
          if (ffile.FullName == file.FullName)
            return;
        }

        _files.Add(file);
        _owner.Root.OnDataChanged();
      }
      #endregion

      #region public TMFile[] ToArray()
      public TMFile[] ToArray() {
        return _files.ToArray();
      }
      #endregion
    }
    #endregion

    private readonly TMFolder _parent;
    private string _name;
    private readonly TMFolderCollection _folders;
    private readonly TMFileCollection _files ;
    private readonly TMRootFolder _rootFolder;
    private readonly string _fullName;

    public TMFolder(string name, TMFolder parent) {
      _folders = new TMFolderCollection(this);
      _files = new TMFileCollection(this);
      _name = name;
      _parent = parent;
      _rootFolder = this.GetRootFolder() as TMRootFolder;
      _fullName = this.GetPath();
    }

    #region public string FullName
    public string FullName {
      get { return _fullName; }
    }
    #endregion

    #region public TMRootFolder Root
    public TMRootFolder Root {
      get { return _rootFolder; }
    }
    #endregion

    #region public TMFolder Parent
    public TMFolder Parent {
      get { return _parent; }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return _name; }
    }
    #endregion

    #region public TMFolderCollection Folders
    public TMFolderCollection Folders {
      get { return _folders; }
    }
    #endregion

    #region public TMFileCollection Files
    public TMFileCollection Files {
      get { return _files; }
    }
    #endregion

    #region public bool Remove(TMFolder folder)
    public bool Remove(TMFolder folder) {
      foreach (TMFolder item in _folders) {
        if (item == folder) {
          _folders.Remove(folder);
          return true;
        } else {
          if (item.Remove(folder))
            return true;
        }
      }
      return false;
    }
    #endregion

    #region public int GetCountFiles()
    public int GetCountFiles() {
      int count = 0;
      foreach (TMFolder folder in _folders) {
        count += folder.GetCountFiles();
      }
      foreach (TMFile file in _files) {
        count++;
      }
      return count;
    }
    #endregion

    #region public TMFile[] GetFiles()
    public TMFile[] GetFiles() {
      List<TMFile> list = new List<TMFile>();
      list.AddRange(_files.ToArray());
      foreach (TMFolder folder in _folders) {
        list.AddRange(folder.GetFiles());
      }
      return list.ToArray();
    }
    #endregion

    #region public ITMObject[] GetTMObjects ()
    public ITMObject[] GetTMObjects (){
      List<ITMObject> list = new List<ITMObject>();
      list.Add(this);
      foreach (TMFolder folder in this.Folders) {
        list.AddRange(folder.GetTMObjects());
      }
      foreach (TMFile file in this.Files) {
        list.Add(file);
      }
      return list.ToArray();
    }
    #endregion

    #region public TMFolder Clone(TMFolder parent)
    public TMFolder Clone(TMFolder parent) {
      TMFolder folder = new TMFolder(this.Name, parent);
      foreach (TMFolder childFolder in _folders) {
        folder.Folders.Add(childFolder.Clone(folder));
      }
      foreach (TMFile file in _files) {
        folder.Files.Add(file.Clone(folder));
      }
      return folder;
    }
    #endregion

    #region private void PushName(Stack<string> stack)
    private void PushName(Stack<string> stack) {
      stack.Push(this.Name);
      if (_parent != null)
        _parent.PushName(stack);
    }
    #endregion

    #region private string GetPath()
    private string GetPath() {
      Stack<string> stack = new Stack<string>();
      this.PushName(stack);
      List<string> list = new List<string>();

      foreach (string name in stack) {
        list.Add(name);
      }

      return string.Join("\\", list.ToArray());
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return _fullName;
    }
    #endregion

    #region public override bool Equals(object obj)
    public override bool Equals(object obj) {
      return _fullName.Equals(obj.ToString());
    }
    #endregion

    #region public override int GetHashCode()
    public override int GetHashCode() {
      return _fullName.GetHashCode();
    }
    #endregion

    #region private TMFolder GetRootFolder()
    private TMFolder GetRootFolder() {
      if (this.Parent == null)
        return this;
      return this.Parent.GetRootFolder();
    }
    #endregion

    #region protected TMFolder FindFolder(string path)
    protected TMFolder FindFolder(string path) {
      if (this.FullName.Equals(path))
        return this;
      foreach (TMFolder childFolder in this.Folders) {
        TMFolder retFolder = childFolder.FindFolder(path);
        if (retFolder != null)
          return retFolder;
      }
      return null;
    }
    #endregion
  }
}
