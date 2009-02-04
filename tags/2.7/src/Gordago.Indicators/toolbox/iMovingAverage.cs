using System;
using System.Drawing;
using Gordago.Analysis;
using System.Collections.Generic;

namespace Gordago.StockOptimizer2.Toolbox {

  #region public class iMovingAverage : Indicator
  /// <summary>
  /// ��������� iMovingAverage
  /// </summary>
  public class iMovingAverage : Indicator {

    /// <summary>
    /// ����� �������������.
    /// ������ � ��� ���������� ��������� ���
    /// ������� ��������� � �����������
    /// </summary>
    public override void Initialize() {
      this.Copyright = "Copyright � 2006, Gordago Software Ltd.";
      this.Link = "http://www.gordago.com/";

      /* ������, � ������� ��������� ��������� */
      this.GroupName = "Absolute scaled";

      /* ������������ ���������� */
      this.Name = "Moving Average";

      /* ����������� ������������ ����������*/
      this.ShortName = "MA";

      /* ��� ������ ���������� (����� ������ ���� � ����� ��������� indicators\images)*/
      this.SetImage("i.gif");

      /* ���� false - ��������� ������������� � ����� ���� */
      this.IsSeparateWindow = false;

      /* ����������� ������� ma. ���������� FunctionStyle ����� ���� ������� �� ������� */
      FunctionStyle fs = RegFunction("ma");

      /* ����������� ����� �� ��������� */
      fs.SetDrawPen(Color.Red);
    }
  }
  #endregion

  #region class VectorMA : Vector
  class VectorMA : Vector {
    private double _smaSumma = 0, _smaSummaLast = 0;
    private double _smma = 0, _smmaLast = 0;
    private int _savedLastIndex;

    #region public double SMASumma
    public double SMASumma {
      get { return this._smaSumma; }
      set { this._smaSumma = value;}
    }
    #endregion

    #region public double SMASummaLast
    public double SMASummaLast {
      get { return this._smaSummaLast; }
      set { this._smaSummaLast = value; }
    }
    #endregion

    #region public int SavedLastIndex
    public int SavedLastIndex {
      get { return _savedLastIndex; }
      set { this._savedLastIndex = value; }
    }
    #endregion

    #region public double SMMA
    public double SMMA {
      get { return this._smma; }
      set { this._smma = value; }
    }
    #endregion

    #region public double SMMALast
    public double SMMALast {
      get { return this._smmaLast; }
      set { this._smmaLast = value; }
    }
    #endregion
  }

  #endregion

  /// <summary>
  /// ������� ������� ���������� Moving Average
  /// </summary>
  [Function("MA")]
  public class MA : Function {

    #region protected override void Initialize()
    /// <summary>
    /// ����� �������������, ������ � ��� ���������� ��������� ������
    /// ��������� � ������������ ���� �������
    /// </summary>
    protected override void Initialize() {

      #region RegParameter(new ParameterInteger("Period", new string[] { "Period", "������" }, 13, 1, 10000))
      /* ����������� ��������� Period ��� ParameterInteger - �����:
       * ��� ���������: Period
       * ����������� � ���� ��������:
       * new string[]{"Period", "������"} - ������ �����, ������ ������ �� ����������, ������ �� �������. 
       * �������� �� ���������: 13
       * ����������� ��������: 1
       * ������������ ��������: 10000
       * */
      RegParameter(new ParameterInteger("Period", new string[] { "Period", "������" }, 13, 1, 10000));
      #endregion

      #region RegParameter(Function.CreateDefParam_MAType(1))
      /* ����������� ��������� ��� Moving Average. 
       * Function.CreateDefParam_MAType(1) - ������������ ��� ��������.
       * ��� ������������
       * new ParameterEnum("__MAType", new string[]{"Type", "���"}, 1,
       *     new string[]{"Simple","Exponential","Linear Weighted","Smoothed"});
       * ��� ���������: __MAType
       * ����������� � ���� ��������: new string[]{"Type", "���"}
       * �������� �� ���������: 1
       * �������� �������� ������������: new string[]{"Simple","Exponential","Linear Weighted","Smoothed"}
       *  */
      RegParameter(Function.CreateDefParam_MAType(1));
      #endregion

      #region RegParameter(Function.CreateDefParam_ApplyTo("Close"))
      /* ����������� ��������� "��������� �" ��� ParameterVector
       * Function.CreateDefParam_ApplyTo("Close") - ������������ ��� ��������
       * ��� ������������
       * new ParameterVector("__ApplyTo", new string[]{"Apply to", "��������� �"}, price)
       * price - ����� ��������� ��������:
       * "close", "open", "high",  "low",  "median", "typical", "weighted"
       * */
      RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      #endregion

      RegParameter(new ParameterColor("ColorMA", new string[] { "Color", "����" }, Color.Red));
    }
    #endregion

    #region private void DEBUG(IVector vector, int count)
    private void DEBUG(IVector vector, int count) {
      List<string> sa = new List<string>();
      count = Math.Min(count, vector.Count);

      for (int j = count - 1; j >= 0; j--) {
        sa.Add(vector[j].ToString());
      }
      System.Diagnostics.Debug.WriteLine(string.Join(" ", sa.ToArray()));
    }
    #endregion

    /* ����� ������� ������� */
    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int period = Math.Max((int)parameters[0], 1);
      int typema = (int)parameters[1];
      IVector vector = (IVector)parameters[2];

      if (period == 1)
        return vector;

      if (result == null)
        result = new VectorMA();

      VectorMA resultMA = result as VectorMA;

      if (result.Count == vector.Count) {
        resultMA.SMASumma = resultMA.SMASummaLast;
        resultMA.RemoveLastValue();
      }

      switch (typema) {
        #region case 0: - Simple MA
        case 0:
          for (int i = result.Count + 1; i < vector.Count + 1; i++) {
            if (i < period) {
              result.Add(float.NaN);
            } else {
              if (resultMA.SavedLastIndex < i) {
                resultMA.SMASummaLast = resultMA.SMASumma;
                resultMA.SavedLastIndex = i;
              }
              if (i == period || float.IsNaN(result.Current)) {
                resultMA.SMASumma = 0;
                for (int j = i - period; j < i; j++) 
                  resultMA.SMASumma += vector[j];
              } else {
                resultMA.SMASumma -= vector[i - period - 1];
                resultMA.SMASumma += vector[i - 1];
              }
              result.Add(Convert.ToSingle(resultMA.SMASumma / period));
#if DEBUG
              double sma = 0;
              for (int j = i - period; j < i; j++)
                sma += vector[j];
              if (resultMA.SMASumma != sma && !double.IsNaN(sma)) {
                throw (new Exception("Error"));
              }
#endif
            }
          }
          return result;
        #endregion
        #region case 1: - Exponential
        case 1:
          float k = 2f / (period + 1f);
          for (int i = result.Count; i < vector.Count; i++) {
            if (i < period) {
              result.Add(float.NaN);
            } else {
              float lastValue;
              if (i == period || float.IsNaN(result.Current))
                lastValue = vector[i];
              else
                lastValue = result.Current;

              result.Add(vector[i] * k + lastValue * (1 - k));
            }
          }
          return resultMA;
        #endregion
        #region case 2: - Linear Weighted
        case 2:
          float sw = 0.5f * period * (period + 1);
          for (int i = result.Count + 1; i < vector.Count + 1; i++) {
            if (i < period) {
              result.Add(float.NaN);
            } else {
              float wma = 0;
              for (int j = i - period, c = 1; j < i; j++, c++)
                wma += vector[j] * c;
              result.Add(wma / sw);
            }
          }
          return resultMA;
        #endregion
        case 3:
          for (int i = result.Count + 1; i < vector.Count + 1; i++) {
            if (i < period) {
              result.Add(float.NaN);
            } else {

              if (resultMA.SavedLastIndex < i) {
                resultMA.SMMALast = resultMA.SMMA;
                resultMA.SavedLastIndex = i;
              }

              if (i == period || float.IsNaN(result.Current)) {
                resultMA.SMMA = 0;
                for (int j = i - period; j < i; j++)
                  resultMA.SMMA += vector[j];
              } else {
                resultMA.SMMA = resultMA.Current * (period - 1) + vector[i - 1];
              }
              result.Add(Convert.ToSingle(resultMA.SMMA / period));
            }
          }
          return resultMA;
        default:
          return vector;
      }
    }
  }
}
