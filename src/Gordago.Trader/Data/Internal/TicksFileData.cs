/**
* @version $Id: TicksFileData.cs 3 2009-02-03 12:52:09Z AKuzmin $
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
  using System.Threading;
  using Gordago.Core;

  /// <summary>
  /// Работа с тиками на физическом уровне
  /// </summary>
  class TicksFileData {

    internal static readonly DateTime EMPTY_DATETIME = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    internal static long UNIXTICKS = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;
    internal const int INFO_SIZE = 4 + 8 + 4 + 60 + 44 + 4 + 4 + 4;
    internal const int INFO_FILE_SIZE = 4 + 8 + 4 + 60 + 44;
    internal const int TICK_SIZE = 8;
    internal const int VERSION = 3;

    private object _locked = new object();

    private FileInfo _file;
    
    private string _symbolName;
    private int _digits = 0;
    private int _count;
    private long _dtmfrom, _dtmto;
    private int _version;

    private FileStream _fsReader, _fsWriter;
    private BinaryReader _breader;
    private BinaryWriter _bwriter;

    private readonly SessionSource _sessionSource;
    private readonly SessionDest _sessionDest;

    private TicksFileData(FileInfo file, bool loadInfo) {
      _file = file;

      _sessionSource = new SessionSource();
      _sessionDest = new SessionDest(_sessionSource);
      _sessionSource.IncrementsLevel1();
      
      if (loadInfo)
        this.LoadFileInfo();
    }

    public TicksFileData(FileInfo file):this(file, true) {
      
    }

    public TicksFileData(FileInfo file, string symbolName, int digits):this(file, false) {

      _symbolName = symbolName;
      _digits = digits;
      this.SaveFileInfo();
    }

    public TicksFileData(DirectoryInfo directory, string symbolName, int digits)
      : this(
        new FileInfo(
          Path.Combine(directory.FullName, 
              string.Format("{0}.{1}", symbolName, HistoryManager.TICKS_FILE_EXT))), symbolName, digits) {
    }

    #region ~TicksFileData()
    ~TicksFileData() {
      this.CloseStream();
    }
    #endregion

    #region public FileInfo File
    public FileInfo File {
      get { return this._file; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return _symbolName; }
    }
    #endregion

    #region public long TimeFrom
    public long TimeFrom {
      get {
        this.UpdateInfo();
        return this._dtmfrom; 
      }
    }
    #endregion

    #region public long TimeTo
    public long TimeTo {
      get {
        this.UpdateInfo();
        return _dtmto; 
      }
    }
    #endregion

    #region public int Count
    public int Count {
      get {
        this.UpdateInfo();
        return this._count; 
      }
    }
    #endregion

    #region public int Digits
    public int Digits {
      get { return _digits; }
    }
    #endregion

    #region private void UpdateInfo()
    private void UpdateInfo() {
      if (_sessionDest.CheckSession() == 0)
        return;

      lock (_locked) {
        this.CheckReader();
        _count = Convert.ToInt32((_breader.BaseStream.Length - INFO_SIZE) / 8); ;
        if (_count == 0) {
          _dtmfrom = EMPTY_DATETIME.Ticks;
          _dtmto = EMPTY_DATETIME.Ticks;
        } else {
          Tick tick = this.Read(0);
          _dtmfrom = tick.Time;

          tick = this.Read(_count - 1);
          _dtmto = tick.Time;
        }
        _sessionDest.Complete();
      }
    }
    #endregion

    #region private void CloseReader()
    private void CloseReader() {
      if (_breader == null)
        return;
      _breader.Close();
      _breader = null;
      _fsReader = null;
    }
    #endregion

    #region private void CloseWriter()
    private void CloseWriter() {
      if (_bwriter == null)
        return;
      _bwriter.Flush();
      _bwriter.Close();
      _bwriter = null;
      _fsWriter = null;
    }
    #endregion

    #region private void CheckReader()
    private void CheckReader() {
      lock (_locked) {
        if (_breader != null)
          return;
        
        this.CloseWriter();

        _fsReader = new FileStream(_file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        _breader = new BinaryReader(_fsReader);
      }
    }
    #endregion

    #region private void CheckWriter()
    private void CheckWriter() {
      lock (_locked) {
        if (_bwriter != null)
          return;
        
        this.CloseReader();

        _fsWriter = new FileStream(_file.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        _bwriter = new BinaryWriter(_fsWriter);
      }
    }
    #endregion

    #region public void CloseStream()
    public void CloseStream() {
      lock (_locked) {
        this.CloseReader();
        this.CloseWriter();
      }
    }
    #endregion

    #region private static string GetSymbolName(byte[] bs)
    private static string GetSymbolName(byte[] bs) {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach (byte b in bs) if ((int)b != 0) sb.Append((char)b);
      return sb.ToString();
    }
    #endregion

    #region private static DateTime ConvertUnixToDateTime(double ctm)
    private static DateTime ConvertUnixToDateTime(double ctm) {
      return new DateTime((long)(ctm * 10000000) + new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks);
    }
    #endregion

    #region public void LoadFileInfo()
    public void LoadFileInfo() {
      lock (_locked) {
        this.CheckReader();
        _breader.BaseStream.Seek(0, SeekOrigin.Begin);
        _version = _breader.ReadInt32();

        _symbolName = GetSymbolName(_breader.ReadBytes(8));
        _digits = _breader.ReadInt32();

        this.UpdateInfo();
      }
    }
    #endregion

    #region internal static UInt32 ConvertDateTimeToUnix(DateTime dt)
    internal static UInt32 ConvertDateTimeToUnix(DateTime dt) {
      UInt32 retval = (UInt32)((dt.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks) / 10000000L);
      return retval;
    }
    #endregion

    #region public void SaveFileInfo()
    public void SaveFileInfo() {
      lock (_locked) {
        this.CheckWriter();
        _bwriter.BaseStream.Seek(0, SeekOrigin.Begin);
        _bwriter.Write(VERSION);
        _bwriter.Write(BarsFileData.GetArray(_symbolName, 8));

        _bwriter.Write(_digits);
        string copyright = "(C)opyright 2007, Gordago Software Corp.";
        _bwriter.Write(BarsFileData.GetArray(copyright, 60));
        _bwriter.Write(BarsFileData.GetArray("", 44 + 4 + 4 + 4));
        this.CloseWriter();
      }
    }
    #endregion

    #region public Tick Read(int index)
    public Tick Read(int index) {
      lock(_locked){
        this.CheckReader();
        long offset = INFO_SIZE + index * 8;
        if (_breader.BaseStream.Position != offset)
          _breader.BaseStream.Seek(offset, SeekOrigin.Begin);

        uint utime = _breader.ReadUInt32();
        float bid = _breader.ReadSingle();
        long ltime = UNIXTICKS + (long)utime * 10000000L;
        
        return new Tick(ltime, bid);
      }
    }
    #endregion

    #region public void SeekWriterBegin()
    public void SeekWriterBegin() {
      this.CheckWriter();
      _bwriter.BaseStream.Seek(0, SeekOrigin.Begin);
    }
    #endregion

    #region public void SeekWriterEnd()
    public void SeekWriterEnd() {
      this.CheckWriter();
      _bwriter.BaseStream.Seek(0, SeekOrigin.End);
    }
    #endregion

    #region public void SeekWriter(int index)
    public void SeekWriter(int index) {
      this.CheckWriter();
      long offset = INFO_SIZE + index * TICK_SIZE;
      _bwriter.BaseStream.Seek(offset, SeekOrigin.Begin);
    }
    #endregion

    #region public void Write(Tick tick, int index)
    public void Write(Tick tick, int index) {
      lock (_locked) {
        
        this.CheckWriter();

        long offset = INFO_SIZE + index * TICK_SIZE;

        if (offset != _bwriter.BaseStream.Position)
        {
            // throw (new Exception(string.Format("offset={0} != TickWriter.Write.index={1}", offset, index)));
            _bwriter.Seek((int)offset, SeekOrigin.Begin);
        }

        uint utime = (UInt32)((tick.Time - UNIXTICKS) / 10000000);
        _bwriter.Write(utime);
        _bwriter.Write(tick.Price);
      }
    }
    #endregion

  }
}
