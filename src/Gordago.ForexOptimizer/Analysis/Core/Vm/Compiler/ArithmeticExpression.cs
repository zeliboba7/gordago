/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	internal class ArithmeticExpression : BinaryExpression {
		public ArithmeticExpression(Expression leftExpression, Expression righExpression,string @operator) : 
			base(leftExpression,righExpression,@operator) {}
		
		public override VmCommand[] Compile() {
			ArrayList temp = new ArrayList(); 
			temp.AddRange( base.Compile() );
			switch ( Operator ) {
				case "+":
					temp.Add(new VmCommand(
						Type == typeof(int) ? VmOpcode.Ad : VmOpcode.Adf));
					break;
				case "-":
					temp.Add(new VmCommand(
						Type == typeof(int) ? VmOpcode.Sb : VmOpcode.Sbf));
					break;
				case "*":
					temp.Add(new VmCommand(
						Type == typeof(int) ? VmOpcode.Ml : VmOpcode.Mlf));
					break;
				case "/":
					temp.Add(new VmCommand(
						Type == typeof(int) ? VmOpcode.Dv : VmOpcode.Dvf));
					break;
			}
			return (VmCommand[])temp.ToArray(typeof(VmCommand));
		}
	}
	
}
