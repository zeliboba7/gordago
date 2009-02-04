/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

using Gordago.Docs;
using Cursit.Applic.APropGrid;

namespace Gordago.Strategy.FIndicator.FIndicParam {
	public class IndicFuncParamTimeFrame: IndicFuncParam {
		public IndicFuncParamTimeFrame(DocParameter docParameter):base(docParameter, IndicFuncParamType.TimeFrame) {
			this.Value = 0;
		}

		internal override string GetOptimizerString(int timeFrame, string prefix, Gordago.Strategy.IO.TradeVariables tvars) {
			string strTF = Convert.ToString(timeFrame);
			this.CompVar = null;
			return strTF;
		}

		public override Cursit.Applic.APropGrid.PropGridValue GetPropGridValue(IndicatorInsertBoxType iiboxType) {
			return new PropGridValueNumber(DocParameter.Name);
		}

		public override string[] GetEditorString() {
			return new string[]{DocParameter.Name, DocParameter.Name};
		}

		internal override object GetOptimizerValue() {
			return null;
		}
		
		internal override void SetOptimizerValue(object values) {}

	}
}
