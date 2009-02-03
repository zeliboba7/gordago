/**
* @version $Id: TMFile.cs 4 2009-02-03 13:20:59Z AKuzmin $
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

  public class TMFile:ITMObject {

    private readonly FileInfo _file;
    private readonly TMFolder _folder;
    private readonly string _fullName;

    private LUFileInfo _luFileInfo;
    
    private DateTime _modifyTime;
    private long _length;

    private int _version = 1;

    public TMFile(FileInfo file, TMFolder folder) {
      _folder = folder;
      _file = file;
      _luFileInfo = LUFileInfoManager.GetLUFileInfo(_file);
      _fullName = folder.FullName + "\\" + _file.Name;
    }

    #region public int Version
    public int Version {
      get { return _version; }
      set { _version = value; }
    }
    #endregion

    #region public string FullName
    public string FullName {
      get { return _fullName; }
    }
    #endregion

    #region public TMFolder Folder
    public TMFolder Folder {
      get {
        return _folder;
      }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return this._file.Name; }
    }
    #endregion

    #region public DateTime ModifyTime
    public DateTime ModifyTime {
      get { return _modifyTime; }
      set { _modifyTime = value; }
    }
    #endregion

    #region public long Length
    public long Length {
      get { return _length; }
      set { this._length = value; }
    }
    #endregion

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public LUFileInfo LUFileInfo
    public LUFileInfo LUFileInfo {
      get { return _luFileInfo; }
    }
    #endregion

    #region public TMFile Clone(TMFolder folder)
    public TMFile Clone(TMFolder folder) {
      TMFile file = new TMFile(_file, folder);
      file._length = _length;
      file._luFileInfo = _luFileInfo;
      file._modifyTime = _modifyTime;
      file._version = _version;
      return file;
    }
    #endregion

    #region public void Update()
    public void Update() {

      this.ModifyTime = this.File.LastWriteTime;
      this.Length = this.File.Length;
    }
    #endregion

    #region public void Delete()
    public void Delete() {
      Folder.Files.Remove(this);
    }
    #endregion
  }
}
