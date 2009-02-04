/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;

namespace Gordago {
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
	class BarFileInfo {
		public const int INFO_SIZE = 4+8+4+4+60+44+4+4+4+4;
		public const int INFO_FILE_SIZE = 4+8+4+4+60+44;
		public const int BAR_SIZE = 4*6;
		public const int VERSION = 2;
    public const string COPYRATE = "(C)opyright 2006, Gordago Software Corp.";

		private string _filename;
		private int _counttick, _countbar;
		private long _fromdtm, _todtm;
		private int _version, _decdig;
		private string _symbolname;
		private int _sectimeframe;

		#region public BarFileInfo(string filename)
		public BarFileInfo(string filename) {
			_filename = filename;
			FileStream fs = new FileStream(filename, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);
			_version = br.ReadInt32();

      _symbolname = SymbolManager.GetSymbolName(br.ReadBytes(8));
			_decdig = br.ReadInt32();
			_sectimeframe = br.ReadInt32();
			
			br.ReadBytes(60);
			br.ReadBytes(44);
			
			_counttick = Convert.ToInt32(br.ReadUInt32());

			_countbar = Convert.ToInt32(br.ReadUInt32());
      _fromdtm = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32()).Ticks;
      _todtm = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32()).Ticks;

			fs.Close();
		}
		#endregion

		#region public BarFileInfo(Symbol symbol, TimeFrame timeframe, string filename)
		public BarFileInfo(Symbol symbol, TimeFrame timeframe, string filename){
			_counttick = 0;
			_countbar = 0;
			_filename = filename;
			_symbolname = symbol.Name;
			_decdig = symbol.DecimalDigits;
			_sectimeframe = timeframe.Second;
      _version = VERSION;


			if (File.Exists(filename))
				File.Delete(filename);
			
			SymbolManager.CheckDir(filename);

			FileStream fs = new FileStream(filename, FileMode.CreateNew);
			BinaryWriter bw = new BinaryWriter(fs);

      this.WriteInfo(bw);

			bw.Flush();
			bw.Close();
		}
		#endregion

    public void WriteInfo(BinaryWriter bw) {
      bw.BaseStream.Seek(0, SeekOrigin.Begin);
      bw.Write(VERSION);
      bw.Write(_symbolname.ToCharArray());

      if(_symbolname.Length < 8)
        bw.Write(new byte[8 - _symbolname.Length]);

      bw.Write(_decdig);
      bw.Write(_sectimeframe);
      bw.Write(GetByteFromString(COPYRATE, 60));
      bw.Write(new byte[44]);
      bw.Write((int)_counttick);
      bw.Write((int)_countbar);

      bw.Write(SymbolManager.ConvertDateTimeToUnix(new DateTime(this.FromDateTime)));
      bw.Write(SymbolManager.ConvertDateTimeToUnix(new DateTime(this.ToDateTime)));
    }

    private byte[] GetByteFromString(string str, int cntByte) {
      byte[] bytes = new byte[cntByte];
      char[] chars = str.ToCharArray();
      int cnt = Math.Min(bytes.Length, chars.Length);
      for(int i = 0; i < cnt; i++) {
        bytes[i] = Convert.ToByte(chars[i]);
      }
      return bytes;
    }

    #region public string FileName
    public string FileName {
			get{return _filename;}
		}
		#endregion

		#region public string SymbolName
		public string SymbolName{
			get{return this._symbolname;}
		}
		#endregion

		#region public int CountTick
		public int CountTick{
			get{return this._counttick;}
			set{this._counttick = value;}
		}
		#endregion

		#region public int CountBar
		public int CountBar{
			get{return this._countbar;}
			set{this._countbar = value;}
		}
		#endregion 

		#region public long FromDateTime
		public long FromDateTime{
			get{return _fromdtm;}
			set{this._fromdtm = value;}
		}
		#endregion

		#region public long ToDateTime
		public long ToDateTime{
			get{return this._todtm;}
			set{this._todtm = value;}
		}
		#endregion

		#region public int TFSecond
		public int TFSecond{
			get{return this._sectimeframe;}
		}
		#endregion

    #region public int Version
    public int Version {
      get { return this._version; }
    }
    #endregion

    #region public static long GetOffset(int index)
    public static long GetOffset(int index){
			return BarFileInfo.INFO_SIZE + index * BarFileInfo.BAR_SIZE;
		}
		#endregion
	}
}
