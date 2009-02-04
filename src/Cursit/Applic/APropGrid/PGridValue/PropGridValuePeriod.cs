using System;

namespace Cursit.Applic.APropGrid{
	public class PropGridValuePeriod : PropGridValue{
		private decimal _numBegin;
		private decimal _numEnd;
		private decimal _min;
		private decimal _max;
		private decimal _step;

		public PropGridValuePeriod(string caption, decimal numBegin, decimal numEnd, decimal min, decimal max, decimal step) {
			this.Caption = caption;
			_numBegin = numBegin;
			_numEnd = numEnd;
			_min = min;
			_max = max;
			_step = step;
			this._valueType = PropGridValueType.Period;
		}

		public decimal ValueBegin{
			get{return _numBegin;}
			set{_numBegin = value;}
		}

		public decimal ValueEnd{
			get{return _numEnd;}
			set{_numEnd = value;}
		}
		public decimal Min{
			get{return _min;}
			set{_min = value;}
		}
		public decimal Max{
			get{return _max;}
			set{_max = value;}
		}
		public decimal Step{
			get{return this._step;}
			set{this._step = value;}
		}
	}
}
