/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;
using System.Collections;

namespace Gordago {
	/// <summary>
	/// Карта по файлу символа - разбивает тиковую иторию по дням.
	/// Формат файла:
	/// int - версия файла
	/// byte[8] - наименование инструмента
	/// byte[64] - резерв
	/// uint - время первого тика дня
	/// int - кол-во тиков за этот день
	/// </summary>
	internal class TickFileMap {

		internal const int INFO_SIZE = 4+8+64;
		private const int TIME_FRAME_SECOND = 86400;

		private string _symbolname;
		private TickFileMapItem[] _items;
		private TickFileInfo _tfi;
		private string _filename;

		#region public TickFileMap(TickFileInfo tfi) 
		public TickFileMap(TickFileInfo tfi) {
			_items = new TickFileMapItem[0];
			_tfi = tfi;
			_filename = tfi.FileName.Replace(".gtk", ".gmp");
			if (!File.Exists(_filename)){
				this.CreateFile();
				return;
			}

			this.Load();

			if (this.GetCountTick() != tfi.CountTick){
				this.CreateFile();
				this._items = new TickFileMapItem[0];
				return;
			}
		}
		#endregion

		#region public int Count
		public int Count{
			get{return this._items.Length;}
		}
		#endregion

		#region public string SymbolName
		public string SymbolName{
			get{return this._symbolname;}
		}
		#endregion

		#region private void CreateFile()
		private void CreateFile(){
			if (File.Exists(_filename))
				File.Delete(_filename);

			FileStream fs = new FileStream(_filename, FileMode.CreateNew);

			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write((int)1);

			bw.Write(_tfi.SymbolName.ToCharArray());
			if (_tfi.SymbolName.Length<8) bw.Write(new byte[8-_tfi.SymbolName.Length]);

			bw.Write(new byte[64]);
			bw.Flush();
			bw.Close();
		}
		#endregion

    #region public void Clear()
    public void Clear(){
			_items = new TickFileMapItem[0];
    }
    #endregion

    #region private void Load()
    private void Load(){

			FileStream fs = new FileStream(_filename, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);

			int version = br.ReadInt32();
      _symbolname = SymbolManager.GetSymbolName(br.ReadBytes(8));
			br.ReadBytes(64);
			
			while(fs.Position < fs.Length) {
        DateTime dtm = SymbolManager.ConvertUnixToDateTime(br.ReadUInt32());
				int cnttick = br.ReadInt32();
				this.AddItem(new TickFileMapItem(dtm.Ticks, cnttick));
			}
      br.Close();
			fs.Close();
		}  
		#endregion

		#region public int GetCountTick()
		public int GetCountTick(){
			int cnt = 0;
			foreach (TickFileMapItem item in this._items){
				cnt += item.CountTick;
			}
			return cnt;
		}
		#endregion

		#region private void AddItem(TickFileMapItem sfmi)
		private void AddItem(TickFileMapItem sfmi){
			ArrayList al = new ArrayList(this._items);
			al.Add(sfmi);
			_items = (TickFileMapItem[])al.ToArray(typeof(TickFileMapItem));
		}
		#endregion

		#region public void AddTick(Tick tick)
		public void AddTick(Tick tick){
			if (this._items.Length == 0){
				this.AddItem(new TickFileMapItem(tick.Time, 1));
				return;
			}
			
			TickFileMapItem item = this._items[_items.Length-1];

			long sec1 = tick.Time / 10000000L;
			long sec2 = item.Time / 10000000L;

			if ( sec1 / TIME_FRAME_SECOND - sec2 / TIME_FRAME_SECOND > 0 ){
				this.AddItem(new TickFileMapItem(tick.Time, 1));
			}else{
				item.CountTick++;
			}
		}
		#endregion

		#region public TickFileMapItem this[int index]
		public TickFileMapItem this[int index]{
			get{return this._items[index];}
		}
		#endregion

		#region public void Save()
		public void Save(){
			
			if (File.Exists(_filename)) File.Delete(_filename);

			FileStream fs = new FileStream(_filename, FileMode.CreateNew);

			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write((int)1);

			bw.Write(_tfi.SymbolName.ToCharArray());
			if (_tfi.SymbolName.Length<8) bw.Write(new byte[8-_tfi.SymbolName.Length]);

			bw.Write(new byte[64]);
			
			foreach (TickFileMapItem item in this._items){
        bw.Write(SymbolManager.ConvertDateTimeToUnix(new DateTime(item.Time)));
				bw.Write(item.CountTick);
			}

			bw.Flush();
			bw.Close();
		}
		#endregion

		#region public int GetOffset(int periodday)
		/// <summary>
		/// Получить смещение в кол-ве тиков в файле относительно периода
		/// </summary>
		public int GetOffset(int periodday){
			if (periodday == 0)
				return 0;
			DateTime fromdtm = DateTime.Now.AddDays(-periodday);
			int offset = 0;
//			foreach(TickFileMapItem sfmi in this._items){
//				if (sfmi.DateTime < fromdtm){
//					offset += sfmi.CountTick;
//				}else{
//					break;
//				}
//			}
			return offset;
		}
		#endregion

	}

	#region internal class TickFileMapItem
	internal class TickFileMapItem{

		private long _time;
		private int _counttick;

		public TickFileMapItem(long time, int counttick){
			_time = time;
			_counttick = counttick;
		}

		#region public long Time 
		public long Time {
			get{return this._time;}
			set{this._time = value;}
		}
		#endregion

		#region public int CountTick
		public int CountTick{
			get{return this._counttick;}
			set{this._counttick = value;}
		}
		#endregion
	}
	#endregion
}
