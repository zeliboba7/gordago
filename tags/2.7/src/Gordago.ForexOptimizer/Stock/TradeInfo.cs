/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.Stock {
	/// <summary>
	/// Информация о зделке
	/// </summary>
	public class TradeInfo {

		public float PriceBuyIn;
		public float PriceBuyOut;

		public float PriceSellIn;
		public float PriceSellOut;
		
		public TradeInfo() {
		PriceBuyIn = float.NaN;
		PriceBuyOut = float.NaN;

		PriceSellIn = float.NaN;
		PriceSellOut = float.NaN;

		}
	}
}
