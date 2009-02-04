/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.Analysis.Chart {
	/// <summary>
	/// Масштаб на графике
	/// </summary>
	public enum ChartZoom: int {
		Smaller = 0,
		Small = 1,
		Medium = 2, 
		Larger = 3,
		Large = 4,
		BigLarge = 5
	}
}
