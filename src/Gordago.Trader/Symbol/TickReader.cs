/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Threading;

namespace Gordago {

	#region internal class TickReaderBuffer 
	internal class TickReaderBuffer {
		private const int BUFFER_SIZE = 4096;

		private Tick[] _ticks;
		private int _size;

		/// <summary>
		/// смещение относительно файла
		/// </summary>
		private int _offset;

		public TickReaderBuffer() {
			_ticks = new Tick[BUFFER_SIZE];
			_offset = _size = 0;
		}
		
		#region public int Size
		public int Size{
			get{return this._size;}
		}
		#endregion

		#region public int OffSet
		/// <summary>
		/// смещение относительно файла
		/// </summary>
		public int OffSet{
			get{return this._offset;}
		}
		#endregion

		#region public bool IsFull
		/// <summary>
		/// Заполнен ли буфер
		/// </summary>
		public bool IsFull{
			get{return _size == BUFFER_SIZE;}
		}
		#endregion

		#region public Tick this[int index]
		public Tick this[int index]{
			get{return this._ticks[index];}
		}
		#endregion

		#region public void Add(Tick tick)
		public void Add(Tick tick) {
			_ticks[_size++] = tick;
		}
		#endregion

		#region public void ClearAndInitialize(int offset)
		public void ClearAndInitialize(int offset){
			_size = 0;
			_offset = offset;
		}
		#endregion

	}
	#endregion

	internal class TickReader {
		private static long UNIXTICKS = new DateTime(1970, 1,1,0,0,0,0).Ticks;

		private BinaryReader _breader;
		private TickFileInfo _tfi;
		private TickReaderBuffer _buffer;
    private bool _synchron = false;

		public TickReader(TickFileInfo tfi) {
      
			_tfi = tfi;
			_buffer = new TickReaderBuffer();
      this.CheckReader();
      //_breader = new BinaryReader(fs);
      //_breader.BaseStream.Seek(TickFileInfo.INFO_SIZE, SeekOrigin.Begin);
		}

    private void CheckReader() {
      if (_breader != null)
        return;
      FileStream fs = new FileStream(_tfi.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      _breader = new BinaryReader(fs);
      _breader.BaseStream.Seek(TickFileInfo.INFO_SIZE, SeekOrigin.Begin);
      System.Diagnostics.Debug.WriteLine("CheckReader");
    }

		#region public Tick Read(int index)
		/// <summary>
		/// Чтение тика.
		/// Если тик не находиться в буфере, то считываеться данные в буфер
		/// </summary>
		/// <param name="index">Номер в файле </param>
		/// <returns></returns>
    public Tick Read(int index) {
      while (_synchron) {
        Thread.Sleep(1);
      }
      _synchron = true;

      this.CheckReader();
      long offset = TickFileInfo.INFO_SIZE + index * 8;
      if (_breader.BaseStream.Position != offset)
        _breader.BaseStream.Seek(offset, SeekOrigin.Begin);

      uint utime = _breader.ReadUInt32();
      float bid = _breader.ReadSingle();
      long ltime = UNIXTICKS + (long)utime * 10000000L;

      _synchron = false;

      return new Tick(ltime, bid);

      /* Передумать этот метод !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! *
       * В нем есть ОШИБКА !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! *
       */

      //if (_buffer.OffSet <= index && index < _buffer.OffSet + _buffer.Size)
      //  return _buffer[index - _buffer.OffSet];

      //_buffer.ClearAndInitialize(index);

      //long offset = TickFileInfo.INFO_SIZE + index * 8;
      //if (_breader.BaseStream.Position != offset)
      //  _breader.BaseStream.Seek(offset, SeekOrigin.Begin);

      //while (_breader.BaseStream.Position < _breader.BaseStream.Length) {
      //  uint utime = _breader.ReadUInt32();
      //  float bid = _breader.ReadSingle();
      //  long ltime = UNIXTICKS + (long)utime * 10000000L;

      //  _buffer.Add(new Tick(ltime, bid));
      //  if (_buffer.IsFull) break;
      //}

      //if (_buffer.Size == 0)
      //  throw (new IndexOutOfRangeException(string.Format("Index={0} out of range in TickReader.Read")));

      //return this.Read(index);
    }
		#endregion

    #region public void Close()
    public void Close(){
      while (_synchron) {
        Thread.Sleep(1);
      }
      _synchron = true;

      //System.Diagnostics.Debug.WriteLine("Close");
			_breader.Close();
			_breader = null;
      _synchron = false;
    }
    #endregion
  }
}
