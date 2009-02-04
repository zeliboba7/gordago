using System;
using System.Drawing;
using Gordago.Analysis;

namespace Gordago.Analysis.Kernel {

	public class iLow: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Low";
			this.ShortName = "Low";
			this.SetImage("l.gif");
			this.IsSeparateWindow = false;

			RegFunction("Low").SetDrawPen(Color.CadetBlue);
		}
	}

	public class iHigh: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "High";
			this.ShortName = "High";
			this.SetImage("h.gif");
			this.IsSeparateWindow = false;

			RegFunction("High").SetDrawPen(Color.CadetBlue);
		}
	}

	public class iOpen: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Open";
			this.ShortName = "Open";
			this.SetImage("o.gif");
			this.IsSeparateWindow = false;

			RegFunction("Open").SetDrawPen(Color.CadetBlue);
		}
	}

	public class iClose: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Close";
			this.ShortName = "Close";
			this.SetImage("c.gif");
			this.IsSeparateWindow = false;

			RegFunction("Close").SetDrawPen(Color.Green);
		}
	}

	public class iMedian: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Median (HL/2)";
			this.ShortName = "Median";
			this.SetImage("m.gif");
			this.IsSeparateWindow = false;

			RegFunction("Median").SetDrawPen(Color.GreenYellow);
		}
	}

	public class iTypical: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Typical (HLC/3)";
			this.ShortName = "Typical";
			this.SetImage("t.gif");
			this.IsSeparateWindow = false;

			RegFunction("Typical").SetDrawPen(Color.Firebrick);
		}
	}

	public class iWeighted: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Weighted (HLCC/4)";
			this.ShortName = "Weighted";
			this.SetImage("w.gif");
			this.IsSeparateWindow = false;

			RegFunction("Weighted").SetDrawPen(Color.Lime);
		}
	}
	public class iNumber: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = "";
			this.Name = "Number";
			this.ShortName = "Number";
			this.SetImage("n.gif");
			this.IsSeparateWindow = false;

			FunctionStyle fs = RegFunction("Number");
			fs.SetDrawPen(Color.Red);
		}
	}

	public class iVolume: Indicator{
		public override void Initialize() {
			this.Copyright = DefaultDescription.Copyright;
			this.Link = DefaultDescription.Link;
			this.GroupName = GroupNames.TopPrice;
			this.Name = "Volume";
			this.ShortName = "Volume";
			this.SetImage("v.gif");
			this.IsSeparateWindow = true;

			FunctionStyle fs = RegFunction("Volume");
			fs.SetDrawPen(Color.DarkBlue);
			fs.SetDrawStyle(FunctionDrawStyle.Histogram);

		}
	}

}
