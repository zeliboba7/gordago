using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis.Toolbox {
  public class iZZEnsign:Indicator {
    public override void Initialize() {
      this.Copyright = "Copyright © 2007, Gordago Software Ltd.";
      this.Link = "http://www.gordago.com/";
      this.GroupName = "Signal indicators";
      this.Name = "ZigZag (Ensign)";
      this.ShortName = "ZZEnsign";
      this.SetImage("i.gif");
      this.IsSeparateWindow = false;
      RegFunction("ZZEnsign");
    }
  }

  #region enum ZZTrend
  enum ZZTrend {
    Unknow,
    Up,
    Down
  }
  #endregion

  #region class ZZEnsignVector:IVector
  class ZZEnsignVector:Vector {
    public bool Initialize = false;
    public float LastLow, LastHigh;
    public int ai, bi, countBar;
    public float SavedLH;
    public bool FlagUp, FlagDown, fcount0;

    public ZZTrend Trend;

    public int SavedIndex = -1, SavedIndexUpTrend = -1, SavedIndexDownTrend = -1;

    public float di;
  }
  #endregion

  public class ZZEnsign:Function {

    protected override void Initialize() {
      this.RegParameter(new ParameterInteger("Bars", 12, 1, 10000));
      this.RegParameter(new ParameterInteger("Size", 5, 1, 10000));

      ParameterVector pvlow = new ParameterVector("Low", "Low");
      pvlow.Visible = false;
      RegParameter(pvlow);

      ParameterVector pvhigh = new ParameterVector("High", "High");
      pvhigh.Visible = false;
      RegParameter(pvhigh);

      RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      ParameterVector pvtime = new ParameterVector("vectorTime", "Time");
      pvtime.Visible = false;
      RegParameter(pvtime);
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {

      int minBars = (int)parameters[0];
      int minSize = (int)parameters[1];
      IVector low = (IVector)parameters[2];
      IVector high = (IVector)parameters[3];
      IVector close = (IVector)parameters[4];
      BarsVector barsVector = (BarsVector)parameters[5];
      IBarList bars = barsVector.Bars;

      if (result == null) {
        result = new ZZEnsignVector();

        ZZEnsignVector zzt = result as ZZEnsignVector;

        zzt.di = minSize * analyzer.Point;
        zzt.countBar = minBars;
      }

      ZZEnsignVector zz = result as ZZEnsignVector;

      if (result.Count == low.Count) 
        zz.RemoveLastValue();

      int indexZeroBar = low.Count - 1;

      for (int i = zz.Count; i < low.Count; i++) {

        zz.Add(float.NaN);

        if (i > 5) {

          // Устанавливаем начальные значения минимума и максимума бара
          if (i == 0) { zz.LastLow = low[i]; zz.LastHigh = high[i]; }

          if (zz.Trend == ZZTrend.Unknow) {
            if (zz.LastLow < low[i] && zz.LastHigh < high[i]) {// тренд восходящий
              zz.Trend = ZZTrend.Up;
              zz.LastHigh = zz.SavedLH = high[i];
              zz.SavedIndexUpTrend = zz.ai = i;
            }
            if (zz.LastLow > low[i] && zz.LastHigh > high[i]) {// тренд нисходящий
              zz.Trend = ZZTrend.Down;
              zz.LastLow = zz.SavedLH = low[i];
              zz.SavedIndexDownTrend = zz.bi = i;
            }
          }

          if (zz.SavedIndex != i) {
            zz.SavedIndex = i;

            int ai0 = zz.SavedIndexUpTrend;
            int bi0 = zz.SavedIndexDownTrend;

            zz.fcount0 = false;
            if ((zz.FlagUp || zz.FlagDown) && zz.countBar > 0) {
              zz.countBar--;
              if (i == indexZeroBar && zz.countBar == 0)
                zz.fcount0 = true;
            }

            // Остановка. Определение дальнейшего направления тренда.
            if (zz.Trend == ZZTrend.Up) {
              if (zz.LastHigh > high[i] && !zz.FlagUp)
                zz.FlagUp = true;

              if (i == indexZeroBar) {
                if (close[i - 1] < zz.LastLow && zz.FlagUp) {
                  zz.Trend = ZZTrend.Down;
                  zz.countBar = minBars;
                  zz.FlagUp = false;
                }
                if (zz.countBar == 0 && zz.SavedLH - zz.di > low[i - 1] && high[i - 1] < zz.LastHigh && ai0 > i - 1 && zz.FlagUp) {
                  zz.Trend = ZZTrend.Down;
                  zz.countBar = minBars;
                  zz.FlagUp = false;
                }
                if (zz.Trend == ZZTrend.Down) { // Тредн сменился с восходящего на нисходящий на предыдущем баре
                  zz[ai0] = high[ai0];
                  zz.LastLow = zz.SavedLH = low[i - 1];
                  zz.SavedIndexDownTrend = zz.bi = i - 1;
                }
              } else {
                if (close[i] < zz.LastLow && zz.FlagUp) {
                  zz.Trend = ZZTrend.Down;
                  zz.countBar = minBars;
                  zz.FlagUp = false;
                }
                if (zz.countBar == 0 && zz.SavedLH - zz.di > low[i] && high[i] < zz.LastHigh && zz.FlagUp) {
                  zz.Trend = ZZTrend.Down;
                  zz.countBar = minBars;
                  zz.FlagUp = false;
                }
                if (zz.Trend == ZZTrend.Down) {// Тредн сменился с восходящего на нисходящий
                  zz[zz.ai] = high[zz.ai];
                  zz.LastLow = zz.SavedLH = low[i];
                  zz.SavedIndexDownTrend = zz.bi = i;
                }
              }
            } else {// fs==2
              if (zz.LastLow < low[i] && !zz.FlagDown)
                zz.FlagDown = true;

              if (i == indexZeroBar) {
                if (close[i - 1] > zz.LastHigh && zz.FlagDown) {
                  zz.Trend = ZZTrend.Up;
                  zz.countBar = minBars;
                  zz.FlagDown = false;
                }
                if (zz.countBar == 0 && zz.SavedLH + zz.di < high[i - 1] &&
                  low[i - 1] > zz.LastLow && bi0 > i - 1 && zz.FlagDown) {
                  zz.Trend = ZZTrend.Up;
                  zz.countBar = minBars;
                  zz.FlagDown = false;
                }
                if (zz.Trend == ZZTrend.Up) {// Тредн сменился с нисходящего на восходящий на предыдущем баре
                  zz[bi0] = low[bi0];
                  zz.LastHigh = zz.SavedLH = high[i - 1];
                  zz.SavedIndexUpTrend = zz.ai = i - 1;
                }
              } else {
                if (close[i] > zz.LastHigh && zz.FlagDown) {
                  zz.Trend = ZZTrend.Up;
                  zz.countBar = minBars;
                  zz.FlagDown = false;
                }
                if (zz.countBar == 0 && zz.SavedLH + zz.di < high[i] && low[i] > zz.LastLow && zz.FlagDown) {
                  zz.Trend = ZZTrend.Up;
                  zz.countBar = minBars;
                  zz.FlagDown = false;
                }
                if (zz.Trend == ZZTrend.Up) {// Тредн сменился с нисходящего на восходящий
                  zz[zz.bi] = low[zz.bi];
                  zz.LastHigh = zz.SavedLH = high[i];
                  zz.SavedIndexUpTrend = zz.ai = i;
                }
              }
            }
          }
          // Продолжение тренда
          if (zz.Trend == ZZTrend.Up && high[i] > zz.SavedLH) {
            zz.SavedIndexUpTrend = zz.ai = i;
            zz.LastHigh = zz.SavedLH = high[i];
            zz.countBar = minBars;
            zz.FlagUp = false;
          }
          if (zz.Trend == ZZTrend.Down && low[i] < zz.SavedLH) {
            zz.bi = i;
            zz.SavedIndexDownTrend = i;
            zz.LastLow = low[i];
            zz.SavedLH = low[i];
            zz.countBar = minBars;
            zz.FlagDown = false;
          }

          if (i == indexZeroBar) { // Нулевой бар. Расчет первого луча ZigZag-a

            int ai0 = zz.SavedIndexUpTrend;
            int bi0 = zz.SavedIndexDownTrend;

            if (zz.Trend == ZZTrend.Up) {
              for (int n = bi0 + 1; n < low.Count - 1; n++) 
                zz[n] = float.NaN;
              zz[ai0] = high[ai0];
            }

            if (zz.Trend == ZZTrend.Down) {
              for (int n = ai0 + 1; n < low.Count - 1; n++) 
                zz[n] = float.NaN;
              zz[bi0] = low[bi0];
            }
          }
        }
      }
      return zz;
    }
  }
}


