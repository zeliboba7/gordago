using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	public class iBollingerBands: Indicator{
	
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Absolute;
			this.Name = "Bollinger Bands";
			this.ShortName = "BB";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			RegFunction("TopBB");
			RegFunction("MiddleBB");
			RegFunction("BottomBB");

      foreach(FunctionStyle fs in this.FunctionStyles) {
        fs.SetDrawPen(Color.Green);
      }
		}
	}

	[Function("TopBB")]
	public class TopBB : Function {
		private Function _funcAdd, _funcSmul, _funcStdDev, _funcMA;
		
		protected override void Initialize() {
			_funcAdd = GetFunction("add");
			_funcSmul = GetFunction("smul");
			_funcStdDev = GetFunction("stddev");
			_funcMA = GetFunction("ma");
			this.RegParameter(_funcMA);
			this.RegParameter(new ParameterFloat("Deviation", new string[]{"Deviation", "Отклонение"}, 2, 1, 10));

			this.GetParameter("Period").Value = 21;
			this.GetParameter(Function.PName_MAType).Value = 0;
      RegParameter(new ParameterColor("ColorBB", new string[] { "Color", "Цвет" }, Color.Green));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			return analyzer.Compute(_funcAdd,analyzer.Compute(_funcSmul,analyzer.Compute(_funcStdDev,parameters[2],parameters[0]),parameters[3]),analyzer.Compute(_funcMA,period,parameters[1],parameters[2]));
		}
	}

	[Function("MiddleBB")]
	public class MiddleBB : Function {
		private Function _funcMA;

		protected override void Initialize() {
			_funcMA = GetFunction("MA");
			this.RegParameter(_funcMA);
			this.GetParameter("Period").Value = 21;
			this.GetParameter(Function.PName_MAType).Value = 0;
      RegParameter(new ParameterColor("ColorBB", new string[] { "Color", "Цвет" }, Color.Green));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			return analyzer.Compute(_funcMA, period, parameters[1], parameters[2]);
		}
	}

	[Function("BottomBB")]
	public class BottomBB : Function {
		private Function _funcSub, _funcSmul, _funcStdDev, _funcMA;

		protected override void Initialize() {
			_funcSub = GetFunction("sub");
			_funcSmul = GetFunction("smul");
			_funcStdDev = GetFunction("stddev");
			_funcMA = GetFunction("ma");

			this.RegParameter(_funcMA);
			this.RegParameter(new ParameterFloat("Deviation", new string[]{"Deviation", "Отклонение"}, 2, 1, 10));
			this.GetParameter("Period").Value = 21;
			this.GetParameter(Function.PName_MAType).Value = 0;
      RegParameter(new ParameterColor("ColorBB", new string[] { "Color", "Цвет" }, Color.Green));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			IVector vstddev = analyzer.Compute(_funcStdDev, parameters[2], period);
			IVector vsmul = analyzer.Compute(_funcSmul,vstddev,parameters[3]);
			IVector vma = analyzer.Compute(_funcMA,period,parameters[1],parameters[2]);

			return analyzer.Compute(_funcSub, vma ,vsmul);
		}
	}
}
