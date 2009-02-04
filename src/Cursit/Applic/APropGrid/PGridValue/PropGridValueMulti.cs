using System;

namespace Cursit.Applic.APropGrid {
	public class PropGridValueMulti : PropGridValue{
		private string[] _invalues;

		private bool _multiRow;

		public PropGridValueMulti(string caption, string[] invalues, string value) {
			init(caption, invalues, new string[]{value});
		}

		public PropGridValueMulti(string caption, string[] invalues, string[] value) {
			init(caption, invalues, value);
		}

		public PropGridValueMulti(string caption, string[] invalues, int[] indexes){
			string[] vals = new string[indexes.Length];
			for (int i=0;i < indexes.Length;i++){
				vals[i] = invalues[indexes[i]];
			}
			init(caption, invalues, vals);
		}

		private void init(string caption, string[] invalues, string[] value){
			this.MultiRow = true;
			this._valueType = PropGridValueType.Multi;
			this.Caption = caption;
			_invalues = invalues;
			this.Value = value;
		}

		public new string[] Value{
			get{ return (string[])base.Value;}
			set{base.Value = value;}
		}
		/// <summary>
		/// Массив значений, из которых необходимо вырбать значения
		/// </summary>
		public string[] InValues{
			get{return _invalues;}
			set{_invalues = value;}
		}

		public bool MultiRow{
			get{return this._multiRow;}
			set{this._multiRow = value;}
		}
	}
}
