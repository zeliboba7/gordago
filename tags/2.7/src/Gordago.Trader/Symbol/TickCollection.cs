/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago {
	public class TickCollection:IEnumerable {

		private Tick[] _ticks;
		private int _size;

		public TickCollection():this(4096) {}
		
		#region public TickCollection(int counttick) 
		public TickCollection(int counttick) {
			int sz = CalculSize(counttick);
			_ticks = new Tick[sz];
			_size = 0;
		}
		#endregion

		#region public TickCollection(Tick[] ticks)
		public TickCollection(Tick[] ticks){
			_size = ticks.Length;
			int sz = CalculSize(ticks.Length);
			_ticks = new Tick[sz];
			Array.Copy(ticks, 0, _ticks, 0, ticks.Length);
		}
		#endregion

		#region public Tick this[int index]
		public Tick this[int index]{
			get{return this._ticks[index];}
		}
		#endregion

		#region public int Count
		public int Count{
			get{return this._size;}
		}
		#endregion

		#region public Tick Current
		public Tick Current{
			get{return this._ticks[_size - 1];}
			set{this._ticks[_size - 1] = value;}
		}
		#endregion

		#region internal int Capacity
		internal int Capacity {
			get{return this._ticks.Length;}
		}
		#endregion

		#region internal Tick[] Ticks
		internal Tick[] Ticks{
			get{return this._ticks;}
		}
		#endregion

		#region public void Add(Tick tick) 
		public void Add(Tick tick) {
			if ( _size == _ticks.Length  ) {
				Tick[] temp = new Tick[_ticks.Length * 2];
				Array.Copy(_ticks, 0, temp, 0, _ticks.Length);
				_ticks = temp;
			}
			_ticks[_size++] = tick;
		}
		#endregion

		#region public void AddRange(Tick[] ticks)
		public void AddRange(Tick[] ticks){
			Tick[] newticks = new Tick[this.Capacity + CalculSize(ticks.Length)];
			Array.Copy(this._ticks, 0, newticks, 0, _size);
			Array.Copy(ticks, 0, newticks, _size, ticks.Length);
			this._ticks = newticks;
			this._size += ticks.Length;
		}
		#endregion

		#region public void AddRange(TickCollection ticks)
		public void AddRange(TickCollection ticks){
			Tick[] newticks = new Tick[this.Capacity + ticks.Capacity];
			Array.Copy(this._ticks, 0, newticks, 0, _size);
			Array.Copy(ticks.Ticks, 0, newticks, _size, ticks.Count);
			this._ticks = newticks;
			this._size += ticks.Count;
		}
		#endregion

		#region public void InsertRange(Tick[] ticks)
		/// <summary>
		/// Вставка тиков в начало массива
		/// </summary>
		public void InsertRange(Tick[] ticks){
			Tick[] newticks = new Tick[this.Capacity + CalculSize(ticks.Length)];
			Array.Copy(ticks, 0, newticks, 0, ticks.Length);
			Array.Copy(this._ticks, 0, newticks, ticks.Length, _size);
			this._ticks = newticks;
			this._size += ticks.Length;
		}
		#endregion

		#region public void InsertRange(Tick[] ticks, int index)
		public void InsertRange(Tick[] ticks, int index){
			Tick[] newticks = new Tick[this.Capacity + CalculSize(ticks.Length)];
			Array.Copy(_ticks, 0, newticks, 0, index);
			Array.Copy(ticks, 0, newticks, index, ticks.Length);
			Array.Copy(_ticks, index, newticks, index+ticks.Length, _size - index);
			this._size += ticks.Length;
		}
		#endregion

		#region public void RemoveFromIndex(int index) - Удаление данных, начиная с индекса
		/// <summary>
		/// Удаление данных, начиная с индекса
		/// </summary>
		/// <param name="index"></param>
		public void RemoveFromIndex(int index){
			_size = index;
		}
		#endregion

		#region public void SetRange(Tick[] ticks)
		public void SetRange(Tick[] ticks){
			if (ticks.Length > this.Capacity){
				int sz = CalculSize(ticks.Length);
				_ticks = new Tick[sz];
			}
			Array.Copy(ticks, 0, _ticks, 0, ticks.Length);
			_size = ticks.Length;
		}
		#endregion

		#region public void Clear()
		public void Clear(){
			_size = 0;
		}
		#endregion

		#region public Tick[] GetTicks()
		public Tick[] GetTicks(){
			return GetTicks(0, _size);
		}
		#endregion

		#region public Tick[] GetTicks(int lenght)
		public Tick[] GetTicks(int lenght){
			return GetTicks(0, lenght);
		}
		#endregion

		#region public Tick[] GetTicks(int startIndex, int endIndex)
		public Tick[] GetTicks(int startIndex, int endIndex){
			int sz = endIndex - startIndex;
			Tick[] ticks = new Tick[sz];
			Array.Copy(this._ticks, startIndex, ticks, 0, sz);
			return ticks;
		}
		#endregion

		#region private static int CalculSize(int lenght)
		private static int CalculSize(int lenght){
			return ((lenght / 2048) + 1) * 2048;
		}
		#endregion

		#region public IEnumerator GetEnumerator() 
		public IEnumerator GetEnumerator() {
			return new TicksEnumerator(this);
		}
		#endregion

		#region private class TicksEnumerator:IEnumerator 
		private class TicksEnumerator:IEnumerator {

			private TickCollection _parent;
			private int _index = -1;

			public TicksEnumerator(TickCollection ticks){
				_parent = ticks;
				_index = -1;
			}

			#region public void Reset() 
			public void Reset() {
				_index = -1;
			}
			#endregion

			#region public object Current 
			public object Current {
				get {
					return _parent._ticks[_index];
				}
			}
			#endregion

			#region public bool MoveNext() 
			public bool MoveNext() {
				_index++;
				return _index < _parent.Count;
			}
			#endregion
		}
		#endregion
	}
}
