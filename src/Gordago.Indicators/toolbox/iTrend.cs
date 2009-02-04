using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.Analysis.CustomIndicators {
	public class iTrend : Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Relative scaled";
			this.Name = "iTrend";
			this.ShortName = "iTrend";
			this.SetImage("i.gif");

			RegFunction("iTrendDir").SetDrawPen(Color.Red);
			RegFunction("iTrendPow").SetDrawPen(Color.Green);
		}
	}

	[Function("iTrendDir")]
	public class iTrendDir: Function {
		
		Function _funcbbTop, _funcbbBot, _funcbbMid, _sub;

		protected override void Initialize() {
			_sub =  GetFunction("sub");
			_funcbbBot = GetFunction("bottombb");
			_funcbbTop = GetFunction("topbb");
			_funcbbMid = GetFunction("middlebb");

			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			RegParameter(new ParameterInteger("PeriodDir", 20, 1, 10000));
			RegParameter(new ParameterEnum("Mode", 0, new string[]{"Main", "Bottom", "Top"}));
			RegParameter(new ParameterFloat("Deviation", new string[]{"Deviation", "Отклонение"}, 2, 1, 10));
      RegParameter(new ParameterColor("ColoriTrendDir", new string[] { "Color", "Цвет" }, Color.Red));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			
			IVector vector = parameters[0] as IVector;

			int period = (int)parameters[1];
			int bbmode = (int)parameters[2];
			float bbdev  = (float)parameters[3];

			IVector resbb;
			
			switch (bbmode) {
				case 1:
					resbb = analyzer.Compute(_funcbbBot, period, 0, vector, bbdev); 
					break;
				case 2:
					resbb = analyzer.Compute(_funcbbTop, period, 0, vector, bbdev); 
					break;
				default:
					resbb = analyzer.Compute(_funcbbMid, period, 0, vector); 
					break;
			}

			result = analyzer.Compute(_sub, vector, resbb);
			return result;
		}
	}

	[Function("iTrendPow")]
	public class iTrendPow: Function {
		Function _funcbep, _funcbup;

		protected override void Initialize() {
			_funcbep = GetFunction("bearspower");
			_funcbup = GetFunction("bullspower");

			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			RegParameter(new ParameterInteger("PeriodPow", 13, 1, 10000));
			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
      RegParameter(new ParameterColor("ColoriTrendPow", new string[] { "Color", "Цвет" }, Color.Blue));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector vector = parameters[0] as IVector;
			int period = (int)parameters[1];
			IVector low =  parameters[2] as IVector; 
			IVector high =  parameters[3] as IVector; 

			IVector resbep, resbup;

			resbep = analyzer.Compute(_funcbep, low, period, 1, vector);
			resbup = analyzer.Compute(_funcbup, high, period, 1, vector);

			if ( result == null ) 
				result = new Vector();
			
			if (result.Count == low.Count)
				result.RemoveLastValue();

			int rsize = result.Count;
			for (int i=result.Count; i < low.Count; i++)
				result.Add(-(resbep[i]+resbup[i]));
			return result;
		}
	}
}
