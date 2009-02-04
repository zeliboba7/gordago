/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;

namespace Gordago {
	class TickFileBuffer {
		private TickFileInfo _tfi;
		private TickBuffer _buffer;
		private TickReader _reader;
		private TickWriter _writer;

		private Symbol _symbol;
    private int _savedIndex = -1;
    private Tick _savedTick;

		public TickFileBuffer(Symbol symbol, TickFileInfo tfi) {
			_symbol = symbol;
			_tfi = tfi;

			if (_tfi.DecimalDigits <= 0)
				throw(new Exception(string.Format("DecimalDigits in {0} is 0!", _symbol.Name)));

			_buffer = new TickBuffer();
		}

		#region public TickFileInfo Info
		public TickFileInfo Info{
			get{return this._tfi;}
		}
		#endregion

		#region public void Flush()
		/// <summary>
		/// Сохраняет весь кеш баров в файл и закрывает писатель
		/// </summary>
		public void Flush(){
			if (_writer == null) return;
			for (int i=_buffer.FirstIndex;i<=_buffer.EndIndex;i++)
				_writer.Write(_buffer[i], i);
			
			_writer.Close();
			_writer = null;
		}
		#endregion

		#region public void RemoveAll()
		public void RemoveAll(){
			this.CloseAllStream();
			_tfi = new TickFileInfo(this._symbol, _tfi.FileName);
		}
		#endregion

		#region internal void CloseAllStream()
		internal void CloseAllStream(){
			if (_writer != null){
				_writer.Close();
				_writer = null;
			}
			if (_reader != null){
				_reader.Close();
				_reader = null;
			}
			_buffer.Clear();
		}
		#endregion

		#region public void AddTick(Tick tick)
		public void AddTick(Tick tick){

			if (_reader != null){
				_reader.Close();
				_reader = null;
			}

			if (_writer == null){
				_writer = new TickWriter(_tfi);
				_writer.SetOffset(_tfi.CountTick);
				_buffer.Clear(_tfi.CountTick);
			}

			_buffer.Add(tick);
			_tfi.SetCountTick(_tfi.CountTick+1);
//      _tfi.SetFromTime(Math.Min(_tfi.FromDateTime, tick.Time));
      _tfi.SetToTime(tick.Time);


			if (_buffer.Full){

				/* буфер полон, необходимо часть записать в файл */
        for(int i = _buffer.FirstIndex; i < _buffer.FirstIndex + TickManager.OFFSET_SIZE; i++)
					_writer.Write(_buffer[i], i);
				
				_buffer.MoveNextOffset();
			}
		}
		#endregion

		#region public Tick GetTick(int index)
    public Tick GetTick(int index) {

      if (_writer != null) {
        this.CloseAllStream();
        // throw (new Exception("GetTick Error: Writer not null"));
      }

      if (_reader == null)
        _reader = new TickReader(this._tfi);

      if (index == _savedIndex)
        return _savedTick;

      _savedIndex = index;
      _savedTick = _reader.Read(index);

      return _savedTick;

      //if (index >= this._tfi.CountTick)
      //  throw (new Exception(string.Format("TickFileBuffer.GetTick(index) Error: index={0}>TickFileInfo.CountTick={1}", index, this._tfi.CountTick)));

      //if (_buffer.FirstIndex <= index && index <= _buffer.EndIndex)
      //  return _buffer[index];

      //if (_writer != null)
      //  this.Flush();

      //bool last = false;
      //if (index < _buffer.FirstIndex) {
      //  _buffer.Clear();
      //  _buffer.OffSet = Math.Max(index - TickManager.OFFSET_SIZE, 0);
      //  last = true;
      //}else if (_buffer.Full) {
      //  _buffer.MoveNextOffset();
      //  return this.GetTick(index);
      //}
      //if (_reader == null)
      //  _reader = new TickReader(this._tfi);

      //if (index != _buffer.EndIndex + 1 && !last) {
      //  _buffer.Clear();
      //  _buffer.OffSet = index;
      //}

      //int rindex = index;
      //if (last)
      //  rindex = _buffer.OffSet;

      //while (!_buffer.Full && rindex < this._tfi.CountTick) {
      //  _buffer.Add(_reader.Read(rindex++));
      //}

      //return this.GetTick(index);
    }
		#endregion

		#region public void Update(TickCollection ticks)
		public void Update(TickCollection ticks){
			if (ticks.Count == 0) return;

			long timefrom = ticks[0].Time;
			long timeto = ticks.Current.Time;

			DebugWriteLine("Обновление данных: ");
			DebugWriteLine(string.Format("Кол-во тиков основной: {0}", _tfi.CountTick));
			DebugWriteLine(string.Format("Кол-во тиков добавляемой: {0}", ticks.Count));

			DebugWriteLine(string.Format("MainFromTime:{0}",new DateTime(this._tfi.FromDateTime).ToString()));
			DebugWriteLine(string.Format("MainToTime  :{0}",new DateTime(this._tfi.ToDateTime).ToString()));
			DebugWriteLine(string.Format("InFromTime  :{0}",new DateTime(timefrom).ToString()));
			DebugWriteLine(string.Format("InToTime    :{0}",new DateTime(timeto).ToString()));


			bool islightmethod = false;
			if (timefrom > _tfi.ToDateTime || this._tfi.CountTick == 0){ /* OK */
				/* добавление в конец */
				DebugWriteLine(string.Format("добавление в конец:"));

				for (int i=0;i<ticks.Count;i++)
					this.AddTick(ticks[i]);

				islightmethod = true;
			}else if (timefrom <= _tfi.FromDateTime && _tfi.ToDateTime <= timeto){ 
				/* замена текущей на новую побольше */
				DebugWriteLine(string.Format("замена текущей на новую побольше:"));

				this.RemoveAll();
				for (int i=0;i<ticks.Count;i++)
					this.AddTick(ticks[i]);

				islightmethod = true;
			}

			if (islightmethod){
				this.Flush();
				return;
			}

			TickFileInfo tmptfi = CreateTempTickFileInfo(this._tfi);
			TickWriter tmpwriter = new TickWriter(tmptfi);
			int windex = 0;
				
			if (timeto < _tfi.FromDateTime){ /* */
				/* вставка в начало */
				DebugWriteLine(string.Format("вставка в начало:"));

				int cnt = ticks.Count;
				for (int i=0;i<cnt;i++)
					tmpwriter.Write(ticks[i], windex++);
				for (int i=0;i<this._tfi.CountTick;i++)
					tmpwriter.Write(this.GetTick(i), windex++);

			}else if (_tfi.FromDateTime < timefrom && timeto < _tfi.ToDateTime){ /* OK */
				/* вставка тиков внутрь истории */
				DebugWriteLine(string.Format("вставка тиков внутрь истории"));

				int inindex = 0;
				for (int i=0;i<_tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time >= timefrom){
						inindex = i;
						break;
					}
					tmpwriter.Write(tick, windex++);
				}
				for (int i=0;i<ticks.Count;i++)
					tmpwriter.Write(ticks[i], windex++);

				for (int i=inindex;i<_tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time > timeto)
						tmpwriter.Write(tick, windex++);
				}

			}else if (timefrom <= _tfi.FromDateTime && timefrom < _tfi.ToDateTime && timeto > _tfi.FromDateTime){
				/* вставка тиков в начало истории с наложением */
				DebugWriteLine(string.Format("вставка тиков в начало истории с наложением"));

				for (int i=0;i<ticks.Count;i++)
					tmpwriter.Write(ticks[i], windex++);

				for (int i=0;i < _tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time > timeto)
						tmpwriter.Write(tick, windex++);
				}
				
			}else if (timefrom > _tfi.FromDateTime && timefrom < _tfi.ToDateTime && timeto >= _tfi.ToDateTime){
				/* добавление тиков к имеющейся истории с наложением */
				DebugWriteLine(string.Format("добавление тиков к имеющейся истории с наложением "));

				for (int i=0;i<_tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time >= timefrom)
						break;
					tmpwriter.Write(tick, windex++);
				}
				for (int i=0;i<ticks.Count;i++)
					tmpwriter.Write(ticks[i], windex++);
			}
      // System.Diagnostics.Debug.WriteLine("begin: " + _tfi.CountTick);
			this.CloseAllStream();
			CloseTempTickFileInfo(windex, this._tfi, tmpwriter);
      // System.Diagnostics.Debug.WriteLine("end: " + _tfi.CountTick);
    }
		#endregion

		#region private void DebugWriteLine(string str)
		private void DebugWriteLine(string str){
			System.Diagnostics.Debug.WriteLine(str);
		}
		#endregion

		#region public void Update(TickFileInfo tfifrom)
		public void Update(TickFileInfo tfifrom){

			long timefrom = tfifrom.FromDateTime;
			long timeto = tfifrom.ToDateTime;

			DebugWriteLine("Обновление данных: ");
			DebugWriteLine(string.Format("Кол-во тиков основной: {0}", _tfi.CountTick));
			DebugWriteLine(string.Format("Кол-во тиков добавляемой: {0}", tfifrom.CountTick));

			DebugWriteLine(string.Format("MainFromTime:{0}",new DateTime(this._tfi.FromDateTime).ToString()));
			DebugWriteLine(string.Format("MainToTime  :{0}",new DateTime(this._tfi.ToDateTime).ToString()));
			DebugWriteLine(string.Format("InFromTime  :{0}",new DateTime(timefrom).ToString()));
			DebugWriteLine(string.Format("InToTime    :{0}",new DateTime(timeto).ToString()));


			bool islightmethod = false;
			TickReader readerfrom = new TickReader(tfifrom);

			if (timefrom > _tfi.ToDateTime || this._tfi.CountTick == 0){ /* OK */
				/* добавление в конец */
				DebugWriteLine(string.Format("добавление в конец:"));

				for (int i=0;i<tfifrom.CountTick;i++){
					this.AddTick(readerfrom.Read(i));
				}

				islightmethod = true;
			}else if (timefrom <= _tfi.FromDateTime && _tfi.ToDateTime <= timeto){ 
				/* замена текущей на новую побольше */
				DebugWriteLine(string.Format("замена текущей на новую побольше:"));

				this.RemoveAll();
				for (int i=0;i<tfifrom.CountTick;i++)
					this.AddTick(readerfrom.Read(i));

				islightmethod = true;
			}

			if (islightmethod){
				this.Flush();
				DebugWriteLine(string.Format("Кол-во тиков после: {0}", _tfi.CountTick));
				readerfrom.Close();
				return;
			}

			TickFileInfo tmptfi = CreateTempTickFileInfo(this._tfi);
			TickWriter tmpwriter = new TickWriter(tmptfi);
			int windex = 0;
				
			if (timeto < _tfi.FromDateTime){ /* */
				/* вставка в начало */
				DebugWriteLine(string.Format("вставка в начало:"));

				int cnt = tfifrom.CountTick;
				for (int i=0;i<cnt;i++)
					tmpwriter.Write(readerfrom.Read(i), windex++);
				for (int i=0;i<this._tfi.CountTick;i++)
					tmpwriter.Write(this.GetTick(i), windex++);

			}else if (_tfi.FromDateTime < timefrom && timeto < _tfi.ToDateTime){ /* OK */
				/* вставка тиков внутрь истории */
				DebugWriteLine(string.Format("вставка тиков внутрь истории"));

				int inindex = 0;
				for (int i=0;i<_tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time >= timefrom){
						inindex = i;
						break;
					}
					tmpwriter.Write(tick, windex++);
				}
				for (int i=0;i<tfifrom.CountTick;i++)
					tmpwriter.Write(readerfrom.Read(i), windex++);

				for (int i=inindex;i<_tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time > timeto)
						tmpwriter.Write(tick, windex++);
				}

			}else if (timefrom <= _tfi.FromDateTime && timefrom < _tfi.ToDateTime && timeto > _tfi.FromDateTime){
				/* вставка тиков в начало истории с наложением */
				DebugWriteLine(string.Format("вставка тиков в начало истории с наложением"));

				for (int i=0;i<tfifrom.CountTick;i++)
					tmpwriter.Write(readerfrom.Read(i), windex++);

				for (int i=0;i < _tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time > timeto)
						tmpwriter.Write(tick, windex++);
				}
				
			}else if (timefrom > _tfi.FromDateTime && timefrom < _tfi.ToDateTime && timeto >= _tfi.ToDateTime){
				/* добавление тиков к имеющейся истории с наложением */
				DebugWriteLine(string.Format("добавление тиков к имеющейся истории с наложением "));

				for (int i=0;i<_tfi.CountTick;i++){
					Tick tick = this.GetTick(i);
					if (tick.Time >= timefrom)
						break;
					tmpwriter.Write(tick, windex++);
				}
				for (int i=0;i<tfifrom.CountTick;i++)
					tmpwriter.Write(readerfrom.Read(i), windex++);
			}
			readerfrom.Close();
			this.CloseAllStream();
			CloseTempTickFileInfo(windex, this._tfi, tmpwriter);
    }
		#endregion

		#region internal static TickFileInfo CreateTempTickFileInfo(TickFileInfo fromtfi)
		internal static TickFileInfo CreateTempTickFileInfo(TickFileInfo fromtfi){
			string tmpfilename = fromtfi.FileName.Replace(".gtk", ".tmp");
			TickFileInfo tmptfi = new TickFileInfo(fromtfi.SymbolName, fromtfi.DecimalDigits, tmpfilename);
			return tmptfi;
		}
		#endregion

    #region internal static void CloseTempTickFileInfo(int counttick, TickFileInfo fromtfi, TickWriter tmpwriter)
    internal static void CloseTempTickFileInfo(int counttick, TickFileInfo fromtfi, TickWriter tmpwriter){
			tmpwriter.Info.SetCountTick(counttick);

			tmpwriter.Close();
			File.Delete(fromtfi.FileName);
			File.Move(tmpwriter.Info.FileName, fromtfi.FileName);
			File.Delete(tmpwriter.Info.FileName);
			fromtfi.SetFieldFrom(tmpwriter.Info);
    }
    #endregion
  }
}

