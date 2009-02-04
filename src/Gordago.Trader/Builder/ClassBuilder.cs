/**
* @version $Id: ClassBuilder.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Builder
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Reflection;
  using System.Globalization;

  public class ClassBuilder {

    private readonly ClassConstructor[] _constructors;
    private readonly Type _classType;
    private readonly Parameter[] _parameters;
   
    public ClassBuilder(Type type) {
      _classType = type;
      Parameter[] defParameters = GetParameters(type);
      
      List<Parameter> listParameters = new List<Parameter>();
      foreach (Parameter prm in defParameters) {
        Parameter newParam = this.OnParameterAdded(prm);
        if (newParam != null)
          listParameters.Add(newParam);
      }
      Parameter[] parameters = listParameters.ToArray();
      _parameters = parameters;
      
      ConstructorInfo[] constructors = type.GetConstructors();
      List<ClassConstructor> list = new List<ClassConstructor>();
      foreach (ConstructorInfo cnst in constructors) {
        if (!cnst.IsPublic)
          continue;
        ClassConstructor classConstructor = new ClassConstructor(cnst, parameters);
        if ((int)classConstructor.Error == -1)
          list.Add(classConstructor);
      }
      _constructors = list.ToArray();
    }

    #region public Parameter[] Parameters
    public Parameter[] Parameters {
      get { return this._parameters; }
    }
    #endregion

    #region protected virtual Parameter OnParameterAdded(Parameter parameter)
    protected virtual Parameter OnParameterAdded(Parameter parameter) {
      return parameter;
    }
    #endregion

    #region internal static Parameter[] GetParameters(Type type)
    internal static Parameter[] GetParameters(Type type) {
      List<Parameter> parameters = new List<Parameter>();

      PropertyInfo[] props = type.GetProperties();

      foreach (PropertyInfo pinfo in props) {
        Parameter param = new Parameter(pinfo);
        if ((int)param.Error == -1)
          parameters.Add(param);
      }
      return parameters.ToArray();
    }
    #endregion

    #region public ClassConstructor[] Constructors
    public ClassConstructor[] Constructors {
      get { return this._constructors; }
    }
    #endregion

    #region public Type ClassType
    public Type ClassType {
      get { return this._classType; }
    }
    #endregion

    #region public Object CreateInstance(params Parameter[] parameters)
    /// <summary>
    /// Создание экземпляра класса
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Object CreateInstance(params Parameter[] parameters) {
      ParameterCollection pcs = new ParameterCollection();
      pcs.AddRange(parameters);

      List<Parameter> inputs = new List<Parameter>();
      ClassConstructor actualConstructor = null;

      foreach (ClassConstructor constructor in Constructors) {
        inputs.Clear();
        bool actual = true;
        foreach (Parameter input in constructor.Parameters) {
          Parameter newParam = pcs[input.Name];
          if (newParam == null) {
            actual = false;
            break;
          }
          inputs.Add(newParam);
        }
        if (actual)
          actualConstructor = constructor;
      }

      if (actualConstructor == null)
        throw (new Exception("Actual constructor not found."));

      List<object> prmObjects = new List<object>();
      
      foreach (Parameter param in inputs) {
        //if (param.Value is Type) {
        //  ClassBuilder cb = new ClassBuilder(param.Value as Type);

        //  param.Value = cb.CreateInstance(parameters);
        //}
        if (param.Value == null) {
          if (param.ClassBuilder == null) {
            if (param.DefaultType != null) {
              param.ClassBuilder = new ClassBuilder(param.DefaultType);
            } else {
              param.ClassBuilder = new ClassBuilder(param.ParameterType);
            }
          }
          param.Value = param.ClassBuilder.CreateInstance(parameters);
        }
        prmObjects.Add(param.Value);
      }

      //BindingFlags bf = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

      //object obj = typeof(Indicator).GetType().
      //  Assembly.CreateInstance(_classType.FullName, false,
      //  bf, null, prmObjects.ToArray(), 
      //  CultureInfo.CurrentCulture, null);
      //return obj;
      
      return Activator.CreateInstance(_classType, prmObjects.ToArray());
    }
    #endregion
  }
}
