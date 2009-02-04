using System;

namespace Cursit.Applic.APropGrid{
		public class PropGridValueText: PropGridValue{
				
				public PropGridValueText(string caption){
						this.Caption = caption;
						initOver();
				}

				public PropGridValueText(string caption, string text){
						this.Caption = caption;
						this.Value = text;
						this.initOver();
				}

				public PropGridValueText() {
						initOver();
				}

				private void initOver(){
						this._valueType = PropGridValueType.Text;
				}

				public new string Value{
						get{return (string)base.Value;}
						set{base.Value = value;}
				}
		}
}
