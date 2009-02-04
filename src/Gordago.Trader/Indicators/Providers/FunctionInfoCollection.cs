/**
* @version $Id: FunctionInfoCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class FunctionInfoCollection:IEnumerable<FunctionInfo> {

    #region struct FunctionInfoKey
    struct FunctionInfoKey{
      private readonly int _hashCode;

      public FunctionInfoKey(FunctionInfo funcitonInfo) :this(funcitonInfo.FunctionType){
        
      }

      public FunctionInfoKey(Type functionType) {
        _hashCode = functionType.FullName.GetHashCode();
      }

      public override bool Equals(object obj) {
        if (!(obj is FunctionInfoKey)) return false;
        FunctionInfoKey key = (FunctionInfoKey)obj;

        return _hashCode == key._hashCode;
      }

      public override int GetHashCode() {
        return _hashCode;
      }
    }
    #endregion

    private readonly Dictionary<FunctionInfoKey, FunctionInfo> _functions = new Dictionary<FunctionInfoKey, FunctionInfo>();

    #region public int Count
    public int Count {
      get { return this._functions.Count; }
    }
    #endregion


    #region public FunctionInfo this[Type functionType]
    public FunctionInfo this[Type functionType] {
      get {
        FunctionInfo fi = null;
        _functions.TryGetValue(new FunctionInfoKey(functionType), out fi);
        return fi;
      }
    }
    #endregion

    #region internal void Add(FunctionInfo functionInfo)
    internal void Add(FunctionInfo functionInfo) {
      _functions.Add(new FunctionInfoKey(functionInfo), functionInfo);
    }
    #endregion

    #region internal bool Remove(FunctionInfo functionInfo)
    internal bool Remove(FunctionInfo functionInfo) {
      return this.Remove(functionInfo.FunctionType);
    }
    #endregion

    #region internal bool Remove(Type functionType)
    internal bool Remove(Type functionType) {
      return _functions.Remove(new FunctionInfoKey(functionType));
    }
    #endregion

    #region internal void Clear()
    internal void Clear() {
      _functions.Clear();
    }
    #endregion

    #region public IEnumerator<FunctionInfo> GetEnumerator()
    public IEnumerator<FunctionInfo> GetEnumerator() {
      return _functions.Values.GetEnumerator();
    }
    #endregion

    #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return _functions.Values.GetEnumerator();
    }
    #endregion
  }
}
