/**
* @version $Id: LanguageFile.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  using System.Globalization;

  class LanguageFile:LanguageInfo {
    
    private readonly EasyProperties _easyProperties = new EasyProperties();
    public readonly FileInfo _file;

    internal static readonly string LANGUAGE_PROPERTIES_NODE_NAME = "LanguageProperties";

    public LanguageFile(FileInfo file) {
      _file = file;
      this.Init();
    }

    public LanguageFile(string code, DirectoryInfo dir) {
      _file = CreateFile(code, dir);
      this.Init();
    }

    public static FileInfo CreateFile(string code, DirectoryInfo dir) {
      return new FileInfo(Path.Combine(dir.FullName, code + "." + LanguageManager.FILE_EXT));
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public EasyProperties Ps
    public EasyProperties Ps {
      get { return _easyProperties; }
    }
    #endregion

    #region public void Save()
    public void Save() {
      _easyProperties.Save(_file);
    }
    #endregion

    #region private void Init()
    private void Init() {
      if (_file.Exists) {
        _easyProperties.Clear();
        _easyProperties.Load(_file);

        string code = _easyProperties[LANGUAGE_PROPERTIES_NODE_NAME].GetValue<string>("Code", "en-US");
        string displayName = _easyProperties[LANGUAGE_PROPERTIES_NODE_NAME].GetValue<string>("DisplayName", "English");
        string englishName = _easyProperties[LANGUAGE_PROPERTIES_NODE_NAME].GetValue<string>("EnglishName", "English");
        this.SetValue(code, displayName, englishName);
      } else {
        string code = Path.GetFileNameWithoutExtension(_file.FullName);
        CultureInfo ci = CultureInfo.GetCultureInfo(code);

        _easyProperties[LANGUAGE_PROPERTIES_NODE_NAME].SetValue<string>("Code", code);
        _easyProperties[LANGUAGE_PROPERTIES_NODE_NAME].SetValue<string>("DisplayName", ci.DisplayName);
        _easyProperties[LANGUAGE_PROPERTIES_NODE_NAME].SetValue<string>("EnglishName", ci.EnglishName);
        this.Save();
        this.File.LastWriteTime = new DateTime(1970, 1, 1);
        this.File.Refresh();
        this.SetValue(code, ci.DisplayName, ci.EnglishName);
      }
    }
    #endregion

    #region public void ReLoad()
    public void ReLoad() {
      this.Init();
    }
    #endregion
  }
}
