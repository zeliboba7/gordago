/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago {

	internal class TickBuffer {


		private int _offset = 0;
		private int _size;
		private Tick[] _ticks;

		public TickBuffer() {
      _ticks = new Tick[TickManager.BUFFER_SIZE];
			_offset = _size = 0;
		}

		#region public int Size
		public int Size{
			get{return this._size;}
		}
		#endregion

		#region public int OffSet
		public int OffSet{
			get{return this._offset;}
			set{this._offset = value;}
		}
		#endregion

		#region public int FirstIndex
		public int FirstIndex{
			get{return this._offset;}
		}
		#endregion

		#region public int EndIndex
		public int EndIndex{
			get{return this._offset + this._size-1;}
		}
		#endregion

		#region public bool Full
		public bool Full{
      get { return _size == TickManager.BUFFER_SIZE; }
		}
		#endregion

		#region public Tick this[int index]
		public Tick this[int index]{
			get{return this._ticks[index - this._offset];}
		}
		#endregion

		#region public void Add(Tick tick)
		public void Add(Tick tick){
			_ticks[_size++] = tick;
		}
		#endregion

		#region public void MoveNextOffset()
    /// <summary>
    /// Запись в буфер следующей партии тиков
    /// </summary>
		public void MoveNextOffset(){
      Tick[] tticks = new Tick[TickManager.BUFFER_SIZE];
      int offset = TickManager.OFFSET_SIZE;
      Array.Copy(_ticks, offset, tticks, 0, TickManager.BUFFER_MIN_SIZE);
			_ticks = tticks;
			_offset += offset;
      _size = TickManager.BUFFER_MIN_SIZE;
		}
		#endregion

    /// <summary>
    /// Обычно это в случае, когда чтение тиков идет в обратном порядке.
    /// </summary>
    //public void MoveLastOffset() {
    //  Tick[] tticks = new Tick[TickManager.BUFFER_SIZE];

    //}

		#region public void Clear()
		public void Clear(){
			_offset = _size = 0;
		}
		#endregion

		#region public void Clear(int offset)
		public void Clear(int offset){
			_offset = offset;
			_size = 0;
		}
		#endregion
	}
}
