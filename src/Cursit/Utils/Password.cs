/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Text;
using System.Security.Cryptography;

namespace Cursit.Utils {
	public class Password {

		private static string over = "qwertyuiopasdfghjklzxcvbnm1234567890";
		private static string overpass = "idjkduf";
		public static byte[] CreateEncrypted(string login, string password) {

			login = login + "|" + over;
			login = login.Substring(0, 16);
			password = overpass + password;
			
			byte[] key = Encoding.Default.GetBytes(login);

			try{
				SymmetricAlgorithm alg = new TripleDESCryptoServiceProvider();
				byte[] IV = Encoding.Default.GetBytes("victorys");
				
				ICryptoTransform transform = alg.CreateEncryptor(key, IV);

				byte[] passwordByte = Encoding.Default.GetBytes(password);
				byte[] encpass = transform.TransformFinalBlock(passwordByte, 0, passwordByte.Length);

				string pass = CreatePass(login, encpass);
				return encpass;
			}catch(Exception){}
			return new byte[]{};
		}

		public static string CreatePass(string login, byte[] encpass){
			if (encpass == null || encpass.Length < 1)
				return "";

			login = login + "|" + over;
			login = login.Substring(0, 16);

			byte[] decryptedPassword = new byte[]{};
			byte[] key = Encoding.Default.GetBytes(login);
			SymmetricAlgorithm alg = new TripleDESCryptoServiceProvider();

			try{
				alg.GenerateIV();

				byte[] IV = Encoding.Default.GetBytes("victorys");

				ICryptoTransform transform  = alg.CreateDecryptor(key, IV);
				decryptedPassword = transform.TransformFinalBlock(encpass, 0, encpass.Length);
			}catch(Exception){}
			string pass = Encoding.Default.GetString(decryptedPassword);
			if (pass.IndexOf(overpass, 0) > -1)
				pass = pass.Substring(overpass.Length, pass.Length - overpass.Length);
			return pass;
		}

	}
}
