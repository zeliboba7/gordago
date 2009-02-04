using System;
using System.Drawing;

namespace Cursit.Applic.APropGrid {
		/// <summary>
		/// Summary description for PropGridValueColor.
		/// </summary>
		public class PropGridValueColor : PropGridValue {
				
				public PropGridValueColor(string caption, Color color){
						this.Value = color;
						this.Caption = caption;
						OverInit();
				}
				public PropGridValueColor(Color color){
						this.Value = color;
						OverInit();
				}

				public PropGridValueColor() {
						this.OverInit();
				}
				private void OverInit(){
						this._valueType = PropGridValueType.Color;
				}

				public new Color Value{
						get{return (Color)base.Value;}
						set{base.Value = value;}
				}
		}
}
