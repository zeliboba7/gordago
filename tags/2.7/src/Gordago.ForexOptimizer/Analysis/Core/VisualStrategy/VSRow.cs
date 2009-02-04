/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis;
using Gordago.Analysis.Vm;
using Gordago.Analysis.Vm.Compiler;

namespace Gordago.Strategy {

  interface IVSRow {
    int MaxBarsBack { get;}
    string[] FunctionNames { get;}
    void Compile(TradeVariables vars);
  }

  class VSRow:IVSRow {
    private string _condition;
    private VmCommand[] _compile;
    private int _maxBarsBack;
    private string[] _functionNames;

    public VSRow(string condition) {
      _condition = condition;
    }

    #region public string[] FunctionNames
    public string[] FunctionNames {
      get { return this._functionNames; }
    }
    #endregion

    #region public int MaxBarsBack
    public int MaxBarsBack {
      get { return this._maxBarsBack; }
    }
    #endregion

    #region public VmCommand[] Commands
    public VmCommand[] Commands {
      get { return this._compile; }
    }
    #endregion

    #region public string Condition
    public string Condition {
      get { return this._condition; }
    }
    #endregion

    #region public void Compile(TradeVariables vars)
    public void Compile(TradeVariables vars) {
      string condition = _condition;
      for (int i = 0; i < vars.Count; i++) {
        ITradeVar var = vars[i];
        condition = condition.Replace("$" + var.Name, var.ToString());
      }
      _compile = Expression.Compile(condition);
      _maxBarsBack = 0;
      List<string> list = new List<string>();
      for (int i = 0; i < _compile.Length; i++) {
        VmCommand vmc = _compile[i];
        if (vmc.Opcode == VmOpcode.Lv && i < _compile.Length - 2 && _compile[i + 1].Opcode == VmOpcode.Sb && _compile[i + 2].Opcode == VmOpcode.Le) {
          if (vmc.Value != null) {
            _maxBarsBack = Math.Max(_maxBarsBack, (int)vmc.Value);
          }
        } else if (vmc.Opcode == VmOpcode.Ink && vmc.Function != null) {
          AddFNameInList(list, vmc.Function.ShortName);
        }
      }
      _functionNames = list.ToArray();
    }
    #endregion

    #region public bool Execute(Analyzer analyzer)
    public bool Execute(Analyzer analyzer) {
      object result = VmEvaluator.Evaluate(_compile, analyzer);
      if (result != null)
        return (bool)result ? true : false;
      return false;
    }
    #endregion

    #region public float Calculate(Analyzer analyzer)
    public float Calculate(Analyzer analyzer) {
      object result = VmEvaluator.Evaluate(_compile, analyzer);

      if (result != null && result is float)
        return (float)result;
      return 0;
    }
    #endregion

    #region public static void AddFNameInList(List<string> list, string fname)
    public static void AddFNameInList(List<string> list, string fname) {
      for (int i = 0; i < list.Count; i++) {
        if (list[i] == fname) return;
      }
      list.Add(fname);
    }
    #endregion
  }
}
