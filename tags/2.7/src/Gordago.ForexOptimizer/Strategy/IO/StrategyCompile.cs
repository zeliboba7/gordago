#region using
using System;
using System.Collections;
using Gordago.Strategy;
using Gordago.Strategy.FIndicator;

using Cursit.Text;
using Gordago.Analysis;
#endregion

namespace Gordago.Strategy {
	class StrategyCompile {

		private TradeVariables _tradeVariables;
		
		private string _cmplBuyEnter, _cmplBuyExit, _cmplSellEnter, _cmplSellExit;
    private string _cmplPOSell, _cmplPOSellDelete, _cmplPOPriceSell; 
    private string _cmplPOBuy, _cmplPOBuyDelete, _cmplPOPriceBuy;

    private VSVariantList _sellVariantsEntry, _buyVariantsEntry;
    private VSVariantList _sellVariantsCreatePO, _buyVariantsCreatePO;
    private VSVariantList _sellVariantsExit, _buyVariantsExit;
    private VSVariantList _sellVariantsDeletePO, _buyVariantsDeletePO;
    private VSRow _sellPOPrice, _buyPOPrice;

		public StrategyCompile(SEditorVariants buy, SEditorVariants buyExit, 
      SEditorVariants sell, SEditorVariants sellExit,
      SEditorVariants poBuy, SEditorVariants poBuyDelete, POPriceCaclulator poPriceBuy,
      SEditorVariants poSell, SEditorVariants poSellDelete, POPriceCaclulator poPrceSell) {
      
      _sellVariantsEntry = new VSVariantList();
      _sellVariantsExit = new VSVariantList();
      _sellVariantsCreatePO = new VSVariantList();
      _sellVariantsDeletePO = new VSVariantList();

      _buyVariantsEntry = new VSVariantList();
      _buyVariantsExit = new VSVariantList();
      _buyVariantsCreatePO = new VSVariantList();
      _buyVariantsDeletePO = new VSVariantList();

			_tradeVariables = new TradeVariables();

			_cmplSellEnter = this.CompileVariant(sell, "s", _sellVariantsEntry);
			_cmplSellExit = this.CompileVariant(sellExit, "s", _sellVariantsExit);

      _cmplBuyEnter = this.CompileVariant(buy, "b", _buyVariantsEntry);
      _cmplBuyExit = this.CompileVariant(buyExit, "b", _buyVariantsExit);

      _cmplPOBuy = this.CompileVariant(poBuy, "pob", _buyVariantsCreatePO);
      _cmplPOBuyDelete = this.CompileVariant(poBuyDelete, "pob", _buyVariantsDeletePO);
      _cmplPOPriceBuy = this.CompilePrice(poPriceBuy, "pob", out _buyPOPrice);

      _cmplPOSell = this.CompileVariant(poSell, "pos", _sellVariantsCreatePO);
      _cmplPOSellDelete = this.CompileVariant(poSellDelete, "pos", _sellVariantsDeletePO);
      _cmplPOPriceSell = this.CompilePrice(poPrceSell, "pos", out _sellPOPrice);
    }

    #region public VSVariantList SellVariantsEntry
    internal VSVariantList SellVariantsEntry {
      get { return this._sellVariantsEntry; }
    }
    #endregion
    #region public VSVariantList SellVariantsExit
    internal VSVariantList SellVariantsExit {
      get { return this._sellVariantsExit; }
    }
    #endregion
    #region public VSVariantList SellVariantsCreatePO
    public VSVariantList SellVariantsCreatePO {
      get { return this._sellVariantsCreatePO; }
    }
    #endregion
    #region public VSVariantList SellVariantsDeletePO
    public VSVariantList SellVariantsDeletePO {
      get { return this._sellVariantsDeletePO; }
    }
    #endregion
    #region public VSRow SellPOPrice
    public VSRow SellPOPrice {
      get { return this._sellPOPrice; }
    }
    #endregion

    #region public VSVariantList BuyVariantsEntry
    internal VSVariantList BuyVariantsEntry {
      get { return this._buyVariantsEntry; }
    }
    #endregion
    #region public VSVariantList BuyVariantsExit
    internal VSVariantList BuyVariantsExit {
      get { return this._buyVariantsExit; }
    }
    #endregion
    #region public VSVariantList BuyVariantsCreatePO
    public VSVariantList BuyVariantsCreatePO {
      get { return this._buyVariantsCreatePO; }
    }
    #endregion
    #region public VSVariantList BuyVariantsDeletePO
    public VSVariantList BuyVariantsDeletePO {
      get { return this._buyVariantsDeletePO; }
    }
    #endregion
    #region public VSRow BuyPOPrice
    public VSRow BuyPOPrice {
      get { return this._buyPOPrice; }
    }
    #endregion

    #region internal TradeVariables TradeVariables
    internal TradeVariables TradeVariables{
			get{return this._tradeVariables;}
		}
		#endregion

    #region internal string BuyEnter
    internal string BuyEnter{
			get{return _cmplBuyEnter;}
		}
		#endregion

    #region internal string BuyExit
    internal string BuyExit{
			get{return _cmplBuyExit;}
		}
		#endregion

    #region public string BuyCreate
    public string BuyCreate {
      get { return this._cmplPOBuy; }
    }
    #endregion

    #region internal string SellEnter
    internal string SellEnter{
			get{return _cmplSellEnter;}
		}
		#endregion

    #region internal string SellExit
    internal string SellExit{
			get{return this._cmplSellExit;}
		}
		#endregion

    #region public string SellCreate
    public string SellCreate {
      get { return this._cmplPOSell; }
    }
    #endregion

    #region internal static string CreateCheckString(string str)
    internal static string CreateCheckString(string str){
			// для начало избавимся от переменных
			string[] sa = str.Split(new char[]{';'});
			for (int i=0;i<sa.Length;i++){
				if (sa[i].IndexOf("$") >=0){
					string ssa = sa[i];
					int index = ssa.IndexOf("$");
					if (index > 0){
						string ssaa = ssa.Substring(0, index) + "0";
						sa[i] = ssaa;
					}else
						sa[i] = "0";
				}
			}
			return string.Join(";", sa);
		}
		#endregion

		#region internal static int CheckLine(string nexpression) 
		internal static int CheckLine(string nexpression) {
			try {
				Type type = Gordago.Analysis.Vm.Compiler.Expression.Check(nexpression);
				if ( type != typeof(bool) )
					return -2;
			}
			catch ( Gordago.Analysis.Vm.Compiler.CompilerException ex ) {
				return ex.Cursor;
			}
			return -1;
		}
		#endregion

    #region internal static int CheckLinePrice(string nexpression)
    internal static int CheckLinePrice(string nexpression) {
      try {
        Type type = Gordago.Analysis.Vm.Compiler.Expression.Check(nexpression);
        if(type != typeof(float))
          return -2;
      } catch(Gordago.Analysis.Vm.Compiler.CompilerException ex) {
        return ex.Cursor;
      }
      return -1;
    }
    #endregion

    #region private string CompileVariant(SEditorVariants variant, string prefix, VSVariantList variants)
    private string CompileVariant(SEditorVariants variant, string prefix, VSVariantList variants){
			string cstr = "";
			StringCreater scr = new StringCreater();
			int i=0;
			foreach (SEditorTable tbl in variant.SETables){
        VSVariant vsvariant = new VSVariant();
				string s = CreateTestStrategyVariant(tbl, prefix+"v"+i.ToString(), vsvariant);
        if(s.Length > 0) {
          scr.AppendString("(" + s + ")");
          variants.Add(vsvariant);
        }
				i++;
			}
			cstr = scr.GetString("|");
			return cstr;
    }
    #endregion

    #region private string CompilePrice(POPriceCaclulator poprice, string prefix, out VSRow vsrow)
    private string CompilePrice(POPriceCaclulator poprice, string prefix, out VSRow vsrow) {
      string cstr = "";
      vsrow = null;
      string[] strrow = CreateTestStrategyRow(poprice.TextBoxObject, poprice.TimeFrameId, prefix);

      if(strrow.Length > 1) {
        // string[] checkstr = strrow[1].Split(new char[] { '|' });

        int nerror = Math.Min(StrategyCompile.CheckLinePrice(strrow[1]), 0);

        switch(nerror) {
          case -1:
            cstr = strrow[0];
            vsrow = new VSRow(cstr); 
            break;
          case -2:
            poprice.TextBoxObject.SetUnderLineAllElement();
            break;
          default:
            poprice.TextBoxObject.SetUnderLine(nerror);
            break;
        }
      } 
      return cstr;
    }
    #endregion

    #region private string CreateTestStrategyVariant(SEditorTable tbl, string prefix, VSVariant variant)
    private string CreateTestStrategyVariant(SEditorTable tbl, string prefix, VSVariant variant){
			if (tbl.SERows.Length < 1) return "";

			StringCreater scr = new StringCreater();
			
			foreach (SEditorRow row in tbl.SERows){
				row.TextBoxObject.UnSetUnderLineAll();
        
        string[] strrow = new string[] { };
        if(row.Status == SEditorRowCompileStatus.Compile)
          strrow = CreateTestStrategyRow(row.TextBoxObject, row.TimeFrameId, prefix);

				if (strrow.Length > 1){
					string[] checkstr = strrow[1].Split(new char[]{'|'});

					int nerror = 0;

					foreach (string chstr in checkstr)
						nerror = Math.Min(StrategyCompile.CheckLine(chstr), nerror);
					
					switch(nerror){
						case -1:
							string appendstr = "(("+strrow[0]+"))";
							appendstr = appendstr.Replace("|", ")|(");
							scr.AppendString(appendstr);
              
              string newstr = "";
              string[] sa = strrow[0].Split('|');
              if(sa.Length == 1)
                newstr = sa[0];
              else {
                for(int ii = 0; ii < sa.Length; ii++) {
                  sa[ii] = "(" + sa[ii] + ")";
                }
                newstr = string.Join("|", sa);
              }

              variant.Add(new VSRow(newstr));
							break;
						case -2:
							row.TextBoxObject.SetUnderLineAllElement();
							break;
						default:
							row.TextBoxObject.SetUnderLine(nerror);
							break;
					}
				}
			}
			return scr.GetString("&");
    }
    #endregion

    private string[] CreateTestStrategyRow(TextBoxObject tbo, int tfsec, string prefix){
			if (tbo.Elements.Length < 1) return new string[]{};

			StringCreater scr = new StringCreater();
			StringCreater scrcheck = new StringCreater();

			foreach (TextBoxObjElement tbe in tbo.Elements){
				string str = "";
				string strcheck = "";

				if (tbe is TextBoxObjElementChar){
					strcheck = str = new string((tbe as TextBoxObjElementChar).Element,1);

				}else if(tbe is TextBoxObjElementCtrl){
					IndicFunction indf = ((tbe as TextBoxObjElementCtrl).Element as IndicFunctionBox).IndicFunction;
					string[] astr = indf.GetOptimizerString(tfsec, prefix, this._tradeVariables);
					str = astr[0];
					strcheck = astr[1];
				}
				str = str.Replace(",",".");
				strcheck = strcheck.Replace(",", ".");

				scr.AppendString(str);
				scrcheck.AppendString(strcheck);
			}
			string str1 = scr.GetString();
			string str2 = scrcheck.GetString();

			return new string[]{str1, str2};
    }
  }
}


