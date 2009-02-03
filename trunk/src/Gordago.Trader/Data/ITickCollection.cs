/**
* @version $Id: ITickCollection.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public interface ITickCollection {

    //DateTime TimeFrom { get;}
    //DateTime TimeTo { get;}
    int Count { get;}
    Tick this[int index]{get;}
    Tick Current { get;}

    IBarsDataCollection BarsDataList { get;}
  }
}
