/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Collections; 
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Gordago.Analysis {
  #region public class FunctionStyle:ICloneable
  public class FunctionStyle:ICloneable{
		private Function _function;
		private FunctionDrawStyle _drawstyle = FunctionDrawStyle.Line;
		private Pen _pen = new Pen(Color.Green, 1);
		private char _wingdingsChar = (char)74;
		private Font _wingdingsFont = new Font("Wingdings", 10);
		private Pen _selectpen = new Pen(Color.Green, 1);

		public FunctionStyle(Function function){
			_function = function;
		}

		#region internal Function Function
		internal Function Function{
			get{return this._function;}
		}
		#endregion

		#region internal Pen Pen
		internal Pen Pen{
			get{return this._pen;}
		}
		#endregion

		#region internal Pen SelectPen
		internal Pen SelectPen{
			get{return this._selectpen;}
		}
		#endregion

		#region internal Char WingdingsChar
		internal Char WingdingsChar{
			get{return this._wingdingsChar;}
		}
		#endregion

		#region internal FunctionDrawStyle DrawStyle
		internal FunctionDrawStyle DrawStyle{
			get{return this._drawstyle;}
		}
		#endregion

		#region public void SetDrawStyle(FunctionDrawStyle style)
		public void SetDrawStyle(FunctionDrawStyle style){
			this._drawstyle = style;
		}
		#endregion

		#region public void SetDrawPen(Pen pen)
		public void SetDrawPen(Pen pen){
			this._pen = pen;
			this._selectpen = new Pen(pen.Color, 3);
		}
		#endregion

		#region public void SetDrawPen(Color color)
		public void SetDrawPen(Color color){
			SetDrawPen(new Pen(color, 1));
		}
		#endregion

		#region public void SetDrawWindingsChar(int index)
		public void SetDrawWindingsChar(int index){
			_wingdingsChar = (char)Math.Max(Math.Min(index, 0), 255);
		}
		#endregion

    #region public object Clone()
    public object Clone() {
      FunctionStyle fs = new FunctionStyle(this._function);
      fs.SetDrawStyle(_drawstyle);
      fs.SetDrawPen(_pen.Color);
      fs.SetDrawWindingsChar((int)_wingdingsChar);
      return fs;
    }
    #endregion
  }
	#endregion

	public abstract class Function {

		internal const string PNameShift = "__Shift";
		internal const string PNameTimeFrame = "__TimeFrame";
		public const string PName_MAType = "__MAType";
		public const string PName_ApplyTo = "__ApplyTo";

		#region public static Parameter CreateDefParam_MAType(int defValue)
		public static Parameter CreateDefParam_MAType(int defValue){
			string[] sa = new string[]{"Simple","Exponential","Linear Weighted","Smoothed"};
			if (defValue < 0 || defValue > sa.Length-1)
				throw(new Exception("The parameter should be in an interval: 0 - 4"));
			return new ParameterEnum(PName_MAType, new string[]{"Type", "Тип"}, defValue, sa);
		}
		#endregion

		#region public static Parameter CreateDefParam_ApplyTo(string price)
		public static Parameter CreateDefParam_ApplyTo(string price){
			switch(price.ToLower()){
				case "close":
				case "open":
				case "high":
				case "low":
				case "median":
				case "typical":
				case "weighted":
					break;
				default:
					throw(new Exception("Unknow price parameter: " + price));
			}

			return new ParameterVector(PName_ApplyTo, new string[]{"Apply to", "Применить к"}, price);
		}
		#endregion

		private Parameter[] _parameters = new Parameter[]{};
		private IndicatorManager _indicatormanager;

		private Function[] _childfunctions = new Function[]{};
		private string _name;
		private string _shortname = "";

		private bool _isinitialize = false;

#if ASDF
    internal IVector PrivateCompute(Analyzer analyzer, object[] parameters, CacheItem cacheItem) {

      BaseVector baseResult = cacheItem.Result;

      IVector result = baseResult as IVector;
      if (baseResult.Blocked)
        return result;

      int deltaCountTick = analyzer.Symbol.Ticks.Count - baseResult.SavedTickCount;

      if (deltaCountTick == 0) {
        if (cacheItem.SourceCache.Count > 0) {
          if (((IVector)cacheItem.SourceCache[0].Result).Count == result.Count)
            return result;
        }else 
          return result;
      }

      /* Проверка, был ли рассчитан последнее значение корректно */
      if (deltaCountTick > 1 && cacheItem.SourceCache.Count > 0 && ((IVector)cacheItem.SourceCache[0].Result).Count > result.Count) {

        int resultCount = result.Count;

        /*  Необходимо заблокировать на пересчет все результаты, 
         * которые имеют полный или частичный рассчет, больше текущей */

        for (int i = 0; i < analyzer.Cache.Count; i++) {
          BaseVector cacheResult = analyzer.Cache[i].Result;
          if ((cacheResult as IVector).Count > resultCount) {
            cacheResult.HideValues(resultCount);
            cacheResult.Blocked = true;
          }
        }

        this.Compute(analyzer, parameters, result);

        for (int i = 0; i < analyzer.Cache.Count; i++) {
          BaseVector cacheResult = analyzer.Cache[i].Result;
          cacheResult.ShowValues();
          cacheResult.Blocked = false;
        }
      }

      result = this.Compute(analyzer, parameters, result);
      baseResult.SavedTickCount = analyzer.Symbol.Ticks.Count;
      return result;
    }
#endif

    public abstract IVector Compute(Analyzer analyzer, object[] parameters, IVector result);

		#region internal IndicatorManager IndicatorManager
		internal IndicatorManager IndicatorManager{
			get{return this._indicatormanager;}
		}
		#endregion

		#region internal Parameter[] Parameters
		internal Parameter[] Parameters{
			get{return this._parameters;}
		}
		#endregion

		#region public string Name
		public string Name{
			get{return this._name;}
		}
		#endregion

		#region public string ShortName
		public string ShortName{
			get{
				if (this._shortname == "")
					return _name;
				return this._shortname;
			}
		}
		#endregion

		#region internal void SetName(string name)
		internal void SetName(string name){
			this._name = name;
		}
		#endregion

		#region internal bool IsInitialize
		internal bool IsInitialize{
			get{return this._isinitialize;}
			set{this._isinitialize = value;}
		}
		#endregion

		#region internal void InitializeComponent()
		internal void InitializeComponent(){
			if (this._isinitialize)
				return;
			this._isinitialize = true;
			this.Initialize();
		}
		#endregion

		protected abstract void Initialize();

		#region internal void SetIndicatorManager(IndicatorManager indicatorManager)
		internal void SetIndicatorManager(IndicatorManager indicatorManager){
			_indicatormanager = indicatorManager;
		}
		#endregion

		#region public Function GetFunction(string functionName)
		public Function GetFunction(string functionName){
			return this._indicatormanager.GetFunction(functionName);
		}
		#endregion

		#region public Parameter[] GetParameters(Parameter[] parameters)
		public Parameter[] GetParameters(Parameter[] parameters){
			Parameter[] prms = new Parameter[this._parameters.Length];
			int i=0;
			foreach (Parameter tprm in this._parameters){
				Parameter prm = null;
				foreach (Parameter iprm in parameters){
					if (tprm.Name.ToLower() == iprm.Name.ToLower()){
						prm = iprm;
						break;
					}
				}
				if (prm == null)
					throw (new Exception("Parameter " + tprm.Name + " not found in " + this.Name));
				prms[i++] = prm;
			}
			return prms;
		}
		#endregion

		#region public Parameter[] GetParameters()
		public Parameter[] GetParameters(){
			Parameter[] prms = new Parameter[this._parameters.Length];
			for (int i=0;i<this._parameters.Length;i++){
				prms[i] = this._parameters[i].Clone();
				prms[i].SetFunction(this);
			}
			return prms;
		}
		#endregion

		#region public Parameter CloneParameter(string name)
		public Parameter CloneParameter(string name){
			for (int i=0;i<this._parameters.Length;i++){
				if (this._parameters[i].Name.ToLower() == name.ToLower())
					return this._parameters[i].Clone();
			}
			return null;
		}
		#endregion

		#region protected void RegParameter(Parameter parameter)
		protected void RegParameter(Parameter parameter){
			foreach (Parameter prm in this._parameters){
				if (prm.Name == parameter.Name)
					throw (new Exception("Double parameter: " + prm.Name));
			}
			if (parameter is ParameterVector){
				ParameterVector pvector = parameter as ParameterVector;
				Function func = this.GetFunction(pvector.Value);
				func.InitializeComponent();
				pvector.Parameters = func.GetParameters();
			}
			ArrayList al = new ArrayList(this._parameters); 
			parameter.SetFunction(this);
			al.Add(parameter);
			this._parameters = (Parameter[])al.ToArray(typeof(Parameter));
		}
		#endregion

		#region protected void RegParameter(Function function)
		protected void RegParameter(Function function){
			if (this == function)
				throw (new Exception("It is impossible to establish parameters of this function"));
			function.InitializeComponent();

			foreach (Parameter param in function.Parameters){
				if (!(param is ParameterColor))
					this.RegParameter(param.Clone());
			}
		}
		#endregion

		#region protected Parameter GetParameter(string name)
		protected Parameter GetParameter(string name){
			foreach (Parameter param in this.Parameters){
				if (param.Name.ToLower() == name.ToLower())
					return param;
			}
			return null;
		}
		#endregion

		#region protected void SetShortName(string shortname)
		protected void SetShortName(string shortname){
			this._shortname = shortname;
		}
		#endregion
	}

	#region public class FunctionAttribute : Attribute 
	[AttributeUsage(AttributeTargets.Class)]
	public class FunctionAttribute : Attribute {
		private string _name;

		[CLSCompliant(false)]
		public FunctionAttribute(string name) {
			this._name = name;
		}

		public string Name {
			get {return _name;}
		}
	}
	#endregion

	#region public enum FunctionDrawStyle
	public enum FunctionDrawStyle{
		None,
		Line,
		Histogram,
		WingdingsChar,
		Block
	}
	#endregion
}
