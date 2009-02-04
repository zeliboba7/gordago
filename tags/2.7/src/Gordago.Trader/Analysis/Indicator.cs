/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.Drawing;
#endregion

namespace Gordago.Analysis {
	public abstract class Indicator{

		private Function[] _functions = new Function[]{};
		private FunctionStyle[] _fstyles = new FunctionStyle[]{};
		private IndicatorManager _indman;
		private bool _isbad = false;
		private float _minscalevalue = float.NaN, _maxscalevalue = float.NaN;
		private string _name = "Custom Indicator";
		private string _shortname = "CustInd";
		private string _groupname = "";
		private Image _image;
		private string _imgfilename;
		private Parameter[] _commonparams = new Parameter[]{};

		private string _link, _copyright;
		private bool _isseparatewindow = true;
		private Type _custompitype;

    private int _decimalDigits = 0;
    private bool _enableCustomScale = false;
    private float[] _curstomScaleValues = new float[] { };

		internal void SetIndicatorManager(IndicatorManager indman){
			_indman = indman;
    }

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return this._decimalDigits; }
      set { this._decimalDigits = Math.Max(0, value); }
    }
    #endregion

    #region public bool EnableCustomScale
    public bool EnableCustomScale {
      get { return _enableCustomScale; }
      set { this._enableCustomScale = value; }
    }
    #endregion

    #region public float[] CustomScaleValues
    public float[] CustomScaleValues {
      get { return this._curstomScaleValues; }
      set { this._curstomScaleValues = value; }
    }
    #endregion

    #region internal Type CustomDrawingIndicatorType
    internal Type CustomDrawingIndicatorType{
			get{return this._custompitype;}
    }
    #endregion

    #region public void SetCustomDrawingIndicator(Type type)
    public void SetCustomDrawingIndicator(Type type){
			this._custompitype = type;
    }
    #endregion

    #region public FunctionStyle[] FunctionStyles
    public FunctionStyle[] FunctionStyles{
			get{return this._fstyles;}
		}
		#endregion

		#region public Function[] Functions
		public Function[] Functions{
			get{return this._functions;}
		}
		#endregion

		#region public Image Image
		public Image Image{
			get{return this._image;}
		}
		#endregion

		#region public string ImageFileName
		public string ImageFileName{
			get{return this._imgfilename;}
		}
		#endregion

		#region internal void IsBad
		internal bool IsBad{
			get{return this._isbad;}
		}
		#endregion

		#region public string Name
		public string Name{
			get{return this._name;}
			set{this._name = value;}
		}
		#endregion

		#region public string ShortName
		public string ShortName{
			get{return this._shortname;}
			set{this._shortname = value;}
		}
		#endregion

		#region public string GroupName
		public string GroupName{
			get{return this._groupname;}
			set{this._groupname = value;}
		}
		#endregion

		#region public Parameter[] GetParameters()
		public Parameter[] GetParameters(){
			Parameter[] prms = new Parameter[this._commonparams.Length];
			for (int i=0;i<this._commonparams.Length;i++){
				prms[i] = this._commonparams[i].Clone();
				prms[i].SetFunction(_commonparams[i].Parent);
			}
			return prms;
		}
		#endregion

		public abstract void Initialize();

		#region protected void RegFunction(string name)
		/// <summary>
		/// Регистрация функции в индикаторе
		/// </summary>
		/// <param name="name"></param>
		protected FunctionStyle RegFunction(string name){
			if (this._functions == null)
				throw(new Exception("Error! Function should be used in Initialize()"));
			Function func = _indman.GetFunction(name);
			if (func == null){
				_isbad = true;
			}else{

				ArrayList al = new ArrayList(this._functions);
				al.Add(func);
				this._functions = (Function[])al.ToArray(typeof(Function));

				ArrayList als = new ArrayList(this._fstyles);
				FunctionStyle fstyle = new FunctionStyle(func);
				als.Add(fstyle);
				this._fstyles = (FunctionStyle[])als.ToArray(typeof(FunctionStyle));
				return fstyle;
			}
			return null;
		}
		#endregion

		#region public FunctionStyle GetStyle(Function function)
		public FunctionStyle GetStyle(Function function){
			foreach (FunctionStyle fstyle in this._fstyles){
				if (fstyle.Function == function)
					return fstyle;
			}
			return new FunctionStyle(null);
		}
		#endregion

		#region public void SetImage(string imgfile)
		public void SetImage(string imgfile){
			this._imgfilename = imgfile;
			if (imgfile.Length < 1)
				return;

			foreach (IndicatorImage indimg in this._indman.IndicatorImage){
				if (imgfile == indimg.GetFileName()){
					this._image = indimg.Image;
					return;
				}
			}
		}
		#endregion

		#region public override string ToString() 
		public override string ToString() {
			return this._name;
		}
		#endregion

		#region internal void FinalizeOfInit()
		/// <summary>
		/// подведение итогов инициализации, формирование общих параметров
		/// </summary>
		internal void FinalizeOfInit(){
			foreach (Function function in this._functions){
				foreach (Parameter param in function.Parameters){
					bool lcom = CheckCommonParam(function, param);
					if (lcom)
						AddCommonParam(param);
					else
						AddFuncParam(param);
				}
			}
			ArrayList alc = new ArrayList();
			ArrayList alf = new ArrayList();
			foreach (Parameter param in this._commonparams){
				if (param.Parent == null)
					alc.Add(param);
				else
					alf.Add(param);
			}
			alc.AddRange((Parameter[])alf.ToArray(typeof (Parameter)));
			this._commonparams = (Parameter[])alc.ToArray(typeof(Parameter));
		}
		#endregion

		#region private void AddCommonParam(Parameter param)
		private void AddCommonParam(Parameter param){
			if (FindParam(param))
				return;
			ArrayList al = new ArrayList(this._commonparams);
			al.Add(param.Clone());
			this._commonparams = (Parameter[])al.ToArray(typeof(Parameter));
		}
		#endregion

		#region private void AddFuncParam(Parameter param)
		private void AddFuncParam(Parameter param){
			if (FindParam(param))
				return;
			Parameter prm = param.Clone();
			prm.SetFunction(prm.Parent);

			ArrayList al = new ArrayList(this._commonparams);
			al.Add(prm);
			this._commonparams = (Parameter[])al.ToArray(typeof(Parameter));
		}
		#endregion
		
		#region private bool FindParam(Parameter param)
		private bool FindParam(Parameter param){
			foreach (Parameter fparam in this._commonparams){
				if (fparam.Name.ToLower() == param.Name.ToLower())
					return true;
			}
			return false;
		}
		#endregion

		#region private bool CheckCommonParam(Function func, Parameter param)
		private bool CheckCommonParam(Function func, Parameter param){
			foreach (Function function in this._functions){
				foreach (Parameter fparam in function.Parameters){
					if (fparam.Name == param.Name && fparam.Parent != func)
						return true;
				}
			}
			return true;
		}
		#endregion

		#region public string Link
		public string Link{
			get{return this._link;}
			set{this._link = value;}
		}
		#endregion

		#region public string Copyright
		public string Copyright{
			get{return this._copyright;}
			set{this._copyright = value;}
		}
		#endregion

		#region public bool IsSeparateWindow
		public bool IsSeparateWindow{
			get{return this._isseparatewindow;}
			set{this._isseparatewindow = value;}
		}
		#endregion

		#region public void SetDrawMinimumValueOfScale(float value)
		public void SetDrawMinimumValueOfScale(float value){
			this._minscalevalue = value;
		}
		#endregion

		#region public void SetDrawMaximumValueOfScale(float value)
		public void SetDrawMaximumValueOfScale(float value){
			this._maxscalevalue = value;
		}
		#endregion
	}
}
