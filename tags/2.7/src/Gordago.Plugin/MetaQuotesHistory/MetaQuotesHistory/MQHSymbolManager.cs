using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Gordago.PlugIn.MetaQuotesHistory {
  
  public class MQHSymbolManager {

    private MQHSymbol[] _symbols;
    
    /// <summary>
    /// Коллекция символов Gordago
    /// </summary>
    private ISymbolList _gSymbols;

    public MQHSymbolManager(ISymbolList gSymbols, string dirMQHistory) {
      _gSymbols = gSymbols;
      _symbols = new MQHSymbol[gSymbols.Count];
      for(int i = 0; i < gSymbols.Count; i++) {
        _symbols[i] = new MQHSymbol(gSymbols[i]);
      }

      CheckDir(dirMQHistory + "\\tmp.tmp");
      string[] files = Directory.GetFiles(dirMQHistory, "*.hst");
      foreach(string file in files) {
        MQHBarReader barReader = new MQHBarReader(file, this);
      }
    }

    #region public MQHSymbol this[int index]
    public MQHSymbol this[int index] {
      get { return _symbols[index]; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _symbols.Length; }
    }
    #endregion

    #region public MQHSymbol GetSymbol(string symbolname)
    public MQHSymbol GetSymbol(string symbolname) {
      foreach(MQHSymbol symbol in _symbols) {
        if(symbol.Name == symbolname)
          return symbol;
      }
      return null;
    }
    #endregion

    #region public static void CheckDir(string filename)
    public static void CheckDir(string filename) {
      string[] da = filename.Split(new char[] { '\\' });
      string bpath = da[0];
      for(int i = 1; i < da.Length - 1; i++) {
        bpath += "\\" + da[i];
        if(!Directory.Exists(bpath))
          Directory.CreateDirectory(bpath);
      }
    }
    #endregion
  }
}
