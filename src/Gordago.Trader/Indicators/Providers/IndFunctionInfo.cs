/**
* @version $Id: IndFunctionInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  using System.Drawing;

  public class IndFunctionInfo : FunctionInfo {
    
    #region public enum ParamError
    [Flags]
    public enum ParamError {
      IsNotFunctionOfIndicator = 2
    }
    #endregion

    private ParamError _error = ParamError.IsNotFunctionOfIndicator;
    private int _order = 0;
    private string _name;
    private Color _color = Color.Red;
    private FunctionStyle _style = FunctionStyle.Line;
    private int _width = 1;

    private PropertyInfo _propertyInfo;
    private IFunction _function;

    public IndFunctionInfo(PropertyInfo propertyInfo) : base(propertyInfo.PropertyType) {

      _propertyInfo = propertyInfo;

      _name = base.Name;

      object[] objs = propertyInfo.GetCustomAttributes(false);
      foreach (object obj in objs) {
        if (obj is FunctionAttribute) {
          FunctionAttribute fattr = (FunctionAttribute)obj;
          _name = fattr.FunctionName;
          _order = fattr.Order;
          _error |= ~ParamError.IsNotFunctionOfIndicator;
        } else if (obj is FunctionColorAttribute) {
          FunctionColorAttribute fattr = obj as FunctionColorAttribute;
          _color = fattr.FunctionColor;
        } else if (obj is FunctionStyleAttribute) {
          FunctionStyleAttribute fattr = obj as FunctionStyleAttribute;
          _style = fattr.Style;
          _width = fattr.Width;
        }
      }
    }

    #region public IFunction Function
    public IFunction Function {
      get { return _function; }
    }
    #endregion

    #region public ParamError Error
    public ParamError Error {
      get { return this._error; }
    }
    #endregion

    #region public new string Name
    public new string Name {
      get { return this._name; }
    }
    #endregion

    #region public int Order
    public int Order {
      get { return this._order; }
    }
    #endregion

    #region public Color Color
    public Color Color {
      get { return this._color; }
    }
    #endregion

    #region public FunctionStyle Style
    public FunctionStyle Style {
      get { return this._style; }
    }
    #endregion

    #region public int Width
    public int Width {
      get { return _width; }
    }
    #endregion

    #region internal void JoinFunction(Indicator indicator)
    internal void JoinFunction(Indicator indicator) {
      //object obj = _propertyInfo.GetValue(null, null);
      //_function = obj as IFunction;

      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(indicator);
      PropertyDescriptor myProperty = properties.Find(_propertyInfo.Name, false);

      _function = myProperty.GetValue(indicator) as IFunction;
    }
    #endregion
  }
}
