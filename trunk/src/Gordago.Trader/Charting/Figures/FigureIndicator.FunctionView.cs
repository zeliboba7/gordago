/**
* @version $Id: FigureIndicator.FunctionView.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Charting.Figures
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;
  using System.Reflection;
  using System.ComponentModel;

  partial class FigureIndicator {
    public class FunctionViewInfo {

      #region enum FlagsError
      [Flags]
      public enum FlagsError {
        NotFunctionView
      }
      #endregion

      private FlagsError _error = FlagsError.NotFunctionView;
      private int _order = 0;
      private string _name;
      private Color _color = Color.Red;
      private FunctionStyle _style = FunctionStyle.Line;
      private int _width = 1;

      private PropertyInfo _propertyInfo;
      private IFunction _function;

      public FunctionViewInfo(PropertyInfo propertyInfo) {
        _propertyInfo = propertyInfo;

        object[] objs = propertyInfo.GetCustomAttributes(false);
        foreach (object obj in objs) {
          if (obj is FunctionAttribute) {
            FunctionAttribute fattr = (FunctionAttribute)obj;
            _name = fattr.FunctionName;
            _order = fattr.Order;
            _error |= ~FlagsError.NotFunctionView;
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

      #region public FlagsError Error
      public FlagsError Error {
        get { return _error; }
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

      #region internal void Join(Indicator indicator)
      public void Join(Indicator indicator) {

        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(indicator);
        PropertyDescriptor myProperty = properties.Find(_propertyInfo.Name, false);

        _function = myProperty.GetValue(indicator) as IFunction;
      }
      #endregion
    }

  }
}
