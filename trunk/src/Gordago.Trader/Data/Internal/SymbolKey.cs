/**
* @version $Id: SymbolKey.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  /// <summary>
  /// Symbol key 
  /// </summary>
  class SymbolKey {

    private readonly string _symbolName;
    private readonly int _hashCode;

    public SymbolKey(string symbolName) {
      _symbolName = symbolName.ToUpper();
      _hashCode = _symbolName.GetHashCode();
    }

    #region public string Name
    public string Name {
      get { return this._symbolName; }
    }
    #endregion

    #region public override bool Equals(object obj)
    public override bool Equals(object obj) {
      return obj.GetHashCode() == _hashCode;
    }
    #endregion

    #region public override int GetHashCode()
    public override int GetHashCode() {
      return _hashCode;
    }
    #endregion
  }
}
