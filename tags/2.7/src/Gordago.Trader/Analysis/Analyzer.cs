/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gordago.Analysis {

  public abstract class Analyzer {

    private ISymbol _symbol;
    private IndicatorManager _imanager;

    private CacheCollection _cache;
    private Dictionary<string, Function> _toolBox;

    private CacheItem _cacheItem = null, _cacheItemCompute = null;

    private bool _isFixedParams = true;

    public Analyzer(IndicatorManager imanager, ISymbol symbol) {
      _imanager = imanager;
      _symbol = symbol;

      _cache = new CacheCollection();
      _toolBox = new Dictionary<string, Function>();
    }

    #region public bool IsFixedParams
    public bool IsFixedParams {
      get { return this._isFixedParams; }
      set { this._isFixedParams = value; }
    }
    #endregion

    #region public IndicatorManager IndicatorManager
    public IndicatorManager IndicatorManager {
      get { return _imanager; }
    }
    #endregion

    #region public FastDictionary Cache
    public CacheCollection Cache {
      get { return _cache; }
    }
    #endregion

    #region public ISymbol Symbol
    public ISymbol Symbol {
      get { return this._symbol; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return this._symbol.DecimalDigits; }
    }
    #endregion

    #region public float Point
    public float Point {
      get { return this._symbol.Point; }
    }
    #endregion

    #region public Function this[string functionName]
    public Function this[string functionName] {
      get {
        Function function = null;
        _toolBox.TryGetValue(functionName.ToLower(), out function);
        return function;
      }
    }
    #endregion

    public abstract IBarList[] BarLists { get;}

    public abstract IBarList GetBars(int second);

    #region internal void Register(Function function)
    internal void Register(Function function) {
      if (function == null)
        throw new ArgumentNullException("Register function error");

      _toolBox.Add(function.Name.ToLower(), function);
    }
    #endregion

    public IVector Compute(Function function, params object[] parameters) {

      CacheItem cacheItem = null;
      if (_cacheItemCompute != null && this.IsFixedParams) {
        if (_cacheItemCompute.SourceCache.Count == 1) {
          cacheItem = _cacheItemCompute.SourceCache[0];
        } else {
          _cacheItemCompute.SourceCache.TryGetValue(function, parameters, out cacheItem);
        }
      } else {
        this.Cache.TryGetValue(function, parameters, out cacheItem);
      }

      #region Инициализация кеша, создание древовидной структуры результатов
      if (cacheItem == null) {

        object[] toprm = new object[parameters.Length];
        string resultKey = function.Name;

        string key = function.Name + "(";

        for (int i = 0; i < parameters.Length; i++) {
          object p = parameters[i];
          //object val = null;
          //if (p is ParameterVector) {
          //  ParameterVector pvector = p as ParameterVector;
          //  val = this.Compute(pvector.Value, pvector.Parameters);
          //} else if (p is Parameter) {
          //  val = (p as Parameter).Value;
          //} else
          //  val = p;

          //toprm[i] = val;

          if (p is BaseVector) {
            CacheItem cacheItemS = this.Cache.GetItem(p as BaseVector);
            key += cacheItemS.Key;
          } else {
            key += p.ToString();
          }
          if (i < parameters.Length - 1)
            key += ";";
        }
        key += ")";

        #region Описание: Здесь создаем родителя...
        /* Здесь создаем родителя, но вектор ему не присваиваем. 
         * Если внутри рассчета будет вызываться этот
         * же метод то будут создаваться исходные вектора 
         * В общем мысль в том, чтобы создать древовидную иерархию CacheItem
         * CacheItem
         *   |
         *   +- CacheItem
         *   +- CacheItem
         *        |
         *        +- CacheItem
         */
        #endregion

        cacheItem = new CacheItem(key, function, parameters, parameters);
        if (_cacheItem != null) 
          _cacheItem.AddSourceCache(cacheItem);

        _cacheItem = cacheItem;

        BaseVector baseResult = function.Compute(this, parameters, null) as BaseVector;

        cacheItem.SetResult(baseResult);
        _cacheItem = null;
        if (cacheItem.Parent != null) {
          _cacheItem = cacheItem.Parent;
        }

        this.Cache.Add(cacheItem);
        return baseResult as IVector;
      }
      #endregion

      if (_cacheItem != null) {
        /* Если текущий кеш есть, значит идет процесс создание кеша */
        _cacheItem.AddSourceCache(cacheItem);
      }

      return this.Compute(cacheItem);
    }

    public IVector Compute(CacheItem cacheItem) {

      BaseVector baseResult = cacheItem.Result;

      IVector result = baseResult as IVector;
      if (baseResult.Blocked)
        return result;

      int deltaCountTick = this.Symbol.Ticks.Count - baseResult.SavedTickCount;

      if (deltaCountTick == 0) {
        if (cacheItem.SourceCache.Count > 0) {
          if (((IVector)cacheItem.SourceCache[0].Result).Count == result.Count)
            return result;
        } else
          return result;
      }

      CacheItem savedCache = _cacheItemCompute;
      _cacheItemCompute = cacheItem;

      /* Проверка, был ли рассчитан последнее значение корректно */
      if (deltaCountTick > 1 && cacheItem.SourceCache.Count > 0 && ((IVector)cacheItem.SourceCache[0].Result).Count > result.Count) {

        int resultCount = result.Count;

        /*  Необходимо заблокировать на пересчет все результаты, 
         * которые имеют полный или частичный рассчет, больше текущей */

        for (int i = 0; i < this.Cache.Count; i++) {
          BaseVector cacheResult = this.Cache[i].Result;
          if ((cacheResult as IVector).Count > resultCount) {
            cacheResult.HideValues(resultCount);
            cacheResult.Blocked = true;
          }
        }

        cacheItem.Function.Compute(this, cacheItem.Parameters, result);

        for (int i = 0; i < this.Cache.Count; i++) {
          BaseVector cacheResult = this.Cache[i].Result;
          cacheResult.ShowValues();
          cacheResult.Blocked = false;
        }
      }

      result = cacheItem.Function.Compute(this, cacheItem.Parameters, result);
      baseResult.SavedTickCount = this.Symbol.Ticks.Count;

      _cacheItemCompute = savedCache;

      return result;
    }

    #region public IVector Compute(string functionName, params object[] parameters)
    public IVector Compute(string functionName, params object[] parameters) {
      Function function = null;

      if (!_toolBox.TryGetValue(functionName.ToLower(), out function)) {
        function = _imanager.GetFunction(functionName);
        if (function == null) {
          return null;
        }
        this.Register(function);
        //return null;
      }
      return Compute(function, parameters);
    }
    #endregion

  }
#if ASDF

        /// <summary>
    /// Результат расчета функции. Класс создан для кеширование рассчетов.
    /// </summary>
    public class Result {

      private IVector _vector = null;
      // private bool _computed = false;
      private object[] _key;
      private Analyzer _analyzer;
      private int _savedTickCount;
      private object[] _parameters;

      public Result(Analyzer analyzer, object[] key, object[] parameters) {
        this._key = key;
        _analyzer = analyzer;
        _parameters = parameters;
      }

      internal void ResetComputeFlag() {
        _savedTickCount = 0;
      }

      #region internal object[] Key
      internal object[] Key {
        get { return _key; }
      }
      #endregion

      #region public override int GetHashCode()
      public override int GetHashCode() {
        int hashCode = 0;
        foreach (object obj in _key)
          hashCode = (hashCode << 8) ^ obj.GetHashCode();
        return hashCode;
      }
      #endregion

      #region public override bool Equals(object obj)
      public override bool Equals(object obj) {
        if (obj == this)
          return true;
        Result result = obj as Result;
        for (int i = 0; i < _key.Length; i++)
          if (!_key[i].Equals(result._key[i]))
            return false;
        return true;
      }
      #endregion

      #region public bool Computed
      public bool Computed {
        get {

          //BaseVector bvector = _vector as BaseVector;
          //if (_savedTickCount != bvector.SavedTickCount) {
          //  _savedTickCount = bvector.SavedTickCount;
          //  return false;
          //}
            
//          return false;
          //if (!_computed) 
          //  return false;

          //if (_analyzer.Symbol.Ticks.Count != _savedTickCount) {
          //  //_computed = false;
          //  _savedTickCount = _analyzer.Symbol.Ticks.Count;
          //  return false;
          //}
          //return _computed;
          return false;
        }
        //set { this._computed = value; }
      }
      #endregion

      #region public IVector Vector
      public IVector Vector {
        get { return _vector; }
        set { _vector = value; }
      }
      #endregion

    }


      //public IVector ComputeOld(Function function, params object[] parameters) {
    //  return ComputeOld(this.GetFunctionCache(function.Name), parameters);
    //}

    //public IVector ComputeOld(string functionName, params object[] parameters) {
    //  return ComputeOld(this.GetFunctionCache(functionName), parameters);
    //}



  #region internal class FunctionCache
  internal class FunctionCache {
		private Function _function;
		private IDictionary _cache = null;

		public FunctionCache(Function function){
			this._function = function;
			_cache = new Hashtable();
		}

		public IDictionary Cache {
			get {return _cache;}
			set {_cache = value;}
		}

  #region public Function Function
		public Function Function{
			get{return this._function;}
		}
    #endregion
  }
  #endregion
#endif
}
