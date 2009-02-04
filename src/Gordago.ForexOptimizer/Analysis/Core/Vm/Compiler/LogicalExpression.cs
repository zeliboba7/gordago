/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	internal class LogicalExpression : BinaryExpression {
		public LogicalExpression(Expression leftExpression,Expression righExpression,string @operator) : base(leftExpression,righExpression,@operator) {
		}

		public override VmCommand[] Compile() {
			ArrayList temp = new ArrayList(); 
			temp.AddRange( base.Compile() );
			switch ( Operator ) {
				case "&":
					temp.Add( new VmCommand(VmOpcode.And) );
					break;
				case "|":
					temp.Add( new VmCommand(VmOpcode.Or) );
					break;
			}
			return (VmCommand[])temp.ToArray(typeof(VmCommand));
		}
	}
}
