using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iAccumulationDistribution:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Accumulation/Distribution";
			this.ShortName = "AD";
			this.SetImage("i.gif");
			this.IsSeparateWindow = true;

			FunctionStyle fsma = RegFunction("AD");
			fsma.SetDrawPen(Color.Red);
		}
	}

	[Function("AD")]
	public class AD : Function {
		private Function _funcSub, _funcMul, _funcDiv;

		protected override void Initialize() {
			_funcSub = GetFunction("sub");
			_funcMul = GetFunction("mul");
			_funcDiv = GetFunction("div");
			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			ParameterVector pvclose = new ParameterVector("VectorClose", "Close");
			pvclose.Visible = false;
			RegParameter(pvclose);
			ParameterVector pvvolume = new ParameterVector("VectorVolume", "Volume");
			pvvolume.Visible = false;
			RegParameter(pvvolume);
      RegParameter(new ParameterColor("ColorAD", new string[] { "Color", "Цвет" }, Color.Red));
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector v2 = analyzer.Compute(_funcSub,parameters[1],parameters[0]);
			IVector v3 = analyzer.Compute(_funcSub,parameters[2],parameters[0]);
			IVector v4 = analyzer.Compute(_funcSub,parameters[1],parameters[2]);
			IVector v5 = analyzer.Compute(_funcSub,v3,v4);
			IVector v6 =  analyzer.Compute(_funcDiv,v5,v2);
			IVector v1 = analyzer.Compute(_funcMul,v6,parameters[3]);

			IVector vector = analyzer.Compute(_funcMul,analyzer.Compute(_funcDiv,analyzer.Compute(_funcSub,analyzer.Compute(_funcSub,parameters[2],parameters[0]),analyzer.Compute(_funcSub,parameters[1],parameters[2])),analyzer.Compute(_funcSub,parameters[1],parameters[0])),parameters[3]);
			
			if ( result == null )
				result = new Vector();
			if (result.Count == vector.Count)
				result.RemoveLastValue();

			float val;
			for ( int i = result.Count; i < vector.Count; i++ ) {
				val = vector[i];

				if ( val.Equals(float.NaN))
					val = 0;
				
				if (result.Count > 0)
					val += result.Current;

				result.Add(val);
			}

			return result;
		}
	}
}
