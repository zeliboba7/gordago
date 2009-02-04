/**
* @version $Id: IndicatorItem.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO.Instruments
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  class IndicatorItem {

    private Type _type;
    private string _name;

    public IndicatorItem(Type type) {
      _type = type;
      _name = type.Name;
    }

    #region public Type IndicatorType
    public Type IndicatorType {
      get { return _type; }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return _name; }
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
      return _name;
    }
    #endregion
  }
}
