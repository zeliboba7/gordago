/**
* @version $Id: ParameterCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class ParameterCollection:ICollection<Parameter> {

    private readonly Dictionary<Parameter.ParameterKey, Parameter> _params = new Dictionary<Parameter.ParameterKey, Parameter>();

    public ParameterCollection() { }


    #region public int Count
    public int Count {
      get { return _params.Count; }
    }
    #endregion

    #region internal void Add(ParameterInfo item)
    public void Add(Parameter item) {
      _params.Add(item.Key, item);
    }
    #endregion

    #region internal void AddRange(ParameterInfo[] items)
    public void AddRange(Parameter[] items) {
      foreach (Parameter item in items) {
        this.Add(item);
      }
    }
    #endregion

    #region public ParameterInfo this[string name]
    public Parameter this[string name] {
      get {
        Parameter pinfo = null;
        _params.TryGetValue(new Parameter.ParameterKey(name), out pinfo);
        return pinfo;
      }
    }
    #endregion

    #region internal void Clear()
    internal void Clear() {
      _params.Clear();
    }
    #endregion

    #region public IEnumerator<ParameterInfo> GetEnumerator()
    public IEnumerator<Parameter> GetEnumerator() {
      return _params.Values.GetEnumerator();
    }
    #endregion

    #region private static int CompareByOrder(ParameterInfo prm1, ParameterInfo prm2)
    private static int CompareByOrder(Parameter prm1, Parameter prm2) {
      return prm1.Order.CompareTo(prm2.Order);
    }
    #endregion

    #region internal void Sort()
    internal void Sort() {
      List<Parameter> list = new List<Parameter>();
      foreach (Parameter pi in _params.Values) {
        list.Add(pi);
      }
      list.Sort(CompareByOrder);
      _params.Clear();
      foreach (Parameter pi in list) {
        _params.Add(pi.Key, pi);
      }
    }
    #endregion

    #region public object[] GetParameters()
    public object[] GetParameters() {
      List<object> list = new List<object>();
      foreach (Parameter param in this._params.Values) {
        list.Add(param.Value);
      }
      return list.ToArray();
    }
    #endregion

    #region public Parameter[] ToArray()
    public Parameter[] ToArray() {
      List<Parameter> list = new List<Parameter>();
      foreach (Parameter param in this._params.Values) {
        list.Add(param);
      }
      return list.ToArray();
    }
    #endregion

    #region The method or operation is not implemented
    bool ICollection<Parameter>.Contains(Parameter item) {
      throw new Exception("The method or operation is not implemented.");
    }

    void ICollection<Parameter>.CopyTo(Parameter[] array, int arrayIndex) {
      throw new Exception("The method or operation is not implemented.");
    }

    #region bool ICollection<ParameterInfo>.IsReadOnly
    bool ICollection<Parameter>.IsReadOnly {
      get { return false; }
    }
    #endregion

    bool ICollection<Parameter>.Remove(Parameter item) {
      throw new Exception("The method or operation is not implemented.");
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }


    void ICollection<Parameter>.Add(Parameter item) {
      throw new Exception("The method or operation is not implemented.");
    }

    void ICollection<Parameter>.Clear() {
      throw new Exception("The method or operation is not implemented.");
    }

    int ICollection<Parameter>.Count {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    IEnumerator<Parameter> IEnumerable<Parameter>.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }
    #endregion 
  }
}
