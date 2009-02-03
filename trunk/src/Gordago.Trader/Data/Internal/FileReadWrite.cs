/**
* @version $Id: FileReadWrite.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  abstract class FileReadWrite {

    public static long UNIXTICKS = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

    private readonly int DATA_INFO_SIZE = 0;
    private readonly int DATA_FIELD_SIZE = 0;

    private FileStream _fsReader, _fsWriter;
    private BinaryReader _breader;
    private BinaryWriter _bwriter;

    private object _locked = new object();

    private FileInfo _file;
    private int _count;

    public FileReadWrite(FileInfo file, int infoSize, int fieldSize) {
      DATA_INFO_SIZE = infoSize;
      DATA_FIELD_SIZE = fieldSize;

      _file = file;
      //_fsWriter = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
      //_bwriter = new BinaryWriter(_fsWriter);

      //_fsReader = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Write);
      //_breader = new BinaryReader(_fsReader);
      this.LoadFileInfo();
    }

    #region ~FileReadWrite()
    ~FileReadWrite() {
      this.CloseStream();
    }
    #endregion

    #region public FileInfo File
    public FileInfo File {
      get { return this._file; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _count; }
    }
    #endregion

    #region protected object Locked
    protected object Locked {
      get { return _locked; }
    }
    #endregion

    #region protected BinaryReader Reader
    protected BinaryReader Reader {
      get {
        this.CheckReader();
        return _breader; 
      }
    }
    #endregion

    #region protected BinaryWriter Writer
    protected BinaryWriter Writer {
      get {
        this.CheckWriter();
        return this._bwriter; 
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

        _fsReader = new FileStream(_file.FullName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
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

    #region public static char[] GetArray(string str, int lenght)
    public static char[] GetArray(string str, int lenght) {
      char[] chars = new char[lenght];
      if (str == null)
        str = "";
      char[] arr = str.ToCharArray();
      for (int i = 0; i < arr.Length; i++) {
        if (i >= chars.Length) {
          break;
        }
        chars[i] = arr[i];
      }
      return chars;
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

    #region public void SeekReader(int index)
    public void SeekReader(int index) {
      this.CheckReader();
      long offset = DATA_INFO_SIZE + index * DATA_FIELD_SIZE;

      if (Reader.BaseStream.Position != offset)
        this.Reader.BaseStream.Seek(offset, SeekOrigin.Begin);
    }
    #endregion

    #region public void SeekWriter(int index)
    public void SeekWriter(int index) {
      this.CheckWriter();
      long offset = DATA_INFO_SIZE + index * DATA_FIELD_SIZE;
      _bwriter.BaseStream.Seek(offset, SeekOrigin.Begin);
    }
    #endregion

    #region public void SaveFileInfo()
    public void SaveFileInfo() {
      lock (_locked) {
        this.CheckWriter();
        _bwriter.BaseStream.Seek(0, SeekOrigin.Begin);
        this.OnSaveFileInfo();
      }
    }
    #endregion

    protected virtual void OnSaveFileInfo() { }

    #region public void LoadFileInfo()
    public void LoadFileInfo() {
      lock (_locked) {
        this.CheckReader();
        if (_breader.BaseStream.Length == 0) {
          this.SaveFileInfo();
          return;
        }
        this.CheckReader();
        _breader.BaseStream.Seek(0, SeekOrigin.Begin);
        _count = Convert.ToInt32((_breader.BaseStream.Length - DATA_INFO_SIZE) / DATA_FIELD_SIZE); ;

        this.OnLoadFileInfo();
      }
    }
    #endregion

    protected virtual void OnLoadFileInfo() { }

    protected long ReadUnixTime() {
      return UNIXTICKS + (long)this._breader.ReadUInt32() * 10000000L;
    }

    protected string ReadString(int count) {
      byte[] bs = _breader.ReadBytes(count);
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach (byte b in bs) if ((int)b != 0) sb.Append((char)b);
      return sb.ToString();
    }

    protected void Write(string str, int lenght) {
      _bwriter.Write(GetArray(str, lenght));
    }

    protected void Write(DateTime time) {
      uint utime = (UInt32)((time.Ticks - UNIXTICKS) / 10000000L);
      Writer.Write(utime);
    }

    protected void CheckWriterOffset(int index) {
      long offset = DATA_INFO_SIZE + index * DATA_FIELD_SIZE;

      if (offset != _bwriter.BaseStream.Position)
        throw (new Exception(string.Format("offset={0} != BarWriter.Write.index={1}", offset, index)));
    }
  }
}
