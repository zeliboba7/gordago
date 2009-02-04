/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.IO;

namespace Gordago {
	
	/// <summary>
	/// Работа по файлу, чтение и запись с кешированием данных
	/// если _writer != null буфер в статусе записи
	/// елси _reader != null буфер в статусе чтения
	/// </summary>
	class BarFileBuffer {
		private BarFileInfo _bfi;
		private BarBuffer _buffer;
		private BarWriter _writer;
		private BarReader _reader;
		
		private string _filename;
		private Symbol _symbol;
		private TimeFrame _tf;

    private bool _isCacheData = false;
		
		internal BarFileBuffer(Symbol symbol, TimeFrame timeframe, string filename) {
			_symbol = symbol;
			_tf = timeframe;
			_filename = filename;
			_buffer = new BarBuffer(timeframe.Second);
			
			if (File.Exists(filename)){
				_bfi = new BarFileInfo(filename);
			}else{
				_bfi = new BarFileInfo(symbol, timeframe, filename);
			}
    }

    #region public bool IsCacheData
    public bool IsCacheData {
      get { return _isCacheData; }
      set { 
        this._isCacheData = value; 
      }
    }
    #endregion

    #region public BarFileInfo Info
    public BarFileInfo Info{
			get{return _bfi;}
		}
		#endregion

		#region public void RemoveAll()
		/// <summary>
		/// Очистка данных, в буфере и в файле
		/// </summary>
		public void RemoveAll(){
			if (_writer != null){
				_writer.Close();
				_writer = null;
			}
			if (_reader != null){
				_reader.Close();
				_reader = null;
			}
			_bfi = new BarFileInfo(_symbol, _tf, _filename);
			_buffer.Clear();
		}
		#endregion

		#region public void AddTick(Tick tick)
		public void AddTick(Tick tick){
			_buffer.AddTick(tick);
			_bfi.CountTick++;

      if (_reader != null) {
        _reader.Close();
        _reader = null;
      }
			if (_writer == null)
				_writer = new BarWriter(_bfi);

			if (_buffer.Full){

				/* буфер полон, необходимо часть записать в файл */
				for (int i=_buffer.FirstIndex;i<_buffer.FirstIndex+BarBuffer.OFFSET_SIZE;i++)
					_writer.Write(_buffer[i], i);
				
				_buffer.MoveNextOffset();
			}
		}
		#endregion

		#region public void Flush()
		/// <summary>
		/// Сохраняет весь кеш баров в файл и закрывает писатель
		/// </summary>
		public void Flush(){
			if (_writer == null) return;

			for (int i=_buffer.FirstIndex;i<=_buffer.EndIndex;i++){
				_writer.Write(_buffer[i], i);
			}
			_writer.Close();
			_writer = null;
		}
		#endregion

		#region public Bar GetBar(int index)
		/// <summary>
		/// Получить БАР из файла или буфера.
		/// Если бар есть в буфере, то возвращаеться из буфера
		/// Если бара нет в буфере, происходит заполнение буфера
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Bar GetBar(int index){
			if (index >= this._bfi.CountBar)
				throw(new Exception(string.Format("BarFileBuffer.GetBar(index) Error: index={0}>BarFileInfo.CountBar={1}", index, this._bfi.CountBar)));

      if (_reader == null)
        _reader = new BarReader(this._bfi);

      return _reader.Read(index);

#if ASDF
      if (_buffer.FirstIndex <= index && index <= _buffer.EndIndex)
				return _buffer[index];

			if (_writer != null) 
				this.Flush();

			if (_buffer.Full){
				_buffer.MoveNextOffset();
				return this.GetBar(index);
			}
			if (_reader == null)
				_reader = new BarReader(this._bfi);
			
			if (index != _buffer.EndIndex+1){
				_buffer.Clear();
				_buffer.OffSet = index;
			}
			
			int rindex = index;
			while(!_buffer.Full && rindex < this._bfi.CountBar){
				_buffer.AddBar(_reader.Read(rindex++));
			}

			return this.GetBar(index);
#endif
		}
		#endregion
	}
}
