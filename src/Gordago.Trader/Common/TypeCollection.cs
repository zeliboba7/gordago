/**
* @version $Id: TypeCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class TypeCollection {

    #region struct TypeKey
    struct TypeKey {
      private readonly int _hashCode;
      
      public TypeKey(Type type):this(type.FullName) {
      }

      public TypeKey(string fullName) {
        _hashCode = fullName.GetHashCode();
      }

      #region public override bool Equals(object obj)
      public override bool Equals(object obj) {
        if (!(obj is TypeKey)) return false;
        TypeKey key = (TypeKey)obj;
        return key._hashCode == _hashCode;
      }
      #endregion

      #region public override int GetHashCode()
      public override int GetHashCode() {
        return _hashCode;
      }
      #endregion
    }
    #endregion

    private readonly Dictionary<TypeKey, Type> _types = new Dictionary<TypeKey, Type>();

    #region public int Count
    public int Count {
      get { return _types.Count; }
    }
    #endregion

    #region public Type this[string fullName]
    public Type this[string fullName] {
      get {
        Type type = null;
        _types.TryGetValue(new TypeKey(fullName), out type);
        return type;
      }
    }
    #endregion

    #region public void Add(Type type)
    public void Add(Type type) {
      _types.Add(new TypeKey(type), type);
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _types.Clear();
    }
    #endregion

    #region public bool Remove(Type type)
    public bool Remove(Type type) {
      return _types.Remove(new TypeKey(type));
    }
    #endregion
  }
}
