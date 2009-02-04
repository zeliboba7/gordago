/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Text;

namespace Cursit.Text {
	// ������������ 
	public class Decoder866: Decoder {

		public override int GetCharCount(byte[] bytes, int index, int count) {
			return count;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {

			// ���� ��������������

			for (int i = 0; i < byteCount; i++) {

				// ������� ��������� ����

				byte b = bytes[byteIndex + i];

				// ���� ������ ����� ������� ��������, ������� �������� ��� ���������

				if (b > 0 && b <= 0x7F) chars[charIndex + i] = (char) b;

					// ���� ������ �..� ��� �..�, �����������

				else if (b >= 0x80 && b <= 0xAF) chars[charIndex + i] = (char) (b - 0x80 + '�');

					// ���� ������ �..�, �����������

				else if (b >= 0xE0 && b <= 0xEF) chars[charIndex + i] = (char) (b - 0xE0 + '�');

					// ������� �������������

				else if (b >= 0xB0 && b <= 0xDF) chars[charIndex + i] = boxDrowingChars[b - 0xB0];

					// �, �, ������ �������

				else if (b >= 0xF0 && b <= 0xFE) chars[charIndex + i] = moreSymbols[b - 0xF0];

					// �� ��������� - ����������� ������ 0xA0

				else chars[charIndex + i] = (char) 0xA0; 

			}

			return byteCount;

		}

		// ���� �������� ������������� Unicode (��������� DrawBoxing)

		// �������� � ������������������, ��������������� ������� ���������� ����������� �������� ������� �������� 866

		static private char[] boxDrowingChars = {

													'-', '-', '-', '�', '+', '�', '�', '�', '�', '�', '�', '�', '-', '-', '-', '�', 

													'L', '+', 'T', '+', '-', '+', '�', '�', 'L', '�', '�', 'T', '�', '=', '+', '�', 

													'�', 'T', 'T', 'L', 'L', '-', '�', '+', '+', '-', '-', '-', '-', '�', '�', '-'};

		// �, �, ������ �������
		static private char[] moreSymbols = {

												'�', '�', '�', '�', '�', '�', '�', '�', '�', '�', '�', 'v', '�', '�', '�'};

	}

}
