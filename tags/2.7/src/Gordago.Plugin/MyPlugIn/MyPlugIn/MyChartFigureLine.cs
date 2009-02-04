using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis.Chart;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gordago;

namespace MyPlugIn {

  #region class FiboLine  - Линия веера Фибоначчи
  /// <summary>
  /// Линия веера Фибоначчи
  /// </summary>
  class FiboLine {
    
    private string _toolText;
    private float _level;
    private Pen _pen;

    /* Графический путь линии */
    private GraphicsPath _gpath;

    public FiboLine(float level) {
      _level = level;
      _gpath = new GraphicsPath();
      _toolText = string.Format("MyFiboLine: {0}%", SymbolManager.ConvertToCurrencyString(level, 2));
      _pen = new Pen(Color.Red);
    }

    #region public string ToolText - Текст всплывающей подсказки
    /// <summary>
    /// Текст всплывающей подсказки
    /// </summary>
    public string ToolText {
      get { return this._toolText; }
      set { this._toolText = value; }
    }
    #endregion

    #region public float Level - Уровень линии в веере( в процентах)
    /// <summary>
    /// Уровень линии в веере
    /// </summary>
    public float Level {
      get { return this._level; }
    }
    #endregion

    #region public Pen Pen - Карандаш для рисования линии
    /// <summary>
    /// Карандаш для рисования линии
    /// </summary>
    public Pen Pen {
      get { return this._pen; }
      set { this._pen = value; }
    }
    #endregion

    #region public GraphicsPath GraphicsPath
    public GraphicsPath GraphicsPath {
      get { return this._gpath; }
    }
    #endregion

    #region public void Update(int x1, int y1, int x2, int y2)
    /// <summary>
    /// Обновление информации о физических координатах линии.
    /// Эта информация нужна для того, чтобы можно было выделить одну
    /// линию из коллекции
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    public void Update(int x1, int y1, int x2, int y2) {
      _gpath.Reset();
      _gpath.AddLine(x1, y1, x2, y2);
    }
    #endregion
  }
  #endregion

  class MyChartFigureLine:ChartFigure {

    private DateTime _time1, _time2;
    private float _price1, _price2;

    private Pen _pen;

    /// <summary>
    /// Коллекция линий веера
    /// </summary>
    private List<FiboLine> _fiboLines;

    private int _savedIndexFiboLine = -1;

    private GraphicsPath _gpath;
    
    public MyChartFigureLine(string name, DateTime time1, float price1, DateTime time2, float price2):base(name, true) {
      this.SetValue(time1, price1, time2, price2);
      _pen = new Pen(Color.Red);

      /* Создаем пустую коллекцию линий веера */
      _fiboLines = new List<FiboLine>();
      this.AddRange(new float[] {0, 38.2F, 61.8F, 76.4F, 85.4F});
    }

    #region protected override GraphicsPath GraphicsPath 
    /// <summary>
    /// Путь фигуры. Необходим для того, чтоб иметь возможность 
    /// при проверке заменять на свой путь каждой линии
    /// </summary>
    protected override GraphicsPath GraphicsPath {
      get {
        return _gpath;
      }
    }
    #endregion

    #region public int Count - Кол-во линий в этом веере
    /// <summary>
    /// Кол-во линий в этом веере
    /// </summary>
    public int Count {
      get { return this._fiboLines.Count; }
    }
    #endregion

    #region public FiboLine this[int index] - Получить линию из коллекции по индексу
    /// <summary>
    /// Получить линию из коллекции по индексу
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public FiboLine this[int index] {
      get { return this._fiboLines[index]; }
    }
    #endregion

    #region public FiboLine this[float level] - Получить линию из коллекции по идентификатору
    /// <summary>
    /// Получить линию из коллекции по идентификатору (в данном случае
    /// идентификатор является уровень)
    /// </summary>
    /// <param name="levelPercent"></param>
    /// <returns></returns>
    public FiboLine this[float level] {
      get {
        for (int i = 0; i < _fiboLines.Count; i++) {
          FiboLine fiboLine = _fiboLines[i];
          if (fiboLine.Level == level)
            return fiboLine;
        }
        return null;
      }
    }
    #endregion

    #region public FiboLine Add(float levelPercent)
    /// <summary>
    /// Создать новую линию в веере из процента уровня и вернуть обьект линия. 
    /// Так же производится проверка: Если линия уже имеется, то она не 
    /// добавляется, дабы исключить одинаковых линий (В данном случае это не 
    /// более чем демонстрация возможностей)
    /// </summary>
    /// <param name="levelPercent"></param>
    /// <returns></returns>
    public FiboLine Add(float levelPercent) {
      
      FiboLine fiboLine = this[levelPercent];
      
      if (fiboLine == null) {
        /* Линии в коллекции нет, создаем новую и добавляем в коллекцию */
        fiboLine = new FiboLine(levelPercent);
        _fiboLines.Add(fiboLine);
      }
      return fiboLine;
    }
    #endregion

    #region public void AddRange(float[] levels)
    /// <summary>
    /// Создание и добавление в коллекцию линий из массива уровней
    /// </summary>
    /// <param name="levelPercents"></param>
    public void AddRange(float[] levels) {
      /* В подобных случаеях так же можно использовать foreach:
       * foreach (float level in levelPercents) {
       *   this.Add(level);
       * }
       * Но там, где перебор по коллекции влияет на скорость, лучше
       * использовать for
       */
      for (int i = 0; i < levels.Length; i++) {
        this.Add(levels[i]);
      }
    }
    #endregion

    #region public override string ToolTipText
    public override string ToolTipText {
      get {
        if (_savedIndexFiboLine < 0 && _savedIndexFiboLine >= this.Count)
          return "Error in _savedIndexFiboLine";

        return _fiboLines[_savedIndexFiboLine].ToolText;
      }
      set {base.ToolTipText = value;}
    }
    #endregion

    #region public void SetValue(DateTime time1, float price1, DateTime time2, float price2)
    /// <summary>
    /// Применение значений опорной линии
    /// </summary>
    /// <param name="time1"></param>
    /// <param name="price1"></param>
    /// <param name="time2"></param>
    /// <param name="price2"></param>
    public void SetValue(DateTime time1, float price1, DateTime time2, float price2) {
      _time1 = time1;
      _time2 = time2;
      _price1 = price1;
      _price2 = price2;
    }
    #endregion

    #region public override bool CheckFigure(Point p)
    /// <summary>
    /// Функция вызывается для проверки на вхождение курсора в область фигуры.
    /// Если функция вернет true, то всплывет подсказка с текстом из ToolTipText
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public override bool CheckFigure(Point p) {

      for (int i = 0; i < _fiboLines.Count; i++) {
        /* Заменим базовый GraphicsPath на тот, что из линии, и 
         * проверим на вхождение позиции курсора */
        this._gpath = _fiboLines[i].GraphicsPath;

        if (base.CheckFigure(p)) {
          /* Вхождение позиции курсора есть на этой линии. 
           * Сохраним индекс этой линии, для использование 
           * его в ToolTipText 
           */
          _savedIndexFiboLine = i;
          return true;
        }
      }
      _savedIndexFiboLine = -1;
      return false;
    }
    #endregion

    #region protected override void OnPaint(System.Drawing.Graphics g)
    protected override void OnPaint(System.Drawing.Graphics g) {
      int barIndex1 = this.ChartBox.ChartManager.Bars.GetBarIndex(_time1);
      int barIndex2 = this.ChartBox.ChartManager.Bars.GetBarIndex(_time2);
      int x1 = this.ChartBox.GetX(barIndex1);
      int x2 = this.ChartBox.GetX(barIndex2);
      int y1 = this.ChartBox.GetY(_price1);
      int y2 = this.ChartBox.GetY(_price2);

      int m = 3;
      for (int i = 0; i < _fiboLines.Count; i++) {
        FiboLine fiboLine = this._fiboLines[i];
        int xx2 = x2 + (x2 - x1) * m;
        float dy = ((y2 - y1) * fiboLine.Level) / 100F;
        int ylevel = y2 - (int)dy;
        int yy2 = ylevel + (ylevel - y1) * m;

        Pen pen = fiboLine.Pen;

        if (_savedIndexFiboLine == i) {
          pen = (Pen)fiboLine.Pen.Clone();
          pen.Width = 3;
        }

        g.DrawLine(pen, x1, y1, xx2, yy2);
        fiboLine.Update(x1, y1, xx2, yy2);
      }
    }
    #endregion
  }
}
