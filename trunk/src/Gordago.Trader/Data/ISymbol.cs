/**
* @version $Id: ISymbol.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public interface ISymbolInfo {
    string Name { get;}
    int Digits { get;}
  }

  public interface ISymbol:ISymbolInfo {
    float Point { get;}
    ITickCollection Ticks { get;}
  }
}
