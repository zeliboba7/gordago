/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Language
{
	/// <summary>
	/// Summary description for LangValue.
	/// </summary>
	public class LangValue
	{
		public int Id;
		public string Text;

		public LangValue(int id, string text)
		{
			this.Id = id;
			this.Text = text;
		}
		public override string ToString() {
			string s = Convert.ToString(Id) + "|" + Text.Trim();
			return s;
		}

	}
}
