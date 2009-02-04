/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	internal class NotExpression : Expression {
		public NotExpression(Expression expression) {
			if ( expression == null )
				throw new ArgumentNullException();
			if ( expression.Type != typeof(bool) )
				throw new ArgumentOutOfRangeException("expression","операция отрицания опредлена только на булевских типах");
			
			this.expression = expression;
		}
		
		public override VmCommand[] Compile() {
			ArrayList temp = new ArrayList();
			temp.AddRange( expression.Compile() ); 
			temp.Add( new VmCommand(VmOpcode.Nt) );
			return (VmCommand[])temp.ToArray(typeof(VmCommand));
		}
		
		public override Type Type { 
			get {
				return typeof(bool);
			}
		}
		
		private Expression expression;
	}
	
}
