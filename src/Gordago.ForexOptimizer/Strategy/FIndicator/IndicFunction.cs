/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using 
using System;
using System.Collections;

using Gordago.Strategy.FIndicator.FIndicParam;

using Cursit.Applic.AConfig;

using Gordago.Stock;
using Gordago.Analysis;
using Gordago.Docs;
using Gordago.Strategy.IO;
#endregion

namespace Gordago.Strategy.FIndicator {
	/// <summary>
	/// Функция содержащиеся в индикаторе
	/// </summary>
	public class IndicFunction {

		private Function _function;
		private IndicatorGUI _indicator;

		public IndicFunction(IndicatorGUI indicator, Function function) {
			this._indicator = indicator;
			_function = function;
		}

		public Function Function{
			get{return this._function;}
		}

		#region public IndicatorGUI Parent
		public IndicatorGUI Parent{
			get{return this._indicator;}
		}
		#endregion

		#region public string Name
		public string Name{
			get{return this._function.Name;}
		}
		#endregion

		/// <summary>
		/// Получает две строки для отображения в редакторе на кнопке и ToolTip-е
		/// </summary>
		public string[] GetIFBoxString(){
			string str = this._function.ShortName;
			string strtt = this._function.ShortName;

			Cursit.Text.StringCreater scr = new Cursit.Text.StringCreater();
			Cursit.Text.StringCreater scrtt = new Cursit.Text.StringCreater();
			
			Parameter[] prms = this._function.GetParameters();
			string strs = "";
			foreach (Parameter param in prms){
				if (param.Visible){
					IndicFuncParam ifp = this.Parent.GetFuncParam(param);
					string[] strp = ifp.GetEditorString();
					if (ifp is IndicFuncParamNumber && ifp.Using == DocParameterUsing.Strategy){
						int val = (ifp as IndicFuncParamNumber).Value;
						if (val > 0){
							strs = strp[0];
						}
						scrtt.AppendString(strp[1]);
					}else{
						scr.AppendString(strp[0]);
						scrtt.AppendString(strp[1]);
					}
				}
			}

			IndicFuncParam ifpn = this.Parent.GetFuncParam("__Shift");

			if (ifpn != null){
				int val = (int)ifpn.Value;
				if (val > 0)
					strs = val.ToString();
			}
			if (scr.Count > 0 || strs.Length > 0){
				string sko = scr.GetString(",");
				if (sko.Length > 0){
					sko = " (" + sko + ")"; 
				}
				str += sko + (strs.Length > 0 ? " ["+strs+"]" : "");
				strtt += "\n" + scrtt.GetString("\n");
			}

			return new string[]{str, strtt};
    }

    #region internal string[] GetOptimizerString(int timeFrameId, string prefix, TradeVariables tvars)
    /// <summary>
		/// Формирования строки для использование в оптимизаторе
		/// </summary>
		/// <returns></returns>
		internal string[] GetOptimizerString(int timeFrameId, string prefix, TradeVariables tvars){
			Cursit.Text.StringCreater sc = new Cursit.Text.StringCreater();
			Cursit.Text.StringCreater sccheck = new Cursit.Text.StringCreater();
			int tfminute = timeFrameId;

			Parameter[] prms = this._function.GetParameters();

			int i=0;
			foreach (Parameter param in prms){
				if (param.Name == "__TimeFrame"){
					sc.AppendString(timeFrameId.ToString());
					sccheck.AppendString(timeFrameId.ToString());
				}else{
					IndicFuncParam ifp = this.Parent.GetFuncParam(param);
					if (ifp == null)
						ifp = this.Parent.CreateParam(param);
					
					string s = ifp.GetOptimizerString(tfminute, prefix + "g" + this.Parent.GroupId.ToString() + "v" + i.ToString(), tvars);
					string scheck = Strategy.StrategyCompile.CreateCheckString(s);
					sc.AppendString(s);
					sccheck.AppendString(scheck);
				}
				i++;
			}

			IndicFuncParamNumber ifpn = this.Parent.GetFuncParam("__Shift") as IndicFuncParamNumber;
			int barundo = ifpn != null ? ifpn.Value : 0;

			string strundobar = barundo > 0 ? "[-" + barundo.ToString() + "]" : "";

			string str1 = _function.Name + "(" + sc.GetString(";") + ")" + strundobar;
			string str2 = _function.Name + "(" + sccheck.GetString(";") + ")";
			return new string [] {str1, str2};
    }
    #endregion

    #region internal string GetReportString(int timeFrameId, TradeVariables vars)
    internal string GetReportString(int timeFrameId, TradeVariables vars){
			Cursit.Text.StringCreater sc = new Cursit.Text.StringCreater();
			int tfminute = timeFrameId/60;

			Parameter[] prms = this._function.GetParameters();

			int barundo = 0;
			foreach (Parameter prm in prms){
				IndicFuncParam ifp = this.Parent.GetFuncParam(prm);
				if (prm.Visible){
					if (ifp.CompVar == null){
						sc.AppendString(ifp.ReportString);
					}else{
						int val = GetCompVarValue(ifp.CompVar.VarName, vars);
						if (ifp is IndicFuncParamNumber){
							sc.AppendString(val.ToString());
						}else if(ifp is IndicFuncParamVector){
							sc.AppendString(ifp.CompVar.Vars[val].Substring(0,1));
						}else if (ifp is IndicFuncParamEnum){
							foreach (string s in ifp.CompVar.Vars){
								string[] sa = s.Split(new char[]{'|'});
								int vl = Convert.ToInt32(sa[1]);
								if (vl == val){
									sc.AppendString(sa[0]);
									break;
								}
							}
						}else if (ifp is IndicFuncParamVector){
							sc.AppendString(ifp.CompVar.Vars[val]);
						}
					}
				}
			}

			IndicFuncParamNumber ifpn = this.Parent.GetFuncParam("__Shift") as IndicFuncParamNumber;
			barundo = ifpn != null ? ifpn.Value : 0;

			string str = "";
			
			str = this._function.Name;

			string strundobar = barundo > 0 ? "[-" + barundo.ToString() + "]" : "";
			return str + "("+sc.GetString(";")+")" + strundobar;
    }
    #endregion

    #region internal IndicFunction GetOptimIndicFunction(TradeVariables vars)
    internal IndicFunction GetOptimIndicFunction(TradeVariables vars){
			IndicatorGUI newindic = this.Parent.Clone();
			IndicFunction newindf = null;
			foreach(IndicFunction indf in newindic.IndicFunctions){
				if (indf.Name == this.Name){
					newindf = indf;
					break;
				}
			}

			IndicFuncParams prms = this.Parent.Params;
			IndicFuncParams newprms = newindf.Parent.Params;

			for (int i=0;i<prms.Params.Length;i++){
				IndicFuncParam ifp = prms.Params[i];
				IndicFuncParam newifp = newprms.Params[i];
				if (ifp.CompVar != null){
					int val = GetCompVarValue(ifp.CompVar.VarName, vars);
					if (ifp is IndicFuncParamNumber){
						newifp.Value = val;
						newifp.SetOptimizerValue(null);
					}else if (ifp is IndicFuncParamEnum){
						foreach (string s in ifp.CompVar.Vars){
							string[] sa = s.Split(new char[]{'|'});
							int vl = Convert.ToInt32(sa[1]);
							if (vl == val){
								newifp.Value = val;
								newifp.SetOptimizerValue(null);
								break;
							}
						}
					}else if (ifp is IndicFuncParamVector){
						newifp.Value = ifp.CompVar.Vars[val];
						newifp.SetOptimizerValue(null);
					}
				}
			}
			return newindf;
    }
    #endregion

    #region internal static int GetCompVarValue(string var, TradeVariable[] tvs)
    internal static int GetCompVarValue(string var, TradeVariables vars){
      for(int i = 0; i < vars.Count; i++) {
        if(vars[i].Name == var.Replace("$", "")) {
          return (vars[i] as TradeVarInt);
        }
      }
			return 0;
		}
		#endregion

    #region public Gordago.Strategy.IO.MQL4FuncParse GetMQL4String(int timeframeId)
    public Gordago.Strategy.IO.MQL4FuncParse GetMQL4String(int timeframeId){
			DocProviderFunc provider = null;
			foreach (DocProviderIndic dpi in GordagoMain.ProviderMQL4){
				DocProviderFunc dpf = dpi.GetProvFunction(this._function.GetType().FullName);
				if (dpf != null){
					provider = dpf;
					break;
				}
			}

			if (provider == null) return null;

			int tfminute = timeframeId/60;

			string mtfunc = provider.ReplString;
			string[] sa = mtfunc.Split(new char[]{'('});
			if (sa.Length == 0) return null;
			string var = "d"+sa[0];

			mtfunc = mtfunc.Replace("%TIMEFRAME%", tfminute.ToString());

			Parameter[] prms = this._function.GetParameters();

			IndicFuncParamNumber ifpn = (this.Parent.GetFuncParam("__Shift") as IndicFuncParamNumber);

			int barundo = 0;
			if (ifpn != null)
				barundo = ifpn.Value;

			mtfunc = mtfunc.Replace("%SHIFT%", barundo.ToString());

      foreach (DocProviderParam pprm in provider.Params){
				string repl = "";
				IndicFuncParam ifp = this.Parent.GetFuncParam(prms[pprm.GSOIndex]);
				switch(pprm.PrmType){
					case DocParameterType.Integer:
						mtfunc = mtfunc.Replace("%" + pprm.MTName + "%", Convert.ToString(ifp.Value));
						break;
					case DocParameterType.Float:
						string valstr = Convert.ToString(ifp.Value);
						string chd = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
						valstr = valstr.Replace(chd, ".");
						mtfunc = mtfunc.Replace("%" + pprm.MTName + "%", valstr);
						break;
					case DocParameterType.Enum:
						int enm = (int)ifp.Value;
						repl = "";
					switch(enm){
						case 0:
							repl = "MODE_SMA";
							break;
						case 1:
							repl = "MODE_EMA";
							break;
						case 2:
							repl = "MODE_LWMA";
							break;
						case 3:
							repl = "MODE_SMMA";
							break;
					}
						mtfunc = mtfunc.Replace("%" + pprm.MTName + "%", repl);
						break;
					case DocParameterType.Vector:
						repl = "";
						IndicFuncParamVector ifpvct = ifp as IndicFuncParamVector;
						string vct = "Close";
						if (ifpvct != null)
						 vct = ifpvct.Value;
					switch(vct){
						case "Close":
							repl = "PRICE_CLOSE";
							break;
						case "Open":
							repl = "PRICE_OPEN";
							break;
						case "High":
							repl = "PRICE_HIGH";
							break;
						case "Low":
							repl = "PRICE_LOW";
							break;
						case "Median":
							repl = "PRICE_MEDIAN";
							break;
						case "Typical":
							repl = "PRICE_TYPICAL";
							break;
						case "Weighted":
							repl = "PRICE_WEIGHTED";
							break;
					}

						mtfunc = mtfunc.Replace("%" + pprm.MTName + "%", repl);
						break;
				}
			}

			Gordago.Strategy.IO.MQL4FuncParse mqp = new Gordago.Strategy.IO.MQL4FuncParse();
			mqp.VarName = var;
			mqp.MTFunc = mtfunc;
			mqp.Provider = provider;
			return mqp;
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return _function.Name;
    }
    #endregion
  }
}
