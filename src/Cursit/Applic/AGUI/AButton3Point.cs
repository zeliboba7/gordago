/**
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Drawing;

namespace Cursit.Applic.AGUI {
		/// <summary>
		/// Кнопка трех точек
		/// </summary>
		public class AButton3Point: Cursit.Applic.AGUI.Primitive.AButtonPrimitive {
				public AButton3Point():base(){
						this.Text = "...";
						int h = this.Size.Height;
						this.Size = new Size(16, h); 
				}
		}
}
