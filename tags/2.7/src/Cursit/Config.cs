/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Microsoft.Win32;

namespace Gordago.GConfig
{
	/// <summary>
	/// Файл конфигурации
	/// </summary>
	public class Config
	{
		//public static readonly string Version = "2.0";
		public static ConfigValue Users = new ConfigValue("Gordago");
		public static ConfigValue Global = new ConfigValue("Gordago");

		public static void LoadGlobal(string fileName){

		}

		/// <summary>
		/// Загрузка занчения в пользовательские настройки
		/// </summary>
		public static void LoadUsers(){
			LoadConfValues("Software\\Gordago", Users);
		}
		private static void LoadConfValues(string keys, ConfigValue vals){
			RegistryKey regKey = null;
			try{
				regKey = Registry.CurrentUser.OpenSubKey(keys);
				if (regKey != null){
					String[] valueNames  = regKey.GetValueNames();
					foreach(String valueName in valueNames){
						object obj = regKey.GetValue(valueName);
						//System.Diagnostics.Debug.WriteLine("type = " + obj.GetType().Name + " = " + obj);
						if (obj is string)
							vals[valueName].SetValue((String)obj);
						else if(obj is int)
							vals[valueName].SetValue((int)obj);
						//System.Diagnostics.Debug.WriteLine(valueName);
						//System.Diagnostics.Debug.WriteLine(regKey.GetValue(valueName));
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

		public static void SaveUsers(){
			SaveConfValues("Software", Config.Users);
		}

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
	}
}
