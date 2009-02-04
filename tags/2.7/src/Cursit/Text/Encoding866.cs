/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Text;

namespace Cursit.Text
{
	//  одировка/декодировка - кодова€ страница 866
	public class Encoding866: Encoding {

		private Encoder encoder = new Encoder866();
		private Decoder decoder = new Decoder866();

		public Encoding866(): base(866) {
		}

		public override Decoder GetDecoder() {

			return new Decoder866();

		}

		public override Encoder GetEncoder() {

			return new Encoder866();

		}

		public override int GetByteCount(char[] chars, int index, int count) {

			return encoder.GetByteCount(chars, index, count, false);

		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {

			return encoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex, false);

		}

		public override int GetCharCount(byte[] bytes, int index, int count) {

			return decoder.GetCharCount(bytes, index, count);

		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {

			return decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex);

		}

		public override int GetMaxByteCount(int charCount) {

			return charCount;

		}

		public override int GetMaxCharCount(int byteCount) {

			return byteCount;

		}

	}
}
