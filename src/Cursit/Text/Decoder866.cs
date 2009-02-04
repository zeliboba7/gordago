/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Text;

namespace Cursit.Text {
	// Декодировщик 
	public class Decoder866: Decoder {

		public override int GetCharCount(byte[] bytes, int index, int count) {
			return count;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {

			// цикл преобразования

			for (int i = 0; i < byteCount; i++) {

				// возьмем очередной байт

				byte b = bytes[byteIndex + i];

				// если первая часть таблицы символов, запишем значение без изменений

				if (b > 0 && b <= 0x7F) chars[charIndex + i] = (char) b;

					// если символ А..Я или а..п, преобразуем

				else if (b >= 0x80 && b <= 0xAF) chars[charIndex + i] = (char) (b - 0x80 + 'А');

					// если символ р..я, преобразуем

				else if (b >= 0xE0 && b <= 0xEF) chars[charIndex + i] = (char) (b - 0xE0 + 'р');

					// символы псевдографики

				else if (b >= 0xB0 && b <= 0xDF) chars[charIndex + i] = boxDrowingChars[b - 0xB0];

					// Ё, ё, прочие символы

				else if (b >= 0xF0 && b <= 0xFE) chars[charIndex + i] = moreSymbols[b - 0xF0];

					// по умолчанию - неразрывный пробел 0xA0

				else chars[charIndex + i] = (char) 0xA0; 

			}

			return byteCount;

		}

		// Коды символов псевдографики Unicode (категории DrawBoxing)

		// Хранятся в последовательности, соответствующей порядку следования аналогичных символов кодовой страницы 866

		static private char[] boxDrowingChars = {

													'-', '-', '-', '¦', '+', '¦', '¦', '¬', '¬', '¦', '¦', '¬', '-', '-', '-', '¬', 

													'L', '+', 'T', '+', '-', '+', '¦', '¦', 'L', 'г', '¦', 'T', '¦', '=', '+', '¦', 

													'¦', 'T', 'T', 'L', 'L', '-', 'г', '+', '+', '-', '-', '-', '-', '¦', '¦', '-'};

		// Ё, ё, прочие символы
		static private char[] moreSymbols = {

												'Ё', 'ё', 'Є', 'є', 'Ї', 'ї', 'Ў', 'ў', '°', '•', '·', 'v', '№', '¤', '¦'};

	}

}
