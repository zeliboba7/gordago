using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iLaguerre : Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "Laguerre";
			this.ShortName = "Laguerre";
			this.SetImage("i.gif");

			RegFunction("Laguerre").SetDrawPen(Color.ForestGreen);
		}
	}
	[Function("Laguerre")]
	public class Laguerre: Function {
		#region class LGVector : Vector
		class LGVector : Vector {
			private float l0,l1,l2,l3;

			public  LGVector() : base() {
				l0 = 0;
				l1 = 0;
				l2 = 0;
				l3 = 0;

			}
			public float L0 {
				get {return l0;}
				set {l0 = value;}
			}
			public float L1 {
				get {return l1;}
				set {l1 = value;}
			}
			public float L2 {
				get {return l2;}
				set {l2 = value;}
			}
			public float L3 {
				get {return l3;}
				set {l3 = value;}
			}
		}
		#endregion

		protected override void Initialize() {
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			RegParameter(new ParameterFloat("Gamma", new string[]{"Gamma", "Gamma"}, 0.7F, 0, 10, 0.1f, 1));
      RegParameter(new ParameterColor("ColorLaguerre", new string[] { "Color", "Цвет" }, Color.ForestGreen));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			IVector vector = parameters[0] as IVector;
			float gamma=(float)parameters[1];
			
			if (result == null) 
				result = new LGVector();

			LGVector lgv = result as LGVector;

			if (result.Count == vector.Count)
				result.RemoveLastValue();

			for (int i = result.Count; i < vector.Count; i++) {
				float l0A = lgv.L0;
				float l1A = lgv.L1;
				float l2A = lgv.L2;
				float l3A = lgv.L3;
			
				lgv.L0 = (1 - gamma) * vector[i] + gamma*l0A;
				lgv.L1 = - gamma *lgv.L0 + l0A + gamma *l1A;
				lgv.L2 = - gamma *lgv.L1 + l1A + gamma *l2A;
				lgv.L3 = - gamma *lgv.L2 + l2A + gamma *l3A;

				float cU = 0;
				float cD = 0;
      
				if (lgv.L0 >= lgv.L1) cU = lgv.L0 - lgv.L1; else cD = lgv.L1 - lgv.L0;
				if (lgv.L1 >= lgv.L2) cU = cU + lgv.L1 - lgv.L2; else cD = cD + lgv.L2 - lgv.L1;
				if (lgv.L2 >= lgv.L3) cU = cU + lgv.L2 - lgv.L3; else cD = cD + lgv.L3 - lgv.L2;

				if (cU + cD != 0)
					lgv.Add(cU / (cU + cD));
				else
					lgv.Add(0);
			}
			return lgv;
		}
	}
}
