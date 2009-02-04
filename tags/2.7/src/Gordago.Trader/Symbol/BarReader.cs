/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Threading;

namespace Gordago {

	internal class BarReader {
		private BinaryReader _breader;
		private BarFileInfo _bfi;
		private static long UNIXTICKS = new DateTime(1970, 1,1,0,0,0,0).Ticks;

    private long _savedIndex = -1;
    private Bar _savedBar;
    private bool _synchron = false;

		public BarReader(BarFileInfo bfi) {
			_bfi = bfi;
			FileStream fs = new FileStream(_bfi.FileName, FileMode.Open, FileAccess.Read,FileShare.Read);
			_breader = new BinaryReader(fs);
			_breader.BaseStream.Seek(BarFileInfo.INFO_SIZE, SeekOrigin.Begin);
		}

		#region public Bar Read(int index)
		public Bar Read(int index){

      if (index == _savedIndex)
        return _savedBar;

      while (_synchron) {
        Thread.Sleep(1);
      }
      _synchron = true;

			long offset = BarFileInfo.GetOffset(index);
			if (_breader.BaseStream.Position != offset)
				_breader.BaseStream.Seek(offset, SeekOrigin.Begin);

			long time = UNIXTICKS + (long)_breader.ReadUInt32() * 10000000;
			float open = _breader.ReadSingle();
			float high = _breader.ReadSingle();
			float low = _breader.ReadSingle();
			float close = _breader.ReadSingle();
			int volume = _breader.ReadInt32();

      Bar bar = new Bar(open, low, high, close, volume, time);
      _savedBar = bar;
      _savedIndex = index;
      _synchron = false;

      return bar;
		}
		#endregion

		#region public void Close()
		public void Close(){
			_breader.Close();
		}
		#endregion
	}
}
