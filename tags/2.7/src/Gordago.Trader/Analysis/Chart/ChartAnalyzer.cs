/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.Analysis.Chart {
	public class ChartAnalyzer: Analyzer {

		private ChartBars _bars;
		private int _position = -1;
    private int _savedBarsCount = 0;
    private ITickList _savedTicks = null;

		public ChartAnalyzer(IndicatorManager imanager, ISymbol symbol):base(imanager, symbol) {
      this.IsFixedParams = false;
    }

    #region public IBars CurrentBars
    public IBarList CurrentBars{
			get{return this._bars;}
    }
    #endregion

    #region public int Offset
    public int Offset{
			get{return this._bars.Offset;}
		}
		#endregion

    #region internal bool SetPosition(int position, TimeFrame tf)
    internal bool SetPosition(int position, TimeFrame tf){
      if (_bars == null || _bars.TimeFrame.Second != tf.Second || _savedTicks != this.Symbol.Ticks) {
        _bars = new ChartBars(this.Symbol.Ticks.GetBarList(tf.Second));
        _savedTicks = this.Symbol.Ticks;
      }

			_position = position;
			return _bars.SetPosition(position);
		}
		#endregion

		#region public override IBars GetBars(int second) 
		public override IBarList GetBars(int second) {
			if (second != _bars.TimeFrame.Second){
				throw(new Exception("ChartAnalyzer.GetBars() error: unknow TimeFrame"));
			}
			return _bars;
		}
		#endregion

    #region public override IBarList[] BarLists
    public override IBarList[] BarLists {
      get { return new IBarList[] { _bars }; }
    }
    #endregion

    #region public new IVector Compute(Function function, params object[] parameters)
    public new IVector Compute(Function function, params object[] parameters) {
      if (Math.Abs(_bars.Count - _savedBarsCount)>1) 
        this.Cache.Clear();

      _savedBarsCount = _bars.Count;
      return base.Compute(function, parameters);
    }
    #endregion
  }

  #region public class ChartBars:IBarList
  public class ChartBars:IBarList {

    public const int MAX_SIZE_BUFFER = 4096;
    public const int MIN_OFFSET_SIZE = 1024;
    public const int MAX_OFFSET_SIZE = 2048;

    private IBarList _bars;

    private int _offset;

    internal ChartBars(IBarList bars) {
      _bars = bars;
    }

    #region public int Count
    public int Count {
      get {
        return Math.Min(MAX_SIZE_BUFFER, this._bars.Count - _offset);
      }
    }
    #endregion

    #region public Bar this[int index]
    public Bar this[int index] {
      get {
        return _bars[_offset + index];
      }
    }
    #endregion

    #region public Bar Current
    public Bar Current {
      get {
        return _bars[_offset + this.Count - 1];
      }
    }
    #endregion

    #region public int Offset
    public int Offset {
      get { return this._offset; }
    }
    #endregion

    #region public bool SetPosition(int position)
    public bool SetPosition(int position) {

      int p1 = Math.Max(position - MAX_OFFSET_SIZE, 0);
      int p2 = Math.Max(position - MIN_OFFSET_SIZE, 0);

      if(this._offset >= p1 && this._offset <= p2)
        return false;

      _offset = (p2 - p1) / 2 + p1;

      return true;
    }
    #endregion

    #region public TimeFrame TimeFrame
    public TimeFrame TimeFrame {
      get { return _bars.TimeFrame; }
    }
    #endregion

    #region public DateTime TimeFrom
    public DateTime TimeFrom {
      get { return _bars[_offset].Time; }
    }
    #endregion

    #region public DateTime TimeTo
    public DateTime TimeTo {
      get { return this.Current.Time; }
    }
    #endregion

    #region public int GetBarIndex(DateTime time)
    public int GetBarIndex(DateTime time) {
      return _bars.GetBarIndex(time);
    }
    #endregion

#if DEBUG
    public int RealIndex(int index) {
      return _offset + index;
    }
#endif
  }
  #endregion
}
