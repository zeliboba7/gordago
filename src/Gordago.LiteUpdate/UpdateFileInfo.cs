/**
* @version $Id: UpdateFileInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  class UpdateFileInfoManager:IEnumerable<UpdateFileInfo> {
    
    private readonly FileInfo _file;

    private readonly string _formatVersion;
    private readonly int _productVersion;
    private List<UpdateFileInfo> _ufiList = new List<UpdateFileInfo>();

    public UpdateFileInfoManager(FileInfo file) {
      _file = file;
      if (!_file.Exists)
        return;

      FileStream fs = file.OpenRead();
      TextReader tr = new StreamReader(fs);
      
      string fileInform = tr.ReadLine();

      string[] sa = fileInform.Split(',');
      _formatVersion = sa[0].Split('=')[1];
      _productVersion = Convert.ToInt32( sa[1].Split('=')[1]);

      string line;
      while ((line = tr.ReadLine()) != null) {
        UpdateFileInfo ufi = new UpdateFileInfo(line);
        _ufiList.Add(ufi);
      }
      fs.Close();
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public string FormatVersion
    public string FormatVersion {
      get { return _formatVersion; }
    }
    #endregion

    #region public int ProductVersion
    public int ProductVersion {
      get { return _productVersion; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _ufiList.Count; }
    }
    #endregion

    #region public UpdateFileInfo this[int index]
    public UpdateFileInfo this[int index] {
      get { return _ufiList[index]; }
    }
    #endregion

    #region public UpdateFileInfo Find(UpdateFileInfo updateFileInfo)
    public UpdateFileInfo Find(UpdateFileInfo updateFileInfo) {
      foreach (UpdateFileInfo ufiCurrent in this) {
        if (ufiCurrent.FileInfo.ToUpper().Equals(updateFileInfo.FileInfo.ToUpper()))
          return ufiCurrent;
      }
      return null;
    }
    #endregion

    #region IEnumerable<UpdateFileInfo> Members

    public IEnumerator<UpdateFileInfo> GetEnumerator() {
      return _ufiList.GetEnumerator();
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }
    #endregion
  }

  #region enum UpdateFileInfoAction
  enum UpdateFileInfoAction {
    Added,
    Updated,
    Deleted
  }
  #endregion

  #region class UpdateFileInfo
  class UpdateFileInfo {

    private UpdateFileInfoAction _action = UpdateFileInfoAction.Updated;

    private readonly string _fileInfo;
    private readonly int _version;
    private readonly int _compressSize;
    private readonly int _fileSize;

    public UpdateFileInfo(string parseLine) {
      string[] sa = parseLine.Split(',');
      _fileInfo = sa[1];
      if (sa[0] == "-") {
        _action = UpdateFileInfoAction.Deleted;
        _compressSize = 0;
        _fileSize = 0;
        _version = 0;
      } else {
        _action = UpdateFileInfoAction.Updated;
        _version = Convert.ToInt32(sa[2]);
        _compressSize = Convert.ToInt32(sa[3]);
        _fileSize = Convert.ToInt32(sa[4]);
      }
    }

    #region public UpdateFileInfoAction Action
    public UpdateFileInfoAction Action {
      get { return _action; }
    }
    #endregion

    #region public string FileInfo
    public string FileInfo {
      get { return _fileInfo; }
    }
    #endregion

    #region public int Version
    public int Version {
      get { return _version; }
    }
    #endregion

    #region public int CompressSize
    public int CompressSize {
      get { return _compressSize; }
    }
    #endregion

    #region public int FileSize
    public int FileSize {
      get { return _fileSize; }
    }
    #endregion
  }
  #endregion
}
