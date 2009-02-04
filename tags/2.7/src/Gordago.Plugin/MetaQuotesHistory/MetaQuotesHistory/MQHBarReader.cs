using System;
using System.IO;
using System.Text;

namespace Gordago.PlugIn.MetaQuotesHistory {

  /// <summary>
  /// Формат файла MetaQuotes 4:
  ///		int				- версия файла
  ///		byte[64]	- copyrate
  ///		byte[12]	- Наименование инструмента
  ///		int				- ТаймФрейм в минутах
  ///		int				- Кол-во знаков после запятой
  ///		byte[60]	- резерв
  ///   * * * * * * * * * * * * Массив баров * * * * * * * * * * * * 
  ///   UInt32		- время бара в UNIX формате
  ///   Double		- цена Open
  ///   Double		- цена Low
  ///   Double		- цена High
  ///   Double		- цена Close
  ///		Double		- Volume
  /// </summary>
  class MQHBarReader {
    private static long UNIXTICKS = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

    public const int INFO_SIZE = 4+64+12+4+4+60;
    public const int BAR_SIZE = 44;

    private string _filename;
    private int _count;
    private long _fromdtm, _todtm;
    private int _timeFrameSecond;
    private string _symbolName;
    private int _decimalDigits;

    private BinaryReader _breader;
    private MQHBarBuffer _buffer;

    public MQHBarReader(string filename, MQHSymbolManager mqhSymbols) {
      _filename = filename;
      _buffer = new MQHBarBuffer();
      FileStream fs = new FileStream(filename, FileMode.Open);
      BinaryReader br = new BinaryReader(fs);
      int version = br.ReadInt32();
      br.ReadBytes(64);

      _symbolName = GetSymbolName(br.ReadBytes(12));
      int minute = br.ReadInt32();
      _timeFrameSecond = minute * 60;
      _decimalDigits = br.ReadInt32();

      br.ReadBytes(60);

      _count = (int)(br.BaseStream.Length - br.BaseStream.Position) / BAR_SIZE;

      _fromdtm = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32()).Ticks;

      br.BaseStream.Seek(-BAR_SIZE, SeekOrigin.End);
      _todtm = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32()).Ticks;

      fs.Close();

      if(mqhSymbols == null) return;

      MQHSymbol symbol = mqhSymbols.GetSymbol(_symbolName);
      if(symbol == null) return;

      MQHBarList bars = symbol.Ticks.GetBarList(_timeFrameSecond) as MQHBarList;
      if(bars == null) return;


      bars.SetBarReader(this);
    }

    #region public string SymbolName
    public string SymbolName {
      get { return this._symbolName; }
    }
    #endregion

    #region public string FileName
    public string FileName {
      get { return _filename; }
    }
    #endregion

    #region public int DecimalDigits
    public int DecimalDigits {
      get { return this._decimalDigits; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return this._count; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { return new DateTime(_fromdtm); }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { return new DateTime(_todtm); }
    }
    #endregion

    #region public int TimeFrameSecond
    public int TimeFrameSecond {
      get { return this._timeFrameSecond; }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
        if(index >= this._count)
          throw (new Exception(string.Format("Error: index={0} out of range", index, this._count)));

        if(_buffer.FirstIndex <= index && index <= _buffer.EndIndex)
          return _buffer[index];

        if(_buffer.Full) {
          _buffer.MoveNextOffset();
          return this[index];
        }

        if(index != _buffer.EndIndex + 1) {
          _buffer.Clear();
          _buffer.OffSet = index;
        }

        int rindex = index;
        while(!_buffer.Full && rindex < this._count) {
          _buffer.AddBar(this.Read(rindex++));
        }
        this.Close();
        return this[index];
      }
    }
    #endregion

    #region private static long GetOffset(int index)
    private static long GetOffset(int index) {
      return INFO_SIZE + index * BAR_SIZE;
    }
    #endregion

    #region private Bar Read(int index)
    private Bar Read(int index) {
      if(_breader == null) {
        FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        _breader = new BinaryReader(fs);
      }

      long offset = GetOffset(index);
      if(_breader.BaseStream.Position != offset)
        _breader.BaseStream.Seek(offset, SeekOrigin.Begin);

      long time = UNIXTICKS + (long)_breader.ReadUInt32() * 10000000;
      float open = Convert.ToSingle(_breader.ReadDouble());
      float low = Convert.ToSingle(_breader.ReadDouble());
      float high = Convert.ToSingle(_breader.ReadDouble());
      float close = Convert.ToSingle(_breader.ReadDouble());
      int volume = Convert.ToInt32(_breader.ReadDouble());

      return new Bar(open, low, high, close, volume, time);
    }
    #endregion

    #region public void Close()
    public void Close() {
      if(_breader != null) {
        _breader.Close();
      }
      _breader = null;
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
      if(this.Count == 0) return 0;

      long periodFrom = this.TimeFrom.Ticks;
      long periodTo = this.TimeTo.Ticks;
      long step = this.TimeFrameSecond;

      if(periodFrom <= time.Ticks && time.Ticks <= periodTo) { 
        /* время входит в период таймсерии */

        long delim = (periodTo - time.Ticks) / (step * 10000000L);

        int delta = (int)Math.Max((delim), 0);

        int count = this.Count;

        delta = Math.Max(count - delta - 10, 0);
        int sec = this.TimeFrameSecond;
        int retIndex = count - 1;
        for(int i = delta; i < count; i++) {
          Bar bar = this[i];
          long barTime = bar.Time.Ticks;

          if(barTime <= time.Ticks && time.Ticks < barTime + sec * 10000000L) {
            retIndex = i;
            break;
          }
        }
        return retIndex;
      } else if(time.Ticks < periodFrom) { /* время меньше периода */
        long delta = (periodFrom - time.Ticks) / (step * 10000000L);
        return this.Count + Convert.ToInt32(delta) * -1;
      } else {
        long delta = (time.Ticks - periodTo) / (step * 10000000L);
        return this.Count + Convert.ToInt32(delta);
      }
    }
    #endregion

    #region private static string GetSymbolName(byte[] bs)
    private static string GetSymbolName(byte[] bs) {
      StringBuilder sb = new StringBuilder();
      foreach(byte b in bs) 
        if((int)b != 0) sb.Append((char)b);
      return sb.ToString();
    }
    #endregion

    #region public static DateTime ConvertUnixToDateTime(double ctm)
    public static DateTime ConvertUnixToDateTime(double ctm) {
      return new DateTime((long)(ctm * 10000000L) + UNIXTICKS);
    }
    #endregion
  }
}
