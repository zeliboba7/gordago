using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iForceIndex:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Force Index";
			this.ShortName = "FI";
			this.SetImage("i.gif");

			FunctionStyle fs = this.RegFunction("fi");
			fs.SetDrawPen(Color.DarkBlue);
		}
	}
	
	[Function("RawFI")]
	public class RawFI : Function {
		protected override void Initialize() {}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector vector = parameters[0] as IVector;
			IVector volume = parameters[1] as IVector;
			
			if ( result == null )
				result = new Vector();

			if (result.Count == vector.Count)
				result.RemoveLastValue();
			
			for ( int i = result.Count; i < vector.Count; i++ ) {
				if (i == 0)
					result.Add(0);
				else
					result.Add(volume[i] * (vector[i] - vector[i-1]));
			}			
			return result;
		}
	}
	
	[Function("FI")]
	public class FI : Function {
		
		private Function _funcMA, _funcRawFI;
		protected override void Initialize() {
			_funcMA = GetFunction("ma");
			_funcRawFI = GetFunction("RawFI");
			RegParameter(_funcMA);
			this.GetParameter(Function.PName_MAType).Value = 0;
			ParameterVector pvvolume = new ParameterVector("VectorVolume", "Volume");
			pvvolume.Visible = false;
			RegParameter(pvvolume);

      RegParameter(new ParameterColor("ColorFI", new string[] { "Color", "Цвет" }, Color.DarkBlue));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			int period = (int)parameters[0];
			return analyzer.Compute(_funcMA, period, parameters[1], analyzer.Compute(_funcRawFI, parameters[2], parameters[3]));
		}
	}
}
