/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.WebUpdate {

  public struct UpdateFileInfo {
		private string _filename;
		private int _version;
		private string _action;

		private bool _isCommercy;

		public UpdateFileInfo(string filename, int version, string action, bool isCommercy){
			this._filename = filename;
			this._version = version;
			this._action = action;
			this._isCommercy = isCommercy;
		}

		#region public bool IsCommerciy
		public bool IsCommerciy{
			get{return this._isCommercy;}
		}
		#endregion

		public string FileName{
			get{return this._filename;}
		}

		/// <summary>
		/// Текущая версия
		/// </summary>
		public int Version{
			get{return this._version;}
		}

		/// <summary>
		/// что необходимо делать с файлом:
		/// + файл добавляеться, либо заменяеться
		/// - файл убираеться
		/// </summary>
		public string Action{
			get{return this._action;}
		}
	}
}
