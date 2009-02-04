/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Language {
	/// <summary>
	/// языкова€ база
	/// </summary>
	public class Dictionary {
		private static LanguageData lngData = null;

		public static string LanguageId = "";

		public static void Init(LanguageData languageData){
			lngData = languageData;
		}
		
		/// <summary>
		/// —трока из словор€
		/// </summary>
		public static string GetString(int GroupId, int ValueId){
			if (lngData == null) return "dict err";
			return ConvertN(lngData.GetDictionaryString(GroupId, ValueId));
		}

		public static string GetString(int GroupId, int ValueId, string defString){
			if (lngData == null) return defString;
			return ConvertN(lngData.GetDictionaryString(GroupId, ValueId));
		}

		private static string ConvertN(string str){
			string sret = str.Replace("\\n", "\n");
			return sret;
		}
	}
}