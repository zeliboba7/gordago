/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
#region using 
using System;
using System.IO;
#endregion

namespace Gordago {
	
	/// <summary>
	/// Чтение информации о инструменте (символе)
	/// формат файла:
	///		* * * * * * * * * * * Описатель * * * * * * * * * * 
	///   int				- версия файла
	///   byte[8]		- наименование символа
	///   int				- кол-во знаков после запятой
	///   byte[60]	- copyrite
	///   byte[44]	- резервное
	///   UInt32		- кол-во тиков в данном файле
	///   UInt32		- время первого тика в UNIX формате
	///   UInt32		- время последнего тика в UNIX формате
	///
	///		* * * * * * * * * * * Массив тиков * * * * * * * * * * 
	///   UInt32		- время в UNIX формате
	///   Single		- bid
	/// </summary>
	public class TickFileInfo {
		internal const int INFO_SIZE = 4+8+4+60+44+4+4+4;
		internal const int INFO_FILE_SIZE = 4+8+4+60+44;
		internal const int TICK_SIZE = 8;
		internal const int VERSION = 2;
		
		#region internal static DateTime EMPTY_DATETIME
		internal static DateTime EMPTY_DATETIME{
			get{return new DateTime(1970, 1,1, 0, 0, 0, 0);}
		}
		#endregion

		#region private property

		private string _symbolname;
		private int _decimalDigits = 0;
		private int _cnttick;
		private long _dtmfrom, _dtmto;
		private int _version;
		private string _filename;
		#endregion

		#region public TickFileInfo(string filename) 
		/// <summary>
		/// Чтение данных из файла
		/// </summary>
		/// <param name="filename"></param>
		public TickFileInfo(string filename) {
			_filename = filename;
			FileStream fs = new FileStream(filename, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);
			_version = br.ReadInt32();

      _symbolname = SymbolManager.GetSymbolName(br.ReadBytes(8));
			_decimalDigits = br.ReadInt32();

#if DEBUG
			if (_decimalDigits == 0)
				throw(new Exception(string.Format("DecimalDigits in {0} is 0!", this._symbolname)));
#endif

			
			br.ReadBytes(60);
			br.ReadBytes(44);

      int counttickreal = Convert.ToInt32((br.BaseStream.Length - TickFileInfo.INFO_SIZE) / 8);

			_cnttick = Convert.ToInt32(br.ReadUInt32());
      _cnttick = counttickreal;

      _dtmfrom = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32()).Ticks;
      _dtmto = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32()).Ticks;
			
			if (_version == 1){
				/* в этой версии необходимо подсчитать кол-во тиков */
				_cnttick = ((int)fs.Length - (INFO_SIZE)) / 8;
			}
			fs.Close();
		}
		#endregion

		public TickFileInfo(ISymbol symbol, string filename):this(symbol.Name, symbol.DecimalDigits, filename){
			
		}

		#region public TickFileInfo(string symbolname, int decimaldigits, string filename)
		public TickFileInfo(string symbolname, int decimaldigits, string filename){

#if DEBUG
			if (decimaldigits <= 0)
				throw(new Exception(string.Format("DecimalDigits in {0} is 0!", symbolname)));
#endif

			_filename = filename;
			_symbolname = symbolname;
			_cnttick = 0;
			_dtmfrom = _dtmto = EMPTY_DATETIME.Ticks;
			_decimalDigits = decimaldigits;

			if (System.IO.File.Exists(filename)) 
				System.IO.File.Delete(filename);

			FileStream fs = new FileStream(filename, FileMode.CreateNew);
			BinaryWriter bw = new BinaryWriter(fs);

			bw.Write(VERSION);
			bw.Write(symbolname.ToCharArray());

			if (symbolname.Length<8) 
				bw.Write(new byte[8-symbolname.Length]);
			bw.Write(decimaldigits);
			bw.Write(new byte[60]);
			bw.Write(new byte[44]);
			
			uint counttick = 0;
			bw.Write(counttick);

      bw.Write(SymbolManager.ConvertDateTimeToUnix(EMPTY_DATETIME));
      bw.Write(SymbolManager.ConvertDateTimeToUnix(EMPTY_DATETIME));
			bw.Flush();
			bw.Close();
		}
		#endregion

		#region public string SymbolName
		public string SymbolName{
			get{return this._symbolname;}
		}
		#endregion

		#region public int DecimalDigits
		public int DecimalDigits{
			get{return this._decimalDigits;}
		}
		#endregion

		#region public string FileName
		public string FileName{
			get{return this._filename;}
		}
		#endregion

		#region public int CountTick
		public int CountTick{
			get{return this._cnttick;}
			set{this._cnttick = value;}
		}
		#endregion

		#region public long FromDateTime
		public long FromDateTime{
			get{return this._dtmfrom;}
		}
		#endregion

		#region public long ToDateTime
		public long ToDateTime{
			get{return this._dtmto;}
		}
		#endregion

		#region internal void SetCountTick(int value)
		internal void SetCountTick(int value){
			this._cnttick = value;
		}
		#endregion

		#region internal void SetFromTime(long time)
		internal void SetFromTime(long time){
			this._dtmfrom = time;
		}
		#endregion

		#region internal void SetToTime(long time)
		internal void SetToTime(long time){
			this._dtmto = time;
		}
		#endregion

		#region internal void SetFieldFrom(TickFileInfo tfi)
		internal void SetFieldFrom(TickFileInfo tfi){
			this._cnttick = tfi.CountTick;
			this._decimalDigits = tfi.DecimalDigits;
			this._dtmfrom = tfi.FromDateTime;
			this._dtmto = tfi.ToDateTime;
			this._symbolname = tfi.SymbolName;

			if (_decimalDigits <= 0)
				throw(new Exception(string.Format("DecimalDigits in {0} is 0!", _symbolname)));
		}
		#endregion

		#region public static long GetOffset(int index)
		public static long GetOffset(int index){
			return TickFileInfo.INFO_SIZE + index * TICK_SIZE;
		}
		#endregion
	}
}
