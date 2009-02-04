/**
* @version $Id: Function.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.ComponentModel;
  using System.Xml;
  using System.Xml.Serialization;
  
  using Gordago.Trader.Indicators;
  using System.Diagnostics;
  using Gordago.Core;

  #region public interface IFunctionItemCollection: IEnumerable<float>
  public interface IFunctionItemCollection: IEnumerable<float> {
    // int Count { get;}
    float this[int index] { get;}
    // float Current { get;}
  }
  #endregion

  #region public abstract class Function : IFunction
  public abstract class Function : IFunction {

    private readonly List<float> _data = new List<float>();

    private readonly IFunction _inData;
    private readonly ItemCollection _items;
    private readonly SessionDest _sessionDest;
    private readonly SessionSource _sessionSource;

    private bool _blockedCompute = false;
    private object _locked = new object();

    //private int _lastIndex = -1;
    //private float _lastValue;

    #region protected Function(IFunction data)
    /// <summary>
    /// Функция индикатора может иметь исходные данные двух типов
    /// iBars - данные Open, High, Low и Close, в данном случае функция не может 
    ///         быть расчитана от другого индикатора.
    /// iBars.RateData - данные Open, High, Low или Close, в данном случае функция
    ///         может быть рассчитана от этих цен
    /// Function - данные другой функции, в данном случае функция может быть рассчитана 
    ///         от этих цен, либо от другой функции
    /// </summary>
    /// <param name="data"></param>
    protected Function(IFunction inData) {
      if (inData == null)
        throw (new ArgumentNullException("inData"));

      _inData = inData;
      _items = new ItemCollection(this);

      /* Поик основателя, установка сессии с ним */
      if (inData is iBars.RateData) {
        _sessionSource = (inData as iBars.RateData).Session;
      } else if (inData is iBars) {
        _sessionSource = (inData as iBars).Session;
      } else if (inData is Function) {
        _sessionSource = (inData as Function)._sessionSource;
      }

      if (_sessionSource == null)
        throw (new NullReferenceException("InitializeSession error. Session not found"));
      _sessionDest = new SessionDest(_sessionSource);
    }
    #endregion

    #region public IFunction InData
    [Parameter("InData", DefaultType=typeof(iBars.FxClose))]
    public IFunction InData {
      get { return _inData; }
    }
    #endregion

    #region public IFunctionItemCollection Items
    [Browsable(false), XmlIgnore]
    public IFunctionItemCollection Items {
      get { return _items; }
    }
    #endregion

    #region public float this[int barsBack]
    [Browsable(false), XmlIgnore]
    public float this[int barsBack] {
      get {
        int index = this.Count - 1 - barsBack;
        if (index < 0)
          return float.NaN;
        return this.GetItem(index);
      }
    }
    #endregion

    #region public int Count
    [Browsable(false), XmlIgnore]
    public int Count {
      get {
        this.NativeCompute();
        return _data.Count;
      }
    }
    #endregion

    #region private float GetItem(int index)
    private float GetItem(int index) {
      this.NativeCompute();
      return this._data[index];
    }
    #endregion

    #region internal float NativeGetItem(int index)
    internal float NativeGetItem(int index) {
      return this._data[index];
    }
    #endregion

    #region protected virtual void OnRemoveLastValue()
    protected virtual void OnRemoveLastValue() {
      this.RemoveLastValue();
    }
    #endregion

    #region internal void NativeCompute()
    internal void NativeCompute() {
      if (_blockedCompute) return;

      lock (_locked) {

        int sess = _sessionDest.CheckSession();
        if (sess == 0) return;
        if (sess < 3) _items.Clear();

        _blockedCompute = true;

        long time = DateTime.Now.Ticks;
        Trace.WriteLine(string.Format("Begin Compute: Type={0}", this.GetType().Name));

        if (this.Count == this.InData.Count) 
          this.OnRemoveLastValue();

        this.OnCompute();

        Trace.WriteLine(string.Format("End Compute: Type={0}, Count={1}, Time={2}", 
          this.GetType(), this.Count, 
          (DateTime.Now.Ticks-time)/100000));

        _sessionDest.Complete();
        _blockedCompute = false;
      }
    }
    #endregion

    #region public void RemoveLastValue()
    public void RemoveLastValue() {
      if (_data.Count == 0)
        return;
      this._data.RemoveAt(_data.Count - 1);
    }
    #endregion

    protected abstract void OnCompute();

    #region protected void Add()
    protected void Add() {
      this.Add(float.NaN);
    }
    #endregion

    #region protected void Add(float value)
    protected void Add(float value) {
      this._items.Add(value);
    }
    #endregion

    #region class ItemCollection : IFunctionItemCollection
    class ItemCollection : IFunctionItemCollection {
      private readonly Function _owner;
      
      internal ItemCollection(Function owner) {
        _owner = owner;
      }
    
      #region public float this[int index]
      public float this[int index] {
        get {
          return _owner.GetItem(index);
        }
      }
      #endregion

      #region internal void Add(float value)
      internal void Add(float value) {
        _owner._data.Add(value);
      }
      #endregion

      #region internal void Clear()
      internal void Clear() {
        this._owner._data.Clear();
      }
      #endregion

      #region public IEnumerator<float> GetEnumerator()
      public IEnumerator<float> GetEnumerator() {
        this._owner.NativeCompute();
        return this._owner._data.GetEnumerator();
      }
      #endregion

      #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
        throw new Exception("The method or operation is not implemented.");
      }
      #endregion
    }
    #endregion

    #region public static void Subtraction(IFunction data1, IFunction data2, IFunction result)
    public static void Subtraction(IFunction data1, IFunction data2, IFunction result) {

      if (result.Count == data1.Count) {
        (result as Function).RemoveLastValue();
      }

      for (int i = result.Count; i < data1.Count; i++) {
        (result as Function).Add(data1.Items[i] - data2.Items[i]);
      }
    }
    #endregion
  }
  #endregion
}
