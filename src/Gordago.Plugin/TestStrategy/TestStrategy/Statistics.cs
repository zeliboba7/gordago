using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gordago.Analysis;
using Gordago.API;
using Gordago;

namespace TestStrategy {
  public class Statistics: Strategy {

    private double _summa;
    private int _count;
    private float _max, _min;

    private string _symbolName = "EURUSD";
    private int _decimalDigits = 4;

    public override void OnConnect() {
      _summa = 0;
      _count = 0;
      _max = _min = 0;
    }

    public override void OnOnlineRateChanged(IOnlineRate onlineRate) {
      if (onlineRate.Symbol.Name != _symbolName)
        return;

      if(_count == 0) 
        _max = _min = onlineRate.SellRate;

      _max = Math.Max(_max, onlineRate.SellRate);
      _min = Math.Min(_min, onlineRate.SellRate);

      _summa += onlineRate.SellRate;
      _count++;
    }

    public override void OnDisconnect() {
      float average = Convert.ToSingle(_summa / _count);

      StatisticsForm form = new StatisticsForm();
      form.AddRow("Symbol", _symbolName);
      form.AddRow("Tick Count", _count.ToString());
      form.AddRow("Maximum Price", this.Format(_max));
      form.AddRow("Minimum Price", this.Format(_min));
      form.AddRow("Average", this.Format(average));
      form.ShowDialog();
    }

    private string Format(float value) {
      return Gordago.SymbolManager.ConvertToCurrencyString(value, _decimalDigits);
    }

    public override void OnDestroy() {
      
    }

    public override bool OnLoad() {
      return true;
    }
  }
}
