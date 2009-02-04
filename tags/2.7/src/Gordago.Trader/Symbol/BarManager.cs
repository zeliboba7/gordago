/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using
using System;
using System.IO;
using System.Collections;
#endregion

namespace Gordago {
	/// <summary>
	/// Клас по работе с барами. 
	/// Мапинг по файлу, если этот таймфрейм являеться базовым, 
	/// иначе мапинг по базовому таймфрейму
	/// </summary>
	class BarManager:IEnumerable, IBarList {

    internal const string DIRECTORY_TIMEFRAME = "timeframe";
		internal const string EXTENSION_FILE = "gtf";
		
		private TickManager _tmanager;
		private TimeFrame _tf;

		private string _filenamehistory, _filenamecache;

		private BarFileBuffer _bfbhistory, _bfbcache;
		private BarCollection _onlinebars;

    public BarManager(TickManager tickmanager, TimeFrame tf) {
			_tmanager = tickmanager;
			_tf = tf;

			string fname = _tmanager.Symbol.Name + tf.Second.ToString() + "." + EXTENSION_FILE;
			_filenamehistory = _tmanager.Symbol.DirHistory + "\\" + DIRECTORY_TIMEFRAME + "\\" + fname;
			_filenamecache = _tmanager.Symbol.DirCache + "\\" + DIRECTORY_TIMEFRAME + "\\" + fname;

			_bfbhistory = new BarFileBuffer(_tmanager.Symbol, tf, _filenamehistory);
			_bfbcache = new BarFileBuffer(_tmanager.Symbol, tf, _filenamecache);
			_onlinebars = new BarCollection(tf);
    }

    #region public TickManager TickManager
    public TickManager TickManager {
      get { return this._tmanager; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get {
        if (this.TickManager.Busy)
          return TickFileInfo.EMPTY_DATETIME;
        if (this.BufferHistory.Info.CountBar > 0) {
          return new DateTime(this.BufferHistory.Info.FromDateTime);
        } else if (this.BufferCache.Info.CountBar > 0) {
          return new DateTime(this.BufferCache.Info.FromDateTime);
        }
        return TickFileInfo.EMPTY_DATETIME;
      }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get {
        if (this.TickManager.Busy)
          return TickFileInfo.EMPTY_DATETIME;

        if (this._onlinebars.Count > 0) {
          return this._onlinebars[_onlinebars.Count - 1].Time;
        }
        if(this.BufferCache.Info.CountBar > 0) {
          return new DateTime(this.BufferCache.Info.ToDateTime);
        } else if(this.BufferHistory.Info.CountBar > 0) {
          return new DateTime(this.BufferHistory.Info.ToDateTime);
        }
        return TickFileInfo.EMPTY_DATETIME;
      }
    }
    #endregion

		#region internal BarFileBuffer BufferHistory
		internal BarFileBuffer BufferHistory{
			get{return _bfbhistory;}
		}
		#endregion

		#region internal BarFileBuffer BufferCache
		internal BarFileBuffer BufferCache{
			get{return this._bfbcache;}
		}
		#endregion

		#region public IEnumerator GetEnumerator()
		public IEnumerator GetEnumerator() {
			return new BarEnumerator(this);
		}
		#endregion

		#region private class BarEnumerator:IEnumerator 
		private class BarEnumerator:IEnumerator {

			private BarManager _bmanager;
			private int _index;

			public BarEnumerator(BarManager bmanager){
				_bmanager = bmanager;
				this.Reset();
			}

			public void Reset() {
				_index = -1;
			}

			public object Current {
				get {
					return _bmanager[_index];
				}
			}

			public bool MoveNext() {
				_index++;
				return _index < _bmanager.Count;
			}
		}
		#endregion

		#region public TimeFrame TimeFrame
		public TimeFrame TimeFrame{
			get{return this._tf;}
		}
		#endregion

    #region public int Count
    public int Count{
      get {
        if (this.TickManager.Busy)
          return 0;

        int bcnt = _bfbhistory.Info.CountBar + _bfbcache.Info.CountBar;
        if(this._onlinebars.Count > 0) {
          if(bcnt > 0)
            return bcnt - 1 + this._onlinebars.Count;
          return this._onlinebars.Count;
        }
        return bcnt;
      }
		}
		#endregion

    #region public Bar Current
    public Bar Current {
      get {
        if (this.TickManager.Busy)
          return new Bar(0, 0, 1, 0, 1, TickFileInfo.EMPTY_DATETIME);

        return this[this.Count - 1]; 
      }
    }
    #endregion

    #region internal Symbol Symbol
    internal Symbol Symbol{
			get{return this._tmanager.Symbol;}
		}
		#endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
        if (this.TickManager.Busy)
          return new Bar(0, 0, 1, 0, 1, TickFileInfo.EMPTY_DATETIME);

        int cntonline = this._onlinebars.Count;
        int cnthistory = _bfbhistory.Info.CountBar;
        int cntcache = this._bfbcache.Info.CountBar;

        if (cntonline > 0) {
          if (cntcache > 0) {
            if (index < cnthistory)
              return _bfbhistory.GetBar(index);
            int realindex = index - cnthistory;
            if (realindex >= cntcache - 1)
              return this._onlinebars[realindex - cntcache + 1];
            return _bfbcache.GetBar(realindex);
          } else if (cnthistory > 0) {
            if (index >= cnthistory - 1)
              return this._onlinebars[index - cnthistory + 1];
            return this._bfbhistory.GetBar(index);
          }
          return this._onlinebars[index];
        } else {
          if (index < cnthistory)
            return _bfbhistory.GetBar(index);
          int cindex = index - cnthistory;
          //if (cindex >= _bfbcache.Info.CountBar)
          //  return new Bar();
          return _bfbcache.GetBar(cindex);
        }
      }
    }
    #endregion

    #region internal void AddTick(Tick tick)
    /// <summary>
		/// Добавление Online тика
		/// </summary>
		/// <param name="tick"></param>
		internal void AddTick(Tick tick){
      if (this.TickManager.Busy)
        return;
			if (this._onlinebars.Count == 0)
				this.DataCaching();
			this._onlinebars.Add(tick);
		}
		#endregion

		#region internal void DataCaching()
		internal void DataCaching(){
			this._onlinebars.Clear();
			
			if (this._tmanager.OnlineTicks.Count == 0)  return;

			if (this._bfbcache.Info.CountBar > 0){
				this._onlinebars.Add(this._bfbcache.GetBar(_bfbcache.Info.CountBar - 1));
			}else if (this._bfbhistory.Info.CountBar > 0){
				this._onlinebars.Add(this._bfbhistory.GetBar(_bfbhistory.Info.CountBar - 1));
			}
			this._onlinebars.Add(this._tmanager.OnlineTicks);
		}
		#endregion

    #region public int GetBarIndex(DateTime time)
    /// <summary>
    /// Получить индек Бара в пределах времени.
    /// Если время не попадает в период таймсерии, то вычисление 
    /// предпологаемого индекса
    /// </summary>
    /// <param name="time">Время</param>
    /// <returns>Индекс</returns>
    public int GetBarIndex(DateTime time) {
      if (this.TickManager.Busy)
        return 0;
      
      if(this.Count == 0) return 0;

      long periodFrom = this.TimeFrom.Ticks;
      long periodTo = this.TimeTo.Ticks;
      long step = this.TimeFrame.Second ;

      if(periodFrom <= time.Ticks && time.Ticks <= periodTo) { /* время входит в период таймсерии */

        long delim = (periodTo - time.Ticks) / (step * 10000000L);

        int delta = (int)Math.Max((delim), 0);

        int count = this.Count;

        delta = Math.Max(count - delta-100, 0);
        int sec = this.TimeFrame.Second;
        int retIndex = count-1;
        for(int i = delta; i < count; i++) {
          Bar bar = this[i];
          if (bar.Time.Ticks > time.Ticks)
            return i - 1;
        }
        return retIndex;
      } else if(time.Ticks < periodFrom) { /* время меньше периода */
        long delta = (periodFrom - time.Ticks) / (step * 10000000L);
        return 0 - Convert.ToInt32(delta);
      } else {
        long delta = (time.Ticks - periodTo) / (step * 10000000L);
        return this.Count + Convert.ToInt32(delta);
      }
    }
    #endregion

#if DEBUG
    public int RealIndex(int index) {
      return index;
    }
#endif
  }

  #region class BarCollection:IBarList
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
      long time = 0;

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
        time = this.TimeFrame.Calculator != null ? this.TimeFrame.Calculator.GetRoundTime(bar.Time).Ticks : this.GetRoundTime(bar.Time.Ticks);

      if(newbar) {
        _bars[_size] = bar;
        _bars[_size].Time = new DateTime(time);
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

    #region private long GetRoundTime(long time)
    private long GetRoundTime(long time) {
      int second = this.TimeFrame.Second;
      long nt = (time / 10000000L) / second;
      return (nt * second) * 10000000L;
    }
    #endregion

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

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { throw new Exception("The method or operation is not implemented."); }
    }
    #endregion

    #region IBarList Members


    public int GetBarIndex(DateTime time) {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion

#if DEBUG
    public int RealIndex(int index) {
      return index;
    }
#endif
  }
  #endregion
}
