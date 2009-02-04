using System;
using System.Collections.Generic;
using System.Text;


namespace Gordago.PlugIn.MetaQuotesHistory {

  /// <summary>
  /// Буферизация баров
  /// </summary>
  class MQHBarBuffer {
    public const int BUFFER_SIZE = 6144;
    public const int BUFFER_MIN_SIZE = 4096;
    public const int OFFSET_SIZE = BUFFER_SIZE - BUFFER_MIN_SIZE;

    private int _offset = 0;
    private int _size;
    private Bar[] _bars;

    public MQHBarBuffer() {
      _bars = new Bar[BUFFER_SIZE];
      _offset = _size = 0;
    }

    #region public int Size
    public int Size {
      get { return this._size; }
    }
    #endregion

    #region public int OffSet
    public int OffSet {
      get { return this._offset; }
      set { this._offset = value; }
    }
    #endregion

    #region public int FirstIndex
    public int FirstIndex {
      get { return this._offset; }
    }
    #endregion

    #region public int EndIndex
    public int EndIndex {
      get { return this._offset + this._size - 1; }
    }
    #endregion

    #region public bool Full
    public bool Full {
      get { return _size == BUFFER_SIZE; }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
        return _bars[index - this._offset];
      }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get { return this._bars[_size - 1]; }
      set { this._bars[_size - 1] = value; }
    }
    #endregion

    #region public void AddBar(Bar bar)
    public void AddBar(Bar bar) {
      _bars[_size++] = bar;
    }
    #endregion

    #region public void MoveNextOffset()
    public void MoveNextOffset() {
      Bar[] tbars = new Bar[BUFFER_SIZE];
      int offset = OFFSET_SIZE;
      Array.Copy(_bars, offset, tbars, 0, BUFFER_MIN_SIZE);
      _bars = tbars;
      _offset += offset;
      _size = BUFFER_MIN_SIZE;
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _offset = _size = 0;
    }
    #endregion
  }
}
