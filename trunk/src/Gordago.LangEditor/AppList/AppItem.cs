/**
* @version $Id: AppItem.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor.AppList
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Core;

  class AppItem {

    private readonly string _appId;
    private readonly LanguageManager _languageManager;

    public AppItem(string appId) {
      _appId = appId;
      _languageManager = new LanguageManager(Global.Setup.LanguagesDirectory, appId);
      
      _languageManager.SyncLanguagePhrases();
    }

    #region public string AppId
    public string AppId {
      get { return _appId; }
    }
    #endregion

    #region public LanguageManager Language
    public LanguageManager Language {
      get { return _languageManager; }
    }
    #endregion
  }
}
