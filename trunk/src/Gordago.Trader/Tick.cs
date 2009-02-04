/**
* @version $Id: Tick.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;

	public struct Tick{
		public long Time;
		public float Price;

		public Tick(long time, float price){
			Time = time;
			Price = price;
		}
	}
}
