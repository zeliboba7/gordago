/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#endregion

namespace Gordago {

  /// <summary>
  /// Класс управления коллекцией финансовых инструметов
  /// </summary>
	public class SymbolManager:ISymbolList{

		private static string EXTTICKFILE = "gtk";
		private ISymbol[] _symbols;

		private string _dirhistory, _dircache;

		public SymbolManager(string dirhistory, string dircache){
			this.Clear();
			_dirhistory = dirhistory;
			_dircache = dircache;
			this.Preload();
      for(int i = 0; i < _symbols.Length; i++) {
        (_symbols[i].Ticks as TickManager).InitializeBarList();
      }
    }

    #region internal string PathHistoryDir
    internal string PathHistoryDir {
			get{return this._dirhistory;}
    }
    #endregion

    #region internal string PathCacheDir
    internal string PathCacheDir{
			get{return this._dircache;}
    }
    #endregion

    #region public void Clear()
    /// <summary>
    /// Очистка коллекции
    /// </summary>
    public void Clear(){
			_symbols = new Symbol[0];
		}
		#endregion

		#region private void Preload() - Прогрузка информации о сохраненых символах
		/// <summary>
		/// Прогрузка информации о сохраненых символах
		/// </summary>
		private void Preload(){

			if(!Directory.Exists(_dirhistory)) Directory.CreateDirectory(_dirhistory);
			if(!Directory.Exists(_dircache)) Directory.CreateDirectory(_dircache);

			Symbol[] symbols = new Symbol[0];

			string[] files = Directory.GetFiles(_dirhistory, "*." + EXTTICKFILE);
			for (int i=0;i<files.Length;i++)
				symbols = AddFromFile(symbols, new TickFileInfo(files[i]), false);
			
			files = Directory.GetFiles(_dircache, "*." + EXTTICKFILE);
			for (int i=0;i<files.Length;i++)
				symbols = AddFromFile(symbols, new TickFileInfo(files[i]), true);

			foreach (Symbol symbol in symbols)
				this.Add(symbol);
		}
		#endregion

		#region private Symbol[] AddFromFile(Symbol[] symbols, TickFileInfo tfi, bool iscache)
		private Symbol[] AddFromFile(Symbol[] symbols, TickFileInfo tfi, bool iscache){
			Symbol symbol = null;
			foreach (Symbol fsymbol in symbols){
				if (fsymbol.Name == tfi.SymbolName){
					symbol = fsymbol;
					break;
				}
			}
			if (symbol == null){
				symbol = new Symbol(tfi.SymbolName, tfi.DecimalDigits);
        symbol.DirCache = _dircache;
        symbol.DirHistory = _dirhistory;
        (symbol.Ticks as TickManager).InitializeBarList();

				ArrayList al = new ArrayList(symbols);
				al.Add(symbol);
				symbols = (Symbol[])al.ToArray(typeof(Symbol));
			}

      TickManager tmanager = (TickManager)symbol.Ticks;
			
			if (iscache)
        tmanager.SetSFICache(tfi);
			else
        tmanager.SetSFIHistory(tfi);
      
//      tmanager.InitializeSession();

			return symbols;
		}
		#endregion

    #region public ISymbol[] Symbols
    /// <summary>
    /// Массив финансовых инструментов
    /// </summary>
    public ISymbol[] Symbols {
			get{return this._symbols;}
    }
    #endregion

    #region public ISymbol GetSymbol(string symbolname)
    /// <summary>
    /// Получить финансовый инструмент
    /// </summary>
    /// <param name="symbolname">Наименование финансового инструмента</param>
    /// <returns></returns>
    public ISymbol GetSymbol(string symbolname){
			foreach (Symbol symbol in _symbols){
				if (symbol.Name == symbolname)
					return symbol;
			}
			return null;
    }
    #endregion

    #region public ISymbol Add(string name, int decimalDigits)
    /// <summary>
    /// Создать и добавить финансовый инструмент
    /// </summary>
    /// <param name="name">Наименование финансового инструмента</param>
    /// <param name="decimalDigits">Количество знаков после запятой</param>
    /// <returns></returns>
    public ISymbol Add(string name, int decimalDigits) {
      Symbol symbol = new Symbol(name, decimalDigits);
      return Add(symbol);
    }
    #endregion 

    #region public ISymbol Add (ISymbol symbol)

    /// <summary>
    /// Добавление финансового инструмента в коллекцию
    /// </summary>
    /// <param name="symbol">Финансовый инструмент</param>
    /// <returns></returns>
    public ISymbol Add (ISymbol symbol){

			string file = symbol.Name + "." + EXTTICKFILE;
			
			string filehistory = _dirhistory + "\\" + file;
			string filecache = _dircache + "\\" + file;

			(symbol as Symbol).DirCache = this._dircache;
			(symbol as Symbol).DirHistory = this._dirhistory;
      (symbol.Ticks as TickManager).InitializeBarList();

      TickManager tmanager = (TickManager)symbol.Ticks;

      if(tmanager.InfoHistory == null) {
				if (File.Exists(filehistory)){
          tmanager.SetSFIHistory(new TickFileInfo(filehistory));
				}else
          tmanager.SetSFIHistory(new TickFileInfo(symbol, filehistory));
			}
      if(tmanager.InfoCache == null) {
				if (File.Exists(filecache)){
          tmanager.SetSFICache(new TickFileInfo(filecache));
				}else
          tmanager.SetSFICache(new TickFileInfo(symbol, filecache));
			}

			Symbol fsymbol = (Symbol) this.GetSymbol(symbol.Name);
			if (fsymbol != null)
				throw(new Exception("Symbol " + symbol.Name + " already exist in base."));

			ArrayList al = new ArrayList(_symbols);
			al.Add(symbol);
			_symbols = (Symbol[])al.ToArray(typeof(Symbol));


      SortedList<string, ISymbol> sortList = new SortedList<string, ISymbol>();
      for(int i = 0; i < _symbols.Length; i++) {
        sortList.Add(_symbols[i].Name, _symbols[i]);
      }
      sortList.Values.CopyTo(_symbols, 0);
      

      return symbol;
    }
    #endregion

    #region internal static void CheckDir(string filename)
    internal static void CheckDir(string filename){
			string[] da = filename.Split(new char[]{'\\'});
			string bpath = da[0];
			for (int i=1;i<da.Length-1;i++){
				bpath += "\\" + da[i];
				if (!Directory.Exists(bpath))
					Directory.CreateDirectory(bpath);
			}
		}
		#endregion

    #region STATIC FUNCTION

    #region public static int GetDelimiter(int point)
    /// <summary>
    /// Формирует делитель в дробном числе.
    /// Например для числа 1.2053: GetDelimiter(4) = 1000
    /// </summary>
    /// <param name="point">Кол-во знаков после запятой</param>
    /// <returns>делитель</returns>
    public static int GetDelimiter(int decimalDigits) {
      return Convert.ToInt32("1" + new string('0', decimalDigits));
    }
    #endregion

    internal static string GetSymbolName(byte[] bs) {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach(byte b in bs) if((int)b != 0) sb.Append((char)b);
      return sb.ToString();
    }

    #region internal static UInt32 ConvertDateTimeToUnix(DateTime dt)
    internal static UInt32 ConvertDateTimeToUnix(DateTime dt) {
      UInt32 retval = (UInt32)((dt.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks) / 10000000L);
      return retval;
    }
    #endregion

    public static DateTime ConvertUnixToDateTime(double ctm) {
      return new DateTime((long)(ctm * 10000000) + new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks);
    }

    #region public static string ConvertToCurrencyString(float price)
    public static string ConvertToCurrencyString(float price) {
      return ConvertToCurrencyString(price, 2, " ", true);
    }
    #endregion

    #region public static string ConvertToCurrencyString(float price, int decimalDigits)
    public static string ConvertToCurrencyString(float price, int decimalDigits) {
      return ConvertToCurrencyString(price, decimalDigits, " ", true);
    }
    #endregion

    public static string ConvertToCurrencyString(float price, int decimalDigits, bool viewZero) {
      return ConvertToCurrencyString(price, decimalDigits, " ", viewZero);
    }
    public static string ConvertToCurrencyString(float price, int decimalDigits, string space) {
      return ConvertToCurrencyString(price, decimalDigits, space, true);
    }

    #region public static string ConvertToCurrencyString(float price, int decimalDigits, string space, bool viewZero)
    public static string ConvertToCurrencyString(float price, int decimalDigits, string space, bool viewZero) {
      double dprice = 0;
      if(price == 0 || float.IsNaN(price) || float.IsInfinity(price) || float.IsNegativeInfinity(price) || float.IsPositiveInfinity(price)) {
        if(!viewZero) return "";
        dprice = 0;
      } else {
        dprice = Convert.ToDouble(price);
      }
      if(decimalDigits > 0) {
        dprice = Math.Round(dprice, decimalDigits);
      }
      string sprice = dprice.ToString();
      string dp = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      string[] sa = sprice.Split(dp.ToCharArray());

      string fc = "";
      char[] chs = sa[0].ToCharArray();
      int i = 0;
      for(int ii = chs.Length - 1; ii >= 0; ii--) {
        char c = chs[ii];
        if(i == 3 && c != '-') {
          fc = space + fc;
          i = 0;
        }
        fc = c + fc;
        i++;
      }
      if(decimalDigits == 0)
        return fc;

      if(sa.Length == 1)
        sa = new string[] { sa[0], "" };
      sa[1] = sa[1] + new string('0', decimalDigits - sa[1].Length);

      return fc + dp + sa[1];
    }
    #endregion
    #endregion

    #region public ISymbol this[int index]
    /// <summary>
    /// Получить финансовый инструмент
    /// </summary>
    /// <param name="index">Порядковый номер в коллекции</param>
    /// <returns>Финансовый инструмент</returns>
    public ISymbol this[int index] {
      get { return this._symbols[index]; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _symbols.Length;}
    }
    #endregion
  }

	#region public enum OnlineStatus
	public enum OnlineStatus{
		ONLINE,
		OFFLINE
	}
	#endregion
}
