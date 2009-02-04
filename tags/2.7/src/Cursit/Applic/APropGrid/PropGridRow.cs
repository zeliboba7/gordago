using System;
using System.Drawing;

namespace Cursit.Applic.APropGrid {
	public class PropGridRowColor: PropGridRowPrimitive {
		public PropGridRowColor(PropGridValueColor pgValueColor):base(new PropGridViewerColor(pgValueColor)){}
		public PropGridRowColor(): base (new PropGridViewerColor(new PropGridValueColor(Color.Red))){	}
		public PropGridRowColor(Color color): base (new PropGridViewerColor(new PropGridValueColor(color))){	}
	}
	public class PropGridRowCaption: PropGridRowPrimitive {
		public PropGridRowCaption(string caption):base(new PropGridViewerText(new PropGridValueText(caption))) {
      this.BackColor = Color.FromArgb(204, 236, 242);
		}
		public override int Splitter {
			get {		return this.Width;			}
			set {				this._splitter = this.Width;			}
		}

	}
	public class PropGridRowNumber : PropGridRowPrimitive {
		public PropGridRowNumber(PropGridValueNumber pgvNumber):base(new PropGridViewerNumber(pgvNumber)){
		}
		public PropGridRowNumber(PropGridValueFloat pgvFloat):base(new PropGridViewerNumber(pgvFloat)){
		}
	}
	public class PropGridRowArrString:PropGridRowPrimitive {
		public PropGridRowArrString(PropGridValueArrString pgvArrString):base(new PropGridViewerArrString(pgvArrString)) {}
	}
	public class PropGridRowMulti: PropGridRowPrimitive  {
		public PropGridRowMulti(PropGridValueMulti pgvMulti):base(new PropGridViewerMulti(pgvMulti)) {	}
	}
	public class PropGridRowPeriod : PropGridRowPrimitive {
		public PropGridRowPeriod(PropGridValuePeriod pgvPeriod):base(new PropGridViewerPeriod(pgvPeriod)) {
		}
	}
	public class PropGridRowText : PropGridRowPrimitive{
		public PropGridRowText(PropGridValueText pgValueText):base(new PropGridViewerText(pgValueText)){}
		public PropGridRowText(string text):base(new PropGridViewerText(new PropGridValueText(text))){}
		public PropGridRowText():base(new PropGridViewerText()){}
	}
}
