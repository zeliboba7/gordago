/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Cursit.Utils {
	/// <summary>
	/// Информация о числе с плавающей точкой
	/// </summary>
	public class FloatInfo {

		#region private property
		private float _min;
		private float _max;
		private int _digits;

		private bool _isinit;
		private string _decPoint;

		#endregion

		#region public FloatInfo() 
		public FloatInfo() {
			this.Clear();
		}
		#endregion

		public FloatInfo(float min, float max, int point){
			this.Clear();
			this._min = min;
			this._max = max;
			this._digits = point;
		}

		#region public float Min
		public float Min{
			get{return _min;}
		}
		#endregion

		#region public float Max
		public float Max{
			get{return _max;}
		}
		#endregion

		#region public int Digits - Кол-во знаков после запятой
		/// <summary>
		/// Кол-во знаков после запятой
		/// </summary>
		public int Digits{
			get{return _digits;}
			set{this._digits = value;}
		}
		#endregion

		#region public void Add(float value)
		public void Add(float value){
			if (!_isinit){
				_min = _max = value;
				_isinit = true;
			}else{
				_min = Math.Min(value, _min);
				_max = Math.Max(value, _max);
			}
			string strval = value.ToString();
			int pointpos = strval.IndexOf(this._decPoint, 0);
			if (pointpos < 0) return;
			int rl = strval.Length-1- pointpos;
			this._digits = Math.Max(rl, this._digits);
		}
		#endregion

		#region public void Clear()
		public void Clear(){
			_decPoint = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			this._isinit = false;
			this._max = this._min = float.NaN;
			this._digits = 0;
		}
		#endregion

		#region public float GetScale()
		/// <summary>
		/// Возвращает размер шкалы в еденичном пункте: например для 4234.153 будет 0.001
		/// </summary>
		public float GetScale(){
			if (this.Digits <= 0) return 1;
			string str = GetScaleNumber('1');
			return Convert.ToSingle(str);
		}
		
		#endregion

		#region public string GetScaleNumber(char number)
		public string GetScaleNumber(char number){
			return "0" + this._decPoint + new string('0', this.Digits-1) + new string(number, 1);
		}
		#endregion


		#region public float GetWidthToString(Font fnt)
		/// <summary>
		/// Расычитывает размер 
		/// </summary>
		public float GetWidthToString(Font fnt){
			float val = _max + this.GetScale();
			return TextChartInfo.GetStringLenght(Convert.ToString(val), fnt).Width;
		}
		#endregion

		public float GetWidthToString(Font fnt, int point){
			int v100 = Convert.ToInt32("1" + new string('0', point));

			float val = 1f / v100;
			return TextChartInfo.GetStringLenght(Convert.ToString(val), fnt).Width;
		}

		public float AddPunkts(float value, int pnkt){
			if (this.Digits == 0) return value + pnkt;

			int v100 = Convert.ToInt32("1" + new string('0', this.Digits));
			return ((value * v100)+pnkt) / v100;
		}

		public int GetPunkts(float value){
			int v100 = Convert.ToInt32("1" + new string('0', this.Digits));
			float d = value * v100;
			return Convert.ToInt32(d);
		}

	}
}
