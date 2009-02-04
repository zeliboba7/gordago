/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {

  public class CacheCollection {
    
    private CacheItem[] _items;
    //private int _countGet = 0;

    public CacheCollection() {
      _items = new CacheItem[] { };
    }

    #region public int Count
    public int Count {
      get { return this._items.Length; }
    }
    #endregion

    #region public CacheItem this[int index]
    public CacheItem this[int index]{
      get {
        return this._items[index];
      }
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _items = new CacheItem[] { };
    }
    #endregion

    #region internal CacheItem Add(CacheItem item)
    internal CacheItem Add(CacheItem item) {
      CacheItem temp = null;
      bool find = this.TryGetValue(item.Key, out temp);

      if (find) {
        return temp;
        // throw(new Exception("Error in CacheCollection"));
      }
      List<CacheItem> list = new List<CacheItem>(_items);
      list.Add(item);
      _items = list.ToArray();
      return item;
    }
    #endregion

    #region public bool TryGetValue(string key, out CacheItem item)
    public bool TryGetValue(string key, out CacheItem item) {
      //_countGet++;
      //if (_countGet == 1000000) {
      //  this.Sort();
      //} else if (_countGet == 5000000) {
      //  this.Sort();
      //  _countGet = 0;
      //}
      for (int i = 0; i < _items.Length; i++) {
        if (_items[i].Key == key) {
          item = _items[i];
//          _items[i].CountGet++;
          return true;
        }
      }
      item = null;
      return false;
    }
    #endregion

    public bool TryGetValue(Function function, object[] parameters, out CacheItem cacheItem) {
      for (int i = 0; i < _items.Length; i++) {
        CacheItem item = _items[i];
        if (item.Function == function && parameters.Length == item.KeyParameters.Length) {

          bool eq = true;
          for (int j = 0; j < item.KeyParameters.Length; j++) {
            if (!item.KeyParameters[j].Equals(parameters[j])) {
              eq = false;
              break;
            }
          }
          if (eq) {
            cacheItem = item;
            return true;
          }
        }
      }
      cacheItem = null;
      return false;
    }

    #region private void Sort()
    private void Sort() {
      
      //int maxCountGet = 0;
      //List<CacheItem> list = new List<CacheItem>();

      //for (int i = 0; i < _items.Length; i++) {
      //  CacheItem item = _items[i];
      //  int insertIndex = list.Count;
      //  for (int j = 0; j < list.Count; j++) {
      //    if (item.CountGet > list[j].CountGet) {
      //      insertIndex = 0;
      //      break;
      //    }
      //  }
      //  list.Insert(insertIndex, item);
      //  maxCountGet = Math.Max(maxCountGet, item.CountGet);
      //}
      //_items = list.ToArray();

      //if (maxCountGet > 10000000) {
      //  for (int i = 0; i < _items.Length; i++) {
      //    _items[i].CountGet = 0;
      //  }
      //}
    }
    #endregion

    #region internal CacheItem GetItem(BaseVector vector)
    internal CacheItem GetItem(BaseVector vector) {
      for (int i = 0; i < _items.Length; i++) {
        if (_items[i].Result == vector)
          return _items[i];
      }
      return null;
    }
    #endregion
  }

  public class CacheItem {

    private Function _function;
    private object[] _parameters, _keyParameters;
    private CacheCollection _sourceItems;
    private CacheItem _parentItem = null;

    private string _key;
    private BaseVector _result;
    private int _countGet;

    internal CacheItem(string key, Function function, object[] parameters, object[] keyParameters) {
      _key = key;
      _function = function;
      _parameters = parameters;
      _keyParameters = keyParameters;
      _sourceItems = new CacheCollection();
    }

    #region old
    //internal CacheItem(CacheCollection cacheCollection, Function function, object[] parameters, BaseVector vector) {
    //  _vector = vector;
    //  _countGet = 0;

    //  _function = function;
    //  _parameters = parameters;
    //  _key = _function.Name + "(";

    //  List<CacheItem> sourceVectors = new List<CacheItem>();
    //  for (int i = 0; i < parameters.Length; i++) {
    //    if (parameters[i] is BaseVector) {
    //      CacheItem sourceItem = cacheCollection.GetItem(parameters[i] as BaseVector);
    //      sourceVectors.Add(sourceItem);
    //      _key += sourceItem.Key;
    //    } else {
    //      _key += parameters[i].ToString();
    //    }
    //    if (i < parameters.Length - 1)
    //      _key += ";";
    //  }
    //  _key += ")";
    //  _sourceItems = sourceVectors.ToArray();
    //}
    #endregion

    #region internal Function Function
    /// <summary>
    /// Функция на основе которой создан данный вектор
    /// </summary>
    internal Function Function {
      get { return this._function; }
    }
    #endregion

    #region internal object[] Parameters
    /// <summary>
    /// Параметры, на основе которых рассчитан данный вектор
    /// </summary>
    internal object[] Parameters {
      get { return _parameters; }
    }
    #endregion

    #region internal object[] KeyParameters
    internal object[] KeyParameters {
      get { return this._keyParameters; }
    }
    #endregion

    #region public string Key
    public string Key {
      get { return this._key; }
    }
    #endregion

    #region public BaseVector Result
    public BaseVector Result {
      get { return this._result; }
    }
    #endregion

    #region internal int CountGet
    internal int CountGet {
      get { return this._countGet; }
      set { this._countGet = value; }
    }
    #endregion

    #region internal CacheItem Parent
    internal CacheItem Parent {
      get { return this._parentItem; }
    }
    #endregion

    #region internal CacheCollection[] SourceCache
    internal CacheCollection SourceCache {
      get { return _sourceItems; }
    }
    #endregion

    #region internal void AddSourceCache(CacheItem item)
    internal void AddSourceCache(CacheItem item) {
      item._parentItem = this;
      _sourceItems.Add(item);
    }
    #endregion

    #region internal void SetResult(BaseVector result)
    internal void SetResult(BaseVector result) {
      _result = result;
    }
    #endregion

    #region public string GetComment(int count)
    public string GetComment(int count) {
      string comment = this.Key;

      IVector vector = this.Result as IVector;

      if (vector == null)
        return comment;

      int indexCount = Math.Max(vector.Count - count, 0);

      List<string> sa = new List<string>();
      for (int i = 1; i <= count; i++) {
        int index = vector.Count - i;
        if (index < 0)
          break;
        string val = "";
        if (!float.IsNaN(vector[index])) {
          double rval = Convert.ToDouble(vector[index]);
          rval = Math.Round(rval, 7);
          val = rval.ToString();
        }
        sa.Add(val);
      }

      return comment + 
#if DEBUG
        "[" + (this.Result as IVector).Count +"]"+
#endif
        ":" + string.Join(";", sa.ToArray());
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      IVector vector = this.Result as IVector;
      return this.Key + ", Count=" + vector.Count;
    }
    #endregion
  }

}
