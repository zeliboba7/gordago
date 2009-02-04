/**
* @version $Id: InputAttribute.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;

  [AttributeUsage(AttributeTargets.Constructor, AllowMultiple=false)]
  public class InputAttribute:Attribute {

    private string[] _parameters;
    public InputAttribute(params string[] parameters) {
      _parameters = parameters;
    }

    #region public string[] Parameters
    public string[] Parameters {
      get { return this._parameters; }
    }
    #endregion
  }
}
