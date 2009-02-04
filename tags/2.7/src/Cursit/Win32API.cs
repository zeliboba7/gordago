/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Cursit {
	/// <summary>
	/// Использование функций АПИ
	/// </summary>
	public class Win32API {

//		public const ulong SND_ALIAS = 0x00010000L;
		
		[Flags]
			public enum SoundFlags : int {
			SND_SYNC = 0x0000,  // play synchronously (default) 
			SND_ASYNC = 0x0001,  // play asynchronously 
			SND_NODEFAULT = 0x0002,  // silence (!default) if sound not found 
			SND_MEMORY = 0x0004,  // pszSound points to a memory file
			SND_LOOP = 0x0008,  // loop the sound until next sndPlaySound 
			SND_NOSTOP = 0x0010,  // don't stop any currently playing sound 
			SND_NOWAIT = 0x00002000, // don't wait if the driver is busy 
			SND_ALIAS = 0x00010000, // name is a registry alias 
			SND_ALIAS_ID = 0x00110000, // alias is a predefined ID
			SND_FILENAME = 0x00020000, // name is file name 
			SND_RESOURCE = 0x00040004  // name is resource name or atom 
		}
    
		[DllImport("winmm.dll", SetLastError=true, CallingConvention=CallingConvention.Winapi)]
		static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf );

		public static bool PlaySoundFile(string filename){
			if (!System.IO.File.Exists(filename))
				return false;

			try{
				if (!PlaySound (filename, IntPtr.Zero,
					SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC ))
					return false;
			}catch{
				return false;
			}
			return true;
		}


		#region CaretRelated

		[DllImport("user32", EntryPoint="CreateCaret")]
		private static extern int CreateCaret(IntPtr hwnd, IntPtr hBitmap, int nWidth, int nHeight);

		[DllImport("user32", EntryPoint="SetCaretPos")]
		private static extern int SetCaretPos(int x, int y);

		[DllImport("user32", EntryPoint="DestroyCaret")]
		private static extern int DestroyCaret();

		[DllImport("user32", EntryPoint="HideCaret")]
		private static extern int HideCaret(IntPtr hwnd);

		[DllImport("user32", EntryPoint="ShowCaret")]
		private static extern int ShowCaret(IntPtr hwnd);
		#endregion 

		public static int CreateCaretApi(IntPtr hwnd, int width, int height ) {
			return CreateCaret(hwnd, IntPtr.Zero, width, height);
		}
		public static void SetCaretPosApi( int x, int y ) {
			SetCaretPos(x,y);
		}
		public static void DestroyCaretApi() {
			DestroyCaret();
		}
		public static void HideCaretApi(IntPtr handle) {
			HideCaret(handle);
		}
		public static void ShowCaretApi(IntPtr handle) {
			ShowCaret(handle);
		}

	}
}
