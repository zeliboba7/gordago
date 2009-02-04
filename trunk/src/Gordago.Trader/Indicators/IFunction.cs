/**
* @version $Id: IFunction.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;

  public interface IFunction {

    float this[int barsBack] { get; }
    int Count { get;}

    IFunctionItemCollection Items { get;}
  }
}
