/**
* @version $Id: FunctionInfo.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;

  public class FunctionInfo {

    private readonly ParameterCollection _parameters = new ParameterCollection();
    private readonly string _name;
    private readonly Type _type;
    
    public FunctionInfo(Type functionType) {
      _type = functionType;
      _name = functionType.Name;

      _parameters.AddRange(GetParameters(functionType));
      _parameters.Sort();
    }

    #region public string Name
    public string Name {
      get { return this._name; }
    }
    #endregion

    #region public Type FunctionType
    public Type FunctionType {
      get { return _type; }
    }
    #endregion

    #region public ParameterInfoCollection Parameters
    public ParameterCollection Parameters {
      get { return this._parameters; }
    }
    #endregion

    #region internal static ParameterInfo[] GetParameters (Type type)
    internal static Parameter[] GetParameters (Type type){
      List<Parameter> parameters = new List<Parameter>();

      PropertyInfo[] props = type.GetProperties();

      foreach (PropertyInfo prop in props) {
        Parameter pinfo = new Parameter(prop);
        if ((int)pinfo.Error == -1)
          parameters.Add(pinfo);
      }
      return parameters.ToArray();
    }
    #endregion

    #region public IFunction CreateInstance(IBarsData data)
    public IFunction CreateInstance(IBarsData data) {
      IFunction instance = null;
      //if (_type == typeof(Close)) {
      //  instance = new Close(data);
      //} else {
      //  object[] parameters = IndicatorInfo.CreateParameters(_parameters, data);

      //  instance = Activator.CreateInstance(_type, parameters) as IFunction;
      //}
      return instance;
    }
    #endregion
  }
}
