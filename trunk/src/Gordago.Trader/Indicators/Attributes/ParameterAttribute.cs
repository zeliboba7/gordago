/**
* @version $Id: ParameterAttribute.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class ParameterAttribute : Attribute {
    private int _order = 0;
    private string _name = "";

    private Type _defaultType = null;

    public ParameterAttribute(string name) : this(name, 0) { }

    public ParameterAttribute(string name, int order) {
      _name = name;
      _order = order;
    }

    #region public int Order
    public int Order {
      get { return _order; }
      set { _order = value; }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return _name; }
      //      set { this._name; }
    }
    #endregion

    #region public Type DefaultType
    public Type DefaultType {
      get { return this._defaultType; }
      set { this._defaultType = value; }
    }
    #endregion
  }
}
