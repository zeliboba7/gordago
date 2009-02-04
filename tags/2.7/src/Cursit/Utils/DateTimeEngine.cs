/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Cursit.Utils {
	public class DateTimeEngine {
		public static DateTime RoundDateTime(DateTime dtm){
			DateTime rdtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0,0,0);
			return rdtm;
		}

		public static uint ConvertDateTimeToUnix(DateTime dt){
			return (uint)((dt.Ticks - new DateTime(1970, 1,1,0,0,0,0).Ticks)/10000000);
		}

		public static DateTime ConvertFromUnixToDateTime(ulong Time){
			return new DateTime((long)(Time * 10000000) + new DateTime(1970, 1,1,0,0,0,0).Ticks);
		}
	}
}
