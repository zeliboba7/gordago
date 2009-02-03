/**
* @version $Id: HistoryManager.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;
  using System.Diagnostics;

  public delegate void HistoryManagerEventHandler(object sender, HistoryManagerEventArgs e);
  
  #region public class HistoryManagerEventArgs : EventArgs
  public class HistoryManagerEventArgs : EventArgs {
    private readonly ISymbol _symbol;
    public HistoryManagerEventArgs(ISymbol symbol)
      : base() {
      _symbol = symbol;
    }

    public ISymbol Symbol {
      get { return _symbol; }
    }
  }
  #endregion

  public class HistoryManager:IEnumerable<ISymbol> {

    internal static readonly string TICKS_FILE_EXT = "gtk";

    public event HistoryManagerEventHandler SymbolAdded;

    private readonly string CACHE_DIR = "cache";

    private DirectoryInfo _dirHistory;
    private DirectoryInfo _dirCache;

    private readonly Dictionary<SymbolKey, ISymbol> _symbols = new Dictionary<SymbolKey, ISymbol>();

    public HistoryManager(DirectoryInfo directory) {
      _dirHistory = directory;
      _dirCache = new DirectoryInfo(directory.FullName + "\\" + CACHE_DIR);

      if (!_dirHistory.Exists) _dirHistory.Create();
      if (!_dirCache.Exists) _dirCache.Create();
    }

    #region public DirectoryInfo HistoryDirectory
    public DirectoryInfo HistoryDirectory {
      get { return _dirHistory; }
    }
    #endregion

    #region public DirectoryInfo CacheDirectory
    public DirectoryInfo CacheDirectory {
      get { return _dirCache; }
    }
    #endregion

    #region public ISymbol this[string symbolName]
    public ISymbol this[string symbolName] {
      get {
        ISymbol symbol = null;
        _symbols.TryGetValue(new SymbolKey(symbolName), out symbol);
        return symbol;
      }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _symbols.Values.Count; }
    }
    #endregion

    #region protected virtual void OnSymbolAdded(HistoryManagerEventArgs e)
    protected virtual void OnSymbolAdded(HistoryManagerEventArgs e) {
      if (this.SymbolAdded != null)
        this.SymbolAdded(this, e);
    }
    #endregion

    #region public void Load()
    public void Load() {
      List<FileInfo> filesHistory = new List<FileInfo>();
      List<FileInfo> filesCache = new List<FileInfo>();
      Dictionary<string, TicksManager> symbols = new Dictionary<string, TicksManager>();

      FileInfo[] files = _dirHistory.GetFiles("*." + TICKS_FILE_EXT);
      foreach (FileInfo file in files) {
        try {
          TicksManager ticksManager = new TicksManager();
          ticksManager.History = new TicksFileData(file);
          symbols.Add(ticksManager.Name, ticksManager);
        } catch (Exception e){
          Trace.TraceError("Load {0} - {1}", file.FullName, e.Message);
        }
      }

      files = _dirCache.GetFiles("*." + TICKS_FILE_EXT);
      foreach (FileInfo file in files) {
        try {
          TicksManager ticksManager = null;
          TicksFileData fileData = new TicksFileData(file);
          symbols.TryGetValue(fileData.SymbolName, out ticksManager);
          if (ticksManager == null) {
            ticksManager = new TicksManager();
            symbols.Add(fileData.SymbolName, ticksManager);
          }
          ticksManager.Cache = fileData;
        } catch (Exception e) {
          Trace.TraceError("Load {0} - {1}", file.FullName, e.Message);
        }
      }

      BarsFolder barsFolderHistory = new BarsFolder(_dirHistory);
      BarsFolder barsFolderCache = new BarsFolder(_dirCache);

      MapsFolder mapsFolderHistory = new MapsFolder(_dirHistory);
      MapsFolder mapsFolderCache = new MapsFolder(_dirCache);

      this.Clear();
      foreach (TicksManager ticks in symbols.Values) {
        if (ticks.History == null) 
          ticks.History = new TicksFileData(_dirHistory, ticks.Name, ticks.Digits);
        
        if (ticks.Cache == null) 
          ticks.Cache = new TicksFileData(_dirCache, ticks.Name, ticks.Digits);
        
        ticks.InitializeBarsFiles(barsFolderHistory, barsFolderCache);
        ticks.InitializeMapsFiles(mapsFolderHistory, mapsFolderCache);
        Symbol symbol = new Symbol(ticks);

        this.Add(symbol);
      }
      barsFolderHistory.DeleteEmpty();
      barsFolderCache.DeleteEmpty();
    }
    #endregion

    #region internal void Clear()
    internal void Clear() {
      _symbols.Clear();
    }
    #endregion

    #region public IEnumerator<ISymbol> GetEnumerator()
    public IEnumerator<ISymbol> GetEnumerator() {
      return _symbols.Values.GetEnumerator();
    }
    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion

    #region public void Add(ISymbol symbol)
    public void Add(ISymbol symbol) {
      SymbolKey key = new SymbolKey(symbol.Name);
      _symbols.Add(key, symbol);
      this.OnSymbolAdded(new HistoryManagerEventArgs(symbol));
    }
    #endregion

    #region public ISymbol Create(string symbolName, int digits)
    public ISymbol Create(string symbolName, int digits) {
      Symbol symbol = this[symbolName] as Symbol;
      if (symbol != null)
        return symbol;

      TicksManager tm = new TicksManager();
      tm.History = new TicksFileData(_dirHistory, symbolName, digits);
      tm.Cache = new TicksFileData(_dirCache, symbolName, digits);

      BarsFolder barsFolderHistory = new BarsFolder(_dirHistory);
      BarsFolder barsFolderCache = new BarsFolder(_dirCache);

      MapsFolder mapsFolderHistory = new MapsFolder(_dirHistory);
      MapsFolder mapsFolderCache = new MapsFolder(_dirCache);

      tm.InitializeBarsFiles(barsFolderHistory, barsFolderCache);
      tm.InitializeMapsFiles(mapsFolderHistory, mapsFolderCache);

      symbol = new Symbol(tm);
      return symbol;
    }
    #endregion
  }
}
