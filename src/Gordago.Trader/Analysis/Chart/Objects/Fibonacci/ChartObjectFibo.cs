/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Gordago.Design;
using System.ComponentModel.Design;

namespace Gordago.Analysis.Chart {

  #region public class FibonacciLevel

  /// <summary>
  /// Уровень Фибоначчи. Для хранения значения и описание к нему.
  /// </summary>
  public class FibonacciLevel {
    private string _description = string.Empty;
    private float _value = 0;

    public FibonacciLevel() { }

    public FibonacciLevel(float value, string description) {
      _value = value;
      _description = description;
    }

    #region public float Value
    /// <summary>
    /// Значение уровня
    /// </summary>
    [Category("Main")]
    public float Value {
      get { return this._value; }
      set {
        if (this._description == string.Empty)
          this.Description =  SymbolManager.ConvertToCurrencyString(value, 1);
        this._value = value; 
      }
    }
    #endregion

    #region public string Description
    /// <summary>
    /// Описание уровня
    /// </summary>
    [Category("Main")]
    public string Description {
      get { return this._description; }
      set { this._description = value; }
    }
    #endregion

    #region public override string ToString()
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return _value.ToString();
    }
    #endregion
  }
  #endregion

  /// <summary>
  /// Абстрактный класс для фигур Фибоначчи
  /// </summary>
  public abstract class ChartObjectFibo:ChartObjectLine {

    private Color _fiboColor;
    private Pen _fiboPen;

    private Font _font;
    private Color _foreColor;
    private Brush _foreBrush;

    private FibonacciLevel[] _levels;

    #region public ChartObjectFibo(string name) : base(name, false)
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name"></param>
    public ChartObjectFibo(string name) : base(name, false) {
      this.LinePen = new Pen(Color.Red);
      this.LinePen.DashStyle = DashStyle.Custom;
      this.LinePen.DashPattern = new float[] { 5, 5 };

      this.FiboColor = Color.Yellow;
    }
    #endregion

    #region public Color BasisLineColor
    /// <summary>
    /// Цвет базовой линии
    /// </summary>
    [Category("Style"), DisplayName("Basis Color")]
    public Color BasisLineColor {
      get { return base.LineColor; }
      set { base.LineColor = value; }
    }
    #endregion

    #region public int BasisLineWidth
    /// <summary>
    /// Ширина базовой линии
    /// </summary>
    [Category("Style"), DisplayName("Basis Width")]
    public int BasisLineWidth {
      get { return Convert.ToInt32(base.LinePen.Width); }
      set { base.LinePen.Width = value; }
    }
    #endregion

    #region public Color FiboColor
    /// <summary>
    /// Цвет линий Фибоначчи
    /// </summary>
    [Category("Style"), DisplayName("Fibo Color")]
    public Color FiboColor {
      get { return this._fiboColor; }
      set { 
        this._fiboColor = value;
        if (_fiboPen == null)
          this._fiboPen = new Pen(value);
        _fiboPen.Color = value;
      }
    }
    #endregion

    #region public int FiboWidth
    /// <summary>
    /// Ширина линий Фибоначчи
    /// </summary>
    [Category("Style"), DisplayName("Fibo Width")]
    public int FiboWidth {
      get { return Convert.ToInt32(this._fiboPen.Width); }
      set { 
        this._fiboPen.Width = value; 
      }
    }
    #endregion

    #region protected Pen FiboPen
    /// <summary>
    /// Карандаш линий Фибоначчи
    /// </summary>
    protected Pen FiboPen {
      get { return this._fiboPen; }
    }
    #endregion

    #region public FibonacciLevel[] Levels
    /// <summary>
    /// Уровни Фибоначчи
    /// </summary>
    [Category("Main")]
    [TypeConverter(typeof(ArrayTypeConverter))]
    [EditorAttribute(typeof(ArrayEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public FibonacciLevel[] Levels {
      get { return this._levels; }
      set { this._levels = value; }
    }
    #endregion

    #region public Font Font
    /// <summary>
    /// Шрифт 
    /// </summary>
    [Category("Style"), DisplayName("Font")]
    public Font Font {
      get {
        if (this.ChartBox != null) {
          if (this._font == null)
            _font = this.ChartBox.ChartManager.Style.ScaleFont;
        } else {
          this.Font = new Font("Microsoft Sans Serif", 7);
        }

        return this._font;
      }
      set { this._font = value; }
    }
    #endregion

    #region public Color ForeColor
    /// <summary>
    /// Цвет шрифта
    /// </summary>
    [Category("Style"), DisplayName("Fore Color")]
    public Color ForeColor {
      get {
        if (_foreBrush == null) {
          _foreColor = this.ChartBox.ChartManager.Style.ScaleForeColor;
          _foreBrush = new SolidBrush(_foreColor);
        }
        return this._foreColor;
      }
      set {
        this._foreBrush = new SolidBrush(value);
        this._foreColor = value;
      }
    }
    #endregion

    #region protected Brush ForeBrush
    /// <summary>
    /// Заливка шрифта
    /// </summary>
    protected Brush ForeBrush {
      get {
        if (_foreBrush == null) {
         _foreBrush = new SolidBrush(this.ForeColor);
        }
        return this._foreBrush; 
      }
    }
    #endregion

    #region protected override void SaveTemplate(XmlNodeManager nodeManager)
    /// <summary>
    /// Сохранение настроек фигуры 
    /// </summary>
    /// <param name="nodeManager"></param>
    protected override void OnSaveTemplate(XmlNodeManager nodeManager) {
      base.OnSaveTemplate(nodeManager);

      nodeManager.SetAttribute("FiboForeColor", this.ForeColor);
      nodeManager.SetAttribute("FiboFont", this.Font);

      nodeManager.SetAttribute("FiboColor", this.FiboColor);
      nodeManager.SetAttribute("FiboWidth", this.FiboPen.Width);

      string[] sa = new string[this.Levels.Length];

      for (int i = 0; i < this.Levels.Length; i++) {
        sa[i] = XmlNodeManager.ConvertFloatToString(this.Levels[i].Value)+"\t"+this.Levels[i].Description;
      }

      string sstr = string.Join("|", sa);
      nodeManager.SetAttribute("FiboLevels", sstr);
    }
    #endregion

    #region protected override void LoatTemplate(XmlNodeManager nodeManager)
    /// <summary>
    /// Загрузка настроек фигуры
    /// </summary>
    /// <param name="nodeManager"></param>
    protected override void OnLoadTemplate(XmlNodeManager nodeManager) {
      base.OnLoadTemplate(nodeManager);

      this.ForeColor = nodeManager.GetAttributeColor("FiboForeColor", Color.Red);
      this.Font = nodeManager.GetAttributeFont("FiboFont", this.Font);
      this.FiboColor = nodeManager.GetAttributeColor("FiboColor", this.FiboColor);
      this.FiboPen.Width = nodeManager.GetAttributeInt32("FiboWidth", 1);

      string sstr = nodeManager.GetAttributeString("FiboLevels", "error");
      if (sstr != "error") {

        string[] sa = sstr.Split('|');
        this.Levels = new FibonacciLevel[sa.Length];
        for (int i = 0; i < sa.Length; i++) {
          string[] ssa = sa[i].Split('\t');
          string s0 = ssa[0], s1=string.Empty;
          if (ssa.Length > 1)
            s1 = ssa[1];

          this.Levels[i] = new FibonacciLevel(XmlNodeManager.ConvertStringToFloat(s0), s1);
        }
      }
    }
    #endregion

    #region protected Size GetStringSize(Graphics g, string str)
    /// <summary>
    /// Размер текста в пикселах
    /// </summary>
    /// <param name="g"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    protected Size GetStringSize(Graphics g, string str) {
      SizeF sizef = g.MeasureString(str + "W", this.Font, 10000);
      return new Size(Convert.ToInt32(sizef.Width), Convert.ToInt32(sizef.Height));
    }
    #endregion
  }
}
