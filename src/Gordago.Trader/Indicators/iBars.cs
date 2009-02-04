/**
* @version $Id: iBars.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Gordago.Trader.Builder;
  
  using System.Diagnostics;
  using Gordago.Core;

  /* 
   * Менеджер истории.
   * Буферизация, ограничитель основной истории
   */

  public class iBars : Indicator, IFunction {

    private readonly FxClose _fxClose;
    private readonly FxOpen _fxOpen;
    private readonly FxHigh _fxHigh;
    private readonly FxLow _fxLow;
    private readonly FxMedian _fxMedian;
    private readonly FxTypical _fxTypical;
    private readonly FxWeighted _fxWeighted;

    private readonly IBarsData _barsData;
    private readonly SessionSource _sessionSource;

    private readonly Bar[] _buffer ;
    private int _bufferSize = 0;
    private int _bufferOffset;

    private bool _initLimit = false;

    private int _limit = -1;
    private int _position = 0, _cntBarInW;
    private int _beginIndex = 0, _limitCount;
    

    public iBars(IBarsData barsData) {
      _barsData = barsData;

      ISession session = barsData as ISession;
      if (session == null)
        throw (new ArgumentException("Parameter is not a ISession", "barsData"));
      _sessionSource = new SessionSource(session.Session);

      int bLenght = Math.Min(Math.Max(300, _barsData.Count), 2000);
      _buffer = new Bar[bLenght];
      Trace.WriteLine(string.Format("Initialize bars buffer: size={0}", bLenght));

      _fxClose = new FxClose(this);
      _fxHigh = new FxHigh(this);
      _fxLow = new FxLow(this);
      _fxOpen = new FxOpen(this);
      _fxMedian = new FxMedian(this);
      _fxTypical = new FxTypical(this);
      _fxWeighted = new FxWeighted(this);
    }

    #region public int Limit
    public int Limit {
      get { return _limit; }
      set {
        
        _limit = Math.Max(value, -1);
        if (_limit > -1) 
          _limit = Math.Max(_limit, 100);

        // this.SetLimit(_position, _cntBarInW);
      }
    }
    #endregion

    #region private int BeginIndex
    private int BeginIndex {
      get { return _beginIndex; }
    }
    #endregion

    #region private int LimitCount
    private int LimitCount {
      get { return _limitCount; }
    }
    #endregion

    #region internal SessionSource Session
    internal SessionSource Session {
      get { 
        return _sessionSource;
      }
    }
    #endregion

    #region internal IBarsData BarsData
    internal IBarsData BarsData {
      get { return _barsData; }
    }
    #endregion

    #region public FxClose Close
    public FxClose Close {
      get { return _fxClose; }
    }
    #endregion

    #region public FxOpen Open
    public FxOpen Open {
      get { return _fxOpen; }
    }
    #endregion

    #region public FxHigh High
    public FxHigh High {
      get { return this._fxHigh; }
    }
    #endregion

    #region public FxLow Low
    public FxLow Low {
      get { return this._fxLow; }
    }
    #endregion

    #region public float this[int barsBack]
    public float this[int barsBack] {
      get { return _fxClose[barsBack]; }
    }
    #endregion

    #region public int Count
    public int Count {
      get { return _fxClose.Count; }
    }
    #endregion

    #region public IFunctionItemCollection Items
    public IFunctionItemCollection Items {
      get { return _fxClose.Items; }
    }
    #endregion

    #region internal void SetLimit(int position, int cntBarInW)
    internal void SetLimit(int position, int cntBarInW) {
    
      cntBarInW = Math.Max(500, cntBarInW);

      _position = position;
      _cntBarInW = cntBarInW;

      if (_limit == -1)
        return;

      int limit = Math.Max(cntBarInW, _limit);
      if (!_initLimit) {

      }
      
      _beginIndex = Math.Max(position - limit - cntBarInW * 2, 0);

      int pl = Math.Max(position-limit, 0);
      int p2 = position + cntBarInW;

      Trace.WriteLine(string.Format("pl={0}, _beginIndex={1}, _beginIndex + cntBarInW={2}",
        pl, _beginIndex, _beginIndex + cntBarInW));

      if (_beginIndex >= pl && pl < _beginIndex + cntBarInW && p2 < _beginIndex + _limitCount)
        return;

      _limitCount = position + cntBarInW * 2 - _beginIndex;

      Debug.WriteLine(string.Format("_beginIndex={0}, _limitCount={1}", _beginIndex, _limitCount));
      _sessionSource.IncrementsLevel1();
    }
    #endregion

    #region public T CreateDefaultInstrument<T>() where T:class
    public T CreateDefaultInstrument<T>() where T : class {
      ClassBuilder cb = new ClassBuilder(typeof(T));
      ParameterCollection inputs = new ParameterCollection();
      inputs.AddRange(cb.Parameters);
      Parameter iBarsParam = inputs["iBars"];
      if (iBarsParam == null) {
        iBarsParam = new Parameter(this, "iBars");
        inputs.Add(iBarsParam);
      }

      iBarsParam.Value = this;

      return cb.CreateInstance(inputs.ToArray()) as T;
    }
    #endregion

    #region public Bar GetBar(int index)
    public Bar GetBar(int index) {
      int findex = index + this.BeginIndex;
      int bLenght = _buffer.Length;

      Bar bar;
      if (findex >= _bufferOffset && findex < _bufferOffset + _bufferSize) {
        bar = _buffer[findex - _bufferOffset];
      } else {
        if (findex >= _bufferOffset + bLenght) {
          _bufferSize = 0;
        } else if (findex < _bufferOffset) {
          _bufferSize = 0;
        } else if (findex > _bufferOffset + _bufferSize) { // внутри буфера, но с разрывом - скачек на новое значение
          _bufferSize = 0;
        } else if (_bufferSize == bLenght) {
          _bufferSize = 0;
        }
        if (_bufferSize == 0) {
          _bufferOffset = findex;
        }
        _buffer[_bufferSize++] = bar = _barsData[findex];
      }
      return bar;
    }
    #endregion

    #region public class RateData : IFunction
    public class RateData : IFunction {

      #region internal enum ValueType
      internal enum ValueType {
        Open,
        High,
        Low,
        Close,
        Median,
        Typical,
        Weighted
      }
      #endregion

      private readonly ItemCollection _items;
      private readonly ValueType _valueType;
      private readonly iBars _owner;
      private readonly IBarsData _barsData;

      #region internal RateData(iBars ibars, ValueType valueType)
      internal RateData(iBars ibars, ValueType valueType) {
        _barsData = ibars._barsData;
        _owner = ibars;
        _items = new ItemCollection(this, ibars.BarsData, valueType);
        _valueType = valueType;
      }
      #endregion

      #region public iBars iBars
      [Parameter("iBars")]
      public iBars iBars {
        get { return this._owner; }
      }
      #endregion

      #region internal SessionSource Session
      internal SessionSource Session {
        get { 
          return _owner.Session; 
        }
      }
      #endregion

      #region public IFunctionItemCollection Items
      public IFunctionItemCollection Items {
        get { return _items; }
      }
      #endregion

      #region public float this[int barsBack]
      public float this[int barsBack] {
        get {
          return _items[this.Count - 1 - barsBack];
        }
      }
      #endregion

      #region public int Count
      public int Count {
        get { 
          if (_owner._limit == -1)
            return _barsData.Count;
          
          return _owner.LimitCount;
        }
      }
      #endregion

      #region class ItemCollection : IFunctionItemCollection
      class ItemCollection : IFunctionItemCollection {

        private readonly IBarsData _bars;
        private readonly ValueType _valueType;
        private readonly RateData _owner;
        public ItemCollection(RateData owner, IBarsData bars, ValueType valueType) {
          _owner = owner;
          _bars = bars;
          _valueType = valueType;
        }

        #region public int Count
        public int Count {
          get { 
            return _owner.Count; 
          }
        }
        #endregion

        #region public float this[int index]
        public float this[int index] {
          get {

            //int findex = index+_owner._owner.BeginIndex;
            //Bar[] buffer = _owner._owner._buffer;
            //int bLenght = buffer.Length;
            //int bSize = _owner._owner._bufferSize;
            //int bOffset = _owner._owner._bufferOffset;

            //Bar bar ;
            //if (findex >= bOffset && findex < bOffset + bSize) {
            //  bar = _owner._owner._buffer[findex - bOffset];
            //} else {
            //  if (findex >= bOffset + bLenght) {
            //    bSize = _owner._owner._bufferSize = 0;
            //  } else if (findex < bOffset) {
            //    bSize = _owner._owner._bufferSize = 0;
            //  } else if (findex > bOffset + bSize) { // внутри буфера, но с разрывом - скачек на новое значение
            //    bSize = _owner._owner._bufferSize = 0;
            //  } else if (bSize == bLenght) {
            //    bSize = _owner._owner._bufferSize = 0;
            //  }
            //  if (bSize == 0) {
            //    _owner._owner._bufferOffset = findex;
            //  }
            //  buffer[_owner._owner._bufferSize++] = bar = _bars[findex];
            //}

            Bar bar = this._owner._owner.GetBar(index);

            switch (_valueType) {
              case ValueType.Close:
                return bar.Close;
              case ValueType.High:
                return bar.High;
              case ValueType.Low:
                return bar.Low;
              case ValueType.Open:
                return bar.Open;
              case ValueType.Median:
                return (bar.High * bar.Low) / 2;
              case ValueType.Typical:
                return (bar.High * bar.Low * bar.Close) / 3;
              case ValueType.Weighted:
                return (bar.High * bar.Low * bar.Close * 2) / 4;
            }
            throw (new Exception("Unknow value type"));
          }
        }
        #endregion

        #region public float Current
        public float Current {
          get {
            return this[this.Count-1];
          }
        }
        #endregion

        #region IEnumerable<float> Members

        public IEnumerator<float> GetEnumerator() {
          return null;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
          throw new Exception("The method or operation is not implemented.");
        }

        #endregion
      }
      #endregion
    }
    #endregion

    #region public class FxClose : RateData
    public class FxClose : RateData {

      [Input("iBars")]
      public FxClose(iBars iBars)
        : base(iBars, ValueType.Close) {
      }
    }
    #endregion

    #region public class FxHigh : RateData
    public class FxHigh : RateData {

      [Input("iBars")]
      public FxHigh(iBars iBars)
        : base(iBars, ValueType.High) {
      }

    }
    #endregion

    #region public class FxLow : RateData
    public class FxLow : RateData {

      [Input("iBars")]
      public FxLow(iBars iBars)
        : base(iBars, ValueType.Low) {
      }
    }
    #endregion

    #region public class FxOpen : RateData
    public class FxOpen : RateData {

      [Input("iBars")]
      public FxOpen(iBars iBars)
        : base(iBars, ValueType.Open) {
      }
    }
    #endregion

    #region public class FxMedian : RateData
    public class FxMedian : RateData {

      [Input("iBars")]
      public FxMedian(iBars iBars)
        : base(iBars, ValueType.Median) {
      }
    }
    #endregion

    #region public class FxTypical : RateData
    public class FxTypical : RateData {
      [Input("iBars")]
      public FxTypical(iBars iBars)
        : base(iBars, ValueType.Typical) {
      }
    }
    #endregion

    #region public class FxWeighted : RateData
    public class FxWeighted : RateData {
      [Input("iBars")]
      public FxWeighted(iBars iBars)
        : base(iBars, ValueType.Weighted) {
      }
    }
    #endregion
  }
}
