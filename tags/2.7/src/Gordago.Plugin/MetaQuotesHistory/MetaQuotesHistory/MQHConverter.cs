using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Gordago.PlugIn.MetaQuotesHistory {

  #region class MQHConverterEventArgs:EventArgs
  class MQHConverterEventArgs:EventArgs {
    private int _current;
    private int _total;
    public MQHConverterEventArgs(int current, int total) {
      _current = current;
      _total = total;
    }

    public int Current {
      get { return this._current; }
    }

    public int Total {
      get { return _total; }
    }
  }
  #endregion

  class MQHConverter {

    public event EventHandler<MQHConverterEventArgs> ProgressChanged;
    
    private MQHBarReader _barReader;
    private string _dir;
    
    public MQHConverter(MQHBarReader barReader, string dir) {
      _barReader = barReader;
      _dir = dir;
    }

    public void Start() {
      Thread th = new Thread(new ThreadStart(this.StartMethod));
      th.Priority = ThreadPriority.Lowest;
      th.Start();
    }

    private void StartMethod(){
      string symbolName = "MQ" + _barReader.SymbolName;
      string fileName = _dir + "\\" + symbolName + ".gtk";

      TickFileInfo tickFileInfo = new TickFileInfo(symbolName, _barReader.DecimalDigits, fileName);
      TickWriter tickWriter = new TickWriter(tickFileInfo);
      int tickIndex = 0;
      long deltaTime = _barReader.TimeFrameSecond * 10000000L;
      float point = GetPoint(_barReader.DecimalDigits);
      Random rnd = new Random();

      for(int i = 0; i < _barReader.Count; i++) {
        Bar bar = _barReader[i];
        
        long beginTime = bar.Time.Ticks;
        long endTime = beginTime + deltaTime-1;
        int tickCount = bar.Volume - 2;
        long stepTime = deltaTime / bar.Volume;

        float widthBar = bar.High - bar.Low;
        int widthBarPoint = Convert.ToInt32(widthBar / point);

        tickWriter.Write(new Tick(beginTime, bar.Open), tickIndex++);

        for(int index = 1; index < bar.Volume - 2; index++) {

          float price = bar.Low + rnd.Next(widthBarPoint) * point;
          tickWriter.Write(new Tick(beginTime+stepTime * index, price), tickIndex++);
        }
        tickWriter.Write(new Tick(endTime, bar.Close), tickIndex++);
        this.OnProgressChanged(i, _barReader.Count);
      }
      tickWriter.Close();
      this.OnProgressChanged(_barReader.Count, _barReader.Count);
    }

    protected virtual void OnProgressChanged(int current, int total) {
      if(this.ProgressChanged != null) {
        this.ProgressChanged(this, new MQHConverterEventArgs(current, total));
      }
    }

    #region public static float GetPoint(int decimalDigits)
    public static float GetPoint(int decimalDigits) {
      float val = 1;
      for(int i = 0; i < decimalDigits; i++)
        val = val / 10;
      return val;
    }
    #endregion
  }
}
