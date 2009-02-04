using System;
using System.Drawing;

namespace Gordago.Analysis.Toolbox {
	public class iPoint: Indicator{
		public override void Initialize() {
			this.Copyright = "Copyright © 2006, Gordago Software Ltd.";
			this.Link = "http://www.gordago.com/";
			this.GroupName = "";
			this.Name = "Point";
			this.ShortName = "Point";
			this.SetImage("n.gif");
			this.IsSeparateWindow = false;

			RegFunction("Point").SetDrawPen(new Pen(Color.RoyalBlue,2));
		}
	}

	[Function("Point")]
	public class PointFunction:Function{

		protected override void Initialize() {
			this.RegParameter(new ParameterInteger("Point", 1, -10000000, 10000000));
			this.RegParameter(new ParameterInteger("Digits", -1, -1, 10));

			ParameterVector pvclose = new ParameterVector("__Close", "Close");
			pvclose.Visible = false;
			RegParameter(pvclose);
      RegParameter(new ParameterColor("ColorPoint", new string[] { "Color", "÷‚ÂÚ" }, Color.RoyalBlue));
    }

		public override IVector Compute(Analyzer analyzer, object[] parameters, IVector result) {
			int point = (int)parameters[0];
			int digits = (int)parameters[1];
			BarsVector bars = (BarsVector)parameters[2];

			if (result == null)
				result = new Vector();

			if (result.Count == bars.Count)
				return result;

			if (digits < 0)
				digits = analyzer.DecimalDigits;

      float points = point * (1F / SymbolManager.GetDelimiter(digits));
			
			for (int i=result.Count;i<bars.Count;i++)
				result.Add(points);
			
			return result;
		}

	}
}
