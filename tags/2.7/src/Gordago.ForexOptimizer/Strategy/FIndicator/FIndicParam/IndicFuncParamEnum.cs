/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Gordago.Analysis;
using Cursit.Applic.APropGrid;

namespace Gordago.Strategy.FIndicator.FIndicParam {

	public class IndicFuncParamEnum: IndicFuncParam  {
		
		private string[] _values;
		private int[] _optvalue;

		public IndicFuncParamEnum(Parameter param):base(param) {
			ParameterEnum penum = param as ParameterEnum;
			this._values = penum.EnumValues;
			this.Value = penum.Value;
		}
				
		#region private string GetValueFromNumber(int number)
		private string GetValueFromNumber(int number){
			return _values[number];
		}
		#endregion

		#region private int GetNumberFromValue(string strval)
		private int GetNumberFromValue(string strval){
			int i=0;
			foreach(string str in _values){
				if (str == strval)
					return i;
				i++;
			}
			return -1;
		}
		#endregion

		#region public new int Value
		public new int Value{
			get{return (int)base.Value;}
			set{base.Value = value;}
		}
		#endregion

		#region public string Text
		public string Text{
			get{return this.GetValueFromNumber(this.Value);}
			set{this.Value = this.GetNumberFromValue(value);}
		}
		#endregion

		#region public string[] Values
		public string[] Values{
			get{return _values;}
		}
		#endregion

		internal override object GetOptimizerValue() {
			return _optvalue;
		}

		internal override void SetOptimizerValue(object values) {
			if (values is int[]){
				this._optvalue = (int[])values;

				if (_optvalue.Length <= 1){
					if (_optvalue.Length == 1)this.Value = _optvalue[0];

					this._optvalue = null;
					return;
				}
				int cnt = _optvalue.Length;
				string[] stra = new string[cnt];
				for (int i =0;i<cnt;i++){
					stra[i] = _values[this._optvalue[i]];
				}
			}else if (values is string[]){
				string[] vrs = (string[])values;
				if (vrs.Length <= 1){
					if (vrs.Length == 1) this.Value = this.GetNumberFromValue(vrs[0]);
					this._optvalue = null;
					return;
				}
				int cnt = vrs.Length;
				int[] intvals = new int[cnt];
				for (int i=0;i<cnt;i++){
					intvals[i] = this.GetNumberFromValue(vrs[i]);
				}
				this._optvalue = intvals;
			}else{
				this._optvalue = null;
			}
		}

		#region public override string ToString()
		public override string ToString() {
			return this.Text;
		}
		#endregion

		#region public override string GetEditorString()
		public override string[] GetEditorString() {
			string str = "";
			string strtt = "";

			if (this._optvalue != null){
				Cursit.Text.StringCreater scr = new Cursit.Text.StringCreater();
				Cursit.Text.StringCreater scrtt = new Cursit.Text.StringCreater();
				foreach (int n in _optvalue){
					scr.AppendString(GetValueFromNumber(n).Substring(0,1));
					scrtt.AppendString(GetValueFromNumber(n));
				}
				str = "[" + scr.GetString(";") + "]";
				strtt = "[" + scrtt.GetString(";") + "]";
			}else{
				str = Text.Substring(0,1);
				strtt = Text;
			}
			return new string[]{str, Parameter.Name + ": " + strtt};
		}
		#endregion

		#region public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType)
		public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType) {
			PropGridValueMulti pgv = null;

			if (iiboxType == IndicatorInsertBoxType.Strategy){
				if(this._optvalue != null){
					pgv = new PropGridValueMulti(this.Caption, this.Values, this._optvalue);
				}else{
					pgv = new PropGridValueMulti(this.Caption, this.Values, this.Text);
				}
			}else{
				pgv = new PropGridValueMulti(this.Caption, this.Values, this.Text);
				pgv.MultiRow = false;
			}
			return pgv;
		}
		#endregion
		
		internal override string GetOptimizerString(int timeFrame, string prefix, TradeVariables tvars) {
			this.CompVar  = null;
			if (this._optvalue != null){
				string varstr = prefix + "_enm";
				tvars.Add(new TradeVarInt(varstr, _optvalue));
				string var = "$"+varstr;
				string[] varr = new string[_optvalue.Length];
				for (int i=0;i< _optvalue.Length;i++){
					varr[i] = this.GetValueFromNumber(_optvalue[i]).Substring(0,1) + "|" + _optvalue[i].ToString();
				}
				this.CompVar = new CompVar(var, varr);
				return var;
			}
			this.ReportString = this.GetValueFromNumber(this.Value).Substring(0,1);
			return Convert.ToString(this.Value);
		}
	}
}
