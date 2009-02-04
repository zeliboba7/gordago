/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis;

namespace Gordago.Strategy {
  class VSVariantList : IVSRow {
    private VSVariant[] _variants;
    private int _maxBarsBack = 0;
    private string[] _functionNames;

    public VSVariantList() {
      _variants = new VSVariant[0];
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

    #region public int Count
    public int Count {
      get { return this._variants.Length; }
    }
    #endregion

    #region public VSVariant this[int index]
    public VSVariant this[int index] {
      get { return this._variants[index]; }
    }
    #endregion

    #region public void Add(VSVariant variant)
    public void Add(VSVariant variant) {
      List<VSVariant> variants = new List<VSVariant>(this._variants);
      variants.Add(variant);
      _variants = variants.ToArray();
    }
    #endregion

    #region public void Clear()
    public void Clear() {
      _variants = new VSVariant[0];
    }
    #endregion

    #region public void Compile(TradeVariables vars)
    public void Compile(TradeVariables vars) {
      _maxBarsBack = 0;
      List<string> list = new List<string>();
      for (int i = 0; i < _variants.Length; i++) {
        _variants[i].Compile(vars);
        _maxBarsBack = Math.Max(_maxBarsBack, _variants[i].MaxBarsBack);

        foreach (string fname in _variants[i].FunctionNames) {
          VSRow.AddFNameInList(list, fname);
        }
      }
      _functionNames = list.ToArray();
    }
    #endregion

  }
}
