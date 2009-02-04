/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.Analysis {
  class BarCollection:IBarList {

    private Bar[] _bars;
    private int _size;
    private TimeFrame _timeframe;

    public BarCollection(TimeFrame timeframe) {
      _size = 0;
      _timeframe = timeframe;
      _bars = new Bar[2048];
    }

    #region public int Size
    public int Count {
      get { return this._size; }
    }
    #endregion

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return this._timeframe; }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get { return this._bars[_size - 1]; }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get { return this._bars[index]; }
    }
    #endregion

    #region public void Add(Tick tick)
    public void Add(Tick tick) {
      this.Add(new Bar(tick.Price, tick.Price, tick.Price, tick.Price, 1, tick.Time));
    }
    #endregion

    #region public void Add(TickCollection ticks)
    public void Add(TickCollection ticks) {
      for(int i = 0; i < ticks.Count; i++) {
        this.Add(ticks[i]);
      }
    }
    #endregion

    #region public void Add(Bar bar)
    public void Add(Bar bar) {
      this.AddMethod(bar);
    }
    #endregion

    #region private void AddMethod(Bar bar)
    private void AddMethod(Bar bar) {
      if(_size == _bars.Length) {
        Bar[] newbars = new Bar[_bars.Length * 2];
        Array.Copy(_bars, 0, newbars, 0, _size);
        _bars = newbars;
      }

      bool newbar = false;
      DateTime time = DateTime.Now;

      if(_size == 0) {
        newbar = true;
      } else {

        if(this.TimeFrame.Calculator != null) {
          newbar = this.TimeFrame.Calculator.CheckNewBar(this.Current, bar.Time);
        } else {
          long sec1 = bar.Time.Ticks / 10000000L / _timeframe.Second;
          long sec2 = Current.Time.Ticks / 10000000L / _timeframe.Second;
          newbar = sec1 - sec2 > 0;
        }
      }

      if(newbar)
        time = this.TimeFrame.Calculator != null ? this.TimeFrame.Calculator.GetRoundTime(bar.Time) : this.GetRoundTime(bar.Time);

      if(newbar) {
        _bars[_size] = bar;
        _bars[_size].Time = time;
        _size++;
      } else {
        int endIndex = _size - 1;
        _bars[endIndex].Close = bar.Close;
        _bars[endIndex].Volume += bar.Volume;
        if(_bars[endIndex].Low > bar.Low)
          _bars[endIndex].Low = bar.Low;
        if(_bars[endIndex].High < bar.High)
          _bars[endIndex].High = bar.High;
      }
    }
    #endregion

    private DateTime GetRoundTime(DateTime time) {
      int second = this.TimeFrame.Second;
      long nt = (time.Ticks / 10000000L) / second;
      return new DateTime((nt * second) * 10000000L);
    }

    #region public void Clear()
    public void Clear() {
      _size = 0;
    }
    #endregion

    #region private static int CalculSize(int lenght)
    private static int CalculSize(int lenght) {
      return ((lenght / 2048) + 1) * 2048;
    }
    #endregion

    #region public void RemoveFirstElements(int count)
    /// <summary>
    /// Удаление определенного кол-ва баров с начала
    /// </summary>
    /// <param name="count">кол-во баров</param>
    public void RemoveFirstElements(int count) {
      Bar[] tbars = new Bar[_bars.Length];
      Array.Copy(_bars, count, tbars, 0, _size - count);
      _bars = tbars;
      _size = _size - count;
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { throw new Exception("The method or operation is not implemented."); }
    }
    #endregion

    public DateTime TimeTo {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public int GetBarIndex(DateTime time) {
      throw new Exception("The method or operation is not implemented.");
    }
  }

	internal class TestBars: IBarList {

		private const int MAX_BUFFER_SIZE = 16384;
		private const int MIN_BUFFER_SIZE = 4096;

		private BarCollection _bars;
		private int _offset;
		private TimeFrame _tf;
		
		public TestBars(TimeFrame tf){
			_tf = tf;
			_bars = new BarCollection(tf);
		}

		#region public TimeFrame TimeFrame
		public TimeFrame TimeFrame{
			get{return this._tf;}
		}
		#endregion

		#region public int Size 
		public int Count {
			get {
				return _bars.Count+_offset;
			}
		}
		#endregion

		#region public Bar this[int index] 
		public Bar this[int index] {
			get {return _bars[Math.Max(index - _offset, 0)];}
		}
		#endregion

		#region public Bar Current 
		public Bar Current {
			get {
				return _bars.Current;
			}
		}
		#endregion

		#region public void Clear()
		public void Clear(){
			_bars.Clear();
			_offset = 0;
		}
		#endregion

		#region public void Add(Tick tick)
		public void Add(Tick tick){
			_bars.Add(tick);
			if (_bars.Count > MAX_BUFFER_SIZE){
				_bars.RemoveFirstElements(MIN_BUFFER_SIZE);
				_offset += MIN_BUFFER_SIZE;
			}
		}
		#endregion

    #region IBarList Members


    public DateTime TimeFrom {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public DateTime TimeTo {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public int GetBarIndex(DateTime time) {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
