/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Gordago.Strategy.FIndicator;

using Gordago.Docs;

using Cursit.Text;
#endregion

namespace Gordago.Strategy.IO {
	class StrategyExportMQL4 {

		private EditorForm _wf;
		private ArrayList _gvars;
		private DefineIndicator[] _deffuncs;

		private bool _noconverted;
		private string _template;
		private string _tfilename;

		private readonly string CMAGICNUMBER = "%magic_number%";
		private readonly string CACCOUNTNUMBER = " %account_number%";
		private readonly string CBUYSTOPLOSSPOINT = "%buy_stop_loss_point%";
		private readonly string CSELLSTOPLOSSPOINT = "%sell_stop_loss_point%";
		private readonly string CBUYTAKEPROFITPOINT = "%buy_take_profit_point%";
		private readonly string CSELLTAKEPROFITPOINT = "%sell_take_profit_point%";
		private readonly string CBUYTRAILINGSTOPPOINT = "%buy_trailing_stop_point%";
		private readonly string CSELLTRAILINGSTOPPOINT = "%sell_trailing_stop_point%";
		private readonly string CLOTSSIZE = "%lots_size%";
		private readonly string CSLIPPAGE = "%slip_page%";
		private readonly string CFLAGUSEHOURTRADE = "%flag_use_hour_trade%";
		private readonly string CFROMHOURTRADE = "%from_hour_trade%";
		private readonly string CTOHOURTRADE = "%to_hour_trade%";
		private readonly string CFLAGUSESOUND = "%flag_use_sound%";
		private readonly string CNAMEFILESOUND = "%sound_file_name%";
		private readonly string CVAREXPRESSION = "%var_expression%";
		private readonly string CBUYOPENEXPRESSION = "%buy_open_expression%";
		private readonly string CSELLOPENEXPRESSION = "%sell_open_expression%";
		private readonly string CBUYCLOSEEXPRESSION = "%buy_close_expression%";
		private readonly string CSELLCLOSEEXPRESSION = "%sell_close_expression%";

		public StrategyExportMQL4(EditorForm wf, MQL4Options mqoptions) {
			_tfilename = Application.StartupPath + "\\mqltemplate\\" + mqoptions.PatternFile;
			if (!System.IO.File.Exists(_tfilename)){
				MessageBox.Show("Error!\n File " + _tfilename + " not found!", GordagoMain.MessageCaption);
				return;
			}

			string line;
			_template = "";
			StreamReader sr =  File.OpenText(_tfilename);
			while ((line=sr.ReadLine())!=null) {
				_template += line + "\n";
			}
			sr.Close();

			_wf = wf;
			_gvars = new ArrayList();
			_deffuncs = new DefineIndicator[]{};

			string exbuyopen = CreateStrategyEnter(_wf.EditBuy, "", 0);
			string exbuyclose = CreateStrategyEnter(_wf.EditBuyExit, "", 0);
			string exsellopen = CreateStrategyEnter(_wf.EditSell, "", 0);
			string exsellclose = CreateStrategyEnter(_wf.EditSellExit, "", 0);

			ReplaceVar(CBUYOPENEXPRESSION, exbuyopen);
			ReplaceVar(CBUYCLOSEEXPRESSION, exbuyclose);
			ReplaceVar(CSELLOPENEXPRESSION, exsellopen);
			ReplaceVar(CSELLCLOSEEXPRESSION, exsellclose);

			string[] vars = (string[])_gvars.ToArray(typeof(string));
			string varexpression = "";
			foreach (string vr in vars){
				varexpression += vr + "\n";
			}

			ReplaceVar(CVAREXPRESSION, varexpression);

			ReplaceVar(CMAGICNUMBER, mqoptions.GetMagic());
			ReplaceVar(CACCOUNTNUMBER, mqoptions.GetAccout());
			ReplaceVar(CLOTSSIZE, mqoptions.Lots);
			ReplaceVar(CSLIPPAGE, mqoptions.SlipPage);
			ReplaceVar(CFLAGUSEHOURTRADE, mqoptions.UseHourTrade);
			ReplaceVar(CFROMHOURTRADE, mqoptions.FromHourTrade);
			ReplaceVar(CTOHOURTRADE, mqoptions.ToHourTrade);
			ReplaceVar(CFLAGUSESOUND, mqoptions.UseSound);
			ReplaceVar(CNAMEFILESOUND, mqoptions.SoundFile);

			ReplaceVar(CBUYSTOPLOSSPOINT, Convert.ToInt32(wf.CheckBuyStop.Value));
			ReplaceVar(CBUYTAKEPROFITPOINT, Convert.ToInt32(wf.CheckBuyLimit.Value));
			ReplaceVar(CBUYTRAILINGSTOPPOINT, Convert.ToInt32(wf.CheckBuyTrail.Value));

			ReplaceVar(CSELLSTOPLOSSPOINT, Convert.ToInt32(wf.CheckSellStop.Value));
			ReplaceVar(CSELLTAKEPROFITPOINT, Convert.ToInt32(wf.CheckSellLimit.Value));
			ReplaceVar(CSELLTRAILINGSTOPPOINT, Convert.ToInt32(wf.CheckSellTrail.Value));
		}

		#region private void ReplaceVar(string var, string val)
		private void ReplaceVar(string var, string val){
			_template = _template.Replace(var, val);
		}
		#endregion

		#region private void ReplaceVar(string var, decimal val)
		private void ReplaceVar(string var, decimal val){
			string d = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			string svar = Convert.ToString(val);
			svar = svar.Replace(d, ".");
			ReplaceVar(var, svar);
		}
		#endregion

		#region private void ReplaceVar(string var, int val)
		private void ReplaceVar(string var, int val){
			this.ReplaceVar(var, Convert.ToString(val));
		}
		#endregion

		#region private void ReplaceVar(string var, bool val)
		private void ReplaceVar(string var, bool val){
			string sval = "";
			if (val)
				sval = "True";
			else
				sval = "False";
			this.ReplaceVar(var, sval);
		}
		#endregion

		#region public bool NoConverted
		public bool NoConverted{
			get{return this._noconverted;}
		}
		#endregion

		#region public DefineIndicator[] DefIndicators
		public DefineIndicator[] DefIndicators{
			get{return _deffuncs;}
		}
		#endregion

		#region public string Template
		public string Template{
			get{return this._template;}
		}
		#endregion

//		#region private string CreateCaption()
//		private string CreateCaption(){
//			string str = 
//				"//+------------------------------------------------------------------+\n"+
//				"//|                           Copyright 2005, Gordago Software Corp. |\n"+
//				"//|                                          http://www.gordago.com/ |\n"+
//				"//+------------------------------------------------------------------+\n\n"+
//				"#property copyright \"Copyright 2005, Gordago Software Corp.\"\n"+
//				"#property link      \"http://www.gordago.com\"\n\n";
//			Random rnd = new Random();
//
//			if (this._mqoptions.UseMagic){
//				str += "#define MAGIC " + this._mqoptions.Magic;
//			}
//			str += "\n\n";
//			return str;
//		}
//		#endregion

//		#region private string CreateExternParam()
//		private string CreateExternParam(){
//			string str = "";
//			if (this._mqoptions.UseApplyAccount){
//				str += InsertLine(0, "extern int Account = "+this._mqoptions.Account+";");
//			}
//			if (_slbuy > 0){
//				str += InsertLine(0, "extern double lStopLoss = " + Convert.ToString(_slbuy) + ";");
//			}
//			if (_slsell > 0){
//				str += InsertLine(0, "extern double sStopLoss = " + Convert.ToString(_slsell) + ";");
//			}
//			if (_tpbuy > 0){
//				str += InsertLine(0, "extern double lTakeProfit = " + Convert.ToString(_tpbuy) + ";");
//			}
//			if (_tpsell > 0){
//				str += InsertLine(0, "extern double sTakeProfit = " + Convert.ToString(_tpsell) + ";");
//			}
//			if (_tsbuy > 0){
//				str += InsertLine(0, "extern double lTrailingStop = " + Convert.ToString(_tsbuy) + ";");
//			}
//			if (_tssell > 0){
//				str += InsertLine(0, "extern double sTrailingStop = " + Convert.ToString(_tssell) + ";");
//			}
//			string overparam = 
//				"extern color clOpenBuy = Blue;\n"+
//				"extern color clCloseBuy = Aqua;\n"+
//				"extern color clOpenSell = Red;\n"+
//				"extern color clCloseSell = Violet;\n"+
//				"extern color clModiBuy = Blue;\n"+
//				"extern color clModiSell = Red;\n"+
//				"extern string Name_Expert = \"Generate from Gordago\";\n"+
//				"extern int Slippage = " + this._mqoptions.SlipPage.ToString() + ";\n";
//
//			if (this._mqoptions.UseHourTrade){
//				string fromhourtrade = Convert.ToString(this._mqoptions.FromHourTrade);
//				string tohourtrade = Convert.ToString(this._mqoptions.ToHourTrade);
//				overparam += 
//					"extern bool UseHourTrade = True;\n"+
//					"extern int FromHourTrade = "+ fromhourtrade+";\n"+
//					"extern int ToHourTrade = "+ tohourtrade+";\n"+
//					"";
//			}
//			
//			string trfl = "True";
//			if (!this._mqoptions.UseSound)
//				trfl = "False";
//			overparam += "extern bool UseSound = " + trfl + ";\n";
//			overparam += "extern string NameFileSound = \""+this._mqoptions.SoundFile+"\";\n";
//
//			overparam +=
//				"extern double Lots = " + this._mqoptions.GetLotsToString() + ";\n\n";
//			str += InsertLine(0,overparam);
//			str += InsertLine(0,"void deinit() {");
//			str += InsertLine(1,"Comment(\"\");");
//			str += InsertLine(0,"}");
//
//			return str;
//		}
//		#endregion

//		#region private string CreateBeginInit()
//		private string CreateBeginInit(){
//			string str = 
//				"//+------------------------------------------------------------------+\n"+
//				"//|                                                                  |\n"+
//				"//+------------------------------------------------------------------+\n"+
//				InsertLine(0,"int start(){");
//
//			if (this._mqoptions.UseHourTrade){
//				str +=
//					InsertLine(1, "if (UseHourTrade){")+
//					InsertLine(2, "if (!(Hour()>=FromHourTrade && Hour()<=ToHourTrade)) {")+
//					InsertLine(3, "Comment(\"Time for trade has not come else!\");")+
//					InsertLine(3, "return(0);")+
//					InsertLine(2, "} else Comment(\"\");")+
//					InsertLine(1, "}else Comment(\"\");");
//			}
//
//			str +=
//				InsertLine(1,"if(Bars<100){")+
//				InsertLine(2,"Print(\"bars less than 100\");")+
//				InsertLine(2,"return(0);")+
//				InsertLine(1,"}");
//
//			if (this._mqoptions.UseApplyAccount){
//				str +=
//					InsertLine(1, "if (Account != AccountNumber()){")+
//					InsertLine(2, "Comment(\"Trade on account :\"+AccountNumber()+\" FORBIDDEN!\");")+
//					InsertLine(2, "return(0);")+
//					InsertLine(1, "}else {Comment(\"\");}");
//			}
//			return str;
//		}
//		#endregion

		#region private string CreateStrategyEnter(SEditorVariants seVariants, string action, int lft)
		private string CreateStrategyEnter(SEditorVariants seVariants, string action, int lft){
			int cntel = 0;
			StringCreater stctbl = new StringCreater();
			foreach (SEditorTable tbl in seVariants.SETables){
				Cursit.Text.StringCreater stcrow = new StringCreater();
				foreach (SEditorRow row in tbl.SERows){
					string srow = "";
					object[] objs = row.TextBoxObject.GetElementsFromType();
					if (objs != null && row.Status == SEditorRowCompileStatus.Compile)
						foreach (object obj in objs){
							if (obj is string){
								string sch = (string)obj;
								srow += sch.Replace("=", "==");
								srow = srow.Replace("<==", "<=");
								srow = srow.Replace(">==", ">=");
							}else if(obj is Control){
								IndicFunction indf = (obj as IndicFunctionBox).IndicFunction;
								MQL4FuncParse mqfp = indf.GetMQL4String(row.TimeFrameId);


								string varname = "";
								string rettype = "";
								string mtfunc = "";

								if (mqfp == null || mqfp.Provider.NoConverted){
									varname = "diError";
									rettype = "double";
									mtfunc = "Error()";
									this._noconverted = true;
								}else{
									varname = mqfp.VarName;
									rettype = mqfp.Provider.FuncRetType;
									mtfunc = mqfp.MTFunc;

								}
								varname += _gvars.Count;
								
								_gvars.Add(rettype + " " + varname + "=" + mtfunc + ";");
								srow += varname;

								if (mqfp == null)
									throw(new Exception("Provider for " + indf.Parent.Indicator.Name + " not found!"));

								if (mqfp.Provider.ProvIndicator.DefineIndicator.Length > 1){

									bool flagfind = false;
									string deffuncname = indf.Parent.Indicator.Name;
									foreach(DefineIndicator deffuncs in this._deffuncs){
										if (deffuncs.Name == deffuncname){
											flagfind = true;
											break;
										}
									}
									if (!flagfind){
										ArrayList al = new ArrayList(this._deffuncs);
										al.Add(new DefineIndicator(deffuncname, mqfp.Provider.ProvIndicator.DefineIndicator));
										this._deffuncs = (DefineIndicator[])al.ToArray(typeof(DefineIndicator));
									}
								}
							}
							cntel++;
						}
					if (srow.Length > 0)
						stcrow.AppendString(srow);
				}
				string s = stcrow.GetString(" && ");
				s = s.Replace("|", " || ");
				if (s.Length > 0)
					stctbl.AppendString("(" + s + ")");
			}

			if (stctbl.Count < 1)
				return "False";

			return stctbl.GetString(" || ");
//			str += InsertLine(2+lft, "if (" + stctbl.GetString(" || ") + "){");
//
//			switch(action){
//				case "buy":
//					if (cntel == 0){
////						_buy = false;
//						return "";
//					}
//					str += 
//						InsertLine(3+lft, "OpenBuy();") +
//						InsertLine(3+lft,"return(0);");
//					break;
//				case "sell":
//					if (cntel == 0){
////						_sell = false;
//						return "";
//					}
//					str += 
//						InsertLine(3+lft, "OpenSell();") +
//						InsertLine(3+lft,"return(0);");
//					break;
//				case "buyexit":
//					if (cntel == 0){
//						_buyexit = false;
//						return "";
//					}
//					str += InsertLine(3+lft, "CloseBuy();")+
//						InsertLine(3+lft, "return(0);");
//					break;
//				case "sellexit":
//					if (cntel == 0){
//						_sellexit = false;
//						return "";
//					}
//					str += InsertLine(3+lft, "CloseSell();")+
//						InsertLine(3+lft, "return(0);");
//					break;
//			}
//			str += InsertLine(2+lft, "}");
//
//			return str;
		}
		#endregion

//		private string CreateOverFunction(){
//			string str = 
//				"bool ExistPositions() {\n"+
//				"	for (int i=0; i<OrdersTotal(); i++) {\n"+
//				"		if (OrderSelect(i, SELECT_BY_POS, MODE_TRADES)) {\n"+
//				"			if (OrderSymbol()==Symbol()" + this.MagicIF() + ") {\n"+
//				"				return(True);\n"+
//				"			}\n"+
//				"		} \n"+
//				"	} \n"+
//				"	return(false);\n"+
//				"}\n";
//
//			if (_tssell > 0){
//				str +=
//					"void TrailingPositionsBuy(int trailingStop) { \n"+
//					"   for (int i=0; i<OrdersTotal(); i++) { \n"+
//					"      if (OrderSelect(i, SELECT_BY_POS, MODE_TRADES)) { \n"+
//					"         if (OrderSymbol()==Symbol()" + this.MagicIF() + ") { \n"+
//					"            if (OrderType()==OP_BUY) { \n"+
//					"               if (Bid-OrderOpenPrice()>trailingStop*Point) { \n"+
//					"                  if (OrderStopLoss()<Bid-trailingStop*Point) \n"+
//					"                     ModifyStopLoss(Bid-trailingStop*Point); \n"+
//					"               } \n"+
//					"            } \n"+
//					"         } \n"+
//					"      } \n"+
//					"   } \n"+
//					"} \n";
//			}
//
//			if (_tssell > 0){
//				str +=
//					"void TrailingPositionsSell(int trailingStop) { \n"+
//					"   for (int i=0; i<OrdersTotal(); i++) { \n"+
//					"      if (OrderSelect(i, SELECT_BY_POS, MODE_TRADES)) { \n"+
//					"         if (OrderSymbol()==Symbol()" + this.MagicIF() + ") { \n"+
//					"            if (OrderType()==OP_SELL) { \n"+
//					"               if (OrderOpenPrice()-Ask>trailingStop*Point) { \n"+
//					"                  if (OrderStopLoss()>Ask+trailingStop*Point || OrderStopLoss()==0)  \n"+
//					"                     ModifyStopLoss(Ask+trailingStop*Point); \n"+
//					"               } \n"+
//					"            } \n"+
//					"         } \n"+
//					"      } \n"+
//					"   } \n"+
//					"} \n";
//			}
//
//			str +=
//				"void ModifyStopLoss(double ldStopLoss) { \n"+
//				"   bool fm;\n"+
//				"   fm = OrderModify(OrderTicket(),OrderOpenPrice(),ldStopLoss,OrderTakeProfit(),0,CLR_NONE); \n"+
//				"   if (fm && UseSound) PlaySound(NameFileSound); \n"+
//				"} \n"+
//				"\n";
//			if (_buyexit){
//				str += "void CloseBuy() { \n"+
//					"   bool fc; \n"+
//					"   fc=OrderClose(OrderTicket(), OrderLots(), Bid, Slippage, clCloseBuy); \n"+
//					"   if (fc && UseSound) PlaySound(NameFileSound); \n"+
//					"} \n";
//			}
//			if (_sellexit){
//				str +=
//					"void CloseSell() { \n"+
//					"   bool fc; \n"+
//					"   fc=OrderClose(OrderTicket(), OrderLots(), Ask, Slippage, clCloseSell); \n"+
//					"   if (fc && UseSound) PlaySound(NameFileSound); \n"+
//					"} \n";
//			}
//			str +=
//				"void OpenBuy() { \n"+
//				"   double ldLot, ldStop, ldTake; \n"+
//				"   string lsComm; \n"+
//				"   ldLot = GetSizeLot(); \n";
//			if (_slbuy > 0)
//				str += "   ldStop = GetStopLossBuy(); \n";
//			else
//				str += "   ldStop = 0; \n";
//
//			if (_tpbuy > 0)
//				str += "   ldTake = GetTakeProfitBuy(); \n";
//			else
//				str += "   ldTake = 0; \n";
//
//			str +=
//				"   lsComm = GetCommentForOrder(); \n"+
//				"   OrderSend(Symbol(),OP_BUY,ldLot,Ask,Slippage,ldStop,ldTake,lsComm," + this.MagicNumber() + ",0,clOpenBuy); \n"+
//				"   if (UseSound) PlaySound(NameFileSound); \n"+
//				"} \n";
//			
//			str +=
//				"void OpenSell() { \n"+
//				"   double ldLot, ldStop, ldTake; \n"+
//				"   string lsComm; \n"+
//				"\n"+
//				"   ldLot = GetSizeLot(); \n";
//			if (_slsell > 0)
//				str += "   ldStop = GetStopLossSell(); \n";
//			else
//				str += "   ldStop = 0; \n";
//			if (_tpsell > 0)
//				str += "   ldTake = GetTakeProfitSell(); \n";
//			else
//				str += "   ldTake = 0; \n";
//
//			str +=
//				"   lsComm = GetCommentForOrder(); \n"+
//				"   OrderSend(Symbol(),OP_SELL,ldLot,Bid,Slippage,ldStop,ldTake,lsComm," + this.MagicNumber() + ",0,clOpenSell); \n"+
//				"   if (UseSound) PlaySound(NameFileSound); \n"+
//				"} \n";
//			
//			str +=
//				"string GetCommentForOrder() { 	return(Name_Expert); } \n"+
//				"double GetSizeLot() { 	return(Lots); } \n";
//			if (_slbuy > 0)
//				str += "double GetStopLossBuy() { 	return (Bid-lStopLoss*Point);} \n";
//			if (_slsell > 0)
//				str += "double GetStopLossSell() { 	return(Ask+sStopLoss*Point); } \n";
//			if (_tpbuy > 0)
//				str += "double GetTakeProfitBuy() { 	return(Ask+lTakeProfit*Point); } \n";
//			if (_tpsell > 0)
//				str += "double GetTakeProfitSell() { 	return(Bid-sTakeProfit*Point); } \n";
//
//			return str;
//		}

//		private string MagicIF(){
//			if (this._mqoptions.UseMagic){
//				return " && OrderMagicNumber()==MAGIC";
//			}
//			return "";
//		}

//		private string MagicNumber(){
//			if (this._mqoptions.UseMagic){
//				return "MAGIC";
//			}
//			return "0";
//		}

//		private static string InsertLine(int level, string str){
//			return (new string(' ', level*3)) + str + "\n";
//		}

//		private string CreateCloseOrder(){
//			string str = "";
//
//			string sbuyexit = CreateStrategyEnter(_wf.SEBuyExit, "buyexit",1);
//
//			if (_buyexit){
//				str += InsertLine(2, "if(OrderType()==OP_BUY){");
//				str += sbuyexit;
//				str += InsertLine(2, "}");
//			}
//
//			string sellexit = this.CreateStrategyEnter(_wf.SESellExit, "sellexit",1);
//			if (_sellexit){
//				str += InsertLine(2, "if(OrderType()==OP_SELL){");
//				str += sellexit;
//				str += InsertLine(2, "}");
//			}
//
//			if (_buyexit || _sellexit){
//				str = InsertLine(1, "if (ExistPositions()){") + 
//					str +
//					InsertLine(1, "}");
//			}
//
//			if (_tsbuy > 0 ){
//				str += InsertLine(1, "TrailingPositionsBuy(lTrailingStop);");
//			}
//
//			if (_tssell > 0){
//				str += InsertLine(1, "TrailingPositionsSell(sTrailingStop);");
//			}
//
//			return str;
//		}
	}

	#region public class DefineIndicator
	public class DefineIndicator{

		#region private property
		private string _name;
		private string _body;
		#endregion

		#region public DefineIndicator(string name, string body)
		public DefineIndicator(string name, string body){
			this._name = name;
			this._body = body;
		}
		#endregion
		
		#region public string Name
		public string Name{
			get{return this._name;}
		}
		#endregion

		#region public string Body
		public string Body{
			get{return this._body;}
		}
		#endregion
	}
	#endregion

	#region public class MQL4FuncParse
	public class MQL4FuncParse{
		private string _varname;
		private string _mtfunc;
		private DocProviderFunc _prov;

		public MQL4FuncParse(){}

		public string VarName{
			get{return this._varname;}
			set{this._varname = value;}
		}
		public string MTFunc{
			get{return this._mtfunc;}
			set{this._mtfunc = value;}
		}

		public DocProviderFunc Provider{
			get{return this._prov;}
			set{this._prov = value;}
		}
	}
	#endregion

	#region public class MQL4Options
	public class MQL4Options{
		private bool _useapplyaccount;
		private bool _usesound;
		private bool _usehourtrade;
		private int _fromhourtrade;
		private int _tohourtrade;
		private int _slippage;
		private decimal _lots;
		private bool _firstcondition;

		private bool _usemagic;
		private string _magic;

		private string _account;
		private string _soundfile;

		private string _patternfile;

		#region public MQL4Options()
		public MQL4Options(){
			_useapplyaccount = false;
			_usehourtrade = false;
			_usesound  = true;
			_account = "";
			_soundfile = "alert.wav";
			_fromhourtrade = 0;
			_tohourtrade = 23;
			this._slippage = 5;
		}
		#endregion

		#region public string PatternFile
		public string PatternFile{
			get{return this._patternfile;}
			set{this._patternfile = value;}
		}
		#endregion

		#region public bool UseMagic
		public bool UseMagic{
			get{return this._usemagic;}
			set{this._usemagic = value;}
		}
		#endregion

		#region public string Magic
		public string Magic{
			get{
				return this._magic;
			}
			set{this._magic = value;}
		}
		#endregion

		#region public string GetMagic()
		public string GetMagic(){
			if (!this._usemagic)
				return "0";
			return _magic;	
		}
		#endregion

		#region public string Account
		public string Account{
			get{return _account;}
			set{this._account = value;}
		}
		#endregion

		#region public string GetAccout()
		public string GetAccout(){
			if (!this._useapplyaccount)
				return "0";
			return _account;
		}
		#endregion

		#region public string SoundFile
		public string SoundFile{
			get{return this._soundfile;}
			set{this._soundfile = value;}
		}
		#endregion

		#region public bool UseApplyAccount
		public bool UseApplyAccount{
			get{return this._useapplyaccount;}
			set{this._useapplyaccount = value;}
		}
		#endregion

		#region public bool UseSound
		public bool UseSound{
			get{return this._usesound;}
			set{this._usesound = value;}
		}
		#endregion

		#region public bool UseHourTrade
		public bool UseHourTrade{
			get{return this._usehourtrade;}
			set{this._usehourtrade = value;}
		}
		#endregion
		
		#region public int FromHourTrade
		public int FromHourTrade{
			get{return this._fromhourtrade;}
			set{this._fromhourtrade = value;}
		}
		#endregion

		#region public int ToHourTrade
		public int ToHourTrade{
			get{return this._tohourtrade;}
			set{this._tohourtrade = value;}
		}
		#endregion

		#region public int SlipPage
		public int SlipPage{
			get{return this._slippage;}
			set{this._slippage = value;}
		}
		#endregion

		#region public decimal Lots
		public decimal Lots{
			get{return this._lots;}
			set{this._lots = value;}
		}
		#endregion

		#region public bool FirstCondition
		public bool FirstCondition{
			get{return this._firstcondition;}
			set{this._firstcondition = value;}
		}
		#endregion

		#region public string GetLotsToString()
		public string GetLotsToString(){
			string dp = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			string tstr = Convert.ToString(this.Lots);
			tstr = tstr.Replace(dp, ".");
			return tstr;
		}
		#endregion

	}
	#endregion
}
