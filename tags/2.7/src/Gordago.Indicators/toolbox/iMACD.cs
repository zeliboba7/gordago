using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {

	/// <summary>
	/// Класс индикатора iMACD 
	/// </summary>
	public class iMACD: Indicator{

		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";

			this.GroupName = "Relative scaled";
			this.Name = "MACD";
			this.ShortName = "MACD";
			this.SetImage("i.gif");

			FunctionStyle fsBaseMACD = RegFunction("BaseMACD");
			fsBaseMACD.SetDrawStyle(FunctionDrawStyle.Histogram);
			fsBaseMACD.SetDrawPen(Color.Blue);

			FunctionStyle fsSignalMACD = RegFunction("SignalMACD");
			Pen pen = new Pen(Color.Red);
			pen.DashStyle = DashStyle.Dot;
			fsSignalMACD.SetDrawPen(pen);
		}
	}

	[Function("BaseMACD")]
	public class BaseMACD : Function {
		Function _funcMA, _funcSub;

		protected override void Initialize() {
			this.SetShortName("HistMACD");
			_funcMA = GetFunction("ma");
			_funcSub = GetFunction("sub");
			RegParameter(new ParameterInteger("PeriodFastMA", new string[]{"Fast MA", "Быстрая МА"}, 12, 1, 10000));
			RegParameter(new ParameterInteger("PeriodSlowMA", new string[]{"Slow MA", "Медленная МА"}, 26, 1, 10000));
			RegParameter(Function.CreateDefParam_MAType(1));
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
      RegParameter(new ParameterColor("ColorBaseMACD", new string[] { "Color", "Цвет" }, Color.Blue));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector resma1 = analyzer.Compute(_funcMA,parameters[0],parameters[2],parameters[3]);
			IVector resma2 = analyzer.Compute(_funcMA,parameters[1],parameters[2],parameters[3]);

			result = analyzer.Compute(_funcSub, resma1, resma2);
			return result;
		}
	}

	[Function("SignalMACD")]
	public class SignalMACD : Function {
		private Function _funcMA, _funcBaseMACD;

		protected override void Initialize() {
			/* Определение функции Moving Average */
			_funcMA = GetFunction("ma");
			
			/* Определение функции BaseMACD - базовая линия MACD */
			_funcBaseMACD = GetFunction("BaseMACD");

			/* Регистрация параметров из функции _funcBaseMACD*/
			this.RegParameter(_funcBaseMACD);
			RegParameter(new ParameterInteger("PeriodSignalMA", new string[]{"Signal MA", "Сигнальная МА"}, 9, 1, 10000));

      RegParameter(new ParameterColor("ColorSignalMACD", new string[] { "Color", "Цвет" }, Color.Red));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int signalperiod = (int)parameters[4];

			IVector resbmacd = analyzer.Compute(_funcBaseMACD, parameters[0], parameters[1],parameters[2],parameters[3]);
			return analyzer.Compute(_funcMA, signalperiod, 0, resbmacd);
		}
		
	}
}
