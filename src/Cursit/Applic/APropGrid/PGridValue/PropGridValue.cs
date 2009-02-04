using System;

namespace Cursit.Applic.APropGrid{
	/// <summary>
	/// Свойство используемое в 
	/// </summary>
	public abstract class PropGridValue {
		public event EventHandler ValueChanged;
		private EventArgs _e;
		internal object _value;
		private object _valueObject;
		internal string _description = "";
		internal PropGridValueType _valueType;

		private string _caption;
				
		public string Description{
			get{return _description;}
			set{_description = value;}
		}
				
		/// <summary>
		///  Тип редактируемого свойства
		/// </summary>
		public PropGridValueType ValueType{
			get{return this._valueType;}
		}

		/// <summary>
		/// Подписывание строки свойства
		/// </summary>
		public string Caption{
			get{return this._caption;}
			set{this._caption = value;}
		}

		public virtual object Value{
			get{return _value;}
			set{
				if (_value != null)
					if (_value.Equals(value))
						return;
				_value = value;
				if (this.ValueChanged != null){
					if (_e == null) _e = new EventArgs();
					this.ValueChanged(this, _e);
				}
			}
		}

		/// <summary>
		/// Дополнительный объект хранимый для упрощения изменения параметров
		/// </summary>
		public object ValueObject{
			get{return _valueObject;}
			set{_valueObject = value;}
		}
	}
}
