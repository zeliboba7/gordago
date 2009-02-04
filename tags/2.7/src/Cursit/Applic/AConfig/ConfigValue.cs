/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.Drawing;

namespace Cursit.Applic.AConfig {
	/// <summary>
	/// Хранитель настроек
	/// </summary>
	public class ConfigValue {
		private ArrayList vals;
		private string _key;
		private object val = null;

		public ConfigValue(string key){
			val = null;
			init(key);
		}
		public ConfigValue(string key, string value){
			this.SetValue(value);
			init(key);
		}
		public ConfigValue(string key, int value){
			this.SetValue(value);
			init(key);
		}
		public ConfigValue(string key, bool value) {
			this.SetValue(value);
			init(key);
		}
		public ConfigValue(string key, Point value){
			this.SetValue(value);
			init(key);
		}
		public ConfigValue(string key, Size value){
			this.SetValue(value);
			init(key);
		}
		public ConfigValue(string key, byte[] value){
			this.SetValue(value);
			init(key);
		}

		private void init(string key){
			this._key = key;
			vals = new ArrayList();
    }

    #region public string Key
    public string Key{
			get{return this._key;}
    }
    #endregion

    #region public void SetValue(DateTime time)
    public void SetValue(DateTime time) {
      this.SetValue(time.Ticks.ToString());
    }
    #endregion

    public void SetValue(string value){
			this.val = value;
		}

		public void SetValue(int value){
			this.val = value;
		}

		public void SetValue(bool value){
			int n = value ? 1:0;
			this.SetValue(n);
		}

		/// <summary>
		/// Установка значения типа Point.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue(Point point){
			string str = "";
			str = Convert.ToString(point.X) + "," + 
				Convert.ToString(point.Y);
			this.SetValue(str);
		}

		public void SetValue(Size size){
			this.SetValue(new Point(size));
		}

		public void SetValue(byte[] value){
			this.val = value;
		}


		#region public string GetStringValue()
		public string GetStringValue(){
			if (val is string){
				return (string)val;
			}
			return "";
		}
		#endregion

		public int GetIntValue(){
			if (val is int){
				return (int)val;
			}
			throw new ArgumentException("GSO.Config.ConfigValue.GetStringValue() хранимое значения не является int");
		}

		public Point GetPointValue(){
			string sp = this.GetStringValue();
			string[] spa = sp.Split(',');
			Point p = new Point(Convert.ToInt32(spa[0]), Convert.ToInt32(spa[1]));
			return p;
		}

		public Size GetSizeValue(){
			return new Size(this.GetPointValue());
		}

		public byte[] GetBytesValue(){
			if (val is byte[]){
				return (byte[])val;
			}
			return new byte[]{};
		}


		public void Add(string key, string value){
			vals.Add(new ConfigValue(key, value));
		}
		public void Add(string key, int value){
			vals.Add(new ConfigValue(key, value));
		}
		public void Add(string key, bool value){
			vals.Add(new ConfigValue(key, value));
		}
		public void Add(string key, Point value){
			vals.Add(new ConfigValue(key, value));
		}
		public void Add(string key, Size value){
			vals.Add(new ConfigValue(key, value));
		}
		public void Add(string key, byte[] value){
			vals.Add(new ConfigValue(key, value));
		}

		#region private int GetIndexFromKey(string key)
		private int GetIndexFromKey(string key){
			int cnt = vals.Count;
			for (int i=0;i<cnt;i++){
				ConfigValue cfgv = (ConfigValue)vals[i];
				if (cfgv._key == key){
					return i;
				}
			}
			return -1;
		}
		#endregion

		#region public ConfigValue this[string key]
		/// <summary>
		/// Потдерево конфигурации
		/// </summary>
		public ConfigValue this[string key]{
			get{
				ConfigValue cfgv = null;
				int index = this.GetIndexFromKey(key);
				if (index < 0){
					cfgv = new ConfigValue(key);
					vals.Add(cfgv);
				}else{
					cfgv = (ConfigValue)vals[index];
				}
				
				return cfgv;
			}
		}
		#endregion

    #region public string this[string key, string defValue]
    public string this[string key, string defValue]{
			get{
				int index = this.GetIndexFromKey(key);
				ConfigValue cfgval;
				if (index < 0){
					cfgval = new ConfigValue(key, defValue);
					vals.Add(cfgval);
				}else {
					cfgval = (ConfigValue)vals[index];
				}
				return cfgval.GetStringValue();
			}
    }
    #endregion

    #region public int this[string key, int defValue]
    public int this[string key, int defValue]{
			get{
				int index = this.GetIndexFromKey(key);
				ConfigValue cfgval;
				if (index < 0){
					cfgval = new ConfigValue(key, defValue);
					vals.Add(cfgval);
				}else {
					cfgval = (ConfigValue)vals[index];
				}
				return cfgval.GetIntValue();
			}
    }
    #endregion

    #region public bool this[string key, bool defValue]
    public bool this[string key, bool defValue]{
			get{
				int val = this[key, defValue ? 1:0];
				return val == 1 ? true : false;
			}
    }
    #endregion

    #region public Point this[string key, Point defValue]
    public Point this[string key, Point defValue]{
			get{
				int index = this.GetIndexFromKey(key);
				ConfigValue cfgval;
				if (index < 0){
					cfgval = new ConfigValue(key, defValue);
					vals.Add(cfgval);
				}else {
					cfgval = (ConfigValue)vals[index];
				}
				return cfgval.GetPointValue();
			}
    }
    #endregion

    #region public Size this[string key, Size defValue]
    public Size this[string key, Size defValue]{
			get{
				Point p = new Point(defValue);
				return new Size(this[key, p]);
			}
    }
    #endregion

    #region public byte[] this[string key, byte[] defValue]
    public byte[] this[string key, byte[] defValue]{
			get{
				int index = this.GetIndexFromKey(key);
				ConfigValue cfgval;
				if (index < 0){
					cfgval = new ConfigValue(key, defValue);
					vals.Add(cfgval);
				}else {
					cfgval = (ConfigValue)vals[index];
				}
				return cfgval.GetBytesValue();
			}
    }
    #endregion

    public DateTime this[string key, DateTime defValue] {
      get {
        string val = this[key, defValue.Ticks.ToString()];
        DateTime time = new DateTime(Convert.ToInt64(val));
        return time;
      }
    }

    public ConfigValue this[int index]{
			get{return (ConfigValue)vals[index];}
		}

		public bool IsNull(){
			if (val == null) return true;
			return false;
		}
		public bool IsIntType(){
			if (this.val is int) return true;
			return false;
		}

		public bool IsStringType(){
			if (val is string) return true;
			return false;
		}

		public bool IsPointType(){
			if (val is Point) return true;
			return false;
		}

		public bool IsBytesType(){
			if (val is byte[]) return true;
			return false;
		}

		public int Count{
			get{return vals.Count;}
		}
	}
}
