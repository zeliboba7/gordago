/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using Gordago.Analysis;

namespace Gordago.Analysis {
	class VirtualAnalyzer: Analyzer {

    private ISymbol _symbol;

    public VirtualAnalyzer(IndicatorManager imanager, ISymbol symbol) : base(imanager, symbol) {
      _symbol = symbol;
    }

    #region public override IBarList GetBars(int second)
    public override IBarList GetBars(int second) {
      return _symbol.Ticks.GetBarList(second);
    }
    #endregion

    #region public override IBarList[] BarLists
    public override IBarList[] BarLists {
      get { return _symbol.Ticks.BarLists; }
    }
    #endregion
  }
}
