/**
* @version $Id: Parameter.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Reflection;
  using System.ComponentModel;
  using Gordago.Trader.Builder;

  public class Parameter {

    #region internal struct ParameterKey
    internal struct ParameterKey {

      private readonly int _hashCode;

      public ParameterKey(string name) {
        _hashCode = name.GetHashCode();
      }

      #region public override bool Equals(object obj)
      public override bool Equals(object obj) {
        if (!(obj is ParameterKey)) return false;
        ParameterKey key = (ParameterKey)obj;
        return _hashCode == key._hashCode;
      }
      #endregion

      #region public override int GetHashCode()
      public override int GetHashCode() {
        return _hashCode;
      }
      #endregion
    }
    #endregion

    #region public enum FlagError
    [Flags]
    public enum FlagError {
      NotParameter = 2,
      ParameterFromNotFound = 4,
      ParameterFromBadType = 8,
      ParameterFromCycleLink = 16
    }
    #endregion

    private bool _browsable = false;
    private Type _parameterType = null;
    private object _defaultValue = null;
    private Type _defaultType = null;
    private string _name = "";
    private string _display = "";
    private int _order;
    private string _category;
    private string _description;
    private ParameterKey _key;
    private object _value;
    private FlagError _error = FlagError.NotParameter;
    private PropertyInfo _propertyInfo;

    private ClassBuilder _classBuilder;

    public Parameter(object obj, string name) {
      _parameterType = obj.GetType();
      _value = obj;
      _name = name;
      _key = new ParameterKey(_name);
    }

    public Parameter(PropertyInfo pinfo) {
      _propertyInfo = pinfo;

      object[] attrs = pinfo.GetCustomAttributes(false);
      _parameterType = pinfo.PropertyType;

      /* 
       * Не является ли этот параметр наследником, если да, то копируем значения 
       * После, установка значений на новый параметр
       */
      foreach (object attr in attrs) {

        if (!(attr is ParameterFromAttribute))
          continue;

        ParameterFromAttribute psource = attr as ParameterFromAttribute;

        if (psource.ClassType == pinfo.DeclaringType) {
          _error |= FlagError.ParameterFromCycleLink;
          break;
        }

        Parameter[] prms = ClassBuilder.GetParameters(psource.ClassType);
        Parameter paramSource = null;
        foreach (Parameter prm in prms) {
          if (prm.Name == psource.ParameterName) {
            if (prm.ParameterType != _parameterType) {
              _error |= FlagError.ParameterFromBadType;
              break;
            }
            paramSource = prm;
            break;
          }
        }
        if (prms == null) {
          _error |= FlagError.ParameterFromNotFound;
          break;
        }
        paramSource.CopyTo(this);
        break;
      }

      _name = pinfo.Name;

      foreach (object attr in attrs) {
        if (attr is BrowsableAttribute) {
          _browsable = (attr as BrowsableAttribute).Browsable;
        } else if (attr is ParameterAttribute) {
          ParameterAttribute pattr = attr as ParameterAttribute;
          _name = pattr.Name;
          _order = pattr.Order;
          _defaultType = pattr.DefaultType;
          _error |= ~FlagError.NotParameter;
        } else if (attr is DisplayNameAttribute) {
          _display = (attr as DisplayNameAttribute).DisplayName;
        } else if (attr is CategoryAttribute) {
          _category = (attr as CategoryAttribute).Category;
        } else if (attr is DescriptionAttribute) {
          _description = (attr as DescriptionAttribute).Description;
        } else if (attr is DefaultValueAttribute) {
          DefaultValueAttribute dattr = attr as DefaultValueAttribute;
          _defaultValue = dattr.Value;
        } 
      }

      if (_display == "")
        _display = _name;

      _key = new ParameterKey(_name);
      _value = _defaultValue;
    }

    #region public ClassBuilder ClassBuilder
    public ClassBuilder ClassBuilder {
      get { return this._classBuilder; }
      set { _classBuilder = value; }
    }
    #endregion

    #region public FlagError Error
    public FlagError Error {
      get { return _error; }
    }
    #endregion

    #region public PropertyInfo PropertyInfo
    public PropertyInfo PropertyInfo {
      get { return _propertyInfo; }
    }
    #endregion

    #region internal ParameterKey Key
    internal ParameterKey Key {
      get { return this._key; }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return this._name; }
    }
    #endregion

    #region public Type ParameterType
    public Type ParameterType {
      get { return _parameterType; }
    }
    #endregion

    #region public bool Browsable
    public bool Browsable {
      get { return _browsable; }
    }
    #endregion

    #region public object DefaultValue
    public object DefaultValue {
      get { return _defaultValue; }
    }
    #endregion

    #region public Type DefaultType
    public Type DefaultType {
      get { return _defaultType; }
    }
    #endregion

    #region public string Display
    public string Display {
      get { return this._display; }
    }
    #endregion

    #region public int Order
    public int Order {
      get { return this._order; }
    }
    #endregion

    #region public string Category
    public string Category {
      get { return this._category; }
    }
    #endregion

    #region public string Description
    public string Description {
      get { return this._description; }
    }
    #endregion

    #region public object Value
    public object Value {
      get { return this._value; }
      set { this._value = value; }
    }
    #endregion

    #region protected virtual void CopyTo(Parameter param)
    protected virtual void CopyTo(Parameter param) {
      param._browsable = _browsable;
      param._category = _category;
      param._defaultValue = _defaultValue;
      param._description = _description;
      param._display = _display;
      param._error = _error;
      param._value = _value;
      param._defaultType = _defaultType;
    }
    #endregion

    public override string ToString() {

      return string.Format("Parameter: Name={0}, Type={1}, Value={2}", _name, _parameterType, _value);
    }
  }
}
