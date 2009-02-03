/**
* @version $Id: ParameterFromAttribute.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class ParameterFromAttribute : Attribute {
    private Type _sourceType;
    private string _name;
    private int _order;

    public ParameterFromAttribute(Type classType, string parameterName):this(classType, parameterName, 0) {
    }

    public ParameterFromAttribute(Type classType, string parameterName, int order) {
      _sourceType = classType;
      _name = parameterName;
      _order = order;
    }

    #region public Type ClassType
    public Type ClassType {
      get { return _sourceType; }
    }
    #endregion

    #region public string ParameterName
    public string ParameterName {
      get { return this._name; }
    }
    #endregion

    #region public int Order
    public int Order {
      get { return _order; }
    }
    #endregion
  }
}
