/**
* @version $Id: AppAction.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LangEditor
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  #region interface IAppAction
  interface IAppAction {
    AppAction Action { get;}
    bool Enabled { get;set;}
  }
  #endregion

  enum AppAction {
    ViewAppList,
    ViewLanguages,
    AppExit,
    AppAbout
  }
}
