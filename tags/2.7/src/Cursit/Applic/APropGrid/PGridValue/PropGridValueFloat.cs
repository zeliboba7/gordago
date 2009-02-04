using System;

namespace Cursit.Applic.APropGrid {
	/// <summary>
	/// Summary description for PropGridValueFloat.
	/// </summary>
	public class PropGridValueFloat: PropGridValue  {

		private float _min, _max;
		private int _decimalPlaces;
		private float _step;

		public PropGridValueFloat(string caption, float value, float min, float max, int decPlaces, float step) {
			this.Caption = caption;
			this._step = step;
			this._decimalPlaces= decPlaces;
			this.Value = value;
			_min = min;
			_max = max;
			OverInit();
		}
		private void OverInit(){
			this._valueType = PropGridValueType.Float;
		}
		public new float Value{
			get{return (float)base.Value;}
			set{base.Value = value;}
		}
		public float Min{
			get{return _min;}
		}
		public float Max{
			get{return _max;}
		}

		public int DecimalPlaces{
			get{return this._decimalPlaces;}
			set{this._decimalPlaces = value;}
		}

		public float Step{
			get{return this._step;}
			set{this._step = value;}
		}
	}
}
