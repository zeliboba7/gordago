/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Microsoft.Win32;

namespace Cursit.Applic.AConfig {

	/// <summary>
	/// Файл конфигурации
	/// </summary>
	public class Config{
		//public static readonly string Version = "2.0";
		public static ConfigValue Users = null;
		private static ConfigValue UsersSaved = null;
		public static ConfigValue Global = null;
		private static string _appName;
		private static string _compName;

		public static void Initialize(string compName, string AppName){
			_compName = compName;
			_appName = AppName;

			UsersSaved = new ConfigValue(compName);
			Users = UsersSaved[AppName];
			//Global = new ConfigValue(AppName);
		}

		public static void LoadGlobal(string fileName){
		}

		/// <summary>
		/// Загрузка занчения в пользовательские настройки
		/// </summary>
		public static void LoadUsers(){
			LoadConfValues("Software\\"+_compName , UsersSaved);
		}

		#region private static void LoadConfValues(string keys, ConfigValue vals)
		private static void LoadConfValues(string keys, ConfigValue vals){
			RegistryKey regKey = null;
			try{
				regKey = Registry.CurrentUser.OpenSubKey(keys);
				if (regKey != null){
					String[] valueNames  = regKey.GetValueNames();
					foreach(String valueName in valueNames){
						object obj = regKey.GetValue(valueName);
						if (obj is string)
							vals[valueName].SetValue((String)obj);
						else if(obj is int)
							vals[valueName].SetValue((int)obj);
						else if(obj is byte[])
							vals[valueName].SetValue((byte[])obj);
					}
					String[] subKeyNames = regKey.GetSubKeyNames();
					foreach(String subKeyName in subKeyNames){
						LoadConfValues(keys + "\\" + subKeyName, vals[subKeyName]);
					}
				}
			}finally{
				if (regKey != null){
					regKey.Close();
				}
			}
		}
		#endregion

		public static void SaveUsers(){
			SaveConfValues("Software", Config.UsersSaved);
		}

		#region private static void SaveConfValues(string keys, ConfigValue vals)
		private static void SaveConfValues(string keys, ConfigValue vals){
			/* выгрибаем само значения */
			if (!vals.IsNull()){
				RegistryKey regKey = null;
				try {
					regKey = Registry.CurrentUser.CreateSubKey(keys);

					if (regKey != null) {
						if (vals.IsStringType()){
							regKey.SetValue(vals.Key, vals.GetStringValue());
						}else if(vals.IsIntType()){
							regKey.SetValue(vals.Key, vals.GetIntValue());
						}else if(vals.IsBytesType()){
							regKey.SetValue(vals.Key, vals.GetBytesValue());
						}
					}
				}
				finally {
					if (regKey != null)
						regKey.Close();
				}
			}
			keys += "\\" + vals.Key;

			/* теперь то что в нем содержится */
			int cnt = vals.Count;
			for (int i=0;i<cnt;i++){
				ConfigValue cfgval = (ConfigValue)vals[i];
				SaveConfValues(keys, cfgval);
			}
		}
		#endregion
	}
}
