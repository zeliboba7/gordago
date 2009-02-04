/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  public class AnalyzerManager {

    private Dictionary<string, Analyzer> _list;
    private IndicatorManager _indicatorManager;
    private ISymbolList _symbols;
    private Type _analyzerType;

    public AnalyzerManager(IndicatorManager indicatorManager, ISymbolList symbols, Type analyzerType) {
      _indicatorManager = indicatorManager;
      _symbols = symbols;
      _list = new Dictionary<string, Analyzer>();
      _analyzerType = analyzerType;
    }

    #region public Analyzer GetAnalyzer(ISymbol symbol)
    public Analyzer GetAnalyzer(ISymbol symbol) {
      string symbolName = symbol.Name.ToUpper();

      Analyzer analyzer = null;
      if (!_list.ContainsKey(symbolName)) {

        ISymbol fsymbol = _symbols.GetSymbol(symbolName);
        if (fsymbol == null)
          throw new Exception("Symbol " + symbolName + " can not be found");

        analyzer = Activator.CreateInstance(_analyzerType, _indicatorManager, fsymbol) as Analyzer;
        _list.Add(fsymbol.Name, analyzer);
      } else {
        analyzer = _list[symbolName];
      }
      return analyzer;
    }
    #endregion

    #region public Vector Compute(string symbolName, string function, params object[] parameters)
    /// <summary>
    /// Расчет функции индикатора
    /// </summary>
    /// <param name="symbol">Инструмент</param>
    /// <param name="function">Наименование функции</param>
    /// <param name="parameters">Параметры для рассчета функции</param>
    /// <returns>Массив рассчитаных значений</returns>
    public IVector Compute(ISymbol symbol, string function, params object[] parameters) {
      Analyzer analyzer = this.GetAnalyzer(symbol);
      return analyzer.Compute(function, parameters);
    }
    #endregion

  }
}
