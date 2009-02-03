/**
* @version $Id: Bar.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;

  /// <summary>
  /// Бар
  /// </summary>
	public struct Bar{
    
    /// <summary>
    /// Цена открытия
    /// </summary>
		public float Open;
    /// <summary>
    /// Минимальная цена
    /// </summary>
    public float Low;

    /// <summary>
    /// Максимальная цена
    /// </summary>
    public float High;

    /// <summary>
    /// Цена закрытия
    /// </summary>
    public float Close;

    /// <summary>
    /// Объем. Кол-во тиков в данном баре.
    /// </summary>
		public int Volume;

    private long _time;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="open"></param>
    /// <param name="low"></param>
    /// <param name="high"></param>
    /// <param name="close"></param>
    /// <param name="volume"></param>
    /// <param name="time"></param>
    public Bar(float open, float low, float high, float close, int volume, DateTime time) : this (open, low, high, close, volume, time.Ticks) {
    }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="open"></param>
    /// <param name="low"></param>
    /// <param name="high"></param>
    /// <param name="close"></param>
    /// <param name="volume"></param>
    /// <param name="time"></param>
		public Bar(float open, float low, float high, float close, int volume, long time){
			this.Open = open;
			this.Low = low;
			this.High = high;
			this.Close = close;
			this.Volume = volume;
			_time = time;
    }

    #region public DateTime Time
    /// <summary>
    /// Время
    /// </summary>
    public DateTime Time {
      get { return new DateTime(_time); }
      set { _time = value.Ticks; }
    }
    #endregion

    #region public override string ToString()
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
		public override string ToString() {
      return this.ToString(false);
		}
		#endregion

    #region public string ToString(bool isOneLine)
    public string ToString(bool isOneLine) {
      string DTime = Time.ToShortDateString() + " " +
          Time.ToShortTimeString();
      string ret = "";
      if (!isOneLine) {
        ret = "Time: " + DTime +
          "\nHigh: " + Convert.ToString(High) +
          "\nOpen: " + Convert.ToString(Open) +
          "\nClose: " + Convert.ToString(Close) +
          "\nLow: " + Convert.ToString(Low) +
          "\nVol: " + Convert.ToString(this.Volume);
      } else {
        ret = "T: " + DTime +
          " H:" + Convert.ToString(High) +
          " O:" + Convert.ToString(Open) +
          " C:" + Convert.ToString(Close) +
          " L:" + Convert.ToString(Low) +
          " V:" + Convert.ToString(this.Volume);

      }
      return ret;
    }
    #endregion
  }
}
