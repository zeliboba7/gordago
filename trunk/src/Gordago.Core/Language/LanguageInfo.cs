/**
* @version $Id: LanguageInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class LanguageInfo {

    private string _code;
    private string _displayName;
    private string _englishName;

    protected LanguageInfo() {
    }

    public LanguageInfo(string code, string displayName, string englishName) {
      _code = code;
      _displayName = displayName;
      _englishName = englishName;
    }

    #region public string Code
    public string Code {
      get { return _code; }
    }
    #endregion

    #region public string DisplayName
    public string DisplayName {
      get { return _displayName; }
    }
    #endregion

    #region public string EnglishName
    public string EnglishName {
      get { return _englishName; }
    }
    #endregion

    #region protected void SetValue(string code, string displayName, string englishName)
    protected void SetValue(string code, string displayName, string englishName) {
      _code = code;
      _displayName = displayName;
      _englishName = englishName;
    }
    #endregion

    public override string ToString() {

      return _englishName;
    }
  }
}
