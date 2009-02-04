/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;

namespace Gordago {
	public class TickWriter {
		private static long UNIXTICKS = new DateTime(1970, 1,1,0,0,0,0).Ticks;
		private TickFileInfo _tfi;
		private BinaryWriter _bwriter;

		public TickWriter(TickFileInfo tfi) {
			this._tfi = tfi;
			FileStream fs = new FileStream(tfi.FileName, FileMode.Open);
			_bwriter = new BinaryWriter(fs);
			fs.Seek(TickFileInfo.GetOffset(0), SeekOrigin.Begin);
		}

		#region public TickFileInfo Info
		public TickFileInfo Info{
			get{return this._tfi;}
		}
		#endregion

		#region internal long Position
		internal long Position{
			get{return (_bwriter.BaseStream.Position -  TickFileInfo.INFO_SIZE) / TickFileInfo.TICK_SIZE;}
		}
		#endregion

		#region public void SetOffset(int indextick)
		public void SetOffset(int indextick){
			long posinfile = TickFileInfo.GetOffset(indextick);
			if (posinfile > _bwriter.BaseStream.Length)
				throw(new Exception(string.Format("TickWriter.SetOffset({0}): Position is more than lengh file", indextick)));
			_bwriter.BaseStream.Seek(posinfile, SeekOrigin.Begin);
		}
		#endregion

		#region public void Write(Tick tick, int index)
		public void Write(Tick tick, int index){
			long offset = TickFileInfo.GetOffset(index);

			if (offset != _bwriter.BaseStream.Position)
				throw(new Exception(string.Format("offset={0} != TickWriter.Write.index={1}", offset, index)));

			uint utime = (UInt32)((tick.Time - UNIXTICKS)/10000000);
			_bwriter.Write(utime);
			_bwriter.Write(tick.Price);

			if (index == 0){
				_tfi.SetFromTime(tick.Time);
			}
			_tfi.SetToTime(tick.Time);
		}
		#endregion

		#region public void Close()
		public void Close(){
			_bwriter.BaseStream.Seek(TickFileInfo.INFO_FILE_SIZE, SeekOrigin.Begin);

      _tfi.CountTick = Convert.ToInt32((_bwriter.BaseStream.Length - TickFileInfo.INFO_SIZE) / 8);

			_bwriter.Write(_tfi.CountTick);
			_bwriter.Write(SymbolManager.ConvertDateTimeToUnix(new DateTime(_tfi.FromDateTime)));
      _bwriter.Write(SymbolManager.ConvertDateTimeToUnix(new DateTime(_tfi.ToDateTime)));
			_bwriter.Flush();

      //  string.Format("Запись {0}: counttick = {1}, from = {2}, to = {3}", _tfi.SymbolName, _tfi.CountTick, new DateTime(_tfi.FromDateTime), new DateTime(_tfi.ToDateTime)));

			/* кол-во рельных тиков */
//			System.Diagnostics.Debug.WriteLine(string.Format("Кол-во тиков по описателю={0}, кол-во тиков по файлу={1}", _tfi.CountTick, counttickreal));

			_bwriter.Close();
		}
		#endregion
	}
}
