/**
* @version $Id: IBarsDataCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;

  public interface IBarsDataCollection {
    int Count { get;}
    IBarsData this[TimeFrame tf] { get;}
    // IBarsData this[int period] { get;}
  }
}
