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
  class VSVariant {
    private VSRow[] _rows;
    private int _maxBarsBack = 0;
    private string[] _functionNames;

    public VSVariant() {
      _rows = new VSRow[0];
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
      get { return this._rows.Length; }
    }
    #endregion

    #region public VSRow this[int index]
    public VSRow this[int index] {
      get { return this._rows[index]; }
    }
    #endregion

    #region public void Add(VSRow row)
    public void Add(VSRow row) {
      List<VSRow> rows = new List<VSRow>(_rows);
      rows.Add(row);
      _rows = rows.ToArray();
    }
    #endregion

    #region public void Compile(TradeVariables vars)
    public void Compile(TradeVariables vars) {
      _maxBarsBack = 0;
      List<string> list = new List<string>();

      for (int i = 0; i < _rows.Length; i++) {
        _rows[i].Compile(vars);
        _maxBarsBack = Math.Max(_maxBarsBack, _rows[i].MaxBarsBack);
        foreach (string fname in _rows[i].FunctionNames) {
          VSRow.AddFNameInList(list, fname);
        }
      }
      _functionNames = list.ToArray();
    }
    #endregion
  }
}
