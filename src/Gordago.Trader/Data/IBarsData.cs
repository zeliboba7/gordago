/**
* @version $Id: IBarsData.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{

  public interface IBarsData {

    Bar Current { get;}
    Bar this[int index] { get;}
    int Count { get;}

    TimeFrame TimeFrame { get;}
  }

}
