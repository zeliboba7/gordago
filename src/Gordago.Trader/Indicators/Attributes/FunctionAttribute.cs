/**
* @version $Id: FunctionAttribute.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Drawing;

  //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  //public class FunctionDataAttribute : Attribute {

  //  private Type _defaultFunctionData;

  //  public FunctionDataAttribute(Type defaultFunctionData) {
  //    _defaultFunctionData = defaultFunctionData;
  //  }

  //  #region public Type DefaultFunctionData
  //  public Type DefaultFunctionData {
  //    get { return _defaultFunctionData; }
  //  }
  //  #endregion
  //}

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class FunctionAttribute : Attribute {
    private string _functionName;
    private int _order;

    public FunctionAttribute(string functionName):this(functionName, 0) {
    }

    public FunctionAttribute(string functionName, int order) {
      _functionName = functionName;
      _order = order;
    }

    #region public string FunctionName
    public string FunctionName {
      get { return _functionName; }
    }
    #endregion

    #region public int Order
    public int Order {
      get { return this._order; }
    }
    #endregion
  }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
  public class FunctionColorAttribute : Attribute {
    private Color _color = Color.Red;

    public FunctionColorAttribute(string name) {
      _color = Color.FromName(name);
    }

    public FunctionColorAttribute(int argb){
       _color = Color.FromArgb(argb);
    }

    public FunctionColorAttribute(int red, int green, int blue) {
      _color = Color.FromArgb(red, green, blue);
    }

    public FunctionColorAttribute(int alpha, int red, int green, int blue) {
      _color = Color.FromArgb(alpha, red, green, blue);
    }


    #region public Color FunctionColor
    public Color FunctionColor {
      get { return this._color; }
      set { this._color = value; }
    }
    #endregion
  }

  public class FunctionStyleAttribute : Attribute {

    private FunctionStyle _style = FunctionStyle.Line;
    private int _width = 1;

    public FunctionStyleAttribute(FunctionStyle style) : this(style, 1) {
    }


    public FunctionStyleAttribute(FunctionStyle style, int width) {
      _style = style;
      _width = width;
    }

    #region public FunctionStyle Style
    public FunctionStyle Style {
      get { return this._style; }
      set { this._style = value; }
    }
    #endregion

    #region public int Width
    public int Width {
      get { return this._width; }
      set {
        this._width = Math.Max(1, value);
      }
    }
    #endregion

  }
}
