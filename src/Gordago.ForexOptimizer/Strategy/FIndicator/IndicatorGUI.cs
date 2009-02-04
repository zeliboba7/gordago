/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

using Gordago.Docs;
using Gordago.Strategy.FIndicator.FIndicParam;
using Language;
using Gordago.Stock;
using Gordago.Analysis;

namespace Gordago.Strategy.FIndicator{
	/// <summary>
	/// Индикатор
	/// </summary>
	public class IndicatorGUI{

		public event EventHandler ParameterChanged;

		private string _lngShift = Dictionary.GetString(9,5,"Сдвиг");
		private string _lngBarsBack = Dictionary.GetString(9,6,"Свечей назад");

		private IndicFunction[] _indFuncs;
		private string _name;
		private WhoIsWho _whoIs;
		private int _groupId;
		private IndicFuncParams _params;
		private Indicator _indicator;
		private Parameter[] _parameters;

		public object _parent;

		public IndicatorGUI(Indicator indicator, Parameter[] parameters, object parent){
			this._indicator = indicator;
			_parent = parent;
			_parameters = parameters;
			_name = _indicator.Name;
			this._whoIs = WhoIsWho.Beginner;
			_params = new IndicFuncParams();

			_indFuncs = new IndicFunction[_indicator.Functions.Length];
			CreateParams();
		}

		#region public object Parent
		public object Parent{
			get{return this._parent;}
		}
		#endregion

		#region public Indicator Indicator
		public Indicator Indicator{
			get{return this._indicator;}
		}
		#endregion

		#region public int GroupId
		public int GroupId{
			get{return this._groupId;}
			set{this._groupId = value;}
		}
		#endregion

		#region internal WhoIsWho WhoIs
		internal WhoIsWho WhoIs{
			get{return _whoIs;}
			set{_whoIs = value;}
		}
		#endregion

		#region private void CreateParams()
		/// <summary>
		/// Создание списка параметров
		/// </summary>
		private void CreateParams(){
			_params = new IndicFuncParams();

			bool findshift = false;
			foreach (Parameter param in _parameters){
				if (param.Visible)
					this._params.Add(this.CreateParam(param));
				if (param.Name == "__Shift")
					findshift = true;
			}

			if (!findshift && this.Indicator.Functions[0].ShortName != "Number")
				this._params.Add(this.CreateParam(this.CreateShiftParameter()));

			int i=0;
			foreach (Function function in this._indicator.Functions)
				_indFuncs[i++] = new IndicFunction(this, function);

			foreach (IndicFuncParam ifprm in this._params.Params)
				ifprm.ParamValue_Changed += new EventHandler(this.OnParameterChanged);
			
		}
		#endregion

		#region public Parameter CreateShiftParameter()
		public Parameter CreateShiftParameter(){
			return new ParameterInteger("__Shift", _lngShift, 0, -10000, 10000);
		}
		#endregion

		#region public IndicFuncParam CreateParam(Parameter param)
		public IndicFuncParam CreateParam(Parameter param){
			
			if (param is ParameterEnum){
				return new IndicFuncParamEnum(param);
			}else if (param is ParameterInteger){
				return new IndicFuncParamNumber(param);
			}else if (param is ParameterVector){
				return new IndicFuncParamVector(param);
			}else if (param is ParameterFloat){
				return new IndicFuncParamFloat(param);
      } else if(param is ParameterColor) {
        return new IndicFuncParamColor(param);
      }
			return null;
		}
		#endregion

		#region public Parameter[] GetParameters()
		public Parameter[] GetParameters(){
			Parameter[] prms = this.Indicator.GetParameters();

			foreach (IndicFuncParam ifp in this.Params.Params){
				foreach (Parameter prm in prms){
					if (ifp.Parameter.Name == prm.Name){
						prm.Value = ifp.Value;
						break;
					}
				}
			}
			return prms;
		}
		#endregion

		#region public int GetShift()
		public int GetShift(){
			foreach (IndicFuncParam ifp in this.Params.Params){
				if (ifp.Parameter.Name == "__Shift"){
					return (int)ifp.Value;
				}
			}
			return 0;
		}
		#endregion

		#region private void OnParameterChanged(object sender, EventArgs e)
		private void OnParameterChanged(object sender, EventArgs e){
			if (this.ParameterChanged != null){
				this.ParameterChanged(sender, e);
			}
		}
		#endregion

		#region public IndicFuncParams Params
		public IndicFuncParams Params{
			get{return this._params;}
		}
		#endregion

		#region public IndicFunction[] IndicFunctions
		public IndicFunction[] IndicFunctions{
			get{return this._indFuncs;}
		}
		#endregion

		#region public IndicFunction GetIndicFunction(string name) - Фнкция по имени
		/// <summary>
		/// Фнкция по имени
		/// </summary>
		public IndicFunction GetIndicFunction(string name){
			int cnt = this._indFuncs.Length;
			for (int i=0;i<cnt;i++){
				if (_indFuncs[i].Name == name)
					return _indFuncs[i];
			}
			return null;
		}
		#endregion 

		#region public IndicFuncParam GetFuncParam(Parameter param)
		public IndicFuncParam GetFuncParam(Parameter param){
			foreach (IndicFuncParam ifp in this._params.Params){
				if (ifp.Parameter.Name.ToLower() == param.Name.ToLower())
					return ifp;
			}
			return null;
		}
		#endregion

		#region public IndicFuncParam GetFuncParam(string prmname)
		public IndicFuncParam GetFuncParam(string prmname){
			foreach (IndicFuncParam ifp in this._params.Params){
				if (ifp.Parameter.Name.ToLower() == prmname.ToLower())
					return ifp;
			}
			return null;
		}
		#endregion

		#region public IndicatorGUI Clone()
		/// <summary>
		/// Клонирование этого индикатора в новый
		/// </summary>
		/// <returns></returns>
		public IndicatorGUI Clone(){
			ArrayList al = new ArrayList();
			foreach (Parameter prm in this._parameters){
				al.Add(prm.Clone());
			}
			IndicatorGUI indic = new IndicatorGUI(this._indicator, (Parameter[])al.ToArray(typeof(Parameter)), this._parent);
			_params.CopyTo(indic.Params);
			return indic;
		}
		#endregion

		#region public string GetChartString()
		public string GetChartString(){
			//string sret = this.Name + ", " + this.Params.GetChartString();
			return this._indicator.Name;
		}
		#endregion

		#region internal enum WhoIsWho - Кто родитель этого индекатора
		/// <summary>
		/// Кто родитель этого индекатора.
		/// Используется для определения метода копирования.
		/// Если родитель первоисточник, то клонирование, если из формы редактора
		/// и прочего, то копирование
		/// </summary>
		internal enum WhoIsWho{
			Beginner,
			Editor
		}
		#endregion

    #region public override string ToString()
    public override string ToString() {
      return this._indicator.Name;
    }
    #endregion
  }
}