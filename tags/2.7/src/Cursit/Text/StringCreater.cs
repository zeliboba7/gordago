/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Cursit.Text {

	public class StringCreater {
		ArrayList _alStr;
		
		public StringCreater() {
			_alStr = new ArrayList();
		}
				
		public void AppendString(string appStr){
			string appS = appStr.Trim();
			if (appS.Length < 1) return;
			_alStr.Add(appStr);
		}

		public void AppendStringRange(string[] appStrings){
			foreach (string s in appStrings){
				this.AppendString(s);
			}
		}
				
		#region public string GetString()
		public string GetString(){
			int cnt = _alStr.Count;
			if (cnt < 1) return "";
			string retstr = "";
			for (int i=0;i<cnt;i++){
				retstr += (string)_alStr[i];
			}
			return retstr;
		}
		#endregion

		#region public string GetString(string separator)
		public string GetString(string separator){
			int cnt = _alStr.Count;
			if (cnt < 1) return "";
								
			string[] astr = new string[cnt];
			for (int i=0;i<cnt;i++){
				astr[i] = (string)_alStr[i];
			}
			return string.Join(separator, astr); 
		}
		#endregion

		public int Count{
			get{return this._alStr.Count;}
		}
	}
}
