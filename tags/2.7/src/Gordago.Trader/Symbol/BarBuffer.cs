/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago {
	internal class BarBuffer{
		public const int BUFFER_SIZE = 6144;
		public const int BUFFER_MIN_SIZE = 4096;
		public const int OFFSET_SIZE = BUFFER_SIZE-BUFFER_MIN_SIZE;

		private int _offset = 0;
		private int _size;
		private Bar[] _bars;
		
    private int _tfsec;
    private TimeFrame _timeframe;
		
		public BarBuffer(int tfsecond){
      _timeframe = TimeFrameManager.TimeFrames.GetTimeFrame(tfsecond);
			_bars = new Bar[BUFFER_SIZE];
			_offset = _size = 0;
			_tfsec = tfsecond;
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
			get{return _size == BUFFER_SIZE;}
		}
		#endregion

		#region public Bar this[int index]
		public Bar this[int index]{
			get{
				return _bars[index - this._offset];
			}
		}
		#endregion

		#region public Bar Current
		public Bar Current{
			get{return this._bars[_size-1];}
			set{this._bars[_size-1] = value;}
		}
		#endregion

		#region public void AddBar(Bar bar)
		public void AddBar(Bar bar){
			_bars[_size++] = bar;
		}
		#endregion

		#region public void AddTick(Tick tick)
		public void AddTick(Tick tick){

			bool flagnewbar = false;
      long time = 0;
      
      if(_size == 0) {
        flagnewbar = true;
      } else {
        if(this._timeframe.Calculator != null) {
          flagnewbar = this._timeframe.Calculator.CheckNewBar(this.Current, new DateTime(tick.Time));
        } else {
          long sec1 = tick.Time / 10000000L / _tfsec;
          long sec2 = this.Current.Time.Ticks / 10000000L / _tfsec;
          flagnewbar = sec1 - sec2 > 0;
        }
      }

      if(flagnewbar)
        time = this._timeframe.Calculator != null ? this._timeframe.Calculator.GetRoundTime(new DateTime(tick.Time)).Ticks : this.GetRoundTime(tick.Time);

			if (flagnewbar){
        Bar bar = new Bar(tick.Price, tick.Price, tick.Price, tick.Price, 1, time);

				this.AddBar(bar);
			}else{
				int index = _size-1;
				float price = tick.Price;
				_bars[index].Close = price;
				_bars[index].Volume++;
				
        if ( _bars[index].Low > price)
					_bars[index].Low = price;
				if ( _bars[index].High < price)
					_bars[index].High = price;
			}
		}
		#endregion

    #region private long GetRoundTime(long time)
    private long GetRoundTime(long time) {
      long nt = (time / 10000000L) / _tfsec;
      return (nt * _tfsec) * 10000000L;
    }
    #endregion

    #region public void MoveNextOffset()
    public void MoveNextOffset(){
			Bar[] tbars = new Bar[BUFFER_SIZE];
			int offset = OFFSET_SIZE;
			Array.Copy(_bars, offset, tbars, 0, BUFFER_MIN_SIZE);
			_bars = tbars;
			_offset += offset;
			_size = BUFFER_MIN_SIZE;
		}
		#endregion

		#region public void Clear()
		public void Clear(){
			_offset = _size = 0;
		}
		#endregion
	}
}
