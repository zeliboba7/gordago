/**
* @version $Id: ClassConstructor.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Builder
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Reflection;
  using Gordago.Trader;

  /// <summary>
  /// Информация о конструкторе и его параметрах
  /// </summary>
  public class ClassConstructor {

    #region enum FlagsError
    [Flags]
    public enum FlagsError {
      InputInfoNotFound = 2,
      ParameterNotFound = 4
    }
    #endregion

    private FlagsError _error = FlagsError.InputInfoNotFound;
    private readonly ParameterCollection _parameters = new ParameterCollection();
    private readonly ConstructorInfo _constructorInfo;

    public ClassConstructor(ConstructorInfo cinfo, Parameter[] parameters) {
      _constructorInfo = cinfo;
      object[] attrs = cinfo.GetCustomAttributes(false);
      List<string> inputs = new List<string>();

      foreach (object obj in attrs) {
        if (obj is InputAttribute) {
          InputAttribute attr = obj as InputAttribute;
          _error |= ~FlagsError.InputInfoNotFound;
          inputs.AddRange(attr.Parameters);
        }
      }
      foreach (string input in inputs) {
        bool find = false;
        foreach (Parameter param in parameters) {
          if (param.Name == input) {
            _parameters.Add(param);
            find = true;
          }
        }
        if (!find)
          _error |= FlagsError.ParameterNotFound;
      }
    }

    #region public FlagsError Error
    public FlagsError Error {
      get { return _error; }
    }
    #endregion

    #region public ConstructorInfo ConstructorInfo
    public ConstructorInfo ConstructorInfo {
      get { return _constructorInfo; }
    }
    #endregion

    #region public ParameterCollection Parameters
    public ParameterCollection Parameters {
      get { return this._parameters; }
    }
    #endregion
  }
}
