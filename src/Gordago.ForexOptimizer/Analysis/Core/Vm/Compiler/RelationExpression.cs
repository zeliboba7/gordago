/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	internal class RelationExpression : BinaryExpression {
		public RelationExpression(Expression leftExpression,Expression righExpression,string @operator) : base(leftExpression,righExpression,@operator) {
		}
		
		public override VmCommand[] Compile() {
			ArrayList temp = new ArrayList(); 
			temp.AddRange( base.Compile() );
			switch ( Operator ) {
				case "=":
					temp.Add(new VmCommand(
						base.Type == typeof(int) ? VmOpcode.Cq : VmOpcode.Cqf));
					break;
				case "<>":
					temp.Add(new VmCommand(
						base.Type == typeof(int) ? VmOpcode.Cne : VmOpcode.Cnef));
					break;
				case ">":
					temp.Add(new VmCommand(
						base.Type == typeof(int) ? VmOpcode.Cgr : VmOpcode.Cgrf));
					break;
				case "<":
					temp.Add(new VmCommand(
						base.Type == typeof(int) ? VmOpcode.Cls : VmOpcode.Clsf));
					break;
				case ">=":
					temp.Add(new VmCommand(
						base.Type == typeof(int) ? VmOpcode.Cge : VmOpcode.Cgef));
					break;
				case "<=":
					temp.Add(new VmCommand(
						base.Type == typeof(int) ? VmOpcode.Cle : VmOpcode.Clef));
					break;
			}
			return (VmCommand[])temp.ToArray(typeof(VmCommand));
		}

		public override Type Type { 
			get {
				return typeof(bool);
			}
		}

		
	}
}
