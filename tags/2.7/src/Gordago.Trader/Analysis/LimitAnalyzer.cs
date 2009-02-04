/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  /// <summary>
  /// Лимит Анализатор. 
  /// Рассчет функций индикатора ограниченен кол-вом баров с конца 
  /// каждого временного периода.
  /// По умолчанию рассчет производиться не более, чем на 1000 баров.
  /// </summary>
  public class LimitAnalyzer:Analyzer {

    private LimitBars[] _barsCollection;
    public const int LIMIT = 1000;

    public LimitAnalyzer(IndicatorManager imanager, ISymbol symbol) : base(imanager, symbol) {
      _barsCollection = new LimitBars[symbol.Ticks.BarLists.Length];
      for(int i = 0; i < _barsCollection.Length; i++) {
        _barsCollection[i] = new LimitBars(symbol.Ticks.BarLists[i], LIMIT);
      }
    }

    #region public override IBarList GetBars(int second)
    public override IBarList GetBars(int second) {
      for(int i = 0; i < _barsCollection.Length; i++) {
        if(_barsCollection[i].TimeFrame.Second == second)
          return _barsCollection[i];
      }
      return null;
    }
    #endregion

    #region public void SetLimit(int second, int limit)
    /// <summary>
    /// Установка лимита на временной период.
    /// </summary>
    /// <param name="second"></param>
    /// <param name="limit">Лимит баров для рассчета, если -1, то лимит отсутствует</param>
    public void SetLimit(int second, int limit) {
      LimitBars bars = this.GetBars(second) as LimitBars;
      bars.SetLimit(limit);
      this.Cache.Clear();
    }
    #endregion

    #region public override IBarList[] BarLists
    public override IBarList[] BarLists {
      get { return _barsCollection; }
    }
    #endregion
  }

  #region public class LimitBars:IBarList
  public class LimitBars:IBarList {

    private IBarList _bars;
    
    private int _limit;
    private int _beginIndex;

    internal LimitBars(IBarList bars, int limitCount) {
      _bars = bars;
      this.SetLimit(limitCount);
    }

    #region public int BeginIndex
    /// <summary>
    /// Индекс элемента, с которого начинается рассчет
    /// </summary>
    public int BeginIndex {
      get { return _beginIndex; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _bars.Count-_beginIndex; }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get { return _bars[_beginIndex + index]; }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get { return _bars.Current; }
    }
    #endregion

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return _bars.TimeFrame; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { return _bars[_beginIndex].Time; }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { return _bars.TimeTo; }
    }
    #endregion

    #region public int GetBarIndex(DateTime time)
    public int GetBarIndex(DateTime time) {
      return _bars.GetBarIndex(time);
    }
    #endregion

    #region internal void SetLimit(int limitCount)
    internal void SetLimit(int limitCount) {
      _limit = limitCount;
      if(limitCount >= 0)
        _beginIndex = Math.Max(_bars.Count - limitCount, 0);
      else
        _beginIndex = 0;
    }
    #endregion

#if DEBUG
    public int RealIndex(int index) {
      return _beginIndex + index;
    }
#endif
  }
  #endregion

  #region public class LimitAnalyzerManager
  /// <summary>
  /// Менеджер лимит анализатор.
  /// Необходим для рассчета функций индикатора различных валютных пар.
  /// </summary>
  public class LimitAnalyzerManager {
    
    private Dictionary<string, LimitAnalyzer> _list;
    
    private ISymbolList _smanager;
    private IndicatorManager _imanager;

    public LimitAnalyzerManager(IndicatorManager indicatorManager, ISymbolList symbols) {
      _smanager = symbols;
      _imanager = indicatorManager;
      _list = new Dictionary<string, LimitAnalyzer>();
    }

    #region public LimitAnalyzer GetAnalyzer(ISymbol symbol)
    public LimitAnalyzer GetAnalyzer(ISymbol symbol) {
      string symbolName = symbol.Name.ToUpper();

      LimitAnalyzer limitAnalyzer = null;
      if(!_list.ContainsKey(symbolName)) {

        ISymbol fsymbol = _smanager.GetSymbol(symbolName);
        if(fsymbol == null) 
          throw new Exception("Symbol " + symbolName + " can not be found");

        limitAnalyzer = new LimitAnalyzer(_imanager, fsymbol);
        _list.Add(fsymbol.Name, limitAnalyzer);
      } else {
        limitAnalyzer = _list[symbolName];
      }
      return limitAnalyzer;
    }
    #endregion

    #region public IVector Compute(ISymbol symbol, string function, params object[] parameters)
    /// <summary>
    /// Расчет функции индикатора
    /// </summary>
    /// <param name="symbol">Инструмент</param>
    /// <param name="function">Наименование функции</param>
    /// <param name="parameters">Параметры для рассчета функции</param>
    /// <returns>Массив рассчитаных значений</returns>
    public IVector Compute(ISymbol symbol, string function, params object[] parameters) {
      LimitAnalyzer analyzer = this.GetAnalyzer(symbol);
      return analyzer.Compute(function, parameters);
    }
    #endregion

    #region public int TransformToRealIndex(ISymbol symbol, TimeFrame tf, int limitBarIndex)
    /// <summary>
    /// Преобразование позиции индекса из массива рассчитаной функции к 
    /// позиции исходного бара.
    /// </summary>
    /// <param name="symbol">Инструмент</param>
    /// <param name="tf">Временной период в секундах</param>
    /// <param name="limitBarIndex">Позиция в массиве рассчитаной функции</param>
    /// <returns>Позиция индекса в основном массиве баров</returns>
    public int TransformToRealIndex(ISymbol symbol, TimeFrame tf, int limitBarIndex) {
      LimitAnalyzer analyzer = this.GetAnalyzer(symbol);
      LimitBars bars = analyzer.GetBars(tf.Second) as LimitBars;
      return limitBarIndex + bars.BeginIndex;
    }
    #endregion

    #region public int TransformToLimitIndex(ISymbol symbol, TimeFrame tf, int realIndex)
    /// <summary>
    /// </summary>
    /// <param name="symbol"></param>
    /// <param name="tf"></param>
    /// <param name="realIndex"></param>
    /// <returns></returns>
    public int TransformToLimitIndex(ISymbol symbol, TimeFrame tf, int realIndex) {
      LimitAnalyzer analyzer = this.GetAnalyzer(symbol);
      LimitBars bars = analyzer.GetBars(tf.Second) as LimitBars;
      return realIndex - bars.BeginIndex;
    }
    #endregion

    #region public int GetBeginIndex(ISymbol symbol, TimeFrame tf)
    /// <summary>
    /// Получить индекc бара с которого производится рассчет
    /// </summary>
    /// <param name="symbol"></param>
    /// <param name="tf"></param>
    /// <returns></returns>
    public int GetBeginIndex(ISymbol symbol, TimeFrame tf) {
      LimitAnalyzer analyzer = this.GetAnalyzer(symbol);
      LimitBars bars = analyzer.GetBars(tf.Second) as LimitBars;
      return bars.BeginIndex;
    }
    #endregion
  }
  #endregion
}
