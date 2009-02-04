/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.API.Comment {
  
  public class CMTTagValue:CMTTag {
    private string _name;
    private object _value;

    public CMTTagValue(string name, object value):base("value") {
      _name = name;
      _value = value;
    }

    #region public string Name
    public string Name {
      get { return _name; }
      set { _name = value; }
    }
    #endregion

    #region public object Value
    public object Value {
      get { return this._value; }
      set { this._value = value; }
    }
    #endregion
  }
}
