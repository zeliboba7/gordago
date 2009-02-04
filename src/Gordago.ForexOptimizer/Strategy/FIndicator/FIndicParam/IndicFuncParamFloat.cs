/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Cursit.Applic.APropGrid;
using Gordago.Analysis;

namespace Gordago.Strategy.FIndicator.FIndicParam {

	public class IndicFuncParamFloat : IndicFuncParam{
		private float _min;
		private float _max;
		private int _decimalPlaces;
		private float _step;

		public IndicFuncParamFloat(Parameter param):base(param) {
			ParameterFloat fparam = param as ParameterFloat;
			this.Value = (float)fparam.Value;
			
			_min = fparam.Minimum;
			_max = fparam.Maximum;
			
			this._decimalPlaces = fparam.Point;
			this._step = fparam.Step;
		}

		#region public static float ConvertFromString(string value)
		public static float ConvertFromString(string value){
			return Convert.ToSingle(ConvertPoint(value));
		}
		#endregion

		private static string ConvertPoint(string str){
			return str.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
		}

		#region public new float Value 
		public new float Value {
			get {
				return (float)base.Value;
			}
			set{
				base.Value = (float)value;
			}
		}
		#endregion

		#region public float Max
		/// <summary>
		/// Максимальное значение
		/// </summary>
		public float Max{get{return _max;}}
		#endregion

		#region public float Min
		/// <summary>
		/// Минимальное значение
		/// </summary>
		public float Min{
			get{return _min;}
		}
		#endregion

		#region public override string ToString()
		public override string ToString() {
			string decPoint = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			string val = Convert.ToString(this.Value).Replace(decPoint, ".");
			if (val.IndexOf(".") < 0){
				val = val + ".0";
			}
			return val;
		}
		#endregion

		#region public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType)
		public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType) {
			return new PropGridValueFloat(this.Caption, this.Value, this.Min, this.Max, this._decimalPlaces, this._step);
		}
		#endregion

		#region public override string[] GetEditorString()
		public override string[] GetEditorString() {
			return new string[]{this.ToString(), base.Parameter.Name + ": " + this.ToString()};
		}
		#endregion 

		internal override string GetOptimizerString(int timeFrame, string prefix, TradeVariables tvars) {
			this.CompVar = null;
			string str = this.ToString();
			this.ReportString = str;
			return str;
		}

		internal override object GetOptimizerValue() {return null;}
		internal override void SetOptimizerValue(object values) {}
	}
}
