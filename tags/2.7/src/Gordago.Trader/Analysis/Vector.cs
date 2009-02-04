/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;

namespace Gordago.Analysis {

  #region public abstract class BaseVector
  /// <summary>
  /// јнализатор введен дл€ убивание погрешности, св€з€нна€ с ситуацией когда
  /// идет рассчет значени€ по цене закрыти€ и при этом происходит пропуск тиков в тестере
  /// так как условие не запрашиваетс€ а потом рассчет происходит на другом баре
  /// </summary>
  public abstract class BaseVector {

    private int _savedTickCount;
    private int _hideCount;
    private bool _hideMode = false;
    private bool _blocked;

    #region internal bool Blocked
    internal bool Blocked {
      get { return this._blocked; }
      set { this._blocked = value; }
    }
    #endregion

    #region internal int HideCount
    internal int HideCount {
      get { return _hideCount; }
    }
    #endregion

    #region internal bool HideMode
    internal bool HideMode {
      get { return this._hideMode; }
    }
    #endregion

    #region internal int SavedTickCount
    internal int SavedTickCount {
      get { return _savedTickCount; }
      set { _savedTickCount = value; }
    }
    #endregion

    #region internal void HideValues(int count)
    /// <summary>
    /// —крыть последние значение в векторе, дабы пересчитать последние рассчитанное 
    /// значение с актулальным значением
    /// </summary>
    /// <param name="count"></param>
    internal void HideValues(int countVector) {
      _hideMode = true;
      _hideCount = countVector;
      _savedTickCount = 0;
    }
    #endregion

    #region internal void ShowValues()
    internal void ShowValues() {
      _hideMode = false;
      _hideCount = 0;
      _savedTickCount = 0;
    }
    #endregion
  }
  #endregion

  #region public interface IVector
  public interface IVector {
    int Count { get;}
    float Current { get;}
    float this[int index] { get;set;}

    void Add(float value);
    void Clear();
    void RemoveLastValue();
  }
  #endregion

  public class Vector : BaseVector, IVector {
    private float[] _vector;
    private int _count = 0;

    public Vector() : this(16) { }

    public Vector(int capacity) {
      _vector = new float[Math.Max(16, capacity)];
    }

    #region internal int Capacity
    internal int Capacity {
      get {
        return _vector.Length;
      }
    }
    #endregion

    #region public int Size
    /// <summary>
    ///  ол-во элементов, оставлена дл€ совместимости со старой версией
    /// </summary>
    public int Size {
      get {
        return _count;
      }
    }
    #endregion

    #region public virtual int Count
    /// <summary>
    ///  ол-во элементов
    /// </summary>
    public virtual int Count {
      get {
        if (this.HideMode)
          return HideCount;
        return _count;
      }
    }
    #endregion

    #region public float Current
    public float Current {
      get { return this[_count - 1]; }
      set { this[_count - 1] = value; }
    }
    #endregion

    #region public virtual float this[int index]
    public virtual float this[int index] {
      get { return _vector[index]; }
      set { _vector[index] = value; }
    }
    #endregion

    #region public void Add(float @value)
    public void Add(float @value) {
      if (_count == _vector.Length) {
        float[] temp = new float[_vector.Length * 2];
        Array.Copy(_vector, 0, temp, 0, _vector.Length);
        _vector = temp;
      }
      _vector[_count++] = @value;
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _count = 0;
    }
    #endregion

    #region public void RemoveLastValue()
    public void RemoveLastValue() {
      if (_count > 0)
        _count--;
    }
    #endregion

    #region public float Shift(int shift)
    public float Shift(int shift) {
      if (_count - 1 - shift < 0) return float.NaN;
      return this[_count - 1 - shift];
    }
    #endregion
  }
}
