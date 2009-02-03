/**
* @version $Id: AppListManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor.AppList
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using Gordago.Core;

  class AppListManager:IEnumerable< AppItem>{
    private readonly List<AppItem> _list = new List<AppItem>();

    public AppListManager() {
      DirectoryInfo[] appIdDirs = Global.Setup.LanguagesDirectory.GetDirectories();
      foreach (DirectoryInfo dir in appIdDirs) {
        if (!(new FileInfo(Path.Combine(dir.FullName, LanguageManager.LANGUAGES_LIST_FILENAME)).Exists))
          continue;
        _list.Add(new AppItem(dir.Name));
      }
    }

    #region public int Count
    public int Count {
      get { return _list.Count; }
    }
    #endregion

    #region public AppItem this[int index]
    public AppItem this[int index] {
      get { return _list[index]; }
    }
    #endregion

    #region public IEnumerator<AppItem> GetEnumerator()
    public IEnumerator<AppItem> GetEnumerator() {
      return _list.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return _list.GetEnumerator();
    }
    #endregion
  }
}
