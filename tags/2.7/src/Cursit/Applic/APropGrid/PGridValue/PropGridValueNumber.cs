using System;

namespace Cursit.Applic.APropGrid{
	public class PropGridValueNumber: PropGridValue {

		private int _min, _max;
				
		public PropGridValueNumber(string caption, int value, int min, int max) {
			this.Caption = caption;
			this.Value = value;
			this._min = min;
			this._max = max;
			OverInit();
		}

		public PropGridValueNumber(string caption) {
			this.Caption = caption;
			OverInit();
		}

		private void OverInit(){
			this._valueType = PropGridValueType.Number;
		}
				
		public new int Value{
			get{return (int)base.Value;}
			set{base.Value = value;}
		}

		public int Min{
			get{return _min;}
		}
		public int Max{
			get{return this._max;}
		}
	}
}
