/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis;
using Gordago.Strategy.FIndicator;

namespace Gordago.Strategy {

  interface IIndicatorBoxCollection {
    IndicFunctionBox[] GetIndicFunctionBoxCollection();
    void SelectIndicFunctionBox(IndicFunctionBox indicFuncBox);
  }

  interface IStrategyForm {
    string FileName { get; set;}
    bool IsDestroy { get; set;}
    bool IsOpen { get; set; }
    string[] StrategyNames { get;}

    bool LoadFromFile(string filename);
    TestReport TestReport { get;set;}

    CompileDllData Compile(string strategyName);
    void SetTestStatus(bool isStart);
  }

  class CompileDllData {
    private TradeVariables _variables;
    private Gordago.Analysis.Strategy _strategy;

    internal CompileDllData(TradeVariables vars, Gordago.Analysis.Strategy strategy) {
      _variables = vars;
      _strategy = strategy;
    }

    #region internal TradeVariables Variables
    internal TradeVariables Variables {
      get { return _variables; }
    }
    #endregion

    #region internal Gordago.Analysis.Strategy Strategy
    internal Gordago.Analysis.Strategy Strategy {
      get { return this._strategy; }
    }
    #endregion
  }


}
