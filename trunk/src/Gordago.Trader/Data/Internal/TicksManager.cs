/**
* @version $Id: TicksManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading;
  using System.IO;
  using System.Diagnostics;
  using Gordago.Core;

  class TicksManager : ITickCollection, ISession, ITicksManager, ISymbolInfo {

    public event TicksManagerTaskEventHandler StartTask;
    public event TicksManagerTaskEventHandler StopTask;
    public event TicksManagerTaskProcessEventHandler TaskProcessChanged;

    private TicksFileData _history = null;
    private TicksFileData _cache = null;

    private DirectoryInfo _dirBarsHistory, _dirBarsCache, _dirMapsHistory, _dirMapsCache;
    private readonly List<Tick> _online = new List<Tick>();

    private string _symbolName = "";
    private int _digits = 0;

    private readonly object _locked = new object();

    /// <summary>
    /// Сессия
    /// Уровень 1 - изменение размера основной истории
    /// Уровень 2 - изменение размера истории с сервера брокера
    /// Уровень 3 - добавлен тик
    /// </summary>
    private readonly SessionSource _sessionSource;
    private readonly SessionDest _sessionDest;

    private int _count;
    private int _countHistory = 0, _countCache = 0;
    //private DateTime _timeFrom = DateTime.Now;
    //private DateTime _timeTo = DateTime.Now;

    private readonly BarsManager _barsManager;

    private readonly TicksMapManager _map;

    public TicksManager() {
      _sessionSource = new SessionSource();
      _sessionDest = new SessionDest(_sessionSource);
      _sessionSource.IncrementsLevel1();

      _barsManager = new BarsManager(this);
      _map = new TicksMapManager(this);
    }

    #region public SessionSource Session
    public SessionSource Session {
      get { return _sessionSource; }
    }
    #endregion

    #region public TicksMapManager Map
    public TicksMapManager Map {
      get { return this._map; }
    }
    #endregion

    #region public DirectoryInfo DirectoryMapsHistory
    public DirectoryInfo DirectoryMapsHistory {
      get { return this._dirMapsHistory; }
    }
    #endregion

    #region public DirectoryInfo DirectoryMapsCache
    public DirectoryInfo DirectoryMapsCache {
      get { return this._dirMapsCache; }
    }
    #endregion

    #region public DirectoryInfo DirectoryBarsHistory
    public DirectoryInfo DirectoryBarsHistory {
      get { return this._dirBarsHistory; }
    }
    #endregion

    #region public DirectoryInfo DirectoryBarsCache
    public DirectoryInfo DirectoryBarsCache {
      get { return this._dirBarsCache; }
    }
    #endregion

    #region public IBarsDataCollection BarsDataList
    public IBarsDataCollection BarsDataList {
      get { return _barsManager; }
    }
    #endregion

    #region public int Digits
    public int Digits {
      get { return this._digits; }
    }
    #endregion

    #region public string Name
    public string Name {
      get { return _symbolName; }
    }
    #endregion

    #region public TicksFileData History
    public TicksFileData History {
      get { return this._history; }
      set {
        if (_history != null)
          throw (new Exception("The file of history is initialized"));

        _symbolName = value.SymbolName;
        _digits = value.Digits;
        this._history = value;
      }
    }
    #endregion

    #region public TicksFileData Cache
    public TicksFileData Cache {
      get { return this._cache; }
      set {
        if (_cache != null)
          throw (new Exception("The file of cache is initialized"));

        _symbolName = value.SymbolName;
        _digits = value.Digits;

        this._cache = value;
      }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        lock (_locked) {
          this.CheckSessionId();
          return _count + _online.Count;
        }
      }
    }
    #endregion
    

    #region public Tick this[int index]
    public Tick this[int index] {
      get {
        lock (_locked) {
          this.CheckSessionId();
          if (index >= 0 && index < _countHistory && _countHistory > 0) {
            return _history.Read(index);
          } else if (index >= _countHistory && index < (_countHistory + _countCache) && _countCache > 0) {
            return _cache.Read(index - _countHistory);
          } else if (index >= (_countHistory + _countCache) && index < (_countHistory + _countCache + _online.Count) && _online.Count > 0) {
            return _online[index - _countHistory - _countCache];
          } else
            throw (new ArgumentOutOfRangeException("index"));
        }
      }
    }
    #endregion

    #region public Tick Current
    public Tick Current {
      get {
        return this[this.Count - 1];
      }
    }
    #endregion

    #region public void InitializeBarsFiles(BarsFolder barsFolderHistory, BarsFolder barsFolderCache)
    public void InitializeBarsFiles(BarsFolder barsFolderHistory, BarsFolder barsFolderCache) {
      _dirBarsHistory = barsFolderHistory.Directory;
      _dirBarsCache = barsFolderCache.Directory;
      this.Check(true, barsFolderHistory);
      this.Check(false, barsFolderCache);
    }
    #endregion

    #region public void InitializeMapsFiles(MapsFolder mapsFolderHistory, MapsFolder mapsFolderCache)
    public void InitializeMapsFiles(MapsFolder mapsFolderHistory, MapsFolder mapsFolderCache) {
      _dirMapsHistory = mapsFolderHistory.Directory;
      _dirMapsCache = mapsFolderCache.Directory;
      this.Check(true, mapsFolderHistory);
      this.Check(false, mapsFolderCache);
    }
    #endregion

    #region private void Check(bool isHistory, MapsFolder mapsFolder)
    private void Check(bool isHistory, MapsFolder mapsFolder) {
      TicksFileData ticksFileData = isHistory ? _history : _cache;
      if (ticksFileData == null)
        return;
      TicksFileMapData[] mapsFiles = mapsFolder.Pop(_symbolName);
      List<TicksFileMapData> listDelete = new List<TicksFileMapData>();

      foreach (TicksFileMapData mapFile in mapsFiles) {
        if (ticksFileData.Count == mapFile.CountTicks) {
          if (isHistory) 
            _map.History = mapFile;
           else 
            _map.Cache = mapFile;
        } else 
          listDelete.Add(mapFile);
      }
      foreach (TicksFileMapData barsFile in listDelete) {
        barsFile.CloseStream();
        barsFile.File.Delete();
      }
    }
    #endregion

    #region private void Check(bool isHistory, BarsFolder barsFolder)
    private void Check(bool isHistory, BarsFolder barsFolder) {
      TicksFileData ticksFileData = isHistory ? _history : _cache;
      BarsFileData[] barsFiles = barsFolder.Pop(_symbolName); 
      List<BarsFileData> listDelete = new List<BarsFileData>();

      foreach (BarsFileData barsFile in barsFiles) {
        if (ticksFileData.Count == barsFile.CountTicks) {
          BarManager bm = (BarManager)this._barsManager[barsFile.TimeFrameSecond];
          if (bm == null) {
            listDelete.Add(barsFile);
          } else {
            if (isHistory) {
              bm.History = barsFile;
            } else {
              bm.Cache = barsFile;
            }
          }
        } else {
          listDelete.Add(barsFile);
        }
      }
      foreach (BarsFileData barsFile in listDelete) {
        barsFile.CloseStream();
        barsFile.File.Delete();
      }
    }
    #endregion

    #region private void CheckSessionId()
    private void CheckSessionId() {
      if (_sessionDest.CheckSession() == 0)
        return;

      _count = _countHistory = _countCache = 0;
      if (_history != null)
        _countHistory = _history.Count;

      if (_cache != null)
        _countCache = _cache.Count;

      _count = _countHistory + _countCache;

      _sessionDest.Complete();
      this.CheckDataCache();
    }
    #endregion

    #region private void CheckDataCache()
    /// <summary>
    /// Проверка, нет ли необходимости запустить кешь данных
    /// если необходимость возникает, запускаем
    /// </summary>
    /// <returns></returns>
    private void CheckDataCache() {
      lock (_locked) {

        BarManager[] bms = ((BarsManager)this.BarsDataList).GetBadDataBarManager();
        if (bms.Length == 0 && this.Map.IsHistoryCaching && this.Map.IsCacheCaching)
          return;

        if (this.CheckTask(TicksManagerTask.DataCaching))
          return;

        this.AddTask(new TaskDataCache());
      }
    }
    #endregion

    #region public void Update(Tick[] ticks)
    public void Update(Tick[] ticks) {
      this.Update(ticks, false);
    }
    #endregion

    #region public void Update(Tick[] ticks, bool isHistory)
    public void Update(Tick[] ticks, bool isHistory) {
      AbstractTask task;
      if (isHistory)
        task = new TaskUpdateHistory(ticks);
      else
        task = new TaskUpdateCache(ticks);
      this.AddTask(task);
    }
    #endregion

    #region protected virtual void OnStartTask(TicksManagerTaskEventArgs e)
    protected virtual void OnStartTask(TicksManagerTaskEventArgs e) {
      if (this.StartTask != null)
        this.StartTask(this, e);
    }
    #endregion

    #region protected virtual void OnStopTask(TicksManagerTaskEventArgs e)
    protected virtual void OnStopTask(TicksManagerTaskEventArgs e) {
      if (this.StopTask != null)
        this.StopTask(this, e);
    }
    #endregion

    #region protected virtual void OnTaskProcessChanged(TicksManagerProcessEventArgs e)
    protected virtual void OnTaskProcessChanged(TicksManagerProcessEventArgs e) {
      if (this.TaskProcessChanged != null)
        this.TaskProcessChanged(this, e);
    }
    #endregion

    #region internal FileInfo[] GetFiles()
    internal FileInfo[] GetFiles() {
      List<FileInfo> files = new List<FileInfo>();
      return files.ToArray();
    }
    #endregion

    private readonly List<AbstractTask> _taskList = new List<AbstractTask>();
    private long _lastCheckTaskPriorityTime = DateTime.Now.Ticks;
    private AbstractTask _currentTask = null;
    private bool _abortTask = false;

    #region private bool CheckTask(TaskType taskType)
    private bool CheckTask(TicksManagerTask taskType) {
      if (_currentTask != null && _currentTask.Task == taskType)
        return true;

      for (int i = 0; i < _taskList.Count; i++) {
        if (_taskList[i].Task == taskType)
          return true;
      }
      return false;
    }
    #endregion

    #region private void AddTask(AbstractTask task)
    private void AddTask(AbstractTask task) {
      Trace.TraceInformation("{0} TicksManager.AddTask({1})", this.Name, task);

      lock (_locked) {
        _taskList.Add(task);
        if (_taskList.Count == 1 && _currentTask == null) {
          Thread th = new Thread(new ThreadStart(TaskExecuteProcess));
          th.IsBackground = true;
          th.Name = "Task Execute: " + this._symbolName;
          th.Priority = ThreadPriority.Lowest;
          th.Start();
        }
      }
    }
    #endregion

    #region private static int CompareByTaskPriority(Task task1, Task task2)
    private static int CompareByTaskPriority(AbstractTask task1, AbstractTask task2) {
      return task1.Priority.CompareTo(task2.Priority);
    }
    #endregion

    #region private bool CheckTaskPriority()
    private bool CheckTaskPriority() {
      lock (_locked) {
        if (_taskList.Count == 0)
          return false;

        if (_currentTask.Priority == 0)
          return false;

        if (DateTime.Now.Ticks - _lastCheckTaskPriorityTime < 10000000L * 3)
          return false;

        bool findDataCacheTask = false;

        for (int i = 0; i < _taskList.Count; i++) {
          if (_taskList[i].Task != TicksManagerTask.DataCaching)
            continue;

          if (findDataCacheTask) {
            _taskList.RemoveAt(i);
            i--;
          } else {
            findDataCacheTask = true;
          }
        }

        _taskList.Sort(CompareByTaskPriority);

        _lastCheckTaskPriorityTime = DateTime.Now.Ticks;
        return _currentTask.Priority > _taskList[0].Priority;
      }
    }
    #endregion

    #region private void ChangeTaskPriority()
    private void ChangeTaskPriority() {
      if (!_abortTask)
        return;

      _taskList.Add(_currentTask);

      _lastCheckTaskPriorityTime = DateTime.Now.AddHours(-10).Ticks;
      this.CheckTaskPriority();

      _abortTask = false;
    }
    #endregion

    #region private Task ReadTask()
    private AbstractTask ReadTask() {
      lock (_locked) {
        if (_taskList.Count == 0)
          return null;
        AbstractTask task = _taskList[0];
        _taskList.RemoveAt(0);
        return task;
      }
    }
    #endregion

    #region private void ExecuteTaskDataCaching()
    private void ExecuteTaskDataCaching() {

      BarManager[] bms = ((BarsManager)this.BarsDataList).GetBadDataBarManager();
      if (bms.Length == 0 && this.Map.IsHistoryCaching && this.Map.IsCacheCaching)
        return;

      Dictionary<int, BarsFileData> bfdHistory = new Dictionary<int, BarsFileData>();
      Dictionary<int, BarsFileData> bfdCache = new Dictionary<int, BarsFileData>();

      bool chistory = false, ccache = false;

      foreach (BarManager bm in bms) {
        if (!bm.IsHistoryCaching) {
          chistory = true;
          if (bm.History != null)
            bfdHistory.Add(bm.TimeFrame.Second, bm.History);
        }

        if (!bm.IsCacheCaching) {
          ccache = true;
          if (bm.Cache != null)
            bfdCache.Add(bm.TimeFrame.Second, bm.Cache);
        }
      }

      if (chistory || !this.Map.IsHistoryCaching) {
        this.CacheBars(true, bfdHistory, -1);
        ccache = true;
      }

      if (!_abortTask && ((this.Cache != null && ccache) || !this.Map.IsCacheCaching)) {
        this.CacheBars(false, bfdCache, -1);
      }
    }
    #endregion

    #region private void CacheBars(bool isHistory, Dictionary<int, BarsFileData> bfds, long beginTime)
    private void CacheBars(bool isHistory, Dictionary<int, BarsFileData> bfds, long beginTime) {
      List<BarsFileDataCacher> cacher = new List<BarsFileDataCacher>();
      TicksFileData tfd = isHistory ? this.History : this.Cache;
      TicksFileMapData tfmd = isHistory ? this.Map.History : this.Map.Cache;

      TicksFileMapDataCacher mapCacher = new TicksFileMapDataCacher(this, isHistory, tfmd);

      foreach (BarManager bm in (BarsManager)this.BarsDataList) {
        BarsFileData bfdBad = null;
        bfds.TryGetValue(bm.TimeFrame.Second, out bfdBad);
        BarsFileDataCacher bfdc =
          new BarsFileDataCacher(this, bm, isHistory, bfdBad, bm.TimeFrame);

        cacher.Add(bfdc);
      }

      long time = DateTime.Now.Ticks;

      for (int i = 0; i < tfd.Count; i++) {
        Tick tick = tfd.Read(i);
        for (int ii = 0; ii < cacher.Count; ii++) {
          cacher[ii].Add(tick);
        }
        
        mapCacher.Add(tick);
        if (DateTime.Now.Ticks - time > 5000000L) {
          this.OnTaskProcessChanged(new TicksManagerProcessEventArgs(_currentTask.Task, tfd.Count, i));
          time = DateTime.Now.Ticks;
        }

        if (this.CheckTaskPriority()) {
          _abortTask = true;
          break;
        }
      }

      for (int i = 0; i < cacher.Count; i++) {
        if (_abortTask) {
          cacher[i].Abort();
        } else {
          /* блокировка */
          lock (_locked) 
            cacher[i].Complete();
        }
      }
      if (_abortTask) {
        mapCacher.Abort();
      } else {
        lock (_locked) 
          mapCacher.Complete();
      }
    }
    #endregion

    #region private bool CheckTaskPriorityUpdateTicks()
    private bool CheckTaskPriorityUpdateTicks() {
      if (!CheckTaskPriority()) return false;
      _abortTask = true;
      return true;
    }
    #endregion

    #region enum UpdateMethod
    enum UpdateMethod {
      Unknow,
      AppendInEmpty,
      InsertInBeginning,
      AdditionInEnd,
      ReplacementOfGreater,
      InsertInBody,
      AdditionInBeginReplacement,
      AdditionInEndReplacement
    }
    #endregion

    #region private long ChangeUpdateProgress(long savedTime, int total, int current)
    private long ChangeUpdateProgress(long savedTime, int total, int current) {
      if (DateTime.Now.Ticks - savedTime < 5000000L)
        return savedTime;
      this.OnTaskProcessChanged(new TicksManagerProcessEventArgs(_currentTask.Task, total, current));
      return DateTime.Now.Ticks;
    }
    #endregion

    #region private void ExecuteTaskUpdateTicks(TaskUpdateTicks task)
    private void ExecuteTaskUpdateTicks(TaskUpdateTicks task) {
      Trace.TraceInformation("{0} TicksManager.ExecuteTaskUpdateTicks({1}) - Start", this.Name, task);

      bool isCutBeginCache = false;

      Tick[] ticks = task.Ticks;

      #region if (ticks.Length == 0) {...}
      if (ticks.Length == 0) {
        if (task.Task == TicksManagerTask.UpdateCache &&
          this.Cache.Count > 0 && this.History.Count > 0 &&
          this.Cache.TimeFrom < this.History.TimeTo) {

          isCutBeginCache = true;

        } else {
          return;
        }
      }
      #endregion

      TicksFileData tfdCurrent = task.Task == TicksManagerTask.UpdateHistory ? this.History : this.Cache;

      FileInfo ticksFileTemp = new FileInfo(tfdCurrent.File.FullName + ".tmp");

      if (ticksFileTemp.Exists)
        ticksFileTemp.Delete();

      TicksFileData tfdTemp = new TicksFileData(ticksFileTemp, this.Name, this.Digits);

      int index = 0;

      bool abort = false;
      long savedTime = DateTime.Now.Ticks;

      if (isCutBeginCache) {
        Trace.TraceInformation("{0} - Cut Begin Ticks in Cache File", this.Name);

        long limitBeginTime = this.History.TimeTo;

        int cCount = this.Cache.Count;
        for (int i = 0; i < cCount; i++) {
          Tick tick = this.Cache.Read(i);
          if (tick.Time < limitBeginTime)
            continue;
          tfdTemp.Write(tick, index++);
          savedTime = ChangeUpdateProgress(savedTime, cCount, i);
          if (CheckTaskPriorityUpdateTicks()) break;
        }
      } else {

        UpdateMethod method = UpdateMethod.Unknow;
        if (tfdCurrent.Count == 0)
          method = UpdateMethod.AppendInEmpty;

        if (method == UpdateMethod.AppendInEmpty) {
          Trace.TraceInformation("{0} - StartUpdate[Method={1}]", this.Name, method);
          for (int i = 0; i < ticks.Length; i++) {
            tfdTemp.Write(ticks[i], index++);
            savedTime = ChangeUpdateProgress(savedTime, ticks.Length, i);
            if (CheckTaskPriorityUpdateTicks()) break;
          }
        } else {
          #region Init Property
          long mTimeFrom = tfdCurrent.TimeFrom;
          long mTimeTo = tfdCurrent.TimeTo;
          long cTimeFrom = ticks[0].Time;
          long cTimeTo = ticks[ticks.Length - 1].Time;

          int mCount = tfdCurrent.Count;
          int cCount = ticks.Length;

          int mIndex = 0, cIndex = 0;
          Tick tick = new Tick();
          Tick mTick = new Tick();
          Tick cTick = new Tick();
          int level = 0;
          Tick cFirstTick = ticks[0];
          Tick cEndTick = ticks[ticks.Length - 1];
          #endregion

          if (cTimeFrom > mTimeTo)
            method = UpdateMethod.AdditionInEnd;
          else if (cTimeTo < mTimeFrom)
            method = UpdateMethod.InsertInBeginning;
          else if (cTimeFrom < mTimeFrom && mTimeTo < cTimeTo)
            method = UpdateMethod.ReplacementOfGreater;
          else if (mTimeFrom < cTimeFrom && cTimeTo < mTimeTo)
            method = UpdateMethod.InsertInBody;
          else if (cTimeFrom <= mTimeFrom && cTimeTo > mTimeFrom && cTimeTo < mTimeTo)
            method = UpdateMethod.AdditionInBeginReplacement;
          else if (cTimeFrom > mTimeFrom && cTimeFrom < mTimeTo && cTimeTo > mTimeTo)
            method = UpdateMethod.AdditionInEndReplacement;

          string mPS = string.Format("Main[B={0},E={1},Count={2}]", new DateTime(mTimeFrom), new DateTime(mTimeTo), mCount);
          string cPS = string.Format("New[B={0},E={1},Count={2}]", new DateTime(cTimeFrom), new DateTime(cTimeTo), cCount);

          Trace.TraceInformation("{0} - StartUpdate[Method={1}], {2}, {3}", this.Name, method, mPS, cPS);

          if (method == UpdateMethod.AdditionInEnd) {
            tfdTemp.CloseStream();
            tfdTemp.File.Delete();
            File.Copy(tfdCurrent.File.FullName, tfdTemp.File.FullName, true);
            tfdTemp = new TicksFileData(ticksFileTemp);
            tfdTemp.SeekWriterEnd();
            index = mCount;
            for (int i = 0; i < cCount; i++) {
              tfdTemp.Write(ticks[i], index++);
              savedTime = ChangeUpdateProgress(savedTime, cCount, i);
              if (CheckTaskPriorityUpdateTicks()) break;
            }
          }else if (method == UpdateMethod.InsertInBeginning) { // if (cTimeTo < mTimeFrom) 
            int count = tfdCurrent.Count + ticks.Length;

            for (int i = 0; i < count; i++) {
              //if (cTimeFrom > mTimeTo)
              //  tick = (i < mCount) ? tfdCurrent.Read(i) : ticks[i - mCount];
              //else
                tick = (i < cCount) ? ticks[i] : tfdCurrent.Read(i - cCount);

              tfdTemp.Write(tick, index++);
              savedTime = ChangeUpdateProgress(savedTime, count, i);
              if (CheckTaskPriorityUpdateTicks()) break;
            }
          } else if (method == UpdateMethod.ReplacementOfGreater) { //if (cTimeFrom < mTimeFrom && mTimeTo < cTimeTo) 
            #region Replacement of the greater
            for (int i = 0; i < ticks.Length; i++) {
              tfdTemp.Write(ticks[i], index++);
              savedTime = ChangeUpdateProgress(savedTime, ticks.Length, i);
              if (CheckTaskPriorityUpdateTicks()) break;
            }
            #endregion
          } else if (mTimeFrom < cTimeFrom && cTimeTo < mTimeTo) {
            #region Insert in a body
            while (mIndex < mCount) {
              tick = new Tick(-1, 0);

              mTick = tfdCurrent.Read(mIndex);
              if (cIndex < cCount)
                cTick = ticks[cIndex];

              if (level == 0) {
                if (mTick.Time < cTick.Time) {
                  tick = mTick;
                  mIndex++;
                } else {
                  level++;
                }
              }

              if (level == 1) {
                if (cIndex < cCount) {
                  tick = cTick;
                  cIndex++;
                } else {
                  level++;
                }
              }

              if (level == 2) {
                if (mTick.Time > cTick.Time) {
                  tick = mTick;
                }
                mIndex++;
              }
              if (tick.Time > -1)
                tfdTemp.Write(tick, index++);

              savedTime = ChangeUpdateProgress(savedTime, mCount+cCount, index);
              if (CheckTaskPriorityUpdateTicks()) break;
            }
            #endregion
          } else if (cTimeFrom <= mTimeFrom && cTimeTo > mTimeFrom && cTimeTo < mTimeTo) {
            #region To add in the beginning
            while (mIndex < mCount) {
              tick = new Tick(-1, 0);
              if (cIndex < cCount) {
                tick = cTick = ticks[cIndex++];
              } else {
                mTick = tfdCurrent.Read(mIndex);
                if (mTick.Time > cTick.Time) 
                  tick = mTick;
                mIndex++;
              }
              if (tick.Time > -1)
                tfdTemp.Write(tick, index++);
              savedTime = ChangeUpdateProgress(savedTime, mCount + cCount, index);
              if (CheckTaskPriorityUpdateTicks()) break;
            }
            #endregion
          } else if (cTimeFrom > mTimeFrom && cTimeFrom < mTimeTo && cTimeTo > mTimeTo) {
            #region To add in the end
            while (cIndex < cCount){
              tick = new Tick(-1, 0);
              cTick = ticks[cIndex];

              if (mIndex < mCount) {
                mTick = tfdCurrent.Read(mIndex);
                if (mTick.Time < cTick.Time) {
                  tick = mTick;
                  mIndex++;
                } else {
                  mIndex = mCount;
                }
              } else {
                tick = cTick;
                cIndex++;
              }

              if (tick.Time > -1)
                tfdTemp.Write(tick, index++);
              savedTime = ChangeUpdateProgress(savedTime, mCount + cCount, index);
              if (CheckTaskPriorityUpdateTicks()) break;
            }
            #endregion
          } else {
            abort = true;
          }
        }
      }
      tfdTemp.CloseStream();

      if (_abortTask) 
        abort = true;

      Trace.TraceInformation("{0} - Stop Update {1}", this.Name, abort?"Abort":"");

      if (abort) {
        tfdTemp.File.Delete();
        return;
      }

      if (task.Task == TicksManagerTask.UpdateHistory && this.Cache.Count > 0) {
        this.Update(new Tick[0], false);
      }

      lock (_locked) {
        tfdCurrent.CloseStream();
        tfdCurrent.File.Delete();
        tfdTemp.CloseStream();

        File.Move(tfdTemp.File.FullName, tfdCurrent.File.FullName);

        tfdTemp = new TicksFileData(tfdCurrent.File);

        if (task.Task == TicksManagerTask.UpdateHistory) {
          _history = tfdTemp;
          _sessionSource.IncrementsLevel1();
        } else {
          _cache = tfdTemp;
          _sessionSource.IncrementsLevel2();
        }
      }
      this.AddTask(new TaskDataCache());
      Trace.TraceInformation("{0} TicksManager.ExecuteTaskUpdateTicks({1}) - Stop", this.Name, task);
    }
    #endregion

    #region private void TaskExecuteProcess()
    private void TaskExecuteProcess() {
      Trace.TraceInformation("{0} TicksManager.TaskExecuteProcess() - Start", this.Name);

      while ((_currentTask = this.ReadTask()) != null) {
        Trace.TraceInformation("{0} - StartTask={1}", this.Name, _currentTask.Task);
        this.OnStartTask(new TicksManagerTaskEventArgs(_currentTask.Task));
        _abortTask = false;

        switch (_currentTask.Task) {
          case TicksManagerTask.DataCaching:
            this.ExecuteTaskDataCaching();
            break;
          case TicksManagerTask.UpdateHistory:
            this.ExecuteTaskUpdateTicks(_currentTask as TaskUpdateTicks);
            break;
        }

        this.OnStopTask(new TicksManagerTaskEventArgs(_currentTask.Task));

        if (_abortTask) {
          Trace.TraceInformation("{0} - AbortTask={1}", this.Name, _currentTask.Task);
          ChangeTaskPriority();
        } else {
          Trace.TraceInformation("{0} - StopTask={1}", this.Name, _currentTask.Task);
        }
      }
      _currentTask = null;
      Trace.TraceInformation("{0} TicksManager.TaskExecuteProcess() - Stop", this.Name);
    }
    #endregion

    #region abstract class TaskUpdateTicks : AbstractTask
    abstract class TaskUpdateTicks : AbstractTask {
      private readonly Tick[] _ticks;
      public TaskUpdateTicks(Tick[] ticks, TicksManagerTask task, int priority):base(task, priority) {
        _ticks = ticks;
      }

      public Tick[] Ticks {
        get { return _ticks; }
      }
    }
    #endregion

    #region class TaskUpdateHistory : TaskUpdateTicks
    class TaskUpdateHistory : TaskUpdateTicks {
      public TaskUpdateHistory(Tick[] ticks)
        : base(ticks, TicksManagerTask.UpdateHistory, 0) {
      }

      public override string ToString() {
        return string.Format("TaskUpdateHistory[Ticks={0}]", Ticks.Length);
      }
    }
    #endregion

    #region class TaskUpdateCache : TaskUpdateTicks
    class TaskUpdateCache : TaskUpdateTicks {
      public TaskUpdateCache(Tick[] ticks)
        : base(ticks, TicksManagerTask.UpdateHistory, 1) {
      }
      public override string ToString() {
        return string.Format("TaskUpdateCache[Ticks={0}]", Ticks.Length);
      }

    }
    #endregion

    #region class TaskDataCache:AbstractTask
    class TaskDataCache:AbstractTask{

      public TaskDataCache():base(TicksManagerTask.DataCaching, 2){
      }
    }
    #endregion

    #region abstract class AbstractTask
    abstract class AbstractTask {
      private readonly TicksManagerTask _taskType;

      /// <summary>
      /// 0 - Update History
      /// 1 - Update Cache
      /// 2 - Cache Data
      /// </summary>
      private readonly int _priority;

      public AbstractTask(TicksManagerTask taskType, int priority) {
        _taskType = taskType;
        _priority = priority;
      }

      #region public TaskType Task
      public TicksManagerTask Task {
        get { return _taskType; }
      }
      #endregion

      #region public int Priority
      public int Priority {
        get { return _priority; }
      }
      #endregion
    }
    #endregion
  }
}

