using System;

namespace Cursit.Applic.APropGrid{
		public class PropGridValueArrString: PropGridValue {
				private string[] _values;
				
				public PropGridValueArrString(string caption, string[] values, string valDefault) {
						this._valueType = PropGridValueType.ArrayStrings;
						this.Caption = caption;
						this.Values = values;
						this.Value = valDefault;
				}

				public new string Value{
						get{return (string)base.Value;}
						set{base.Value = value;}
				}

				public string[] Values{
						get{return _values;}
						set{_values = value;}
				}
		}
}
