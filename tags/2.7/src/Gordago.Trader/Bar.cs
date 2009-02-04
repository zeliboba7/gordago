/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago {

  /// <summary>
  /// ���
  /// </summary>
	public struct Bar{
    
    /// <summary>
    /// ���� ��������
    /// </summary>
		public float Open;
    /// <summary>
    /// ����������� ����
    /// </summary>
    public float Low;

    /// <summary>
    /// ������������ ����
    /// </summary>
    public float High;

    /// <summary>
    /// ���� ��������
    /// </summary>
    public float Close;

    /// <summary>
    /// �����. ���-�� ����� � ������ ����.
    /// </summary>
		public int Volume;

    private long _time;
    
    /// <summary>
    /// �����������
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
    /// �����������
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

    /// <summary>
    /// �����
    /// </summary>
    public DateTime Time {
      get { return new DateTime(_time); }
      set { _time = value.Ticks; }
    }
		
		#region public override string ToString() 
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
		public override string ToString() {
      return this.ToString(false);
		}
		#endregion

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
	}
}
