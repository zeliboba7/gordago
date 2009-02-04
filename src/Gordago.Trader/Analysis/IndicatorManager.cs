/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using Gordago.Analysis.Kernel;
using System.Xml;
#endregion
using System.Collections.Generic;
using Gordago.Analysis.Chart;

namespace Gordago.Analysis {
	/// <summary>
	/// Менеджер по работе с индикаторами
	/// </summary>
	public class IndicatorManager {
		private IndicatorImage[] _indimgs;
		private Indicator[] _indicators;
		private Indicator[] _tmpindicators;
		private Function[] _functions;

		#region public IndicatorManager(string indicatorsDirectory) 
		public IndicatorManager(string indicatorsDirectory) {
			string imgpath = indicatorsDirectory + "\\images";
			if (Directory.Exists(imgpath)){
				string[] files = System.IO.Directory.GetFiles(imgpath, "*.gif");
				ArrayList al = new ArrayList();
				foreach (string file in files){
					Image img = Image.FromFile(file);

					al.Add(new IndicatorImage(img, file));
				}
				this._indimgs = (IndicatorImage[])al.ToArray(typeof(IndicatorImage));
			}else
				this._indimgs = new IndicatorImage[0];

			_indicators = new Indicator[]{};
			_tmpindicators = new Indicator[]{};
			_functions = new Function[]{};
			
			Register(new Add()); 
			Register(new Subtract()); 
			Register(new Multiply()); 
			Register(new ScalarMultiply()); 
			Register(new Divide()); 
			Register(new Absolute()); 

			Register(new Open()); 
			Register(new Low()); 
			Register(new High()); 
			Register(new Close()); 
			Register(new Volume()); 
			Register(new Time()); 
			Register(new Median()); 
			Register(new Typical()); 
			Register(new Weighted()); 
			Register(new SwitchFunction());
			Register(new Number());

			this.LoadingFromDirectory(indicatorsDirectory);
			this.InitializationOfIndicators();
#if !DEBUG
      this.SaveInfo(indicatorsDirectory + "\\info.xml");
#endif
    }
		#endregion

		#region public void SaveInfo(string filename)
    public void SaveInfo(string filename) {
      if(this._indicators.Length == 0) return;

      XmlDocument xmldoc = new XmlDocument();
      xmldoc.LoadXml("<Indicators LoadTime=\"" + DateTime.Now.ToString() + "\"></Indicators>");


      for(int i = 0; i < this._indicators.Length; i++) {
        Indicator indicator = _indicators[i];
        XmlNode nd = xmldoc.CreateElement("Indicator");

        SetAttrVal(xmldoc, nd, "Name", indicator.Name);
        SetAttrVal(xmldoc, nd, "ShortName", indicator.ShortName);

        XmlNode ndcop = xmldoc.CreateElement("Copyright");
        ndcop.InnerText = indicator.Copyright + ", " + indicator.Link;
        nd.AppendChild(ndcop);

        SetAttrVal(xmldoc, nd, "Image", indicator.ImageFileName);
        foreach(Function function in indicator.Functions) {
          this.SaveFunction(function, xmldoc, nd);
        }
        xmldoc.DocumentElement.AppendChild(nd);
      }

      XmlNode ndFunc = xmldoc.CreateElement("FreeFunction");
      xmldoc.DocumentElement.AppendChild(ndFunc);

      /* сохранение функций не входящие в состав индикаторов */
      for(int i = 0; i < _functions.Length; i++) {
        Function function = _functions[i];
        bool find = false;
        for(int ii = 0; ii < _indicators.Length; ii++) {
          Indicator indicator = _indicators[ii];
          for(int iii = 0; iii < indicator.Functions.Length; iii++) {
            if(function == indicator.Functions[iii]) {
              find = true;
              break;
            }
          }
          if(find)
            break;
        }
        if(!find) {
          this.SaveFunction(function, xmldoc, ndFunc);
        }
      }

      xmldoc.Save(filename);
    }
		#endregion

    #region private void SaveFunction(Function function, XmlDocument xmldoc,XmlNode nd)
    private void SaveFunction(Function function, XmlDocument xmldoc,XmlNode nd) {
      XmlNode ndf = xmldoc.CreateElement("Function");
      SetAttrVal(xmldoc, ndf, "Name", function.Name);
      foreach(Parameter parameter in function.Parameters) {
        XmlNode ndp = xmldoc.CreateElement("Parameter");

        XmlNode ndchild = xmldoc.CreateElement("Options");
        ndp.AppendChild(ndchild);

        SetAttrVal(xmldoc, ndp, "Name", parameter.Name);

        if(parameter is ParameterEnum) {
          ParameterEnum prmenum = parameter as ParameterEnum;
          SetAttrVal(xmldoc, ndp, "Type", "Enum");
          SetAttrVal(xmldoc, ndchild, "Values", string.Join(", ", prmenum.EnumValues));
        } else if(parameter is ParameterFloat) {
          ParameterFloat prmfloat = parameter as ParameterFloat;
          SetAttrVal(xmldoc, ndp, "Type", "Float");

          SetAttrVal(xmldoc, ndchild, "Min", prmfloat.Minimum.ToString());
          SetAttrVal(xmldoc, ndchild, "Max", prmfloat.Maximum.ToString());
          SetAttrVal(xmldoc, ndchild, "Step", prmfloat.Step.ToString());
          SetAttrVal(xmldoc, ndchild, "Point", prmfloat.Point.ToString());
        } else if(parameter is ParameterInteger) {
          ParameterInteger prminteger = parameter as ParameterInteger;
          SetAttrVal(xmldoc, ndp, "Type", "Integer");
          SetAttrVal(xmldoc, ndchild, "Min", prminteger.Minimum.ToString());
          SetAttrVal(xmldoc, ndchild, "Max", prminteger.Maximum.ToString());
          SetAttrVal(xmldoc, ndchild, "Step", prminteger.Step.ToString());
        } else if(parameter is ParameterVector) {
          ParameterVector prmvector = parameter as ParameterVector;
          SetAttrVal(xmldoc, ndp, "Type", "Vector");
        }
        SetAttrVal(xmldoc, ndp, "Visible", parameter.Visible.ToString());
        SetAttrVal(xmldoc, ndp, "LangCapt", string.Join(", ", parameter.Captions));

        SetAttrVal(xmldoc, ndchild, "Value", parameter.Value.ToString());
        ndf.AppendChild(ndp);
      }
      nd.AppendChild(ndf);
    }
    #endregion

    #region private static void SetAttrVal(XmlDocument xmldoc, XmlNode nd, string name, string val)
    private static void SetAttrVal(XmlDocument xmldoc, XmlNode nd, string name, string val){
			nd.Attributes.Append(xmldoc.CreateAttribute(name));
			nd.Attributes[name].Value = val;
		}
		#endregion

		#region internal IndicatorImage[] IndicatorImage
		internal IndicatorImage[] IndicatorImage{
			get{return this._indimgs;}
		}
		#endregion

		#region public Indicator[] Indicators
		public Indicator[] Indicators{
			get{return this._indicators;}
		}
		#endregion

		#region private void LoadingFromDirectory(string dir)
		private void LoadingFromDirectory(string dir){
      if(!System.IO.Directory.Exists(dir)) return;

			string[] files = Directory.GetFiles(dir, "*.dll");
			foreach (string filename in files){
				try{
					this.LoadingFromFile(filename);
				}catch{
					System.Windows.Forms.MessageBox.Show(filename + " is old version", "Gordago Forex Optimizer TT");
				}
			}
		}
		#endregion

		#region private void LoadingFromFile(string filename)
		private void LoadingFromFile(string filename){
			Type[] types = Assembly.LoadFile(filename).GetTypes();
			ArrayList ali = new ArrayList(this._tmpindicators);
			foreach (Type type in types){
				if (type.BaseType == typeof(Function) && type.IsPublic) {
					Function func = Activator.CreateInstance(type) as Function;
					func.SetIndicatorManager(this);
					this.Register(func);
				}else if (type.BaseType == typeof(Indicator) && type.IsPublic){
					Indicator ind = Activator.CreateInstance(type) as Indicator;
					ind.SetIndicatorManager(this);
					ali.Add(ind);
				}
			}
			this._tmpindicators = (Indicator[])ali.ToArray(typeof(Indicator));
		}
		#endregion

    #region public void Register(Indicator indicator)
    public void Register(Indicator indicator) {
      Function[] func = indicator.Functions;
      for(int i = 0; i < func.Length; i++) {
        func[i].SetIndicatorManager(this);
        this.Register(func[i]);
      }
      indicator.SetIndicatorManager(this);
      List<Indicator> list = new List<Indicator>(this._indicators);
      list.Add(indicator);
      _indicators = list.ToArray();
    }
    #endregion

    #region public void Register(Function function)
    public void Register(Function function) {
			if ( function == null )
				throw new ArgumentNullException("Register function error");

			Type type = function.GetType();
			FunctionAttribute attribute = 
				Attribute.GetCustomAttribute(type, typeof(FunctionAttribute)) as FunctionAttribute;

			string fname = type.Name;
			if ( attribute != null )
				fname = attribute.Name;
			
			function.SetName(fname);
      function.SetIndicatorManager(this);

      for(int i = 0; i < this._functions.Length; i++) {
        if(fname.ToLower() == this._functions[i].Name.ToLower())
          throw new Exception("Function \""+fname+"\" is already registered");
      }

			ArrayList al = new ArrayList(this._functions);
			al.Add(function);
			this._functions = (Function[])al.ToArray(typeof(Function));
		}
		#endregion

		#region public Function GetFunction(string name)
		public Function GetFunction(string name){
			foreach (Function function in this._functions){
				if (function.Name.ToLower() == name.ToLower()){
					if (!function.IsInitialize)
						function.InitializeComponent();
					return function;
				}
			}
			return null;
		}
		#endregion

		#region private void InitializationOfIndicators()
		private void InitializationOfIndicators(){

			foreach (Function function in this._functions){
				function.InitializeComponent();
			}

			ArrayList al = new ArrayList();
			ArrayList alstr = new ArrayList();
			foreach (Indicator ind in this._tmpindicators){
				ind.Initialize();
				ind.FinalizeOfInit();
				if (!ind.IsBad && ind.Name.Length > 0){
					al.Add(ind);
					alstr.Add(ind.Name + " " + ind.GetType().FullName);
				}
			}
			_indicators = (Indicator[])al.ToArray(typeof(Indicator));
			string[] strs = (string[])alstr.ToArray(typeof(string));
			Array.Sort(strs);
			al.Clear();
			foreach (string name in strs){
				foreach (Indicator indicator in _indicators){
					string sf = indicator.Name + " " + indicator.GetType().FullName;
					if (sf == name){
						al.Add(indicator);
						break;
					}
				}
			}
			_indicators = (Indicator[])al.ToArray(typeof(Indicator));
		}
		#endregion

    #region internal Chart.ChartAnalyzer CreateChartAnalyzer(ISymbol symbol)
    internal ChartAnalyzer CreateChartAnalyzer(ISymbol symbol){
			return new ChartAnalyzer(this, symbol);
    }
    #endregion

    #region public Indicator GetIndicator(string typename)
    public Indicator GetIndicator(string typename) {
      foreach(Indicator indicator in this._indicators){
        if(indicator.GetType().FullName == typename)
          return indicator;
      }
      return null;
    }
    #endregion
  }

	#region internal class IndicatorImage
	internal class IndicatorImage{
		private Image _image;
		private string _file;
		public IndicatorImage(Image image, string file){
			_image = image;
			_file = file;
		}
		public Image Image{
			get{return this._image;}
		}

		public string File{
			get{return this._file;}
		}

		public string GetFileName(){
			string[] sa = this._file.Split('\\');
			return sa[sa.Length-1];
		}
	}
	#endregion

}
