/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;

namespace Gordago.Analysis.Vm {

	public enum VmOpcode : short {
		Ad,	Adf, Sb, Sbf, Ml, Mlf, Dv, Dvf,
		Nt, And, Or, 
		Cq, Cqf, Cne, Cnef, Cgr, Cgrf, Cls, Clsf, Cge, Cgef, Cle, Clef, 
		Ink, 
    Lv, 
    Le, Ct, Ctf,
	}

	public class VmCommand {
		private VmOpcode _opcode;
		private object _value;
		private Function _target;
    private CacheItem _cacheItem;

		public VmCommand(VmOpcode opcode, object @value, Function target) {
			this._opcode = opcode;
			this._value = @value;
			this._target = target;
      _cacheItem = null;
		}

		public VmCommand(VmOpcode opcode,object @value) : this(opcode,@value,null) {
		}

		public VmCommand(VmOpcode opcode) : this(opcode,null,null) {
		}

		#region public VmOpcode Opcode 
		public VmOpcode Opcode {
			get {
				return _opcode;
			}
		}
		#endregion

		#region public object Value 
		public object Value {
			get {
				return _value;
			}
		}
		#endregion

		#region public Function Function
		public Function Function {
			get {
				return _target;
			}
		}
		#endregion

    public CacheItem CacheItem {
      get { return _cacheItem; }
      set { this._cacheItem = value; }
    }
	}
}
