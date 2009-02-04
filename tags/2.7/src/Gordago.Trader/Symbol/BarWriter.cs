/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;

namespace Gordago {
	/// <summary>
	/// Запись БарДанных на диск
	/// </summary>
	class BarWriter {
		private static long UNIXTICKS = new DateTime(1970, 1,1,0,0,0,0).Ticks;
		private BarFileInfo _bfi;
		private BinaryWriter _bwriter;

		public BarWriter(BarFileInfo bfi) {
			_bfi = bfi;
			FileStream fs = new FileStream(bfi.FileName, FileMode.Open);
			_bwriter = new BinaryWriter(fs);
			fs.Seek(BarFileInfo.GetOffset(0), SeekOrigin.Begin);
		}

		internal long Position{
			get{return (_bwriter.BaseStream.Position -  BarFileInfo.INFO_SIZE) / BarFileInfo.BAR_SIZE;}
		}

		#region public void Write(Bar bar, int index)
		public void Write(Bar bar, int index){
			long offset = BarFileInfo.GetOffset(index);

			if (offset != _bwriter.BaseStream.Position)
				throw(new Exception(string.Format("offset={0} != BarWriter.Write.index={1}", offset, index)));

			uint utime = (UInt32)((bar.Time.Ticks - UNIXTICKS)/10000000L);

			_bwriter.Write(utime);
			_bwriter.Write(bar.Open);
			_bwriter.Write(bar.High);
			_bwriter.Write(bar.Low);
			_bwriter.Write(bar.Close);
			_bwriter.Write(bar.Volume);
			
			_bfi.CountBar++;
			if (index == 0){
				_bfi.FromDateTime = bar.Time.Ticks;
			}
			_bfi.ToDateTime = bar.Time.Ticks;
		}
		#endregion

		#region public void Close()
		public void Close(){

      _bfi.WriteInfo(_bwriter);

			_bwriter.Flush();
			_bwriter.Close();
		}
		#endregion
	}
}
