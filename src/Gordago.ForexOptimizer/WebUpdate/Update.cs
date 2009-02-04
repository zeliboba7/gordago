/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Threading;

namespace Gordago.WebUpdate {
  
  public class Update {

		/// <summary>
		/// Обновление с демо на комерческую версию
		/// </summary>
		public static bool IsUpdateFromDemo = false;
	}

  public enum UpdateEngineCheckUpdateType {
    Yes,
    None,
    UpdateRealToDemo,
    ErrorOnServer,
    ErrorLoginOrPassword,
    ErrorCreatedSession,
    ErrorDownloadListFile
  }
}
