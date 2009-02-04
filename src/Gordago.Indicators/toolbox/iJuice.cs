using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iJuice : Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "Relative scaled";
			this.Name = "Juice";
			this.ShortName = "Juice";
			this.SetImage("i.gif");

			FunctionStyle fs = RegFunction("JuiceUp");
			fs.SetDrawPen(Color.Green);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);

			fs = RegFunction("JuiceDown");
			fs.SetDrawPen(Color.DarkRed);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);

		}
	}

	[Function("JuiceUp")]
	public class JuiceUp: Function{
		Function _funcJuice;
		protected override void Initialize() {
			_funcJuice = this.GetFunction("Juice");
			this.RegParameter(_funcJuice);
      RegParameter(new ParameterColor("ColorJuiceUp", new string[] { "Color", "÷‚ÂÚ" }, Color.Green));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			int levelpoint = (int)parameters[1];
			IVector vector = (IVector)parameters[2];

			JuiceVector jresult = (JuiceVector)analyzer.Compute(_funcJuice, period, levelpoint, vector);

			return jresult.UpVector;
		}
	}

	[Function("JuiceDown")]
	public class JuiceDown: Function{
		Function _funcJuice;
		protected override void Initialize() {
			_funcJuice = this.GetFunction("Juice");
			this.RegParameter(_funcJuice);
      RegParameter(new ParameterColor("ColorJuiceDown", new string[] { "Color", "÷‚ÂÚ" }, Color.DarkRed));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			int levelpoint = (int)parameters[1];
			IVector vector = (IVector)parameters[2];

			JuiceVector jresult = (JuiceVector)analyzer.Compute(_funcJuice, period, levelpoint, vector);

			return jresult.DownVector;
		}
	}


	public class JuiceVector: Vector{
		public IVector UpVector, DownVector;
		public JuiceVector(){
			UpVector = new Vector();
			DownVector = new Vector();
		}
	}

	[Function("Juice")]
	public class Juice: Function{
		private Function _funcStdDev;
		protected override void Initialize() {
			_funcStdDev = this.GetFunction("StdDev");

			this.RegParameter(new ParameterInteger("Period", 7, 1, 10000));
			this.RegParameter(new ParameterInteger("Level", "Level (point)", 4, 0, 100));
			this.RegParameter(Function.CreateDefParam_ApplyTo("Close"));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[0];
			int levelpoint = (int)parameters[1];
			IVector vector = (IVector)parameters[2];
			float level = analyzer.Point * levelpoint;

			if (result == null)
				result = new JuiceVector();

			JuiceVector jresult = result as JuiceVector;

			if (jresult.Count == vector.Count){
				jresult.RemoveLastValue();
				jresult.UpVector.RemoveLastValue();
				jresult.DownVector.RemoveLastValue();
			}

			IVector resStdDev = analyzer.Compute(_funcStdDev, vector, period);
			for (int i=result.Count;i<vector.Count;i++){
				float val = resStdDev[i] - level;
				jresult.Add(val);
				jresult.UpVector.Add(0);
				jresult.DownVector.Add(0);
				if(val>0){
					jresult.UpVector[i]=val;
				}else if(val < 0){
					jresult.DownVector[i]=val;
				}
			}
			return jresult;
		}
	}
}
