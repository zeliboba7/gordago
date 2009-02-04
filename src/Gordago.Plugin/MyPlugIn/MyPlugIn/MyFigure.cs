using System;
using System.Collections.Generic;
using System.Text;

using Gordago;
using Gordago.Analysis;
using Gordago.Analysis.Chart;

namespace MyPlugIn {
  
  class MyFigure:ChartFigure {

    private LimitAnalyzerManager _analyzer;
    private int _countTick, _countBar;

    /// <summary>
    /// Все создаваемые фигуры здесь, будут заноситься в эту коллекцию, 
    /// для последующего их удаления
    /// </summary>
    private List<ChartFigure> _figures;

    public MyFigure(string name, LimitAnalyzerManager analyzer):base(name, true) {
      _analyzer = analyzer;
      _figures = new List<ChartFigure>();
    }

    protected override void OnPaint(System.Drawing.Graphics g) {
      if (_countTick != this.ChartBox.ChartManager.Symbol.Ticks.Count || _countBar != this.ChartBox.ChartManager.Bars.Count) {
        _countTick = this.ChartBox.ChartManager.Symbol.Ticks.Count;
        _countBar = this.ChartBox.ChartManager.Bars.Count;
        Compute();
      }

      /* Простая сортировка фигур на графике: фигура баров поверх остальных фигур */
      if (this.ChartBox == this.ChartBox.ChartManager.ChartBoxes[0]) {
        /* Если верхняя (последняя фигура не бары), то переместить фигуру баров */
        if (!(ChartBox.Figures[ChartBox.Figures.Count - 1] is ChartFigureBar)) {
          ChartFigureBar fBar = null;
          for (int i = 0; i < ChartBox.Figures.Count; i++) {
            if (ChartBox.Figures[i] is ChartFigureBar) {
              fBar = ChartBox.Figures[i] as ChartFigureBar;
              break;
            }
          }
          if (fBar != null) {
            ChartBox.Figures.ChangeIndex(fBar, ChartBox.Figures.Count - 1);
          }
        }
      }

      //LimitAnalyzer lanalyzer = _analyzer.GetAnalyzer(this.ChartBox.ChartManager.Symbol);
      //for (int i = 0; i < TimeFrameManager.TimeFrames.Count; i++) {
      //  TimeFrame tf = TimeFrameManager.TimeFrames[i];
      //  /* SetLimit(int second, int limit) 
      //   * second - таймфрейм 
      //   * limit - ограничение в кол-во баров, если -1, то нет ограничений */
      //  lanalyzer.SetLimit(tf.Second, -1);
      //}

    }

    /// <summary>
    /// Удаление фигуры с графика
    /// </summary>
    protected override void OnDestroy() {
      foreach(ChartFigure figure in _figures){
        this.ChartBox.Figures.Remove(figure);
      }
    }

    /// <summary>
    /// Рассчет опорных точек веера Фибоначчи.
    /// Наименование функций индикаторов и их параметры можно просмотреть в 
    /// файле indicators\info.xml который находиться  в папке программы 
    /// (по умолчанию C:\Program Files\Gordago\Forex Optimizer TT\indicators\info.xml)
    /// </summary>
    /// <param name="tf">ТаймФрейм</param>
    /// <returns></returns>
    private COPoint[] FiboCompute(TimeFrame tf) {


      Vector low = _analyzer.Compute(this.ChartBox.ChartManager.Symbol, "Low", tf.Second);
      Vector high = _analyzer.Compute(this.ChartBox.ChartManager.Symbol, "High", tf.Second);

      Vector lowestIndex = _analyzer.Compute(this.ChartBox.ChartManager.Symbol, "LowestIndex", 30, low);
      Vector highestIndex = _analyzer.Compute(this.ChartBox.ChartManager.Symbol, "HighestIndex", 30, high);

      int lIndex = (int)lowestIndex.Current;
      int hIndex = (int)highestIndex.Current;

      float price1, price2;
      int barIndex1, barIndex2;

      if(lIndex < hIndex) {
        barIndex1 = lIndex;
        barIndex2 = hIndex;
        price1 = low[lIndex];
        price2 = high[hIndex];
      } else {
        barIndex1 = hIndex;
        barIndex2 = lIndex;
        price1 = high[hIndex];
        price2 = low[lIndex];
      }

      IBarList bars = this.ChartBox.ChartManager.Symbol.Ticks.GetBarList(tf.Second);

      DateTime time1 = bars[_analyzer.TransformToRealIndex(this.ChartBox.ChartManager.Symbol, tf, barIndex1)].Time;
      DateTime time2 = bars[_analyzer.TransformToRealIndex(this.ChartBox.ChartManager.Symbol, tf, barIndex2)].Time;

      return new COPoint[] { new COPoint(time1, price1), new COPoint(time2, price2) };
    }

    #region private void Compute()
    private void Compute() {

      COPoint[] cpos = this.FiboCompute(this.ChartBox.ChartManager.Bars.TimeFrame);
      string fiboName = "MyFiboFun";

      bool useSampleFigure = true;

      // Пример перенос Баров в окно ниже
      //if (this.ChartBox.ChartManager.ChartBoxes.Length >= 2) {
      //  IChartBox cbox0 = this.ChartBox.ChartManager.ChartBoxes[0];
      //  IChartBox cbox1 = this.ChartBox.ChartManager.ChartBoxes[1];

      //  for (int i = 0; i < cbox0.Figures.Count; i++) {
      //    if (cbox0.Figures[i] is ChartFigureBar) {
      //      cbox0.Figures.Remove(cbox0.Figures[i] as ChartFigureBar);
      //      cbox1.Figures.Add(new ChartFigureBar("MyBars"));
      //      break;
      //    }
      //  }
      //}

      if (!useSampleFigure) {

        MyChartObjectFiboFan myFibo = this.ChartBox.Figures.GetFigure(fiboName) as MyChartObjectFiboFan;
        if (myFibo == null) {
          myFibo = new MyChartObjectFiboFan(fiboName);
          myFibo.COPoints.Create(cpos[0].Time, cpos[0].Price);
          myFibo.COPoints.Create(cpos[1].Time, cpos[1].Price);
          this.ChartBox.Figures.Add(myFibo);
          _figures.Add(myFibo);
        } else {
          myFibo.COPoints[0].SetValue(cpos[0].Time, cpos[0].Price);
          myFibo.COPoints[1].SetValue(cpos[1].Time, cpos[1].Price);
        }
      } else {
        MyChartFigureLine myLine = this.ChartBox.Figures.GetFigure(fiboName) as MyChartFigureLine;
        if (myLine == null) {
          myLine = new MyChartFigureLine(fiboName, cpos[0].Time, cpos[0].Price, cpos[1].Time, cpos[1].Price);
          this.ChartBox.Figures.Add(myLine);
          _figures.Add(myLine);
        } else {
          myLine.SetValue(cpos[0].Time, cpos[0].Price, cpos[1].Time, cpos[1].Price);
        }
      }
    }
    #endregion
  }
}
