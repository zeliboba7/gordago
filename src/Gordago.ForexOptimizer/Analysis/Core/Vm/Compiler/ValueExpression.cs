/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;


namespace Gordago.Analysis.Vm.Compiler {
	internal class ValueExpression : Expression {
		public ValueExpression(object @value) {
			if ( @value == null )
				throw new ArgumentNullException();
			
			this.@value = @value;
		}
		public override VmCommand[] Compile() {
			return new VmCommand[] { new VmCommand(VmOpcode.Lv,@value) };
		}
		
		public override Type Type { 
			get {
				return @value.GetType();
			}
		}
		
		private object @value;
	}
	
}
