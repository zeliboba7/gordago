﻿/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Gordago.Analysis.Kernel {

  [Function("High")]
  class High : Function {

    protected override void Initialize() {
      ParameterInteger iparam = new ParameterInteger(PNameTimeFrame, 0);
      iparam.Visible = false;
      this.RegParameter(iparam);
      RegParameter(new ParameterColor("ColorHigh", new string[] { "Color", "Цвет" }, Color.MediumBlue));
    }

    public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
      int second = (int)parameters[0];

      if (result == null)
        result = new BarsVector(analyzer.GetBars(second), second, BarsTypeValue.High);

      return result;
    }
  }
}
