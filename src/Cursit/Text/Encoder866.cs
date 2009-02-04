/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Text;


namespace Cursit.Text {

	// Кодировщик 

	public class Encoder866: Encoder {

		public override int GetByteCount(char[] chars, int index, int count, bool flush) {

			return count;

		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush) {

			// цикл преобразования

			for (int i = 0; i < charCount; i++) {

				// возьмем очередной символ

				char c = chars[charIndex + i];

				// если первая часть таблицы символов, запишем значение без изменений

				if (c > 0 && c <= 0x7F) bytes[byteIndex + i] = (byte) c;

					// если символ А..Я или а..п, преобразуем

				else if (c >= 'А' && c <= 'п') bytes[byteIndex + i] = (byte) (c - 'А' + 0x80);

					// если символ р..я, преобразуем

				else if (c >= 'р' && c <= 'я') bytes[byteIndex + i] = (byte) (c - 'р' + 0xE0);

					// символы псевдографики

				else if (c >= '-' && c <= '-') bytes[byteIndex + i] = (byte) boxDrowingChars[c - '-'];

					// Ё, ё, прочие символы

				else 

					switch (c) {

						case 'Ё': bytes[byteIndex + i] = 0xF0; break;

						case 'ё': bytes[byteIndex + i] = 0xF1; break;

						case 'Є': bytes[byteIndex + i] = 0xF2; break;

						case 'є': bytes[byteIndex + i] = 0xF3; break;

						case 'Ї': bytes[byteIndex + i] = 0xF4; break;

						case 'ї': bytes[byteIndex + i] = 0xF5; break;

						case 'Ў': bytes[byteIndex + i] = 0xF6; break;

						case 'ў': bytes[byteIndex + i] = 0xF7; break;

						case '°': bytes[byteIndex + i] = 0xF8; break;

						case '•': bytes[byteIndex + i] = 0xF9; break;

						case '·': bytes[byteIndex + i] = 0xFA; break;

						case 'v': bytes[byteIndex + i] = 0xFB; break;

						case '№': bytes[byteIndex + i] = 0xFC; break;

						case '¤': bytes[byteIndex + i] = 0xFD; break;

						case '¦': bytes[byteIndex + i] = 0xFE; break;

							// по умолчанию - 255

						default: bytes[byteIndex + i] = 255; break;

					}

			}

			return charCount;

		}

		// Коды символов псевдографики кодовой страницы 866
		// Хранятся в последовательности, соответствующей порядку следования аналогичных символов Unicode
		// категории DrawBoxing
		static private int[] boxDrowingChars = { 
												   0xC4, 0xB3, 0xDA, 0xBF, 0xC0, 0xD9, 0xC3, 0xB4, 0xC2, 0xC1, 0xC5, 0xCD, 0xBA, 0xD5, 0xD6, 0xC9, 
												   0xB8, 0xB7, 0xBB, 0xD4, 0xD3, 0xC8, 0xBE, 0xBD, 0xBC, 0xC6, 0xC7, 0xCC, 0xB5, 0xB6, 0xB9, 0xD1, 
												   0xD2, 0xCB, 0xCF, 0xD0, 0xCA, 0xD8, 0xD7, 0xCE, 0xDF, 0xDC, 0xDB, 0xDD, 0xDE, 0xB0, 0xB1, 0xB2};
	}


}
