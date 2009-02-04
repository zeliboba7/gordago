/**
* @version $Id: IndicatorInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Reflection;
  
  using System.Drawing;
  using System.ComponentModel;
  using Gordago.Trader.Builder;

  public class IndicatorInfo:ClassBuilder {
    
    public IndicatorInfo(Type type):base(type) {

      PropertyInfo[] props = type.GetProperties();
      foreach (PropertyInfo pinfo in props) {

      }
    }
  }

  public class old_IndicatorInfo {

    #region public class IndFunctionInfoCollection : FunctionInfoCollection
    public class IndFunctionInfoCollection : FunctionInfoCollection {

      #region public new IndFunctionInfo this[Type typeFunction]
      public new IndFunctionInfo this[Type typeFunction] {
        get {
          return base[typeFunction] as IndFunctionInfo;
        }
      }
      #endregion

      #region private static int CompareByOrder(IndFunctionInfo prm1, IndFunctionInfo prm2)
      private static int CompareByOrder(IndFunctionInfo prm1, IndFunctionInfo prm2) {
        return prm1.Order.CompareTo(prm2.Order);
      }
      #endregion

      #region internal void Sort()
      internal void Sort() {
        List<IndFunctionInfo> list = new List<IndFunctionInfo>();
        foreach (IndFunctionInfo pi in this) {
          list.Add(pi);
        }
        list.Sort(CompareByOrder);
        base.Clear();
        foreach (IndFunctionInfo pi in list) {
          this.Add(pi);
        }
      }
      #endregion
    }
    #endregion

    private readonly IndFunctionInfoCollection _functions = new IndFunctionInfoCollection();
    private readonly ParameterCollection _parameters = new ParameterCollection();

    private Type _indicatorType;

    private Indicator _currentInstance = null;

    public old_IndicatorInfo(Type indicatorType) {
      _indicatorType = indicatorType;
      PropertyInfo[] props = indicatorType.GetProperties();
      foreach (PropertyInfo prop in props) {
        if (!CheckBaseType(prop.PropertyType, typeof(Function)))
          continue;
        IndFunctionInfo fi = new IndFunctionInfo(prop);
        if ((int)fi.Error == -1)
          _functions.Add(fi);
      }
      _functions.Sort();

      Parameter[] parameters = FunctionInfo.GetParameters(indicatorType);
      _parameters.AddRange(parameters);
    }

    #region public Type IndicatorType
    public Type IndicatorType {
      get { return this._indicatorType; }
    }
    #endregion

    #region public IndFunctionInfoCollection Functions
    public IndFunctionInfoCollection Functions {
      get { return _functions; }
    }
    #endregion

    #region public ParameterInfoCollection Parameters
    public ParameterCollection Parameters {
      get { return this._parameters; }
    }
    #endregion

    #region public Indicator Indicator
    public Indicator Indicator {
      get { return _currentInstance; }
    }
    #endregion

    #region internal static bool CheckBaseType(Type type1, Type type2)
    internal static bool CheckBaseType(Type type1, Type type2) {
      if (type1 == type2)
        return true;
      if (type1.BaseType == null)
        return false;

      return CheckBaseType(type1.BaseType, type2);
    }
    #endregion

    #region public static object[] CreateParameters(ParameterInfoCollection parameterInfoCollection, IBarsData data)
    public static object[] CreateParameters(ParameterCollection parameterInfoCollection, IBarsData data) {
      object[] retParameters = new object[parameterInfoCollection.Count];
      int i = 0;
      foreach (Parameter param in parameterInfoCollection) {

        //if (param.FunctionDataType != null) {

        //  FunctionInfo fi = new FunctionInfo(param.FunctionDataType);
        //  retParameters[i] = fi.CreateInstance(data);
        //} else {
        //  retParameters[i] = param.Value;
        //}

        i++;
      }
      return retParameters;
    }
    #endregion

    #region public void CreateInstance(IBarsData data)
    public void CreateInstance(IBarsData data) {

      object[] parameters = CreateParameters(_parameters, data);
      
      _currentInstance = Activator.CreateInstance(_indicatorType, parameters) as Indicator;

      foreach (IndFunctionInfo finfo in _functions) {
        finfo.JoinFunction(_currentInstance);
      }
      
    }
    #endregion

    #region public void ReleaseInstance()
    public void ReleaseInstance() {
      _currentInstance = null;
    }
    #endregion
  }
}
