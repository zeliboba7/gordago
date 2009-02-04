using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.StockOptimizer2.Toolbox {
	public class iOnBalanceVolume:Indicator {
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.Relative;
			this.Name = "On Balance Volume";
			this.ShortName = "OBV";
			this.SetImage("i.gif");

			RegFunction("OBV").SetDrawPen(Color.ForestGreen);
		}
	}
	[Function("OBV")]
	public class OBV : Function {
		protected override void Initialize() {
			RegParameter(Function.CreateDefParam_ApplyTo("Close"));
			ParameterVector pvvolume = new ParameterVector("VectorVolume", "Volume");
			pvvolume.Visible = false;
			RegParameter(pvvolume);
      RegParameter(new ParameterColor("ColorOBV", new string[] { "Color", "Цвет" }, Color.ForestGreen));
    }

		public override IVector Compute(Analyzer analyzer,object[] parameters,IVector result) {
			IVector vector1 = (IVector)parameters[0];
			IVector vector2 = (IVector)parameters[1];
			
			if ( result == null )
				result = new Vector();
			if (result.Count == vector1.Count)
				result.RemoveLastValue();

			for ( int i = result.Count; i < vector1.Count; i++ ) {
				if ( result.Count == 0 )
					result.Add( vector2[i]);
				else {
					if ( vector1[i] > vector1[i - 1] )
						result.Add(result.Current + vector2[i] );
					else if ( vector1[i] < vector1[i - 1] )
						result.Add(result.Current - vector2[i] );
					else
						result.Add(result.Current); 
				}
			}
			return result;
		}
	}
}

