/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections.Generic;
using System.Text;
using Gordago.Analysis;
using System.Drawing;
using Cursit.Applic.APropGrid;

namespace Gordago.Strategy.FIndicator.FIndicParam {
  class IndicFuncParamColor:IndicFuncParam {
    public IndicFuncParamColor(Parameter prm): base(prm) {
      this.Value = (Color)prm.Value;
    }

    public new Color Value {
      get { return (Color)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString() {return "";}

    public override PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType) {
      return new PropGridValueColor(this.Caption, this.Value);
    }
    internal override string GetOptimizerString(int timeFrame, string prefix, TradeVariables tvars) {return "";}
    internal override void SetOptimizerValue(object values) { }
    public override string[] GetEditorString() {
      return new string[] { "", ""};
    }
    internal override object GetOptimizerValue() {
      return null;
    }
  }
}
