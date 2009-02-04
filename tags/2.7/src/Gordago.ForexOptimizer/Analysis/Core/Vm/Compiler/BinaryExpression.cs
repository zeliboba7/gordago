/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	internal abstract class BinaryExpression : Expression {
		protected BinaryExpression(Expression leftExpression,Expression righExpression,string @operator) {
			if ( leftExpression == null || righExpression == null ) 
				throw new ArgumentNullException();
			if ( (leftExpression.Type == typeof(bool) && righExpression.Type != typeof(bool)) ||
				(leftExpression.Type != typeof(bool) && righExpression.Type == typeof(bool)) )
				throw new ArgumentOutOfRangeException("leftExpression,righExpression","несоответсвие типов");

			this.leftExpression = leftExpression;
			this.righExpression = righExpression;
			this.@operator = @operator;
		}
		
		public override VmCommand[] Compile() {
			ArrayList temp = new ArrayList();
			temp.AddRange( leftExpression.Compile() ); 
			if ( leftExpression.Type == typeof(IVector) )
				temp.Add( new VmCommand(VmOpcode.Le,0) );
			else if ( (leftExpression.Type == typeof(int) && (righExpression.Type == typeof(float) || righExpression.Type == typeof(IVector) || @operator == "/" )) )
				temp.Add( new VmCommand(VmOpcode.Ctf) );  

			temp.AddRange( righExpression.Compile() );
      if (righExpression.Type == typeof(IVector))
				temp.Add( new VmCommand(VmOpcode.Le,0) );
      else if ((righExpression.Type == typeof(int) && (leftExpression.Type == typeof(float) || leftExpression.Type == typeof(IVector) || @operator == "/")))
				temp.Add( new VmCommand(VmOpcode.Ctf) );  
			return (VmCommand[])temp.ToArray(typeof(VmCommand));
		}

		public override Type Type { 
			get {
				if ( leftExpression.Type == typeof(bool) && righExpression.Type == typeof(bool) )
					return typeof(bool);
        else if (leftExpression.Type == typeof(float) || righExpression.Type == typeof(float) || leftExpression.Type == typeof(IVector) || righExpression.Type == typeof(IVector))
					return typeof(float);
				else if ( leftExpression.Type == typeof(int) && righExpression.Type == typeof(int) ) {
					if ( @operator == "/")
						return typeof(float);
					else
						return typeof(int);
				}
				return typeof(object);
			}
		}

		protected Expression LeftExpression {
			get {
				return leftExpression;
			}
		}

		protected Expression RighExpression {
			get {
				return righExpression;
			}
		}

		protected string Operator {
			get {
				return @operator;
			}
		}

		private Expression leftExpression,righExpression;
		private string @operator;
	}
	
}

