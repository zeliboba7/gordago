using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iADX:Indicator {

		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "ADX";
			this.ShortName = "ADX";
			this.SetImage("i.gif");

			FunctionStyle fspdi = RegFunction("PDI");
			fspdi.SetDrawPen(Color.Green);
			FunctionStyle fsmdi = RegFunction("MDI");
			fsmdi.SetDrawPen(Color.BurlyWood);
			FunctionStyle fsadx = RegFunction("ADX");
			fsadx.SetDrawPen(Color.RosyBrown);
		}
  }

  #region public class MSDI : Function
  [Function("MSDI")]
	public class MSDI : Function {

		protected override void Initialize() {
			ParameterVector pvlow = new ParameterVector("VectorLow", "Low");
			pvlow.Visible = false;
			RegParameter(pvlow);
			ParameterVector pvhigh = new ParameterVector("VectorHigh", "High");
			pvhigh.Visible = false;
			RegParameter(pvhigh);
			ParameterVector pvclose = new ParameterVector("VectorClose", "Close");
			pvclose.Visible = false;
			RegParameter(pvclose);

		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector1 = (IVector)parameters[0];
			IVector vector2 = (IVector)parameters[1];
			IVector vector3 = (IVector)parameters[2];
			
			if ( result == null ) 
				result = new Vector();

			if (result.Count == vector1.Count)
				result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++) {
				if (i > 0 && vector1[i] < vector1[i-1]) {
					float tr = Math.Max(Math.Max(vector2[i] - vector1[i],
						vector2[i] - vector3[i-1]),
						vector3[i - 1] - vector1[i]);
					result.Add((vector1[i - 1] - vector1[i]) / tr * 100);
				} else
					result.Add(0);
			}
			return result;
		}
  }
  #endregion

  #region public class PSDI : Function
  [Function("PSDI")]
	public class PSDI : Function {
		protected override void Initialize() {
			Function funcMSDI = GetFunction("MSDI");
			RegParameter(funcMSDI);
		}

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector1 = parameters[0] as IVector;
			IVector vector2 = parameters[1] as IVector;
			IVector vector3 = parameters[2] as IVector;
			
			if ( result == null ) 
				result = new Vector();

      if (result.Count == vector1.Count)
				result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++) {
				if (i > 0 && vector2[i] > vector2[i-1]) {
					float tr = Math.Max(Math.Max(vector2[i] - vector1[i],
						vector2[i] - vector3[i - 1]),
						vector3[i - 1] - vector1[i]);
					result.Add((vector2[i] - vector2[i - 1]) / tr * 100);
				} else
					result.Add(0);
			}
			return result;
		}
  }
  #endregion

  #region public class PDI : Function
  [Function("PDI")]
	public class PDI : Function {
		private Function _funcPSDI, _funcMA;

		protected override void Initialize() {
			this.SetShortName("+DI");
			_funcMA = GetFunction("MA");
			_funcPSDI = GetFunction("PSDI");
			RegParameter(_funcPSDI);
			RegParameter(_funcMA.CloneParameter("Period"));
      RegParameter(new ParameterColor("ColorPDI", new string[] { "+DI Color", "Цвет +DI" }, Color.Green));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[3];
			return analyzer.Compute(_funcMA,period,1,analyzer.Compute(_funcPSDI,parameters[0],parameters[1],parameters[2]));
		}
  }
  #endregion

  [Function("MDI")]
	public class MDI : Function {
		private Function _funcMSDI, _funcMA;

		protected override void Initialize() {
			this.SetShortName("-DI");
			_funcMA = GetFunction("MA");
			_funcMSDI = GetFunction("MSDI");

			Function funcPDI = GetFunction("PDI");
			RegParameter(funcPDI);

      RegParameter(new ParameterColor("ColorMDI", new string[] { "-DI Color", "Цвет -DI" }, Color.BurlyWood));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[3];
			return analyzer.Compute(_funcMA,period,1,analyzer.Compute(_funcMSDI,parameters[0],parameters[1],parameters[2]));
		}
	}

	[Function("DX")]
	public class DX : Function {
		private Function _funcPDI, _funcMDI;

		protected override void Initialize() {
			
			_funcPDI = GetFunction("PDI");
			_funcMDI = GetFunction("MDI");

			this.RegParameter(_funcPDI);
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {

			int period = (int)parameters[3];
			
			IVector vector1 = analyzer.Compute(_funcPDI,parameters[0],parameters[1],parameters[2],period);
			IVector vector2 = analyzer.Compute(_funcMDI,parameters[0],parameters[1],parameters[2],period);

			if ( result == null )
				result = new Vector();

      if (result.Count == vector1.Count)
				result.RemoveLastValue();

      for (int i = result.Count; i < vector1.Count; i++) {
				result.Add( Math.Abs(vector1[i] - vector2[i]) /
					Math.Abs(vector1[i] + vector2[i]) * 100 );
			}
			return result;
		}
	}

	[Function("ADX")]
	public class ADX : Function {
		private Function _funcDX, _funcMA;

		protected override void Initialize() {
			_funcDX = GetFunction("DX");
			_funcMA = GetFunction("MA");
			this.RegParameter(_funcDX);
      RegParameter(new ParameterColor("ColorADX", new string[] { "DX Color", "Цвет DX" }, Color.RosyBrown));
		}

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int period = (int)parameters[3];

			result = analyzer.Compute(_funcMA,period,1,analyzer.Compute(_funcDX,parameters[0],parameters[1],parameters[2],period));
			return result;
		}
	}
}
