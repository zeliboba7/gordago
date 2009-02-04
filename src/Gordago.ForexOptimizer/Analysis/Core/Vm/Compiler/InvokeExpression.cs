/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm.Compiler {
	class InvokeExpression : Expression {

    private Function _function;
    private Expression[] _parameters;
    private Expression _element;

		public InvokeExpression(Function function,Expression[] parameters,Expression element) {
			if ( function == null || parameters == null )
				throw new ArgumentNullException();
			if ( element != null && element.Type != typeof(int) )
				throw new ArgumentOutOfRangeException("element","индекс элемента вектора должен быть целым числом");
			
			this._function = function;
			this._parameters = parameters;
			this._element = element;
		}
		
		public override VmCommand[] Compile() {
			ArrayList temp = new ArrayList();
			foreach ( Expression p in _parameters )
				temp.AddRange( p.Compile() );
			temp.Add( new VmCommand(VmOpcode.Ink,_parameters.Length,_function));  
			if ( _element != null ) {
				temp.AddRange( _element.Compile() ); 
				temp.Add( new VmCommand(VmOpcode.Le) );  
			}
			return (VmCommand[])temp.ToArray(typeof(VmCommand));
		}
		
		public override Type Type { 
			get {
				if ( _element != null)
					return typeof(float);
				else
          return typeof(IVector);
			}
		}
		
	}
	
}
