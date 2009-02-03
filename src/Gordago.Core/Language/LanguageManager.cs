/**
* @version $Id: LanguageManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  public class LanguageManager {

    #region public class Phrase
    public class Phrase {
      private readonly string _name;
      private readonly string _value;
      
      public Phrase(string name, string value) {
        _name = name;
        _value = value;
      }

      public string Name {
        get { return _name; }
      }

      public string Value {
        get { return _value; }
      }
    }
    #endregion

    #region public class PhraseGroup
    public class PhraseGroup {
      
      private readonly EasyPropertiesNode _epn;
      private readonly string _name;
      private LanguageManager _owner;

      internal PhraseGroup(string name, LanguageManager owner) {
        _name = name;
        _owner = owner;

        _epn = _owner._current.Ps[name];
      }

      #region public string Name
      public string Name {
        get { return _name; }
      }
      #endregion

      #region public string Phrase(string name, string defaultPhrase)
      public string Phrase(string name, string defaultPhrase) {
        if (!_epn.ContainsProperty(name))
          _owner._modify = true;
        return _epn.GetValue<string>(name, defaultPhrase);
      }
      #endregion

      #region public string this[string phrase]
      public string this[string phrase] {
        get {
          return this.Phrase(phrase, phrase);
        }
      }
      #endregion

      #region public Phrase[] GetPhrases()
      public Phrase[] GetPhrases() {
        EasyPropertiesNode.Property[] prases = _epn.GetValues();
        List<Phrase> list = new List<Phrase>();
        foreach (EasyPropertiesNode.Property phrase in prases) {
          list.Add(new Phrase(phrase.Name, (string)phrase.Value));
        }
        return list.ToArray();
      }
      #endregion

      #region public void SetPhrase(string name, string value)
      public void SetPhrase(string name, string value) {
        _epn.SetValue<string>(name, value);
      }
      #endregion
    }
    #endregion

    public static readonly string LANGUAGES_LIST_FILENAME = "languages.lst";

    internal static readonly string FILE_EXT = "lng";

    private readonly DirectoryInfo _languagesDirectory;
    private LanguageFile _current = null;
    private DateTime _currentModifyTime;

    private readonly FileInfo _listFile;
    private readonly Dictionary<string, LanguageInfo> _languages = new Dictionary<string, LanguageInfo>();
    private readonly string _appId;
    private bool _modify = false;

    public LanguageManager(DirectoryInfo languagesRootDirectory, string appId) {
      _languagesDirectory = new DirectoryInfo(Path.Combine(languagesRootDirectory.FullName, appId));
      _languagesDirectory.Create();
      _appId = appId;
      _listFile = new FileInfo(Path.Combine(_languagesDirectory.FullName, LANGUAGES_LIST_FILENAME));
      this.Refresh();
      this.Load();
    }

    #region public string Code
    public string Code {
      get {
        this.Check();
        return _current.Code; 
      }
    }
    #endregion

    #region public string DisplayName
    public string DisplayName {
      get {
        this.Check();
        return _current.DisplayName;
      }
    }
    #endregion

    #region public string EnglishName
    public string EnglishName {
      get {
        this.Check();
        return _current.EnglishName;
      }
    }
    #endregion

    #region public string AppId
    public string AppId {
      get { return _appId; }
    }
    #endregion

    #region private void Check()
    private void Check() {
      if (_current != null)
        return;
      this.Select("en-US");
    }
    #endregion

    #region private void Refresh()
    private void Refresh() {
      FileInfo[] files = _languagesDirectory.GetFiles("*." + LanguageManager.FILE_EXT);
      bool update = !_listFile.Exists;
      foreach (FileInfo file in files) {
        string code = Path.GetFileNameWithoutExtension(file.FullName);
        if (!_languages.ContainsKey(code)) {
          update = true;
          break;
        }
      }
      if (!update) {
        List<LanguageInfo> list = this.Read();
        foreach (LanguageInfo li in list) {
          string code = Path.GetFileNameWithoutExtension(_listFile.FullName);
          if (!code.Equals(li.Code)) {
            update = true;
            break;
          }
        }
      }
      if (!update)
        return;

      if (_listFile.Exists)
        _listFile.Delete();

      using (FileStream fs = _listFile.Create()) {
        TextWriter tw = new StreamWriter(fs);

        foreach (FileInfo file in files) {
          LanguageFile lfile = new LanguageFile(file);
          tw.WriteLine(string.Format("{0},{1},{2}", lfile.Code, lfile.DisplayName, lfile.EnglishName));
        }
        tw.Flush();
        fs.Close();
      }
      _listFile.Refresh();
      this.Load();
    }
    #endregion

    #region private List<LanguageInfo> Read()
    private List<LanguageInfo> Read() {
      List<LanguageInfo> list = new List<LanguageInfo>();
      if (!_listFile.Exists)
        return list;
      using (FileStream fs = _listFile.OpenRead()) {
        TextReader tr = new StreamReader(fs);
        string line;
        while ((line = tr.ReadLine()) != null) {
          string[] sa = line.Split(',');
          string code = sa[0];
          string dname = sa[1];
          string ename = sa[2];
          list.Add(new LanguageInfo(code, dname, ename));
        }
        fs.Close();
      }
      return list;
    }
    #endregion

    #region private void Load()
    private void Load() {
      _languages.Clear();
      List<LanguageInfo> list = this.Read();
      foreach (LanguageInfo li in list) {
        _languages.Add(li.Code, li);
      }
    }
    #endregion

    #region public void Select(string code)
    public void Select(string code) {
      //if (_current != null) {
      //  if (code == _current.Code)
      //    return;

      //  this.Save();
      //  _current = null;
      //}

      _modify = false;
      bool isNew = !_languages.ContainsKey(code); 

      _current = new LanguageFile(code, this._languagesDirectory);
      if (isNew) {
        this.Refresh();
        this.SyncLanguagePhrases();
        _current.ReLoad();
      }
      _current.File.Refresh();
      _currentModifyTime = _current.File.LastWriteTime;

      if (isNew) {
      }
    }
    #endregion

    #region public void BuildComplete()
    public void BuildComplete() {
      if (!_modify)
        return;
      _current.File.Refresh();
      if (_currentModifyTime != _current.File.LastWriteTime)
        return;
      this.Save();
    }
    #endregion

    #region public void Save()
    public void Save() {
      if (_current == null)
        return;

      _current.Save();
      _current.File.Refresh();
      _currentModifyTime = _current.File.LastWriteTime;
    }
    #endregion

    #region public PhraseGroup this[string name]
    public PhraseGroup this[string name] {
      get {
        this.Check();
        return new PhraseGroup(name, this);
      }
    }
    #endregion

    #region public LanguageInfo[] ReLoad()
    public LanguageInfo[] ReLoad() {
      this.Refresh();
      List<LanguageInfo> list = this.Read();
      return list.ToArray();
    }
    #endregion

    #region public PhraseGroup[] GetGroups()
    public PhraseGroup[] GetGroups() {
      EasyPropertiesNode[] nodes = _current.Ps.GetChildProperties();
      List<PhraseGroup> list = new List<PhraseGroup>();
      foreach (EasyPropertiesNode node in nodes) {
        if (node.Name.Equals(LanguageFile.LANGUAGE_PROPERTIES_NODE_NAME))
          continue;
        list.Add(new PhraseGroup(node.Name, this));
      }
      return list.ToArray();
    }
    #endregion

    #region public void SyncLanguagePhrases()
    public void SyncLanguagePhrases() {
      if (this._languages.Count <= 1)
        return;
      
      string engCode = "en-US";

      #region Auto Select from date
      /* Auto Select from date */ 
      //LanguageInfo[] currentLangFiles = this.ReLoad();
      //LanguageInfo lastModifyLangInfoFile = null;
      //foreach (LanguageInfo li in currentLangFiles) {
      //  if (lastModifyLangInfoFile == null) {
      //    lastModifyLangInfoFile = li;
      //    continue;
      //  }
      //  FileInfo actualFile = LanguageFile.CreateFile(lastModifyLangInfoFile.Code, _languagesDirectory);
      //  FileInfo currentFile = LanguageFile.CreateFile(li.Code, _languagesDirectory);

      //  if (currentFile.LastWriteTime > actualFile.LastWriteTime)
      //    lastModifyLangInfoFile = li;
      //}
      //engCode = lastModifyLangInfoFile.Code;
      #endregion

      LanguageFile lActualEngFile = new LanguageFile(engCode, this._languagesDirectory);

      foreach (LanguageInfo li in _languages.Values) {
        if (li.Code.Equals(engCode))
          continue;

        LanguageFile lfile = new LanguageFile(li.Code, this._languagesDirectory);
        this.SyncLangFile(lActualEngFile, lfile);
      }
    }
    #endregion

    #region private void SyncLangFile(LanguageFile lFileEng, LanguageFile lFileSync)
    private void SyncLangFile(LanguageFile lFileEng, LanguageFile lFileSync) {
      bool changed = false;
      foreach (EasyPropertiesNode nodeEng in lFileEng.Ps.GetChildProperties()) {
        if (nodeEng.Name.Equals(LanguageFile.LANGUAGE_PROPERTIES_NODE_NAME))
          continue;

        EasyPropertiesNode nodeSync = lFileSync.Ps[nodeEng.Name];
        EasyPropertiesNode.Property[] valsEng = nodeEng.GetValues();

        foreach (EasyPropertiesNode.Property valEng in valsEng) {
          if (nodeSync.ContainsProperty(valEng.Name))
            continue;
          changed = true;
          nodeSync.SetValue<string>(valEng.Name, (string)valEng.Value);
        }
      }
      if (!changed)
        return;
      lFileSync.Save();
    }
    #endregion
  }
}
