/**
* @version $Id: TimeFrame.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{

  using System;
  using System.Collections;

	#region public class TimeFrame 
	public class TimeFrame {
		private string _name;
		private int _second;
    private TimeFrameCalculator _calculator;
		
		public TimeFrame(string name, int second) {
			_name = name;
			_second = second;
		}

		#region public string Name
		public string Name{
			get{return this._name;}
      set { this._name = value; }
		}
		#endregion

		#region public int Second
		public int Second{
			get{return this._second;}
		}
		#endregion

    #region public TimeFrameCalculator Calculator
    public TimeFrameCalculator Calculator {
      get { return this._calculator; }
      set { this._calculator = value; }
    }
    #endregion

    #region public override string ToString()
    public override string ToString() {
			return _name;
		}
		#endregion
	}
	#endregion

  public class TimeFrameManager: IEnumerable	{

    #region public static TimeFrameManager TimeFrames

    #region private static TimeFrameManager _timeframes
    private static TimeFrameManager _timeframesStatic =
      new TimeFrameManager(new TimeFrame[]{
																						new TimeFrame("M1", 60),
																						new TimeFrame("M5", 300),
																						new TimeFrame("M10", 600),
																						new TimeFrame("M15", 900),
																						new TimeFrame("M30", 1800),
																						new TimeFrame("H1", 3600),
																						new TimeFrame("H4", 14400),
																						new TimeFrame("D1", 86400)
																					});
    #endregion

    public static TimeFrameManager TimeFrames {
      get { return _timeframesStatic; }
    }
    #endregion

		private TimeFrame[] _timeframes;

		#region public TimeFrameManager(TimeFrame[] timeframes)
		public TimeFrameManager(TimeFrame[] timeframes){
			_timeframes = new TimeFrame[]{};
			foreach (TimeFrame tf in timeframes){
				this.AddTimeFrame(tf);
			}
		}
		#endregion

		public TimeFrameManager():this(new TimeFrame[0]){}

		#region public TimeFrame this[int index]
		public TimeFrame this[int index]{
			get{return this._timeframes[index];}
		}
		#endregion

		#region public int Count
		public int Count{
			get{return this._timeframes.Length;}
		}
		#endregion

		#region public TimeFrame Find(int second)
		public TimeFrame Find(int second){
			foreach (TimeFrame tf in this._timeframes){
				if (tf.Second == second)
					return tf;
			}
			return null;
		}
		#endregion

		#region public TimeFrame GetTimeFrame(int second)
		public TimeFrame GetTimeFrame(int second){
			foreach (TimeFrame tf in this._timeframes){
				if (tf.Second == second)
					return tf;
			}

//			TimeFrame tfnew = this.CreateNew(second);
//			this.AddTimeFrame(tfnew);
//			return tfnew;
			//throw(new Exception("TimeFrame " + second.ToString() + " not found!"));
      return null;
		}
		#endregion

		#region public TimeFrame CreateNew(int second)
		public TimeFrame CreateNew(int second){
			string prefix = "";
			if (second < 60){
				prefix = "S";
				prefix += second.ToString();
			}else if (second < 3600){
				prefix = "M";
				prefix += Convert.ToString(second / 60);
			}else if (second < 86400){
				prefix = "H";
				prefix += Convert.ToString(second / 3600);
			}else {
				prefix = "D";
				prefix += Convert.ToString(second / 86400);
			}
			TimeFrame tf = new TimeFrame(prefix, second);
			return tf;
		}
		#endregion

		#region public void AddTimeFrame(TimeFrame timeframe)
		public void AddTimeFrame(TimeFrame timeframe){
			int[] scds = new int[_timeframes.Length+1];
			for (int i=0;i<_timeframes.Length;i++){
				TimeFrame tf = _timeframes[i];
				scds[i] = tf.Second;
				if (tf.Second == timeframe.Second){
					return;
				}
			}
			scds[_timeframes.Length] = timeframe.Second;
			Array.Sort(scds);

			ArrayList al = new ArrayList();

			foreach (int scd in scds){
				TimeFrame addtf = null;
				foreach (TimeFrame ftf in _timeframes){
					if (ftf.Second == scd){
						addtf = ftf;
						break;
					}
				}
				if (addtf == null)
					addtf = timeframe;
				al.Add(addtf);
			}
      switch(timeframe.Second) {
        case TFMonth.MONTH_SECOND: /* Month */
          timeframe.Calculator = new TFMonth();
          timeframe.Name = "MN";
          break;
        case TFWeek.WEEK_SECOND: /* Week */
          // timeframe.Calculator = new TFWeek
          timeframe.Name = "W1";
          break;
      }
			_timeframes = (TimeFrame[])al.ToArray(typeof(TimeFrame));
		}
		#endregion

		#region public void RemoveAllTimeFrame()
		public void RemoveAllTimeFrame(){
			this._timeframes = new TimeFrame[0];
		}
		#endregion

		#region public IEnumerator GetEnumerator()
		public IEnumerator GetEnumerator(){
			return new TFEnumerator(this);
		}
		#endregion

		#region private class TFEnumerator: IEnumerator 
		private class TFEnumerator: IEnumerator {
			TimeFrameManager _tfm;
			private int _index;
			public TFEnumerator(TimeFrameManager tfm){
				_tfm = tfm;
				this.Reset();
			}

			public void Reset() {
				_index  = -1;
			}

			public object Current {
				get {
					return _tfm._timeframes[_index];
				}
			}

			public bool MoveNext() {
				_index++;
				return _index < _tfm._timeframes.Length;
			}
		}
		#endregion
  }

  #region public abstract class TimeFrameCalculator
  public abstract class TimeFrameCalculator {
    public abstract bool CheckNewBar(Bar lastBar, DateTime time);
    public abstract DateTime GetRoundTime(DateTime time);
  }
  #endregion

  #region class TFMonth:TimeFrameCalculator
  class TFMonth:TimeFrameCalculator {
    
    public const int MONTH_SECOND = 2592000;

    public override bool CheckNewBar(Bar lastBar, DateTime time) {
      int lastMonth = lastBar.Time.Month;
      int lastYear = lastBar.Time.Year;

      int newMonth = time.Month;
      int newYear = time.Year;

      if(lastMonth == newMonth && lastYear == newYear)
        return false;

      return true;
    }

    public override DateTime GetRoundTime(DateTime time) {
      return new DateTime(time.Year, time.Month, 1);
    }
  }
  #endregion

  #region class TFWeek:TimeFrameCalculator
  class TFWeek:TimeFrameCalculator {
    public const int WEEK_SECOND = 604800;

    public override bool CheckNewBar(Bar lastBar, DateTime time) {
      throw new Exception("The method or operation is not implemented.");
      
    }
    public override DateTime GetRoundTime(DateTime time) {
      throw new Exception("The method or operation is not implemented.");
    }
  }
  #endregion
}
