/**
* @version $Id: iStandardDeviation.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Indicators
{
  using System;
  using System.ComponentModel;

  public class iStandardDeviation:Indicator {

    private FxStdDev _stdDev;
    private IFunction _inData;
    private int _period;

    [Input("InData", "Period")]
    public iStandardDeviation(IFunction inData, int period) {
      _stdDev = new FxStdDev(inData, period);
      _inData = inData;
      _period = period;
    }

    #region public IFunction InData
    [ParameterFrom(typeof(Function), "InData", 0)]
    public IFunction InData {
      get { return _inData; }
    }
    #endregion

    #region public int Period
    [Parameter("Period"), DefaultValue(13)]
    public int Period {
      get { return this._period; }
    }
    #endregion

    #region public FxStdDev StdDev
    [Function("StdDev")]
    public FxStdDev StdDev {
      get { return _stdDev; }
    }
    #endregion

    #region public class FxStdDev:Function
    public class FxStdDev:Function {

      private int _period;

      public FxStdDev(IFunction inData, int period):base(inData) {
        _period = period;
      }

      #region public int Period
      public int Period {
        get { return this._period; }
      }
      #endregion

      protected override void OnCompute() {
        float sma = 0, sd = 0;
        int count1 = this.Count + 1;
        int count2 = this.InData.Count +1;

        for (int i = count1; i < count2; i++) {
          if (i < _period) {
            this.Add();
          } else {
            sma = 0;
            for (int j = i - _period; j < i; j++)
              sma += this.InData.Items[j];

            sma /= _period;
            sd = 0;

            float item;
            for (int j = i - _period; j < i; j++) {
              item = this.InData.Items[j];
              sd += (sma - item) * (sma - item);
            }

            this.Add((float)Math.Sqrt(sd / _period));
          }
        }
      }
    }
    #endregion
  }
}
