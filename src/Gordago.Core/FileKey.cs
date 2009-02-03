/**
* @version $Id: FileKey.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  public class FileKey {

    private FileInfo _file;
    private string _key;

    public FileKey(FileInfo file) {
      _file = file;
      _key = file.FullName.Replace('/', '\\').ToUpper();
    }

    public FileKey(string fileName)
      : this(new FileInfo(fileName)) {
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public string Key
    public string Key {
      get { return _key; }
    }
    #endregion

    #region public override bool Equals(object obj)
    public override bool Equals(object obj) {
      if (!(obj is FileKey)) {
        return false;
      }
      return this._key.Equals((obj as FileKey).Key);
    }
    #endregion

    #region public override int GetHashCode()
    public override int GetHashCode() {
      return this._key.GetHashCode();
    }
    #endregion
  }
}
