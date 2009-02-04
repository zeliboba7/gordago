/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using Cursit.Text;
using Cursit.Applic.APropGrid;
using Gordago.Analysis;

namespace Gordago.Strategy.FIndicator.FIndicParam {

	public class IndicFuncParamVector:IndicFuncParam {

		private string[] _optvalue;
		private string[] _values;

		public IndicFuncParamVector(Parameter param):base(param) {
			ParameterVector vparam = param as ParameterVector;
			this.Value = vparam.Value;
			_values = new string[]{"Open","High","Low","Close","Median","Typical","Weighted"};
		}

		#region public new string Value
		public new string Value{
			get{return (string)base.Value;}
			set{base.Value = value;}
		}
		#endregion

		#region public int GetIndexFromValues(string strval)
		public int GetIndexFromValues(string strval){
			for (int i=0;i<this._values.Length;i++){
				if (strval == this._values[i])
					return i;
			}
			return 0;
		}
		#endregion

		#region public string[] Values
		public string[] Values{
			get{return _values;}
			set{_values = value;}
		}
		#endregion

		internal override void SetOptimizerValue(object values) {
			if (values is string[]){
				this._optvalue = (string[])values;
				if (this._optvalue.Length <= 1){
					if (this._optvalue.Length == 1) this.Value = this._optvalue[0];
					this._optvalue = null;
				}
			}	else
				this._optvalue = null;
		}

		#region internal override object GetOptimizerValue() 
		internal override object GetOptimizerValue() {
			return this._optvalue;
		}
		#endregion

		#region public override string GetEditorString()
		public override string[] GetEditorString() {
			string str="";
			string strtt="";
			if (_optvalue != null){
				Cursit.Text.StringCreater scr = new Cursit.Text.StringCreater();
				Cursit.Text.StringCreater scrtt = new Cursit.Text.StringCreater();
				foreach (string s in _optvalue){
					scr.AppendString(s.Substring(0,1));
					scrtt.AppendString(s);
				}
				str = "[" + scr.GetString(";") + "]";
				strtt = "[" + scrtt.GetString(";") + "]";
			}else{
				str = this.Value.Substring(0,1); 
				strtt = this.Value; 
			}
			return  new string[]{str, Parameter.Name + ": " + strtt};
		}
		#endregion

		public static string[] DESCVALUES = 
			new string[]{"Open", "High", "Low", "Close", "Median (HL/2)", "Typical (HLC/3)", "Weighted (HLCC/4)"};


		public static string GetIndicFDescFromName(string val){
			foreach (string s in DESCVALUES){
				if (s.IndexOf(val) > -1)
					return s;
			}
			return "";
		}

		public string GetIndicFNameFromDes(string val){
			foreach (string s in this._values){
				if (val.IndexOf(s) > -1)
					return s;
			}
			return "";
		}

		public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType) {
			string val = GetIndicFDescFromName(Value);
			if (iiboxType == IndicatorInsertBoxType.Strategy){
				PropGridValueMulti pgv = null;
				if (_optvalue != null){
					string[] selvals = new string[_optvalue.Length];
					for(int i=0;i<_optvalue.Length;i++){
						selvals[i] = GetIndicFDescFromName(_optvalue[i]);
					}
					pgv = new PropGridValueMulti(Caption, DESCVALUES, selvals);
				}else{
					pgv = new PropGridValueMulti(Caption, DESCVALUES, val);
				}
				return pgv;
			}else{
				PropGridValueMulti pgvMulti = new PropGridValueMulti(Caption, DESCVALUES, val);
				pgvMulti.MultiRow = false;
				return pgvMulti;
			}
		}

		#region internal override string GetOptimizerString(int timeFrame, string prefix, Gordago.Strategy.IO.TradeVariables tvars) 
		internal override string GetOptimizerString(int timeFrame, string prefix, TradeVariables tvars) {
			string strTF = Convert.ToString(timeFrame);
			string retstr = "";
			this.CompVar = null;

			if (_optvalue != null){
				string varstr = prefix + "_vct";
				int[] vars = new int[_optvalue.Length];
				string[] varrs = new string[vars.Length];

				StringCreater scr = new StringCreater();
				scr.AppendString("$"+varstr);
				int i = 0;
				foreach (string valstr in _optvalue){
					vars[i] = i;
					string valstrc = valstr;
					string s = valstrc+"("+strTF+")";
					varrs[i] = valstrc;
					scr.AppendString(s);
					i++;
				}

        TradeVarInt vv = new TradeVarInt(varstr, vars);
				tvars.Add(vv);

				retstr = "switch("+scr.GetString(";")+")";

				this.CompVar = new CompVar(varstr, varrs);
				return retstr;
			}else
				retstr = this.Value + "("+strTF+")";
			this.ReportString = retstr.Substring(0,1);
			return retstr;
		}
		#endregion
	}
}
