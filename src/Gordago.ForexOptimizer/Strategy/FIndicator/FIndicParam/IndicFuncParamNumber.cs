/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Cursit.Applic.APropGrid;
using Gordago.Analysis;

namespace Gordago.Strategy.FIndicator.FIndicParam {
	public class IndicFuncParamNumber: IndicFuncParam {
		private int _min;
		private int _max;
		private float _step;
		private int[] _optvalue;

		public IndicFuncParamNumber(Parameter param):base(param) {
			ParameterInteger iparam = param as ParameterInteger;
			_min = iparam.Minimum;
			_max = iparam.Maximum;
			_step = iparam.Step;
			this.Value = iparam.Value;
		}

		#region public new int Value 
		public new int Value {
			get {
				if (base.Value == null)
					base.Value = 0;
				return (int)base.Value;
			}
			set{
				base.Value = value;
			}
		}
		#endregion

		#region public int Max
		/// <summary>
		/// Максимальное значение
		/// </summary>
		public int Max{
			get{return _max;}
		}
		#endregion

		#region public int Min
		/// <summary>
		/// Минимальное значение
		/// </summary>
		public int Min{
			get{return _min;}
		}
		#endregion

		#region public float Step
		public float Step{
			get{return this._step;}
			set{this._step = value;}
		}
		#endregion

		#region internal override object GetOptimizerValue() 
		internal override object GetOptimizerValue() {
			return this._optvalue;
		}
		#endregion

		#region internal override void SetOptimizerValue(object values) 
		internal override void SetOptimizerValue(object values) {
			if (values is int[]){
				int[] inta = (int[])values;
				if (inta.Length == 0){
					this._optvalue = null;
					return;
				}
				if (inta.Length == 2 && inta[0] == inta[1]){
					this._optvalue = null;
					this.Value = inta[0];
					return;
				}
				_optvalue = inta;
			}else{
				this._optvalue = null;
			}
		}
		#endregion

		#region public override string[] GetEditorString()
		public override string[] GetEditorString() {
			string str = "";
			if (_optvalue != null){
				int v1 = _optvalue[0];
				int v2 = _optvalue[1];
				str += "["+v1.ToString()+";"+v2.ToString()+"]";
			}else{
				str = this.ToString(); 
			}
			return new string[]{str, Parameter.Name + ": " + str};
		}
		#endregion

		#region public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType)
		public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType) {
			if (iiboxType == IndicatorInsertBoxType.Strategy){
				if (_optvalue != null){
					int[] vo = _optvalue;
					return new PropGridValuePeriod(this.Caption, vo[0], vo[1], this.Min, this.Max, Convert.ToDecimal(Step));
				}else{
					if (this.Parameter.Name == "__Shift" && IndicatorInsertBoxType.Strategy == iiboxType){
						return new PropGridValueNumber(Language.Dictionary.GetString(9,6,"Bars back"), this.Value, 0, this.Max);
					}else
						return new PropGridValuePeriod(this.Caption, this.Value, this.Value, this.Min, this.Max, Convert.ToDecimal(Step));
				}
			}else{
				PropGridValueNumber pgv = new PropGridValueNumber(this.Caption, this.Value, this.Min, this.Max);
				return pgv;
			}
		}
		#endregion

		#region internal override string GetOptimizerString(int timeFrame, string prefix, Gordago.Strategy.IO.TradeVariables tvars) 
		internal override string GetOptimizerString(int timeFrame, string prefix, TradeVariables tvars) {
			this.CompVar = null;
			if (_optvalue != null){
				string varstr = prefix + "_nbr";
				int v1 = _optvalue[0];
				int v2 = _optvalue[1];

        tvars.Add(new TradeVarInt(varstr, v1, v2, Convert.ToInt32(this.Step)));
				string var = "$"+varstr;

				this.CompVar = new CompVar(var, new string[]{v1.ToString(), v2.ToString()});
				return var;
			}
			string str = this.Value.ToString();
			this.ReportString = str;
			return str;
		}
		#endregion
	}
}

