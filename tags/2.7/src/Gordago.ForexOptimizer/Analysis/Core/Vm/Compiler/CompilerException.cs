/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	public class CompilerException : Exception {
		public CompilerException(string message,string expression,int c) : base(message) {
			this.expression = expression.Trim();
			this.c = c;
		}

		#region public string Expression
		public string Expression {
			get {
				return expression;
			}
		}
		#endregion

		#region public int Cursor 
		public int Cursor {
			get {
				return c;
			}
		}
		#endregion

		public override string ToString() {
			return string.Format("{0} в выражение \"{1}\" в позициии {2}",
				Message,expression,c); 
		}

		
		private string expression;
		private int c;
	}
}
