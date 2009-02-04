using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iEnvelope :Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Absolute;
			this.Name = "Envelope";
			this.ShortName = "Envelope";
			this.SetImage("i.gif");
			this.IsSeparateWindow = false;

			this.RegFunction("TopEnvelope");
			this.RegFunction("MiddleEnvelope");
			this.RegFunction("BottomEnvelope");

			foreach (FunctionStyle fs in this.FunctionStyles){
				fs.SetDrawPen(Color.DarkCyan);
			}
		}
	}
	[Function("TopEnvelope")]
	public class TopEnvelope : Function {
		private Function _funcSmul, _funcMA;

		protected override void Initialize() {
			this.SetShortName("Top Env.");
			_funcSmul = GetFunction("smul");
			_funcMA = GetFunction("MA");
			this.RegParameter(_funcMA);
			this.GetParameter("Period").Value = 21;
			this.RegParameter(new ParameterFloat("Deviation", new string[]{"Deviation", "Отклонение"}, 0.1F, 0.01F, 100, 0.01f, 2));
      RegParameter(new ParameterColor("ColorTopEnvelope", new string[] { "Color", "Цвет" }, Color.DarkCyan));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			return analyzer.Compute(_funcSmul,analyzer.Compute(_funcMA,period,parameters[1],parameters[2]),1f + (float)parameters[3] / 100f);
		}
	}
	[Function("MiddleEnvelope")]
	public class MiddleEnvelope : Function {
		private Function _funcMA;

		protected override void Initialize() {
			this.SetShortName("Mid.Env.");
			_funcMA = GetFunction("MA");
			this.RegParameter(_funcMA);
			this.GetParameter("Period").Value = 21;
      RegParameter(new ParameterColor("ColorMiddleEnvelope", new string[] { "Color", "Цвет" }, Color.DarkCyan));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			result = analyzer.Compute(_funcMA,period,parameters[1],parameters[2]);
			return result;
		}
	}
	[Function("BottomEnvelope")]
	public class BottomEnvelope : Function {
		private Function _funcSmul, _funcMA;

		protected override void Initialize() {
			this.SetShortName("Bot.Env.");
			_funcSmul = GetFunction("smul");
			_funcMA = GetFunction("MA");

			Function topenv = GetFunction("TopEnvelope");
			this.RegParameter(topenv);
      RegParameter(new ParameterColor("ColorBottomEnvelope", new string[] { "Color", "Цвет" }, Color.DarkCyan));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			result = analyzer.Compute(_funcSmul,analyzer.Compute(_funcMA,period,parameters[1],parameters[2]),1f - (float)parameters[3] / 100f);
			return result;
		}
	}
}
