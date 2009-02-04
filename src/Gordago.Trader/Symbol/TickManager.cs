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

using System.Threading;

namespace Gordago {

  #region public enum TickManagerStatus
  public enum TickManagerStatus {
    Default,
    DataCachingStarting,
    DataCaching,
    UpdateStarting,
    Updating
    
  }
  #endregion

  /// <summary>
	/// Класс, работающий с массивами тиковых данных.
	/// Массив данных состоит из двух типов:
	/// 1) Исторические данные без дыр.
	/// 2) Кещ данные, с возможными дырами.
	/// </summary>
	class TickManager:IEnumerable, ITickList, ITickManager {
	
    private const int DEFAULT_BUFFER_SIZE = 16384;
    private const int DEFAULT_BUFFER_MIN_SIZE = 12288;

    private static int _buffer_size = DEFAULT_BUFFER_SIZE;
    private static int _buffer_min_size = DEFAULT_BUFFER_MIN_SIZE;

    #region internal static int BUFFER_SIZE
    /// <summary>
    /// Размер буфера
    /// </summary>
    internal static int BUFFER_SIZE {
      get { return _buffer_size; }
      set {
        _buffer_size = Math.Max(DEFAULT_BUFFER_SIZE, value);
      }
    }
    #endregion

    #region internal static int BUFFER_MIN_SIZE
    /// <summary>
    /// Минимальный размер буфера
    /// </summary>
    internal static int BUFFER_MIN_SIZE {
      get { return _buffer_min_size; }
      set {
        _buffer_min_size = Math.Max(Math.Max(_buffer_min_size, value), BUFFER_SIZE);
      }
    }
    #endregion

    #region internal static int OFFSET_SIZE
    internal static int OFFSET_SIZE {
      get {
        return BUFFER_SIZE - BUFFER_MIN_SIZE;
      }
    }
    #endregion

    //public event TickManagerHandler DataCachingStopping;
    //public event TickManagerProcessHandler DataCachingProcess;

    public event TickManagerEventHandler DataCachingChanged;

		private Symbol _symbol;
		private BarManager[] _bars;

		private TickFileInfo _tfihistory, _tficache;
		private TickFileBuffer _tfbhistory, _tfbcache;
		private TickFileMap _maphistory, _mapcache;
		private TickCollection _onlineticks;

//		private bool _usedatacachingevent = true;

    private TickManagerStatus _status;

    private int _sessionID = 1, _savedSessionId = 0;

		internal TickManager(Symbol symbol) {
			_symbol = symbol;
			_bars = new BarManager[]{};
			_onlineticks = new TickCollection();
			_status = TickManagerStatus.Default;
    }

    #region public bool IsDataCaching
    public bool IsDataCaching {
      get {
        bool isCaching = this._sessionID == _savedSessionId;
        if (!isCaching) {
          isCaching = !this.CheckDataCaching();
          if (isCaching) {
            _savedSessionId = _sessionID;
          }
        }
        return isCaching; 
      }
    }
    #endregion

    #region public bool Busy
    public bool Busy {
      get {
        return this._status != TickManagerStatus.Default;
      }
    }
    #endregion

    #region public IBarList[] BarLists
    public IBarList[] BarLists {
      get { return _bars; }
    }
    #endregion

    #region public int Count
    public int Count{
			get{
        if (this.Busy)
          return 0;
				return this._tfbhistory.Info.CountTick + this._tfbcache.Info.CountTick + this._onlineticks.Count;
			}
		}
		#endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom{
			get{
        if (this.Busy)
          return DateTime.Now;

				if (InfoHistory.CountTick > 0){
					return new DateTime(InfoHistory.FromDateTime);
				}else if (InfoCache.CountTick > 0){
					return new DateTime(InfoCache.FromDateTime);
				}
				return TickFileInfo.EMPTY_DATETIME;
			}
		}
		#endregion

    #region public DateTime TimeTo
    public DateTime TimeTo{
			get{
        if (this.Busy)
          return DateTime.Now;

				if (InfoCache.CountTick > 0){
					return new DateTime(InfoCache.ToDateTime);
				}else if (InfoHistory.CountTick > 0){
					return new DateTime(InfoHistory.ToDateTime);
				}
				return TickFileInfo.EMPTY_DATETIME;
			}
		}
		#endregion

		#region public TickFileInfo InfoHistory
		public TickFileInfo InfoHistory{
			get{return this._tfihistory;}
		}
		#endregion

		#region public TickFileInfo InfoCache
		public TickFileInfo InfoCache{
			get{return this._tficache;}
		}
		#endregion

    #region public TickManagerStatus Status
    public TickManagerStatus Status {
      get { return this._status; }
    }
    #endregion

    #region internal TickFileMap MapHistory
    internal TickFileMap MapHistory{
			get{return this._maphistory;}
		}
		#endregion

		#region internal TickFileMap MapCache
		internal TickFileMap MapCache{
			get{return this._mapcache;}
		}
		#endregion

		#region internal Symbol Symbol
		internal Symbol Symbol{
			get{return this._symbol;}
		}
		#endregion

		#region internal TickCollection OnlineTicks
		internal TickCollection OnlineTicks{
			get{return _onlineticks;}
		}
		#endregion

    #region internal void SetSFIHistory(TickFileInfo tfihistory)
    internal void SetSFIHistory(TickFileInfo tfihistory) {
      _tfihistory = tfihistory;
      _tfbhistory = new TickFileBuffer(_symbol, tfihistory);
      _maphistory = new TickFileMap(tfihistory);
    }
    #endregion

    #region internal void SetSFICache(TickFileInfo tficache)
    internal void SetSFICache(TickFileInfo tficache) {
      _tficache = tficache;
      _tfbcache = new TickFileBuffer(_symbol, tficache);
      _mapcache = new TickFileMap(tficache);
    }
    #endregion

    #region public IBarList GetBarList(int second)
    public IBarList GetBarList(int second){
			foreach (BarManager bm in this._bars){
				if (bm.TimeFrame.Second == second)
					return bm;
			}
			return null;
    }
    #endregion

    #region public IEnumerator GetEnumerator()
    public IEnumerator GetEnumerator() {
			return new TickReaderEnumerator(this);
		}
		#endregion

		#region private class TickReaderEnumerator:IEnumerator 
		/// <summary>
		/// Класс отвечающий за чтение данных
		/// </summary>
		private class TickReaderEnumerator:IEnumerator {
			private TickManager _parent;
			private int _index;

			public TickReaderEnumerator(TickManager tickmanager){
				_parent = tickmanager;
				this.Reset();
			}

			#region public void Reset() 
			public void Reset() {
				_index = -1;
			}
			#endregion

			#region public int Position
			public int Position{
				get{return _index;}
			}
			#endregion

			#region public object Current 
			public object Current {
				get {
					return _parent[_index];
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

    #region public Tick this[int index]
    public Tick this[int index] {
      get {
        if (this.Busy)
          return new Tick();
        if (index < _tfbhistory.Info.CountTick)
          return _tfbhistory.GetTick(index);
        else if (index < _tfbcache.Info.CountTick + _tfbhistory.Info.CountTick)
          return _tfbcache.GetTick(index - _tfbhistory.Info.CountTick);
        return _onlineticks[index - _tfbhistory.Info.CountTick - _tfbcache.Info.CountTick];
      }
    }
    #endregion

    #region public Tick Current
    public Tick Current {
      get {
        if (this.Busy)
          return new Tick();
        return this[this.Count - 1]; 
      }
    }
    #endregion

    #region public void InitializeBarList()
    public void InitializeBarList() {
      if(_bars.Length != TimeFrameManager.TimeFrames.Count) {
        _bars = new BarManager[TimeFrameManager.TimeFrames.Count];

        for(int i = 0; i < TimeFrameManager.TimeFrames.Count; i++) {
          TimeFrame tf = TimeFrameManager.TimeFrames[i];
          _bars[i] = new BarManager(this, tf);
        }
      }
    }
    #endregion

    //public void InitializeSession() {
    //  if (this.CheckDataCaching()) {
    //    _sessionID++;
    //  }
    //}

    #region private bool CheckDataCaching()
    /// <summary>
    /// Необходимо ли кешировать данные
    /// </summary>
    /// <returns>true - необходимо, false - все ОК</returns>
    private bool CheckDataCaching() {
      bool dcuhc = DataCheckUnionHstCache();
      bool isHistory = DataCheckHistory();
      bool isCache = DataCheckCache();

      return dcuhc || isHistory || isCache;
    }
    #endregion

    #region private void OnDataCachingStarting()
    private void OnDataCachingStarting() {
      _status = TickManagerStatus.DataCachingStarting;
      if (this.DataCachingChanged != null)
        this.DataCachingChanged(this, new TickManagerEventArgs(_status, 0, 0));
    }
    #endregion

    #region private void OnDataCachingProcess(int current, int total)
    private void OnDataCachingProcess(int current, int total) {
      _status = TickManagerStatus.DataCaching;
      if (this.DataCachingChanged != null)
        this.DataCachingChanged(this, new TickManagerEventArgs(_status, current, total));
    }
    #endregion

    #region private void OnDataUpdateStarting()
    private void OnDataUpdateStarting() {
      _status = TickManagerStatus.Updating;
      if (this.DataCachingChanged != null)
        this.DataCachingChanged(this, new TickManagerEventArgs(_status, 0, 0));
    }
    #endregion

    #region private void OnDataUpdateProcess(int current, int total) 
    private void OnDataUpdateProcess(int current, int total) {
      if (this.DataCachingChanged != null)
        this.DataCachingChanged(this, new TickManagerEventArgs(_status, current, total));
    }
    #endregion

    #region private void OnDataCachingStopping()
    private void OnDataCachingStopping() {
      _status = TickManagerStatus.Default;
      this.OnDataCachingChanged(0, 0);
    }
    #endregion

    #region private void OnDataCachingChanged(int current, int total)
    private void OnDataCachingChanged(int current, int total) {
      if (this.DataCachingChanged != null)
        this.DataCachingChanged(this, new TickManagerEventArgs(_status, current, total));
    }
    #endregion

    #region private bool DataCheckUnionHstCache()
    private bool DataCheckUnionHstCache() {
      return this.InfoCache.CountTick > 0 && this.InfoHistory.CountTick > 0 &&
        this.InfoHistory.ToDateTime > this.InfoCache.FromDateTime;
    }
    #endregion

    #region private bool DataCheckHistory()
    private bool DataCheckHistory() {


      ArrayList al = new ArrayList();
      foreach(BarManager bm in this._bars) {
        if(bm.BufferHistory.Info.CountTick != this._tfbhistory.Info.CountTick ||
           bm.BufferHistory.Info.Version != BarFileInfo.VERSION)
          al.Add(bm);
      }

      BarManager[] badbms = (BarManager[])al.ToArray(typeof(BarManager));

      int cnthsttick = _maphistory.GetCountTick();

      bool badmap = cnthsttick != this.InfoHistory.CountTick;
      return badbms.Length > 0 || badmap;
    }
    #endregion

    #region private bool DataCheckCache()
    private bool DataCheckCache() {
      ArrayList al = new ArrayList();

      foreach(BarManager bm in this._bars) {

        if(bm.BufferCache.Info.CountTick != this._tfbcache.Info.CountTick ||
           bm.BufferCache.Info.Version != BarFileInfo.VERSION) {
          al.Add(bm);
        }
      }

      BarManager[] badbms = (BarManager[])al.ToArray(typeof(BarManager));

      bool badmap = _mapcache.GetCountTick() != this.InfoCache.CountTick;
      return badbms.Length > 0 || badmap;
    }
    #endregion

    #region private void DataCachingBars()
    /// <summary>
    /// Кеширование данных связаных с онлайн
    /// </summary>
    private void DataCachingBars() {
      for (int i = 0; i < this._bars.Length; i++)
        this._bars[i].DataCaching();
    }
    #endregion

    #region private void DataCachingProcess()
    private void DataCachingProcess() {
      bool dcuhc = DataCheckUnionHstCache();
      bool isHistory = DataCheckHistory();
      bool isCache = DataCheckCache();

      if (!dcuhc && !isHistory && !isCache) {
        this.OnDataCachingStopping();
        return;
      }

      this.OnDataCachingChanged(0, 1);

      _tfbhistory.CloseAllStream();
      _tfbcache.CloseAllStream();

      #region проверка массивов на возможное наложение дат
      if (dcuhc) {


        TickFileInfo tmptfi = TickFileBuffer.CreateTempTickFileInfo(this.InfoCache);
        TickWriter tmpwriter = new TickWriter(tmptfi);
        int windex = 0;

        for (int i = 0; i < this.InfoCache.CountTick; i++) {
          Tick tick = this._tfbcache.GetTick(i);
          if (tick.Time > this.InfoHistory.ToDateTime)
            tmpwriter.Write(tick, windex++);
        }
        _tfbcache.CloseAllStream();
        TickFileBuffer.CloseTempTickFileInfo(windex, this.InfoCache, tmpwriter);
      }
      #endregion

      #region Проверка онлайн массива на возможность наложения
      if (this.InfoCache.CountTick > 0 && this._onlineticks.Count > 0) {
        Tick firstonltick = this._onlineticks[0];

        if (this.InfoCache.ToDateTime > firstonltick.Time) {
          TickCollection onlnewticks = new TickCollection();
          for (int i = 0; i < this._onlineticks.Count; i++) {
            Tick tick = this._onlineticks[i];
            if (this.InfoCache.ToDateTime < tick.Time)
              onlnewticks.Add(tick);
          }
          _onlineticks = onlnewticks;
        }
      }
      #endregion

      #region Проверка исторических данных
      if (isHistory) {
        /* отбор таймфреймов, которые не имеют кешированных данных */
        ArrayList al = new ArrayList();
        foreach (BarManager bm in this._bars) {
          if (bm.BufferHistory.Info.CountTick != this._tfbhistory.Info.CountTick ||
             bm.BufferHistory.Info.Version != BarFileInfo.VERSION)
            al.Add(bm);
        }

        BarManager[] badbms = (BarManager[])al.ToArray(typeof(BarManager));

        int cnthsttick = _maphistory.GetCountTick();

        bool badmap = cnthsttick != this._tfbhistory.Info.CountTick;

        if (badbms.Length > 0 || badmap) {
          System.Diagnostics.Debug.WriteLine("Кеширование исторических данных!");
          foreach (BarManager bm in badbms) {
//            bm.IsCachingData = true;
            bm.BufferHistory.RemoveAll();
          }

          if (badmap)
            _maphistory.Clear();

          int counttick = _tfbhistory.Info.CountTick;
          int curtemp = -1;

          for (int i = 0; i < counttick; i++) {

            int temp = i * 100 / counttick;
            if (curtemp != temp) {
              curtemp = temp;
              this.OnDataCachingChanged(curtemp, 100);
            }

            Tick tick = _tfbhistory.GetTick(i);
            if (badmap)
              _maphistory.AddTick(tick);

            for (int ii = 0; ii < badbms.Length; ii++)
              badbms[ii].BufferHistory.AddTick(tick);
          }

          foreach (BarManager bm in badbms) {
            bm.BufferHistory.Flush();
//            bm.IsCachingData = false;
          }
          if (badmap)
            _maphistory.Save();
        }
      }
      #endregion

      #region Проверка кешированных данных
      if (isCache) {
        ArrayList al = new ArrayList();
        foreach (BarManager bm in this._bars) {
          if (bm.BufferCache.Info.CountTick != this._tfbcache.Info.CountTick ||
             bm.BufferCache.Info.Version != BarFileInfo.VERSION) {
            al.Add(bm);
          }
        }

        BarManager[] badbms = (BarManager[])al.ToArray(typeof(BarManager));

        bool badmap = _mapcache.GetCountTick() != this.InfoCache.CountTick;

        if (badbms.Length > 0 || badmap) {
          System.Diagnostics.Debug.WriteLine("Кеширование онлайн данных данных!");

          foreach (BarManager bm in badbms) {
//            bm.IsCachingData = true;
            bm.BufferCache.RemoveAll();
          }

          if (badmap)
            _mapcache.Clear();

          int curtemp = -1;
          int counttick = this._tfbcache.Info.CountTick;

          for (int i = 0; i < counttick; i++) {

            int temp = i * 100 / counttick;
            if (curtemp != temp) {
              curtemp = temp;
              this.OnDataCachingChanged(i, counttick);
            }

            Tick tick = _tfbcache.GetTick(i);
            if (badmap) _mapcache.AddTick(tick);
            for (int ii = 0; ii < badbms.Length; ii++) {
              badbms[ii].BufferCache.AddTick(tick);
            }
          }
          foreach (BarManager bm in badbms) {
            bm.BufferCache.Flush();
//            bm.IsCachingData = false;
          }
          if (badmap)
            _mapcache.Save();
        }
      }
      #endregion

      _tfbhistory.CloseAllStream();
      _tfbcache.CloseAllStream();

      DataCachingBars();
      this.OnDataCachingStopping();
    }
    #endregion

    #region public void DataCaching()
    /// <summary>
    /// Выравнивание массивов по дате.
    /// Кэширование данных.
    /// чтение тиков, преобразование в таймфремы, 
    /// сохранение таймфреймов, сохранение мапинга по файлам
    /// </summary>
    /// <returns>True - процесс будет запущень на выполение</returns>
    public void DataCaching() {

      if (this.IsDataCaching)
        return;

      _savedSessionId = _sessionID;
      this.OnDataCachingStarting();

      Thread th = new Thread(new ThreadStart(this.DataCachingProcess));
      th.IsBackground = true;
      th.Name = "DataCaching";
      th.Priority = ThreadPriority.Lowest;
      th.Start();

      #region musor
      //if (this._status == TickManagerStatus.DataCaching)
      //  return true;

      //if (this._status != TickManagerStatus.Default) {
      //  Thread.Sleep(1);
      //}

      //this._status = TickManagerStatus.DataCaching;

      //bool dcuhc = DataCheckUnionHstCache();
      //bool isHistory = DataCheckHistory();
      //bool isCache = DataCheckCache();
      //this._status = TickManagerStatus.Default;

      //if(dcuhc || isHistory || isCache) {
      //  Thread th = new Thread(new ThreadStart(this.DataCachingMethod));
      //  th.IsBackground = true;
      //  th.Name = "DataCaching";
      //  th.Priority = ThreadPriority.Lowest;
      //  th.Start();
      //  return true;
      //}
      //DataCachingBars();
      //return false;
      #endregion
    }
    #endregion

    #region public void DataCachingMethod()
    public void DataCachingMethod(){

      if (this.IsDataCaching)
        return;
      
      _savedSessionId = _sessionID;
      this.OnDataCachingStarting();

      this.DataCachingProcess();
      
      #region old
      //if (this._status == TickManagerStatus.DataCaching)
      //  return;
      //this._status = TickManagerStatus.DataCaching;
      //_status = TickManagerStatus.Default;
      //this.OnDataCachingStopping();
      #endregion
    }
		#endregion

    #region public void Update(TickCollection ticks)
    public void Update(TickCollection ticks){
			this.Update(ticks, true);
		}
		#endregion

		#region public void Update(TickCollection ticks, bool isCacheBuffer)
		public void Update(TickCollection ticks, bool isCacheBuffer){

      while (this._status != TickManagerStatus.Default) {
        Thread.Sleep(5);
      }

      this.OnDataUpdateStarting();

			if (isCacheBuffer)
				this._tfbcache.Update(ticks);
			else
				this._tfbhistory.Update(ticks);
      _sessionID++;

      this.OnDataCachingStopping();
    }
		#endregion

		#region public void Update(TickFileInfo tfi)
		public void Update(TickFileInfo tfi){
			this.Update(tfi, true);
		}
		#endregion

		#region public void Update(TickFileInfo tfi, bool isCacheBuffer)
		public void Update(TickFileInfo tfi, bool isCacheBuffer){
      while (this._status != TickManagerStatus.Default) {
        Thread.Sleep(5);
      }

      this.OnDataUpdateStarting();
      if (isCacheBuffer) {
				this._tfbcache.Update(tfi);
			}else{
				this._tfbhistory.Update(tfi);
			}
      _sessionID++;
      this.OnDataCachingStopping();
    }
		#endregion

		#region public void Add(Tick tick)
		/// <summary>
		/// добавление тика в онлайн массив
		/// </summary>
		/// <param name="tick"></param>
		public void Add(Tick tick){
			if (this.Busy) return;

			_onlineticks.Add(tick);

			for (int i=0;i<this._bars.Length;i++){
				this._bars[i].AddTick(tick);
			}
		}
		#endregion

		#region public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick)
		public bool CheckPeriodInMap(DateTime fromdtm, DateTime todtm, int cnttick){

			fromdtm = new DateTime(fromdtm.Year, fromdtm.Month, fromdtm.Day, 0, 0, 0);
			todtm = new DateTime(todtm.Year, todtm.Month, todtm.Day, 0, 0, 0, 0);

			int counttick = 0;
			bool begin = false;
			
			for (int i=0;i<this.MapHistory.Count;i++){

				DateTime dtm = new DateTime(this.MapHistory[i].Time);
				dtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0, 0, 0, 0);

				if (!begin && dtm == fromdtm)
					begin = true;
				if (begin && dtm > todtm)
					break;
				if (begin)
					counttick += this.MapHistory[i].CountTick;
			}

			begin = false;
			
			for (int i=0;i<this.MapCache.Count;i++){

				DateTime dtm = new DateTime(this.MapCache[i].Time);
				dtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0, 0, 0, 0);

				if (!begin && dtm == fromdtm)
					begin = true;
				if (begin && dtm > todtm)
					break;
				if (begin)
					counttick += this.MapCache[i].CountTick;
			}
			if (counttick == 0) return false;

			return cnttick == counttick;
		}
		#endregion

    #region public int GetPositionFromMap(DateTime fdtm)
    public int GetPositionFromMap(DateTime fdtm){

			fdtm = new DateTime(fdtm.Year, fdtm.Month, fdtm.Day, 0, 0, 0);

			int counttick = 0;
			for (int i=0;i<this.MapHistory.Count;i++){
				DateTime dtm = new DateTime(this.MapHistory[i].Time);
				dtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0, 0, 0, 0);
				
				if (fdtm.Ticks <= dtm.Ticks) return counttick;
				counttick += this.MapHistory[i].CountTick;
			}

			for (int i=0;i<this.MapCache.Count;i++){
				DateTime dtm = new DateTime(this.MapCache[i].Time);
				dtm = new DateTime(dtm.Year, dtm.Month, dtm.Day, 0, 0, 0, 0);

				if (fdtm.Ticks <= dtm.Ticks) return counttick;
				counttick += this.MapCache[i].CountTick;
			}

			return this.Count;
		}
		#endregion

    #region public int GetPostition(DateTime fdtm)
    public int GetPosition(DateTime time) {

      if (this.Count == 0)
        return -1;
      if (time.Ticks < this.TimeFrom.Ticks)
        return -1;
      if (time.Ticks > this.TimeTo.Ticks)
        return this.Count;

      int beginIndex = this.GetPositionFromMap(time);

      for (int i = beginIndex; i < this.Count; i++) {
        if (this[i].Time >= time.Ticks) {
          return i;
        }
      }
      return this.Count;
    }
    #endregion
  }
}
