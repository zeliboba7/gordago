/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago {

	public struct Tick{
		public long Time;
		public float Price;

		public Tick(long time, float price){
			Time = time;
			Price = price;
		}

	}
}
