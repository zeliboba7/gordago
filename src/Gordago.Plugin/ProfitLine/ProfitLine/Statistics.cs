using System;
using System.Collections.Generic;
using System.Text;

namespace Gordago.Analysis {
  public class Statistics:Strategy {


    private int _count;
    private float _prevRate;
    private int _countUp, _countDown;
    private int _maxUp, _maxDown;

    public override bool OnLoad() {
      _count = 0;
      return true;
    }

    public override void OnExecute() {
      float rate = this.Symbol.Bid;

      if(_count == 0) {
        _prevRate = rate;
        _countDown = _countUp = 0;
      }

      if(rate > _prevRate) {
        _countUp++;
      } else {
        _countUp = 0;
      }

      if(rate < _prevRate) {
        _countDown++;
      } else {
        _countDown = 0;
      }

      _maxUp = Math.Max(_maxUp, _countUp);
      _maxDown = Math.Max(_maxDown, _countDown);

      _count++;
      _prevRate = rate;

      Vector dayOpen = Function("Open", 86400);
      Vector dayClose = Function("Close", 86400);
      
    }

    #region public override void OnDestroy()
    public override void OnDestroy() {
      ReportForm form = new ReportForm();
      form.AddRow("MaxCountUp", _maxUp);
      form.AddRow("MaxCountDown", _maxDown);
      form.ShowDialog();
    }
    #endregion
  }
}
