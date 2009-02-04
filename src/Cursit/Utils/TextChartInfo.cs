/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace Cursit.Utils {
	/// <summary>
	/// Все что связанно со строками и графиком
	/// </summary>
	public class TextChartInfo {

		/// <summary>
		/// Расчет размер строки
		/// </summary>
		public static SizeF GetStringLenght(string str, Font fnt){
			Graphics g = Graphics.FromImage(new Bitmap(10,10));

			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			StringFormat m_sf = new StringFormat(StringFormat.GenericTypographic);
			m_sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces; 
			SizeF sz = g.MeasureString(str, fnt, 1000, m_sf);
			return sz;
		}
	}
}
