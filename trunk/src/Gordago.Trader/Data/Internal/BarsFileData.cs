/**
* @version $Id: BarsFileData.cs 3 2009-02-03 12:52:09Z AKuzmin $
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

  /// <summary>
  /// Информация о баре
  /// формат файла:
  ///		int				- версия файла
  ///		byte[8]		- наименование символа
  ///		int				- кол-во знаков после запятой
  ///		int				- таймфрейм в секундах
  ///		byte[60]	- copyrite
  ///		byte[44]	- резервное
  ///		int				- кол-во тиков, из которого был построен данный таймфрейм - необходим для идентификации
  ///   int				- кол-во баров в данном файле
  ///   UInt32		- время первого бара в UNIX формате
  ///   UInt32		- время последнего бара в UNIX формате
  ///   
  ///   * * * * * * * * * * * * Массив баров * * * * * * * * * * * * 
  ///   UInt32		- время бара в UNIX формате
  ///   Single		- цена Open
  ///   Single		- цена High
  ///   Single		- цена Low
  ///   Single		- цена Close
  ///		int				- Volume
  /// </summary>
  class BarsFileData:FileReadWrite {
    internal static readonly DateTime EMPTY_DATETIME = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    public const int INFO_SIZE = 4 + 8 + 4 + 4 + 60 + 44 + 4 + 4 + 4 + 4;
    public const int INFO_FILE_SIZE = 4 + 8 + 4 + 4 + 60 + 44;
    public const int BAR_SIZE = 4 * 6;
    public const int VERSION = 2;
    public const string COPYRATE = "(C)opyright 2007, Gordago Software Corp.";

    private string _symbolName;
    private int _digits, _timeFrameSecond, _countTicks;
    private int _version;
    private int _lastIndex = -1;
    private Bar _lastBar;


    public BarsFileData(FileInfo file)
      : base(file, INFO_SIZE, BAR_SIZE) {
    }

    #region public string SymbolName
    public string SymbolName {
      get { return _symbolName; }
      set { this._symbolName = value; }
    }
    #endregion

    #region public int Digits
    public int Digits {
      get { return this._digits; }
      set { this._digits = value; }
    }
    #endregion

    #region public int TimeFrameSecond
    public int TimeFrameSecond {
      get { return _timeFrameSecond; }
      set{this._timeFrameSecond = value;}
    }
    #endregion

    #region public int CountTicks
    public int CountTicks {
      get { return this._countTicks; }
      set { this._countTicks = value; }
    }
    #endregion

    #region protected override void OnSaveFileInfo()
    protected override void OnSaveFileInfo() {
      this.Writer.Write(VERSION);
      this.Write(_symbolName, 8);
      this.Writer.Write(_digits);
      this.Writer.Write(_timeFrameSecond);
      this.Write(COPYRATE, 60);
      this.Writer.Write(new byte[44]);
      this.Writer.Write(_countTicks);
      this.Writer.Write(new byte[12]);
    }
    #endregion

    #region protected override void OnLoadFileInfo()
    protected override void OnLoadFileInfo() {
      _version = Reader.ReadInt32();

      _symbolName = this.ReadString(8);
      _digits = Reader.ReadInt32();
      _timeFrameSecond = Reader.ReadInt32();

      Reader.ReadBytes(60);
      Reader.ReadBytes(44);

      _countTicks = Convert.ToInt32(Reader.ReadUInt32());
    }
    #endregion

    #region public Bar Read(int index)
    public Bar Read(int index) {

      if (_lastIndex == index)
        return _lastBar;

      lock (Locked) {

        this.SeekReader(index);

        long time = ReadUnixTime();
        float open = Reader.ReadSingle();
        float high = Reader.ReadSingle();
        float low = Reader.ReadSingle();
        float close = Reader.ReadSingle();
        int volume = Reader.ReadInt32();
        
        _lastBar = new Bar(open, low, high, close, volume, time);
        _lastIndex = index;
        
        return _lastBar;
       }
    }
    #endregion

    #region public void Write(Bar bar, int index)
    public void Write(Bar bar, int index) {
      lock (Locked) {
        this.CheckWriterOffset(index);

        this.Write(bar.Time);
        Writer.Write(bar.Open);
        Writer.Write(bar.High);
        Writer.Write(bar.Low);
        Writer.Write(bar.Close);
        Writer.Write(bar.Volume);
      }
    }
    #endregion
  }
}
